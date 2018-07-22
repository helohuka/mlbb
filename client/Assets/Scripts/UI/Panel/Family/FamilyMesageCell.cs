using UnityEngine;
using System.Collections;
using System;

public class FamilyMesageCell : MonoBehaviour {

	public UILabel nameLabel;
	public UILabel levelLabel;
	public UILabel zhiyeLabel;
	public UILabel timeLabel;
	public UIButton acceptBtn;
	public UIButton RefuseBtn;

	public UILabel _AcceptLable;
	public UILabel _RefuseLable;

	private COM_GuildRequestData  _requestData;
	public COM_GuildRequestData MemRequestDataber
	{
		set
		{
			if(value !=null)
			{
				_requestData = value;
				nameLabel.text = _requestData.roleName_;
				levelLabel.text = _requestData.level_.ToString();
				Profession prof = Profession.get((JobType)_requestData.prof_, _requestData.profLevel_);
				zhiyeLabel.text = prof.jobName_;
				timeLabel.text = GetTime((uint)_requestData.time_);
				//zhiyeLabel.text = Profession.get ((JobType)_member.job_, _member.profLevel_).jobName_;
				//zhiweiLabel.text = 
			}
		}
		get
		{
			return _requestData;
		}
	}
	private string GetTime(uint timeStamp)
	{
		System.TimeSpan ts = System.DateTime.Now - Define.TransUnixTimestamp(timeStamp);
		if (ts.Days > 10)
		{
			return LanguageManager.instance.GetValue("shitianqian");
		}
		else if (ts.Days > 0)
		{
			return LanguageManager.instance.GetValue("tianshu").Replace("{n}", ts.Days.ToString());
		}
		else if (ts.Hours > 0)
		{
			return LanguageManager.instance.GetValue("dayuyixiaoshi").Replace("{n}", ts.Hours.ToString());
		}
		else
		{
			return LanguageManager.instance.GetValue("xiayuyixiaoshi");
		}
		
	}
	void Start () {
		_AcceptLable.text = LanguageManager.instance.GetValue ("Guild_Accept");
		_RefuseLable.text = LanguageManager.instance.GetValue ("Guild_Refuse");
		UIManager.SetButtonEventHandler(acceptBtn.gameObject, EnumButtonEvent.OnClick, OnClickaccept, 0, 0);
		UIManager.SetButtonEventHandler(RefuseBtn.gameObject, EnumButtonEvent.OnClick, OnClickRefuse, 0, 0);
	}
	private void OnClickaccept(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.acceptRequestGuild ((int)MemRequestDataber.roleId_);
	}
	private void OnClickRefuse(ButtonScript obj, object args, int param1, int param2)
	{
		if(IsRefShopCountDown())
		{
			NetConnection.Instance.refuseRequestGuild ((int)MemRequestDataber.roleId_);
		}else
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("EN_CommandPositionLess"));
		}

	}
	bool IsRefShopCountDown()
	{
		GuildJob go = (GuildJob)Enum.ToObject (typeof(GuildJob), GuildSystem.GetGuildMemberSelf(GamePlayer.Instance.InstId).job_);

		if(go == GuildJob.GJ_Premier )
		{
			return true;
			
		}else if(go == GuildJob.GJ_SecretaryHead)
		{
			return true;
			
		}else if(go == GuildJob.GJ_VicePremier)
		{
			return true;
		}
		return false;
	}
}
