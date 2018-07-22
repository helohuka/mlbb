
#include "Activity.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "npctable.h"
#include "Scene.h"
#include "GameEvent.h"
#include "broadcaster.h"
#include "scenehandler.h"
#include "exam.h"
#include "dbhandler.h"
#include "worldserv.h"
#include "json/json.h"
std::map<U32,Activity::Record*> Activity::records_;
std::vector<std::pair<U32,U32> > Activity::rewards_;
bool Activity::load(const char* fn)
{
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		SRV_ASSERT(0);
	}
	records_.clear();
	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		Record* pRecord = NEW_MEM(Record);
		pRecord->actId_ = (ActivityType)ENUM(ActivityType).getItemId(csv.get_string(row,"ActivitiesKind"));
		pRecord->counter_ = csv.get_int(row,"maxtime");
		pRecord->reward_ = csv.get_int(row,"Active");
		records_[pRecord->actId_] = pRecord;
	}

	return true;
}

bool Activity::loadrewards(const char* fn)
{
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		SRV_ASSERT(0);
	}
	rewards_.clear();
	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		std::pair<U32,U32> pair;
		pair.first = csv.get_int(row,"itemID");
		pair.second = csv.get_int(row,"price");
		rewards_.push_back(pair);
	}
	return true;
}

bool Activity::isReward(COM_ActivityTable& table, U32 index){
	for (size_t i = 0; i < table.flag_.size(); ++i)
	{
		if(table.flag_[i] == index)
			return false;
	}
	return true;
}

void Player::requestActivityReward(U32 index)
{

	for(size_t i=0; i<Activity::rewards_.size(); ++i)
	{
		if(i == index)
		{
			if(Activity::rewards_[i].second > activity_.reward_)
				return ;
			if(!Activity::isReward(activity_,index))
				return;
			if(calcEmptyItemNum(Activity::rewards_[i].first) < 1){
				errorMessageToC(EN_BagFull);
				return;
			}
			activity_.flag_.push_back(i);
			addBagItemByItemId(Activity::rewards_[i].first,1,false,16);
			CALL_CLIENT(this,requestActivityRewardOK(i));
		}
	}
}

void Player::cleanActivation(ActivityType type){
	if(type == ACT_None)
	{
		for (size_t i=0 ;i< activity_.activities_.size(); ++i)
		{
			activity_.activities_[i].counter_ = 0;
			CALL_CLIENT(this,updateActivityCounter(type,0,0));
		}
	}
	else 
	{
		for (size_t i=0 ;i< activity_.activities_.size(); ++i)
		{
			if (activity_.activities_[i].actId_ == type)
			{
				activity_.activities_[i].counter_ = 0;
				CALL_CLIENT(this,updateActivityCounter(type,0,activity_.reward_));
				break;
			}
		}
		
	}
}

int Player::getActivitionCount(ActivityType type){
	for (size_t i=0 ;i< activity_.activities_.size(); ++i)
	{
		if(activity_.activities_[i].actId_ == type)
		{
			return activity_.activities_[i].counter_;
		}
	}
	return 0;
}

void Player::resetActivation(){
	activity_.activities_.clear();
	activity_.flag_.clear();
	activity_.reward_ = 0;
	for(size_t i=0; i<Activity::records_.size(); ++i)
	{
		if(!Activity::records_[i])
			continue;
		COM_Activity act;
		act.actId_ = Activity::records_[i]->actId_;
		act.counter_ = 0;

		activity_.activities_.push_back(act);
	}
	CALL_CLIENT(this,syncActivity(activity_));
}

void Player::addActivation(ActivityType actId, U32 counter)
{	
	const Activity::Record* pRecord = Activity::getReward(actId);
	
	if(NULL == pRecord){
		return;
	}

	for (size_t i=0 ;i< activity_.activities_.size(); ++i)
	{
		if(activity_.activities_[i].actId_ == actId)
		{
			if(activity_.activities_[i].counter_ < pRecord->counter_){
				activity_.activities_[i].counter_+= counter;
				activity_.reward_ += (pRecord->reward_/pRecord->counter_);
			}
			CALL_CLIENT(this,updateActivityCounter(actId,activity_.activities_[i].counter_,activity_.reward_));
			return;
		}
	}
	
	COM_Activity act;
	act.actId_ = actId;
	act.counter_ = counter;
	activity_.activities_.push_back(act);
	
}

void Activity::sycnactivityfromdb(U32 playerGuid, COM_ActivityTable& data){
	//tables_[playerGuid] = data;
}

void Activity::sycnallActData(SGE_SysActivity data){
	//int64 curtime = WorldServ::instance()->curTime_;
	Festival::data_ = data.loginData_;
	Festival::isOpen_ = data.loginData_.sinceStamp_ < data.loginData_.endStamp_ ? true : false;
	//ACE_DEBUG((LM_ERROR,ACE_TEXT("sycnallActData Festival startTime=[%d]========endTime=[%d]\n"),data.loginData_.sinceStamp_,data.loginData_.endStamp_));
	if(Festival::isOpen_){
		GameTimer* p = NEW_MEM(FestivalTimer,data.loginData_.sinceStamp_,data.loginData_.endStamp_);
		GameTimer::pushtimer(p);
	}
		
	RechargeTotal::data_ = data.chData_;
	RechargeTotal::isOpen_ = data.chData_.sinceStamp_ < data.chData_.endStamp_ ? true : false;
	//ACE_DEBUG((LM_ERROR,ACE_TEXT("sycnallActData RechargeTotal startTime=[%d]========endTime=[%d]\n"),data.chData_.sinceStamp_,data.chData_.endStamp_));
	if(RechargeTotal::isOpen_){
		GameTimer* p = NEW_MEM(RechargeTotalTimer,data.chData_.sinceStamp_,data.chData_.endStamp_);
		GameTimer::pushtimer(p);
	}
		
	RechargeSingle::data_ = data.ceData_;
	RechargeSingle::isOpen_ = data.ceData_.sinceStamp_ < data.ceData_.endStamp_ ? true : false;
	//ACE_DEBUG((LM_ERROR,ACE_TEXT("sycnallActData RechargeSingle startTime=[%d]========endTime=[%d]\n"),data.ceData_.sinceStamp_,data.ceData_.endStamp_));
	if(RechargeSingle::isOpen_){
		GameTimer* p = NEW_MEM(RechargeSingleTimer,data.ceData_.sinceStamp_,data.ceData_.endStamp_);
		GameTimer::pushtimer(p);
	}
		
	DiscountStore::data_ = data.stData_;
	DiscountStore::isOpen_ = data.stData_.sinceStamp_ < data.stData_.endStamp_ ? true : false;
	//ACE_DEBUG((LM_ERROR,ACE_TEXT("sycnallActData DiscountStore startTime=[%d]========endTime=[%d]\n"),data.stData_.sinceStamp_,data.stData_.endStamp_));
	if(DiscountStore::isOpen_){
		GameTimer* p = NEW_MEM(DiscountStoreTimer,data.stData_.sinceStamp_,data.stData_.endStamp_);
		GameTimer::pushtimer(p);
	}
		
	HotShop::data_ = data.hrData_;
	HotShop::isOpen_ = data.hrData_.sinceStamp_ < data.hrData_.endStamp_ ? true : false;
	//ACE_DEBUG((LM_ERROR,ACE_TEXT("sycnallActData HotShop startTime=[%d]========endTime=[%d]\n"),data.hrData_.sinceStamp_,data.hrData_.endStamp_));
	if(HotShop::isOpen_){
		GameTimer*p = NEW_MEM(HotShopTimer,data.hrData_.sinceStamp_,data.hrData_.endStamp_);
		GameTimer::pushtimer(p);
	}
		
	EmployeeActivityTotal::data_ = data.etdata_;
	EmployeeActivityTotal::isOpen_ = data.etdata_.sinceStamp_ < data.etdata_.endStamp_ ? true : false;
	//ACE_DEBUG((LM_ERROR,ACE_TEXT("sycnallActData EmployeeActivityTotal startTime=[%d]========endTime=[%d]\n"),data.etdata_.sinceStamp_,data.etdata_.endStamp_));
	if(EmployeeActivityTotal::isOpen_){
		GameTimer*p = NEW_MEM(EmployeeActivityTotalTimer,data.etdata_.sinceStamp_,data.etdata_.endStamp_);
		GameTimer::pushtimer(p);
	}

	MinGift::data_ = data.gbdata_;
	MinGift::isOpen_ = data.gbdata_.sinceStamp_ < data.gbdata_.endStamp_ ? true : false;
	//ACE_DEBUG((LM_ERROR,ACE_TEXT("sycnallActData EmployeeActivityTotal startTime=[%d]========endTime=[%d]\n"),data.etdata_.sinceStamp_,data.etdata_.endStamp_));
	if(MinGift::isOpen_){
		GameTimer*p = NEW_MEM(MinGiftTimer,data.gbdata_.sinceStamp_,data.gbdata_.endStamp_);
		GameTimer::pushtimer(p);
	}

	Zhuanpan::data_ = data.zpdata_;
	Zhuanpan::isopen_ = data.zpdata_.sinceStamp_ < data.zpdata_.endStamp_ ? true : false;
	//ACE_DEBUG((LM_ERROR,ACE_TEXT("sycnallActData EmployeeActivityTotal startTime=[%d]========endTime=[%d]\n"),data.etdata_.sinceStamp_,data.etdata_.endStamp_));
	if(Zhuanpan::isopen_){
		GameTimer*p = NEW_MEM(ZhuanpanTimer,data.zpdata_.sinceStamp_,data.zpdata_.endStamp_);
		GameTimer::pushtimer(p);
	}


	IntegralShop::data_ = data.icdata_;
	IntegralShop::isOpen_ = data.icdata_.sinceStamp_ < data.icdata_.endStamp_ ? true : false;
	//ACE_DEBUG((LM_ERROR,ACE_TEXT("sycnallActData EmployeeActivityTotal startTime=[%d]========endTime=[%d]\n"),data.etdata_.sinceStamp_,data.etdata_.endStamp_));
	if(IntegralShop::isOpen_){
		GameTimer*p = NEW_MEM(ZhuanpanTimer,data.icdata_.sinceStamp_,data.icdata_.endStamp_);
		GameTimer::pushtimer(p);
	}
}

bool DayliActivity::status_[ACT_Max] = {false};

void DayliActivity::mushroomOpen(S32 totalSize){
	SceneHandler::instance()->initDynamicNpcs(NT_Mogu,totalSize);
	WorldBroadcaster::instance()->updateActivityStatus(ACT_Mogu,true); //通知世界开启蘑菇
	status_[ACT_Mogu] = true;
}


void DayliActivity::mushroomRefresh(S32 totalSize){
	SceneHandler::instance()->refreshDynamicNpcs(NT_Mogu,totalSize);
}

void DayliActivity::mushroomClose(){
	SceneHandler::instance()->finiDynamicNpcs(NT_Mogu);
	WorldBroadcaster::instance()->updateActivityStatus(ACT_Mogu,false); //通知世界关闭蘑菇
	status_[ACT_Mogu] = false;
}


void DayliActivity::xijiOpen(S32 totalSize){
	SceneHandler::instance()->initDynamicNpcs(NT_Xiji,totalSize);
	//WorldBroadcaster::instance()->updateActivityStatus(ACT_Xiji,true); //通知世界开启蘑菇
	//status_[ACT_Xiji] = true;
}

void DayliActivity::xijiRefresh(S32 totalSize){
	SceneHandler::instance()->refreshDynamicNpcs(NT_Xiji,totalSize);
}

void DayliActivity::xijiClose(){
	SceneHandler::instance()->finiDynamicNpcs(NT_Xiji);
	//WorldBroadcaster::instance()->updateActivityStatus(ACT_Xiji,false); //通知世界关闭蘑菇
	//status_[ACT_Xiji] = false;
}

void DayliActivity::alonepkOpen(S32 totalSize){
	SceneHandler::instance()->initDynamicNpcs(NT_SinglePK,totalSize);
	WorldBroadcaster::instance()->updateActivityStatus(ACT_AlonePK,true); //通知世界开启蘑菇
	status_[ACT_AlonePK] = true;
}

void DayliActivity::alonepkRefresh(S32 totalSize){
	SceneHandler::instance()->refreshDynamicNpcs(NT_SinglePK,totalSize);
}

void DayliActivity::alonepkClose(){
	SceneHandler::instance()->finiDynamicNpcs(NT_SinglePK);
	WorldBroadcaster::instance()->updateActivityStatus(ACT_AlonePK,false); //通知世界关闭蘑菇
	status_[ACT_AlonePK] = false;
}

void DayliActivity::teampkOpen(S32 totalSize){
	SceneHandler::instance()->initDynamicNpcs(NT_TeamPK,totalSize);
	WorldBroadcaster::instance()->updateActivityStatus(ACT_TeamPK,true); //通知世界开启PK
	status_[ACT_TeamPK] = true;
}

void DayliActivity::teampkRefresh(S32 totalSize){
	SceneHandler::instance()->refreshDynamicNpcs(NT_TeamPK,totalSize);
}

void DayliActivity::teampkClose(){
	SceneHandler::instance()->finiDynamicNpcs(NT_TeamPK);
	WorldBroadcaster::instance()->updateActivityStatus(ACT_TeamPK,false); //通知世界关闭PK
	status_[ACT_TeamPK] = false;
}

void DayliActivity::examOpen()
{
	ExamTable::openExam();
	WorldBroadcaster::instance()->updateActivityStatus(ACT_Exam,true); //通知世界开启答题
	status_[ACT_Exam] = true;
}

void DayliActivity::examClose()
{
	ExamTable::closeExam();
	WorldBroadcaster::instance()->updateActivityStatus(ACT_Exam,false); //通知世界开启答题
	status_[ACT_Exam] = false;
}

void DayliActivity::warriorOpen()
{
	WorldBroadcaster::instance()->updateActivityStatus(ACT_Warrior,true); //通知世界开启勇者选拔
	status_[ACT_Warrior] = true;
}

void DayliActivity::warriorClose()
{
	WorldBroadcaster::instance()->updateActivityStatus(ACT_Warrior,false); //通知世界开启勇者选拔
	status_[ACT_Warrior] = false;
}

void DayliActivity::petbattleopen(){
	WorldBroadcaster::instance()->updateActivityStatus(ACT_Pet,true); //通知世界开启宠物神殿
	status_[ACT_Pet] = true;
}

void DayliActivity::petbattleclose(){
	WorldBroadcaster::instance()->updateActivityStatus(ACT_Pet,false); //通知世界关闭宠物神殿
	status_[ACT_Pet] = false;
}

void DayliActivity::reqeust(Player* p){
	for(S32 i=ACT_None;i<ACT_Max; ++i){
		if(status_[i]){
			CALL_CLIENT(p,updateActivityStatus((ActivityType)i,status_[i]));
			if(i == ACT_Exam && status_[i])
				ExamTable::addExam(p);
		}
	}
	//SceneHandler::instance()->initDynamicNpcs(NT_Mogu,15);
}

void DayliActivity::guildbattleopen(){
	WorldBroadcaster::instance()->updateActivityStatus(ACT_Family_0,true); //通知世界关闭宠物神殿
	status_[ACT_Family_0] = true;
}

void DayliActivity::guildbattleclose(){
	WorldBroadcaster::instance()->updateActivityStatus(ACT_Family_0,false); //通知世界关闭宠物神殿
	status_[ACT_Family_0] = false;
}

// --------------------------------- [3/31/2016 lwh] --------------------------------------
bool ZhuanpanTimer::start(){
	Zhuanpan::open();
	return false;
}

bool ZhuanpanTimer::stop(){
	Zhuanpan::close();
	return false;
}


bool Zhuanpan::isopen_ = false;
COM_ZhuanpanData Zhuanpan::data_;
std::vector< std::pair<std::string, U32> > Zhuanpan::record_;
std::vector< U32 > Zhuanpan::pond_;

bool
Zhuanpan::load(char const *fn)
{
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);

	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		COM_ZhuanpanContent content ;
		S32 id = csv.get_int(row,"ID");
		content.id_			= id;
		content.item_		= csv.get_int(row,"Item");
		content.itemNum_	= csv.get_int(row,"Number");
		content.rate_		= csv.get_int(row,"Probability");
		content.maxdrop_	= csv.get_int(row,"DailyOutput");
		data_.contents_.push_back(content);
	}

	return true;
}

COM_ZhuanpanContent const *
Zhuanpan::getzhuanpanDataById(U32 id)
{
	for (size_t i = 0; i < data_.contents_.size(); ++i)
	{
		if(data_.contents_[i].id_ == id)
			return &data_.contents_[i];
	}
	return NULL;
}


bool
Zhuanpan::check()
{
	for (size_t i = 0; i < data_.contents_.size(); ++i)
	{
		ItemTable::ItemData const* pData = ItemTable::getItemById(data_.contents_[i].item_);
		if(pData == NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("zhuanpan Don't find this itemid[%d] in the item table\n"),data_.contents_[i].item_));
			return false;
		}
	}
	return true;
}

//-----
void
Zhuanpan::initZhuanpan()
{
	if(!isopen_)
	{
		pond_.clear();
		record_.clear();
		data_ = COM_ZhuanpanData();
	}
}

void
Zhuanpan::passZeroHour(){
	if(isopen_)
		record_.clear();
}

void
Zhuanpan::calcPond()
{
	for (size_t i = 0; i < data_.contents_.size(); ++i)
	{
		if(checkpond(data_.contents_[i].id_))
			pond_.push_back(data_.contents_[i].id_);
	}
}

void
Zhuanpan::randZhuanpan(std::string playerName, U32 counter)
{
	Player* player = Player::getPlayerByName(playerName);
	if (player == NULL)
		return;
	if(!isopen_){
		CALL_CLIENT(player,errorno(EN_ActivityNoTime));
		return;
	}
	if(counter > player->getBagEmptySlot()){
		CALL_CLIENT(player,errorno(EN_BagFull));
		return;
	}

	if(counter == 1)
	{
		int32 need = Global::get<int>(C_ZhuanPanOneGo);
		if(player->getProp(PT_MagicCurrency) < need)
			return;
		player->addMagicCurrency(-need);
	}
	else
	{
		int32 need = Global::get<int>(C_ZhuanPanTenGo);
		if(player->getProp(PT_MagicCurrency) < need)
			return;
		player->addMagicCurrency(-need);
	}

	std::vector<U32> ids;
	for (size_t i = 0; i < counter; ++i)
	{
		pond_.clear();
		calcPond();
		std::vector<std::pair<int ,int> > tmp;
		for (size_t i = 0;i < pond_.size(); ++i)
		{
			const COM_ZhuanpanContent* pCore = Zhuanpan::getzhuanpanDataById(pond_[i]);
			if(pCore == NULL)
			{
				SRV_ASSERT(pCore);
			}
			std::pair<int ,int> pool;
			pool = std::make_pair(pCore->id_,pCore->rate_);
			tmp.push_back(pool);
		}
		U32 index = UtlMath::randWeight(tmp);

		const COM_ZhuanpanContent* pCore = Zhuanpan::getzhuanpanDataById(index);
		if(pCore == NULL)
			continue;

		if(israrity(index))
		{
			GEParam param[2];
			param[0].type_ = GEP_INT;
			param[0].value_.i = player->getGUID();
			param[1].type_ = GEP_INT;
			param[1].value_.i = index;
			GameEvent::procGameEvent(GET_Zhuanpan,param,2,player->getHandleId());

			saveRarityRecord(playerName,index);
			
			COM_Zhuanpan zhuanp;
			zhuanp.playerName_ = playerName;
			zhuanp.zhuanpanId_ = index;
			WorldBroadcaster::instance()->updateZhuanpanNotice(zhuanp);
		}

		player->addBagItemByItemId(pCore->item_,pCore->itemNum_,false,984);

		std::pair<std::string, U32> tmpp;
		tmpp = std::make_pair(player->getNameC(),index);
		record_.push_back(tmpp);

		ids.push_back(index);
	}
	CALL_CLIENT(player,zhuanpanOK(ids));
}

bool
Zhuanpan::checkpond(U32 zhuanpanId)
{
	const COM_ZhuanpanContent* pcore = Zhuanpan::getzhuanpanDataById(zhuanpanId);
	if(pcore == NULL)
		return false;
	if(pcore->maxdrop_ == 0)
		return true;
	else
	{
		U32 index = 0;
		std::vector< std::pair<std::string,U32> >::iterator itr = record_.begin();
		for (;itr != record_.end(); ++itr)
		{
			if(itr->second == zhuanpanId)
				++index;
		}
		if(index < pcore->maxdrop_)
			return true;
	}
	return false;
}

bool
Zhuanpan::israrity(U32 zhuanpanId)
{
	const COM_ZhuanpanContent* pcore = Zhuanpan::getzhuanpanDataById(zhuanpanId);
	if(pcore == NULL)
		return false;
	if(pcore->maxdrop_ == 0)
		return false;
	return true;
}

void
Zhuanpan::saveRarityRecord(std::string playerName, U32 zhuanpanId)
{
	if(data_.rarity_.size() >= 50)
		data_.rarity_.erase(data_.rarity_.begin());
	COM_Zhuanpan zhuanp;
	zhuanp.playerName_ = playerName;
	zhuanp.zhuanpanId_ = zhuanpanId;
	data_.rarity_.push_back(zhuanp);
}

void Zhuanpan::open(int64 start, int64 stop){
	SRV_ASSERT(start < stop);
	data_.sinceStamp_ = start;
	data_.endStamp_ = stop;
	ZhuanpanTimer *timerT = NEW_MEM(ZhuanpanTimer,start,stop);
	GameTimer::pushtimer(timerT);
	//ACE_DEBUG((LM_INFO,ACE_TEXT("Festival::open startTime=[%d]========endTime=[%d]\n"),data_.sinceStamp_,data_.endStamp_));
	SGE_SysActivity sys;
	sys.zpdata_ = data_;
	DBHandler::instance()->insertActivity(ADT_Zhuanpan,sys);
}

void Zhuanpan::open(){
	if(isopen_)
		return;
	isopen_ = true;
}

void Zhuanpan::close(){
	isopen_ = false;
	initZhuanpan();
	for (size_t i=0;i<Player::store_.size();++i)
	{
		CALL_CLIENT(Player::store_[i],agencyActivity(ADT_Zhuanpan,isopen_));
	}
}

void Zhuanpan::request(Player* p){
	if(isopen_ && p != NULL){
		CALL_CLIENT(p,sycnZhuanpanData(data_));
	}
}

void Zhuanpan::update(COM_ZhuanpanData& data){
	open(data.sinceStamp_,data.endStamp_);
	/*if(!isOpen_)
		return;*/
	data_ = data;
	SGE_SysActivity sys;
	sys.zpdata_ = data_;
	DBHandler::instance()->insertActivity(ADT_Zhuanpan,sys);
}
//------------------------------------------------------翻牌-----------------------------------------------------------
bool ReversalCardTimer::start(){
	ReversalCard::open();
	return false;
}

bool ReversalCardTimer::stop(){
	ReversalCard::close();
	return false;
}

std::vector<ReversalCard::Cardsper*> ReversalCard::sper_;
std::vector<ReversalCard::CardReward*> ReversalCard::high_;
std::vector<ReversalCard::CardReward*> ReversalCard::middle_;
std::vector<ReversalCard::CardReward*> ReversalCard::low_;
bool ReversalCard::isOpen_ = false;
uint64 ReversalCard::sinceStamp_ = 0;
uint64 ReversalCard::endStamp_ = 0;

bool
ReversalCard::loadcardsper(const char* fn){
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);

	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		Cardsper* co = NEW_MEM(Cardsper);
		co->time_ = csv.get_int(row,"time");
		co->high_ = csv.get_int(row,"high");
		co->middle_ = csv.get_int(row,"middle");
		co->low_ = csv.get_int(row,"low");
		co->cost_ = csv.get_int(row,"cost");

		sper_.push_back(co);
	}
	return true;
}

bool
ReversalCard::loadcardreward(const char* fn){
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);

	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		CardReward* co = NEW_MEM(CardReward);
		co->id_ = csv.get_int(row,"id");
		co->itemid_ = csv.get_int(row,"reward");
		co->itemnum_ = csv.get_int(row,"num");
		co->type_ = csv.get_int(row,"type");

		if(co->type_ == 1){
			low_.push_back(co);
		}else if(co->type_ == 2){
			middle_.push_back(co);
		}else if(co->type_ == 3){
			high_.push_back(co);
		}else{
			SRV_ASSERT(0);
		}
	}
	return true;
}

bool ReversalCard::check(){
	for (size_t i = 0; i < high_.size(); ++i)
	{
		SRV_ASSERT(ItemTable::getItemById(high_[i]->itemid_));
	}
	return true;
}

ReversalCard::Cardsper const *
ReversalCard::getcardsperbytime(U32 utime){
	for (size_t i=0; i<sper_.size();++i)
	{
		if(sper_[i]->time_ == utime)
			return sper_[i];
	}
	return NULL;
}

ReversalCard::CardReward const *
ReversalCard::getrewardhigh(Player* p){
	if(p == NULL)
		return NULL;
	std::vector<CardReward*> tmp;
	for (size_t i = 0; i < high_.size(); ++i)
	{
		if(p->hasCardReward(high_[i]->id_))
			continue;
		tmp.push_back(high_[i]);
	}
	U32 index = UtlMath::randN(tmp.size());
	return tmp[index];
}

ReversalCard::CardReward const *
ReversalCard::getrewardmiddle(Player* p){
	if(p == NULL)
		return NULL;
	std::vector<CardReward*> tmp;
	for (size_t i = 0; i < middle_.size(); ++i)
	{
		if(p->hasCardReward(middle_[i]->id_))
			continue;
		tmp.push_back(middle_[i]);
	}
	U32 index = UtlMath::randN(tmp.size());
	return tmp[index];
}

ReversalCard::CardReward const *
ReversalCard::getrewardlow(Player* p){
	if(p == NULL)
		return NULL;
	std::vector<CardReward*> tmp;
	for (size_t i = 0; i < low_.size(); ++i)
	{
		if(p->hasCardReward(low_[i]->id_))
			continue;
		tmp.push_back(low_[i]);
	}
	U32 index = UtlMath::randN(tmp.size());
	return tmp[index];
}

void ReversalCard::open(int64 start, int64 stop){
	sinceStamp_ = start;
	endStamp_ = stop;
	SRV_ASSERT(start < stop);
	ReversalCardTimer *openT = NEW_MEM(ReversalCardTimer,start,stop);
	GameTimer::pushtimer(openT);
}

void ReversalCard::open(){
	isOpen_ = true;
}

void ReversalCard::close(){
	isOpen_ = false;
}

void ReversalCard::request(Player* p){
	if(p == NULL)
		return;
	if(!isOpen_){
		p->selfCards_ = COM_ADCards();
	}
	else
	{
		p->selfCards_.sinceStamp_ = sinceStamp_;
		p->selfCards_.endStamp_ = endStamp_;
	}
}

//------------------------------------------------------------------------------------------------------------------

bool FestivalTimer::start(){
	Festival::open();
	return false;
}

bool FestivalTimer::stop(){
	Festival::close();
	return false;
}

bool Festival::load(const char* fn){
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
		SRV_ASSERT(0);

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);

	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		COM_ADLoginTotalContent item;
		item.totalDays_ = csv.get_int(row,"day");
		{
			std::string strItems = csv.get_string(row,"reward");
			std::vector<std::string> arrItems = String::Split(strItems,";");
			for(size_t i=0; i<arrItems.size(); ++i){
				item.itemIds_.push_back(atoi(arrItems[i].c_str()));
			}
		}
		{
			std::string str = csv.get_string(row,"renum");
			std::vector<std::string> arr = String::Split(str,";");
			for(size_t i=0; i<arr.size(); ++i){
				item.itemStacks_.push_back(atoi(arr[i].c_str()));
			}
		}
		SRV_ASSERT(item.itemIds_.size() == item.itemStacks_.size());
		item.status_ = 0;
		data_.contents_.push_back(item);
	}
	return true;
}

bool Festival::check(){
	for(size_t i=0; i<data_.contents_.size(); ++i){
		for(size_t j=0; j<data_.contents_[i].itemIds_.size(); ++j){
			SRV_ASSERT(ItemTable::getItemById(data_.contents_[i].itemIds_[j]));
		}
		for(size_t j=0; j<data_.contents_[i].itemStacks_.size(); ++j){
			SRV_ASSERT(data_.contents_[i].itemStacks_[j]);
		}
	}
	return true;
}

void Festival::open(int64 start, int64 stop){
	SRV_ASSERT(start < stop);
	data_.sinceStamp_ = start;
	data_.endStamp_ = stop;
	FestivalTimer *timerT = NEW_MEM(FestivalTimer,start,stop);
	GameTimer::pushtimer(timerT);
	//ACE_DEBUG((LM_ERROR,ACE_TEXT("Festival::open startTime=[%d]========endTime=[%d]\n"),data_.sinceStamp_,data_.endStamp_));
	SGE_SysActivity sys;
	sys.loginData_ = data_;
	DBHandler::instance()->insertActivity(ADT_LoginTotal,sys);
}

void Festival::open(){
	if(isOpen_)
		return;
	isOpen_ = true;
}

void Festival::close(){
	isOpen_ = false;
}

void Festival::request(Player* p){
	if(!isOpen_)
	{
		p->festival_ = COM_ADLoginTotal();
		return ;
	}
	if ( p->festival_.sinceStamp_ != data_.sinceStamp_ || p->festival_.endStamp_ != data_.endStamp_){
		p->festival_.contents_.clear();
	}
	if(p->festival_.contents_.empty()){
		p->festival_ = data_;
		p->updateFestival();
	}
}

void Festival::update(COM_ADLoginTotal& date){
	open(date.sinceStamp_,date.endStamp_);
	/*if(!isOpen_)
		return;*/
	data_ = date;
	SGE_SysActivity sys;
	sys.loginData_ = data_;
	DBHandler::instance()->insertActivity(ADT_LoginTotal,sys);
}

bool Festival::isOpen_ = false;
COM_ADLoginTotal Festival::data_;

bool RechargeTotalTimer::start(){
	RechargeTotal::open();
	return false;
}

bool RechargeTotalTimer::stop(){
	RechargeTotal::close();
	return false;
}

bool RechargeTotal::load(const char* fn){
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);
	
	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		COM_ADChargeTotalContent item;
		item.currencyCount_ = csv.get_int(row,"num");
		{
			std::string strItems = csv.get_string(row,"reward");
			std::vector<std::string> arrItems = String::Split(strItems,";");
			for(size_t i=0; i<arrItems.size(); ++i){
				item.itemIds_.push_back(atoi(arrItems[i].c_str()));
			}
		}
		{
			std::string str = csv.get_string(row,"renum");
			std::vector<std::string> arr = String::Split(str,";");
			for(size_t i=0; i<arr.size(); ++i){
				item.itemStacks_.push_back(atoi(arr[i].c_str()));
			}
		}
		SRV_ASSERT(item.itemIds_.size() == item.itemStacks_.size());
		item.status_ = 0;
		data_.contents_.push_back(item);
	}
	return true;
}
bool RechargeTotal::check(){
	for(size_t i=0; i<data_.contents_.size(); ++i){
		for(size_t j=0; j<data_.contents_[i].itemIds_.size(); ++j){
			SRV_ASSERT(ItemTable::getItemById(data_.contents_[i].itemIds_[j]));
		}
		for(size_t j=0; j<data_.contents_[i].itemStacks_.size(); ++j){
			SRV_ASSERT(data_.contents_[i].itemStacks_[j]);
		}
	}
	return true;
}
void RechargeTotal::open(int64 start1, int64 stop1){
	data_.sinceStamp_ = start1;
	data_.endStamp_ = stop1;
	SRV_ASSERT(start1 < stop1);
	RechargeTotalTimer *openT = NEW_MEM(RechargeTotalTimer,start1,stop1);
	GameTimer::pushtimer(openT);
	//ACE_DEBUG((LM_ERROR,ACE_TEXT("RechargeTotal::open startTime=[%d]========endTime=[%d]\n"),data_.sinceStamp_,data_.endStamp_));
	SGE_SysActivity sys;
	sys.chData_ = data_;
	DBHandler::instance()->insertActivity(ADT_ChargeTotal,sys);
}
void RechargeTotal::open(){
	isOpen_ = true;
}
void RechargeTotal::close(){
	isOpen_ = false;
}
void RechargeTotal::request(Player* p){
	if(!isOpen_){
		p->sysRecharge_ = COM_ADChargeTotal();
		return;
	}
	if ( p->sysRecharge_.sinceStamp_ != data_.sinceStamp_ || p->sysRecharge_.endStamp_ != data_.endStamp_){
		p->sysRecharge_.contents_.clear();
	}
	if(p->sysRecharge_.contents_.empty()){
		p->sysRecharge_ = data_;
		p->updateSysRecharge();
	}
}
void RechargeTotal::requestSelf(Player* p){
	if(!isOpen_){
		p->selfRecharge_ = COM_ADChargeTotal();
		return;
	}
	if(p->selfRecharge_.contents_.empty()){
		p->selfRecharge_ = data_;
		p->updateSelfRecharge();
	}
}

void RechargeTotal::update(COM_ADChargeTotal& date){
	open(date.sinceStamp_,date.endStamp_);
	data_ = date;
	SGE_SysActivity sys;
	sys.chData_ = data_;
	DBHandler::instance()->insertActivity(ADT_ChargeTotal,sys);
}

bool RechargeTotal::isOpen_ = false;
COM_ADChargeTotal RechargeTotal::data_;


//////////////////////////////////////////////////////////////////////////

bool DiscountStoreTimer::start(){
	DiscountStore::open();
	return false;
}

bool DiscountStoreTimer::stop(){
	DiscountStore::close();
	return false;
}

bool DiscountStore::load(const char* fn){
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
		SRV_ASSERT(0);

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);

	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		COM_ADDiscountStoreContent item;
		item.price_ = csv.get_int(row,"price");
		item.itemId_ = csv.get_int(row,"ID");
		item.discount_ = csv.get_float(row,"sale");
		item.buyLimit_ = csv.get_int(row,"buytimes");
		data_.contents_.push_back(item);
	}
	return true;
}
bool DiscountStore::check(){
	for(size_t i=0; i<data_.contents_.size(); ++i){
		SRV_ASSERT(ItemTable::getItemById(data_.contents_[i].itemId_));
		SRV_ASSERT(data_.contents_[i].price_ != 0);
		SRV_ASSERT(data_.contents_[i].discount_ > 0 || data_.contents_[i].discount_ <= 1 );
	}
	return true;
}
void DiscountStore::open(int64 start, int64 stop){
	data_.sinceStamp_ = start;
	data_.endStamp_ = stop;
	SRV_ASSERT(start < stop);
	DiscountStoreTimer *openT = NEW_MEM(DiscountStoreTimer,start,stop);
	GameTimer::pushtimer(openT);
	//ACE_DEBUG((LM_ERROR,ACE_TEXT("DiscountStore::open startTime=[%d]========endTime=[%d]\n"),data_.sinceStamp_,data_.endStamp_));
	SGE_SysActivity sys;
	sys.stData_ = data_;
	DBHandler::instance()->insertActivity(ADT_DiscountStore,sys);
}
void DiscountStore::open(){
	isOpen_ = true;
}
void DiscountStore::close(){
	isOpen_ = false;
}
void DiscountStore::request(Player* p){
	if(!isOpen_){
		p->sysDiscountStore_ = COM_ADDiscountStore();
		return;
	}
	if ( p->sysDiscountStore_.sinceStamp_ != data_.sinceStamp_ || p->sysDiscountStore_.endStamp_ != data_.endStamp_){
		p->sysDiscountStore_.contents_.clear();
	}
	if(p->sysDiscountStore_.contents_.empty()){
		p->sysDiscountStore_ = data_;
		p->updateSysDiscountStore();
	}
	
}
void DiscountStore::requestSelf(Player* p){
	if(!isOpen_){
		p->selfDiscountStore_ = COM_ADDiscountStore();
		return;
	}
	if(p->selfDiscountStore_.contents_.empty()){
		p->selfDiscountStore_ = data_;
		p->updateSelfDiscountStore();
	}
}

void DiscountStore::update(COM_ADDiscountStore& date){
	open(date.sinceStamp_,date.endStamp_);
	/*if(!isOpen_)
		return;*/
	data_ = date;
	SGE_SysActivity sys;
	sys.stData_ = data_;
	DBHandler::instance()->insertActivity(ADT_DiscountStore,sys);
}

bool DiscountStore::isOpen_ = false;
COM_ADDiscountStore DiscountStore::data_;

bool RechargeSingleTimer::start(){
	RechargeSingle::open();
	return false;
}

bool RechargeSingleTimer::stop(){
	RechargeSingle::close();
	return false;
}

bool RechargeSingle::load(const char* fn){
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);

	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		COM_ADChargeEveryContent item;
		item.currencyCount_ = csv.get_int(row,"num");
		{
			std::string strItems = csv.get_string(row,"reward");
			std::vector<std::string> arrItems = String::Split(strItems,";");
			for(size_t i=0; i<arrItems.size(); ++i){
				item.itemIds_.push_back(atoi(arrItems[i].c_str()));
			}
		}
		{
			std::string str = csv.get_string(row,"renum");
			std::vector<std::string> arr = String::Split(str,";");
			for(size_t i=0; i<arr.size(); ++i){
				item.itemStacks_.push_back(atoi(arr[i].c_str()));
			}
		}
		SRV_ASSERT(item.itemIds_.size() == item.itemStacks_.size());
		item.status_ = 0;
		item.count_ = csv.get_int(row,"time");
		data_.contents_.push_back(item);
	}
	return true;
}
bool RechargeSingle::check(){
	for(size_t i=0; i<data_.contents_.size(); ++i){
		for(size_t j=0; j<data_.contents_[i].itemIds_.size(); ++j){
			SRV_ASSERT(ItemTable::getItemById(data_.contents_[i].itemIds_[j]));
		}
		for(size_t j=0; j<data_.contents_[i].itemStacks_.size(); ++j){
			SRV_ASSERT(data_.contents_[i].itemStacks_[j]);
		}
	}
	return true;
}
void RechargeSingle::open(int64 start, int64 stop){
	data_.sinceStamp_ = start;
	data_.endStamp_ = stop;
	SRV_ASSERT(start < stop);
	RechargeSingleTimer *openT = NEW_MEM(RechargeSingleTimer,start,stop);
	GameTimer::pushtimer(openT);
	//ACE_DEBUG((LM_ERROR,ACE_TEXT("RechargeSingle::open startTime=[%d]========endTime=[%d]\n"),data_.sinceStamp_,data_.endStamp_));
	SGE_SysActivity sys;
	sys.ceData_ = data_;
	DBHandler::instance()->insertActivity(ADT_ChargeEvery,sys);
}
void RechargeSingle::open(){
	isOpen_ = true;
}
void RechargeSingle::close(){
	isOpen_ = false;
}
void RechargeSingle::request(Player* p){
	if(!isOpen_){
		p->sysOnceRecharge_ = COM_ADChargeEvery();
		return;
	}
	if ( p->sysOnceRecharge_.sinceStamp_ != data_.sinceStamp_ || p->sysOnceRecharge_.endStamp_ != data_.endStamp_){
		p->sysOnceRecharge_.contents_.clear();
	}
	if(p->sysOnceRecharge_.contents_.empty()){
		p->sysOnceRecharge_ = data_;
		CALL_CLIENT(p,updateSysOnceRecharge(p->sysOnceRecharge_));
	}
}
void RechargeSingle::requestSelf(Player* p){
	if(!isOpen_){
		p->selfOnceRecharge_ = COM_ADChargeEvery();
		return;
	}
	if(p->selfOnceRecharge_.contents_.empty()){
		p->selfOnceRecharge_ = data_;
		CALL_CLIENT(p,updateSelfOnceRecharge(p->selfOnceRecharge_));
	}
}

void RechargeSingle::update(COM_ADChargeEvery& date){
	open(date.sinceStamp_,date.endStamp_);
	/*if(!isOpen_)
		return;*/
	data_ = date;
	SGE_SysActivity sys;
	sys.ceData_ = data_;
	DBHandler::instance()->insertActivity(ADT_ChargeEvery,sys);
}

bool RechargeSingle::isOpen_ = false;
COM_ADChargeEvery RechargeSingle::data_;

///////////////////////////////////////////////////////////////////////////////////

bool HotShopTimer::start(){
	HotShop::open();
	return false;
}

bool HotShopTimer::stop(){
	HotShop::close();
	return false;
}

void HotShop::open(int64 start, int64 stop){
	data_.sinceStamp_ = start;
	data_.endStamp_ = stop;

	SRV_ASSERT(start < stop);
	HotShopTimer *openT = NEW_MEM(HotShopTimer,start,stop);
	GameTimer::pushtimer(openT);
	//ACE_DEBUG((LM_ERROR,ACE_TEXT("HotShop::open startTime=[%d]========endTime=[%d]\n"),data_.sinceStamp_,data_.endStamp_));
	SGE_SysActivity sys;
	sys.hrData_ = data_;
	DBHandler::instance()->insertActivity(ADT_HotRole,sys);
}

void HotShop::open(){
	isOpen_ = true;
}

void HotShop::close(){
	isOpen_ = false;
}


bool HotShop::isOpen_ = false;
COM_ADHotRole HotShop::data_;

bool
HotShop::load(const char* fn){
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);

	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		COM_ADHotRoleContent tmp;
		tmp.roleId_ = csv.get_int(row,"ID");

		std::string stt = csv.get_string(row,"type");
		int stte = ENUM(EntityType).getItemId(stt);
		if(-1 == stte)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("EntityType error in row %d , id is %d\n"),row,tmp.roleId_));
			SRV_ASSERT(0);	
		}
		tmp.type_ = (EntityType)stte;
		tmp.price_= csv.get_int(row,"price");
		data_.contents_.push_back(tmp);
		data_.contents_.back().buyNum_ = Global::get<int>(C_HotShopBuyNum);
	}
	return true;
}

bool HotShop::check(){
	for (size_t i = 0; i < data_.contents_.size(); ++i)
	{
		if(data_.contents_[i].type_ == ET_Baby){
			SRV_ASSERT(MonsterTable::getMonsterById(data_.contents_[i].roleId_));
		}
		if(data_.contents_[i].type_ == ET_Emplyee){
			SRV_ASSERT(EmployeeTable::getEmployeeById(data_.contents_[i].roleId_));
		}
		if(data_.contents_[i].price_ <= 0)
			SRV_ASSERT(0);
	}
	return true;
}

void HotShop::request(Player* p){
	if(p == NULL)
		return;
	if(!isOpen_)
	{
		p->hotdata_ = COM_ADHotRole();
		return;
	}
	if ( p->hotdata_.sinceStamp_ != data_.sinceStamp_ || p->hotdata_.endStamp_ != data_.endStamp_){
		p->hotdata_.contents_.clear();
	}
	if(p->hotdata_.contents_.empty()){
		p->hotdata_ = data_;
		/*for (size_t i = 0; i < p->hotdata_.contents_.size(); ++i)
		{
			p->hotdata_.contents_[i].buyNum_ = Global::get<int>(C_HotShopBuyNum);
		}*/
		CALL_CLIENT(p,sycnHotRole(p->hotdata_));
	}
}

void HotShop::update(COM_ADHotRole& date){
	open(date.sinceStamp_,date.endStamp_);
	/*if(!isOpen_)
		return;*/
	data_ = date;
	SGE_SysActivity sys;
	sys.hrData_ = data_;
	DBHandler::instance()->insertActivity(ADT_HotRole,sys);
}

///////////////////////////////////////////////////////////////////////////////
std::vector<SevenDayTable::SevenDay*>	SevenDayTable::data_;

bool
SevenDayTable::load(const char* fn){
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);

	for(U32 row=0; row<csv.get_records_counter(); ++row)
	{
		SevenDay* pcore = NEW_MEM(SevenDay);
		pcore->day_ = csv.get_int(row,"Day");
		pcore->quest_ = csv.get_int(row,"ID");

		std::string stt = csv.get_string(row,"type");
		int stte = ENUM(AchievementType).getItemId(stt);
		if(-1 == stte)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("AchievementType error in row %d , id is %d\n"),row,pcore->quest_));
			SRV_ASSERT(0);	
		}
		pcore->type_ = (AchievementType)stte;
		pcore->target_ = csv.get_int(row,"target");

		std::string str1 = csv.get_string(row,"Item");
		const char* cstr1 = str1.c_str();
		std::string strtoken1;
		if(!str1.empty())
		{
			while(TokenParser::getToken( cstr1 , strtoken1 , ';'))
			{
				const char* cstr2 = strtoken1.c_str();
				std::string strtoken2;
				TokenParser::getToken( cstr2 , strtoken2 , ':');
				S32 group = ACE_OS::atoi(strtoken2.c_str());

				TokenParser::getToken( cstr2 , strtoken2 , ':');
				S32 level = ACE_OS::atoi(strtoken2.c_str());
				std::pair<S32,S32> one(group,level);
				pcore->reward_.push_back(one);
			}
		}

		data_.push_back(pcore);
	}
	return true;
}

bool
SevenDayTable::check(){
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if(data_[i]->target_ == 0)
			SRV_ASSERT(0);
		for (size_t j = 0; j < data_[i]->reward_.size(); ++j)
		{
			if(data_[i]->reward_[j].second == 0)
				SRV_ASSERT(0);
			SRV_ASSERT(ItemTable::getItemById(data_[i]->reward_[j].first));
		}
	}
	return true;
}

SevenDayTable::SevenDay const*
SevenDayTable::get(U32 quest){
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if(data_[i]->quest_ == quest)
			return data_[i];
	}
	return NULL;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
bool EmployeeActivityTotalTimer::start(){
	EmployeeActivityTotal::open();
	return false;
}

bool EmployeeActivityTotalTimer::stop(){
	EmployeeActivityTotal::close();
	return false;
}

bool EmployeeActivityTotal::load(const char* fn){
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);

	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		COM_ADEmployeeTotalContent item;
		item.outputCount_ = csv.get_int(row,"num");
		{
			std::string strItems = csv.get_string(row,"reward");
			std::vector<std::string> arrItems = String::Split(strItems,";");
			for(size_t i=0; i<arrItems.size(); ++i){
				item.itemIds_.push_back(atoi(arrItems[i].c_str()));
			}
		}
		{
			std::string str = csv.get_string(row,"renum");
			std::vector<std::string> arr = String::Split(str,";");
			for(size_t i=0; i<arr.size(); ++i){
				item.itemStacks_.push_back(atoi(arr[i].c_str()));
			}
		}
		SRV_ASSERT(item.itemIds_.size() == item.itemStacks_.size());
		item.status_ = 0;
		data_.contents_.push_back(item);
	}
	return true;
}
bool EmployeeActivityTotal::check(){
	for(size_t i=0; i<data_.contents_.size(); ++i){
		for(size_t j=0; j<data_.contents_[i].itemIds_.size(); ++j){
			SRV_ASSERT(ItemTable::getItemById(data_.contents_[i].itemIds_[j]));
		}
		for(size_t j=0; j<data_.contents_[i].itemStacks_.size(); ++j){
			SRV_ASSERT(data_.contents_[i].itemStacks_[j]);
		}
	}
	return true;
}
void EmployeeActivityTotal::open(int64 start, int64 stop){
	data_.sinceStamp_ = start;
	data_.endStamp_ = stop;

	SRV_ASSERT(start < stop);
	EmployeeActivityTotalTimer *openT = NEW_MEM(EmployeeActivityTotalTimer,start,stop);
	GameTimer::pushtimer(openT);
	//ACE_DEBUG((LM_ERROR,ACE_TEXT("EmployeeActivityTotal::open startTime=[%d]========endTime=[%d]\n"),data_.sinceStamp_,data_.endStamp_));
	SGE_SysActivity sys;
	sys.etdata_ = data_;
	DBHandler::instance()->insertActivity(ADT_BuyEmployee,sys);
}
void EmployeeActivityTotal::open(){
	isOpen_ = true;
}
void EmployeeActivityTotal::close(){
	isOpen_ = false;
}
void EmployeeActivityTotal::request(Player* p){
	if(!isOpen_){
		p->empact_ = COM_ADEmployeeTotal();
		return;
	}
	if ( p->empact_.sinceStamp_ != data_.sinceStamp_ || p->empact_.endStamp_ != data_.endStamp_){
		p->empact_.contents_.clear();
	}
	if(p->empact_.contents_.empty()){
		p->empact_ = data_;
		p->updateEmployeeActivity();
	}
}

void EmployeeActivityTotal::update(COM_ADEmployeeTotal& date){
	open(date.sinceStamp_,date.endStamp_);
	/*if(!isOpen_)
		return;*/
	data_ = date;
	SGE_SysActivity sys;
	sys.etdata_ = data_;
	DBHandler::instance()->insertActivity(ADT_BuyEmployee,sys);
}

bool EmployeeActivityTotal::isOpen_ = false;
COM_ADEmployeeTotal EmployeeActivityTotal::data_;

//////////////////////////////////////////////////////////////////////////
bool MySelfRecharge::load(const char* fn){
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);

	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		COM_ADChargeTotalContent item;
		item.currencyCount_ = csv.get_int(row,"num");
		{
			std::string strItems = csv.get_string(row,"reward");
			std::vector<std::string> arrItems = String::Split(strItems,";");
			for(size_t i=0; i<arrItems.size(); ++i){
				item.itemIds_.push_back(atoi(arrItems[i].c_str()));
			}
		}
		{
			std::string str = csv.get_string(row,"renum");
			std::vector<std::string> arr = String::Split(str,";");
			for(size_t i=0; i<arr.size(); ++i){
				item.itemStacks_.push_back(atoi(arr[i].c_str()));
			}
		}
		SRV_ASSERT(item.itemIds_.size() == item.itemStacks_.size());
		item.status_ = 0;
		data_.contents_.push_back(item);
	}
	return true;
}
bool MySelfRecharge::check(){
	for(size_t i=0; i<data_.contents_.size(); ++i){
		for(size_t j=0; j<data_.contents_[i].itemIds_.size(); ++j){
			SRV_ASSERT(ItemTable::getItemById(data_.contents_[i].itemIds_[j]));
		}
		for(size_t j=0; j<data_.contents_[i].itemStacks_.size(); ++j){
			SRV_ASSERT(data_.contents_[i].itemStacks_[j]);
		}
	}
	return true;
}

void MySelfRecharge::requestSelf(Player* p){
	if(p->myselfrecharge_.contents_.empty()){
		p->myselfrecharge_ = data_;
		CALL_CLIENT(p,updateMySelfRecharge(p->myselfrecharge_));
	}
}
COM_ADChargeTotal MySelfRecharge::data_;

//////////////////////////////////////////////////////////////////////////
std::vector<LevelGift::LevelGiftContent*> LevelGift::data_;
bool
LevelGift::load(const char* fn){
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);

	for(U32 row=0; row<csv.get_records_counter(); ++row)
	{
		LevelGiftContent* pcore = NEW_MEM(LevelGiftContent);
		pcore->level_ = csv.get_int(row,"lv");

		std::string str1 = csv.get_string(row,"reward");
		const char* cstr1 = str1.c_str();
		std::string strtoken1;
		if(!str1.empty())
		{
			while(TokenParser::getToken( cstr1 , strtoken1 , ';'))
			{
				const char* cstr2 = strtoken1.c_str();
				std::string strtoken2;
				TokenParser::getToken( cstr2 , strtoken2 , ':');
				S32 itemid = ACE_OS::atoi(strtoken2.c_str());

				TokenParser::getToken( cstr2 , strtoken2 , ':');
				S32 itemnum = ACE_OS::atoi(strtoken2.c_str());
				std::pair<S32,S32> one(itemid,itemnum);
				pcore->reward_.push_back(one);
			}
		}

		data_.push_back(pcore);
	}

	///小额礼包
	MinGift::init();

	return true;
}

bool
LevelGift::check(){
	for (size_t i = 0; i < data_.size(); ++i)
	{
		for (size_t j = 0; j < data_[i]->reward_.size(); ++j)
		{
			if(data_[i]->reward_[j].second == 0)
				SRV_ASSERT(0);
			SRV_ASSERT(ItemTable::getItemById(data_[i]->reward_[j].first));
		}
	}
	return true;
}

LevelGift::LevelGiftContent const*
LevelGift::get(U32 level){
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if(data_[i]->level_ == level)
			return data_[i];
	}
	return NULL;
}

//////////////////////////////////////////////////////////////////////////
COM_ADGiftBag MinGift::data_;
bool MinGift::isOpen_ = false;
void MinGift::init()
{
	Json::Value root;
	Json::Reader jreader;
	std::string content;
	content = Global::get<std::string>(C_MinGiftBag);
	if(!jreader.parse(content ,root)){
		SRV_ASSERT(0);
	}
	if (!root.isArray()){
		SRV_ASSERT(0);
	}

	for (Json::UInt i =0; i<root.size(); ++i){
		COM_GiftItem item;
		if(!root[i].isObject()){
			SRV_ASSERT(0);
		}
		if (root[i]["ItemId"].isNull()){
			SRV_ASSERT(0);
		}
		item.itemId_ = root[i]["ItemId"].asInt();
		if (root[i]["ItemStack"].isNull()){
			SRV_ASSERT(0);
		}
		item.itemNum_ = root[i]["ItemStack"].asInt();
		data_.itemdata_.push_back(item);
	}
	data_.price_ = Global::get<int>(C_MinGiftPrice);
	data_.oldprice_ = Global::get<int>(C_MinGiftOldPrice);
	if(data_.itemdata_.empty())
		SRV_ASSERT(0);
	for (size_t i = 0; i < data_.itemdata_.size(); ++i)
	{
		SRV_ASSERT(ItemTable::getItemById(data_.itemdata_[i].itemId_));
	}
}

bool MinGiftTimer::start(){
	MinGift::open();
	return false;
}

bool MinGiftTimer::stop(){
	MinGift::close();
	return false;
}

void MinGift::open(int64 start, int64 stop){
	data_.sinceStamp_ = start;
	data_.endStamp_ = stop;

	SRV_ASSERT(start < stop);
	MinGiftTimer *openT = NEW_MEM(MinGiftTimer,start,stop);
	GameTimer::pushtimer(openT);
	//ACE_DEBUG((LM_ERROR,ACE_TEXT("EmployeeActivityTotal::open startTime=[%d]========endTime=[%d]\n"),data_.sinceStamp_,data_.endStamp_));
	SGE_SysActivity sys;
	sys.gbdata_ = data_;
	DBHandler::instance()->insertActivity(ADT_GiftBag,sys);
}
void MinGift::open(){
	isOpen_ = true;
}
void MinGift::close(){
	isOpen_ = false;
}
void MinGift::request(Player* p){
	if(!isOpen_){
		p->gbact_ = COM_ADGiftBag();
		return;
	}
	if ( p->gbact_.sinceStamp_ != data_.sinceStamp_ || p->gbact_.endStamp_ != data_.endStamp_){
		p->gbact_.itemdata_.clear();
	}
	if(p->gbact_.itemdata_.empty()){
		p->gbact_ = data_;
		p->updateMinGiftActivity();
	}
}

void MinGift::update(COM_ADGiftBag& date){
	open(date.sinceStamp_,date.endStamp_);
	/*if(!isOpen_)
		return;*/
	data_ = date;
	SGE_SysActivity sys;
	sys.gbdata_ = data_;
	DBHandler::instance()->insertActivity(ADT_GiftBag,sys);
}

///////////////////////////////////积分商店///////////////////////////////////////

bool IntegralTimer::start(){
	IntegralShop::open();
	return false;
}

bool IntegralTimer::stop(){
	IntegralShop::close();
	return false;
}

bool IntegralShop::isOpen_ = false;
COM_IntegralData IntegralShop::data_;

bool IntegralShop::load(const char* fn){
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);

	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		COM_IntegralContent content;
		content.id_ = csv.get_int(row,"ID");
		content.itemid_ = csv.get_int(row,"ItemID");
		content.times_ = csv.get_int(row,"Times");
		content.cost_ = csv.get_int(row,"Cost");
		data_.contents_.push_back(content);
	}
	return true;
}

bool IntegralShop::check(){
	for (size_t i = 0; i < data_.contents_.size(); ++i)
	{	
		if(data_.contents_[i].times_ <= 0)
			SRV_ASSERT(0);
		if(data_.contents_[i].cost_<= 0)
			SRV_ASSERT(0);
		SRV_ASSERT(ItemTable::getItemById(data_.contents_[i].itemid_));
	}
	return true;
}

void IntegralShop::open(int64 start, int64 stop){
	data_.sinceStamp_ = start;
	data_.endStamp_ = stop;

	SRV_ASSERT(start < stop);
	IntegralTimer *openT = NEW_MEM(IntegralTimer,start,stop);
	GameTimer::pushtimer(openT);
	//ACE_DEBUG((LM_ERROR,ACE_TEXT("EmployeeActivityTotal::open startTime=[%d]========endTime=[%d]\n"),data_.sinceStamp_,data_.endStamp_));
	SGE_SysActivity sys;
	sys.icdata_ = data_;
	DBHandler::instance()->insertActivity(ADT_IntegralShop,sys);
}
void IntegralShop::open(){
	isOpen_ = true;
}
void IntegralShop::close(){
	isOpen_ = false;
}

void IntegralShop::request(Player* p){
	if(!isOpen_){
		p->icdata_ = COM_IntegralData();
		return;
	}
	if(p->icdata_.contents_.empty()){
		p->icdata_ = data_;
		p->updateIntegralShop();
	}
}

void IntegralShop::update(COM_IntegralData& date){
	open(date.sinceStamp_,date.endStamp_);
	/*if(!isOpen_)
		return;*/
	data_ = date;
	SGE_SysActivity sys;
	sys.icdata_ = data_;
	DBHandler::instance()->insertActivity(ADT_IntegralShop,sys);
}

//////////////////////////////////历程礼包////////////////////////////////////////

std::vector<CourseGiftTable::CourseGiftContent*> CourseGiftTable::data_;
bool
CourseGiftTable::load(const char* fn){
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);

	for(U32 row=0; row<csv.get_records_counter(); ++row)
	{
		CourseGiftContent* pcore = new CourseGiftContent;
		pcore->id_ = csv.get_int(row,"ShopID");
		pcore->level_ = csv.get_int(row,"Lv");
		pcore->timeout_ = csv.get_int(row,"Time");
		std::string str1 = csv.get_string(row,"ItemID");
		const char* cstr1 = str1.c_str();
		std::string strtoken1;
		if(!str1.empty())
		{
			while(TokenParser::getToken( cstr1 , strtoken1 , ';'))
			{
				const char* cstr2 = strtoken1.c_str();
				std::string strtoken2;
				TokenParser::getToken( cstr2 , strtoken2 , ':');
				S32 itemid = ACE_OS::atoi(strtoken2.c_str());

				TokenParser::getToken( cstr2 , strtoken2 , ':');
				S32 itemnum = ACE_OS::atoi(strtoken2.c_str());
				std::pair<S32,S32> one(itemid,itemnum);
				pcore->reward_.push_back(one);
			}
		}

		data_.push_back(pcore);
	}

	return true;
}

bool
CourseGiftTable::check(){
	for (size_t i = 0; i < data_.size(); ++i)
	{
		for (size_t j = 0; j < data_[i]->reward_.size(); ++j)
		{
			if(data_[i]->reward_[j].second == 0)
				SRV_ASSERT(0);
			SRV_ASSERT(ItemTable::getItemById(data_[i]->reward_[j].first));
		}
	}
	return true;
}

CourseGiftTable::CourseGiftContent const*
CourseGiftTable::getbylevel(U32 level){
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if(data_[i]->level_ == level)
			return data_[i];
	}
	return NULL;
}

CourseGiftTable::CourseGiftContent const*
CourseGiftTable::getbyshopid(U32 shopid){
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if(data_[i]->id_ == shopid)
			return data_[i];
	}
	return NULL;
}