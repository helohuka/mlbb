using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BabySkillLearning : UIBase {


	public UILabel _TitleLable;
	public UILabel _SkillDesLable;
	public UILabel _XiaoHaoLable;
	public UILabel _LearningLable;
	public UILabel _SkillAttackLable;
	public UILabel _AttackMLable;
	public UILabel _StateAttackLable;
	public UILabel _FZLable;




	public GameObject babySkillInfo;
	public GameObject babyListObj;
	public GameObject babyskillObj;
	public UIButton CloseBtn;
	public GameObject item;
	public UIGrid grid;
	public static int SkillId;
	public static int newLevel;
	public static int Price;
	public List<UIButton> btns = new List<UIButton> ();
	private List<SkillData> skillist = new List<SkillData> ();
	private List<int> lIds = new List<int> ();
	private List<GameObject> itemList = new List<GameObject> ();
	private List<SkillData> ATskillist = new List<SkillData> ();
	private List<SkillData> AMskillist = new List<SkillData> ();
	private List<SkillData> Sskillist = new List<SkillData> ();
	private List<SkillData> Fskillist = new List<SkillData> ();
	public static BabySkillLearning BabySkillLearn;
	private BabyLearningCell _curLearningCell;
	void Awake()
	{
		BabySkillLearn = this;
	}
	public static BabySkillLearning Instance
	{
		get
		{
			return BabySkillLearn;	
		}
	}

	void Start () { 
		item.SetActive (false);
		_TitleLable.text = LanguageManager.instance.GetValue("babyL_Title");
		_SkillDesLable.text = LanguageManager.instance.GetValue("babyL_SkillDes");
		_XiaoHaoLable.text = LanguageManager.instance.GetValue("babyL_XiaoHao");
		_LearningLable.text = LanguageManager.instance.GetValue("babyL_Learning");
		_SkillAttackLable.text = LanguageManager.instance.GetValue("babyL_SkillAttack");
		_AttackMLable.text = LanguageManager.instance.GetValue("babyL_AttackM");
		_StateAttackLable.text = LanguageManager.instance.GetValue("babyL_StateAttack");
		_FZLable.text = LanguageManager.instance.GetValue("babyL_FZ");
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		for(int i =0;i<btns.Count;i++)
		{
			UIManager.SetButtonEventHandler (btns[i].gameObject, EnumButtonEvent.OnClick, OnClickbtns, i, 0);
		}
        NpcData ndata = NpcData.GetData(NpcRenwuUI.NpcId);

		if(ndata.BabySkillLearn !="")
		{
		 string [] str = ndata.BabySkillLearn.Split(';');
			for(int i = 0;i<str.Length;i++)
			{
				lIds.Add(int.Parse(str[i]));
			}
		}

		for(int i = 0;i<lIds.Count;i++)
		{
            SkillData sData = SkillData.GetMinxiLevelData(lIds[i]);
			skillist.Add(sData);

		}
		for(int i =0;i<skillist.Count;i++)
		{
			if(skillist[i]._LearnGroup == 5)
			{
				ATskillist.Add(skillist[i]);
			}else
				if(skillist[i]._LearnGroup == 6)
			{
				AMskillist.Add(skillist[i]);
			}else
				if(skillist[i]._LearnGroup == 7)
			{
				Sskillist.Add(skillist[i]);
				
			}else
				if(skillist[i]._LearnGroup == 8)
			{
				Fskillist.Add(skillist[i]);
				
			}
		}

//		List<COM_Skill> skills = GamePlayer.Instance.skill_list_;
//		string groupName = "";
//		for(int i=0; i < skills.Count; ++i)
//		{
//			groupName = SkillData.GetData((int)skills[i].skillID_).groupName_;
//			if(groupName.Equals("null"))
//			{
//				continue;
//			}
//			skillist.Add(SkillData.GetData((int)skills[i].skillID_));
//		}
//		for (int j = 0; j<skillist.Count-1; j++) {
//			for (int k = skillist.Count-1; k>j; k--)
//			{
//				if(skillist[k].groupName_.Equals(skillist[j].groupName_))
//				{
//					skillist.RemoveAt(k);
//				}
//			}
//		}
		AddItem (ATskillist);
	}
	void OnClickbtns(ButtonScript obj, object args, int param1, int param2)
	{
		Refresh ();
		ButtonToSelect (param1);
		if(param1 == 0)
		{
			AddItem (ATskillist);
		}else
			if(param1 == 1)
		{
			AddItem (AMskillist);
		}else
			if(param1 ==2)
		{
			AddItem (Sskillist);
		}
		else
			if(param1 ==3)
		{
			AddItem (Fskillist);
		}
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
	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}

	void AddItem(List<SkillData> datas)
	{
		for(int i = 0;i<datas.Count;i++)
		{
			GameObject clone = GameObject.Instantiate(item)as GameObject;
			clone.SetActive(true);
			BabyLearningCell lCell = clone.GetComponent<BabyLearningCell>();
			lCell.SkpData = datas[i];
			clone.transform.parent = grid.transform;
			clone.transform.localPosition = Vector3.zero;
			clone.transform.localScale = Vector3.one;
			itemList.Add(clone);
			UIManager.SetButtonEventHandler (clone, EnumButtonEvent.OnClick, OnClickBtn, datas[i]._Id, datas[i]._Level);
            if (i == 0)
                GuideManager.Instance.RegistGuideAim(clone, GuideAimType.GAT_FirstLearningBabySkill);
		}
		grid.repositionNow = true;
		if (itemList.Count != 0&&itemList != null)
		{
			BabySkillInfo sinfo = babySkillInfo.GetComponent<BabySkillInfo>();
			BabyLearningCell lCell = itemList[0].GetComponent<BabyLearningCell>();
			sinfo.SkpData = lCell.SkpData;
			SkillId = lCell.SkpData._Id;
			newLevel = lCell.SkpData._Level;

		}
	}
	void OnClickBtn(ButtonScript obj, object args, int param1, int param2)
	{
		SkillId = param1;
		newLevel = param2;
		BabyLearningCell lCell = obj.GetComponent<BabyLearningCell>();
		BabySkillInfo info = babySkillInfo.GetComponent<BabySkillInfo>();
		if(_curLearningCell != null )
		{

			_curLearningCell.rasp.gameObject.SetActive(false);
		}		
		_curLearningCell = lCell;
		lCell.rasp.gameObject.SetActive (true);
		info.SkpData = lCell.SkpData;
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_ClickBabyLearningSkill);
	}


	public void CloseBabyListObj()
	{
		babyListObj.SetActive (false);
	}
	public void CloseBabyskillObj()
	{
		babyskillObj.SetActive (false);
	}
	public static void ShowMe()
	{
		if (!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_BabyLeranSkill))
			return;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS__BabySkillLearning);
	}
	public static void SwithShowMe()
	{
		if (!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_BabyLeranSkill))
						return;
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS__BabySkillLearning);
	}
	public override void Destroyobj ()
	{
		//AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS__BabySkillLearning, AssetLoader.EAssetType.ASSET_UI), true);
	}
}
