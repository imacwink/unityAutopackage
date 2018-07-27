using System;
using System.Collections;

namespace com.stoneus.sdkDemo {
	public abstract class SdkAdapterImpl {

		/// <summary>
		/// Inits the SDK.
		/// </summary>
		/// <param name="sdkConfig">Sdk config.</param>
		public abstract void initSDK (SdkConfig sdkConfig);

		/// <summary>
		/// Opens the SDK.
		/// </summary>
		/// <param name="strMsg">String message.</param>
		public abstract void openSDK (string strMsg);

	}
}