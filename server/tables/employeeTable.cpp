#include "employeeTable.h"
#include "itemtable.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"
#include "skilltable.h"

std::map< S32 , EmployeeTable::EmployeeData* >  EmployeeTable::data_;

bool EmployeeTable::load(char const *fn)
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
			ACE_DEBUG((LM_ERROR,ACE_TEXT("BuddyTable  has same id in row %d , id is %d\n"),row,id));
			SRV_ASSERT(0);	
		}

		EmployeeData * pCore = new EmployeeData ;
		pCore->Id_ = id;
	
		pCore->name_ = csv.get_string(row,"Name");

		pCore->properties_.resize(PT_Max);
		pCore->grows_.resize(QC_Max);
		pCore->properties_[PT_TableId] = id;
		pCore->properties_[PT_Stama] = csv.get_float(row,"PT_Stama");
		pCore->properties_[PT_Strength] = csv.get_float(row,"PT_Strength");
		pCore->properties_[PT_Power] = csv.get_float(row,"PT_Power");
		pCore->properties_[PT_Speed] = csv.get_float(row,"PT_Speed");
		pCore->properties_[PT_Magic] = csv.get_float(row,"PT_Magic");
		pCore->properties_[PT_HpMax] = csv.get_float(row,"PT_Hp");
		pCore->properties_[PT_MpMax] = csv.get_float(row,"PT_Mp");
		pCore->properties_[PT_Attack] = csv.get_float(row,"PT_Attack");
		pCore->properties_[PT_Defense] = csv.get_float(row,"PT_Defense");
		pCore->properties_[PT_Agile] = csv.get_float(row,"PT_Agile");
		pCore->properties_[PT_AssetId] = csv.get_float(row,"AssetID");
		
		pCore->AIClassName_ = csv.get_string(row, "AI_class");

		std::string strRace = csv.get_string(row,"Race");
		int tmp = ENUM(RaceType).getItemId(strRace); 
		pCore->race_  = (RaceType)tmp;

		std::string strJob = csv.get_string(row,"JobType");
		int jobTmp = ENUM(JobType).getItemId(strJob); 
		pCore->Job_ = (JobType)jobTmp;
		pCore->properties_[PT_Profession] = jobTmp;
	
		std::string strQuality = csv.get_string(row,"Quality");
		int tmp1 = ENUM(QualityColor).getItemId(strQuality); 
		if(-1 == tmp1)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("employeeTable Quality error in row %d , id is %d\n"),row,id));
			SRV_ASSERT(0);	
		}
		pCore->quality_  = (QualityColor)tmp1;

		pCore->star_ = csv.get_int(row,"InitialStar");
		pCore ->JobLevel_ = csv.get_int(row,"JobLv");
		pCore->properties_[PT_ProfessionLevel] = pCore ->JobLevel_ ;

		std::string strEvo = csv.get_string(row, "EvolutionNum");
		if(!strEvo.empty())
		{
			char const * pChar = strEvo.c_str();
			std::string strToken;
			while( TokenParser::getToken( pChar , strToken , ';'))
			{
				U32 uNum= atoi(strToken.c_str());
				pCore->evolutionNum_.push_back(uNum);
			}
		}

		std::string strDefaultSkill = csv.get_string(row,"DefalutSkill");
		const char* cstr1 = strDefaultSkill.c_str();
		std::string strtoken1;
		if(!strDefaultSkill.empty())
		{
			while(TokenParser::getToken( cstr1 , strtoken1 , ';'))
			{
				const char* cstr2 = strtoken1.c_str();
				std::string strtoken2;
				TokenParser::getToken( cstr2 , strtoken2 , ':');
				S32 group = ACE_OS::atoi(strtoken2.c_str());

				TokenParser::getToken( cstr2 , strtoken2 , ':');
				S32 level = ACE_OS::atoi(strtoken2.c_str());
				std::pair<S32,S32> one(group,level);
				pCore->defaultSkill_.push_back(one);
			}
		}

		std::string strsklv = csv.get_string(row, "Skill_levelup");
		if(!strsklv.empty())
		{
			char const * pChar = strsklv.c_str();
			std::string strToken;
			while( TokenParser::getToken( pChar , strToken , ';'))
			{
				U32 uNum= atoi(strToken.c_str());
				pCore->skillneed_.push_back(uNum);
			}
		}

		pCore->grows_[QC_White] = csv.get_float(row,"Grow_White");
		pCore->grows_[QC_Green] = csv.get_float(row,"Grow_Green");
		pCore->grows_[QC_Blue] = csv.get_float(row,"Grow_Blue");
		pCore->grows_[QC_Blue1] = csv.get_float(row,"Grow_Blue1");
		pCore->grows_[QC_Purple] = csv.get_float(row,"Grow_Purple");
		pCore->grows_[QC_Purple1] = csv.get_float(row,"Grow_Purple1");
		pCore->grows_[QC_Purple2] = csv.get_float(row,"Grow_Purple2");
		pCore->grows_[QC_Golden] = csv.get_float(row,"Grow_Golden");
		pCore->grows_[QC_Golden1] = csv.get_float(row,"Grow_Golden1");
		pCore->grows_[QC_Golden2] = csv.get_float(row,"Grow_Golden2");
		pCore->grows_[QC_Orange] = csv.get_float(row,"Grow_Orange");
		pCore->grows_[QC_Orange1] = csv.get_float(row,"Grow_Orange1");
		pCore->grows_[QC_Orange2] = csv.get_float(row,"Grow_Orange2");
		pCore->grows_[QC_Pink] = csv.get_float(row,"Grow_Pink");
		pCore->soul_ = csv.get_int(row,"AST");
		data_[id] = pCore;
	}

	return true;
}

bool EmployeeTable::check()
{	
	 std::map<S32, EmployeeData*>::iterator  itor = data_.begin();
	 while(itor != data_.end())
	 {
		 EmployeeData* pEmp = itor->second;

		 if (!pEmp)
		 {
			 ACE_DEBUG((LM_ERROR,ACE_TEXT("EmployeeTable not Employee item error\n")));
			 return false;
		 }

		 if(pEmp->quality_ < QC_White)
		 {
			ACE_DEBUG((LM_ERROR,ACE_TEXT("EmployeeTable TableId[%d] quality_ < QC_White \n"),pEmp->Id_));
			return false;
		 }

		 for(size_t i=0; i<pEmp->defaultSkill_.size(); ++i){
			 if(NULL == SkillTable::getSkillById(pEmp->defaultSkill_[i].first,pEmp->defaultSkill_[i].second)){
				ACE_DEBUG((LM_ERROR,"Can not find skill id %d at employee %d\n",pEmp->defaultSkill_[i],pEmp->Id_));
				return false;
			 }
		 }

		  if(pEmp->skillneed_.size() < 1)
			  return false;

		 for (size_t i = 0; i < pEmp->skillneed_.size(); ++i)
		 {
			if(pEmp->skillneed_[i] <= 0)
				return false;
		 }

		 itor++;
	 }
	return true;
}

EmployeeTable::EmployeeData const* EmployeeTable::getEmployeeById(U32 id)
{
	return data_[id];
}

U32
EmployeeTable::getSkillLevelUpNeed(U32 id,U32 sklv)
{
	EmployeeData* pEmp = data_[id];
	if(pEmp == NULL)
		return 0;
	if(sklv > pEmp->skillneed_.size())
		return 0;
	if(sklv < 1)
		return 0;
	return pEmp->skillneed_[sklv-1];
}
