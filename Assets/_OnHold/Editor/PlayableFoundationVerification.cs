using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using OnHold.Gameplay;
using OnHold.Presentation;

namespace OnHold.Editor
{
    public static class PlayableFoundationVerification
    {
        // Explicit authoring command; never executes on import or during normal play.
        public static void BuildVerifiedMac() { GenerateAndVerify(); FoundationBuilder.BuildMac(); }
        public static void GenerateAndVerify()
        {
            FoundationBuilder.Generate(); var first = Inspect();
            FoundationBuilder.Generate(); var second = Inspect();
            if (first.objects != second.objects || first.fixtureObjects != second.fixtureObjects || first.itemIds != second.itemIds)
                throw new InvalidOperationException("Foundation generation is not idempotent");
            second.generatedTwice = true;
            File.WriteAllText("docs/Implementation/Evidence/S005-016-generation.json", JsonUtility.ToJson(second, true));
            Debug.Log("S005-016 generation: PASS; repeated generation preserves unique managers, cameras, fixtures and item IDs");
        }
        static Evidence Inspect()
        {
            FoundationBuilder.Validate();
            var objects = SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(r => r.GetComponentsInChildren<Transform>(true)).ToArray();
            var world = UnityEngine.Object.FindAnyObjectByType<FoundationWorld>();
            var frontend = UnityEngine.Object.FindAnyObjectByType<FoundationFrontend>();
            if (objects.Count(t => t.GetComponent<FoundationWorld>()) != 1 || objects.Count(t => t.GetComponent<NetworkManager>()) != 1 ||
                objects.Count(t => t.GetComponent<Camera>()) != 1 || objects.Count(t => t.GetComponent<AudioListener>()) != 1 ||
                objects.Count(t => t.GetComponent<PlayerBody>()) != 0 || world.PlayerPrefab.GetComponents<PlayerBody>().Length != 1 ||
                world.PlayerPrefab.GetComponents<CharacterController>().Length != 1 ||
                frontend.Actions.FindAction("Gameplay/Jump", true).bindings.All(b => b.path != "<Keyboard>/space"))
                throw new InvalidOperationException("Invalid playable foundation wiring");
            var fixtures = objects.Single(t => t.name == "S005-016 traversal fixtures");
            return new Evidence { editor = Application.unityVersion, generated = DateTime.UtcNow.ToString("O"), result = "PASS",
                objects = objects.Length, fixtureObjects = fixtures.GetComponentsInChildren<Transform>().Length,
                itemIds = string.Join("|", world.Items.Select(i => i.InstanceId + ":" + i.NetworkId)) };
        }
        [Serializable] sealed class Evidence
        {
            public string result, editor, generated, itemIds;
            public int objects, fixtureObjects;
            public bool generatedTwice;
        }
    }
}
