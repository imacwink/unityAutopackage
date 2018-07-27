using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Examples : MonoBehaviour {

	public GameObject mCubeGameObject = null; //测试直播游戏体;
	void Start () {
	}

	void OnDestroy(){
	}

	void Update () {
		if(mCubeGameObject != null)
			mCubeGameObject.transform.Rotate(Vector3.up * Time.deltaTime * 200);
	}

	public void onOpenHYLiveEvent () {
	}
}
