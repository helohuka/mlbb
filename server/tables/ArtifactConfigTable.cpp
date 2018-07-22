#include "ArtifactConfigTable.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"
#include "itemtable.h"
std::map< S32 , std::vector<ArtifactConfigTable::ArtifactConfigData*> >  ArtifactConfigTable::data_;

bool ArtifactConfigTable::load(char const *fn)
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
		ArtifactConfigData * pCore = new ArtifactConfigData;
		pCore->level_ = csv.get_int(row,"Lv");
		pCore->diamonds_ = csv.get_int(row,"Diamonds");
		pCore->tupoLevel_ = csv.get_int(row,"LvBreach");
		pCore->grow_ = csv.get_int(row,"LvQuality");

		ArtifactConfigItem item;
		item.itemId_ = csv.get_int(row,"Item_1");
		item.itemNum_ =  csv.get_int(row,"Item_1Num");
		pCore->items_.push_back(item);
		
		std::string stt = csv.get_string(row,"JobType");
		int stte = ENUM(JobType).getItemId(stt);
		if(-1 == stte)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("ItemMainType error in row %d , id is %d\n"),row,pCore->level_));
			SRV_ASSERT(0);	
		}
		pCore->professionType_ = (JobType)stte;


		if(!data_[pCore->level_].empty())
		{
			data_[pCore->level_].push_back(pCore);
		}
		else
		{
			std::vector<ArtifactConfigData*> dataList ;
			dataList.push_back(pCore);
			data_[pCore->level_] = dataList;
		}

	}
		return true;
}

bool ArtifactConfigTable::check(){
	for(std::map< S32 , std::vector<ArtifactConfigData*> >::iterator i = data_.begin(), e = data_.end(); i!=e; ++i)
	{
		std::vector<ArtifactConfigData*> &dv = i->second;
		for(size_t i=0; i<dv.size(); ++i){
			for(size_t j=0; j<dv[i]->items_.size(); ++j){
				if(dv[i]->items_[j].itemId_){
					SRV_ASSERT(ItemTable::getItemById(dv[i]->items_[j].itemId_));
					SRV_ASSERT(dv[i]->items_[j].itemNum_);
				}
			}
		}
	}
	
	return true;
}

ArtifactConfigTable::ArtifactConfigData const*
ArtifactConfigTable::getArtifactById(S32 level,JobType job)
{
	for(size_t i =0;i<data_[level].size();i++)
	{
		if(data_[level][i]->professionType_ == job)
		{
			return data_[level][i];
		}
	}
	return NULL;
}