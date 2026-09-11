using System;
using System.IO;
using System.Linq;
using OnHold.Gameplay;
using OnHold.Persistence;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace OnHold.Editor
{
    public static class HeldManipulationVerification
    {
        public static void RefreshContentHash()
        {
            FoundationBuilder.Validate();
            var world = UnityEngine.Object.FindAnyObjectByType<FoundationWorld>();
            string hash = CampaignStore.Hash(world.Configuration.text + "|foundation.1|" + string.Join("|", world.Items.Select(i => JsonUtility.ToJson(i.Definition))));
            const string scene = "Assets/_OnHold/Scenes/Bootstrap.unity";
            string text = File.ReadAllText(scene);
            text = text.Replace("  ContentHash: " + world.ContentHash, "  ContentHash: " + hash);
            File.WriteAllText(scene, text); AssetDatabase.ImportAsset(scene);
        }
        public static void BuildPlayers() { BuildDevelopment(); BuildVisibleQA(); }
        public static void BuildDevelopment() => Build(false, "S019", "ONHOLD_MANIPULATION_ACCEPTANCE");
        public static void BuildVisibleQA() => Build(true, "S019", "ONHOLD_MANIPULATION_ACCEPTANCE");
        public static void BuildS020Development() => Build(false, "S020", "ONHOLD_RELEASE_ACCEPTANCE");
        public static void BuildS020VisibleQA() => Build(true, "S020", "ONHOLD_RELEASE_ACCEPTANCE");
        static void Build(bool qa, string sprint, string qaDefine)
        {
            var target = EditorUserBuildSettings.activeBuildTarget;
            if (target != BuildTarget.StandaloneOSX && target != BuildTarget.StandaloneWindows64 && target != BuildTarget.StandaloneLinux64)
                throw new InvalidOperationException(sprint + " validation requires the configured desktop target");
            string directory = "Builds/" + sprint + (qa ? "-QA" : "");
            string path = directory + "/On Hold" + (target == BuildTarget.StandaloneOSX ? ".app" : target == BuildTarget.StandaloneWindows64 ? ".exe" : "");
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions {
                scenes = new[] { "Assets/_OnHold/Scenes/Bootstrap.unity" }, target = target,
                locationPathName = path, options = BuildOptions.Development,
                extraScriptingDefines = qa ? new[] { qaDefine } : Array.Empty<string>()
            });
            File.WriteAllText("docs/Implementation/Evidence/" + sprint + "-" + (qa ? "qa" : "development") + "-build.json", JsonUtility.ToJson(new Evidence {
                result = report.summary.result.ToString(), target = target.ToString(), editor = Application.unityVersion,
                path = Path.GetFullPath(path), errors = report.summary.totalErrors, warnings = report.summary.totalWarnings,
                seconds = report.summary.totalTime.TotalSeconds, utc = DateTime.UtcNow.ToString("O") }, true));
            if (report.summary.result != BuildResult.Succeeded) throw new InvalidOperationException(sprint + " build failed");
        }
        [Serializable] sealed class Evidence { public string result, target, editor, path, utc; public int errors, warnings; public double seconds; }
    }
}
