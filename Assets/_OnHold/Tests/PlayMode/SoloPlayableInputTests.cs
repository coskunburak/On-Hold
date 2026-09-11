using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using OnHold.Core;
using OnHold.Domain;
using OnHold.Gameplay;
using OnHold.Networking;
using OnHold.Presentation;

namespace OnHold.Tests
{
    public sealed class SoloPlayableInputTests
    {
        FoundationFrontend frontend;
        NetworkSession network;
        Keyboard keyboard;
        Mouse mouse;
        InputSettings.BackgroundBehavior background;
#if UNITY_EDITOR
        InputSettings.EditorInputBehaviorInPlayMode editorInput;
#endif
        [UnitySetUp] public IEnumerator Setup()
        {
            background = InputSystem.settings.backgroundBehavior;
            InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
#if UNITY_EDITOR
            editorInput = InputSystem.settings.editorInputBehaviorInPlayMode;
            InputSystem.settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
#endif
            yield return SceneManager.LoadSceneAsync("Assets/_OnHold/Scenes/Bootstrap.unity");
            frontend = UnityEngine.Object.FindAnyObjectByType<FoundationFrontend>(); network = frontend.Network;
            keyboard = InputSystem.AddDevice<Keyboard>(); mouse = InputSystem.AddDevice<Mouse>();
            yield return StartSolo();
        }
        IEnumerator StartSolo()
        {
            var task = frontend.PlaySolo(); float deadline = Time.realtimeSinceStartup + 10;
            while ((!task.IsCompleted || network.World.Session?.Phase != SessionPhase.Active) && Time.realtimeSinceStartup < deadline) yield return null;
            Assert.IsTrue(task.IsCompletedSuccessfully); Assert.AreEqual(SessionPhase.Active, network.World.Session?.Phase);
            yield return new WaitForSeconds(.15f);
            Assert.IsFalse(frontend.MenuOpen); Assert.IsTrue(frontend.Actions.FindAction("Gameplay/Jump").enabled);
            if (!Application.isBatchMode)
            { Assert.AreEqual(CursorLockMode.Locked, Cursor.lockState); Assert.IsFalse(Cursor.visible); }
            else TestContext.WriteLine("Native cursor lock is unavailable in headless Unity; verify in the visible Development Build.");
        }
        [UnityTearDown] public IEnumerator Teardown()
        {
            if (keyboard != null) InputSystem.RemoveDevice(keyboard);
            if (mouse != null) InputSystem.RemoveDevice(mouse);
            InputSystem.settings.backgroundBehavior = background;
#if UNITY_EDITOR
            InputSystem.settings.editorInputBehaviorInPlayMode = editorInput;
#endif
            if (network) network.Stop(); yield return null;
        }
        [UnityTest] public IEnumerator S005_OneClickSoloOwnsOneCameraAndListenerAcrossThreeSessions()
        {
            for (int cycle = 0; cycle < 3; cycle++)
            {
                Assert.AreEqual(1, network.World.Players.Count);
                Assert.AreEqual(1, UnityEngine.Object.FindObjectsByType<PlayerBody>(FindObjectsInactive.Exclude).Length);
                Assert.AreEqual(1, Camera.allCamerasCount);
                var listeners = UnityEngine.Object.FindObjectsByType<AudioListener>(FindObjectsInactive.Exclude);
                Assert.AreEqual(1, listeners.Length); Assert.IsTrue(listeners[0].enabled);
                var player = network.World.Players[network.LocalActor.Id];
                Assert.Less(Vector3.Distance(frontend.ViewCamera.transform.position, player.Eye), .01f);
                Assert.IsTrue(player.Grounded); Assert.AreEqual(SessionMode.Solo, network.World.Session.Mode);
                var duplicate = frontend.PlaySolo(); yield return null; Assert.IsTrue(duplicate.IsCompleted); Assert.AreEqual(1, network.World.Players.Count);
                network.Stop(); yield return new WaitForSeconds(.2f);
                Assert.AreEqual(0, network.World.Players.Count); Assert.AreEqual(0, network.World.ActiveHandles);
                Assert.IsNull(network.World.LatestFrame); Assert.IsTrue(frontend.MenuOpen);
                Assert.AreEqual(CursorLockMode.None, Cursor.lockState);
                if (cycle < 2) yield return StartSolo();
            }
        }
        [UnityTest] public IEnumerator S008_RealSpaceBindingSinglePressHoldNoAirJumpMenuBlocksInput()
        {
            var p = network.World.Players[network.LocalActor.Id]; float startY = p.transform.position.y;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Space)); yield return new WaitForSeconds(.14f);
            Assert.Greater(p.transform.position.y, startY + .25f); Assert.IsFalse(p.Grounded);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState()); yield return null;
            float speed = p.VerticalSpeed;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Space)); yield return new WaitForSeconds(.08f);
            Assert.Less(p.VerticalSpeed, speed, "Air press must not add upward speed");
            yield return new WaitForSeconds(.9f); Assert.IsTrue(p.Grounded);
            yield return new WaitForSeconds(.2f); Assert.IsTrue(p.Grounded, "Held Space must not auto-jump on landing");
            InputSystem.QueueStateEvent(keyboard, new KeyboardState()); yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Space)); yield return new WaitForSeconds(.14f);
            Assert.IsFalse(p.Grounded, "Fresh grounded press jumps again");
            InputSystem.QueueStateEvent(keyboard, new KeyboardState()); yield return new WaitForSeconds(.8f);
            frontend.SetMenu(true); yield return new WaitForSeconds(.08f); var pos = p.transform.position; var rotation = frontend.ViewCamera.transform.rotation;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W, Key.Space));
            InputSystem.QueueStateEvent(mouse, new MouseState { delta = new Vector2(200, 200) }); yield return new WaitForSeconds(.3f);
            Assert.Less(Vector3.Distance(pos, p.transform.position), .03f); Assert.IsTrue(p.Grounded);
            Assert.Less(Quaternion.Angle(rotation, frontend.ViewCamera.transform.rotation), .01f);
            Assert.IsFalse(frontend.Actions.FindAction("Gameplay/Move").enabled);
        }
        [UnityTest] public IEnumerator S006_MouseDeltaPitchClampSensitivityAndFramePartition()
        {
            var before = frontend.ViewCamera.transform.rotation;
            InputSystem.QueueStateEvent(mouse, new MouseState { delta = new Vector2(100, 0) }); yield return null; yield return null;
            Assert.AreEqual(100 * frontend.Settings.Sensitivity, Quaternion.Angle(before, frontend.ViewCamera.transform.rotation), .3f);
            foreach (int fps in new[] { 30, 60, 144 })
            {
                before = frontend.ViewCamera.transform.rotation;
                for (int i = 0; i < fps; i++) frontend.ApplyLook(new Vector2(100f / fps, 0), false, 1f / fps);
                yield return null;
                Assert.AreEqual(100 * frontend.Settings.Sensitivity, Quaternion.Angle(before, frontend.ViewCamera.transform.rotation), .1f);
            }
            frontend.ApplyLook(new Vector2(0, 100000), false, .02f); yield return null;
            Assert.AreEqual(85, Vector3.Angle(frontend.ViewCamera.transform.forward, Vector3.ProjectOnPlane(frontend.ViewCamera.transform.forward, Vector3.up)), .1);
            frontend.ApplyLook(new Vector2(0, -100000), false, .02f); yield return null;
            Assert.AreEqual(85, Vector3.Angle(frontend.ViewCamera.transform.forward, Vector3.ProjectOnPlane(frontend.ViewCamera.transform.forward, Vector3.up)), .1);
            Assert.Less(frontend.ViewCamera.transform.forward.y, 0);
        }
        [UnityTest] public IEnumerator S017_ProductionHudUsesGameplayOwnershipAndBindingOverrides()
        {
            var cargo = network.World.Items[0]; var p = network.World.Players[network.LocalActor.Id];
            cargo.ResetState(); cargo.Body.position = p.Eye + frontend.ViewCamera.transform.forward * 1.7f;
            cargo.Body.rotation = Quaternion.identity; Physics.SyncTransforms(); yield return null;
            Assert.IsTrue(frontend.InteractionFeedback.Actionable); Assert.AreEqual(CommandType.Grab, frontend.InteractionFeedback.Action);
            Assert.That(frontend.InteractionPrompt, Does.Contain("Grab"));
            frontend.Actions.FindAction("Gameplay/Grab").ApplyBindingOverride(0, "<Keyboard>/f");
            yield return null; Assert.That(frontend.InteractionPrompt, Does.Contain("F"));
            frontend.SetMenu(true); Assert.IsFalse(frontend.InteractionFeedback.Visible); Assert.AreEqual("", frontend.InteractionPrompt);
            yield return null; frontend.SetMenu(false); yield return null;
            Assert.IsTrue(frontend.InteractionFeedback.Visible);
        }

        [UnityTest] public IEnumerator S018_RealMousePreviewCommitReleaseAndMenuMarkerLifetime()
        {
            frontend.Settings.ToggleHold = true;
            var cargo = network.World.Items[0]; var p = network.World.Players[network.LocalActor.Id];
            for (int cycle = 0; cycle < 3; cycle++)
            {
                cargo.ResetState(); cargo.Body.position = p.Eye + frontend.ViewCamera.transform.forward * 1.7f;
                cargo.Body.rotation = Quaternion.identity; Physics.SyncTransforms();
                InputSystem.QueueStateEvent(mouse, new MouseState()); yield return null;
                var preview = frontend.InteractionFeedback.Grip; Assert.IsTrue(preview.Success); Assert.IsTrue(frontend.GripMarkerVisible);
                int transforms = UnityEngine.Object.FindObjectsByType<Transform>(FindObjectsInactive.Include).Length;
                InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 }); yield return new WaitForSeconds(.08f);
                Assert.AreEqual(preview.GripIndex, cargo.HeldGripIndex(network.LocalActor));
                Assert.AreEqual(CommandType.Release, frontend.InteractionFeedback.Action); Assert.IsFalse(frontend.GripMarkerVisible);
                Assert.That(frontend.InteractionPrompt, Does.Contain("Release"));
                Assert.AreEqual(transforms, UnityEngine.Object.FindObjectsByType<Transform>(FindObjectsInactive.Include).Length, "Marker/hold must not add scene objects");
                InputSystem.QueueStateEvent(mouse, new MouseState()); yield return null;
                InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 }); yield return new WaitForSeconds(.06f);
                Assert.AreEqual(0, cargo.HandleCount); Assert.AreNotEqual(CommandType.Release, frontend.InteractionFeedback.Action);
                InputSystem.QueueStateEvent(mouse, new MouseState()); yield return null;
            }
            frontend.SetMenu(true); Assert.IsFalse(frontend.GripMarkerVisible); Assert.AreEqual("", frontend.InteractionPrompt);
            frontend.SetMenu(false); yield return null;
            network.Stop(); yield return null; Assert.IsFalse(frontend.GripMarkerVisible);
        }
        [UnityTest] public IEnumerator S019_RealModifierRoutesMouseAndRestoresCameraOnRelease()
        {
            frontend.Settings.ToggleHold = true; yield return PlaceAndClickCargo();
            var cargo = network.World.Items[0]; int grip = cargo.HeldGripIndex(network.LocalActor);
            InputSystem.QueueStateEvent(mouse, new MouseState()); yield return null;
            var camera = frontend.ViewCamera.transform.rotation; var initial = cargo.Body.rotation;
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 2, delta = new Vector2(100, 100) }); yield return null;
            Assert.IsTrue(frontend.ManipulationActive); Assert.Less(Quaternion.Angle(camera, frontend.ViewCamera.transform.rotation), .01f);
            for (int i = 0; i < 50; i++)
            {
                InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 2, delta = new Vector2(10, 7) }); yield return new WaitForSeconds(.02f);
                Assert.AreEqual(grip, cargo.HeldGripIndex(network.LocalActor));
                Assert.Less(Quaternion.Angle(camera, frontend.ViewCamera.transform.rotation), .01f);
            }
            Assert.Greater(Quaternion.Angle(initial, cargo.Body.rotation), 15);
            InputSystem.QueueStateEvent(mouse, new MouseState { delta = new Vector2(100, 100) }); yield return null;
            Assert.IsFalse(frontend.ManipulationActive); Assert.Less(Quaternion.Angle(camera, frontend.ViewCamera.transform.rotation), .01f);
            InputSystem.QueueStateEvent(mouse, new MouseState { delta = new Vector2(15, 0) }); yield return null;
            Assert.Greater(Quaternion.Angle(camera, frontend.ViewCamera.transform.rotation), .2f);
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 2 }); yield return null;
            Assert.IsTrue(frontend.ManipulationActive);
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 3 }); yield return new WaitForSeconds(.06f);
            Assert.AreEqual(0, cargo.HandleCount); Assert.IsFalse(frontend.ManipulationActive);
            camera = frontend.ViewCamera.transform.rotation;
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 2, delta = new Vector2(15, 0) }); yield return null;
            Assert.Greater(Quaternion.Angle(camera, frontend.ViewCamera.transform.rotation), .2f, "Modifier without cargo must not freeze camera");
        }
        [UnityTest] public IEnumerator S019_ModifierWithoutCargoMenuDisableAndSessionCleanup()
        {
            var camera = frontend.ViewCamera.transform.rotation;
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 2, delta = new Vector2(15, 0) }); yield return null;
            Assert.IsFalse(frontend.ManipulationActive); Assert.Greater(Quaternion.Angle(camera, frontend.ViewCamera.transform.rotation), .2f);
            frontend.Settings.ToggleHold = true; yield return PlaceAndClickCargo();
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 2 }); yield return null; Assert.IsTrue(frontend.ManipulationActive);
            frontend.enabled = false; yield return new WaitForSeconds(.06f);
            Assert.IsFalse(frontend.ManipulationActive); Assert.AreEqual(0, network.World.ActiveHandles);
            Assert.IsFalse(frontend.Actions.FindAction("Gameplay/Manipulate").enabled);
            frontend.enabled = true; frontend.SetMenu(false); yield return PlaceAndClickCargo();
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 2 }); yield return null; Assert.IsTrue(frontend.ManipulationActive);
            network.Stop(); yield return null; Assert.IsFalse(frontend.ManipulationActive); Assert.IsTrue(frontend.MenuOpen);
        }
        [UnityTest] public IEnumerator S019_ReboundModifierAndGamepadUseSameCameraContract()
        {
            frontend.Settings.ToggleHold = true; yield return PlaceAndClickCargo();
            var action = frontend.Actions.FindAction("Gameplay/Manipulate"); action.ApplyBindingOverride(0, "<Keyboard>/f");
            InputSystem.QueueStateEvent(mouse, new MouseState());
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.F)); yield return null;
            Assert.IsTrue(frontend.ManipulationActive);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState()); yield return null;
            var pad = InputSystem.AddDevice<Gamepad>();
            try
            {
                InputSystem.QueueStateEvent(pad, new GamepadState { leftTrigger = 1 }); yield return null;
                Assert.IsTrue(frontend.ManipulationActive); var camera = frontend.ViewCamera.transform.rotation;
                for (int i = 0; i < 20; i++)
                { InputSystem.QueueStateEvent(pad, new GamepadState { leftTrigger = 1, rightStick = new Vector2(.8f, .5f) }); yield return new WaitForSeconds(.02f); }
                Assert.Less(Quaternion.Angle(camera, frontend.ViewCamera.transform.rotation), .01f);
                Assert.IsTrue(network.World.Items[0].TryHeldOrientation(network.LocalActor, out var state)); Assert.IsTrue(state.Engaged);
                InputSystem.QueueStateEvent(pad, new GamepadState()); yield return null; Assert.IsFalse(frontend.ManipulationActive);
            }
            finally { InputSystem.RemoveDevice(pad); }
        }
        [UnityTest] public IEnumerator S019_DestroyedCargoImmediatelyRestoresCameraEligibility()
        {
            frontend.Settings.ToggleHold = true; yield return PlaceAndClickCargo();
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 2 }); yield return null;
            Assert.IsTrue(frontend.ManipulationActive);
            UnityEngine.Object.Destroy(network.World.Items[0].gameObject); yield return null; yield return null;
            Assert.IsFalse(frontend.ManipulationActive); Assert.AreEqual(0, network.World.ActiveHandles);
            var camera = frontend.ViewCamera.transform.rotation;
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 2, delta = new Vector2(15, 0) }); yield return null;
            Assert.Greater(Quaternion.Angle(camera, frontend.ViewCamera.transform.rotation), .2f);
            frontend.SetMenu(true); Assert.IsFalse(frontend.ManipulationActive);
        }
        [UnityTest] public IEnumerator S020_BlockedMouseReleasePreservesManipulationAndRetries()
        {
            frontend.Settings.ToggleHold = true; yield return PlaceAndClickCargo();
            var world = network.World; var cargo = world.Items[0];
            world.enabled = false; // Explicitly advance queued production commands below.
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 2 }); yield return null;
            Assert.IsTrue(frontend.ManipulationActive);
            var barrier = GameObject.CreatePrimitive(PrimitiveType.Cube);
            try
            {
                barrier.layer = 8; barrier.transform.position = cargo.Body.position; barrier.transform.localScale = Vector3.one * 6;
                Physics.SyncTransforms();
                int grip = cargo.HeldGripIndex(network.LocalActor); uint revision = cargo.State.Revision;
                var camera = frontend.ViewCamera.transform.rotation;
                InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 3 }); yield return null;
                Assert.IsTrue(frontend.ManipulationActive, "A release request must not dispose control before the authority decides");
                world.Step(.02f); yield return null;
                Assert.AreEqual(Reason.Blocked, network.LastReason);
                Assert.AreEqual(grip, cargo.HeldGripIndex(network.LocalActor)); Assert.AreEqual(revision, cargo.State.Revision);
                Assert.IsTrue(frontend.ManipulationActive);
                InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 2, delta = new Vector2(20, 10) }); yield return null;
                Assert.Less(Quaternion.Angle(camera, frontend.ViewCamera.transform.rotation), .01f);
                Assert.IsTrue(cargo.TryHeldOrientation(network.LocalActor, out var orientation)); Assert.IsTrue(orientation.Active);
                barrier.SetActive(false);
                var player = world.Players[network.LocalActor.Id];
                cargo.Body.position = player.Eye + frontend.ViewCamera.transform.forward * 1.7f;
                cargo.Body.rotation = Quaternion.identity; Physics.SyncTransforms();
                InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 3 }); yield return null;
                world.Step(.02f); yield return null;
                Assert.AreEqual(0, cargo.HandleCount); Assert.IsFalse(frontend.ManipulationActive);
                Assert.AreEqual(Vector3.zero, cargo.LastForce); Assert.AreEqual(Vector3.zero, cargo.LastTorque);
            }
            finally { UnityEngine.Object.Destroy(barrier); world.enabled = true; }
        }

        IEnumerator PlaceAndClickCargo()
        {
            var cargo = network.World.Items[0]; var p = network.World.Players[network.LocalActor.Id];
            cargo.ResetState(); cargo.Body.position = p.Eye + frontend.ViewCamera.transform.forward * 1.7f;
            cargo.Body.rotation = Quaternion.identity; Physics.SyncTransforms();
            InputSystem.QueueStateEvent(mouse, new MouseState()); yield return new WaitForSeconds(.04f);
            Assert.AreSame(cargo, InteractionRay.Detect(frontend.ViewCamera.transform.position, frontend.ViewCamera.transform.forward, network.World.Config).Item);
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 }); yield return new WaitForSeconds(.08f);
            Assert.AreEqual(1, cargo.HandleCount); Assert.AreEqual(Handling.Held, cargo.State.Handling);
        }
        [UnityTest] public IEnumerator S014_S015_RealMouseHoldToggleMenuAndSessionExitCleanUp()
        {
            frontend.Settings.ToggleHold = false; yield return PlaceAndClickCargo();
            InputSystem.QueueStateEvent(mouse, new MouseState()); yield return new WaitForSeconds(.08f);
            Assert.AreEqual(0, network.World.ActiveHandles);
            frontend.Settings.ToggleHold = true; yield return PlaceAndClickCargo();
            InputSystem.QueueStateEvent(mouse, new MouseState()); yield return new WaitForSeconds(.06f); Assert.AreEqual(1, network.World.ActiveHandles);
            InputSystem.QueueStateEvent(mouse, new MouseState { buttons = 1 }); yield return new WaitForSeconds(.06f); Assert.AreEqual(0, network.World.ActiveHandles);
            yield return PlaceAndClickCargo(); frontend.SetMenu(true); yield return new WaitForSeconds(.08f); Assert.AreEqual(0, network.World.ActiveHandles);
            frontend.SetMenu(false); yield return PlaceAndClickCargo(); network.Stop(); yield return new WaitForSeconds(.2f);
            Assert.AreEqual(0, network.World.ActiveHandles); Assert.IsTrue(frontend.MenuOpen);
            yield return StartSolo(); Assert.AreEqual(0, network.World.ActiveHandles); Assert.AreEqual(1, network.World.Players.Count);
            Assert.IsFalse(InteractionRay.Detect(frontend.ViewCamera.transform.position, frontend.ViewCamera.transform.forward, network.World.Config).Valid, "No stale target from previous session");
        }
        [UnityTest] public IEnumerator S007_RealWASDAndEscapeImmediatelyStopHorizontalIntent()
        {
            var p = network.World.Players[network.LocalActor.Id]; var start = p.transform.position;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W)); yield return new WaitForSeconds(.3f);
            Assert.Greater(p.transform.position.z - start.z, .5f);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W, Key.Escape)); yield return new WaitForSeconds(.08f);
            Assert.IsTrue(frontend.MenuOpen); start = p.transform.position;
            yield return new WaitForSeconds(.25f); Assert.Less(Vector3.Distance(start, p.transform.position), .03f);
            Assert.AreEqual(CursorLockMode.None, Cursor.lockState); Assert.IsTrue(Cursor.visible);
        }
    }
}
