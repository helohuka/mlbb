#include "challengeTable.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "itemtable.h"
#include "BattleData.h"
#include "scenetable.h"
std::vector<ChallengeTable::Core> ChallengeTable::data_;

bool
ChallengeTable::load(char const *fn)
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

	for(U32 row=0; row<csv.get_records_counter(); ++row)
	{
		S32 id = csv.get_int(row,"ID");
		Core co;
		co.id_ = id;
		co.battleId_ = csv.get_int(row,"BattleID");
		co.senceId_  = csv.get_int(row,"SenceName");
		data_.push_back(co);
	}

	return true;
}

bool
ChallengeTable::check()
{
	for (size_t i = 0; i < data_.size(); ++i)
	{
		SRV_ASSERT(BattleData::getBattleDataById(data_[i].battleId_));
		SRV_ASSERT(SceneTable::getSceneById(data_[i].senceId_));
	}

	return true;
}

ChallengeTable::Core const *
ChallengeTable::getDataById(U32 id)
{
	for (size_t i=0; i<data_.size(); ++i)
	{
		if(id == data_[i].id_)
			return &data_[i];
	}

	return NULL;
}

ChallengeTable::Core const*
ChallengeTable::getDataByBattleId(U32 id)
{
	for (size_t i=0; i<data_.size(); ++i)
	{
		if(id == data_[i].battleId_)
			return &data_[i];
	}

	return NULL;
}

bool
ChallengeTable::isHundredSence(U32 senceId)
{
	for (size_t i=0; i<data_.size(); ++i)
	{
		if(senceId == data_[i].senceId_)
			return true;
	}

	return false;
}
