/* arpcc auto generated cpp file. */
#include "FieldMask.h"
#include "com.h"
//=============================================================
static void initFuncMajorVersion(EnumInfo* e)
{
		e->items_.push_back("Major_0");
		e->items_.push_back("MajorNumber");
}
EnumInfo enumMajorVersion("MajorVersion", initFuncMajorVersion);
//=============================================================
static void initFuncMinorVersion(EnumInfo* e)
{
		e->items_.push_back("Minor_0");
		e->items_.push_back("Minor_1");
		e->items_.push_back("Minor_2");
		e->items_.push_back("Minor_3");
		e->items_.push_back("Minor_4");
		e->items_.push_back("Minor_5");
		e->items_.push_back("Minor_6");
		e->items_.push_back("MinorNumber");
}
EnumInfo enumMinorVersion("MinorVersion", initFuncMinorVersion);
//=============================================================
static void initFuncPatchVersion(EnumInfo* e)
{
		e->items_.push_back("Patch_0");
		e->items_.push_back("Patch_1");
		e->items_.push_back("Patch_2");
		e->items_.push_back("Patch_3");
		e->items_.push_back("Patch_4");
		e->items_.push_back("Patch_5");
		e->items_.push_back("PatchNumber");
}
EnumInfo enumPatchVersion("PatchVersion", initFuncPatchVersion);
//=============================================================
static void initFuncPetQuality(EnumInfo* e)
{
		e->items_.push_back("PE_None");
		e->items_.push_back("PE_White");
		e->items_.push_back("PE_Green");
		e->items_.push_back("PE_Blue");
		e->items_.push_back("PE_Purple");
		e->items_.push_back("PE_Golden");
		e->items_.push_back("PE_Orange");
		e->items_.push_back("PE_Pink");
}
EnumInfo enumPetQuality("PetQuality", initFuncPetQuality);
//=============================================================
static void initFuncErrorNo(EnumInfo* e)
{
		e->items_.push_back("EN_None");
		e->items_.push_back("EN_VersionNotMatch");
		e->items_.push_back("EN_AccountNameSame");
		e->items_.push_back("EN_PlayerNameSame");
		e->items_.push_back("EN_FilterWord");
		e->items_.push_back("EN_CannotfindPlayer");
		e->items_.push_back("EN_AcceptQuestNotFound");
		e->items_.push_back("EN_AcceptQuestNoItem");
		e->items_.push_back("EN_AcceptSecendDaily");
		e->items_.push_back("EN_DailyNoNum");
		e->items_.push_back("EN_AcceptSecendProfession");
		e->items_.push_back("EN_Battle");
		e->items_.push_back("EN_MoneyLess");
		e->items_.push_back("EN_DiamondLess");
		e->items_.push_back("EN_NoSubSyste");
		e->items_.push_back("EN_InTeam");
		e->items_.push_back("EN_NoTeamLeader");
		e->items_.push_back("EN_TeamPassword");
		e->items_.push_back("EN_TeamIsFull");
		e->items_.push_back("EN_NoTeam");
		e->items_.push_back("EN_TeamIsRunning");
		e->items_.push_back("EN_TeamMemberLeaving");
		e->items_.push_back("EN_NoBackTeam");
		e->items_.push_back("EN_InTeamBlackList");
		e->items_.push_back("EN_EmployeeIsFull");
		e->items_.push_back("EN_NoUpSkill");
		e->items_.push_back("EN_PropisNull");
		e->items_.push_back("EN_DoubleExpTimeFull");
		e->items_.push_back("EN_DoubleExpTimeNULL");
		e->items_.push_back("EN_NoTeamNoTongji");
		e->items_.push_back("EN_TongjiTimesMax");
		e->items_.push_back("EN_TongjiTeamMemberTimesMax");
		e->items_.push_back("EN_NoTeamLeaderNoTongji");
		e->items_.push_back("EN_TeamSizeTongjiError");
		e->items_.push_back("EN_GetPoisonMushroom");
		e->items_.push_back("EN_GetMushroom");
		e->items_.push_back("EN_TongjiTeamLevelTooLow");
		e->items_.push_back("EN_PlayerIsInTeam");
		e->items_.push_back("EN_AcceptQuestBagMax");
		e->items_.push_back("EN_SubmitQuestBagMax");
		e->items_.push_back("EN_GuildNameSame");
		e->items_.push_back("EN_PlayerGoldLess");
		e->items_.push_back("EN_PlayerHasGuild");
		e->items_.push_back("EN_InRequestErr");
		e->items_.push_back("EN_RequestListFull");
		e->items_.push_back("EN_joinGuildRequestOk");
		e->items_.push_back("EN_JoinOtherGuild");
		e->items_.push_back("EN_PremierQuitError");
		e->items_.push_back("EN_CommandPositionLess");
		e->items_.push_back("EN_PositionUpMax");
		e->items_.push_back("EN_MallBuyOk");
		e->items_.push_back("EN_MallBuyFailBagFull");
		e->items_.push_back("EN_MallBuyFailBabyFull");
		e->items_.push_back("EN_MallBuyFailDiamondLess");
		e->items_.push_back("EN_MallBuyFailSelled");
		e->items_.push_back("EN_OpenBaoXiangBagFull");
		e->items_.push_back("EN_NoBaby");
		e->items_.push_back("EN_BagFull");
		e->items_.push_back("EN_BagSizeMax");
		e->items_.push_back("EN_BabyStorageFull");
		e->items_.push_back("EN_BabyFullToStorage");
		e->items_.push_back("EN_NewItemError");
		e->items_.push_back("EN_BabyFull");
		e->items_.push_back("EN_RemouldBabyLevel");
		e->items_.push_back("EN_SkillSoltFull");
		e->items_.push_back("EN_WorldChatPayError");
		e->items_.push_back("EN_DontTalk");
		e->items_.push_back("EN_BadMushroom");
		e->items_.push_back("EN_ItemMushroom");
		e->items_.push_back("EN_GetMailItemBagFull");
		e->items_.push_back("EN_Materialless");
		e->items_.push_back("EN_OpenGatherlose");
		e->items_.push_back("EN_OpenGatherRepetition");
		e->items_.push_back("EN_GatherLevelLess");
		e->items_.push_back("EN_GatherTimesLess");
		e->items_.push_back("EN_OpenBaoXiangLevel");
		e->items_.push_back("EN_NoBattleBaby");
		e->items_.push_back("EN_NoThisPoint");
		e->items_.push_back("EN_BabyLevelHigh");
		e->items_.push_back("EN_AddMoney1W");
		e->items_.push_back("EN_AddDionmand100");
		e->items_.push_back("EN_AddMoney2W");
		e->items_.push_back("EN_AddDionmand200");
		e->items_.push_back("EN_AddMoney3W");
		e->items_.push_back("EN_AddDionmand300");
		e->items_.push_back("EN_AddMoney4W");
		e->items_.push_back("EN_AddDionmand400");
		e->items_.push_back("EN_AddMoney5W");
		e->items_.push_back("EN_AddDionmand500");
		e->items_.push_back("EN_AddMoney6W");
		e->items_.push_back("EN_AddDionmand600");
		e->items_.push_back("EN_DelBaby1000");
		e->items_.push_back("EN_DelBaby30");
		e->items_.push_back("EN_DelBaby54");
		e->items_.push_back("EN_DelBaby10030");
		e->items_.push_back("EN_DelBaby10015");
		e->items_.push_back("EN_DelBaby10157");
		e->items_.push_back("EN_DelDefaultSkill");
		e->items_.push_back("EN_GreenBoxFreeNum");
		e->items_.push_back("EM_NotNormalVip");
		e->items_.push_back("EN_NotSuperVip");
		e->items_.push_back("EN_FirendNotOpen");
		e->items_.push_back("EN_BlackCannotFriend");
		e->items_.push_back("EN_HasFriend");
		e->items_.push_back("EN_HunderdNoNum");
		e->items_.push_back("EN_HunderdTier");
		e->items_.push_back("EN_HunderdLevel");
		e->items_.push_back("EN_SkillExperr");
		e->items_.push_back("EN_TimeMushroom");
		e->items_.push_back("EN_TimeTongji");
		e->items_.push_back("EN_TimeXiji");
		e->items_.push_back("EN_Storefull");
		e->items_.push_back("EN_HasBattleTime");
		e->items_.push_back("EN_HourLess24");
		e->items_.push_back("EN_HourLess24_Join");
		e->items_.push_back("EN_TeamMemberHourLess24");
		e->items_.push_back("EN_GuildBattleJoinSceneMoveValue");
		e->items_.push_back("EN_IdgenNull");
		e->items_.push_back("EN_Idgenhas");
		e->items_.push_back("EN_Gifthas");
		e->items_.push_back("EN_UseMakeRepeat");
		e->items_.push_back("EN_WishNull");
		e->items_.push_back("EN_NoGuild");
		e->items_.push_back("EN_GuildMemberMax");
		e->items_.push_back("EN_LevelupGuildBuilding");
		e->items_.push_back("EN_LevelupGuildBuildingLevelMax");
		e->items_.push_back("EN_LevelupGuildBuildingMoneyLess");
		e->items_.push_back("EN_LevelupGuildBuildingHallBuildLevelLess");
		e->items_.push_back("EN_AddGuildMoneyMax");
		e->items_.push_back("EN_PresentGuildOk");
		e->items_.push_back("EN_RefreshShopTimeLess");
		e->items_.push_back("EN_TeamMemberNoGuild");
		e->items_.push_back("EN_MagicCurrencyLess");
		e->items_.push_back("EN_DisShopLimitLess");
		e->items_.push_back("EN_FamilyPremierCanntDelete");
		e->items_.push_back("EN_MyFamilyMonster");
		e->items_.push_back("EN_NoBattleTime");
		e->items_.push_back("EN_OtherNoBattleTime");
		e->items_.push_back("EN_GuildBattleTimeout2");
		e->items_.push_back("EN_GuildBattleHasTeam");
		e->items_.push_back("EN_AccountIsSeal");
		e->items_.push_back("EN_PlayerNoOnline");
		e->items_.push_back("EN_ActivityNoTime");
		e->items_.push_back("EN_NoTeamExist");
		e->items_.push_back("EN_GuildNoMember");
		e->items_.push_back("EN_MallSellTimeLess");
		e->items_.push_back("EN_GuildMemberLess24");
		e->items_.push_back("EN_InviteeLeaveGuildLess24");
		e->items_.push_back("EN_PboneNumberSuccess");
		e->items_.push_back("EN_PhoneNumberError");
		e->items_.push_back("EN_OtherPlayerInBattle");
		e->items_.push_back("EN_GuildBattleNotMatch");
		e->items_.push_back("EN_GuildBattleIsStart");
		e->items_.push_back("EN_GuileBattleIsClose");
		e->items_.push_back("EN_GuildBattleTeamNoSameGuild");
		e->items_.push_back("EN_BackTeamCommandLeaderInGuildHomeSceneAndYouAreNotSameGuild");
		e->items_.push_back("EN_AccecptRandQuestSizeLimitError");
		e->items_.push_back("EN_Max");
}
EnumInfo enumErrorNo("ErrorNo", initFuncErrorNo);
//=============================================================
static void initFuncOperateType(EnumInfo* e)
{
		e->items_.push_back("OT_0");
		e->items_.push_back("OT_P1");
		e->items_.push_back("OT_P2");
		e->items_.push_back("OT_B");
}
EnumInfo enumOperateType("OperateType", initFuncOperateType);
//=============================================================
static void initFuncBindType(EnumInfo* e)
{
		e->items_.push_back("BIT_None");
		e->items_.push_back("BIT_Bag");
		e->items_.push_back("BIT_Use");
		e->items_.push_back("BIT_Max");
}
EnumInfo enumBindType("BindType", initFuncBindType);
//=============================================================
static void initFuncReconnectType(EnumInfo* e)
{
		e->items_.push_back("RECT_None");
		e->items_.push_back("RECT_LoginOk");
		e->items_.push_back("RECT_EnterGameOk");
		e->items_.push_back("RECT_JoinTeamOk");
		e->items_.push_back("RECT_EnterSceneOk");
		e->items_.push_back("RECT_EnterBattleOk");
		e->items_.push_back("RECT_Max");
}
EnumInfo enumReconnectType("ReconnectType", initFuncReconnectType);
//=============================================================
static void initFuncSexType(EnumInfo* e)
{
		e->items_.push_back("ST_Unknown");
		e->items_.push_back("ST_Male");
		e->items_.push_back("ST_Female");
}
EnumInfo enumSexType("SexType", initFuncSexType);
//=============================================================
static void initFuncBattleType(EnumInfo* e)
{
		e->items_.push_back("BT_None");
		e->items_.push_back("BT_PVE");
		e->items_.push_back("BT_PVR");
		e->items_.push_back("BT_PVP");
		e->items_.push_back("BT_PVH");
		e->items_.push_back("BT_PET");
		e->items_.push_back("BT_PK1");
		e->items_.push_back("BT_PK2");
		e->items_.push_back("BT_Guild");
		e->items_.push_back("BT_MAX");
}
EnumInfo enumBattleType("BattleType", initFuncBattleType);
//=============================================================
static void initFuncEntityType(EnumInfo* e)
{
		e->items_.push_back("ET_None");
		e->items_.push_back("ET_Player");
		e->items_.push_back("ET_Baby");
		e->items_.push_back("ET_Monster");
		e->items_.push_back("ET_Boss");
		e->items_.push_back("ET_Emplyee");
		e->items_.push_back("ET_Max");
}
EnumInfo enumEntityType("EntityType", initFuncEntityType);
//=============================================================
static void initFuncMineType(EnumInfo* e)
{
		e->items_.push_back("MT_None");
		e->items_.push_back("MT_JinShu");
		e->items_.push_back("MT_MuCai");
		e->items_.push_back("MT_BuLiao");
		e->items_.push_back("MT_Max");
}
EnumInfo enumMineType("MineType", initFuncMineType);
//=============================================================
static void initFuncGroupType(EnumInfo* e)
{
		e->items_.push_back("GT_None");
		e->items_.push_back("GT_Up");
		e->items_.push_back("GT_Down");
}
EnumInfo enumGroupType("GroupType", initFuncGroupType);
//=============================================================
static void initFuncSkillTargetType(EnumInfo* e)
{
		e->items_.push_back("STT_None");
		e->items_.push_back("STT_Self");
		e->items_.push_back("STT_Team");
		e->items_.push_back("STT_TeamDead");
		e->items_.push_back("STT_TeamNoSelf");
		e->items_.push_back("STT_All");
		e->items_.push_back("STT_AllNoSelf");
		e->items_.push_back("STT_Max");
}
EnumInfo enumSkillTargetType("SkillTargetType", initFuncSkillTargetType);
//=============================================================
static void initFuncSkillType(EnumInfo* e)
{
		e->items_.push_back("SKT_None");
		e->items_.push_back("SKT_DefaultSecActive");
		e->items_.push_back("SKT_DefaultSecPassive");
		e->items_.push_back("SKT_DefaultActive");
		e->items_.push_back("SKT_DefaultPassive");
		e->items_.push_back("SKT_Active");
		e->items_.push_back("SKT_Passive");
		e->items_.push_back("SKT_Gather");
		e->items_.push_back("SKT_Make");
		e->items_.push_back("SKT_CannotUse");
		e->items_.push_back("SKT_GuildPlayerSkill");
		e->items_.push_back("SKT_GuildBabySkill");
		e->items_.push_back("SKT_Max");
}
EnumInfo enumSkillType("SkillType", initFuncSkillType);
//=============================================================
static void initFuncPassiveType(EnumInfo* e)
{
		e->items_.push_back("PAT_None");
		e->items_.push_back("PAT_Buff");
		e->items_.push_back("PAT_Deference");
		e->items_.push_back("PAT_Dodge");
		e->items_.push_back("PAT_Counter");
		e->items_.push_back("PAT_Change");
		e->items_.push_back("PAT_Guard");
		e->items_.push_back("PAT_Runaway");
		e->items_.push_back("PAT_BabyInnout");
		e->items_.push_back("PAT_SecKill");
}
EnumInfo enumPassiveType("PassiveType", initFuncPassiveType);
//=============================================================
static void initFuncPlayerStatus(EnumInfo* e)
{
		e->items_.push_back("PS_Idle");
		e->items_.push_back("PS_Login");
		e->items_.push_back("PS_Game");
		e->items_.push_back("PS_Logout");
		e->items_.push_back("PS_Illegal");
}
EnumInfo enumPlayerStatus("PlayerStatus", initFuncPlayerStatus);
//=============================================================
static void initFuncOccupationType(EnumInfo* e)
{
		e->items_.push_back("OT_None");
		e->items_.push_back("OT_HeavyArmor");
		e->items_.push_back("OT_LightArmor");
		e->items_.push_back("OT_Spell");
		e->items_.push_back("OT_Max");
}
EnumInfo enumOccupationType("OccupationType", initFuncOccupationType);
//=============================================================
static void initFuncPeriodType(EnumInfo* e)
{
		e->items_.push_back("PT_Daily");
		e->items_.push_back("PT_Weekly");
		e->items_.push_back("PT_Customly");
}
EnumInfo enumPeriodType("PeriodType", initFuncPeriodType);
//=============================================================
static void initFuncJobType(EnumInfo* e)
{
		e->items_.push_back("JT_None");
		e->items_.push_back("JT_Newbie");
		e->items_.push_back("JT_Axe");
		e->items_.push_back("JT_Sword");
		e->items_.push_back("JT_Knight");
		e->items_.push_back("JT_Archer");
		e->items_.push_back("JT_Fighter");
		e->items_.push_back("JT_Ninja");
		e->items_.push_back("JT_Mage");
		e->items_.push_back("JT_Sage");
		e->items_.push_back("JT_Wizard");
		e->items_.push_back("JT_Word");
}
EnumInfo enumJobType("JobType", initFuncJobType);
//=============================================================
static void initFuncRaceType(EnumInfo* e)
{
		e->items_.push_back("RT_None");
		e->items_.push_back("RT_Human");
		e->items_.push_back("RT_Insect");
		e->items_.push_back("RT_Plant");
		e->items_.push_back("RT_Extra");
		e->items_.push_back("RT_Dragon");
		e->items_.push_back("RT_Animal");
		e->items_.push_back("RT_Fly");
		e->items_.push_back("RT_Undead");
		e->items_.push_back("RT_Metal");
		e->items_.push_back("RT_Max");
}
EnumInfo enumRaceType("RaceType", initFuncRaceType);
//=============================================================
static void initFuncBabyInitGear(EnumInfo* e)
{
		e->items_.push_back("BIG_None");
		e->items_.push_back("BIG_Stama");
		e->items_.push_back("BIG_Strength");
		e->items_.push_back("BIG_Power");
		e->items_.push_back("BIG_Speed");
		e->items_.push_back("BIG_Magic");
		e->items_.push_back("BIG_Max");
}
EnumInfo enumBabyInitGear("BabyInitGear", initFuncBabyInitGear);
//=============================================================
static void initFuncQualityColor(EnumInfo* e)
{
		e->items_.push_back("QC_None");
		e->items_.push_back("QC_White");
		e->items_.push_back("QC_Green");
		e->items_.push_back("QC_Blue");
		e->items_.push_back("QC_Blue1");
		e->items_.push_back("QC_Purple");
		e->items_.push_back("QC_Purple1");
		e->items_.push_back("QC_Purple2");
		e->items_.push_back("QC_Golden");
		e->items_.push_back("QC_Golden1");
		e->items_.push_back("QC_Golden2");
		e->items_.push_back("QC_Orange");
		e->items_.push_back("QC_Orange1");
		e->items_.push_back("QC_Orange2");
		e->items_.push_back("QC_Pink");
		e->items_.push_back("QC_Max");
}
EnumInfo enumQualityColor("QualityColor", initFuncQualityColor);
//=============================================================
static void initFuncPropertyType(EnumInfo* e)
{
		e->items_.push_back("PT_None");
		e->items_.push_back("PT_NoSleep");
		e->items_.push_back("PT_NoPetrifaction");
		e->items_.push_back("PT_NoDrunk");
		e->items_.push_back("PT_NoChaos");
		e->items_.push_back("PT_NoForget");
		e->items_.push_back("PT_NoPoison");
		e->items_.push_back("PT_Assassinate");
		e->items_.push_back("PT_Money");
		e->items_.push_back("PT_Diamond");
		e->items_.push_back("PT_MagicCurrency");
		e->items_.push_back("PT_EmployeeCurrency");
		e->items_.push_back("PT_Gather");
		e->items_.push_back("PT_Level");
		e->items_.push_back("PT_Exp");
		e->items_.push_back("PT_ConvertExp");
		e->items_.push_back("PT_OneDayReputation");
		e->items_.push_back("PT_Reputation");
		e->items_.push_back("PT_TableId");
		e->items_.push_back("PT_AssetId");
		e->items_.push_back("PT_Sex");
		e->items_.push_back("PT_BagNum");
		e->items_.push_back("PT_Race");
		e->items_.push_back("PT_Profession");
		e->items_.push_back("PT_ProfessionLevel");
		e->items_.push_back("PT_Stama");
		e->items_.push_back("PT_Strength");
		e->items_.push_back("PT_Power");
		e->items_.push_back("PT_Speed");
		e->items_.push_back("PT_Magic");
		e->items_.push_back("PT_Durability");
		e->items_.push_back("PT_HpCurr");
		e->items_.push_back("PT_MpCurr");
		e->items_.push_back("PT_HpMax");
		e->items_.push_back("PT_MpMax");
		e->items_.push_back("PT_Attack");
		e->items_.push_back("PT_Defense");
		e->items_.push_back("PT_Agile");
		e->items_.push_back("PT_Spirit");
		e->items_.push_back("PT_Reply");
		e->items_.push_back("PT_Magicattack");
		e->items_.push_back("PT_Magicdefense");
		e->items_.push_back("PT_Damage");
		e->items_.push_back("PT_SneakAttack");
		e->items_.push_back("PT_Hit");
		e->items_.push_back("PT_Dodge");
		e->items_.push_back("PT_Crit");
		e->items_.push_back("PT_counterpunch");
		e->items_.push_back("PT_Front");
		e->items_.push_back("PT_Wind");
		e->items_.push_back("PT_Land");
		e->items_.push_back("PT_Water");
		e->items_.push_back("PT_Fire");
		e->items_.push_back("PT_Free");
		e->items_.push_back("PT_Title");
		e->items_.push_back("PT_GuildID");
		e->items_.push_back("PT_Glamour");
		e->items_.push_back("PT_DoubleExp");
		e->items_.push_back("PT_TongjiComplateTimes");
		e->items_.push_back("PT_VipLevel");
		e->items_.push_back("PT_VipTime");
		e->items_.push_back("PT_FightingForce");
		e->items_.push_back("PT_Max");
}
EnumInfo enumPropertyType("PropertyType", initFuncPropertyType);
//=============================================================
static void initFuncVipLevel(EnumInfo* e)
{
		e->items_.push_back("VL_None");
		e->items_.push_back("VL_1");
		e->items_.push_back("VL_2");
		e->items_.push_back("VL_Max");
}
EnumInfo enumVipLevel("VipLevel", initFuncVipLevel);
//=============================================================
static void initFuncItemMainType(EnumInfo* e)
{
		e->items_.push_back("IMT_None");
		e->items_.push_back("IMT_Quest");
		e->items_.push_back("IMT_Consumables");
		e->items_.push_back("IMT_Equip");
		e->items_.push_back("IMT_Employee");
		e->items_.push_back("IMT_EmployeeEquip");
		e->items_.push_back("IMT_Debris");
		e->items_.push_back("IMT_FuWen");
		e->items_.push_back("IMT_BabyEquip");
		e->items_.push_back("IMT_Max");
}
EnumInfo enumItemMainType("ItemMainType", initFuncItemMainType);
//=============================================================
static void initFuncItemSubType(EnumInfo* e)
{
		e->items_.push_back("IST_None");
		e->items_.push_back("IST_Axe");
		e->items_.push_back("IST_Sword");
		e->items_.push_back("IST_Spear");
		e->items_.push_back("IST_Bow");
		e->items_.push_back("IST_Stick");
		e->items_.push_back("IST_Knife");
		e->items_.push_back("IST_Hat");
		e->items_.push_back("IST_Helmet");
		e->items_.push_back("IST_Clothes");
		e->items_.push_back("IST_Robe");
		e->items_.push_back("IST_Armor");
		e->items_.push_back("IST_Boot");
		e->items_.push_back("IST_Shoes");
		e->items_.push_back("IST_Shield");
		e->items_.push_back("IST_Crystal");
		e->items_.push_back("IST_Charm");
		e->items_.push_back("IST_Earrings");
		e->items_.push_back("IST_Bracelet");
		e->items_.push_back("IST_Ring");
		e->items_.push_back("IST_Necklace");
		e->items_.push_back("IST_Headband");
		e->items_.push_back("IST_Instruments");
		e->items_.push_back("IST_EquipMax");
		e->items_.push_back("IST_Ornament");
		e->items_.push_back("IST_Lottery");
		e->items_.push_back("IST_Coupun");
		e->items_.push_back("IST_OpenGird");
		e->items_.push_back("IST_ConsBegin");
		e->items_.push_back("IST_Consumables");
		e->items_.push_back("IST_Blood");
		e->items_.push_back("IST_Buff");
		e->items_.push_back("IST_Gem");
		e->items_.push_back("IST_Material");
		e->items_.push_back("IST_ItemDebris");
		e->items_.push_back("IST_BabyDebris");
		e->items_.push_back("IST_EmployeeDebris");
		e->items_.push_back("IST_BabyExp");
		e->items_.push_back("IST_SkillExp");
		e->items_.push_back("IST_ConsEnd");
		e->items_.push_back("IST_Gloves");
		e->items_.push_back("IST_EmployeeEquip");
		e->items_.push_back("IST_PVP");
		e->items_.push_back("IST_Fashion");
		e->items_.push_back("IST_FuWenAttack");
		e->items_.push_back("IST_FuWenDefense");
		e->items_.push_back("IST_FuWenAssist");
		e->items_.push_back("IST_Max");
}
EnumInfo enumItemSubType("ItemSubType", initFuncItemSubType);
//=============================================================
static void initFuncItemUseFlag(EnumInfo* e)
{
		e->items_.push_back("IUF_None");
		e->items_.push_back("IUF_Battle");
		e->items_.push_back("IUF_Scene");
		e->items_.push_back("IUF_All");
}
EnumInfo enumItemUseFlag("ItemUseFlag", initFuncItemUseFlag);
//=============================================================
static void initFuncEquipmentSlot(EnumInfo* e)
{
		e->items_.push_back("ES_None");
		e->items_.push_back("ES_Boot");
		e->items_.push_back("ES_SingleHand");
		e->items_.push_back("ES_Ornament_0");
		e->items_.push_back("ES_Head");
		e->items_.push_back("ES_Ornament_1");
		e->items_.push_back("ES_Body");
		e->items_.push_back("ES_DoubleHand");
		e->items_.push_back("ES_Crystal");
		e->items_.push_back("ES_Fashion");
		e->items_.push_back("ES_Max");
}
EnumInfo enumEquipmentSlot("EquipmentSlot", initFuncEquipmentSlot);
//=============================================================
static void initFuncWeaponType(EnumInfo* e)
{
		e->items_.push_back("WT_None");
		e->items_.push_back("WT_Axe");
		e->items_.push_back("WT_Sword");
		e->items_.push_back("WT_Spear");
		e->items_.push_back("WT_Bow");
		e->items_.push_back("WT_Stick");
		e->items_.push_back("WT_Knife");
		e->items_.push_back("WT_Hoe");
		e->items_.push_back("WT_Pickax");
		e->items_.push_back("WT_Lumberaxe");
		e->items_.push_back("WT_Gloves");
		e->items_.push_back("WT_Max");
}
EnumInfo enumWeaponType("WeaponType", initFuncWeaponType);
//=============================================================
static void initFuncChatKind(EnumInfo* e)
{
		e->items_.push_back("CK_None");
		e->items_.push_back("CK_World");
		e->items_.push_back("CK_Team");
		e->items_.push_back("CK_System");
		e->items_.push_back("CK_Friend");
		e->items_.push_back("CK_GM");
		e->items_.push_back("CK_Guild");
		e->items_.push_back("CK_Max");
}
EnumInfo enumChatKind("ChatKind", initFuncChatKind);
//=============================================================
static void initFuncBattlePosition(EnumInfo* e)
{
		e->items_.push_back("BP_None");
		e->items_.push_back("BP_Down0");
		e->items_.push_back("BP_Down1");
		e->items_.push_back("BP_Down2");
		e->items_.push_back("BP_Down3");
		e->items_.push_back("BP_Down4");
		e->items_.push_back("BP_Down5");
		e->items_.push_back("BP_Down6");
		e->items_.push_back("BP_Down7");
		e->items_.push_back("BP_Down8");
		e->items_.push_back("BP_Down9");
		e->items_.push_back("BP_Up0");
		e->items_.push_back("BP_Up1");
		e->items_.push_back("BP_Up2");
		e->items_.push_back("BP_Up3");
		e->items_.push_back("BP_Up4");
		e->items_.push_back("BP_Up5");
		e->items_.push_back("BP_Up6");
		e->items_.push_back("BP_Up7");
		e->items_.push_back("BP_Up8");
		e->items_.push_back("BP_Up9");
		e->items_.push_back("BP_Max");
}
EnumInfo enumBattlePosition("BattlePosition", initFuncBattlePosition);
//=============================================================
static void initFuncBattleJudgeType(EnumInfo* e)
{
		e->items_.push_back("BJT_None");
		e->items_.push_back("BJT_Continue");
		e->items_.push_back("BJT_Win");
		e->items_.push_back("BJT_Lose");
}
EnumInfo enumBattleJudgeType("BattleJudgeType", initFuncBattleJudgeType);
//=============================================================
static void initFuncOrderParamType(EnumInfo* e)
{
		e->items_.push_back("OPT_None");
		e->items_.push_back("OPT_BabyId");
		e->items_.push_back("OPT_Unite");
		e->items_.push_back("OPT_Huwei");
		e->items_.push_back("OPT_IsNo");
		e->items_.push_back("OPT_Max");
}
EnumInfo enumOrderParamType("OrderParamType", initFuncOrderParamType);
//=============================================================
static void initFuncOrderStatus(EnumInfo* e)
{
		e->items_.push_back("OS_None");
		e->items_.push_back("OS_ActiveOk");
		e->items_.push_back("OS_RunawayOk");
		e->items_.push_back("OS_FangBaobao");
		e->items_.push_back("OS_ShouBaobao");
		e->items_.push_back("OS_Weapon");
		e->items_.push_back("OS_Zhuachong");
		e->items_.push_back("OS_Flee");
		e->items_.push_back("OS_Summon");
}
EnumInfo enumOrderStatus("OrderStatus", initFuncOrderStatus);
//=============================================================
static void initFuncAIEvent(EnumInfo* e)
{
		e->items_.push_back("ME_None");
		e->items_.push_back("ME_Born");
		e->items_.push_back("ME_Deadth");
		e->items_.push_back("ME_AttackGo");
		e->items_.push_back("ME_SkillGO");
		e->items_.push_back("ME_Update");
		e->items_.push_back("ME_Max");
}
EnumInfo enumAIEvent("AIEvent", initFuncAIEvent);
//=============================================================
static void initFuncSyncIPropType(EnumInfo* e)
{
		e->items_.push_back("SPT_None");
		e->items_.push_back("SPT_Player");
		e->items_.push_back("SPT_Baby");
		e->items_.push_back("SPT_Employee");
		e->items_.push_back("SPT_Max");
}
EnumInfo enumSyncIPropType("SyncIPropType", initFuncSyncIPropType);
//=============================================================
static void initFuncBoxType(EnumInfo* e)
{
		e->items_.push_back("BX_None");
		e->items_.push_back("BX_Normal");
		e->items_.push_back("BX_Blue");
		e->items_.push_back("BX_Glod");
}
EnumInfo enumBoxType("BoxType", initFuncBoxType);
//=============================================================
static void initFuncQuestKind(EnumInfo* e)
{
		e->items_.push_back("QK_None");
		e->items_.push_back("QK_Main");
		e->items_.push_back("QK_Daily");
		e->items_.push_back("QK_Profession");
		e->items_.push_back("QK_Sub");
		e->items_.push_back("QK_Tongji");
		e->items_.push_back("QK_Copy");
		e->items_.push_back("QK_Wishing");
		e->items_.push_back("QK_Guild");
		e->items_.push_back("QK_Rand");
		e->items_.push_back("QK_Sub1");
		e->items_.push_back("QK_Max");
}
EnumInfo enumQuestKind("QuestKind", initFuncQuestKind);
//=============================================================
static void initFuncQuestType(EnumInfo* e)
{
		e->items_.push_back("QT_None");
		e->items_.push_back("QT_Dialog");
		e->items_.push_back("QT_Battle");
		e->items_.push_back("QT_Kill");
		e->items_.push_back("QT_KillAI");
		e->items_.push_back("QT_Item");
		e->items_.push_back("QT_Profession");
		e->items_.push_back("QT_Other");
		e->items_.push_back("QT_GiveItem");
		e->items_.push_back("QT_GiveBaby");
		e->items_.push_back("QT_Max");
}
EnumInfo enumQuestType("QuestType", initFuncQuestType);
//=============================================================
static void initFuncRequireType(EnumInfo* e)
{
		e->items_.push_back("RT_Nil");
}
EnumInfo enumRequireType("RequireType", initFuncRequireType);
//=============================================================
static void initFuncTeamType(EnumInfo* e)
{
		e->items_.push_back("TT_None");
		e->items_.push_back("TT_MainQuest");
		e->items_.push_back("TT_TongjiQuest");
		e->items_.push_back("TT_Daochang");
		e->items_.push_back("TT_CaoMoGu");
		e->items_.push_back("TT_Zhanchang");
		e->items_.push_back("TT_Hero");
		e->items_.push_back("TT_Pet");
		e->items_.push_back("TT_JJC");
		e->items_.push_back("TT_ShuaGuai");
		e->items_.push_back("TT_Copy");
		e->items_.push_back("TT_Max");
}
EnumInfo enumTeamType("TeamType", initFuncTeamType);
//=============================================================
static void initFuncStateType(EnumInfo* e)
{
		e->items_.push_back("ST_None");
		e->items_.push_back("ST_Normal");
		e->items_.push_back("ST_Defense");
		e->items_.push_back("ST_Dodge");
		e->items_.push_back("ST_ActionAbsorb");
		e->items_.push_back("ST_MagicAbsorb");
		e->items_.push_back("ST_Shield");
		e->items_.push_back("ST_ActionBounce");
		e->items_.push_back("ST_MagicBounce");
		e->items_.push_back("ST_ActionInvalid");
		e->items_.push_back("ST_MagicInvalid");
		e->items_.push_back("ST_Defend");
		e->items_.push_back("ST_NoDodge");
		e->items_.push_back("ST_Poison");
		e->items_.push_back("ST_Basilisk");
		e->items_.push_back("ST_Sleep");
		e->items_.push_back("ST_Chaos");
		e->items_.push_back("ST_Drunk");
		e->items_.push_back("ST_Forget");
		e->items_.push_back("ST_BeatBack");
		e->items_.push_back("ST_Recover");
		e->items_.push_back("ST_StrongRecover");
		e->items_.push_back("ST_GroupRecover");
		e->items_.push_back("ST_MagicDef");
		e->items_.push_back("ST_Max");
}
EnumInfo enumStateType("StateType", initFuncStateType);
//=============================================================
static void initFuncSceneType(EnumInfo* e)
{
		e->items_.push_back("SCT_None");
		e->items_.push_back("SCT_New");
		e->items_.push_back("SCT_Home");
		e->items_.push_back("SCT_Scene");
		e->items_.push_back("SCT_City");
		e->items_.push_back("SCT_Bairen");
		e->items_.push_back("SCT_Instance");
		e->items_.push_back("SCT_AlonePK");
		e->items_.push_back("SCT_TeamPK");
		e->items_.push_back("SCT_GuildHome");
		e->items_.push_back("SCT_GuildBattleScene");
		e->items_.push_back("SCT_Max");
}
EnumInfo enumSceneType("SceneType", initFuncSceneType);
//=============================================================
static void initFuncBornType(EnumInfo* e)
{
		e->items_.push_back("BOT_None");
		e->items_.push_back("BOT_BornPos");
		e->items_.push_back("BOT_Cached");
		e->items_.push_back("BOT_FromSceneEntry");
		e->items_.push_back("BOT_FromMazeEntry");
		e->items_.push_back("BOT_NormalMazeEntry");
		e->items_.push_back("BOT_Max");
}
EnumInfo enumBornType("BornType", initFuncBornType);
//=============================================================
static void initFuncWeaponActionType(EnumInfo* e)
{
		e->items_.push_back("WAT_None");
		e->items_.push_back("WAT_Chop");
		e->items_.push_back("WAT_Stab");
		e->items_.push_back("WAT_Bow");
		e->items_.push_back("WAT_Throw");
		e->items_.push_back("WAT_Max");
}
EnumInfo enumWeaponActionType("WeaponActionType", initFuncWeaponActionType);
//=============================================================
static void initFuncSceneOutpuType(EnumInfo* e)
{
		e->items_.push_back("SOT_None");
		e->items_.push_back("SOT_PVE");
		e->items_.push_back("SOT_PVP");
		e->items_.push_back("SOT_Max");
}
EnumInfo enumSceneOutpuType("SceneOutpuType", initFuncSceneOutpuType);
//=============================================================
static void initFuncTogetherStateType(EnumInfo* e)
{
		e->items_.push_back("TST_None");
		e->items_.push_back("TST_Self");
		e->items_.push_back("TST_Enemy");
		e->items_.push_back("TST_Max");
}
EnumInfo enumTogetherStateType("TogetherStateType", initFuncTogetherStateType);
//=============================================================
static void initFuncGuideAimType(EnumInfo* e)
{
		e->items_.push_back("GAT_None");
		e->items_.push_back("GAT_FirstAchievement");
		e->items_.push_back("GAT_FirstSkill");
		e->items_.push_back("GAT_FirstLevelSkill");
		e->items_.push_back("GAT_FirstQuest");
		e->items_.push_back("GAT_DialogUI");
		e->items_.push_back("GAT_MainTeamBtn");
		e->items_.push_back("GAT_MainTaskBtn");
		e->items_.push_back("GAT_QuestMiniFirst");
		e->items_.push_back("GAT_QuestMiniSecond");
		e->items_.push_back("GAT_QuestMiniThird");
		e->items_.push_back("GAT_MainCrystal");
		e->items_.push_back("GAT_MainCastle");
		e->items_.push_back("GAT_MainJJC");
		e->items_.push_back("GAT_OfflineJJC");
		e->items_.push_back("GAT_OfflineJJC4");
		e->items_.push_back("GAT_WorldMapER");
		e->items_.push_back("GAT_WorldMapFL");
		e->items_.push_back("GAT_WorldMapWorldBtn");
		e->items_.push_back("GAT_MiniMap");
		e->items_.push_back("GAT_TeamCreateBtn");
		e->items_.push_back("GAT_TeamWorldMapBtn");
		e->items_.push_back("GAT_FirstPartner_PosUI");
		e->items_.push_back("GAT_FreeLootPartner");
		e->items_.push_back("GAT_FriendAddBtn");
		e->items_.push_back("GAT_PartnerShowCancel");
		e->items_.push_back("GAT_PartnerPositionTab");
		e->items_.push_back("GAT_PartnerDetailBaseTab");
		e->items_.push_back("GAT_PartnerDetailBodySlot");
		e->items_.push_back("GAT_PartnerDetailEquipBtn");
		e->items_.push_back("GAT_PartnerDetailBaseFirstSkill");
		e->items_.push_back("GAT_PartnerDetailBaseSkillLvUpBtn");
		e->items_.push_back("GAT_LearnSkillAttackSkillTab");
		e->items_.push_back("GAT_LearnSkillAttackMagicTab");
		e->items_.push_back("GAT_LearnSkillStatusMagicTab");
		e->items_.push_back("GAT_LearnSkillAidSkillTab");
		e->items_.push_back("GAT_LearnSkillBtn");
		e->items_.push_back("GAT_FirstLearningBabySkill");
		e->items_.push_back("GAT_FirstLearningBabySkill_BabyList");
		e->items_.push_back("GAT_ThirdLearningBabySkill_SkillSlot");
		e->items_.push_back("GAT_BabySkillLearningBtn");
		e->items_.push_back("GAT_MessageBoxOkBtn");
		e->items_.push_back("GAT_MainReturn");
		e->items_.push_back("GAT_MainBag");
		e->items_.push_back("GAT_MainBagTipUseItem");
		e->items_.push_back("GAT_MainbagTipEquip");
		e->items_.push_back("GAT_MainBagFuwenTab");
		e->items_.push_back("GAT_MainBagFuwenFirstItem");
		e->items_.push_back("GAT_MainBagFuwenTipsCombieBtn");
		e->items_.push_back("GAT_MainBagFuwenTipsInsertBtn");
		e->items_.push_back("GAT_MainFuwenUICombieBtn");
		e->items_.push_back("GAT_MainFuwenCloseBtn");
		e->items_.push_back("GAT_MainSkill");
		e->items_.push_back("GAT_MainMake");
		e->items_.push_back("GAT_MainMakeSword");
		e->items_.push_back("GAT_MainMakeAxe");
		e->items_.push_back("GAT_MainMakeStick");
		e->items_.push_back("GAT_MainMakeBow");
		e->items_.push_back("GAT_MainMakeCompoundBtn");
		e->items_.push_back("GAT_MainMakeLevel10");
		e->items_.push_back("GAT_MainMakeSubFirst");
		e->items_.push_back("GAT_MainMakeSubSecond");
		e->items_.push_back("GAT_MainMakeSubThird");
		e->items_.push_back("GAT_MainMakeGemBtn");
		e->items_.push_back("GAT_MainMakeGemClose");
		e->items_.push_back("GAT_MainMakeGemFirst");
		e->items_.push_back("GAT_MainBaby");
		e->items_.push_back("GAT_MainBabyStatusBtn");
		e->items_.push_back("GAT_MainBabyPropertyBtn");
		e->items_.push_back("GAT_MainBabyPropertyContainer");
		e->items_.push_back("GAT_MainBabyPropertyConfirm");
		e->items_.push_back("GAT_MainBabyClose");
		e->items_.push_back("GAT_MainMagic");
		e->items_.push_back("GAT_MainMagicLevelFirst");
		e->items_.push_back("GAT_MainMagicLevelBtn");
		e->items_.push_back("GAT_MainFriend");
		e->items_.push_back("GAT_MainPartner");
		e->items_.push_back("GAT_MainSetting");
		e->items_.push_back("GAT_MainRecharge");
		e->items_.push_back("GAT_MainActivity");
		e->items_.push_back("GAT_MainPlayerInfo");
		e->items_.push_back("GAT_MainPlayerInfoStatusBtn");
		e->items_.push_back("GAT_MainPlayerInfoPropertyBtn");
		e->items_.push_back("GAT_MainPlayerInfoPropertyContainer");
		e->items_.push_back("GAT_MainPlayerInfoPropertyConfirm");
		e->items_.push_back("GAT_MainPlayerInfoClose");
		e->items_.push_back("GAT_MainJiubaHouse");
		e->items_.push_back("GAT_MainStick");
		e->items_.push_back("GAT_MainAchievement");
		e->items_.push_back("GAT_MainRaise");
		e->items_.push_back("GAT_MainFamily");
		e->items_.push_back("GAT_BattleAttack");
		e->items_.push_back("GAT_BattleSkill");
		e->items_.push_back("GAT_BattleBabySkill");
		e->items_.push_back("GAT_BattleDeference");
		e->items_.push_back("GAT_BattleChangePosition");
		e->items_.push_back("GAT_BattleAuto");
		e->items_.push_back("GAT_BattleBag");
		e->items_.push_back("GAT_BattleCatch");
		e->items_.push_back("GAT_BattleBaby");
		e->items_.push_back("GAT_BattleRunaway");
		e->items_.push_back("GAT_BattlePlayerInfo");
		e->items_.push_back("GAT_BattleRewardClose");
		e->items_.push_back("GAT_FirstAutoSkill");
		e->items_.push_back("GAT_PlayerAuto");
		e->items_.push_back("GAT_Max");
}
EnumInfo enumGuideAimType("GuideAimType", initFuncGuideAimType);
//=============================================================
static void initFuncScriptGameEvent(EnumInfo* e)
{
		e->items_.push_back("SGE_None");
		e->items_.push_back("SGE_MainPanelOpen");
		e->items_.push_back("SGE_FirstEnterMainScene");
		e->items_.push_back("SGE_EnterScene");
		e->items_.push_back("SGE_EnterMainScene");
		e->items_.push_back("SGE_Talk_FirstEnterMainScene");
		e->items_.push_back("SGE_Talk_BattleReady");
		e->items_.push_back("SGE_Talk_ActorReady");
		e->items_.push_back("SGE_Talk_BattleOver");
		e->items_.push_back("SGE_WorldMapOpen");
		e->items_.push_back("SGE_WorldMapToWorld");
		e->items_.push_back("SGE_MiniMapOpen");
		e->items_.push_back("SGE_TeamUIOpen");
		e->items_.push_back("SGE_AchievementUIOpen");
		e->items_.push_back("SGE_AchievementReceived");
		e->items_.push_back("SGE_TeamUISelectMapOpen");
		e->items_.push_back("SGE_PartnerForBattle");
		e->items_.push_back("SGE_PartnerPositionTabShow");
		e->items_.push_back("SGE_PartnerListTabShow");
		e->items_.push_back("SGE_PartnerDetailUIOpen");
		e->items_.push_back("SGE_PartnerDetailBaseOpen");
		e->items_.push_back("SGE_PartnerDetailEquipClick");
		e->items_.push_back("SGE_PartnerDetailEquipSucc");
		e->items_.push_back("SGE_PartnerDetailBaseSkillOpen");
		e->items_.push_back("SGE_ParnterDetailBaseSkillLvUpSucc");
		e->items_.push_back("SGE_MainMakeSub");
		e->items_.push_back("SGE_MainMakeSubDetail");
		e->items_.push_back("SGE_MainMakeItemOk");
		e->items_.push_back("SGE_MainMakeGemUI");
		e->items_.push_back("SGE_MainMakeGemOk");
		e->items_.push_back("SGE_MainMakeGemUIClose");
		e->items_.push_back("SGE_MainTeamUI");
		e->items_.push_back("SGE_MainTaskUI");
		e->items_.push_back("SGE_MainTaskFlushOk");
		e->items_.push_back("SGE_JJCEntryUI");
		e->items_.push_back("SGE_OfflineJJCUI");
		e->items_.push_back("SGE_BagItemDoubleClick");
		e->items_.push_back("SGE_BagFuwenOpen");
		e->items_.push_back("SGE_BagFuwenCombieUI");
		e->items_.push_back("SGE_BagFuwenCombieSuccess");
		e->items_.push_back("SGE_BagFuwenClickTipsInsertBtn");
		e->items_.push_back("SGE_FuwenUIClose");
		e->items_.push_back("SGE_NpcDialogBegin");
		e->items_.push_back("SGE_NpcRenwuUIOpen");
		e->items_.push_back("SGE_NpcRenwuPreAccept");
		e->items_.push_back("SGE_NpcRenwuAccept");
		e->items_.push_back("SGE_NpcRenwuSubmit");
		e->items_.push_back("SGE_EnterNPCBattle");
		e->items_.push_back("SGE_EnterBattle");
		e->items_.push_back("SGE_MessageBoxOpen");
		e->items_.push_back("SGE_BeforeEnterBattle");
		e->items_.push_back("SGE_PlayerLevelUp");
		e->items_.push_back("SGE_PlayerUIOpen");
		e->items_.push_back("SGE_PlayerUIStatusSwitch");
		e->items_.push_back("SGE_PlayerUIPropertySwitch");
		e->items_.push_back("SGE_PlayerUIAddPoint");
		e->items_.push_back("SGE_PlayerUIPropertyConfirmClick");
		e->items_.push_back("SGE_PlayerUIClose");
		e->items_.push_back("SGE_BabyLevelUp");
		e->items_.push_back("SGE_BabyUIOpen");
		e->items_.push_back("SGE_BabyUIStatusSwitch");
		e->items_.push_back("SGE_BabyUIPropertySwitch");
		e->items_.push_back("SGE_BabyUIAddPoint");
		e->items_.push_back("SGE_BabyUIPropertyConfirmClick");
		e->items_.push_back("SGE_BabyUIClose");
		e->items_.push_back("SGE_BattleTurn");
		e->items_.push_back("SGE_ExitBattle");
		e->items_.push_back("SGE_SelectTarget");
		e->items_.push_back("SGE_SelectTargetOk");
		e->items_.push_back("SGE_BabySelectSkill");
		e->items_.push_back("SGE_SelectSkill");
		e->items_.push_back("SGE_SelectSkillLevel");
		e->items_.push_back("SGE_StickDisplay");
		e->items_.push_back("SGE_StickTouchDown");
		e->items_.push_back("SGE_StickTouchMove");
		e->items_.push_back("SGE_StickTouchUp");
		e->items_.push_back("SGE_BattleOverRewardOpen");
		e->items_.push_back("SGE_BattleOverRewardClose");
		e->items_.push_back("SGE_CheckState");
		e->items_.push_back("SGE_TogetherState");
		e->items_.push_back("SGE_BagTipOpen");
		e->items_.push_back("SGE_UseItem");
		e->items_.push_back("SGE_GainItem");
		e->items_.push_back("SGE_EquipItem");
		e->items_.push_back("SGE_MainLearningUI");
		e->items_.push_back("SGE_MainLearningClickTab");
		e->items_.push_back("SGE_MainLearningOneSkillClick");
		e->items_.push_back("SGE_MainLearningSkillOk");
		e->items_.push_back("SGE_MainMakeUIOpen");
		e->items_.push_back("SGE_MainMagicTipClose");
		e->items_.push_back("SGE_MainMagicUIOpen");
		e->items_.push_back("SGE_MainMagicLevelUp");
		e->items_.push_back("SGE_MainMagicFirstClick");
		e->items_.push_back("SGE_PartnerShowUI");
		e->items_.push_back("SGE_PartnerHideUI");
		e->items_.push_back("SGE_BabyLearningSkillUI");
		e->items_.push_back("SGE_BabyLearningSkill_BabyListUI");
		e->items_.push_back("SGE_BabyLearningSkill_BabySkillUI");
		e->items_.push_back("SGE_BabyLearningSkillOk");
		e->items_.push_back("SGE_ClickBabyLearningSkill");
		e->items_.push_back("SGE_ClickMiniQuest");
		e->items_.push_back("SGE_ClickMainBag");
		e->items_.push_back("SGE_ClickMainSkill");
		e->items_.push_back("SGE_ClickMainBaby");
		e->items_.push_back("SGE_ClickMainFriend");
		e->items_.push_back("SGE_ClickMainPartner");
		e->items_.push_back("SGE_ClickMainSetting");
		e->items_.push_back("SGE_ClickMainRecharge");
		e->items_.push_back("SGE_ClickMainActivity");
		e->items_.push_back("SGE_ClickMainInfo");
		e->items_.push_back("SGE_ClickBattleAttack");
		e->items_.push_back("SGE_ClickBattleSkill");
		e->items_.push_back("SGE_ClickBattleBabySkill");
		e->items_.push_back("SGE_ClickBattleDeference");
		e->items_.push_back("SGE_ClickBattleChangePosition");
		e->items_.push_back("SGE_ClickBattleAuto");
		e->items_.push_back("SGE_ClickBattleBag");
		e->items_.push_back("SGE_ClickBattleBaby");
		e->items_.push_back("SGE_ClickBattleRunaway");
		e->items_.push_back("SGE_ClickBattleInfo");
		e->items_.push_back("SGE_ClickAddFriendBtn");
		e->items_.push_back("SGE_ClickMainFamily");
		e->items_.push_back("SGE_ClickRaiseUpBtn");
		e->items_.push_back("SGE_UseItemOk");
		e->items_.push_back("SGE_CheckBuff");
		e->items_.push_back("SGE_PlayerPropUpdate");
		e->items_.push_back("SGE_NpcTalked");
		e->items_.push_back("SGE_EnterCopy");
		e->items_.push_back("SGE_PlayerAutoOrder");
		e->items_.push_back("SGE_OpenAutoPanel");
		e->items_.push_back("SGE_SenseEnter2");
		e->items_.push_back("SGE_WaitTalk");
		e->items_.push_back("SGE_SenseTalked");
		e->items_.push_back("SGE_ExitSense");
		e->items_.push_back("SGE_Max");
}
EnumInfo enumScriptGameEvent("ScriptGameEvent", initFuncScriptGameEvent);
//=============================================================
static void initFuncSenseActorType(EnumInfo* e)
{
		e->items_.push_back("SAT_Guard");
		e->items_.push_back("SAT_Ambassdor");
		e->items_.push_back("SAT_King");
		e->items_.push_back("SAT_Yingzi");
		e->items_.push_back("SAT_VillageKing");
		e->items_.push_back("SAT_Archor");
		e->items_.push_back("SAT_Axe");
		e->items_.push_back("SAT_Sage");
		e->items_.push_back("SAT_Mage");
		e->items_.push_back("SAT_Girl");
		e->items_.push_back("SAT_AllMonster");
}
EnumInfo enumSenseActorType("SenseActorType", initFuncSenseActorType);
//=============================================================
static void initFuncGameEventType(EnumInfo* e)
{
		e->items_.push_back("GET_None");
		e->items_.push_back("GET_Online");
		e->items_.push_back("GET_Offline");
		e->items_.push_back("GET_CreatePlayerOK");
		e->items_.push_back("GET_Kill");
		e->items_.push_back("GET_Die");
		e->items_.push_back("GET_LevelUp");
		e->items_.push_back("GET_Flee");
		e->items_.push_back("GET_LearnSkill");
		e->items_.push_back("GET_SkillLevelUp");
		e->items_.push_back("GET_UseSkill");
		e->items_.push_back("GET_EnterJJC");
		e->items_.push_back("GET_MakeEquip");
		e->items_.push_back("GET_RecruitEmp");
		e->items_.push_back("GET_EmployeeEvolve");
		e->items_.push_back("GET_AddBaby");
		e->items_.push_back("GET_DelBaby");
		e->items_.push_back("GET_CatchBaby");
		e->items_.push_back("GET_DepositBaby");
		e->items_.push_back("GET_BabyLevelUp");
		e->items_.push_back("GET_ResetBaby");
		e->items_.push_back("GET_BabyNo");
		e->items_.push_back("GET_BabyLearnSkill");
		e->items_.push_back("GET_AddSkillExp");
		e->items_.push_back("GET_HalfHourAgo");
		e->items_.push_back("GET_Sign");
		e->items_.push_back("GET_BattleChangeProp");
		e->items_.push_back("GET_BattleOver");
		e->items_.push_back("GET_TalkNpc");
		e->items_.push_back("GET_Activity");
		e->items_.push_back("GET_PvR");
		e->items_.push_back("GET_JJC");
		e->items_.push_back("GET_Challenge");
		e->items_.push_back("GET_Zhuanpan");
		e->items_.push_back("GET_Richang");
		e->items_.push_back("GET_Pet");
		e->items_.push_back("GET_Tongji");
		e->items_.push_back("GET_Babyintensify");
		e->items_.push_back("GET_CreateGuild");
		e->items_.push_back("GET_JoinGuild");
		e->items_.push_back("GET_GuildBattleWin");
		e->items_.push_back("GET_GuildBattleLose");
		e->items_.push_back("GET_OpenGuildBattle");
		e->items_.push_back("GET_CloseGuildBattle");
		e->items_.push_back("GET_OpenGuildDemonInvaded");
		e->items_.push_back("GET_CloseGuildDemonInvaded");
		e->items_.push_back("GET_OpenGuildLeaderInvaded");
		e->items_.push_back("GET_CloseGuildLeaderInvaded");
		e->items_.push_back("GET_Exam");
		e->items_.push_back("GET_Wish");
		e->items_.push_back("GET_ChangeMoney");
		e->items_.push_back("GET_ChangeDiamond");
		e->items_.push_back("GET_ChangeMagicCurrency");
		e->items_.push_back("GET_WearEquip");
		e->items_.push_back("GET_AddFleeProp");
		e->items_.push_back("GET_Gather");
		e->items_.push_back("GET_AddFriend");
		e->items_.push_back("GET_ExtendStorage");
		e->items_.push_back("GET_FinishAch");
		e->items_.push_back("GET_AddTeamMemberCondition");
		e->items_.push_back("GET_Shenqishengji");
		e->items_.push_back("GET_NewServer");
		e->items_.push_back("GET_Recharge");
		e->items_.push_back("GET_PhoneNumber");
		e->items_.push_back("GET_ChangeProfession");
		e->items_.push_back("GET_BagFullSendMail");
		e->items_.push_back("GET_WearFuwen");
		e->items_.push_back("GET_Max");
}
EnumInfo enumGameEventType("GameEventType", initFuncGameEventType);
//=============================================================
static void initFuncSneakAttackType(EnumInfo* e)
{
		e->items_.push_back("SAT_None");
		e->items_.push_back("SAT_SneakAttack");
		e->items_.push_back("SAT_SurpriseAttack");
		e->items_.push_back("SAT_Max");
}
EnumInfo enumSneakAttackType("SneakAttackType", initFuncSneakAttackType);
//=============================================================
static void initFuncShopType(EnumInfo* e)
{
		e->items_.push_back("SIT_None");
		e->items_.push_back("SIT_FirstRecharge");
		e->items_.push_back("SIT_Recharge");
		e->items_.push_back("SIT_Shop");
		e->items_.push_back("SIT_EmployeeShop");
		e->items_.push_back("SIT_VIP");
		e->items_.push_back("SIT_Fund");
		e->items_.push_back("SIT_Giftbag");
		e->items_.push_back("SIT_CourseGiftBag");
		e->items_.push_back("SIT_Equip");
		e->items_.push_back("SIT_Max");
}
EnumInfo enumShopType("ShopType", initFuncShopType);
//=============================================================
static void initFuncShopPayType(EnumInfo* e)
{
		e->items_.push_back("SPT_Nil");
		e->items_.push_back("SPT_RMB");
		e->items_.push_back("SPT_Diamond");
		e->items_.push_back("SPT_MagicCurrency");
		e->items_.push_back("SPT_Gold");
}
EnumInfo enumShopPayType("ShopPayType", initFuncShopPayType);
//=============================================================
static void initFuncGuidePointerRotateType(EnumInfo* e)
{
		e->items_.push_back("GPRT_None");
		e->items_.push_back("GPRT_R45");
		e->items_.push_back("GPRT_R90");
		e->items_.push_back("GPRT_R135");
		e->items_.push_back("GPRT_R180");
		e->items_.push_back("GPRT_R225");
		e->items_.push_back("GPRT_R270");
		e->items_.push_back("GPRT_R315");
		e->items_.push_back("GPRT_Max");
}
EnumInfo enumGuidePointerRotateType("GuidePointerRotateType", initFuncGuidePointerRotateType);
//=============================================================
static void initFuncNpcType(EnumInfo* e)
{
		e->items_.push_back("NT_None");
		e->items_.push_back("NT_Normal");
		e->items_.push_back("NT_Tongji");
		e->items_.push_back("NT_SinglePK");
		e->items_.push_back("NT_TeamPK");
		e->items_.push_back("NT_Mogu");
		e->items_.push_back("NT_Xiji");
		e->items_.push_back("NT_Caiji1");
		e->items_.push_back("NT_Caiji2");
		e->items_.push_back("NT_Caiji3");
		e->items_.push_back("NT_Max");
}
EnumInfo enumNpcType("NpcType", initFuncNpcType);
//=============================================================
static void initFuncOpenSubSystemFlag(EnumInfo* e)
{
		e->items_.push_back("OSSF_None");
		e->items_.push_back("OSSF_Skill");
		e->items_.push_back("OSSF_Baby");
		e->items_.push_back("OSSF_Friend");
		e->items_.push_back("OSSF_EmployeeGet");
		e->items_.push_back("OSSF_EmployeeList");
		e->items_.push_back("OSSF_EmployeePosition");
		e->items_.push_back("OSSF_EmployeeEquip");
		e->items_.push_back("OSSF_Bar");
		e->items_.push_back("OSSF_Castle");
		e->items_.push_back("OSSF_JJC");
		e->items_.push_back("OSSF_PVPJJC");
		e->items_.push_back("OSSF_Make");
		e->items_.push_back("OSSF_Hundred");
		e->items_.push_back("OSSF_Activity");
		e->items_.push_back("OSSF_Handbook");
		e->items_.push_back("OSSF_EveryTask");
		e->items_.push_back("OSSF_Achieve");
		e->items_.push_back("OSSF_Rank");
		e->items_.push_back("OSSF_OnKyTalk");
		e->items_.push_back("OSSF_Setting");
		e->items_.push_back("OSSF_Shop");
		e->items_.push_back("OSSF_DoubleExp");
		e->items_.push_back("OSSF_Family");
		e->items_.push_back("OSSF_AuctionHouse");
		e->items_.push_back("OSSF_BabyLeranSkill");
		e->items_.push_back("OSSF_MagicItem");
		e->items_.push_back("OSSF_EmployeePos10");
		e->items_.push_back("OSSF_EmployeePos15");
		e->items_.push_back("OSSF_EmployeePos20");
		e->items_.push_back("OSSF_Guid");
		e->items_.push_back("OSSF_Team");
		e->items_.push_back("OSSF_choujiang");
		e->items_.push_back("OSSF_Cystal");
		e->items_.push_back("OSSF_PetEquip");
		e->items_.push_back("OSSF_Max");
}
EnumInfo enumOpenSubSystemFlag("OpenSubSystemFlag", initFuncOpenSubSystemFlag);
//=============================================================
static void initFuncAchievementType(EnumInfo* e)
{
		e->items_.push_back("AT_None");
		e->items_.push_back("AT_EarnConis");
		e->items_.push_back("AT_SpendMoney");
		e->items_.push_back("AT_SpendDiamond");
		e->items_.push_back("AT_Recharge");
		e->items_.push_back("AT_RoleLevel");
		e->items_.push_back("AT_PetLevel");
		e->items_.push_back("AT_AttackLevel");
		e->items_.push_back("AT_DefenseLevel");
		e->items_.push_back("AT_AgileLevel");
		e->items_.push_back("AT_WearCrystal");
		e->items_.push_back("AT_WearAccessories");
		e->items_.push_back("AT_TotalDamage");
		e->items_.push_back("AT_TotalTreatment");
		e->items_.push_back("AT_HasSkillNum");
		e->items_.push_back("AT_BabySkill");
		e->items_.push_back("AT_CatchPet");
		e->items_.push_back("AT_RecruitPartner");
		e->items_.push_back("AT_PartnerCard");
		e->items_.push_back("AT_PartnersUpgradeGreen");
		e->items_.push_back("AT_PartnersUpgradeBlue");
		e->items_.push_back("AT_PartnersUpgradePurple");
		e->items_.push_back("AT_PartnersUpgradeGold");
		e->items_.push_back("AT_PartnersUpgradeOrangle");
		e->items_.push_back("AT_PartnersUpgradePink");
		e->items_.push_back("AT_QualifyingAdvanced");
		e->items_.push_back("AT_ArenaWin");
		e->items_.push_back("AT_KillMonster");
		e->items_.push_back("AT_KillBoss");
		e->items_.push_back("AT_KillPlayer");
		e->items_.push_back("AT_MakeEquipment");
		e->items_.push_back("AT_Reward50");
		e->items_.push_back("AT_EverydayActivities");
		e->items_.push_back("AT_Sign");
		e->items_.push_back("AT_Wanted");
		e->items_.push_back("AT_Copy30");
		e->items_.push_back("AT_Copy40");
		e->items_.push_back("AT_Blood");
		e->items_.push_back("AT_Magic");
		e->items_.push_back("AT_Bag");
		e->items_.push_back("AT_PetBag");
		e->items_.push_back("AT_GoodMake");
		e->items_.push_back("AT_PetIntensive");
		e->items_.push_back("AT_PetHuman");
		e->items_.push_back("AT_PetInsect");
		e->items_.push_back("AT_PetPlant");
		e->items_.push_back("AT_PetExtra");
		e->items_.push_back("AT_PetDragon");
		e->items_.push_back("AT_PetAnimal");
		e->items_.push_back("AT_PetFly");
		e->items_.push_back("AT_PetUndead");
		e->items_.push_back("AT_PetMetal");
		e->items_.push_back("AT_Home");
		e->items_.push_back("AT_CollectMaterial");
		e->items_.push_back("AT_Friend");
		e->items_.push_back("AT_Billboard");
		e->items_.push_back("AT_OwnConis");
		e->items_.push_back("AT_MagicEquip");
		e->items_.push_back("AT_RunesLevel");
		e->items_.push_back("AT_Max");
}
EnumInfo enumAchievementType("AchievementType", initFuncAchievementType);
//=============================================================
static void initFuncCategoryType(EnumInfo* e)
{
		e->items_.push_back("ACH_All");
		e->items_.push_back("ACH_Growup");
		e->items_.push_back("ACH_Battle");
		e->items_.push_back("ACH_Pet");
		e->items_.push_back("ACH_Partner");
		e->items_.push_back("ACH_Illustrations");
		e->items_.push_back("ACH_Max");
}
EnumInfo enumCategoryType("CategoryType", initFuncCategoryType);
//=============================================================
static void initFuncClassifyType(EnumInfo* e)
{
		e->items_.push_back("SD_Debris");
		e->items_.push_back("SD_Data");
		e->items_.push_back("SD_Pet");
		e->items_.push_back("SD_Fashion");
		e->items_.push_back("SD_Diamond");
		e->items_.push_back("SD_1Ji");
		e->items_.push_back("SD_2Ji");
}
EnumInfo enumClassifyType("ClassifyType", initFuncClassifyType);
//=============================================================
static void initFuncFunctionalPointType(EnumInfo* e)
{
		e->items_.push_back("FPT_None");
		e->items_.push_back("FPT_Tongji");
		e->items_.push_back("FPT_Mogu");
		e->items_.push_back("FPT_Xiji");
		e->items_.push_back("FPT_Npc");
		e->items_.push_back("FPT_Max");
}
EnumInfo enumFunctionalPointType("FunctionalPointType", initFuncFunctionalPointType);
//=============================================================
static void initFuncActivityType(EnumInfo* e)
{
		e->items_.push_back("ACT_None");
		e->items_.push_back("ACT_Tongji");
		e->items_.push_back("ACT_Mogu");
		e->items_.push_back("ACT_Richang");
		e->items_.push_back("ACT_Pet");
		e->items_.push_back("ACT_AlonePK");
		e->items_.push_back("ACT_TeamPK");
		e->items_.push_back("ACT_JJC");
		e->items_.push_back("ACT_Challenge");
		e->items_.push_back("ACT_Exam");
		e->items_.push_back("ACT_Copy");
		e->items_.push_back("ACT_Xuyuan");
		e->items_.push_back("ACT_Family_0");
		e->items_.push_back("ACT_Family_1");
		e->items_.push_back("ACT_Family_2");
		e->items_.push_back("ACT_Family_3");
		e->items_.push_back("ACT_Family_4");
		e->items_.push_back("ACT_Warrior");
		e->items_.push_back("ACT_Money");
		e->items_.push_back("ACT_Rand");
		e->items_.push_back("ACT_Max");
}
EnumInfo enumActivityType("ActivityType", initFuncActivityType);
//=============================================================
static void initFuncMailType(EnumInfo* e)
{
		e->items_.push_back("MT_Normal");
		e->items_.push_back("MT_System");
}
EnumInfo enumMailType("MailType", initFuncMailType);
//=============================================================
static void initFuncHelpType(EnumInfo* e)
{
		e->items_.push_back("HT_None");
		e->items_.push_back("HT_Money");
		e->items_.push_back("HT_Diamond");
		e->items_.push_back("HT_Role");
		e->items_.push_back("HT_Baby");
		e->items_.push_back("HT_Employee");
		e->items_.push_back("HT_Skill");
		e->items_.push_back("HT_Exp");
		e->items_.push_back("HT_Magic");
		e->items_.push_back("HT_Equip");
}
EnumInfo enumHelpType("HelpType", initFuncHelpType);
//=============================================================
static void initFuncGuildJob(EnumInfo* e)
{
		e->items_.push_back("GJ_None");
		e->items_.push_back("GJ_People");
		e->items_.push_back("GJ_Minister");
		e->items_.push_back("GJ_SecretaryHead");
		e->items_.push_back("GJ_VicePremier");
		e->items_.push_back("GJ_Premier");
		e->items_.push_back("GJ_Max");
}
EnumInfo enumGuildJob("GuildJob", initFuncGuildJob);
//=============================================================
static void initFuncModifyListFlag(EnumInfo* e)
{
		e->items_.push_back("MLF_Add");
		e->items_.push_back("MLF_Delete");
		e->items_.push_back("MLF_ChangeOnline");
		e->items_.push_back("MLF_ChangeOffline");
		e->items_.push_back("MLF_ChangePosition");
		e->items_.push_back("MLF_ChangeContribution");
		e->items_.push_back("MLF_ChangeLevel");
		e->items_.push_back("MLF_ChangeProfession");
		e->items_.push_back("MLF_ChangeGuuildBattleCon");
}
EnumInfo enumModifyListFlag("ModifyListFlag", initFuncModifyListFlag);
//=============================================================
static void initFuncGuildBuildingType(EnumInfo* e)
{
		e->items_.push_back("GBT_MIN");
		e->items_.push_back("GBT_Main");
		e->items_.push_back("GBT_Bank");
		e->items_.push_back("GBT_Shop");
		e->items_.push_back("GBT_Collection");
		e->items_.push_back("GBT_Goddess");
		e->items_.push_back("GBT_Progenitus");
		e->items_.push_back("GBT_MAX");
}
EnumInfo enumGuildBuildingType("GuildBuildingType", initFuncGuildBuildingType);
//=============================================================
static void initFuncSellItemType(EnumInfo* e)
{
		e->items_.push_back("SelIT_None");
		e->items_.push_back("SelIT_Max");
}
EnumInfo enumSellItemType("SellItemType", initFuncSellItemType);
//=============================================================
static void initFuncDiamondConfigClass(EnumInfo* e)
{
		e->items_.push_back("DBT_Type_None");
		e->items_.push_back("DBT_Type_Mine_Famu");
		e->items_.push_back("DBT_Type_Mine_Caikuang");
		e->items_.push_back("DBT_Type_Mine_Zhibu");
		e->items_.push_back("DBT_Type_Max");
}
EnumInfo enumDiamondConfigClass("DiamondConfigClass", initFuncDiamondConfigClass);
//=============================================================
static void initFuncDiamondConfigType(EnumInfo* e)
{
		e->items_.push_back("DBT_None");
		e->items_.push_back("DBT_Day");
		e->items_.push_back("DBT_Week");
		e->items_.push_back("DBT_Month");
		e->items_.push_back("DBT_Max");
}
EnumInfo enumDiamondConfigType("DiamondConfigType", initFuncDiamondConfigType);
//=============================================================
static void initFuncFixType(EnumInfo* e)
{
		e->items_.push_back("FT_None");
		e->items_.push_back("FT_Money");
		e->items_.push_back("FT_Diamond");
		e->items_.push_back("FT_Max");
}
EnumInfo enumFixType("FixType", initFuncFixType);
//=============================================================
static void initFuncStorageType(EnumInfo* e)
{
		e->items_.push_back("ST_Item");
		e->items_.push_back("ST_Baby");
}
EnumInfo enumStorageType("StorageType", initFuncStorageType);
//=============================================================
static void initFuncEmployeesBattleGroup(EnumInfo* e)
{
		e->items_.push_back("EBG_None");
		e->items_.push_back("EBG_Free");
		e->items_.push_back("EBG_GroupOne");
		e->items_.push_back("EBG_GroupTwo");
		e->items_.push_back("EBG_Max");
}
EnumInfo enumEmployeesBattleGroup("EmployeesBattleGroup", initFuncEmployeesBattleGroup);
//=============================================================
static void initFuncGiftType(EnumInfo* e)
{
		e->items_.push_back("GFT_Bug");
		e->items_.push_back("GFT_UC1");
		e->items_.push_back("GFT_UC2");
		e->items_.push_back("GFT_Group");
}
EnumInfo enumGiftType("GiftType", initFuncGiftType);
//=============================================================
static void initFuncGMT_Protocol(EnumInfo* e)
{
		e->items_.push_back("GMT_None");
		e->items_.push_back("GMT_GMCommand");
		e->items_.push_back("GMT_Notice");
		e->items_.push_back("GMT_InsertMail");
		e->items_.push_back("GMT_QueryPlayer");
		e->items_.push_back("GMT_LoginActivity");
		e->items_.push_back("GMT_7Days");
		e->items_.push_back("GMT_Cards");
		e->items_.push_back("GMT_ExtractEmployee");
		e->items_.push_back("GMT_ChargeTotal");
		e->items_.push_back("GMT_ChargeEvery");
		e->items_.push_back("GMT_DiscountStore");
		e->items_.push_back("GMT_Foundation");
		e->items_.push_back("GMT_LoginTotal");
		e->items_.push_back("GMT_OnlineReward");
		e->items_.push_back("GMT_HotRole");
		e->items_.push_back("GMT_SelfChargeTotal");
		e->items_.push_back("GMT_SelfChargeEvery");
		e->items_.push_back("GMT_MinGiftBag");
		e->items_.push_back("GMT_DoScript");
		e->items_.push_back("GNT_MakeOrder");
		e->items_.push_back("GMT_Zhuanpan");
		e->items_.push_back("GMT_IntegralShop");
		e->items_.push_back("GMT_QueryRoleList");
		e->items_.push_back("GMT_QueryRMB");
		e->items_.push_back("GMT_QueryDia");
		e->items_.push_back("GMT_QueryMoney");
		e->items_.push_back("GMT_MAX");
}
EnumInfo enumGMT_Protocol("GMT_Protocol", initFuncGMT_Protocol);
//=============================================================
static void initFuncGMCommandType(EnumInfo* e)
{
		e->items_.push_back("GMCT_NoTalk");
		e->items_.push_back("GMCT_Freeze");
		e->items_.push_back("GMCT_Seal");
		e->items_.push_back("GMCT_Kick");
		e->items_.push_back("GMCT_OpenTalk");
		e->items_.push_back("GMCT_Unseal");
		e->items_.push_back("GMCT_SkipGuide");
		e->items_.push_back("GMCT_AddMoney");
		e->items_.push_back("GMCT_AddDiamond");
		e->items_.push_back("GMCT_AddExp");
		e->items_.push_back("GMCT_OpenGM");
		e->items_.push_back("GMCT_CloseGM");
		e->items_.push_back("GMCT_DoScript");
		e->items_.push_back("GMCT_Max");
}
EnumInfo enumGMCommandType("GMCommandType", initFuncGMCommandType);
//=============================================================
static void initFuncNoticeSendType(EnumInfo* e)
{
		e->items_.push_back("NST_Immediately");
		e->items_.push_back("NST_Timming");
		e->items_.push_back("NST_Loop");
}
EnumInfo enumNoticeSendType("NoticeSendType", initFuncNoticeSendType);
//=============================================================
static void initFuncInsertMailType(EnumInfo* e)
{
		e->items_.push_back("IGMT_PlayerId");
		e->items_.push_back("IGMT_AllOnline");
		e->items_.push_back("IGMT_AllRegist");
}
EnumInfo enumInsertMailType("InsertMailType", initFuncInsertMailType);
//=============================================================
static void initFuncItemContainerType(EnumInfo* e)
{
		e->items_.push_back("ICT_EquipContainer");
		e->items_.push_back("ICT_BagContainer");
}
EnumInfo enumItemContainerType("ItemContainerType", initFuncItemContainerType);
//=============================================================
static void initFuncUIBehaviorType(EnumInfo* e)
{
		e->items_.push_back("UBT_None");
		e->items_.push_back("UBT_Bag");
		e->items_.push_back("UBT_Baby");
		e->items_.push_back("UBT_Employee");
		e->items_.push_back("UBT_SkillView");
		e->items_.push_back("UBT_SkillLearn");
		e->items_.push_back("UBT_Task");
		e->items_.push_back("UBT_Make");
		e->items_.push_back("UBT_Gather");
		e->items_.push_back("UBT_MagicItem");
		e->items_.push_back("UBT_Store");
		e->items_.push_back("UBT_Help");
		e->items_.push_back("UBT_Friend");
		e->items_.push_back("UBT_Email");
		e->items_.push_back("UBT_Auction");
		e->items_.push_back("UBT_Activity");
		e->items_.push_back("UBT_SignUp");
		e->items_.push_back("UBT_EmployessList");
		e->items_.push_back("UBT_EmployessPos");
		e->items_.push_back("UBT_EmployessTavern");
		e->items_.push_back("UBT_PlayerProperty");
		e->items_.push_back("UBT_Max");
}
EnumInfo enumUIBehaviorType("UIBehaviorType", initFuncUIBehaviorType);
//=============================================================
static void initFuncHelpRaiseType(EnumInfo* e)
{
		e->items_.push_back("HRT_None");
		e->items_.push_back("HRT_PlayerAddProp");
		e->items_.push_back("HRT_PlayerResetProp");
		e->items_.push_back("HRT_PlayerAddEvolve");
		e->items_.push_back("HRT_BabyAddProp");
		e->items_.push_back("HRT_BabyReset");
		e->items_.push_back("HRT_BabyStr");
		e->items_.push_back("HRT_BabySkill");
		e->items_.push_back("HRT_BabyChange");
		e->items_.push_back("HRT_SkillAuto");
		e->items_.push_back("HRT_SkillItem");
		e->items_.push_back("HRT_EquipCompound");
		e->items_.push_back("HRT_EquipGem");
		e->items_.push_back("HRT_EmployeeBuy");
		e->items_.push_back("HRT_EmployeePos");
		e->items_.push_back("HRT_EmployeeSkill");
		e->items_.push_back("HRT_EmployeeEquip");
		e->items_.push_back("HRT_EmployeeEvolve");
		e->items_.push_back("HRT_MagicLevelUp");
		e->items_.push_back("HRT_MagicEvolve");
		e->items_.push_back("HRT_Max");
}
EnumInfo enumHelpRaiseType("HelpRaiseType", initFuncHelpRaiseType);
//=============================================================
static void initFuncSceneFilterType(EnumInfo* e)
{
		e->items_.push_back("SFT_None");
		e->items_.push_back("SFT_Team");
		e->items_.push_back("SFT_Friend");
		e->items_.push_back("SFT_Guild");
		e->items_.push_back("SFT_All");
		e->items_.push_back("SFT_Max");
}
EnumInfo enumSceneFilterType("SceneFilterType", initFuncSceneFilterType);
//=============================================================
static void initFuncGatherStateType(EnumInfo* e)
{
		e->items_.push_back("GST_None");
		e->items_.push_back("GST_Vulgar");
		e->items_.push_back("GST_Advanced");
		e->items_.push_back("GST_Max");
}
EnumInfo enumGatherStateType("GatherStateType", initFuncGatherStateType);
//=============================================================
static void initFuncWishType(EnumInfo* e)
{
		e->items_.push_back("WIT_None");
		e->items_.push_back("WIT_Exp");
		e->items_.push_back("WIT_Money");
		e->items_.push_back("WIT_Baby");
		e->items_.push_back("WIT_Employee");
		e->items_.push_back("WIT_Max");
}
EnumInfo enumWishType("WishType", initFuncWishType);
//=============================================================
static void initFuncADType(EnumInfo* e)
{
		e->items_.push_back("ADT_None");
		e->items_.push_back("ADT_7Days");
		e->items_.push_back("ADT_Cards");
		e->items_.push_back("ADT_ChargeTotal");
		e->items_.push_back("ADT_ChargeEvery");
		e->items_.push_back("ADT_DiscountStore");
		e->items_.push_back("ADT_Foundation");
		e->items_.push_back("ADT_LoginTotal");
		e->items_.push_back("ADT_OnlineReward");
		e->items_.push_back("ADT_HotRole");
		e->items_.push_back("ADT_SelfChargeTotal");
		e->items_.push_back("ADT_SelfChargeEvery");
		e->items_.push_back("ADT_SelfDiscountStore");
		e->items_.push_back("ADT_BuyEmployee");
		e->items_.push_back("ADT_PhoneNumber");
		e->items_.push_back("ADT_Level");
		e->items_.push_back("ADT_Sign");
		e->items_.push_back("ADT_GiftBag");
		e->items_.push_back("ADT_Zhuanpan");
		e->items_.push_back("ADT_IntegralShop");
		e->items_.push_back("ADT_Max");
}
EnumInfo enumADType("ADType", initFuncADType);
//=============================================================
static void initFuncEmployeeSkillType(EnumInfo* e)
{
		e->items_.push_back("EKT_GroupDamage");
		e->items_.push_back("EKT_DeadlyDamage");
		e->items_.push_back("EKT_BattleTimeLimit");
		e->items_.push_back("EKT_Thump");
		e->items_.push_back("EKT_SiegeDamage");
		e->items_.push_back("EKT_SuperMagic");
		e->items_.push_back("EKT_Debuff");
		e->items_.push_back("EKT_PhysicalDefense");
		e->items_.push_back("EKT_Max");
}
EnumInfo enumEmployeeSkillType("EmployeeSkillType", initFuncEmployeeSkillType);
//=============================================================
static void initFuncEmployeeQuestColor(EnumInfo* e)
{
		e->items_.push_back("EQC_White");
		e->items_.push_back("EQC_Blue");
		e->items_.push_back("EQC_Purple");
		e->items_.push_back("EQC_Max");
}
EnumInfo enumEmployeeQuestColor("EmployeeQuestColor", initFuncEmployeeQuestColor);
//=============================================================
static void initFuncEmployeeQuestStatus(EnumInfo* e)
{
		e->items_.push_back("EQS_None");
		e->items_.push_back("EQS_Running");
		e->items_.push_back("EQS_Complate");
}
EnumInfo enumEmployeeQuestStatus("EmployeeQuestStatus", initFuncEmployeeQuestStatus);
//=============================================================
static void initFuncRobotActionType(EnumInfo* e)
{
		e->items_.push_back("RAT_None");
		e->items_.push_back("RAT_Resting");
		e->items_.push_back("RAT_Move");
		e->items_.push_back("RAT_QuestMove");
		e->items_.push_back("RAT_TeamMove");
		e->items_.push_back("RAT_Max");
}
EnumInfo enumRobotActionType("RobotActionType", initFuncRobotActionType);
