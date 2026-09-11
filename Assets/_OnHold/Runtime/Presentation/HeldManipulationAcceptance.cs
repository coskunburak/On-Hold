#if ONHOLD_MANIPULATION_ACCEPTANCE || ONHOLD_RELEASE_ACCEPTANCE
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using OnHold.Domain;
using OnHold.Gameplay;

namespace OnHold.Presentation
{
    // QA-only scene arrangement and telemetry. All gameplay input still goes through the production actions.
    #if ONHOLD_RELEASE_ACCEPTANCE
    [DefaultExecutionOrder(1000)]
#endif
    public sealed partial class HeldManipulationAcceptance : MonoBehaviour
    {
        FoundationFrontend frontend;
        GameObject wall;
        Keyboard controls;
#if !ONHOLD_RELEASE_ACCEPTANCE
        Keyboard modifier;
#endif
        bool latched;
        Mouse motion;
        Vector2 replayDirection;
        float replayUntil;
        int scenario = -1, previousGrip = -1, preview = -1, capture;
        bool lastActive;
        float maxAngular, maxTorque;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void StartQA() => new GameObject("Explicit carry visible acceptance").AddComponent<HeldManipulationAcceptance>();
        void Start()
        {
#if ONHOLD_RELEASE_ACCEPTANCE
            StartReleaseQA();
#else
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
            frontend = FindAnyObjectByType<FoundationFrontend>();
            // A virtual modifier allows visible QA when the UI driver cannot hold a button across calls.
            controls = Keyboard.current; modifier = InputSystem.AddDevice<Keyboard>(); motion = InputSystem.AddDevice<Mouse>();
            frontend.Actions.FindAction("Gameplay/Grab").ApplyBindingOverride(0, "<Keyboard>/f");
            frontend.Actions.FindAction("Gameplay/Manipulate").ApplyBindingOverride(0, "<Keyboard>/f13");
            frontend.Settings.ToggleHold = true;
            wall = GameObject.CreatePrimitive(PrimitiveType.Cube); wall.name = "S019 QA wall"; wall.layer = 8;
            wall.transform.localScale = new Vector3(8, 4, .4f); wall.transform.position = new Vector3(0, 2, -1.65f); wall.SetActive(false);
#endif
        }
        void Update()
        {
#if ONHOLD_RELEASE_ACCEPTANCE
            UpdateReleaseQA();
#else
            if (!frontend || frontend.World.Session?.Phase != SessionPhase.Active) return;
            if (controls.f5Key.wasPressedThisFrame)
            {
                latched = !latched;
                InputSystem.QueueStateEvent(modifier, latched ? new KeyboardState(Key.F13) : new KeyboardState());
            }
            if (controls.f9Key.wasPressedThisFrame) { replayDirection = Vector2.right; replayUntil = Time.unscaledTime + .7f; }
            if (controls.f10Key.wasPressedThisFrame) { replayDirection = Vector2.up; replayUntil = Time.unscaledTime + .7f; }
            if (Time.unscaledTime < replayUntil) InputSystem.QueueDeltaStateEvent(motion.delta, replayDirection * (500 * Time.unscaledDeltaTime));
            if (controls.f6Key.wasPressedThisFrame) Arrange();
            var cargo = frontend.World.Items[scenario == 2 ? 1 : 0]; var actor = frontend.Network.LocalActor;
            int grip = cargo.HeldGripIndex(actor);
            if (frontend.InteractionFeedback.HasGrip) preview = frontend.InteractionFeedback.Grip.GripIndex;
            if (grip != previousGrip || frontend.ManipulationActive != lastActive)
            {
                Debug.Log($"S019 QA scenario={scenario} preview={preview} held={grip} manipulating={frontend.ManipulationActive} cargo={cargo.Body.rotation.eulerAngles} camera={frontend.ViewCamera.transform.rotation.eulerAngles}");
                previousGrip = grip; lastActive = frontend.ManipulationActive;
            }
            maxAngular = Mathf.Max(maxAngular, cargo.Body.angularVelocity.magnitude); maxTorque = Mathf.Max(maxTorque, cargo.LastTorque.magnitude);
            if (controls.f8Key.wasPressedThisFrame)
            {
                string directory = System.IO.Path.Combine(Application.persistentDataPath, "S019QA");
                System.IO.Directory.CreateDirectory(directory);
                ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(directory, "S019-scenario-" + scenario + "-" + capture++ + ".png"));
                Debug.Log($"S019 QA sample scenario={scenario} held={grip} rotation={cargo.Body.rotation.eulerAngles} camera={frontend.ViewCamera.transform.rotation.eulerAngles} peakAngular={maxAngular} peakTorque={maxTorque}");
            }
#endif
        }
#if !ONHOLD_RELEASE_ACCEPTANCE
        void Arrange()
        {
            latched = false; InputSystem.QueueStateEvent(modifier, new KeyboardState());
            scenario = (scenario + 1) % 3; var world = frontend.World; var actor = frontend.Network.LocalActor;
            frontend.SetMenu(false); world.Apply(new CommandEnvelope { CommandType = CommandType.Release }, actor);
            var player = world.Players[actor.Id]; player.Respawn(new Vector3(0, .02f, -4));
            foreach (var item in world.Items) item.ResetState();
            var cargo = world.Items[scenario == 2 ? 1 : 0];
            world.Items[scenario == 2 ? 0 : 1].Body.position = new Vector3(5, 1, -2);
            cargo.Body.position = new Vector3(0, scenario == 2 ? .34f : .31f, -2.4f); cargo.Body.rotation = Quaternion.identity;
            wall.SetActive(scenario == 1); Physics.SyncTransforms();
            var desired = Quaternion.LookRotation(cargo.Body.worldCenterOfMass - player.Eye).eulerAngles;
            var current = frontend.ViewCamera.transform.eulerAngles;
            frontend.ApplyLook(new Vector2(Mathf.DeltaAngle(current.y, desired.y), -Mathf.DeltaAngle(current.x, desired.x)) / frontend.Settings.Sensitivity, false, 0);
            frontend.RefreshInteractionFeedback(); maxAngular = maxTorque = 0;
            Debug.Log("S019 QA arranged " + scenario + " (0=long free, 1=long wall, 2=crate)");
        }
#endif
        void OnGUI()
        {
#if ONHOLD_RELEASE_ACCEPTANCE
            DrawReleaseQA();
#else
            GUI.matrix = Matrix4x4.identity;
            GUI.Box(new Rect(310, 590, 860, 100), "S019 QA — F6: arrange next cargo / wall scenario — F: grab / release\nF5: latch QA modifier; mouse/drag: rotate — mouse: look — WASD / Space unchanged\nF8: capture; F9/F10: explicit mouse input replay — Defaults: LMB grab, hold RMB + look to rotate");
            if (frontend && scenario >= 0)
            {
                var cargo = frontend.World.Items[scenario == 2 ? 1 : 0];
                GUI.Label(new Rect(330, 660, 820, 28), $"Scenario {scenario} | active {frontend.ManipulationActive} | grip {cargo.HeldGripIndex(frontend.Network.LocalActor)} | cargo {cargo.Body.rotation.eulerAngles:F1} | max angular {maxAngular:F2} rad/s");
            }
#endif
        }
        void OnDestroy() {
#if ONHOLD_RELEASE_ACCEPTANCE
            CleanupReleaseQA();
#endif
#if !ONHOLD_RELEASE_ACCEPTANCE
            if (modifier != null) InputSystem.RemoveDevice(modifier);
#endif
            if (motion != null) InputSystem.RemoveDevice(motion); if (wall) Destroy(wall); }
    }
}
#endif
