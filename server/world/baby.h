/************************************************************************/
/**
 * @file	pet.h
 * @date	2015-3-2015/03/03 	
 * @author 	Lucifer<hotlala8088@gmail.com>            					
 * */
/************************************************************************/

#ifndef __BABY_H__
#define __BABY_H__

#include "config.h"
#include "entity.h"
#include "tmptable.h"
#include "struct.h"
#include "monstertable.h"

class ClientHandler;
class InnerPlayer;
class Baby : public Entity
{
public:
	static void genBabyData(U32 babyTmpId,COM_BabyInst& out);
public:
	Baby(InnerPlayer *owner);
	~Baby();
	Baby * asBaby(){return this;}
	U32	getGUID(){return babyId_;}
	const char* getNameC(){return babyName_.c_str();}
	ClientHandler *getClient();
	void syncProp();
	void addProperty(const std::vector<COM_Addprop> &props);
	void changeProp(PropertyType type, float uVal);
public:
	void setPropFromTable(MonsterTable::MonsterData const *tmp,U32 level = 1);
	void setPropFromMonster(Monster* monster);
	void resetBaby();
	void calcProperties(); //从新计算属性
	void calcProperties2(); //从新计算属性2
	void intensify();						//宠物强化
	void calcintensifyProp();				//计算单次强化属性
	//
	void intensify(U32 target);		//直接强化到目标等级
public:
	void setBabyInst(COM_BabyInst &tmp);
	void getBabyInst(COM_BabyInst &out);
	void getBattleEntityInformation(COM_BattleEntityInformation& info);
	void chackLevelUp();
	void levelup();
	inline InnerPlayer* getOwner(){return owner_;}
public:
	void changeBabyName(const char* name);
	void postEvent(AIEvent me,BattlePosition target, std::map<S32,S32> &posTable);
	void babyLearnSkill(U32 oldSkId, U32 newSkId, U32 newSkLv = 1);
	bool canUseSkill(U32 oldSkId, U32 newSkId, U32 newSkLv = 1);
	void calcLoyal();
	S32	 calcrandDelta();

	COM_Item* wearEquipment(COM_Item* equip);
	COM_Item* takeoffEquipment(U32 instId);
	void checkEquipGroup();
	Skill* getEquipSkill();

	void forgetSkill(S32 skId);
	void initBattleStatus(U32 battleId, GroupType battleForce, BattlePosition battlePos, bool initActive = false);
	void cleanBattleStatus(bool resetProperty = false);
public:
	bool		isBattle_;
	bool		isShow_;
	bool		isBind_;
	bool		isLock_;
	U32			babyId_;
	S32			tableId_;
	S32			slot_;
	U32			intensifyLevel_;		//强化等级
	U32			intensifynum_;			//强化失败次数
	int32		lastSellTime_;
	Skill		equipSkill_;	//装备技能
	std::string ownerName_;
	std::string babyName_;
	std::vector<S32> gear_;
	std::vector<float> addprop_;		//手动加点存储
private:
	InnerPlayer	   *owner_;
};

#endif