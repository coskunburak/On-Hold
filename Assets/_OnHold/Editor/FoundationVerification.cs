using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using OnHold.Bootstrap;
using OnHold.Gameplay;
using OnHold.Networking;
using OnHold.Persistence;
using OnHold.Presentation;

namespace OnHold.Editor
{
    public static class FoundationVerification
    {
        [MenuItem("On Hold/Verify existing foundation (read only)")]
        public static void Audit()
        {
            string before = CampaignStore.Hash(File.ReadAllText(FoundationBuilder.ScenePath));
            FoundationBuilder.Validate();
            var roots = SceneManager.GetActiveScene().GetRootGameObjects();
            var objects = roots.SelectMany(r => r.GetComponentsInChildren<Transform>(true)).Select(t => t.gameObject).ToArray();
            Require(objects.SelectMany(o => o.GetComponents<BootstrapEntry>()).Count() == 1, "One bootstrap");
            Require(objects.SelectMany(o => o.GetComponents<FoundationWorld>()).Count() == 1, "One world");
            Require(objects.SelectMany(o => o.GetComponents<NetworkManager>()).Count() == 1, "One NetworkManager");
            Require(objects.SelectMany(o => o.GetComponents<AudioListener>()).Count() == 1, "One local AudioListener");
            var network = objects.SelectMany(o => o.GetComponents<NetworkSession>()).Single();
            var frontend = objects.SelectMany(o => o.GetComponents<FoundationFrontend>()).Single();
            Require(network.Manager && network.Transport && network.World && frontend.Actions && frontend.ViewCamera, "Startup references");
            Require(network.Manager.NetworkConfig.NetworkTransport == network.Transport, "Transport assignment");
            Require(!network.Manager.NetworkConfig.EnableSceneManagement, "Custom snapshot scene management");
            // This foundation replicates explicitly registered stable IDs through NGO custom
            // messages. It intentionally has no NGO NetworkObject spawning or prefab list.
            Require(objects.SelectMany(o => o.GetComponents<NetworkObject>()).Count() == 0, "No competing NetworkObject driver");
            foreach (var go in objects)
            {
                Require(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go) == 0, "Missing script: " + go.name);
                foreach (var component in go.GetComponents<Component>())
                {
                    Require(component != null, "Missing component: " + go.name);
                    using var serialized = new SerializedObject(component);
                    var property = serialized.GetIterator();
                    while (property.Next(true))
                        if (property.propertyType == SerializedPropertyType.ObjectReference)
                            Require(property.objectReferenceValue != null || property.objectReferenceEntityIdValue.Equals(default(EntityId)), "Broken reference: " + go.name + "/" + property.propertyPath);
                }
            }
            foreach (var file in Directory.GetFiles("Assets/_OnHold", "*", SearchOption.AllDirectories).Where(p => !p.EndsWith(".meta")))
                Require(File.Exists(file + ".meta"), "Missing meta: " + file);
            Require(before == CampaignStore.Hash(File.ReadAllText(FoundationBuilder.ScenePath)), "Validation must not rewrite scene");
            Directory.CreateDirectory("docs/Implementation/Evidence");
            File.WriteAllText("docs/Implementation/Evidence/V001-structure.json", JsonUtility.ToJson(new Evidence {
                test = "V001", result = "PASS", editor = Application.unityVersion, objects = objects.Length,
                ngo = PackageInfo.FindForAssembly(typeof(NetworkManager).Assembly).version,
                transport = PackageInfo.FindForAssembly(typeof(Unity.Networking.Transport.NetworkDriver).Assembly).version,
                lockHash = CampaignStore.Hash(File.ReadAllText("Packages/packages-lock.json")), sceneHash = before,
                generated = DateTime.UtcNow.ToString("O"), replication = "NGO custom messages; world-owned registry; zero NetworkObject prefabs by design"
            }, true));
            Debug.Log("ONHOLD V001 PASS: scene, metadata, unique managers, assigned references, stable registry");
        }
        static void Require(bool condition, string detail) { if (!condition) throw new BuildFailedException(detail); }
        [Serializable] sealed class Evidence { public string test, result, editor, ngo, transport, lockHash, sceneHash, generated, replication; public int objects; }
    }
}
