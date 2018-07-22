#include "player.h"
#include "team.h"
#include "sceneplayer.h"
#include "Guild.h"
#include "pvpJJC.h"
#include "scenetable.h"
void
Player::joinTeam(U32 teamId,STRING& pwd)
{
	if(isBattle())
		return;
	if(isTeamMember())
	{
		CALL_CLIENT(this,errorno(EN_InTeam));
		return;
	}
	Team* p = TeamLobby::instance()->getTeam(teamId);

	if(p == NULL)
	{
		errorMessageToC(EN_NoTeam);
		return;
	}

	if(Guild::isBattleOpen()){
		if(p->getLeader()->isInGuildBattleScene()){
			//在帮派战场景
			if(isInGuildBattleScene()){
				if(myGuild() != p->getLeader()->myGuild()){
					errorMessageToC(EN_GuildBattleTeamNoSameGuild);
					return;
				}
			}else{
				errorMessageToC(EN_GuildBattleHasTeam);
				return;
			}
		}
		else{
			if(isInGuildBattleScene()){
				errorMessageToC(EN_GuildBattleHasTeam);
				return;
			}
		}
	}

	if(sceneId_ == SceneTable::getGuildHomeScene()->sceneId_){
		if(myGuild() != p->getLeader()->myGuild())
		{
			errorMessageToC(EN_TeamMemberNoGuild);
			return;
		}
	}
	
	/*if(p->getLeader()->myGuild() && p->getLeader()->sceneId_ == p->getLeader()->myGuild()->getBattleSceneId()){
		if(p->getLeader()->myGuild() != myGuild())
		{
			CALL_CLIENT(this,errorno(EN_TeamIsRunning));
			return;
		}
	}*/
	if(!p->checkPassword(pwd))
	{
		CALL_CLIENT(this,errorno(EN_TeamPassword));
		return;
	}

	if(p->teamIsRunning_)
	{
		CALL_CLIENT(this,errorno(EN_TeamIsRunning));
		return;
	}
	if(getProp(PT_Level) > p->teamMaxLevel_ || getProp(PT_Level) < p->teamMinLevel_){
		errorMessageToC(EN_NoTeamExist);
		return;
	}

	if(Warriorchoose::instance()->isWarriorchoose(teamId))
		Warriorchoose::instance()->teamWarriorstop(p);
	p->addMember(this);
	COM_TeamInfo info;
	p->getTeamInfo(info);
	COM_SimpleTeamInfo* pinfo = (COM_SimpleTeamInfo*)&info;
	TeamLobby::instance()->syncUpdateLobbyTeam(info);
}

void Player::leaveTeam(){	

	if(isBattle())
		return;
	Scene* s = SceneManager::instance()->getScene(sceneId_);
	if(!s)
		return;
	if(s->sceneType_ == SCT_Instance)
		return;
	Team* p = isTeamMember();
	if(p && !isTeamLeader())
	{
		if(isLeavingTeam_)
			return;
		/*if(Guild::isBattleOpen()){
			if(p->getLeader()->isInGuildBattleScene()){
				return;
			}
		}*/
		if(Warriorchoose::instance()->isWarriorchoose(p->teamId_))
			Warriorchoose::instance()->teamWarriorstop(p);
		p->leaveTeam(this);
	} 
}

void Player::backTeam(){

	if(isBattle())
		return;
	Team* p = isTeamMember();
	if(p && !isTeamLeader())
	{
		if(!isLeavingTeam_)
			return;
		if(p->getLeader() == NULL)
			return;
		Scene* s = SceneManager::instance()->getScene(p->getLeader()->sceneId_);
		if(!s)
			return;
		if(s->sceneType_ == SCT_Instance){
			errorMessageToC(EN_NoBackTeam);
			return;
		}
		else if (s->sceneType_ == SCT_Bairen){
			if(!getOpenSubSystemFlag(OSSF_Hundred)){
				errorMessageToC(EN_HunderdLevel);
				return;
			}
			if(getProp(PT_Level) < Global::get<int>(C_HundredBattle)){
				errorMessageToC(EN_HunderdLevel);
				return;
			}
			if(hundredNum_ < Global::get<int>(C_HundredChallengeNum)){
				errorMessageToC(EN_HunderdNoNum);
				return;
			}
			if(p->getLeader()->tier_ > tier_){
				tier_ = p->getLeader()->tier_;
			}
		}
		else if(s->sceneType_ == SCT_GuildHome){
			if(myGuild() != p->getLeader()->myGuild()){
				errorMessageToC(EN_BackTeamCommandLeaderInGuildHomeSceneAndYouAreNotSameGuild);
				return;
			}
		}

		if(Guild::isBattleOpen()){
			if(p->getLeader()->isInGuildBattleScene()){
				if(isInGuildBattleScene()){
					if(myGuild() != p->getLeader()->myGuild()){
						errorMessageToC(EN_GuildBattleTeamNoSameGuild);
						return;
					}
				}
				else{
					errorMessageToC(EN_GuildBattleHasTeam);
					return;
				}
			}
			else {
				if(isInGuildBattleScene()){
					errorMessageToC(EN_GuildBattleHasTeam);
					return;
				}
			}
		}
		if(Warriorchoose::instance()->isWarriorchoose(teamId_))
			Warriorchoose::instance()->teamWarriorstop(p);
		p->backTeam(this);
	} 
}

void Player::teamCallMember(S32 playerId){
	if(isBattle())
		return;
	Team* p = isTeamLeader();
	if(p)
	{
		if(Guild::isBattleOpen()){
			if(!p->isSameGuild())
				return;
		}
		Player* memberP = p->findMemberById(playerId);
		if(memberP)
			CALL_CLIENT(memberP,teamCallMemberBack());
	} 
}

void Player::refuseBackTeam(){

	if(isBattle())
		return;
	Team* p = isTeamMember();
	if(p&&!isTeamLeader()){
		if(Guild::isBattleOpen()){
			if(!p->isSameGuild())
				return;
		}
		CALL_CLIENT(p->getLeader(),refuseBackTeamOk(playerId_));
	}
}

void
Player::exitTeam()
{
	if(isBattle())
		return;
	Team* p = myTeam();
	if(NULL == p)
		return;
	if(Warriorchoose::instance()->isWarriorchoose(p->teamId_))
		Warriorchoose::instance()->teamWarriorstop(p);
	p->exitTeam(this);
	cleanTeamQuest();
	cleanCopyQuest();
}

Team* Player::isTeamMember(){
	return myTeam() ;
}

Team*
Player::isTeamLeader()
{
	Team* t = myTeam();
	if((NULL != t && t->isTeamLeader(this)))
		return t;
	return NULL;
}

Team* Player::myTeam(){
	if(teamId_)
		return TeamLobby::instance()->getTeam(this,teamId_);
	return NULL;
}

void
Player::kickTeamMember(U32 uPlayerId)
{
	if(isBattle())
		return;
	Team* p = isTeamLeader();

	if(p == NULL)
		return;
	if(Warriorchoose::instance()->isWarriorchoose(p->teamId_))
		Warriorchoose::instance()->teamWarriorstop(p);

	p->delMember(uPlayerId,true);
}

void
Player::isjoinTeam(U32 teamId,B8 isFlag)
{
	if(isTeamMember())
	{
		CALL_CLIENT(this,errorno(EN_NoTeam));
		return;
	}
	Team* p = TeamLobby::instance()->getTeam(teamId);
	if(!p)
		return;

	if(isFlag){
		p->addMember(this);
	}
}

void Player::changeTeamSettings(U32 teamId,COM_CreateTeamInfo& ccti){
	Team* p = isTeamLeader();
	if(p) p->change(this,ccti);
}

void Player::requestJoinTeam(std::string& targetName)
{ ///申请入队
	if(isBattle())
	{
		errorMessageToC(EN_Battle);
		return;
	}
	Team* t = myTeam();
	if(t != NULL)
	{
		return;
	}

	Player* player = getPlayerByName(targetName);
	if(player == NULL)
	{
		errorMessageToC(EN_CannotfindPlayer);
		return;
	}
	
	if(!player->getOpenSubSystemFlag(OSSF_Team))
	{
		errorMessageToC(EN_NoSubSyste);
		return;
	}
	if(player->isBattle()){
		errorMessageToC(EN_OtherPlayerInBattle);
		return;
	}
	Team* pteam = player->myTeam();
	if(NULL == pteam)
	{
		CALL_CLIENT(this,errorno(EN_NoTeam));
		return;
	}
	if(Warriorchoose::instance()->isWarriorchoose(pteam->teamId_))
		Warriorchoose::instance()->teamWarriorstop(pteam);

	if(Guild::isBattleOpen()){
		if(player->isInGuildBattleScene()){
			//如果对面人在帮派战场景里
			if(isInGuildBattleScene()){
				//我不在帮派战场景里
				if(myGuild() != player->myGuild()){
					errorMessageToC(EN_GuildBattleTeamNoSameGuild);
					//不是一个帮派
					return;
				}
			}else{
				errorMessageToC(EN_GuildBattleHasTeam);
				//不是一个帮派
				return;
			}
		}
		else {
			//对面不在
			if(isInGuildBattleScene()){
				errorMessageToC(EN_GuildBattleHasTeam);
				//我在 
				return;
			}
		}
	}

	if(pteam->getLeader()->sceneId_ == SceneTable::getGuildHomeScene()->sceneId_){
		if(myGuild() != pteam->getLeader()->myGuild())
		{
			errorMessageToC(EN_TeamMemberNoGuild);
			return;
		}
	}

	CALL_CLIENT(pteam->getLeader(),requestJoinTeamTranspond(playerName_));
}

void Player::ratifyJoinTeam(std::string& sendName)
{///INVALUD INTERFACE
	if(isBattle())
		return;
	Player* player = getPlayerByName(sendName);
	if(player == NULL)
	{
		CALL_CLIENT(this,errorno(EN_CannotfindPlayer));
		return;
	}
	if(player->getOpenSubSystemFlag(OSSF_Team) == false)
	{
		CALL_CLIENT(this,errorno(EN_NoSubSyste));
		return;
	}
	if(player->isBattle()){
		errorMessageToC(EN_OtherPlayerInBattle);
	}
	if(player->myTeam())
		return;
	Team* t = myTeam();
	if(t == NULL)
	{
		CALL_CLIENT(this,errorno(EN_NoTeam));
		return;
	}
	if(!t->isTeamLeader(this))
	{
		CALL_CLIENT(this,errorno(EN_NoTeamLeader));
		return;
	}
	if(Warriorchoose::instance()->isWarriorchoose(t->teamId_))
		Warriorchoose::instance()->teamWarriorstop(t);

	if(!t->addMember(player))
		return;
	COM_TeamInfo info;
	t->getTeamInfo(info);
	COM_SimpleTeamInfo* pinfo = (COM_SimpleTeamInfo*)&info;
	TeamLobby::instance()->syncUpdateLobbyTeam(info);
}