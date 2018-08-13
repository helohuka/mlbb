using UnityEngine;
using System.Collections.Generic;
using System.Net.NetworkInformation;

public enum AnimatorParamType
{
	APT_None,
	APT_Trigger,
	APT_Boolean,
	APT_Integer,
	APT_Float,
	APT_Max,
}

public static class GlobalValue
{
    //TODO
#region StageName
    public const string StageName_LoginScene = "LoginScene";
    public const string StageName_ReLoginScene = "ReturnScene";
	public const string StageName_MainScene = "MainScene";
    public const string StageName_AttackScene_Maze = "MazeBattleScene";
    public const string StageName_AttackScene_HaiDi = "HaiDiBattle";
    public const string StageName_AttackScene = "AttackScene";
    public const string StageName_AttackScene_CunZhuang2 = "CunZhuang2";
    public const string StageName_HundredPkScene = "Cloister_zhandou";
    public const string StageName_ZhaozeScene = "Swamp";
    public const string StageName_Zhaoze1Scene = "Swamp1";
    public const string StageName_AttackZhaozeScene = "Swamp_zhandou";
    public const string StageName_Desert_zhandou = "Desert_zhandou";
	public const string StageName_CreateRoleScene = "CreateRoleScene";
	public const string StageName_LoadingScene = "LoadingScene";
	public const string StageName_MazeScene = "MazeScene";
    public const string StageName_HaMazeScene = "Ha_Maze";
    public const string StageName_HaMaze1Scene = "Ha_Maze1";
    public const string StageName_HaBattleScene = "Ha_zhandou";
    public const string StageName_piantoudonghuaf = "PalaceScene";
    public const string StageName_groupScene = "groupscene";
    public const string StageName_MineScene = "Mine";
    public const string StageName_MineZhandou = "Mine_zhandou";
    public const string StageName_Snow = "Snow";
	public const string StageName_SnowZhandou = "Snow_zhandou";
    public const string StageName_LabScene = "laboratoryRoom";
    public const string StageName_Lab1Scene = "laboratoryRoom1";
    public const string StageName_DesertScene = "Desert";
    public const string StageName_DesertMazeScene = "Desert_Maze";
    public const string StageName_HaidiScene = "Submarine";
    public const string StageName_Haidi1Scene = "Submarine1";
    public const string StageName_ArenaScene = "Arena1";
    public const string StageName_PubScene = "Pub";
    public const string StageName_JiazuPkScene = "jiazhupk";

    public static List<string> battleScenes_;

    public static List<string> fbScenes_;

    public static void AddBattleScene(string sceneName)
    {
        if(battleScenes_ == null)
            battleScenes_ = new List<string>();
        if(!battleScenes_.Contains(sceneName))
            battleScenes_.Add(sceneName);
    }

    public static void AddFBScene(string sceneName)
    {
        if(fbScenes_ == null)
            fbScenes_ = new List<string>();
        if(!fbScenes_.Contains(sceneName))
            fbScenes_.Add(sceneName);
    }

    public static bool isBattleScene(string sceneName)
    {
        //bool flag = sceneName.Equals(StageName_AttackScene_Maze) ||
        //    sceneName.Equals(StageName_AttackScene_HaiDi) ||
        //    sceneName.Equals(StageName_AttackScene) ||
        //    sceneName.Equals(StageName_AttackScene_CunZhuang2) ||
        //    sceneName.Equals(StageName_HundredPkScene) ||
        //    sceneName.Equals(StageName_HaBattleScene) ||
        //    sceneName.Equals(StageName_AttackZhaozeScene) ||
        //    sceneName.Equals(StageName_Desert_zhandou) ||
        //    sceneName.Equals(StageName_MineZhandou) ||
        //    sceneName.Equals(StageName_SnowZhandou);

        bool flag = battleScenes_.Contains(sceneName);

        if (flag == false)
            ClientLog.Instance.LogWarning(" scene : " + sceneName + "  is not BattleScene!");
        return flag;
    }

    public static bool isFBScene(string sceneName)
    {
        //bool flag = sceneName.Equals(StageName_MainScene) ||
        //    sceneName.Equals(StageName_HaMazeScene) ||
        //    sceneName.Equals(StageName_HaMaze1Scene) ||
        //    sceneName.Equals(StageName_MineScene) ||
        //    sceneName.Equals(StageName_LabScene) ||
        //    sceneName.Equals(StageName_Lab1Scene) ||
        //    sceneName.Equals(StageName_ZhaozeScene) ||
        //    sceneName.Equals(StageName_Zhaoze1Scene) ||
        //    sceneName.Equals(StageName_HaidiScene) ||
        //    sceneName.Equals(StageName_Haidi1Scene) ||
        //    sceneName.Equals(StageName_ArenaScene) ||
        //    sceneName.Equals(StageName_PubScene) ||
        //    sceneName.Equals(StageName_Snow);

        bool flag = fbScenes_.Contains(sceneName);

        if (flag == false)
            ClientLog.Instance.LogWarning(" scene : " + sceneName + "  is not FbScene!");
        return flag;
    }
#endregion

	public const string MainSceneXml = "Scene_1";

#region GlobalGameObject
	public const string GlobalGameObjectName = "GlobalObject";
#endregion

#region ActionName
	public const string ActionName			= "state";
	public const int	Action_Idle 		= 0;
	public const int	Action_Walk 		= 1;
	public const int 	Action_Run 			= 2;
	public const int 	Action_Attack		= 3;
	public const int	Action_Sing			= 4;
	public const int	Action_BeAttack_1 	= 5;
	public const int	Action_Die			= 6;
	public const int	Action_BeAttack_2 	= 7;
	public const int	Action_Deference_1 	= 8;
	public const int	Action_Deference_2	= 9;
	public const int	Action_Dodge		= 10;

	public const string TTakeDmg = "t_takeDmg";
	public const string FMove = "f_move";
	public const string TDeference = "t_def";
	public const string BDead = "b_dead";
	public const string TAttack = "t_attack";
	public const string TDodge = "t_dodge";
	public const string TCast = "t_cast";
    public const string BBossIn = "ruchang";
#endregion

    public static int GetAttackID(WeaponType weaponType)
    {
        switch(weaponType)
        {
            case WeaponType.WT_None:
                return 1;
            case WeaponType.WT_Axe:
            case WeaponType.WT_Sword:
            case WeaponType.WT_Stick:
                return 8;
            case WeaponType.WT_Spear:
                return 9;
            case WeaponType.WT_Bow:
                return 10;
            case WeaponType.WT_Knife:
            //case WeaponType.WT_V:
                return 11;
            default:
                return 1;
        }
    }

    public static int GetAttackID(JobType jt)
    {
        switch (jt)
        {
            case JobType.JT_Newbie:
            case JobType.JT_Fighter:
                return 1;
            case JobType.JT_Axe:
            case JobType.JT_Sword:
            case JobType.JT_Ninja:
            case JobType.JT_Sage:
            case JobType.JT_Wizard:
            case JobType.JT_Word:
            case JobType.JT_Mage:
                return 8;
            case JobType.JT_Knight:
                return 9;
            default: 
                return 1;
        }
    }

    public static int GetAttackID(Actor actor)
    {
        WeaponType wt = actor.GetWeaponType();
        switch (wt)
        {
            case WeaponType.WT_None:
                return GetAttackID((JobType)actor.GetIprop(PropertyType.PT_Profession));
            case WeaponType.WT_Axe:
            case WeaponType.WT_Sword:
            case WeaponType.WT_Stick:
                return 8;
            case WeaponType.WT_Spear:
                return 9;
            case WeaponType.WT_Bow:
                return 10;
            case WeaponType.WT_Knife:
                //case WeaponType.WT_V:
                return 11;
            default:
                return 1;
        }
    }

    public static int GetAttackID(BattleActor actor)
    {
        WeaponType wt = WeaponType.WT_None;
        ItemData weapon = ItemData.GetData(actor.battlePlayer.weaponItemId_);
        if (weapon != null)
            wt = weapon.weaponType_;
        switch (wt)
        {
            case WeaponType.WT_None:
                return GetAttackID((JobType)actor.battlePlayer.jt_);
            case WeaponType.WT_Axe:
            case WeaponType.WT_Sword:
            case WeaponType.WT_Stick:
                return 8;
            case WeaponType.WT_Spear:
                return 9;
            case WeaponType.WT_Bow:
                return 10;
            case WeaponType.WT_Knife:
                //case WeaponType.WT_V:
                return 11;
            default:
                return 1;
        }
    }

    public static UIASSETS_ID GetMoreActivityID(ADType type)
    {
        switch (type)
        {
            case ADType.ADT_Cards:
                return UIASSETS_ID.UIASSETS_MoreActivityCards;
		case ADType.ADT_ChargeTotal:
			return UIASSETS_ID.UIASSETS_GoodActivityTotalPanel;
		case ADType.ADT_ChargeEvery:
			return UIASSETS_ID.UIASSETS_GoodaActivityOnePanel;
		case ADType.ADT_DiscountStore:
			return UIASSETS_ID.UIASSETS_GoodActivityShopPanel;
		case ADType.ADT_LoginTotal:
			return UIASSETS_ID.UIASSETS_GoodActivitySigninPanel;
		case ADType.ADT_OnlineReward:
			return UIASSETS_ID.UIASSETS_OnlinePanel;
        case ADType.ADT_HotRole:
            return UIASSETS_ID.UIASSETS_MoreActivityHotRole;
		case ADType.ADT_Foundation:
			return UIASSETS_ID.UIASSETS_GrowthfundPanel;
		case ADType.ADT_SelfChargeTotal:
			return UIASSETS_ID.UIASSETS_GoodActivitySelfTotalPanel;
		case ADType.ADT_SelfChargeEvery:
			return UIASSETS_ID.UIASSETS_GoodaActivitySelfOnePanel;
		case ADType.ADT_SelfDiscountStore:
			return UIASSETS_ID.UIASSETS_GoodActivitySelfShopPanel;
            case ADType.ADT_7Days:
            return UIASSETS_ID.UIASSETS_7DaysPanel;
		case ADType.ADT_BuyEmployee:
			return UIASSETS_ID.UIASSETS_MoreBuyEmp;
		case ADType.ADT_Level:
			return UIASSETS_ID.UIASSETS_MoreLevel;
		case ADType.ADT_PhoneNumber:
			return UIASSETS_ID.UIASSETS_PhoneBinding;
		case ADType.ADT_Sign:
			return UIASSETS_ID.UIASSETS_SignUpUI;
		case ADType.ADT_Zhuanpan:
			return UIASSETS_ID.UIASSETS_LotteryPanel;
		case ADType.ADT_IntegralShop:
			return UIASSETS_ID.UIASSETS_JiFenShop;
        default:
            return (UIASSETS_ID)0;
        }
    }

    static public Color RED = new Color(255, 0, 0);
    static public Color GREEN = new Color(0, 152, 57);
    static public Color YELLOW = new Color(255, 139, 0);

    static public string PlayerNameInScene = "[b][FFFF2F]{0}[-][-]";
    static public string OtherPlayerNameInScene = "[b][42FFFD]{0}[-][-]";
    static public string EnemyNameInScene = "[b][FF0000]{0}[-][-]";

    static public string NickNameInScene = "[b][20FF55]<{0}>[-][-]\n";

    public const int    GuideBattleID = 1;

	public const int	SKILL_DEFENSEID = 2;
	public const int 	SKILL_CHANGEPOSID = 4;
	public const int 	SKILL_COUNTERID = 5;
	public const int	SKILL_FLEEID	= 6;
	public const int	SKILL_BABYINNOUT	= 7;
    public const int    SKILL_ZhuaChong = 13;
	public const string ActorObjName	= "Actor";
	public const string ActorUIObjName  = "ActorUI";

    public const int    EFFECT_PlayerLvUpOnUI = 1001;
    public const int    EFFECT_Guaguaka = 1002;
    public const int    EFFECT_AcceptQuest = 1003;
    public const int    EFFECT_SkillLevelUp = 1004;
    public const int    EFFECT_JinhuaSuccess = 1005;
    public const int    EFFECT_JinjieSuccess = 1006;
    public const int    EFFECT_JinjieStar = 1007;
    public const int    EFFECT_EnterScene = 1008;
    public const int    LevelUpEffectId = 1009;
    public const int    EFFECT_ComplishQuest = 1010;
    public const int    EFFECT_EquipCombie = 1011;
    public const int    EFFECT_Zhaomu = 1012;
    public const int    EFFECT_Zhiyin = 1013;
    public const int    EFFECT_TenComboZhaomu = 1014;
    public const int    EFFECT_Critical = 45;
	public const int    EFFECT_Caiji = 1019;
    public const int    EFFECT_Fanji = 48;
    public const int    EFFECT_FanjiCast = 20;
    public const int    EFFECT_Heji = 49;
    public const int    EFFECT_Dead = 51;
	public const int 	EFFECT_Butterfly = 1025;
    public const int    QuestDestEffectId = 1027;
	public const int    EFFECT_PlayerLvUpInScene = 1028;
    public const int    EFFECT_AchievementTrack = 1029;
    public const int    EFFECT_AchievementIcon = 1030;
	public const int    EFFECT_QuestAchiev = 1033;
	public const int    EFFECT_ChongwuNo = 1034;
	public const int    EFFECT_chilun = 1051;
	public const int    EFFECT_kaiqihuoban = 1053;
	public const int    EFFECT_UI_huidacuowu = 1057;
	public const int    EFFECT_UI_huidazhengque = 1058;
	public const int    EFFECT_UI_huodongkaishi = 1059;
	public const int    EFFECT_UI_pinzhijinjie = 1060;
	public const int    EFFECT_UI_juepinItem = 1061;
	public const int    EFFECT_UI_employeeStarOk = 1062;
	public const int    EFFECT_UI_employeejinjie1 = 1063;
	public const int    EFFECT_UI_employeejinjie2 = 1064;
	public const int    EFFECT_UI_magicTupo = 1065;
	public const int    EFFECT_familyLevelUp = 1067;
	public const int    EFFECT_familyCaiji = 1068;
	public const int    EFFECT_familyZhufu = 1069;
	public const int    EFFECT_jiazhuzhankaiqi = 1070;
	public const int    EFFECT_jiazhuzhanjieshu = 1071;
	public const int    EFFECT_jiazuzhandoukaishi = 1072;
	public const int    EFFECT_jiazuzhandoushengli = 1073;
	public const int    EFFECT_jiazuzhanshibai = 1074;
	public const int    EFFECT_fuwenhecheng = 1075;
	public const int    EFFECT_zhihuancg = 1076;
	public const int    EFFECT_zhihuanKS = 1077;
	public const int    EFFECT_zhihuanSb = 1078;
	public const int    EFFECT_magicTuposuolian = 1079;
	public const float MoveMinGap = 0.5f;
    public const float MoveAnimationCondition = 0.9f;

    public static bool GuideOpen = true;

    public const string PlayerSelectSkill = "PlayerSelectSkill";

    /// <summary>
    /// 
    /// </summary>
    static int[] Integers = new int[(int)Constant.C_Max];
    static string[] Strings = new string[(int)Constant.C_Max];
    static float[] Floats = new float[(int)Constant.C_Max];

    public static void Get(Constant index, out int val)
    {
        val = Integers[(int)index];
    }

    public static void Get(Constant index, out string val)
    {
        val = Strings[(int)index];
    }

    public static void Get(Constant index, out float val)
    {
        val = Floats[(int)index];
    }

    public static void Set(Constant index, int val)
    {
        Integers[(int)index] = val;
    }

    public static void Set(Constant index, string val)
    {
        Strings[(int)index] = val;
    }

    public static void Set(Constant index, float val)
    {
        Floats[(int)index] = val;
    }

    /*
     * 中央服务器地址
     * 包含：
     *      总渠道公告地址
     *      各区服公告地址
     *      服务器列表地址
     *      
     *      （AnySDK后台）
     *      渠道登录验证地址
     *      渠道支付验证地址
     */
#if UNITY_IOS
    public static string centerservhost = "https://loginmhflc.tanyu.mobi/";
#else
	public static string centerservhost = "http://loginmhflc.tanyu.mobi/";
#endif
    public static string resVersion = "servs/cdn";
    public static string sysNotice = "notice/system";
    public static string servNotice = "notice/server";
    public static string servListUrl = "servs/query";

    /*
     * 资源cdn服务器地址
     */
#if UNITY_IOS
    public static string cdnservhost = "https://z5.tanyu.mobi/mhflc/Version/";
#else
	public static string cdnservhost = "http://z5.tanyu.mobi/mhflc/Version/";
#endif

    //获取渠道号 如果是"" 则由渠道sdk赋值
    public static string channelID = "999";
	
    public static bool IsDebugMode
	{
		get
		{
			bool isDebug = channelID.Equals(TestChannelID) || channelID.Equals(AnysdkChannelID);
			return isDebug;
		}
	}
	//开发测试的ChannelID is 9
	public const string TestChannelID = "999";

    //anysdk的ChannelID is 999999
    public const string AnysdkChannelID = "999999";

    //备案包
    public const string BackupChannelID = "999998";

    //审核包
    public const string ReviewChannelID = "999997";
}