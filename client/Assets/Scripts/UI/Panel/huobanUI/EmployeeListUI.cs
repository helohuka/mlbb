using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmployeeListUI : UIBase
{
	public UIButton closeBtn;
	public UILabel nameLab;
	public UILabel decslab;
	public UILabel levelLab;
	public UILabel hpLab;
	public UILabel spiritLab;
	public UILabel magicLab;
	public UILabel critLab;
	public UILabel attLab;
	public UILabel hitLab;
	public UILabel defenseLab;
	public UILabel dodgeLab;
	public UILabel agileLab;
	public UILabel counterpunchLab;
	public UILabel replyLab;
	public UISprite qualityBg;
	public List<UISprite> starList = new List<UISprite> ();
	public Transform mpos;
	public UISprite qAddImg;
	public UISprite jobIcon;
	public UISprite pinzhiImg;
	public UIButton fireBtn;
	private GameObject babyObj;
	public UILabel profssLab;
	public UIButton inBattleBtn;
	public UILabel battleBtnLab;
	public UILabel pingfenLab;
	public EmployeeSkillLevelUpUI skillLevleUpUI;

	public List<UIButton> tabBtns = new List<UIButton> ();
	public List<GameObject> funObj = new List<GameObject> ();

	public GameObject infoObj;
	public GameObject equipObj;
	public UIButton equipBtn;
	public UISprite panelImg;
	public List<UISprite> skillIconList = new List<UISprite> ();
	private Employee curEmployee;
	private int  _selectEmployeeGroup;
	private List<string> _icons = new List<string>();
	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnCloseBtn, 0, 0);

		for(int i= 0;i<tabBtns.Count;i++)
		{
            if (i == 0)
            {
                // 注册基础分页
                GuideManager.Instance.RegistGuideAim(tabBtns[i].gameObject, GuideAimType.GAT_PartnerDetailBaseTab);
            }
			UIManager.SetButtonEventHandler (tabBtns[i].gameObject, EnumButtonEvent.OnClick, OnTabBtns, i, 0); 
		}
		//foreach(var x in skillIconList)
		//{
			//UIManager.SetButtonEventHandler (x.gameObject, EnumButtonEvent.OnClick, OnSkillLevelUp, 0, 0); 
		//}
		UIManager.SetButtonEventHandler (equipBtn.gameObject, EnumButtonEvent.OnClick, OnEquipBtn, 0, 0);
		UIManager.SetButtonEventHandler (inBattleBtn.gameObject, EnumButtonEvent.OnClick, OnInBattleBtn, 0, 0);
		equipBtn.isEnabled = false;
		ShowProp ();
		AddActorClone ();

        GamePlayer.Instance.EmployeeEvolveOkEnvent += new RequestEventHandler<Employee>(OnEmployeeEvolveOk);
		GamePlayer.Instance.EmployeeStarOkEnvent += new RequestEventHandler<Employee> (OnEmployeeStarOk);
		GamePlayer.Instance.UpdateEmployeeEnvent += new  EmpOnBattleEvent(OnUpdateBattle);
		skillLevleUpUI.callBack = OnHideSkill;


		//GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_PartnerDetailUIOpen);
	}
	

	
	#region Fixed methods for UIBase derived cass
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_EmployeeInfoPanel);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_EmployeeInfoPanel);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_EmployeeInfoPanel);
	}
	
	public override void Destroyobj ()
	{
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_TopMoneyPanel, AssetLoader.EAssetType.ASSET_UI), true);
		//AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_EmployeeInfoPanel, AssetLoader.EAssetType.ASSET_UI), true);
	}
	#endregion



	void AssetLoadCallBack(GameObject ro, ParamData data)
	{
        /*NGUITools.SetChildLayer(ro.transform, LayerMask.NameToLayer("3D"));
		ro.transform.parent = mpos;
		ro.transform.localScale = new Vector3(400f,400f,400f);
		ro.transform.localPosition = Vector3.zero;
		ro.transform.localRotation = Quaternion.Euler (10f, 180f, 0f);
		*/
		if(hasDestroy)
		{
			Destroy(ro);     
			PlayerAsseMgr.DeleteAsset((ENTITY_ID)data.iParam, false);
			return;
		}
		if (gameObject == null || !this.gameObject.activeSelf)
			return;
		if (babyObj != null)
		{ 
			Destroy(ro);
			PlayerAsseMgr.DeleteAsset((ENTITY_ID)data.iParam, false); 
			return;
		}
		ro.transform.parent = mpos;
		ro.transform.localPosition = Vector3.forward * -200f;
		ro.transform.localScale = new Vector3(400f,400f,400f);
		ro.transform.localRotation = Quaternion.Euler (0f, 180f, 0f);
		//EffectLevel el =ro.AddComponent<EffectLevel>();
		//el.target =ro.transform.parent.parent.GetComponent<UISprite>();

		babyObj = ro;
	}

	private void ShowProp()
	{
		curEmployee = EmployessSystem.instance.CurEmployee;
		if (curEmployee == null)
			return;

		if(GamePlayer.Instance.GetEmployeeIsBattle(curEmployee.InstId))
		{
			inBattleBtn.isEnabled = false;
			battleBtnLab.text = LanguageManager.instance.GetValue("yishangzhen");
		}
		else
		{
			inBattleBtn.isEnabled = true;
			battleBtnLab.text = LanguageManager.instance.GetValue("shangzhen");
		}


		nameLab.text = curEmployee.InstName;
		profssLab.text =  Profession.get((JobType)curEmployee.GetIprop(PropertyType.PT_Profession), 
		                                 curEmployee.GetIprop(PropertyType.PT_ProfessionLevel)).jobName_;
        decslab.text = EmployeeData.GetData(curEmployee.GetIprop(PropertyType.PT_TableId)).desc_;
		pingfenLab.text = curEmployee.GetIprop (PropertyType.PT_FightingForce).ToString();

		for(int i=0;i<curEmployee.SkillInsts.Count&&i<6;i++)
		{
			if(curEmployee.SkillInsts[i]== null)
			{
				UIManager.SetButtonEventHandler (skillIconList[i].gameObject, EnumButtonEvent.OnClick, OnSkillLevelUp, -1, 0); 
				continue;
			}
			SkillData sData  =SkillData.GetData((int)curEmployee.SkillInsts[i].skillID_,(int)curEmployee.SkillInsts[i].skillLevel_);
			if(sData == null)
				continue;

            if(i == 0)
            {
                // 注册第一个技能
                GuideManager.Instance.RegistGuideAim(skillIconList[i].gameObject, GuideAimType.GAT_PartnerDetailBaseFirstSkill);
            }

			if(sData._SkillType == SkillType.SKT_Active || sData._SkillType == SkillType.SKT_Passive ||sData._SkillType == SkillType.SKT_CannotUse)
			{
				skillIconList[i].gameObject.SetActive(true);
				skillIconList[i].name = curEmployee.SkillInsts[i].skillID_.ToString();
				HeadIconLoader.Instance.LoadIcon(SkillData.GetData((int)curEmployee.SkillInsts[i].skillID_,(int)curEmployee.SkillInsts[i].skillLevel_)._ResIconName,skillIconList[i].transform.Find("Sprite").GetComponent<UITexture>()) ;

				if(!_icons.Contains(SkillData.GetData((int)curEmployee.SkillInsts[i].skillID_,(int)curEmployee.SkillInsts[i].skillLevel_)._ResIconName))
				{
					_icons.Add(SkillData.GetData((int)curEmployee.SkillInsts[i].skillID_,(int)curEmployee.SkillInsts[i].skillLevel_)._ResIconName);
				}

				skillIconList[i].transform.Find("Sprite").GetComponent<UITexture>().gameObject.SetActive(true);
				skillIconList[i].transform.Find("Label").GetComponent<UILabel>().text  =LanguageManager.instance.GetValue("dengji")+": " +sData._Level.ToString();

				UIManager.SetButtonEventHandler (skillIconList[i].gameObject, EnumButtonEvent.OnClick, OnSkillLevelUp, (int)curEmployee.SkillInsts[i].skillID_, 0); 
			}
			else
			{
				HeadIconLoader.Instance.LoadIcon("skillSuo",skillIconList[i].transform.Find("Sprite").GetComponent<UITexture>()) ;

				if(!_icons.Contains("skillSuo"))
				{
					_icons.Add("skillSuo");
				}


				UIManager.SetButtonEventHandler (skillIconList[i].gameObject, EnumButtonEvent.OnClick, OnSkillLevelUp, -1, 0); 
			}
		}
		qAddImg.spriteName = LanguageManager.instance.GetValue(curEmployee.quality_.ToString());
		jobIcon.spriteName = ((JobType)curEmployee.GetIprop (PropertyType.PT_Profession)).ToString ();
		pinzhiImg.spriteName = getPinzhiImg ((int)curEmployee.quality_);
		for(int i =0;i<starList.Count;i++)
		{
			starList[i].gameObject.SetActive(false);
		}
		int len = (int)curEmployee.star_;
		if(curEmployee.star_ >=6)
		{
			len  = (int)curEmployee.star_- 5;
			for(int j =0;j<len && j<5;j++)
			{
				starList[j].spriteName = "zixingxing";
				starList[j].gameObject.SetActive(true);
			}
		}
		else
		{
			for(int j =0;j<curEmployee.star_ && j<5;j++)
			{
				starList[j].spriteName = "xingxing";
				starList[j].gameObject.SetActive(true);
			}
		}
		levelLab.text = curEmployee.GetIprop (PropertyType.PT_Level).ToString ();
		hpLab.text = curEmployee.GetIprop (PropertyType.PT_HpMax).ToString();
		spiritLab.text = curEmployee.GetIprop (PropertyType.PT_Spirit).ToString(); 
		magicLab.text = curEmployee.GetIprop (PropertyType.PT_MpMax).ToString();
		critLab.text = curEmployee.GetIprop (PropertyType.PT_Crit).ToString();
		attLab.text = curEmployee.GetIprop (PropertyType.PT_Attack).ToString();
		hitLab.text = curEmployee.GetIprop (PropertyType.PT_Hit).ToString();
		defenseLab.text = curEmployee.GetIprop (PropertyType.PT_Defense).ToString();
		dodgeLab.text = curEmployee.GetIprop (PropertyType.PT_Dodge).ToString();
		agileLab.text = curEmployee.GetIprop (PropertyType.PT_Agile).ToString();
		counterpunchLab.text = curEmployee.GetIprop (PropertyType.PT_counterpunch).ToString();
		replyLab.text = curEmployee.GetIprop (PropertyType.PT_Reply).ToString();


		//UpdateRed ();


	}

	private void AddActorClone()
	{
		if(babyObj != null && !babyObj.gameObject.activeSelf)
		{
			return;
		}
		if(babyObj != null)
		{
			Destroy (babyObj);
			babyObj = null;
		}
		GameManager.Instance.GetActorClone ((ENTITY_ID)EmployeeData.GetData (curEmployee.GetIprop (PropertyType.PT_TableId)).asset_id, 
								(ENTITY_ID)curEmployee.WeaponAssetID, EntityType.ET_Emplyee, AssetLoadCallBack, 
		                                    new ParamData (curEmployee.InstId,EmployeeData.GetData (curEmployee.GetIprop (PropertyType.PT_TableId)).asset_id),"UI");

		
	}
	


	void OnHideEvolve(int instId)
	{
		if(babyObj!= null)
		{
			babyObj.gameObject.SetActive (true);
		}

		curEmployee = GamePlayer.Instance.GetEmployeeById (instId); 
	}

	private void OnSkillLevelUp(ButtonScript obj, object args, int param1, int param2)
	{
		//int id = int.Parse (obj.name);
		if (param1 <=0 || SkillData.GetMinxiLevelData (param1) == null)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("jinnengweikaiqi"));
			return;
		}
	//	babyObj.gameObject.SetActive (false);
		skillLevleUpUI.Show ();
		skillLevleUpUI.EmpInstId = curEmployee.InstId;
		skillLevleUpUI.SkillId = param1;//int.Parse (obj.name);
	}

	private void OnCloseBtn(ButtonScript obj, object args, int param1, int param2)
	{

		Hide ();
	}

	protected override void DoHide ()
	{
		GamePlayer.Instance.EmployeeEvolveOkEnvent -= OnEmployeeEvolveOk;
		GamePlayer.Instance.EmployeeStarOkEnvent -= OnEmployeeStarOk;
		GamePlayer.Instance.UpdateEmployeeEnvent -= OnUpdateBattle;
		if(equipObj != null)
		{
			equipObj.GetComponent<EmployeeEquipUI>().ClosePanel();
		}
		base.DoHide ();
	}

	private void OnTabBtns(ButtonScript obj, object args, int param1, int param2)
	{
		foreach(UIButton x in tabBtns)
		{
			x.isEnabled = true;
		}
		equipBtn.isEnabled = true;
		foreach(GameObject o in funObj) 
		{
			o.SetActive(false);
		}
		infoObj.SetActive (true);
		if(equipObj != null)
		{
			equipObj.SetActive(false);
		}
		tabBtns [param1].isEnabled = false;
		funObj [param1].SetActive (true);

        GlobalInstanceFunction.Instance.Invoke(() =>
        {
            // 抛基础分页打开事件
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_PartnerDetailBaseOpen);
        }, 1);
	}
	private void OnEquipBtn(ButtonScript obj, object args, int param1, int param2)
	{
		foreach(UIButton x in tabBtns)
		{
			x.isEnabled = true;
		}
		equipBtn.isEnabled = false;
		infoObj.SetActive (false);
		if(equipObj == null)
		{
			LoadUI(UIASSETS_ID.UIASSETS_EmployeeEquipPanel);
		}
		else
		{
			equipObj.SetActive(true);
		}
	}

	private void OnInBattleBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if( GamePlayer.Instance.GetEmployeeIsBattle(curEmployee.InstId))
		{
			return;
		}

		List<Employee> emp = GamePlayer.Instance.GetBattleEmployees();

		for(int i =0;i<emp.Count;i++)
		{
			if(emp[i] == null)
				continue;
			if(emp[i].InstName ==curEmployee.InstName)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("samehuoban"));
				return;
			}
		}

		if(GamePlayer.Instance.GetBattleEmployees().Count>= 4 || ! EmployessSystem.instance.GetBattleEmpty())
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("meiyouweizhi"));
			return;
		}
		NetConnection.Instance.setBattleEmp((uint)curEmployee.InstId, EmployeesBattleGroup.EBG_GroupOne,true);
        NetWaitUI.ShowMe();
	}
	
	private void LoadUI(UIASSETS_ID id)
	{
		string uiResPath = GlobalInstanceFunction.Instance.GetAssetsName( (int)id , AssetLoader.EAssetType.ASSET_UI );
		
		AssetLoader.LoadAssetBundle (uiResPath, AssetLoader.EAssetType.ASSET_UI,(Assets,paramData)=> {
			if( null == Assets || null == Assets.mainAsset )
			{
				return ;
			}

			GameObject go = (GameObject)GameObject.Instantiate(Assets.mainAsset)as GameObject;
			if(hasDestroy)
			{
				Destroy(go);
				return;
			}
			if(this == null && !gameObject.activeSelf)
			{
				Destroy(go);
			}
			go.transform.parent = panelImg.transform;    
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one;
			
			equipObj = go;
		}
		, null);
	}


	void OnHideSkill(int instId)
	{
		babyObj.gameObject.SetActive (true);
	}

    void OnEmployeeEvolveOk(Employee inst)
	{
		curEmployee = inst;
	//	EmployessSystem.instance._OldEmployee = EmployessSystem.instance.CurEmployee;
		EmployeeJinjieOkUI.ShowMe (curEmployee.InstId);
		EmployessSystem.instance.CurEmployee = curEmployee;
		//funObj[1].GetComponent<EmployeeStarUpUI>().UpdateStarInfo();
		funObj[2].GetComponent<EmployeeEvolveUI>().UpdateEvolveInfo();
		PopText.Instance.Show (LanguageManager.instance.GetValue("jinjieok"));
		//EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_EquipCombie, gameObject.transform);
		ShowProp ();
	}
	
	void OnEmployeeStarOk(Employee inst)
	{


		curEmployee = inst;
		EmployessSystem.instance.CurEmployee = curEmployee;
		//funObj[1].GetComponent<EmployeeStarUpUI>().UpdateStarInfo();
		funObj[2].GetComponent<EmployeeEvolveUI>().UpdateEvolveInfo();
	//	PopText.Instance.Show (LanguageManager.instance.GetValue("shengxingok"));
		//EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_JinjieSuccess, gameObject.transform);
	//	EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_UI_employeeStarOk, gameObject.transform);
		ShowProp ();
	}


	public void OnUpdateBattle(Employee inst,int grop)
	{
        NetWaitUI.HideMe();

		if(inst.isForBattle_)
		{
			if(curEmployee.InstId == curEmployee.InstId)
			{
				inBattleBtn.isEnabled = false;
				battleBtnLab.text = LanguageManager.instance.GetValue("yishangzhen");
				//PopText.Instance.Show(LanguageManager.instance.GetValue("yishangzhen"));
			}
		}

		ShowProp ();
	}

	private string getPinzhiImg(int quality)
	{
		if ((int)quality >= (int)QualityColor.QC_Orange2)
		{
			return "shuxing_cheng";
		} 
		if((int)quality <= (int)QualityColor.QC_White)
		{
			return "shuxing_bai";
		}
		else if ((int)quality <= (int)QualityColor.QC_Green)
		{
			return "shuxing_lv";
		}
		else if((int)quality <= (int)QualityColor.QC_Blue1)
		{
			return "shuxing_lan";
		}
		else if ((int)quality <= (int)QualityColor.QC_Purple2)
		{
			return "shuxing_zi";
		}
		else if ((int)quality <= (int)QualityColor.QC_Golden2)
		{
			return "shuxing_huang";
		}
		else if ((int)quality <= (int)QualityColor.QC_Orange2)
		{
			return "shuxing_cheng";
		}
		else if ((int)quality <= (int)QualityColor.QC_Pink)
		{
			return "shuxing_fen";
		}
		return "";
	}


	public void UpdateRed()
	{
		//List<Employee> employees = GamePlayer.Instance.EmployeeList;
		
		
		uint employeeNum = curEmployee.soul_;
		/*foreach(var x in employees)
		{
			if (x.Properties[(int)PropertyType.PT_TableId] == curEmployee.Properties[(int)PropertyType.PT_TableId] && x.quality_ == curEmployee.quality_
			    && x.InstId != curEmployee.InstId)
			{
				employeeNum++;
			}
		}
		*/
		if((int)curEmployee.quality_ > (int)QualityColor.QC_Orange )
		{
			return;
		}
		int needNum = int.Parse(EmployeeData.GetData(curEmployee.GetIprop(PropertyType.PT_TableId)).evolutionNum[(int)curEmployee.quality_-1]);
		
		if(employeeNum >=  needNum)
		{
			//tabBtns[2].GetComponentInChildren<UISprite>().MarkOn(UISprite.MarkAnthor.MA_RightTop,-10,-15);
		}
		else
		{
			//tabBtns[2].GetComponentInChildren<UISprite>().MarkOff();
		}
	}

	bool hasDestroy;
	void OnDestroy()
	{
		hasDestroy = true;
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
		PlayerAsseMgr.DeleteAsset ((ENTITY_ID)EmployeeData.GetData (curEmployee.GetIprop (PropertyType.PT_TableId)).asset_id, false);
        if (equipObj != null)
        {
            //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_EmployeeEquipPanel, AssetLoader.EAssetType.ASSET_UI), true);
        }
	}



}

