#ifndef __TRANSLATION_H__
#define __TRANSLATION_H__
#include "config.h"
#include "struct.h"
#include "structold.h"


static inline void TranslationDBData(SGE_DBPlayerData_Old const &old, SGE_DBPlayerData & newd){
	*((COM_Entity*)(&newd)) = *((COM_Entity*)(&old));

#define __TRANS_SET(NAME) newd.NAME = old.NAME;
	
	newd.versionNumber_ = VERSION_NUMBER;
	__TRANS_SET(isLeavingTeam_);
	__TRANS_SET(isBattle_);
	__TRANS_SET(autoBattle_);
	__TRANS_SET(isTeamLeader_);
	__TRANS_SET(sceneId_);
	//__TRANS_SET(openSubSystemFlag_);
	newd.openSubSystemFlag_ = newd.openSubSystemFlag_ | uint64(old.openSubSystemFlag_) & 0X00000000FFFFFFFF;
	__TRANS_SET(createTime_);
	__TRANS_SET(guildName_);
	__TRANS_SET(scenePos_);
	__TRANS_SET(pvpInfo_);
	__TRANS_SET(onlineTimeFlag_);
	__TRANS_SET(onlineTime_);
	__TRANS_SET(onlineTimeReward_);
	__TRANS_SET(isFund_);
	__TRANS_SET(fundtags_);
	__TRANS_SET(openDoubleTimeFlag_);
	__TRANS_SET(isFirstLogin_);
	__TRANS_SET(firstRechargeDiamond_);
	__TRANS_SET(isFirstRechargeGift_);
	__TRANS_SET(offlineExp_);
	__TRANS_SET(rivalTime_);
	__TRANS_SET(rivalNum_);
	__TRANS_SET(promoteAward_);
	__TRANS_SET(guideIdx_);
	__TRANS_SET(noTalkTime_);
	__TRANS_SET(wishShareNum_);
	__TRANS_SET(warriortrophyNum_);
	__TRANS_SET(employeelasttime_);
	__TRANS_SET(employeeonecount_);
	__TRANS_SET(employeetencount_);
	__TRANS_SET(greenBoxTimes_);
	__TRANS_SET(blueBoxTimes_);
	__TRANS_SET(greenBoxFreeNum_);
	__TRANS_SET(hbInfo_);
	__TRANS_SET(openScenes_);
	__TRANS_SET(copyNum_);
	__TRANS_SET(magicItemLevel_);
	__TRANS_SET(magicItemeExp_);
	__TRANS_SET(magicItemeJob_);
	__TRANS_SET(magicTupoLevel_);
	__TRANS_SET(cachedNpcs_);
	__TRANS_SET(gft_);
	__TRANS_SET(babycache_);
	__TRANS_SET(titles_);
	__TRANS_SET(guildContribution_);
	__TRANS_SET(exitGuildTime_);
	__TRANS_SET(guildSkills_);
	__TRANS_SET(gmActivities_);
	__TRANS_SET(festival_);
	__TRANS_SET(selfRecharge_);
	__TRANS_SET(sysRecharge_);
	__TRANS_SET(selfDiscountStore_);
	__TRANS_SET(sysDiscountStore_);
	__TRANS_SET(selfOnceRecharge_);
	__TRANS_SET(sysOnceRecharge_);
	__TRANS_SET(empact_);
	__TRANS_SET(selfCards_);
	__TRANS_SET(myselfRecharge_);
	__TRANS_SET(hotdata_);
	__TRANS_SET(gbdata_);
	__TRANS_SET(sevenflag_);
	__TRANS_SET(signFlag_);
	__TRANS_SET(sevendata_);
	__TRANS_SET(viprewardflag_);
	__TRANS_SET(phoneNumber_);
	__TRANS_SET(levelgift_);
	__TRANS_SET(activity_);
	__TRANS_SET(fuwen_);
	__TRANS_SET(freeze_);
	__TRANS_SET(seal_);
	__TRANS_SET(signs_);
	__TRANS_SET(sellIdMax_);
	__TRANS_SET(acceptRandQuestCounter_);
	__TRANS_SET(submitRandQuestCounter_);
	__TRANS_SET(pfid_);
	__TRANS_SET(orders_);
	__TRANS_SET(loginTime_);
	__TRANS_SET(logoutTime_);
	__TRANS_SET(genItemMaxGuid_);
	__TRANS_SET(gaterMaxNum_);
	__TRANS_SET(firstRollEmployeeCon_);
	__TRANS_SET(firstRollEmployeeDia_);
	__TRANS_SET(employees_);
	__TRANS_SET(itemStorage_);
	__TRANS_SET(babyStorage_);
	__TRANS_SET(babies_);
	__TRANS_SET(bagItems_);
	__TRANS_SET(quests_);
	__TRANS_SET(completeQuests_);
	__TRANS_SET(mineReward_);
	__TRANS_SET(jjcBattleMsg_);
	__TRANS_SET(friend_);
	__TRANS_SET(blacklist_);
	__TRANS_SET(achValues_);
	__TRANS_SET(achievement_);
	__TRANS_SET(empBattleGroup_);
	__TRANS_SET(employeeGroup1_);
	__TRANS_SET(employeeGroup2_);
	__TRANS_SET(gatherData_);
	__TRANS_SET(compoundList_);
	newd.itemStoreSize_ = old.itemStorage_.size();
	if(old.babyStorage_.size() < 6)
		newd.babyStoreSize_ = 6;
	else if (old.babyStorage_.size() > 30 )
		newd.babyStoreSize_ = 30;
	else 
		newd.babyStoreSize_ = UtlMath::ceil(float(old.babyStorage_.size())/6.F) * 6;
#undef __TRANS_SET
}

#endif