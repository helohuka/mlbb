using UnityEngine;
using System.Collections;
using System;
public class OperatingTips : MonoBehaviour {

	public UIButton appointfZBtn;
	public UIButton KickBtn;
	public UIButton appointZLBtn;
	public UIButton appointJYBtn;
	public UIButton appointCYBtn;
	public UIButton YQBtn;
	public UIButton JWBtn;
	public UISprite jiantou;

	public GameObject Close;
	public GameObject changeObj;
	private GuildJob gjob;
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
		UIManager.SetButtonEventHandler(Close, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		UIManager.SetButtonEventHandler(appointfZBtn.gameObject, EnumButtonEvent.OnClick, OnClickappoint, (int)GuildJob.GJ_VicePremier, 0);
		UIManager.SetButtonEventHandler(KickBtn.gameObject, EnumButtonEvent.OnClick, OnClickKick, 0, 0);
		UIManager.SetButtonEventHandler(appointZLBtn.gameObject, EnumButtonEvent.OnClick, OnClickappoint,(int)GuildJob.GJ_SecretaryHead, 0);
		UIManager.SetButtonEventHandler(appointJYBtn.gameObject, EnumButtonEvent.OnClick, OnClickappoint, (int)GuildJob.GJ_Minister, 0);
		UIManager.SetButtonEventHandler(appointCYBtn.gameObject, EnumButtonEvent.OnClick, OnClickappoint, (int)GuildJob.GJ_People, 0);
		UIManager.SetButtonEventHandler(YQBtn.gameObject, EnumButtonEvent.OnClick, OnClickYQ, 0, 0);
		UIManager.SetButtonEventHandler(JWBtn.gameObject, EnumButtonEvent.OnClick, OnClickJW, 0, 0);
	}

	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}
	private void OnClickYQ(ButtonScript obj, object args, int param1, int param2)
	{

		if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Team))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("duiwuweikai"));
			return;
		}
//		if(!(((PlayerInst.openSubSystemFlag_) &(0x1 << (int)OpenSubSystemFlag.OSSF_Team)) != 0))
//		{
//			PopText.Instance.Show(LanguageManager.instance.GetValue("Duifangduiwu"));
//			return;
//		}
		SceneData ssd = SceneData.GetData (GameManager.SceneID);
		if(ssd.sceneType_ == SceneType.SCT_AlonePK||ssd.sceneType_ == SceneType.SCT_TeamPK)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("bunengzudui"));
			return;
		}
		if(TeamSystem.IsInTeam())
		{
			NetConnection.Instance.inviteTeamMember (Member.roleName_);
			gameObject.SetActive (false);
		}else
		{
			//NetConnection.Instance.jointLobby();
			
			COM_CreateTeamInfo cti = new COM_CreateTeamInfo();
			cti.type_ = TeamType.TT_None;
			cti.name_ = "";
			//cti.pwd_ = passWordInput.text;
			cti.maxMemberSize_ = 5;
			cti.minLevel_ = 1;
			cti.maxLevel_ = 60;
			NetConnection.Instance.createTeam(cti);
		}




		//NetConnection.Instance.inviteTeamMember (Member.roleName_);
	}
	private void OnClickJW(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.addFriend ((uint)Member.roleId_);
	}

	private void OnClickappoint(ButtonScript obj, object args, int param1, int param2)
	{
		gjob = (GuildJob)Enum.ToObject (typeof(GuildJob), param1);

		MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("renming").Replace("{n}",Member.roleName_).Replace("(n1)",LanguageManager.instance.GetValue(param1.ToString())),()=>{
			if(gjob == GuildJob.GJ_VicePremier && GuildSystem.GetJopNumber(GuildJob.GJ_VicePremier)==2)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("reminfuzuzhang"));
				return;
			}else if(gjob == GuildJob.GJ_SecretaryHead&& GuildSystem.GetJopNumber(GuildJob.GJ_SecretaryHead)==4)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("renmingzhanglao"));
				return;
			}else
				if(gjob == GuildJob.GJ_Minister&& GuildSystem.GetJopNumber(GuildJob.GJ_Minister)==12)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("renmingjingying"));
				return;
			}
			NetConnection.Instance.changeMemberPosition ((int)Member.roleId_,gjob);
		},false,()=>{
			gameObject.SetActive (false);
		});
		gameObject.SetActive (false);
//		changeObj.SetActive (true);
//		AppointTips atips = changeObj.GetComponent<AppointTips>();
//		atips.Member = Member;
//		gameObject.SetActive (false);
	}
	private void OnClickKick(ButtonScript obj, object args, int param1, int param2)
	{
		COM_GuildMember self = GuildSystem.GetGuildMemberSelf (GamePlayer.Instance.InstId);
		GuildJob job = (GuildJob)Enum.ToObject (typeof(GuildJob), self.job_);
		if(job == GuildJob.GJ_Premier ||  job == GuildJob.GJ_VicePremier)
		{
			GuildJob mjob = (GuildJob)Enum.ToObject (typeof(GuildJob), Member.job_);
			if(mjob == GuildJob.GJ_People )
			{
				NetConnection.Instance.kickOut ((int)Member.roleId_);
			}else
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("tichuduiyuan"));
			}

		}else
		{

			PopText.Instance.Show(LanguageManager.instance.GetValue("EN_CommandPositionLess"));
		}


		gameObject.SetActive (false);
	}

}
