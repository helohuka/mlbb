using UnityEngine;
using System.Collections;
using UnityHTTP;
using System.IO;

public class TransferRate : MonoBehaviour {

	static public TransferRate _Inst;
	// Use this for initialization
	void Start () {
		_Inst = this;
		DontDestroyOnLoad(this);
	}
	
	public void Send(string progress)
	{
		StartCoroutine(SendTransferRate(progress));
	}

	public void SendLog(string msg)
	{
		StartCoroutine(SendTransferLog(msg));
	}

	IEnumerator SendTransferLog(string msg)
	{
        //string deviceId = "Editor";
        //#if UNITY_ANDROID && !UNITY_EDITOR
        //deviceId = XyskAndroidAPI.getMacAndroid();
        //#elif UNITY_IOS && !UNITY_EDITOR
        //deviceId = XyskIOSAPI.GetIDFA();
        //#endif
        //WWWForm form = new WWWForm();
        //form.AddField("Derror", msg);
        //Request www = new Request("post", "http://106.75.76.143:19208/statistics", form);
        //www.Send ();
		
        //while( !www.isDone )
        //{
        //    yield return null;
        //}
		
        //if (www.isDone)
        //{
        //    if (www.exception == null && www.response != null)
        //    {
				
        //    }
        //    else
        //    {
				
        //    }
        //}
		yield return null;
	}

	IEnumerator SendTransferRate(string msg)
	{
        //string deviceId = "Editor";
        //#if UNITY_ANDROID && !UNITY_EDITOR
        //deviceId = XyskAndroidAPI.getMacAndroid();
        //#elif UNITY_IOS && !UNITY_EDITOR
        //deviceId = XyskIOSAPI.GetIDFA();
        //#endif
        //WWWForm form = new WWWForm();
        //form.AddField("id", deviceId);
        //form.AddField("MemorySize", SystemInfo.systemMemorySize.ToString());
        //form.AddField("CPUCount", SystemInfo.processorCount.ToString());
        //form.AddField("CPUType", SystemInfo.processorType);
        //form.AddField("OperateSystem", SystemInfo.operatingSystem);

        //form.AddField("content", msg);
        //Request www = new Request("post", "http://106.75.76.143:19208/statistics", form);
        //www.Send ();
		
        //while( !www.isDone )
        //{
        //    yield return null;
        //}
		
        //if (www.isDone)
        //{
        //    if (www.exception == null && www.response != null)
        //    {

        //    }
        //    else
        //    {

        //    }
        //}
		yield return null;
	}
}
