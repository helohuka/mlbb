using UnityEngine;
using System.Collections;

public class HaoyouShezhi : MonoBehaviour {

	public UIButton AddHaoBtn;
	public UIButton ViewProfileBtn;
	public UIButton teamBtn;
	public UIButton BlacklistBtn;
	public UIButton CloseBtn;
	public  string name; 
	public uint insetId;
	void Start () {
		UIManager.SetButtonEventHandler (AddHaoBtn.gameObject, EnumButtonEvent.OnClick, OnClickAddHaoBtn, 0, 0);
		UIManager.SetButtonEventHandler (ViewProfileBtn.gameObject, EnumButtonEvent.OnClick, OnClickViewProfileBtn, 0, 0);

		UIManager.SetButtonEventHandler (teamBtn.gameObject, EnumButtonEvent.OnClick, OnClickteamBtn, 0, 0);
		UIManager.SetButtonEventHandler (BlacklistBtn.gameObject, EnumButtonEvent.OnClick, OnClickBlacklistBtn, 0, 0);
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickCloseBtn, 0, 0);

	}
	void ShowInfo(COM_SimplePlayerInst Inst)
	{
		TeamPlayerInfo.SwithShowMe ((COM_SimplePlayerInst)Inst);
	}
	void OnEnable()
	{
		ChartsSystem.QueryPlayerEventOk += ShowInfo;
	}
	void OnDisable()
	{
		ChartsSystem.QueryPlayerEventOk -= ShowInfo;
	}
	private void OnClickAddHaoBtn(ButtonScript obj, object args, int param1, int param2)
	{

		int fMax = 0;
		GlobalValue.Get(Constant.C_FriendMax, out fMax);
		if(FriendSystem.Instance().friends_.Count >= fMax)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue( "haoyoumax"));
			return;
		}

		NetConnection.Instance.addFriend (insetId);

	}
	private void OnClickViewProfileBtn(ButtonScript obj, object args, int param1, int param2)
	{
		/// <summary>
		/// 查看资料		/// </summary>
		/// <param name="obj">Object.</param>

		NetConnection.Instance.queryPlayerbyName (name);
		//
	}

	private void OnClickteamBtn(ButtonScript obj, object args, int param1, int param2)
	{
		/// <summary>
		/// 申请入队		/// </summary>

		if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Team))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("duiwuweikai"));
			return;
		}
		NetConnection.Instance.requestJoinTeam (name);
	}
	private void OnClickBlacklistBtn(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.addBlacklist (insetId);
	}
	private void OnClickCloseBtn(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}

}
