using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SkillInfo : MonoBehaviour {

	public delegate void LearningLose(int sid,int d);
	public static LearningLose palyerLearningLose;

	public delegate void LearningOK();
	public static LearningOK palyerLearningOK;

	public delegate void BeginState(bool isShow);
	public static BeginState SetBeginState;
	
	public UILabel needLevel;
	public UILabel ProfessionLabel;
	public UILabel deyiLabel;
	public UITexture skillIcon;
	public UISprite CurrencyIcon;
	public UILabel nameLabel;
	public UILabel desLabel;
	public UILabel levelLabel;
	//public UILabel ConditionLabel;
	public GameObject ConditionObj;
	public UILabel TaskNameLabel;
	public UILabel CurrencyLabel;
	public GameObject diObj;
	public UILabel tishiLabel;
	public  UIButton BeginBtn;
	private Profession prof;
	private SkillData skData_;

	private List<string> _icons = new List<string>();

	public SkillData SkpData
	{
		set
		{
			if(value != null)
			{
				skData_ = value;
				prof = Profession.get ((JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession), GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel));
				nameLabel.text = skData_._Name;
				desLabel.text = skData_._Desc;
				CurrencyLabel.text = skData_._LearnCoin.ToString();
				needLevel.text = skData_._LearnLv.ToString();
				needLevel.alignment = NGUIText.Alignment.Left;
				//skillIcon.spriteName = skData_.resIconName;
				HeadIconLoader.Instance.LoadIcon (skData_._ResIconName, skillIcon);

				if(!_icons.Contains(skData_._ResIconName))
				{
					_icons.Add(skData_._ResIconName);
				}

//				if(skData_.LearnQuestID_!=0)
//				{
//					//renwuLabel.gameObject.SetActive(true);
//					TaskNameLabel.text = QuestData.GetData(skData_.LearnQuestID_).questName_;
//				}else
//				{
//					TaskNameLabel.text = "";
//				}
                int level = prof.getSkilMaxLevel(skData_._Id);
				if(level==0)
				{
					tishiLabel.gameObject.SetActive(true);
					levelLabel.gameObject.SetActive(false);
					diObj.SetActive(false);
					tishiLabel.text =  "当前职业不能学习该技能";
				}else
				{
					tishiLabel.gameObject.SetActive(false);
					diObj.SetActive(true);
					levelLabel.gameObject.SetActive(true);
					levelLabel.text = level.ToString ();
				}
                bool isdeyi = prof.isProudSkill(GamePlayer.Instance.GetIprop(PropertyType.PT_Profession), skData_._Id, GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel));
				if(isdeyi)
				{
					deyiLabel.gameObject.SetActive(true);
				}else
				{
					deyiLabel.gameObject.SetActive(false);
				}
//				if(QuestSystem.IsComplate(skData_.LearnQuestID_))
//				{
//					//TaskNameLabel.gameObject.SetActive(false);
//					//renwuLabel.gameObject.SetActive(false);
//					//ConditionObj.SetActive(true);
////					ConditionLabel.gameObject.SetActive(true);
////					ConditionLabel.text = "已完成";
//				}else
//				{
//					//renwuLabel.gameObject.SetActive(true);
//					//TaskNameLabel.gameObject.SetActive(true);
//					//ConditionObj.SetActive(true);
////					ConditionLabel.gameObject.SetActive(true);
////					ConditionLabel.text = "完成";
//				}
//				if(skData_.LearnQuestID_ == 0)
//				{
//					//renwuLabel.gameObject.SetActive(false);
//					//ConditionLabel.gameObject.SetActive(false);
//					//ConditionObj.SetActive(false);
//				}
//				if(!IsAlreadyLearnedSkills())
//				{
//					BeginBtn.gameObject.SetActive(true);
//				}
			}
		}
		get
		{
			return skData_;
		}
	}
	
	void Start () {
		prof = Profession.get ((JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession), GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel));
		palyerLearningLose = SkillLose;
		palyerLearningOK = TishiMwssage;
		UIManager.SetButtonEventHandler (BeginBtn.gameObject, EnumButtonEvent.OnClick, OnClickBegin, 0, 0);
		ProfessionLabel.text = prof.jobName_;
		SetBeginState = BeginBtnState;
        GuideManager.Instance.RegistGuideAim(BeginBtn.gameObject, GuideAimType.GAT_LearnSkillBtn);
	}
	void BeginBtnState(bool isShow)
	{
		BeginBtn.gameObject.SetActive(isShow);
	}
	void SkillLose(int sid,int d)
	{
		if(d==1)
		{

            PopText.Instance.Show(LanguageManager.instance.GetValue("cannotLearnSkill"));
		}else
			if(d==2)
		{
            PopText.Instance.Show(LanguageManager.instance.GetValue("skillNotMatchJob"));

		}else
			if(d==3)
		{
            //PopText.Instance.Show("您不能学习当前技能");
            PopText.Instance.Show(LanguageManager.instance.GetValue("cannotLearnSkill"));
		}

	}
    void TishiMwssage()
	{
//		if(IsAlreadyLearnedSkills())
//		{
//			BeginBtn.gameObject.SetActive(false);
//		}

        PopText.Instance.Show(LanguageManager.instance.GetValue("learnSuccess"));
		SkillXiDeUI (SkpData._Id);
        ChatSystem.PushSystemMessage(LanguageManager.instance.GetValue("wuxude").Replace("{n}", SkillData.GetMinxiLevelData(SkpData._Id)._Name));
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainLearningSkillOk);
	}
	void OnClickBegin(ButtonScript obj, object args, int param1, int param2)
	{
        int max = 0;
        GlobalValue.Get(Constant.C_LearnSkillMaxNum, out max);
		int level = prof.getSkilMaxLevel(skData_._Id);
        if (level == 0)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("learnSkillJobNotMatch"));
            return;
        }
		else if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level)<skData_._LearnLv)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("skillLevel").Replace("{n}",skData_._LearnLv.ToString()));
			return;
		}else
			if (GamePlayer.Instance.Properties[(int)PropertyType.PT_Money] < skData_._LearnCoin)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("EN_MoneyLess"));
            return;

        }
        else if (IsAlreadyLearnedSkills(SkpData._Id))
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("learnSkillReplice"));
            return;
        }
		else if (skData_._LearnQuestID != 0 && !QuestSystem.IsComplate(skData_._LearnQuestID))
        {
			QuestData qd = QuestData.GetData(skData_._LearnQuestID);
            if (qd != null)
            {
                if (QuestSystem.IsComplate(qd.id_))
                {
                    PopText.Instance.Show(LanguageManager.instance.GetValue("YouHadQuest"));
                    return;
                }
                else if (QuestSystem.IsQuestDoing(qd.id_))
                {
                    PopText.Instance.Show(LanguageManager.instance.GetValue("YouHadComplateQuest"));
                    return;
                }
                string msg = LanguageManager.instance.GetValue("YouNeedDoThisQuest") +
					StringTool.MakeNGUIStringInfoFmt(skData_._LearnQuestID.ToString(), qd.questName_);
                MessageBoxUI.ShowMe(msg, null, true, null, __TryGotoQuestNpc);
            }

            //PopText.Instance.Show("任务没有完成");
        }
        else if (GetLearaSkillNum() >= max)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("EN_SkillSoltFull"));
            return;
        }
        else
        {
            MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("learSkill"), __NormalLearnSkill);
        }

	}

    void __TryGotoQuestNpc(string info)
    {
        int qi = int.Parse(info);

        if (!QuestSystem.IsQuestAcceptable(qi))
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("YouCanntAcceptQuest"));
            return;
        }
      
        else
        {
            QuestData qd = QuestData.GetData(qi);
            if (GameManager.Instance.ParseNavMeshInfo(qd.xunlu))
            {
                Prebattle.Instance.ChangeWalkEff(Prebattle.WalkState.WS_AFP);
            }
        }
       

    }

    void __NormalLearnSkill()
    {
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainLearningSkillOk);
        NetConnection.Instance.learnSkill((uint)SkpData._Id);
    }

    void __LernSkillGotoQuestLink()
    {

    }

	void SkillXiDeUI(int skillId)
	{
		for(int i =0;i<LearningUI.Instance.itemList.Count;i++)
		{
			LearningCell lcell = LearningUI.Instance.itemList[i].GetComponent<LearningCell>();
			if(lcell.SkpData._Id == skillId)
			{
				lcell.xideSp.gameObject.SetActive(true);
			}
		}
	}
	bool IsProfessionSkill(int GroupId)
	{

		for(int i=0;i<prof.GroupsId_.Count;i++)
		{
			if(GroupId == int.Parse(prof.GroupsId_[i]))
			{
				return true;
			}
		}
		return false;
	}
	bool IsAlreadyLearnedSkills(int skid)
	{
		for (int i =0; i< GamePlayer.Instance.SkillInsts.Count; i++) {
            if (skid == GamePlayer.Instance.SkillInsts[i].skillID_)
			{
				return true;
			}
		}
		return false;
	}

	public int GetLearaSkillNum()
	{
		int skillNum = 0;
        foreach (COM_Skill s in GamePlayer.Instance.SkillInsts)
		{
			SkillData Sdata = SkillData.GetMinxiLevelData((int)s.skillID_);
			if( Sdata._SkillType== SkillType.SKT_Active || Sdata._SkillType== SkillType.SKT_CannotUse || Sdata._SkillType== SkillType.SKT_Passive)
			{
				++skillNum;
			}
		}
		return skillNum;
	}



	void OnDestroy()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}
}
