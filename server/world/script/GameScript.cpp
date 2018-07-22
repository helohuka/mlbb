#include "config.h"
#include "com.h"
#include "struct.h"
#include "ComScriptEvn.h"
#include "GMCmdMgr.h"
#include "ComScriptHeader.h"
#include "account.h"
#include "entity.h"
#include "player.h"
#include "tmptable.h"
#include "battle.h"
#include "monster.h"
#include "itemtable.h"
#include "skilltable.h"
#include "Robot.h"
#include "baby.h"
#include "Activity.h"
#include "team.h"
#include "worldserv.h"
#include "scenehandler.h"
#include "profession.h"
#include "EndlessStair.h"
#include "pvpJJC.h"
#include "npctable.h"
#include "monstertable.h"
#include "Guild.h"
#include "scenehandler.h"
#include "loghandler.h"
#include "DropTable.h"
#include "Quest.h"
#include "titleTable.h"
#include "EmployeeQuestSystem.h"
#include "TableSystem.h"
GAME_SCRIPT_API(Sys, reload_item_table){
	ItemTable::load(GetTableFilePath("ItemData.csv"));
	R_NONE;
}

GAME_SCRIPT_API(Sys, reload_drop_table){
	DropTable::load(GetTableFilePath("Drop.csv"));
	R_NONE;
}

GAME_SCRIPT_API(Sys, reload_ai_table){
	MonsterClass::load(GetTableFilePath("AIclass.csv"));
	R_NONE;
}

GAME_SCRIPT_API(Sys, reload_quest_table){
	Quest::load(GetTableFilePath("Quest.csv"));
	R_NONE;
}

GAME_SCRIPT_API( Sys, add_gm_cmd )
{
	P_BEGIN;
	P_STR( cmd );
	P_INT( level );
	P_STR( funName );
	P_END;
	GMCmdMgr::addGmCmd( cmd, (GMLevel)level, funName );
	R_NONE;
}

GAME_SCRIPT_API( Sys, pass_zero_hour){
	Player::OnlinePlayerPassZeroHour();
	
	R_NONE;
}
GAME_SCRIPT_API( Sys, get_world_level){
	
	R_BEGIN;
	R_INT(WorldServ::instance()->getAverageLevel());
	R_END;
}

GAME_SCRIPT_API( Sys, updaterolerogtable){
	WorldServ::instance()->updateRoleLogTable();
	R_NONE;
}

GAME_SCRIPT_API( Sys, allonlineplayeraddmoney){
	P_BEGIN;
	P_INT( money );
	P_END;
	Player::OnlinePlayerAddMoney(money);
	R_NONE;
}

GAME_SCRIPT_API( Sys, sys_refreshEmployeeQuest){
	P_BEGIN;
	P_INT( color );
	P_END;
	EmployeeQuestSystem::Refresh((EmployeeQuestColor)color);
	R_NONE;
}

GAME_SCRIPT_API( Player, errorhint)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(hint);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->errorMessageToC((ErrorNo)hint);

	R_NONE;
}

GAME_SCRIPT_API( Player, add_Reputation)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(value);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->addReputation(value);
	
	R_NONE;
}

GAME_SCRIPT_API( Player, sub_property)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(pt);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;
	player->subProperty((PropertyType)pt);
	R_NONE;
}

GAME_SCRIPT_API( Player, reset_property)
{
	P_BEGIN;
	P_PTR(handle);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;
	player->resetProperty();
	R_NONE;
}

GAME_SCRIPT_API( Player, add_baby )
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(babyid);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;
	
	MonsterTable::MonsterData const * tmp = MonsterTable::getMonsterById(babyid);
	if(NULL == tmp)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find %d baby form baby-table\n"),babyid));
		R_NONE;
	}
	player->addBaby(tmp->monsterId_);
	
	R_NONE;
}

GAME_SCRIPT_API( Player, del_baby )
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(babyid);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->delBaby(babyid);
	R_NONE;
}

GAME_SCRIPT_API( Player, hasbaby)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(babyid);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_BOOL(false);
		R_END;
	}

	MonsterTable::MonsterData const * tmp = MonsterTable::getMonsterById(babyid);
	if(NULL == tmp)
	{
		R_BEGIN;
		R_BOOL(false);
		R_END;
	}

	Baby* p = player->findBabybyTableId(babyid);
	if(p != NULL)
	{
		R_BEGIN;
		R_BOOL(true);
		R_END;
	}

	R_BEGIN;
	R_BOOL(false);
	R_END;
}

GAME_SCRIPT_API( Player, get_hasbaby_race)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(race);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}
	S32 num = player->calchasBabybyRace((RaceType)race);
	R_BEGIN;
	R_INT(num);
	R_END;
}

GAME_SCRIPT_API( Player, get_baby_size){
	P_BEGIN;
	P_PTR(handle);
	P_END;

	if(NULL == handle)
	{
		R_BEGIN;
		R_INT(-1);
		R_END;
	}
	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_INT(-1);
		R_END;
	}
	R_BEGIN;
	R_INT(player->babies_.size());
	R_END;
}

GAME_SCRIPT_API( Player, get_minlevel_baby){
	P_BEGIN;
	P_PTR(handle);
	P_INT(tableid);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_INT(-1);
		R_END;
	}

	U32 instid = player->getminlevelbaby(tableid);

	if(instid == 0)
	{
		R_BEGIN;
		R_INT(-1);
		R_END;
	}

	R_BEGIN;
	R_INT(instid);
	R_END;
}

GAME_SCRIPT_API( Player, add_item)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(itemid);
	P_INT(itemNum);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->addBagItemByItemId(itemid,itemNum,false,8);

	R_NONE;
}

GAME_SCRIPT_API( Player, del_item)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(itemid);
	P_INT(itemNum);
	P_END;
	
	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->delBagItemByItemId(itemid,itemNum);
	
	R_NONE;
}

GAME_SCRIPT_API( Player, add_money)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(num);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->addMoney(num);

	SGE_LogProduceTrack track;
	track.playerId_ = player->getGUID();
	track.playerName_ = player->getNameC();
	track.from_ = 8;
	track.money_ = num;
	LogHandler::instance()->playerTrack(track);

	R_NONE;
}

GAME_SCRIPT_API( Player, add_diamond)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(num);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;
	ACE_DEBUG((LM_INFO,"Add diamond %s(%d) %d\n",player->getNameC(),player->getGUID(),num));
	player->addDiamond(num);

	SGE_LogProduceTrack track;
	track.playerId_ = player->getGUID();
	track.playerName_ = player->getNameC();
	track.from_ = 8;
	track.diamond_ = num;
	LogHandler::instance()->playerTrack(track);

	R_NONE;
}

GAME_SCRIPT_API( Player, add_rmb)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(num);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;
	ACE_DEBUG((LM_INFO,"Add MagicCurrency %s(%d) %d\n",player->getNameC(),player->getGUID(),num));
	player->addMagicCurrency(num);

	SGE_LogProduceTrack track;
	track.playerId_ = player->getGUID();
	track.playerName_ = player->getNameC();
	track.from_ = 8;
	track.magic_ = num;
	LogHandler::instance()->playerTrack(track);

	R_NONE;
}

GAME_SCRIPT_API( Player, learn_skill)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(skid);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->learnSkill(skid);

	R_NONE;
}

GAME_SCRIPT_API( Player, forget_skill)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(skid);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->forgetSkill(skid);

	R_NONE;
}

GAME_SCRIPT_API( Player, add_Exp)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(expValue);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->addExp(expValue);

	R_NONE;
}

GAME_SCRIPT_API( Player, accept_quest)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(questId);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->acceptQuest(questId);

	R_NONE;
}

GAME_SCRIPT_API( Player, GMaccept_quest)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(questId);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->gmAcceptQuest(questId);

	R_NONE;
}

GAME_SCRIPT_API( Player, submit_quest)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(questId);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->gmSubmitQuest(questId);

	R_NONE;
}
GAME_SCRIPT_API( Player, is_complate_quest)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(questId);
	P_END;
	if(NULL == handle)
	{
		R_BEGIN;
		R_BOOL(0);
		R_END;
	}
	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_BOOL(0);
		R_END;
	}
	R_BEGIN;
	R_BOOL(player->isQuestComplate(questId)) ;
	R_END;
}

GAME_SCRIPT_API( Player, is_current_quest){
	P_BEGIN;
	P_PTR(handle);
	P_INT(questId);
	P_END;
	if(NULL == handle)
	{
		R_BEGIN;
		R_BOOL(0);
		R_END;
	}
	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_BOOL(0);
		R_END;
	}
	R_BEGIN;
	R_BOOL( (player->getQuestIndex(questId) != -1));
	R_END;
}
GAME_SCRIPT_API( Player, jump_quest)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(questId);
	P_END;
	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;
	player->gmJumpQuest(questId);

	R_NONE;
}

GAME_SCRIPT_API( Player, gm_add_skillExp)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(skillId);
	P_INT(expValue);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->addSkillExp(skillId, expValue,IUF_Scene);

	R_NONE;
}

GAME_SCRIPT_API( Player, add_ExpByBattleBaby)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(expValue);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	Baby *pBaby = player->getBattleBaby();
	if(NULL == pBaby)
		R_NONE;
	pBaby->addExp(expValue);

	R_NONE;
}

GAME_SCRIPT_API( Player, add_Employee)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(tmpId);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->addEmployee(tmpId);

	R_NONE;
}

GAME_SCRIPT_API( Player, set_level)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(level);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	for (S32 i = 0; i < level; ++i)
	{
		if(player->getProp(PT_Level) >= Global::get<float>(C_PlayerMaxLevel))
			R_NONE ;
		player->levelup();
	}
	player->scenePlayer_->scenePlayerUpLevel(player->getProp(PT_Level));
	R_NONE;
}

GAME_SCRIPT_API( Player, set_BabyLevel)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(level);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	Baby *pBaby = player->getBattleBaby();
	if(NULL == pBaby)
		R_NONE;

	for (S32 i = 0; i < level; ++i)
	{
		pBaby->levelup();
	}

	R_NONE;
}

GAME_SCRIPT_API( Player, set_HundredTier)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(tier);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->setHundredTier(tier);

	R_NONE;
}

GAME_SCRIPT_API( Player,complete_Achievement)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(achID);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->completeAchievement(achID);

	R_NONE;
}

GAME_SCRIPT_API( Player, complete_AllAchievement)
{
	P_BEGIN;
	P_PTR(handle);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->completeAllAchievement();

	R_NONE;
}

GAME_SCRIPT_API( Player, setAchuevementValue)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(achtype);
	P_INT(value);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;
	//ACE_DEBUG((LM_ERROR,ACE_TEXT("setAchievement achtype[%d]======> value==[%d] \n"),achtype,value));
	player->setAchievement((AchievementType)achtype,value);

	R_NONE;
}

GAME_SCRIPT_API( Player, addAchuevementValue)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(achtype);
	P_INT(value);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->addAchievement((AchievementType)achtype,value);

	R_NONE;
}

GAME_SCRIPT_API( Player, addGuildContribution)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(value);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	Guild::addMemberContribution(player,value);

	R_NONE;
}

GAME_SCRIPT_API( Player, addGuildMoney)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(value);
	P_END;

	if(NULL == handle)
		R_NONE;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE;

	Guild::addGuildMoneyOnly(player,value);

	R_NONE;
}

GAME_SCRIPT_API( Player, reset_counter)
{
	P_BEGIN;
	P_PTR(handle);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->resetCounter();

	R_NONE;
}

GAME_SCRIPT_API( Player, enter_Scene)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(sceneId);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->transforScene(sceneId);

	R_NONE;
}

GAME_SCRIPT_API( Player, set_glamour)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(glamour);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->setProp(PT_Glamour,glamour);

	R_NONE;
}

GAME_SCRIPT_API( Player, add_pvpjjcgrade)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(grade);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->pvpInfo_.value_ += grade;
	player->checkJJCsec();

	R_NONE;
}

//---------------------------------------------------------------------------------------------------

GAME_SCRIPT_API( Player, change_property )
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(target);
	P_INT(type);
	P_FLOAT(val);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->changeProp(target,(PropertyType)type, val);

	R_NONE;
}

GAME_SCRIPT_API( Player, autoprop)
{
	P_BEGIN;
	P_PTR(handle);
	P_END;

	if(NULL == handle)
		R_NONE ;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	player->autoaddprop();

	R_NONE;
}

GAME_SCRIPT_API( Player, get_property )
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(type);
	P_END;

	if(NULL == handle)
	{
		R_BEGIN;
		R_FLOAT(0);
		R_END;
	}
	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_FLOAT(0);
		R_END;
	}

	R_BEGIN;
	R_FLOAT(player->getProp((PropertyType)type));
	R_END;
}

GAME_SCRIPT_API( Player, getBabyProp)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(instId);
	P_INT(type);
	P_END;

	if(NULL == handle)
	{
		R_BEGIN;
		R_FLOAT(0);
		R_END;
	}
	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_FLOAT(0);
		R_END;
	}

	Baby* pbaby = player->findBaby(instId);
	if(pbaby == NULL)
	{
		R_BEGIN;
		R_FLOAT(0);
		R_END;
	}
	R_BEGIN;
	R_FLOAT(pbaby->getProp((PropertyType)type));
	R_END;
}

GAME_SCRIPT_API( Player, get_BattleBabyProp)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(type);
	P_END;

	if(NULL == handle)
	{
		R_BEGIN;
		R_FLOAT(0);
		R_END;
	}
	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_FLOAT(0);
		R_END;
	}

	if(player->getBattleBaby() == NULL)
	{
		R_BEGIN;
		R_FLOAT(0);
		R_END;
	}

	R_BEGIN;
	R_FLOAT(player->getBattleBaby()->getProp((PropertyType)type));
	R_END;
}

GAME_SCRIPT_API(Player, Lottery_Item)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(itemId);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_NONE;
	}

	player->lotteryGo(itemId);

	R_NONE;
}

GAME_SCRIPT_API( Player, canuseaddskillexpitem)
{
	P_BEGIN;
	P_PTR(handle);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_BOOL(false);
		R_END;
	}

	Profession const * prof = Profession::get((JobType)(int)player->getProp(PT_Profession),(int)player->getProp(PT_ProfessionLevel));
	if(NULL == prof)
	{
		R_BEGIN;
		R_BOOL(false);
		R_END;
	}

	for (size_t i = 0;i < player->skills_.size(); ++i)
	{
		if( player->skills_[i]->skLevel_ < prof->getSkillMaxLevel(player->skills_[i]->skId_) && player->canLevelUpSkill(player->skills_[i]->skId_,player->skills_[i]->skLevel_)){
			R_BEGIN;
			R_BOOL(true);
			R_END;
		}
	}
	R_BEGIN;
	R_BOOL(false);
	R_END;
}

GAME_SCRIPT_API( Player,add_SkillExp)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(itemId);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_NONE;
	}

	player->randSkillExp(itemId);
	R_NONE;
}

GAME_SCRIPT_API(Player,joinbattle){
	
	P_BEGIN;
	P_PTR(handle);
	P_INT(batleId);
	P_END;
	//ACE_DEBUG((LM_INFO, "Script >> Player.joinbattle%d\n",batleId));
	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_NONE;
	}

	player->enterBattle(batleId);

	R_NONE;
}


GAME_SCRIPT_API(Player,joinbattle2){

	P_BEGIN;
	P_PTR(handle);
	P_INT(batleId);
	P_END;
	//ACE_DEBUG((LM_INFO, "Script >> Player.joinbattle%d\n",batleId));
	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_NONE;
	}

	player->enterBattle2(batleId);

	R_NONE;
}

GAME_SCRIPT_API( Player, joinbattlez){

	P_BEGIN;
	P_PTR(handle);
	P_INT(zoneId);
	P_END;
	//ACE_DEBUG((LM_INFO, "Script >> Player.joinbattle%d\n",batleId));
	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_NONE;
	}

	player->zoneJoinBattle(zoneId);

	R_NONE;
}

GAME_SCRIPT_API( Player, addprivnpc){
	P_BEGIN;
	P_PTR(handle);
	P_INT(npcid);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE;
	player->localAddNpc(npcid);
	R_NONE;
}


GAME_SCRIPT_API(Player,delprivnpc){
	P_BEGIN;
	P_PTR(handle);
	P_INT(npcid);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE;
	player->localDelNpc(npcid);
	R_NONE;
	
}


GAME_SCRIPT_API(Player,delnpc){
	P_BEGIN;
	P_PTR(handle);
	P_INT(npcid);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE;
	SceneHandler::instance()->delDynamicNpc(npcid);

	R_NONE;
}

GAME_SCRIPT_API( Player, getnpctype)
{
	P_BEGIN;
	P_INT(npcid);
	P_END;

	const NpcTable::NpcData* npcd = NpcTable::getNpcById(npcid);

	if(npcd == NULL)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	R_BEGIN;
	R_INT(npcd->npcType_);
	R_END;
}

//----------------------------------------------------------------------------------------------------------------------------

GAME_SCRIPT_API( Player, set_opensubsystem)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(ossf);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_NONE;
	}

	player->setOpenSubSystemFlag((OpenSubSystemFlag)ossf);
	R_NONE;
}

GAME_SCRIPT_API( Player, get_opensubsystem)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(ossf);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_BOOL(false);
		R_END;
	}
	R_BEGIN;
	R_BOOL(player->getOpenSubSystemFlag((OpenSubSystemFlag)ossf));
	R_END;
}

GAME_SCRIPT_API( Player, add_questcounter)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(questid);
	P_END;
	
	Player *player = handle->asPlayer();
	if(NULL == player){
		R_NONE;
	}

	player->addQuestCounter(questid);

	R_NONE;
}

GAME_SCRIPT_API( Player, reduce_questcounter)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(questid);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player){
		R_NONE;
	}

	player->reduceQuestCounter(questid);

	R_NONE;
}

GAME_SCRIPT_API( Player, set_guide)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(guide);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_NONE;
	}

	player->guideIdx_ |= (1<<(U64)guide);
	//CALL_CLIENT(player,initGuide(player->guideIdx_));
	R_NONE;
}

GAME_SCRIPT_API( Player, set_guide_all){
	P_BEGIN;
	P_PTR(handle);
	P_INT(guide);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_NONE;
	}

	player->guideIdx_ = guide;
	//CALL_CLIENT(player,initGuide(player->guideIdx_));
	R_NONE;
}

GAME_SCRIPT_API( Player, get_guide)
{
	P_BEGIN;
	P_PTR(handle);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}
	R_BEGIN;
	R_INT(player->guideIdx_);
	R_END;
}

GAME_SCRIPT_API( Player, getPlayerInstId)
{
	P_BEGIN;
	P_PTR(handle);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}
	R_BEGIN;
	R_INT(player->getGUID());
	R_END;
}

GAME_SCRIPT_API( Player, get_BattleBaby)
{
	P_BEGIN;
	P_PTR(handle);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	if(player->getBattleBaby() == NULL)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	R_BEGIN;
	R_INT(player->getBattleBaby()->getGUID());
	R_END;
}

GAME_SCRIPT_API( Player, intensifybaby2target)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(babyid);
	P_INT(target);
	P_END;
	Player *player = handle->asPlayer();
	if(NULL == player){
		R_NONE;
	}
	Baby* baby = player->findBaby(babyid);
	if(baby == NULL)
		R_NONE;
	baby->intensify(target);
	R_NONE;
}

GAME_SCRIPT_API( Player, getbabyintensifylevel)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(babyid);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	Baby* baby = player->findBaby(babyid);
	if(NULL == baby)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	R_BEGIN;
	R_INT(baby->intensifyLevel_);
	R_END;
}

GAME_SCRIPT_API(Player, openvip){
	P_BEGIN;
	P_PTR(handle);
	P_INT(level);
	P_INT(time);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_NONE;
	}
	if(level >= VL_Max || level <= VL_None)
		R_NONE;
	player->setProp(PT_VipLevel,level);
	player->setProp(PT_VipTime,time);
	WorldServ::instance()->updateContactInfo(player);
	R_NONE;
}

GAME_SCRIPT_API(Player,getviptime){
	P_BEGIN;
	P_PTR(handle);
	P_END;
	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}
	R_BEGIN;
	R_INT(player->getProp(PT_VipTime));
	R_END;
}

GAME_SCRIPT_API( Player, send_errorno){
	P_BEGIN;
	P_PTR(handle);
	P_INT(eno);
	P_END;
	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_NONE;
	}
	CALL_CLIENT(player,errorno((ErrorNo)eno));
	R_NONE;
}

GAME_SCRIPT_API( Player, get_bag_free_slot){
	P_BEGIN;
	P_PTR(handle);
	P_END;
	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}
	R_BEGIN;
	R_INT(player->getBagEmptySlot());
	R_END;
}

GAME_SCRIPT_API( Player, get_EquipItem)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(slot);
	P_END;
	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	COM_Item* equip = player->getPlayerEquipBySlot((EquipmentSlot)slot);
	if(equip)
	{
		R_BEGIN;
		R_INT(equip->itemId_);
		R_END;
	}

	R_BEGIN;
	R_INT(0);
	R_END;
}

GAME_SCRIPT_API( Player, getskillName)
{
	P_BEGIN;
	P_INT(skId);
	P_INT(skLv);
	P_END;
	SkillTable::Core const *pCore = SkillTable::getSkillById(skId,skLv);
	if(NULL == pCore)
	{
		R_BEGIN;
		R_STR("");
		R_END;
	}
	R_BEGIN;
	R_STR(pCore->skillName_.c_str());
	R_END;
}
GAME_SCRIPT_API( Player, getplayerName)
{
	P_BEGIN;
	P_PTR(handle);
	P_END;
	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_STR("");
		R_END;
	}
	R_BEGIN;
	R_STR(player->getNameC());
	R_END;
}

GAME_SCRIPT_API( Player, getbabyName)
{
	P_BEGIN;
	P_INT(tableid);
	P_END;
	MonsterTable::MonsterData const * tmp = MonsterTable::getMonsterById(tableid);
	if(NULL == tmp)
	{
		R_BEGIN;
		R_STR("");
		R_END;
	}
	R_BEGIN;
	R_STR(tmp->name_.c_str());
	R_END;
}
GAME_SCRIPT_API( Player, getemployeeName)
{
	P_BEGIN;
	P_INT(tableid);
	P_END;
	EmployeeTable::EmployeeData const * tmp = EmployeeTable::getEmployeeById(tableid);
	if(NULL == tmp)
	{
		R_BEGIN;
		R_STR("");
		R_END;
	}
	
	R_BEGIN;
	R_STR(tmp->name_.c_str());
	R_END;
}

GAME_SCRIPT_API( Player, startonlinetime)
{
	P_BEGIN;
	P_PTR(handle);
	P_END;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE;
	player->onlinetimeflag_ = true;
	CALL_CLIENT(player,startOnlineTime());
	R_NONE;
}

GAME_SCRIPT_API( Player, stoponlinetime)
{
	P_BEGIN;
	P_PTR(handle);
	P_END;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE;
	player->onlinetimeflag_ = false;
	R_NONE;
}

GAME_SCRIPT_API( Player, sevenopen)
{
	P_BEGIN;
	P_PTR(handle);
	P_END;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE;
	player->sevenflag_ = true;
	CALL_CLIENT(player,agencyActivity(ADT_7Days, player->sevenflag_));
	R_NONE;
}

GAME_SCRIPT_API( Player,sevenclose)
{
	P_BEGIN;
	P_PTR(handle);
	P_END;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE;
	player->sevendayClose();
	R_NONE;
}

GAME_SCRIPT_API( Player, openCystal)
{
	P_BEGIN;
	P_PTR(handle);
	P_END;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE;
	player->initCrystal();
	R_NONE;
}

//--------------------------------------------------------------------------------------------------------------------

GAME_SCRIPT_API( Entity, get_skills){
	P_BEGIN;
	P_PTR(handle);
	P_END;

	Entity *e = handle->asEntity();
	
	std::vector<S32> arr;
	if(NULL == e)
	{
		R_BEGIN;
		R_ARR(arr);
		R_END;
	}
	arr = e->getSkillIds();

	R_BEGIN;
	R_ARR(arr);
	R_END;
}

GAME_SCRIPT_API( Entity, check_state){
	P_BEGIN;
	P_PTR(handle);
	P_INT(stateId);
	Entity *e = handle->asEntity();

	if(NULL == e)
	{
		R_BEGIN;
		R_BOOL(false);
		R_END;
	}

	for(size_t i=0; i<e->states_.size(); ++i)
	{
		if(e->states_[i].stateId_ == stateId){
			R_BEGIN;
			R_BOOL(true);
			R_END;
		}
	}

	R_BEGIN;
	R_BOOL(false);
	R_END;
}
GAME_SCRIPT_API( Entity, remove_state){
	P_BEGIN;
	P_PTR(handle);
	P_INT(stateId);
	Entity *e = handle->asEntity();

	if(NULL == e)
	{
		R_NONE;
	}

	e->removeState(stateId);

	R_NONE;
}

GAME_SCRIPT_API(Entity, insert_state){
	P_BEGIN;
	P_PTR(handle);
	P_INT(stateId);
	P_INT(value0);
	P_INT(value1);
	P_END;

	Entity *e = handle->asEntity();
	
	if(NULL == e)
		R_NONE;

	e->insertState(stateId,value0,value1);

	R_NONE;

}

GAME_SCRIPT_API(Entity, set_state_value){
	P_BEGIN;
	P_PTR(handle);
	P_INT(stateId);
	P_INT(value0);
	P_INT(value1);
	P_END;

	Entity *e = handle->asEntity();

	if(NULL == e)
		R_NONE;

	e->setStateValue(stateId,value0,value1);

	R_NONE;
}

GAME_SCRIPT_API( Entity, get_state_value){
	P_BEGIN;
	P_PTR(handle);
	P_INT(stateId);
	P_END;

	S32 v0=0,v1=0;
	Entity *e = handle->asEntity();
	if(NULL == e)
		R_NONE;

	e->getStateValue(stateId,v0,v1);

	R_BEGIN;
	R_INT(v0);
	R_INT(v1);
	R_END;
}
GAME_SCRIPT_API( Entity, getWeapon){
	P_BEGIN;
	P_PTR(handle);
	P_END;
	S32 weaponId = 0;
	WeaponType weaponType = WT_None;

	Entity *e = handle->asEntity();
	if(NULL == e)
	{
		R_BEGIN;
		R_INT(weaponId);
		R_INT(weaponType);
		R_END;
	}
	
	e->getWeapon(weaponId,weaponType);
	R_BEGIN;
	R_INT(weaponId);
	R_INT(weaponType);
	R_END;
}

GAME_SCRIPT_API( Entity, getType){
	P_BEGIN;
	P_PTR(handle);
	P_END;

	
	if(!handle){
		R_BEGIN;
		R_INT(ET_None);
		R_END;
	}
	else if(handle->asBaby()){
		R_BEGIN;
		R_INT(ET_Baby);
		R_END;
	}
	else if(handle->asEmployee()){
		R_BEGIN;
		R_INT(ET_Emplyee);
		R_END;
	}
	else if(handle->asPlayer()){
		R_BEGIN;
		R_INT(ET_Player);
		R_END;
	}
	else
	{
		R_BEGIN;
		R_INT(ET_None);
		R_END;
	}
	
}

//---------------------------------------------------------------------------------------------------------------

GAME_SCRIPT_API( Battle, get_BattleType)
{
	P_BEGIN;
	P_INT(battleId);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("pBattle is nil , battle id=%d\n"),battleId));
		R_BEGIN;
		R_FLOAT(0);
		R_END;
	}

	R_BEGIN;
	R_INT(pBattle->battleType_);
	R_END;
}

GAME_SCRIPT_API( Battle, get_prop)
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(position);
	P_INT(type);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("pBattle is nil , battle id=%d\n"),battleId));
		R_BEGIN;
		R_FLOAT(0);
		R_END;
	}

	Entity *pEntity = pBattle->findEntityByPos(position);
	if(NULL == pEntity)
	{
		//ACE_DEBUG((LM_ERROR,ACE_TEXT("Entity is nil , position is %d\n"),position));
		R_BEGIN;
		R_FLOAT(0);
		R_END;
	}
	R_BEGIN;
	R_FLOAT(pEntity->getProp((PropertyType)type));
	R_END;
}

GAME_SCRIPT_API( Battle, getMaxMpPos)
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(force);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("pBattle is nil , battle id=%d\n"),battleId));
		R_BEGIN;
		R_FLOAT(0);
		R_END;
	}

	R_BEGIN;
	R_INT(pBattle->findMaxMpPos((GroupType)force));
	R_END;
}

GAME_SCRIPT_API( Battle, getMinHpPos)
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(force);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("pBattle is nil , battle id=%d\n"),battleId));
		R_BEGIN;
		R_FLOAT(0);
		R_END;
	}

	R_BEGIN;
	R_INT(pBattle->findMinHpPos((GroupType)force));
	R_END;
}

GAME_SCRIPT_API( Battle, getMaxHpPos)
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(force);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("pBattle is nil , battle id=%d\n"),battleId));
		R_BEGIN;
		R_FLOAT(0);
		R_END;
	}

	R_BEGIN;
	R_INT(pBattle->findMaxHpPos((GroupType)force));
	R_END;
}

GAME_SCRIPT_API( Battle, getBossPos)
{
	P_BEGIN;
	P_INT(battleId);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("pBattle is nil , battle id=%d\n"),battleId));
		R_BEGIN;
		R_FLOAT(0);
		R_END;
	}

	R_BEGIN;
	R_INT(pBattle->findBossPos());
	R_END;
}


GAME_SCRIPT_API( Battle, change_prop)
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(position);
	P_INT(type);
	P_FLOAT(val);
	P_BOOL(bao);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
		R_NONE ;

	if(val > 0 && val < 1)
		val = 1;
	else if(val < 0 && val > -1)
		val = -1;

	pBattle->changeProp(position,(PropertyType)type,val,bao);

	R_NONE;
}

GAME_SCRIPT_API( Battle, changeProp_state)
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(position);
	P_INT(type);
	P_FLOAT(val);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
		R_NONE ;

	pBattle->changePropBystate(position,(PropertyType)type,val);
	//ACE_DEBUG((LM_INFO,"GAME-SCRIPT-API ===> %f\n",val));
	R_NONE;
}

GAME_SCRIPT_API( Battle, changeProp_fanji)
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(position);
	P_INT(type);
	P_FLOAT(val);
	P_BOOL(bao);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
		R_NONE ;

	pBattle->changePropByfanji(position,(PropertyType)type,val,bao);

	R_NONE;
}


GAME_SCRIPT_API( Battle, insert_state)
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(targetPos);
	P_INT(stateId);
	P_INT(isAction);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
		R_NONE ;
	
	pBattle->insertState(targetPos,stateId,isAction);

	R_NONE;
}
GAME_SCRIPT_API( Battle, check_state)
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(targetPos);
	P_INT(type);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		R_BEGIN;
		R_BOOL(false);
		R_END;
	}

	bool b = pBattle->checkState(targetPos,type);

	R_BEGIN;
	R_BOOL(b);
	R_END;

}
GAME_SCRIPT_API( Battle, cutTime_state)
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(targetPos);
	P_INT(statetype);
	P_INT(cutNum);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
		R_NONE ;

	pBattle->cuttickState((StateType)statetype,(BattlePosition)targetPos,cutNum);

	R_NONE;
}
GAME_SCRIPT_API( Battle, clear_state)
{
	P_BEGIN;
	P_INT(battleId);
	P_PTR(caster);
	P_INT(targetPos);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
		R_NONE ;

	if(NULL == caster)
		R_NONE;

	Entity *pEntity = caster->asEntity();
	if(NULL == pEntity)
		R_NONE;

	pBattle->clearState(targetPos);

	R_NONE;
}

GAME_SCRIPT_API( Battle, remove_state )
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(targetPos);
	P_INT(type);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		R_NONE;
	}

	pBattle->removeState(targetPos,type);

	R_NONE;
}


GAME_SCRIPT_API( Battle, change_position)
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(srcPos);
	P_INT(destPos);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
		R_NONE ;

	pBattle->changePosition((BattlePosition)srcPos,(BattlePosition)destPos);

	R_NONE;
}

GAME_SCRIPT_API( Battle, is_battle_baby)
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(babyInstId);
	P_END;
	
	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		R_BEGIN;
		R_BOOL(false);
		R_END;
	}

	bool res = pBattle->isBattleBaby(babyInstId);

	R_BEGIN;
	R_BOOL(res);
	R_END;

}

GAME_SCRIPT_API( Battle, isBabybyPos )
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(pos);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		R_BEGIN;
		R_BOOL(false);
		R_END;
	}

	bool res = false;
	Entity* pEnt = pBattle->findEntityByPos(pos);
	if(pEnt && pEnt->asBaby() && !pEnt->isDeadth())
		res = true;
	R_BEGIN;
	R_BOOL(res);
	R_END;
}

GAME_SCRIPT_API( Battle, select_baby)
{
	P_BEGIN;
	P_INT(battleId);
	P_PTR(handle);
	P_INT(babyId);
	P_BOOL(isBattle);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
		R_NONE ;
	
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE ;

	pBattle->selectBaby(player,babyId,isBattle);
	
	R_NONE;
}

GAME_SCRIPT_API( Battle, get_Force)
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(position);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	Entity *pEntity = pBattle->findEntityByPos(position);
	if(NULL == pEntity)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	R_BEGIN;
	R_INT(pEntity->getForce());
	R_END;
}

GAME_SCRIPT_API( Battle, run_away)
{
	P_BEGIN;
	P_INT(battleId);
	P_PTR(handle);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
		R_NONE ;
	
	if(NULL == handle)
		R_NONE;
	
	Player *pPlayer = handle->asPlayer();

	if(NULL == pPlayer)
		R_NONE;

	pBattle->flee(pPlayer);

	R_NONE;
}

GAME_SCRIPT_API( Battle, get_runawayNum)
{
	P_BEGIN;
	P_INT(battleId);
	P_PTR(handle);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	if(NULL == handle)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	Player *pPlayer = handle->asPlayer();

	if(NULL == pPlayer)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	R_BEGIN;
	R_INT(pPlayer->battleRunawayNum_);
	R_END;
}

GAME_SCRIPT_API( Battle, set_runawayNum)
{
	P_BEGIN;
	P_INT(battleId);
	P_PTR(handle);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	if(NULL == handle)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	Player *pPlayer = handle->asPlayer();

	if(NULL == pPlayer)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	pPlayer->battleRunawayNum_ += 1;

	R_NONE;
}

GAME_SCRIPT_API( Battle, add_Counter)
{
	P_BEGIN;
	P_INT(casterPos);
	P_INT(battleId);
	P_INT(targetPos);
	P_INT(type);
	P_FLOAT(val);
	P_BOOL(bao);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
		R_NONE ;

	Entity* pEntity =pBattle->findEntityByPos(casterPos);

	if(NULL == pEntity)
		R_NONE;

	pBattle->addActionCounter(pEntity->getGUID(), targetPos,(PropertyType)type,val,bao);

	R_NONE;
}
GAME_SCRIPT_API( Battle, change_report_skill)
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(curPos);
	P_INT(skillId);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
		R_NONE ;

	pBattle->changeReportSkillId(skillId,curPos);
	
	R_NONE;
}

GAME_SCRIPT_API( Battle, change_order_skill){
	P_BEGIN;
	P_INT(battleId);
	P_INT(curPos);
	P_INT(skillId);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
		R_NONE ;

	Entity* pEntity =pBattle->findEntityByPos(curPos);

	if(NULL == pEntity)
		R_NONE;

	pBattle->changeOrderSkillId(skillId);

	R_NONE;
}

GAME_SCRIPT_API( Battle, change_order_target){
	P_BEGIN;
	P_INT(battleId);
	P_INT(curPos);
	P_INT(pos);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
		R_NONE ;

	Entity* pEntity = pBattle->findEntityByPos(curPos);

	if(NULL == pEntity)
		R_NONE;

	pBattle->changeOrderTarget((int)pos);
	R_NONE;
}

GAME_SCRIPT_API( Battle, check_Order)
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(targetPos);
	P_INT(skillId);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		R_BEGIN;
		R_BOOL(false);
		R_END;
	}

	bool isOrder = false;
	isOrder = pBattle->findOrder((BattlePosition)targetPos,skillId);
	R_BEGIN;
	R_BOOL(isOrder);
	R_END;
	
	R_NONE;
}

GAME_SCRIPT_API( Battle, set_huwei)
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(pos1);
	P_INT(pos2);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)R_NONE ;
	Entity* pEntity =pBattle->findEntityByPos(pos1);
	if(NULL == pEntity)R_NONE;
	pEntity  =pBattle->findEntityByPos(pos2);
	if(NULL == pEntity)R_NONE;
	
	pBattle->addHuwei((BattlePosition)pos1,(BattlePosition)pos2);

	R_NONE;
}

GAME_SCRIPT_API( Battle, getItemType_BySlot)
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(castPos);
	P_INT(slot);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		R_BEGIN;
		R_FLOAT(0);
		R_END;
	}
	Entity* pEntity =pBattle->findEntityByPos(castPos);
	if(NULL == pEntity)
	{
		R_BEGIN;
		R_FLOAT(0);
		R_END;
	}

	Player* pPlayer = pEntity->asPlayer();

	if (pPlayer)
	{
		ItemTable::ItemData const *pItem =  pPlayer->getEquipmentItemData((EquipmentSlot)slot);

		if(pItem == NULL || pItem->weaponType_ == -1)
		{
			R_BEGIN;
			R_FLOAT(0);
			R_END;
		}

		R_BEGIN;
		R_INT(pItem->weaponType_);
		R_END;
	}

	Employee* pEmp = pEntity->asEmployee();
	
	if (pEmp)
	{
		ItemTable::ItemData const *pItem =  pEmp->getWearEquipData((EquipmentSlot)slot);

		if(pItem == NULL || pItem->weaponType_ == -1)
		{
			R_BEGIN;
			R_FLOAT(0);
			R_END;
		}

		R_BEGIN;
		R_INT(pItem->weaponType_);
		R_END;
	}

	Robot* pRobot = pEntity->asRobot();

	if (pRobot)
	{
		ItemTable::ItemData const *pItem =  pRobot->getWearEquipData((EquipmentSlot)slot);

		if(pItem == NULL || pItem->weaponType_ == -1)
		{
			R_BEGIN;
			R_FLOAT(0);
			R_END;
		}

		R_BEGIN;
		R_INT(pItem->weaponType_);
		R_END;
	}
	
	R_BEGIN;
	R_INT(0);
	R_END;
}

///AI
GAME_SCRIPT_API( Battle, ai_pushOrder )
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(targetPos);
	P_INT(skill);
	P_END;

	if(NULL == handle)
	{
		R_BEGIN;
		R_FLOAT(0);
		R_END;
	}

	Employee* pEmp = handle->asEmployee();

	if (pEmp)
	{
		COM_Order	order;
		order.casterId_ = pEmp->getGUID();
		order.target_ = targetPos;
		order.skill_ = skill;

		Battle * pBattle = Battle::find(pEmp->battleId_);

		if(NULL == pBattle)
		{
			R_BEGIN;
			R_FLOAT(0);
			R_END;
		}

		pBattle->pushOrderByAI(order);
	}

	Monster* pMonster = handle->asMonster();

	if (pMonster)
	{
		COM_Order	order;
		order.casterId_ = pMonster->getGUID();
		order.target_ = targetPos;
		order.skill_ = skill;

		Battle * pBattle = Battle::find(pMonster->battleId_);

		if(NULL == pBattle)
		{
			R_BEGIN;
			R_FLOAT(0);
			R_END;
		}

		pBattle->pushOrderByAI(order);
	}

	InnerPlayer* pRobot = handle->asInnerPlayer();

	if (pRobot)
	{
		COM_Order	order;
		order.casterId_ = pRobot->getGUID();
		order.target_ = targetPos;
		order.skill_ = skill;

		Battle * pBattle = Battle::find(pRobot->battleId_);

		if(NULL == pBattle)
		{
			R_BEGIN;
			R_FLOAT(0);
			R_END;
		}

		pBattle->pushOrderByAI(order);
	}

	Baby* pBaby = handle->asBaby();

	if (pBaby)
	{
		COM_Order	order;
		order.casterId_ = pBaby->getGUID();
		order.target_ = targetPos;
		order.skill_ = skill;

		Battle * pBattle = Battle::find(pBaby->battleId_);

		if(NULL == pBattle)
		{
			R_BEGIN;
			R_FLOAT(0);
			R_END;
		}

		pBattle->pushOrderByAI(order);
	}

	R_NONE;
}

GAME_SCRIPT_API( Battle, getLevel_State)
{
	P_BEGIN;
	P_INT(battleId)
	P_INT(type);
	P_INT(pos);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	U32 level = pBattle->getStateLevel((StateType)type, (BattlePosition)pos);

	R_BEGIN;
	R_INT(level);
	R_END;
}

GAME_SCRIPT_API( Battle, getSkill_TargetType)
{
	P_BEGIN;
	P_INT(battleId)
	P_INT(skillId);
	P_END;

	SkillTable::Core const * pCore = SkillTable::getSkillById(skillId,1);

	if(NULL == pCore)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	R_BEGIN;
	R_INT(pCore->targetType_);
	R_END;
}

GAME_SCRIPT_API( Battle, get_skill_melee){
	P_BEGIN;
	P_INT(skillId);
	P_END;

	SkillTable::Core const * pCore = SkillTable::getSkillById(skillId,1);

	if(NULL == pCore)
	{
		R_BEGIN;
		R_BOOL(false);
		R_END;
	}

	R_BEGIN;
	R_BOOL(!!(pCore->isMelee_));
	R_END;
}

GAME_SCRIPT_API( Battle, current_order_skill){
	P_BEGIN;
	P_INT(battleId)
	P_END;
	
	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	COM_Order* pOrder = pBattle->getCurrentOrder();
	if(NULL == pOrder)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	R_BEGIN;
	R_INT(pOrder->skill_);
	R_END;
}

GAME_SCRIPT_API( Battle, zhuachong)
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(pos);
	P_PTR(handle);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}
	
	pBattle->catchBaby(pos,handle->asEntity());
	R_NONE;
}

GAME_SCRIPT_API( Battle, getSneakAttack)
{

	P_BEGIN;
	P_INT(battleId);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		R_BEGIN;
		R_INT(SAT_None);
		R_END;
	}

	R_BEGIN;
	R_INT(pBattle->sneakattack_);
	R_END;
}


GAME_SCRIPT_API( Battle, getRound)
{
	P_BEGIN;
	P_INT(battleId);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	R_BEGIN;
	R_INT(pBattle->roundCount_);
	R_END;
}

GAME_SCRIPT_API( Battle, getBattleDataID)
{
	P_BEGIN;
	P_INT(battleId);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}
	R_BEGIN;
	R_INT(pBattle->battleDataId_);
	R_END;
}

GAME_SCRIPT_API( Battle, CheckHp)
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(battleForce);
	P_INT(value);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		R_BEGIN;
		R_BOOL(false);
		R_END;
	}
	R_BEGIN;
	R_BOOL(pBattle->checkForceHp((GroupType)battleForce,value));
	R_END;
}

GAME_SCRIPT_API( Battle, add_Monster )
{
	std::vector<std::string> monsterClass;
	P_BEGIN;
	P_INT(battleId);
	P_ARR(monsterClass);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		R_BEGIN;
		R_BOOL(false);
		R_END;
	}

	pBattle->addMonster(monsterClass);
		
	R_NONE;
}

GAME_SCRIPT_API( Battle, getMonsterTypebyPos)
{
	P_BEGIN;
	P_INT(battleId);
	P_INT(pos);
	P_END;

	Battle * pBattle = Battle::find(battleId);
	if(NULL == pBattle)
	{
		R_BEGIN;
		R_INT(SAT_None);
		R_END;
	}

	R_BEGIN;
	R_INT(pBattle->getMonsterType((BattlePosition)pos));
	R_END;
}

//--------------------------------------------

GAME_SCRIPT_API( Activity, update)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(actid);
	P_INT(counter);
	P_END;

	if(NULL == handle)
		R_NONE;
	
	Player* player = handle->asPlayer();
	if(NULL == player)
		R_NONE;
	
	if(player->isTeamMember()){
		Team* p = player->isTeamLeader();
		if(p)
		{
			p->teamAddActivation((ActivityType)actid,counter);
		}
		
	}else {
		player->addActivation((ActivityType)actid,counter);
	}


	R_NONE;
}
GAME_SCRIPT_API( Sys, init_dynamic_npc){
	P_BEGIN;
	P_INT(type);
	P_INT(size);
	P_END;
	if(type <=NT_Normal ||type >= NT_Max || size <=0 )
		R_NONE;
	SceneHandler::instance()->initDynamicNpcs((NpcType)type,size);
	R_NONE;
}
GAME_SCRIPT_API( Sys, refresh_dynamic_npc){
	P_BEGIN;
	P_INT(type);
	P_INT(size);
	P_END;
	if(type <=NT_Normal ||type >= NT_Max || size <=0 )
		R_NONE;
	SceneHandler::instance()->refreshDynamicNpcs((NpcType)type,size);
	R_NONE;

}
GAME_SCRIPT_API( Sys, close_dynamic_npc){
	P_BEGIN;
	P_INT(type);
	P_END;
	if(type <=NT_Normal ||type >= NT_Max )
		R_NONE;
	
	SceneHandler::instance()->finiDynamicNpcs((NpcType)type);
	R_NONE;
}

GAME_SCRIPT_API(Sys,openmushroom){
	P_BEGIN;
	P_INT(counter);
	P_END;
	DayliActivity::mushroomOpen(counter);
	R_NONE;
}

GAME_SCRIPT_API( Sys, refreshmushroom){
	P_BEGIN;
	P_INT(counter);
	P_END;
	DayliActivity::mushroomRefresh(counter);
	R_NONE;
}

GAME_SCRIPT_API(Sys,closemushroom){

	DayliActivity::mushroomClose();
	R_NONE;
}


GAME_SCRIPT_API(Sys,openxiji){
	P_BEGIN;
	P_INT(counter);
	P_END;
	DayliActivity::xijiOpen(counter);
	R_NONE;
}

GAME_SCRIPT_API( Sys, refreshxiji){
	P_BEGIN;
	P_INT(counter);
	P_END;
	DayliActivity::xijiRefresh(counter);
	R_NONE;
}

GAME_SCRIPT_API(Sys,closexiji){

	DayliActivity::xijiClose();
	R_NONE;
}

GAME_SCRIPT_API(Sys,openalonepk){
	P_BEGIN;
	P_INT(counter);
	P_END;
	DayliActivity::alonepkOpen(counter);
	R_NONE;
}

GAME_SCRIPT_API( Sys, refreshalonepk)
{
	P_BEGIN;
	P_INT(counter);
	P_END;
	DayliActivity::alonepkRefresh(counter);
	R_NONE;
}

GAME_SCRIPT_API(Sys,closealonepk){

	DayliActivity::alonepkClose();
	R_NONE;
}

GAME_SCRIPT_API(Sys,openteampk){
	P_BEGIN;
	P_INT(counter);
	P_END;
	DayliActivity::teampkOpen(counter);
	R_NONE;
}

GAME_SCRIPT_API( Sys, refreshteampk)
{
	P_BEGIN;
	P_INT(counter);
	P_END;
	DayliActivity::teampkRefresh(counter);
	R_NONE;
}

GAME_SCRIPT_API(Sys,closeteampk){

	DayliActivity::teampkClose();
	R_NONE;
}

GAME_SCRIPT_API(Sys,openExam)
{
	DayliActivity::examOpen();
	R_NONE;
}

GAME_SCRIPT_API(Sys,closeExam)
{
	DayliActivity::examClose();
	R_NONE;
}

GAME_SCRIPT_API(Sys,openWarrior)
{
	DayliActivity::warriorOpen();
	R_NONE;
}

GAME_SCRIPT_API(Sys,closeWarrior)
{
	DayliActivity::warriorClose();
	R_NONE;
}

GAME_SCRIPT_API(Sys,openpet)
{
	DayliActivity::petbattleopen();
	R_NONE;
}

GAME_SCRIPT_API(Sys,closepet)
{
	DayliActivity::petbattleclose();
	R_NONE;
}

//GAME_SCRIPT_API( Sys, reset_minetimes){
//	Player::resetPlayerMineTimes();
//	R_NONE;
//}

GAME_SCRIPT_API(Player, isteamleader){
	P_BEGIN;
	P_PTR(handle);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	if(player->isTeamLeader()){
		R_BEGIN;
		R_INT(player->teamId_);
		R_END;
	}
	else{
		R_BEGIN;
		R_INT(0);
		R_END;
	}
}

GAME_SCRIPT_API(Team, get_teammembers){
	P_BEGIN;
	P_INT(teamid);
	P_END;

	R_BEGIN;
	Team* p = TeamLobby::instance()->getTeam(teamid);
	if(p){
		for (int i=0; i<5; ++i){
			if(i<p->teamMembers_.size() && !(p->teamMembers_[i]->isLeavingTeam_)){
				R_INT(p->teamMembers_[i]->handleId_);
			}else{
				R_INT(0);
			}
		}
	}
	else{
		for (int i=0; i<5; ++i){
			R_INT(0);
		}
	}
	R_END;
}

GAME_SCRIPT_API(Team, get_level){
	P_BEGIN;
	P_INT(teamid);
	P_END;

	Team* p = TeamLobby::instance()->getTeam(teamid);
	if(NULL == p){
		R_BEGIN;
		R_INT(0);
		R_END;
	}
	else{
		if(p->teamMembers_.empty()){
			R_BEGIN;
			R_INT(0);
			R_END;
		}
		S32 level = 0;
		for (int i=0; i<p->teamMembers_.size(); ++i){
			level += p->teamMembers_[i]->getProp(PT_Level);
		}
		level /= p->teamMembers_.size();
		R_BEGIN;
		R_INT(level);
		R_END;
	}
}

GAME_SCRIPT_API( Player, checkteamLevel)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(level);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_BOOL(false);
		R_END;
	}

	Team * p = player->myTeam();
	if(p == NULL)
	{
		R_BEGIN;
		R_BOOL(false);
		R_END;
	}

	for (size_t i = 0; i < p->teamMembers_.size(); ++i)
	{
		if(p->teamMembers_[i]->getProp(PT_Level) < level)
		{
			R_BEGIN;
			R_BOOL(false);
			R_END;
		}
	}

	R_BEGIN;
	R_BOOL(true);
	R_END;
}

GAME_SCRIPT_API(Sys, refreshTongjiTimes){
	
	Player::cleanActivationAll(ACT_Tongji);
	R_NONE;
}

GAME_SCRIPT_API( Player, add_activation_counter){
	P_BEGIN;
	P_PTR(handle);
	P_INT(type);
	P_END;
	Player *player = handle->asPlayer();
	if(NULL == player){
		R_NONE;
	}

	if(type <= ACT_None || type >= ACT_Max){
		R_NONE;
	}
	
	Team* pteam = player->isTeamLeader();
	if(pteam)
		pteam->teamAddActivation((ActivityType)type,1);
	else
		player->addActivation((ActivityType)type,1);
	R_NONE;
}

GAME_SCRIPT_API( Sys, add_activation_counter_all){
	P_BEGIN;
	P_INT(type);
	P_INT(counter);
	P_END;
	
	for(size_t i=0; i<Player::store_.size(); ++i){
		Player::store_[i]->addActivation((ActivityType)type,counter);
	}
	R_NONE;
}

GAME_SCRIPT_API( Player, get_activation_counter){
	P_BEGIN;				
	P_PTR(handle);
	P_INT(type);
	P_END;
	Player *player = handle->asPlayer();
	if(NULL == player){
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	if(type <= ACT_None || type >= ACT_Max){
		R_BEGIN;
		R_INT(0);
		R_END;
	}
	R_BEGIN;
	R_INT(player->getActivitionCount((ActivityType)type));
	R_END;
	
}

GAME_SCRIPT_API(Player, send_mail){
	P_BEGIN;
	P_PTR(handle);
	P_STR(recvName);
	P_STR(title);
	P_STR(content);
	P_END;

	Player* player = handle->asPlayer();
	if(NULL == player)
		R_NONE;
	player->sendMail(recvName,title,content);

	R_NONE;
}

GAME_SCRIPT_API( Player, openbaggrid)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(itemid);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE;

	player->openBagGrid(itemid);
	R_NONE;
}

GAME_SCRIPT_API( Player, getCurBagGirdNum)
{
	P_BEGIN;
	P_PTR(handle);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player){
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	R_BEGIN;
	R_INT(player->getProp(PT_BagNum));
	R_END;
}

GAME_SCRIPT_API( Player, openstoragegrid)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(type);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE;
	
	player->openGrid((StorageType)type);
	R_NONE;
}

GAME_SCRIPT_API( Player, isstorageFull)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(type);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE;

	bool isfull = false;

	if((StorageType)type == ST_Item)
	{
		if(player->findItemStorageFirstemptySlot() == -1)
			isfull = true;
	}
	else if ((StorageType)type == ST_Baby)
	{
		if(player->findBabyStorageFirstemptySlot() == -1)
			isfull = true;
	}

	R_BEGIN;
	R_BOOL(isfull);
	R_END;
}

GAME_SCRIPT_API( Player, getstoragesize)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(type);
	P_END;

	Player *player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}

	if(type == ST_Item)
	{
		R_BEGIN;
		R_INT(player->itemStorageSize_);
		R_END;
	}
	else if (type == ST_Baby)
	{
		R_BEGIN;
		R_INT(player->babyStorageSize_);
		R_END;
	}

	R_BEGIN;
	R_INT(0);
	R_END;
}

GAME_SCRIPT_API( Player, openscene){
	P_BEGIN;
	P_PTR(handle);
	P_INT(sceneid);
	P_END;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE;

	player->openScene(sceneid);
	R_NONE;
}

GAME_SCRIPT_API( Player, sendawardByDropId)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(dropId);
	P_END;
	Player *player = handle->asPlayer();
	if(NULL == player)
		R_NONE;

	player->giveDrop(dropId);
	R_NONE;
}

GAME_SCRIPT_API(Sys, send_mail){
	P_BEGIN;
	P_STR(sendName);
	P_STR(recvName);
	P_STR(title);
	P_STR(content);
	P_INT(money);
	P_INT(diam);
	P_STR(itemstr);
	P_END;

	std::vector<std::string> itemstrs = String::Split(itemstr,";");
	std::vector<COM_MailItem> items;
	for (size_t i=0; i<itemstrs.size(); ++i)
	{
		COM_MailItem item;
		std::vector<std::string> stritem = String::Split(itemstrs[i],",");
		item.itemId_ = atoi(stritem[0].c_str());
		item.itemStack_ = atoi(stritem[1].c_str());
		items.push_back(item);
	}

	WorldServ::instance()->sendMail(sendName,recvName,title,content,money,diam,items);
	R_NONE;
}

GAME_SCRIPT_API(Sys, send_mail_all){
	P_BEGIN;
	P_STR(sendName);
	P_STR(title);
	P_STR(content);
	P_INT(money);
	P_INT(diam);
	P_STR(itemstr);
	P_END;
	
	COM_Mail mail;
	mail.mailType_ = MT_System;
	mail.sendPlayerName_ = sendName;
	mail.title_ = title;
	mail.content_ = content;
	mail.money_ = money;
	mail.diamond_ = diam;
	std::vector<std::string> itemstrs = String::Split(itemstr,";");
	for (size_t i=0; i<itemstrs.size(); ++i)
	{
		COM_MailItem item;
		std::vector<std::string> stritem = String::Split(itemstrs[i],",");
		item.itemId_ = atoi(stritem[0].c_str());
		item.itemStack_ = atoi(stritem[1].c_str());
		mail.items_.push_back(item);
	}
	mail.timestamp_ = WorldServ::instance()->curTime_;

	WorldServ::instance()->sendMailAll(mail);
	R_NONE;
}

GAME_SCRIPT_API( Sys, send_mail_drop)
{
	P_BEGIN;
	P_STR(sendName);
	P_STR(recvName);
	P_STR(title);
	P_STR(content);
	P_INT(dropId);
	P_END;

	WorldServ::instance()->sendMailByDrop(sendName,recvName,title,content,dropId);
	R_NONE;
}

GAME_SCRIPT_API( Sys, send_mail_all_drop)
{
	P_BEGIN;
	P_STR(sendName);
	P_STR(title);
	P_STR(content);
	P_INT(dropId);
	P_END;

	WorldServ::instance()->sendMailByDropAll(sendName,title,content,dropId);
	R_NONE;
}

GAME_SCRIPT_API(Sys, gm_mail){
	P_BEGIN;
	P_INT(mailtype);
	P_INT(drop);
	P_END;

	std::string sendname = "ϵͳ";
	std::string title	 = "lalalala";
	std::string content	 = "lalalalalalalalala";

	switch(mailtype)
	{
	case 1:
		PvpJJC::rankrewardbyday(sendname,title,content);
		break;
	case 2:
		PvpJJC::rankrewardbysenson(sendname,title,content);
		break;
	case 3:
		WorldServ::instance()->sendMailByDropAll(sendname,title,content,drop);
		break;
	default:
		break;
	}

	R_NONE;
}

GAME_SCRIPT_API( Sys, reset_activity){
	//Activity::reset();
	R_NONE;
}

GAME_SCRIPT_API( Sys, notice){
	P_BEGIN;
	P_STR(contant);
	P_END;
	
	WorldServ::instance()->notice(contant, false);

	R_NONE;
}

GAME_SCRIPT_API( Sys, vipitem){
	P_BEGIN;
	P_STR(sendName);
	P_STR(title);
	P_STR(content);
	P_END;
	WorldServ::instance()->vipitemmaill(sendName,title,content);
	R_NONE;
}

GAME_SCRIPT_API( Sys, isfirstwin_pvr)
{
	P_BEGIN;
	P_INT(rank);
	P_END;

	R_BEGIN;
	R_BOOL(EndlessStair::instance()->isFirstWin(rank));
	R_END
}

GAME_SCRIPT_API( Sys, firstwinreward_pvr)
{
	P_BEGIN;
	P_STR(sendName);
	P_STR(title);
	P_STR(content);
	P_INT(rank);
	P_INT(dropid);
	P_END;

	EndlessStair::instance()->firstWinreward(sendName,title,content,rank,dropid);
	R_NONE;
}

GAME_SCRIPT_API( Sys, sendPVRrewardbytimes)
{
	P_BEGIN;
	P_STR(sendName);
	P_STR(title);
	P_STR(content);
	P_INT(rank);
	P_END;

	EndlessStair::instance()->rankrewardbytimes(sendName,title,content,rank);
	R_NONE;
}

GAME_SCRIPT_API( Sys, sendPVRrewardbyday)
{
	P_BEGIN;
	P_STR(sendName);
	P_STR(title);
	P_STR(content);
	P_END;

	EndlessStair::instance()->rankrewardbyday(sendName,title,content);
	R_NONE;
}

GAME_SCRIPT_API( Sys, sendPVRrewardbysenson)
{
	P_BEGIN;
	P_STR(sendName);
	P_STR(title);
	P_STR(content);
	P_END;

	EndlessStair::instance()->rankrewardbysenson(sendName,title,content);
	R_NONE;
}

GAME_SCRIPT_API( Sys, sendPVPrewardbyday)
{
	P_BEGIN;
	P_STR(sendName);
	P_STR(title);
	P_STR(content);
	P_END;

	PvpJJC::rankrewardbyday(sendName,title,content);
	R_NONE;
}

GAME_SCRIPT_API( Sys, sendPVPrewardbysenson)
{
	P_BEGIN;
	P_STR(sendName);
	P_STR(title);
	P_STR(content);
	P_END;

	PvpJJC::rankrewardbysenson(sendName,title,content);
	R_NONE;
}

GAME_SCRIPT_API( Sys, open_guild_battle){
	Guild::openBattle();
	R_NONE;
}
GAME_SCRIPT_API( Sys, start_guild_battle){
	Guild::startBattle();
	R_NONE;
}
GAME_SCRIPT_API( Sys, stop_guild_battle){
	Guild::stopBattle();
	R_NONE;
}
GAME_SCRIPT_API( Sys, close_guild_battle){
	Guild::closeBattle();
	DayliActivity::guildbattleclose();
	R_NONE;
}
GAME_SCRIPT_API( Sys, join_guild_battle_scene){
	P_BEGIN;
	P_PTR(handle);
	P_END;

	Player* player = handle->asPlayer();
	if(NULL == player)
		R_NONE;
	player->joinGuildBattleScene();
	R_NONE;
}

GAME_SCRIPT_API( Sys, talked_guild_progenitus){
	P_BEGIN;
	P_PTR(handle);
	P_INT(npcid);
	P_END;

	Player* player = handle->asPlayer();
	if(NULL == player)
		R_NONE;
	Guild::talked2Progenitus(player,npcid);
	R_NONE;
}

GAME_SCRIPT_API( Sys,prepare_guild_battle_timeout){
	Guild::prepareBattleTimeout();
	DayliActivity::guildbattleopen();
	R_NONE;
}

GAME_SCRIPT_API( Sys, get_guild_name){
	P_BEGIN;
	P_INT(guildId);
	P_END;

	Guild* pGuild = Guild::findGuildById(guildId);
	if(pGuild){
		R_BEGIN;
		R_STR(pGuild->guildData_.guildName_.c_str());
		R_END
	}
	R_BEGIN;
	R_STR("");
	R_END
}
GAME_SCRIPT_API( Sys, send_guild_mail){
	P_BEGIN;
	P_INT(guildId);
	P_STR(sender);
	P_STR(title);
	P_STR(content);
	P_INT(dia);
	P_INT(money);
	P_STR(stritems);
	P_END;

	Guild* pGuild = Guild::findGuildById(guildId);
	if(NULL == pGuild)
		R_NONE;
	
	if(sender.empty() || title.empty() || content.empty())
		R_NONE;
	
	std::vector<COM_MailItem> items;
	if(!stritems.empty()){
		std::vector<std::string> vecitems = String::Split(stritems,";");
		for(size_t i=0; i<vecitems.size(); ++i){
			std::vector<std::string> stritem = String::Split(vecitems[i],",");
			if(stritem.size() == 2){
				COM_MailItem mi;
				mi.itemId_ = atoi(stritem[0].c_str());
				mi.itemStack_ =  atoi(stritem[1].c_str());
				items.push_back(mi);
			}
		}
	}
	
	pGuild->sendMemberMail(sender,title,content,dia,money,items);
	R_NONE;
}

GAME_SCRIPT_API( Sys, del_guild_npc){
	P_BEGIN;
	P_PTR(handle);
	P_INT(npcId);
	P_END;

	if(Guild::battleState_ != Guild::BS_Battle)
		R_NONE;

	Player* player = handle->asPlayer();
	if(NULL == player)
	{
		R_NONE;
	}

	Guild* pGuild = player->myGuild();
	if(!pGuild)
		R_NONE;

	Scene* s = SceneManager::instance()->getScene(pGuild->battleSceneCopyId_);

	if(!s)
		R_NONE;
	
	s->delNpc(npcId);

	R_NONE;
}

GAME_SCRIPT_API( Sys, add_npc){
	P_BEGIN;
	P_INT(sceneId);
	P_INT(npcId);
	P_END;

	Scene* s = SceneManager::instance()->getScene(sceneId);
	if(s){
		std::vector<S32> npcs;
		npcs.push_back(npcId);
		s->addNpcs(npcs);
	}
	R_NONE;
}

GAME_SCRIPT_API( Sys, del_npc){
	P_BEGIN;
	P_INT(sceneId);
	P_INT(npcId);
	P_END;

	Scene* s = SceneManager::instance()->getScene(sceneId);
	if(s){
		s->delNpc(npcId);
	}
	R_NONE;
}

GAME_SCRIPT_API( Sys, transfor_scene){
	P_BEGIN;
	P_PTR(handle);
	P_INT(sceneId);
	P_END;

	Player* player = handle->asPlayer();
	if(!player)
		R_NONE;
	Scene* s = SceneManager::instance()->getScene(sceneId);
	if(!s)
		R_NONE;
	
	if(player->scenePlayer_)
		player->scenePlayer_->transforScene(sceneId);
	R_NONE;
}

GAME_SCRIPT_API( Sys, open_festival){
	P_BEGIN;
	P_INT(daytime);
	P_END;

	int64 opentime = WorldServ::instance()->curTime_ + 60;
	int64 closetime = opentime + ONE_DAY_SEC * daytime;

	Festival::open(opentime,closetime);
	R_NONE;
}
GAME_SCRIPT_API( Sys, close_festival){
	Festival::close();
	R_NONE;
}

GAME_SCRIPT_API( Sys, open_card){
	P_BEGIN;
	P_INT(daytime);
	P_END;

	int64 opentime = WorldServ::instance()->curTime_ + 60;
	int64 closetime = opentime + ONE_DAY_SEC * daytime;

	ReversalCard::open(opentime,closetime);
	R_NONE;
}

GAME_SCRIPT_API( Sys, close_card){
	ReversalCard::close();
	R_NONE;
}

GAME_SCRIPT_API( Sys, open_rechargeTotal){
	P_BEGIN;
	P_INT(daytime);
	P_END;

	int64 opentime = WorldServ::instance()->curTime_ + 60;
	int64 closetime = opentime + ONE_DAY_SEC * daytime;

	RechargeTotal::open(opentime,closetime);
	R_NONE;
}

GAME_SCRIPT_API( Sys, close_rechargeTotal){
	RechargeTotal::close();
	R_NONE;
}

GAME_SCRIPT_API( Sys, open_rechargeSingle){
	P_BEGIN;
	P_INT(daytime);
	P_END;

	int64 opentime = WorldServ::instance()->curTime_ + 60;
	int64 closetime = opentime + ONE_DAY_SEC * daytime;

	RechargeSingle::open(opentime,closetime);
	R_NONE;
}

GAME_SCRIPT_API( Sys, close_rechargeSingle){
	RechargeSingle::close();
	R_NONE;
}

GAME_SCRIPT_API( Sys, open_discountStore){
	P_BEGIN;
	P_INT(daytime);
	P_END;

	int64 opentime = WorldServ::instance()->curTime_ + 60;
	int64 closetime = opentime + ONE_DAY_SEC * daytime;

	DiscountStore::open(opentime,closetime);
	R_NONE;
}

GAME_SCRIPT_API( Sys, close_discountStore){
	DiscountStore::close();
	R_NONE;
}

GAME_SCRIPT_API( Sys, open_hotShop){
	P_BEGIN;
	P_INT(daytime);
	P_END;

	int64 opentime = WorldServ::instance()->curTime_ + 60;
	int64 closetime = opentime + ONE_DAY_SEC * daytime;

	HotShop::open(opentime,closetime);
	R_NONE;
}

GAME_SCRIPT_API( Sys, close_hotShop){
	HotShop::close();
	R_NONE;
}

GAME_SCRIPT_API( Sys, open_employeeActivityTotal){
	P_BEGIN;
	P_INT(daytime);
	P_END;

	int64 opentime = WorldServ::instance()->curTime_ + 60;
	int64 closetime = opentime + ONE_DAY_SEC * daytime;

	EmployeeActivityTotal::open(opentime,closetime);
	R_NONE;
}

GAME_SCRIPT_API( Sys, close_employeeActivityTotal){
	EmployeeActivityTotal::close();
	R_NONE;
}

GAME_SCRIPT_API( Sys, open_mingiftbag){
	P_BEGIN;
	P_INT(daytime);
	P_END;

	int64 opentime = WorldServ::instance()->curTime_ + 60;
	int64 closetime = opentime + ONE_DAY_SEC * daytime;

	MinGift::open(opentime,closetime);
	R_NONE;
}

GAME_SCRIPT_API( Sys, close_mingiftbag){
	MinGift::close();
	R_NONE;
}

GAME_SCRIPT_API( Sys, open_zhuanpan){
	P_BEGIN;
	P_INT(daytime);
	P_END;

	int64 opentime = WorldServ::instance()->curTime_ + 60;
	int64 closetime = opentime + ONE_DAY_SEC * daytime;

	Zhuanpan::open(opentime,closetime);
	R_NONE;
}

GAME_SCRIPT_API( Sys, close_zhuanpan){
	Zhuanpan::close();
	R_NONE;
}

GAME_SCRIPT_API( Sys, open_integralshop){
	P_BEGIN;
	P_INT(daytime);
	P_END;

	int64 opentime = WorldServ::instance()->curTime_ + 60;
	int64 closetime = opentime + ONE_DAY_SEC * daytime;

	IntegralShop::open(opentime,closetime);
	R_NONE;
}

GAME_SCRIPT_API( Sys, close_integralshop){
	IntegralShop::close();
	R_NONE;
}

GAME_SCRIPT_API( Sys, open_guild_demon_invaded){
	Guild::openGuildDemonInvaded();
	R_NONE;
}
GAME_SCRIPT_API( Sys, close_guild_demon_invaded){
	Guild::closeGuildDemonInvaded();
	R_NONE;
}
GAME_SCRIPT_API( Sys, open_guild_leader_invaded){
	Guild::openGuildLeaderInvaded();
	R_NONE;
}
GAME_SCRIPT_API( Sys, close_guild_leader_invaded){
	Guild::closeGuildLeaderInvaded();
	R_NONE;
}

GAME_SCRIPT_API(Player, openMagic)
{
	P_BEGIN;
	P_PTR(handle);
	P_END;

	Player* player = handle->asPlayer();
	if(NULL == player)
		R_NONE;
	player->initMagicItemLevelUp();

	R_NONE;
}

GAME_SCRIPT_API( Player, getbabydelat)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(babyid);
	P_END;

	Player* player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}
	Baby* pbaby = player->findBaby(babyid);
	if(pbaby == NULL)
	{
		R_BEGIN;
		R_INT(0);
		R_END;
	}
	R_BEGIN;
	R_INT(pbaby->calcrandDelta());
	R_END;
}

GAME_SCRIPT_API( Sys, kickPlayer){
	P_BEGIN;
	P_PTR(handle);
	P_END;
	Player* player = handle->asPlayer();
	if(!player)
		R_NONE;
	if(player->account_)
		Account::removeAccountByName(player->account_->username_);

	R_NONE;
}

GAME_SCRIPT_API( Player, checkOpenGather)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(gatherid);
	P_INT(tp);
	P_END;

	Player* player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_INT(EN_CannotfindPlayer);
		R_END;
	}
	ErrorNo err = player->checkGatherStates(gatherid,(GatherStateType)tp);

	R_BEGIN;
	R_INT(err);
	R_END;
}

GAME_SCRIPT_API( Player, opengather){
	P_BEGIN;
	P_PTR(handle);
	P_INT(gatherid);
	P_INT(tp);
	P_END;
	Player* player = handle->asPlayer();
	if(!player)
		R_NONE;
	player->openGather(gatherid,(GatherStateType)tp);

	R_NONE;
}


GAME_SCRIPT_API( Player,checkcompound )
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(compoundId);
	P_END;

	Player* player = handle->asPlayer();
	if(NULL == player)
	{
		R_BEGIN;
		R_BOOL(false);
		R_END;
	}
	bool ishas = player->checkCompound(compoundId);

	R_BEGIN;
	R_BOOL(!ishas);
	R_END;
}

GAME_SCRIPT_API( Player, opencompound)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(itemId);
	P_END;
	Player* player = handle->asPlayer();
	if(!player)
		R_NONE;
	player->openCompound(itemId);

	R_NONE;
}

GAME_SCRIPT_API( Player, copyGo)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(startSceneId);
	P_INT(sceneId);
	P_END;
	Player* player = handle->asPlayer();
	if(!player)
		R_NONE;
	player->startCopy(startSceneId,sceneId);

	R_NONE;
}

GAME_SCRIPT_API( Player, checkquestitem)
{
	P_BEGIN;
	P_PTR(handle);
	P_END;
	Player* player = handle->asPlayer();
	if(!player)
		R_NONE;
	player->checkQuestItem();

	R_NONE;
}

GAME_SCRIPT_API( Player, setPlayerLevel)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(level);
	P_END;
	Player* player = handle->asPlayer();
	if(!player)
		R_NONE;
	if(level >= Global::get<float>(C_PlayerMaxLevel))
		R_NONE ;
	player->setProp(PT_Level,level);

	R_NONE;
}

GAME_SCRIPT_API( Player, addplayertitle)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(title);
	P_END;
	Player* player = handle->asPlayer();
	if(!player)
		R_NONE;

	bool istitle = TitleTable::isTitle(title);
	if(!istitle)
		R_NONE;
	player->addPlayerTitle(title);

	R_NONE;
}

GAME_SCRIPT_API( Player, setPlayerJob)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(jobtype);
	P_END;
	Player* player = handle->asPlayer();
	if(!player)
		R_NONE;
	player->setProp(PT_Profession,(int)jobtype);
	player->setProp(PT_ProfessionLevel,1);
	R_NONE;
}

GAME_SCRIPT_API( Player, addcoursegift)
{
	P_BEGIN;
	P_PTR(handle);
	P_END;
	Player* player = handle->asPlayer();
	if(!player)
		R_NONE;
	player->addCourseGift();

	R_NONE;
}

GAME_SCRIPT_API( Player, addIntegral)
{
	P_BEGIN;
	P_PTR(handle);
	P_INT(pay);
	P_END;
	Player* player = handle->asPlayer();
	if(!player)
		R_NONE;
	player->updateIntegral(pay);
	R_NONE;
}

GAME_SCRIPT_API( Player, reset_property_1){
	P_BEGIN;
	P_PTR(handle);
	P_END;
	Player* player = handle->asPlayer();
	if(!player)
		R_NONE;
	player->calcProperty();
	R_NONE;
}

GAME_SCRIPT_API( Player, addBabyProp){
	P_BEGIN;
	P_PTR(handle);
	P_INT(babyinstid);
	P_INT(propType);
	P_INT(propValue);
	P_END;
	Player* player = handle->asPlayer();
	if(!player)
		R_NONE;
	Baby* pbaby = player->findBaby(babyinstid);
	if(pbaby == NULL)
		R_NONE;
	std::vector<COM_Addprop> v;
	COM_Addprop prop;
	prop.type_ = (PropertyType)propType;
	prop.uVal_ = propValue;
	v.push_back(prop);
	pbaby->addProperty(v);
	R_NONE;
}

GAME_SCRIPT_API( Player, setemployeebattlegroup){
	P_BEGIN;
	P_PTR(handle);
	P_INT(tableid);
	P_END;
	Player* player = handle->asPlayer();
	if(!player)
		R_NONE;
	U32 instid = player->findEmployeebyTableid(tableid);
	if(instid == 0)
		R_NONE;
	player->setBattleEmployee(instid,EBG_GroupOne,true);
	R_NONE;
}