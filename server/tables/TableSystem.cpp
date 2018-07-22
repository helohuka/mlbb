#include "config.h"
#include "tinyxml/tinyxml.h"
#include "CSVParser.h"
#include "TableSystem.h"
#include "tmptable.h"
#include "skilltable.h"
#include "sttable.h"
#include "TokenParser.h"
#include "itemtable.h"
#include "DropTable.h"
#include "npctable.h"
#include "monstertable.h"
#include "scenetable.h"
#include "employeeTable.h"
#include "Quest.h"
#include "LiveSkilltable.h"
#include "LotteryTable.h"
#include "randName.h"
#include "robotTable.h"
#include "profession.h"
#include "BattleData.h"
#include "Shop.h"
#include "GameEvent.h"
#include "titleTable.h"
#include "achievementTable.h"
#include "challengeTable.h"
#include "DailyReward.h"
#include "PVPrunkTable.h"
#include "EmployeeConfig.h"
#include "DebrisTable.h"
#include "ArtifactLevelTable.h"
#include "ArtifactConfigTable.h"
#include "ArtifactChangeData.h"
#include "FilterWord.h"
#include "Activities.h"
#include "GuildData.h"
#include "copyscene.h"
#include "lastOrder.h"
#include "EmployeeQuestTable.h"
bool TableSystem::Load(){
	///load
	FUNCTION_PROBE;
	if(false == PlayerTmpTable::load(GetTableFilePath("PlayerData.csv")))
		return false;
	if(false == SkillTable::load(GetTableFilePath("SkillData.csv")))
		return false;
	if(false == SkillTable::loadBabyEquipSkillGroup(GetTableFilePath("BabySuitSkill.csv")))
		return false;
	if(false == StateTable::load(GetTableFilePath("State.csv")))
		return false;
	if(false == ItemTable::load(GetTableFilePath("ItemData.csv")))
		return false;
	if(false == DropTable::load(GetTableFilePath("Drop.csv")))
		return false;
	if(false == NpcTable::load(GetTableFilePath("Npc.csv")))
		return false;
	if(false == MonsterTable::load(GetTableFilePath("Monster.csv")))
		return false;
	if(false == MonsterClass::load(GetTableFilePath("AIclass.csv")))
		return false;
	if(false == Monster2Table::load(GetTableFilePath("Monster2.csv")))
		return false;
	if(false == SceneTable::load(GetTableFilePath("Scene.csv")))
		return false;
	if(false == EmployeeTable::load(GetTableFilePath("EmployeeData.csv")))
		return false;
	if(false == ExpTable::load(GetTableFilePath("Exp.csv")))
		return false;
	if(false == Quest::load(GetTableFilePath("Quest.csv")))
		return false;
	if(false == GatherTable::load(GetTableFilePath("Gather.csv")))
		return false;
	if(false == MakeTable::load(GetTableFilePath("Make.csv")))
		return false;
	if(false == LotteryTable::load(GetTableFilePath("Lottery.csv")))
		return false;
	if(false == RandNameTable::load(GetTableFilePath("Randname.csv")))
		return false;
	if(false == RobotTab::load(GetTableFilePath("RobotData.csv")))
		return false;
	if(false == PlayerAI::load(GetTableFilePath("ArenaAiData.csv")))
		return false;
	if(false == Profession::load(GetTableFilePath("ProfessionData.csv")))
		return false;
	if(false == BattleData::load(GetTableFilePath("BattleData.csv")))
		return false;
	if(false == Shop::load(GetTableFilePath("ShopData.csv")))
		return false;
	if(false == GameEvent::load(GetTableFilePath("GameEvent.xml")))
		return false;
	if(false == TitleTable::load(GetTableFilePath("Title.csv")))
		return false;
	if(false == AchievementTable::load(GetTableFilePath("AchieveData.csv")))
		return false;
	if(false == ChallengeTable::load(GetTableFilePath("ChallengeData.csv")))
		return false;
	if(false == DailyReward::load(GetTableFilePath("DailyReward.csv")))
		return false;
	if(false == PvpRunkTable::load(GetTableFilePath("PVPrunk.csv")))
		return false;
	if(false == PvRrewardTable::load(GetTableFilePath("PVRreward.csv")))
		return false;
	if(false == EmployeeConfigTable::load(GetTableFilePath("EmployeeConfig.csv")))
		return false;
	if(false == DiamondsConfig::load(GetTableFilePath("Diamondsconfig.csv")))
		return false;
	if(false == DebrisTable::load(GetTableFilePath("DebrisConfig.csv")))
		return false;
	if(false == ArtifactLevelTable::load(GetTableFilePath("ArtifactLevel.csv")))
		return false;
	if(false == ArtifactConfigTable::load(GetTableFilePath("ArtifactConfig.csv")))
		return false;
	if(false == FilterWord::load(GetTableFilePath("filterword.csv")))
		return false;
	if(false == ArtifactChangeTable::load(GetTableFilePath("ArtifactChange.csv")))
		return false;
	if(false == PetActivity::load(GetTableFilePath("PetActivityData.csv")))
		return false;
	if(false == GuildShopItemData::load(GetTableFilePath("HomeShopData.csv")))
		return false;
	if(false == GuildBuildingData::load(GetTableFilePath("family.csv")))
		return false;
	if(false == GuildBlessingData::load(GetTableFilePath("Blessing.csv")))
		return false;
	if(false == PetIntensive::load(GetTableFilePath("PetIntensive.csv")))
		return false;
	if(false == CopyScene::load(GetTableFilePath("Copy.csv")))
		return false;
	if(false == OnlineTimeClass::load(GetTableFilePath("timereward.csv")))
		return false;
	if(false == GrowFundTable::load(GetTableFilePath("foundation.csv")))
		return false;
	if(false == RuneTable::load(GetTableFilePath("Runes.csv")))
		return false;
	if(false == LastOrderTable::load(GetTableFilePath("lastOrder.csv")))
		return false;
	if(false == RobotActionTable::load(GetTableFilePath("AiNt.csv")))
		return false;
	if(false == EmployeeQuest::load(GetTableFilePath("EmployeeQuest.csv")))
		return false;
	if(false == EmployeeSkill::load(GetTableFilePath("EmployeeSkill.csv")))
		return false;
	if(false == EmployeeMonster::load(GetTableFilePath("EmployeeMonster.csv")))
		return false;
	if(false == CrystalTable::load(GetTableFilePath("Crystal.csv")))
		return false;
	if(false == CrystalUpTable::load(GetTableFilePath("CrystalUp.csv")))
		return false;
	return true;
}
bool TableSystem::Check(){
	///check
	if(false == PlayerTmpTable::check())
		return false;
	if(false == SkillTable::check())
		return false;
	if(false == StateTable::check())
		return false;
	if(false == ItemTable::check())
		return false;
	if(false == DropTable::check())
		return false;
	if(false == NpcTable::check())
		return false;
	if(false == MonsterTable::check())
		return false;
	if(false == Monster2Table::check())
		return false;
	if(false == MonsterClass::check())
		return false;
	if(false == SceneTable::check())
		return false;
	if(false == EmployeeTable::check())
		return false;
	if(false == ExpTable::check())
		return false;
	if(false == GatherTable::check())
		return false;
	if(false == MakeTable::check())
		return false;
	if(false == LotteryTable::check())
		return false;
	if(false == RandNameTable::check())
		return false;
	if(false == RobotTab::check())
		return false;
	if(false == PlayerAI::check())
		return false;
	if(false == Profession::check())
		return false;
	if(false == BattleData::check())
		return false;
	if(false == Shop::check())
		return false;
	if(false == GameEvent::check())
		return false;
	if(false == TitleTable::check())
		return false;
	if(false == AchievementTable::check())
		return false;
	if(false == ChallengeTable::check())
		return false;
	if(false == DailyReward::check())
		return false;
	if(false == PvpRunkTable::check())
		return false;
	if(false == EmployeeConfigTable::check())
		return false;
	if(false == DebrisTable::check())
		return false;
	if(false == PetActivity::check())
		return false;
	if(false == CopyScene::check())
		return false;
	if(false == ArtifactLevelTable::check())
		return false;
	if(false == ArtifactConfigTable::check())
		return false;
	if(false == GuildBlessingData::check())
		return false;
	if(false == OnlineTimeClass::check())
		return false;
	if(false == GrowFundTable::check())
		return false;
	if(false == RuneTable::check())
		return false;
	if(false == Quest::check())
		return false;
	if(false == RobotActionTable::check())
		return false;
	if(false == EmployeeQuest::check())
		return false;
	if(false == CrystalTable::check())
		return false;
	if(false == CrystalUpTable::check())
		return false;
	return true;
}

