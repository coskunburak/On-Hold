using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using OnHold.Core;
using OnHold.Gameplay;
using OnHold.Networking;
using OnHold.Presentation;
using OnHold.Bootstrap;
using OnHold.Persistence;

namespace OnHold.Editor
{
    public static class FoundationBuilder
    {
        const string Root = "Assets/_OnHold";
        public const string ScenePath = Root + "/Scenes/Bootstrap.unity";
        [MenuItem("On Hold/Generate foundation scene and prefabs")]
        public static void Generate()
        {
            foreach (var folder in new[] { "Scenes", "Prefabs", "Data", "Materials" }) Directory.CreateDirectory(Root + "/" + folder);
            AssetDatabase.Refresh();
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var config = new FoundationConfig();
            string configPath = Root + "/Data/FoundationConfig.json";
            if (File.Exists(configPath)) JsonUtility.FromJsonOverwrite(File.ReadAllText(configPath), config);
            config.Validate();
            File.WriteAllText(Root + "/Data/FoundationConfig.json", JsonUtility.ToJson(config, true));
            AssetDatabase.ImportAsset(Root + "/Data/FoundationConfig.json");
            ConfigureLayers();
            var ground = Material("Ground", new Color(.12f, .17f, .2f));
            var wall = Material("Concrete", new Color(.55f, .59f, .57f));
            var orange = Material("Equipment", new Color(1, .45f, .12f));
            var green = Material("Delivery", new Color(.17f, .65f, .46f));
            var cream = Material("Cargo", new Color(.8f, .74f, .57f));
            var blue = Material("Player", new Color(.25f, .6f, .85f));
            var root = new GameObject("Foundation");
            var world = root.AddComponent<FoundationWorld>();
            world.Configuration = AssetDatabase.LoadAssetAtPath<TextAsset>(Root + "/Data/FoundationConfig.json");
            var floor = Box("Courtyard", new Vector3(0, -.25f, 1), new Vector3(16, .5f, 18), ground, 8, root.transform);
            Box("Upper landing", new Vector3(0, 3.75f, 6), new Vector3(7, .5f, 5), wall, 8, root.transform);
            Box("Passage left", new Vector3(-2.05f, 1, -2.3f), new Vector3(1.4f, 2, 1), wall, 8, root.transform);
            Box("Passage right", new Vector3(2.05f, 1, -2.3f), new Vector3(1.4f, 2, 1), wall, 8, root.transform);
            for (int i = 0; i < 2; i++) Box("Lift rail", new Vector3(i == 0 ? -1.5f : 1.5f, 2.6f, 2.8f), new Vector3(.12f, 5.4f, .12f), orange, 8, root.transform);
            TraversalFixtures(root.transform, wall, ground);
            var platformGo = new GameObject("Platform"); platformGo.transform.SetParent(root.transform); platformGo.transform.position = new Vector3(0, .25f, 2);
            platformGo.layer = 11; platformGo.AddComponent<Rigidbody>().isKinematic = true;
            var platform = platformGo.AddComponent<ControlledPlatform>();
            Box("Deck", platformGo.transform.position, new Vector3(config.DeckWidth, .3f, config.DeckDepth), orange, 11, platformGo.transform);
            Box("Centre marker", platformGo.transform.position + Vector3.up * .155f, new Vector3(.16f, .01f, 1.5f), green, 13, platformGo.transform, false);
            platform.Panel = Box("Winch panel", new Vector3(1, 1.1f, 2.5f), new Vector3(.3f, .35f, .2f), green, 11, platformGo.transform).transform;
            platform.Anchors = new[] { Point("Anchor_L", new Vector3(-.8f, .45f, 2), platformGo.transform), Point("Anchor_R", new Vector3(.8f, .45f, 2), platformGo.transform) };
            PrefabUtility.SaveAsPrefabAsset(platformGo, Root + "/Prefabs/Platform.prefab"); world.Platform = platform;
            var longItem = MakeItem("LongLoad", "item.longload.a", new Vector3(2, .6f, .7f), 100, 200, cream);
            var crateItem = MakeItem("Crate", "item.crate.a", Vector3.one * .65f, 35, 100, cream);
            world.Items = new[] { SpawnItem(longItem, "cargo.longload.01", 1, new Vector3(0, .55f, -.7f), root.transform), SpawnItem(crateItem, "cargo.crate.01", 2, new Vector3(2.5f, .5f, -.7f), root.transform) };
            var player = new GameObject("Player"); player.layer = 9;
            var cc = player.AddComponent<CharacterController>(); cc.height = config.PlayerHeight; cc.radius = config.PlayerRadius; cc.center = Vector3.up * (config.PlayerHeight / 2); cc.stepOffset = config.StepHeight; cc.skinWidth = config.SkinWidth; cc.slopeLimit = config.SlopeLimit + .1f; cc.minMoveDistance = 0;
            player.AddComponent<PlayerBody>(); var mesh = GameObject.CreatePrimitive(PrimitiveType.Capsule); mesh.name = "Visual"; mesh.transform.SetParent(player.transform); mesh.transform.localPosition = Vector3.up * .9f; mesh.transform.localScale = new Vector3(.6f, .9f, .6f); UnityEngine.Object.DestroyImmediate(mesh.GetComponent<Collider>()); mesh.GetComponent<Renderer>().sharedMaterial = blue;
            world.PlayerPrefab = PrefabUtility.SaveAsPrefabAsset(player, Root + "/Prefabs/Player.prefab"); UnityEngine.Object.DestroyImmediate(player);
            world.PlayerSpawns = Enumerable.Range(0, 4).Select(i => Point("SafePlayer_" + i, new Vector3(-3 + i * 2, .05f, -5), root.transform)).ToArray();
            world.RecoveryPoints = new[] { Point("Recovery_A", new Vector3(-4, 1, 0), root.transform), Point("Recovery_B", new Vector3(4, 1, 0), root.transform) };
            var zone = new GameObject("Delivery volume"); zone.transform.SetParent(root.transform); zone.transform.position = new Vector3(0, 5, 6); zone.layer = 12;
            world.DeliveryZone = zone.AddComponent<BoxCollider>(); world.DeliveryZone.isTrigger = true; world.DeliveryZone.size = new Vector3(5, 2, 3);
            Box("Delivery bay", new Vector3(0, 4.015f, 6), new Vector3(5, .025f, 3), green, 13, root.transform, false);
            Label("ON HOLD", new Vector3(-4, 3, 4), root.transform, .22f);
            Label("DELIVERY  /  02", new Vector3(-2, 6.2f, 7.8f), root.transform, .12f);
            Label("LIFT TO LANDING", new Vector3(-1.25f, 1.7f, 3.1f), root.transform, .065f);
            var sun = new GameObject("Sun").AddComponent<Light>(); sun.type = LightType.Directional; sun.intensity = 2; sun.shadows = LightShadows.Soft; sun.transform.rotation = Quaternion.Euler(48, -35, 0);
            RenderSettings.ambientMode = AmbientMode.Flat; RenderSettings.ambientLight = new Color(.6f, .65f, .7f);
            var camera = new GameObject("Local camera").AddComponent<Camera>(); camera.tag = "MainCamera"; camera.gameObject.AddComponent<AudioListener>(); camera.nearClipPlane = .05f; camera.farClipPlane = 100; camera.clearFlags = CameraClearFlags.SolidColor; camera.backgroundColor = new Color(.16f, .23f, .28f);
            var netGo = new GameObject("NetworkManager"); var manager = netGo.AddComponent<NetworkManager>(); var transport = netGo.AddComponent<UnityTransport>();
            var network = root.AddComponent<NetworkSession>(); network.World = world; network.Manager = manager; network.Transport = transport;
            manager.NetworkConfig.NetworkTransport = transport; manager.NetworkConfig.EnableSceneManagement = false; manager.NetworkConfig.ConnectionApproval = true;
            var frontend = root.AddComponent<FoundationFrontend>(); frontend.Network = network; frontend.World = world; frontend.ViewCamera = camera;
            frontend.Actions = AssetDatabase.LoadAssetAtPath<InputActionAsset>(Root + "/Data/Foundation.inputactions");
            var bootstrap = root.AddComponent<BootstrapEntry>(); bootstrap.World = world; bootstrap.Network = network;
            world.ContentHash = CampaignStore.Hash(world.Configuration.text + "|foundation.1|" + string.Join("|", world.Items.Select(i => JsonUtility.ToJson(i.Definition))));
            EditorSceneManager.SaveScene(scene, ScenePath);
            // Keep the existing SampleScene build entry, with Bootstrap as the real startup.
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) }.Concat(EditorBuildSettings.scenes.Where(s => s.path != ScenePath)).ToArray();
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone, ScriptingImplementation.Mono2x);
            AssetDatabase.SaveAssets(); Validate(); Debug.Log("ONHOLD foundation generated and validated");
        }
        static void TraversalFixtures(Transform root, Material wall, Material ground)
        {
            var fixtures = new GameObject("S005-016 traversal fixtures").transform; fixtures.SetParent(root);
            Box("Traversal annex", new Vector3(-16, -.25f, 0), new Vector3(16, .5f, 18), ground, 8, fixtures);
            Box("Low step 0.12m", new Vector3(-10, .06f, -3), new Vector3(2, .12f, 1), wall, 8, fixtures);
            Box("Medium step 0.28m", new Vector3(-10, .14f, 0), new Vector3(2, .28f, 1), wall, 8, fixtures);
            Box("Rejected step 0.8m", new Vector3(-10, .4f, 3), new Vector3(2, .8f, 1), wall, 8, fixtures);
            for (int i = 0; i < 3; i++)
            {
                float angle = new[] { 20f, 45f, 60f }[i];
                var rotation = Quaternion.Euler(-angle, 0, 0);
                var start = new Vector3(-14 - i * 4, 0, -1);
                var ramp = Box("Slope " + angle + " degrees", start + rotation * new Vector3(0, -.1f, 2.5f), new Vector3(2, .2f, 5), wall, 8, fixtures);
                ramp.transform.rotation = rotation;
            }
            Box("Low ceiling", new Vector3(-10, 2.1f, -6), new Vector3(2, .2f, 2), wall, 8, fixtures);
            Box("Narrow doorway left", new Vector3(-17, 1.2f, -5), new Vector3(1, 2.4f, .4f), wall, 8, fixtures);
            Box("Narrow doorway right", new Vector3(-15.2f, 1.2f, -5), new Vector3(1, 2.4f, .4f), wall, 8, fixtures);
            Box("Corner wall A", new Vector3(-23.8f, 1.5f, -5.5f), new Vector3(.2f, 3, 6), wall, 8, fixtures);
            Box("Corner wall B", new Vector3(-22, 1.5f, -8.4f), new Vector3(3.8f, 3, .2f), wall, 8, fixtures);
            Label("TRAVERSAL TESTS  /  SPACE TO JUMP", new Vector3(-21, 3, 7), fixtures, .08f);
        }
        static ItemDefinition MakeItem(string name, string id, Vector3 size, float mass, int value, Material material)
        {
            string path = Root + "/Data/" + name + ".asset";
            var definition = AssetDatabase.LoadAssetAtPath<ItemDefinition>(path);
            if (!definition) { definition = ScriptableObject.CreateInstance<ItemDefinition>(); AssetDatabase.CreateAsset(definition, path); }
            definition.DefinitionId = id; definition.DisplayNameKey = id; definition.MassKg = mass; definition.BaseValue = value;
            definition.BoundsSize = size; definition.Grips = new[] { new Vector3(-size.x / 2, .1f, 0), new Vector3(size.x / 2, .1f, 0) };
            var go = new GameObject(name); go.layer = 10; var rb = go.AddComponent<Rigidbody>(); rb.mass = mass;
            go.AddComponent<CarryableBody>().Definition = definition;
            // Two authored collider halves exercise compound-contact damage aggregation.
            Box("Left collision", new Vector3(-size.x / 4, 0, 0), new Vector3(size.x / 2, size.y, size.z), material, 10, go.transform);
            Box("Right collision", new Vector3(size.x / 4, 0, 0), new Vector3(size.x / 2, size.y, size.z), material, 10, go.transform);
            for (int i = 0; i < 2; i++) Point("Grip_" + i, definition.Grips[i], go.transform);
            definition.Prefab = PrefabUtility.SaveAsPrefabAsset(go, Root + "/Prefabs/" + name + ".prefab"); EditorUtility.SetDirty(definition); UnityEngine.Object.DestroyImmediate(go); return definition;
        }
        static CarryableBody SpawnItem(ItemDefinition def, string id, int networkId, Vector3 position, Transform parent)
        {
            var go = (GameObject)PrefabUtility.InstantiatePrefab(def.Prefab); go.transform.SetParent(parent); go.transform.position = position;
            var body = go.GetComponent<CarryableBody>(); body.InstanceId = id; body.NetworkId = networkId; return body;
        }
        static Material Material(string name, Color color)
        {
            string path = Root + "/Materials/" + name + ".mat"; var m = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (!m) { m = new Material(Shader.Find("Universal Render Pipeline/Lit")); AssetDatabase.CreateAsset(m, path); }
            m.color = color; EditorUtility.SetDirty(m); return m;
        }
        static GameObject Box(string name, Vector3 position, Vector3 scale, Material material, int layer, Transform parent, bool collision = true)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube); go.name = name; go.layer = layer; go.transform.SetParent(parent); go.transform.position = position; go.transform.localScale = scale; go.GetComponent<Renderer>().sharedMaterial = material;
            if (!collision) UnityEngine.Object.DestroyImmediate(go.GetComponent<Collider>()); return go;
        }
        static Transform Point(string name, Vector3 position, Transform parent) { var go = new GameObject(name); go.transform.SetParent(parent); go.transform.position = position; return go.transform; }
        static void Label(string text, Vector3 position, Transform parent, float size)
        {
            var go = new GameObject(text); go.transform.SetParent(parent); go.transform.position = position; go.transform.rotation = Quaternion.Euler(0, 180, 0);
            var mesh = go.AddComponent<TextMesh>(); mesh.text = text; mesh.fontSize = 64; mesh.characterSize = size; mesh.color = Color.white;
        }
        static void ConfigureLayers()
        {
            var tags = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]); var layers = tags.FindProperty("layers");
            var names = new[] { "WorldStatic", "Player", "Carryable", "Platform", "Trigger", "Cosmetic" };
            for (int i = 0; i < names.Length; i++) { var layer = layers.GetArrayElementAtIndex(8 + i); if (layer.stringValue != "" && layer.stringValue != names[i]) throw new BuildFailedException("Layer conflict: " + (8 + i)); layer.stringValue = names[i]; }
            tags.ApplyModifiedProperties(); Physics.IgnoreLayerCollision(9, 9, true);
        }
        [MenuItem("On Hold/Validate foundation")]
        public static void Validate()
        {
            var ids = new System.Collections.Generic.HashSet<string>();
            foreach (var guid in AssetDatabase.FindAssets("t:ItemDefinition", new[] { Root }))
            {
                var d = AssetDatabase.LoadAssetAtPath<ItemDefinition>(AssetDatabase.GUIDToAssetPath(guid)); d.Validate(); if (!ids.Add(d.DefinitionId)) throw new BuildFailedException("Duplicate definition");
                var p = d.Prefab;
                if (p.transform.localScale != Vector3.one || p.GetComponentsInChildren<Rigidbody>().Length != 1 || !p.GetComponent<Rigidbody>() || !p.GetComponent<CarryableBody>() || p.GetComponentsInChildren<Collider>().Length < 1 ||
                    p.GetComponentsInChildren<MeshCollider>().Any(m => !m.convex) || p.GetComponentsInChildren<Renderer>().Any(r => r.sharedMaterials.Any(m => !m || !m.shader || m.shader.name.Contains("Error")))) throw new BuildFailedException("Invalid prefab " + d.DefinitionId);
            }
            if (ids.Count < 2) throw new BuildFailedException("Foundation requires two item definitions");
            var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            var w = UnityEngine.Object.FindAnyObjectByType<FoundationWorld>();
            if (!w || !w.PlayerPrefab || !w.Platform || !w.DeliveryZone || w.Items.Length < 2 || w.RecoveryPoints.Length < 2 || w.PlayerSpawns.Length != 4 ||
                w.Items.Select(i => i.InstanceId).Distinct().Count() != w.Items.Length || w.Items.Select(i => i.NetworkId).Distinct().Count() != w.Items.Length ||
                w.Items.Any(i => !Numbers.Id(i.InstanceId) || i.NetworkId <= 0) || !UnityEngine.Object.FindAnyObjectByType<FoundationFrontend>().Actions) throw new BuildFailedException("Incomplete scene wiring");
            foreach (var file in Directory.GetFiles(Root + "/Runtime/Domain", "*.cs"))
                if (File.ReadAllText(file).Contains("using Unity")) throw new BuildFailedException("Domain dependency leak");
        }
        public static void BuildMac() => Build(BuildTarget.StandaloneOSX, "Builds/Mac/On Hold.app");
        public static void BuildWindows() => Build(BuildTarget.StandaloneWindows64, "Builds/Windows/On Hold.exe");
        static void Build(BuildTarget target, string output)
        {
            Validate(); Directory.CreateDirectory(Path.GetDirectoryName(output));
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions { scenes = new[] { ScenePath }, target = target, locationPathName = output, options = BuildOptions.Development });
            Directory.CreateDirectory("docs/Implementation/Evidence");
            File.WriteAllText("docs/Implementation/Evidence/build-" + target + ".json", JsonUtility.ToJson(new BuildEvidence { target = target.ToString(), result = report.summary.result.ToString(), seconds = report.summary.totalTime.TotalSeconds, bytes = report.summary.totalSize, errors = report.summary.totalErrors, warnings = report.summary.totalWarnings, editor = Application.unityVersion, lockHash = CampaignStore.Hash(File.ReadAllText("Packages/packages-lock.json")) }, true));
            if (report.summary.result != BuildResult.Succeeded) throw new BuildFailedException("Foundation build failed");
        }
        [Serializable] sealed class BuildEvidence { public string target, result, editor, lockHash; public double seconds; public ulong bytes; public int errors, warnings; }
    }
    public sealed class FoundationBuildGuard : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;
        public void OnPreprocessBuild(BuildReport report) { FoundationBuilder.Validate(); }
    }
}
