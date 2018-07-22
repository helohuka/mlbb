
#include "titleTable.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"

std::vector<TitleTable::TitleData* >  TitleTable::data_;

bool TitleTable::load(char const *fn)
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

	for(U32 row=0; row<csv.get_records_counter(); ++row)
	{
		S32 id = csv.get_int(row,"TitleId");
	

		TitleData * pCore = new TitleData;

		pCore->titleId_ = id;
		pCore->minR_	= csv.get_int(row,"MinReputation");
		pCore->maxR_	= csv.get_int(row,"MaxReputation");

		data_.push_back(pCore);
	}

	return true;
}

bool
TitleTable::check()
{
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if(data_[i] == NULL)
			return false;

		if((data_[i]->minR_ != 0)&&(data_[i]->minR_ >= data_[i]->maxR_))
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("TitleTable [%d] Here is wrong\n"),data_[i]->titleId_));
			return false;
		}
	}	

	return true;
}

S32 TitleTable::findTitleByValue(S32 value)
{
	for(size_t i=0; i<data_.size(); ++i)
	{
		if(data_[i]->maxR_ == 0 && data_[i]->minR_ == 0)
			continue;

		if(value <= data_[i]->maxR_ && value >= data_[i]->minR_)
		{
			return data_[i]->titleId_;
		}
	}

	return 0;
}

bool
TitleTable::isReputationTitle(S32 value)
{
	for(size_t i=0; i<data_.size(); ++i)
	{
		if(data_[i]->titleId_ == value)
		{
			if(data_[i]->maxR_ == 0 && data_[i]->minR_ == 0)
				return false;
			else
				return true;
		}
	}
	return false;
}

bool
TitleTable::isTitle(S32 title){
	for (size_t i=0; i<data_.size();++i)
	{
		if(data_[i]->titleId_ == title)
			return true;
	}
	return false;
}
