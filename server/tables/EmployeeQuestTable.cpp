#include "config.h"
#include "CSVParser.h"
#include "itemtable.h"
#include "EmployeeQuestTable.h"
std::map<int32,std::vector<int32> > EmployeeSkill::data_;

bool EmployeeSkill::load(char const* fn){
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

	for(size_t row=0; row<csv.get_records_counter(); ++row){
		int32 skillId = csv.get_int(row,"SkillID");
		
		SRV_ASSERT(data_.find(skillId)==data_.end());
		
		data_[skillId].resize(EKT_Max);
		for (int32 ekt=EKT_GroupDamage; ekt < EKT_Max; ++ekt){
			int32 n = csv.get_int(row,ENUM(EmployeeSkillType).getItemName(ekt));
			data_[skillId][ekt] = n;
		}
	}

	return true;
}

int32 EmployeeSkill::getRestrain(int32 skillId, EmployeeSkillType type){
	if(data_.find(skillId) == data_.end())
		return 0;
	if(type < 0 || type >= data_[skillId].size() ){
		return 0;
	}
	return data_[skillId][type];
}


std::map<int32, EmployeeMonster*> EmployeeMonster::data_;

bool EmployeeMonster::load(char const* fn){
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

	for(size_t row=0; row<csv.get_records_counter(); ++row){
		int32 id = csv.get_int(row,"ID");
		SRV_ASSERT(data_.find(id)==data_.end());
		
		EmployeeMonster *p = new EmployeeMonster();
		
		SRV_ASSERT(p);

		p->id_ = id;
		std::string strSkill = csv.get_string(row,"Skill");
		if(!strSkill.empty()){
			std::vector<std::string> vStrSkill = String::Split(strSkill,";");
			for(size_t i=0; i<vStrSkill.size(); ++i){
				if(vStrSkill[i].empty())
					continue;
				int32 iType = ENUM(EmployeeSkillType).getItemId(vStrSkill[i]);
				SRV_ASSERT(iType!=-1);
				SRV_ASSERT(iType >= EKT_GroupDamage);
				SRV_ASSERT(iType < EKT_Max);
				p->skills_.push_back((EmployeeSkillType)iType);
			}
		}

		data_[id] = p;
	}
	
	return true;
}

EmployeeMonster const* EmployeeMonster::find(int32 id){
	return data_[id];
}

std::vector< std::vector<int32> > EmployeeQuest::colors_;
std::map<int32, EmployeeQuest*> EmployeeQuest::data_;

bool EmployeeQuest::load(char const* fn){
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
	
	colors_.resize(EQC_Max);
	for(size_t row=0; row<csv.get_records_counter(); ++row){
		int32 id = csv.get_int(row,"EQID");
		SRV_ASSERT(data_.find(id)==data_.end());
		
		EmployeeQuest*p = new EmployeeQuest();
		p->id_ = id;
		
		{
			std::string strMonsters = csv.get_string(row,"EmployeeMonster");
			if(!strMonsters.empty()){
				std::vector<std::string> vStrMonsters = String::Split(strMonsters,";");
				for(size_t i=0; i<vStrMonsters.size(); ++i){
					if(vStrMonsters[i].empty()){
						continue;
					}
					p->monsters_.push_back(atoi(vStrMonsters[i].c_str()));
				}
			}
			SRV_ASSERT(!(p->monsters_.empty()));
		}

		{
			std::string strEmployeeQuestColor = csv.get_string(row,"EmployeeQuestColor");
			SRV_ASSERT(!strEmployeeQuestColor.empty());
			int32 iEmployeeQuestColor = ENUM(EmployeeQuestColor).getItemId(strEmployeeQuestColor);
			SRV_ASSERT(iEmployeeQuestColor != -1);
			SRV_ASSERT(iEmployeeQuestColor >= EQC_White);
			SRV_ASSERT(iEmployeeQuestColor < EQC_Max);
			p->color_ = (EmployeeQuestColor)iEmployeeQuestColor;
		}

		p->timeRequier_ = csv.get_float(row,"TimeRequier") * 60 * 60;
		SRV_ASSERT(p->timeRequier_ > 0);
		p->employeeRequier_ = csv.get_int(row,"EmployeeRequier");
		SRV_ASSERT(p->employeeRequier_ > 0);
		p->successRate_ = csv.get_int(row,"SuccessRate");
		SRV_ASSERT(p->successRate_ >= 0);
		{
			int32 itemId = csv.get_int(row,"award1");
			if(itemId != 0){
				p->rewards_.push_back(itemId);
			}
		}
		{
			int32 itemId = csv.get_int(row,"award2");
			if(itemId != 0){
				p->rewards_.push_back(itemId);
			}
		}
		{
			int32 itemId = csv.get_int(row,"award3");
			if(itemId != 0){
				p->rewards_.push_back(itemId);
			}
		}
		SRV_ASSERT(!(p->rewards_.empty()));
		p->cost_ = csv.get_int(row,"cost");
		data_[id] = p;
		colors_[p->color_].push_back(id);
	}
	return true;
}

bool EmployeeQuest::check(){
	for(std::map<int32, EmployeeQuest*>::iterator i=data_.begin(); i!=data_.end(); ++i){
		for(size_t j=0; j<i->second->monsters_.size(); ++j){
			SRV_ASSERT(EmployeeMonster::find(i->second->monsters_[j]));
		}
		
		for(size_t j=0; j<i->second->rewards_.size(); ++j){
			SRV_ASSERT(ItemTable::getItemById(i->second->rewards_[j]));
		}
	}
	return true;
}