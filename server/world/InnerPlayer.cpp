#include "InnerPlayer.h"
#include "robotTable.h"
#include "monstertable.h"
#include "baby.h"
#include "employee.h"
InnerPlayer::InnerPlayer()
:genItemMaxGuid_(1)
{
}
Baby* 
InnerPlayer::getBattleBaby()
{
	for(size_t i=0; i<babies_.size(); ++i)
	{
		if(babies_[i]->isBattle_)
			return babies_[i];
	}
	return NULL;
}

InnerPlayer::~InnerPlayer(){
	//ACE_DEBUG((LM_DEBUG,"InnerPlayer::~InnerPlayer()\n"));
	for(size_t i=0; i<babies_.size(); ++i){
		if(babies_[i])
		{
			Baby*p = babies_[i];
			DEL_MEM(p);
		}
	}
	for(size_t i=0; i<employees_.size(); ++i){
		if(employees_[i]){
			Employee *p = employees_[i];
			DEL_MEM(p);
		}
	}
	babies_.clear();
	employees_.clear();
}

void
InnerPlayer::init()
{
	PlayerAI const* pData = PlayerAI::getAI((JobType)(int)getProp(PT_Profession));

	if (pData == NULL)
	{
		ACE_DEBUG((LM_DEBUG,"-----------------BDplayerAI is nil--------------------\n"));
		return;
	}

	class_	= pData->playerClass_;
	babyClass_	= pData->babyClass_;

	const MonsterClass::Core* robotClazz = MonsterClass::getClassByName(class_);
	const MonsterClass::Core* babyClazz = MonsterClass::getClassByName(babyClass_);
	SRV_ASSERT(robotClazz);
	SRV_ASSERT(babyClazz);

	babyEvents_ = babyClazz->events_; 
	robotEvents_= robotClazz->events_;
}


void 
InnerPlayer::postEvent(AIEvent me, BattlePosition target, std::map<S32,S32> &posTable)
{
	if(isDeadth())
		return;
	if (robotEvents_[me].empty() || robotEvents_[me].length() == 0)
	{
		return;
	}
	enum 
	{
		ARG_BATTLEID,		//0
		ARG_CASTERPOS,		//1
		ARG_TARGETPOS,		//2
		ARG_POSTABLE,		//3
		ARG_SKILLTABLE,		//4
		ARG_MAX_
	};

	GEParam param[ARG_MAX_];
	param[ARG_BATTLEID].type_		= GEP_INT;
	param[ARG_BATTLEID].value_.i	= battleId_;
	param[ARG_CASTERPOS].type_		= GEP_INT;
	param[ARG_CASTERPOS].value_.i	= battlePosition_;
	param[ARG_TARGETPOS].type_		= GEP_INT;
	param[ARG_TARGETPOS].value_.i	= target;
	param[ARG_POSTABLE].type_		= GEP_POS_TABLE;
	param[ARG_POSTABLE].value_.hPosTable	= &posTable;
	param[ARG_SKILLTABLE].type_		= GEP_HANDLE_ARRAY;
	param[ARG_SKILLTABLE].value_.hArray	= &aiSkills_;
	std::string err;
	if( !ScriptEnv::callGEProc(robotEvents_[me].c_str(),getHandleId(),param,ARG_MAX_,err) )
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("robot post event %s\n"),err.c_str()));
		return;
	}
}