#ifndef __PVPJJC_TABLE_H__
#define __PVPJJC_TABLE_H__

#include "config.h"
class Team;
class Player;

class PvpJJC
{
public:
	static void		startMatching(Team* p);				//开始匹配对手
	static void		stopMatching(Team* p);
	static void		startMatching(Player* p);				//开始匹配对手
	static void		stopMatching(Player* p);
	static bool		addParticipant(Team* p);						//添加、删除、查询，参与队伍
	static bool		delParticipant(U32 teamId);
	static Team*	findParticipantTeam(U32 teamId);
	static Player*	findParticipantPlayer(U32 instId);	

	static void		rankrewardbyday(std::string& sendName,std::string& title, std::string& content);
	static void		rankrewardbysenson(std::string& sendName,std::string& title, std::string& content);

	static void		syncEnemyMsg(Player* my,Team* enemyTeam);
	static void		syncEnemyMsg(Team* myTeam, Team* enemyTeam);
	static void		syncEnemyMsg(Player* my, Player* enemy);
public:
	static void		tick(float dt);
public:
	static U32		calcMeanSection(Team* p);			//计算队伍平均段位
	static void		checkEnemy(Team* p, float dt);		//查找敌人
	static void		exitTeamPvpJJC(Team* p);
	static void		pvpjjcBattleGo(Player* player,U32 id);
	static void		createjjcBattle(Team* myteam,Team* enemyteam);
	static void		createjjcBattle(Player* my, Player* enemy);
	static void		createjjcBattle(Team* pteam,Player* player);
public:
	//单人
	static bool		addSingleParticipant(Player* p);
	static bool		delSingleParticipant(U32 instId);				//instId playerInstId
	static void		checkPlayerEnemy(Player* p, float dt);			//查找敌人
	static void		exitSingleJJC(U32 instId);
public:
	static std::vector<Team*>		teamstore_;
	static std::vector<Player*>		playerstore_;
};

//  勇者选拔战活动[9/21/2016 lwh]
class Warriorchoose{
public:
	static Warriorchoose* instance() {static Warriorchoose serv; return &serv;}
public:
	void		tick(float dt);
	
	void		start(U32 teamid);		//开始匹配
	void		stop(U32 teamid);
	void		close();
	void		teamWarriorstop(Team* pteam);
	bool		isWarriorchoose(U32 teamid);	//队伍是否在活动中
	
	U32			calclevelsum(Team* pteam);
	void		checkEnemy(Team* p, float dt);		//查找敌人
	void		syncEnemyMsg(Team* myTeam, Team* enemyTeam);
	void		delParticipant(U32 teamid);
	U32			sendtrophy(Player* player,bool iswin);
public:
	std::vector<Team*>		teamstore_;
};

#endif