using System;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.Diagnostics;
using System.IO;
public class AutoBuilder
{
    static string[] SCENES = FindEnabledEditorScenes();

    static string APP_NAME = "YourAppName";
    static string TARGET_DIR = "Builds";

    private void Start()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        // Windows-specific code
        TARGET_DIR = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        // Mac-specific code
        TARGET_DIR = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
#else
        // Default code for other platforms (you can modify this based on your needs)
        TARGET_DIR = Application.persistentDataPath;
#endif
    }

    #region AOS
    [MenuItem("Build/Build and Run APK")]
    static void BuildAndRunAPK()
    {
        if (IsAndroidPlatform())
        {
            BuildAPK();
            RunOnAndroidDevice();
        }
        else
        {
            UnityEngine.Debug.LogError("Build target is not supported for APK. Please choose an Android platform.");
        }
    }

    [MenuItem("Build/Build APK")]
    static void BuildAPK()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = SCENES;
        buildPlayerOptions.locationPathName = TARGET_DIR + "/" + APP_NAME + ".apk";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        EditorUserBuildSettings.androidBuildType = AndroidBuildType.Release;
        PlayerSettings.SplashScreen.show = false;

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    [MenuItem("Build/Build and Run AAB")]
    static void BuildAndRunAAB()
    {
        if (IsAndroidPlatform())
        {
            BuildAAB();
            RunOnAndroidDevice();
        }
        else
        {
            UnityEngine.Debug.LogError("Build target is not supported for APK. Please choose an Android platform.");
        }
    }

    [MenuItem("Build/Build aab")]
    static void BuildAAB()
    {
        // Set the keystore information
        PlayerSettings.Android.keystoreName = "YourKeystoreName.keystore";
        PlayerSettings.Android.keystorePass = "YourKeystorePassword";
        PlayerSettings.Android.keyaliasName = "YourKeyAlias";
        PlayerSettings.Android.keyaliasPass = "YourKeyAliasPassword";

        // Set the build options
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = SCENES;
        buildPlayerOptions.locationPathName = TARGET_DIR + "/" + APP_NAME + ".aab";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;

        Directory.CreateDirectory(TARGET_DIR);
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        EditorUserBuildSettings.androidBuildType = AndroidBuildType.Release;
        PlayerSettings.SplashScreen.show = false;

        BuildPipeline.BuildPlayer(buildPlayerOptions);

        // Reset the keystore information to avoid conflicts in future builds
        PlayerSettings.Android.keystoreName = "";
        PlayerSettings.Android.keystorePass = "";
        PlayerSettings.Android.keyaliasName = "";
        PlayerSettings.Android.keyaliasPass = "";
    }

    static void RunOnAndroidDevice()
    {
        // Add code here to run on Android device
        string androidSDKPath = EditorPrefs.GetString("AndroidSdkRoot");
        string packageName = $"com.{PlayerSettings.companyName}." + APP_NAME.ToLower();

        // Example command to install and run the APK on a connected device
        string adbInstallCommand = $"\"{androidSDKPath}/platform-tools/adb\" install -r {TARGET_DIR}/{APP_NAME}.apk";
        string adbRunCommand = $"\"{androidSDKPath}/platform-tools/adb\" shell am start -n {packageName}/{packageName}.MainActivity";


        ExecuteCommand(adbInstallCommand);
        ExecuteCommand(adbRunCommand);
    }

    static bool IsAndroidPlatform()
    {
        return EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android;
    }
    #endregion


    #region iOS
    [MenuItem("Build/Build and Run iOS")]
    static void BuildAndRuniOS()
    {
        if (IsiOSPlatform())
        {
            BuildiOS();
            RunOniOSDevice();
        }
        else
        {
            UnityEngine.Debug.LogError("Build target is not supported for iOS. Please choose an iOS platform.");
        }
    }

    [MenuItem("Build/Build iOS")]
    static void BuildiOS()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = SCENES;
        buildPlayerOptions.locationPathName = TARGET_DIR + "/" + APP_NAME;
        buildPlayerOptions.target = BuildTarget.iOS;
        buildPlayerOptions.options = BuildOptions.None;

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    static void RunOniOSDevice()
    {
        // Add code here to run on iOS device
        string xcodeProjectPath = $"{TARGET_DIR}/{APP_NAME}.xcodeproj";
        string xcodeTarget = APP_NAME;
        string xcodeConfig = "Release";

        // Example command to build and run the Xcode project
        string xcodeBuildCommand = $"xcodebuild -project {xcodeProjectPath} -target {xcodeTarget} -configuration {xcodeConfig} -sdk iphoneos";
        string xcodeRunCommand = $"open -a Xcode {xcodeProjectPath}";

        ExecuteCommand(xcodeBuildCommand);
        ExecuteCommand(xcodeRunCommand);
    }

    static bool IsiOSPlatform()
    {
        return EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS;
    }

    #endregion


    static string[] FindEnabledEditorScenes()
    {
        var editorScenes = EditorBuildSettings.scenes;
        var sceneNames = new string[editorScenes.Length];

        for (int i = 0; i < editorScenes.Length; i++)
        {
            sceneNames[i] = editorScenes[i].path;
        }

        return sceneNames;
    }

    static void ExecuteCommand(string command)
    {
        Process process = new Process();
        process.StartInfo.FileName = "/bin/bash";
        process.StartInfo.Arguments = $"-c \"{command}\"";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;

        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            UnityEngine.Debug.LogError($"Command failed: {command}\nOutput: {output}\nError: {error}");
        }
    }
}