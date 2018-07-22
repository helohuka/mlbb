using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AttaclPanel : UIBase {

	public GameObject mpObj;
	public GameObject hpObj;
	public GameObject levelObj;
	public UIButton teamBtn;
	public UIButton zihyinBtn;
	public UIButton renwuBtn;
	public UIButton skilliBtn;
	//public UIButton bagaBtn;
	public UIButton shopBtn;
	public UIButton famBtn;
	public UIButton huodongBtn;
	public UIButton jincaBtn;

	public UITexture SkillBack;
	public UITexture SkillName;
	public UITexture Icon;

	public GameObject babyHUD;
	public UITexture babyRaceT;
	public UITexture babyIconT;
	public UILabel babyNameLable;
	public UILabel babyHpLable;
	public UILabel babyMpLable;
	public UISlider babympSlider;
	public UISlider babyhpSlider;

	public GameObject huiheObj;
	public GameObject skillView;
	public GameObject scrollViewTwo;
	public GameObject petScrollView;
	public GameObject babyScrollView;
	public GameObject HUD;
	public GameObject CountDownG;
	public UISprite minSp;
	public UISprite secondSp;
	public UISlider Hp;
	public UISlider Mp;
	public UISprite attackNumSp;
	public UITexture palyIcon;

	//public UISprite BYCbackGround;
	public List<GameObject> EntityObjs = new List<GameObject> ();
	public UIButton backBtn;
	public UIButton	PlayerBackBtn;

	public UIButton bagBtn;

    public UILabel catchItemNum;
    public UIButton catchBtn;
	public UIButton attackBtn;
	public UIButton skillBtn;
	public UIButton DefenseBtn;
	public UIButton PosBtn;
	public UIButton AUTOBtn;
	public UIButton ArticleBtn;
	public UIButton petBtn;
	public UIButton fleeBtn;
	public UIButton CanAUTOBtn;

    public UIButton playerActionBtn;
    public UIButton petActionBtn;

	public UILabel hpLabel;
	public UILabel mpLabel;
	public UILabel levelLabel;
	public GameObject selectedSkill;
    public UILabel selectedSkillName;
    public UISprite whoTurn;
	public UILabel playernanmeLabel;

    public TogetherHitControl togetherHitCrl_;
    public UISprite battleTurn_ge;
    public UISprite battleTurn_shi;

    public UISprite attackType_;

    public UIButton speedUpBtn_;

    public GameObject AutoPlayerPanel;
    public GameObject AutoPetPanel;
	public GameObject playerIcon;
	public GameObject babyIcon;


	private List<UIButton> buttons = new List<UIButton> ();
	private bool isEnityUIMove;
	private	List<GameObject>	lstEntityUI;
	private string[] iconNames = {"0-0","1-1","2-2","3-3","4-4","5-5","6-6","7-7","8-8","9-9"};
	private string[] nums = {"tf_huang0","tf_huang1","tf_huang2","tf_huang3","tf_huang4","tf_huang5","tf_huang6","tf_huang7","tf_huang8","tf_huang9"};
	private string[] hnums = {"tf_hong0","tf_hong1","tf_hong2","tf_hong3","tf_hong4","tf_hong5","tf_hong6","tf_hong7","tf_hong8","tf_hong9"};
	private string[] countDowndecade = {"djs_tf_huang0","djs_tf_huang1","djs_tf_huang2","djs_tf_huang3","djs_tf_huang4","djs_tf_huang5","djs_tf_huang6","djs_tf_huang7","djs_tf_huang8","djs_tf_huang9"};
	private string[] countDowndtheUnit = {"djs_tf_hong0","djs_tf_hong1","djs_tf_hong2","djs_tf_hong3","djs_tf_hong4","djs_tf_hong5","djs_tf_hong6","djs_tf_hong7","djs_tf_hong8","djs_tf_hong9"};
	private string[] numbers = {"yidong","erdong"};
	private int endTime;
    private float preTimeStamp;
	private float currentTime;
	private int startTime =29;
	private int second;
	private bool isCountDown;
	public static AttaclPanel AttackP;
	private bool isDonw;
	private Hashtable btnHas = new Hashtable ();
	private Hashtable btnDHas = new Hashtable ();

	public const string SKILL_BTN = "SkillButton";
	public const string ATTACK_BTN = "AttackButton";
	public const string PET_BTN = "PetButton";
	public const string DEFENSE_BTN = "DefenseButton";
	public const string POSITION_BTN = "PositionButton";
	public const string FLEE_BTN = "fleeButton";
	public const string AUTO_BTN = "AUTOButton";
	public const string ARTICLE_BTN = "ArticleButton";
    public const string CATCH_BTN = "CatchButton";

	public GameObject playeroBJ;

    const string speedUpImg = "jiasuguanbi";
    const string normalImg = "jiasukaiqii";


    public delegate void SpeedUpHandler(bool on);
    public event SpeedUpHandler OnSpeedUp;
	string tempstr="";
    bool hasDestroy = false;

	void Awake()
	{
		AttackP = this;
	}
	public static AttaclPanel Instance
	{
		get
		{
			return AttackP;	
		}
	}
	void Start ()
	{
		int mucId = 0;
		if(Battle.Instance.battleType == BattleType.BT_PVE)
		{
			if(Battle.Instance.BattleID==0)
			{
				GlobalValue.Get(Constant.C_BossMuc, out mucId);
				SoundTools.PlayMusic((MUSIC_ID)mucId);
			}else
			{
				GlobalValue.Get(Constant.C_PutnMuc, out mucId);
				SoundTools.PlayMusic((MUSIC_ID)mucId);
			}
		}else
			if(Battle.Instance.battleType == BattleType.BT_PVP)
		{
			GlobalValue.Get(Constant.C_PvpMuc, out mucId);
			SoundTools.PlayMusic((MUSIC_ID)mucId);
		}
        HideAttackType();
		levelLabel.text = GamePlayer.Instance.Properties [(int)PropertyType.PT_Level].ToString ();
		HeadIconLoader.Instance.LoadIcon ("R_"+EntityAssetsData.GetData ((int)GamePlayer.Instance.Properties [(int)PropertyType.PT_AssetId]).assetsIocn_, palyIcon);
		HUD.SetActive (false);
		attackNumSp.transform.gameObject.SetActive (false);
		closeSkillWindow ();
		closeSkillTwoWindow ();
		ClosePetWindow ();
		lstEntityUI = new List<GameObject>();
		GameObject pos = GameObject.Find ("CenterPoint");
        if (pos == null)
        {
			return;
        }

		CountDownG.transform.localPosition = GlobalInstanceFunction.WorldToUI (pos.transform.position);
	    tempstr = playernanmeLabel.text;
        BagSystem.instance.UpdateItemEvent += OnItemUpdate;
        BagSystem.instance.DelItemInstEvent += OnItemDelete;
        BagSystem.instance.ItemChanged += OnItemAdd;
		BagSystem.instance.BattleOpenBagEvent  += new RequestEventHandler<bool>(OpenBagEvent);
        UIManager.Instance.showMainPanelEnvent += new RequestEventHandler<bool>(AttackPanelShow);

        UpdateItemCount();

		buttons.Add (attackBtn);
		buttons.Add (skillBtn);
		buttons.Add (DefenseBtn);
		buttons.Add (PosBtn);
		buttons.Add (AUTOBtn);
		buttons.Add (ArticleBtn);
		buttons.Add (petBtn);
		buttons.Add (fleeBtn);
		buttons.Add(catchBtn);	


		UIManager.SetButtonEventHandler (teamBtn.gameObject, EnumButtonEvent.OnClick, teamBtnClick, 0, 0);
		UIManager.SetButtonEventHandler(zihyinBtn.gameObject, EnumButtonEvent.OnClick, zihyinBtnClick, 0, 0);
		UIManager.SetButtonEventHandler(renwuBtn.gameObject, EnumButtonEvent.OnClick, OnClickrenwuBtn, 0, 0);
		UIManager.SetButtonEventHandler (skilliBtn.gameObject, EnumButtonEvent.OnClick, OnClickskilliBtn, 0, 0);
		//UIManager.SetButtonEventHandler (bagaBtn.gameObject, EnumButtonEvent.OnClick, OnClickbagaBtn, 0, 0);
		UIManager.SetButtonEventHandler (shopBtn.gameObject, EnumButtonEvent.OnClick, OnClickshopBtn, 0, 0);
		UIManager.SetButtonEventHandler (famBtn.gameObject, EnumButtonEvent.OnClick, OnClickfamBtn, 0, 0);
		UIManager.SetButtonEventHandler (huodongBtn.gameObject, EnumButtonEvent.OnClick, OnClickhuodongBtn, 0, 0);
		UIManager.SetButtonEventHandler (jincaBtn.gameObject, EnumButtonEvent.OnClick, OnClickjincaBtn, 0, 0);

		UIManager.SetButtonEventHandler (backBtn.gameObject, EnumButtonEvent.OnClick, BackClick, 0, 0);
		UIManager.SetButtonEventHandler(PlayerBackBtn.gameObject, EnumButtonEvent.OnClick, PlayerBackClick, 0, 0);
        UIManager.SetButtonEventHandler(catchBtn.gameObject, EnumButtonEvent.OnClick, OnClickCatchBtn, 0, 0);
		UIManager.SetButtonEventHandler (attackBtn.gameObject, EnumButtonEvent.OnClick, OnClickattackBtn, 0, 0);
		UIManager.SetButtonEventHandler (skillBtn.gameObject, EnumButtonEvent.OnClick, OnClickskillBtn, 0, 0);
		UIManager.SetButtonEventHandler (DefenseBtn.gameObject, EnumButtonEvent.OnClick, OnClickDefenseBtn, 0, 0);
		UIManager.SetButtonEventHandler (PosBtn.gameObject, EnumButtonEvent.OnClick, OnClickPosBtn, 0, 0);
		UIManager.SetButtonEventHandler (AUTOBtn.gameObject, EnumButtonEvent.OnClick, OnClickAUTOBtn, 0, 0);
		UIManager.SetButtonEventHandler (ArticleBtn.gameObject, EnumButtonEvent.OnClick, OnClickArticleBtn, 0, 0);
		UIManager.SetButtonEventHandler (petBtn.gameObject, EnumButtonEvent.OnClick, OnClickpetBtn, 0, 0);
		UIManager.SetButtonEventHandler (fleeBtn.gameObject, EnumButtonEvent.OnClick, OnClickfleeBtn, 0, 0);
		UIManager.SetButtonEventHandler (CanAUTOBtn.gameObject, EnumButtonEvent.OnClick, OnClickCanAUTO, 0, 0);
        UIManager.SetButtonEventHandler(speedUpBtn_.gameObject, EnumButtonEvent.OnClick, OnSpeedUpBtn, 0, 0);
        UIManager.SetButtonEventHandler(playerActionBtn.gameObject, EnumButtonEvent.OnClick, OnPlayerActionBtn, 0, 0);
        UIManager.SetButtonEventHandler(petActionBtn.gameObject, EnumButtonEvent.OnClick, OnPetActionBtn, 0, 0);
		UIManager.SetButtonEventHandler(babyHUD.gameObject, EnumButtonEvent.OnClick, OnShowBaby, 0, 0);
		UIEventListener.Get (playeroBJ).onClick = OnOpenPlayer;
        AutoPlayerPanel.GetComponent<ChangeAutoOrder>().operatType_ = OperateType.OT_P1;
        AutoPetPanel.GetComponent<ChangeAutoOrder>().operatType_ = OperateType.OT_B;

		curHp = GamePlayer.Instance.GetIprop(PropertyType.PT_HpCurr);
		curMp = GamePlayer.Instance.GetIprop(PropertyType.PT_MpCurr);

        if (GamePlayer.Instance.BattleBaby != null)
        {
            curHpbb = GamePlayer.Instance.BattleBaby.GetIprop(PropertyType.PT_HpCurr);
            curMpbb = GamePlayer.Instance.BattleBaby.GetIprop(PropertyType.PT_MpCurr);
        }
        
        speedUpBtn_.normalSprite = Battle.Instance.speedUpOn_ ? speedUpImg : normalImg;
		if(GuildSystem.IsInGuild())
		{
			famBtn.isEnabled = true;
			UISprite sp = famBtn.GetComponentInChildren<UISprite>();
			sp.color = new Color(255,255,255);
		}else
		{
			UISprite sp = famBtn.GetComponentInChildren<UISprite>();
			sp.color = new Color(0,255,0);
			famBtn.isEnabled = false;
		}
		if(TeamSystem.IsInTeam())
		{
			UISprite sp = teamBtn.GetComponentInChildren<UISprite>();
			sp.color = new Color(255,255,255);
			teamBtn.isEnabled = true;
		}else
		{
			UISprite sp = teamBtn.GetComponentInChildren<UISprite>();
			sp.color = new Color(0,255,0);
			teamBtn.isEnabled = false;
		}
		if(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Shop))
		{
			UISprite sp = shopBtn.GetComponentInChildren<UISprite>();
			sp.color =  new Color(255,255,255);
			shopBtn.isEnabled = true;
		}else
		{
			UISprite sp = shopBtn.GetComponentInChildren<UISprite>();
			sp.color =  new Color(0,255,0);
			shopBtn.isEnabled = false;
		}
		if(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Guid))
		{
			UISprite sp = zihyinBtn.GetComponentInChildren<UISprite>();
			sp.color =  new Color(255,255,255);
			zihyinBtn.isEnabled = true;
		}else
		{
			UISprite sp = zihyinBtn.GetComponentInChildren<UISprite>();
			sp.color =  new Color(0,255,0);
			zihyinBtn.isEnabled = false;
		}
		if(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Activity))
		{
			UISprite sp = huodongBtn.GetComponentInChildren<UISprite>();
			sp.color =  new Color(255,255,255);
			huodongBtn.isEnabled = true;
		}else
		{
			UISprite sp = huodongBtn.GetComponentInChildren<UISprite>();
			sp.color =  new Color(0,255,0);
			huodongBtn.isEnabled = false;
		}
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level) >= GamePlayer.Instance.openADActivityLv)
		{
			UISprite sp = jincaBtn.GetComponentInChildren<UISprite>();
			if ( GamePlayer.Instance.adTypes.Count == 0)
			{
				sp.color =  new Color(0,255,0);
				jincaBtn.isEnabled = false;

			}
			
			if ( GamePlayer.Instance.adTypes.Count > 0)
			{
				sp.color =  new Color(255,255,255);
				jincaBtn.isEnabled = true;
			}
		}else
		{
			UISprite sp = jincaBtn.GetComponentInChildren<UISprite>();
			sp.color =  new Color(0,255,0);
			jincaBtn.isEnabled = false;

		}
        GuideManager.Instance.RegistGuideAim(attackBtn.gameObject, GuideAimType.GAT_BattleAttack);
        GuideManager.Instance.RegistGuideAim(skillBtn.gameObject, GuideAimType.GAT_BattleSkill);
        GuideManager.Instance.RegistGuideAim(AUTOBtn.gameObject, GuideAimType.GAT_BattleAuto);
        GuideManager.Instance.RegistGuideAim(ArticleBtn.gameObject, GuideAimType.GAT_BattleBag);
        GuideManager.Instance.RegistGuideAim(catchBtn.gameObject, GuideAimType.GAT_BattleCatch);
        GuideManager.Instance.RegistGuideAim(playerActionBtn.gameObject, GuideAimType.GAT_PlayerAuto);
		GlobalInstanceFunction.Instance.UpdatecounDownTime += attackEvent;
        if (GuideManager.Instance.InBattleGuide_)
            CloseCountDown();

        AttaclEvent.getInstance.OnSetPanelActive += SetSelfActive;
		huiheObj.SetActive (false);
        hasDestroy = false;
		SetBabyInfo ();
	}
	private void OnShowBaby(ButtonScript obj, object args, int param1, int param2)
	{
		MainBabyPanle.SwithShowMe ();
		gameObject.SetActive (false);
	}
	public UISprite typeBabySp;
	public void SetBabyInfo()
	{
        BattleActor battbaby = Battle.Instance.SelfActorBattleBaby;
		if(battbaby == null)
		{
			babyHpLable.text = "";
			babyMpLable.text = "";
			babyhpSlider.value = 0;
			babympSlider.value = 0;
			babyNameLable.text = "";
//			babyRaceT.mainTexture = null;
			babyIconT.mainTexture = null;
			typeBabySp.gameObject.SetActive(false);
			mpObj.SetActive(false);
			hpObj.SetActive(false);
			levelObj.SetActive(false);
		}else
		{
			//HeadIconLoader.Instance.LoadIcon (BabyData.GetData(GamePlayer.Instance.BattleBaby.GetIprop(PropertyType.PT_TableId))._RaceIcon, babyRaceT);
			mpObj.SetActive(true);
			hpObj.SetActive(true);
			levelObj.SetActive(true);
            HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData(BabyData.GetData(GamePlayer.Instance.BattleBaby.GetIprop(PropertyType.PT_TableId))._AssetsID).assetsIocn_, babyIconT);
            babyNameLable.text = GamePlayer.Instance.BattleBaby.GetIprop(PropertyType.PT_Level).ToString();
			BabyData bdata = BabyData.GetData(GamePlayer.Instance.BattleBaby.GetIprop(PropertyType.PT_TableId));
			typeBabySp.gameObject.SetActive(true);
			typeBabySp.spriteName  = bdata._Tpye.ToString();
			int crtHp =battbaby.battlePlayer.hpCrt_;
            int crtMp = battbaby.battlePlayer.mpCrt_;
            int maxHp = battbaby.battlePlayer.hpMax_;
            int maxMp = battbaby.battlePlayer.mpMax_;
			babyHpLable.text = crtHp + "/" + maxHp;
			babyMpLable.text = crtMp + "/" +maxMp;
            babyhpSlider.value = crtHp * 1f / maxHp * 1f;
            babympSlider.value = crtMp * 1f / maxMp * 1f;
		}
		
		
	}
	void SetSelfActive(bool istrue)
	{
		gameObject.SetActive (istrue);
//		closeSkillWindow ();
//		closeSkillTwoWindow ();
//		CloseBabyWindow();
	}
	public void SetSkillIcon(int skillId,int skillLv,int assId)
	{
		SkillData sdat = SkillData.GetData(skillId,skillLv);

		if (sdat._SkillType == SkillType.SKT_Active) {
			EntityAssetsData enData = EntityAssetsData.GetData (assId);
			HeadIconLoader.Instance.LoadIcon (sdat._SkillBack, SkillBack);
			HeadIconLoader.Instance.LoadIcon (sdat._SkillIconTex, SkillName);
			HeadIconLoader.Instance.LoadIcon (enData._skillhard, Icon);
			
			TweenPosition tp = SkillBack.GetComponent<TweenPosition> ();
			tp.enabled = false;
			tp.delay = 0.5f;
			tp.from = new Vector3 (278,226,0);
			tp.to = new Vector3 (-279,226,0);
			tp.enabled = true;
			GlobalInstanceFunction.Instance.Invoke (()=>{
				tp.enabled = true;
				tp.delay = 1.2f;
				tp.from = new Vector3 (-279,226,0); 
				tp.to = new Vector3 (278,226,0);
			},1.2f);
		}



	}
	public void attackEvent()
	{
		if (isCountDown) 
		{
			Countdown();		
		}
	}
    public void InitData(float crtHp, float crtMp, float maxHp, float maxMp)
    {
        hpLabel.text = (int)crtHp + "/" + (int)maxHp;
        mpLabel.text = (int)crtMp + "/" + (int)maxMp;

        Hp.value = crtHp / maxHp;
        Mp.value = crtMp / maxMp;
    }

	private int curHp;
	private int curMp;
	public void ChangeValue(PropertyType type,int val, int hpMax, int mpMax)
	{
		if(type.Equals(PropertyType.PT_HpCurr))
		{
			curHp =curHp +val;
			if(curHp<=0)
			{
				curHp= 0;
			}
				if(curHp>hpMax)
				{
					curHp = hpMax;
				}

			hpLabel.text = curHp + "/" + hpMax;
			BloodChanged(val, hpMax);
		}
		else if(type.Equals(PropertyType.PT_MpCurr))
		{
			curMp= curMp + val;
			if(curMp<=0)
			{
				curMp = 0;
			}
			if(curMp>mpMax)
			{
				curMp = mpMax;
			}
			mpLabel.text = curMp + "/" + mpMax;
			MagicChanged(val, mpMax);
		}
	}

    private int curHpbb;
    private int curMpbb;
    public void ChangeValueBaby(PropertyType type, int val, int hpMax, int mpMax)
    {
        if (type.Equals(PropertyType.PT_HpCurr))
        {
            curHpbb = curHpbb + val;
            if (curHpbb <= 0)
            {
                curHpbb = 0;
            }
            if (curHpbb > hpMax)
            {
                curHpbb = hpMax;
            }

            babyHpLable.text = curHpbb + "/" + hpMax;
            BloodChangedBaby(val, hpMax);
        }
        else if (type.Equals(PropertyType.PT_MpCurr))
        {
            curMpbb = curMpbb + val;
            if (curMpbb <= 0)
            {
                curMpbb = 0;
            }
            if (curMpbb > mpMax)
            {
                curMpbb = mpMax;
            }
            babyMpLable.text = curMpbb + "/" + mpMax;
            MagicChangedBaby(val, mpMax);
        }
    }

    void UpdateItemCount()
    {
        int itemID = 0;
        GlobalValue.Get(Constant.C_CatchPetItemID, out itemID);
        int num = BagSystem.instance.GetItemMaxNum((uint)itemID);
        catchItemNum.GetComponent<UILabel>().text = string.Format("[b]{0}[-]", num.ToString());
    }

    void OnItemAdd(COM_Item inst)
    {
        int itemID = 0;
        GlobalValue.Get(Constant.C_CatchPetItemID, out itemID);
        if (inst.itemId_ == itemID)
        {
            int num = BagSystem.instance.GetItemMaxNum((uint)itemID);
            catchItemNum.GetComponent<UILabel>().text = string.Format("[b]{0}[-]", num.ToString());
        }
    }

    void OnItemUpdate(COM_Item inst)
    {
        int itemID = 0;
        GlobalValue.Get(Constant.C_CatchPetItemID, out itemID);
        if (inst.itemId_ == itemID)
        {
            int num = BagSystem.instance.GetItemMaxNum((uint)itemID);
            catchItemNum.GetComponent<UILabel>().text = string.Format("[b]{0}[-]", num.ToString());
        }
    }

    void OnItemDelete(COM_Item inst)
    {
        int itemID = 0;
        GlobalValue.Get(Constant.C_CatchPetItemID, out itemID);
        if (inst.itemId_ == itemID)
        {
            int num = BagSystem.instance.GetItemMaxNum((uint)itemID);
            catchItemNum.GetComponent<UILabel>().text = string.Format("[b]{0}[-]", num.ToString());
        }
    }

	void OpenBagEvent(bool isOpen)
	{
		if(!isOpen)
		{
			gameObject.SetActive(true);
		}
	}

    void AttackPanelShow(bool bshow)
    {
        gameObject.SetActive(bshow);
    }


	private void teamBtnClick(ButtonScript obj, object args, int param1, int param2)
	{
		TeamUI.SwithShowMe ();
	}
	private void zihyinBtnClick(ButtonScript obj, object args, int param1, int param2)
	{
		HelpUI.SwithShowMe ();
	}
	private void OnClickrenwuBtn(ButtonScript obj, object args, int param1, int param2)
	{
		TaskUI.SwithShowMe ();
	}
	private void OnClickskilliBtn(ButtonScript obj, object args, int param1, int param2)
	{
		SkillViewUI.SwithShowMe ();
	}
//	private void OnClickbagaBtn(ButtonScript obj, object args, int param1, int param2)
//	{
//		BagUI.SwithShowMe ();
//	}
	private void OnClickshopBtn(ButtonScript obj, object args, int param1, int param2)
	{
		StoreUI.SwithShowMe ();
	}
	private void OnClickfamBtn(ButtonScript obj, object args, int param1, int param2)
	{
		MyFamilyInfo.SwithShowMe ();
	}
	private void OnClickhuodongBtn(ButtonScript obj, object args, int param1, int param2)
	{
		HongDongPanel.SwithShowMe ();
	}
	private void OnClickjincaBtn(ButtonScript obj, object args, int param1, int param2)
	{
		MoreActivity.SwithShowMe ();
	}
    private void OnClickCatchBtn(ButtonScript obj, object args, int param1, int param2)
    {
        int itemID = 0;
        int shopID = 0;
        GlobalValue.Get(Constant.C_CatchPetItemID, out itemID);
        GlobalValue.Get(Constant.C_CatchPetItemInShopID, out shopID);
        COM_Item itemInst = BagSystem.instance.GetItemByItemId((uint)itemID);
        if(itemInst == null)
        {
            ItemData idata = ItemData.GetData(itemID);
            if(idata != null)
                PopText.Instance.Show(string.Format(LanguageManager.instance.GetValue("notEnoughItemCount"), idata.name_), PopText.WarningType.WT_Warning, true);
            //QuickBuyUI.ShowMe(shopID, () =>
            //{
            //    if (AttaclEvent.getInstance.CatchEvent != null)
            //    {
            //        AttaclEvent.getInstance.CatchEvent(null);
            //    }
            //    closeSkillWindow();
            //    closeSkillTwoWindow();
            //    CloseBabyWindow();
            //    SetButtonSelectState(obj.name);
            //    SetAllButtonVisible(false);
            //    SetPlayerBackBtnVisible(true);
            //});
        }
        else
        {
            
            if (AttaclEvent.getInstance.CatchEvent != null)
            {
                AttaclEvent.getInstance.CatchEvent(null);
            }
            closeSkillWindow();
            closeSkillTwoWindow();
            CloseBabyWindow();
            SetButtonSelectState(obj.name);
            SetAllButtonVisible(false);
            SetPlayerBackBtnVisible(true);
        }
    }

	private void OnClickattackBtn(ButtonScript obj, object args, int param1, int param2)
	{

		if(AttaclEvent.getInstance.attackEvent!= null)
		{
			AttaclEvent.getInstance.attackEvent(null);
		}
		closeSkillWindow ();
		closeSkillTwoWindow ();
		CloseBabyWindow();
		SetButtonSelectState (obj.name);
		SetAllButtonVisible(false);
		SetPlayerBackBtnVisible(true);
	}
	private void OnClickskillBtn(ButtonScript obj, object args, int param1, int param2)
	{
		Battle.Instance.SelectFlag = true;
        openSkillWindow();
        GlobalInstanceFunction.Instance.Invoke(() =>
        {
            if (AttaclEvent.getInstance.SkillEvent != null)
            {
                AttaclEvent.getInstance.SkillEvent(GamePlayer.Instance);
            }
        }, 2);
		CloseBabyWindow();
		SetButtonSelectState (obj.name);
	}
	private void OnClickDefenseBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(AttaclEvent.getInstance.DefenseEvent!= null)
		{
			AttaclEvent.getInstance.DefenseEvent(GamePlayer.Instance);
		}
		closeSkillWindow ();
		closeSkillTwoWindow ();
		CloseBabyWindow();
		SetButtonSelectState (obj.name);
	}
	private void OnClickPosBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(AttaclEvent.getInstance.PositionEvent!= null)
		{
			AttaclEvent.getInstance.PositionEvent(null);
		}
		closeSkillWindow ();
		closeSkillTwoWindow ();
		CloseBabyWindow();
		SetButtonSelectState (obj.name);
	}
	private void OnClickAUTOBtn(ButtonScript obj, object args, int param1, int param2)
	{
        if (AttaclEvent.getInstance.AUTOEvent != null)
        {
            AttaclEvent.getInstance.AUTOEvent(null);
        }
        else
        {
            Debug.Log("什么情况能导致战斗不初始化？？？");
            Battle.Instance.OnClickAutoBtn(null);
        }
		closeSkillWindow ();
		closeSkillTwoWindow ();
		CloseBabyWindow();
        //SetButtonSelectState (obj.name);
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_ClickBattleAuto);
	}
	private void OnClickCanAUTO(ButtonScript obj, object args, int param1, int param2)
	{
        if (AttaclEvent.getInstance.AUTOEvent != null)
        {
            AttaclEvent.getInstance.AUTOEvent(null);
        }
        else
        {
            Debug.Log("什么情况能导致战斗不初始化？？？");
            Battle.Instance.OnClickAutoBtn(null);
        }
	}
	private void OnClickArticleBtn(ButtonScript obj, object args, int param1, int param2)
	{
		BagUI.SwithShowMe ();
		BagSystem.instance.battleOpenBag = true;
		gameObject.SetActive (false);

		Battle.Instance.SelectFlag = false;
		if(AttaclEvent.getInstance.ArticleEvent!= null)
		{
			AttaclEvent.getInstance.ArticleEvent(null);
		}
		closeSkillWindow ();
		closeSkillTwoWindow ();
		CloseBabyWindow();
		SetButtonSelectState (obj.name);
	}
	
	private void OnClickpetBtn(ButtonScript obj, object args, int param1, int param2)
	{
		Battle.Instance.SelectFlag = false;
		ShowBabyWindow();
		closeSkillWindow ();
		closeSkillTwoWindow ();
		if(AttaclEvent.getInstance.PetEvent!= null)
		{
            AttaclEvent.getInstance.PetEvent(GamePlayer.Instance.BattleBaby);
		}
		SetButtonSelectState (obj.name);
	}
	private void OnClickfleeBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(AttaclEvent.getInstance.FleeEvent!= null)
		{
			AttaclEvent.getInstance.FleeEvent(GamePlayer.Instance);
		}
		closeSkillWindow ();
		closeSkillTwoWindow ();
		CloseBabyWindow();
		SetButtonSelectState (obj.name);
	}

    bool playerActionState = false;
    bool petActionState = false;
    private void OnPlayerActionBtn(ButtonScript obj, object args, int param1, int param2)
    {
        playerActionState = !playerActionState;
        AutoPlayerPanel.SetActive(playerActionState);
        AutoPetPanel.SetActive(false);
    }
    private void OnPetActionBtn(ButtonScript obj, object args, int param1, int param2)
    {
        petActionState = !petActionState;
        AutoPetPanel.SetActive(petActionState);
        AutoPlayerPanel.SetActive(false);
    }
	private void OnOpenPlayer(GameObject sender)
	{
		PlayerPropertyUI.ShowMe ();
		gameObject.SetActive (false);
	}

	private void BackClick(ButtonScript obj, object args, int param1, int param2)
	{
		if(AttaclEvent.getInstance.BackEvent!= null)
		{
			AttaclEvent.getInstance.BackEvent();
		}
	}

	private void PlayerBackClick(ButtonScript obj, object args, int param1, int param2)
	{
		if( Battle.Instance.GetBattleOperateState() == OperateType.OT_P1
		        || Battle.Instance.GetBattleOperateState() == OperateType.OT_P2 )
		{
			SetAllButtonVisible(true);
		}

        Battle.Instance.itemId_ = 0;
        Battle.Instance.useItem_ = null;
		SetPlayerBackBtnVisible(false);
        SetSelectedSkill(0);
	}

    public void OnSpeedUpBtn(ButtonScript obj, object args, int param1, int param2)
    {
        if (GamePlayer.Instance.isInBattle == false)
        {
            // tips
            return;
        }

        if (TeamSystem.IsInTeam())
        {
            ErrorTipsUI.ShowMe("组队中不可加速");
            return;
        }

        if (speedUpBtn_.normalSprite.Equals(speedUpImg))
        {
            speedUpBtn_.normalSprite = normalImg;
            if (OnSpeedUp != null)
                OnSpeedUp(false);
        }
        else
        {
            speedUpBtn_.normalSprite = speedUpImg;
            if (OnSpeedUp != null)
                OnSpeedUp(true);
        }
    }

    public void SetSpeedUpBtnVisable(bool visable)
    {
        //speedUpBtn_.gameObject.SetActive(visable);
    }
	public GameObject autoObj;
    int currentSkillId_;
	public void SetSelectedSkill(int skillId)
	{
        if (Battle.Instance.useItem_ != null)
        {
            if((Battle.Instance.useItem_.id_ == Battle.Instance.itemId_) &&
                Battle.Instance.useItem_.skillId_ != skillId)
            Battle.Instance.itemId_ = 0;
            Battle.Instance.useItem_ = null;
        }
        currentSkillId_ = skillId;
        //从玩家身上获取
        COM_Skill skillInst = GamePlayer.Instance.GetSkillById(skillId);
        //没有的话 看看宝宝上有没有
        if (skillInst == null)
        {
            if (GamePlayer.Instance.BattleBaby != null)
                skillInst = GamePlayer.Instance.BattleBaby.GetSkillCore(skillId);
        }
        
        SkillData data = null;
        if(skillInst != null)
            data = SkillData.GetData (skillId, (int)skillInst.skillLevel_);

        if (data != null)
        {
            if (selectedSkill != null)
                selectedSkill.SetActive(true);
            if (selectedSkillName != null && selectedSkillName.gameObject != null)
                selectedSkillName.gameObject.SetActive(true);
            if (playernanmeLabel != null)
                playernanmeLabel.text = Battle.Instance.GetCrtBattleActor().battlePlayer.instName_ + tempstr;
            if (selectedSkillName != null)
                selectedSkillName.text = data._Name;
            //selectedSkillIcon.spriteName = data.resIconName;
			//HeadIconLoader.Instance.LoadIcon (data.resIconName, selectedSkillIcon);
        }
        else
        {
            if (selectedSkill != null)
                selectedSkill.SetActive(false);
            if (selectedSkillName != null && selectedSkillName.gameObject != null)
                selectedSkillName.gameObject.SetActive(false);
        }
        if (whoTurn != null && whoTurn.gameObject != null)
            whoTurn.gameObject.SetActive(false);
	}

    // params. int : 1 人物1动  2 人物2动  3 宝宝
    public void SetWhoTurn(int who)
    {
        switch(who)
        {
            case 1:
                whoTurn.spriteName = "renwu1dongorder";
                break;
            case 2:
                whoTurn.spriteName = "renwu2dongorder";
                break;
            case 3:
                whoTurn.spriteName = "chongwuorder";
                break;
            default:
                break;
        }
        selectedSkill.SetActive(true);
        selectedSkillName.gameObject.SetActive(false);
        whoTurn.gameObject.SetActive(true);
    }

    public void SetCancelAutoTime(string val = "")
    {
        if (hasDestroy)
            return;

        //UILabel timeLabel = CanAUTOBtn.gameObject.GetComponentInChildren<UILabel>();
		Transform tr = CanAUTOBtn.transform.FindChild ("num");
		if(tr != null)
		{
			UISprite sp = tr.GetComponent<UISprite>();
			sp.spriteName = val;
		}
//        if(timeLabel != null)
//            timeLabel.text = val;

		autoObj.SetActive (!string.IsNullOrEmpty(val));
    }

	public void SetCanAUTOBtnVisible(bool isVisible)
	{
		CanAUTOBtn.gameObject.SetActive (isVisible);
        UILabel timeLabel = CanAUTOBtn.gameObject.GetComponentInChildren<UILabel>();
        if (timeLabel != null)
            timeLabel.text = "";
        petActionBtn.gameObject.SetActive(GamePlayer.Instance.BattleBaby != null);

        if (isVisible)
        {
            GlobalInstanceFunction.Instance.Invoke(() =>
            {
                GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_PlayerAutoOrder, Battle.Instance.BattleID);
            }, 1);
        }
	}
    public void SetCatchBtnVisible(bool isVisible)
    {
        catchBtn.gameObject.SetActive(isVisible);
    }
	public void SetAttackBtnVisible(bool isVisible)
	{
		attackBtn.gameObject.SetActive (isVisible);
	}
	public void SetskillBtnVisible(bool isVisible)
	{
		skillBtn.gameObject.SetActive (isVisible);
	}

	public void SetDefenseBtnVisible(bool isVisible)
	{
		DefenseBtn.gameObject.SetActive (isVisible);
	}
	public void SetPosBtnVisible(bool isVisible)
	{
		PosBtn.gameObject.SetActive (isVisible);
	}
	public void SetAUTOBtnVisible(bool isVisible)
	{
		AUTOBtn.gameObject.SetActive (isVisible);
	}
	public void SetArticleBtnVisible(bool isVisible)
	{
		ArticleBtn.gameObject.SetActive (isVisible);
	}
	public void SetpetBtnVisible(bool isVisible)
	{
		petBtn.gameObject.SetActive (isVisible);
	}
	public void SetfleeBtnVisible(bool isVisible)
	{
		fleeBtn.gameObject.SetActive (isVisible);
	}

	public void SetBackBtnVisible(bool isVisible)
	{
		backBtn.gameObject.SetActive (isVisible);
	}

	public void SetPlayerBackBtnVisible(bool isVisible)
	{
		PlayerBackBtn.gameObject.SetActive(isVisible);
	}

	public void SetAllButtonVisible(bool isVisible)
	{
        SetCatchBtnVisible(isVisible);
		SetAttackBtnVisible (isVisible);
		SetskillBtnVisible (isVisible);
		SetDefenseBtnVisible (isVisible);
		SetPosBtnVisible (isVisible);
        SetAUTOBtnVisible(Battle.Instance.autoBattle_? false: isVisible);
		SetArticleBtnVisible (isVisible);
		SetpetBtnVisible (isVisible);
		SetfleeBtnVisible (isVisible);
        SetCanAUTOBtnVisible(Battle.Instance.autoBattle_ ? isVisible: false);
	}

	public void SetButtonSelectState(string btnName)
	{
		for (int i = 0; i<buttons.Count; i++) 
		{
			if(btnName.Equals(buttons[i].gameObject.name))
			{
				buttons[i].gameObject.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
			}else
			{
				buttons[i].gameObject.transform.localScale = Vector3.one;
			}
		}
	}

	public void RecoverButtonStateNormal()
	{
		SetAllButtonVisible (true);
		for (int i = 0; i<buttons.Count; i++) {
			buttons[i].gameObject.transform.localScale = Vector3.one;
		}
	}
	public void SetButtonState(bool enable, params string[] name)
	{
		for(int i=0; i < name.Length; ++i)
		{
			foreach (UIButton sender in  buttons) {
				if(sender.gameObject.name.Equals(name[i]))
				{
					if(gameObject.activeSelf)
					{
						sender.gameObject.transform.localScale = enable? Vector3.one: new Vector3(0.8f,0.8f,0.8f);
						sender.gameObject.GetComponent<BoxCollider>().enabled = enable;
						sender.GetComponentInChildren<UISprite>().color = enable? Color.white: new Color(0f, 1f, 1f);
						sender.enabled = enable;
					}

				}
			}
		}
	}

	private void OnBagClick(ButtonScript obj, object args, int param1, int param2)
	{
		BagUI.SwithShowMe ();
		BagSystem.instance.battleOpenBag = true;
		gameObject.SetActive (false);
	}

	public GameObject CreatePlayerUI( BattleActor entity, bool hideBar)
	{
		if( null == entity ) return null;
        if (entity.ControlEntity == null)
        {
            ApplicationEntry.Instance.PostSocketErr(57557);
            return null;
        }
		
		GameObject	EntityObj = entity.ControlEntity.ActorObj;
		GameObject pre = GameObject.Instantiate(HUD)as GameObject;
		pre.SetActive (true);
		pre.transform.parent = transform;//hudRoot.transform;
        Vector3	battlepos = Vector3.zero;
        if (entity != null)
        {
            Transform point = Battle.Instance.GetStagePointByIndex(entity.BattlePos);
            if(point != null)
                battlepos = point.position;
        }
		battlepos.y -=0.2f;
		pre.transform.localPosition = GlobalInstanceFunction.WorldToUI(battlepos);
		pre.transform.localScale = new Vector3(0.7f,0.7f,0.7f);
		Roleui rui = pre.GetComponent<Roleui> ();
		rui.SetRoleObj(EntityObj);
        rui.InitData(entity);
		rui.hp.gameObject.SetActive (!hideBar);
		rui.mp.gameObject.SetActive (!hideBar);
		pre.name = GlobalValue.ActorUIObjName + entity.InstId;
		lstEntityUI.Add( pre );
		EntityObjs.Add (EntityObj);
		return pre;
		
	}
	
	public void SetEnityUIVisble( int EntityID , bool bVisble )
	{
		if( null == lstEntityUI ) return ;
		for( int iCount = 0; iCount < lstEntityUI.Count; ++ iCount )
		{
			if( lstEntityUI[iCount].name == ( GlobalValue.ActorUIObjName + EntityID.ToString() ) )
			{
				lstEntityUI[iCount].SetActive( bVisble );
				return ;
			}
		}
	}


	void SetEnityUIPosition()
	{
		for(int i = 0;i<EntityObjs.Count ;i++)
		{
			if( null == EntityObjs[i]) continue;
			Vector3 pos = EntityObjs[i].transform.position;
			pos.y -= 0.2f;
			lstEntityUI[i].transform.localPosition = GlobalInstanceFunction.WorldToUI( pos );
		}
	}
	public void UpdateEnityUIPosition(bool isUpdate)
	{
		isEnityUIMove = isUpdate;
	}
	public override void Destroyobj ()
	{
        hasDestroy = true;
		Destroy (gameObject);
		lstEntityUI.Clear();
		lstEntityUI = null;
		EntityObjs.Clear ();
		UIManager.RemoveButtonEventHandler (backBtn.gameObject, EnumButtonEvent.OnClick);
		UIManager.RemoveButtonEventHandler(PlayerBackBtn.gameObject, EnumButtonEvent.OnClick);
		UIManager.RemoveButtonEventHandler (attackBtn.gameObject, EnumButtonEvent.OnClick);
		UIManager.RemoveButtonEventHandler (skillBtn.gameObject, EnumButtonEvent.OnClick);
		UIManager.RemoveButtonEventHandler (DefenseBtn.gameObject, EnumButtonEvent.OnClick);
		UIManager.RemoveButtonEventHandler (PosBtn.gameObject, EnumButtonEvent.OnClick);
		UIManager.RemoveButtonEventHandler (AUTOBtn.gameObject, EnumButtonEvent.OnClick);
		UIManager.RemoveButtonEventHandler (ArticleBtn.gameObject, EnumButtonEvent.OnClick);
		UIManager.RemoveButtonEventHandler (petBtn.gameObject, EnumButtonEvent.OnClick);
		UIManager.RemoveButtonEventHandler (fleeBtn.gameObject, EnumButtonEvent.OnClick);
		UIManager.RemoveButtonEventHandler (CanAUTOBtn.gameObject, EnumButtonEvent.OnClick);
		UIManager.RemoveButtonEventHandler(speedUpBtn_.gameObject, EnumButtonEvent.OnClick);
		GlobalInstanceFunction.Instance.UpdatecounDownTime -= attackEvent;
        BagSystem.instance.UpdateItemEvent -= OnItemUpdate;
        BagSystem.instance.DelItemInstEvent -= OnItemDelete;
        BagSystem.instance.ItemChanged -= OnItemAdd;
		BagSystem.instance.BattleOpenBagEvent  -= OpenBagEvent;
        UIManager.Instance.showMainPanelEnvent -= AttackPanelShow;
		AttaclEvent.getInstance.OnSetPanelActive -= SetSelfActive;

	}

	public void DeletePlayerInfoUI( GameObject	obj )
	{
		if( null == lstEntityUI ) return ;
		for( int iCount = 0; iCount < lstEntityUI.Count; ++ iCount )
		{
			if( lstEntityUI[iCount].Equals(obj) )
			{
				GameObject.Destroy( lstEntityUI[iCount] );
				lstEntityUI.Remove( lstEntityUI[iCount] );
				EntityObjs.RemoveAt(iCount);
				break;
			}
		}
	}

	void Update ()
	{

		if(isEnityUIMove)
		{
			SetEnityUIPosition ();
		}

        if (BagSystem.instance.isDirty_)
        {
            UpdateItemCount();
            BagSystem.instance.isDirty_ = false;
        }
	}

	public void openSkillWindow()
	{
		skillView.SetActive (true);
	}
	public void CloseScrollView()
	{
		closeSkillWindow ();
		closeSkillTwoWindow ();
	}
	public void closeSkillWindow()
	{
		skillView.SetActive (false);
	}

	public  void openSkillTwoWindow ()
	{
		scrollViewTwo.SetActive (true);
	}

	public  void closeSkillTwoWindow ()
	{
		scrollViewTwo.SetActive (false);
	}

	public void OpenPetWindow()
	{
		petScrollView.SetActive (true);
	}

	public void ClosePetWindow()
	{
		petScrollView.SetActive (false);
	}

	public void ShowBabyWindow()
	{
		babyScrollView.SetActive(true);
	}
	public void CloseBabyWindow()
	{
		babyScrollView.SetActive(false);
	}
	public void Countdown()
	{
        float deltaTime = Time.realtimeSinceStartup - preTimeStamp;
        currentTime += deltaTime;
		endTime = startTime - Mathf.CeilToInt(currentTime);
		int shiwei = endTime / 10;
		int gewei = endTime % 10;

		if( endTime < 0 )
		{
            if (AttaclEvent.getInstance.CountDownEvent != null)
            {
                AttaclEvent.getInstance.CountDownEvent();
            }
            else
            {
                Debug.Log("什么情况啊？ 战斗不初始化吗？ 不注册事件吗？");
                Battle.Instance.OnCountDownTimeOut();
            }
			shiwei = 0;
			gewei = 0;
			CloseCountDown();
			CountDownG.GetComponent<Animator>().enabled = false;
		}
		else
		{
			if( endTime < 10 )
			{
				CountDownG.GetComponent<Animator>().enabled = true;
				minSp.spriteName = countDowndtheUnit [shiwei];
				secondSp.spriteName = countDowndtheUnit [gewei];
			}
			else
			{
				CountDownG.GetComponent<Animator>().enabled = false;
				minSp.transform.localScale = Vector3.one;
				secondSp.transform.localScale = Vector3.one;
				minSp.spriteName = countDowndecade [shiwei];
				if( gewei < 0 ) gewei = 0;
				secondSp.spriteName = countDowndecade [gewei];
			}
		}
        preTimeStamp = Time.realtimeSinceStartup;
	}

    public void DisplayNumberOfAttacks(BattleActor actor, bool isShow, int num)
	{
        if (actor != null && actor.ControlEntity != null && actor.ControlEntity.PlayerInfoUI != null)
            actor.ControlEntity.PlayerInfoUI.GetComponent<Roleui>().DisplayNumberOfAttacks(isShow, num);
	}

	public void CloseCountDown()
	{
		if(selectedSkill != null)
        	selectedSkill.SetActive(false);
		isCountDown = false;
		if(CountDownG != null)
			CountDownG.SetActive (false);
	}

	public void StartCountDown()
	{
        if (isCountDown)
            return;

        if (currentSkillId_ != 0 && Battle.Instance.autoBattle_ == false)
            selectedSkill.SetActive(true);
        if (GuideManager.Instance.InBattleGuide_)
            return;
		if(CountDownG != null)
			CountDownG.SetActive (true);
		isCountDown = true;
		startTime = 30;
		currentTime = 0;
        preTimeStamp = Time.realtimeSinceStartup;
	}

	public void HideUI()
	{
		SetAllButtonVisible (false);
		SetPlayerBackBtnVisible(false);
	}
	public void ShowUI()
	{
		SetAllButtonVisible (true);
	}
	
	private	void  BloodChanged(int bloodValue, int maxvalue)
	{
		if (Hp == null)return;
			Hp.value += (bloodValue * 1f )/( maxvalue * 1f);
	}
	private void MagicChanged(int MagicValue, int maxvalue)
	{
		if (Mp == null)return;
			Mp.value += MagicValue * 1f / maxvalue * 1f;
	}

    private void BloodChangedBaby(int bloodValue, int maxvalue)
    {
        if (babyhpSlider == null) return;
        babyhpSlider.value += (bloodValue * 1f) / (maxvalue * 1f);
    }
    private void MagicChangedBaby(int MagicValue, int maxvalue)
    {
        if (babympSlider == null) return;
        babympSlider.value += MagicValue * 1f / maxvalue * 1f;
    }

    public void ShowAttackType(SneakAttackType type)
    {
        if(type == SneakAttackType.SAT_SurpriseAttack)
            EffectAPI.PlayUIEffect(EFFECT_ID.EFFECT_UIBeitouxi, ApplicationEntry.Instance.uiRoot.transform);
        else if (type == SneakAttackType.SAT_SneakAttack)
            EffectAPI.PlayUIEffect(EFFECT_ID.EFFECT_UITouxi, ApplicationEntry.Instance.uiRoot.transform);
        //if (type != SneakAttackType.SAT_None && type != SneakAttackType.SAT_Max)
        //{
        //    attackType_.gameObject.SetActive(true);
        //    attackType_.spriteName = type.ToString();
        //    attackType_.MakePixelPerfect();
        //}
    }

    public void HideAttackType()
    {
        if(attackType_.gameObject != null)
            attackType_.gameObject.SetActive(false);
    }

    public int BattleTurn
    {
        set
        {
			huiheObj.SetActive (true);
            int gw = value % 10;
            int sw = value / 10 % 10;
            battleTurn_ge.spriteName = gw + "-3";
            if (sw >= 0)
                battleTurn_shi.spriteName = sw + "-3";
            else
                battleTurn_shi.spriteName = "";
            //battleTurn_.text = LanguageManager.instance.GetValue("battleTurn").Replace("{n}", value.ToString());
        }
    }

    }
