#include "achievementTable.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "itemtable.h"
#include "DropTable.h"
std::map<S32,AchievementTable::AchievementData*> AchievementTable::data_;

bool
AchievementTable::load(char const *fn)
{
	if(NULL == fn)
		return false;
	if(strlen(fn) == 0)
		return false;
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		return false;
	}

	if(csv.get_records_counter() == 0)
		return false;

	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		S32 id = csv.get_int(row,"ID");
		if(data_[id] != NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("Achievement has same id in row %d , id is %d\n"),row,id));
			return false;
		}

		AchievementData* pAch = new AchievementData;
		pAch->achId_ = id;
		std::string stt = csv.get_string(row,"type");
		int stte = ENUM(AchievementType).getItemId(stt);
		if(-1 == stte)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("AchievementType error in row %d , id is %d\n"),row,id));
			return false;
		}
		pAch->type_ = (AchievementType)stte;
		pAch->titleid_ = csv.get_int(row,"title");
		pAch->value_ = csv.get_int(row,"target");
		pAch->dropId_ = csv.get_int(row,"DropID");
	
		data_[id] = pAch;
	}

	return true;
}

bool
AchievementTable::check()
{
	std::map< S32 , AchievementData* >::iterator itor = data_.begin();

	while(itor != data_.end())
	{
		AchievementData* pAch = itor->second;

		if (!pAch)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("AchievementTable not find Achievement error\n")));
			return false;
		}
		if(pAch->value_ == 0 )
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("AchievementTable id[%d] num==0\n"),pAch->achId_));
			return false;
		}

		if(DropTable::getDropById(pAch->dropId_) == NULL){
			ACE_DEBUG((LM_ERROR,ACE_TEXT("AchievementTable id[%d] drop not find\n"),pAch->achId_));
			return false;
		}
		
		itor++;
	}
	return true;
}

AchievementTable::AchievementData const*
AchievementTable::getAchievementById(U32 id)
{
	return data_[id];
}

void
AchievementTable::geAchDataByType(AchievementType achtype,std::vector<AchievementData*> ach)
{
	std::map< S32 , AchievementData* >::iterator itor = data_.begin();

	while (itor != data_.end())
	{
		AchievementData* pData = itor->second;
		if(!pData)
			continue;
		if(pData->type_ == achtype)
			ach.push_back(pData);
				
		++itor;
	}
}

U32
AchievementTable::getAchievementNum()
{
	return data_.size();
}