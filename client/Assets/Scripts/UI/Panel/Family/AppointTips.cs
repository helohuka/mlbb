using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AppointTips : MonoBehaviour {

	public UIButton enterBtn;
	public UIButton cancelBtn;
	public UIButton CloseBtn;
	public UIToggle DeputyToggle;
	public UIToggle mainToggle;
	private bool isDeputy;
	private bool ismain;
	private GuildJob gjob;
	private List<UIToggle> Toggles = new List<UIToggle> ();
	private COM_GuildMember _member;
	public COM_GuildMember Member
	{
		set
		{
			_member = value;
		}
		get
		{
			return _member;
		}
	}
	void Start () {
		Toggles.Add (mainToggle);
		Toggles.Add (DeputyToggle);
		UIManager.SetButtonEventHandler(enterBtn.gameObject, EnumButtonEvent.OnClick, OnClickenter, 0, 0);
		UIManager.SetButtonEventHandler(cancelBtn.gameObject, EnumButtonEvent.OnClick, OnClickcancel, 0, 0);
		UIManager.SetButtonEventHandler(CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
//		UIManager.SetButtonEventHandler(DeputyToggle.gameObject, EnumButtonEvent.OnClick, OnClickDeputyToggle, 0, 0);
//		UIManager.SetButtonEventHandler(mainToggle.gameObject, EnumButtonEvent.OnClick, OnClickmainToggle, 0, 0);
	}
	private void OnClickDeputyToggle(ButtonScript obj, object args, int param1, int param2)
	{
		UIToggle tog = obj.GetComponent<UIToggle> ();
		RadioToggle (tog);
		gjob = GuildJob.GJ_VicePremier;
	}
	private void OnClickmainToggle(ButtonScript obj, object args, int param1, int param2)
	{
		UIToggle tog = obj.GetComponent<UIToggle> ();
		RadioToggle (tog);
		gjob = GuildJob.GJ_SecretaryHead;
	}
	private void OnClickenter(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.changeMemberPosition ((int)Member.roleId_,gjob);
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
	void RadioToggle(UIToggle tog)
	{
		for(int i =0;i<Toggles.Count;i++)
		{
			if(tog.gameObject.name.Equals(Toggles[i].gameObject.name))
			{
				Toggles[i].value = true;
			}else
			{
				Toggles[i].value = false;
			}
		}
	}

}
