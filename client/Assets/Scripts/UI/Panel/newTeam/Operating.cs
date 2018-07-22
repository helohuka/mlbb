using UnityEngine;
using System.Collections;

public class Operating : MonoBehaviour {

	public UIButton OperatingBtn;
	public UIButton changeHBtn;
	public UIButton KickBtn;
	private bool isShow;
	private int uId;
	void Start () {
		UIManager.SetButtonEventHandler (OperatingBtn.gameObject, EnumButtonEvent.OnClick, OnClickOperating, 0, 0);
		UIManager.SetButtonEventHandler (changeHBtn.gameObject, EnumButtonEvent.OnClick, OnClickchangeH, 0, 0);
		UIManager.SetButtonEventHandler (KickBtn.gameObject, EnumButtonEvent.OnClick, OnClickKick, 0, 0);
		string UID = gameObject.name;
		uId = int.Parse(UID);
	}
	void OnClickOperating(ButtonScript obj, object args, int param1, int param2)
	{
		isShow = !isShow;
		SetBtnsState (isShow);
		SetBtnsDisplay ();

	}
	void OnClickchangeH(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.changeTeamLeader ((uint)uId);
		isShow = false;
		SetBtnsState (false);

	}
	void OnClickKick(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.kickTeamMember ((uint)uId);
		isShow = false;
		SetBtnsState (false);

	}
	void SetBtnsState(bool isstate)
	{
		changeHBtn.gameObject.SetActive (isstate);
		KickBtn.gameObject.SetActive (isstate);
	}
	void SetBtnsDisplay()
	{
//		for (int i = 0; i<TeamUIPanel.ross.Count; i++)
//		{
//			if(TeamUIPanel.ross[i].name != gameObject.name)
//			{
//				UIButton [] btns = TeamUIPanel.ross[i].GetComponentsInChildren<UIButton>();
//				foreach(UIButton bt in btns)
//				{
//					if(bt.gameObject.name.Equals("changeButton"))
//					{
//						bt.gameObject.SetActive(false);
//					}
//					if(bt.gameObject.name.Equals("KickButton"))
//					{
//						bt.gameObject.SetActive(false);
//					}
//				}
//
//			}
//		}
	}
	public void HideBtns()
	{
//		for (int i = 0; i<TeamUIPanel.ross.Count; i++)
//		{
//			UIButton [] btns = TeamUIPanel.ross[i].GetComponentsInChildren<UIButton>();
//			foreach(UIButton bt in btns)
//			{
//				if(bt.gameObject.name.Equals("changeButton"))
//				{
//					bt.gameObject.SetActive(false);
//				}
//				if(bt.gameObject.name.Equals("KickButton"))
//				{
//					bt.gameObject.SetActive(false);
//				}
//			}
//		}
	}

}
