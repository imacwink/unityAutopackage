using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using com.stoneus.sdkDemo;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class STPostProcessBuild {

	//该属性是在build完成后，被调用的callback;
	#if UNITY_IPHONE
	[PostProcessBuildAttribute(0)]
	#elif UNITY_ANDROID
	[PostProcessBuild]
	#endif
	public static void OnPostprocessBuild(BuildTarget target, string targetPath) {
		if (target == BuildTarget.iOS) {
			#if UNITY_IPHONE
			// 初始化;
			PBXProject fbxProject = new PBXProject ();
			var projectPath = targetPath + "/Unity-iPhone.xcodeproj/project.pbxproj";
			fbxProject.ReadFromFile (projectPath);
			string targetGuid = fbxProject.TargetGuidByName ("Unity-iPhone");

			// 添加flag;
			fbxProject.AddBuildProperty (targetGuid, "OTHER_LDFLAGS", "-ObjC");

			// 关闭Bitcode;
			fbxProject.SetBuildProperty (targetGuid, "ENABLE_BITCODE", "NO");

			// 添加framwrok
			fbxProject.AddFrameworkToProject (targetGuid, "WebKit.framework", false);

			//添加lib
			AddLibToProject (fbxProject, targetGuid, "libresolv.9.tbd");
			AddLibToProject (fbxProject, targetGuid, "libz.tbd");
			AddLibToProject (fbxProject, targetGuid, "libc++abi.tbd");
			AddLibToProject (fbxProject, targetGuid, "libc++.1.bd");
			AddLibToProject (fbxProject, targetGuid, "libstdc++.6.0.9.tbd");

			//XcodeDirectoryProcessor xdp = new XcodeDirectoryProcessor();
			//xdp.CopyAndAddBuildToXcode(proj, target, "XcodeFiles/TXWYSDK/", path, "SDKFiles");

			//AddBundle ("/Plugins/iOS/bundle", targetPath, targetGuid, "", fbxProject);

			// 应用修改;
			File.WriteAllText (projectPath, fbxProject.WriteToString ());


			// 修改Info.plist文件;
			/*var plistPath = Path.Combine(targetPath, "Info.plist");
			var plist = new PlistDocument();
			plist.ReadFromFile(plistPath);

			// 插入URL Scheme到Info.plsit（理清结构）;
			var array = plist.root.CreateArray("CFBundleURLTypes");

			// 插入dict;
			var urlDict = array.AddDict();
			urlDict.SetString("CFBundleTypeRole", "Editor");

			// 插入array;
			var urlInnerArray = urlDict.CreateArray("CFBundleURLSchemes");
			urlInnerArray.AddString("blablabla");

			// 应用修改;
			plist.WriteToFile(plistPath);*/

			//插入代码
			//读取UnityAppController.mm文件;
			string unityAppControllerPath = targetPath + "/Classes/UnityAppController.mm";
			HYClass UnityAppController = new HYClass (unityAppControllerPath);

			//在指定代码后面增加一行代码;
			UnityAppController.WriteBelow("#include \"PluginBase/AppDelegateListener.h\"", "#import <UMSocialCore/UMSocialCore.h>");

			string newCode = "\n" +
			"    [[HYLiveSDKManager defaultManager] openLog:YES];\n" +
			"    [HYLiveSDKManager shareInstance].type = @\"u3d\";\n" +
			"    \n"
			;
			//在指定代码后面增加一大行代码;
			UnityAppController.WriteBelow("// if you wont use keyboard you may comment it out at save some memory", newCode);
			#endif
		}
	}

	/// <summary>
	/// Copies the file.
	/// </summary>
	/// <param name="srcPath">Source path.</param>
	/// <param name="tarPath">Tar path.</param>
	private static void CopyFile( string srcPath, string tarPath ) {
		string[] filesList = Directory.GetFiles (srcPath);
		foreach (string f in filesList) {
			string fTarPath = tarPath + "\\" + f.Substring (srcPath.Length);
			if (File.Exists (fTarPath)) {
				File.Copy (f, fTarPath, true);
			} else {
				File.Copy (f, fTarPath);
			}
			File.Delete (f);
		}
	}

	/// <summary>
	/// Copies the and replace directory.
	/// </summary>
	/// <param name="srcPath">Source path.</param>
	/// <param name="dstPath">Dst path.</param>
	private static void CopyAndReplaceDirectory(string srcPath, string dstPath)
	{
		if (Directory.Exists(dstPath))
			Directory.Delete(dstPath);
		if (File.Exists(dstPath))
			File.Delete(dstPath);

		Directory.CreateDirectory(dstPath);

		foreach (var file in Directory.GetFiles(srcPath))
			File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)));

		foreach (var dir in Directory.GetDirectories(srcPath))
			CopyAndReplaceDirectory(dir, Path.Combine(dstPath, Path.GetFileName(dir)));
	}
		
	#if UNITY_IPHONE
	/// <summary>
	/// Adds the lib to project.
	/// </summary>
	/// <param name="fbxProject">Fbx project.</param>
	/// <param name="targetGuid">Target GUID.</param>
	/// <param name="libName">Lib name.</param>
	private static void AddLibToProject(PBXProject fbxProject, string targetGuid, string libName) {
		string fileGuid = fbxProject.AddFile("usr/lib/" + libName, "Frameworks/" + libName, PBXSourceTree.Sdk);
		fbxProject.AddFileToBuild(targetGuid, fileGuid);
	}

	/// <summary>
	/// Adds the bundle.
	/// </summary>
	/// <param name="secondFilePath">Second file path.</param>
	/// <param name="xcodeTargetPath">Xcode target path.</param>
	/// <param name="xcodeTargetGuid">Xcode target GUID.</param>
	/// <param name="rootPath">Root path.</param>
	/// <param name="xcodeProj">Xcode proj.</param>
	private static void AddBundle(string secondFilePath, string xcodeTargetPath, string xcodeTargetGuid, string rootPath ,PBXProject xcodeProj) {
		SearchOption searchOption = SearchOption.AllDirectories;
		Debug.Log("secondFilePath : " + secondFilePath);
		string[] secondDirectories = Directory.GetDirectories(secondFilePath, "*.bundle", searchOption);
		foreach (string lastFilePath in secondDirectories) {
			Debug.Log("lastFilePath" + lastFilePath);
			string bundlePath = lastFilePath.Replace(rootPath, "");
			Debug.Log("bundlePath" + bundlePath);
			string savePath = xcodeTargetPath + bundlePath;
			//将 framework copy到指定目录
			DirectoryInfo bundleInfo = new DirectoryInfo(lastFilePath);
			DirectoryInfo saveBundleInfo = new DirectoryInfo(savePath);
			CopyAll (bundleInfo,saveBundleInfo);
			//将 framework 加入 proj中
			xcodeProj.AddFileToBuild(xcodeTargetGuid, xcodeProj.AddFile(savePath, "MOB"+bundlePath, PBXSourceTree.Sdk));
		}
	}

	/// <summary>
	/// 拷贝目录下的所有文件并剔除.meta.
	/// </summary>
	/// <param name="source">Source.</param>
	/// <param name="target">Target.</param>
	private static void CopyAll(DirectoryInfo source, DirectoryInfo target) {
		if (source.FullName.ToLower() == target.FullName.ToLower()) {
			return;
		}

		// Check if the target directory exists, if not, creatAddURLSchemesAddURLSchemese it.
		if (!Directory.Exists(target.FullName)) {
			Directory.CreateDirectory(target.FullName);
		}

		// Copy each file into it's new directory.
		foreach (FileInfo fi in source.GetFiles()) {

		if(!fi.Name.EndsWith(".meta")) {
			string name = fi.Name;
			if(name.Contains(".mobjs")) {
				name = name.Replace (".mobjs",".js");
			}
				fi.CopyTo(Path.Combine(target.ToString(), name), true);
			}
		}

		// Copy each subdirectory using recursion.
		foreach (DirectoryInfo diSourceSubDir in source.GetDirectories()) {
			DirectoryInfo nextTargetSubDir =
			target.CreateSubdirectory(diSourceSubDir.Name);
			CopyAll(diSourceSubDir, nextTargetSubDir);
		}
	}
	#endif
}