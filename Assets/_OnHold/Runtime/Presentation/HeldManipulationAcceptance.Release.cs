#if ONHOLD_RELEASE_ACCEPTANCE
using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using OnHold.Core;
using OnHold.Domain;
using OnHold.Gameplay;

namespace OnHold.Presentation
{
    // S020 extends the existing visible acceptance fixture. Only this QA define compiles it.
    // Virtual devices feed unchanged production bindings; geometry is arranged once per explicit scenario.
    public sealed partial class HeldManipulationAcceptance
    {
        static readonly string[] ReleaseScenarios = { "Free release", "Floor support", "Wall contact", "Nearby correction", "No safe pose / retry", "Prolonged obstruction" };
        Keyboard locomotion;
        GameObject qaFloor;
        bool originalToggle, armed, pendingRelease;
        int stageRelease;
        float clickUntil, moveUntil, jumpUntil, obstructionStart;
        Key moveKey;
        Vector3 releaseOrigin;
        float peakLinear, peakReleaseLinear, peakReleaseAngular, peakDisplacement, peakForce;
        string releaseOutcome = "Ready";
        string EvidenceDirectory => Path.Combine(Application.persistentDataPath, "S020QA");

        void StartReleaseQA()
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
            frontend = FindAnyObjectByType<FoundationFrontend>();
            controls = Keyboard.current; motion = InputSystem.AddDevice<Mouse>(); locomotion = InputSystem.AddDevice<Keyboard>();
            originalToggle = frontend.Settings.ToggleHold; frontend.Settings.ToggleHold = true;
            frontend.Network.Rejected += ReleaseRejected;
            wall = GameObject.CreatePrimitive(PrimitiveType.Cube); wall.name = "S020 explicit QA obstacle"; wall.layer = 8;
            wall.GetComponent<Renderer>().sharedMaterial = frontend.World.Items[0].GetComponentInChildren<Renderer>().sharedMaterial;
            wall.SetActive(false);
            qaFloor = GameObject.CreatePrimitive(PrimitiveType.Cube); qaFloor.name = "S020 QA open floor"; qaFloor.layer = 8;
            qaFloor.transform.position = new Vector3(30, -.25f, 0); qaFloor.transform.localScale = new Vector3(24, .5f, 24);
            qaFloor.GetComponent<Renderer>().sharedMaterial = frontend.World.Platform.GetComponentInChildren<Renderer>().sharedMaterial;
        }

        void UpdateReleaseQA()
        {
            if (!frontend || frontend.World.Session?.Phase != SessionPhase.Active) return;
            var world = frontend.World; var cargo = world.Items[0]; var actor = frontend.Network.LocalActor;
            if (controls.f6Key.wasPressedThisFrame && cargo.HandleCount == 0) ArrangeReleaseQA();
            if (controls.f5Key.wasPressedThisFrame) latched = !latched;
            if (controls.f9Key.wasPressedThisFrame) { replayDirection = Vector2.right; replayUntil = Time.unscaledTime + .7f; }
            if (controls.f10Key.wasPressedThisFrame) { replayDirection = Vector2.up; replayUntil = Time.unscaledTime + .7f; }
            if (controls.f2Key.wasPressedThisFrame) { moveKey = Key.W; moveUntil = Time.unscaledTime + .7f; }
            if (controls.f3Key.wasPressedThisFrame) { moveKey = Key.D; moveUntil = Time.unscaledTime + .7f; }
            if (controls.f4Key.wasPressedThisFrame) jumpUntil = Time.unscaledTime + .12f;
            if (controls.f7Key.wasPressedThisFrame && cargo.HandleCount > 0)
            {
                if (wall.activeSelf) { wall.SetActive(false); armed = false; Physics.SyncTransforms(); Debug.Log("S020 QA obstacle removed; recovery under normal world physics"); }
                else if (scenario == 5) { PlaceReleaseObstacle(); obstructionStart = Time.unscaledTime; }
                else { armed = true; Debug.Log("S020 QA next release will stage " + ReleaseScenarios[Mathf.Max(0, scenario)]); }
            }
            if (controls.fKey.wasPressedThisFrame)
            {
                clickUntil = Time.unscaledTime + .1f;
                if (cargo.HandleCount > 0) { pendingRelease = true; releaseOrigin = cargo.Body.position; if (armed) stageRelease = 2; }
            }
            // This component runs after the frontend. On the following Update, its release command
            // has been queued; arrange the fixture before the next authoritative physics tick.
            if (stageRelease > 0 && --stageRelease == 0) { PlaceReleaseObstacle(); releaseOrigin = cargo.Body.position; armed = false; }
            var mouseState = new MouseState { buttons = (ushort)((latched ? 2 : 0) | (Time.unscaledTime < clickUntil ? 1 : 0)),
                delta = Time.unscaledTime < replayUntil ? replayDirection * (500 * Time.unscaledDeltaTime) : Vector2.zero };
            InputSystem.QueueStateEvent(motion, mouseState);
            var keyState = new KeyboardState();
            if (Time.unscaledTime < moveUntil) keyState.Set(moveKey, true);
            if (Time.unscaledTime < jumpUntil) keyState.Set(Key.Space, true);
            InputSystem.QueueStateEvent(locomotion, keyState);
            int grip = cargo.HeldGripIndex(actor);
            if (frontend.InteractionFeedback.HasGrip) preview = frontend.InteractionFeedback.Grip.GripIndex;
            if (grip != previousGrip || lastActive != frontend.ManipulationActive)
            {
                if (pendingRelease && grip < 0)
                {
                    peakDisplacement = Mathf.Max(peakDisplacement, Vector3.Distance(releaseOrigin, cargo.Body.position));
                    peakReleaseLinear = Mathf.Max(peakReleaseLinear, cargo.Body.linearVelocity.magnitude);
                    peakReleaseAngular = Mathf.Max(peakReleaseAngular, cargo.Body.angularVelocity.magnitude);
                    releaseOutcome = "Released"; pendingRelease = false;
                }
                Debug.Log($"S020 QA transition scenario={scenario} preview={preview} grip={grip} manipulation={frontend.ManipulationActive} result={releaseOutcome} rotation={cargo.Body.rotation.eulerAngles} camera={frontend.ViewCamera.transform.eulerAngles}");
                previousGrip = grip; lastActive = frontend.ManipulationActive;
            }
            if (grip >= 0)
            {
                peakLinear = Mathf.Max(peakLinear, cargo.Body.linearVelocity.magnitude);
                maxAngular = Mathf.Max(maxAngular, cargo.Body.angularVelocity.magnitude);
                peakForce = Mathf.Max(peakForce, cargo.LastForce.magnitude); maxTorque = Mathf.Max(maxTorque, cargo.LastTorque.magnitude);
            }
            if (controls.f8Key.wasPressedThisFrame)
            {
                Directory.CreateDirectory(EvidenceDirectory);
                string path = Path.Combine(EvidenceDirectory, "S020-" + scenario + "-" + capture++ + ".png");
                ScreenCapture.CaptureScreenshot(path);
                Debug.Log($"S020 QA capture={path} scenario={scenario} held={grip} manipulating={frontend.ManipulationActive} result={releaseOutcome} peakLinear={peakLinear} peakAngular={maxAngular} force={peakForce} torque={maxTorque} releaseLinear={peakReleaseLinear} releaseAngular={peakReleaseAngular} rotation={cargo.Body.rotation.eulerAngles} camera={frontend.ViewCamera.transform.eulerAngles} observedFrameDisplacement={peakDisplacement} obstructionSeconds={(obstructionStart > 0 ? Time.unscaledTime - obstructionStart : 0)} finite={SafeMath.Finite(cargo.Body.position) && SafeMath.Finite(cargo.Body.linearVelocity) && SafeMath.Finite(cargo.Body.angularVelocity)}");
            }
        }

        void ReleaseRejected(Reason reason)
        {
            if (!pendingRelease) return;
            releaseOutcome = reason.ToString(); pendingRelease = false;
            var cargo = frontend.World.Items[0];
            Debug.Log($"S020 QA rejected={reason} grip={cargo.HeldGripIndex(frontend.Network.LocalActor)} manipulating={frontend.ManipulationActive}");
        }
        
void ArrangeReleaseQA()
        {
            scenario = (scenario + 1) % ReleaseScenarios.Length;
            wall.SetActive(false); latched = armed = pendingRelease = false; stageRelease = 0;
            var world = frontend.World; var player = world.Players[frontend.Network.LocalActor.Id];
            player.Respawn(new Vector3(30, .02f, -4));
            foreach (var item in world.Items) item.ResetState();
            world.Items[1].Body.position = new Vector3(35, .4f, -2);
            var cargo = world.Items[0]; cargo.Body.position = new Vector3(30, .31f, -2.4f); cargo.Body.rotation = Quaternion.identity;
            cargo.SuppressDamageUntil = world.Now + 120; Physics.SyncTransforms();
            var desired = Quaternion.LookRotation(cargo.Body.worldCenterOfMass - player.Eye).eulerAngles;
            var current = frontend.ViewCamera.transform.eulerAngles;
            frontend.ApplyLook(new Vector2(Mathf.DeltaAngle(current.y, desired.y), -Mathf.DeltaAngle(current.x, desired.x)) / frontend.Settings.Sensitivity, false, 0);
            peakLinear = maxAngular = peakForce = maxTorque = peakReleaseLinear = peakReleaseAngular = peakDisplacement = obstructionStart = 0;
            releaseOutcome = "Ready"; frontend.RefreshInteractionFeedback();
            Debug.Log("S020 QA arranged " + scenario + " " + ReleaseScenarios[scenario]);
        }
        void PlaceReleaseObstacle()
        {
            var world = frontend.World; var cargo = world.Items[0]; var player = world.Players[frontend.Network.LocalActor.Id];
            if (scenario != 5)
            {
                cargo.Body.position = player.Eye + Vector3.forward * 1.7f; cargo.Body.rotation = Quaternion.identity;
                cargo.Body.linearVelocity = Vector3.zero; cargo.Body.angularVelocity = Vector3.zero;
            }
            var origin = cargo.Body.position;
            if (scenario == 1) cargo.Body.position = new Vector3(origin.x, .3f, origin.z);
            if (scenario == 2) { wall.transform.position = origin + Vector3.right * 0.8f; wall.transform.localScale = new Vector3(.4f, 2f, 2f); }
            if (scenario == 3) { wall.transform.position = origin - Vector3.up * .27f; wall.transform.localScale = new Vector3(2f, .1f, 2f); }
            if (scenario == 4) { wall.transform.position = origin + player.transform.forward * 0.5f; wall.transform.localScale = new Vector3(2f, 2f, 1.0f); }
            if (scenario == 5) { wall.transform.position = player.transform.position + Vector3.up * 1.5f + player.transform.forward * 2.5f; wall.transform.localScale = new Vector3(4f, 4f, 1f); }
            wall.SetActive(scenario >= 2); Physics.SyncTransforms();
            var result = ReleaseSafety.Evaluate(cargo, world.Config, out var candidate);
            Debug.Log($"S020 QA staged evaluation={result} candidateDisplacement={Vector3.Distance(cargo.Body.position, candidate)}");
            Debug.Log("S020 QA staged " + scenario + " at " + cargo.Body.position);
        }
        void DrawReleaseQA()
        {
            GUI.matrix = Matrix4x4.identity;
            GUI.Box(new Rect(220, 552, 1040, 145), "S020 VISIBLE QA | " + (scenario < 0 ? "Click PLAY SOLO then F6" : ReleaseScenarios[scenario]) +
                "\nF6 next setup (when free) | F grab / release via default LMB | F5 latch default RMB\nF9 yaw replay / F10 pitch replay | F7 arm contact fixture, or clear obstacle | F8 capture\nF2 W replay / F3 D replay / F4 Space replay | Native WASD / mouse / Space remain available");
            if (frontend && scenario >= 0)
                GUI.Label(new Rect(240, 656, 990, 30), $"Grip {frontend.World.Items[0].HeldGripIndex(frontend.Network.LocalActor)} | manipulating {frontend.ManipulationActive} | {releaseOutcome} | speed peak {peakLinear:F2} / spin {maxAngular:F2} | armed {armed}");
        }
        void CleanupReleaseQA()
        {
            if (frontend) { frontend.Network.Rejected -= ReleaseRejected; frontend.Settings.ToggleHold = originalToggle; }
            if (locomotion != null) InputSystem.RemoveDevice(locomotion);
            if (qaFloor) Destroy(qaFloor);
        }
    }
}
#endif
