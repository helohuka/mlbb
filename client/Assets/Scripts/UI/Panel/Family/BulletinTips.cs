using UnityEngine;
using System.Collections;

public class BulletinTips : MonoBehaviour {

	public UIButton enterBtn;
	public UIButton cancelBtn;
	public UIButton CloseBtn;
	public UIInput input;

	void Start () {
		UIManager.SetButtonEventHandler(enterBtn.gameObject, EnumButtonEvent.OnClick, OnClickenter, 0, 0);
		UIManager.SetButtonEventHandler(cancelBtn.gameObject, EnumButtonEvent.OnClick, OnClickcancel, 0, 0);
		UIManager.SetButtonEventHandler(CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
	}
	private void OnClickenter(ButtonScript obj, object args, int param1, int param2)
	{

		NetConnection.Instance.changeGuildNotice (input.value);
		//input.value = "";
		gameObject.SetActive (false);
	}
	private void OnClickcancel(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}
	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}

}
