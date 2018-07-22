using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CreatePlayerInfo : MonoBehaviour {

	public UILabel OccupationLabel;
	public UILabel nameLabel;
	public UILabel levelLabel;
	public UIButton DeletePlayer;
	public GameObject PromptPanel;
	public UIButton startBtn;
	private  List< COM_SimpleInformation> roles;
	private static string nameStr;
	private static string LevelStr;
	void Start () {
	
		roles = CreatePlayerRole.GetRoles ();
	
		UIManager.SetButtonEventHandler (DeletePlayer.gameObject, EnumButtonEvent.OnClick, OnClickDeletePlayer, 0, 0);
		UIManager.SetButtonEventHandler (startBtn.gameObject, EnumButtonEvent.OnClick, OnClickstart, 0, 0);
		nameLabel.text =nameStr;
		levelLabel.text = LevelStr;
	}
    public static void SetRolesInfo(int insID)
	{
//		for (int i=0; i<roles.Length; i++)
//		{
//			if(insID== roles [i].instId_)
//			{
//				nameStr = "名字："+ roles [i].instName_;
//				LevelStr = "等级："+ roles [i].level_;
//
//			}
//		}
	}
	void OnClickDeletePlayer(ButtonScript obj, object args, int param1, int param2)
	{
		PromptPanel.SetActive (true);
	}
	void OnClickstart(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.enterGame((uint)CreatePlayerRole.roleId);
	}

}
