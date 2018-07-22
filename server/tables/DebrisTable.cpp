#include "config.h"
#include "DebrisTable.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"
#include "itemtable.h"
#include "employeeTable.h"


std::map< S32 , DebrisTable::DebrisData* >  DebrisTable::data_;

bool DebrisTable::load(char const *fn)
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
	for(uint32 row=0; row<csv.get_records_counter(); ++row)
	{
		S32 id = csv.get_int(row,"ID");
		if(data_[id] != NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("debrisTable  has same id in row %d , id is %d\n"),row,id));
			SRV_ASSERT(0);	
		}

		DebrisData * pCore = new DebrisData ;
		pCore->id_ = id;
		pCore->needNum_ = csv.get_int(row,"Number");
		pCore->itemId_ = csv.get_int(row,"Item");
		pCore->itemNum_ = csv.get_int(row,"ItemNumber");
	
		std::string stt = csv.get_string(row,"ItemSubType");
		int stte = ENUM(ItemSubType).getItemId(stt);
		if(-1 == stte)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("ItemSubType error in row %d , id is %d\n"),row,id));
			SRV_ASSERT(0);	
		}
		pCore->subType_ = (ItemSubType)stte;



	data_[id] = pCore;

	}

	return true;
}

bool DebrisTable::check()
{
	for(std::map< S32 , DebrisData* >::iterator i=data_.begin(),e=data_.end(); i!=e; ++i){

		if(i->second->subType_ == IST_ItemDebris )
			SRV_ASSERT(ItemTable::getItemById(i->second->itemId_));
		if(i->second->subType_ == IST_EmployeeDebris)
			SRV_ASSERT(EmployeeTable::getEmployeeById(i->second->itemId_));
		SRV_ASSERT(i->second->itemNum_);
	}
	return true;
}

DebrisTable::DebrisData const*
DebrisTable::getItemById(U32 id)
{
	return data_[id];
}
