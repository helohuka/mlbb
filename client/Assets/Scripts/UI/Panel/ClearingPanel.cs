using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ClearingPanel : UIBase {




	//public Transform roPos;
	public UIButton closeBtn;
//	public GameObject chongObj;
//
//	public UITexture raceIcon;
//	public UILabel moneyLabel;
//	public UITexture icon;
//	public UITexture cicon;
//	public UIGrid Skillgrid;
//	public GameObject skillItem;
//
//	public UIGrid itemgrid;
//	public GameObject iItem; 
//
//	public UILabel playerL;
//	public UILabel playerE;
//
//	public UILabel babyL;
//	public UILabel babyE;
//
//	public UISprite fuhaoSp;
//	//public UISprite textSp;
//	public UISprite levelup;
//	public UISprite clevelup;
	private List< COM_Skill> cskills = new List<COM_Skill>();
	private COM_DropItem[] item;
//	private string babyName;
//	private string PlayerName;
//	private string babyExp;
//	private string PlayerExp;
	List<ItemData> itemDa = new List<ItemData>();
	List<COM_Skill> skData = new List<COM_Skill>();


	public UITexture playerIcon;
	public UITexture babyIcon;
	public UITexture RaceIcon;
	public UISlider playerExp;
	public UISlider BabyExp;
	public UILabel playerExpLabel;
	public UILabel babyExpLabel;
	public UILabel MoneyLabel;
	public GameObject sItem;
	public GameObject dItem;
	public UIGrid dGrrid;
	public UIGrid sGrrid;
	public GameObject babykuang;
	void Awake()
	{
		//Battle.Instance.BattleReward.babyExp_
		//Battle.Instance.BattleReward.playerExp_
	}

	void Start () {

		sItem.SetActive (false);
		dItem.SetActive (false);
		if (GamePlayer.Instance.BattleBaby != null)
		{
			if(!GamePlayer.Instance.BattleBaby.isDead)
			{
				babykuang.SetActive(true);
				//babyExpLabel.text = "+"+ Battle.Instance.BattleBabyExp.ToString();
				HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData((int)GamePlayer.Instance.BattleBaby.Properties[(int)PropertyType.PT_AssetId]).assetsIocn_, babyIcon);
				HeadIconLoader.Instance.LoadIcon (BabyData.GetData((int)GamePlayer.Instance.BattleBaby.GetIprop(PropertyType.PT_TableId))._RaceIcon, RaceIcon);
                BabyExp.value = (float)GamePlayer.Instance.BattleBaby.GetIprop(PropertyType.PT_Exp) / ExpData.GetBabyMaxExp(GamePlayer.Instance.BattleBaby.GetIprop(PropertyType.PT_Level));
				babyExpLabel.text = Battle.Instance.BattleBabyExp.ToString();
				//StartCoroutine(NumScrollEffect((int)Battle.Instance.BattleBabyExp,babyExpLabel));
			}
		}else
		{
			babykuang.SetActive(false);
		}

		playerExpLabel.text = Battle.Instance.BattleReward.playExp_.ToString ();
		//StartCoroutine(NumScrollEffect((int)Battle.Instance.BattleReward.playExp_,playerExpLabel));
		MoneyLabel.text = Battle.Instance.BattleReward.money_.ToString ();
		//StartCoroutine(NumScrollEffect((int)Battle.Instance.BattleReward.money_,MoneyLabel));

		HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData((int)GamePlayer.Instance.Properties[(int)PropertyType.PT_AssetId]).assetsIocn_, playerIcon);

		long curExp = (long)GamePlayer.Instance.Properties [(int)PropertyType.PT_Exp];
		long macExp = ExpData.GetPlayerMaxExp (GamePlayer.Instance.GetIprop (PropertyType.PT_Level));
		long valueExp = curExp / macExp;
		playerExp.value=(float) valueExp;


//		levelup.gameObject.SetActive(false);
//		clevelup.gameObject.SetActive (false);
        hasDestroyed = false;
		//babyL.gameObject.SetActive (false);
		//playerL.gameObject.SetActive (false);
        GlobalInstanceFunction.Instance.Invoke(() => { OnClickclose(null, null, 0, 0); }, 5f);
		//skillItem.SetActive (false);
		//iItem.SetActive (false);
		bool isFlag = true;
		// Battle.Instance.BattleReward.skills_;

		for(int i = 0;i<Battle.Instance.BattleReward.skills_.Length;i++)
		{
			if(i<5)
			{
				cskills.Add(Battle.Instance.BattleReward.skills_[i]);
			}
				
		}

		List<COM_Skill> tmpsk = new List<COM_Skill>();
		for (int i = 0; i < cskills.Count; ++i) {
			if(tmpsk.Count == 0)
				tmpsk.Add(cskills[i]);
			else
			{
				for(int j = 0; j < tmpsk.Count; ++j)
				{
					if(tmpsk[j].skillID_ == cskills[i].skillID_)
					{
						tmpsk[j].skillExp_ += cskills[i].skillExp_;
						tmpsk[j].skillLevel_ = cskills[i].skillLevel_;
						isFlag = false;
					}
				}
				if(isFlag)
				{
					tmpsk.Add(cskills[i]);
					isFlag = true;
				}
			}
		}

		AddSkillItems (tmpsk);
		item = Battle.Instance.BattleReward.items_;
		AddPropsItems (item);
//			for(int i = 0 ;i <item.Length;i++)
//			{
//				ItemData idata = ItemData.GetData((int)item[i]);
//				itemDa.Add(idata);
//			}
//		    PlayerName = GamePlayer.Instance.InstName;
//			PlayerExp = Battle.Instance.BattleReward.playExp_.ToString ();
//			playerL.text = PlayerName;
//			playerE.text = PlayerExp;
//		   moneyLabel.text = Battle.Instance.BattleReward.money_.ToString ();
		    //HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData((int)GamePlayer.Instance.Properties[(int)PropertyType.PT_AssetId]).assetsIocn_, icon);
		   
		   
//		    if (GamePlayer.Instance.BattleBaby != null)
//			{		
//				if(!GamePlayer.Instance.BattleBaby.isDead)
//				{
//				    chongObj.gameObject.SetActive(true);
//					if(GamePlayer.Instance.BattleBaby.isLevelUp_)
//					{
//					clevelup.gameObject.SetActive(true);
//					    //PopText.Instance.Show (levelup.mainTexture);
//						EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_PlayerLvUpOnUI, transform);
//					}
//					babyL.gameObject.SetActive(true);
//					babyE.gameObject.SetActive(true);
//					fuhaoSp.gameObject.SetActive(true);
//					//textSp.gameObject.SetActive(true);
//					babyName = GamePlayer.Instance.BattleBaby.InstName;
//					babyExp = Battle.Instance.BattleBabyExp.ToString();
//                    Battle.Instance.BattleBabyExp = 0;
//					babyL.text = babyName;
//					babyE.text = babyExp;
//				HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData((int)GamePlayer.Instance.BattleBaby.Properties[(int)PropertyType.PT_AssetId]).assetsIocn_, cicon);
//				HeadIconLoader.Instance.LoadIcon (BabyData.GetData((int)GamePlayer.Instance.BattleBaby.GetIprop(PropertyType.PT_TableId)).RaceIcon_, raceIcon);
//			   }
//
//			}
//			else
//			{
//				babyL.gameObject.SetActive(false);
//				babyE.gameObject.SetActive(false);
//				fuhaoSp.gameObject.SetActive(false);
//			    chongObj.gameObject.SetActive(false);
//				
//			}
				
			

		GamePlayer.Instance.OpenSystemEnvetString += new RequestEventHandler<int> (UpdateOpenSystemStr);

           //GameManager.Instance.GetActorClone((ENTITY_ID)GamePlayer.Instance.Properties[(int)PropertyType.PT_AssetId], (ENTITY_ID)GamePlayer.Instance.WeaponAssetID, AssetLoadCallBack, new ParamData(GamePlayer.Instance.InstId), "UI");

		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickclose, 0, 0);

        GuideManager.Instance.RegistGuideAim(closeBtn.gameObject, GuideAimType.GAT_BattleRewardClose);

        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BattleOverRewardOpen);
	}

	int skillexp(List<COM_Skill> sks)
	{
		int sExp = 0;
		for(int i =0;i<sks.Count;i++)
		{
			sExp+= (int)sks[i].skillExp_;
		}
		return sExp;
	}


	void OnClickclose(ButtonScript obj, object args, int param1, int param2)
	{
        if (hasDestroyed)
            return;

		Battle.Instance.BattleReward = null;
		Hide ();
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BattleOverRewardClose);
	}

	public  void AddSkillItems(Dictionary<int,List<COM_Skill>> sks)
	{
		foreach(KeyValuePair<int,List<COM_Skill>> pair in sks)
		{
			GameObject o = GameObject.Instantiate(sItem)as GameObject;
			o.SetActive(true);
			o.transform.parent = sGrrid.transform;
			SkillData sdata = SkillData.GetMinxiLevelData(pair.Key);
			UITexture [] texs =  o.GetComponentsInChildren<UITexture>();
			foreach(UITexture te in texs)
			{
				if(te.name.Equals("skillIcon"))
				{
					HeadIconLoader.Instance.LoadIcon (sdata._ResIconName, te);
				}
			}
			UILabel []las = o.GetComponentsInChildren<UILabel>();
			foreach(UILabel la in las)
			{
				if(la.name.Equals("numLabel"))
				{
					la.text = skillexp(pair.Value).ToString();
					//StartCoroutine(NumScrollEffect(int.Parse(la.text),la));
				}
			}
			if(IsSkillLevelUp(pair.Value[0]))
			{
				EffectAPI.PlayUIEffect(EFFECT_ID.EFFECT_jinengshengji,o.transform);
			}
			o.transform.localPosition = new Vector3(0,0,0);
			o.transform.localScale= new Vector3(1,1,1);	
			sGrrid.repositionNow = true;
		}
	}

	public  void AddSkillItems(List<COM_Skill> sk)
	{
		for (int i = 0; i<sk.Count; i++) 
		{
			if(sk[i].skillExp_ <= 0)
				continue;
			GameObject o = GameObject.Instantiate(sItem)as GameObject;
			o.SetActive(true);
			o.name = o.name+i;
			SkillData sdata = SkillData.GetMinxiLevelData((int)sk[i].skillID_);
			UITexture [] texs =  o.GetComponentsInChildren<UITexture>();
			foreach(UITexture te in texs)
			{
				if(te.name.Equals("skillIcon"))
				{
					HeadIconLoader.Instance.LoadIcon (sdata._ResIconName, te);
				}
			}

			UILabel []las = o.GetComponentsInChildren<UILabel>();
			foreach(UILabel la in las)
			{
				if(la.name.Equals("numLabel"))
				{
					la.text = sk[i].skillExp_.ToString();
					//StartCoroutine(NumScrollEffect((int)sk[i].skillExp_,la));
				}
			}

			if(IsSkillLevelUp(sk[i]))
			{
				EffectAPI.PlayUIEffect(EFFECT_ID.EFFECT_jinengshengji,o.transform);
			}

			o.transform.parent = sGrrid.transform;
			o.transform.localPosition = new Vector3(0,0,0);
			o.transform.localScale= new Vector3(1,1,1);	
			sGrrid.repositionNow = true;
			
		}
	}
	static COM_Skill cSkill;
	bool IsSkillLevelUp(COM_Skill skill)
	{
		if(cSkill != null &&cSkill.skillID_ == skill.skillID_  )
		{
			if(cSkill.skillLevel_<skill.skillLevel_)
			{
				cSkill = skill;
				return true;
			}

		}
		cSkill = skill;
//		for(int i =0;i<GamePlayer.Instance.SkillInsts.Count;i++)
//		{
//			if(GamePlayer.Instance.SkillInsts[i].skillID_ == skill.skillID_ )
//			{
//				COM_Skill cSkill = GamePlayer.Instance.SkillInsts[i];
//				if(skill.skillLevel_>cSkill.skillLevel_)
//				{
//					return true;
//				}
//
//			}
//		}
		return false;
	}


	public  void AddPropsItems(COM_DropItem [] ida)
	{
		for (int i = 0; i<ida.Length; i++) {
			GameObject o = GameObject.Instantiate(dItem)as GameObject;
			o.SetActive(true);
			o.name = o.name+i;
			UILabel[] las = o.GetComponentsInChildren<UILabel>();
			//la.text = ida[i].itemNum_.ToString();
			UITexture sps = o.GetComponentInChildren<UITexture>();
			HeadIconLoader.Instance.LoadIcon (ItemData.GetData((int)ida[i].itemId_).icon_, sps);
			foreach(UILabel la in las)
			{
				if(la.gameObject.name.Equals("itemnumLabel"))
				{
					la.text = ida[i].itemNum_.ToString();
				}
				if(la.gameObject.name.Equals("itemnameLabel"))
				{
					la.text = ItemData.GetData((int)ida[i].itemId_).name_;
				}

			}
			UISprite sp = o.GetComponent<UISprite>();
			ItemData idd = ItemData.GetData((int)ida[i].itemId_);
			sp.spriteName = BagSystem.instance.GetQualityBack((int)idd.quality_);
			o.transform.parent = dGrrid.transform;
			o.transform.localPosition = new Vector3(0,0,0);
			o.transform.localScale= new Vector3(1,1,1);	
			dGrrid.repositionNow = true;
			
		}
	}

    bool hasDestroyed = false;
//	void AssetLoadCallBack(GameObject ro, ParamData data)
//	{
//        if (hasDestroyed)
//            return;
//
//		ro.transform.parent = roPos;
//		ro.transform.localPosition = Vector3.forward * 1000f;
//		ro.transform.localRotation = new Quaternion (ro.transform.localRotation.x,-180,ro.transform.localRotation.z,ro.transform.localRotation.w);
//		ro.transform.localScale = new Vector3 (ro.transform.localScale.x/2,ro.transform.localScale.y/2,ro.transform.localScale.z/2);
//        EffectLevel el = ro.AddComponent<EffectLevel>();
//        el.target = roPos.parent.GetComponent<UISprite>();
//
//        if (GamePlayer.Instance.isLevelUp_)
//		{
//			levelup.gameObject.SetActive(true);
//			//PopText.Instance.Show (levelup.mainTexture);
//			EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.LevelUpEffectId,transform);
//		}
//			
//	}

	public static void ShowMe()
	{
        if (Battle.Instance.BattleReward == null)
            return;

		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_ClearingPanel);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_ClearingPanel);
	}
	public override void Destroyobj ()
	{
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_ClearingPanel, AssetLoader.EAssetType.ASSET_UI), true);
		//Destroy (gameObject);
	}

	IEnumerator NumScrollEffect(int num,UILabel la)
	{
		int n = 0;
		if(num>500)
		{
			n=20;
		}else
		{
			if(num<10)
			{
				n = 1;
			}else
			{
				n=10;
			}

		}
		int i = 0;
		while(i<=num)
		{
			yield return new WaitForSeconds (0.002f);
			i+=n;
			if((i+=n)>=num)
			{
				la.text =num.ToString();
			}else
			{
				la.text =i.ToString();
			}
		}
//		for(int i =0;i<=num;i+=n)
//		{
//			yield return new WaitForSeconds (0.001f);
//			if((i+=n)>=num)
//			{
//				la.text =num.ToString();
//			}else
//			{
//				la.text =i.ToString();
//			}
//
//		}

	}


	public void UpdateOpenSystemStr(int str)
	{
        if (MainPanle.Instance == null)
            return;

		if(MainPanle.Instance.gameObject.activeSelf)
		{
			return;
		}
		if(str == (int)OpenSubSystemFlag.OSSF_EmployeePos10 || str == (int)OpenSubSystemFlag.OSSF_EmployeePos15 ||str == (int)OpenSubSystemFlag.OSSF_EmployeePos20)
		{
			EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_kaiqihuoban, gameObject.transform,()=>{});
		}
	}

    void OnDestroy()
    {
        hasDestroyed = true;
		if(TeamSystem.isBattleOpen)
		{
			TeamSystem.BackTeam();
		}
		if(TeamSystem.isYQ)
		{
			NetConnection.Instance.jointLobby ();
			TeamSystem.isYQ = false;
		}
		GamePlayer.Instance.OpenSystemEnvetString -= UpdateOpenSystemStr;
	}

	
}
