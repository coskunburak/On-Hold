using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using OnHold.Bootstrap;
using OnHold.Gameplay;
using OnHold.Networking;
using OnHold.Presentation;
using OnHold.Domain;
using Unity.Netcode;

namespace OnHold.Tests
{
    public sealed class SoloSessionSmoke
    {
        const string BootstrapScene = "Assets/_OnHold/Scenes/Bootstrap.unity";
        const float TimeoutSeconds = 10f;

        [UnityTest]
        public IEnumerator SoloSession_StartsAndReachesActiveState_WithoutRelay()
        {
            // 1. Load the generated Bootstrap scene.
            var asyncOp = SceneManager.LoadSceneAsync(BootstrapScene, LoadSceneMode.Single);
            yield return asyncOp;

            var networkSession = Object.FindAnyObjectByType<NetworkSession>();
            Assert.IsNotNull(networkSession, "NetworkSession must exist");
            
            // 2. Start Solo Connection
            var connectionTask = networkSession.StartConnection(host: true, relay: false, mode: SessionMode.Solo, addressOrCode: "127.0.0.1");
            
            // Wait for connection to start
            while (!connectionTask.IsCompleted)
            {
                yield return null;
            }
            
            Assert.IsTrue(networkSession.Manager.IsHost, "NetworkManager should be running as Host");
            Assert.AreEqual(SessionMode.Solo, networkSession.World.Session.Mode, "Session Mode should be Solo");

            // 3. Go through the session lifecycle manually or wait for the host to auto-transition.
            // Wait a moment for network initialization and Lobby entry
            yield return new WaitForSeconds(0.5f);

            var coordinator = networkSession.World.Session;
            Assert.IsNotNull(coordinator, "SessionCoordinator should be created");

            // Join the local player to the Domain session via NetworkSession's internal mechanics
            // Actually, NetworkSession creates a connection and sends a Hello payload. 
            // The Host auto-accepts and auto-transitions if we send the right commands.
            // In Solo, we just send CommandType.Start from Lobby, then Ready from Loading, then Start from Briefing.

            // Send Start (Lobby -> Loading)
            networkSession.Send(CommandType.Start);
            yield return WaitUntilPhase(coordinator, SessionPhase.Loading);
            
            // Send Ready (Loading)
            networkSession.Send(CommandType.Ready);
            
            // Send Start (Loading -> Briefing)
            networkSession.Send(CommandType.Start);
            yield return WaitUntilPhase(coordinator, SessionPhase.Briefing);
            
            // Send Start (Briefing -> Active)
            networkSession.Send(CommandType.Start);
            yield return WaitUntilPhase(coordinator, SessionPhase.Active);

            Assert.AreEqual(SessionPhase.Active, coordinator.Phase, "Session should reach Active phase");

            // Verify exactly one local player exists
            var players = Object.FindObjectsByType<PlayerBody>(FindObjectsInactive.Exclude);
            Assert.AreEqual(1, players.Length, "Exactly one PlayerBody should be spawned for Solo mode");

            // Shutdown cleanly
            networkSession.Stop();
            
            yield return new WaitForSeconds(0.5f);
            
            Assert.IsFalse(networkSession.Manager.IsHost, "NetworkManager should stop running");
        }

        private IEnumerator WaitUntilPhase(SessionCoordinator coordinator, SessionPhase targetPhase)
        {
            float timer = 0f;
            while (coordinator.Phase != targetPhase && timer < TimeoutSeconds)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            Assert.AreEqual(targetPhase, coordinator.Phase, $"Failed to reach phase {targetPhase} within {TimeoutSeconds} seconds. Stuck in {coordinator.Phase}.");
        }
    }
}
