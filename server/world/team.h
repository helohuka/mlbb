/************************************************************************/
/**
 * @file	team.h
 * @date	2015-4-2015/04/08 	
 * @author 	Lucifer<hotlala8088@gmail.com>            					
 * */
/************************************************************************/

//组队房间

#ifndef __TEAM_H__
#define __TEAM_H__

#include "config.h"
#include "client.h"
#include "player.h"
#include "broadcaster.h"
class Team : public Broadcaster
{	
public:	
	Team(int32 teamId):teamId_(teamId),isUsed_(false),teamIsRunning_(false),teamJJCTimes_(0),teamisWarrior_(false){}
	void create(Player* player,COM_CreateTeamInfo& cti);
	void change(Player* player, COM_CreateTeamInfo& cti);
	void release();
	bool addMember(Player* player);
	void delMember(S32 playerId, bool iskick = false);
	void delMember(Player* player,bool iskick = false);
	void exitTeam(Player* player); //推出
	void leaveTeam(Player* player); //暂离
	void backTeam(Player* player);
public:
	void sortMemberByLeave();
	S32 getMemberSize(){return teamMembers_.size();}
	U32	getTeamLeaveNum();
	bool isTeamMember(Player* player);
	Player* getLeader(){ if(teamMembers_.empty())return NULL; else return teamMembers_[0];}
	bool isTeamLeader(Player* player){return player == getLeader();}
	void getTeamSimpleInfo(COM_SimpleTeamInfo& info);
	void getTeamInfo(COM_TeamInfo& info);
	Player* findMemberById(U32 uPlayerId);
	void changeTeamLeader(U32 targetId);
	void changeTeamPassword(std::string pwd);
	bool checkPassword(std::string pwd);
	void inviteTeamMember(Player* player,std::string name);
	void createBttleUnit(Battle* battle,GroupType gt,bool hasLeaderEmployee = true);
	bool isBattle();
	bool canPetActivity(S32 battleId); 
	bool canHundredBattle(S32 battleId);
	bool hasLeaveMember(){
		for(size_t i=0; i<teamMembers_.size(); ++i)
			if(teamMembers_[i]->isLeavingTeam_)
				return true;
		return false;
	}
	bool isSameGuild();
	void lock(){
		++teamIsRunning_; ///回到组队场景
		COM_TeamInfo info1;
		COM_SimpleTeamInfo* info2 = (COM_SimpleTeamInfo*)&info1;
		Team::syncUpdateLobbyTeam(*info2);
	}
	void unlock(){
		if(teamIsRunning_<=0)
		{
			ACE_DEBUG((LM_DEBUG,"Team lock is mujl\n"));
			return ;
		}
		--teamIsRunning_;
		COM_TeamInfo info1;
		COM_SimpleTeamInfo* info2 = (COM_SimpleTeamInfo*)&info1;
		Team::syncUpdateLobbyTeam(*info2);
	}
	bool locked(){return teamIsRunning_ != 0;}
	void acceptTeamQuest(Player* leader, S32 questId);
	void submitTeamQuest(Player* leader, S32 npcId, S32 questId, int32 instId);
	void giveupTeamQuest(Player* leader, S32 questId);
	void teamAddActivation(ActivityType type,int count);
	void joinCopy(int sceneId);
	bool isunwelcome(U32 playerId);
	float calcTeamMemberExp(float exp);
	S32 getMinLevel();
	void robotCreatTeam(Player* player,COM_CreateTeamInfo& cti);
public:
	const U32				teamId_;
	bool isUsed_;
	U8						maxMemberSize_;
	TeamType				teamType_;
	std::string				teamName_;
	std::string				teamPassword_;
	std::vector<Player*>	teamMembers_;
	std::vector<U32>		teamblacklist_;
	int						teamIsRunning_;
	U16						teamMinLevel_;
	U16						teamMaxLevel_;
	float					teamJJCTimes_;
	bool					teamisWarrior_;			//队伍是否在参与勇者选拔战活动
};

class TeamLobby : public Broadcaster {
public:
	SINGLETON_FUNCTION(TeamLobby);
	TeamLobby();
	~TeamLobby();
	void delChannel(Player* p);
	
	void joinLobby(Player* p);
	void exitLobby(Player* p);
	
	Team* getTeam(Player* p, int32 teamId);
	Team* getTeam(int32 teamId);
	Team* apply();
	Team* findTeam(int32 teamId);
	void getLobbyTeams(std::vector<COM_SimpleTeamInfo>& teams);
public:
	std::vector<Team*> teamCache_;
	std::set<Player*>  players_;
};

#endif