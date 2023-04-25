using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

//gz : Original From https://lhamed.github.io/66th-post-copy-2/

public enum AppIdentifier
{
    none,
    Identifier1,
    Identifier2,
    Identifier3
}

public enum TargetServer
{
    TEST,
    QA,
    KR,
    HK
}

public enum TargetMarket
{
    GOOGLE,
    ZARINPAL,
    NONE
}
public enum DefaultLanguage
{
    Sys
}
public class AutoBuilder : ScriptableObject
{
    static readonly string STR_TARGET_DIR = "C:/project/Build/";
    static readonly string[] SCENES = FindEnabledEditorScenes();
    static readonly Dictionary<AppIdentifier, string> APPIDENTIFIERDICT = new Dictionary<AppIdentifier, string>
    {
        { AppIdentifier.Identifier1, "com.companyName1.projectName1" },
        { AppIdentifier.Identifier2, "com.companyName2.projectName2" },
        { AppIdentifier.Identifier3, "com.companyName2.projectName3" }
    };

    static AppIdentifier eAppIdentiFier;
    static TargetServer eTargetServer;
    static TargetMarket eTargetMarket;
    static DefaultLanguage eDefaultLanguage = DefaultLanguage.Sys; // add by hugh - 20221209 config 파일 생성시 사용할 기본 언어 설정변수
    
    static string StrAppIdentifier => APPIDENTIFIERDICT[eAppIdentiFier];
    static string ProjectPath {  get => Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')); }


    [MenuItem("Custom Tools/Build For Android/TEST")]
    public static void BuildTest()
    {
        SetData(AppIdentifier.Identifier1, TargetServer.TEST, TargetMarket.GOOGLE);
        PerformBuildAOS();
    }
    [MenuItem("Custom Tools/Build For Android/QA")]
    public static void BuildQA()
    {
        SetData(AppIdentifier.Identifier1, TargetServer.QA, TargetMarket.GOOGLE);
        PerformBuildAOS();
    }
    [MenuItem("Custom Tools/Build For Android/KR")]
    public static void BuildKR()
    {
        SetData(AppIdentifier.Identifier1, TargetServer.KR, TargetMarket.GOOGLE);
        PerformBuildAOS();
    }
    [MenuItem("Custom Tools/Build For Android/HK")]
    public static void BuildHK()
    {
        SetData(AppIdentifier.Identifier1, TargetServer.HK, TargetMarket.GOOGLE);
        PerformBuildAOS();
    }

    [MenuItem("Custom Tools/Build For Android/HK - Mena")]
    public static void BuildIdentifier3()
    {
        SetData(AppIdentifier.Identifier2, TargetServer.HK, TargetMarket.GOOGLE);
        eDefaultLanguage = default;
        PerformBuildAOS();
    }

    /*[MenuItem("Custom Tools/Build For Android/HK - Zarinpal")]
    public static void BuildIdentifier3Zarinpal()
    {
        SetData(AppIdentifier.Identifier3, TargetServer.HK, TargetMarket.ZARINPAL);
        eDefaultLanguage = DefaultLanguage.Persian;
        PerformBuildAOS();
    }    */

    public static void PerformBuildAOS()
    {
        InitConfigData();
        // PlayerSettings.Android.bundleVersionCode = 동적으로 버전 코드 설정
        // PlayerSettings.bundleVersion = 동적으로 번들 버전 설정 
        if(!Directory.Exists(STR_TARGET_DIR))
            Directory.CreateDirectory(STR_TARGET_DIR);

        string strAppName = $"{eAppIdentiFier}_{DateTime.Now.ToString("yyyy-MM-dd")}_{eTargetServer}_{eTargetMarket}_Release.apk";
        int index = 0;
        while (File.Exists($"{STR_TARGET_DIR}/{strAppName}"))
        {
            strAppName = $"{eAppIdentiFier}_{DateTime.Now.ToString("yyyy-MM-dd")}_{eTargetServer}_{eTargetMarket}_{index++}.apk";
        }
        //APP_FILE_NAME = strAppName;
        PlayerSettings.Android.keyaliasName = "keyAliasName";
        PlayerSettings.Android.keystoreName = ProjectPath + "keyStore path";
        PlayerSettings.Android.keyaliasPass = "keyAlias password";
        PlayerSettings.Android.keystorePass = "keyStore password";
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle; 
        EditorUserBuildSettings.androidBuildType = AndroidBuildType.Debug;

        if (StrAppIdentifier != PlayerSettings.applicationIdentifier)
        {
            Debug.Log("Autobuilder.PerformBuildAOS() : App Identifier is changed!");
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, StrAppIdentifier);
            SetGoogleServiceJson();
            SwapTargetAPI();
        }

        BuildAndroid(SCENES, STR_TARGET_DIR + "/" + strAppName, BuildTargetGroup.Android, BuildTarget.Android, BuildOptions.CompressWithLz4HC);
    }

    private static void BuildAndroid(string[] scenes, string app_target, BuildTargetGroup build_target_group, BuildTarget build_target, BuildOptions build_options)
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = app_target;
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = build_options;
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    public static void InitConfigData()
    {
        /*ConfigData configData = new ConfigData();
        configData.version = "1.0";
        configData.account = "";
        configData.serverIp = "";
        configData.appIdentifier = eAppIdentiFier;
        configData.purchaseSDK = eTargetMarket;
        configData.targetServer = eTargetServer;
        configData.language = eDefaultLanguage;

        string toJson = JsonUtility.ToJson(configData);
        File.WriteAllText("Assets/Resources/config/config.json", toJson);*/
    }

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }

    [MenuItem("Custom Tools/Build iOS/Not Work Yet!")]
    public static void PerformBuildIOS()
    {
        //BuildOptions opt = BuildOptions.Il2CPP; // 기본이 cpp
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK; // 시뮬레이터에서 돌리시려면 시뮬레이터 sdk 로 
                                                                 //PlayerSettings.bundleVersion = GetArg ("-BUNDLE_VERSION"); //todo
                                                                 //PlayerSettings.iOS.buildNumber = (GetArg ("-VERSION_CODE")); //todo
        char sep = Path.DirectorySeparatorChar;
        string BUILD_TARGET_PATH = ProjectPath + "/ios"; //ios 폴더로 뱉습니다. 
        Directory.CreateDirectory(BUILD_TARGET_PATH);
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
        try
        {
            BuildIOS(SCENES, BUILD_TARGET_PATH, BuildTarget.iOS, BuildOptions.None);

        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    static void BuildIOS(string[] scenes, string target_path,BuildTarget build_target, BuildOptions build_options)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, build_target);
        string res = BuildPipeline.BuildPlayer(scenes, target_path, build_target, build_options).ToString();
        if (res.Length > 0) { throw new Exception("BuildPlayer failure: " + res); }
    }
    static void SetData(AppIdentifier appIdentifier, TargetServer targetServer, TargetMarket targetMarket)
    {
        eAppIdentiFier = appIdentifier;
        eTargetServer = targetServer;
        eTargetMarket = targetMarket;
    }
    static void SwapTargetAPI()
    {
        Debug.LogWarning("Autobuilder.SwapTargetAPI() : Swap API Level / because of some bug from Editor and plugins");
        if (PlayerSettings.Android.targetSdkVersion == (AndroidSdkVersions)31)
            PlayerSettings.Android.targetSdkVersion = (AndroidSdkVersions)32;
        else
            PlayerSettings.Android.targetSdkVersion = (AndroidSdkVersions)31;
    }
    static void SetGoogleServiceJson()
    {
        Debug.Log("Autobuilder.SetGoogleServiceJson() : Copy GoogleServiceJson");
        string destFile = ProjectPath + "/Assets/Firebase/google-services.json";
        switch (eAppIdentiFier)
        {
            case AppIdentifier.Identifier1:
                System.IO.File.Copy(ProjectPath + "/Assets/Firebase/Editor/google-services.Identifier1.json", destFile, true);
                break;
            case AppIdentifier.Identifier2:
                System.IO.File.Copy(ProjectPath + "/Assets/Firebase/Editor/google-services.Identifier2.json", destFile, true);
                break;
            case AppIdentifier.Identifier3:
                System.IO.File.Copy(ProjectPath + "/Assets/Firebase/Editor/google-services.Identifier3,json", destFile, true);
                break;
        }
    }
}