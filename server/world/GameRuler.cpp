#include "config.h"
#include "player.h"
#include "team.h"
#include "GameRuler.h"
#include "Activity.h"
#include "Activities.h"
#include "BattleData.h"
#include "PeriodEvent.h"
#include "challengeTable.h"
bool GameRuler::CanPetActivity(Player* player,S32 battleId){
	const PetActivity* pa = PetActivity::getPetActivityByBattleId(battleId);
	if(NULL == pa){
		ACE_DEBUG((LM_ERROR,"this is pet act battle but not find pet act data (%d)\n",battleId));
		return false;
	}
	if(!pa->isOpen(PeriodEvent::getCurWeek())){
		ACE_DEBUG((LM_ERROR,"this is pet act battle but not open (%d)\n",pa->petActId_));
		return false;
	}
	if(!pa->condition((S32)player->getProp(PT_Level),battleId)){
		ACE_DEBUG((LM_ERROR,"this is pet act battle but condition fail (%d)\n",pa->petActId_));
		return false;
	}
	return player&&(player->getActivitionCount(ACT_Pet) < Global::get<int>(C_PetActivityNum));
}
ErrorNo GameRuler::CanHundredBattle(Player* player,S32 battleId){
	ChallengeTable::Core const *tmp = ChallengeTable::getDataByBattleId(battleId);
	if(tmp == NULL){
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Don't find this Data[%d] in the ChallengeTable table\n"),battleId));
		return EN_Max;
	}

	if(player == NULL)
		return EN_Max;
	if(player->getProp(PT_Level) < Global::get<int>(C_HundredBattle))
		return EN_HunderdLevel;
	if(player->hundredNum_ <= 0)
		return EN_HunderdNoNum;
	if(tmp->id_ != player->tier_)		//只能挑战最高层
		return EN_HunderdTier;

	player->curTier_ = tmp->id_;

	return EN_None;

}