using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpenFunction
{
	public bool _isShow;
	GameObject panelObj_;
	static OpenFunction inst_;
	public List<string> funName = new List<string>(); 
	public List<GameObject> openFunObj = new List<GameObject>();

	public static OpenFunction Instance
	{
		get
		{
			if (inst_ == null)
			{
				inst_ = new OpenFunction();
				inst_.Init();
			}
			return inst_;
		}
	}

	public string GetOpenFun()
	{
		return funName [0];
	}

	public void RemoveName(string name)
	{
		funName.Remove(name);
	}

	public void Init()
	{
		if(panelObj_ == null)
			panelObj_ = Resources.Load<GameObject>("OpenFunctionPanel") as GameObject;
	}


	public void Show(string content,GameObject obj)
	{
		if (obj == null)
			return;

		if(funName.Count== 0)
		{
			GameObject clone = (GameObject)GameObject.Instantiate(panelObj_);
			
			UIPanel rootPane = ApplicationEntry.Instance.uiRoot.GetComponent<UIPanel>();
			
			clone.transform.parent = rootPane.transform;
			clone.transform.localScale = Vector3.one;
			clone.GetComponent<OpenFunctionUI>().SetFunIcon(LanguageManager.instance.GetValue (content));
			clone.GetComponent<OpenFunctionUI>().content_ = content;
			//clone.GetComponent<OpenFunctionUI>().content_ = content;
			//clone.GetComponent<OpenFunctionUI>().btnObj = obj;
			//clone.GetComponent<OpenFunctionUI>().btnObj.SetActive (true);
			//obj.transform.FindChild ("Background").gameObject.SetActive(false);
			//clone.GetComponent<OpenFunctionUI> ().functionImg.transform.parent = obj.transform;
			//clone.GetComponent<OpenFunctionUI> ().functionImg.GetComponent<TweenPosition> ().from = clone.GetComponent<OpenFunctionUI> ().functionImg.transform.localPosition;
			//clone.GetComponent<OpenFunctionUI> ().functionImg.GetComponent<TweenPosition> ().to = Vector3.zero;
			//Destroy des = clone.AddComponent<Destroy>();
			//des.SetLifeTime (5);//lifetime = 10f;

		}

		funName.Add (content);

		//openFunObj.Add (clone);
	}


	public void UpdateShowUI(string str)
	{
		if (funName.Contains (str))
		{
			funName.Remove (str);
		}
		if(funName.Count > 0)
		{
			GameObject clone = (GameObject)GameObject.Instantiate(panelObj_);
			
			UIPanel rootPane = ApplicationEntry.Instance.uiRoot.GetComponent<UIPanel>();
			
			clone.transform.parent = rootPane.transform;
			clone.transform.localScale = Vector3.one;
			clone.GetComponent<OpenFunctionUI>().SetFunIcon(LanguageManager.instance.GetValue (funName[0]));
			clone.GetComponent<OpenFunctionUI>().content_ = funName[0];
		}
	}



	//public void Show(string fun)
	//{
	//	funName.Add (fun);
	//	if(!_isShow)
	//	{
		//	OpenFunctionUI.ShowMe("");
		//	_isShow = true;
		//}
	//}


}

