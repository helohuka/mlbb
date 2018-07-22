#include "team.h"
#include "Scene.h"
#include "sceneplayer.h"
#include "Quest.h"
#include "challengeTable.h"
#include "battle.h"
#include "GameRuler.h"
#include "Guild.h"
#include "pvpJJC.h"
#include "robotTable.h"
#include "account.h"

void
Team::release()
{
	isUsed_ = false;
	maxMemberSize_ = 0;
	teamType_ = TT_None;
	teamName_ = "";
	teamPassword_ = "";
	teamMembers_.clear();
	teamblacklist_.clear();
	TeamLobby::instance()->syncDelLobbyTeam(teamId_);
}

void Team::create(Player* player,COM_CreateTeamInfo& cti)
{
	std::string userName = player->account_->username_;
	bool isrobot = RobotActionTable::isRobot(userName);
	if(isrobot){
		robotCreatTeam(player,cti);
		return;
	}
	if(player->getProp(PT_Level) < 10)
		return;
	if(player->isTeamMember())
		return;
	isUsed_ = true;
	maxMemberSize_ = cti.maxMemberSize_ > 5 ? 5 : cti.maxMemberSize_;
	teamType_ = cti.type_;
	teamName_ = cti.name_;
	teamPassword_ = cti.pwd_;
	teamIsRunning_ = false;
	teamisWarrior_ = false;
	teamMinLevel_ = cti.minLevel_;
	teamMaxLevel_ = cti.maxLevel_;
	teamMembers_.push_back(player);
	teamJJCTimes_ = 0;
	player->teamId_ = teamId_;
	COM_TeamInfo info;
	getTeamInfo(info);
	CALL_CLIENT(player,createTeamOk(info));
	player->myScene()->setTeamLeader(player->getGUID(),true);
	player->myScene()->updateTeamMember(player->getGUID(),true);
	addChannel(player->getClient());
	{
		COM_SimpleTeamInfo* pinfo = (COM_SimpleTeamInfo*)&info;
		TeamLobby::instance()->exitLobby(player);
		TeamLobby::instance()->syncAddLobbyTeam(*pinfo);
		TeamLobby::instance()->syncUpdateLobbyTeam(*pinfo);
	}
}

void Team::change(Player* player, COM_CreateTeamInfo& cti)
{
	maxMemberSize_	=  cti.maxMemberSize_ > 5 ? 5 : cti.maxMemberSize_;
	teamType_		= cti.type_;
	teamName_		= cti.name_;
	teamMinLevel_ = cti.minLevel_;
	teamMaxLevel_ = cti.maxLevel_;
	teamPassword_ = cti.pwd_;

	{
		COM_SimpleTeamInfo info;
		getTeamSimpleInfo(info);
		TeamLobby::instance()->syncUpdateLobbyTeam(info);
	}
	{
		COM_TeamInfo info;
		getTeamInfo(info);
		updateTeam(info);
	}
}

void
Team::getTeamSimpleInfo(COM_SimpleTeamInfo& info){
	SRV_ASSERT(getLeader());
	info.leaderName_ = getLeader()->getNameC();
	info.job_ = (JobType)(int)getLeader()->getProp(PT_Profession);
	info.joblevel_ = (int)getLeader()->getProp(PT_ProfessionLevel);
	info.needPassword_ = !teamPassword_.empty();
	info.isRunning_	= teamIsRunning_;
	info.teamId_ = teamId_;
	info.type_ = teamType_;
	info.curMemberSize_ = teamMembers_.size();
	info.maxMemberSize_ = maxMemberSize_;
	info.name_ = teamName_;
	info.minLevel_ = teamMinLevel_;
	info.maxLevel_ = teamMaxLevel_;

	info.pwd_ = teamPassword_;
}

void
Team::getTeamInfo(COM_TeamInfo& info)
{
	getTeamSimpleInfo(info);
	for(size_t i=0; i<teamMembers_.size(); ++i)
	{
		COM_SimplePlayerInst simp;
		teamMembers_[i]->getSimplePlayerInst(simp);
		info.members_.push_back(simp);
	}
	info.job_ = (JobType)(U32)getLeader()->getProp(PT_Profession);
	info.joblevel_ = (U32)getLeader()->getProp(PT_ProfessionLevel);
}

bool 
Team::addMember(Player* player)
{
	if(player == NULL)
		return false;

	if(isTeamMember(player))
	{
		CALL_CLIENT(player,errorno(EN_InTeam));
		return false;
	}	
	if(teamMembers_.size() >= maxMemberSize_ )
	{
		CALL_CLIENT(player,errorno(EN_TeamIsFull));
		return false;
	}
	if(isunwelcome(player->getGUID()))
	{
		CALL_CLIENT(player,errorno(EN_InTeamBlackList));
		return false;
	}

	static std::string luacheck = "check_team_addmember";
	static std::string luaerror = "";
	static int luacheckret = 0;
	enum{
		ARG0,
		ARG1,
		ARG_MAX_,
	};
	static GEParam luaparams[ARG_MAX_];
	luaparams[ARG0].type_ = GEP_HANDLE;
	luaparams[ARG0].value_.p = player;
	luaparams[ARG1].type_ = GEP_INT;
	luaparams[ARG1].value_.i = teamId_;
	if(!ScriptEnv::callGEProc(luacheck.c_str(),getLeader()->getHandleId(),luaparams,ARG_MAX_,luacheckret,luaerror)){
		return false;
	}
	if(!luacheckret)
		return false;
	
	///判断是否暂离状态
	player->isLeavingTeam_ = !((player->sceneId_ == getLeader()->sceneId_ ) && IsInRange(player->position_,getLeader()->position_,3.F));

	{
		COM_SimplePlayerInst joinsimp;
		player->getSimplePlayerInst(joinsimp);

		addTeamMember(joinsimp);

		player->myScene()->updateTeamMember(player->getGUID(),true);

		if(player->getClient())
			addChannel(player->getClient());
		teamMembers_.push_back(player);
		player->teamId_ = teamId_;
	}
	
	COM_TeamInfo info;	
	getTeamInfo(info);
	CALL_CLIENT(player,joinTeamOk(info));
	if(!player->isLeavingTeam_)
		getLeader()->scenePlayer_->addFollow(player->playerId_);
	return true;
}

void Team::exitTeam(Player* player){
	if(!isTeamMember(player))
		return;

	bool isLeader = isTeamLeader(player);
	delMember(player);
	sortMemberByLeave();
	if(isLeader && !teamMembers_.empty()){
		Player* leaderMember = teamMembers_[0];
		if(!leaderMember->isLeavingTeam_){
			changeTeamLeaderOk(leaderMember->getGUID());
			//leaderMember->myScene()->setTeamLeader(leaderMember->getGUID(),true);
			for(size_t i=1; i<teamMembers_.size(); ++i){
				if(!teamMembers_[i]->isLeavingTeam_){
					leaderMember->scenePlayer_->addFollow(teamMembers_[i]->getGUID());
				}
			}
		
		}else{
			while(!teamMembers_.empty()){
				delMember(teamMembers_.back());
			}
		}
	}
}

void Team::delMember(S32 playerId, bool iskick){
	Player* p = findMemberById(playerId);
	if(NULL == p)
		return;
	delMember(p,iskick);
}

void 
Team::delMember(Player* player,bool iskick)
{
	if(player == NULL)
		return;

	if(!isTeamMember(player))
		return;
	
	bool isLeader = isTeamLeader(player);
	if(isLeader)
	{
		player->scenePlayer_->delFollows();
		Scene* s = player->myScene();
		if(s)
			s->setTeamLeader(player->getGUID(),false);
	}
	else
	{
		Player * leader = getLeader();
		leader->scenePlayer_->delFollow(player->playerId_);
	}
	std::vector<Player*>::iterator itr = std::find(teamMembers_.begin(),teamMembers_.end(),player);
	teamMembers_.erase(itr);
	if(player->getClient())
		removeChannel(player->getClient());
	delTeamMember(player->getGUID());
	player->teamId_ = 0;
	player->isLeavingTeam_ = false;
	player->cleanTeamQuest();
	player->cleanCopyQuest();
	CALL_CLIENT(player,exitTeamOk(iskick));
	
	player->myScene()->updateTeamMember(player->getGUID(),false);

	if(iskick)
		teamblacklist_.push_back(player->getGUID());
	if(teamMembers_.empty())
	{
		release();
		ACE_DEBUG((LM_DEBUG,ACE_TEXT("player[%d] exit team release\n"),player->getGUID()));
	}
	else 
	{
		if(isLeader)
		{
			Player * leader = getLeader();
			//leader->scenePlayer_->delFollow(player->playerId_);
			leader->myScene()->setTeamLeader(leader->getGUID(),true);
		}
		ACE_DEBUG((LM_DEBUG,ACE_TEXT("player[%d] exit team\n"),player->getGUID()));
	}
}

void Team::leaveTeam(Player* player){
	if(!isTeamMember(player))
		return ;
	if(isTeamLeader(player))
		return;
	if(!player->isLeavingTeam_){
		player->isLeavingTeam_ = true;
		leaveTeamOk(player->playerId_);
		getLeader()->scenePlayer_->delFollow(player->playerId_);
	}
}
void Team::backTeam(Player* player){
	if(!isTeamMember(player))
		return ;
	if(isTeamLeader(player))
		return;
	if(player->getProp(PT_Level) < 20)
	{
		Scene *sd = SceneManager::instance()->getScene(getLeader()->sceneId_);
		if(sd == NULL)
			return;
		if(sd->sceneType_ == SCT_TeamPK)
		{
			CALL_CLIENT(player,errorno(EN_NoBackTeam));
			return;
		}
		if(sd->sceneType_ == SCT_Instance)
		{
			CALL_CLIENT(player,errorno(EN_NoBackTeam));
			return;
		}
	}
	if(player->isLeavingTeam_){
		
		getLeader()->scenePlayer_->addFollow(player->playerId_);
		//player->move(getLeader()->position_);
		player->isLeavingTeam_ = false;
		backTeamOK(player->playerId_);
	}
}

void 
Team::sortMemberByLeave(){
	

	for (int i = 0; i < teamMembers_.size(); i++)
	{
		for (int j = i; j < teamMembers_.size(); j++)
		{
			if (teamMembers_[i]->isLeavingTeam_ > teamMembers_[j]->isLeavingTeam_)
			{
				Player* temp = teamMembers_[i];
				teamMembers_[i] = teamMembers_[j];
				teamMembers_[j] = temp;
			}
		}
	}

}

U32
Team::getTeamLeaveNum(){
	U32 index = 0;
	for (size_t i=0; i<teamMembers_.size();++i)
	{
		if(teamMembers_[i]->isLeavingTeam_)
			++index;
	}
	return index;
}

bool
Team::isTeamMember(Player* player)
{
	if(teamMembers_.empty())
		return false;

	return std::find(teamMembers_.begin(),teamMembers_.end(),player) != teamMembers_.end();
}

Player*
Team::findMemberById(U32 uPlayerId)
{
	for (size_t i = 0; i < teamMembers_.size(); ++i)
	{
		if (teamMembers_[i]->getGUID() == uPlayerId)
		{
			return teamMembers_[i];
		}
	}

	return NULL;
}

void
Team::changeTeamLeader(U32 targetId)
{
	Player* player = findMemberById(targetId);

	if(player == NULL)
		return;

	Player* leader = getLeader();
	if(leader == player)
		return;
	if(player->isLeavingTeam_)
		return; //不能给暂离的人
	
	leader->myScene()->setTeamLeader(leader->getGUID(),false);
	player->myScene()->setTeamLeader(player->getGUID(),true);
	leader->scenePlayer_->delFollows();
	teamMembers_.erase(std::find(teamMembers_.begin(),teamMembers_.end(),player));
	teamMembers_.insert(teamMembers_.begin(),player);
	
	for(size_t i=1; i<teamMembers_.size(); ++i){
		if(!teamMembers_[i]->isLeavingTeam_)
			getLeader()->scenePlayer_->addFollow(teamMembers_[i]->playerId_);
	}
	changeTeamLeaderOk(player->getGUID());   
}

bool
Team::checkPassword(std::string pwd)
{
	if(teamPassword_.empty())
		return true;

	if(strcmp(teamPassword_.c_str(),pwd.c_str()) == 0)
		return true;

	return false;
}

void
Team::changeTeamPassword(std::string pwd){
	teamPassword_ = pwd;
}

void
Team::inviteTeamMember(Player* player,std::string name)
{
	if(player == NULL)
		return;

	if(strcmp(player->getNameC(),name.c_str()) == 0)
		return;

	if(!player->isTeamLeader())
	{
		CALL_CLIENT(player,errorno(EN_NoTeamLeader));
		return;
	}

	if(teamMembers_.size() >= maxMemberSize_ )
	{
		CALL_CLIENT(player,errorno(EN_TeamIsFull));
		return;
	}

	Player* p = Player::getPlayerByName(name);

	if(p == NULL){
		CALL_CLIENT(player,errorno(EN_CannotfindPlayer));
		return;
	}
	if(p->isTeamMember()){
		CALL_CLIENT(player,errorno(EN_PlayerIsInTeam));
		return;
	}

	if(!p->getOpenSubSystemFlag(OSSF_Team)){
		CALL_CLIENT(player,errorno(EN_NoSubSyste));
		return;
	}

	if(Guild::isBattleOpen()){
		if(getLeader()->isInGuildBattleScene()){
			//在帮派战场景
			if(p->isInGuildBattleScene()){
				if(getLeader()->myGuild() != p->myGuild()){
					getLeader()->errorMessageToC(EN_GuildBattleTeamNoSameGuild);
					return;
				}
			}else{
				getLeader()->errorMessageToC(EN_GuildBattleHasTeam);
				return;
			}
		}
		else{
			if(p->isInGuildBattleScene()){
				getLeader()->errorMessageToC(EN_GuildBattleHasTeam);
				return;
			}
		}
	}

	if(getLeader()->sceneId_ == SceneTable::getGuildHomeScene()->sceneId_){
		if(getLeader()->myGuild() != p->myGuild()){
			player->errorMessageToC(EN_TeamMemberNoGuild);
			return;
		}
	}

	CALL_CLIENT(p,inviteJoinTeam(teamId_,player->getNameC()));
}

void Team::createBttleUnit(Battle* battle,GroupType gt,bool hasLeaderEmployee){
	for (size_t i=0; i<teamMembers_.size(); ++i)
	{
		if(!teamMembers_[i]->isLeavingTeam_)
			battle->createPlayerUnit(teamMembers_[i],gt);
	}
	if(hasLeaderEmployee)
		battle->createPlayerEmployees(getLeader());
}

bool Team::isBattle(){
	for (size_t i = 0; i < teamMembers_.size(); ++i)
	{
		if(teamMembers_[i]->isBattle())
			return true;
	}
	return false;
}

bool Team::canPetActivity(S32 battleId){
	if(teamMembers_.size() < 3)
		return false;
	for (size_t i = 0; i < teamMembers_.size(); ++i)
	{
		if(!GameRuler::CanPetActivity(teamMembers_[i],battleId))
		{
			CALL_CLIENT(getLeader(),petActivityNoNum(teamMembers_[i]->getNameC()));
			return false;
		}
	}
	return true;
}

bool Team::canHundredBattle(S32 battleId){
	ChallengeTable::Core const *tmp = ChallengeTable::getDataByBattleId(battleId);
	if(tmp == NULL)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Don't find this Data[%d] in the ChallengeTable table\n"),battleId));
		return false;
	}
	ErrorNo en;
	for (size_t i = 0; i < teamMembers_.size(); ++i)
	{
		en = GameRuler::CanHundredBattle(teamMembers_[i],battleId);
		if(en != EN_None)
		{
			if(en != EN_Max)
				CALL_CLIENT(getLeader(),teamerrorno(teamMembers_[i]->getNameC(),en));
			return false;
		}
	}

	for(size_t i=0; i<teamMembers_.size(); ++i){

		teamMembers_[i]->curTier_ = tmp->id_;
		teamMembers_[i]->scenePlayer_->setEntryFlag(teamMembers_[i]->playerId_,false);
	}
	return true;
}
bool Team::isSameGuild(){
	Guild* p = teamMembers_[0]->myGuild();
	for(size_t i=0; i<teamMembers_.size(); ++i)
	{
		if(p != teamMembers_[i]->myGuild())
			return false;
	}
	return true;
}

void Team::acceptTeamQuest(Player* leader, S32 questId){
	if(!(isTeamMember(leader) && isTeamLeader(leader))){
		return ; ///不是队长不能接取任务
	}
	
	const Quest* q = Quest::getQuestById(questId);
	if(NULL == q)
		return ;
	if(q->questKind_ == QK_Guild)
		return;//家族任务组队不能接
	if(q->questKind_ == QK_Tongji){
		if(leader->hasQuestByType(QK_Tongji))
			return;	
		S32 level = getMinLevel();
		q = Quest::randomTongjiQuest(level);
		if(q == NULL)
			return ;
		questId = q->questId_;

		if(teamMembers_.size() < Global::get<int>(C_TongjiTeamSizeMin)){
			CALL_CLIENT(leader,errorno(EN_TeamSizeTongjiError));
			return;
		} else {
			for (size_t i=0; i<teamMembers_.size(); ++i){
				if(teamMembers_[i]->getProp(PT_Level) < Global::get<int>(C_TongjiTeamMemberLevelMin)){
					CALL_CLIENT(leader,teamerrorno(teamMembers_[i]->getNameC(),EN_TongjiTeamLevelTooLow));
					return;
				}
				else if(true == teamMembers_[i]->hasQuestByType(QK_Tongji)){
					teamMembers_[i]->cleanTeamQuest();
				}
			}
		}
	}
	
	for (size_t i=0; i<teamMembers_.size(); ++i)
	{
		teamMembers_[i]->acceptQuest(questId);
	}
	
}
void Team::submitTeamQuest(Player* leader,S32 npcId, S32 questId, int32 instId){
	if(!(isTeamMember(leader) && isTeamLeader(leader))){
		return ; ///不是队长不能接取任务
	}
	const Quest* quest = Quest::getQuestById(questId);
	if(NULL == quest){
		CALL_CLIENT(leader,errorno(EN_AcceptQuestNotFound));
		return;
	}
	
	for (size_t i=0; i<teamMembers_.size(); ++i)
	{
		teamMembers_[i]->submitQuest(npcId,questId,instId);
	}

}
void Team::giveupTeamQuest(Player* leader, S32 questId){
	if(!(isTeamMember(leader) && isTeamLeader(leader))){
		return ; ///不是队长不能接取任务
	}
	
	for (size_t i=0; i<teamMembers_.size(); ++i)
	{
		teamMembers_[i]->giveupQuest(questId);
	}
}

void Team::teamAddActivation(ActivityType type,int count)
{
	for (size_t i=0; i<teamMembers_.size(); ++i)
	{
		if(teamMembers_[i]->isLeavingTeam_)
			continue;
		teamMembers_[i]->addActivation(type,count);
	}
}

bool Team::isunwelcome(U32 playerId){
	for (size_t i=0; i<teamblacklist_.size();++i)
	{
		if(teamblacklist_[i] == playerId)
			return true;
	}
	return false;
}

float Team::calcTeamMemberExp(float exp){
	if(teamMembers_.size() < 2)
		return exp;
	return exp + exp*teamMembers_.size()*0.02f;
}

S32 Team::getMinLevel(){
	S32 level = teamMembers_[0]->getProp(PT_Level);
	for (size_t i=0; i<teamMembers_.size();++i)
	{
		if(teamMembers_[i]->getProp(PT_Level) < level)
			level = teamMembers_[i]->getProp(PT_Level);
	}
	return level;
}

void Team::robotCreatTeam(Player* player,COM_CreateTeamInfo& cti){
	if(player->isTeamMember())
		return;
	isUsed_ = true;
	maxMemberSize_ = cti.maxMemberSize_ > 5 ? 5 : cti.maxMemberSize_;
	teamType_ = cti.type_;
	teamName_ = cti.name_;
	teamPassword_ = cti.pwd_;
	teamIsRunning_ = false;
	teamisWarrior_ = false;
	teamMinLevel_ = cti.minLevel_;
	teamMaxLevel_ = cti.maxLevel_;
	teamMembers_.push_back(player);
	teamJJCTimes_ = 0;
	player->teamId_ = teamId_;
	COM_TeamInfo info;
	getTeamInfo(info);
	CALL_CLIENT(player,createTeamOk(info));
	player->myScene()->setTeamLeader(player->getGUID(),true);
	addChannel(player->getClient());
	{
		COM_SimpleTeamInfo* pinfo = (COM_SimpleTeamInfo*)&info;
		TeamLobby::instance()->exitLobby(player);
		TeamLobby::instance()->syncAddLobbyTeam(*pinfo);
		TeamLobby::instance()->syncUpdateLobbyTeam(*pinfo);
	}

	for (size_t i=0; i<RobotActionTable::actiondata_.size();++i)
	{
		if(RobotActionTable::actiondata_[i]->userName_ == player->account_->username_)
			continue;
		if(RobotActionTable::actiondata_[i]->actionType_ != RAT_TeamMove)
			continue;
		Team* pteam = player->myTeam();
		if(pteam == NULL)
			return;
		Player* pp = Player::getPlayerByName(RobotActionTable::actiondata_[i]->robotName_);
		if(pp == NULL)
			continue;
		pteam->addMember(pp);
	}
}

//////////////////////////////////////////////////////////////////////////
TeamLobby::TeamLobby(){
	for(size_t i=0; i<Global::get<int>(C_TeamMaxSize); ++i)
	{
		Team* p = NEW_MEM(Team,i+1);
		teamCache_.push_back(p);
	}
}
TeamLobby::~TeamLobby(){
	for (size_t i=0; i<teamCache_.size(); ++i)
	{
		DEL_MEM(teamCache_[i]);
	}
	teamCache_.clear();
}

void TeamLobby::delChannel(Player* p){
	if(players_.find(p) != players_.end())
		removeChannel(p->getClient());
}

void TeamLobby::joinLobby(Player* p){
	std::vector<COM_SimpleTeamInfo> infos;
	getLobbyTeams(infos);

	for (size_t i = 0; i < infos.size(); ++i)
	{
		Team* t = findTeam(infos[i].teamId_);
		if(t == NULL)
			continue;
		infos[i].isWelcome_ = t->isunwelcome(p->getGUID());
	}

	players_.insert(p);
	addChannel(p->getClient());
	CALL_CLIENT(p,jointLobbyOk(infos));
}

void TeamLobby::exitLobby(Player* p){
	int32 r = players_.erase(p);
	if(r!=0)
	{
		removeChannel(p->getClient());
		CALL_CLIENT(p,exitLobbyOk());
	}
}

Team* TeamLobby::getTeam(Player* p, int32 teamId){
	Team* t = findTeam(teamId);
	if(!t)
		return NULL;
	if(!t->isUsed_)
		return NULL;
	if(!t->isTeamMember(p))
		return NULL;
	return t;
}
Team* TeamLobby::getTeam(int32 teamId){
	Team* t = findTeam(teamId);
	if(!t)
		return NULL;
	if(!t->isUsed_)
		return NULL;
	return t;
}

Team* TeamLobby::apply(){
	for (size_t i=0; i<teamCache_.size(); ++i)
	{
		if(!(teamCache_[i]->isUsed_))
			return teamCache_[i];
	}
	return NULL;
}

Team* TeamLobby::findTeam(int32 teamId){
	if(teamId <=0 || teamId > teamCache_.size())
		return NULL;
	return teamCache_[teamId-1];
}

void TeamLobby::getLobbyTeams(std::vector<COM_SimpleTeamInfo>& teams){
	for(size_t i=0; i<teamCache_.size(); ++i){
		if(teamCache_[i]->isUsed_){
			COM_SimpleTeamInfo info;
			teamCache_[i]->getTeamSimpleInfo(info);
			teams.push_back(info);
		}
	}
}