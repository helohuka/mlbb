using UnityEngine;
using System.Collections;

public class TeamCell : MonoBehaviour {

	public UISprite titleSp;
	public UISprite zhiyeSp;	
	public GameObject objBtn;
	public UIButton OperatingBtn;
	public UIButton changeBtn;
	public UIButton KickBtn;
	public UIButton lookBtn;
	public Transform modesPos;
	public UILabel nameLabel;
	public UILabel LevelLabel;
	public UILabel zhiyeLabel;
	public GameObject tipsObj;
	public int instId;
	//public GameObject playerInfoObj;
	private bool isShow;
	private int uId;
	void Start () {
		 
		//tipsObj.layer = LayerMask.NameToLayer ("3D");
		//NGUITools.SetChildLayer(tipsObj.transform, LayerMask.NameToLayer("3D"));
		UIManager.SetButtonEventHandler (OperatingBtn.gameObject, EnumButtonEvent.OnClick, OnClickOperating, 0, 0);
		UIManager.SetButtonEventHandler (changeBtn.gameObject, EnumButtonEvent.OnClick, OnClickchangeH, 0, 0);
		UIManager.SetButtonEventHandler (KickBtn.gameObject, EnumButtonEvent.OnClick, OnClickKick, 0, 0);
		UIManager.SetButtonEventHandler (lookBtn.gameObject, EnumButtonEvent.OnClick, OnClicklook, 0, 0);
		UIManager.SetButtonEventHandler (objBtn.gameObject, EnumButtonEvent.OnClick, OnClickobjBtnClose, 0, 0);

	
		uId = int.Parse (gameObject.name);
	}
	void OnClickobjBtnClose(ButtonScript obj, object args, int param1, int param2)
	{
		objBtn.SetActive (false);
		isShow = false;
	}
	void OnClicklook(ButtonScript obj, object args, int param1, int param2)
	{
		//playerInfoObj.SetActive (true);
//		TeamPlayerInfo aplayerui = playerInfoObj.GetComponent<TeamPlayerInfo>();
//		aplayerui.SPlayerInfo = GetPlayer (uId);
		TeamPlayerInfo.ShowMe (GetPlayer (uId));
		objBtn.SetActive (false);
	}
	COM_SimplePlayerInst GetPlayer(int uid)
	{
		for(int i =0;i<TeamSystem.GetTeamMembers().Length;i++)
		{
			if(uid == TeamSystem.GetTeamMembers()[i].instId_)
			{
				return TeamSystem.GetTeamMembers()[i];
			}
		}
		return null;
	}

	void OnClickOperating(ButtonScript obj, object args, int param1, int param2)
	{

		isShow = !isShow;
		objBtn.SetActive (true);
		objBtn.SetActive (isShow);
		if(isShow)
		{
			BoxCollider bCollider = objBtn.GetComponent<BoxCollider> ();

			bCollider.size = new Vector3( ApplicationEntry.Instance.UIWidth,ApplicationEntry.Instance.UIHeight,0);
			bCollider.center = new Vector3(0,Screen.height/2-bCollider.size.y/2,0);
		}


		//SetBtnsState (isShow);
//		SetBtnsDisplay ();
		
	}
	void OnClickchangeH(ButtonScript obj, object args, int param1, int param2)
	{
		MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("duizhangtishi"),()=>{
			NetConnection.Instance.changeTeamLeader ((uint)uId);
		});

		//isShow = false;
		//SetBtnsState (false);
		objBtn.SetActive (false);
		
	}
	void OnClickKick(ButtonScript obj, object args, int param1, int param2)
	{
		MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("tirentishi"),()=>{
			TeamSystem.isTuiteam = false;
			NetConnection.Instance.kickTeamMember ((uint)uId);
			//ChatSystem.instance.AddchatInfo(TeamMember(uId).instName_+LanguageManager.instance.GetValue("messageTichu"),ChatKind.CK_System);
		});
//		isShow = false;
//		SetBtnsState (false);
		objBtn.SetActive (false);
		
	}
	COM_SimplePlayerInst TeamMember(int uId)
	{
		for(int i=0;i<TeamSystem.GetTeamMembers().Length;i++)
		{
			if(uId == TeamSystem.GetTeamMembers()[i].instId_)
			{
				return TeamSystem.GetTeamMembers()[i];
			}
		}
		return null;
	}
	void SetBtnsState(bool isstate)
	{
		changeBtn.gameObject.SetActive (isstate);
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
