/* arpcc auto generated cpp file. */
#include "FieldMask.h"
#include "structold.h"
//=============================================================
SGE_PlayerInst_Old::SGE_PlayerInst_Old():
isLeavingTeam_(false)
,isBattle_(false)
,autoBattle_(false)
,isTeamLeader_(false)
,sceneId_(0)
,openSubSystemFlag_(0)
,createTime_(0)
,onlineTimeFlag_(false)
,onlineTime_(0)
,isFund_(false)
,openDoubleTimeFlag_(false)
,isFirstLogin_(false)
,firstRechargeDiamond_(false)
,isFirstRechargeGift_(false)
,offlineExp_(0)
,rivalTime_(0)
,rivalNum_(0)
,promoteAward_(0)
,guideIdx_(0)
,noTalkTime_(0)
,wishShareNum_(0)
,warriortrophyNum_(0)
,employeelasttime_(0)
,employeeonecount_(0)
,employeetencount_(0)
,greenBoxTimes_(0)
,blueBoxTimes_(0)
,greenBoxFreeNum_(0)
,magicItemLevel_(0)
,magicItemeExp_(0)
,magicItemeJob_((JobType)(0))
,magicTupoLevel_(0)
,guildContribution_(0)
,exitGuildTime_(0)
,sevenflag_(false)
,signFlag_(false)
,viprewardflag_(false)
{}
void SGE_PlayerInst_Old::serialize(ProtocolWriter* __s__) const
{
	COM_Entity::serialize(__s__);
	//field mask
	FieldMask<9> __fm__;
	__fm__.writeBit(isLeavingTeam_);
	__fm__.writeBit(isBattle_);
	__fm__.writeBit(autoBattle_);
	__fm__.writeBit(isTeamLeader_);
	__fm__.writeBit((sceneId_==0)?false:true);
	__fm__.writeBit((openSubSystemFlag_==0)?false:true);
	__fm__.writeBit((createTime_==0)?false:true);
	__fm__.writeBit(guildName_.length()?true:false);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(onlineTimeFlag_);
	__fm__.writeBit((onlineTime_==0)?false:true);
	__fm__.writeBit(onlineTimeReward_.size()?true:false);
	__fm__.writeBit(isFund_);
	__fm__.writeBit(fundtags_.size()?true:false);
	__fm__.writeBit(openDoubleTimeFlag_);
	__fm__.writeBit(isFirstLogin_);
	__fm__.writeBit(firstRechargeDiamond_);
	__fm__.writeBit(isFirstRechargeGift_);
	__fm__.writeBit((offlineExp_==0)?false:true);
	__fm__.writeBit((rivalTime_==0)?false:true);
	__fm__.writeBit((rivalNum_==0)?false:true);
	__fm__.writeBit((promoteAward_==0)?false:true);
	__fm__.writeBit((guideIdx_==0)?false:true);
	__fm__.writeBit((noTalkTime_==0)?false:true);
	__fm__.writeBit((wishShareNum_==0)?false:true);
	__fm__.writeBit((warriortrophyNum_==0)?false:true);
	__fm__.writeBit((employeelasttime_==0)?false:true);
	__fm__.writeBit((employeeonecount_==0)?false:true);
	__fm__.writeBit((employeetencount_==0)?false:true);
	__fm__.writeBit((greenBoxTimes_==0)?false:true);
	__fm__.writeBit((blueBoxTimes_==0)?false:true);
	__fm__.writeBit((greenBoxFreeNum_==0)?false:true);
	__fm__.writeBit(true);
	__fm__.writeBit(openScenes_.size()?true:false);
	__fm__.writeBit(copyNum_.size()?true:false);
	__fm__.writeBit((magicItemLevel_==0)?false:true);
	__fm__.writeBit((magicItemeExp_==0)?false:true);
	__fm__.writeBit((magicItemeJob_==(JobType)(0))?false:true);
	__fm__.writeBit((magicTupoLevel_==0)?false:true);
	__fm__.writeBit(cachedNpcs_.size()?true:false);
	__fm__.writeBit(gft_.size()?true:false);
	__fm__.writeBit(babycache_.size()?true:false);
	__fm__.writeBit(titles_.size()?true:false);
	__fm__.writeBit((guildContribution_==0)?false:true);
	__fm__.writeBit((exitGuildTime_==0)?false:true);
	__fm__.writeBit(guildSkills_.size()?true:false);
	__fm__.writeBit(gmActivities_.size()?true:false);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(true);
	__fm__.writeBit(sevenflag_);
	__fm__.writeBit(signFlag_);
	__fm__.writeBit(sevendata_.size()?true:false);
	__fm__.writeBit(viprewardflag_);
	__fm__.writeBit(phoneNumber_.length()?true:false);
	__fm__.writeBit(levelgift_.size()?true:false);
	__fm__.writeBit(true);
	__fm__.writeBit(fuwen_.size()?true:false);
	__s__->write(__fm__.masks_, 9);
	// serialize isLeavingTeam_
	{
	}
	// serialize isBattle_
	{
	}
	// serialize autoBattle_
	{
	}
	// serialize isTeamLeader_
	{
	}
	// serialize sceneId_
	{
		if(sceneId_ != 0){
		__s__->writeType(sceneId_);
		}
	}
	// serialize openSubSystemFlag_
	{
		if(openSubSystemFlag_ != 0){
		__s__->writeType(openSubSystemFlag_);
		}
	}
	// serialize createTime_
	{
		if(createTime_ != 0){
		__s__->writeType(createTime_);
		}
	}
	// serialize guildName_
	{
		if(guildName_.length()){
		__s__->writeType(guildName_);
		}
	}
	// serialize scenePos_
	{
		scenePos_.serialize(__s__);
	}
	// serialize pvpInfo_
	{
		pvpInfo_.serialize(__s__);
	}
	// serialize onlineTimeFlag_
	{
	}
	// serialize onlineTime_
	{
		if(onlineTime_ != 0){
		__s__->writeType(onlineTime_);
		}
	}
	// serialize onlineTimeReward_
	if(onlineTimeReward_.size())
	{
		size_t __len__ = (size_t)onlineTimeReward_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(onlineTimeReward_[i]);
		}
	}
	// serialize isFund_
	{
	}
	// serialize fundtags_
	if(fundtags_.size())
	{
		size_t __len__ = (size_t)fundtags_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(fundtags_[i]);
		}
	}
	// serialize openDoubleTimeFlag_
	{
	}
	// serialize isFirstLogin_
	{
	}
	// serialize firstRechargeDiamond_
	{
	}
	// serialize isFirstRechargeGift_
	{
	}
	// serialize offlineExp_
	{
		if(offlineExp_ != 0){
		__s__->writeType(offlineExp_);
		}
	}
	// serialize rivalTime_
	{
		if(rivalTime_ != 0){
		__s__->writeType(rivalTime_);
		}
	}
	// serialize rivalNum_
	{
		if(rivalNum_ != 0){
		__s__->writeType(rivalNum_);
		}
	}
	// serialize promoteAward_
	{
		if(promoteAward_ != 0){
		__s__->writeType(promoteAward_);
		}
	}
	// serialize guideIdx_
	{
		if(guideIdx_ != 0){
		__s__->writeType(guideIdx_);
		}
	}
	// serialize noTalkTime_
	{
		if(noTalkTime_ != 0){
		__s__->writeType(noTalkTime_);
		}
	}
	// serialize wishShareNum_
	{
		if(wishShareNum_ != 0){
		__s__->writeType(wishShareNum_);
		}
	}
	// serialize warriortrophyNum_
	{
		if(warriortrophyNum_ != 0){
		__s__->writeType(warriortrophyNum_);
		}
	}
	// serialize employeelasttime_
	{
		if(employeelasttime_ != 0){
		__s__->writeType(employeelasttime_);
		}
	}
	// serialize employeeonecount_
	{
		if(employeeonecount_ != 0){
		__s__->writeType(employeeonecount_);
		}
	}
	// serialize employeetencount_
	{
		if(employeetencount_ != 0){
		__s__->writeType(employeetencount_);
		}
	}
	// serialize greenBoxTimes_
	{
		if(greenBoxTimes_ != 0){
		__s__->writeType(greenBoxTimes_);
		}
	}
	// serialize blueBoxTimes_
	{
		if(blueBoxTimes_ != 0){
		__s__->writeType(blueBoxTimes_);
		}
	}
	// serialize greenBoxFreeNum_
	{
		if(greenBoxFreeNum_ != 0){
		__s__->writeType(greenBoxFreeNum_);
		}
	}
	// serialize hbInfo_
	{
		hbInfo_.serialize(__s__);
	}
	// serialize openScenes_
	if(openScenes_.size())
	{
		size_t __len__ = (size_t)openScenes_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(openScenes_[i]);
		}
	}
	// serialize copyNum_
	if(copyNum_.size())
	{
		size_t __len__ = (size_t)copyNum_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(copyNum_[i]);
		}
	}
	// serialize magicItemLevel_
	{
		if(magicItemLevel_ != 0){
		__s__->writeType(magicItemLevel_);
		}
	}
	// serialize magicItemeExp_
	{
		if(magicItemeExp_ != 0){
		__s__->writeType(magicItemeExp_);
		}
	}
	// serialize magicItemeJob_
	{
		EnumSize __e__ = (EnumSize)magicItemeJob_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize magicTupoLevel_
	{
		if(magicTupoLevel_ != 0){
		__s__->writeType(magicTupoLevel_);
		}
	}
	// serialize cachedNpcs_
	if(cachedNpcs_.size())
	{
		size_t __len__ = (size_t)cachedNpcs_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(cachedNpcs_[i]);
		}
	}
	// serialize gft_
	if(gft_.size())
	{
		size_t __len__ = (size_t)gft_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(gft_[i]);
		}
	}
	// serialize babycache_
	if(babycache_.size())
	{
		size_t __len__ = (size_t)babycache_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(babycache_[i]);
		}
	}
	// serialize titles_
	if(titles_.size())
	{
		size_t __len__ = (size_t)titles_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(titles_[i]);
		}
	}
	// serialize guildContribution_
	{
		if(guildContribution_ != 0){
		__s__->writeType(guildContribution_);
		}
	}
	// serialize exitGuildTime_
	{
		if(exitGuildTime_ != 0){
		__s__->writeType(exitGuildTime_);
		}
	}
	// serialize guildSkills_
	if(guildSkills_.size())
	{
		size_t __len__ = (size_t)guildSkills_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			guildSkills_[i].serialize(__s__);
		}
	}
	// serialize gmActivities_
	if(gmActivities_.size())
	{
		size_t __len__ = (size_t)gmActivities_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			EnumSize __e__ = (EnumSize)gmActivities_[i];
			__s__->writeType(__e__);
		}
	}
	// serialize festival_
	{
		festival_.serialize(__s__);
	}
	// serialize selfRecharge_
	{
		selfRecharge_.serialize(__s__);
	}
	// serialize sysRecharge_
	{
		sysRecharge_.serialize(__s__);
	}
	// serialize selfDiscountStore_
	{
		selfDiscountStore_.serialize(__s__);
	}
	// serialize sysDiscountStore_
	{
		sysDiscountStore_.serialize(__s__);
	}
	// serialize selfOnceRecharge_
	{
		selfOnceRecharge_.serialize(__s__);
	}
	// serialize sysOnceRecharge_
	{
		sysOnceRecharge_.serialize(__s__);
	}
	// serialize empact_
	{
		empact_.serialize(__s__);
	}
	// serialize selfCards_
	{
		selfCards_.serialize(__s__);
	}
	// serialize myselfRecharge_
	{
		myselfRecharge_.serialize(__s__);
	}
	// serialize hotdata_
	{
		hotdata_.serialize(__s__);
	}
	// serialize gbdata_
	{
		gbdata_.serialize(__s__);
	}
	// serialize sevenflag_
	{
	}
	// serialize signFlag_
	{
	}
	// serialize sevendata_
	if(sevendata_.size())
	{
		size_t __len__ = (size_t)sevendata_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			sevendata_[i].serialize(__s__);
		}
	}
	// serialize viprewardflag_
	{
	}
	// serialize phoneNumber_
	{
		if(phoneNumber_.length()){
		__s__->writeType(phoneNumber_);
		}
	}
	// serialize levelgift_
	if(levelgift_.size())
	{
		size_t __len__ = (size_t)levelgift_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(levelgift_[i]);
		}
	}
	// serialize activity_
	{
		activity_.serialize(__s__);
	}
	// serialize fuwen_
	if(fuwen_.size())
	{
		size_t __len__ = (size_t)fuwen_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			fuwen_[i].serialize(__s__);
		}
	}
}
bool SGE_PlayerInst_Old::deserialize(ProtocolReader* __r__)
{
	if(!COM_Entity::deserialize(__r__)) return false;
	//field mask
	FieldMask<9> __fm__;
	if(!__r__->read(__fm__.masks_, 9)) return false;
	// deserialize isLeavingTeam_
	{
		isLeavingTeam_ = __fm__.readBit();
	}
	// deserialize isBattle_
	{
		isBattle_ = __fm__.readBit();
	}
	// deserialize autoBattle_
	{
		autoBattle_ = __fm__.readBit();
	}
	// deserialize isTeamLeader_
	{
		isTeamLeader_ = __fm__.readBit();
	}
	// deserialize sceneId_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sceneId_)) return false;
		}
	}
	// deserialize openSubSystemFlag_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(openSubSystemFlag_)) return false;
		}
	}
	// deserialize createTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(createTime_)) return false;
		}
	}
	// deserialize guildName_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guildName_, 65535)) return false;
		}
	}
	// deserialize scenePos_
	{
		if(__fm__.readBit()){
		if(!scenePos_.deserialize(__r__)) return false;
		}
	}
	// deserialize pvpInfo_
	{
		if(__fm__.readBit()){
		if(!pvpInfo_.deserialize(__r__)) return false;
		}
	}
	// deserialize onlineTimeFlag_
	{
		onlineTimeFlag_ = __fm__.readBit();
	}
	// deserialize onlineTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(onlineTime_)) return false;
		}
	}
	// deserialize onlineTimeReward_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		onlineTimeReward_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(onlineTimeReward_[i])) return false;
		}
	}
	// deserialize isFund_
	{
		isFund_ = __fm__.readBit();
	}
	// deserialize fundtags_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		fundtags_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(fundtags_[i])) return false;
		}
	}
	// deserialize openDoubleTimeFlag_
	{
		openDoubleTimeFlag_ = __fm__.readBit();
	}
	// deserialize isFirstLogin_
	{
		isFirstLogin_ = __fm__.readBit();
	}
	// deserialize firstRechargeDiamond_
	{
		firstRechargeDiamond_ = __fm__.readBit();
	}
	// deserialize isFirstRechargeGift_
	{
		isFirstRechargeGift_ = __fm__.readBit();
	}
	// deserialize offlineExp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(offlineExp_)) return false;
		}
	}
	// deserialize rivalTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rivalTime_)) return false;
		}
	}
	// deserialize rivalNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(rivalNum_)) return false;
		}
	}
	// deserialize promoteAward_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(promoteAward_)) return false;
		}
	}
	// deserialize guideIdx_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guideIdx_)) return false;
		}
	}
	// deserialize noTalkTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(noTalkTime_)) return false;
		}
	}
	// deserialize wishShareNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(wishShareNum_)) return false;
		}
	}
	// deserialize warriortrophyNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(warriortrophyNum_)) return false;
		}
	}
	// deserialize employeelasttime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(employeelasttime_)) return false;
		}
	}
	// deserialize employeeonecount_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(employeeonecount_)) return false;
		}
	}
	// deserialize employeetencount_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(employeetencount_)) return false;
		}
	}
	// deserialize greenBoxTimes_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(greenBoxTimes_)) return false;
		}
	}
	// deserialize blueBoxTimes_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(blueBoxTimes_)) return false;
		}
	}
	// deserialize greenBoxFreeNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(greenBoxFreeNum_)) return false;
		}
	}
	// deserialize hbInfo_
	{
		if(__fm__.readBit()){
		if(!hbInfo_.deserialize(__r__)) return false;
		}
	}
	// deserialize openScenes_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		openScenes_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(openScenes_[i])) return false;
		}
	}
	// deserialize copyNum_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		copyNum_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(copyNum_[i])) return false;
		}
	}
	// deserialize magicItemLevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(magicItemLevel_)) return false;
		}
	}
	// deserialize magicItemeExp_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(magicItemeExp_)) return false;
		}
	}
	// deserialize magicItemeJob_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 12) return false;
		magicItemeJob_ = (JobType)__e__;
		}
	}
	// deserialize magicTupoLevel_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(magicTupoLevel_)) return false;
		}
	}
	// deserialize cachedNpcs_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		cachedNpcs_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(cachedNpcs_[i])) return false;
		}
	}
	// deserialize gft_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		gft_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(gft_[i], 65535)) return false;
		}
	}
	// deserialize babycache_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		babycache_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(babycache_[i])) return false;
		}
	}
	// deserialize titles_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		titles_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(titles_[i])) return false;
		}
	}
	// deserialize guildContribution_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(guildContribution_)) return false;
		}
	}
	// deserialize exitGuildTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(exitGuildTime_)) return false;
		}
	}
	// deserialize guildSkills_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		guildSkills_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!guildSkills_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize gmActivities_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		gmActivities_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			EnumSize __e__;
			if(!__r__->readType(__e__) || __e__ >= 21) return false;
			gmActivities_[i] = (ADType)__e__;
		}
	}
	// deserialize festival_
	{
		if(__fm__.readBit()){
		if(!festival_.deserialize(__r__)) return false;
		}
	}
	// deserialize selfRecharge_
	{
		if(__fm__.readBit()){
		if(!selfRecharge_.deserialize(__r__)) return false;
		}
	}
	// deserialize sysRecharge_
	{
		if(__fm__.readBit()){
		if(!sysRecharge_.deserialize(__r__)) return false;
		}
	}
	// deserialize selfDiscountStore_
	{
		if(__fm__.readBit()){
		if(!selfDiscountStore_.deserialize(__r__)) return false;
		}
	}
	// deserialize sysDiscountStore_
	{
		if(__fm__.readBit()){
		if(!sysDiscountStore_.deserialize(__r__)) return false;
		}
	}
	// deserialize selfOnceRecharge_
	{
		if(__fm__.readBit()){
		if(!selfOnceRecharge_.deserialize(__r__)) return false;
		}
	}
	// deserialize sysOnceRecharge_
	{
		if(__fm__.readBit()){
		if(!sysOnceRecharge_.deserialize(__r__)) return false;
		}
	}
	// deserialize empact_
	{
		if(__fm__.readBit()){
		if(!empact_.deserialize(__r__)) return false;
		}
	}
	// deserialize selfCards_
	{
		if(__fm__.readBit()){
		if(!selfCards_.deserialize(__r__)) return false;
		}
	}
	// deserialize myselfRecharge_
	{
		if(__fm__.readBit()){
		if(!myselfRecharge_.deserialize(__r__)) return false;
		}
	}
	// deserialize hotdata_
	{
		if(__fm__.readBit()){
		if(!hotdata_.deserialize(__r__)) return false;
		}
	}
	// deserialize gbdata_
	{
		if(__fm__.readBit()){
		if(!gbdata_.deserialize(__r__)) return false;
		}
	}
	// deserialize sevenflag_
	{
		sevenflag_ = __fm__.readBit();
	}
	// deserialize signFlag_
	{
		signFlag_ = __fm__.readBit();
	}
	// deserialize sevendata_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		sevendata_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!sevendata_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize viprewardflag_
	{
		viprewardflag_ = __fm__.readBit();
	}
	// deserialize phoneNumber_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(phoneNumber_, 65535)) return false;
		}
	}
	// deserialize levelgift_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		levelgift_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(levelgift_[i])) return false;
		}
	}
	// deserialize activity_
	{
		if(__fm__.readBit()){
		if(!activity_.deserialize(__r__)) return false;
		}
	}
	// deserialize fuwen_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		fuwen_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!fuwen_[i].deserialize(__r__)) return false;
		}
	}
		return true;
}
void SGE_PlayerInst_Old::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	COM_Entity::serializeJson(ss,false);
	// serialize isLeavingTeam_
	ss << "\"isLeavingTeam_\":";
	{
		ss << (isLeavingTeam_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isBattle_
	ss << "\"isBattle_\":";
	{
		ss << (isBattle_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize autoBattle_
	ss << "\"autoBattle_\":";
	{
		ss << (autoBattle_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isTeamLeader_
	ss << "\"isTeamLeader_\":";
	{
		ss << (isTeamLeader_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize sceneId_
	ss << "\"sceneId_\":";
	{
		ss << (S64)sceneId_;
	}
	 ss << ",\n";
	// serialize openSubSystemFlag_
	ss << "\"openSubSystemFlag_\":";
	{
		ss << (S64)openSubSystemFlag_;
	}
	 ss << ",\n";
	// serialize createTime_
	ss << "\"createTime_\":";
	{
		ss << (S64)createTime_;
	}
	 ss << ",\n";
	// serialize guildName_
	ss << "\"guildName_\":";
	{
		ss << "\"" << guildName_ << "\"";
	}
	 ss << ",\n";
	// serialize scenePos_
	ss << "\"scenePos_\":";
	{
		scenePos_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize pvpInfo_
	ss << "\"pvpInfo_\":";
	{
		pvpInfo_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize onlineTimeFlag_
	ss << "\"onlineTimeFlag_\":";
	{
		ss << (onlineTimeFlag_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize onlineTime_
	ss << "\"onlineTime_\":";
	{
		ss << (double)onlineTime_;
	}
	 ss << ",\n";
	// serialize onlineTimeReward_
	ss << "\"onlineTimeReward_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)onlineTimeReward_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)onlineTimeReward_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize isFund_
	ss << "\"isFund_\":";
	{
		ss << (isFund_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize fundtags_
	ss << "\"fundtags_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)fundtags_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)fundtags_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize openDoubleTimeFlag_
	ss << "\"openDoubleTimeFlag_\":";
	{
		ss << (openDoubleTimeFlag_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isFirstLogin_
	ss << "\"isFirstLogin_\":";
	{
		ss << (isFirstLogin_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize firstRechargeDiamond_
	ss << "\"firstRechargeDiamond_\":";
	{
		ss << (firstRechargeDiamond_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize isFirstRechargeGift_
	ss << "\"isFirstRechargeGift_\":";
	{
		ss << (isFirstRechargeGift_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize offlineExp_
	ss << "\"offlineExp_\":";
	{
		ss << (double)offlineExp_;
	}
	 ss << ",\n";
	// serialize rivalTime_
	ss << "\"rivalTime_\":";
	{
		ss << (double)rivalTime_;
	}
	 ss << ",\n";
	// serialize rivalNum_
	ss << "\"rivalNum_\":";
	{
		ss << (S64)rivalNum_;
	}
	 ss << ",\n";
	// serialize promoteAward_
	ss << "\"promoteAward_\":";
	{
		ss << (S64)promoteAward_;
	}
	 ss << ",\n";
	// serialize guideIdx_
	ss << "\"guideIdx_\":";
	{
		ss << (S64)guideIdx_;
	}
	 ss << ",\n";
	// serialize noTalkTime_
	ss << "\"noTalkTime_\":";
	{
		ss << (double)noTalkTime_;
	}
	 ss << ",\n";
	// serialize wishShareNum_
	ss << "\"wishShareNum_\":";
	{
		ss << (S64)wishShareNum_;
	}
	 ss << ",\n";
	// serialize warriortrophyNum_
	ss << "\"warriortrophyNum_\":";
	{
		ss << (S64)warriortrophyNum_;
	}
	 ss << ",\n";
	// serialize employeelasttime_
	ss << "\"employeelasttime_\":";
	{
		ss << (S64)employeelasttime_;
	}
	 ss << ",\n";
	// serialize employeeonecount_
	ss << "\"employeeonecount_\":";
	{
		ss << (S64)employeeonecount_;
	}
	 ss << ",\n";
	// serialize employeetencount_
	ss << "\"employeetencount_\":";
	{
		ss << (S64)employeetencount_;
	}
	 ss << ",\n";
	// serialize greenBoxTimes_
	ss << "\"greenBoxTimes_\":";
	{
		ss << (double)greenBoxTimes_;
	}
	 ss << ",\n";
	// serialize blueBoxTimes_
	ss << "\"blueBoxTimes_\":";
	{
		ss << (double)blueBoxTimes_;
	}
	 ss << ",\n";
	// serialize greenBoxFreeNum_
	ss << "\"greenBoxFreeNum_\":";
	{
		ss << (S64)greenBoxFreeNum_;
	}
	 ss << ",\n";
	// serialize hbInfo_
	ss << "\"hbInfo_\":";
	{
		hbInfo_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize openScenes_
	ss << "\"openScenes_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)openScenes_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)openScenes_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize copyNum_
	ss << "\"copyNum_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)copyNum_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)copyNum_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize magicItemLevel_
	ss << "\"magicItemLevel_\":";
	{
		ss << (S64)magicItemLevel_;
	}
	 ss << ",\n";
	// serialize magicItemeExp_
	ss << "\"magicItemeExp_\":";
	{
		ss << (S64)magicItemeExp_;
	}
	 ss << ",\n";
	// serialize magicItemeJob_
	ss << "\"magicItemeJob_\":";
	{
		ss << "\"" << ENUM(JobType).getItemName(magicItemeJob_) << "\"";
	}
	 ss << ",\n";
	// serialize magicTupoLevel_
	ss << "\"magicTupoLevel_\":";
	{
		ss << (S64)magicTupoLevel_;
	}
	 ss << ",\n";
	// serialize cachedNpcs_
	ss << "\"cachedNpcs_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)cachedNpcs_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)cachedNpcs_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize gft_
	ss << "\"gft_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)gft_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << "\"" << gft_[i] << "\"";
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize babycache_
	ss << "\"babycache_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)babycache_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)babycache_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize titles_
	ss << "\"titles_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)titles_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)titles_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize guildContribution_
	ss << "\"guildContribution_\":";
	{
		ss << (S64)guildContribution_;
	}
	 ss << ",\n";
	// serialize exitGuildTime_
	ss << "\"exitGuildTime_\":";
	{
		ss << (S64)exitGuildTime_;
	}
	 ss << ",\n";
	// serialize guildSkills_
	ss << "\"guildSkills_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)guildSkills_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			guildSkills_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize gmActivities_
	ss << "\"gmActivities_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)gmActivities_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << "\"" << ENUM(ADType).getItemName(gmActivities_[i]) << "\"";
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize festival_
	ss << "\"festival_\":";
	{
		festival_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize selfRecharge_
	ss << "\"selfRecharge_\":";
	{
		selfRecharge_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize sysRecharge_
	ss << "\"sysRecharge_\":";
	{
		sysRecharge_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize selfDiscountStore_
	ss << "\"selfDiscountStore_\":";
	{
		selfDiscountStore_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize sysDiscountStore_
	ss << "\"sysDiscountStore_\":";
	{
		sysDiscountStore_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize selfOnceRecharge_
	ss << "\"selfOnceRecharge_\":";
	{
		selfOnceRecharge_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize sysOnceRecharge_
	ss << "\"sysOnceRecharge_\":";
	{
		sysOnceRecharge_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize empact_
	ss << "\"empact_\":";
	{
		empact_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize selfCards_
	ss << "\"selfCards_\":";
	{
		selfCards_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize myselfRecharge_
	ss << "\"myselfRecharge_\":";
	{
		myselfRecharge_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize hotdata_
	ss << "\"hotdata_\":";
	{
		hotdata_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize gbdata_
	ss << "\"gbdata_\":";
	{
		gbdata_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize sevenflag_
	ss << "\"sevenflag_\":";
	{
		ss << (sevenflag_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize signFlag_
	ss << "\"signFlag_\":";
	{
		ss << (signFlag_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize sevendata_
	ss << "\"sevendata_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)sevendata_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			sevendata_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize viprewardflag_
	ss << "\"viprewardflag_\":";
	{
		ss << (viprewardflag_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize phoneNumber_
	ss << "\"phoneNumber_\":";
	{
		ss << "\"" << phoneNumber_ << "\"";
	}
	 ss << ",\n";
	// serialize levelgift_
	ss << "\"levelgift_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)levelgift_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)levelgift_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize activity_
	ss << "\"activity_\":";
	{
		activity_.serializeJson(ss);
	}
	 ss << ",\n";
	// serialize fuwen_
	ss << "\"fuwen_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)fuwen_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			fuwen_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
//=============================================================
SGE_DBPlayerData_Old::SGE_DBPlayerData_Old():
freeze_(false)
,seal_(false)
,signs_(0)
,sellIdMax_(0)
,acceptRandQuestCounter_(0)
,submitRandQuestCounter_(0)
,loginTime_(0)
,logoutTime_(0)
,genItemMaxGuid_(0)
,gaterMaxNum_(0)
,firstRollEmployeeCon_(false)
,firstRollEmployeeDia_(false)
,empBattleGroup_((EmployeesBattleGroup)(0))
{}
void SGE_DBPlayerData_Old::serialize(ProtocolWriter* __s__) const
{
	SGE_PlayerInst_Old::serialize(__s__);
	//field mask
	FieldMask<4> __fm__;
	__fm__.writeBit(freeze_);
	__fm__.writeBit(seal_);
	__fm__.writeBit((signs_==0)?false:true);
	__fm__.writeBit((sellIdMax_==0)?false:true);
	__fm__.writeBit((acceptRandQuestCounter_==0)?false:true);
	__fm__.writeBit((submitRandQuestCounter_==0)?false:true);
	__fm__.writeBit(pfid_.length()?true:false);
	__fm__.writeBit(orders_.size()?true:false);
	__fm__.writeBit((loginTime_==0)?false:true);
	__fm__.writeBit((logoutTime_==0)?false:true);
	__fm__.writeBit((genItemMaxGuid_==0)?false:true);
	__fm__.writeBit((gaterMaxNum_==0)?false:true);
	__fm__.writeBit(firstRollEmployeeCon_);
	__fm__.writeBit(firstRollEmployeeDia_);
	__fm__.writeBit(employees_.size()?true:false);
	__fm__.writeBit(itemStorage_.size()?true:false);
	__fm__.writeBit(babyStorage_.size()?true:false);
	__fm__.writeBit(babies_.size()?true:false);
	__fm__.writeBit(bagItems_.size()?true:false);
	__fm__.writeBit(quests_.size()?true:false);
	__fm__.writeBit(completeQuests_.size()?true:false);
	__fm__.writeBit(mineReward_.size()?true:false);
	__fm__.writeBit(jjcBattleMsg_.size()?true:false);
	__fm__.writeBit(friend_.size()?true:false);
	__fm__.writeBit(blacklist_.size()?true:false);
	__fm__.writeBit(achValues_.size()?true:false);
	__fm__.writeBit(achievement_.size()?true:false);
	__fm__.writeBit((empBattleGroup_==(EmployeesBattleGroup)(0))?false:true);
	__fm__.writeBit(employeeGroup1_.size()?true:false);
	__fm__.writeBit(employeeGroup2_.size()?true:false);
	__fm__.writeBit(gatherData_.size()?true:false);
	__fm__.writeBit(compoundList_.size()?true:false);
	__s__->write(__fm__.masks_, 4);
	// serialize freeze_
	{
	}
	// serialize seal_
	{
	}
	// serialize signs_
	{
		if(signs_ != 0){
		__s__->writeType(signs_);
		}
	}
	// serialize sellIdMax_
	{
		if(sellIdMax_ != 0){
		__s__->writeType(sellIdMax_);
		}
	}
	// serialize acceptRandQuestCounter_
	{
		if(acceptRandQuestCounter_ != 0){
		__s__->writeType(acceptRandQuestCounter_);
		}
	}
	// serialize submitRandQuestCounter_
	{
		if(submitRandQuestCounter_ != 0){
		__s__->writeType(submitRandQuestCounter_);
		}
	}
	// serialize pfid_
	{
		if(pfid_.length()){
		__s__->writeType(pfid_);
		}
	}
	// serialize orders_
	if(orders_.size())
	{
		size_t __len__ = (size_t)orders_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			orders_[i].serialize(__s__);
		}
	}
	// serialize loginTime_
	{
		if(loginTime_ != 0){
		__s__->writeType(loginTime_);
		}
	}
	// serialize logoutTime_
	{
		if(logoutTime_ != 0){
		__s__->writeType(logoutTime_);
		}
	}
	// serialize genItemMaxGuid_
	{
		if(genItemMaxGuid_ != 0){
		__s__->writeType(genItemMaxGuid_);
		}
	}
	// serialize gaterMaxNum_
	{
		if(gaterMaxNum_ != 0){
		__s__->writeType(gaterMaxNum_);
		}
	}
	// serialize firstRollEmployeeCon_
	{
	}
	// serialize firstRollEmployeeDia_
	{
	}
	// serialize employees_
	if(employees_.size())
	{
		size_t __len__ = (size_t)employees_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			employees_[i].serialize(__s__);
		}
	}
	// serialize itemStorage_
	if(itemStorage_.size())
	{
		size_t __len__ = (size_t)itemStorage_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			itemStorage_[i].serialize(__s__);
		}
	}
	// serialize babyStorage_
	if(babyStorage_.size())
	{
		size_t __len__ = (size_t)babyStorage_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			babyStorage_[i].serialize(__s__);
		}
	}
	// serialize babies_
	if(babies_.size())
	{
		size_t __len__ = (size_t)babies_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			babies_[i].serialize(__s__);
		}
	}
	// serialize bagItems_
	if(bagItems_.size())
	{
		size_t __len__ = (size_t)bagItems_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			bagItems_[i].serialize(__s__);
		}
	}
	// serialize quests_
	if(quests_.size())
	{
		size_t __len__ = (size_t)quests_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			quests_[i].serialize(__s__);
		}
	}
	// serialize completeQuests_
	if(completeQuests_.size())
	{
		size_t __len__ = (size_t)completeQuests_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(completeQuests_[i]);
		}
	}
	// serialize mineReward_
	if(mineReward_.size())
	{
		size_t __len__ = (size_t)mineReward_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			mineReward_[i].serialize(__s__);
		}
	}
	// serialize jjcBattleMsg_
	if(jjcBattleMsg_.size())
	{
		size_t __len__ = (size_t)jjcBattleMsg_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			jjcBattleMsg_[i].serialize(__s__);
		}
	}
	// serialize friend_
	if(friend_.size())
	{
		size_t __len__ = (size_t)friend_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			friend_[i].serialize(__s__);
		}
	}
	// serialize blacklist_
	if(blacklist_.size())
	{
		size_t __len__ = (size_t)blacklist_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			blacklist_[i].serialize(__s__);
		}
	}
	// serialize achValues_
	if(achValues_.size())
	{
		size_t __len__ = (size_t)achValues_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(achValues_[i]);
		}
	}
	// serialize achievement_
	if(achievement_.size())
	{
		size_t __len__ = (size_t)achievement_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			achievement_[i].serialize(__s__);
		}
	}
	// serialize empBattleGroup_
	{
		EnumSize __e__ = (EnumSize)empBattleGroup_;
		if(__e__){
		__s__->writeType(__e__);
		}
	}
	// serialize employeeGroup1_
	if(employeeGroup1_.size())
	{
		size_t __len__ = (size_t)employeeGroup1_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(employeeGroup1_[i]);
		}
	}
	// serialize employeeGroup2_
	if(employeeGroup2_.size())
	{
		size_t __len__ = (size_t)employeeGroup2_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(employeeGroup2_[i]);
		}
	}
	// serialize gatherData_
	if(gatherData_.size())
	{
		size_t __len__ = (size_t)gatherData_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			gatherData_[i].serialize(__s__);
		}
	}
	// serialize compoundList_
	if(compoundList_.size())
	{
		size_t __len__ = (size_t)compoundList_.size();
		__s__->writeDynSize(__len__); 
		for(size_t i = 0; i < __len__; i++)
		{
			__s__->writeType(compoundList_[i]);
		}
	}
}
bool SGE_DBPlayerData_Old::deserialize(ProtocolReader* __r__)
{
	if(!SGE_PlayerInst_Old::deserialize(__r__)) return false;
	//field mask
	FieldMask<4> __fm__;
	if(!__r__->read(__fm__.masks_, 4)) return false;
	// deserialize freeze_
	{
		freeze_ = __fm__.readBit();
	}
	// deserialize seal_
	{
		seal_ = __fm__.readBit();
	}
	// deserialize signs_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(signs_)) return false;
		}
	}
	// deserialize sellIdMax_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(sellIdMax_)) return false;
		}
	}
	// deserialize acceptRandQuestCounter_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(acceptRandQuestCounter_)) return false;
		}
	}
	// deserialize submitRandQuestCounter_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(submitRandQuestCounter_)) return false;
		}
	}
	// deserialize pfid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(pfid_, 65535)) return false;
		}
	}
	// deserialize orders_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		orders_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!orders_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize loginTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(loginTime_)) return false;
		}
	}
	// deserialize logoutTime_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(logoutTime_)) return false;
		}
	}
	// deserialize genItemMaxGuid_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(genItemMaxGuid_)) return false;
		}
	}
	// deserialize gaterMaxNum_
	{
		if(__fm__.readBit()){
		if(!__r__->readType(gaterMaxNum_)) return false;
		}
	}
	// deserialize firstRollEmployeeCon_
	{
		firstRollEmployeeCon_ = __fm__.readBit();
	}
	// deserialize firstRollEmployeeDia_
	{
		firstRollEmployeeDia_ = __fm__.readBit();
	}
	// deserialize employees_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		employees_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!employees_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize itemStorage_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		itemStorage_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!itemStorage_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize babyStorage_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		babyStorage_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!babyStorage_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize babies_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		babies_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!babies_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize bagItems_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		bagItems_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!bagItems_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize quests_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		quests_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!quests_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize completeQuests_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		completeQuests_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(completeQuests_[i])) return false;
		}
	}
	// deserialize mineReward_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		mineReward_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!mineReward_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize jjcBattleMsg_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		jjcBattleMsg_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!jjcBattleMsg_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize friend_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		friend_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!friend_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize blacklist_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		blacklist_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!blacklist_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize achValues_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		achValues_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(achValues_[i])) return false;
		}
	}
	// deserialize achievement_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		achievement_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!achievement_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize empBattleGroup_
	{
		EnumSize __e__ = 0;
		if(__fm__.readBit()){
		if(!__r__->readType(__e__) || __e__ >= 5) return false;
		empBattleGroup_ = (EmployeesBattleGroup)__e__;
		}
	}
	// deserialize employeeGroup1_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		employeeGroup1_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(employeeGroup1_[i])) return false;
		}
	}
	// deserialize employeeGroup2_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		employeeGroup2_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(employeeGroup2_[i])) return false;
		}
	}
	// deserialize gatherData_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		gatherData_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!gatherData_[i].deserialize(__r__)) return false;
		}
	}
	// deserialize compoundList_
	if(__fm__.readBit())
	{
		size_t __len__;
		if(!__r__->readDynSize(__len__) || __len__ > 65535) return false;
		compoundList_.resize(__len__);
		for(size_t i = 0; i < __len__; i++)
		{
			if(!__r__->readType(compoundList_[i])) return false;
		}
	}
		return true;
}
void SGE_DBPlayerData_Old::serializeJson(std::ostream& ss, bool needBracket)const
{
	if(needBracket){ ss << "{"; }
	SGE_PlayerInst_Old::serializeJson(ss,false);
	// serialize freeze_
	ss << "\"freeze_\":";
	{
		ss << (freeze_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize seal_
	ss << "\"seal_\":";
	{
		ss << (seal_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize signs_
	ss << "\"signs_\":";
	{
		ss << (S64)signs_;
	}
	 ss << ",\n";
	// serialize sellIdMax_
	ss << "\"sellIdMax_\":";
	{
		ss << (S64)sellIdMax_;
	}
	 ss << ",\n";
	// serialize acceptRandQuestCounter_
	ss << "\"acceptRandQuestCounter_\":";
	{
		ss << (S64)acceptRandQuestCounter_;
	}
	 ss << ",\n";
	// serialize submitRandQuestCounter_
	ss << "\"submitRandQuestCounter_\":";
	{
		ss << (S64)submitRandQuestCounter_;
	}
	 ss << ",\n";
	// serialize pfid_
	ss << "\"pfid_\":";
	{
		ss << "\"" << pfid_ << "\"";
	}
	 ss << ",\n";
	// serialize orders_
	ss << "\"orders_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)orders_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			orders_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize loginTime_
	ss << "\"loginTime_\":";
	{
		ss << (S64)loginTime_;
	}
	 ss << ",\n";
	// serialize logoutTime_
	ss << "\"logoutTime_\":";
	{
		ss << (S64)logoutTime_;
	}
	 ss << ",\n";
	// serialize genItemMaxGuid_
	ss << "\"genItemMaxGuid_\":";
	{
		ss << (S64)genItemMaxGuid_;
	}
	 ss << ",\n";
	// serialize gaterMaxNum_
	ss << "\"gaterMaxNum_\":";
	{
		ss << (S64)gaterMaxNum_;
	}
	 ss << ",\n";
	// serialize firstRollEmployeeCon_
	ss << "\"firstRollEmployeeCon_\":";
	{
		ss << (firstRollEmployeeCon_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize firstRollEmployeeDia_
	ss << "\"firstRollEmployeeDia_\":";
	{
		ss << (firstRollEmployeeDia_ ? "true" : "false");
	}
	 ss << ",\n";
	// serialize employees_
	ss << "\"employees_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)employees_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			employees_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize itemStorage_
	ss << "\"itemStorage_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)itemStorage_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			itemStorage_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize babyStorage_
	ss << "\"babyStorage_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)babyStorage_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			babyStorage_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize babies_
	ss << "\"babies_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)babies_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			babies_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize bagItems_
	ss << "\"bagItems_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)bagItems_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			bagItems_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize quests_
	ss << "\"quests_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)quests_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			quests_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize completeQuests_
	ss << "\"completeQuests_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)completeQuests_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)completeQuests_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize mineReward_
	ss << "\"mineReward_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)mineReward_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			mineReward_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize jjcBattleMsg_
	ss << "\"jjcBattleMsg_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)jjcBattleMsg_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			jjcBattleMsg_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize friend_
	ss << "\"friend_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)friend_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			friend_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize blacklist_
	ss << "\"blacklist_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)blacklist_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			blacklist_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize achValues_
	ss << "\"achValues_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)achValues_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)achValues_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize achievement_
	ss << "\"achievement_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)achievement_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			achievement_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize empBattleGroup_
	ss << "\"empBattleGroup_\":";
	{
		ss << "\"" << ENUM(EmployeesBattleGroup).getItemName(empBattleGroup_) << "\"";
	}
	 ss << ",\n";
	// serialize employeeGroup1_
	ss << "\"employeeGroup1_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)employeeGroup1_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)employeeGroup1_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize employeeGroup2_
	ss << "\"employeeGroup2_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)employeeGroup2_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)employeeGroup2_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize gatherData_
	ss << "\"gatherData_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)gatherData_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			gatherData_[i].serializeJson(ss);
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	 ss << ",\n";
	// serialize compoundList_
	ss << "\"compoundList_\":";
	{
		ss << "[";
		size_t __len__ = (size_t)compoundList_.size();
		for(size_t i = 0; i < __len__; i++)
		{
			ss << (S64)compoundList_[i];
			ss <<(((i+1) == __len__) ? "":",")<<"";
		}
		ss << "]";
	}
	ss<<"\n";
	if(needBracket){ ss << "}"; }
}
