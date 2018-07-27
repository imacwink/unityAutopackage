using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.stoneus.sdkDemo {
	#if UNITY_ANDROID
	public class AndroidSDKImpl : SdkAdapterImpl {
		/// <summary>
		/// Android 实体对象.
		/// </summary>
		private AndroidJavaObject javaObj;

		/// <summary>
		/// 调用android java端接口类
		/// </summary>
		private const string SDK_JAVA_CLASS = "com.stoneus.sdkDemo.SdkAdapter";

		/// <summary>
		/// AndroidHYLiveImpl Constructed Function.
		/// </summary>
		public AndroidSDKImpl (GameObject go) {
			Debug.Log("AndroidSDKImpl  ===>>>  AndroidSDKImpl " + go.name);
			try{
				javaObj = new AndroidJavaObject(SDK_JAVA_CLASS);
			} catch(Exception e) {
				Console.WriteLine("{0} Exception caught.", e);
			}
		}
			
		/// <summary>
		/// Inits the SDK.
		/// </summary>
		/// <param name="sdkConfig">Sdk config.</param>
		public override void initSDK (SdkConfig sdkConfig) {
			Debug.Log("AndroidSDKImpl  ===>>>  initSDK === appID:" + sdkConfig.appID + "|appSecretKey:" + sdkConfig.appSecretKey);
			if (javaObj != null) {
				javaObj.CallStatic ("initSDK", sdkConfig.appID, sdkConfig.appSecretKey);
			} else {
				Debug.Log(SDK_JAVA_CLASS + "  ===>>>  javaObj is null!");
			}
		}
			
		/// <summary>
		/// Opens the SDK.
		/// </summary>
		/// <param name="strMsg">String message.</param>
		public override void openSDK (string strMsg) {
			Debug.Log("AndroidSDKImpl  ===>>>  openSDK === strMsg:" + strMsg);
			if(javaObj != null) {
				javaObj.CallStatic ("openSDK", strMsg);
			} else {
				Debug.Log(SDK_JAVA_CLASS + "  ===>>>  javaObj is null!");
			}
		}
	}
	#endif
}
