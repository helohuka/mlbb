using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LearningUI : UIBase {

	public UILabel _TitleLable;
	public UILabel _SkillDesLable;
	public UILabel _ConsumptionLable;
	public UILabel _XLevelLable;
	public UILabel _ProudSkillsLable;
	public UILabel _ProfessionLable;
	public UILabel _AttackLable;
	public UILabel _MagicAttackLable;
	public UILabel _StateMagicLable;
	public UILabel _SecondarySkillsLable;
	public UILabel _LearningLable;


	public GameObject infoObj;
	public GameObject item;
	public UIGrid grid;
	public UIButton AttackSkillButton;
	public UIButton AttackMagicButton;
	public UIButton StatusMagicButton;
	public UIButton AidSkillButton;
	public UIButton jobButton;
	public UIButton ProduceSkillButton;
	public UIButton ForgingSkillButton;
	public UIButton closeBtn;

	public List<GameObject> itemList = new List<GameObject> ();
	private List<SkillData> skillist = new List<SkillData> ();

	private List<SkillData> ATskillist = new List<SkillData> ();
	private List<SkillData> AMskillist = new List<SkillData> ();
	private List<SkillData> Sskillist = new List<SkillData> ();
	private List<SkillData> Askillist = new List<SkillData> ();
	private List<SkillData> Pskillist = new List<SkillData> ();
	private List<SkillData> Fskillist = new List<SkillData> ();
	private List<UIButton>btns = new List<UIButton>();
	private Profession prof;
	private Profession _profData;
	private int _jobType;
	private int _proflevel;
	public static LearningUI LearningInstance;
	private LearningCell _curLearningCell;

	void Awake()
	{
		LearningInstance = this;
	}
	public static LearningUI Instance
	{
		get
		{
			return LearningInstance;	
		}
	}
	void Start () {
		item.SetActive (false);
		InitUIText ();
		btns.Add (AttackSkillButton);
		btns.Add (AttackMagicButton);
		btns.Add (StatusMagicButton);
		btns.Add (AidSkillButton);
		btns.Add (jobButton);
//		btns.Add (ProduceSkillButton);
//		btns.Add (ForgingSkillButton);

		UIManager.SetButtonEventHandler (jobButton.gameObject, EnumButtonEvent.OnClick, OnClicjobButton, 4, 0);
		UIManager.SetButtonEventHandler (AttackSkillButton.gameObject, EnumButtonEvent.OnClick, OnClicAttackSkill, 0, 0);
		UIManager.SetButtonEventHandler (AttackMagicButton.gameObject, EnumButtonEvent.OnClick, OnClickAttackMagic, 1, 0);
		UIManager.SetButtonEventHandler (StatusMagicButton.gameObject, EnumButtonEvent.OnClick, OnClickStatusMagic, 2, 0);
		UIManager.SetButtonEventHandler (AidSkillButton.gameObject, EnumButtonEvent.OnClick, OnClickAidSkill, 3, 0);
//		UIManager.SetButtonEventHandler (ProduceSkillButton.gameObject, EnumButtonEvent.OnClick, OnClickProduceSkill, 4, 0);
//		UIManager.SetButtonEventHandler (ForgingSkillButton.gameObject, EnumButtonEvent.OnClick, OnClickForgingSkill, 5, 0);
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickclose, 5, 0);


		_profData = Profession.get ((JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession), GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel));
		_jobType = GamePlayer.Instance.GetIprop(PropertyType.PT_Profession);
		_proflevel = GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel);
		InItData ();

        GuideManager.Instance.RegistGuideAim(AttackSkillButton.gameObject, GuideAimType.GAT_LearnSkillAttackSkillTab);
        GuideManager.Instance.RegistGuideAim(AttackMagicButton.gameObject, GuideAimType.GAT_LearnSkillAttackMagicTab);
        GuideManager.Instance.RegistGuideAim(StatusMagicButton.gameObject, GuideAimType.GAT_LearnSkillStatusMagicTab);
        GuideManager.Instance.RegistGuideAim(AidSkillButton.gameObject, GuideAimType.GAT_LearnSkillAidSkillTab);
        
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainLearningUI, GamePlayer.Instance.GetIprop(PropertyType.PT_Profession));

		UIManager.Instance.LoadMoneyUI (this.gameObject);

	}
	void InitUIText()
	{
		_TitleLable.text = LanguageManager.instance.GetValue("Skill_Title");
		_SkillDesLable.text = LanguageManager.instance.GetValue("Skill_SkillDes");
		_ConsumptionLable.text = LanguageManager.instance.GetValue("Skill_Consumption");
		_XLevelLable.text = LanguageManager.instance.GetValue("Skill_XLevel");
		_ProudSkillsLable.text = LanguageManager.instance.GetValue("Skill_ProudSkills");
		_ProfessionLable.text = LanguageManager.instance.GetValue("Skill_Profession");
		_AttackLable.text = LanguageManager.instance.GetValue("Skill_Attack");
		_MagicAttackLable.text = LanguageManager.instance.GetValue("Skill_MagicAttack");
		_StateMagicLable.text = LanguageManager.instance.GetValue("Skill_StateMagic");
		_SecondarySkillsLable.text = LanguageManager.instance.GetValue("Skill_SecondaryS");
		_LearningLable.text = LanguageManager.instance.GetValue("Skill_Learning");

	
	}
	void InItData()
	{
        string groupName = "";
        List<int> keys = new List<int> ();
        prof = Profession.get ((JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession), GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel));
		bool isdeyi = false;
        foreach(int key in SkillData.GetAllData().Keys)
        {
            keys.Add(key);
        }
        for(int i =0;i<keys.Count;i++)
        {
            groupName = SkillData.GetMinxiLevelData(keys[i])._Name;
			isdeyi = prof.isProudSkill(GamePlayer.Instance.GetIprop(PropertyType.PT_Profession),keys[i],GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel));

            if(groupName.Equals("null"))
            {
                continue;
            }
			if(SkillData.GetMinxiLevelData(keys[i])._Level!= 1)
            {
                continue;
            }
			SkillData sdd = SkillData.GetMinxiLevelData(keys[i]);
			int lev = prof.getSkilMaxLevel(sdd._Id);
			if(lev==0)
			{
				continue;
			}
			if(isdeyi)
			{
				Pskillist.Add(SkillData.GetMinxiLevelData(keys[i]));
				//continue;
			}
			skillist.Add(SkillData.GetMinxiLevelData(keys[i]));
        }

        for(int i =0;i<skillist.Count;i++)
        {
            if(skillist[i]._LearnGroup == 1)
            {
                ATskillist.Add(skillist[i]);
            }
			if(skillist[i]._LearnGroup == 2)
            {
                AMskillist.Add(skillist[i]);
            }
			if(skillist[i]._LearnGroup == 3)
            {
                Sskillist.Add(skillist[i]);

            }
			if(skillist[i]._LearnGroup == 4)
            {
                Askillist.Add(skillist[i]);

            }
//            if(skillist[i].LearnGroup_ == 5)
//            {
//                Pskillist.Add(skillist[i]);
//
//            }
//            if(skillist[i].LearnGroup_ == 6)
//            {
//                Fskillist.Add(skillist[i]);
//            }
        }
        ATskillist.Sort (SortPSkill);
        AMskillist.Sort (SortPSkill);
        Sskillist.Sort (SortPSkill);
        Askillist.Sort (SortPSkill);

        ATskillist.Sort (SortSkill);
        AMskillist.Sort (SortSkill);
        Sskillist.Sort (SortSkill);
        Askillist.Sort (SortSkill);



        ButtonToSelect (4);
		AddItem (Pskillist);

	}
	void OnClickclose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	void OnClicjobButton(ButtonScript obj, object args, int param1, int param2)
	{
		ButtonToSelect (param1);
		Refresh ();
		AddItem (Pskillist);
	}
	void OnClicAttackSkill(ButtonScript obj, object args, int param1, int param2)
	{
		ButtonToSelect (param1);
		Refresh ();
		AddItem (ATskillist);
	}
	void OnClickAttackMagic(ButtonScript obj, object args, int param1, int param2)
	{
		ButtonToSelect (param1);
		Refresh ();
		AddItem (AMskillist);
	}
	void OnClickStatusMagic(ButtonScript obj, object args, int param1, int param2)
	{
		ButtonToSelect (param1);
		Refresh ();
		AddItem (Sskillist);
	}
	void OnClickAidSkill(ButtonScript obj, object args, int param1, int param2)
	{
		ButtonToSelect (param1);
		Refresh ();
		AddItem (Askillist);
	}
	void OnClickProduceSkill(ButtonScript obj, object args, int param1, int param2)
	{
		ButtonToSelect (param1);
		Refresh ();
		AddItem (Pskillist);
	}
	void OnClickForgingSkill(ButtonScript obj, object args, int param1, int param2)
	{
		ButtonToSelect (param1);
		Refresh ();
		AddItem (Fskillist);
	}
	void AddItem(List<SkillData> sd)
	{
		for(int i = 0;i<sd.Count;i++)
		{
			GameObject clone = GameObject.Instantiate(item)as GameObject;
			clone.SetActive(true);
			LearningCell lCell = clone.GetComponent<LearningCell>();
			lCell.SkpData = sd[i];
			clone.transform.parent = grid.transform;
			clone.transform.localPosition = Vector3.zero;
			clone.transform.localScale = Vector3.one;
			itemList.Add(clone);
			UIManager.SetButtonEventHandler (clone, EnumButtonEvent.OnClick, OnClickBtn, 0, 0);
			grid.repositionNow = true;
		}

		if (itemList.Count != 0&&itemList != null)
		{
			SkillInfo sinfo = infoObj.GetComponent<SkillInfo>();
			LearningCell lCell = itemList[0].GetComponent<LearningCell>();
			sinfo.SkpData = lCell.SkpData;
		}

        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainLearningClickTab, GamePlayer.Instance.GetIprop(PropertyType.PT_Profession));
	}

    public GameObject GetSkillObjByID(int skillId)
    {
        for (int i = 0; i < itemList.Count; ++i)
        {
            if (itemList[i].GetComponent<LearningCell>().SkpData._Id == skillId)
                return itemList[i];
        }
        return null;
    }

	void OnClickBtn(ButtonScript obj, object args, int param1, int param2)
	{
		SkillInfo sinfo = infoObj.GetComponent<SkillInfo>();
		LearningCell lCell = obj.GetComponent<LearningCell>();

		if(_curLearningCell != null )
		{
			_curLearningCell.raSp.gameObject.SetActive(false);
		}
		if(SkillInfo.SetBeginState != null)
		{
			if(_profData.getSkilMaxLevel(lCell.SkpData._Id)==0)
			{
				SkillInfo.SetBeginState(false);
			}else
			{
				SkillInfo.SetBeginState(true);
			}
		}
		_curLearningCell = lCell;
		lCell.raSp.gameObject.SetActive (true);

		sinfo.SkpData = lCell.SkpData;

        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainLearningOneSkillClick);
	}

	void Refresh()
	{
		if(grid == null)return;
		foreach(Transform tr in grid.transform)
		{
			Destroy(tr.gameObject);
		}
		itemList.Clear ();

	}
	void ButtonToSelect(int index)
	{
		for(int i = 0;i<btns.Count;i++)
		{
			if(i==index)
			{
				btns[i].isEnabled = false;
			}else
			{
				btns[i].isEnabled = true;
			}
		}
	}

	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS__LearningUI);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS__LearningUI);
	}
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS__LearningUI);
	}


	public bool isLearn(int sid)
	{
		for(int i=0;i<GamePlayer.Instance.SkillInsts.Count;i++)
		{
            if ((int)GamePlayer.Instance.SkillInsts[i].skillID_ == sid)
			{
				return true;
			}
		}
		return false;
	}

	private int SortSkill( SkillData a,SkillData b)
	{
		if (_profData.isProudSkill(_jobType, a._Id, _proflevel) && !_profData.isProudSkill(_jobType, b._Id, _proflevel))
		{
			return -1;
		}
		else if (_profData.isProudSkill(_jobType, a._Id, _proflevel) && _profData.isProudSkill(_jobType, b._Id, _proflevel))
		{
			return 0;
		}
		else if (!_profData.isProudSkill(_jobType, a._Id, _proflevel) && !_profData.isProudSkill(_jobType, b._Id, _proflevel))
		{
			return 0;
		}

		else
		{
			return 1;
		}
	}
	private int SortPSkill( SkillData a,SkillData b)
	{
		if (_profData.getSkilMaxLevel(a._Id) == 0 && _profData.getSkilMaxLevel(b._Id) != 0)
		{
			return 0;
		}
		else if (_profData.getSkilMaxLevel(a._Id) != 0 && _profData.getSkilMaxLevel(b._Id) == 0)
		{
			return 1;
		}
		else if (_profData.getSkilMaxLevel(a._Id) == 0 && _profData.getSkilMaxLevel(b._Id) == 0)
		{
			return -1;
		}else
		{
			return 1;
		}
	}
	public override void Destroyobj ()
	{
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_TopMoneyPanel, AssetLoader.EAssetType.ASSET_UI), true);
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS__LearningUI, AssetLoader.EAssetType.ASSET_UI), true);
	}
}
