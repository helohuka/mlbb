/************************************************************************/
/**
 * @file	entity.h
 * @date	2015-3-2015/03/03 	
 * @author 	Lucifer<hotlala8088@gmail.com>            					
 * */
/************************************************************************/

///<实例信息

#ifndef __ENTITY_H__
#define __ENTITY_H__

#include "config.h"
#include "skill.h"
#include "ScriptHandle.h"
#include "itemtable.h"
class Baby;
class Player;
class ClientHandler;


class Entity : public ScriptHandle
{
public:
	Entity();
	virtual ~Entity();
	virtual Entity *asEntity(){return this;}
	virtual U32	getGUID(){return 0;}
	virtual const char* getNameC(){return "";}
	virtual ClientHandler *getClient(){return NULL;}
	virtual void chackLevelUp(){}
public:
	float getProp(PropertyType type);
	void setProp(PropertyType type, float val);
	void addDirtyProp(PropertyType type, float val);
	void addHp(float hp);
	void addExp(U32 exp,bool Extra = true);
	float caleExpExtraValue();
	
	void calcFightingForce(); //计算战斗力
	bool isDeadth();
public:
	void insertState(U32 stId ,S32 v0 =0 , S32 v1=0);
	void setStateValue(U32 stId, S32 v0 =0 , S32 v1=0);
	void getStateValue(U32 stId, S32& v0, S32& v1);
	bool checkState(U32 type);
	bool checkStateById(U32 id);
	bool updateState(std::map<S32,S32> &posTable);
	bool updateState();
	void removeState(U32 stId);
	void clearState(bool battle = true);
	
	void clearSkills();
	void initSkillById(std::vector< std::pair<S32,S32> > const &skillIds);
	void initSkillByInst(std::vector<COM_Skill> const &skillInsts);
	virtual void learnSkill(S32 skId,S32 skLv);
	void learnPassiveSkill(S32 skId,S32 skLv);
	void resetPassiveSkill();
	virtual void forgetSkill(S32 skId);
	bool castSkill(S32 skId, BattlePosition target, U32 battleId,std::map<S32,S32> &posTable,std::map<S32,S32> &orderParam);
	Skill* getSkillById(S32 skId);
	std::vector<S32> getSkillIds();
	virtual void addSkillExp(S32 skillID, U32 exp,ItemUseFlag flag);
	virtual void skillLevelUp(S32 skillId);

	void setEntityInst(COM_Entity& entity);
	void getEntityInst(COM_Entity& entity);
	virtual void getBattleEntityInformation(COM_BattleEntityInformation& info);
	COM_Item* getEquipment(U32 instId);
	void getWeapon(S32& weaponId,WeaponType& weaponType);
	S32 getEquipSlot(const ItemTable::ItemData* item);
	const ItemTable::ItemData* getEquipmentItemData(EquipmentSlot slot);
	std::vector<COM_Skill>& calcSkillExp();
	virtual COM_Item* wearEquipment(COM_Item* equip);
	virtual COM_Item* takeoffEquipment(U32 instId);
	void addEquipmentEffect(const COM_Item* equip,float ratio = 1,bool needSkillCost = true);
	void delEquipmentEffect(const COM_Item* equip,float ratio = 1,bool needSkillCost = true);
public:
	bool getBattleActive(){return battleActive_;}
	void setBattleActive(bool b){battleActive_ = b;}
	bool isBattle(){return battleId_!=0;}
	class Battle* myBattle();
	GroupType  getForce(){return battleForce_;}
	virtual void initBattleStatus(U32 battleId,GroupType battleForce,BattlePosition battlePos,bool initActive = false);
	virtual void cleanBattleStatus(bool resetProperty = false);
	virtual bool isBattleAtkTimeout();
	void addAttachedPropertyD(PropertyType pt, float value);
	void clearAttachedProperties();
	void refreshProperty();
	void addSkillCostMana(S32 skid,float manap);
	float getSkillCostMana(S32 skid);
	
	/// ai 抽象
public:
	virtual void init(){};
	virtual void postEvent(AIEvent me,BattlePosition target, std::map<S32,S32> &posTable){};

	std::vector<S32>		aiSkills_;		//战斗中AI所带技能
	
public:
	bool					isJoinBattle_;			//能否进战斗
	bool					battleActive_;
	bool					battleRunaway_;
	BattlePosition			battlePosition_;
	U32						battleId_;
	GroupType				battleForce_;
	U32						battleRunawayNum_;			//逃跑次数
	bool					openDoubleTimeFlag_;	///双倍经验开关
	S32						battleValue_;			//战斗获得值,如:经验,JJC积分
	time_t					battleAtkTime_;
	std::vector<COM_Skill>  useSkillCounter_;
	std::vector<U32>		battleUseSkill_;

	std::vector<U32> curRoundStates_; ///当前回合添加的状态
	std::vector<COM_State>  states_;
	std::vector<Skill*>		skills_;

	std::vector<std::pair<S32,float> > skillCostMana_; ///技能所扣的mana
	
	std::vector<COM_PropValue> attachedProperties_;
	std::vector<float>		properties_;
	std::vector<float>		battleTmpProp_;
	std::vector<COM_PropValue> dirtyProp_;
	std::vector<COM_Item*>	equipItems_;
	std::vector<S32>		achValues_;			//成就相关数值
	COM_Order				lastOrder_;
	
};
#endif