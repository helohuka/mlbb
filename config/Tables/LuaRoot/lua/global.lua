Sys.log("running Global.lua")

Global.setFloat(C_SceneVisibleMaxRadius,5); --个人最大可见半径
Global.setInt(C_SceneVisibleMaxNum,25); --同场景最大可见人数

--GM开关
Global.setInt(C_GM,1);
--//MAZE RAND TIME
Global.setFloat(C_PlayerMaxLevel,70);
Global.setFloat(C_OnceMaxExp,80000000);
Global.setFloat(C_MazeRandTime,   21600); 
Global.setInt(C_TiliMax, 100);
Global.setInt(C_TiliResetInterval, 1800);
Global.setInt(C_TiliResetValue, 5);
Global.setInt(C_JJCRivalNum,5);
Global.setFloat(C_JJCRivalTimeCD, 3);
Global.setInt(C_JJCOpenlevel,20);
Global.setInt(C_PVPJJCOpenlevel,35);
Global.setFloat(C_PVPJJCMeanTime1,30);
Global.setFloat(C_PVPJJCMeanTime2,60);
Global.setFloat(C_PVPJJCMeanTime3,120);
Global.setFloat(C_PVPJJCMeanTime4,180);
Global.setFloat(C_PVPJJCMeanTime5,240);
Global.setFloat(C_PVPJJCMeanTime6,300);

Global.setInt(C_WarriorTrophylow,241139);		--勇者选拔三种奖励DROPID
Global.setInt(C_WarriorTrophymiddle,241138);
Global.setInt(C_WarriorTrophyhigh,241137);
Global.setInt(C_WarriorTrophyMax,5);		--勇者选拔箱子最大个数

Global.setInt(C_FamilyShopConsume,20);

Global.setInt(C_CompRunesNeedNum,3);              --合成符文需要数量

Global.setInt(C_CrystalNeedItem,21365);               --水晶升级所需道具id
Global.setInt(C_CrystalNeedDiamond,20);               --水晶置换花费数 钻石

Global.setInt(C_AccecptRandQuestLimit, 200);                     --任务链相关
Global.setInt(C_SubmitRandQuestRewaredNumber0, 100);
Global.setInt(C_SubmitRandQuestRewaredNumber1, 200);
Global.setInt(C_SubmitRandQuestRewared0, 101001);
Global.setInt(C_SubmitRandQuestRewared1, 101002);
Global.setInt(C_CleanRandQuestWeekDay,1);


Global.setInt(C_LearnSkillMaxNum, 10);
Global.setInt(C_RandomMin, 250);
Global.setInt(C_RandomMax, 750);
Global.setInt(C_EquipDurVar,80);

Global.setInt(C_WishNpcID, 9572);
Global.setInt(C_WishDistance, 4);
Global.setInt(C_MonsterSneakAttack,5);
Global.setFloat(C_BoxGreenTimeCD,600);
Global.setFloat(C_BoxBlueTimeCD,86400);
Global.setInt(C_BoxGreenFreeNum,5);
Global.setInt(C_BoxGreenSpend, 10000);
Global.setInt(C_BoxBlueSpendItem,4511);
Global.setInt(C_BoxBlueSpend,2);
Global.setInt(C_BoxGoldSpend,20);
Global.setInt(C_BoxBlueSpendDiamond,200);
Global.setInt(C_BoxGoldSpendDiamond,1800);
Global.setInt(C_Employeelasttime,20);
Global.setInt(C_EmployeeRare,5);
Global.setInt(C_EmpDelFeedback1,0);
Global.setInt(C_EmpDelFeedback2,8500);
Global.setInt(C_EmpDelFeedback3,32000);
Global.setInt(C_EmpDelFeedback4,76500);
Global.setInt(C_EmpDelFeedback5,170000);

Global.setInt(C_InitMoney,0);
Global.setInt(C_InitDiamond,0);

Global.setInt(C_GapWorldLevel_One,10);
Global.setInt(C_GapWorldLevel_Two,20);
Global.setFloat(C_GapWorldLvExtra_One,0.5);
Global.setFloat(C_GapWorldLvExtra_Two,0.75);
Global.setFloat(C_GapWorldLvExtra_Three,1);

Global.setInt(C_HundredTier,51);
Global.setInt(C_HundredBattle,35);
Global.setInt(C_HundredChallengeNum,1);

Global.setInt(C_PetActivityNum,1);

Global.setFloat(C_RankUpdataTime,60);

Global.setInt(C_SignPay,20);
Global.setInt(C_SignReward7,5113);
Global.setInt(C_SignReward14,5114);
Global.setInt(C_SignReward28,5115);

Global.setInt(C_ResetBabyPay,4509);
Global.setInt(C_SkillExpItem,1757);
Global.setInt(C_BabyExpItem,1762);
Global.setInt(C_DoubleExpItem,1719);
Global.setInt(C_ResetPlayerPay,1723);


Global.setInt(C_BabyLoyalMin,60);
Global.setInt(C_BabyLoyalMax,100);
Global.setInt(C_BabyLoyalVar,20);
Global.setInt(C_AttackSkillId,1);
Global.setInt(C_DefenseSkillId,2);
Global.setInt(C_NothingSkillId,1);
Global.setInt(C_BabyLoyalSkillId,2591);

Global.setInt(C_BabyStrongSkillId,2601);

Global.setInt(C_TeamMaxSize,2000);

--//背包
Global.setInt(C_BagInitSize,60);
Global.setInt(C_BagMaxSize,100);

Global.setInt(C_MineTimeMax,5); --//每天最大挖矿次数
Global.setFloat(C_EverydayDoubleExp, 3600); --//每天赠送双倍经验 秒，一小时
Global.setFloat(C_DoubleExpMax,21600);		--秒，六小时


Global.setInt(C_MushroomHitMax,20);

Global.setInt(C_TongjiTimesMax,19);
Global.setInt(C_TongjiMaxDrop,99);
Global.setInt(C_TongjiTeamMemberLevelMin,20);
Global.setInt(C_TongjiTeamSizeMin,1);

Global.setInt(C_XijiHitMax,20);

--//答题
Global.setInt(C_ExamOpenLevel,20);
Global.setInt(C_ExamNumMax,20);
Global.setInt(C_ExamRight10,5116);
Global.setInt(C_ExamRight20,4509);

--//帮派
Global.setInt(C_CreateGuildLevel,30);
Global.setInt(C_CreateGuildGold, 200000);
Global.setInt(C_RequsetJoinGuildMax,30);
Global.setInt(C_GuildMemberMaxNum, 20);
Global.setInt(C_VicePremierMaxNum, 2);
Global.setInt(C_FamilyOneDayFundzLose, 10000);
Global.setInt(C_FamilyNoMoneyDays, 7);
Global.setInt(C_CreateGuildItem, 21362);
Global.setInt(C_FamilyBattleLevelMin, 2);
Global.setInt(C_GuildBattleStartIntervalTime, 3600);
Global.setInt(C_FamilyJoinGuildIntervalTime, 86400);

--//Vip
Global.setInt(C_Vip1Reward, 4511);  --ItemId
Global.setInt(C_Vip2Reward, 4511);
Global.setInt(C_Vip1RewardNum, 1);
Global.setInt(C_Vip2RewardNum, 2);
Global.setInt(C_Vip1ShopID, 2101);
Global.setInt(C_Vip2ShopID, 2102);
Global.setFloat(C_VipTime,2592000);

--//商店
Global.setInt(C_GrowFundShopID,5001);
Global.setInt(C_SmallChange1ShopID,5002);
Global.setInt(C_SmallChange3ShopID,5003);

--//商会
Global.setFloat(C_MallTax,0.1);
Global.setFloat(C_MallSellPay,500);
Global.setString(C_MallMailSender,"系统");
Global.setString(C_MallMailTitle,"商品出售成功");
Global.setString(C_MallMailContent,"您的商品出售成功，获得钻石，请查收");
Global.setInt(C_MallMinPrice, 10);


--//包满邮件
Global.setString(C_ShopMailSender,"系统");
Global.setString(C_ShopMailContent,"由于您背包已满,奖励发放到邮箱,给您带来的不便尽请谅解！！！");

--//许愿
Global.setInt(C_WishShareMaxNum,10);
Global.setInt(C_WishStoreMax,10);
Global.setInt(C_WishShareExp,10000);
Global.setInt(C_WishShareMoney,2000);
Global.setInt(C_WishItem,5508);

--//存储经验
Global.setInt(C_TongJiExp,50000);
Global.setInt(C_RiChangExp,80000);
Global.setInt(C_BaiRenExp,50000);
Global.setInt(C_FuBenExp,150000);
Global.setInt(C_PetExp,50000);
Global.setInt(C_WuDouExp,50000);
Global.setInt(C_ExamExp,20000);
Global.setInt(C_PromiseExp,150000);
Global.setInt(C_ConvertExpMax,10000000);

--//上限
Global.setInt(C_MoneyMax, 80000000);
Global.setInt(C_DiamondMax, 30000000);
Global.setInt(C_BabyMax, 3);

--//喊话倒计时
Global.setFloat(C_WorldChatTime,15);

--//1854 【需求】伙伴-伙伴第一次抽取时抽取指定伙伴
Global.setInt(C_FirstRollEmployeeCon, 2039);
Global.setInt(C_FirstRollEmployeeDia, 2001);

Global.setInt(C_GuideNewSceneId0,24);
Global.setInt(C_GuideNewSceneId1,24);

Global.setInt(C_TeamGuideStep1, 62);
Global.setInt(C_TeamGuideStep2, 63);

--组队功能中默认最低等级
Global.setInt(C_TeamNoTaget, 10);
Global.setInt(C_Team10, 10);
Global.setInt(C_Team20, 20);
Global.setInt(C_Team30, 30);
Global.setInt(C_Team40, 40);
Global.setInt(C_Team50, 50);
Global.setInt(C_Team60, 60);
Global.setInt(C_TeamTongji, 20);
Global.setInt(C_TeamMogu, 20);
Global.setInt(C_TeamXiji, 20);
Global.setInt(C_TeamBairen, 30);
Global.setInt(C_TeamPVP, 35);
Global.setInt(C_TeamOutSide, 10);
Global.setInt(C_Copy, 30);
Global.setInt(C_Hero, 20);

Global.setFloat(C_AiAttackTime, 10);
Global.setInt(C_ItemStroagePageGridNum, 20);
Global.setInt(C_BabyStroagePageGridNum, 6);
Global.setInt(C_ItemStroageGridMax, 100);
Global.setInt(C_BabyStroageGridMax, 48);

--封魔卡道具id和封魔卡在商店中的商店id
Global.setInt(C_CatchPetItemID, 5002);
Global.setInt(C_CatchPetItemInShopID, 3017);

--战斗音乐
Global.setInt(C_BossMuc, 10);
Global.setInt(C_PutnMuc, 9);
Global.setInt(C_PvpMuc, 11);

Global.setInt(C_PkItemDorp, 50);

Global.setInt(C_ZhuanPanOneGo, 200);
Global.setInt(C_ZhuanPanTenGo, 1800);

Global.setInt(C_AllGatherNum, 1000);
Global.setInt(C_GatherNumMax, 1000);
--//好友和黑名单上限
Global.setInt(C_FriendMax, 100);
--帮派战

Global.setInt(C_FamilyInitFundz, 100000);
Global.setInt(C_FamilyLearnSkillPay, 10000);
Global.setInt(C_FamilySkillExp, 100);
Global.setInt(C_FamilyPresentItemId, 21351); --//捐献ID
Global.setString(C_FamilyPresentMailSender, "家族采集场补给发放");
Global.setString(C_FamilyPresentMailTitle, "家族采集场补给发放");
Global.setString(C_FamilyPresentMailContent, "采集场已经收集够足够的材料箱了，现发放给大家");
Global.setInt(C_FamilyPresentMailItemId0, 21352);
Global.setInt(C_FamilyPresentMailItemId1, 21353);
Global.setInt(C_FamilyPresentMailItemId2, 21354);
Global.setInt(C_FamilyPresentMailItemId3, 21355);
Global.setInt(C_FamilyPresentMailItemId4, 21356);
Global.setInt(C_FamilyPresentMailItemId5, 21357);
Global.setInt(C_FamilyPresentMailItemId6, 21358);
Global.setInt(C_FamilyPresentMailItemId7, 21359);
Global.setInt(C_FamilyPresentMailItemId8, 21360);
Global.setInt(C_FamilyPresentMailItemId9, 21361);

Global.setInt(C_FamilyProgenitusMonsterId0, 1 );
Global.setInt(C_FamilyProgenitusMonsterId1, 2 );
Global.setInt(C_FamilyProgenitusMonsterId2, 3 );
Global.setInt(C_FamilyProgenitusMonsterId3, 4 );

Global.setInt(C_FamilyProgenitusAddExpPay, 1000);
Global.setInt(C_FamilyProgenitusAddExp, 10000);
Global.setInt(C_FamilyProgenitusAddExpSuperPay, 10000);
Global.setInt(C_FamilyProgenitusAddSuperExp, 100000);
Global.setInt(C_FamilyBattleOpenRand, 4); --//家族战随机
Global.setInt(C_FamilyBattleScene, 810); --//家族战场景
Global.setInt(C_FamilyLeaderOffineTimeMax, 2592000);
Global.setInt(C_FamilyLoseLeaderItem, 21363);
Global.setInt(C_NoBattleTime, 20);
Global.setInt(C_FamilySignDrop, 320044);

--战斗速度
Global.setFloat(C_BattleSpeed,1.2);
Global.setInt(C_IosPayServerId,999);
Global.setInt(C_AndroidPayServerId,1);

--翻牌
Global.setInt(C_OpenCardMax,9);
Global.setInt(C_OpenCardNeedItem,5087);
Global.setInt(C_ResetCardNeedItemNum,10);
--热点购买次数
Global.setInt(C_HotShopBuyNum,10);
--等级开启精彩活动
Global.setInt(C_ADActivityOpenLv, 5);
--对话缩放系数
Global.setInt(C_DialogScale, 80);

Global.setInt(C_MinGiftPrice, 1);
Global.setInt(C_MinGiftOldPrice, 30);

Global.setString(C_MinGiftBag,"[{\"ItemId\":5086,\"ItemStack\":10},{\"ItemId\":5044,\"ItemStack\":1},{\"ItemId\":5002,\"ItemStack\":10},{\"ItemId\":1756,\"ItemStack\":1}]")

--buff商城id
Global.setInt(C_HPBuffShopID, 1000);
Global.setInt(C_MPBuffShopID, 1001);
Global.setInt(C_PhoneItem, 5045);
Global.setInt(C_MallShopFreezeTime, 7);
Global.setString(C_PT_NoSleep, "降低被昏睡的概率");
Global.setString(C_PT_NoPetrifaction, "降低被石化的概率");
Global.setString(C_PT_NoDrunk, "降低被酒醉的概率");
Global.setString(C_PT_NoChaos, "降低被混乱的概率");
Global.setString(C_PT_NoPoison, "降低被毒的概率");
Global.setString(C_PT_Stama, "主要影响生命属性");
Global.setString(C_PT_Strength, "主要影响攻击属性");
Global.setString(C_PT_Power, "主要影响防御属性");
Global.setString(C_PT_Speed, "主要影响敏捷属性");
Global.setString(C_PT_Magic, "主要影响魔法属性和精神属性");
Global.setString(C_PT_Attack, "影响物理攻击和技能的伤害");
Global.setString(C_PT_Defense, "影响受到物理攻击和技能的伤害");
Global.setString(C_PT_Agile, "影响出手速度和闪避的几率");
Global.setString(C_PT_Spirit, "影响魔法技能的伤害和受到魔法技能的伤害");
Global.setString(C_PT_Reply, "影响被补血回复的血量");
Global.setString(C_PT_Hit, "影响物理攻击和技能命中目标的概率");
Global.setString(C_PT_Dodge, "影响躲避物理攻击和技能的概率");
Global.setString(C_PT_Crit, "影响物理攻击和技能出现暴击的概率");
Global.setString(C_PT_counterpunch, "影响被近战物理攻击和技能攻击后反击的概率");
Global.setInt(C_FuWenOpenLevel, 30);
Global.setInt(C_GiftBtn, 1);
Global.setInt(C_AucGoodProtect, 7);

Global.setInt(C_EverydayIntegral,10); 	--每日领取积分
Global.setInt(C_IntegralRatio,1);		--充值返还积分比例