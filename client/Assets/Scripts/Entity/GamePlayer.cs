using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GamePlayer : Actor
{
	public bool isCreate;
	public WearEquipEventHandler  EmployeeEquipEvent;
	public DelEquipEventHandler DelEquipEvent;
	public List<Baby> babies_list_;
	private List<Employee> employee_list_;
	private uint[] _EmployeesBattleGroup1 = new uint[4];
	private uint[] _EmployeesBattleGroup2 = new uint[4];
	private EmployeesBattleGroup _EmployeesBattleGroup;
	//public List<int> achValues_list = new List<int> ();
	//public List<COM_Achievement> Achievement_list = new List<COM_Achievement> ();
	public List<int>titles = new List<int> ();
	public EmpOnBattleEvent UpdateEmployeeEnvent;
	public EmpOnBattleEvent UpdateEmployeeSkillEnvent;
	public RequestEventHandler<COM_Item[]> DrawEmployeeEnvent;
    public RequestEventHandler<Employee> EmployeeEvolveOkEnvent;
	public RequestEventHandler<Employee> EmployeeStarOkEnvent;
	public RequestEventHandler<uint> DelEmployeeEnvent;
	public RequestEventHandler<int> babyUpdateIpropEvent;
	public RequestEventHandler<int> PlayerLevelUpEvent;
    public RequestEventHandler<int[]> BabyLevelUpEvent;
	public RequestEventHandler<ulong> OpenSubSystemEnvet;
	public RequestEventHandler<int> OpenSystemEnvetString;
	public RequestEventHandler<uint> forgetSkillEnvet;
	public RequestEventHandler<float> doubleTimeEnvet;
	public RequestEventHandler<COM_Item> fixItmeEnvet;
	public RequestEventHandler<int> DebrisItmeEnvet;
	public RequestEventHandler<int> MagicItmeEnvet;
	public RequestEventHandler<int> MagicItmeJobEnvet;
	public RequestEventHandler<int> MagicItmeTupoEnvet;
	public RequestEventHandler<int> SkillExpEnvet;
    //public RequestEventHandler<int> AddBuffEnvet;
	public RequestEventHandler<int> CopyNumEnvet;
	public RequestEventHandler<int> moreLevelEnvet;
	public RequestEventHandler<bool> shouChongEnvet;
	public RequestEventHandler<bool> getShouChongEnvet;
	public RequestEventHandler<bool> vipRewardfigEnvet;
	public RequestEventHandler<COM_ADChargeTotal> getFanliEnvet;
	public RequestEventHandler<COM_ADGiftBag> MiaoshaEnvet;
	public RequestEventHandler<int> levelRewadEnvet;
	public RequestEventHandler<int> fuwenEnvet;
	public RequestEventHandler<int> fuwenCompoundOkEnvet;
	public RequestEventHandler<bool> signEnvet;
	public bool dressChanged_;
	public bool isUseCloseBtn;
	public delegate void updateOnlineHandler();
	public  updateOnlineHandler OnOnlineUpdate;
	public delegate void updateGrowthfundHandler(int val);
	public  updateGrowthfundHandler OnGrowthfundUpdate;

	public delegate void AchievementEventHandler();
    public event AchievementEventHandler OnAchievementUpdate;
    public delegate void VipUpdateHandler();
    public event VipUpdateHandler OnVipUpdate;
    public delegate void BabyUpdateHandler();
    public event BabyUpdateHandler OnBabyUpdate;
    public delegate void AutoMeetEnemyHandler(bool ok);
    public event AutoMeetEnemyHandler OnAutoMeetEnemy;

	public delegate void UpdateConvertExpHandler(int exp);
	public UpdateConvertExpHandler OnUpdateConvertExp;

	public delegate void ShowBabyHandler(int bid,bool isshow);
	public ShowBabyHandler OnShowBaby;

	public List<int>achfinId = new List<int> ();
	public COM_ActivityTable ActivityTable;
    public int wait4EnterBattleId;
	public static int newBabyId;
    public bool hasBabyLevelUp = false;
	public static bool isIngroupScene = false;
    public int vipLevel_;
    public int vipLeftDays_;
    public long createTime_;
    private bool isFirstLogin = false;
	public bool isOpenDoubleTime; 
	public float doubleTime;
	private uint _doubleTimeStart; //记录双倍时间戳.	
    public bool MagicLevelDirty;
	private int _magicItemLevel;
	private int _magicItemExp;
	private JobType _magicItemJob;
	private int _magicTupoLevel;
	public bool isInitEmployees;
	public bool isInitStorageItem;
	public bool isInitStorageBaby;
    public bool leaderSkipTalk;
    public bool nextFuncClose_;
    public bool levelIsDirty_;
    public bool titleHide_;
	public List<uint> copyNum_= new List<uint>();
	public COM_Item[] fuwens_ = new COM_Item[6];
	public bool isShouchong;
	public bool isGetShouchong;
	public List<uint> levelgift = new List<uint> ();
	public bool vipRewardfig = false;
    public bool buffIsDirty_ = false;
    public bool ShowBabyDirty;
	public COM_ADGiftBag miaoshaData_;

	public List<COM_CourseGift> levelShopList = new List<COM_CourseGift> ();

	private uint levelShopTimeStart;		 //记录重置CD开始时间.	
	private float levelremainCDTime; 		 //CD 时间.


    public void ShowBaby(int instid)
    {
        for (int i = 0; i < babies_list_.Count; ++i)
        {
            babies_list_[i].isShow_ = (babies_list_[i].InstId == instid);
        }
        ShowBabyDirty = true;
    }

    //有活动开启中且玩家没有点击过
    public bool hasActivityOpen_;

	private List<int> _openFunEffectBtns = new List<int> ();
    //public COM_PropValue[] battlePropCache = new COM_PropValue[(int)PropertyType.PT_Max];

	private COM_ContactInfo[] contactInfoList; 

	public bool IsFirstLogin
    {
        get
        {
            bool t = isFirstLogin;
            isFirstLogin = false;
            return t;
        }
    }

	public bool isInBattle = false;

	public bool playerLaunched = false;

    public ulong openSubSystemFlag_;
	public COM_ADChargeTotal  myselfRecharge_;//个人累计充值
    
    private AssetBundle playerAsset_, weaponAsset_;

    bool bottomButtonDisplay_;
    public bool BottomBtnState
    {
        set { bottomButtonDisplay_ = value; }
        get { return bottomButtonDisplay_; }
    }

    bool topButtonDisplay_;
	public  bool topButtonDisplay1_;
	public bool TopBtnState
    {
        set { topButtonDisplay_ = value; }
        get { return topButtonDisplay_; }
    }

    public void ClearEmplyee()
    {
        isInitEmployees = false;
        if(employee_list_ != null)
            employee_list_.Clear(); 
    }

    public static int activityType_ = 2;

    public bool hasCreated_ = false;

    List<int> openedScenes;

    public bool MyEmpCurrencyIsDirty = false;

	public static GamePlayer Instance = new GamePlayer();

	public GamePlayer()
    {
        babies_list_ = new List<Baby>();
        employee_list_ = new List<Employee>();
    }

    public Entity GetUnit(uint guid)
    {
        if (InstId == guid)
            return this;

        for (int i = 0; i < babies_list_.Count; ++i)
        {
            if (babies_list_[i].InstId == guid)
                return babies_list_[i];
        }

        for (int i = 0; i < employee_list_.Count; ++i)
        {
            if (employee_list_[i].InstId == guid)
                return employee_list_[i];
        }
        return null;
    }

    public void setBabies(COM_BabyInst[] insts)
    {
        babies_list_.Clear();
        for (int i = 0; i < insts.Length; ++i)
        {
            Baby baby = new Baby();
            baby.SetBaby(insts[i]);
            babies_list_.Add(baby);
        }

    }

	public void setEmployees(COM_EmployeeInst[] insts, bool flag)
    {
        for (int k = 0; k < insts.Length; k++)
        {
			if(IsHaveEmployees((int)insts[k].instId_))
			{
				continue;
			}
            Employee employee = new Employee();
            employee.SetEntity(insts[k]);
            employee_list_.Add(employee);
        }
		employee_list_.Sort (SortEmployess);

		if(flag)
		{
			isInitEmployees = true;
			if(EmployessSystem.instance.openUIEmployee)
			{
				EmployessSystem.instance.openUIEmployee = false;
				NetWaitUI.HideMe();
				EmployessControlUI.SwithShowMe(EmployessSystem.instance.openEmployeeType);
			}
			if(TeamSystem.openInitUI)
			{
				TeamSystem.openInitUI = false;
				NetWaitUI.HideMe();
                if(TeamSystem.UIShowType == 1)
				    TeamUI.ShowMe();
                else
                    TeamUI.SwithShowMe();
			}
			if(EmployessSystem.instance.openUIEmpTask)
			{
				EmployessSystem.instance.openUIEmpTask = false;
				NetWaitUI.HideMe();
				EmployeeTaskUI.SwithShowMe();
			}

		}
	 }
	public COM_Item [] StorageItems;
	public COM_BabyInst [] Storagebaby;
	public string _GuildName = "";
	public int wishShareNum_ = 0;
	public int guildContribution_;
	public bool onlineTimeFlag_;
	public float onlineTime_;
	public List<uint> onlineTimeRewards_ = new List<uint>();
	public List<uint> fundtags_ = new List<uint>();
	public bool isFund_;
    public bool todaySigned_;
	public int fund_;
	public int exitguildtime;
	//public List<COM_BabyInst> StorageBabys = new List<COM_BabyInst> ();
    public int openADActivityLv;
	public List<ADType> adTypes = new List<ADType> ();//{ ADType.ADT_Cards, ADType.ADT_ChargeTotal, ADType.ADT_ChargeEvery, ADType.ADT_DiscountStore, ADType.ADT_LoginTotal, ADType.ADT_OnlineReward, ADType.ADT_HotRole, ADType.ADT_Foundation, ADType.ADT_7Days, ADType.ADT_BuyEmployee,ADType.ADT_Level };
	public void SetPlayer(COM_PlayerInst inst)
	{
		StorageItems = new COM_Item[100];
		Storagebaby = new COM_BabyInst[100];
        GlobalValue.Get(Constant.C_ADActivityOpenLv, out openADActivityLv);
		GameManager.Instance.GamePlayerInfoReset ();
		OpenFunEffectBtns.Clear ();
		SetEntity (inst);
        buffIsDirty_ = true;
        playerSimp_.isLeader_ = inst.isTeamLeader_;
        createTime_ = inst.createTime_;
        GameManager.SceneID = inst.sceneId_;
        openedScenes = new List<int>(inst.openScenes_);
		_GuildName = inst.guildName_;
		fundtags_.Clear();
		fundtags_.AddRange (inst.fundtags_);
		isFund_ = inst.isFund_;
		//ActivitySystem.Instance.petTempTimes_ = (int)inst.petActivityNum_;
		exitguildtime = (int)inst.exitGuildTime_;

        UpdateVip((int)inst.properties_[(int)PropertyType.PT_VipLevel]);
        UpdateVipTime((int)inst.properties_[(int)PropertyType.PT_VipTime]);
		guildContribution_ = inst.guildContribution_;
        isFirstLogin = inst.isFirstLogin_;
        OpenSubSystem = inst.openSubSystemFlag_;
		updateDoubleTime (inst.openDoubleTimeFlag_,(float)inst.properties_ [(int)PropertyType.PT_DoubleExp]);
		hundredSystem.instance.HundredBattle = inst.hbInfo_;
		BoxSystem.Instance.GreenCDTime = inst.greenBoxTimes_;
		BoxSystem.Instance.BlueCDTime = inst.blueBoxTimes_;
		BoxSystem.Instance.FreeNum = (int)inst.greenBoxFreeNum_;
		BagSystem.instance._remainTimeStart = (uint)Time.realtimeSinceStartup;
        //GatherSystem.instance.InitMingTimes(inst.mineTimes_);
		ArenaPvpSystem.Instance.MyInfo = inst.pvpInfo_;
		wishShareNum_ = (int)inst.wishShareNum_;
		//employee_list_.Sort (SortEmployess);
		MagicItemLevel = inst.magicItemLevel_;
		MagicItemExp = inst.magicItemeExp_;
		MagicItemJob = inst.magicItemeJob_;
		MagicTupoLevel = inst.magicTupoLevel_;
        FamilySystem.instance.ZhuFuSkills.Clear();
		FamilySystem.instance.ZhuFuSkills.AddRange(inst.guildSkills_);
		onlineTimeRewards_.Clear ();
		onlineTimeRewards_.AddRange(inst.onlineTimeReward_);
		onlineTime_ = inst.onlineTime_;						
        onlineTimeFlag_ = inst.onlineTimeFlag_;
		copyNum_.AddRange( inst.copyNum_);
		TodaySiged = inst.signFlag_;
		isShouchong = inst.firstRechargeDiamond_;
		isGetShouchong = inst.isFirstRechargeGift_;
        adTypes = new List<ADType>(inst.gmActivities_);
        if (inst.onlineTimeFlag_)
        {
            if (!adTypes.Contains(ADType.ADT_OnlineReward))
                adTypes.Add(ADType.ADT_OnlineReward);
        }
        MoreActivityData.InitCardsData(inst.selfCards_);
        MoreActivityData.InitHotRoleData(inst.hotdata_);
		MoreActivityData.Init7DaysData(inst.sevendata_);
		MoreActivityData.instance.UpdateLoginTotal (inst.festival_);
		MoreActivityData.instance.UpdateSelfRecharge (inst.selfRecharge_);
		MoreActivityData.instance.UpdateSysRecharge (inst.sysRecharge_);
		MoreActivityData.instance.UpdateSelfDiscountStore (inst.selfDiscountStore_);
		MoreActivityData.instance.UpdateSysDiscountStore (inst.sysDiscountStore_);
		MoreActivityData.instance.UpdateSelfChargeEvery (inst.selfOnceRecharge_);
		MoreActivityData.instance.UpdateSysChargeEvery (inst.sysOnceRecharge_);
		MoreActivityData.instance.updateEmployeeActivity (inst.empact_);
		MoreActivityData.instance.updateIntegralShop (inst.integralData_);
		miaoshaData_ = inst.gbdata_;
        GamePlayer.Instance.ActivityTable = inst.activity_; 
		GemSystem.instance.sycnCrystal (inst.crystalData_);
		updateLevelRewardShop(inst.coursegift_);
		ActivitySystem.flags.Clear ();
		ActivitySystem.flags.AddRange (GamePlayer.Instance.ActivityTable.flag_);

		levelgift.AddRange (inst.levelgift_);
		MoreActivityData.instance.UpdateLevelUpRad ();
		vipRewardfig = inst.viprewardflag_;
		//for(int n = 0;n<inst.achValues_.Length;n++)
		//{
		//	achValues_list.Add(inst.achValues_[n]);
		//}
		//Achievement_list.AddRange (inst.achievement_);
		titles.Clear ();
		titles.AddRange(inst.titles_);

        bottomButtonDisplay_ = true;
        topButtonDisplay_ = true;
		playerLaunched = true;

		for(int j=0;j<fuwens_.Length;j++)
		{
			fuwens_[j] = null;
		}
		
		for(int k =0;k<inst.fuwen_.Length;k++)
		{
			fuwens_[ inst.fuwen_[k].slot_] = inst.fuwen_[k];
		}
		//FriendSystem.Instance ().InitFrintList (inst.friend_);
		//FriendSystem.Instance ().InitBlackList (inst.blacklist_);
		//EmployeesBattleGroup1 = inst.employeeGroup1_;
		//EmployeesBattleGroup2 = inst.employeeGroup2_;
		//CurEmployeesBattleGroup = inst.empBattleGroup_;

        //ActivitySystem.Instance.SyncCounter(inst.activationCounter_);
        //PlayerAsseMgr.LoadAsset((ENTITY_ID)inst.properties_[(int)PropertyType.PT_AssetId], (AssetBundle asset, ParamData data) =>
        //{
        //    playerAsset_ = asset;
        //}, null);

        //COM_Item weapon = BagSystem.instance.GetItemInstBySlot((int)EquipmentSlot.ES_SingleHand);
        //if (weapon != null)
        //{
        //    WeaponAssetMgr.LoadAsset((ENTITY_ID)weapon.instId_, (AssetBundle asset, ParamData data) =>
        //    {
        //        weaponAsset_ = asset;
        //    }, null);
        //}
        nextFuncClose_ = PlayerPrefs.GetInt(InstId.ToString() + "nextFuncClose") == 1 ? true : false;
        titleHide_ = PlayerPrefs.GetInt(InstId.ToString() + "hideTitle") == 1 ? true : false;

        NetConnection.Instance.requestAchievement();

        hasCreated_ = true;
		myselfRecharge_ = inst.myselfRecharge_;
        if (onlineTimeFlag_)
        {
            if (!adTypes.Contains(ADType.ADT_OnlineReward))
                adTypes.Add(ADType.ADT_OnlineReward);
        }
		adTypes.Add (ADType.ADT_Level);
        if (String.IsNullOrEmpty(inst.phoneNumber_) && GameManager.ServId_ != 801 && GameManager.ServId_ != 802) //评测
		{
			adTypes.Add (ADType.ADT_PhoneNumber);
			MoreActivityData.instance.SetTypeRad((int)ADType.ADT_PhoneNumber, 1);
		}
		adTypes.Add (ADType.ADT_Sign);
		if(adTypes.Contains(ADType.ADT_GiftBag))
		{ 
			adTypes.Remove(ADType.ADT_GiftBag);
		}
		//adTypes.Add (ADType.ADT_Zhuanpan);
		//adTypes.Add (ADType.ADT_IntegralShop);
	}

    public void UpdateAd(ADType type, bool add)
    {
        if (add && !adTypes.Contains(type))
            adTypes.Add(type);
        else if (!add && adTypes.Contains(type))
            adTypes.Remove(type);
	
        if (type == ADType.ADT_OnlineReward && add)
        {
            onlineTime_ = 0f;
            onlineTimeFlag_ = true;
            onlineTimeRewards_.Clear();
            if (OnOnlineUpdate != null)
                OnOnlineUpdate();
        }

		if(OpenSubSystemEnvet  != null)
			OpenSubSystemEnvet(0);
    }

    public void SetTitleDisable(bool bHide)
    {
        PlayerPrefs.SetInt(GamePlayer.Instance.InstId.ToString() + "hideTitle", bHide ? 1 : 0);
        titleHide_ = bHide;
    }

    override public void SetIprop(COM_PropValue[] props)
    {
        for (int i = 0; i < props.Length; ++i)
        {
            if (props[i] == null)
                continue;

            if (props[i].type_ == PropertyType.PT_Money)
            {
                int tmp = (int)props[i].value_ - GetIprop(PropertyType.PT_Money);
                string msg = "";
                PopText.WarningType wtype = PopText.WarningType.WT_None;
                if (tmp > 0)
                {
                    msg = LanguageManager.instance.GetValue("huodejinbi").Replace("{n}", Mathf.Abs(tmp).ToString());
                    wtype = PopText.WarningType.WT_Tip;
                }
                else if (tmp < 0)
                {
                    msg = LanguageManager.instance.GetValue("costqian").Replace("{n}", Mathf.Abs(tmp).ToString());
                    wtype = PopText.WarningType.WT_Warning;
                }
                else
                {
                    //undo
                }

                if(!string.IsNullOrEmpty(msg))
                {
                    ChatSystem.PushSystemMessage(msg);
                    PopText.Instance.Show(msg, wtype);
                }
            }
			else if (props[i].type_ == PropertyType.PT_Exp)
			{
				long tmp = (long)props[i].value_ -  (long)properties_[(int)PropertyType.PT_Exp];
				string msg = "";
				PopText.WarningType wtype = PopText.WarningType.WT_None;
				if (tmp > 0)
				{ 
					msg = LanguageManager.instance.GetValue("huodejingyan").Replace("{n}", Mathf.Abs(tmp).ToString());
					wtype = PopText.WarningType.WT_Tip;
				}
				
				if(!string.IsNullOrEmpty(msg))
				{
					PopText.Instance.Show(msg, wtype);
				} 
			}
            else
				if (props[i].type_ == PropertyType.PT_MagicCurrency)
			{
                int tmp1 = (int)props[i].value_ - GetIprop(PropertyType.PT_MagicCurrency);
                string msg = "";
                PopText.WarningType wtype = PopText.WarningType.WT_None;
				if (tmp1 > 0)
				{
                    msg = LanguageManager.instance.GetValue("huodemolibi").Replace("{n}", Mathf.Abs(tmp1).ToString());
                    wtype = PopText.WarningType.WT_Tip;
				}
				else if (tmp1 < 0)
				{
                    msg = LanguageManager.instance.GetValue("costmolibi").Replace("{n}", Mathf.Abs(tmp1).ToString());
                    wtype = PopText.WarningType.WT_Warning;
				}
                if(!string.IsNullOrEmpty(msg))
                {
                    ChatSystem.PushSystemMessage(msg);
                    PopText.Instance.Show(msg, wtype);
                }
			}
				else if (props[i].type_ == PropertyType.PT_Diamond)
            {
                int tmp = (int)props[i].value_ - GetIprop(PropertyType.PT_Diamond);
                string msg = "";
                PopText.WarningType wtype = PopText.WarningType.WT_None;
                if (tmp > 0)
                {
                    msg = LanguageManager.instance.GetValue("huodezuanshi").Replace("{n}", Mathf.Abs(tmp).ToString());
                    wtype = PopText.WarningType.WT_Tip;
                }
                else if (tmp < 0)
                {
                    msg = LanguageManager.instance.GetValue("costzuanshi").Replace("{n}", Mathf.Abs(tmp).ToString());
                    wtype = PopText.WarningType.WT_Warning;
                }
                if(!string.IsNullOrEmpty(msg))
                {
                    ChatSystem.PushSystemMessage(msg);
                    PopText.Instance.Show(msg, wtype);
                }
			}
			else if(props[i].type_ == PropertyType.PT_HpCurr)
			{
                //ClientLog.Instance.LogError("PT_HPCURR ======= ======= ====== ====== >>>> " + props[i].value_.ToString());
				int tmp = GetIprop(PropertyType.PT_HpCurr) - (int)props[i].value_;
//				if (tmp > 0)
//				{
//					//ChatSystem.PushSystemMessage(LanguageManager.instance.GetValue("huifuHp").Replace("{n}", Mathf.Abs(tmp).ToString()));
//					// ChatSystem.PushSystemMessage(String.Format(LanguageManager.instance.GetValue("zuanshi"), -tmp));
//				}
//				else 
					if (tmp < 0)
				{
					ChatSystem.PushSystemMessage(LanguageManager.instance.GetValue("huifuHp").Replace("{n}", Mathf.Abs(tmp).ToString()));
					// ChatSystem.PushSystemMessage(String.Format(LanguageManager.instance.GetValue("huodezuanshi"), tmp));
				}
				else
				{
					//undo
				}
			}else if(props[i].type_ == PropertyType.PT_MpCurr)
			{
				int tmp = GetIprop(PropertyType.PT_MpCurr) - (int)props[i].value_;
//				if (tmp > 0)
//				{
//					//ChatSystem.PushSystemMessage(LanguageManager.instance.GetValue("huifuHp").Replace("{n}", Mathf.Abs(tmp).ToString()));
//					// ChatSystem.PushSystemMessage(String.Format(LanguageManager.instance.GetValue("zuanshi"), -tmp));
//				}
//				else 
					if (tmp < 0)
				{
					ChatSystem.PushSystemMessage(LanguageManager.instance.GetValue("huifuMp").Replace("{n}", Mathf.Abs(tmp).ToString()));
					// ChatSystem.PushSystemMessage(String.Format(LanguageManager.instance.GetValue("huodezuanshi"), tmp));
				}
				else
				{
					//undo
				}
			}
			else if(props[i].type_ == PropertyType.PT_DoubleExp)
			{
				properties_[(int)props[i].type_] = props[i].value_;
  				updateDoubleTime(isOpenDoubleTime,props[i].value_);
			}
            else if (props[i].type_ == PropertyType.PT_ConvertExp)
            {
                properties_[(int)props[i].type_] = props[i].value_;
                if(OnUpdateConvertExp != null)
                    OnUpdateConvertExp((int)props[i].value_);
            }
			else if(props[i].type_ == PropertyType.PT_EmployeeCurrency)
			{
				int tmp = GetIprop(PropertyType.PT_EmployeeCurrency) - (int)props[i].value_;
			
				if (tmp < 0)
				{
					PopText.Instance.Show(  LanguageManager.instance.GetValue("fireyinjiok").Replace("{n}", Mathf.Abs(tmp).ToString()) );
				}
                MyEmpCurrencyIsDirty = true;
			}
         
            properties_[(int)props[i].type_] = props[i].value_; ///这个数据更改流程绝壁有问题 这个是导致遍地是 update 调用的病根
                                                                ///
            if(props[i].type_ == PropertyType.PT_Title)
            {
                //更新称号
                Avatar self = Prebattle.Instance.GetSelf();
                self.UpdateTitle();
            }
        }
        OnPropUpdate();

        if(hasPlayerLevelUpdate(props))
        {
            QuestSystem.UpdateAcceptableQuests(); ///需要更新可接任务列表 任务表做优化(按可接等级筛选列表
            if (PlayerLevelUpEvent != null)
            {
				CommonEvent.ExcuteAccountChange(CommonEvent.DefineAccountOperate.LevelUp);
                PlayerLevelUpEvent(GetIprop(PropertyType.PT_Level));
            }
			MoreActivityData.instance.UpdateLevelUpRad ();
        }

        if (PlayerStateUI.SetPlayerTitleOk != null)
        {
            PlayerStateUI.SetPlayerTitleOk();
        }

        for (int i = 0; i < props.Length; ++i)
        {
            if (props[i].type_ == PropertyType.PT_VipLevel)
                GamePlayer.Instance.UpdateVip(GamePlayer.Instance.GetIprop(PropertyType.PT_VipLevel));
            //else if (props[i].type_ == PropertyType.PT_VipTime)
            //    GamePlayer.Instance.UpdateVipTime(GamePlayer.Instance.GetIprop(PropertyType.PT_VipTime));
        }
    }

    bool hasPlayerLevelUpdate(COM_PropValue[] props)
    {
        for (int i = 0; i < props.Length; ++i)
        {
            if (props[i] == null)
                continue;

            if (props[i].type_ == PropertyType.PT_Level)
                return true;
        }
        return false;
    }

//		public void AddAchievement(COM_Achievement[] com_ach)
//		{
//			Achievement_list.Clear ();
//			for(int i =0;i<com_ach.Length;i++)
//			{
//				Achievement_list.Add (com_ach[i]);
//			}
//
//		}
	public void DelAchid(int aId)
	{
		for(int i =0;i<achfinId.Count;i++)
		{
			if(aId == achfinId[i])
			{
				achfinId.RemoveAt(i);
			}
		}
        UpdateAchievement();
	}
    public GameObject GetPlayerClone()
    {
        GameObject player = null;
        GameObject weapon = null;
        if (playerAsset_ != null)
            player = (GameObject)GameObject.Instantiate(playerAsset_.mainAsset) as GameObject;
        if (weaponAsset_ != null)
            weapon = (GameObject)GameObject.Instantiate(weaponAsset_.mainAsset) as GameObject;
        if(weapon != null)
            weapon.transform.parent = player.transform;
        return player;
    }

	// 添加一个技能.
	public void AddSkill(COM_Skill skillInst) 
	{
		skillInsts_.Add (skillInst);
	}
	
	// 删除一个已经学会的技能.
	public void RemoveSkill(uint id)
	{
		for (int i = 0;i< skillInsts_.Count;i++)
		{
			if(skillInsts_[i].skillID_ == id)
			{
				skillInsts_.Remove(skillInsts_[i]);
				break;
			}
		}
		if (forgetSkillEnvet != null)
			forgetSkillEnvet (id);
	}
	Dictionary<int,int> skillIdExps = new Dictionary<int, int> ();
	public void UpdateSkill(int id, int exp,ItemUseFlag flag)
	{
        for (int i = 0; i < skillInsts_.Count; ++i)
		{
            if (skillInsts_[i].skillID_ == id)
			{
				if(flag == ItemUseFlag.IUF_Scene)
				{
					uint mexp = (uint)exp - skillInsts_[i].skillExp_;
//					if(skillIdExps.ContainsKey(id))
//					{
//						skillIdExps[id] = skillIdExps[id]+(int)mexp;
//					}else
//					{
//						skillIdExps.Add(id,(int)mexp);
//					}

					if(mexp>0)
					{
						SkillData skdata = SkillData.GetData((int)skillInsts_[i].skillID_, (int)skillInsts_[i].skillLevel_);
						PopText.Instance.Show(LanguageManager.instance.GetValue("jinengjingyan").Replace("{n}",skdata._Name).Replace("{n1}",mexp.ToString()));
					}

				}
				skillInsts_[i].skillExp_ = (uint)exp;
				break;
			}
		}

		if (SkillExpEnvet != null)
		{
			SkillExpEnvet (id);
		}

	}

    //public void SetBattleCacheProp(COM_PropValue[] props)
    //{
    //    for(int i=0; i < props.Length; ++i)
    //    {
    //        battlePropCache[(int)props[i].type_] = props[i];
    //    }
    //}

	public void SkillLevelUp(uint id, COM_Skill skill)
	{
		if(id == InstId)
		{
			for (int i = 0; i < skillInsts_.Count; ++i)
			{
				if (skillInsts_[i].skillID_ == skill.skillID_)
				{
					skillInsts_[i] = skill;
					break;
				}
			}
		}

		Employee emp = GetEmployeeById ((int)id);

		if(emp != null)
		{
			for (int i = 0; i < emp.SkillInsts.Count; ++i)
			{
				if (emp.SkillInsts[i].skillID_ == skill.skillID_)
				{
					emp.SkillInsts[i] = skill;
					UpdataEmployeeInst(emp);
					if(UpdateEmployeeSkillEnvent != null)
					{
						UpdateEmployeeSkillEnvent(emp,0);
					}
					break;
				}
			}
		}

		if (SkillExpEnvet != null)
		{
			SkillExpEnvet ((int)id);
		}

	}

    //找一个该技能中等级最高的
	public COM_Skill GetSkillById(int id)
	{
        COM_Skill inst = null;
        for (int i = 0; i < skillInsts_.Count; ++i)
        {
            if (skillInsts_[i].skillID_ == id)
            {
                if (inst != null)
                {
                    if (inst.skillLevel_ < skillInsts_[i].skillLevel_)
                    {
                        inst = skillInsts_[i];
                    }
                }
                else
                    inst = skillInsts_[i];
            }
        }
        return inst;
	}

	public void AddBaby(COM_BabyInst inst)
	{
		Baby baby = new Baby ();
		baby.SetBaby (inst);
		newBabyId = baby.InstId;
		babies_list_.Add(baby);
        if (OnBabyUpdate != null)
            OnBabyUpdate();
		if(MainbabyListUI.RefreshBabyListOk != null)
		{
			MainbabyListUI.RefreshBabyListOk(newBabyId);
		}
	}

    public void DelBaby(int babyInstId)
    {
        for (int i = 0; i < babies_list_.Count; ++i)
        {
            if (babies_list_[i].InstId == babyInstId)
            {
				if(babies_list_[i].isShow_)
					ShowBabyDirty = true;
				ChatSystem.PushSystemMessage(LanguageManager.instance.GetValue("shanchubaby").Replace("{n}",GamePlayer.Instance.GetBabyInst(babyInstId).InstName));
				babies_list_.RemoveAt(i);
                if (OnBabyUpdate != null)
                    OnBabyUpdate();
                break;
            }    
        }
		//MainbabyListUI.CrtSelectIdx--;
		if(MainbabyListUI.RefreshBabyListOk != null)
		{
			MainbabyListUI.RefreshBabyListOk(babyInstId);
		}
    }

    public Baby GetShowBaby()
    {
        if (TeamSystem.IsInTeam() && !TeamSystem.AwayTeam(InstId))
            return null;

        for (int i = 0; i < babies_list_.Count; ++i)
        {
            if (babies_list_[i].isShow_)
                return babies_list_[i];
        }
        return null;
    }

	public void AddEmployee(COM_EmployeeInst inst)
	{
		if(IsHaveEmployees((int)inst.instId_))
		{
			return;
		}
		Employee employee = new Employee ();
		employee.SetEntity (inst);
		employee_list_.Add (employee);
		employee_list_.Sort (SortEmployess);

		EmployessSystem.instance.UpdateEmployeeRed ();
	}

	public void delEmployee(uint[] insts)
	{

		for(int i = 0;i<insts.Length;i++)
		{
            for (int j = 0; j < employee_list_.Count; ++j)
            {
                if (employee_list_[j].InstId == insts[i])
                {
                    employee_list_.RemoveAt(j);
                    break;
                }
            }
		}
		if( DelEmployeeEnvent!= null)
		{
			DelEmployeeEnvent(0);
		}
		employee_list_.Sort (SortEmployess);
	}


	public void UpdataEmployee(Employee employee)
	{
		employee_list_.Sort (SortEmployess);
		if(UpdateEmployeeEnvent != null)
		{
			UpdateEmployeeEnvent(employee,0);
		}
		EmployessSystem.instance.UpdateEmployeeRed ();
	}

	public void UpdateEmployeeSoul(int id ,uint soulNum)
	{

		for(int i=0;i<employee_list_.Count;i++)
		{
			if(employee_list_[i].InstId == id)
			{
				employee_list_[i].soul_ = soulNum;
				if(UpdateEmployeeEnvent != null)
				{
					UpdateEmployeeEnvent(employee_list_[i],0);
				}
				break;
			}
		}
	
	}
	public void UpdataEmployeeInst(Employee emp)
	{
		for(int i=0;i<employee_list_.Count;i++)
		{
			if(employee_list_[i].InstId == emp.InstId)
			{
				employee_list_[i] = emp;
			}
		}
	}


	public List<Employee> EmployeeList
	{
		get
		{
			return employee_list_;
		}
	}
	private int SortEmployess(Employee  a,Employee b)
	{
		if (a.isForBattle_)
		{
			return -1;
		}
		else if (b.isForBattle_)
		{
			return 0;
		}
		else if (a.quality_ > b.quality_)
		{
			return -1;
		}
		else if(a.star_ > b.star_)
		{
			return 0;
		}
		else
		{
			return 1;
		}
	}




	public bool GetEmployeeIsBattle(int instId,int group = (int)EmployeesBattleGroup.EBG_GroupOne)
	{
		uint[] emps = new uint[4];
		if(group == (int)EmployeesBattleGroup.EBG_GroupOne)
		{
			emps = EmployeesBattleGroup1;
		}
		else if(group ==(int)EmployeesBattleGroup.EBG_GroupTwo)
		{
			emps = EmployeesBattleGroup2;
		}

		for(int i=0;i<emps.Length;i++)
		{
			if(emps[i] == instId)
			{
				return true;
			}
		}

		return false;
	}

	public List<Employee> GetBattleEmployees()
	{
		List<Employee> arr = new List<Employee> ();

		for(int i=0;i<employee_list_.Count;i++)
		{
			if(employee_list_[i].isForBattle_)
			{
				arr.Add(employee_list_[i]);
			}
		}

		return arr;
	}

	public bool IsHaveEmployees(int instId)
	{
		for(int i=0;i<employee_list_.Count;i++)
		{
			if(employee_list_[i].InstId == instId)
				return true;
		}

		return false;
	}

	public void RemoveBaby(int slot)
	{

	}

	// 获取出战的宝宝.
	public Baby BattleBaby
	{
		get
		{
			for(int i=0; i < babies_list_.Count; ++i)
			{
				if((babies_list_[i]).isForBattle_)
					return babies_list_[i];
			}
			return null;
		}
	}

    public void RefreshBaby(COM_BabyInst inst)
    {

        for (int i = 0; i < babies_list_.Count; ++i)
        {
            if (babies_list_[i].InstId == (int)inst.instId_)
            {
                babies_list_[i].SetBaby(inst);
                if (babyUpdateIpropEvent != null)
                    babyUpdateIpropEvent((int)inst.instId_);
				if (MainbabyListUI.RefreshBabyOk != null)
				{
					MainbabyListUI.RefreshBabyOk(inst);
				}

                return;
            }
        }
    }

	public void SetBabyIProp(int guid, COM_PropValue[] prop)
	{
		FindBaby (guid).SetIprop (prop);
		if (babyUpdateIpropEvent!=null)
			babyUpdateIpropEvent(guid);
        for (int i = 0; i < prop.Length; ++i)
        {
            if (prop[i].type_ == PropertyType.PT_Level)
            {
                if (BabyLevelUpEvent != null)
                    BabyLevelUpEvent(new int[]{guid, (int)prop[i].value_});
                hasBabyLevelUp = true;
            }
        }
	}
	
	public Entity FindBaby(int guid)
	{
		for(int i=0; i < babies_list_.Count; ++i)
		{
			if(babies_list_[i].InstId == guid)
				return babies_list_[i];
		}
		return null;
	}

    public Actor FindBabyActor(int guid)
    {
        for (int i = 0; i < babies_list_.Count; ++i)
        {
            if (babies_list_[i].InstId == guid)
                return babies_list_[i];
        }
        return null;
    }

    public BattleActor FindBabyBattleActor(int guid)
    {
        for (int i = 0; i < babies_list_.Count; ++i)
        {
            if (babies_list_[i].InstId == guid)
            {
                COM_BattleEntityInformation bei = new COM_BattleEntityInformation();
                bei.instId_ = babies_list_[i].InstId;
                bei.instName_ = babies_list_[i].InstName;
                bei.level_ = babies_list_[i].GetIprop(PropertyType.PT_Level);
                bei.hpCrt_ = babies_list_[i].GetIprop(PropertyType.PT_HpCurr);
                bei.hpMax_ = babies_list_[i].GetIprop(PropertyType.PT_HpMax);
                bei.mpCrt_ = babies_list_[i].GetIprop(PropertyType.PT_MpCurr);
                bei.mpMax_ = babies_list_[i].GetIprop(PropertyType.PT_MpMax);
                bei.tableId_ = babies_list_[i].GetIprop(PropertyType.PT_TableId);
                bei.assetId_ = babies_list_[i].GetIprop(PropertyType.PT_AssetId);
                bei.type_ = babies_list_[i].type_;
                BattleActor bActor = new BattleActor();
                bActor.SetBattlePlayer(bei);
                return bActor;
            }
        }
        return null;
    }

	public void BabyState(int uid, bool setState)
	{
		ResetBabyState ();
		for(int i=0; i < babies_list_.Count; ++i)
		{
			if(babies_list_[i].InstId == uid)
				babies_list_[i].isForBattle_ = setState;
		}
	}

	void ResetBabyState()
	{
		for(int i=0; i < babies_list_.Count; ++i)
		{
			babies_list_[i].isForBattle_ = false;
		}
	}

	public bool isMineBaby(int uid)
	{
		for(int i=0; i < babies_list_.Count; ++i)
		{
			if(babies_list_[i].InstId == uid)
				return true;
        }
        return false;
	}

    //public void SetBattleCache()
    //{
    //    SetIprop(battlePropCache);
    //    battlePropCache = new COM_PropValue[(int)PropertyType.PT_Max];
    //}

	public Baby GetBabyInst(int uid)
	{
		for(int i=0; i < babies_list_.Count; ++i)
		{
			if(babies_list_[i].InstId == uid)
				return babies_list_[i];
		}
		return null;
	}

	// 获取宝宝个数.
	public int BabyCount
	{
		get { return babies_list_.Count; }
	}

	//baby 学习新技能
	public void BabyLearnSkill(uint babyId, uint skillId)
	{
		Baby baby = GetBabyInst((int)babyId);
		if (baby == null)
			return;
        if (SkillData.GetMinxiLevelData((int)skillId) == null)
			return;
		COM_Skill skill = new COM_Skill ();
		skill.skillID_ = skillId;
		skill.skillExp_ = 0;
		skill.skillLevel_ = (uint)SkillData.GetMinxiLevelData ((int)skillId)._Level;
		baby.SkillInsts.Add(skill);

		if(BabySkill.BabyskillLearnOk !=null)
		{
			BabySkill.BabyskillLearnOk((int)babyId,(int)skillId);
		}
	}

 


	//装备.		
	public void InitEquips(COM_Item[] eqs)
	{
		for(int i =0;i<eqs.Length;i++)
		{
			Equips[eqs[i].slot_] = eqs[i];
		}
	}

	public void wearEquip(COM_Item equip)
	{
        base.wearEquip(equip);

        if (equip.slot_ == (short)EquipmentSlot.ES_Fashion)
        {
            dressChanged_ = true;
            Prebattle.Instance.UpdateSelfOutlook();
        }
	}

	public Employee GetEmployeeById(int instId)
	{
		for(int i=0; i < employee_list_.Count; ++i)
		{
			if(employee_list_[i].InstId == instId)
				return employee_list_[i];
		}
		return null;
	}

	public void delEquipmnet(uint target,uint instId)
	{
//		if (target < 0 || target > employee_list_.Count)
//		{
//			return;
//		}
		COM_Item[] equip;
        Baby baby = GetBabyInst((int)target);
		if(target == GamePlayer.Instance.InstId)
		{
			equip = Equips;
		}
        else if (baby != null)
        {
            equip = baby.Equips;
            baby.isEquipDirty_ = true;
        }
		else
		{
			equip = GetEmployeeById((int)target).Equips;
		}
        int delItemId = 0;
		for(int i= 0;i<equip.Length;i++)
		{
			if(equip[i]==null)
			{
				continue;
			}
			if(equip[i].instId_ == instId)
			{
                delItemId = (int)equip[i].itemId_;
				equip[i] = null;
                if (DelEquipEvent != null)
				{
					DelEquipEvent(target,(uint)i);
				}

				break;
			}
		}

        if(baby != null)
            baby.CalcBabyEquipSkill();
        ItemData item = ItemData.GetData(delItemId);
        if (item != null && item.slot_ == EquipmentSlot.ES_Fashion)
        {
            dressChanged_ = true;
            Prebattle.Instance.UpdateSelfOutlook();
        }
	}

	//
	public void UpdataEquips(List<COM_Item> equipItems)
	{
	}

	public void UpdateEquipments(int instId, COM_Item weapon)
	{
		if(InstId == instId)
            wearEquip(weapon);
        else if (GetBabyInst(instId) != null)
        {
            GetBabyInst(instId).wearEquip(weapon);
        }
        else
        {
            Employee emp = GetEmployeeById(instId);
            if (emp != null)
            {
                emp.wearEquip(weapon);
                UpdataEmployeeInst(emp);
            }
        }

		//if(WearEquipEvent != null)
		//{
		//	WearEquipEvent((uint)instId, weapon);
		//}

		if (EmployeeEquipEvent != null)
		{
			EmployeeEquipEvent((uint)instId, weapon);
		}

	}



	public void DrawEmployeeOk(COM_Item[] employess)
	{
		if(DrawEmployeeEnvent != null)
		{
			DrawEmployeeEnvent(employess);
		}
	}

	public void EmployeeStarOk(int guid, int star, COM_Skill skillInst)
	{
        Employee emp = GetEmployeeById(guid);
        emp.star_ = (uint)star;
        emp.SkillInsts.Add(skillInst);
        UpdataEmployee(emp);
		if(EmployeeStarOkEnvent != null)
		{
            EmployeeStarOkEnvent(emp);
		}
	}

	public void EmployeeEvolveOk(int guid, QualityColor color)
	{
        Employee emp = GetEmployeeById(guid);
        emp.quality_ = color;
        UpdataEmployee(emp);
		if(EmployeeEvolveOkEnvent != null)
		{
            EmployeeEvolveOkEnvent(emp);
		}
	}

	public bool IsSystemOpen(OpenSubSystemFlag ossf,ulong v){
		return (v & (((ulong)0X1) << ((int)ossf))) != 0;
	}

	public ulong OpenSubSystem
	{
		set
		{
			 
			ulong oro = openSubSystemFlag_ ^ value;  
			if(oro != 0)
			{
				for(OpenSubSystemFlag i=OpenSubSystemFlag.OSSF_Skill;i<OpenSubSystemFlag.OSSF_Max;i++)
				{
					if(IsSystemOpen(i,oro))
					{	

						//OpenFunction.Instance.Show(Enum.GetName(typeof(OpenSubSystemFlag),i));
						if(OpenSystemEnvetString != null)
						{
							OpenSystemEnvetString((int)i);

							if((int)i == (int)OpenSubSystemFlag.OSSF_EmployeePos10 || (int)i == (int)OpenSubSystemFlag.OSSF_EmployeePos15 ||(int)i == (int)OpenSubSystemFlag.OSSF_EmployeePos20)
							{
								EmployessSystem.instance.isShowAddPos = true;
							}
							//Enum.GetName(typeof(OpenSubSystemFlag),i));
						}
					}
				}
			}
			//if(openSubSystemFlag_ != value)
			//{
				openSubSystemFlag_ = value;
				if(OpenSubSystemEnvet  != null)
					OpenSubSystemEnvet(openSubSystemFlag_);
			//}

		}
		get
		{
			return openSubSystemFlag_;
		}
	}


    public bool GetOpenSubSystemFlag(OpenSubSystemFlag flag)
    {
        return ((openSubSystemFlag_) & (((ulong)0x1) << (int)flag)) != 0;
    }

    public bool GetSceneAvaliable(int sceneId)
    {
        //Dictionary<int, SceneSimpleData> metaData = SceneSimpleData.GetData();
        //foreach (KeyValuePair<int, SceneSimpleData> pair in metaData)
        //{
        //    if (sceneId != pair.Key)
        //        continue;

        //    if (pair.Value.UnLockQuestID_ == 0)
        //        return true;
        //    else
        //    {
        //        for (int i = 0; i < QuestSystem.CompletedList.Count; i++)
        //            if (QuestSystem.CompletedList[i] == pair.Value.UnLockQuestID_)
        //                return true;
        //    }
        //}
        return openedScenes.Contains(sceneId);
        //return false;
    }

    public void OpenScene(int sceneId)
    {
        if (!openedScenes.Contains(sceneId))
        {
            openedScenes.Add(sceneId);
        }
    }

    public int GetSceneOpenLimit(int sceneId)
    {
        return SceneData.GetData(sceneId).UnLockQuestID_;
    }

    public void UpdateAchievement()
    {
        if (OnAchievementUpdate != null)
            OnAchievementUpdate();
    }

	public COM_Item GetItemInStoreById(int instId)
	{
		for(int i=0; i < StorageItems.Length; ++i)
		{
			if(StorageItems[i] == null)
				continue;

			if(StorageItems[i].instId_ == instId)
				return StorageItems[i];
		}
		return null;
	}

	public COM_Item GetItemInStoreBySlot(int slot)
	{
		if(StorageItems == null || slot >= StorageItems.Length || slot < 0)
			return null;
		return StorageItems[slot];
	}

	public COM_BabyInst GetBabyInStoreById(int instId)
	{
		for(int i=0; i < Storagebaby.Length; ++i)
		{
			if(Storagebaby[i] == null)
				continue;
			
			if(Storagebaby[i].instId_ == instId)
				return Storagebaby[i];
		}
		return null;
	}

	public COM_BabyInst GetBabyInStoreBySlot(int slot)
	{
		if(Storagebaby == null || slot >= Storagebaby.Length || slot < 0)
			return null;
		return Storagebaby[slot];
	}


	public string GetQlityBack(int quality)
	{
		if ((int)quality >= (int)QualityColor.QC_Orange2)
		{
			return "biankuang_cheng";
		}
		if((int)quality <= (int)QualityColor.QC_White)
		{
			return "biankuang_bai";
		}
		else if ((int)quality <= (int)QualityColor.QC_Green)
		{
			return "biankuang_lv";
		}
		else if ((int)quality <= (int)QualityColor.QC_Blue1)
		{
			return "biankuang_lan";
		}
		else if ((int)quality <= (int)QualityColor.QC_Purple2)
		{
			return "biankuang_fen";
		}
		else if ((int)quality <= (int)QualityColor.QC_Golden2)
		{
			return "biankuang_huang";
		}
		else if ((int)quality <= (int)QualityColor.QC_Orange2)
		{
			return "biankuang_cheng";
		}
		else if ((int)quality <= (int)QualityColor.QC_Pink)
		{
			return "biankuang_cheng";
		}
		return "";
	}

    public void UpdateVip(int lv)
    {
        vipLevel_ = lv;
        if (OnVipUpdate != null)
            OnVipUpdate();
    }

    public void UpdateVipTime(int leftSec)
    {
        int day = leftSec / 86400;
        vipLeftDays_ = day < 1 ? 1 : day;
        if (OnVipUpdate != null)
            OnVipUpdate();
    }

	public void updateDoubleTime(bool open,float time)
	{
		isOpenDoubleTime = open;

		_doubleTimeStart =  (uint)Time.realtimeSinceStartup;
		doubleTime = time;

		if(doubleTimeEnvet != null)
		{
			doubleTimeEnvet(doubleTime);
		}
	}


	public float getDoubleTime
	{
		get
		{ 
			return (uint)Mathf.Max(0, doubleTime - (Time.realtimeSinceStartup - _doubleTimeStart));
		}
	}

	public void fixItemOk(COM_Item item)
	{
		Equips [item.slot_] = item;
		if(fixItmeEnvet != null)
		{
			fixItmeEnvet(item);
		}
	}
	public void MakeDebrisItemOk()
	{
		if(DebrisItmeEnvet != null)
		{
			DebrisItmeEnvet(1);
		}
	}

	public void updateMagicItem(int level,int exp)
	{
		MagicItemLevel = level;
		MagicItemExp = exp;

		if(MagicItmeEnvet != null)
		{
			MagicItmeEnvet(1);
		}
	}


	public int MagicItemLevel
	{
		set
		{
			_magicItemLevel = value;
            MagicLevelDirty = true;
		}
		get
		{
			return _magicItemLevel;
		}
	}

	public int MagicItemExp
	{
		set
		{
			_magicItemExp = value;
		}
		get
		{
			return _magicItemExp;
		}
	}

	public JobType MagicItemJob
	{
		set
		{
			_magicItemJob = value;
			if(MagicItmeJobEnvet != null)
				MagicItmeJobEnvet((int)_magicItemJob);
		}
		get
		{
			return _magicItemJob;
		}
	}

	public int MagicTupoLevel
	{
		set
		{
			_magicTupoLevel = value;
			if(MagicItmeTupoEnvet != null)
				MagicItmeTupoEnvet(value);
		}
		get
		{
			return _magicTupoLevel;
		}
	}

    public void UpdateAmeInfo(bool bOk)
    {
        if (OnAutoMeetEnemy != null)
            OnAutoMeetEnemy(bOk);
    }


	public uint[] EmployeesBattleGroup1
	{
		set
		{
			_EmployeesBattleGroup1 = value;
		}
		get
		{
			return _EmployeesBattleGroup1;
		}
	}

	public uint[] EmployeesBattleGroup2
	{
		set
		{
			_EmployeesBattleGroup2 = value;
		}
		get
		{
			return _EmployeesBattleGroup2;
		}
	}

	public EmployeesBattleGroup CurEmployeesBattleGroup
	{
		set
		{
			_EmployeesBattleGroup =value;
		}
		get
		{
			return _EmployeesBattleGroup;
		}
	}

	public uint[] GetEmployeesBattles(int group)
	{
		if(group == (int)EmployeesBattleGroup.EBG_GroupTwo)
		{
			return _EmployeesBattleGroup2;
		}
		else
		{
			return _EmployeesBattleGroup1;
		}
	}


	public void UpdataEmployeesBattle(int instId,EmployeesBattleGroup group,bool isBattle)
	{
		uint[] emps = new uint[4];
		if(group == EmployeesBattleGroup.EBG_GroupOne)
		{
			emps = EmployeesBattleGroup1;
		}
		else if(group == EmployeesBattleGroup.EBG_GroupTwo)
		{
			emps = EmployeesBattleGroup2;
		}

		if(isBattle)
		{
			for(int i= 0;i<emps.Length;i++)
			{
				if(emps[i] == 0)
				{
					emps[i]= (uint)instId;
					break;
				}
			}
		}
		else
		{
			for(int j= 0;j<emps.Length;j++)
			{
				if(emps[j] == (uint)instId)
				{
					emps[j]= 0;
					break;
				}
			}
		}
		employee_list_.Sort (SortEmployess);
		Employee emp = GamePlayer.Instance.GetEmployeeById(instId);
		emp.isForBattle_ = isBattle;
		
		if (UpdateEmployeeEnvent != null)
			UpdateEmployeeEnvent(emp,(int)group);

	}

	public List<int> OpenFunEffectBtns
	{
		set
		{

		}
		get
		{
			return _openFunEffectBtns;
		}
	}

	public bool IsCanUseSkillExpItem()
	{
		for(int i=0;i<SkillInsts.Count;i++)
		{
			if(SkillData.GetData((int)SkillInsts[i].skillID_,(int)SkillInsts[i].skillLevel_)._Proficiency  > 0)
				return true;
		}

		return false;
	}


	public int MaxUseAllNum(COM_Item item,int stack)
	{
		
		int playerLevel = GamePlayer.Instance.GetIprop (PropertyType.PT_Level);
		int babyLevel = GamePlayer.Instance.BattleBaby.GetIprop (PropertyType.PT_Level);
		if (babyLevel - playerLevel >= 5)
			return 0;
		int bExp =  GamePlayer.Instance.BattleBaby.GetIprop (PropertyType.PT_Exp);
		int count = 0;
		ItemData ida = ItemData.GetData ((int)item.itemId_);
		for(int i =0;i<stack;i++)
		{
			bExp +=ida.AddValue_;
			if(bExp < ExpData.GetBabyMaxExp(playerLevel+5))
			{
				count++;
			}
		}
		return count;
	}
	public void initItemStorage(ushort num,COM_Item[] items)
	{
		BankSystem.instance.itemNum = num;
		for(int i =0;i<GamePlayer.Instance.StorageItems.Length;i++)
		{
			GamePlayer.Instance.StorageItems[i] = null;
		}
		
		for(int i =0;i<items.Length;i++)
		{
			GamePlayer.Instance.StorageItems[items[i].slot_] = items[i];
		}
		isInitStorageItem = true;
		if(BankSystem.instance.isOpeninitBank)
		{
			if(isInitStorageBaby && isInitStorageItem)
			{
				BankSystem.instance.isOpeninitBank = false;
				NetWaitUI.HideMe();
				BankControUI.ShowMe(BankSystem.instance.isOpenBankType);
                
			}
		}
	}
	public void initBabyStorage(ushort num,COM_BabyInst[] babys)
	{
		BankSystem.instance.babyNum = num;
		for(int i =0;i<babys.Length;i++)
		{
			GamePlayer.Instance.Storagebaby[babys[i].slot_] = babys[i];
		}
		isInitStorageBaby = true;

		if(BankSystem.instance.isOpeninitBank)
		{
			if(isInitStorageBaby && isInitStorageItem)
			{
				BankSystem.instance.isOpeninitBank = false;
				NetWaitUI.HideMe();
				BankControUI.ShowMe(BankSystem.instance.isOpenBankType);
			}
		}
		
	}


	public List<COM_Skill>  GetMianSkillList()
	{
		List<COM_Skill> mianSkill = new List<COM_Skill> ();
		List<string> skillStr = new List<string> ();
		List<COM_Skill> skills = SkillInsts;
		for(int i =0;i<skills.Count;i++)
		{
			SkillData sData = SkillData.GetData((int)skills[i].skillID_,(int)skills[i].skillLevel_);
			if(sData._SkillType== SkillType.SKT_Active || sData._SkillType == SkillType.SKT_Passive ||sData._SkillType ==  SkillType.SKT_CannotUse)
			{
				if(sData._Name == "null")
				{
					continue;
				}
				if (!skillStr.Contains(sData._Name))
				{
					skillStr.Add(sData._Name);
					mianSkill.Add(skills[i]);
				}
				else
				{
					for(int j = 0;j<mianSkill.Count;j++)
					{
						if (SkillData.GetData((int)mianSkill[j].skillID_, (int)mianSkill[j].skillLevel_)._Name == sData._Name)
						{
							if(SkillData.GetData((int)mianSkill[j].skillID_,(int)mianSkill[j].skillLevel_)._Level < sData._Level)
							{
								mianSkill[j] = skills[i];
								break;
							}
						}
					}
				}
			}
		}
		
		return mianSkill;
	}

    public List<int> newBuffSlot = new List<int>();
	public void AddState(COM_State State)
	{
		base.AddState (State);
        int slot = BuffID2Slot(State.stateId_);
        if(!newBuffSlot.Contains(slot))
            newBuffSlot.Add(slot);
        buffIsDirty_ = true;
	}

	public void UpdateState(COM_State state)
	{ 
		base.UpdateState(state);
        buffIsDirty_ = true;
	}

    public int BuffID2Slot(uint id)
    {
        int hpid = 0;
        GlobalValue.Get(Constant.C_HPBuffShopID, out hpid);
        int mpid = 0;
        GlobalValue.Get(Constant.C_MPBuffShopID, out mpid);

        if (id == hpid)
            return 0;
        else if (id == mpid)
            return 1;
        return 2;
    }

    //public int Slot2BuffID(int slot)
    //{
    //    int hpid = 0;
    //    GlobalValue.Get(Constant.C_HPBuffShopID, out hpid);
    //    int mpid = 0;
    //    GlobalValue.Get(Constant.C_MPBuffShopID, out mpid);

    //    if (slot == 0)
    //        return hpid;
    //    else if (slot == 1)
    //        return mpid;
    //    return 2;
    //}

    public COM_State GetBuff(int slot)
    {
        int hpid = 0;
        GlobalValue.Get(Constant.C_HPBuffShopID, out hpid);
        int mpid = 0;
        GlobalValue.Get(Constant.C_MPBuffShopID, out mpid);
        if (slot == 0)
        {
            for (int i = 0; i < buffList_.Count; ++i)
            {
                if ((int)buffList_[i].stateId_ == hpid)
                    return buffList_[i];
            }
        }
        else if (slot == 1)
        {
            for (int i = 0; i < buffList_.Count; ++i)
            {
                if ((int)buffList_[i].stateId_ == mpid)
                    return buffList_[i];
            }
        }
        else
        {
            for (int i = 0; i < buffList_.Count; ++i)
            {
                if ((int)buffList_[i].stateId_ != mpid && (int)buffList_[i].stateId_ != hpid)
                    return buffList_[i];
            }
        }
        return null;
    }

	public COM_ContactInfo[]  ContactInfoList
	{
		set
		{
			contactInfoList = value;
			ArenaPvpPanelUI.SwithShowMe();
		}
		get
		{
			return contactInfoList;
		}
	}

    public int DaysOld
    {
        get
        {
            try
            {
				long adjCreaTime = createTime_ / 86400 * 86400;
				adjCreaTime -= 28800;
				long totalSec = Define.GetTimeStamp() - adjCreaTime;
				int days = (int)(totalSec / 86400 + (totalSec % 86400 > 0 ? 1 : 0));
				return days;
            }
            catch (System.Exception ex)
            {
				return 7;
            }
        }

    }

	public int GetCopyNum(int cId)
	{
		int num = 0;
		for(int i = 0;i<copyNum_.Count;i++)
		{

			if(cId == copyNum_[i])
			{
				num ++;
			}
		}
		return num;
	}

	public void joinCopySceneOK(int id)
	{
		copyNum_.Add ((uint)id);
		if (CopyNumEnvet != null)
			CopyNumEnvet (id);
	}

	public void firstRechargeOK(bool b)
	{
		isShouchong = b;
		if(shouChongEnvet != null)
			shouChongEnvet(b);
	}
	
	public void firstRechargeGiftOK(bool b)
	{
		isGetShouchong = b;
		if(getShouChongEnvet != null)
			getShouChongEnvet(b);
	}

	public void updateMySelfRecharge(COM_ADChargeTotal Total)
	{
		myselfRecharge_ = Total;
		if (getFanliEnvet != null)
			getFanliEnvet (Total);
	}

	public void requestLevelGiftOK(int level)
	{
		if (levelgift.Contains ((uint)level))
			return;
		levelgift.Add ((uint)level);

		MoreActivityData.instance.UpdateLevelUpRad ();
		if (moreLevelEnvet != null)
			moreLevelEnvet (level);
	}

	public void sycnVipflag(bool flag)
	{
		vipRewardfig = flag;
		if (vipRewardfigEnvet != null)
			vipRewardfigEnvet (flag);
	}

	public void signUp(bool sign)
	{
		TodaySiged = sign;
		SignUpManager.Instance.SignUpOk();
	}

	bool TodaySiged
	{
		set
		{
			todaySigned_ = value;
			if(value)
			{
				MoreActivityData.instance.SetTypeRad((int)ADType.ADT_Sign, 0);
			}
			else
			{
				MoreActivityData.instance.SetTypeRad((int)ADType.ADT_Sign, 1);
			}
		}

	}


    //public void SetUpdateStateEventHandler(RequestEventHandler<int> hander)
    //{
    //    AddBuffEnvet += hander;
    //    for (int i = 0; i < buffList_.Count; ++i)
    //    {
    //        if (AddBuffEnvet != null)
    //        {
    //            AddBuffEnvet(BuffID2Slot(buffList_[i].stateId_));
    //        }

    //    }
    //}
	public void updateMinGiftActivity(COM_ADGiftBag miaosha)
	{
		miaoshaData_ = miaosha;
		if (MiaoshaEnvet != null)
			MiaoshaEnvet (miaosha);
	}


	public void updateLevelRewardShop(COM_CourseGift[] list)
	{
		levelShopList.Clear ();
		levelShopList.AddRange(list);
		if (levelRewadEnvet != null)
			levelRewadEnvet (1);
		if (levelShopList.Count > 0)
		ShopCDTime = (float)levelShopList [0].timeout_; 


	}

	public float ShopCDTime
	{
		set
		{
			levelShopTimeStart = (uint)Time.realtimeSinceStartup;
			levelremainCDTime  = value;
			//_remainCDTime  = 10000;
		}
		get
		{ 
			return (uint)Mathf.Max(0, levelShopTimeStart - (Time.realtimeSinceStartup - levelremainCDTime));
		}
	}


	public void  wearFuwenOk(COM_Item item)
	{
		fuwens_ [item.slot_] = item;	
		if(fuwenEnvet != null)
		{
			fuwenEnvet(item.slot_);
		}
	}
	
	public void takeoffFuwenOk(int slot)
	{
		fuwens_ [slot] = null;
		if(fuwenEnvet != null)
		{
			fuwenEnvet(slot);
		}
	}

	public void compFuwenOk()
	{
		if (fuwenCompoundOkEnvet != null)
			fuwenCompoundOkEnvet (1);
	}

	public bool GetADTypeIsOpen(ADType type)
	{
		bool isOpen = false;
		for(int i=0;i<adTypes.Count;i++)
		{
			if(adTypes[i] == type)
			{
				isOpen = true;
				break;
			}
		}
		return isOpen;
	}
}

