using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MainPanle : UIBase {

	public UIButton paimaihangBtn;

	public UILabel selfWinLable;
	public UILabel selfMonsterLable;
	public UILabel DiWinLable;
	public UILabel DiMonsterLable;

	public UITexture babyRaceT;
	public UITexture babyIconT;
	public UILabel babyNameLable;
	public UILabel babyHpLable;
	public UILabel babyMpLable;
	public UISlider babympSlider;
	public UISlider babyhpSlider;
	public UISlider palyerExpSlider;
	public UIButton shouchongBtn;
	public UISprite typeBabySp;

	public GameObject GuildObj;
	public UILabel xingdongliLabel;
	public UILabel jifengSelfLabel;
	public UILabel jifenLabel;
	public UILabel TimeLabel;
	public UISprite wifi01;
	public UISprite wifi02;
	public UISprite wifi03;
	public UISprite wifi04;
	public UISprite _4G01;
	public UISprite _4G02;
	public UISprite _4G03;
	public UISprite _4G04;
    public UISlider battery;
    public UISprite batteryColor;
    public UILabel nextlbl;
    public UIButton nextbtn;
    public UISprite nexticon;

	public UIButton auto;
	public UISprite autoSp;
	public UIButton paihangBtn;
	public UIButton ChengjiuBtn;
	public UIButton mrTaskBtn;
	public UIButton TujianBtn;
	public UIButton bagBtn;
	public UIButton roleBtn;
	public UIButton babyBtn;
	public GameObject babyHud;
	public GameObject babyhudbackobj;
	public UIButton SkillBtn;
	public UIButton PartnerBtn;
	public UIButton HuoBtn;
	public UIButton backBtn;
	public UIButton payBtn;
	public UIButton taskArBtn;
	public UIButton makeBtn;
	public UIButton magicItemBtn;
    public UIButton storeBtn;
	public UIButton settingBtn;
	public UIButton guideBtn;
	public UIButton choujiangBtn;
	public UIButton familylBtn;
	public UIButton sevenDayBtn;
	public UILabel levelLabel;
	//public GameObject messageObj;
	public UILabel hpLabel;
	public UILabel mpLabel;
	public UITexture icon;
	public UISprite iconRed; 
	public UILabel MoneyLabel;
	public UILabel DiamondLabel;
	public  GameObject TaskObj;
	public UIButton gatherBtn;
	public UIButton tishengBtn;
	public UIButton	sigupBtn;
	public UISprite sceneName;

	public UIButton MoneyBtn;
	public UIButton DiamondBtn;
	public UISprite emailImg;
    public UISprite vipSt_;
    public GameObject vipBtn;
    UISpriteAnimation spAnim_;

    public GameObject[] BottomBtnRoot_;
    public GameObject[] TopBtnRoot_;
    public GameObject Stick_;
    public GameObject stickCore_;
    //public GameObject topTweener;
    public GameObject bottomTweener;
    Vector2 originStickCorePos_;
	public GameObject babyEffect;
	public GameObject playerEffect;

	public UIButton propAddBtn;

	public GameObject NpcItemUiObj;
    public GameObject rightBottomGroup_;
    public GameObject leftBottomGroup_;
    public GameObject leftTopGroup_;
    public GameObject rightTopGroup_;
    public GameObject leftGroup_;
    public GameObject rightGroup_;

    public GameObject fastUsePanel_;
	public GameObject ScenePlayerPanel_;

    public UISprite[] stateSlot_;
	public GameObject[] stateSlotEffect;
	public UISprite bagfull;
	public UIButton openDoubleBtn;
	public UIButton closeDoubleBtn;
	public UILabel doubleTimeLab;
	public GameObject doubleExpObj;
	public UIButton friendNewsBtn;
    bool isKaitou_ = false;
    bool signUpMended = false;
    public RaisePanel raisePanel_;
	//public UIButton NpcPanleObj;
	//public UIButton wordPosition;
	public UIButton DTBtn;
	public GameObject openFunZhuan;
	public UIButton topRightBtn;
    public Transform mutiActorPanel;
    public UIGrid mutiActorGrid;
    public GameObject mutiActorItem;
	public UIButton miaoShaBtn;
	public UIButton gemBtn;
	public UIButton levelRewardShopBtn;
	private bool isTopRigthBtnDown = false;
	private bool _isPlayLevelUpEffect;
	public static bool isAuto = false;
	private UISprite _HuobanBtnSprite;
	private bool employeeFreeRed;
	private bool employeeEvolveRed;
	private bool employeePosRed;
	private bool babyRed;
	public minmapUI minmap; 
	public UILabel levelRewardTimeLab;
	public UILabel levelRewardLevelLab;
	public UISprite fuliBtnImg;
	public GameObject employeeAddPos;
	public GameObject employeeAddPosClose;
	
	private static MainPanle _mainPanle = null;
	public static MainPanle Instance 
	{
		get{
			return _mainPanle;
		}
	}

	void Awake()
	{
		DisplayFPS.OnTimeUpdate += UpdateTime;
		DisplayFPS.OnBatteryUpdate += UpdateBattery;
		DisplayFPS.OnPingUpdate += UpdatePing;
	}

	// Use this for initialization
	void Start () {
	
        _mainPanle = this;
		item.SetActive (false);
        StageMgr.OnSceneBeginLoad += OnBeginLoadScene;
//		if(isAuto)
//		{
//			autoSp.spriteName = "quxiaozidong";
//			auto.normalSprite = "quxiaozidong";
//		}else
//		{
//			autoSp.spriteName = "zidong";
//			auto.normalSprite = "zidong";			
//		}
		SceneData ssdata = SceneData.GetData (GameManager.SceneID);
		if(ssdata.sceneType_ == SceneType.SCT_Instance)
		{
            GuildObj.SetActive(false);
		}
        else if (ssdata.sceneType_ == SceneType.SCT_GuildBattleScene)
        {
            GuildObj.SetActive(true);
            selfWinLable.gameObject.SetActive(false);
            DiWinLable.gameObject.SetActive(false);
            xingdongliLabel.gameObject.SetActive(false);
			StartBattleState (GuildSystem.otherName, GuildSystem.otherwinCount, GuildSystem.selfwinCount);
        }
        else
		{
			GuildObj.SetActive(false);
		}
		TimeLabel.text = "";
		SetBabyInfo ();
        //提升事件注册提到更新逻辑之前
        RaiseUpSystem.OnUpdateRaisePanelUI += UpdateRaisePanel;
		//GlobalInstanceFunction.Instance.GuildBattCounDownTime += GuildBattCounDownTimer;
		if (Application.loadedLevelName.Equals(SceneData.GetData(SceneData.HomeID).sceneLevelName_)) {
			SetKaitou(false);
//            backBtn.gameObject.SetActive(false);
//			auto.gameObject.SetActive(false);
            if (GamePlayer.Instance.IsFirstLogin)
            {
                NpcRenwuUI.talkFinishCallBack_ = () =>
                {
                    GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_EnterMainScene);
                };
                bool ret = GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_Talk_FirstEnterMainScene);
                if (!ret)
                {
                    NpcRenwuUI.talkFinishCallBack_ = null;
                    GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_EnterMainScene);
                }
            }
            else
            {
                GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_EnterMainScene);
            }
            RaiseUpSystem.Instance.EnterGame();
		}
        else if (Application.loadedLevelName.Equals(GlobalValue.StageName_piantoudonghuaf))
        {
            SetKaitou(true);
        }
        else
		{
            //if (SignUpManager.Instance.IsSignUped(SignUpManager.Instance.Today) == false && signUpMended == false)
            //{
            //    SignUpPanel.SwithShowMe();
            //    signUpMended = true;
            //}
            SetKaitou(false);
			//backBtn.gameObject.SetActive(true);
			//auto.gameObject.SetActive(true);
			//UIManager.SetButtonEventHandler (backBtn.gameObject, EnumButtonEvent.OnClick, OnClickbackBtn, 0, 0);
            RaiseUpSystem.Instance.EnterGame();
        }
//        SceneSimpleData ssd = SceneSimpleData.GetData(GameManager.SceneID);
//        if (ssd != null)
//        {
//            sceneName.spriteName = ssd.nameEffectId_;
//            sceneName.gameObject.SetActive(true);
//        }
//        else
//            sceneName.gameObject.SetActive(false);
		UpdateTrackingQuests ();
		Prebattle.Instance.OnTouchOtherPlayer += OnTouchOPlayer;
        GamePlayer.Instance.OnIPropUpdate += UpdateInfoBar;
        GamePlayer.Instance.OnAchievementUpdate += UpdateMark;
		GamePlayer.Instance.babyUpdateIpropEvent += new RequestEventHandler<int> (OnUpdateBaby);
		GamePlayer.Instance.PlayerLevelUpEvent += new RequestEventHandler<int> (OnLevelUp);
        GamePlayer.Instance.BabyLevelUpEvent += new RequestEventHandler<int[]>(OnBabyLevelUp);
        //GamePlayer.Instance.OnStateUpdate += UpdateState;
        //GamePlayer.Instance.OnAutoMeetEnemy += UpdateAME;
        //GamePlayer.Instance.AddBuffEnvet += new RequestEventHandler<int> (ShowBuffEffect);
		GamePlayer.Instance.getShouChongEnvet += new RequestEventHandler<bool> (OnGetShouChongEnvet);
		GamePlayer.Instance.MiaoshaEnvet += new RequestEventHandler<COM_ADGiftBag> (OnMiaoshaEnvet);
		GamePlayer.Instance.levelRewadEnvet += new RequestEventHandler<int> (OnLecelRewardShopEnvet);
        //GamePlayer.Instance.SetUpdateStateEventHandler(new RequestEventHandler<int>(ShowBuffEffect));
        
        //GatherSystem.instance.MineTimeout += ShowGatehrRed;
        FastUpload.Instance.OnClose += HideFastUse;
        ActivitySystem.Instance.OnActivityOpen += OnActivityOpen;
        GameScript.OnHideRenwuList += ExcuteHideRenwuScriptEvent;
        UpdateInfoBar();
		OnUpdateBaby (0);
        //GamePlayer.OnUpdateTrackingQuestsUI = UpdateTrackingQuests;
		UIManager.SetButtonEventHandler (SkillBtn.gameObject, EnumButtonEvent.OnClick, OnClickSkillBtn, 0, 0);
		UIManager.SetButtonEventHandler (bagBtn.gameObject, EnumButtonEvent.OnClick, OnClickBag, 0, 0);
		UIManager.SetButtonEventHandler (roleBtn.gameObject, EnumButtonEvent.OnClick, OnClickRile, 0, 0);
		UIManager.SetButtonEventHandler (babyBtn.gameObject, EnumButtonEvent.OnClick, OnClickbabyBtn, 0, 0);
		//UIManager.SetButtonEventHandler (PartnerBtn.gameObject, EnumButtonEvent.OnClick, OnClickPartnerBtn, 0, 0);
		UIManager.SetButtonEventHandler (HuoBtn.gameObject, EnumButtonEvent.OnClick, OnClickHuoBtn, 0, 0);
		UIManager.SetButtonEventHandler (payBtn.gameObject, EnumButtonEvent.OnClick, OnClickpayBtn, 0, 0);
        UIManager.SetButtonEventHandler(sigupBtn.gameObject, EnumButtonEvent.OnClick, OnClickSignBtn, 0, 0);
		UIManager.SetButtonEventHandler (TujianBtn.gameObject, EnumButtonEvent.OnClick, OnClickTujianBtn, 0, 0);
		UIManager.SetButtonEventHandler (MoneyBtn.gameObject, EnumButtonEvent.OnClick, OnClickMoney, 0, 0);
		UIManager.SetButtonEventHandler (DiamondBtn.gameObject, EnumButtonEvent.OnClick, OnClickDiamond, 0, 0);
		//UIManager.SetButtonEventHandler (mrTaskBtn.gameObject, EnumButtonEvent.OnClick, OnClickmrTask, 0, 0);
		UIManager.SetButtonEventHandler (ChengjiuBtn.gameObject, EnumButtonEvent.OnClick, OnClickChengjiu, 0, 0);
		UIManager.SetButtonEventHandler(makeBtn.gameObject,EnumButtonEvent.OnClick, OnClickMake, 0, 0);
		UIManager.SetButtonEventHandler(magicItemBtn.gameObject,EnumButtonEvent.OnClick, OnMagicItem, 0, 0);
        UIManager.SetButtonEventHandler(storeBtn.gameObject, EnumButtonEvent.OnClick, OnClickStore, 0, 0);
        UIManager.SetButtonEventHandler(shouchongBtn.gameObject, EnumButtonEvent.OnClick, OnClickShowChongBtn, 0, 0);
        UIManager.SetButtonEventHandler(bottomTweener, EnumButtonEvent.OnClick, OnBottomBtnTween, 0, 0);
        //UIManager.SetButtonEventHandler(topTweener, EnumButtonEvent.OnClick, OnTopBtnTween, 0, 0);
		//UIManager.SetButtonEventHandler(auto.gameObject, EnumButtonEvent.OnClick, OnautoOrCanauto, 0, 0);
		UIManager.SetButtonEventHandler(propAddBtn.gameObject, EnumButtonEvent.OnClick, OnClickRile, 0, 0);
		UIManager.SetButtonEventHandler(paihangBtn.gameObject, EnumButtonEvent.OnClick, OnClickpaihang, 0, 0);
        UIManager.SetButtonEventHandler(settingBtn.gameObject, EnumButtonEvent.OnClick, OnClickSetting, 0, 0);
        //UIManager.SetButtonEventHandler(vipBtn_.gameObject, EnumButtonEvent.OnClick, OnClickVip, 0, 0);
		UIManager.SetButtonEventHandler(familylBtn.gameObject, EnumButtonEvent.OnClick, OnClickFamily, 0, 0);
		UIManager.SetButtonEventHandler(openDoubleBtn.gameObject, EnumButtonEvent.OnClick, openDouble, 0, 0);
        UIManager.SetButtonEventHandler(tishengBtn.gameObject, EnumButtonEvent.OnClick, OnClickTisheng, 0, 0);
		UIManager.SetButtonEventHandler(closeDoubleBtn.gameObject, EnumButtonEvent.OnClick, closeDouble, 0, 0);
        UIManager.SetButtonEventHandler(gatherBtn.gameObject, EnumButtonEvent.OnClick, OnClickGatherBtn, 0, 0);
		UIManager.SetButtonEventHandler(guideBtn.gameObject, EnumButtonEvent.OnClick, OnClickGuide, 0, 0);
		UIManager.SetButtonEventHandler(friendNewsBtn.gameObject, EnumButtonEvent.OnClick, OnFriendNews, 0, 0);
		UIManager.SetButtonEventHandler(paimaihangBtn.gameObject, EnumButtonEvent.OnClick, OnClickAuctionBtn, 0, 0);
		UIManager.SetButtonEventHandler(emailImg.gameObject, EnumButtonEvent.OnClick, OnClickEmail, 0, 0);
		UIManager.SetButtonEventHandler(miaoShaBtn.gameObject,EnumButtonEvent.OnClick, OnClickMiaosha, 0, 0);
		UIManager.SetButtonEventHandler(gemBtn.gameObject,EnumButtonEvent.OnClick, OnClickGem, 0, 0);
		UIManager.SetButtonEventHandler(levelRewardShopBtn.gameObject,EnumButtonEvent.OnClick, OnClickLevelRewardShopBtn, 0, 0);
		UIManager.SetButtonEventHandler (sevenDayBtn.gameObject, EnumButtonEvent.OnClick, OnClickSevenDay, 0, 0);
		UIManager.Instance.DoActive (this.GetComponent<UIPanel>());
		PlayerData pdata = PlayerData.GetData ((int)GamePlayer.Instance.Properties[(int)PropertyType.PT_TableId]);
		EntityAssetsData enData = EntityAssetsData.GetData (pdata.lookID_);
		HeadIconLoader.Instance.LoadIcon ("R_"+enData.assetsIocn_, icon);
		UIManager.SetButtonEventHandler(choujiangBtn.gameObject, EnumButtonEvent.OnClick, OnClickchoujiangBtn, 0, 0);
		//UIManager.SetButtonEventHandler(topRightBtn.gameObject, EnumButtonEvent.OnClick, OnClickTopRightBtn, 0, 0);
        UIManager.SetButtonEventHandler(nextbtn.gameObject, EnumButtonEvent.OnClick, OnClickNextBtn, 0, 0);
		UIManager.SetButtonEventHandler(employeeAddPosClose.gameObject, EnumButtonEvent.OnClick, OnEmployeeAddPosClose, 0, 0);
        //XInput.Instance.OnMoveBegin += BeginStick;
        //XInput.Instance.OnMove += MoveStick;
        //XInput.Instance.OnMoveEnd += EndStick;

        XInput.Instance.OnTouchMutiActor += MutiActor;

		//UIManager.SetButtonEventHandler (wordPosition.gameObject, EnumButtonEvent.OnClick, OnClickwordPosition, 0, 0);
		UIManager.SetButtonEventHandler (taskArBtn.gameObject, EnumButtonEvent.OnClick, OnClicktaskAr, 0, 0);

        UIEventListener.Get(vipBtn).onClick += delegate(GameObject go)
        {
            StoreUI.SwithShowMe(3);
        };

        int[] buffItem = new int[2]{ 3003, 3004 };
        UIEventListener listener = null;
        for (int i = 0; i < stateSlot_.Length; ++i)
        {
            listener = UIEventListener.Get(stateSlot_[i].gameObject);
            listener.onClick += delegate(GameObject go)
            {
                QuickBuyUI.ShowMe(buffItem[(int)(UIEventListener.Get(go).parameter)]);
            };
            listener.parameter = i;
        }

		//UIManager.SetButtonEventHandler (NpcPanleObj.gameObject, EnumButtonEvent.OnClick, OnClickNpcPanleObj, 0, 0);
        GuideManager.Instance.RegistGuideAim(bagBtn.gameObject, GuideAimType.GAT_MainBag);
        GuideManager.Instance.RegistGuideAim(babyBtn.gameObject, GuideAimType.GAT_MainBaby);
        GuideManager.Instance.RegistGuideAim(roleBtn.gameObject, GuideAimType.GAT_MainPlayerInfo);
        GuideManager.Instance.RegistGuideAim(HuoBtn.gameObject, GuideAimType.GAT_MainPartner);
        //GuideManager.Instance.RegistGuideAim(backBtn.gameObject, GuideAimType.GAT_MainReturn);
        GuideManager.Instance.RegistGuideAim(makeBtn.gameObject, GuideAimType.GAT_MainMake);
        GuideManager.Instance.RegistGuideAim(magicItemBtn.gameObject, GuideAimType.GAT_MainMagic);
        GuideManager.Instance.RegistGuideAim(ChengjiuBtn.gameObject, GuideAimType.GAT_MainAchievement);
        GuideManager.Instance.RegistGuideAim(tishengBtn.gameObject, GuideAimType.GAT_MainRaise);
        GuideManager.Instance.RegistGuideAim(storeBtn.gameObject, GuideAimType.GAT_MainActivity);
        GuideManager.Instance.RegistGuideAim(familylBtn.gameObject, GuideAimType.GAT_MainFamily);
        //GuideManager.Instance.RegistGuideAim(Stick_, GuideAimType.GAT_MainStick);
        GamePlayer.Instance.OpenSubSystemEnvet += new RequestEventHandler<ulong> (UpdateOpenSystem);
		GamePlayer.Instance.OpenSystemEnvetString += new RequestEventHandler<int> (UpdateOpenSystemStr);
        GamePlayer.Instance.OnVipUpdate += UpdateVip;

		BagSystem.instance.DelItemEvent += new ItemDelEventHandler (OnDelBagNum);
		BagSystem.instance.ItemChanged += new ItemChangedEventHandler (OnAddBagNum);
		BagSystem.instance.UpdateItemEvent += new ItemUpdateEventHandler (OnAddBagNum);
		GamePlayer.Instance.doubleTimeEnvet += new RequestEventHandler<float> (OnDoubleTimeEnvet);
		EmployessSystem.instance.employeeRedEnvent += new RequestEventHandler<int> (OnEmployeeRedEvent);
		FriendSystem.Instance ().FriendChat += new FriendChatHandler (OnFriendNewsEvent);
		ArenaPvpSystem.Instance.OpenPvpUIEnven += new RequestEventHandler<bool> (PvpUIEnvet);
		updateGuildBattleOk (GuildSystem.otherName);
		UIManager.Instance.showMainPanelEnvent += new RequestEventHandler<bool>(ShowMainPanel);
		GuildSystem.updateGuildBattleWinEventOk += UpdateguildBattleWinCount;
		GuildSystem.updateGuildBattleEventOk += updateGuildBattleOk;
		GuildSystem.startGuildBattleOk += StartBattleState;
		ARPCProxy.OnAddPlayerTitle += AddPlayerTitle;
		MoreActivityData.instance.MoreActivityRedEvent += new RequestEventHandler<int>(MoreActivityRedEvent);
		UpdateguildBattleWinCount (GuildSystem.selfwinCount,GuildSystem.otherwinCount);
		UpdateOpenSystem (0);

		UpdateMark ();
		ShowGatehrRed ();
		MoreActivityRedEvent (1);
        SetBottomButton(GamePlayer.Instance.BottomBtnState);
       // SetTopButton(GamePlayer.Instance.TopBtnState);
		//DailyTaskUI.OnMar += ShowDaTask;
		OnMiaoshaEnvet (GamePlayer.Instance.miaoshaData_);
		OnLecelRewardShopEnvet (1);
		RewardPrice ();
		//QuestSystem.OnQuestEffectFinish += OnEffectFinish;
		ArenaSystem.Instance.IsOPenArena ();
		if(QuestSystem.isEffectF)
		{
			OnEffectFinish(QuestSystem.sqid);
			QuestSystem.isEffectF = false;
		}

		if(BagSystem.instance.BagIsFull())
		{
			bagfull.gameObject.SetActive(true);
		}
		else
		{
			bagfull.gameObject.SetActive(false);
		}
		EmployessSystem.instance.UpdateEmployeeRed ();

		UpdateDoubleTime ();
        UpdateVip();
        UpdateState();
		guildState ();
        //UpdateState();
		FriendSystem.Instance().UpdateFriend += new UpdateFriendHandler(OnUpdataFriendList);

        //StageMgr.SceneLoadedFinish();



		for(int f=0;f<GamePlayer.Instance.OpenFunEffectBtns.Count;f++)
		{
			GameObject obj = getBtnObj (GamePlayer.Instance.OpenFunEffectBtns[f]);
			if(obj != null)
			{
				GameObject tx = GameObject.Instantiate(openFunZhuan.gameObject) as GameObject;
				
				tx.transform.parent = obj.transform;
				tx.SetActive(true);
				UISprite sp =  getbtnBackground(obj);
				EffectLevel ev = tx.AddComponent<EffectLevel>();
				ev.target = sp;
				tx.transform.localPosition = Vector3.zero;
				tx.transform.localScale = Vector3.one;
			}
		}

        //有活动开启 并用户没点过
        if (GamePlayer.Instance.hasActivityOpen_)
        {
            GameObject obj2 = getBtnObj((int)OpenSubSystemFlag.OSSF_Activity);
            Transform eff = obj2.transform.FindChild("lizixuanzhuan(Clone)");
            if (eff == null)
            {
                GameObject tx = GameObject.Instantiate(openFunZhuan.gameObject) as GameObject;

                tx.transform.parent = obj2.transform;
                tx.SetActive(true);
				UISprite sp =   getbtnBackground(obj2);
				EffectLevel ev = tx.AddComponent<EffectLevel>();
				ev.target = sp;
                tx.transform.localPosition = Vector3.zero;
                tx.transform.localScale = Vector3.one;
            }
        }

		//ExamSystem.OpenExamH += OpenExamBtn;
		ExamSystem.UpdateActivityState += OpenExamBtn;
		if(ExamSystem._IsOpenExam)
		{
			DTBtn.gameObject.SetActive (true);
			UIManager.SetButtonEventHandler (DTBtn.gameObject, EnumButtonEvent.OnClick, OnClickDTBtn, 0, 0);
		}else
		{
			DTBtn.gameObject.SetActive (false);
		}

        GamePlayer.Instance.levelIsDirty_ = true;

		if(GamePlayer.Instance.isGetShouchong)
		{
			shouchongBtn.gameObject.SetActive(false);
		}
        if(tishengBtn.gameObject != null)
            tishengBtn.gameObject.SetActive(RaiseUpSystem.Instance.HasItem);

		if(EmployessSystem.instance.isShowAddPos)
			employeeAddPos.gameObject.SetActive(true);
		else
			employeeAddPos.gameObject.SetActive(false);

	}
	public GameObject item;
	public UIGrid grid;
	//public UIButton closeBtn;
	public void AddPlayerTitle(int title)
	{
		GameObject go = Instantiate (item)as GameObject;
		go.SetActive (true);
		go.transform.parent = grid.transform;
		go.transform.localScale = Vector3.one;
		TitleData tData = TitleData.GetTitleData(title);
		TitlePlayerCell tpcell = go.GetComponent<TitlePlayerCell>();
		tpcell.titData = tData;
		grid.repositionNow = true;
		Destroy des = go.AddComponent<Destroy> ();
		des.SetLifeTime(6.0f);
	}
	BoxCollider bc;
	float bcX = 0;
	float bccX = 0;
	public void SetBabyInfo()
	{

		Baby battbaby =  GamePlayer.Instance.BattleBaby;
		if(battbaby == null)
		{
			babyHpLable.text = "";
			babyMpLable.text = "";
			babyhpSlider.value = 0;
			babympSlider.value = 0;
			typeBabySp.gameObject.SetActive(false);
			babyhudbackobj.SetActive(false);
			babyIconT.mainTexture = null;
			babyhpSlider.gameObject.SetActive(false);
			babympSlider.gameObject.SetActive(false);
				if(babyBtn.gameObject.activeSelf)
					babyBtn.GetComponentInChildren<UISprite>().MarkOff();
		}else
		{
			babyhudbackobj.SetActive(true);
			babyhpSlider.gameObject.SetActive(true);
			babympSlider.gameObject.SetActive(true);
			HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData(BabyData.GetData(battbaby.GetIprop(PropertyType.PT_TableId))._AssetsID).assetsIocn_, babyIconT);
			babyNameLable.text = battbaby.GetIprop(PropertyType.PT_Level).ToString();
			BabyData bdata = BabyData.GetData(battbaby.GetIprop(PropertyType.PT_TableId));
			typeBabySp.gameObject.SetActive(true);
			typeBabySp.spriteName  = bdata._Tpye.ToString();
			int crtHp = battbaby.GetIprop(PropertyType.PT_HpCurr);
			int crtMp = battbaby.GetIprop(PropertyType.PT_MpCurr);
			int maxHp = battbaby.GetIprop (PropertyType.PT_HpMax);
			int maxMp =battbaby.GetIprop (PropertyType.PT_MpMax);
			babyHpLable.text = crtHp + "/" + maxHp;
			babyMpLable.text = crtMp + "/" +maxMp;
			babyhpSlider.value = battbaby.Properties[(int)PropertyType.PT_HpCurr] * 1f / battbaby.Properties[(int)PropertyType.PT_HpMax] * 1f;
			babympSlider.value = battbaby.Properties[(int)PropertyType.PT_MpCurr] * 1f / battbaby.Properties[(int)PropertyType.PT_MpMax] * 1f;
		}


	}
	void OnTouchOPlayer(COM_ScenePlayerInformation player)
	{
		if(player == null)return;
		ScenePlayerPanel_.GetComponent<ScenePlayerUI>().PlayerInst = player;
	}
	void OpenExamBtn(bool isopen)
	{
		if(DTBtn !=null)
		{
			DTBtn.gameObject.SetActive (isopen);
			UIManager.SetButtonEventHandler (DTBtn.gameObject, EnumButtonEvent.OnClick, OnClickDTBtn, 0, 0);
		}



	}
	void UpdateTime(string time)
	{
		TimeLabel.text = time;
	}
	void UpdateBattery(float bty)
	{
        if (!battery.gameObject.activeSelf)
            battery.gameObject.SetActive(true);
        battery.value = bty;
        if (bty <= 0.2f)
            batteryColor.color = Color.red;
        else
            batteryColor.color = Color.white;
	}
	void UpdatePing(float lag)
	{
		int level = 0;
		if(lag <=100)
		{
			level = 4;
		}else
			if(lag <=200)
		{
			level = 3;
		}else
			if(lag <= 300)
		{
			level = 2;
		}
		else
			level = 1;

		if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)               
		{ 
			_4G01.gameObject.SetActive(false);
			wifi01.gameObject.SetActive(true);
			switch(level)
			{
			case 4:
				wifi01.spriteName ="wifi1";
				wifi02.spriteName ="wifi2";
				wifi03.spriteName ="wifi3";
				wifi04.spriteName ="wifi4";
				wifi02.gameObject.SetActive(true);
				wifi03.gameObject.SetActive(true);
				wifi04.gameObject.SetActive(true);
				break;
			case 3:
				wifi01.spriteName ="awifi1";
				wifi02.spriteName ="awifi2";
				wifi03.spriteName ="awifi3";

				wifi02.gameObject.SetActive(true);
				wifi03.gameObject.SetActive(true);
				wifi04.gameObject.SetActive(false);

				break;
			case 2:
				wifi01.spriteName ="awifi1";
				wifi02.spriteName ="awifi2";
				wifi02.gameObject.SetActive(true);
				wifi03.gameObject.SetActive(false);
				wifi04.gameObject.SetActive(false);

				break;
			case 1:

				wifi02.gameObject.SetActive(false);
				wifi03.gameObject.SetActive(false);
				wifi04.gameObject.SetActive(false);
				wifi01.spriteName = "hwifi1_r";
				break;
			}

		}
		else  
		if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)             
		{
			_4G01.gameObject.SetActive(true);
			wifi01.gameObject.SetActive(false);
			switch(level)
			{
			case 4:
				_4G01.spriteName = "liuliang1";
				_4G02.spriteName = "liuliang2";
				_4G03.spriteName = "liuliang3";
				_4G04.spriteName = "liuliang4";
				_4G02.gameObject.SetActive(true);
				_4G03.gameObject.SetActive(true);
				_4G04.gameObject.SetActive(true);
				break;
			case 3:
				_4G01.spriteName = "aliuliang1";
				_4G02.spriteName = "aliuliang2";
				_4G03.spriteName = "aliuliang3";
				_4G02.gameObject.SetActive(true);
				_4G03.gameObject.SetActive(true);
				_4G04.gameObject.SetActive(false);
			
				break;
			case 2:
				_4G01.spriteName = "aliuliang1";
				_4G02.spriteName = "aliuliang2";
				_4G02.gameObject.SetActive(true);
				_4G03.gameObject.SetActive(false);
				_4G04.gameObject.SetActive(false);
				break;
			case 1:

				_4G02.gameObject.SetActive(false);
				_4G03.gameObject.SetActive(false);
				_4G04.gameObject.SetActive(false);
				_4G01.spriteName = "hliuliang1_r";
				break;
			}
		}
	}
	int selfMonsterCount;
	int otherMonsterCount;
	void updateGuildBattleOk(string oidName)
	{
		SceneData ssdata = SceneData.GetData (GameManager.SceneID);
		if(ssdata.sceneType_ == SceneType.SCT_GuildBattleScene)
		{
			
			if(GuildSystem.IsInGuild())
			{
				jifengSelfLabel.text = GuildSystem.Mguild.guildName_;
				jifenLabel.text = oidName;
			}
		}
	}
	void guildState()
	{
		if(GuildSystem.battleState==1)
		{
			xingdongliLabel.gameObject.SetActive (true);
			selfWinLable.gameObject.SetActive (false);
			DiWinLable.gameObject.SetActive (false);
		}else
			if(GuildSystem.battleState==2)
		{
			xingdongliLabel.gameObject.SetActive (false);
			selfWinLable.gameObject.SetActive (true);
			DiWinLable.gameObject.SetActive (true);
		}


	}
//	void GuildBattCounDownTimer(string time)
//	{
//		xingdongliLabel.gameObject.SetActive (true);
//		xingdongliLabel.text = LanguageManager.instance.GetValue("kaishijishi").Replace("{n}",time);
//	}
	void StartBattleState(string otherName, int otherCon, int selfCon)
	{
		xingdongliLabel.gameObject.SetActive (false);
		selfWinLable.gameObject.SetActive (true);
		selfWinLable.text = LanguageManager.instance.GetValue("wofangjifen").Replace("{n}",selfCon.ToString());
		DiWinLable.gameObject.SetActive (true);
		DiWinLable.text  =LanguageManager.instance.GetValue("duifangjifen").Replace("{n}",otherCon.ToString());
		jifengSelfLabel.text = GuildSystem.Mguild.guildName_;
		jifenLabel.text = otherName;
	}
	void UpdateguildBattleWinCount(int selfCount , int othercount)
	{
		xingdongliLabel.gameObject.SetActive (false);
		selfWinLable.text = LanguageManager.instance.GetValue("wofangjifen").Replace("{n}",selfCount.ToString());
		DiWinLable.text  =LanguageManager.instance.GetValue("duifangjifen").Replace("{n}",othercount.ToString());
	}
	void OnUpdataFriendList(COM_ContactInfo contact,bool isNew)
	{
		
		if(isNew)
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("addfriendok"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("addfriendok"));
		}
		else
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("delfriendOk"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("delfriendOk"));
		}
		
	}
    void UpdateRaisePanel()
    {
        if (GamePlayer.Instance.isInBattle)
            return;

        tishengBtn.gameObject.SetActive(RaiseUpSystem.Instance.HasItem);
        if (raisePanel_ != null && raisePanel_.gameObject.activeSelf)
			raisePanel_.UpdateData();
    }

    void OnEnable()
    {
        UpdateInfoBar();
		OnUpdateBaby(0);
		SetBabyInfo ();
		ShowEmpAddpos ();
        if(fastUsePanel_.activeSelf)
        {
            if (FastUpload.preItem != null)
            {
                if (BagSystem.instance.GetItemByInstId((int)FastUpload.preItem.instId_) == null)
                {
                    ShowFastUse(FastUpload.Instance.GetOne());
                }
            }
        }

			if(SuccessSystem.isReceived())
			{
			if(ChengjiuBtn.gameObject.activeSelf)
			{
				if(ChengjiuBtn.GetComponentInChildren<UISprite>()!= null)
					ChengjiuBtn.GetComponentInChildren<UISprite>().MarkOn(UISprite.MarkAnthor.MA_RightTop,-12,-25);
			}
				
			}else
			{

			if(ChengjiuBtn.gameObject.activeSelf)
			{
				if(ChengjiuBtn.GetComponentInChildren<UISprite>()!= null)
				ChengjiuBtn.GetComponentInChildren<UISprite>().MarkOff();
			}
				
			}
            GlobalInstanceFunction.Instance.Invoke(() =>
            {
                GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainPanelOpen);
            }, 1);
    }

    StateCellUI scuiHp, scuiMp, worldExp;
    void UpdateState()
    {
		ClientLog.Instance.Log (" Has " + GamePlayer.Instance.buffList_.Count + " Buffs.");
        for (int i = 0; i < GamePlayer.Instance.buffList_.Count; ++i)
        {
            if (i > 2)
                break;

            //checkbuff只执行一次
            if (GameManager.Instance.noNeedCheckBuff_ == false)
            {
                if (GlobalValue.isBattleScene(StageMgr.preScene_))
                {
                    int left = GamePlayer.Instance.buffList_[i].value0_;
                    GameManager.Instance.procCheckBuff_ = true;
                    GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_CheckBuff, (int)GamePlayer.Instance.buffList_[i].stateId_, left);
                    GameManager.Instance.procCheckBuff_ = false;
                }
            }
        }

        COM_State hpb = GetHpBuff();
        if (hpb != null)
        {
            scuiHp = UIManager.Instance.AddStateCellUI(stateSlot_[0], hpb);
            stateSlot_[0].gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            stateSlot_[0].gameObject.GetComponent<BoxCollider>().enabled = true;
            if (scuiHp != null)
                Destroy(scuiHp);
        }

        COM_State mpb = GetMpBuff();
        if (mpb != null)
        {
            scuiMp = UIManager.Instance.AddStateCellUI(stateSlot_[1], mpb);
            stateSlot_[1].gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            stateSlot_[1].gameObject.GetComponent<BoxCollider>().enabled = true;
            if (scuiMp != null)
                Destroy(scuiMp);
        }

        COM_State we = GetWorldEXP();
        if(we != null)
            worldExp = UIManager.Instance.AddStateCellUI(stateSlot_[2], we);
        stateSlot_[2].gameObject.SetActive(we != null);
    }

    COM_State GetHpBuff()
    {
        int hpid = 0;
        GlobalValue.Get(Constant.C_HPBuffShopID, out hpid);
        for(int i=0; i < GamePlayer.Instance.buffList_.Count; ++i)
        {
            if (GamePlayer.Instance.buffList_[i].stateId_ == hpid)
                return GamePlayer.Instance.buffList_[i];
        }
        return null;
    }

    COM_State GetMpBuff()
    {
        int mpid = 0;
        GlobalValue.Get(Constant.C_MPBuffShopID, out mpid);
        for(int i=0; i < GamePlayer.Instance.buffList_.Count; ++i)
        {
            if (GamePlayer.Instance.buffList_[i].stateId_ == mpid)
                return GamePlayer.Instance.buffList_[i];
        }
        return null;
    }

    COM_State GetWorldEXP()
    {
        int hpid = 0;
        GlobalValue.Get(Constant.C_HPBuffShopID, out hpid);
        int mpid = 0;
        GlobalValue.Get(Constant.C_MPBuffShopID, out mpid);
        for (int i = 0; i < GamePlayer.Instance.buffList_.Count; ++i)
        {
            if (GamePlayer.Instance.buffList_[i].stateId_ != hpid && GamePlayer.Instance.buffList_[i].stateId_ != mpid)
                return GamePlayer.Instance.buffList_[i];
        }
        return null;
    }

	void OnGetShouChongEnvet(bool b)
	{
		if(b)
		{
			shouchongBtn.gameObject.SetActive(false);
		}
	}

	void OnLecelRewardShopEnvet(int num)
	{
		levelRewardShopBtn.gameObject.SetActive(false);
		List<COM_CourseGift> shop = GamePlayer.Instance.levelShopList;
		if(shop != null && shop.Count > 0)
		{  
			for(int i=0;i<shop.Count;i++)
			{
				if(shop[i].timeout_ > 0) 
				{  
					levelRewardShopBtn.gameObject.SetActive(true); 
					levelRewardLevelLab.text = CourseGiftData.GetData((int)shop[i].id_).level_.ToString() + LanguageManager.instance.GetValue("jilibao");
					break;
				} 
			}
		}
		else
		{
			levelRewardShopBtn.gameObject.SetActive(false);
		}

	}

	void OnMiaoshaEnvet(COM_ADGiftBag adg)
	{
		if (!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Shop))
		{
			miaoShaBtn.gameObject.SetActive(false);
			return;
		}
		if(adg == null)  
		{
			miaoShaBtn.gameObject.SetActive(false);
			return;
		}
		if(adg.isflag_)
		{
			miaoShaBtn.gameObject.SetActive(false);
		}
		else
		{
			miaoShaBtn.gameObject.SetActive(true);
		}
	}

	int buffEffectSlot =0;
	void ShowBuffEffect(int solt)
	{
        COM_State state = GamePlayer.Instance.GetBuff(solt);
        if (state == null)
            return;

        buffEffectSlot = solt;
        StateCellUI scui = UIManager.Instance.AddStateCellUI(stateSlot_[solt], state);
		GameObject clone = (GameObject)GameObject.Instantiate(scui.gameObject);
        clone.GetComponent<StateCellUI>().stateInst = GamePlayer.Instance.GetBuff(solt);
		clone.SetActive(true);
		UIPanel rootPane = ApplicationEntry.Instance.uiRoot.GetComponent<UIPanel>();
		clone.transform.parent = rootPane.transform;
		clone.transform.localScale = Vector3.one;
		clone.transform.parent = stateSlot_[solt].gameObject.transform;
        if(solt == 2)
            stateSlot_[solt].gameObject.SetActive(true);
		clone.AddComponent<TweenPosition>();

		clone.GetComponent<TweenPosition> ().from = clone.transform.localPosition;
		clone.GetComponent<TweenPosition> ().to = Vector3.zero;
		Destroy des = clone.AddComponent<Destroy>();
		des.SetLifeTime (2);;
		clone.GetComponent<TweenPosition> ().SetOnFinished (OnBuffEffectFinish);
		//stateSlot_[solt].gameObject.SetActive(true);
	}
	void  OnBuffEffectFinish()
	{
		stateSlotEffect[buffEffectSlot].gameObject.SetActive(false);
		stateSlotEffect[buffEffectSlot].gameObject.SetActive(true);
	}

    void SetKaitou(bool kaitou)
    {
        isKaitou_ = kaitou;
        rightBottomGroup_.SetActive(!kaitou);
        leftBottomGroup_.SetActive(!kaitou);
        leftTopGroup_.SetActive(!kaitou);
        rightTopGroup_.SetActive(!kaitou);
        //leftGroup_.SetActive(!kaitou);
        rightGroup_.SetActive(!kaitou);
    }

    void ExcuteHideRenwuScriptEvent()
    {
        leftGroup_.SetActive(false);
    }

    void ResetAutoBtnState()
    {
        autoSp.spriteName = "zidong";
        auto.normalSprite = "zidong";
    }

    List<GameObject> gameObjPool_ = new List<GameObject>();
    void MutiActor(Vector2 uipos, XInput.ClickObj[] actors, bool show)
    {
        mutiActorPanel.gameObject.SetActive(show);
        if (show == false)
            return;

        int reduceActor = 0;
        mutiActorPanel.transform.localPosition = uipos;
        GameObject go = null;
        for (int i = 0; i < actors.Length; ++i)
        {
            if (actors[i].aType_ == XInput.ActorType.AT_Npc)
            {
                Npc npc = Prebattle.Instance.FindNpc(actors[i].id_);
                if (npc == null)
                {
                    reduceActor++;
                    continue;
                }

                NpcData npcData = NpcData.GetData(npc.npcId_);
                if (npcData == null)
                {
                    reduceActor++;
                    continue;
                }

                if (i - reduceActor >= gameObjPool_.Count)
                {
                    go = GameObject.Instantiate(mutiActorItem) as GameObject;
                    gameObjPool_.Add(go);
                    go.transform.parent = mutiActorGrid.transform;
                    go.transform.localScale = Vector3.one;
                    go.transform.localPosition = Vector3.zero;
                    UIEventListener.Get(go).onClick += OnClickActorList;
                }
                else
                {
                    go = gameObjPool_[i - reduceActor];
                }
                go.SetActive(true);
                int idx = npcData.Name.IndexOf(' ');
                string npcName = "";
                if (idx >= 0 && idx < npcData.Name.Length)
                    npcName = npcData.Name.Substring(idx + 1);
                else
                    npcName = npcData.Name;
                UILabel label = go.GetComponentInChildren<UILabel>();
                if (label != null)
                    label.text = npcName;
            }
            else if (actors[i].aType_ == XInput.ActorType.AT_OtherPlayer)
            {
                Avatar role = Prebattle.Instance.FindPlayer(actors[i].id_);
                if (role == null)
                {
                    reduceActor++;
                    continue;
                }

                if (i - reduceActor >= gameObjPool_.Count)
                {
                    go = GameObject.Instantiate(mutiActorItem) as GameObject;
                    gameObjPool_.Add(go);
                    go.transform.parent = mutiActorGrid.transform;
                    go.transform.localScale = Vector3.one;
                    go.transform.localPosition = Vector3.zero;
                    UIEventListener.Get(go).onClick += OnClickActorList;
                }
                else
                {
                    go = gameObjPool_[i - reduceActor];
                }
                go.SetActive(true);
                go.GetComponentInChildren<UILabel>().text = role.GetName();
            }

            if (go != null)
            {
                UIEventListener.Get(go).parameter = i;
            }
        }

        for (int i = actors.Length - reduceActor; i < gameObjPool_.Count; ++i)
        {
            gameObjPool_[i].SetActive(false);
        }

        mutiActorGrid.repositionNow = true;
    }

    void OnClickActorList(GameObject go)
    {
        int idx = (int)UIEventListener.Get(go).parameter;
        XInput.Instance.ExcuteActor(idx);
        mutiActorPanel.gameObject.SetActive(false);
    }

    public void UpdateVip()
    {
        vipSt_.spriteName = string.Format("vip_{0}", GamePlayer.Instance.vipLevel_);
        Prebattle.Instance.UpdateVIP(GamePlayer.Instance.InstId, GamePlayer.Instance.vipLevel_);
    }

	void OnEffectFinish(int qid)
	{
		QuestData qdat = QuestData.GetData (qid);
		if(qdat.questType_ != QuestType.QT_Dialog)
			EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_QuestAchiev, ApplicationEntry.Instance.uiRoot.transform);
	}
	public void UpdateMark()
	{
        if (isKaitou_)
            return;

		if(GamePlayer.Instance.achfinId.Count != 0)
		{
			MarOn();
		}			
		else
		{
			MarOff();
		}
			
	}

	public void MarOn()
	{
        if (isKaitou_)
            return;

//		if(ChengjiuBtn.gameObject.activeSelf)
//		ChengjiuBtn.GetComponentInChildren<UISprite> ().MarkOn (UISprite.MarkAnthor.MA_RightTop,-12,-25);
	}
	public void MarOff()
	{
        if (isKaitou_)
            return;

//		if(ChengjiuBtn.gameObject.activeSelf)
//		ChengjiuBtn.GetComponentInChildren<UISprite> ().MarkOff ();
	}
	public void UpdateTrackingQuests()
	{

        //for(int i =0;i<GamePlayer.Instance.completedQuests_.Count;i++)
        //{
        //    for(int j =0;j<NpcRenwuUI.TrackingQuests_.Count;j++)
        //    {
        //        if(GamePlayer.Instance.completedQuests_[i] ==NpcRenwuUI.TrackingQuests_[j].questData_.id_)
        //        {
        //            NpcRenwuUI.TrackingQuests_.RemoveAt(j);
        //        }
        //    }
        //}
        //if (NpcRenwuUI.TrackingQuests_.Count > 0)
        //{
        //    TaskObj.SetActive(true);
        //}else
        //{
        //    TaskObj.SetActive(false);
        //}
	}


	int oldLevel = 0;
	long oldExp = 0;
	long pcurExp =0; 
	private long curExp;
	private long maxExp;

	void ShowEmpAddpos()
	{
		if(EmployessSystem.instance.isShowAddPos)
			employeeAddPos.gameObject.SetActive(true);
		else
			employeeAddPos.gameObject.SetActive(false);
	}


    public void UpdateInfoBar()
    {
        if (gameObject.activeSelf == false || isKaitou_)
            return;

        int crtHp = GamePlayer.Instance.GetIprop(PropertyType.PT_HpCurr);
        int crtMp = GamePlayer.Instance.GetIprop(PropertyType.PT_MpCurr);
		int maxHp = GamePlayer.Instance.GetIprop (PropertyType.PT_HpMax);
		int maxMp = GamePlayer.Instance.GetIprop (PropertyType.PT_MpMax);
//		if (crtHp > maxHp)
//		   crtHp = maxHp;
//		if (crtMp > maxMp)
//		   crtMp = maxMp;
		hpLabel.text = crtHp + "/" + maxHp;
		mpLabel.text = crtMp + "/" +maxMp;
        UISlider hpBar = hpLabel.GetComponentInParent<UISlider>();
        hpBar.value = GamePlayer.Instance.Properties[(int)PropertyType.PT_HpCurr] * 1f / GamePlayer.Instance.Properties[(int)PropertyType.PT_HpMax] * 1f;
        UISlider mpBar = mpLabel.GetComponentInParent<UISlider>();
        mpBar.value = GamePlayer.Instance.Properties[(int)PropertyType.PT_MpCurr] * 1f / GamePlayer.Instance.Properties[(int)PropertyType.PT_MpMax] * 1f;
        levelLabel.text = GamePlayer.Instance.GetIprop(PropertyType.PT_Level).ToString();
		MoneyLabel.text = GamePlayer.Instance.GetIprop(PropertyType.PT_Money).ToString();
		DiamondLabel.text = GamePlayer.Instance.GetIprop(PropertyType.PT_Diamond).ToString();

		curExp = (long)GamePlayer.Instance.Properties[(int)PropertyType.PT_Exp];
		pcurExp = (long)GamePlayer.Instance.Properties[(int)PropertyType.PT_Exp];
		maxExp = ExpData.GetPlayerMaxExp(GamePlayer.Instance.GetIprop(PropertyType.PT_Level));
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level)>1)
		{
			oldLevel = (GamePlayer.Instance.GetIprop(PropertyType.PT_Level)-1);
		}else
		{
			oldExp = 0;
		}
		oldExp =  ExpData.GetPlayerMaxExp(oldLevel);
		palyerExpSlider.value = ((curExp -  oldExp)* 1f) / ((maxExp-oldExp)* 1f) ;
//		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Free) > 0)
//		{
//			//propAddBtn.gameObject.SetActive(true);
//			//iconRed.MarkOn();
//		}else
//		{
//			//propAddBtn.gameObject.SetActive(false);
//			//iconRed.MarkOff();
//		}

    }

	public void OnUpdateBaby(int id)
	{
        if (gameObject.activeSelf == false || isKaitou_)
            return;
		if(GamePlayer.Instance.babies_list_.Count==0)
		{
			babyRed = false;
			return;
		}

		for(int i=0;i<GamePlayer.Instance.babies_list_.Count;i++)
		{
			if(GamePlayer.Instance.babies_list_[i].Properties[(int)PropertyType.PT_Free] > 0)
			{
				babyRed = true;
				break;
			}
			else
			{
				babyRed = false;
			}
		}
	}

    void OnBabyLevelUp(int[] info)
    {
        int guid = info[0];
        int level = info[1];
        //GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BabyLevelUp, GamePlayer.Instance.GetBabyInst(guid).GetIprop(PropertyType.PT_Level));
    }

	void OnLevelUp(int level)
	{
        GamePlayer.Instance.levelIsDirty_ = true;
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_PlayerLevelUp, GamePlayer.Instance.GetIprop(PropertyType.PT_Level));
	}

    public void SetBottomButton(bool display)
    {
        for (int i = 0; i < BottomBtnRoot_.Length; ++i)
        {
            BottomBtnRoot_[i].transform.localScale = display ? Vector3.one : Vector3.zero;
        }
        UIPlayTween[] playTween = bottomTweener.GetComponents<UIPlayTween>();
        for (int i = 0; i < playTween.Length; ++i)
        {
            UITweener[] tweeners = playTween[i].tweenTarget.GetComponents<UITweener>();
            for (int j = 0; j < tweeners.Length; ++j)
            {
                tweeners[j].tweenFactor = display ? 0f : 1f;
            }
        }
    }

//    public void SetTopButton(bool display)
//    {
//        for (int i = 0; i < TopBtnRoot_.Length; ++i)
//        {
//            TopBtnRoot_[i].transform.localScale = display ? Vector3.one : Vector3.zero;
//        }
//        UIPlayTween[] playTween = topTweener.GetComponents<UIPlayTween>();
//        for (int i = 0; i < playTween.Length; ++i)
//        {
//            UITweener[] tweeners = playTween[i].tweenTarget.GetComponents<UITweener>();
//            for (int j = 0; j < tweeners.Length; ++j)
//            {
//                tweeners[j].tweenFactor = display ? 0f : 1f;
//            }
//        }
//    }
	private void OnClickDTBtn(ButtonScript obj, object args, int param1, int param2)
	{
		DatiPanel.SwithShowMe ();
	}

    private void OnClickShowChongBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Shop))
		{
			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_Shop);
		}
		
		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}	

        StoreUI.SwithShowMe(0);
	}

//	private void OnClickNpcPanleObj(ButtonScript obj, object args, int param1, int param2)
//	{
//		NpcListPanle.SwithShowMe ();
//	}
    private void OnBottomBtnTween(ButtonScript obj, object args, int param1, int param2)
    {
        GamePlayer.Instance.BottomBtnState = !GamePlayer.Instance.BottomBtnState;
        if (!GamePlayer.Instance.BottomBtnState)
		{
			//if((BoxSystem.Instance.FreeNum > 0 && BoxSystem.Instance.GreenCDTime<=0) || BoxSystem.Instance.BlueCDTime<=0)
			//{
				// bottomTweener.GetComponentInChildren<UISprite>().MarkOn(UISprite.MarkAnthor.MA_RightCenter);
			//}

			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_MagicItem))
			{
				Transform txObj = magicItemBtn.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
				if(txObj != null)
				{
					txObj.gameObject.SetActive(false);
					Destroy(txObj.gameObject);
				}	
			}

			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Make))
			{
				Transform txObj = makeBtn.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
				if(txObj != null)
				{
					txObj.gameObject.SetActive(false);
					Destroy(txObj.gameObject);
				}	
			}

			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_EmployeeGet) ||GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_EmployeeList )
			   ||GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_EmployeeEquip))
			{
				Transform txObj = HuoBtn.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
				if(txObj != null)
				{
					txObj.gameObject.SetActive(false);
					Destroy(txObj.gameObject);
				}	
			}

			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Baby))
			{
				Transform txObj = babyBtn.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
				if(txObj != null)
				{
					txObj.gameObject.SetActive(false);
					Destroy(txObj.gameObject);
				}	
			}

			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Skill))
			{
				Transform txObj = SkillBtn.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
				if(txObj != null)
				{
					txObj.gameObject.SetActive(false);
					Destroy(txObj.gameObject);
				}	
			}




		}
        else
		{
            bottomTweener.GetComponentInChildren<UISprite>().MarkOff();

			
			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_MagicItem))
			{
				GameObject tx = GameObject.Instantiate(openFunZhuan.gameObject) as GameObject;
				tx.transform.parent = magicItemBtn.gameObject.transform;
				tx.SetActive(true);
				UISprite sp =  getbtnBackground(magicItemBtn);

				EffectLevel ev = tx.AddComponent<EffectLevel>();
				ev.target = sp;
				tx.transform.localPosition = Vector3.zero;
				tx.transform.localScale = Vector3.one;
			}
			
			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Make))
			{
				GameObject tx = GameObject.Instantiate(openFunZhuan.gameObject) as GameObject;
				tx.transform.parent = makeBtn.gameObject.transform;
				tx.SetActive(true);
				UISprite sp =  getbtnBackground(makeBtn);
				EffectLevel ev = tx.AddComponent<EffectLevel>();
				ev.target = sp;
				tx.transform.localPosition = Vector3.zero;
				tx.transform.localScale = Vector3.one;
			}
			
			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_EmployeeGet) ||GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_EmployeeList )
			   ||GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_EmployeeEquip))
			{
				GameObject tx = GameObject.Instantiate(openFunZhuan.gameObject) as GameObject;
				tx.transform.parent = HuoBtn.gameObject.transform;
				tx.SetActive(true);
				UISprite sp =  getbtnBackground(HuoBtn);
				EffectLevel ev = tx.AddComponent<EffectLevel>();
				ev.target = sp;
				tx.transform.localPosition = Vector3.zero;
				tx.transform.localScale = Vector3.one;
			}
			
			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Baby))
			{
				GameObject tx = GameObject.Instantiate(openFunZhuan.gameObject) as GameObject;
				tx.transform.parent = babyBtn.gameObject.transform;
				tx.SetActive(true);
				UISprite sp =  getbtnBackground(babyBtn);
				EffectLevel ev = tx.AddComponent<EffectLevel>();
				ev.target = sp;
				tx.transform.localPosition = Vector3.zero;
				tx.transform.localScale = Vector3.one;
			}

			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Skill))
			{
				GameObject tx = GameObject.Instantiate(openFunZhuan.gameObject) as GameObject;
				tx.transform.parent = SkillBtn.gameObject.transform;
				tx.SetActive(true);
				UISprite sp =  getbtnBackground(SkillBtn);
				EffectLevel ev = tx.AddComponent<EffectLevel>();
				ev.target = sp;
				tx.transform.localPosition = Vector3.zero;
				tx.transform.localScale = Vector3.one;
			}




		}
    }
	private void OnClickchoujiangBtn(ButtonScript obj, object args, int param1, int param2)
	{
//		if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_choujiang))
//		{
//			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_choujiang);
//		}
//
//		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
//		if(txObj != null)
//		{
//			txObj.gameObject.SetActive(false);
//			Destroy(txObj.gameObject);
//		}	
//
//		LotteryPanelUI.ShowMe ();
		//jingcaihuodong

        MoreActivity.SwithShowMe();
	}

	private void OnClickTopRightBtn(ButtonScript obj, object args, int param1, int param2)
	{
		isTopRigthBtnDown = !isTopRigthBtnDown;

		if(isTopRigthBtnDown)
		{
			ShowBtnOpenEffect(payBtn.gameObject,false);
			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Activity))
			{
				ShowBtnOpenEffect(storeBtn.gameObject,false);
			}
			ShowBtnOpenEffect(sigupBtn.gameObject,false);
			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Achieve))
			{
				ShowBtnOpenEffect(ChengjiuBtn.gameObject,false);
			}
			ShowBtnOpenEffect(tishengBtn.gameObject,false);
			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Guid))
			{
				ShowBtnOpenEffect(guideBtn.gameObject,false);
			}
			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_choujiang))
			{
				ShowBtnOpenEffect(choujiangBtn.gameObject,false);
			}
		}
		else
		{
			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Activity))
			{
				ShowBtnOpenEffect(storeBtn.gameObject,true);
			}
			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Achieve))
			{
				ShowBtnOpenEffect(ChengjiuBtn.gameObject,true);
			}
			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Guid))
			{
				ShowBtnOpenEffect(guideBtn.gameObject,true);
			}
			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_choujiang))
			{
				ShowBtnOpenEffect(choujiangBtn.gameObject,true);
			}
		}
	}

    private void OnTopBtnTween(ButtonScript obj, object args, int param1, int param2)
    {
        GamePlayer.Instance.TopBtnState = !GamePlayer.Instance.TopBtnState;
        if (!GamePlayer.Instance.TopBtnState) 
		{
			//topTweener.GetComponentInChildren<UISprite> ().MarkOn (UISprite.MarkAnthor.MA_LeftBottom);

//			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Friend))
//			{
//				Transform txObj = PartnerBtn.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
//				if(txObj != null)
//				{
//					txObj.gameObject.SetActive(false);
//					Destroy(txObj.gameObject);
//				}	
//			}
			
			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Family))
			{
				Transform txObj = familylBtn.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
				if(txObj != null)
				{
					txObj.gameObject.SetActive(false);
					Destroy(txObj.gameObject);
				}	
			}
			
			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Rank))
			{
				Transform txObj = paihangBtn.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
				if(txObj != null)
				{
					txObj.gameObject.SetActive(false);
					Destroy(txObj.gameObject);
				}	
			}




		}
        else
		{
          //  topTweener.GetComponentInChildren<UISprite>().MarkOff();


//			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Friend))
//			{
//				GameObject tx = GameObject.Instantiate(openFunZhuan.gameObject) as GameObject;
//				tx.transform.parent = PartnerBtn.gameObject.transform;
//				tx.SetActive(true);
//				UISprite sp =  getbtnBackground(PartnerBtn);
//				EffectLevel ev = tx.AddComponent<EffectLevel>();
//				ev.target = sp;
//				tx.transform.localPosition = Vector3.zero;
//				tx.transform.localScale = Vector3.one;
//			}
			
			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Family))
			{
				GameObject tx = GameObject.Instantiate(openFunZhuan.gameObject) as GameObject;
				tx.transform.parent = familylBtn.gameObject.transform;
				tx.SetActive(true);
				UISprite sp = getbtnBackground(familylBtn);
				EffectLevel ev = tx.AddComponent<EffectLevel>();
				ev.target = sp;
				tx.transform.localPosition = Vector3.zero;
				tx.transform.localScale = Vector3.one;
			}

			if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Rank))
			{
				GameObject tx = GameObject.Instantiate(openFunZhuan.gameObject) as GameObject;
				tx.transform.parent = paihangBtn.gameObject.transform;
				tx.SetActive(true);
				UISprite sp =  getbtnBackground(paihangBtn);
				EffectLevel ev = tx.AddComponent<EffectLevel>();
				ev.target = sp;
				tx.transform.localPosition = Vector3.zero;
				tx.transform.localScale = Vector3.one;
			}

		}
    }

    private void OnClickNextBtn(ButtonScript obj, object args, int param1, int param2)
    {
        // show zhiyin ui.
        HelpUI.SwithShowMe(1,6);
    }

	private void OnEmployeeAddPosClose(ButtonScript obj, object args, int param1, int param2)
	{
		employeeAddPos.gameObject.SetActive (false);
		EmployessSystem.instance.isShowAddPos = false;
	}

    private void OnClickStore(ButtonScript obj, object args, int param1, int param2)
    {
		if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Activity))
		{
			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_Activity);
		}
		
		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}	
        //SignUpPanel.SwithShowMe();
        HongDongPanel.SwithShowMe();
        GamePlayer.Instance.hasActivityOpen_ = false;
    }

	private void OnClickChengjiu(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Achieve))
		{
			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_Achieve);
		}
		
		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}	
        //if(SuccessSystem.isInitialized_)
        //{
			SuccessPanelUI.ShowMe ();
        //}else
        //{
        //    NetConnection.Instance.requestAchievement();
        //}

	}
	private void OnautoOrCanauto(ButtonScript obj, object args, int param1, int param2)
	{
        if (TeamSystem.IsInTeam() && !TeamSystem.IsTeamLeader() && !TeamSystem.AwayTeam(GamePlayer.Instance.InstId))
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("onlyLeader"));
            return;
        }
		isAuto = !isAuto;
		if(isAuto)
		{
            autoSp.spriteName = "quxiaozidong";
            auto.normalSprite = "quxiaozidong";
            NetConnection.Instance.autoBattle();
		}else
		{
			autoSp.spriteName = "zidong";
			auto.normalSprite = "zidong";
            NetConnection.Instance.stopAutoBattle();
            Prebattle.Instance.ChangeWalkEff(Prebattle.WalkState.WS_Normal);
		}
	}

    void UpdateAME(bool bOk)
    {
        isAuto = bOk;
        if (bOk)
        {
            autoSp.spriteName = "quxiaozidong";
            auto.normalSprite = "quxiaozidong";
            //Prebattle.Instance.clickedQuestId_ = 0;
        }
        else
        {
            autoSp.spriteName = "zidong";
            auto.normalSprite = "zidong";
        }
    }

	private void OnClickMake(ButtonScript obj, object args, int param1, int param2)
	{
		CompoundUI.SwithShowMe ();
		if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Make))
		{
			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_Make);
		}

		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}
		NetConnection.Instance.uiBehavior (UIBehaviorType.UBT_Make);

	}

	private void OnMagicItem(ButtonScript obj, object args, int param1, int param2)
	{
		magicItemUI.SwithShowMe ();

		if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_MagicItem))
		{
			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_MagicItem);
		}
		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}	
		NetConnection.Instance.uiBehavior (UIBehaviorType.UBT_MagicItem);

	}
	private void OnClickDiamond(ButtonScript obj, object args, int param1, int param2)
	{

	}
	private void OnClickpaihang(ButtonScript obj, object args, int param1, int param2)
	{
		ChartsPanelUI.ShowMe ();
		if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Rank))
		{
			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_Rank);
		}

		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}	
	}
	private void OnClickMoney(ButtonScript obj, object args, int param1, int param2)
	{

	}
	private void OnClickTujianBtn(ButtonScript obj, object args, int param1, int param2)
	{
		//TuJianUI.ShowMe ();
	}
    private void OnClickSignBtn(ButtonScript obj, object args, int param1, int param2)
	{
//		SignUpPanel.SwithShowMe();

		FeaturesUIPanel.SwithShowMe ();
		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}	
		NetConnection.Instance.uiBehavior (UIBehaviorType.UBT_SignUp);
	}

//	private void OnClickwordPosition(ButtonScript obj, object args, int param1, int param2)
//	{
//		if(TeamSystem.IsInTeam()&&!TeamSystem.AwayTeam(GamePlayer.Instance.InstId)&&!TeamSystem.IsTeamLeader(GamePlayer.Instance.InstId))
//		{
//			PopText.Instance.Show(LanguageManager.instance.GetValue("duizhangkeyi"));
//		}else
//		{
//			WorldMap.ShowMe ();
//		}
//
//
//	}
	private void OnClicktaskAr(ButtonScript obj, object args, int param1, int param2)
	{
        //if (MainTaskUI.TrackingQuestsInfo != null)
        //{
        //    MainTaskUI.TrackingQuestsInfo(NpcRenwuUI.TrackingQuests_);	
        //}
	}
	private void OnClickbackBtn(ButtonScript obj, object args, int param1, int param2)
	{
        //if (TeamSystem.IsInTeam())
        //{
        //    MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("tishiDuiwu"), () => {
        //        NetConnection.Instance.exitTeam ();
        //        if (PrebattleEvent.getInstance.BackEvent != null) {
        //            PrebattleEvent.getInstance.BackEvent();
        //        }

        //    });

        //    //messageObj.SetActive(true);
        //}
        //else
        //{
        //    if (PrebattleEvent.getInstance.BackEvent != null) {
        //        PrebattleEvent.getInstance.BackEvent();
        //    }
        //}

		if (TeamSystem.IsInTeam() && !TeamSystem.IsTeamLeader(GamePlayer.Instance.InstId)&&!TeamSystem.AwayTeam(GamePlayer.Instance.InstId))
        {
			//PopText.Instance.Show(LanguageManager.instance.GetValue("chuansong"), PopText.WarningType.WT_Tip);
			if(CopyData.IsCopyScene(GameManager.SceneID))
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("likaifuben"),()=>{
					NetConnection.Instance.exitCopy();
					if(PrebattleEvent.getInstance.BackEvent != null)
					{
						PrebattleEvent.getInstance.BackEvent();
					}
					
				});
			}else
			{
				MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("chuansong"), () => {
					
					NetConnection.Instance.leaveTeam();
					if (PrebattleEvent.getInstance.BackEvent != null)
					{
						PrebattleEvent.getInstance.BackEvent();
					}
				});
			}

		}
		else
		{
			if(CopyData.IsCopyScene(GameManager.SceneID))
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("likaifuben"),()=>{
					NetConnection.Instance.exitCopy();
					if(PrebattleEvent.getInstance.BackEvent != null)
					{
						PrebattleEvent.getInstance.BackEvent();
					}
					
				});
			}else
			{
				MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("shifouhuicheng"), () => {
					
					if(PrebattleEvent.getInstance.BackEvent != null)
					{
						PrebattleEvent.getInstance.BackEvent();
					}				 
				});
			}

		}
		
		
	}
	public void SetNpcItemUiObj(bool isActive, int npcId = 0)
	{
        NpcItemUi caiji = NpcItemUiObj.GetComponent<NpcItemUi>();
        if (isActive)
		{
            caiji.Show(npcId);
		}
        else
		{
			caiji.isStop = isActive;
		    NpcItemUiObj.SetActive (false);
		}
	}

    public bool CaijiIng
    {
        get { return NpcItemUiObj.activeSelf; }
    }

	private void OnClickHuoBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_EmployeeGet))
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("cannotopen"));
			//return;
		}
		EmployessControlUI.SwithShowMe ();

		if(employeeAddPos.gameObject.activeSelf)
		{
			employeeAddPos.gameObject.SetActive(false);
			EmployessSystem.instance.isShowAddPos = false;
		}

		if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_EmployeeGet))
		{
			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_EmployeeGet);
		}
		else if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_EmployeeEquip))
		{
			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_EmployeeEquip);
		}
		else if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_EmployeeList))
		{
			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_EmployeeList);
		}


		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}	
		NetConnection.Instance.uiBehavior (UIBehaviorType.UBT_Employee);
	}

	private void OnClickRile(ButtonScript obj, object args, int param1, int param2)
	{
		PlayerPropertyUI.SwithShowMe();
		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}	
		NetConnection.Instance.uiBehavior (UIBehaviorType.UBT_PlayerProperty);

	}
	private void OnClickSkillBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Skill))
		{
			return;
		}

		if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Skill))
		{
			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_Skill);
		}

		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}

		if (GamePlayer.Instance.GetMianSkillList ().Count <= 0)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("qiwuxuexijineng"));
			return;
		}
		SkillViewUI.SwithShowMe ();
		NetConnection.Instance.uiBehavior (UIBehaviorType.UBT_SkillView);
	
	}
//	private void OnClickPartnerBtn(ButtonScript obj, object args, int param1, int param2)
//	{
//		if(!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Friend))
//		{
//			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("cannotopen"));
//			PopText.Instance.Show (LanguageManager.instance.GetValue("cannotopen"));
//			return;
//		}
//		FriendUI.SwithShowMe();
//		if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Friend))
//		{
//			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_Friend);
//		}
//
//		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
//		if(txObj != null)
//		{
//			txObj.gameObject.SetActive(false);
//			Destroy(txObj.gameObject);
//		}	
//		NetConnection.Instance.uiBehavior (UIBehaviorType.UBT_Friend);
//	}
	private void OnClickbabyBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Baby))
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("cannotopen"));
			PopText.Instance.Show (LanguageManager.instance.GetValue("cannotopen"));
			return;
		}
		//BabyPanel.SwithShowMe ();
		//MainBabyPanle.ShowMe ();
		MainbabyUI.SwithShowMe ();
		if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Baby))
		{
			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_Baby);
		}

		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}	
		NetConnection.Instance.uiBehavior (UIBehaviorType.UBT_Baby);
	}

	private void OnClickBag(ButtonScript obj, object args, int param1, int param2)
	{
		BagUI.SwithShowMe ();
		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}	
		NetConnection.Instance.uiBehavior (UIBehaviorType.UBT_Bag);
	}
	private void OnClickpayBtn(ButtonScript obj, object args, int param1, int param2)
	{ 
		if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Shop))
		{
			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_Shop);
		}
		
		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}	

		StoreUI.SwithShowMe (2);
	}

	private void OnClickmrTask(ButtonScript obj, object args, int param1, int param2)
	{
	//	DailyTaskUI.ShowMe ();
	}

    private void OnClickSetting(ButtonScript obj, object args, int param1, int param2)
	{
		SetPanelUI.SwithShowMe ();
		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}	

        //SignUpPanel.SwithShowMe();
	}

    private void OnClickVip(ButtonScript obj, object args, int param1, int param2)
	{
      //  VipPanel.SwithShowMe();
	}

    private void OnClickTisheng(ButtonScript obj, object args, int param1, int param2)
    {
       // raisePanel_.gameObject.SetActive(true);
		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}	
		raisePanel_.Show ();
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_ClickRaiseUpBtn);
    }

	private void OnClickFamily(ButtonScript obj, object args, int param1, int param2)
	{

		if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Family))
		{
			PopText.Instance.Show (LanguageManager.instance.GetValue("cannotopen"));
			return;
		}
		if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Family))
		{
			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_Family);
		}

		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}
        if (GuildSystem.IsInGuild())
        {
            MyFamilyInfo.ShowMe();
        }
        else
        {
            FamilyPanelUI.ShowMe();
        }
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_ClickMainFamily);
	}

	private void openDouble(ButtonScript obj, object args, int param1, int param2)
	{

		if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_DoubleExp))
		{
			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_DoubleExp);
		}
		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}	
		FeaturesUIPanel.ShowMe ();
//		MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("opendoubletime"),()=>{
//		
//			NetConnection.Instance.setOpenDoubleTimeFlag (true);
//		});
	}

	private void closeDouble(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_DoubleExp))
		{
			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_DoubleExp);
		}
		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}	
		FeaturesUIPanel.ShowMe ();
//		MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("clostdoubletime"),()=>{
//			NetConnection.Instance.setOpenDoubleTimeFlag (false);
//		});
	}

    private void OnClickGatherBtn(ButtonScript obj, object args, int param1, int param2)
    {
		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}	
		//GatherUI.SwithShowMe();
		//GatherUI.ShowMe ();
		NetConnection.Instance.uiBehavior (UIBehaviorType.UBT_Gather);
    }

	private void OnClickGuide(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Guid))
		{
			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_Guid);
		}
		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}	
		HelpUI.SwithShowMe ();
		NetConnection.Instance.uiBehavior (UIBehaviorType.UBT_Help);
	}

	private void OnFriendNews(ButtonScript obj, object args, int param1, int param2)
	{
		obj.gameObject.SetActive (false);


		FriendUI.SwithShowMe ();
	}
	private void OnClickEmail(ButtonScript obj, object args, int param1, int param2)
	{
		EmailUI.SwithShowMe ();
		NetConnection.Instance.uiBehavior (UIBehaviorType.UBT_Email);
	}
	private void OnClickMiaosha(ButtonScript obj, object args, int param1, int param2)
	{
		MiaoshaUI.SwithShowMe ();
	}


	private void OnClickLevelRewardShopBtn(ButtonScript obj, object args, int param1, int param2)
	{
		LevelRewardShopUI.SwithShowMe ();
	}

	private void OnClickSevenDay(ButtonScript obj, object args, int param1, int param2)
	{
		MA_7Days.SwithShowMe ();
	}

	private void OnClickGem(ButtonScript obj, object args, int param1, int param2) 
	{
		if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Cystal))
		{
			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_Cystal);
		}
		
		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}	

		GemPanelUI.SwithShowMe ();
	}

    private void OnClickAuctionBtn(ButtonScript obj, object args, int param1, int param2)
    {
        if (AuctionHouseSystem.Open_ == false)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("AuctionHouseClosed"), PopText.WarningType.WT_Warning);
           return;
        }
		if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_AuctionHouse))
		{
			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_AuctionHouse);
		}

		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}	

        AuctionHousePanel.SwithShowMe();
//		NetConnection.Instance.uiBehavior (UIBehaviorType.UBT_Auction);
    }

	void BtnClick(GameObject sendr)
	{

	}
	public override void Destroyobj ()
	{
        XInput.Instance.OnMoveBegin -= BeginStick;
        XInput.Instance.OnMove -= MoveStick;
        XInput.Instance.OnMoveEnd -= EndStick;
		Prebattle.Instance.OnTouchOtherPlayer -= OnTouchOPlayer;
        GamePlayer.Instance.OnIPropUpdate -= UpdateInfoBar;
		GamePlayer.Instance.babyUpdateIpropEvent -= OnUpdateBaby;
		GamePlayer.Instance.OpenSubSystemEnvet -= UpdateOpenSystem;
		GamePlayer.Instance.OpenSystemEnvetString -= UpdateOpenSystemStr;
		GamePlayer.Instance.PlayerLevelUpEvent -= OnLevelUp;
        GamePlayer.Instance.OnAchievementUpdate -= UpdateMark;
        //GamePlayer.Instance.OnStateUpdate -= UpdateState;
        //GamePlayer.Instance.OnAutoMeetEnemy -= UpdateAME;
        RaiseUpSystem.OnUpdateRaisePanelUI -= UpdateRaisePanel;
        //GatherSystem.instance.MineTimeout -= ShowGatehrRed;
        GamePlayer.Instance.OnVipUpdate -= UpdateVip;
        ActivitySystem.Instance.OnActivityOpen -= OnActivityOpen;
        FastUpload.Instance.OnClose -= HideFastUse;
		BagSystem.instance.DelItemEvent -= OnDelBagNum;
		BagSystem.instance.ItemChanged -= OnAddBagNum;
		BagSystem.instance.UpdateItemEvent -= OnAddBagNum;
		GamePlayer.Instance.doubleTimeEnvet -= OnDoubleTimeEnvet;
		EmployessSystem.instance.employeeRedEnvent -= OnEmployeeRedEvent;
		FriendSystem.Instance ().FriendChat -= OnFriendNewsEvent;
		ArenaPvpSystem.Instance.OpenPvpUIEnven -= PvpUIEnvet;
		UIManager.Instance.showMainPanelEnvent -= ShowMainPanel;
		MoreActivityData.instance.MoreActivityRedEvent -= MoreActivityRedEvent;
        GameScript.OnHideRenwuList -= ExcuteHideRenwuScriptEvent;
		FriendSystem.Instance().UpdateFriend -= OnUpdataFriendList;
		GuildSystem.updateGuildBattleEventOk -= updateGuildBattleOk;
		DisplayFPS.OnTimeUpdate -= UpdateTime;
		DisplayFPS.OnBatteryUpdate -= UpdateBattery;
		DisplayFPS.OnPingUpdate -= UpdatePing;
        //GamePlayer.Instance.AddBuffEnvet -= ShowBuffEffect;
		GamePlayer.Instance.getShouChongEnvet -= OnGetShouChongEnvet;
		GamePlayer.Instance.MiaoshaEnvet -= OnMiaoshaEnvet;
		GamePlayer.Instance.levelRewadEnvet -= OnLecelRewardShopEnvet;
        XInput.Instance.OnTouchMutiActor -= MutiActor;
		GuildSystem.updateGuildBattleWinEventOk -= UpdateguildBattleWinCount;
		GuildSystem.startGuildBattleOk -= StartBattleState;
		ARPCProxy.OnAddPlayerTitle -= AddPlayerTitle;
		//GlobalInstanceFunction.Instance.GuildBattCounDownTime -= GuildBattCounDownTimer;
        for (int i = 0; i < gameObjPool_.Count; ++i)
        {
            UIEventListener.Get(gameObjPool_[i]).onClick -= OnClickActorList;
            Destroy(gameObjPool_[i]);
        }
		//QuestSystem.OnQuestEffectFinish -= OnEffectFinish;
		//DailyTaskUI.OnMar -= ShowDaTask;
		Destroy (gameObject);
	}


    float stickRadius_ = 100f;
    Vector2 mouseUIPos_, stickFinalPos_;

    public void MoveStick(float x, float y)
    {
        mouseUIPos_ = GlobalInstanceFunction.ScreenToUI(Input.mousePosition);
        stickFinalPos_ = (mouseUIPos_ - originStickCorePos_);

        float length = stickFinalPos_.magnitude;
        stickFinalPos_ = stickFinalPos_.normalized * (length > stickRadius_? stickRadius_: length);
        stickCore_.transform.localPosition = stickFinalPos_;//new Vector2(originStickCorePos_.x + finalPlus.x, originStickCorePos_.y + finalPlus.y);
    }

    public void BeginStick(Vector2 pos)
    {
        //Stick_.SetActive(true);
        //Stick_.transform.localPosition = pos;
        //originStickCorePos_ = GlobalInstanceFunction.ScreenToUI(Input.mousePosition);
    }

    public void EndStick()
    {
        //Stick_.SetActive(false);
        //stickCore_.transform.localPosition = Vector3.zero;
    }

	// Update is called once per frame
	void Update() 
	{
		if(roleBtn.gameObject.activeSelf)
		{
			if(GamePlayer.Instance.Properties[(int)PropertyType.PT_Free] > 0)
			{
				if(roleBtn.GetComponentInChildren<UISprite>()!= null)
				roleBtn.GetComponentInChildren<UISprite>().MarkOn(UISprite.MarkAnthor.MA_RightTop,0,0);
			}else
			{
				if(roleBtn.GetComponentInChildren<UISprite>()!= null)
				roleBtn.GetComponentInChildren<UISprite>().MarkOff();
			}
			
		}
		ShowGetEmployeesRed ();
		if(GuildSystem.battleState == 1)
		{
			jifengSelfLabel.text = GuildSystem.Mguild.guildName_;
			jifenLabel.text = GuildSystem.otherName;
			xingdongliLabel.text = LanguageManager.instance.GetValue("kaishijishi").Replace("{n}",GlobalInstanceFunction.Instance.StateTime);
		}

        if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level) >= GamePlayer.Instance.openADActivityLv)
        {
            if (choujiangBtn.gameObject.activeSelf && GamePlayer.Instance.adTypes.Count == 0)
            {
                choujiangBtn.gameObject.SetActive(false);
            }

            if (!choujiangBtn.gameObject.activeSelf && GamePlayer.Instance.adTypes.Count > 0)
            {
                choujiangBtn.gameObject.SetActive(true);
            }
        }
	
        if (!fastUsePanel_.activeSelf)
        {
            ShowFastUse(FastUpload.Instance.GetOne());
        }

		if(GamePlayer.Instance.isOpenDoubleTime)
		{
			doubleTimeLab.text = FormatTimeHasHour ((int)GamePlayer.Instance.getDoubleTime); 
		}

		if(EmailSystem.instance.Mails.Length > 0)
		{
            bool hasNew = false;
			foreach(var x in EmailSystem.instance.Mails)
			{
				if(x == null)
					continue;
				if(!x.isRead_)
				{
					emailImg.gameObject.SetActive(true);
                    hasNew = true;
				}
			}

            if (!hasNew)
			    emailImg.gameObject.SetActive(false);
		}
		else
		{
			if(emailImg.gameObject.activeSelf)
			{
				emailImg.gameObject.SetActive(false);
			}
		}

        if (GamePlayer.Instance.levelIsDirty_)
        {
            if (!GamePlayer.Instance.nextFuncClose_)
            {
                HelpLevelData hlData = HelpLevelData.NextData(GamePlayer.Instance.GetIprop(PropertyType.PT_Level));
                if (hlData != null)
                {
                    nextbtn.gameObject.SetActive(true);
                    nextlbl.text = string.Format(LanguageManager.instance.GetValue("OpenByLv"), hlData.level);
                    nexticon.spriteName = hlData.icon;
                    Debug.Log(nexticon.spriteName);
                }
                else
                    nextbtn.gameObject.SetActive(false);
            }
            else
                nextbtn.gameObject.SetActive(false);

            GamePlayer.Instance.levelIsDirty_ = false;
        }

        if(GamePlayer.Instance.buffIsDirty_)
        {
            for (int i = 0; i < GamePlayer.Instance.buffList_.Count; ++i)
            {
                if (i > 2)
                    break;

                //checkbuff只执行一次
                if (GameManager.Instance.noNeedCheckBuff_ == false)
                {
                    if (GlobalValue.isBattleScene(StageMgr.preScene_))
                    {
                        int left = GamePlayer.Instance.buffList_[i].value0_;
                        GameManager.Instance.procCheckBuff_ = true;
                        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_CheckBuff, (int)GamePlayer.Instance.buffList_[i].stateId_, left);
                        GameManager.Instance.procCheckBuff_ = false;
                    }
                }
            }

            if (GamePlayer.Instance.newBuffSlot.Count == 0)
            {
                COM_State hpb = GetHpBuff();
                if (hpb != null)
                {
                    scuiHp = UIManager.Instance.AddStateCellUI(stateSlot_[0], hpb);
                    stateSlot_[0].gameObject.GetComponent<BoxCollider>().enabled = false;
                }
                else
                {
                    stateSlot_[0].gameObject.GetComponent<BoxCollider>().enabled = true;
                    if (scuiHp != null)
                        Destroy(scuiHp);
                }

                COM_State mpb = GetMpBuff();
                if (mpb != null)
                {
                    scuiMp = UIManager.Instance.AddStateCellUI(stateSlot_[1], mpb);
                    stateSlot_[1].gameObject.GetComponent<BoxCollider>().enabled = false;
                }
                else
                {
                    stateSlot_[1].gameObject.GetComponent<BoxCollider>().enabled = true;
                    if (scuiMp != null)
                        Destroy(scuiMp);
                }

                COM_State we = GetWorldEXP();
                if (we != null)
                {
                    UIManager.Instance.AddStateCellUI(stateSlot_[2], we);
                    stateSlot_[2].gameObject.SetActive(true);
                }
                else
                {
                    stateSlot_[2].gameObject.SetActive(false);
                    if (worldExp != null)
                        Destroy(worldExp);
                }
            }
            else
            {
                for (int i = 0; i < GamePlayer.Instance.newBuffSlot.Count; ++i)
                {
                    ShowBuffEffect(GamePlayer.Instance.newBuffSlot[i]);
                }
                GamePlayer.Instance.newBuffSlot.Clear();
            }
            GamePlayer.Instance.buffIsDirty_ = false;
        }


		if(levelRewardShopBtn.gameObject.activeSelf)
		{
			levelRewardTimeLab.text =  FormatLevelShopTimeHasHour((int)GamePlayer.Instance.ShopCDTime);
		}


	}

	public void UpdateOpenSystemStr(int str)
	{ 

		if(str == (int)OpenSubSystemFlag.OSSF_EmployeePos10 || str == (int)OpenSubSystemFlag.OSSF_EmployeePos15 ||str == (int)OpenSubSystemFlag.OSSF_EmployeePos20)
		{
			if(!this.gameObject.activeSelf)
			{
				return;
			}
			EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_kaiqihuoban, gameObject.transform,()=>{});

			employeeAddPos.gameObject.SetActive(true);
			EmployessSystem.instance.isShowAddPos = true;
			return;	
		}


		GamePlayer.Instance.OpenFunEffectBtns.Add (str);

		GameObject obj = getBtnObj (str);
		if(obj != null)
		{
			GameObject tx = GameObject.Instantiate(openFunZhuan.gameObject) as GameObject;

			tx.transform.parent = obj.transform;
			tx.SetActive(true);
			UISprite sp = getbtnBackground(obj);
			EffectLevel ev = tx.AddComponent<EffectLevel>();
			ev.target = sp;
			tx.transform.localPosition = Vector3.zero;
			tx.transform.localScale = Vector3.one;
		}

		if(str == (int)OpenSubSystemFlag.OSSF_MagicItem )
		{
			MagicOpenUI.SwithShowMe();
			return;
		}

		OpenFunction.Instance.Show(Enum.GetName(typeof(OpenSubSystemFlag),str),obj);
	}


	void OnDelBagNum(uint slot)
	{
		if(BagSystem.instance.BagIsFull())
		{
			bagfull.gameObject.SetActive(true);
		}
		else
		{
			bagfull.gameObject.SetActive(false);
		}
	}

	void OnAddBagNum(COM_Item item)
	{
		if(BagSystem.instance.BagIsFull())
		{
			bagfull.gameObject.SetActive(true);
		}
		else
		{
			bagfull.gameObject.SetActive(false);
		}
	}

	void OnDoubleTimeEnvet(float time)
	{
		UpdateDoubleTime();
	}

	void OnEmployeeRedEvent(int inst)
	{
		if(inst == -1)
		{
			employeeEvolveRed = false;
		}
		else
		{
			employeeEvolveRed = true;
		}

	}

	void OnFriendNewsEvent(COM_ContactInfo contact,COM_Chat msg)
	{
		if(!FriendSystem.Instance().isOpenFried)
			friendNewsBtn.gameObject.SetActive (true);
	}


	void PvpUIEnvet(bool show)
	{
		if(show)
		{
			leftBottomGroup_.gameObject.SetActive(false);
		}
		else
		{
			leftBottomGroup_.gameObject.SetActive(true);
		}
	}


	void ShowMainPanel(bool show)
	{
		this.gameObject.SetActive(show);
	}

	void MoreActivityRedEvent(int num)
	{
		if(choujiangBtn == null || !choujiangBtn.gameObject.activeSelf)
		{
			return;
		}
		int[]redList_ = MoreActivityData.instance.redList;

		for(int i=0;i<redList_.Length;i++)
		{
			if(redList_[i] == 1)
			{
				fuliBtnImg.MarkOn(UISprite.MarkAnthor.MA_RightTop,-12,-25);
				return;
			}
		}
		fuliBtnImg.MarkOff();


	}

	void UpdateDoubleTime()
	{
		if(GamePlayer.Instance.isOpenDoubleTime) 
		{
			openDoubleBtn.gameObject.SetActive(false);
			closeDoubleBtn.gameObject.SetActive(true);
		}
		else
		{
			openDoubleBtn.gameObject.SetActive(true);
			closeDoubleBtn.gameObject.SetActive(false);
		}
		doubleTimeLab.text = FormatTimeHasHour (GamePlayer.Instance.GetIprop(PropertyType.PT_DoubleExp)); 
	}


	public  string FormatTimeHasHour(int time)
	{
		int hour = time/3600;
		int min = (time%3600)/60;
		int second = time%60;
		return DoubleTime (hour) + ":" + DoubleTime (min);//+ ":" + DoubleTime(second);
	}


	public  string FormatLevelShopTimeHasHour(int time)
	{
		int hour = time/3600;
		int min = (time%3600)/60;
		int second = time%60;
		return DoubleTime(hour) + ":" + DoubleTime(min) + ":" + DoubleTime(second);
	}	

	public string DoubleTime(int time)
	{
		return (time > 9)?time.ToString():("0" + time);
	}



	public void UpdateOpenSystem(ulong open)
	{
        if (isKaitou_)
            return;

		babyBtn.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Baby));
		SkillBtn.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Skill));
		//PartnerBtn.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Friend));
		HuoBtn.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_EmployeeGet));
		makeBtn.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Make));
		guideBtn.gameObject.SetActive (GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Guid));
		magicItemBtn.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_MagicItem));
		bool b = GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Cystal);
		gemBtn.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Cystal));
		shouchongBtn.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Shop));
		payBtn.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Shop));
		OnMiaoshaEnvet (GamePlayer.Instance.miaoshaData_);
		//miaoShaBtn.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Shop));
		paimaihangBtn.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_AuctionHouse));

		ChengjiuBtn.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Achieve));
		paihangBtn.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Rank));
		//TujianBtn.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Handbook));
		//settingBtn.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Setting));
		//mrTaskBtn.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_EveryTask));
		storeBtn.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Activity));
		familylBtn.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Family));
		doubleExpObj.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_DoubleExp));
		choujiangBtn.gameObject.SetActive (GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_choujiang));
		sevenDayBtn.gameObject.SetActive(GamePlayer.Instance.GetADTypeIsOpen (ADType.ADT_7Days));
//		if( /*paihangBtn.gameObject.activeSelf ||*/PartnerBtn.gameObject.activeSelf || familylBtn.gameObject.activeSelf )
//		{
//			topTweener.gameObject.SetActive(true);
//		}
//		else
//		{
//			topTweener.gameObject.SetActive(false);
//		}


	}
    public	void RewardPrice()
	{
		int money = GamePlayer.Instance.GetIprop(PropertyType.PT_Money);
		if(money >ACT_RewardData.minprice_())
		{
			ShowDaTask();
		}
	}

	public void ShowGatehrRed()
	{
        if (isKaitou_)
            return;

        if (!makeBtn.gameObject.activeSelf)
            return;
        //if(GatherSystem.instance.MineItemList.Count > 0 && GatherSystem.instance.GatherTime <= 0)
        //{
        //    UISprite sp = makeBtn.GetComponentInChildren<UISprite>();
        //    if(sp != null)
        //        sp.MarkOn(UISprite.MarkAnthor.MA_RightTop,-6,-20);
        //}
        //else
        //{
        //    UISprite sp = makeBtn.GetComponentInChildren<UISprite>();
        //    if(sp != null)
        //        sp.MarkOff();
        //}
	}



    bool freeCollectFlag = false;
	public void ShowGetEmployeesRed()
	{
        if (isKaitou_)
            return;

		if(_HuobanBtnSprite == null)
		{
			_HuobanBtnSprite = HuoBtn.GetComponentInChildren<UISprite>();
		}
			 
        if (_HuobanBtnSprite == null)
        {
            return;
        }

		if((BoxSystem.Instance.FreeNum > 0 && BoxSystem.Instance.GreenCDTime<=0) || BoxSystem.Instance.BlueCDTime<=0)
		{

			employeeFreeRed = true;
			if (!freeCollectFlag)
			{
				RaiseUpSystem.OnPartnerCollect();
				freeCollectFlag = true;
			}
         /*   if (_HuobanBtnSprite.gameObject.activeSelf)
            {

               // _HuobanBtnSprite.MarkOn();
				_HuobanBtnSprite.MarkOn(UISprite.MarkAnthor.MA_RightTop,-12,-25);
            }
            */
		}
		else
		{
			employeeFreeRed = false;

			if (freeCollectFlag)
			{
				RaiseUpSystem.ResetRaise(RaisePanel.RaiseType.RT_PartnerCollect);
				freeCollectFlag = false;
			}
          /*  if (_HuobanBtnSprite.gameObject.activeSelf)
            {

                _HuobanBtnSprite.MarkOff();
            }
            */
		}


		if(EmployessSystem.instance.GetBattleEmpty())
		{
			employeePosRed = true;
		}
		else
		{
			employeePosRed = false;
		}


		if(employeeFreeRed || employeeEvolveRed || employeePosRed)
		{
			if (_HuobanBtnSprite.gameObject.activeSelf)
			{
				_HuobanBtnSprite.MarkOn(UISprite.MarkAnthor.MA_RightTop,-12,-25);
			}

		}
		else
		{
			 if (_HuobanBtnSprite.gameObject.activeSelf)
            {
          
                _HuobanBtnSprite.MarkOff();
            }
		}


		if(babyRed)
		{
			if(GamePlayer.Instance.BattleBaby!=null)
			{
				if(babyBtn.gameObject.activeSelf)
					babyBtn.GetComponentInChildren<UISprite>().MarkOn(UISprite.MarkAnthor.MA_RightBottom,0,0);
			}

		}
		else
		{
			if(GamePlayer.Instance.BattleBaby!=null)
			{
				if(babyBtn.gameObject.activeSelf)
					babyBtn.GetComponentInChildren<UISprite>().MarkOff();
			}

		}
		if(roleBtn.gameObject.activeSelf)
		{
			if(GamePlayer.Instance.Properties[(int)PropertyType.PT_Free] > 0)
			{
				roleBtn.GetComponentInChildren<UISprite>().MarkOn(UISprite.MarkAnthor.MA_RightTop,0,0);
			}else
			{
				roleBtn.GetComponentInChildren<UISprite>().MarkOff();
			}
		
		}
		if(GuildSystem.IsInGuild())
		{
			if(GuildSystem.IsGuildMessage())
			{
				familylBtn.GetComponentInChildren<UISprite>().MarkOn(UISprite.MarkAnthor.MA_RightTop,-12,-25);
			}else
			{
				familylBtn.GetComponentInChildren<UISprite>().MarkOff();
			}
		}
		if(SuccessSystem.isDirty)
		{
			if(SuccessSystem.isReceived())
			{
				if(ChengjiuBtn.GetComponentInChildren<UISprite>() != null)
				ChengjiuBtn.GetComponentInChildren<UISprite>().MarkOn(UISprite.MarkAnthor.MA_RightTop,-12,-25);
			}else
			{
				if(ChengjiuBtn.GetComponentInChildren<UISprite>() != null)
				ChengjiuBtn.GetComponentInChildren<UISprite>().MarkOff();
			}
			//SuccessSystem.isDirty = false;
		}
			



	}

	public void ShowDaTask()
	{
       // if (isKaitou_)
         //   return;

	//if(mrTaskBtn.gameObject.activeSelf)
			//mrTaskBtn.GetComponentInChildren<UISprite> ().MarkOn(UISprite.MarkAnthor.MA_LeftTop,-10,-10);
	}
	public void HidenDaTask()
	{
       // if (isKaitou_)
          //  return;

		//if(mrTaskBtn.gameObject.activeSelf)
		//	mrTaskBtn.GetComponentInChildren<UISprite> ().MarkOff ();
	}

	private GameObject getBtnObj(int fun)
	{
		switch(fun)
		{
		case (int)OpenSubSystemFlag.OSSF_Baby:
			return babyBtn.gameObject;
		case (int)OpenSubSystemFlag.OSSF_Skill:
			return SkillBtn.gameObject;
//		case (int)OpenSubSystemFlag.OSSF_Friend:
//			return PartnerBtn.gameObject;
		case (int)OpenSubSystemFlag.OSSF_EmployeeGet:
			return HuoBtn.gameObject;
		case (int)OpenSubSystemFlag.OSSF_Rank:
			return paihangBtn.gameObject;
		//case (int)OpenSubSystemFlag.OSSF_Handbook:
		//	return TujianBtn.gameObject;
			//break;
		case (int)OpenSubSystemFlag.OSSF_Setting:
			return settingBtn.gameObject;
		//case (int)OpenSubSystemFlag.OSSF_EveryTask:
			//return mrTaskBtn.gameObject;
			//break;
		case (int)OpenSubSystemFlag.OSSF_Activity:
			return storeBtn.gameObject;
		case (int)OpenSubSystemFlag.OSSF_Make:
			return makeBtn.gameObject;
		case (int)OpenSubSystemFlag.OSSF_Achieve:
			return ChengjiuBtn.gameObject;
		case (int)OpenSubSystemFlag.OSSF_Family:
			return familylBtn.gameObject;
		case (int)OpenSubSystemFlag.OSSF_DoubleExp:
			return openDoubleBtn.gameObject;
		case (int)OpenSubSystemFlag.OSSF_AuctionHouse:
			return paimaihangBtn.gameObject;
		case (int)OpenSubSystemFlag.OSSF_MagicItem:
			return magicItemBtn.gameObject;
		case (int)OpenSubSystemFlag.OSSF_choujiang:
			return choujiangBtn.gameObject;
		case (int)OpenSubSystemFlag.OSSF_Guid:
			return guideBtn.gameObject;
		case (int)OpenSubSystemFlag.OSSF_Shop:
			return payBtn.gameObject;
		case (int)OpenSubSystemFlag.OSSF_Cystal:
			return gemBtn.gameObject;
		}

		return null;
	} 
	UISprite getbtnBackground(GameObject obj)
	{
		UISprite [] sps = obj.GetComponentsInChildren<UISprite> (true);
		if (sps.Length == 0)
			return null;
		return sps [0];
	}
	UISprite getbtnBackground(UIButton obj)
	{
		UISprite [] sps = obj.GetComponentsInChildren<UISprite> (true);
		if (sps.Length == 0)
			return null;
		return sps [0];
	}
    void OnActivityOpen(int activityId)
    {
        if (GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Activity) == false)
            return;



        DaliyActivityData data = DaliyActivityData.GetData(activityId);
        if (data.joinLv_ > GamePlayer.Instance.GetIprop(PropertyType.PT_Level))
            return;

        GamePlayer.Instance.hasActivityOpen_ = true;
        //MessageBoxUI.ShowMe(string.Format(LanguageManager.instance.GetValue("ActivityOpen"), data.activityName_), () =>
        //{
        //    GamePlayer.activityType_ = data.activityType_;
        //    HongDongPanel.SwithShowMe();
        //    NetConnection.Instance.uiBehavior (UIBehaviorType.UBT_Activity);
        //});

        //添加特效
        GameObject obj2 = getBtnObj((int)OpenSubSystemFlag.OSSF_Activity);
        Transform eff = obj2.transform.FindChild("lizixuanzhuan(Clone)");
        if (eff == null)
        {
            GameObject tx = GameObject.Instantiate(openFunZhuan.gameObject) as GameObject;

            tx.transform.parent = obj2.transform;
            tx.SetActive(true);
			UISprite sp =  getbtnBackground(obj2);
			EffectLevel ev = tx.AddComponent<EffectLevel>();
			ev.target = sp;
            tx.transform.localPosition = Vector3.zero;
            tx.transform.localScale = Vector3.one;
        }
    }

    void ShowFastUse(COM_Item item)
    {
        if (item == null)
        {
            fastUsePanel_.SetActive(false);
            FastUpload.preItem = null;
            return;
        }
        ItemData _itemData = ItemData.GetData((int)item.itemId_);
        if (_itemData == null)
        {
            return;
        }

        if(_itemData.mainType_ == ItemMainType.IMT_Equip)
        {
            if (GamePlayer.Instance.GetIprop(PropertyType.PT_Level) / 10 + 1 < _itemData.level_)
            {
                return;
            }

            JobType jt = (JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession);
            int level = GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel);
            Profession profession = Profession.get(jt, level);
            if (null == profession)
                return;

            if (_itemData.slot_ == EquipmentSlot.ES_Ornament_0 || _itemData.slot_ == EquipmentSlot.ES_Ornament_1)
            {

            }
            else
            {
                if (!profession.canuseItem(_itemData.subType_, _itemData.level_))
                {
                    return;
                }
            }
        }
        FastUpload.preItem = item;
        fastUsePanel_.GetComponent<FastUsePanel>().SetData(item);
    }

    void HideFastUse()
    {
        fastUsePanel_.SetActive(false);
    }

    void OnBeginLoadScene()
    {
        if(raisePanel_ != null)
            Destroy(raisePanel_.gameObject);
    }


	private void ShowBtnOpenEffect(GameObject obj, bool show)
	{
		if(!show)
		{
			Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
			if(txObj != null)
			{
				txObj.gameObject.SetActive(false);
			    Destroy(txObj.gameObject);
			}	
		}
		else
		{
			GameObject tx = GameObject.Instantiate(openFunZhuan.gameObject) as GameObject;
			tx.transform.parent = obj.gameObject.transform;
			tx.SetActive(true);
			UISprite sp =  getbtnBackground(obj);
			EffectLevel ev = tx.AddComponent<EffectLevel>();
			ev.target = sp;
			tx.transform.localPosition = Vector3.zero;
			tx.transform.localScale = Vector3.one;
		}
	}

}

class FastUpload
{
    static COM_Item[] bodySlotShadow = new COM_Item[(int)EquipmentSlot.ES_Max];
    static List<COM_Item> fastUse = new List<COM_Item>();

    public delegate void CloseCurrentHandler();
    public event closeArenaCheckEvent OnClose;

    static FastUpload inst_ = new FastUpload();
    static public FastUpload Instance
    {
        get
        {
            if (inst_ == null)
                inst_ = new FastUpload();
            return inst_;
        }
    }

    public FastUpload()
    {
        BagSystem.instance.ItemChanged += OnItemAdd;
        BagSystem.instance.DelItemInstEvent += OnItemDel;
    }

    public static COM_Item preItem = null;

    public uint preInst = 0;
    public COM_Item GetOne()
    {
        COM_Item item;
        for (int i = 0; i < bodySlotShadow.Length; ++i)
        {
            if (bodySlotShadow[i] != null)
            {
                if (bodySlotShadow[i].instId_ == preInst)
                {
                    bodySlotShadow[i] = null;
                    preInst = 0;
                    continue;
                }

                if (BagSystem.instance.GetItemByInstId((int)bodySlotShadow[i].instId_) == null)
                {
                    bodySlotShadow[i] = null;
                    continue;
                }

                preInst = bodySlotShadow[i].instId_;
                item = bodySlotShadow[i];
                bodySlotShadow[i] = null;
                return item;
            }
        }

        if (fastUse.Count > 0)
        {
            item = fastUse[0];
            fastUse.RemoveAt(0);
            return item;
        }

        return null;
    }
	public void UseAll()
	{
		COM_Item item;
		ItemData data;
		for (int i = 0; i < bodySlotShadow.Length; ++i)
		{
			if (bodySlotShadow[i] != null)
			{
				item = bodySlotShadow[i];
				data = ItemData.GetData((int)item.itemId_);
				if (data.slot_ == EquipmentSlot.ES_SingleHand)
				{
					if (GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand] != null)
					{
						return;
					}
				}
				else if (data.slot_ == EquipmentSlot.ES_DoubleHand)
				{
					if (GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_SingleHand] != null)
					{
						return;
					}
				}
				
				if (GamePlayer.Instance.GetIprop(PropertyType.PT_Level) / 10 + 1 < data.level_)
				{
					return;
				}
				
				JobType jt = (JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession);
				int level = GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel);
				Profession profession = Profession.get(jt, level);
				if (null == profession)
					return;
				
				if (data.slot_ == EquipmentSlot.ES_Ornament_0 || data.slot_ == EquipmentSlot.ES_Ornament_1)
				{
					
				}
				else
				{
					if (!profession.canuseItem(data.subType_, data.level_))
					{
						return;
					}
				}
				
				if (data.slot_ == EquipmentSlot.ES_SingleHand)
				{
					if (data.subType_ == ItemSubType.IST_Shield)
					{
						if (GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand] != null)
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId, GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand].instId_);
						}
					}
					else
					{
						if (GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand] != null && ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand].itemId_).subType_ != ItemSubType.IST_Shield)
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId, GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand].instId_);
						}
					}
				}
				else if (data.slot_ == EquipmentSlot.ES_DoubleHand)
				{
					if (GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_SingleHand] != null)
					{
						NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId, GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_SingleHand].instId_);
					}
				}
				else if (data.slot_ == EquipmentSlot.ES_Ornament_0 || data.slot_ == EquipmentSlot.ES_Ornament_1)
				{
					
					if (GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0] != null && GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1] != null)
					{
						if (ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == data.subType_)
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId, GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
						}
						else if (ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].itemId_).subType_ == data.subType_)
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId, GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].instId_);
						}
						else
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId, GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
						}
					}
					else if (GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0] != null)
					{
						if (ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == data.subType_)
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId, GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
						}
						else if (GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1] != null)
						{
							if (ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].itemId_).subType_ == data.subType_)
							{
								NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId, GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].instId_);
							}
						}
					}
					else if (GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1] != null)
					{
						if (ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].itemId_).subType_ == data.subType_)
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId, GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].instId_);
						}
						else if (GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0] != null)
						{
							if (ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == data.subType_)
							{
								NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId, GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
							}
						}
					}
				}
				NetConnection.Instance.wearEquipment((uint)GamePlayer.Instance.InstId, bodySlotShadow[i].instId_);
				bodySlotShadow[i] = null;
			}
		}

		for (int i = 0; i < fastUse.Count; ++i)
		{
			data = ItemData.GetData((int)fastUse[i].itemId_);
			NetConnection.Instance.useItem((uint)fastUse[i].slot_, (uint)GamePlayer.Instance.InstId, (uint)1);
		}
		fastUse.Clear();
	}
    // 背包获得道具事件
   	public void OnItemAdd(COM_Item item)
    {
        if (BagSystem.instance.addItemIsFromBody)
            return;

        //如果新添加的物品的类型和之前快捷使用的物品类型相同(都是装备) 槽位相同.
        //说明是交换下来的 并且战斗力没有旧的高 不重复添加.
        ItemData newItem = ItemData.GetData((int)item.itemId_);
        ItemData oldItem = null;
        if(preItem != null)
            oldItem = ItemData.GetData((int)preItem.itemId_);
        if (preItem != null && oldItem.mainType_ == ItemMainType.IMT_Equip && oldItem.slot_ == newItem.slot_)
        {
            float newForce = Define.CALC_BASE_FightingForce(item);
            float oldForce = Define.CALC_BASE_FightingForce(preItem);
            if (newForce <= oldForce)
            {
                preItem = null;
                return;
            }
        }

        ItemData data = ItemData.GetData((int)item.itemId_);
        if (data.mainType_ == ItemMainType.IMT_Equip)
        {
            JobType jt = (JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession);
            int level = GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel);
            Profession profession = Profession.get(jt, level);
            ItemData _itemData = ItemData.GetData((int)item.itemId_);
            if (!profession.canuseItem(_itemData.subType_, _itemData.level_))
            {
                return;
            }

            if (GamePlayer.Instance.GetIprop(PropertyType.PT_Level) / 10 + 1 < _itemData.level_)
            {
                return;
            }

            if (data.slot_ == EquipmentSlot.ES_SingleHand)
            {
                if (GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand] != null)
                    return;
            }

            if (data.slot_ == EquipmentSlot.ES_DoubleHand)
            {
                if (GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_SingleHand] != null)
                    return;
            }
			int comSlot = 0;
			if(data.slot_ == EquipmentSlot.ES_DoubleHand)
			{
				if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_SingleHand] != null)
				{
					comSlot = (int)EquipmentSlot.ES_SingleHand;
				}
				else
				{
					comSlot = (int)EquipmentSlot.ES_DoubleHand;
				}
			}
			else if(data.slot_ == EquipmentSlot.ES_SingleHand)
			{
				if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand] != null)
				{
					comSlot = (int)EquipmentSlot.ES_DoubleHand;
				}
				else
				{
					comSlot = (int)EquipmentSlot.ES_SingleHand;
				}
			}
			else
			{
				comSlot = (int)data.slot_;
			}


            float newForce = Define.CALC_BASE_FightingForce(item);
			float oldForce = Define.CALC_BASE_FightingForce(GamePlayer.Instance.Equips[comSlot]);
            float preForce = Define.CALC_BASE_FightingForce(bodySlotShadow[(int)data.slot_]);
            if (newForce > oldForce && newForce > preForce)
            {
                bodySlotShadow[(int)data.slot_] = item;
            }
        }
        else if(data.subType_ == ItemSubType.IST_Buff)
        {
            float newForce = Define.CALC_BASE_FightingForce(item);
            float oldForce = Define.CALC_BASE_FightingForce(GamePlayer.Instance.Equips[(int)data.slot_]);
            float preForce = Define.CALC_BASE_FightingForce(bodySlotShadow[(int)data.slot_]);
            if (newForce > oldForce && newForce > preForce)
            {
                fastUse.Add(item);
            }
        }

    }

    // 背包删除道具事件
    void OnItemDel(COM_Item inst)
    {
        uint instId = inst.instId_;
        for (int i = 0; i < bodySlotShadow.Length; ++i)
        {
            if (bodySlotShadow[i] != null && bodySlotShadow[i].instId_ == instId)
            {
                if (preInst == bodySlotShadow[i].instId_)
                {
                    preInst = 0;
                    if (OnClose != null)
                        OnClose();
                }
                bodySlotShadow[i] = null;
                return;
            }
        }
        for (int i = 0; i < fastUse.Count; ++i)
        {
            if (fastUse[i].instId_ == instId)
            {
                if (preInst == fastUse[i].instId_)
                {
                    preInst = 0;
                    if (OnClose != null)
                        OnClose();
                }
                fastUse.RemoveAt(i);
                return;
            }
        }
        
    }


}