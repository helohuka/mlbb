using UnityEngine;
using System.Collections;

public class Global :MonoBehaviour
{
	public static Global instance;
	
	static Global()
	{
		GameObject go = GameObject.Find("GlobalObject");
		instance = go.AddComponent<Global>();
	}
	
	public void DoSomeThings()
	{
		ClientLog.Instance.Log("DoSomeThings");
	}
	
	void Start () 
	{
		ClientLog.Instance.Log("Start");
	}

	void Update()
	{

	}
	
}