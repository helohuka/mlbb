#include "PVPrunkTable.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"

std::map< S32 , PvpRunkTable::PvpRunkDate* >  PvpRunkTable::data_;

bool
PvpRunkTable::load(char const *fn)
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
		S32 id = csv.get_int(row,"runk");

		if(data_[id] != NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("PVPrunk has same id in row %d , id is %d\n"),row,id));
			SRV_ASSERT(0);	
		}

		PvpRunkDate* pCore = new PvpRunkDate;

		pCore->runk_ = id;
		pCore->gradeMin_ = csv.get_int(row,"min");
		pCore->gradeMax_ = csv.get_int(row,"max");
		pCore->dropitem_ = csv.get_int(row,"dropID_times");
		pCore->dropDay_  = csv.get_int(row,"dropID_day");
		pCore->dropQuarter_ = csv.get_int(row,"dropID_senson");

		data_[id] = pCore;
	}
	return true;
}

bool
PvpRunkTable::check()
{
	return true;
}

PvpRunkTable::PvpRunkDate const*
PvpRunkTable::getPvpRunkById(U32 id)
{
	return data_[id];
}

// --------------------------- [1/22/2016 lwh]
std::vector<PvRrewardTable::PvrrewardData*>	PvRrewardTable::data_;

bool
PvRrewardTable::load(char const *fn)
{
	if(NULL == fn)
		return false;
	if(strlen(fn) == 0)
		return false;
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
		return false;
	if(csv.get_records_counter() == 0)
		return false;
	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		U32 id = csv.get_int(row,"ID");
		if(id == 0)
			continue;

		PvrrewardData* pCore = new PvrrewardData;
		std::string strRanking = csv.get_string(row,"Ranking");
		const char* cstr1 = strRanking.c_str();
		if(!strRanking.empty())
		{
			std::string strtoken2;
			TokenParser::getToken( cstr1 , strtoken2 , ';');
			S32 group = ACE_OS::atoi(strtoken2.c_str());

			TokenParser::getToken( cstr1 , strtoken2 , ';');
			S32 level = ACE_OS::atoi(strtoken2.c_str());
			std::pair<S32,S32> one(group,level);
			pCore->ranking_.push_back(one);
		}

		pCore->dropitem_ = csv.get_int(row,"dropID_times");
		pCore->dropDay_ = csv.get_int(row,"dropID_day");
		pCore->dropQuarter_ = csv.get_int(row,"dropID_senson");

		data_.push_back(pCore);
	}

	return true;
}

bool
PvRrewardTable::check()
{
	return true;
}

PvRrewardTable::PvrrewardData const*
PvRrewardTable::getPvrRunkById(U32 id)
{
	for(size_t i = 0; i < data_.size(); ++i)
	{
		for (size_t j = 0; j < data_[i]->ranking_.size(); ++j)
		{
			if(data_[i]->ranking_[j].first <= id && data_[i]->ranking_[j].second >= id)
				return data_[i];
		}
	}
	return NULL;
}