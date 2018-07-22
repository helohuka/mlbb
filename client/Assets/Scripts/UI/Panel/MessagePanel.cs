using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MessagePanel : MonoBehaviour {

	public UIButton closeBtn;
	public UIButton enterBtn;

	void Start () {

		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickclose, 0, 0);
		UIManager.SetButtonEventHandler (enterBtn.gameObject, EnumButtonEvent.OnClick, OnClickenterBtn, 0, 0);
	}

	private void OnClickclose(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}

	private void OnClickenterBtn(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.exitTeam ();

		gameObject.SetActive (false);
	}



}
