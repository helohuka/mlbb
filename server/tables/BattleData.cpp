#include "config.h"
#include "BattleData.h"
#include "CSVParser.h"
#include "monstertable.h"
std::vector<BattleData*> BattleData::data_;


bool BattleData::checkInside(){
	for(size_t i=0;i<upGroup_.size(); ++i){
		for (size_t j=0; j<upGroup_[i].size(); ++j)
		{
			if(upGroup_[i][j].empty() || upGroup_[i][j]== "")
				continue;
			if(NULL == MonsterClass::getClassByName(upGroup_[i][j]))
			{
				ACE_DEBUG((LM_ERROR,"Can not find monster %s in monster class \n",upGroup_[j][j].c_str()));
				return false;
			}
		}
	}

	for(size_t i=0;i<downGroup_.size(); ++i){
		for (size_t j=0; j<downGroup_[i].size(); ++j)
		{
			if(downGroup_[i][j].empty() || downGroup_[i][j]== "")
				continue;
			if(NULL == MonsterClass::getClassByName(downGroup_[i][j]))
			{
				ACE_DEBUG((LM_ERROR,"Can not find monster %s in monster class \n",downGroup_[j][j].c_str()));
				return false;
			}
		}
	}

	return true;
}

bool BattleData::getUpMonsters(uint32 waveIndex,std::vector<std::string>& monsters)const{
	if(waveIndex >= upGroup_.size())
		return false;
	monsters = upGroup_[waveIndex];
	return true;
}
bool BattleData::getDownMonsters(uint32 waveIndex,std::vector<std::string>& monsters)const{
	if(waveIndex >= downGroup_.size())
		return false;
	monsters = downGroup_[waveIndex];
	return true;
}

void BattleData::clear()
{

}

std::vector<std::string> BattleData::parseMonsters(std::string strMonsters){
	std::vector<std::string> monsters;
	String::Split(strMonsters.c_str(),';',monsters);
	return monsters;
}

bool BattleData::load(const char* fn)
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
		BattleData *pbd = new BattleData();
		pbd->battleId_ = csv.get_int(row,"ID");

		for(int i=BP_Down0;i<=BP_Down9;++i){
			std::vector<std::string> monsters;
			monsters = parseMonsters(csv.get_string(row,ENUM(BattlePosition).getItemName(i)));
			for(size_t j=0;j<monsters.size(); ++j){
				if(pbd->downGroup_.size() <= j){
					std::vector<std::string> tmp;
					tmp.resize(BP_Max);
					pbd->downGroup_.push_back(tmp);
				}

				pbd->downGroup_[j][i] = monsters[j];
			}
		}

		for(int i=BP_Up0;i<=BP_Up9;++i){
			std::vector<std::string> monsters;
			monsters = parseMonsters(csv.get_string(row,ENUM(BattlePosition).getItemName(i)));
			for(size_t j=0;j<monsters.size(); ++j){
				if(pbd->upGroup_.size() <= j){
					std::vector<std::string> tmp;
					tmp.resize(BP_Max);
					pbd->upGroup_.push_back(tmp);
				}
			
				pbd->upGroup_[j][i] = monsters[j];
			}
		}

		std::string strType = csv.get_string(row,"BattleType");
		int tmp = ENUM(BattleType).getItemId(strType);
		pbd->battleType_ = (BattleType)tmp;

		data_.push_back(pbd);
	}

	return true;
}

bool BattleData::check()
{
	for(size_t i=0; i<data_.size(); ++i){
		if(!data_[i]->checkInside())
			return false;
	}

	return true;
}

BattleData const*
BattleData::getBattleDataById(S32 battleId)
{
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if(data_[i]->battleId_ == battleId)
			return data_[i];
	}

	return NULL;
}