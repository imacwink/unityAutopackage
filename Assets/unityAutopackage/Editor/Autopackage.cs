using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using com.stoneus.sdkDemo;

public class Autopackage : Editor {

	private static string projectName = "XXXSDK";
	private static string versionCode = "1.0.1";
	private static string androidGradlePath = "Export/Android/Gradle";
	private static string androidADTPath = "Export/Android/ADT";
	private static string androidJavaSrc = "Assets/HYLive/Copy/Android/HYLiveSDK.java";

	/// <summary>
	/// Gets all scenes.
	/// </summary>
	/// <returns>The all scenes.</returns>
	static string[] GetAllScenes() {
		List<string> names = new List<string>();
		foreach(EditorBuildSettingsScene e in EditorBuildSettings.scenes) {
			if(e==null)
				continue;
			if(e.enabled)
				names.Add(e.path);
		}
		return names.ToArray();
	}
		
	/// <summary>
	/// Gens the ADT debug AP.
	/// </summary>
	[MenuItem("AutoPackage/GenADTDebugAPK")]
	public static void GenADTDebugAPK() {

		//首先导出项目;
		GenADTProject ();

		//执行编译脚本;
		string cdProjectPath = "cd " + androidADTPath;
		string bulidLibProject = cdProjectPath + " && " + "android update project --target 3 --path ./berry_lib/" + " && exit";
		ProcessCommand(bulidLibProject);

		string bulidMainProject = cdProjectPath + " && " + "android update project --target 3 --path ./HYLiveSDK/ --subprojects" + " && exit";
		ProcessCommand(bulidMainProject);

		//开始打Debug包;
		string antDebugGenApk = cdProjectPath + "/" + projectName + " && " + "ant debug" + " && exit";
		ProcessCommand(antDebugGenApk);

		//文件路径;
		string srcApkFile = "Export/Android/ADT/HYLiveSDK/bin/UnityPlayerActivity-debug.apk";
		string dstApkFile = "Export/Android/ADT/DebugApk/HYLiveSDK-" + versionCode + "-debug.apk";
		string srcApkUnalignedFile = "Export/Android/ADT/HYLiveSDK/bin/UnityPlayerActivity-debug-unaligned.apk";
		string dstApkUnalignedFile = "Export/Android/ADT/DebugApk/HYLiveSDK-" + versionCode + "-debug-unaligned.apk";

		//删除存在的APK文件;
		STIOTools.DeleteFolder(androidADTPath + "/DebugApk");
		//HYIOTools.DeleteFolder(androidADTPath + "/ReleaseApk");

		//将APK移动到指定目录;
		STIOTools.CopyFile(srcApkFile, dstApkFile);
		STIOTools.CopyFile(srcApkUnalignedFile, dstApkUnalignedFile);
	}

	/// <summary>
	/// Gens the ADT release AP.
	/// </summary>
	[MenuItem("AutoPackage/GenADTReleaseAPK")]
	public static void GenADTReleaseAPK() {

		//首先导出项目;
		GenADTProject ();

		//执行编译脚本;
		string cdProjectPath = "cd " + androidADTPath;
		string bulidLibProject = cdProjectPath + " && " + "android update project --target 3 --path ./berry_lib/" + " && exit";
		ProcessCommand(bulidLibProject);

		string bulidMainProject = cdProjectPath + " && " + "android update project --target 3 --path ./HYLiveSDK/ --subprojects" + " && exit";
		ProcessCommand(bulidMainProject);

		//开始打Release包;
		string antDebugGenApk = cdProjectPath + "/" + projectName + " && " + "ant release" + " && exit";
		ProcessCommand(antDebugGenApk);

		//文件路径;
		string srcApkFile = "Export/Android/ADT/HYLiveSDK/bin/UnityPlayerActivity-release.apk";
		string dstApkFile = "Export/Android/ADT/ReleaseApk/HYLiveSDK-" + versionCode + "-release.apk";
		string srcApkUnalignedFile = "Export/Android/ADT/HYLiveSDK/bin/UnityPlayerActivity-release-unaligned.apk";
		string dstApkUnalignedFile = "Export/Android/ADT/ReleaseApk/HYLiveSDK-" + versionCode + "-release-unaligned.apk";

		//删除存在的APK文件;
		//HYIOTools.DeleteFolder(androidADTPath + "/DebugApk");
		STIOTools.DeleteFolder(androidADTPath + "/ReleaseApk");

		//将APK移动到指定目录;
		STIOTools.CopyFile(srcApkFile, dstApkFile);
		STIOTools.CopyFile(srcApkUnalignedFile, dstApkUnalignedFile);
	}

	/// <summary>
	/// Gens the ADT project.
	/// </summary>
	[MenuItem("AutoPackage/GenADTProject")]
	public static void GenADTProject() {

		//首先清理之前的工程;
		string cdADTPath = "cd " + androidADTPath;
		string removeProject = cdADTPath + " && " + "rd/s/q " + projectName  + " && exit";
		DirectoryInfo mydir = new DirectoryInfo(androidGradlePath + "/" + projectName); 
		if (mydir.Exists) {  
			ProcessCommand (removeProject);
		}

		//HYIOTools.DeleteFolder(androidADTPath + "/berry_lib");
		//HYIOTools.DeleteFolder(androidADTPath + "/HYLiveSDK");

		//项目设置;
		EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.ADT;
		EditorUserBuildSettings.exportAsGoogleAndroidProject = true; 

		//生成ADT Android Project;
		BuildPipeline.BuildPlayer (GetAllScenes (), androidADTPath, BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);

		/*代码文件的拷贝*/
		string dstCopyFile = "Export/Android/ADT/HYLiveSDK/src/com/huya/hylivesdk/HYLiveSDK.java";
		STIOTools.CopyFile(androidJavaSrc, dstCopyFile);

		/*项目文件修改*/
		string srcCopyProjectFile = "Assets/HYLive/Copy/Android/ADT/project.properties";
		string dstCopyProjectFile = "Export/Android/ADT/HYLiveSDK/project.properties";
		STIOTools.CopyFile(srcCopyProjectFile, dstCopyProjectFile);

		/*Ant文件*/
		string srcCopyAntFile = "Assets/HYLive/Copy/Android/ADT/ant.properties";
		string dstCopyAntFile = "Export/Android/ADT/HYLiveSDK/ant.properties";
		STIOTools.CopyFile(srcCopyAntFile, dstCopyAntFile);
	}

	/// <summary>
	/// Gens the gradle debug APK.
	/// 注释:需要手动配置BulidSetting的配置为Gradle和导出工程模式
	/// </summary>
	[MenuItem("AutoPackage/GenGradleDebugAPK")]
	public static void GenGradleDebugAPK() {

		//生成项目;
		GenGradleProject ();

		//执行脚本命令构建APK包(gradle assembleDebug);
		string cdProjectPath = "cd " + androidGradlePath + "/" + projectName;
		string gradleCMD = cdProjectPath + " && " + "gradle assembleDebug" + " && exit";
		ProcessCommand(gradleCMD);

		//文件路径;
		string srcApkFile = "Export/Android/Gradle/HYLiveSDK/build/outputs/apk/HYLiveSDK-debug.apk";
		string dstApkFile = "Export/Android/Gradle/DebugApk/HYLiveSDK-" + versionCode + "-debug.apk";
		string srcApkUnalignedFile = "Export/Android/Gradle/HYLiveSDK/build/outputs/apk/HYLiveSDK-debug-unaligned.apk";
		string dstApkUnalignedFile = "Export/Android/Gradle/DebugApk/HYLiveSDK-" + versionCode + "-debug-unaligned.apk";

		//删除存在的APK文件;
		STIOTools.DeleteFolder(androidGradlePath + "/DebugApk");
		//HYIOTools.DeleteFolder(androidADTPath + "/ReleaseApk");

		//将APK移动到指定目录;
		STIOTools.CopyFile(srcApkFile, dstApkFile);
		STIOTools.CopyFile(srcApkUnalignedFile, dstApkUnalignedFile);
	}

	/// <summary>
	/// Gens the gradle release AP.
	/// </summary>
	[MenuItem("AutoPackage/GenGradleReleaseAPK")]
	public static void GenGradleReleaseAPK() {
		
		//生成项目;
		GenGradleProject ();

		//执行脚本命令构建APK包(gradle assembleRelease);
		string cdProjectPath = "cd " + androidGradlePath + "/" + projectName;
		string gradleCMD = cdProjectPath + " && " + "gradle assembleRelease" + " && exit";
		ProcessCommand(gradleCMD);

		//文件路径;
		string srcApkFile = "Export/Android/Gradle/HYLiveSDK/build/outputs/apk/HYLiveSDK-release.apk";
		string dstApkFile = "Export/Android/Gradle/ReleaseApk/HYLiveSDK-" + versionCode + "-release.apk";
		string srcApkUnalignedFile = "Export/Android/Gradle/HYLiveSDK/build/outputs/apk/HYLiveSDK-release-unaligned.apk";
		string dstApkUnalignedFile = "Export/Android/Gradle/ReleaseApk/HYLiveSDK-" + versionCode + "-release-unaligned.apk";

		//删除存在的APK文件;
		//HYIOTools.DeleteFolder(androidGradlePath + "/DebugApk");
		STIOTools.DeleteFolder(androidADTPath + "/ReleaseApk");

		//将APK移动到指定目录;
		STIOTools.CopyFile(srcApkFile, dstApkFile);
		STIOTools.CopyFile(srcApkUnalignedFile, dstApkUnalignedFile);
	}

	/// <summary>
	/// Gens the gradle debug Project.
	/// 注释:需要手动配置BulidSetting的配置为Gradle和导出工程模式
	/// </summary>
	[MenuItem("AutoPackage/GenGradleProject")]
	public static void GenGradleProject() {

		//删除先前生成的工程目录;
		//HYIOTools.DeleteFolder(androidGradlePath + "/HYLiveSDK");
		string cdGradlePath = "cd " + androidGradlePath;
		string removeProject = cdGradlePath + " && " + "rd/s/q " + projectName  + " && exit";
		DirectoryInfo mydir = new DirectoryInfo(androidGradlePath + "/" + projectName); 
		if (mydir.Exists) {  
			ProcessCommand (removeProject);
		}

		//项目设置;
		EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
		EditorUserBuildSettings.exportAsGoogleAndroidProject = true; 

		//生成Gradle Android Project;
		BuildPipeline.BuildPlayer (GetAllScenes (), androidGradlePath, BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);

		/*代码文件的拷贝*/
		string dstCopyFile = "Export/Android/Gradle/HYLiveSDK/src/main/java/com/huya/hylivesdk/HYLiveSDK.java";
		STIOTools.CopyFile(androidJavaSrc, dstCopyFile);

		/*Bulid 文件*/
		string srcCopyBulidFile = "Assets/HYLive/Copy/Android/Gradle/build.gradle";
		string dstCopyBulidFile = "Export/Android/Gradle/HYLiveSDK/build.gradle";
		STIOTools.CopyFile(srcCopyBulidFile, dstCopyBulidFile);
	}

	/// <summary>
	/// Builds for iPhone.
	/// </summary>
	[MenuItem("AutoPackage/GenXcodeProject")]
	public static void GenXcodeProject() { 
		//宏定义标签 HY_LIVE_SDK 主要目的针对于多渠道区分代码;
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, "HY_LIVE_SDK");

		//构建xcode工程方法 
		//参数1 需要打包的所有场景
		//参数2 需要打包的名子， 这里可以是shell传进来的字符串，也可以是自己定义的字符串
		//参数3 打包平台
		BuildPipeline.BuildPlayer(GetAllScenes(), projectName, BuildTarget.iOS, BuildOptions.None);
	}

	/// <summary>
	/// Starts the cmd.
	/// </summary>
	/// <param name="Command">Command.</param>
	private static void ProcessCommand(string command) {
		Process p = new Process();
		p.StartInfo.FileName = "cmd.exe";
		p.StartInfo.UseShellExecute = true;
		p.StartInfo.RedirectStandardInput = false;
		p.StartInfo.RedirectStandardOutput = false;
		p.StartInfo.Arguments = "/k "+ command;
		p.Start();
		p.WaitForExit();
		p.Close();
	}
}