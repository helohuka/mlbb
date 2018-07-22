using UnityEngine;
using System.Collections;

public class HaoYouCell : MonoBehaviour {

	public UIButton YaoqingBtn;
	public UILabel nameLabel;
	public UILabel levelLabel;
	public UILabel zhiYeLabel;

	public UILabel _LevelLable;
	public UILabel _JobLable;
	public UILabel _InviteLabel;
	private string name;
	private COM_ContactInfo _ContactInfo;
	public COM_ContactInfo ContactInfo
	{
		set
		{
			if(value!=null)
			{
				_ContactInfo = value;
				nameLabel.text = _ContactInfo.name_;
				name = _ContactInfo.name_;
				levelLabel.text = _ContactInfo.level_.ToString();
				zhiYeLabel.text = Profession.get ((JobType)_ContactInfo.job_,(int)_ContactInfo.jobLevel_).jobName_;
			}
		}
		get
		{
			return _ContactInfo;
		}
	}
	private COM_GuildMember _GuildMember;
	public COM_GuildMember GuildMember
	{
		set
		{
			if(value!=null)
			{
				_GuildMember = value;
				nameLabel.text = _GuildMember.roleName_;
				name = _GuildMember.roleName_;
				levelLabel.text = _GuildMember.level_.ToString();
				zhiYeLabel.text = Profession.get ((JobType)_GuildMember.profType_,(int)_GuildMember.profLevel_).jobName_;
			}
		}
		get
		{
			return _GuildMember;
		}
	}
	private Avatar _Avatar;
	public Avatar Avatarr
	{
		set
		{
			if(value!=null)
			{
				_Avatar = value;
				nameLabel.text = _Avatar.playerData_.instName_;
				name = _Avatar.playerData_.instName_;
				levelLabel.text = _Avatar.playerData_.level_.ToString();
				zhiYeLabel.text = Profession.get ((JobType)_Avatar.playerData_.jt_,(int)_Avatar.playerData_.jl_).jobName_;
			}
		}
		get
		{
			return _Avatar;
		}
	}
	void Start () {
		_LevelLable.text = LanguageManager.instance.GetValue("Team_Level1");
		_JobLable.text = LanguageManager.instance.GetValue("Team_Job");
		_InviteLabel.text = LanguageManager.instance.GetValue("Team_Invite1");
		UIManager.SetButtonEventHandler(YaoqingBtn.gameObject, EnumButtonEvent.OnClick, OnClicYaoqing,0, 0);
	}
	void OnClicYaoqing(ButtonScript obj, object args, int param1, int param2)
	{
		if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Team))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("duiwuweikai"));
			return;
		}
//		if(!(((GuildMember.openSubSystemFlag_) &(0x1 << (int)OpenSubSystemFlag.OSSF_Team)) != 0))
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
		if(ssd.sceneType_ == SceneType.SCT_GuildBattleScene)
		{
			if(!GuildSystem.IsInMyGuild(GuildMember.roleId_))
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("bunengyaoqingdifang"));
				return;
			}
			
		}
		NetConnection.Instance.inviteTeamMember (name);
	}

}
