#include "config.h"
#include "GuildData.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"
#include "itemtable.h"
#include "skilltable.h"
std::map<S32 ,std::vector<GuildShopItemData*> >  GuildShopItemData::cache_;
std::map<S32 ,GuildShopItemData*> GuildShopItemData::data_;
bool GuildShopItemData::load(char const *fn)
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
		GuildShopItemData * pCore = new GuildShopItemData();
		pCore->id_ =	csv.get_int(row,"ID");
		pCore->itemId_ = csv.get_int(row,"Itemid");
		pCore->itemStack_ = csv.get_int(row,"Num");
		pCore->price_ = csv.get_int(row,"Price");
		pCore->needLevel_ = csv.get_int(row,"needlv");
		pCore->timeLimit_ = csv.get_int(row,"Timelimit");
		cache_[pCore->needLevel_].push_back(pCore);
		data_[pCore->id_] = pCore;
	}
	return true;
}

bool
GuildShopItemData::check()
{
	for( std::map<S32 ,GuildShopItemData*>::iterator i=data_.begin(), e=data_.end(); i!=e; ++i){
		SRV_ASSERT(i->second->price_);
		SRV_ASSERT(i->second->itemStack_);
		SRV_ASSERT(ItemTable::getItemById(i->second->itemId_));
	}
	return true;
}

void 
GuildShopItemData::randomItems(std::vector<COM_GuildShopItem>& items,int32 lev){

	if(cache_[lev].empty())
		return;

	std::random_shuffle(cache_[lev].begin(),cache_[lev].end());
	
	for(size_t i=0; i<cache_[lev].size(); ++i){
		COM_GuildShopItem sitem;
		sitem.shopId_ = cache_[lev][i]->id_;
		sitem.buyLimit_ = cache_[lev][i]->timeLimit_;
		items.push_back(sitem);
	}
}


std::vector<std::vector<GuildBuildingData*> > GuildBuildingData::cache_;

bool GuildBuildingData::load(char const *fn){
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

	enum {
		MIN_Level = 1,
		MAX_Level = 10,
	};

	cache_.resize(GBT_MAX);
	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		S32 id = csv.get_int(row,"ArchitectureID");
		SRV_ASSERT(id > GBT_MIN && id < GBT_MAX);
		if(cache_[id].empty()){
			cache_[id].resize(MAX_Level);
		}

		GuildBuildingData *p = new GuildBuildingData();
		p->id_ = id;
		p->level_ = csv.get_int(row,"Level");
		p->needMoney_ = csv.get_int(row,"NeedMoney");
		p->number_ = csv.get_int(row,"Number");

		SRV_ASSERT(p->level_ >=MIN_Level && p->level_ <=MAX_Level);


		cache_[id][p->level_-1] = p;
	}
	
	return true;
}


GuildBuildingData const * GuildBuildingData::getGuildBuidingData(GuildBuildingType gbt,int level){
	if(gbt > GBT_MIN && gbt < GBT_MAX){
		level -= 1;
		if(level < 0 || level > cache_[gbt].size()){
			return NULL;
		}
		return cache_[gbt][level];
	}
	return NULL;
}

bool GuildBlessingData::load(char const *fn){
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

	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		GuildBlessingData gbd;
		gbd.id_ = csv.get_int(row,"ID");
		gbd.skId_ = csv.get_int(row,"SkillID");

		cache_.push_back(gbd);
	}
	return true;
}

bool GuildBlessingData::check(){
	for(size_t i=0; i<cache_.size();++i){
		SRV_ASSERT(SkillTable::getSkillById(cache_[i].skId_,1));
	}
	return true;
}

std::vector<GuildBlessingData> GuildBlessingData::cache_;
