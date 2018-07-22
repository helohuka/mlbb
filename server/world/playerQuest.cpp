#include "player.h"
#include "Quest.h"
#include "team.h"
#include "Scene.h"
#include "DropTable.h"
#include "worldserv.h"
#include "profession.h"
#include "sceneplayer.h"
#include "GameEvent.h"
#include "Guild.h"
///任务相关

S32
Player::getQuestIndex(S32 questId)
{
	for(size_t i=0; i<currentQuest_.size(); ++i)
	{
		if(questId == currentQuest_[i].questId_)
			return i;
	}
	return -1;
}

COM_QuestInst*
Player::getQuestInst(S32 questId)
{
	for(size_t i=0; i<currentQuest_.size(); ++i)
	{
		if(questId == currentQuest_[i].questId_)
			return &currentQuest_[i];
	}
	return NULL;
}

bool Player::isQuestComplate(S32 questId){
	for(size_t i=0; i<completeQuest_.size(); ++i){
		if(questId == completeQuest_[i])
			return true;
	}
	return false;
}

void
Player::addQuestCounter(S32 questId)
{
	for(size_t i=0; i<currentQuest_.size(); ++i){
		if(questId == currentQuest_[i].questId_){
			for (size_t j = 0; j < currentQuest_[i].targets_.size(); ++j){
				++(currentQuest_[i].targets_[j].targetNum_);
			}
			//ACE_DEBUG((LM_INFO, ACE_TEXT("postQuestEvent QT_Kill monsterID[%d] questNum_[%d]\n"),killedId,currentQuest_[i].questNum_));
			CALL_CLIENT(this,updateQuestInst(currentQuest_[i]));
			return;
		}
	}
}

void
Player::reduceQuestCounter(S32 questId)
{
	for(size_t i=0; i<currentQuest_.size(); ++i){
		if(questId == currentQuest_[i].questId_){
			for (size_t j = 0; j < currentQuest_[i].targets_.size(); ++j){
				--(currentQuest_[i].targets_[j].targetNum_);
				if(currentQuest_[i].targets_[j].targetNum_ < 0)
					currentQuest_[i].targets_[j].targetNum_ = 0;
			}
			CALL_CLIENT(this,updateQuestInst(currentQuest_[i]));
			return;
		}

	}
}

void Player::prepareAcceptQuest(S32 questId){
	const Quest* quest = Quest::getQuestById(questId);
	if(NULL == quest){
		CALL_CLIENT(this,errorno(EN_AcceptQuestNotFound));
		return;
	}
	if(quest->questType_ == QT_Item){
		//2141 【需求】背包位置不足的情况下，对于任务的一些处理
		if(!getBagEmptySlot()){
			CALL_CLIENT(this,errorno(EN_AcceptQuestBagMax));
			return ;
		}
	}
	
	if(quest->questKind_ == QK_Rand){
		if (accecptQuestCount_ > Global::get<int>(C_AccecptRandQuestLimit)){
			errorMessageToC(EN_AccecptRandQuestSizeLimitError);
			return;
		}
		if (hasQuestByType(QK_Rand)){
			return ;
		}
		
	}

	Team* p = myTeam();
	
	if(quest->questKind_ == QK_Guild){
		if(p && !p->isSameGuild())
		{
			errorMessageToC(EN_TeamMemberNoGuild);
			return;
		}
		else if(!myGuild())
		{
			errorMessageToC(EN_NoGuild);
			return;
		}
	}
	
	if(p && quest->questKind_ != QK_Profession && quest->questKind_ != QK_Rand && !isLeavingTeam_)
		p->acceptTeamQuest(this,questId);
	else{
		if(quest->questKind_ == QK_Tongji){
			U32 level = getProp(PT_Level);
			if(level < Global::get<int>(C_TongjiTeamMemberLevelMin))
				return;
			if(hasQuestByType(QK_Tongji))
				return;
			quest = Quest::randomTongjiQuest(level);
		}
		acceptQuest(quest->questId_);
	}
}

bool 
Player::acceptQuest(S32 questId){	
	const Quest* quest = Quest::getQuestById(questId);
	if(NULL == quest){
		CALL_CLIENT(this,errorno(EN_AcceptQuestNotFound));
		return true;
	}
	
	if (quest->questKind_ == QK_Rand){
		//随机
		quest = Quest::randomRandQuest();
		accecptQuestCount_++;
	}

	if(quest->questType_ == QT_Item){
		//2141 【需求】背包位置不足的情况下，对于任务的一些处理
		if(!getBagEmptySlot()){
			CALL_CLIENT(this,errorno(EN_AcceptQuestBagMax));
			return true;
		}
	}

	if(quest->needLevel_ > getProp(PT_Level))
		return true;

	for (size_t i = 0; i < quest->prevQuest_.size(); ++i){
		std::vector<S32>::iterator itr = std::find(completeQuest_.begin(),completeQuest_.end(),quest->prevQuest_[i]);

		if(itr==completeQuest_.end())
			return true;
	}

	for (size_t i = 0; i < currentQuest_.size(); ++i){
		if(currentQuest_[i].questId_ == questId)
			return true;
	}

	if(quest->questKind_ == QK_Wishing)
	{
		int32 count = 0;
		for(size_t i=0; i<completeQuest_.size(); ++i){
			const Quest* q = Quest::getQuestById(completeQuest_[i]);
			if(q && q->questKind_ == QK_Wishing){
				count++;
			}
		}

		if(count > 0)
			return true;
	}

	if(quest->questKind_ == QK_Daily)
	{
		int32 count = 0;
		for(size_t i=0; i<completeQuest_.size(); ++i){
			const Quest* q = Quest::getQuestById(completeQuest_[i]);
			if(q && q->questKind_ == QK_Daily){
				count++;
			}
		}
		if(count >= 10 ){
			CALL_CLIENT(this,errorno(EN_DailyNoNum));
			return true;
		}
		for(size_t i=0; i<currentQuest_.size();)
		{
			const Quest* q = Quest::getQuestById(currentQuest_[i].questId_);
			if(NULL == q)
				currentQuest_.erase(currentQuest_.begin()+i);
			else if(q->questKind_ == QK_Daily)
			{
				CALL_CLIENT(this,errorno(EN_AcceptSecendDaily));
				return true;
			}
			else 
				++i;
		}
	}
	else if(quest->questKind_ == QK_Profession)
	{
		if(quest->jl_ == 1 || quest->jl_ == 0){
		}
		else {
			if(quest->jt_ != (S32)getProp(PT_Profession))
				return true;
			if(quest->jl_ - (S32)getProp(PT_ProfessionLevel) != 1)
				return true;
		}

		for(size_t i=0; i<currentQuest_.size();)
		{
			const Quest* q = Quest::getQuestById(currentQuest_[i].questId_);
			if(NULL == q)
				currentQuest_.erase(currentQuest_.begin()+i);
			else if(q->questKind_ == QK_Profession)
			{
				CALL_CLIENT(this,errorno(EN_AcceptSecendProfession));
				return true;
			}
			else 
				++i;
		}

	}else if(quest->questKind_ == QK_Tongji){
		//等级 判断
	}

	if(!quest->needItemId_.empty()){	
		for (size_t i=0 ; i<quest->needItemId_.size(); ++i){
			std::vector<COM_Item*> items;
			getBagItemByItemId(quest->needItemId_[i],items);
			if(items.empty()){
				CALL_CLIENT(this,errorno(EN_AcceptQuestNoItem));
				return true;
			}
		}
	}
	
	if(quest->questKind_ != QK_Profession)
	{
		std::vector<S32>::iterator itr = std::find(completeQuest_.begin(),completeQuest_.end(),questId);
		if(itr!=completeQuest_.end())
			return true;
	}

	if(quest->questKind_ == QK_Guild)
	{
		if(myGuild() == NULL)
			return true;
	}

	COM_QuestInst inst;
	inst.questId_ = quest->questId_;

	for (size_t i = 0; i < quest->target_.size(); ++i)
	{
		COM_QuestTarget target;
		target.targetId_ = quest->target_[i];
		target.targetNum_= 0;
		inst.targets_.push_back(target);
	}

	///任务相关NPC 处理

	currentQuest_.push_back(inst);
	CALL_CLIENT(this,acceptQuestOk(inst));
	scenePlayer_->scenePlayerAddCurrentQuest(quest->questId_);
	postAcceptEvent(questId);

	return true;
}

void Player::prepareSubmitQuest(S32 npcId, S32 questId, int32 instId){
	S32 index = getQuestIndex(questId);
	if(index == -1)
		return ;

	const Quest* quest = Quest::getQuestById(questId);
	if(NULL == quest)
		return ;

	Team* p = isTeamLeader();
	if(p && quest->questKind_ != QK_Profession && quest->questKind_ != QK_Rand){
		p->submitTeamQuest(this,npcId,questId,instId);
		if(quest->questKind_ == QK_Tongji){
			///通缉自动接取下一个任务
			S32 level = p->getMinLevel();
			questId = Quest::randomTongjiQuest(level)->questId_;
			p->acceptTeamQuest(this,questId);
		}
	}
	else 
	{	
		submitQuest(npcId,questId,instId);
		if(quest->questKind_ == QK_Tongji){
			///通缉自动接取下一个任务
			S32 level = getProp(PT_Level);
			if(level < Global::get<int>(C_TongjiTeamMemberLevelMin))
				return;
			questId = Quest::randomTongjiQuest(level)->questId_;
			acceptQuest(questId);
		}
	}
	//有可能没有交
	if (quest->questKind_ == QK_Rand){
		//自动接一个跑环
		Quest const * q = Quest::randomRandQuest();
		if(q){
			prepareAcceptQuest(q->questId_);
		}
	}
}

bool 
Player::submitQuest(S32 npcId, S32 questId, int32 instId)
{
	S32 index = getQuestIndex(questId);
	if(index == -1)
		return true;

	const Quest* quest = Quest::getQuestById(questId);
	if(NULL == quest)
		return true;

	if(quest->submitNpcId_ != npcId)
		return true;
	
	if(quest->questType_ == QT_Item){
		for (size_t i = 0; i < quest->target_.size(); ++i){
			int32 num = getItemNumByItemId(quest->target_[i]);
			if(quest->targetNum_[i] > currentQuest_[index].targets_[i].targetNum_)
				return true;

			if(quest->targetNum_[i] > num){
				ACE_DEBUG((LM_ERROR,"Quest counter item number not match!!!\n"));
				return true;
			}
		}

		for (size_t i = 0; i < quest->target_.size(); ++i){
			int32 num = getItemNumByItemId(quest->target_[i]);
			ItemTable::ItemData const * data = ItemTable::getItemById(quest->target_[i]);
			if(NULL == data)
				continue;
			if(data->mainType_ != IMT_Quest)
				delBagItemByItemId(quest->target_[i], quest->targetNum_[i]);
			else
				delBagItemByItemId(quest->target_[i], num);
		}
	}
	else if(quest->questType_ == QT_Kill || quest->questType_ == QT_KillAI || quest->questType_ == QT_Other ){
		for (size_t i = 0; i < quest->target_.size(); ++i){
			COM_QuestTarget* tg = getQuestTarget(currentQuest_[index],quest->target_[i]);
			if(tg == NULL)
				return true;
			if(quest->targetNum_[i] > tg->targetNum_)
				return true;
		}
	}else if (quest->questType_ == QT_GiveItem){
		if(quest->target_.empty() ||quest->targetNum_.empty())
			return true;
		COM_Item * itemInst = getBagItemByInstId(instId);
		if (NULL == itemInst)
			return true;
		if(quest->target_[0] != itemInst->itemId_)
			return true;
		if(quest->targetNum_[0] > itemInst->stack_)
			return true;
		delBagItemByInstId(instId,quest->targetNum_[0]);
	}else if( quest->questType_ == QT_GiveBaby){
		if(quest->target_.empty())
			return true;
		Baby* babyInst = findBaby(instId);
		if(NULL == babyInst)
			return true;
		if(quest->target_[0] != int32(babyInst->getProp(PT_TableId)))
			return true;
		delBaby(instId);
	}
	
	const DropTable::Drop* pDrop = DropTable::getDropById(quest->dropId_);
	if(pDrop != NULL){
		U32 tmpdropid = 0;
	
		if(quest->questKind_ == QK_Tongji && getActivitionCount(ACT_Tongji) <= Global::get<int>(C_TongjiTimesMax))
			tmpdropid = quest->dropId_;
		else if(quest->questKind_ == QK_Tongji && getActivitionCount(ACT_Tongji) > Global::get<int>(C_TongjiTimesMax) && getActivitionCount(ACT_Tongji) <= Global::get<int>(C_TongjiMaxDrop))
			tmpdropid = quest->attenuationdrop_;
		else if(quest->questKind_ != QK_Tongji){
			tmpdropid = quest->dropId_;
		}

		if(pDrop->items_.size() > getBagEmptySlot()){//2141 【需求】背包位置不足的情况下，对于任务的一些处理
			enum {
				ARG0,
				ARG_MAX_,
			};
			GEParam param[ARG_MAX_];
			param[ARG0].type_  = GEP_INT;
			param[ARG0].value_.i = tmpdropid;
			GameEvent::procGameEvent(GET_BagFullSendMail,param,ARG_MAX_,handleId_);
		}
		else
			giveDrop(tmpdropid);
	}
	
	if(quest->questKind_ == QK_Profession && quest->jt_ != 0 && quest->jl_ != 0){
		U32 index = 0;
		for (size_t i = 0; i < equipItems_.size(); ++i)
		{
			if(equipItems_[i] == NULL)
				continue;
			const ItemTable::ItemData *itemData = ItemTable::getItemById(equipItems_[i]->itemId_);
			if(itemData == NULL)
				continue;
			const Profession* profession = Profession::get(quest->jt_,quest->jl_);
			if(NULL == profession)
				continue;
			if(!profession->canUseItem(itemData->subType_,itemData->level_))
				++index;
		}
		if( index > getBagEmptySlot() )
		{
			CALL_CLIENT(this,errorno(EN_SubmitQuestBagMax));
			return true;
		}

		enum {
			OLDJT,
			OLDJL,
			NEWJT,
			NEWJL,
			ARG_MAX_,
		};
		GEParam param[ARG_MAX_];
		param[OLDJT].type_  = GEP_INT;
		param[OLDJT].value_.i = getProp(PT_Profession);
		param[OLDJL].type_  = GEP_INT;
		param[OLDJL].value_.i = getProp(PT_ProfessionLevel);
		param[NEWJT].type_  = GEP_INT;
		param[NEWJT].value_.i = (int)quest->jt_;
		param[NEWJL].type_  = GEP_INT;
		param[NEWJL].value_.i = quest->jl_;
		GameEvent::procGameEvent(GET_ChangeProfession,param,ARG_MAX_,handleId_);

		setProp(PT_Profession,(int)quest->jt_);
		setProp(PT_ProfessionLevel,quest->jl_);
		WorldServ::instance()->updateContactInfo(this);
		transferCheckEquip();
		transferCheckSkill();

		if(getProp(PT_GuildID) != 0)
		{
			Guild::memberchangeProfession(this);
		}

		for(S32 i=0; i<completeQuest_.size();++i){
			const Quest* q = Quest::getQuestById(completeQuest_[i]);
			if(NULL == q || QK_Profession == q->questKind_)
				completeQuest_.erase(completeQuest_.begin() + i--);
		}
	}
	else if(quest->questKind_ == QK_Daily){
		enum {
			ARG0,
			ARG_MAX_,
		};
		GEParam param[ARG_MAX_];
		param[ARG0].type_  = GEP_INT;
		param[ARG0].value_.i = 1;
		GameEvent::procGameEvent(GET_Richang,param,ARG_MAX_,handleId_);
	}
	else if(quest->questKind_ == QK_Tongji){
		enum {
			ARG0,
			ARG_MAX_,
		};
		GEParam param[ARG_MAX_];
		param[ARG0].type_  = GEP_INT;
		param[ARG0].value_.i = 1;
		GameEvent::procGameEvent(GET_Tongji,param,ARG_MAX_,handleId_);
	}

	if (quest->questKind_ == QK_Rand){
		++submitQuestCount_;
		CALL_CLIENT(this,updateRandSubmitQuestCount(submitQuestCount_));
		
		float exp = CALC_RAND_QUEST_EXP(submitQuestCount_,getProp(PT_Level));
		if(submitQuestCount_ < Global::get<int>(C_AccecptRandQuestLimit) )
			addExp(exp,false);
		if(submitQuestCount_ == Global::get<int>(C_SubmitRandQuestRewaredNumber0)){
			giveDrop(Global::get<int>(C_SubmitRandQuestRewared0));
		}else if(submitQuestCount_ == Global::get<int>(C_SubmitRandQuestRewaredNumber1)){
			giveDrop(Global::get<int>(C_SubmitRandQuestRewared1));
		}
	}

	if(quest->questKind_ != QK_Tongji && quest->questKind_ != QK_Rand)
		completeQuest_.push_back(questId);
	currentQuest_.erase(currentQuest_.begin() + index);
	postSubmitEvent(questId);
	scenePlayer_->scenePlayerDelCurrentQuest(questId);
	CALL_CLIENT(this,submitQuestOk(questId));
	return true;
}

bool Player::cleanTeamQuest(){
	for(size_t i=0; i<currentQuest_.size(); ){
		const Quest*q = Quest::getQuestById(currentQuest_[i].questId_);
		if(NULL == q){
			currentQuest_.erase(currentQuest_.begin() + i);
			continue;
		}
		else{
			if(q->questKind_ == QK_Tongji){
				giveupQuest(q->questId_);
				continue;
			}
		}
		++i;
	}
	return true;
}

bool 
Player::giveupQuest(S32 questId)
{
	S32 index = getQuestIndex(questId);
	if(index == -1)
		return true;
	
	const Quest* quest = Quest::getQuestById(questId);
	if(NULL == quest)
		return true;
	
	currentQuest_.erase(currentQuest_.begin() + index);
	
	if(isTeamMember() && isTeamLeader()){ ///队伍任务同步
		Team* t = myTeam();
		for(size_t i=0; t != NULL && i<t->teamMembers_.size(); ++i){
			if(t->teamMembers_[i] == this) continue;
			t->teamMembers_[i]->giveupQuest(questId);
		}
	}
	scenePlayer_->scenePlayerDelCurrentQuest(questId);
	CALL_CLIENT(this,giveupQuestOk(questId));
	return true;
}

bool Player::hasQuestByType(QuestKind qk){
	for(size_t i=0; i<currentQuest_.size(); ++i){
		const Quest* quest = Quest::getQuestById(currentQuest_[i].questId_);
		if(NULL == quest)
			return false;
		if(quest->questKind_ == qk)
			return true;
	}
	return false;
}

bool
Player::postQuestEvent(S32 killedId,S32 killNum)
{
	for(size_t i=0; i<currentQuest_.size(); ++i)
	{
		const Quest* quest = Quest::getQuestById(currentQuest_[i].questId_);
		if(NULL == quest)
			continue;
		if(quest->questType_ == QT_Kill)
		{
			if(!isQuestTarget(currentQuest_[i],killedId))
				continue;

			COM_QuestTarget* tg = getQuestTarget(currentQuest_[i], killedId);
			if(tg == NULL)
			{
				COM_QuestTarget target;
				target.targetId_ = killedId;
				target.targetNum_= killNum;
				currentQuest_[i].targets_.push_back(target);
			}
			else
				tg->targetNum_ += killNum;

			if(checkQuestComplate(currentQuest_[i])){
				scenePlayer_->scenePlayerDelCurrentQuest(currentQuest_[i].questId_);
			}

			CALL_CLIENT(this,updateQuestInst(currentQuest_[i]));
		}
	}

	return true;
}

bool Player::postQuestKillAiEvent(S32 killedAiId, S32 killNum)
{
	for(size_t i=0; i<currentQuest_.size(); ++i)
	{
		const Quest* quest = Quest::getQuestById(currentQuest_[i].questId_);
		if(NULL == quest)
			continue;

		if(quest->questKind_ == QK_Wishing && quest->questType_ == QT_KillAI)
		{
			COM_QuestTarget* tg = getQuestTarget(currentQuest_[i], 0);
			if(tg == NULL)
			{
				COM_QuestTarget target;
				target.targetId_ = 0;
				target.targetNum_= killNum;
				currentQuest_[i].targets_.push_back(target);
			}
			else
				tg->targetNum_ += killNum;

			if(checkQuestComplate(currentQuest_[i])){
				scenePlayer_->scenePlayerDelCurrentQuest(currentQuest_[i].questId_);
			}

			CALL_CLIENT(this,updateQuestInst(currentQuest_[i]));
			continue;
		}

		if(quest->questType_ == QT_KillAI)
		{
			if(!isQuestTarget(currentQuest_[i],killedAiId))
				continue;

			COM_QuestTarget* tg = getQuestTarget(currentQuest_[i], killedAiId);
			if(tg == NULL)
			{
				COM_QuestTarget target;
				target.targetId_ = killedAiId;
				target.targetNum_= killNum;
				currentQuest_[i].targets_.push_back(target);
			}
			else
				tg->targetNum_ += killNum;

			if(checkQuestComplate(currentQuest_[i])){
				scenePlayer_->scenePlayerDelCurrentQuest(currentQuest_[i].questId_);
			}

			CALL_CLIENT(this,updateQuestInst(currentQuest_[i]));
		}
	}
	return true;
}

bool Player::postQuestItemEvent(S32 itemId, S32 itemNum)
{
	for(size_t i=0; i<currentQuest_.size(); ++i)
	{
		const Quest* quest = Quest::getQuestById(currentQuest_[i].questId_);
		if(NULL == quest)
			continue;

		if(quest->questType_ == QT_Item)
		{
			if(!isQuestTarget(currentQuest_[i],itemId))
			{
				CALL_CLIENT(this,updateQuestInst(currentQuest_[i]));
				continue;
			}
			COM_QuestTarget* tg = getQuestTarget(currentQuest_[i], itemId);
			if(tg == NULL)
			{
				COM_QuestTarget target;
				target.targetId_ = itemId;
				target.targetNum_= getItemNumByItemId(itemId);
				currentQuest_[i].targets_.push_back(target);
			}
			else
				tg->targetNum_ =  getItemNumByItemId(itemId);
			if(checkQuestComplate(currentQuest_[i])){
				scenePlayer_->scenePlayerDelCurrentQuest(currentQuest_[i].questId_);
			}
			CALL_CLIENT(this,updateQuestInst(currentQuest_[i]));
		}
	}

	return true;
}

S32
Player::checkQuestItem(S32 itemId)
{
	const ItemTable::ItemData* itemdata = ItemTable::getItemById(itemId);
	if(NULL == itemdata)
		return false;
	S32 has = getItemNumByItemId(itemId);
	if(itemdata->mainType_ == IMT_Quest)
	{
		for (size_t i = 0; i < currentQuest_.size(); ++i)
		{
			const Quest* quest = Quest::getQuestById(currentQuest_[i].questId_);

			if(quest == NULL)
				continue;
			if(quest->questType_ != QT_Item)
				continue;
			if(isQuestTarget(currentQuest_[i],itemId))
			{
				S32 index = getQuestTargetIndex(currentQuest_[i].questId_,itemId);
				return has < quest->targetNum_[index] ? quest->targetNum_[index]-has : 0;
			}
		}
	}

	return 0;
}

void Player::checkQuestItem(){
	for (size_t i = 0; i < bagItmes_.size(); ++i)
	{
		if(bagItmes_[i] == NULL)
			continue;
		postQuestItemEvent(bagItmes_[i]->itemId_, bagItmes_[i]->stack_);
	}
}

bool
Player::completeQuest(S32 questId)
{
	for (U32 i = 0; i < completeQuest_.size(); ++i)
	{
		if(completeQuest_[i] == questId)
			return true;
	}

	return false;
}

void
Player::gmAcceptQuest(S32 questId)
{
	const Quest* quest = Quest::getQuestById(questId);
	if(NULL == quest)
		return;

	COM_QuestInst inst;
	inst.questId_ = quest->questId_;
	for (size_t i = 0; i < quest->target_.size(); ++i)
	{
		COM_QuestTarget target;
		target.targetId_ = quest->target_[i];
		target.targetNum_= 0;
		inst.targets_.push_back(target);
	}
	currentQuest_.push_back(inst);
	postAcceptEvent(questId);
	CALL_CLIENT(this,acceptQuestOk(inst));
}


void
Player::gmJumpQuest(S32 questId)
{
	if(!Quest::getQuestById(questId))
		return;
	for(size_t i=0; i<completeQuest_.size(); ++i){
		if (completeQuest_[i] == questId)
			return;
	}

	completeQuest_.push_back(questId);
	CALL_CLIENT(this,submitQuestOk(questId));
}

void
Player::gmSubmitQuest(S32 questId)
{
	S32 index = getQuestIndex(questId);
	if(index == -1)
		return;

	const Quest* quest = Quest::getQuestById(questId);
	if(NULL == quest)
		return;

	currentQuest_.erase(currentQuest_.begin() + index);
	completeQuest_.push_back(questId);

	if(quest->jt_ != JT_None)
	{
		setProp(PT_Profession,(int)quest->jt_);
		setProp(PT_ProfessionLevel,quest->jl_);
		transferCheckEquip();
		transferCheckSkill();
	}
	giveDrop(quest->dropId_);
	postSubmitEvent(questId);
	CALL_CLIENT(this,submitQuestOk(questId));
}

void
Player::transferCheckEquip()
{
	JobType jt = (JobType)(int)getProp(PT_Profession);
	U32 level = (U32) getProp(PT_ProfessionLevel);

	for (size_t i = 0; i < equipItems_.size(); ++i)
	{
		if(equipItems_[i] == NULL)
			continue;

		const ItemTable::ItemData *itemData = ItemTable::getItemById(equipItems_[i]->itemId_);
		if(itemData == NULL)
			continue;
		const Profession* profession = Profession::get(jt,level);
		if(NULL == profession)
			continue;
		if(profession->canUseItem(itemData->subType_,itemData->level_))
			continue;
		delEquipment(getGUID(),equipItems_[i]->instId_);
	}
}

void
Player::transferCheckSkill()
{
	JobType jt = (JobType)(int)getProp(PT_Profession);
	U32 level = (U32) getProp(PT_ProfessionLevel);

	const Profession* profession = Profession::get(jt,level);
	if(NULL == profession)
		return;
	
	//check 不能用的技能
	std::vector<int> invalidSkills;
	for (size_t i = 0; i < skills_.size(); ++i){
		if(!profession->canLearnSkill(skills_[i]->skId_)){
			SkillTable::Core const *pCore = SkillTable::getSkillById(skills_[i]->skId_,skills_[i]->skLevel_);
			if(NULL == pCore || pCore->learnGroup_ == 0 )
				continue;
			invalidSkills.push_back(skills_[i]->skId_);
		}
	}

	//删除不能用的技能
	for (size_t i = 0; i < invalidSkills.size(); ++i){
		forgetSkill(invalidSkills[i]);
	}

	//从新刷新职业学习技能最高等级
	for (size_t i = 0; i < skills_.size(); ++i){
		SkillTable::Core const *pCore = SkillTable::getSkillById(skills_[i]->skId_,skills_[i]->skLevel_);
		if(NULL == pCore || pCore->learnGroup_ == 0 )
			continue;
		if(skills_[i]->skLevel_ >= profession->getSkillMaxLevel(skills_[i]->skId_))
		{
			if(pCore->skType_ == SKT_CannotUse){
				setProp(pCore->resistPropType_, getProp(pCore->resistPropType_) - pCore->resistNum_);
			}

			skills_[i]->skLevel_ = profession->getSkillMaxLevel(skills_[i]->skId_);
			skills_[i]->skExp_ = 0;
			skills_[i]->reset();
			COM_Skill inst;
			inst.skillID_ = skills_[i]->skId_;
			inst.skillExp_ = skills_[i]->skExp_;
			inst.skillLevel_ = skills_[i]->skLevel_;
			CALL_CLIENT(this,skillLevelUp(getGUID(),inst));
		
			pCore = SkillTable::getSkillById(skills_[i]->skId_,skills_[i]->skLevel_);
			if(pCore->skType_ == SKT_CannotUse){
				setProp(pCore->resistPropType_, getProp(pCore->resistPropType_) + pCore->resistNum_);
			}
		}
	}
	//resetPassiveSkill();
}

void Player::postAcceptEvent(S32 questId)
{
	const Quest *p = Quest::getQuestById(questId);
	if(NULL == p) return;
	if(p->acceptScript_.empty())return;

	GEParam param[1];
	param[0].type_ = GEP_INT;
	param[0].value_.i = questId;

	std::string err;
	if(false == ScriptEnv::callGEProc(p->acceptScript_.c_str(),handleId_,param,1,err)){
		ACE_DEBUG((LM_INFO, ACE_TEXT("false == ScriptEnv::callGEProc(p->acceptScript_.c_str() Script====>%s \n"),p->acceptScript_.c_str()));
	}
}

void Player::postSubmitEvent(S32 questId)
{
	const Quest *p = Quest::getQuestById(questId);
	if(NULL == p) return;
	if(p->submitScript_.empty())return;

	if(p->title_ != 0)
		addPlayerTitle(p->title_);

	GEParam param[1];
	param[0].type_ = GEP_INT;
	param[0].value_.i = questId;

	std::string err;
	if(false == ScriptEnv::callGEProc(p->submitScript_.c_str(),handleId_,param,1,err)){
		ACE_DEBUG((LM_INFO, ACE_TEXT("false == ScriptEnv::callGEProc(p->submitScript_.c_str() Script====>%s \n"),p->acceptScript_.c_str()));
	}
}

COM_QuestTarget*
Player::getQuestTarget(COM_QuestInst &inst, U32 target)
{
	//inst当前任务
	for (size_t i = 0; i < inst.targets_.size(); ++i)
	{
		if(inst.targets_[i].targetId_ == target)
			return &inst.targets_[i];
	}

	return NULL;
}

bool Player::checkQuestComplate(COM_QuestInst& inst){
	Quest const* quest = Quest::getQuestById(inst.questId_);
	if(!quest)
		return true;
	
	if(quest->questType_ == QT_Item){
		for (size_t i = 0; i < quest->target_.size(); ++i){
			int32 num = getItemNumByItemId(quest->target_[i]);
			if(inst.targets_[i].targetNum_ != num){
				inst.targets_[i].targetNum_ = num;
				//CALL_CLIENT(this,updateQuestInst(inst));
			}
			if(quest->targetNum_[i] > inst.targets_[i].targetNum_)
				return false;
		}
	}
	else{
		for (size_t i = 0; i < quest->target_.size(); ++i)
		{
			for(size_t j=0; j<inst.targets_.size(); ++j){
				if((inst.targets_[j].targetId_ == quest->target_[i]) && (inst.targets_[j].targetNum_ < quest->targetNum_[i]))
					return false;
			}
		}
	}

	
	return true;
}

bool
Player::isQuestTarget(COM_QuestInst& inst,U32 targetId)
{
	const Quest* quest = Quest::getQuestById(inst.questId_);
	if(NULL == quest)
		return false;
	for (size_t i = 0; i < quest->target_.size(); ++i)
	{
		if(targetId == quest->target_[i])
		{
			COM_QuestTarget* tg = getQuestTarget(inst,targetId);
			if(tg == NULL)
				return false;
			if(quest->questType_ == QT_Item){
				tg->targetNum_ = getItemNumByItemId(tg->targetId_);
			}
			if(tg->targetNum_ < quest->targetNum_[i])
				return true;
		}
	}
	return false;
}

S32
Player::getQuestTargetIndex(S32 questId,U32 targetId)
{
	const Quest* quest = Quest::getQuestById(questId);
	if(NULL == quest)
		return 0;
	for (size_t i = 0; i < quest->target_.size(); ++i)
	{
		if(targetId == quest->target_[i])
			return i;
	}

	return 0;
}

void
Player::cleanCopyQuest()
{
	bool isChange = false;
	for(size_t i=0; i<currentQuest_.size(); ++i){
		const Quest*q = Quest::getQuestById(currentQuest_[i].questId_);
		if(NULL == q){
			currentQuest_.erase(currentQuest_.begin() + i--);
		}
		else if (q->questKind_ == QK_Copy){
			isChange = true;
			currentQuest_.erase(currentQuest_.begin() + i--);
		}
	}

	for(size_t i=0; i<completeQuest_.size(); ++i){
		const Quest*q = Quest::getQuestById(completeQuest_[i]);
		if(NULL == q){
			completeQuest_.erase(completeQuest_.begin() + i--);
		}
		else if (q->questKind_ == QK_Copy){
			isChange = true;
			completeQuest_.erase(completeQuest_.begin() + i--);
		}
	}
	if(	isChange)
		CALL_CLIENT(this,initQuest(currentQuest_,completeQuest_));
}

void
Player::cleanGuildQuest()
{
	for(size_t i=0; i<currentQuest_.size(); ++i){
		const Quest*q = Quest::getQuestById(currentQuest_[i].questId_);
		if(NULL == q){
			currentQuest_.erase(currentQuest_.begin() + i--);
		}
		else if (q->questKind_ == QK_Guild){
			currentQuest_.erase(currentQuest_.begin() + i--);
		}
	}

	for(size_t i=0; i<completeQuest_.size(); ++i){
		const Quest*q = Quest::getQuestById(completeQuest_[i]);
		if(NULL == q){
			completeQuest_.erase(completeQuest_.begin() + i--);
		}
		else if (q->questKind_ == QK_Guild){
			completeQuest_.erase(completeQuest_.begin() + i--);
		}
	}

	CALL_CLIENT(this,initQuest(currentQuest_,completeQuest_));
}