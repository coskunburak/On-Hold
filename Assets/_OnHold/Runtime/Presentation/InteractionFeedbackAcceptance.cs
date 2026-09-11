#if ONHOLD_FEEDBACK_ACCEPTANCE
using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;
using OnHold.Domain;
using OnHold.Gameplay;
namespace OnHold.Presentation
{
    // Compiled only into the separately named QA player. F6 arranges real production world scenarios.
    // It never grabs on behalf of the user or substitutes the frontend, ray, motor, or transaction.
    public sealed class InteractionFeedbackAcceptance : MonoBehaviour
    {
        FoundationFrontend frontend;
        GameObject wall;
        int step = -1, capture, preview = -1, previousHeld = -1;
        static readonly string[] Steps = { "Empty space", "Floor", "Wall", "Eligible cargo", "Look away", "Out of range", "Return to range", "Occluded cargo", "Menu", "Resume", "Grip 1 side", "Grip 2 side", "Free grab / carry / release — toggle hold" };
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void StartQA() => new GameObject("Explicit feedback acceptance fixture").AddComponent<InteractionFeedbackAcceptance>();
        void Start()
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
            frontend = FindAnyObjectByType<FoundationFrontend>();
            wall = GameObject.CreatePrimitive(PrimitiveType.Cube); wall.name = "QA occluder"; wall.layer = 8;
            wall.GetComponent<Renderer>().sharedMaterial = frontend.World.Items[0].GetComponentInChildren<Renderer>().sharedMaterial;
            wall.transform.localScale = new Vector3(2, 2.5f, .2f); wall.SetActive(false);
        }
        void Update()
        {
            if (Keyboard.current.f7Key.wasPressedThisFrame) Screen.SetResolution(1280, Screen.height == 720 ? 800 : 720, FullScreenMode.Windowed);
            if (!frontend || frontend.World.Session?.Phase != SessionPhase.Active) return;
            if (Keyboard.current.f6Key.wasPressedThisFrame) Arrange((step + 1) % Steps.Length);
            var cargo = frontend.World.Items[0]; var actor = frontend.Network.LocalActor;
            int held = cargo.HeldGripIndex(actor);
            if (held != previousHeld) { Debug.Log("QA hold transition preview=" + preview + " actual=" + held); previousHeld = held; }
            if (frontend.InteractionFeedback.HasGrip) preview = frontend.InteractionFeedback.Grip.GripIndex;
            if (Keyboard.current.f8Key.wasPressedThisFrame)
            {
                string directory = Path.Combine(Application.persistentDataPath, "FeedbackQA"); Directory.CreateDirectory(directory);
                string path = Path.Combine(directory, "S018-stage-" + step + "-" + capture++ + ".png");
                ScreenCapture.CaptureScreenshot(path);
                Debug.Log("QA capture " + path + " resolution=" + Screen.width + "x" + Screen.height + " preview=" + preview + " held=" + held + " marker=" + frontend.GripMarkerVisible);
            }
        }
        void Arrange(int next)
        {
            step = next; var world = frontend.World; var actor = frontend.Network.LocalActor;
            var player = world.Players[actor.Id]; var cargo = world.Items[0];
            frontend.SetMenu(false); world.Apply(new OnHold.Domain.CommandEnvelope { CommandType = CommandType.Release }, actor);
            player.Respawn(new Vector3(0, .02f, -4)); cargo.ResetState(); cargo.Body.position = new Vector3(0, .31f, -2.4f);
            cargo.Body.rotation = Quaternion.identity; wall.SetActive(step == 2 || step == 7);
            wall.transform.position = new Vector3(0, 1.25f, -3.2f);
            if (step == 5) cargo.Body.position = new Vector3(0, .31f, .5f);
            if (step == 10) player.Respawn(new Vector3(-.55f, .02f, -4));
            if (step == 11) player.Respawn(new Vector3(.55f, .02f, -4));
            if (step >= 10) frontend.Settings.ToggleHold = true;
            Physics.SyncTransforms();
            Vector3 direction = cargo.Body.worldCenterOfMass - player.Eye;
            if (step == 0 || step == 4) direction = new Vector3(0, 1, 1);
            if (step == 1) direction = new Vector3(0, -1, .15f);
            var rotation = Quaternion.LookRotation(direction).eulerAngles;
            float pitch = Mathf.DeltaAngle(0, rotation.x), yaw = rotation.y;
            var current = frontend.ViewCamera.transform.eulerAngles;
            frontend.ApplyLook(new Vector2(Mathf.DeltaAngle(current.y, yaw), -Mathf.DeltaAngle(current.x, pitch)) / frontend.Settings.Sensitivity, false, 0);
            if (step == 8) frontend.SetMenu(true);
            frontend.RefreshInteractionFeedback();
            Debug.Log("Feedback QA stage " + step + " " + Steps[step] + " visible=" + frontend.InteractionFeedback.Visible + " action=" + frontend.InteractionFeedback.Action + " target=" + frontend.InteractionFeedback.TargetId);
        }
        void OnGUI()
        {
            GUI.matrix = Matrix4x4.identity;
            GUI.Label(new Rect(Screen.width / 2 - 250, 12, 500, 30), "FEEDBACK QA  /  F6: scenario · F7: aspect · F8: capture  /  " + (step < 0 ? "Click PLAY SOLO, then F6" : Steps[step]));
        }
        void OnDestroy() { if (wall) Destroy(wall); }
    }
}
#endif
