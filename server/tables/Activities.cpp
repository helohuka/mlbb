#include "config.h"
#include "Activities.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "BattleData.h"
#include "itemtable.h"

bool PetActivity::isOpen(S32 weekday)const{
	for(size_t i=0; i<openWeekdays_.size(); ++i)
		if(openWeekdays_[i] == weekday)
			return true;
	return false;
}
bool PetActivity::condition(S32 level, S32 battleId)const{
	S32 index = -1;
	for(size_t i=0; i<battleIds_.size(); ++i)
	{
		if(battleIds_[i] == battleId)
		{
			index = i;
			break;
		}
	}
	if(-1 == index)
		return false;
	if(index >= (S32)conditionLevels_.size())
		return false;
	return conditionLevels_[index] <= level;
}
PetActivity::PetActivityContainer PetActivity::cache_;
bool PetActivity::load(const char* fn){
	SRV_ASSERT(fn);
	SRV_ASSERT(strlen(fn));
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	SRV_ASSERT(csv.load_table_file(fn))	;
	SRV_ASSERT(csv.get_records_counter());
	
	clear();
	cache_.resize(csv.get_records_counter() + 1);
	for(U32 r = 0; r < csv.get_records_counter(); ++r){
		PetActivity* pa = new PetActivity();
		pa->petActId_ = csv.get_int(r,"ID");
		
		{
			std::string strTmp = csv.get_string(r,"OpenTimeDesc");
			char const * pChar = strTmp.c_str();
			std::string strToken;
			while( !strTmp.empty() && TokenParser::getToken( pChar , strToken , ';')){
				pa->openWeekdays_.push_back(ACE_OS::atoi(strToken.c_str()));
			}
		}

		{
			std::string strTmp = csv.get_string(r,"Difficults");
			char const * pChar = strTmp.c_str();
			std::string strToken;
			while( !strTmp.empty() && TokenParser::getToken( pChar , strToken , ';')){
				pa->battleIds_.push_back(ACE_OS::atoi(strToken.c_str()));
			}
		}

		{
			std::string strTmp = csv.get_string(r,"Levels");
			char const * pChar = strTmp.c_str();
			std::string strToken;
			while( !strTmp.empty() && TokenParser::getToken( pChar , strToken , ';')){
				pa->conditionLevels_.push_back(ACE_OS::atoi(strToken.c_str()));
			}
		}

		cache_[pa->petActId_] = pa;
	}
	return true;
}
void PetActivity::clear(){
	for(size_t i=0; i<cache_.size(); ++i){
		if(cache_[i])
			delete cache_[i];
	}
	cache_.clear();
}
bool PetActivity::check(){
	for(size_t i=0; i<cache_.size(); ++i){
		if(!cache_[i])continue;
		for(size_t j=0; j<cache_[i]->battleIds_.size(); ++j){
			SRV_ASSERT(BattleData::getBattleDataById(cache_[i]->battleIds_[j]));
		}
	}
	return true;
}
const PetActivity* PetActivity::getPetActivityById(S32 id){
	if(id <= 0 || id >= (S32)cache_.size())
		return NULL;
	return cache_[id];
}

const PetActivity* PetActivity::getPetActivityByBattleId(S32 battleId){
	for (size_t i=0; i<cache_.size(); ++i){
		if(cache_[i] != NULL && cache_[i]->isMyBattle(battleId))
			return cache_[i];
	}
	return NULL;
}

//--------------------------------------------------------------------------------------------------------

std::vector<OnlineTimeClass::onlinetimedata> OnlineTimeClass::data_;

bool OnlineTimeClass::load(const char* fn){
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
	for(S32 row=0; row<csv.get_records_counter(); ++row){
		S32 id = csv.get_int(row,"id");
		onlinetimedata co;
		co.index_ = id;
		co.targettime_ = csv.get_int(row,"time");
		std::string strTmp = csv.get_string(row,"reward");
		if(!strTmp.empty())
		{
			char const * pChar = strTmp.c_str();
			std::string strToken;
			while( TokenParser::getToken( pChar , strToken , ';'))
			{
				co.rewards_.push_back(atoi(strToken.c_str()));
			}
		}

		std::string strTmp1 = csv.get_string(row,"renum");
		if(!strTmp1.empty())
		{
			char const * pChar = strTmp1.c_str();
			std::string strToken;
			while( TokenParser::getToken( pChar , strToken , ';'))
			{
				co.reNum_.push_back(atoi(strToken.c_str()));
			}
		}
		data_.push_back(co);
	}

	return true;
}

bool OnlineTimeClass::check(){
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if(data_[i].targettime_ == 0)
			return false;
		if(data_[i].rewards_.size() != data_[i].reNum_.size())
			return false;
		for (size_t j = 0; j < data_[i].rewards_.size(); ++j)
		{
			if(data_[i].reNum_[j] == 0)
				return false;
			SRV_ASSERT(ItemTable::getItemById(data_[i].rewards_[j]) != NULL);
		}
	}
	return true;
}

OnlineTimeClass::onlinetimedata const*
OnlineTimeClass::getonlinereward(U32 index){
	for (size_t i=0; i<data_.size();++i)
	{
		if(data_[i].index_ == index)
			return &data_[i];
	}
	return NULL;
}

U32 OnlineTimeClass::getMaxIndex(){
	U32 index = data_.size();
	return index;
}

//---------------------------------------------------------------------------------------------------
std::vector<GrowFundTable::GrowFundData*> GrowFundTable::data_;

bool GrowFundTable::load(const char* fn){
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
	for(S32 row=0; row<csv.get_records_counter(); ++row){
		S32 id = csv.get_int(row,"lv");
		GrowFundData* co = new GrowFundData;
		co->level_ = id;
		co->item_ = csv.get_int(row,"reward");
		co->itemNum_ = csv.get_int(row,"renum");
		data_.push_back(co);
	}

	return true;
}

bool GrowFundTable::check(){
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if(data_[i] == NULL)
			return false;
		SRV_ASSERT(ItemTable::getItemById(data_[i]->item_) != NULL);
		SRV_ASSERT(data_[i]->itemNum_);
	}
	return true;
}

GrowFundTable::GrowFundData const*
GrowFundTable::getDataByLevel(U32 level)
{
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if(data_[i] == NULL)
			return NULL;
		if(data_[i]->level_ == level)
			return data_[i];
	}
	return NULL;
}
