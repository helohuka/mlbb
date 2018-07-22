#ifndef __MONSTER_H__
#define __MONSTER_H__

#include "config.h"
#include "entity.h"
#include "monstertable.h"

class Battle;
class Monster : public Entity
{
public:
	Monster(int32 pId,int32 pLev);
	Monster(std::string& className);
	~Monster();
public:
	void init();
	void fini();	
	Monster* asMonster(){return this;}
	void getMonsterInst(COM_MonsterInst &out);
	void getBattleEntityInformation(COM_BattleEntityInformation& info);
	U32	getGUID(){return instId_;}
	const char* getNameC(){return monsterName_.c_str();}
	void appendPropValue(std::string& className);
	bool isBattleAtkTimeout(){return !battleActive_;}
public:
	void postEvent(AIEvent me,  BattlePosition target, std::map<S32,S32> &posTable);
public:
	U32	    instId_;
	U32		monsterId_;
	U32		monsterLevel_;	
	U32		monsterDropId_;
	std::string monsterName_;
	std::vector<std::string> monsterEvents_;
	std::string monsterClass_;
	std::vector<S32> gearProperties_;

	static S32 IDMaker;
};

#endif