using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using OnHold.Bootstrap;
using OnHold.Gameplay;
using OnHold.Networking;
using OnHold.Presentation;

namespace OnHold.Tests
{
    /// <summary>
    /// V003 — Bootstrap Runtime Smoke.
    /// Loads the generated Bootstrap scene and verifies that all foundation
    /// objects initialize without fatal errors for a bounded smoke duration.
    /// </summary>
    public sealed class BootstrapRuntimeSmoke
    {
        const string BootstrapScene = "Assets/_OnHold/Scenes/Bootstrap.unity";
        const float SmokeDuration = 2f;

        [UnityTest]
        public IEnumerator BootstrapScene_LoadsAndInitializes_WithoutFatalErrors()
        {
            // Load the generated Bootstrap scene.
            var asyncOp = SceneManager.LoadSceneAsync(BootstrapScene, LoadSceneMode.Single);
            Assert.IsNotNull(asyncOp, "Scene load async operation must not be null");
            yield return asyncOp;

            var scene = SceneManager.GetActiveScene();
            Assert.AreEqual("Bootstrap", scene.name, "Active scene must be Bootstrap");
            Assert.IsTrue(scene.isLoaded, "Scene must be loaded");

            // Verify BootstrapEntry exists and is unique.
            var bootstraps = Object.FindObjectsByType<BootstrapEntry>();
            Assert.AreEqual(1, bootstraps.Length, "Exactly one BootstrapEntry expected");
            var bootstrap = bootstraps[0];
            Assert.IsNotNull(bootstrap.World, "BootstrapEntry.World must be assigned");
            Assert.IsNotNull(bootstrap.Network, "BootstrapEntry.Network must be assigned");

            // Verify FoundationWorld initialized.
            var world = bootstrap.World;
            Assert.IsNotNull(world.Config, "FoundationConfig must be loaded by Awake/Initialize");
            Assert.IsNotNull(world.Platform, "Platform reference must be assigned");
            Assert.IsNotNull(world.DeliveryZone, "DeliveryZone must be assigned");
            Assert.IsNotNull(world.PlayerPrefab, "PlayerPrefab must be assigned");
            Assert.IsTrue(world.Items != null && world.Items.Length >= 2, "At least 2 items must exist");
            Assert.IsTrue(world.RecoveryPoints != null && world.RecoveryPoints.Length >= 2, "At least 2 recovery points");
            Assert.IsTrue(world.PlayerSpawns != null && world.PlayerSpawns.Length == 4, "Exactly 4 player spawns");

            // Verify NetworkManager exists and is unique.
            var managers = Object.FindObjectsByType<Unity.Netcode.NetworkManager>();
            Assert.AreEqual(1, managers.Length, "Exactly one NetworkManager expected");
            Assert.IsNotNull(managers[0].NetworkConfig.NetworkTransport, "NetworkTransport must be assigned");

            // Verify NetworkSession.
            var sessions = Object.FindObjectsByType<NetworkSession>();
            Assert.AreEqual(1, sessions.Length, "Exactly one NetworkSession expected");
            Assert.IsNotNull(sessions[0].Manager, "NetworkSession.Manager must be assigned");
            Assert.IsNotNull(sessions[0].Transport, "NetworkSession.Transport must be assigned");

            // Verify Frontend.
            var frontends = Object.FindObjectsByType<FoundationFrontend>();
            Assert.AreEqual(1, frontends.Length, "Exactly one FoundationFrontend expected");
            Assert.IsNotNull(frontends[0].Actions, "InputActions must be assigned");

            // Verify no duplicate persistent managers.
            var audioListeners = Object.FindObjectsByType<AudioListener>();
            Assert.AreEqual(1, audioListeners.Length, "Exactly one AudioListener expected");

            // Run the scene for a bounded smoke duration and check for errors.
            // LogAssert will fail the test if any unexpected errors or exceptions occur.
            float elapsed = 0;
            while (elapsed < SmokeDuration)
            {
                yield return null;
                elapsed += Time.deltaTime;
            }

            // Final sanity: scene still loaded, world not destroyed.
            Assert.IsTrue(world != null, "FoundationWorld must survive smoke duration");
            Assert.IsTrue(bootstrap != null, "BootstrapEntry must survive smoke duration");
        }
    }
}
