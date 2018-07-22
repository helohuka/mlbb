using UnityEngine;
using System.Collections;

public class MemberCell : MonoBehaviour {

	public UILabel nameLabel;
	public UILabel levelLabel;
	public UILabel zhiyeLabel;
	public UILabel zhiweiLabel;
	public UILabel gongxianLabel;
	public UILabel timelabel;
	private COM_GuildMember  _member;
	public COM_GuildMember Member
	{
		set
		{
			if(value == null)
			{
				_member = value;
				return ;
			}
//			if(value !=null)
//			{
				_member = value;
				nameLabel.text = _member.roleName_;
				levelLabel.text = _member.level_.ToString();
				gongxianLabel.text = _member.contribution_.ToString();
                string fmt = "MM月dd日HH:mm";
				if(_member.offlineTime_ == 0)
				{
					timelabel.text = LanguageManager.instance.GetValue("zaixian");
				}else
				{
			
					//Define.FormatUnixTimestamp(ref fmt, (int)_member.offlineTime_);
				   timelabel.text =GetTime(_member.offlineTime_);
					
						
				}
                
                Profession prof = Profession.get((JobType)_member.profType_, _member.profLevel_);
                if (prof == null)
                {
                    ClientLog.Instance.LogError("Member: " + _member.roleName_ + " has no Profression!");
                    zhiyeLabel.text = "-";
                }
                else
                {
                    zhiyeLabel.text = prof.jobName_;
                }
				zhiweiLabel.text = LanguageManager.instance.GetValue(_member.job_.ToString());
//			}
		}
		get
		{
			return _member;
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
	}
	

}
