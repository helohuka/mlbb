#include "config.h"
#include "com.h"
#include "tmptable.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "monstertable.h"
#include "skilltable.h"
std::vector<PlayerTmpTable::Core> PlayerTmpTable::data_;
std::vector<ExpTable::Core*> ExpTable::data_;

bool
PlayerTmpTable::load(char const *fn)
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
		S32 id = csv.get_int(row,"ID");
		
		Core co;

		co.id_ = id;
		co.defaultSceneId_ =  csv.get_int(row,"DefaultSceneId");
		co.properties_.resize(PT_Max);

		co.properties_[PT_TableId] = (float)id;
		co.properties_[PT_Level] = csv.get_float(row,"PT_Level");
		co.properties_[PT_Exp] = csv.get_float(row,"PT_Exp");
		co.properties_[PT_Reputation] = csv.get_float(row,"PT_Reputation");
		co.properties_[PT_Stama] = csv.get_float(row,"PT_Stama");
		co.properties_[PT_Strength] = csv.get_float(row,"PT_Strength");
		co.properties_[PT_Power] = csv.get_float(row,"PT_Power");
		co.properties_[PT_Speed] = csv.get_float(row,"PT_Speed");
		co.properties_[PT_Magic] = csv.get_float(row,"PT_Magic");
		co.properties_[PT_AssetId] = csv.get_float(row,"PT_AssetId");

		co.properties_[PT_HpMax] = csv.get_float(row,"PT_Hp");
		co.properties_[PT_MpMax] = csv.get_float(row,"PT_Mp");
		co.properties_[PT_Attack] = csv.get_float(row,"PT_Attack");
		co.properties_[PT_Defense] = csv.get_float(row,"PT_Defense");
		co.properties_[PT_Agile] = csv.get_float(row,"PT_Agile");
		co.properties_[PT_Free] = 0;
		co.properties_[PT_BagNum] = csv.get_float(row,"PT_BagNum");
		co.properties_[PT_SneakAttack] = csv.get_int(row,"PT_SneakAttack");

		std::string strJt = csv.get_string(row,"JobType");
		int jt = ENUM(JobType).getItemId(strJt);
		co.properties_[PT_Profession] = jt;

		std::string tmp = csv.get_string(row,"Race");
		int racetype = ENUM(RaceType).getItemId(tmp); 
		co.properties_[PT_Race] = racetype;
		
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
				co.defaultSkill_.push_back(one);
			}
		}

		std::string strDefaultBaby = csv.get_string(row,"DefalutBabyId");
		
		if(!strDefaultBaby.empty())
		{
			char const * pChar = strDefaultBaby.c_str();
			std::string strToken;
			while( TokenParser::getToken( pChar , strToken , ';'))
			{
				U32 babyId= atoi(strToken.c_str());
				co.defaultBaby_.push_back(babyId);
			}
		}

		data_.push_back(co);
	}

	return true;
}

bool
PlayerTmpTable::check()
{
	for (size_t i=0; i<data_.size(); ++i)
	{
		for (size_t j=0; j<data_[i].defaultBaby_.size();++j)
		{
			if(NULL == MonsterTable::getMonsterById(data_[i].defaultBaby_[j])){
				ACE_DEBUG((LM_ERROR,"Monster table can not find %d\n",data_[i].defaultBaby_[j]));
				return false;
			}
		}
		for (size_t j=0; j<data_[i].defaultSkill_.size();++j)
		{
			if(NULL ==  SkillTable::getSkillById(data_[i].defaultSkill_[j].first,data_[i].defaultSkill_[j].second))
			{
				ACE_DEBUG((LM_ERROR,"Skill table can not find %d\n",data_[i].defaultSkill_[j]));
				return false;
			}
		}
	}
	return true;
}

PlayerTmpTable::Core const *
PlayerTmpTable::getTemplateById(U32 id)
{
	for (size_t i=0; i<data_.size(); ++i)
	{
		if(id == data_[i].id_)
			return &data_[i];
	}

	return NULL;
}

///------------------------

bool
ExpTable::load(char const *fn)
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
		Core* pCore = new Core;

		pCore->level_		= csv.get_int(row,"Lv");
		pCore->playerExp_	= csv.get_float(row,"Exp");
		pCore->babyExp_		= csv.get_float(row,"PetExp");

		data_.push_back(pCore);
	}

	return true;
}

bool
ExpTable::check()
{
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if (data_[i]->level_ == 0)
		{
			ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading ExpTable Error-----level[%d]\n"),data_[i]->level_) );
			return false;
		}

		if (data_[i]->playerExp_ == 0)
		{
			ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading ExpTable Error-----playerExp_[%d]\n"),data_[i]->playerExp_) );
			return false;
		}

		if (data_[i]->babyExp_ == 0)
		{
			ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading ExpTable Error-----babyExp_[%d]\n"),data_[i]->babyExp_) );
			return false;
		}
			
	}
	return true;
}

ExpTable::Core const *
ExpTable::getTemplateById(U32 lv)
{
	for (size_t i=0; i<data_.size(); ++i)
	{
		if(lv == data_[i]->level_)
			return data_[i];
	}

	return NULL;
}