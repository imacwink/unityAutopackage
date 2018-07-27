using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.stoneus.sdkDemo {

	/// <summary>
	/// SDK config.
	/// </summary>
	public class SdkConfig {
		/// <summary>
		/// 应用标示ID
		/// </summary>
		private string mAppID;
		public string appID {
			set { 
				mAppID = value;
			}
			get { 
				return mAppID;
			}
		}

		/// <summary>
		/// 应用秘钥
		/// </summary>
		private string mAppSecretKey;
		public string appSecretKey {
			set { 
				mAppSecretKey = value;
			}
			get { 
				return mAppSecretKey;
			}
		}
	}
}
