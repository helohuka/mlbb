
#include "config.h"
#include "skill.h"
#include "entity.h"
#include "skilltable.h"
#include "ComScriptEvn.h"

bool
SkillCondition::condition(Entity *caster,BattlePosition target,U32 battleId,std::map<S32,S32> &posTable,std::map<S32,S32> &orderParam,float costMana)
{
	if(lua_.empty())
		return true;

	enum 
	{
		ARG_BATTLEID,		//0
		ARG_CASTERPOS,		//1
		ARG_TARGETPOS,		//2
		ARG_POSTABLE,		//3
		ARG_ORDERPARAM,		//4
		ARG_COSTMANA,
		ARG_MAX_ //= 6
	};
	static GEParam param[ARG_MAX_];


	param[ARG_BATTLEID].type_ = GEP_INT;
	param[ARG_BATTLEID].value_.i = caster->battleId_;

	param[ARG_CASTERPOS].type_ = GEP_INT;
	param[ARG_CASTERPOS].value_.i = caster->battlePosition_;

	param[ARG_TARGETPOS].type_ = GEP_INT;
	param[ARG_TARGETPOS].value_.i = target;

	param[ARG_POSTABLE].type_ = GEP_POS_TABLE;
	param[ARG_POSTABLE].value_.hPosTable = &posTable;

	param[ARG_ORDERPARAM].type_ = GEP_POS_TABLE;
	param[ARG_ORDERPARAM].value_.hPosTable = &orderParam;

	param[ARG_COSTMANA].type_ = GEP_FLOAT;
	param[ARG_COSTMANA].value_.f = costMana;

	std::string err;

	//ACE_DEBUG((LM_INFO,ACE_TEXT("LUA ACTIVE ==> %s\n"),lua_.c_str()));

	int ret = 1;

	if(false == ScriptEnv::callGEProc(lua_.c_str(), caster->handleId_,param,ARG_MAX_,ret,err))
	{
		return false;
	}

	return !!ret;
}

SkillAction::SkillAction():condition_(NULL)
{
	condition_ = NEW_MEM(SkillCondition);
	SRV_ASSERT(condition_);
}

SkillAction::~SkillAction()
{
	if(condition_)
		DEL_MEM(condition_);
}

bool
SkillAction::active(Entity *caster, BattlePosition target, U32 battleId,std::map<S32,S32> &posTable,std::map<S32,S32> &orderParam)
{
	/*if(condition_ && false == condition_->condition(caster,target,battleId,posTable,orderParam))
		return false;*/
	
	enum 
	{
		ARG_BATTLEID,		//0
		ARG_CASTERPOS,		//1
		ARG_TARGETPOS,		//2
		ARG_POSTABLE,		//3
		ARG_ORDERPARAM,		//4
		ARG_MAX_ //= 5
	};

	static GEParam param[ARG_MAX_];
	
	U8 argIndex = 0;

	param[ARG_BATTLEID].type_ = GEP_INT;
	param[ARG_BATTLEID].value_.i = caster->battleId_;

	param[ARG_CASTERPOS].type_ = GEP_INT;
	param[ARG_CASTERPOS].value_.i = (S32)caster->battlePosition_;
	
	param[ARG_TARGETPOS].type_ = GEP_INT;
	param[ARG_TARGETPOS].value_.i = target;
	
	param[ARG_POSTABLE].type_ = GEP_POS_TABLE;
	param[ARG_POSTABLE].value_.hPosTable = &posTable;
	
	param[ARG_ORDERPARAM].type_ = GEP_POS_TABLE;
	param[ARG_ORDERPARAM].value_.hPosTable = &orderParam;

	std::string err;
	int ret = 1;
	//ACE_DEBUG((LM_INFO,ACE_TEXT("LUA ACTIVE ==> %s\n"),action_.c_str()));
	if(false == ScriptEnv::callGEProc(action_.c_str(), caster->handleId_,param,ARG_MAX_,ret,err))
	{
		return false;
	}

	return !!ret;
}

Skill::Skill()
:skId_(0)
,skLevel_(0)
,skExp_(0)
,gloAction_(NULL)
,gloCondition_(NULL)
{
	//ACE_DEBUG((LM_INFO,"New Skill\n"));
}

Skill::~Skill()
{
	fini();
}

void 
Skill::init(SkillTable::Core const *core)
{
	fini();

	skId_ = core->id_;
	skExp_= 0;
	skLevel_ = core->level_;
	gloAction_ = NEW_MEM(SkillAction);
	SRV_ASSERT(gloAction_);
	gloAction_->action_ = core->gloAction_;
	

	SkillCondition *gsc = NEW_MEM(SkillCondition);
	SRV_ASSERT(gsc);
	gsc->lua_ = core->gloCondition_;
	gloCondition_ = gsc;

	for(size_t i=0; i<core->conditions_.size(); ++i)
	{
		SkillAction *tmp = NEW_MEM(SkillAction);
		SRV_ASSERT(tmp);
		tmp->condition_->lua_ = core->conditions_[i];
		tmp->action_ = core->actions_[i];

		actions_.push_back(tmp);
	}
}

void 
Skill::reset(){
	SkillTable::Core const * core = SkillTable::getSkillById(skId_ , skLevel_);
	fini();
	
	if(NULL == core)
		return;
	gloAction_ = NEW_MEM(SkillAction);
	SRV_ASSERT(gloAction_);
	gloAction_->action_ = core->gloAction_;


	SkillCondition *gsc = NEW_MEM(SkillCondition);
	SRV_ASSERT(gsc);
	gsc->lua_ = core->gloCondition_;
	gloCondition_ = gsc;

	for(size_t i=0; i<core->conditions_.size(); ++i)
	{
		SkillAction *tmp = NEW_MEM(SkillAction);
		SRV_ASSERT(tmp);
		tmp->condition_->lua_ = core->conditions_[i];
		tmp->action_ = core->actions_[i];

		actions_.push_back(tmp);
	}

	
}

void 
Skill::fini()
{
	if(gloAction_){
		DEL_MEM(gloAction_);
	}
	gloAction_ = NULL;
	if(gloCondition_)
	{
		DEL_MEM( gloCondition_);
	}

	gloCondition_ = NULL;

	for(size_t i=0; i<actions_.size(); ++i){
		if(actions_[i]){
			SkillAction *p = actions_[i];
			DEL_MEM(p);
		}
	}
	actions_.clear();
}


bool
Skill::active(Entity *caster,BattlePosition target,U32 battleId,std::map<S32,S32> &posTable,std::map<S32,S32> &orderParam)
{
	SkillTable::Core const* p = SkillTable::getSkillById(skId_,skLevel_);

	if(NULL == p)
		return false;

	S32 mana = p->costMana_;
	
	if(mana)
		mana = mana - mana * caster->getSkillCostMana(skId_);
	

	if(gloCondition_ && false == gloCondition_->condition(caster,target,battleId,posTable,orderParam,mana))
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Skill[%d] Action Condition return false\n"),skId_));
		return false;
	}
	
	if(gloAction_)
		gloAction_->active(caster,target,battleId,posTable,orderParam);

	///actions 没用上~
	for(size_t i=0; i<actions_.size(); ++i)
	{
		if(actions_[i])
			actions_[i]->active(caster,target,battleId,posTable,orderParam);
	}
	if(mana)
		caster->setProp(PT_MpCurr,caster->getProp(PT_MpCurr) - mana);
	return true;
}