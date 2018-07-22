/************************************************************************/
/**
 * @file	battle.h
 * @date	2015-3-2015/03/06 	
 * @author 	Lucifer<hotlala8088@gmail.com>            					
 * */
/************************************************************************/

#ifndef __BATTLE_H__
#define __BATTLE_H__ 


#include "config.h"
#include "scenetable.h"
#include "ScriptHandle.h"
class Player;
class Entity;
class Baby;
class Monster;
class Team;
class Robot;

class Battle
{
public:
	
	typedef std::vector<COM_ReportAction> RAList;

	enum
	{
		BattleCacheSize = 4000
	};

	enum BattleState 
	{
		BS_None,
		BS_Used,
	};


	enum ChangePropType{
		CPT_Normal,
		CPT_State,
		CPT_Fanji,
	};

	//static const char* logdir_;
	static std::vector<Battle*> battleCache_;

	static void init();
	static void fini();
	static Battle* find(S32 id);
	static Battle* getOneFree();
	static void updateBattle(float dt);
public:

	Battle();

	void create(Team* pTeam, ZoneInfo* pZone);
	void create(Player* player,ZoneInfo* zone);
	void create(Player* p0, Player* p1,BattleType tp = BT_PVP);
	void create(Player* pPlayer, Robot* pRobot);
	void create(Team* pTeam,S32 battleTableId);
	void create(Player* p0, S32 battleTableId);
	void create(Team* pTeamDown,Team* pTeamUp,BattleType tp);
	void create(Player* player,Team* pTeam,BattleType tp);
	void create(Player* sendplayer,U32 playerId); //金场景PK
	void create(Player* player, Monster* monster);

	void update(float dt);
	
	void battleOver();
	SneakAttackType calcSneakAttack(Player *player);
public:
///回合
	bool execRound(float dt);
	bool execOneOrder();
	void startBattle();
	void syncRoundReport();
	void cleanRound();
public:
	void insertState(S8 pos, U32 stId, U32 isAction);
	bool checkState(S8 pos, U32 type);
	void removeState(S8 pos, U32 stId);
	void clearState(S8 pos);
	void deleteStateByTrun();
	void lookupStateByTrun();
	COM_State*  findState(StateType type, BattlePosition pos);
	U32  getStateLevel(StateType type, BattlePosition pos);
	void cuttickState(StateType type, BattlePosition pos, U32 cutNum);
	void postState();
public:
	std::vector<COM_ReportAction>& actions(){return roundReport_.actionResults_;}
	void pushRA(COM_Order& order,int sklevel = 0);
	void pushOrder(COM_Order &order);
	bool checkSecondOrder(COM_Order &order);
	void pushOrderByAI(COM_Order &order);
	void pushOrderTimeout(U32 instId);
	void orderSort();
	void checkUnite();									///合击
	void updateUnite();
	COM_Order* findOrder(U32 instId, bool skipDontCare = true);
	COM_Order* getCurrentOrder();
	bool findOrder(BattlePosition targetPos,U32 skillId);
	Entity* findEntityByPos(S8 pos);  
	Entity* findEntityById(U32 id); 
	void eraseEntityByPos(S8 pos);
	bool isInThisBattle(Entity *pEntity);
	S32 getIndex(U32 instId);
	void selectBaby(Player *player, U32 instId, bool isBattle);
	bool hasBaby(Player *player);
	bool isBattleBaby(U32 babyId);
	bool flee(Player* player,bool isFly = false);
	void fly(Entity* entity);
	void catchBaby(S8 pos,Entity* handle);
	void changePosition(BattlePosition src, BattlePosition dest);
	void changeProp(ChangePropType, BattlePosition, PropertyType, float, bool isBao=false);
	void changeProp(S8 pos, PropertyType type, float val, bool bao= false);
	void changePropBystate(S8 pos, PropertyType type, float val);
	void changePropByfanji(S8 pos, PropertyType type, float val, bool bao=false);
	void changeReportSkillId(U32 skillId,S8 pos);
	void changeOrderSkillId(U32 skillId);
	void changeOrderTarget(S8 pos);
	void removeCurrentOrder();
	void addActionCounter(U32 casterId, U32 uTargetPos, PropertyType type, float val, bool bao);
	//void exitBattleinst();
	void allLose();
	void calcBattleReward();
	void calcBattleRewardPVE();
	void calcBattlePlayerRewardPVE(Player* player);
	void calcBattleRewardPVR();
	void calcBattleRewardPVH();
	void calcBattlePlayerRewardPVH(Player* player);
	void calcBattleRewardGuild();
	void calcBattleRewardPVP();
	void calcBattleRewardPK();
	void calcBattlePKFlee(Player* player);
	void checkKillMonster(Player* player);
	Monster* findMonster(U32 id);
	bool checkCurrentAction();
	COM_ReportAction *getCurrentAction();
	BattlePosition calcFriendPosition(Entity *pEntity); ///计算友军位

	U32 orderCounter(Entity *pEntity);
	bool hasOnceMagic(Entity *pEntity);
	void calcWinner();
	void addWaveMonster();
	BattlePosition findPosition(Entity* pEntity,GroupType type);
	BattlePosition findMonsterPosition();

	void initAI();
	void update(Entity* ent);
	void addHuwei(BattlePosition target, BattlePosition caster){ huwei_.push_back(std::pair<BattlePosition ,BattlePosition>(target,caster));}
	void addMonster(std::vector<std::string>& className);
	void calcPlayerKillEvent(Player* caster, Entity* target);
	
	void getInitData(COM_InitBattle& r,U32 playerid);
	S32 findMaxMpPos(GroupType type);
	S32 findMinHpPos(GroupType type);
	S32 findMaxHpPos(GroupType type);
	bool checkForceHp(GroupType type, float valueHp);
	BattlePosition findBossPos();
	U32 getMonsterType(BattlePosition pos);
protected:
	void cleanPosTable();
	void regenPosTable();
	void setPosTable(BattlePosition bp, Entity *p);
	void resetEntityProp(Entity *p);//竞技场战斗前回血魔
	void cleanOderParam();
	void setOrderParam(OrderParamType opt, S32 param);
	void cleanHuwei(){huwei_.clear();}
	BattlePosition getHuwei(BattlePosition target);

	void prepareOrder(); ///提前检测order
	bool checkState(Entity *p); ///检测无操作状态
	bool isInvalid(Entity *p);
	void removePlayer(Player *player,COM_ReportAction* pra);
	
	bool calcBabyLoyal(COM_Order& order);
	void battleItem();		//战斗使用道具
	void battleWeapon();	//战斗中换武器
	void battleSkill();		//战斗使用技能
	void checkBattleSkill();
	
	S32 totalAtkVal(S32 unitId, S32 unitNum, S32 val);
	void cleanUnitAtk(){unitAtk_.clear();}

public:
	void createPlayerUnit(Player* player,GroupType gt = GT_Down);
	void createMonsterUnits(std::vector<std::string>& monsters, std::vector<Monster*> &units);
	void createPlayerEmployees(Player*player);
protected:
	void cleanBattle();
	void checkBattleStatus();
	void checkCleaner(COM_BattleOverClearing&);
	void checkInit(COM_InitBattle&);
	void checkReport();
	void checkEntity();
	void checkOrder();
	void checkSameGuid();
	void logJoinBattle();
	void logExitBattle();
public:
	
	S32									opentime_; //
	
	bool								activeRound_;	///开始回合 标志
	bool								hasPlayerPushOrder_; //是否有玩家推送命令
	S32									id_;
	S32									battleDataId_;	///战斗表ID 
	BattleState							battleState_;
	BattleType							battleType_;
	GroupType							battleWinner_;
	U32									actionIndex_;		///当前只能命令
	U32									roundCount_;	///战斗回合数
	COM_BattleReport 					roundReport_;	///回合战报
	SneakAttackType						sneakattack_;	///偷袭玩家角度
	S32									upWave_;   ///上怪物波次
	S32									downWave_; ///下怪物波次
	float								waitCloseTimeout_; ///等不到ORDER 关闭战斗时间
	float								waitOrderTimeout_; ///第一个玩家发送order 开始计时
	float								waitCalcOrderTimeout_; ///最短计算间隔
	Robot*								battleRobot_;		///离线竞技场机器人
	///怪物
	std::vector<Entity*>				entities_;		//战斗成员
	std::vector<Monster*>				monsters_;		//总怪物
	std::vector<std::vector<Monster*> > monsterList_;   //怪物队列
	std::vector<Monster*>				killmonsters_;	//击杀怪物
	std::vector<Baby*>					deadthBaby_;
	std::map<S32,S32>					posTable_;
	std::map<S32,S32>					orderParam_;
	std::vector<std::pair<BattlePosition, BattlePosition> > huwei_;	///护卫被护卫者位置, 护卫者位置		
	std::vector<Team*> teams_;
	std::vector<std::pair<S32 ,S32> > unitAtk_;	///每回合合击伤害统计
	int32 upLosePlayerSum_;
	int32 downLosePlayerSum_;
	int32 guildSceneId_;
	int32 guildNpcId_;
};

#endif