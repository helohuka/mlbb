#include "Common.h"
#include "Shop.h"
#include "CSVParser.h"
#include "itemtable.h"
std::vector<Shop::Record*> Shop::records_;

void Shop::clear()
{
	for (size_t i=0; i<records_.size(); ++i)
	{
		if(records_[i])
			delete records_[i];
	}

	records_.clear();
}
bool Shop::load(const char* fn)
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

	for(S32 row=0; row<csv.get_records_counter(); ++row)
	{
		Record* record = new Record;

		record->id_ = csv.get_int(row,"ID");
		record->name_ = csv.get_string(row,"Name");
		record->itemId_ = csv.get_int(row,"Itemid");
		record->itemNum_ = csv.get_int(row,"Num");
		record->pay_ = csv.get_int(row,"Price");
		record->idName_ = csv.get_string(row,"IOSID");
		{
			std::string strenum = csv.get_string(row,"ShopType");
			S32 intenum = ENUM(ShopType).getItemId(strenum);
			if(-1 == intenum)
				return false;
			record->type_ = (ShopType)intenum;
		}
		{
			std::string strenum = csv.get_string(row,"ShopPayType");
			S32 intenum = ENUM(ShopPayType).getItemId(strenum);
			if(-1 == intenum)
				return false;
			record->paytype_ = (ShopPayType)intenum;
		}

		

		records_.push_back(record);
	}
	return true;
}
bool Shop::check()
{
	for(size_t i=0; i<records_.size(); ++i)
	{
		if(records_[i]->itemId_)
		{
			SRV_ASSERT(ItemTable::getItemById(records_[i]->itemId_));
			SRV_ASSERT(records_[i]->itemNum_);
		}
	}
	return true;
}

const Shop::Record* Shop::getRecordById(S32 id)
{
	for(size_t i=0; i<records_.size(); ++i)
	{
		if(records_[i]->id_ == id)
			return records_[i];
	}

	return NULL;
}

const Shop::Record* Shop::getRecordByName(std::string const& name)
{
	for(size_t i=0; i<records_.size(); ++i)
	{
		if(records_[i]->idName_ == name)
			return records_[i];
	}

	return NULL;
}