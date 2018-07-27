using UnityEditor;
using System;
using System.IO;
using System.Threading;
using UnityEngine;

namespace com.stoneus.sdkDemo {
	public class STIOTools  {
		/// <summary>
		/// 检测文件是否存在Application.dataPath目录下.
		/// </summary>
		/// <returns><c>true</c> if is file exists the specified fileName; otherwise, <c>false</c>.</returns>
		/// <param name="fileName">File name.</param>
		public static bool IsFileExists (string fileName) {
			if (fileName.Equals (string.Empty)) {
				return false;
			}

			return File.Exists (GetFullPath (fileName));
		}

		/// <summary>
		/// 在Application.dataPath目录下创建文件.
		/// </summary>
		/// <param name="fileName">File name.</param>
		public static void CreateFile (string fileName) {
			if (!IsFileExists (fileName)) {
				CreateFolder (fileName.Substring (0, fileName.LastIndexOf ('/')));

				#if UNITY_4 || UNITY_5
				FileStream stream = File.Create (GetFullPath (fileName));
				stream.Close ();
				#else
				File.Create (GetFullPath (fileName));
				#endif
			}

		}

		/// <summary>
		/// 写入数据到对应文件.
		/// </summary>
		/// <param name="fileName">File name.</param>
		/// <param name="contents">Contents.</param>
		public static void Write (string fileName, string contents) {
			CreateFolder (fileName.Substring (0, fileName.LastIndexOf ('/')));

			TextWriter tw = new StreamWriter (GetFullPath (fileName), false);
			tw.Write (contents);
			tw.Close (); 

			AssetDatabase.Refresh ();
		}

		/// <summary>
		///  从对应文件读取数据.
		/// </summary>
		/// <param name="fileName">File name.</param>
		public static string Read (string fileName) {
			#if !UNITY_WEBPLAYER
			if (IsFileExists (fileName)) {
				return File.ReadAllText (GetFullPath (fileName));
			} else {
				return "";
			}
			#endif

			#if UNITY_WEBPLAYER
			Debug.LogWarning("FileStaticAPI::CopyFolder is innored under wep player platfrom");
			#endif
		}

		/// <summary>
		/// 复制文件.
		/// </summary>
		/// <param name="srcFileName">Source file name.</param>
		/// <param name="destFileName">Destination file name.</param>
		public static void CopyFile (string srcFileName, string destFileName) {
			if (IsFileExists (srcFileName) && !srcFileName.Equals (destFileName)) {
				int index = destFileName.LastIndexOf ("/");
				string filePath = string.Empty;

				if (index != -1) {
					filePath = destFileName.Substring (0, index);
				}

				if (!Directory.Exists (GetFullPath (filePath))) {
					Directory.CreateDirectory (GetFullPath (filePath));
				}

				File.Copy (GetFullPath (srcFileName), GetFullPath (destFileName), true);

				AssetDatabase.Refresh ();
			}
		}

		/// <summary>
		/// 删除文件.
		/// </summary>
		/// <param name="fileName">File name.</param>
		public static void DeleteFile (string fileName) {
			if (IsFileExists (fileName)) {
				File.Delete (GetFullPath (fileName));

				AssetDatabase.Refresh ();
			}
		}

		/// <summary>
		/// 检测是否存在文件夹.
		/// </summary>
		/// <returns><c>true</c> if is folder exists the specified folderPath; otherwise, <c>false</c>.</returns>
		/// <param name="folderPath">Folder path.</param>
		public static bool IsFolderExists (string folderPath) {
			if (folderPath.Equals (string.Empty)) {
				return false;
			}

			return Directory.Exists (GetFullPath (folderPath));
		}

		/// <summary>
		/// 创建文件夹.
		/// </summary>
		/// <param name="folderPath">Folder path.</param>
		public static void CreateFolder (string folderPath) {
			if (!IsFolderExists (folderPath)) {
				Directory.CreateDirectory (GetFullPath (folderPath));

				AssetDatabase.Refresh ();
			}
		}

		/// <summary>
		/// 复制文件夹.
		/// </summary>
		/// <param name="srcFolderPath">Source folder path.</param>
		/// <param name="destFolderPath">Destination folder path.</param>
		public static void CopyFolder (string srcFolderPath, string destFolderPath) {
			#if !UNITY_WEBPLAYER
			if (!IsFolderExists (srcFolderPath)) {
				return;
			}

			CreateFolder (destFolderPath);

			srcFolderPath = GetFullPath (srcFolderPath);
			destFolderPath = GetFullPath (destFolderPath);

			// 创建所有的对应目录
			foreach (string dirPath in Directory.GetDirectories(srcFolderPath, "*", SearchOption.AllDirectories)) {
				Directory.CreateDirectory (dirPath.Replace (srcFolderPath, destFolderPath));
			}

			// 复制原文件夹下所有内容到目标文件夹，直接覆盖
			foreach (string newPath in Directory.GetFiles(srcFolderPath, "*.*", SearchOption.AllDirectories)) {

				File.Copy (newPath, newPath.Replace (srcFolderPath, destFolderPath), true);
			}

			AssetDatabase.Refresh ();
			#endif

			#if UNITY_WEBPLAYER
			Debug.LogWarning("FileStaticAPI::CopyFolder is innored under wep player platfrom");
			#endif
		}

		/// <summary>
		/// 删除文件夹.
		/// </summary>
		/// <param name="folderPath">Folder path.</param>
		public static void DeleteFolder (string folderPath) {
			#if !UNITY_WEBPLAYER
			if (IsFolderExists (folderPath)) {

				Directory.Delete (GetFullPath (folderPath), true);

				AssetDatabase.Refresh ();
			}
			#endif

			#if UNITY_WEBPLAYER
			Debug.LogWarning("FileStaticAPI::DeleteFolder is innored under wep player platfrom");
			#endif
		}

		/// <summary>
		/// 返回Application.dataPath下完整目录.
		/// </summary>
		/// <returns>The full path.</returns>
		/// <param name="srcName">Source name.</param>
		public static string GetFullPath (string srcName, bool assetsPath = false) {

			string rootPath = Application.dataPath;
			if(!assetsPath) {
				rootPath = System.Environment.CurrentDirectory;
			}

			if (srcName.Equals (string.Empty)) {
				return rootPath;
			}

			if (srcName [0].Equals ('/')) {
				srcName.Remove (0, 1);
			}
				
			return rootPath + "/" + srcName;
		}
	}

}