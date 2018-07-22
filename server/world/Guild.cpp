//==============================================================================
/**
@date:		2015:10:15
@file: 		Guild.cpp
@author:	liwenhao
*/
//==============================================================================
#include "Guild.h"
#include "player.h"
#include "worldserv.h"
#include "FilterWord.h"
#include "GuildData.h"
#include "Scene.h"
#include "GameEvent.h"
#include "loghandler.h"
#include "skill.h"
#include "LiveSkilltable.h"
#include "monster.h"
#include "battle.h"
std::vector<Guild*>  Guild::guildList_;
IdGuildMap	Guild::guilds_;
IdGuildMap Guild::playerGuild_;
NameGuildMap Guild::nameGuild_;
S32									Guild::maxGuildId_;
std::set<std::string>				Guild::tempGuildName_;
std::set<S32>						Guild::tempGuildPlayerId_;
int					Guild::battleIntervalTime_;
Guild::BattleState						Guild::battleState_ = Guild::BS_Close;			
std::vector< Guild::BattleInfo* >	Guild::battleInfos_;

void Guild::BattleInfo::open(int32 copyId){

	pLeft->battleSceneCopyId_ = pRight->battleSceneCopyId_ = copyId;
	
	if(!copyId)
		return;
	pLeft->openGuildBattle(pRight->guildData_.guildId_);
	pRight->openGuildBattle(pLeft->guildData_.guildId_);
}

void Guild::BattleInfo::close(){
	if(isCalced_)
		return;
	pLeft->closeGuildBattle(winner_ == BW_Left);
	pRight->closeGuildBattle(winner_ == BW_Right);
	isCalced_ = true;
}

void Guild::openBattle(){
	if(battleState_ != BS_Close)
		return;
	battleIntervalTime_ = Global::get<int>(C_GuildBattleStartIntervalTime);
	calcGuildRank();

	std::vector<Guild*> glist;
	for(size_t i=0;i<guildList_.size(); ++i){
		if(guildList_[i]->getGuildLevel() >= Global::get<int>(C_FamilyBattleLevelMin) && guildList_[i]->getOnlinePlayerNum() > 0){
			glist.push_back(guildList_[i]);
		}
	}
	SceneData const* sd = SceneTable::getGuildBattleScene();
	Guild* pFirstGuild = NULL;
	while(!glist.empty())
	{
		if(NULL == pFirstGuild){
			pFirstGuild = glist.back();
			glist.pop_back();
			
		}
		else{
			int32 index = UtlMath::randN(Global::get<int>(C_FamilyBattleOpenRand));
			index  = (index >= glist.size()) ? 0 : index;
			Guild* pSecondGuild = glist[index];
			glist.erase(glist.begin()+index);
			BattleInfo*  pinfo = NEW_MEM(BattleInfo,pFirstGuild,pSecondGuild);
			battleInfos_.push_back(pinfo);
			pFirstGuild = NULL;

			Scene* s = SceneManager::instance()->openSceneCopy(sd->sceneId_);
			SRV_ASSERT(s);
			pinfo->open(s->sceneId_);
		}
	}

	if(pFirstGuild){
		pFirstGuild->closeGuildBattle(true);
		pFirstGuild = NULL;
	}

	battleState_ = BS_Prepare;
	
}

void Guild::prepareBattleTimeout(){
	for(size_t k=0; k<battleInfos_.size(); ++k){
		Guild* pLeft = battleInfos_[k]->pLeft;
		Guild* pRigh = battleInfos_[k]->pRight;
		if(pLeft)
			pLeft->broadcaster_.errorno(EN_GuildBattleTimeout2);
		if(pRigh)
			pRigh->broadcaster_.errorno(EN_GuildBattleTimeout2);
	}
}

void Guild::startBattle(){
	if(battleState_ != BS_Prepare)
		return;
	
	for(size_t k=0; k<battleInfos_.size(); ++k){
		Guild* pLeft = battleInfos_[k]->pLeft;
		Guild* pRigh = battleInfos_[k]->pRight;
		if(pRigh != NULL){
			pLeft->broadcaster_.startGuildBattle(pRigh->guildData_.guildName_,0,0);
			pRigh->broadcaster_.startGuildBattle(pLeft->guildData_.guildName_,0,0);
			Scene* s = SceneManager::instance()->getScene(pRigh->battleSceneCopyId_);	
			std::vector<S32> usednpc; 

			std::vector<S32> progenitus  = pRigh->guildData_.progenitusPositions_;
			for(size_t i=0; i<progenitus.size(); ++i){
				COM_GuildProgen* progen = pRigh->getProgenitusById(progenitus[i]);
				if(!progen)
					continue;
				S32 npcid = Monster2Table::getMonsterRightNpc(progen->mId_,progen->lev_,i%2);
				if(!npcid)
					continue;
				usednpc.push_back(npcid);
			}

			progenitus  = pLeft->guildData_.progenitusPositions_;

			for(size_t i=0; i<progenitus.size(); ++i){
				COM_GuildProgen* progen = pLeft->getProgenitusById(progenitus[i]);
				if(!progen)
					continue;
				S32 npcid = Monster2Table::getMonsterLeftNpc(progen->mId_,progen->lev_,i%2);
				if(!npcid)
					continue;
				usednpc.push_back(npcid);
			}
			s->addNpcs(usednpc);
			battleInfos_[k]->usedNpcs_ = usednpc;
		}
	}
	//神兽
	battleState_ = BS_Battle;
}

void Guild::stopBattle(){
	if(battleState_ !=BS_Battle)
		return;
	for(size_t i=0; i<battleInfos_.size(); ++i){
		if(battleInfos_[i]->hasWinner())
			continue;
		Guild* pLeft = battleInfos_[i]->pLeft;
		Guild* pRight = battleInfos_[i]->pRight;	
		
		if(pLeft->battleWinCount_ > pRight->battleWinCount_)
			battleInfos_[i]->setWinner(BW_Left);
		else if(pLeft->battleWinCount_ < pRight->battleWinCount_)
			battleInfos_[i]->setWinner(BW_Right);
		else
			battleInfos_[i]->setWinner(BW_Lose);
	}
	
	for(size_t i=0; i<battleInfos_.size(); ++i){
		battleInfos_[i]->close();
	}
	battleState_ = BS_Stop;
}

void Guild::closeBattle(){
	if(battleState_ != BS_Stop)
		return;

	for(size_t i=0; i<battleInfos_.size(); ++i){
		SceneManager::instance()->closeSceneCopy(battleInfos_[i]->pLeft->battleSceneCopyId_);
		battleInfos_[i]->open(0);
		DEL_MEM(battleInfos_[i]);
	}
	battleInfos_.clear();
	battleState_ = BS_Close;
}


void Guild::updateBattle(float interval){
	if(battleState_ == BS_Prepare)
		battleIntervalTime_ -= interval;
	else if(battleState_ ==BS_Battle)
	{
	for(size_t i=0; i<battleInfos_.size(); ++i){
		if(battleInfos_[i]->hasWinner())
		{
			battleInfos_[i]->close();
			continue;
		}
		Guild* pLeft = battleInfos_[i]->pLeft;
		Guild* pRight = battleInfos_[i]->pRight;
		int32 lSum = pLeft->sumInBattleSceneMembers();
		int32 rSum = pRight->sumInBattleSceneMembers();
		if((lSum == rSum) && (rSum == 0)){
			battleInfos_[i]->setWinner(BW_Lose);
		}
		else if(lSum == 0){
			battleInfos_[i]->setWinner(BW_Right);
		}
		else if(rSum == 0){
			battleInfos_[i]->setWinner(BW_Left);
		}
	}
	}
}

void Guild::talked2Progenitus(Player* player,int32 npcId){
	Guild* pMine = player->myGuild();
	if(!pMine)
		return;
	if(battleState_ != BS_Battle)
		return;
	BattleInfo* pInfo = pMine->bInfo_;
	if(!pInfo){
		return;
	}
	if(pInfo->hasWinner())
		return;
	bool isLeft = false;
	int32 mId = Monster2Table::findMonsterIdByNpcId(npcId,isLeft);
	if(pMine->isLeft_ == isLeft)
	{
		player->errorMessageToC(EN_MyFamilyMonster);
		return; //相同阵营
	}
	else{
		Guild* pOther = pInfo->getOtherGuild(pMine);
		COM_GuildProgen* pp = pOther->getProgenitusById(mId);
		if(NULL == pp)
			return;
		Scene* s = SceneManager::instance()->getScene(pMine->battleSceneCopyId_);
		if(NULL == s)
			return;
		if(!s->addBattleNpc(npcId))
			return;
		Monster* m = NEW_MEM(Monster,pp->mId_,pp->lev_);
		Battle* b = Battle::getOneFree();
		b->create(player,m);
		b->guildSceneId_ = pMine->battleSceneCopyId_;
		b->guildNpcId_ = npcId;
	}
}

void Guild::openGuildDemonInvaded(){ //开启魔族入侵
	enum {
		ARG0,
		ARGM,
	};
	static GEParam params[ARGM];
	params[ARG0].type_ = GEP_INT;
	for(size_t i=0; i<guildList_.size(); ++i){
		guildList_[i]->demonCount_ = 0;
		params[ARG0].value_.i = guildList_[i]->guildData_.guildId_;
		GameEvent::procGameEvent(GET_OpenGuildDemonInvaded,params,ARGM,0);
	}
}
void Guild::closeGuildDemonInvaded(){ //关闭魔族入侵
	enum {
		ARG0,
		ARG1,
		ARGM,
	};

	static GEParam params[ARGM];
	params[ARG0].type_ = GEP_INT;
	params[ARG1].type_ = GEP_INT;
	int32 count = Global::get<int>(C_FamilyDemonCount);
	for(size_t i=0; i<guildList_.size(); ++i){
		params[ARG0].value_.i = guildList_[i]->guildData_.guildId_;
		params[ARG1].value_.i = guildList_[i]->demonCount_ >= count ? 1:0;

		GameEvent::procGameEvent(GET_CloseGuildDemonInvaded,params,ARGM,0);
	}
}
void Guild::openGuildLeaderInvaded(){ //开启首领入侵
	enum {
		ARG0,
		ARGM,
	};
	static GEParam params[ARGM];
	params[ARG0].type_ = GEP_INT;
	for(size_t i=0; i<guildList_.size(); ++i){
		guildList_[i]->leaderCount_ = 0;
		params[ARG0].value_.i = guildList_[i]->guildData_.guildId_;
		GameEvent::procGameEvent(GET_OpenGuildLeaderInvaded,params,ARGM,0);
	}
}
void Guild::closeGuildLeaderInvaded(){ //关闭首领入侵
	enum {
		ARG0,
		ARG1,
		ARGM,
	};

	static GEParam params[ARGM];
	params[ARG0].type_ = GEP_INT;
	params[ARG1].type_ = GEP_INT;
	int32 count = Global::get<int>(C_FamilyLeaderCount);
	for(size_t i=0; i<guildList_.size(); ++i){
		params[ARG0].value_.i = guildList_[i]->guildData_.guildId_;
		params[ARG1].value_.i = guildList_[i]->leaderCount_ >= count ? 1:0;

		GameEvent::procGameEvent(GET_CloseGuildLeaderInvaded,params,ARGM,0);
	}
}

void Guild::clear()
{
	IdGuildMap::iterator it =guilds_.begin();
	while(it!=guilds_.end()){
		if (it->second!=NULL)
			DEL_MEM(it->second);
		++it;
	}
	guilds_.clear();
	playerGuild_.clear();
	nameGuild_.clear();
	tempGuildName_.clear();
	tempGuildPlayerId_.clear();
	maxGuildId_=0;
}

void Guild::calcGuildRank()
{
	struct RankSort{
		bool operator()(Guild const* pL, Guild const* pR){
			int32 lFF = pL->getTotalFF();
			int32 rFF = pR->getTotalFF();
			return (rFF == lFF) ? (pL->guildData_.guildId_ > pL->guildData_.guildId_) : (lFF > rFF);
		}
	};

	static RankSort rs;
	std::sort(guildList_.begin(),guildList_.end(),rs);
}

bool Guild::addCritical(const std::string& name , S32 id)
{
	std::pair<std::set<std::string>::iterator ,bool> nameres = tempGuildName_.insert(name);
	std::pair<std::set<S32>::iterator ,bool> idres = tempGuildPlayerId_.insert(id);
	return ((nameres.second)&&(idres.second));
}
void Guild::delCritical(const std::string& name , S32 id)
{
	tempGuildPlayerId_.erase(id);
	tempGuildName_.erase(name);
}

Guild* Guild::addGuild(COM_Guild&guild)
{
	Guild* pGuild=NEW_MEM(Guild);
	SRV_ASSERT(pGuild);
	pGuild->guildData_=guild;
	guildList_.push_back(pGuild);
	Guild::guilds_[guild.guildId_]=pGuild;
	Guild::nameGuild_[guild.guildName_]=pGuild;
	if (maxGuildId_<guild.guildId_){
		maxGuildId_=guild.guildId_;
	}
	return pGuild;
}

void Guild::checkGuildMember(std::vector< COM_GuildMember >& guildMember)
{
	for (size_t i = 0; i < guildMember.size(); ++i){
		Guild* pGuild = findGuildById(guildMember[i].guildId_);
		if (pGuild == NULL){
			ACE_DEBUG((LM_DEBUG ,"1111111111111111111111"));
			return;
		}
		if (pGuild->getLeader() == NULL)
		{
			COM_GuildMember * nextPremier =  NULL;
			for(size_t i=0;i < pGuild->guildMember_.size(); ++i){
				if (nextPremier == NULL){
					nextPremier = &pGuild->guildMember_[i];
					continue;
				}
				if(nextPremier->job_ < pGuild->guildMember_[i].job_){
					nextPremier = &pGuild->guildMember_[i];
				}
			}
			if(nextPremier){
				//更新帮主名
				pGuild->guildData_.master_=nextPremier->roleId_;
				pGuild->guildData_.masterName_=nextPremier->roleName_;
				//通知客户端更新
				pGuild->updateGuild();
				DBHandler::instance()->updateMemberPosition(nextPremier->roleId_, GJ_Premier);
				DBHandler::instance()->updateGuild(pGuild->guildData_);
			}
			
		}
	}
}

bool Guild::delGuild(U32 id)
{
	if(battleState_ != BS_Close)
		return true;
	IdGuildMap::iterator itr0	= guilds_.find(id);
	if(itr0 == guilds_.end())
		return false;
	
	Guild* pGuild = itr0->second;
	guilds_.erase(itr0);
	SRV_ASSERT(NULL != pGuild);
	
	std::vector<Guild*>::iterator itrV = std::find(guildList_.begin(),guildList_.end(),pGuild);
	if(itrV != guildList_.end())
		guildList_.erase(itrV);
	
	NameGuildMap::iterator itr1	=  nameGuild_.find(pGuild->guildData_.guildName_); //通过帮派名找
	SRV_ASSERT(nameGuild_.end() != itr1);
	nameGuild_.erase(itr1);
	
	IdGuildMap::iterator itr = playerGuild_.begin();
	while(itr != playerGuild_.end())
	{
		Guild* tmp = itr->second;
		if(tmp == pGuild)
		{
			Player* pPlayer = Player::getPlayerByInstId(itr->first);
			if(NULL != pPlayer)
			{
				pPlayer->cleanGuildInfomation();
			}
			DBHandler::instance()->deleteGuildMember(itr->first);
			playerGuild_.erase(itr++);
			continue;
		}
		++itr;
	}
	pGuild->guildMember_.clear();
	
	DBHandler::instance()->delGuild(id);

	DEL_MEM(pGuild);
	pGuild = NULL;
	return true;
}

bool Guild::addGuildMember(COM_GuildMember& member)
{
	Guild* pGuild=Guild::findGuildById(member.guildId_);
	if(NULL == pGuild ) 
	{
		ACE_DEBUG((LM_ERROR ,"no guild id %d \n",member.guildId_));
		SRV_ASSERT(0);
		return false;
	}
	
	if( !pGuild->addMember(member) )
	{
		ACE_DEBUG((LM_ERROR ," guild member id confict %d \n",member.roleId_));
		SRV_ASSERT(0);
		return false;
	}
	member.joinTime_ = WorldServ::instance()->curTime_;
	Guild::playerGuild_[member.roleId_]  = pGuild;
	Player* pPlayer = Player::getPlayerByInstId(member.roleId_);
	if(NULL != pPlayer)	{
		pGuild->memberOnline(pPlayer);
	}
	
	if(pPlayer)
		GameEvent::procGameEvent(GET_JoinGuild,NULL,0,pPlayer->getHandleId());
	return true;
}

bool Guild::delGuildMember(S32 roleId, bool isKick)
{
	if(battleState_ != BS_Close)
		return true;
	IdGuildMap::iterator itr = playerGuild_.find(roleId);
	if(playerGuild_.end() == itr)
		return true;
	Guild* guild = itr->second;
	if(guild == NULL)
		return true;
	guild->delMember(roleId);	
	Player* pPlayer = Player::getPlayerByInstId(roleId);
	if(NULL != pPlayer)
	{
		pPlayer->setProp(PT_GuildID,0);
		pPlayer->cleanGuildQuest();
		if (isKick)
			CALL_CLIENT(pPlayer,leaveGuildOk(pPlayer->playerName_,true)); //被提出
		else{
			guild->broadcaster_.leaveGuildOk(pPlayer->playerName_,false);
			CALL_CLIENT(pPlayer,leaveGuildOk(pPlayer->playerName_,false)); 
		}
	}

	//删除数据库数据
	DBHandler::instance()->deleteGuildMember(roleId);

	if(guild != NULL && guild->guildMember_.empty())
		delGuild(guild->guildData_.guildId_);
	else
		playerGuild_.erase(itr);

	return true;
}

Guild* Guild::findGuildById(U32 guildId)
{
	IdGuildMap::iterator it=guilds_.find(guildId);
	if (it!=guilds_.end())
	{
		return it->second;
	}
	return NULL;
}

Guild* Guild::findGuildByPlayer(S32 player)
{
	IdGuildMap::iterator it=playerGuild_.find(player);
	if (it!=playerGuild_.end())
	{
		return it->second;
	}
	return NULL;
}
Guild* Guild::findGuildByName(const std::string& guildName)
{
	NameGuildMap::iterator it=nameGuild_.find(guildName);
	if (it!=nameGuild_.end()){
		return it->second;
	}
	return NULL;
}

void  Guild::passZeroHour(){
	for (IdGuildMap::iterator i=guilds_.begin(),e=guilds_.end();i!=e;++i){
		i->second->checkFundz();
		i->second->requestGuildShopItems();
		i->second->resetGuildSign();
	}
}

void Guild::createGuild(Player* player,const std::string& guildName)
{
	if(player == NULL || player->getClient() == NULL)
		return;
	//玩家小于C_CreateGuildLevel不能创建帮派
	if(player->getProp(PT_Level) < Global::get<int>(C_CreateGuildLevel))
		return;
	if(player->getItemNumByItemId(Global::get<int>(C_CreateGuildItem)) <= 0)
		return;
	if( guildName.empty())
		return ;
	if(GetUtf8Len2GBLen(guildName.c_str(),guildName.size())>MAX_GUILD_NAME_LEN)
		return ;
	if(NULL!=findGuildByName(guildName) ) {
		CALL_CLIENT(player,errorno(EN_GuildNameSame));
		return ;
	}
	
	std::string tmpName = guildName;
	if(true == FilterWord::replace(tmpName))
	{
		CALL_CLIENT(player,errorno(EN_FilterWord));
		return ;
	}

	if(player->getProp(PT_Money) < Global::get<int>(C_CreateGuildGold)){
		//钱不够
		CALL_CLIENT(player,errorno(EN_PlayerGoldLess));
		return;
	}
	if(NULL != Guild::findGuildByPlayer(player->getGUID())){
		//已经有帮派
		CALL_CLIENT(player,errorno(EN_PlayerHasGuild));
		return;
	}
	//设置占位
	if(false==Guild::addCritical(guildName,player->getGUID())){	
		//已经有帮派
		CALL_CLIENT(player,errorno(EN_PlayerHasGuild));
		return;
	}
	
	COM_Guild	createGuildData;
	createGuildData.createTime_ = WorldServ::instance()->curTime_;
	createGuildData.guildId_	= ++maxGuildId_;
	createGuildData.guildLevel_	= 1;
	createGuildData.guildName_	= guildName;
	createGuildData.notice_		= "";
	createGuildData.master_		= player->getGUID();
	createGuildData.masterName_ = player->getNameC();
	createGuildData.fundz_ = Global::get<int>(C_FamilyInitFundz);
	
	int32 progenitusPositionMax = Global::get<int>(C_ProgenitusPositionMax);
	if(progenitusPositionMax <=0 )
		progenitusPositionMax = 8;
	createGuildData.progenitusPositions_.resize(progenitusPositionMax);
	///初始化帮派建筑
	COM_GuildBuilding building;
	building.level_     = 1;	///<初始 为0
	building.struction_ = 0;

	for(size_t i=GBT_Main; i<GBT_MAX; ++i){
		createGuildData.buildings_.push_back(building);
	}	

	for(size_t i=C_FamilyProgenitusMonsterId0,i2=0; i<=C_FamilyProgenitusMonsterId3; ++i){
		COM_GuildProgen gp;
		gp.mId_ = Global::get<int>((Constant)i);
		gp.lev_ = 30;
		gp.exp_ = 0;
		createGuildData.progenitus_.push_back(gp);
		createGuildData.progenitusPositions_[i2] = createGuildData.progenitusPositions_[i2+1] = gp.mId_;
		i2+=2;
	}
	

	COM_GuildMember	guildMember;
	guildMember.guildId_		= createGuildData.guildId_;
	guildMember.profType_		= player->getProp(PT_Profession);
	guildMember.profLevel_		= player->getProp(PT_ProfessionLevel);
	guildMember.job_			= GJ_Premier;
	guildMember.roleId_			= player->getGUID();
	guildMember.roleName_		= player->getNameC();
	guildMember.level_			= player->getProp(PT_Level);
	guildMember.joinTime_ = createGuildData.createTime_;
	guildMember.contribution_ = player->getGuildContribution();
	requestGuildShopItems(createGuildData,guildMember);
	player->addMoney(-Global::get<int>(C_CreateGuildGold));
	player->delBagItemByItemId(Global::get<int>(C_CreateGuildItem),1);
	SGE_LogProduceTrack track;
	track.playerId_ = player->getGUID();
	track.playerName_ = player->getNameC();
	track.from_ = 11;
	track.money_ = -Global::get<int>(C_CreateGuildGold);
	LogHandler::instance()->playerTrack(track);

	///通知db 
	DBHandler::instance()->insertGuild(createGuildData,guildMember);
	if(player)
		GameEvent::procGameEvent(GET_CreateGuild,NULL,0,player->getHandleId());
}

void Guild::memberchangeProfession(Player *player)
{
	if(NULL==player)
		return ;
	Guild *pGuild = player->myGuild();
	if(NULL==pGuild){
		return ;
	}

	COM_GuildMember *pMember = player->myGuildMember();

	if(NULL == pMember)
		return;

	pMember->profType_ = player->getProp(PT_Profession);
	pMember->profLevel_= player->getProp(PT_ProfessionLevel);

	pGuild->updateMember(*pMember,MLF_ChangeProfession,true);
}

void Guild::memberLevelUp(Player *player)
{
	if(NULL==player)
		return ;
	Guild *pGuild = player->myGuild();
	if(NULL==pGuild){
		return ;
	}
	
	COM_GuildMember *pMember = player->myGuildMember();

	if(NULL == pMember){
		return;
	}

	pMember->level_ = player->getProp(PT_Level);
	pGuild->updateMember(*pMember,MLF_ChangeLevel,true);
}

void Guild::memberOnline( Player* player )
{
	Guild* pGuild = player->myGuild();
	if (NULL == pGuild){
		return;
	}

	COM_GuildMember* pMember = player->myGuildMember();
	if( pMember){
		player->setProp(PT_GuildID,pGuild->guildData_.guildId_);
		pMember->offlineTime_ = 0;
		//更新帮派信息
		CALL_CLIENT(player,initGuildData(pGuild->guildData_));
		//更新帮派成员
		pGuild->updateMemberList(player);
		//通知其他成员
		pGuild->updateMember(*pMember,MLF_ChangeOnline);
		if(player->getClient())
			pGuild->broadcaster_.addChannel(player->getClient());
	}
	
	
	if(battleState_ != BS_Close && pGuild->getGuildLevel() >= Global::get<int>(C_FamilyBattleLevelMin) ){
		Guild* pOther = Guild::findGuildByName(pGuild->otherGuildName_);
		if(pOther)
		{
			if(battleState_ == BS_Prepare)
				CALL_CLIENT(player,openGuildBattle(pOther->guildData_.guildName_,pOther->guildMember_.size(),pOther->getGuildLevel(),pGuild->isLeft_,battleIntervalTime_));
			else if(battleState_ == BS_Battle)
				CALL_CLIENT(player,startGuildBattle(pOther->guildData_.guildName_,pOther->battleWinCount_,pGuild->battleWinCount_));
			
		}
	}
}

void Guild::memberOffline( Player* player )
{
	Guild* pGuild = player->myGuild();
	if (NULL == pGuild){
		return;
	}
	COM_GuildMember* pMember = player->myGuildMember();
	if(NULL != pMember){
		player->setProp(PT_GuildID,pGuild->guildData_.guildId_);
		pMember->offlineTime_ = WorldServ::instance()->curTime_;
		pGuild->updateMember(*pMember,MLF_ChangeOffline);
		if(player->getClient())
			pGuild->broadcaster_.removeChannel(player->getClient());
	}
}


void Guild::leave( Player* pPlayer )
{
	if(battleState_ != BS_Close)
	{
		pPlayer->errorMessageToC(EN_TeamMemberHourLess24);
		return;
	}
	if(pPlayer == NULL)
		return;

	Guild* pGuild = pPlayer->myGuild();
	if(NULL == pGuild ) return ;
	S32 guildId=pGuild->guildData_.guildId_;

	COM_GuildMember* pMember = pPlayer->myGuildMember();
	SRV_ASSERT(pMember);
	if( GJ_Premier == pMember->job_&&1 != pGuild->guildMember_.size()){
		//帮主不能退出 预先交付帮主
		pPlayer->errorMessageToC(EN_PremierQuitError);
		return ;
	}

	//退出帮派
	SRV_ASSERT( Guild::delGuildMember(pPlayer->getGUID(),false));

	//如果没人了
	if (pGuild->guildMember_.empty())
	{
		//最后一人退出销毁帮派
		//SRV_ASSERT(Guild::delGuild(guildId));
		DBHandler::instance()->delGuild(guildId);
	}
}

void Guild::leave( int32 playerId )
{

	Guild* pGuild = Guild::findGuildByPlayer(playerId);
	if(NULL == pGuild ) return ;
	COM_GuildMember* pMember = pGuild->findMember(playerId);
	SRV_ASSERT(pMember);
	if( GJ_Premier == pMember->job_ ){
		COM_GuildMember * nextPremier =  NULL;
		for(size_t i=0;i < pGuild->guildMember_.size(); ++i){
			if(pGuild->guildMember_[i].roleId_ == pMember->roleId_){
				continue;
			}
			if(nextPremier == NULL){
				nextPremier = &pGuild->guildMember_[i];
				continue;
			}
			if(nextPremier->job_ < pGuild->guildMember_[i].job_){
				nextPremier = &pGuild->guildMember_[i];
			}
		}
		if(nextPremier)
			pGuild->transferPremier(playerId, nextPremier->roleId_);
	}

	//退出帮派
	SRV_ASSERT( Guild::delGuildMember(playerId,false));

	//如果没人了
	if (pGuild->guildMember_.empty())
	{
		//最后一人退出销毁帮派
		//SRV_ASSERT(Guild::delGuild(guildId));
		DBHandler::instance()->delGuild(pGuild->guildData_.guildId_);
	}
}


void Guild::kickOut( Player* player,S32 currPlayerId )
{
	if(battleState_ != BS_Close)
	{
		player->errorMessageToC(EN_TeamMemberHourLess24);
		return;
	}
	if(player == NULL)
		return;

	Guild* pGuild = player->myGuild();
	if(NULL == pGuild ) 
		return ; //没帮派
	COM_GuildMember* pMember = pGuild->findMember(currPlayerId);
	if(NULL == pMember) 
		return ; //不是本帮成员
	
	COM_GuildMember* pOffical = player->myGuildMember();

	if(pOffical->job_ < GJ_VicePremier){
		//权限不够
		CALL_CLIENT(player,errorno(EN_CommandPositionLess));
		return ;
	}

	if(pMember->job_ != GJ_People){
		//只能T帮众 
		return ;
	}
	pGuild->delMember(currPlayerId);
	//删除
	SRV_ASSERT( Guild::delGuildMember(currPlayerId,true) );
}

void Guild::loseLeader(Player* player){
	if(battleState_ != BS_Close)
	{
		player->errorMessageToC(EN_TeamMemberHourLess24);
		return;
	}
	if(player == NULL)
		return;
	Guild* pGuild = player->myGuild();
	if(NULL == pGuild ) 
		return ; //没帮派
	COM_GuildMember* pMember = player->myGuildMember();
	if(NULL == pMember) 
		return ; //不是本帮成员
	COM_GuildMember* pLeader = pGuild->getLeader();
	if(NULL == pLeader) 
		return ; //不是本帮成员
	if(player->getItemNumByItemId(Global::get<int>(C_FamilyLoseLeaderItem))<=0){
		return;
	}
	if(WorldServ::instance()->curTime_ - pMember->offlineTime_ < Global::get<int>(C_FamilyLeaderOffineTimeMax)){
		return;
	}
	pMember->job_ = pLeader->job_;
	pLeader->job_ = GJ_People;
	DBHandler::instance()->updateMemberPosition(pMember->roleId_,(GuildJob)pMember->job_);
	DBHandler::instance()->updateMemberPosition(pLeader->roleId_,(GuildJob)pLeader->job_);
	pGuild->updateMember(*pMember,MLF_ChangeProfession);
	pGuild->updateMember(*pLeader,MLF_ChangeProfession);
}

void Guild::requestJoinGuild(Player*player , U32 guid)
{
	if(battleState_ != BS_Close)
	{
		player->errorMessageToC(EN_TeamMemberHourLess24);
		return;
	}
	if(player == NULL)
		return;
	if(!player->getOpenSubSystemFlag(OSSF_Family)){
		CALL_CLIENT(player,errorno(EN_NoSubSyste));
		return;
	}

	if(WorldServ::instance()->curTime_ - player->exitGuildTime_ < Global::get<int>(C_FamilyJoinGuildIntervalTime) )
	{
		player->errorMessageToC(EN_GuildMemberLess24);
		return; ////不能邀请
	}
	Guild* pGuild = findGuildById(guid);
	if(NULL == pGuild) 
		return ;
	
	//检查玩家是否有帮派
	if(NULL != player->myGuild()){
		//TODO 提示
		return;
	}
	if(pGuild->findRequest(player->getGUID())){
		//已经申请了
		player->errorMessageToC(EN_InRequestErr);
		return ;
	}

	COM_GuildRequestData newRequest;
	newRequest.roleId_  =player->getGUID();
	newRequest.level_	=player->getProp(PT_Level);
	newRequest.roleName_=player->getNameC();
	newRequest.prof_ = player->getProp(PT_Profession);
	newRequest.profLevel_ = player->getProp(PT_ProfessionLevel);
	newRequest.time_ = WorldServ::instance()->curTime_;
	if( Global::get<int>(C_RequsetJoinGuildMax) <= pGuild->guildData_.requestList_.size()){
		//长度超过申请列表
		player->errorMessageToC(EN_RequestListFull);
		return;
	}

	pGuild->addRequestList(newRequest);
	DBHandler::instance()->updateGuildRequestList(guid,pGuild->guildData_.requestList_);
	player->errorMessageToC(EN_joinGuildRequestOk);
}

void Guild::acceptRequestGuild(Player*player , S32 playerId)
{
	if(battleState_ != BS_Close)
	{
		player->errorMessageToC(EN_TeamMemberHourLess24);
		return;
	}
	if(player == NULL)
		return;
	/*Player * pJoinPlayer= Player::getPlayerByInstId(playerId);
	if(pJoinPlayer == NULL)
		return;
	if(!pJoinPlayer->getOpenSubSystemFlag(OSSF_Family)){
		CALL_CLIENT(pJoinPlayer,errorno(EN_NoSubSyste));
		return;
	}*/
	Guild* pGuild = findGuildByPlayer(player->getGUID());
	if(NULL == pGuild) 
		return ;	
	
	if(pGuild->guildMember_.size() >= pGuild->getMemberLimit()){
		CALL_CLIENT(player,errorno(EN_GuildMemberMax));
		//超过最大人数上限
		return ;
	}
	COM_GuildMember* pMember = pGuild->findMember(player->getGUID());
	SRV_ASSERT(pMember);
	if(pMember->job_ < GJ_SecretaryHead){
		CALL_CLIENT(player,errorno(EN_CommandPositionLess));
		//没有权限
		return ;
	}

	if(NULL == findGuildByPlayer(playerId)){
		//申请人没有帮派
		COM_ContactInfo* pCache = WorldServ::instance()->findContactInfo(playerId);
		if(pCache == NULL)
			return;

		COM_GuildMember member;
		member.profType_ = pCache->job_;
		member.profLevel_= pCache->jobLevel_;
		member.guildId_ = pGuild->guildData_.guildId_;
		member.level_   = pCache->level_;
		member.job_= GJ_People;
		member.roleId_  = playerId;
		member.roleName_= pCache->name_;	
		member.joinTime_ = WorldServ::instance()->curTime_;
		SGE_ContactInfoExt* p = WorldServ::instance()->findContactInfoExt(playerId);
		if(p)
			member.contribution_ = p->guildContribute_;
		if(!Guild::addGuildMember(member)){
			return ;
		}
		DBHandler::instance()->createGuildMember(member);
		/// 事件抛给加入帮派的玩家
		/*if( NULL != pJoinPlayer){
			pJoinPlayer->updateMyGuildMember();
		}*/
	}
	else{
		// 提示对方已经加入帮派
		CALL_CLIENT(player,errorno(EN_JoinOtherGuild));
	}
	const COM_GuildRequestData* pData = pGuild->findRequest(playerId);
	if(!pData)
		return ;
	//清除申请列表 并更新DB 
	pGuild->delRequestList(playerId); 
	DBHandler::instance()->updateGuildRequestList(pGuild->guildData_.guildId_ , pGuild->guildData_.requestList_);
}

void Guild::refuseRequestGuild(Player*player , S32 playerId)
{
	if(player == NULL)
		return ;
	Guild* pGuild = findGuildByPlayer(player->getGUID());
	if(NULL == pGuild)
		return ;							//自
	const COM_GuildRequestData* pData = pGuild->findRequest(playerId);
	if(NULL == pData)
		return;
	COM_GuildMember* pMember = pGuild->findMember(player->getGUID());
	SRV_ASSERT(pMember);
	if(pMember->job_ < GJ_VicePremier)
		return ;
	pGuild->delRequestList(playerId);
	//清除申请列表 并更新DB
	DBHandler::instance()->updateGuildRequestList(pGuild->guildData_.guildId_ , pGuild->guildData_.requestList_);
}

void Guild::inviteJoinGuild(Player* sendPlayer,const std::string& name)
{
	if(!sendPlayer)
		return;
	Guild* pGuild = sendPlayer->myGuild();
	if(NULL == pGuild) return ;
	COM_GuildMember * pOffical = sendPlayer->myGuildMember();
	SRV_ASSERT(pOffical);
	if(pOffical->job_ < GJ_SecretaryHead)
	{
		//没有权限
		CALL_CLIENT(sendPlayer,errorno(EN_CommandPositionLess));
		return;
	}
	if(pGuild->guildMember_.size() >= pGuild->getMemberLimit())
	{
		CALL_CLIENT(sendPlayer,errorno(EN_GuildMemberMax));
		//超过最大人数上限
		return ;
	}
	Player* p = Player::getPlayerByName(name);
	if(p == NULL){
		//不在线
		CALL_CLIENT(sendPlayer,errorno(EN_CannotfindPlayer));
		return;
	}

	if(WorldServ::instance()->curTime_ - p->exitGuildTime_ < Global::get<int>(C_FamilyJoinGuildIntervalTime) ){
		sendPlayer->errorMessageToC(EN_InviteeLeaveGuildLess24);			
		return; ////不能邀请
	}

	if(!p->getOpenSubSystemFlag(OSSF_Family))
	{
		//功能没开
		CALL_CLIENT(sendPlayer,errorno(EN_NoSubSyste));
		return;
	}
	Guild* pG = Guild::findGuildByPlayer(p->getGUID());
	if(pG != NULL)
	{
		CALL_CLIENT(sendPlayer,errorno(EN_PlayerHasGuild));
		return;
	}
	CALL_CLIENT(p,inviteGuild(sendPlayer->getNameC(),pGuild->guildData_.guildName_));
}

void Guild::respondInviteJoinGuild(const std::string& sendPlayer,U32 rePlayerId)
{
	Player* p = Player::getPlayerByName(sendPlayer);
	if(p == NULL){
		//不在线
		return;
	}
	Guild::acceptRequestGuild(p,rePlayerId);
}

void Guild::requestGuildShopItems(Player* player){
	Guild* pGuild = player->myGuild();
	if(pGuild == NULL)
		return;
	COM_GuildMember *member = player->myGuildMember();
	if(member == NULL)
		return;
	
	int pay = (int)UtlMath::pow(2,member->shopRefreshTimes_) * 20;
	if(pay > player->getGuildContribution())
	{
		player->errorMessageToC(EN_GuildBattleJoinSceneMoveValue);
		return;
	}
	if(member->shopRefreshTimes_ >= member->job_ + 1){
		player->errorMessageToC(EN_RefreshShopTimeLess);
		return;
	}
	pGuild->requestGuildShopItems(*member);
	
	member->shopRefreshTimes_ += 1;
	player->updateMyGuildMember();
	player->addGuildContribution(-pay);

}

void Guild::requestGuildShopItems(COM_Guild const& guild, COM_GuildMember & member){
	GuildBuildingData const* p = GuildBuildingData::getGuildBuidingData(GBT_Shop,guild.buildings_[GBT_Shop-1].level_);
	if(NULL == p)
		return;

	member.shopItems_.clear();
	GuildShopItemData::randomItems(member.shopItems_,p->level_);
	std::random_shuffle(member.shopItems_.begin(),member.shopItems_.end());

	if(member.shopItems_.size() > p->number_)
		member.shopItems_.erase(member.shopItems_.begin() + p->number_ , member.shopItems_.end());
}

void Guild::requestGuildShopItems(COM_GuildMember &member){
	requestGuildShopItems(guildData_,member);
}

void Guild::requestGuildShopItems(){
	for(IdGuildMemberMap::iterator i=guildMember_.begin(),e=guildMember_.end();i != e; ++i){
		requestGuildShopItems(guildData_,i->second);
		i->second.shopRefreshTimes_ = 0;
		Player* p = Player::getPlayerByInstId(i->first);
		if(p){
			p->updateMyGuildMember();
		}
	}
}

void Guild::presentGuildItem(Player *player, int32 num){
	Guild* pGuild = player->myGuild();
	if(pGuild == NULL)
		return;
	if(num <=0)
		return;
	if(player->getItemNumByItemId(Global::get<int>(C_FamilyPresentItemId)) < num)
		return;
	
	pGuild->guildData_.presentNum_ += num;
	player->delBagItemByItemId(Global::get<int>(C_FamilyPresentItemId),num);
	pGuild->checkPresent();
	pGuild->broadcaster_.presentGuildItemOk(pGuild->guildData_.presentNum_);
	DBHandler::instance()->updateGuild(pGuild->guildData_);
}

void Guild::levelupSkill(Player* player, int skId){
	if(NULL == player)
		return;
	Guild* pGuild = player->myGuild();
	if(NULL == pGuild)
		return;
	
	COM_GuildMember* pMember = player->myGuildMember();
	if(NULL == pMember)
		return;

	SkillTable::Core const* pSkCore = SkillTable::getSkillById(skId,1);
	if(NULL == pSkCore)
		return;

	COM_Skill* pSkill = player->findGuildSkill(skId);
	if(NULL == pSkill)
	{
		COM_Skill newSkill;
		newSkill.skillID_ = pSkCore->id_;
		newSkill.skillLevel_ = 1;

		player->guildSkills_.push_back(newSkill);
		SRV_ASSERT(pSkill = player->findGuildSkill(skId));
	}	
	
	pSkCore = SkillTable::getSkillById(pSkill->skillID_,pSkill->skillLevel_);
	if(NULL == pSkCore)
		return;
	
	int level = pGuild->getBuildingLevel(GBT_Goddess);

	if(level * 2 <= pSkill->skillLevel_)
		return ;
	
	if(player->getProp(PT_Money) < Global::get<int>(C_FamilyLearnSkillPay)){
		return ;
	}
	
	player->addMoney(-Global::get<int>(C_FamilyLearnSkillPay));
	
	pSkill->skillExp_ += Global::get<int>(C_FamilySkillExp);
	
	if(pSkill->skillExp_ >= pSkCore->exp_){
		pSkill->skillExp_ -= pSkCore->exp_;
		pSkill->skillLevel_ += 1;
	}
	CALL_CLIENT(player,levelupGuildSkillOk(*pSkill));
} 

void Guild::progenitusAddExp(Player* player,int32 mId,bool isSuper){
	if(NULL == player)
		return;
	Guild* pGuild = player->myGuild();
	if(NULL == pGuild)
		return;

	COM_GuildMember* pMember = player->myGuildMember();
	if(NULL == pMember)
		return;

	COM_GuildProgen* pProgen = pGuild->getProgenitusById(mId);
	if(NULL == pProgen){
		return ;
	}

	if(pGuild->guildData_.fundz_ < (isSuper ? Global::get<int>(C_FamilyProgenitusAddExpSuperPay) : Global::get<int>(C_FamilyProgenitusAddExpPay))){
		return;
	}

	int blev = pGuild->getBuildingLevel(GBT_Progenitus);

	GuildBuildingData const* pData = GuildBuildingData::getGuildBuidingData(GBT_Progenitus,blev);
	
	if(!pData)
		return;

	if(pProgen->lev_ >= pData->number_)
		return;
	
	pGuild->guildData_.fundz_ -= (isSuper ? Global::get<int>(C_FamilyProgenitusAddExpSuperPay) : Global::get<int>(C_FamilyProgenitusAddExpPay));
	pProgen->exp_ +=  (isSuper ? Global::get<int>(C_FamilyProgenitusAddSuperExp) : Global::get<int>(C_FamilyProgenitusAddExp));
	
	Monster2Table::Core const* pExpData = Monster2Table::getMonster2Core(pProgen->mId_,pProgen->lev_);
	while ( pExpData && (pExpData->levelExp_ < pProgen->exp_) ) 
	{
		pProgen->lev_ += 1;
		pProgen->exp_ -= pExpData->levelExp_;
		pExpData = Monster2Table::getMonster2Core(pProgen->mId_,pProgen->lev_);
	} 
	//如果超了 ···· 
	if(pProgen->lev_ >= pData->number_){
		pProgen->lev_ = pData->number_;
		pProgen->exp_ = 0;
	}

	pGuild->broadcaster_.updateGuildFundz(pGuild->guildData_.fundz_);
	pGuild->broadcaster_.progenitusAddExpOk(*pProgen);
	DBHandler::instance()->updateGuild(pGuild->guildData_);
}

void Guild::setProgenitusPosition(Player* player,int32 mId, int32 position){
	if(NULL == player)
		return;
	Guild* pGuild = player->myGuild();
	if(NULL == pGuild)
		return;
	COM_GuildMember* pMember = player->myGuildMember();
	if(NULL == pMember)
		return;
	if(pMember->job_ < GJ_VicePremier)
		return; 
	COM_GuildProgen* pProgen = pGuild->getProgenitusById(mId);
	if(NULL == pProgen && mId != 0)
		return;
	if(position <0 )
		return;
	if(position >= pGuild->guildData_.progenitusPositions_.size())
		return;

	pGuild->guildData_.progenitusPositions_[position] = mId;
	pGuild->broadcaster_.setProgenitusPositionOk(pGuild->guildData_.progenitusPositions_);
	DBHandler::instance()->updateGuild(pGuild->guildData_);
}

int32 Guild::getGuildRank(int32 guildId){
	for(size_t i=0; i<guildList_.size(); ++i)
	{
		if(guildList_[i]->guildData_.guildId_ == guildId)
			return i+1;
	}
	return 0;
}

void Guild::changeNotice(Player* player , const std::string& notice)
{
	if(player == NULL)
		return;
	if(FilterWord::isValid(notice)){
		CALL_CLIENT(player,errorno(EN_FilterWord));
		return ;
	}

	Guild* pGuild = Guild::findGuildByPlayer(player->getGUID());
	if(NULL == pGuild) return ;
	COM_GuildMember * pOffical = pGuild->findMember(player->getGUID());
	SRV_ASSERT(pOffical);
	
	if(pOffical->job_ < GJ_VicePremier)
		return ;		//没有权限
	pGuild->guildData_.notice_ = notice;
	pGuild->updateGuild();
	DBHandler::instance()->updateGuildNotice(pGuild->guildData_.guildId_ , notice);
}
void Guild::changePosition(Player* player , S32 playerId , GuildJob job)
{
	if(player == NULL)
		return;
	if(player->getGUID() == playerId)
		return;
	Guild* pGuid = Guild::findGuildByPlayer(player->getGUID());
	if(NULL == pGuid) return ;
	COM_GuildMember * pOffical = player->myGuildMember();
	SRV_ASSERT(pOffical);

	if(pOffical->job_ < GJ_SecretaryHead){
		CALL_CLIENT(player,errorno(EN_CommandPositionLess));
		return ;//没有权限
	}

	COM_GuildMember* member = pGuid->findMember(playerId);
	if (NULL == member) return; //没有这个成员
	
	if(member->job_ >= pOffical->job_)
	{	
		player->errorMessageToC(EN_CommandPositionLess);
		return;
	}
	if(pOffical->job_<=job){
		//没有权限  操作者不能对比自己职位高的成员进行操作
		CALL_CLIENT(player,errorno(EN_CommandPositionLess));
		return ;
	}

	if(job == GJ_VicePremier && pGuid->getPositionNum(GJ_VicePremier) >= Global::get<int>(C_VicePremierMaxNum))
	{
		CALL_CLIENT(player,errorno(EN_PositionUpMax));
		return;
	}

	DBHandler::instance()->updateMemberPosition(member->roleId_ , GuildJob(job) );
}

void Guild::deposeMemberPosition(Player* player , S32 playerId)
{
}

void Guild::transferPremier(Player*player , S32 playerId){
	if(player == NULL)
		return;
	Guild* pGuid = Guild::findGuildByPlayer(player->getGUID());
	if(NULL == pGuid) return ;
	COM_GuildMember * pOffical = pGuid->findMember(player->getGUID());
	SRV_ASSERT(pOffical);

	if(GJ_Premier != pOffical->job_ ){
		//没有权限
		CALL_CLIENT(player,errorno(EN_CommandPositionLess));
		return ;
	}	

	COM_GuildMember* member = pGuid->findMember(playerId);
	if (NULL == member){
		CALL_CLIENT(player,errorno(EN_GuildNoMember));
		return; //没有这个成员
	} 

	if(GJ_VicePremier != member->job_ ){
		//只能移交给副帮主
		CALL_CLIENT(player,errorno(EN_CommandPositionLess));
		return ;
	}	
	//更新帮主名
	pGuid->guildData_.master_=member->roleId_;
	pGuid->guildData_.masterName_=member->roleName_;
	//通知客户端更新
	pGuid->updateGuild();
	DBHandler::instance()->updateMemberPosition(pOffical->roleId_, GJ_SecretaryHead);
	DBHandler::instance()->updateMemberPosition(member->roleId_, GJ_Premier);
	DBHandler::instance()->updateGuild(pGuid->guildData_);
}

void Guild::transferPremier(S32 officer, S32 playerId){
	Guild* pGuid = Guild::findGuildByPlayer(officer);
	if(NULL == pGuid) return ;
	if(this != pGuid) return ;
	
	COM_GuildMember * pOffical = pGuid->findMember(officer);
	SRV_ASSERT(pOffical);

	if(GJ_Premier != pOffical->job_ ){
		return ;
	}	

	COM_GuildMember* member = pGuid->findMember(playerId);
	if (NULL == member){
		return; //没有这个成员
	} 

	//更新帮主名
	pGuid->guildData_.master_=member->roleId_;
	pGuid->guildData_.masterName_=member->roleName_;
	//通知客户端更新
	pGuid->updateGuild();
	DBHandler::instance()->updateMemberPosition(pOffical->roleId_, GJ_SecretaryHead);
	DBHandler::instance()->updateMemberPosition(member->roleId_, GJ_Premier);
	DBHandler::instance()->updateGuild(pGuid->guildData_);
}

void Guild::addGuildMoney(Player* player , int32 money){
	if(!player->myGuild()){
		CALL_CLIENT(player,errorno(EN_NoGuild));
		return;
	}
	int gmoney = TRANSFOR_GUILD_MONEY(money);
	if(gmoney <= 0){
		CALL_CLIENT(player,errorno(EN_MoneyLess));
		return;
	}
	if(player->getProp(PT_Money) < money){
		CALL_CLIENT(player,errorno(EN_MoneyLess));
		return;
	}
	Guild* pGuild = player->myGuild();
	
	if(pGuild->guildData_.fundz_ + gmoney > pGuild->getGuildMoneyLimit()){
		CALL_CLIENT(player,errorno(EN_AddGuildMoneyMax));
		return;
	}
	
	pGuild->guildData_.fundz_ += gmoney;
	COM_GuildMember *pGuildMember = player->myGuildMember();
	player->addGuildContribution(gmoney);
	pGuild->broadcaster_.updateGuildFundz(pGuild->guildData_.fundz_);
	player->addMoney(-money);
	DBHandler::instance()->updateGuild(pGuild->guildData_);
}

void Guild::addGuildMoneyOnly(Player* player , int32 gmoney){
	if(!player->myGuild()){
		CALL_CLIENT(player,errorno(EN_NoGuild));
		return;
	}
	
	Guild* pGuild = player->myGuild();

	if(pGuild->guildData_.fundz_ + gmoney > pGuild->getGuildMoneyLimit()){
		CALL_CLIENT(player,errorno(EN_AddGuildMoneyMax));
		return;
	}

	pGuild->guildData_.fundz_ += gmoney;
	COM_GuildMember *pGuildMember = player->myGuildMember();
	player->addGuildContribution(gmoney);
	pGuild->broadcaster_.updateGuildFundz(pGuild->guildData_.fundz_);
	DBHandler::instance()->updateGuild(pGuild->guildData_);
}

void Guild::levelupBuilding(Player* player, GuildBuildingType gbt){
	if(gbt <= GBT_MIN || gbt >= GBT_MAX)
		return ;
	

	Guild* pGuild = player->myGuild();
	if(NULL == pGuild)
		return;
	
	COM_GuildMember* pOfficer = player->myGuildMember();
	if(NULL == pOfficer)
		return;
	else if( gbt == GBT_Main && pOfficer->job_ < GJ_VicePremier)
	{
		player->errorMessageToC(EN_CommandPositionLess);
		return; ///
	}
	else if( pOfficer->job_ < GJ_SecretaryHead) {
		player->errorMessageToC(EN_CommandPositionLess);
		return;
	}
	COM_GuildBuilding &mainbuilding = pGuild->guildData_.buildings_[GBT_Main-1];
	COM_GuildBuilding &building = pGuild->guildData_.buildings_[gbt-1];
	GuildBuildingData const* p = GuildBuildingData::getGuildBuidingData(gbt,building.level_);

	if(NULL == p){
		CALL_CLIENT(player,errorno(EN_LevelupGuildBuilding));
		return;
	}
	if(0 == p->needMoney_){
		CALL_CLIENT(player,errorno(EN_LevelupGuildBuildingLevelMax));
		return;
	}
	if(pGuild->guildData_.fundz_ < p->needMoney_){
		CALL_CLIENT(player,errorno(EN_LevelupGuildBuildingMoneyLess));
		return;
	}
	if(GBT_Main != gbt && mainbuilding.level_ <= building.level_){
		CALL_CLIENT(player,errorno(EN_LevelupGuildBuildingHallBuildLevelLess));
		return;
	}

	pGuild->guildData_.fundz_ -= p->needMoney_;
	++(building.level_);
	pGuild->broadcaster_.updateGuildBuilding(gbt,building);
	pGuild->broadcaster_.updateGuildFundz(pGuild->guildData_.fundz_);
	
	DBHandler::instance()->updateGuild(pGuild->guildData_);
}

void Guild::addMemberContribution(Player* player,S32 contri)
{
	if(!DBHandler::instance()->isConnect_){	
		return ;
	}

	Guild* pGuild = findGuildByPlayer(player->getGUID());
	if(NULL == pGuild) 
		return ;
	COM_GuildMember* pMember = pGuild->findMember(player->getGUID());
	SRV_ASSERT(pMember);		
	
	//给帮贡
	player->addGuildContribution(contri);
	
}

int32 Guild::getBuildingLevel(GuildBuildingType gbt){
	if(gbt <= GBT_MIN || gbt >= GBT_MAX)
		return 0;
	return  guildData_.buildings_[gbt-1].level_;
}

COM_GuildProgen*Guild::getProgenitusById(int32 mid){
	for(size_t i=0; i<guildData_.progenitus_.size(); ++i){
		if(guildData_.progenitus_[i].mId_ == mid)
			return &(guildData_.progenitus_[i]);
	}
	return NULL;
}

int32 Guild::getGuildLevel(){
	return getBuildingLevel(GBT_Main);
}

int32 Guild::getMemberLimit(){
	GuildBuildingData const* p = GuildBuildingData::getGuildBuidingData(GBT_Main,guildData_.buildings_[GBT_Main-1].level_);
	if(NULL == p){
		return 0;
	}
	return p->number_;
}
int32 Guild::getGuildMoneyLimit(){
	GuildBuildingData const* p = GuildBuildingData::getGuildBuidingData(GBT_Bank,guildData_.buildings_[GBT_Bank-1].level_);
	if(NULL == p){
		return 0;
	}
	return p->number_;
}

void Guild::updateMemberList( Player* player )
{
	std::vector<COM_GuildMember> members;
	
	for(IdGuildMemberMap::iterator it=guildMember_.begin();it!=guildMember_.end();++it)
	{
		SGE_ContactInfoExt* pCache = WorldServ::instance()->findContactInfoExt(it->second.roleId_);
		if(!pCache){
			ACE_DEBUG((LM_ERROR,"Can not find member in cache %d\n",it->second.roleId_));
			continue;
		}
		it->second.level_ = pCache->level_;
		it->second.profType_ = pCache->job_;
		it->second.profLevel_ = pCache->jobLevel_;
		it->second.contribution_ = pCache->guildContribute_;
		it->second.offlineTime_  =  pCache->logoutTime_;
		Player* pPlayer = Player::getPlayerByInstId(it->second.roleId_);
		if(pPlayer){
			it->second.offlineTime_ = 0;
		}
		members.push_back(it->second);
	}
	CALL_CLIENT(player,initGuildMemberList(members));
}

void Guild::updateMember(COM_GuildMember& member,ModifyListFlag change,bool self)
{
	Player* player=Player::getPlayerByInstId(member.roleId_);
	if(player && player->getClient() && (!self) ){
		broadcaster_.removeChannel(player->getClient());
	}
	
	broadcaster_.modifyGuildMemberList(member,change);

	if(player && player->getClient() && (!self) ){
		broadcaster_.addChannel(player->getClient());
	}
}

void Guild::updateGuild(){
	broadcaster_.initGuildData(guildData_);
}

bool Guild::addMember(COM_GuildMember& member){
	if (findMember(member.roleId_)!=NULL)
		return false;
	
	Player* player=Player::getPlayerByInstId(member.roleId_);
	if(player && player->getClient())
		broadcaster_.addChannel(player->getClient());
	requestGuildShopItems(member);
	guildMember_[member.roleId_]=member;
	broadcaster_.modifyGuildMemberList(member,MLF_Add);
	
	return true;
}

bool Guild::delMember( S32 playerId )
{
	COM_GuildMember *pMember = findMember(playerId);
	if (NULL==pMember)
		return false;
	broadcaster_.modifyGuildMemberList(*pMember,MLF_Delete);
	
	Player* player=Player::getPlayerByInstId(playerId);
	if(player){
		if(player->getClient()){
			broadcaster_.removeChannel(player->getClient());
		}
		
		if(player->isInGuildBattleScene() || player->isInGuildScene() ){
			player->transforHome(); ///可能会卡死
		}
		player->exitGuildTime_ = WorldServ::instance()->curTime_;
	}
	guildMember_.erase(playerId);
	return true;
}

COM_GuildMember* Guild::findMember( S32 playerId )
{
	IdGuildMemberMap::iterator it=guildMember_.find(playerId);
	if(it!=guildMember_.end())
		return &(it->second);
	return NULL;
}

S16 Guild::getPositionNum(GuildJob job)
{
	S16 jobNum = 0;
	for(IdGuildMemberMap::iterator itr=guildMember_.begin() ; itr!=guildMember_.end() ; ++itr){
		if(itr->second.job_ == job){
			++jobNum;
		}
	}
	return jobNum;
}

int	Guild::getOnlinePlayerNum(){
	int tmp = 0;
	for(IdGuildMemberMap::iterator itr=guildMember_.begin() ; itr!=guildMember_.end() ; ++itr){
		if(Player::getPlayerByInstId(itr->second.roleId_) != NULL){
			++ tmp;
		}
	}
	return tmp;
}

bool Guild::addRequestList(COM_GuildRequestData& newRequest){
	guildData_.requestList_.push_back(newRequest) ;
	updateGuild();
	return true;
}
bool Guild::delRequestList( S32 playerId )
{
	std::vector<COM_GuildRequestData>& tmp = guildData_.requestList_;
	for( std::vector<COM_GuildRequestData>::iterator itr = tmp.begin() ; itr!=tmp.end() ; ++itr)
	{
		if ( (*itr).roleId_ == playerId)
		{
			tmp.erase(itr);
			break;
		}
	}	
	updateGuild();
	return true;
}
const COM_GuildRequestData* Guild::findRequest(S32 roleId)
{
	for (size_t i=0 ; i<guildData_.requestList_.size() ; ++i)
	{
		if(guildData_.requestList_[i].roleId_ == roleId)
		{
			return & guildData_.requestList_[i];
		}
	}
	return NULL;
}
inline bool requestRange(S16 totalNum,S16 page,S16& pageNum,S16& start,S16& end,S16 listNum=7)
{
	pageNum = ((totalNum%listNum) == 0) ? (totalNum/listNum) :(totalNum/listNum + 1);
	if ( page < 0 || page > pageNum )
		return false;	
	//从第几个开始
	start = page *listNum;

	if(start >= totalNum)
		return false;

	//到第几个
	end   = (totalNum < (start +listNum)) ?totalNum : (start +listNum);
	return true;
}

void Guild::queryGuildList(Player* player , S16 page )
{
	S16 pageNum=0,start=0, end=0;
	if(!requestRange(guilds_.size(),page,pageNum,start,end))
		return;

	//填充此页的帮会信息
	IdGuildMap::iterator it=guilds_.begin();
	for(size_t i=0;i<start;i++)
		++it;
	std::vector<COM_GuildViewerData>	guildList;
	for (start;start<end;start++)
	{
		Guild* guild =  it->second;
		COM_GuildViewerData	guildViewerData;
		guild->exportGuildViewerData(guildViewerData);
		guildList.push_back(guildViewerData);
	
		++it;
	}

	CALL_CLIENT(player,queryGuildListResult(page,pageNum,guildList));
}

void Guild::exportGuildViewerData( COM_GuildViewerData& guildViewerData )
{
	guildViewerData.guid_		= guildData_.guildId_;
	guildViewerData.guildName_	= guildData_.guildName_;
	guildViewerData.playerName_	= guildData_.masterName_;
	guildViewerData.level_		= getGuildLevel();
	guildViewerData.memberNum_	= guildMember_.size();
	guildViewerData.memberTotal_= getMemberLimit();
	guildViewerData.notice_		= guildData_.notice_;
	guildViewerData.guildRank_	= getGuildRank(guildData_.guildId_);
}

void Guild::checkPresent(){
	int l = getBuildingLevel(GBT_Collection);
	GuildBuildingData const * p = GuildBuildingData::getGuildBuidingData(GBT_Collection,l);
	std::vector<std::string> recvs;
	for(IdGuildMemberMap::iterator i=guildMember_.begin(),e=guildMember_.end(); i!=e; ++i){
		recvs.push_back(i->second.roleName_);
	}
	while(guildData_.presentNum_ >= p->number_){
		COM_MailItem item;
		item.itemId_ = Global::get<int>(Constant(C_FamilyPresentMailItemId0 + l - 1));
		item.itemStack_ = 1;
		std::vector<COM_MailItem> items;
		items.push_back(item);
		std::string sender = Global::get<std::string>(C_FamilyPresentMailSender);
		std::string title = Global::get<std::string>(C_FamilyPresentMailTitle);
		std::string content = Global::get<std::string>(C_FamilyPresentMailContent);
		WorldServ::instance()->sendMail(sender,recvs,title,content,0,0,items);
		guildData_.presentNum_ -= p->number_;
	}
}

void Guild::checkFundz(){
	int32 pay = Global::get<int>(C_FamilyOneDayFundzLose) * getGuildLevel();

	if(guildData_.fundz_ < pay){
		guildData_.noFundzDays_ -= 1;	
	}
	else {
		guildData_.fundz_ -= pay;
		broadcaster_.updateGuildFundz(guildData_.fundz_);
		guildData_.noFundzDays_ = 0;
	}
	int32 days = Global::get<int>(C_FamilyNoMoneyDays) + guildData_.noFundzDays_;

	if(days <=0){
		int32 index = UtlMath::randNM(GBT_Bank,GBT_Progenitus);
		if(guildData_.buildings_[index].level_ > 1){
			guildData_.buildings_[index].level_ -= 1;
			broadcaster_.updateGuildBuilding((GuildBuildingType)index,guildData_.buildings_[index]);
		}
	}
	DBHandler::instance()->updateGuild(guildData_);
}

int32 Guild::getTotalFF()const{
	int32 ff = 0;
	for (IdGuildMemberMap::const_iterator i=guildMember_.begin(),e=guildMember_.end(); i!=e; ++i)
	{
		COM_ContactInfo* pInfo = WorldServ::instance()->findContactInfo(i->second.roleName_);
		if(pInfo){
			ff += pInfo->ff_;
		}
	}
	return ff;
}

int32 Guild::sumInBattleSceneMembers(){
	int32 sum = 0;
	for (IdGuildMemberMap::iterator i=guildMember_.begin(); i!=guildMember_.end(); ++i){
		Player* p = Player::getPlayerByInstId(i->second.roleId_);
		if(p){
			if(p->sceneId_ == battleSceneCopyId_){
				++sum;
			}
		}
	}
	return sum;
}

void Guild::openGuildBattle(int32 otherId,bool needEvt){
	
	Guild* pGuild = Guild::findGuildById(otherId);
	if(NULL == pGuild)
		return;
	
	broadcaster_.openGuildBattle(pGuild->guildData_.guildName_,pGuild->getOnlinePlayerNum(),pGuild->getGuildLevel(),isLeft_,battleIntervalTime_);
	otherGuildName_ = pGuild->guildData_.guildName_;

	if(!needEvt)
		return;

	enum {
		ARG_OTHER_ID,
		ARG_MAX_,
	};
	GEParam param[ARG_MAX_];
	param[ARG_OTHER_ID].type_ = GEP_INT;
	param[ARG_OTHER_ID].value_.i = guildData_.guildId_;

	GameEvent::procGameEvent(GET_OpenGuildBattle,param,ARG_MAX_,0);
}

void Guild::closeGuildBattle(bool isWin){
	broadcaster_.closeGuildBattle(isWin);
	enum {
		ARG_OTHER_ID,
		ARG_IS_WIN,
		ARG_MAX_,
	};
	GEParam param[ARG_MAX_];
	param[ARG_OTHER_ID].type_ = GEP_INT;
	param[ARG_OTHER_ID].value_.i = guildData_.guildId_;
	param[ARG_IS_WIN].type_ = GEP_INT;
	param[ARG_IS_WIN].value_.i = (int)isWin;

	GameEvent::procGameEvent(GET_CloseGuildBattle,param,ARG_MAX_,0);

	bInfo_ = NULL;
	battleWinCount_ = 0;
	otherGuildName_ = "";
}

void Guild::addGuildBattleWinCount(int32 i){
	if(!bInfo_)
		return;
	battleWinCount_ += i;
	Guild* pOther = bInfo_->getOtherGuild(this);
	broadcaster_.syncGuildBattleWinCount(battleWinCount_,(pOther?pOther->battleWinCount_:0));

	if(pOther){
		pOther->broadcaster_.syncGuildBattleWinCount(pOther->battleWinCount_,battleWinCount_);
	}
}

void Guild::sendMemberMail( std::string &sender,std::string &title, std::string &content, int32 dia, int32 money,std::vector<COM_MailItem> &items){
	std::vector<std::string> names;
	for(IdGuildMemberMap::iterator i=guildMember_.begin(); i!=guildMember_.end(); ++i){
		names.push_back(i->second.roleName_);
	}
		
	WorldServ::instance()->sendMail(sender,names,title,content,money,dia,items);
}

COM_GuildMember* Guild::getLeader(){
	for(IdGuildMemberMap::iterator i=guildMember_.begin(), e = guildMember_.end(); i!=e; ++i){
		if(i->second.job_ == GJ_Premier)
			return &(i->second);
	}
	return NULL;
}

void Guild::guildsign(Player* player){
	if(player == NULL)
		return;
	COM_GuildMember* member = player->myGuildMember();
	if(member == NULL)
		return;
	if(member->signflag_)
		return;
	player->giveDrop(Global::get<int>(C_FamilySignDrop));
	member->signflag_ = true;
	player->updateMyGuildMember();
}

void Guild::resetGuildSign(){
	for(IdGuildMemberMap::iterator i=guildMember_.begin(),e=guildMember_.end();i != e; ++i){
		i->second.signflag_ = false;
		Player* p = Player::getPlayerByInstId(i->first);
		if(p){
			p->updateMyGuildMember();
		}
	}
}