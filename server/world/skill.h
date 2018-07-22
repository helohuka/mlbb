/************************************************************************/
/**
 * @file	skill.h
 * @date	2015-3-2015/03/05 	
 * @author 	Lucifer<hotlala8088@gmail.com>            					
 * */
/************************************************************************/
#ifndef __SKILL_H__
#define __SKILL_H__

/// Lua params RECV, BATTLEID, CASTERPOS, TARGETPOS,  POSTABLE{PTR}, ORDERPARAMS{BABYID}
#include "config.h"
#include "skilltable.h"
#include "ComScriptEvn.h"
class Entity;
class SkillCondition
{
public:
	virtual bool condition(Entity *caster,BattlePosition target,U32 battleId,std::map<S32,S32> &posTable,std::map<S32,S32> &orderParam,float costMana);
	std::string lua_;
};

class SkillAction
{
	
public:
	SkillAction();
	virtual ~SkillAction();
public:
	bool active(Entity *caster,BattlePosition target,U32 battleId,std::map<S32,S32> &posTable,std::map<S32,S32> &orderParam);

public:
	SkillCondition *condition_;
	std::string filter_;
	std::string action_;
};

class Skill
{
public:
	Skill();
	~Skill();
	void init(SkillTable::Core const *);
	void reset();
	void fini();
public:
	bool active(Entity *caster, BattlePosition target,U32 battleId,std::map<S32,S32> &posTable,std::map<S32,S32> &orderParam);
public:
	S32				skId_;
	S32				skExp_;
	S32				skLevel_;
	SkillAction	   *gloAction_;
	SkillCondition *gloCondition_;
	std::vector<SkillAction*> actions_;
};

#endif