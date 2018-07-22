#include "player.h"
#include "skilltable.h"
#include "achievementTable.h"
#include "GameEvent.h"
#include "Activity.h"
#include "worldserv.h"

void
Player::requestAchaward(S32 achId)
{
	COM_Achievement* pCom = findAchievement(achId);

	if(pCom == NULL)
	{
		//必须不为空
		ACE_DEBUG((LM_ERROR,ACE_TEXT("requestAchaward COM_Achievement is nil id[%d]!!!\n"),achId));
		return;
	}

	if(!pCom->isAch_ || pCom->isAward_)
		return;

	const AchievementTable::AchievementData* pData = AchievementTable::getAchievementById(achId);
	if(pData == NULL)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("requestAchaward canot find AchievementData Error id[%d]!!!\n"),achId));
		return;
	}

	pCom->isAward_ = giveDrop(pData->dropId_);
	if(pCom->isAward_)
		CALL_CLIENT(this,updateAchievementinfo(*pCom));
}

void
Player::checkAchievement(S32 achId)
{
	const AchievementTable::AchievementData* pData = AchievementTable::getAchievementById(achId);
	if(pData == NULL)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("canot find AchievementData Error id[%d]!!!\n"),achId));
		return;
	}

	COM_Achievement* pCom = findAchievement(achId);

	if(pCom == NULL)
	{
		//必须不为空
		ACE_DEBUG((LM_ERROR,ACE_TEXT("checkAchievement COM_Achievement is nil id[%d]!!!\n"),achId));
		return;
	}

	if(pCom->isAch_ || pCom->achValue_ < pData->value_)
	{
		if(pCom->achValue_ < pData->value_)
		{
			CALL_CLIENT(this,updateAchievementinfo(*pCom));
		}
		return;
	}
		
	pCom->isAch_ = true;
	if(pData->titleid_ != 0)
		addPlayerTitle(pData->titleid_);
	CALL_CLIENT(this,updateAchievementinfo(*pCom));

	GEParam params[1];
	params[0].type_ = GEP_INT;
	params[0].value_.i = 1;
	GameEvent::procGameEvent(GET_FinishAch,params,1,getHandleId());
}

bool
Player::isAchievement(S32 achId)
{
	for (size_t i = 0; i < achievement_.size(); ++i)
	{
		if(achievement_[i].achId_ == achId)
			return true;
	}

	return false;
}

COM_Achievement*
Player::findAchievement(U32 achId)
{
	for (size_t i = 0; i < achievement_.size(); ++i)
	{
		if (achievement_[i].achId_ == achId)
			return &achievement_[i];
	}

	return NULL;
}

void
Player::caleAchievement(AchievementType achType, U32 achValue)
{
	std::map< S32 ,AchievementTable::AchievementData* >::iterator itor = AchievementTable::data_.begin();

	for (; itor != AchievementTable::data_.end(); ++itor)
	{
		AchievementTable::AchievementData* pData = itor->second;
		if(pData == NULL)
		{
			ACE_DEBUG((LM_ERROR,"canot find AchievementData Error!!!\n"));
			continue;
		}

		if (pData->type_ == achType)
		{
			if(!isAchievement(pData->achId_))
			{
				COM_Achievement sAch;
				sAch.achId_		= pData->achId_;
				sAch.achType_	= pData->type_;
				sAch.achValue_	= achValue;
				sAch.isAch_		= false;
				sAch.isAward_	= false;
				achievement_.push_back(sAch);
			}
			else
			{
				COM_Achievement* sAch = findAchievement(pData->achId_);
				if(sAch == NULL)
				{
					ACE_DEBUG((LM_ERROR,"canot find COM_Achievement Error!!!\n"));
					continue;
				}

				if(sAch->isAch_)		//成就已达成不在做计算
					continue;
				sAch->achValue_ = achValue;
			}

			checkAchievement(pData->achId_);
		}
	}
}

//-------------------GM命令------------------------
void
Player::completeAchievement(S32 achId)
{
	const AchievementTable::AchievementData* pData = AchievementTable::getAchievementById(achId);
	if(pData == NULL)
	{
		return;
	}
	
	if(!isAchievement(pData->achId_))
	{
		COM_Achievement sAch;
		sAch.achId_		= pData->achId_;
		sAch.achType_	= pData->type_;
		sAch.achValue_	= pData->value_;
		sAch.isAch_		= false;
		sAch.isAward_	= false;
		achievement_.push_back(sAch);
	}
	else
	{
		COM_Achievement* sAch = findAchievement(pData->achId_);
		if(sAch == NULL)
		{
			return;
		}

		if(sAch->isAch_)		//成就已达成不在做计算
			return;
		sAch->achValue_ = pData->value_;
	}

	checkAchievement(pData->achId_);
}

void
Player::completeAllAchievement()
{
	std::map< S32 ,AchievementTable::AchievementData* >::iterator itor = AchievementTable::data_.begin();

	for (; itor != AchievementTable::data_.end(); ++itor)
	{
		AchievementTable::AchievementData* pData = itor->second;
		if(pData == NULL)
		{
			continue;
		}

		if(!isAchievement(pData->achId_))
		{
			COM_Achievement sAch;
			sAch.achId_		= pData->achId_;
			sAch.achType_	= pData->type_;
			sAch.achValue_	= pData->value_;
			sAch.isAch_		= false;
			sAch.isAward_	= false;
			achievement_.push_back(sAch);
		}
		else
		{
			COM_Achievement* sAch = findAchievement(pData->achId_);
			if(sAch == NULL)
			{
				continue;
			}

			if(sAch->isAch_)		//成就已达成不在做计算
				continue;
			sAch->achValue_ = pData->value_;
		}
		checkAchievement(pData->achId_);
	}
}

void
Player::setAchievement(AchievementType achType, U32 achValue)
{
	if(achType < AT_None || achType > AT_Max)
	{
		ACE_DEBUG((LM_ERROR,"setAchievement[%d] Error!!!\n",achType));
		return;
	}
	if(achType > achValues_.size())
		return;
	achValues_[achType] = achValue;
	caleAchievement(achType,achValues_[achType]);
	if(sevenflag_)
		caleSevenday(achType,achValues_[achType]);
}

void
Player::addAchievement(AchievementType achType, U32 achValue)
{
	if(achType < AT_None || achType > AT_Max)
	{
		ACE_DEBUG((LM_ERROR,"addAchievement[%d] Error!!!\n",achType));
		return;
	}
	if(achType > achValues_.size())
		return;
	achValues_[achType] += achValue;
	caleAchievement(achType,achValues_[achType]);
	if(sevenflag_)
		caleSevenday(achType,achValues_[achType]);
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void Player::caleSevenday(AchievementType type,U32 qvalue){
	for (size_t i = 0; i < SevenDayTable::data_.size(); ++i)
	{
		SevenDayTable::SevenDay* pcore = SevenDayTable::data_[i];
		if(pcore == NULL)
			continue;
		if(pcore->type_ == type)
		{
			if(!isSevenday(pcore->quest_))
			{	
				COM_Sevenday cs;
				//cs.day_		= pcore->day_;
				cs.stype_	= type;
				cs.quest_	= pcore->quest_;
				cs.qvalue_	= qvalue;
				cs.isfinish_= false;
				cs.isreward_= false;
				sevenday_.push_back(cs);
			}
			else
			{
				COM_Sevenday* sAch = findSevenday(pcore->quest_);
				if(sAch == NULL){
					ACE_DEBUG((LM_ERROR,"canot find COM_Sevenday Error!!!\n"));
					continue;
				}
				if(sAch->isfinish_)		//成就已达成不在做计算
					continue;
				sAch->qvalue_ = qvalue;
			}
			checkSevenday(pcore->quest_);
		}
	}
}

void Player::checkSevenday(U32 qid){
	SevenDayTable::SevenDay const* pData = SevenDayTable::get(qid);
	if(pData == NULL)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("canot find SevenDayTable Error id[%d]!!!\n"),qid));
		return;
	}
	COM_Sevenday* pCom = findSevenday(qid);
	if(pCom == NULL)
	{
		//必须不为空
		ACE_DEBUG((LM_ERROR,ACE_TEXT("checkSevenday COM_Sevenday is nil id[%d]!!!\n"),qid));
		return;
	}

	if(pCom->isfinish_ || pCom->qvalue_ < pData->target_)
	{
		CALL_CLIENT(this,updateSevenday(*pCom));
		return;
	}
	pCom->isfinish_ = true;
	CALL_CLIENT(this,updateSevenday(*pCom));
}

bool Player::isSevenday(U32 qid){
	for (size_t i = 0; i < sevenday_.size(); ++i)
	{
		if(sevenday_[i].quest_ == qid)
			return true;
	}
	return false;
}

COM_Sevenday*
Player::findSevenday(U32 qid){
	for (size_t i = 0; i < sevenday_.size(); ++i)
	{
		if(sevenday_[i].quest_ == qid)
			return &sevenday_[i];
	}
	return NULL;
}

void Player::sevenReward(U32 qid){
	COM_Sevenday* pCom = findSevenday(qid);
	if(pCom == NULL)
		return;
	if(!pCom->isfinish_)
		return;
	SevenDayTable::SevenDay const* pData = SevenDayTable::get(qid);
	if(pData == NULL)
		return;
	if(pCom->isreward_)
		return;
	time_t curtime = WorldServ::instance()->curTime_;
	ACE_Date_Time creatTimeDT,nowDT;
	creatTimeDT.update(ACE_Time_Value(createTime_));
	nowDT.update(ACE_Time_Value(curtime));
	creatTimeDT.day();
	S32 daycd = (nowDT.day() - creatTimeDT.day()) + 1;
	if(pData->day_ > daycd)
		return;
	for (size_t i = 0; i < pData->reward_.size(); ++i)
	{
		addBagItemByItemId(pData->reward_[i].first,pData->reward_[i].second,false,985);
	}
	pCom->isreward_ = true;
	CALL_CLIENT(this,updateSevenday(*pCom));
}

void Player::sevendayClose(){
	sevenflag_ = false;
	sevenday_.clear();
	CALL_CLIENT(this,agencyActivity(ADT_7Days, sevenflag_));
}