using UnityEngine;
using System.Collections;

public delegate void MessageBoxCallBack();
public class FriendMegBoxUI : MonoBehaviour
{

	public UIButton OkBtn;
	public UIButton cancelBtn;
	public UIButton closeBtn;
	public UILabel infoLab;
	public UIPanel msgBoxPane;

	private MessageBoxCallBack _callBack;

	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnCloseBtn, 0, 0);
		UIManager.SetButtonEventHandler (OkBtn.gameObject, EnumButtonEvent.OnClick, OnOkBtn, 0, 0);
		UIManager.SetButtonEventHandler (cancelBtn.gameObject, EnumButtonEvent.OnClick, OnCancelBtn, 0, 0);
	}
	


	public void Show(string msg,MessageBoxCallBack callback)
	{
		infoLab.text = msg; 
		msgBoxPane.gameObject.SetActive(true);
		_callBack = callback;
	}


	private void OnCloseBtn(ButtonScript obj, object args, int param1, int param2)
	{
		msgBoxPane.gameObject.SetActive (false);
	}

	private void OnOkBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(_callBack != null)
		{
			_callBack();
		}
		msgBoxPane.gameObject.SetActive (false);
	}

	private void OnCancelBtn(ButtonScript obj, object args, int param1, int param2)
	{
		msgBoxPane.gameObject.SetActive (false);
	}

		

}

