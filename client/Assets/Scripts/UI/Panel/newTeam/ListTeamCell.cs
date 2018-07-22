using UnityEngine;
using System.Collections;

public class ListTeamCell : MonoBehaviour {


	public UILabel _FTeamNameLable;
	public UILabel _FTargetLable;
	public UILabel _FTeamNumLable;
	public UILabel _FLevelLable;
	public UILabel _FJionLable;


	public UILabel teamNameLabel;
	public UILabel mubiaoLabel;
	public UILabel levelLabel;
	public UILabel nameLabel;
	public UIButton JoinBtn;
	public GameObject tips;
	public UILabel numLabel;
	public UILabel teanName;
	public UISprite[] menbersSp;
	public static int teamId;
	private COM_SimpleTeamInfo _SimpleTeamInfo;
	public COM_SimpleTeamInfo SimpleTeamInfo
	{
		set{
			if(value !=null)
			{
				_SimpleTeamInfo = value;
				teamNameLabel.text = _SimpleTeamInfo.leaderName_;
				teanName.text = _SimpleTeamInfo.name_;
				//nameLabel.text = Profession.get ((JobType)_SimpleTeamInfo.job_,(int)_SimpleTeamInfo.joblevel_).jobName_;
				mubiaoLabel.text = LanguageManager.instance.GetValue(_SimpleTeamInfo.type_.ToString());
				numLabel.text = _SimpleTeamInfo.curMemberSize_+ "/"+_SimpleTeamInfo.maxMemberSize_;
				levelLabel.text = _SimpleTeamInfo.minLevel_ +"-"+_SimpleTeamInfo.maxLevel_;
//				for(int i=0;i<menbersSp.Length;i++)
//				{
//					if(i<_SimpleTeamInfo.curMemberSize_)
//					{
//						menbersSp[i].spriteName = "renshuding";
//					}
//				}
			}
		}
		get{
			return _SimpleTeamInfo;
		}
	}
	void Start () {
		InitUIText ();
		UIManager.SetButtonEventHandler (JoinBtn.gameObject, EnumButtonEvent.OnClick, OnClickJoin, 0, 0);	

	}
	void InitUIText()
	{
		_FTeamNameLable.text = LanguageManager.instance.GetValue("TeamF_FTeamName");
		_FTargetLable.text = LanguageManager.instance.GetValue("TeamF_FTarget");
		_FTeamNumLable.text = LanguageManager.instance.GetValue("TeamF_FTeamNum");
		_FLevelLable.text = LanguageManager.instance.GetValue("TeamF_FLevel");
		_FJionLable.text = LanguageManager.instance.GetValue("TeamF_FJion");
	}
	void OnClickJoin(ButtonScript obj, object args, int param1, int param2)
	{
		SceneData ssd = SceneData.GetData (GameManager.SceneID);

		if (SimpleTeamInfo.isRunning_)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("jiaruteamkaishi"));
		}
		else 
		{
			int level = GamePlayer.Instance.GetIprop(PropertyType.PT_Level);
			if(level>=SimpleTeamInfo.minLevel_ && level<=SimpleTeamInfo.maxLevel_)
			{
				if(SimpleTeamInfo.needPassword_)
				{
					teamId = (int)SimpleTeamInfo.teamId_;
					tips.SetActive(true);
				}else
				{
					NetConnection.Instance.joinTeam(SimpleTeamInfo.teamId_,"");
					FastTeamPanel.HideMe ();
				}
			}else
			{
				//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("dengjitishi"));
				PopText.Instance.Show(LanguageManager.instance.GetValue("dengjitishi"));
			}
		}



	}

}
