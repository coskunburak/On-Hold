using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
namespace OnHold.Editor
{
    public static class InteractionFeedbackVerification
    {
        public static void BuildVisibleQA()
        {
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions {
                scenes = new[] { "Assets/_OnHold/Scenes/Bootstrap.unity" }, target = BuildTarget.StandaloneOSX,
                locationPathName = "Builds/FeedbackQA/On Hold Feedback.app", options = BuildOptions.Development,
                extraScriptingDefines = new[] { "ONHOLD_FEEDBACK_ACCEPTANCE" }
            });
            File.WriteAllText("docs/Implementation/Evidence/feedback-qa-build.json", JsonUtility.ToJson(new Evidence {
                result = report.summary.result.ToString(), errors = report.summary.totalErrors,
                warnings = report.summary.totalWarnings, utc = DateTime.UtcNow.ToString("O") }, true));
            if (report.summary.result != BuildResult.Succeeded) throw new InvalidOperationException("Feedback QA build failed");
        }
        [Serializable] sealed class Evidence { public string result, utc; public int errors, warnings; }
    }
}
