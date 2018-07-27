using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace com.stoneus.sdkDemo {
	#if UNITY_IPHONE
	public class iOSSDKImpl : SdkAdapterImpl {

		[DllImport("__Internal")]
		private static extern void __initSDK ();

		[DllImport("__Internal")]
		private static extern void __openSDK ();


		/// <summary>
		/// Event Name, Notice.
		/// </summary>
		private static string _gameObjectName;

		/// <summary>
		/// Initializes a new instance of the <see cref="com.stoneus.sdkDemo.iOSSDKImpl"/> class.
		/// </summary>
		/// <param name="go">Go.</param>
		public iOSSDKImpl (GameObject go) {
			Debug.Log("iOSSDKImpl  ===>>>  iOSSDKImpl" + go.name);
			_gameObjectName = go.name;
		}
			
		/// <summary>
		/// Inits the SDK.
		/// </summary>
		/// <param name="sdkConfig">Sdk config.</param>
		public override void initSDK (SdkConfig sdkConfig) {
		}
			
		/// <summary>
		/// Opens the SDK.
		/// </summary>
		/// <param name="strMsg">String message.</param>
		public override void openSDK (string strMsg) {
		}
	}
	#endif
}
