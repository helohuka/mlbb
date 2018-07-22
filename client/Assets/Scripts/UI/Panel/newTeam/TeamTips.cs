using UnityEngine;
using System.Collections;

public class TeamTips : MonoBehaviour {

	public UIButton CloseBtn;
	public UIButton changeBtn;
	public UIButton KickBtn;
	public UIButton lookBtn;
	public UIButton HyBtn;
	//public GameObject ChenkPlayer;
	public int uId;
	void Start () {
		UIManager.SetButtonEventHandler (changeBtn.gameObject, EnumButtonEvent.OnClick, OnClickchangeH, 0, 0);
		UIManager.SetButtonEventHandler (KickBtn.gameObject, EnumButtonEvent.OnClick, OnClickKick, 0, 0);
		UIManager.SetButtonEventHandler (lookBtn.gameObject, EnumButtonEvent.OnClick, OnClicklook, 0, 0);
		UIManager.SetButtonEventHandler (HyBtn.gameObject, EnumButtonEvent.OnClick, OnClickHy, 0, 0);
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
	}
	void OnClickchangeH(ButtonScript obj, object args, int param1, int param2)
	{
		MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("duizhangtishi"),()=>{
			if(TeamSystem.AwayTeam(uId))
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("zanliduizhan"));
				return;
			}
			NetConnection.Instance.changeTeamLeader ((uint)uId);
			gameObject.SetActive(false);
		});	
	}
	void OnClickKick(ButtonScript obj, object args, int param1, int param2)
	{
		MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("tirentishi"),()=>{
			TeamSystem.isTuiteam = false;
			NetConnection.Instance.kickTeamMember ((uint)uId);
			gameObject.SetActive(false);
		});
	}
	void OnClicklook(ButtonScript obj, object args, int param1, int param2)
	{
		//ChenkPlayer.SetActive (true);
//		TeamPlayerInfo aplayerui = ChenkPlayer.GetComponent<TeamPlayerInfo>();
//		aplayerui.SPlayerInfo = GetPlayer (uId);
		TeamPlayerInfo.ShowMe (GetPlayer (uId));
		gameObject.SetActive(false);
	}
	void OnClickHy(ButtonScript obj, object args, int param1, int param2)
	{
		if(FriendSystem.Instance().IsmyFriend(uId))
		{
			PopText.Instance.Show( LanguageManager.instance.GetValue("alreadyhave"));
		}else
		{
			int fMax = 0;
			GlobalValue.Get(Constant.C_FriendMax, out fMax);
			if(FriendSystem.Instance().friends_.Count >= fMax)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue( "haoyoumax"));
				return;
			}
			NetConnection.Instance.addFriend ((uint)uId);
		}
		gameObject.SetActive(false);

	}
	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
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

}
