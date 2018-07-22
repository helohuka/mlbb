using UnityEngine;
using System.Collections.Generic;

public class ScrollViewPanel : UIBase {
	public static ScrollViewPanel scrollViewP;
	public static int Skillid;
	public GameObject item;

	public UILabel zhiyeLabe;

	//public UISprite skillNameSp;
    private Entity _Caser; // 技能使用者
	public static int curMp;
	public GameObject closeBtn;
	public List<GameObject> items = new List<GameObject> ();
	private UIGrid grid;
	private ScrollViewPanel scrollView;
	private UIEventListener Listener;
	AttaclPanel attackPanel;
	void Awake()
	{
		scrollViewP = this;
	}
	public static ScrollViewPanel Instance{
		get{
			return scrollViewP;	
		}
	}
	void Start () {

		UIPanel panel = gameObject.GetComponent<UIPanel> ();
		panel.depth = 21;
		attackPanel = GameObject.Find ("AttackPanel").GetComponent<AttaclPanel>();
		scrollView = gameObject.GetComponent<ScrollViewPanel> ();		
		grid = gameObject.GetComponentInChildren<UIGrid> ();
		curMp = GamePlayer.Instance.GetIprop(PropertyType.PT_MpCurr);
        //InitData ();
		//item.SetActive (false);
		UIEventListener.Get (closeBtn).onClick = CloseClick;
		AttaclEvent.getInstance.SkillEvent = RefreshData;
		zhiyeLabe.text = Profession.get ((JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession), GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel)).jobName_;
        //Recovery();
	}

	void CloseClick(GameObject sender)
	{
		AttaclPanel.Instance.RecoverButtonStateNormal ();
		AttaclPanel.Instance.CloseScrollView ();
	}


	public  bool InitData ()
	{
        if(_Caser != null)
            AddScrollViewItem(_Caser.SkillInsts);
		return true;
	}

	public void  RefreshData(Entity caster)
	{
		for (int i = 0; i < items.Count; ++i)
		{
			PlayerSkillCell pc = items[i].GetComponent<PlayerSkillCell>();
			pc.clearObj();
		}
		//items.Clear ();
        _Caser = caster;
		InitData ();
	}

	void AddScrollViewItem(List<COM_Skill> skills)
	{
        UpdateListView(skills);
        for (int i = 0; i < Battle.Instance.disableLst_.Count; ++i)
        {
            SetSkillEnable(Battle.Instance.disableLst_[i]);
        }

        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_ClickBattleSkill);
	}

    void UpdateListView(List<COM_Skill> skills, bool registGuide = true)
    {
		List<COM_Skill> sdata = new List<COM_Skill>();
        for (int i = 0; i < items.Count; ++i)
        {
			PlayerSkillCell pc = items[i].GetComponent<PlayerSkillCell>();
			pc.clearObj();
        }
        int first = 0;
		for (int i = 0; i < skills.Count; i++)
		{
			SkillData skdata = SkillData.GetData((int)skills[i].skillID_,(int)skills[i].skillLevel_);
			if (skdata._SkillType != SkillType.SKT_Active && skdata._SkillType != SkillType.SKT_Passive)
				continue;
			sdata.Add(skills[i]);
		}
		for(int i=0;i<sdata.Count;i++)
		{
			SkillData skdat = SkillData.GetData((int)sdata[i].skillID_,(int)sdata[i].skillLevel_);
			PlayerSkillCell skillCell = items[i].GetComponent<PlayerSkillCell>();
			skillCell.SkData = sdata[i];
			items[i].name = skdat._Name;
			if (curMp < skdat._Cost_mana)
			{
				DisableItem( items[i], DisableType.DT_NoMana);
			}
			else if (skdat._SkillType == SkillType.SKT_CannotUse)
			{
				DisableItem( items[i], DisableType.DT_NoPassitive);
			}
			else
			{
				EnableItem( items[i],skdat._Id);
			}
			if (first == 0)
				GuideManager.Instance.RegistGuideAim( items[i], GuideAimType.GAT_FirstSkill);
			first++;
		}
		//			for (int i = 0; i < skills.Count; i++)
		//			{
		//				SkillData skdata = SkillData.GetData((int)skills[i].skillID_,(int)skills[i].skillLevel_);
		//				if (skdata._SkillType != SkillType.SKT_Active && skdata._SkillType != SkillType.SKT_Passive)
		//					continue;
		//				GameObject o = Instantiate(item) as GameObject;
		//				o.SetActive(true);
		//				o.transform.parent = grid.transform;
		//				PlayerSkillCell skillCell = o.GetComponent<PlayerSkillCell>();
		//				skillCell.SkData = skills[i];
		//				o.name = skdata._Name;
		//				if (curMp < skdata._Cost_mana)
		//				{
		//					DisableItem(o, DisableType.DT_NoMana);
		//				}
		//				else if (skdata._SkillType == SkillType.SKT_CannotUse)
		//				{
		//					DisableItem(o, DisableType.DT_NoPassitive);
		//				}
		//				else
		//				{
		//					EnableItem(o,skdata._Id);
		//				}
		//				o.transform.localPosition = new Vector3(0, 0, 0);
		//				o.transform.localScale = new Vector3(1, 1, 1);
		//				items.Add(o);
		//				grid.repositionNow = true;
		//				if (first == 0)
		//					GuideManager.Instance.RegistGuideAim(o, GuideAimType.GAT_FirstSkill);
		//				first++;
		//			}
    }

   

	public void SetSkillEnable(int index)
	{
		for (int i =0; i<items.Count; i++)
		{
			if(i==index)
			{
                DisableItem(items[i], DisableType.DT_NoWeapon);
			}
		}
	}
	public void Recovery()
	{
        UpdateListView(_Caser.SkillInsts);
	}

    void DisableItem(GameObject go, DisableType disableType)
    {
        UIButton btn = go.GetComponent<UIButton>();
        btn.enabled = false;
        PlayerSkillCell cell = go.GetComponent<PlayerSkillCell>();
		cell.huiSp.gameObject.SetActive (true);
        //cell.backSp.color = Color.gray;
        UIManager.RemoveButtonEventHandler(go, EnumButtonEvent.OnClick);
        UIManager.SetButtonEventHandler(go, EnumButtonEvent.OnClick, OnClickDisableBtn, (int)disableType, 0);
    }

    enum DisableType
    {
        DT_None,
        DT_NoMana,
        DT_NoWeapon,
        DT_NoPassitive,
    }

    private void OnClickDisableBtn(ButtonScript obj, object args, int param1, int param2)
    {
        DisableType dt = (DisableType)param1;
        switch(dt)
        {
            case DisableType.DT_NoMana:
                PopText.Instance.Show(LanguageManager.instance.GetValue("notEnoughMana"), PopText.WarningType.WT_Warning, true);
                break;
            case DisableType.DT_NoPassitive:
                PopText.Instance.Show(LanguageManager.instance.GetValue("isPassitiveSkill"), PopText.WarningType.WT_Warning, true);
                break;
            case DisableType.DT_NoWeapon:
                PopText.Instance.Show(LanguageManager.instance.GetValue("notCorrectWeaponType"), PopText.WarningType.WT_Warning, true);
                break;
            default:
                break;
        }
    }

    void EnableItem(GameObject go,int skid)
    {
        UIButton btn = go.GetComponent<UIButton>();
        btn.isEnabled = true;
        PlayerSkillCell cell = go.GetComponent<PlayerSkillCell>();
		cell.huiSp.gameObject.SetActive (false);
       // cell.backSp.color = Color.white;
        UIManager.RemoveButtonEventHandler(go, EnumButtonEvent.OnClick);
        UIManager.SetButtonEventHandler(go, EnumButtonEvent.OnClick, OnClickBtn, skid, 0);
    }
	
	private void OnClickBtn(ButtonScript obj, object args, int param1, int param2)
	{
        if (AttaclEvent.getInstance.SkillShowEvent != null)
            AttaclEvent.getInstance.SkillShowEvent(param1);
        attackPanel.closeSkillWindow();

	}
	public override void Destroyobj ()
	{
		GameObject.Destroy (gameObject);
	}
}
