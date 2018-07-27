using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.stoneus.sdkDemo {
	public class SdkAdapter : MonoBehaviour {

		//初始化回调函数;
		public delegate void OnInitCallback (string msgInfo);
		public OnInitCallback onInitCallback;

		//SDK实例;
		public SdkAdapterImpl mSdkAdapterImpl;

		/// <summary>
		/// Awake this instance.
		/// </summary>
		void Awake() {
			#if UNITY_IPHONE
			mSdkAdapterImpl = new iOSSDKImpl (gameObject);
			#elif UNITY_ANDROID
			mSdkAdapterImpl = new AndroidSDKImpl (gameObject);
			#endif
		}

		/// <summary>
		/// Ons the init callback.
		/// </summary>
		/// <param name="msgInfo">Message info.</param>
		private void __OnInitCallback (string msgInfo) {
			//TODO::
		}
			
		/// <summary>
		/// Inits the SDK.
		/// </summary>
		/// <param name="config">Config.</param>
		public void InitSDK(SdkConfig config) {
			if (mSdkAdapterImpl != null) {
				mSdkAdapterImpl.initSDK (config);	
			} else {
				Debug.LogError ("mSdkAdapterImpl is null !!!!");
			}
		}

		/// <summary>
		/// Opens the SDK.
		/// </summary>
		/// <param name="strMsg">String message.</param>
		public void OpenSDK(string strMsg) {
			if (mSdkAdapterImpl != null) {
				mSdkAdapterImpl.openSDK (strMsg);
			} else {
				Debug.LogError ("mSdkAdapterImpl is null !!!!");
			}
		}
	}
}
