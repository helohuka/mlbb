using UnityEngine;
using System.Collections;

public class ChannelSwitching : MonoBehaviour {

	public UIToggle worldToggle;
	public UIToggle TeamToggle;
	public UIToggle GuildToggle;
	public UIButton closeBtn;
	//public UIButton DetermineBrn;

	void Start () {
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		//UIManager.SetButtonEventHandler (DetermineBrn.gameObject, EnumButtonEvent.OnClick, OnClickDetermine, 0, 0);
		UIManager.SetButtonEventHandler (worldToggle.gameObject, EnumButtonEvent.OnClick, OnClickworld, 0, 0);
		UIManager.SetButtonEventHandler (TeamToggle.gameObject, EnumButtonEvent.OnClick, OnClickTeam, 0, 0);
		UIManager.SetButtonEventHandler (GuildToggle.gameObject, EnumButtonEvent.OnClick, OnClickGuild, 0, 0);

	}
	private void OnClickDetermine(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.transform.parent.gameObject.SetActive (false);
	}
	private void OnClickworld(ButtonScript obj, object args, int param1, int param2)
	{
		//ChatSystem.instance.isShowWorldInfo = !ChatSystem.instance.isShowWorldInfo;
//		if (ChatSystem.instance.isShowWorldInfo)
//		{
//			ChatSystem.instance.chatType = ChatKind.CK_World;
//			//ChatSystem.instance.ChatKinds[0] = ChatKind.CK_World;		
//		}else
//		{
//			//ChatSystem.instance.ChatKinds[0] = ChatKind.CK_None;
//			ChatSystem.instance.chatType = ChatKind.CK_None;
//		}

	}
	private void OnClickTeam(ButtonScript obj, object args, int param1, int param2)
	{
		//ChatSystem.instance.isShowTeamInfo = !ChatSystem.instance.isShowTeamInfo;
//		if (ChatSystem.instance.isShowTeamInfo)
//		{
//			ChatSystem.instance.chatType = ChatKind.CK_Team;
//			//ChatSystem.instance.ChatKinds[1] = ChatKind.CK_Team;		
//		}else
//		{
//			//ChatSystem.instance.ChatKinds[1] = ChatKind.CK_None;
//			ChatSystem.instance.chatType = ChatKind.CK_None;
//		}

	}
	private void OnClickGuild(ButtonScript obj, object args, int param1, int param2)
	{
		//ChatSystem.instance.isShowGuildInfo = !ChatSystem.instance.isShowGuildInfo;
//		if (ChatSystem.instance.isShowGuildInfo)
//		{
//			ChatSystem.instance.chatType = ChatKind.CK_Guild;
//			//ChatSystem.instance.ChatKinds[2] = ChatKind.CK_World;		
//		}else
//		{
//			//ChatSystem.instance.ChatKinds[2] = ChatKind.CK_None;
//			ChatSystem.instance.chatType = ChatKind.CK_None;
//		}
	}
	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.transform.parent.gameObject.SetActive (false);
	}
	
}
