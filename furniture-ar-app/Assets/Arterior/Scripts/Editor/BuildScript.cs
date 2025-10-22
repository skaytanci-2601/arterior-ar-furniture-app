using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace Arterior
{
    /// <summary>
    /// Build script for automating iOS and Android builds
    /// </summary>
    public class BuildScript
    {
        private static string buildPath = "Builds";
        
        [MenuItem("Build/Build iOS")]
        public static void BuildiOS()
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = new[] { "Assets/Arterior/Scenes/Main.unity" };
            buildPlayerOptions.locationPathName = $"{buildPath}/iOS";
            buildPlayerOptions.target = BuildTarget.iOS;
            buildPlayerOptions.options = BuildOptions.None;
            
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;
            
            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("iOS build succeeded: " + summary.totalSize + " bytes");
            }
            else
            {
                Debug.LogError("iOS build failed");
            }
        }
        
        [MenuItem("Build/Build Android")]
        public static void BuildAndroid()
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = new[] { "Assets/Arterior/Scenes/Main.unity" };
            buildPlayerOptions.locationPathName = $"{buildPath}/Android/Arterior.apk";
            buildPlayerOptions.target = BuildTarget.Android;
            buildPlayerOptions.options = BuildOptions.None;
            
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;
            
            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Android build succeeded: " + summary.totalSize + " bytes");
            }
            else
            {
                Debug.LogError("Android build failed");
            }
        }
        
        [MenuItem("Build/Build All Platforms")]
        public static void BuildAllPlatforms()
        {
            BuildiOS();
            BuildAndroid();
        }
    }
}
