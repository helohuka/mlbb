#include "LiveSkilltable.h"
#include "itemtable.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "DropTable.h"
//------------------------------------------------------------------------------------------------------------------

std::map< S32 , GatherTable::GatherCore* >  GatherTable::gatherData_;

bool
GatherTable::load(char const *fn)
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

	for(U32 row=0; row < csv.get_records_counter(); ++row)
	{
		S32 id = csv.get_int(row,"Id");
		if(gatherData_[id] != NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("LiveSkill has same id in row %d , id is %d\n"),row,id));
			SRV_ASSERT(0);	
		}

		GatherCore* pCore = new GatherCore;

		pCore->id_				= id;
		pCore->type_			= (MineType)ENUM(MineType).getItemId(csv.get_string(row,"Type"));
		pCore->level_			= csv.get_int(row,"Level");
		pCore->dropId_	= csv.get_int(row,"dropID");
		pCore->money_ = csv.get_int(row,"money");
		pCore->superdrop_	= csv.get_int(row,"seniordropID");
		pCore->addmoney_ = csv.get_int(row,"addvalue");

		std::string show = String::Split(csv.get_string(row,"Show"),";")[0];
		pCore->itemId_ = atoi(show.c_str());
		gatherData_[id] = pCore;
	}

	return true;
}

bool
GatherTable::check()
{
	for(std::map< S32 , GatherCore* >::iterator itr=gatherData_.begin(); itr!=gatherData_.end(); ++itr){
		if(itr->second->level_ <=0 ){
			return false;
		}
		if(!DropTable::getDropBaseById(itr->second->dropId_)){
			ACE_DEBUG((LM_ERROR,"Gather table drop not found %d\n",itr->second->dropId_));
			return false;
		}
	}
	return true;
}

GatherTable::GatherCore const*
GatherTable::getGatherById(S32 id)
{
	return gatherData_[id];
}

std::vector<int32> GatherTable::getItemsByLevel(S32 lev){
	std::vector<int32> items;
	for( std::map< S32 , GatherCore* >::iterator i=gatherData_.begin(),e=gatherData_.end(); i!=e; ++i){
		if(i->second->level_ == lev){
			items.push_back(i->second->itemId_);
		}
	}
	return items;
}

//------------------------------------------------------------------------------------------------------------------

std::map< S32 , MakeTable::MakeCore* >  MakeTable::MakeData_;

bool
MakeTable::load(char const *fn)
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

	for(U32 row=0; row < csv.get_records_counter(); ++row)
	{
		S32 id = csv.get_int(row,"item_id");
		if(MakeData_[id] != NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("LiveSkill has same id in row %d , id is %d\n"),row,id));
			SRV_ASSERT(0);	
		}

		MakeCore* pCore = new MakeCore;

		pCore->id_					= id;
		pCore->kind_			= csv.get_int(row,"skill_kind");
		pCore->level_			= csv.get_int(row,"skill_lv");
		pCore->gainSkillExp_	= csv.get_int(row,"gain_skillexp");
		pCore->chance_			= csv.get_int(row,"Chance");
		pCore->newItem_			= csv.get_int(row,"SpecialID");

		std::string res			= csv.get_string(row,"cost_res");
		int restype				= ENUM(PropertyType).getItemId(res); 
		pCore->costType_		= (PropertyType)restype;

		pCore->costNum_			= csv.get_int(row,"cost_num");
		pCore->profession_		= csv.get_int(row,"profession");
		pCore->needMoney_		= csv.get_int(row,"need_money");

		std::string strTmp = csv.get_string(row,"need_item");
		char const * pChar = strTmp.c_str();
		std::string strToken;
		if(!strTmp.empty())
		{
			int idx = 0;
			while( TokenParser::getToken(pChar,strToken,';'))
			{
				pCore->costItems_.push_back(atoi(strToken.c_str()));
			}
		}

		std::string strNumTmp = csv.get_string(row,"need_itemnum");
		char const * pCharTmp = strNumTmp.c_str();
		std::string strTokens;
		if(!strTmp.empty())
		{
			int idx = 0;
			while(TokenParser::getToken(pCharTmp,strTokens,';'))
			{
				pCore->costItemNum_.push_back(atoi(strTokens.c_str()));
			}
		}

		MakeData_[id] = pCore;
	}

	return true;
}

bool
MakeTable::check()
{
	std::map<S32,MakeCore*>::iterator itr = MakeData_.begin();

	while (itr != MakeData_.end())
	{
		MakeCore* pCore = itr->second;

		if(pCore == NULL)
			return false;

		ItemTable::ItemData const* item = ItemTable::getItemById(pCore->id_);
		if(item == NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("Don't find this item[%d] in the item table\n"),pCore->id_));
			return false;
		}
		
		SRV_ASSERT(pCore->costItems_.size() == pCore->costItemNum_.size());

		for(size_t i=0; i<pCore->costItems_.size(); ++i){
			if(pCore->costItems_[i]){
				SRV_ASSERT(ItemTable::getItemById(pCore->costItems_[i]));
				SRV_ASSERT(pCore->costItemNum_[i]);
			}
		}

		SRV_ASSERT(ItemTable::getItemById(pCore->newItem_));

		++itr;
	}

	return true;
}

MakeTable::MakeCore const*
MakeTable::getMakeById(S32 id){
	return MakeData_[id];
}