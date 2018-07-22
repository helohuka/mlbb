using UnityEngine;
using System.Collections;

public class MemoryManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#if UNITY_IOS
	public void SDK_MomoryClean()
	{
		PlayerAsseMgr.ClearAll();
		EffectAssetMgr.ClearAll();
		Resources.UnloadUnusedAssets ();
		//Debug.Log ("SDK_MomoryClean");
	}
	#endif
}
