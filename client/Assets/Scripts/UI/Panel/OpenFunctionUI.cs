using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpenFunctionUI :MonoBehaviour
{
	public UISprite kuang;
	public UITexture functionImg;
	public GameObject btnObj;
	public float lifetime = 2.0f;
	public string content_;

	private List<string> _icons = new List<string>();
	
	void Start () 
	{
		//functionImg.GetComponent<TweenPosition> ().SetOnFinished (OnIconFinish);
		//functionImg.spriteName = LanguageManager.instance.GetValue (content_);
	}


	void Awake()
	{
		Invoke("DestorySelf", lifetime);
	}
	
	public void SetLifeTime(float time)
	{
		CancelInvoke ("DestorySelf");
		Invoke("DestorySelf", time);
	}
	
	void DestorySelf()
	{
		OpenFunction.Instance.UpdateShowUI (content_);
		Destroy(gameObject);
	}

	public void SetFunIcon(string iconName)
	{
		HeadIconLoader.Instance.LoadIcon (iconName, functionImg);
		if(!_icons.Contains(iconName))
		{
			_icons.Add(iconName);
		}

	}

	void OnIconFinish()
	{
		//btnObj.transform.FindChild ("Background").gameObject.SetActive(true);
	//	Destroy (functionImg.gameObject);
	}

	void OnDestroy()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}


	/*public void Show()
	{

	}
	
	void OnDestroy()
	{
		//if(btnObj != null)
			//btnObj.transform.FindChild ("Background").gameObject.SetActive(true);
	}
	*/


}

