using System;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.Diagnostics;
using System.IO;
using UnityEditor.Build.Reporting;

public class AutoBuilder
{
    static string[] SCENES = FindEnabledEditorScenes();
    static string APP_NAME = "YourAppName";
    static string TARGET_DIR = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Builds/Android/";


    #region AOS
    [MenuItem("Build/Build APK")]
    static void BuildAPK()
    {
        APP_NAME = PlayerSettings.productName + ".apk";

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = SCENES;
        buildPlayerOptions.locationPathName = TARGET_DIR + "/" + APP_NAME;
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        EditorUserBuildSettings.androidBuildType = AndroidBuildType.Release;
        PlayerSettings.SplashScreen.show = false;

        BuildAndroid(SCENES, TARGET_DIR + "/" + APP_NAME, BuildTargetGroup.Android, BuildTarget.Android, BuildOptions.CompressWithLz4HC);
    }
    static void BuildAndroid(string[] scenes, string app_target, BuildTargetGroup build_target_group, BuildTarget build_target, BuildOptions build_options)
    {
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;

        EditorUserBuildSettings.buildAppBundle = false;
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = app_target;
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = build_options;
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    [MenuItem("Build/Build and Run APK")]
    static void BuildAndRunAPK()
    {
        IsAndroidPlatform();
        BuildAPK();
        RunOnAndroidDevice();
    }



    [MenuItem("Build/Build aab")]
    static void BuildAAB()
    {
        APP_NAME = PlayerSettings.productName + ".aab";

        // Set the keystore information
        // PlayerSettings.Android.keystoreName = "YourKeystoreName.keystore";
        // PlayerSettings.Android.keystorePass = "YourKeystorePassword";
        // PlayerSettings.Android.keyaliasName = "YourKeyAlias";
        // PlayerSettings.Android.keyaliasPass = "YourKeyAliasPassword";

        // Set the build options
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = SCENES;
        buildPlayerOptions.locationPathName = TARGET_DIR + "/" + APP_NAME;
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;

        Directory.CreateDirectory(TARGET_DIR);
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        EditorUserBuildSettings.androidBuildType = AndroidBuildType.Release;
        PlayerSettings.SplashScreen.show = false;

        BuildAndroidAAB(SCENES, TARGET_DIR + "/" + APP_NAME, BuildTargetGroup.Android, BuildTarget.Android, BuildOptions.CompressWithLz4HC /*| BuildOptions.*/);
    }
    static void BuildAndroidAAB(string[] scenes, string app_target, BuildTargetGroup build_target_group, BuildTarget build_target, BuildOptions build_options)
    {
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;

        EditorUserBuildSettings.buildAppBundle = true;
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = app_target;
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = build_options;
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    [MenuItem("Build/Build and Run AAB")]
    static void BuildAndRunAAB()
    {
        IsAndroidPlatform();
        BuildAAB();
        RunOnAndroidDevice();
    }


    static void RunOnAndroidDevice()
    {
        string androidSDKPath = EditorPrefs.GetString("AndroidSdkRoot");
        string packageName = $"com.{PlayerSettings.companyName}." + APP_NAME.ToLower();

        string adbInstallCommand = $"\"{androidSDKPath}/platform-tools/adb\" install -r {TARGET_DIR}/{APP_NAME}.apk";
        string adbRunCommand = $"\"{androidSDKPath}/platform-tools/adb\" shell am start -n {packageName}/{packageName}.MainActivity";


        ExecuteCommand(adbInstallCommand);
        ExecuteCommand(adbRunCommand);
    }

    static void IsAndroidPlatform()
    {
        bool isAndroid = EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android ? true : false;

        if (!isAndroid)
        {
            UnityEngine.Debug.LogError("Build target is not supported for APK. Please choose an Android platform.");
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        }
    }
    #endregion


    #region iOS
    public static string APP_FOLDER = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    public static string IOS_FOLDER = string.Format("{0}/Builds/iOS/", APP_FOLDER);

    [MenuItem("Build/Build iOS")]
    static void BuildiOS()
    {
        APP_NAME = PlayerSettings.productName + ".apk";

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = SCENES;
        buildPlayerOptions.target = BuildTarget.iOS;
        buildPlayerOptions.options = BuildOptions.None;

        // PlayerSettings.iOS.appleDeveloperTeamID = "";
        PlayerSettings.iOS.appleEnableAutomaticSigning = true;
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
        PlayerSettings.SplashScreen.show = false;

        BuildIOS(SCENES, IOS_FOLDER, BuildTarget.iOS, BuildOptions.None);
    }

    [MenuItem("Build/Build and Run iOS")]
    static void BuildAndRuniOS()
    {
        IsiOSPlatform();
        BuildiOS();
        RunOniOSDevice();
    }

    static void BuildIOS(string[] scenes, string target_path, BuildTarget build_target, BuildOptions build_options)
    {
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, build_target);
        BuildReport res = BuildPipeline.BuildPlayer(scenes, target_path, build_target, build_options);
        if (res.name.Length > 0) { throw new Exception("BuildPlayer failure: " + res); }
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

    static void IsiOSPlatform()
    {
        bool isiOS = EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS ? true : false;

        if (!isiOS)
        {
            UnityEngine.Debug.LogError("Build target is not supported for iOS. Please choose an iOS platform.");
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
        }
    }
    #endregion


    static string[] FindEnabledEditorScenes()
    {
        var editorScenes = EditorBuildSettings.scenes;
        var sceneNames = new string[editorScenes.Length];

        for (int i = 0; i < editorScenes.Length; i++)
            sceneNames[i] = editorScenes[i].path;

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