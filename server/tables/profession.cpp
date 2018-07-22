#include "profession.h"
#include "CSVParser.h"
#include "TokenParser.h"

std::vector<Profession*> Profession::professions_;

void Profession::clear()
{
	for (size_t i=0; i<professions_.size(); ++i)
	{
		if(professions_[i])
			delete professions_[i];
	}

	professions_.clear();
}

bool Profession::load(const char* fn)
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
		Profession* p = new Profession();

		p->id_ = csv.get_int(row,"ID");
		
		{
			std::string enumstr = csv.get_string(row,"JobType");
			if(enumstr.empty())
				SRV_ASSERT(0);
			S32 enumint = ENUM(JobType).getItemId(enumstr);
			if(-1 == enumint)
				SRV_ASSERT(0);
			p->jobtype_ = (JobType)enumint;
			p->joblevel_= csv.get_int(row,"JobLv");
		}

		{
			std::string str1 = csv.get_string(row,"EquipType");
			const char* cstr1 = str1.c_str();
			std::string strtoken1;
			if(!str1.empty())
			{
				while(TokenParser::getToken( cstr1 , strtoken1 , ';'))
				{
					const char* cstr2 = strtoken1.c_str();
					std::string strtoken2;
					TokenParser::getToken( cstr2 , strtoken2 , ':');
					S32 enumint = ENUM(ItemSubType).getItemId(strtoken2);
					if(-1 == enumint)
						SRV_ASSERT(0);
					
					TokenParser::getToken( cstr2 , strtoken2 , ':');
					S32 level = ACE_OS::atoi(strtoken2.c_str());
					std::pair<ItemSubType,S32> one((ItemSubType)enumint,level);
					p->canuseitem_.push_back(one);
				}
			}
		}

		{
			std::string str1 = csv.get_string(row,"SkillGroup");
			const char* cstr1 = str1.c_str();
			std::string strtoken1;
			if(!str1.empty())
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
					p->canuseskill_.push_back(one);
				}
			}
		}

		{
			std::string str = csv.get_string(row,"ProudSkillGroup");
			const char* cstr = str.c_str();
			std::string strtoken;
			if(!str.empty())
			{
				while(TokenParser::getToken( cstr , strtoken , ';'))
				{
					p->proudskillgroup_.push_back(ACE_OS::atoi(strtoken.c_str()));
				}
			}

		}

		{
			std::string str1 = csv.get_string(row,"Recommand");
			const char* cstr1 = str1.c_str();
			std::string strtoken1;
			if(!str1.empty())
			{
				while(TokenParser::getToken( cstr1 , strtoken1 , ';'))
				{
					const char* cstr2 = strtoken1.c_str();
					std::string strtoken2;
					TokenParser::getToken( cstr2 , strtoken2 , ':');
					S32 pf = ENUM(PropertyType).getItemId(strtoken2);
					if(pf == -1)
						SRV_ASSERT(0);
					PropertyType profp = (PropertyType)pf;
					TokenParser::getToken( cstr2 , strtoken2 , ':');
					S32 prop = ACE_OS::atoi(strtoken2.c_str());
					std::pair<PropertyType,S32> one(profp,prop);
					p->profprop_.push_back(one);
				}
			}
		}

		professions_.push_back(p);
		
	}
	return true;
}

bool Profession::check()
{
	return true;
}

const Profession* Profession::get(JobType type, S32 level)
{
	for (size_t i=0; i<professions_.size(); ++i)
	{
		if(professions_[i]->jobtype_ == type && professions_[i]->joblevel_ == level)
			return professions_[i];
	}
	return NULL;
}

bool Profession::canUseItem(ItemSubType type, S32 lev)const
{
	for (size_t i=0; i<canuseitem_.size(); ++i)
	{
		if(canuseitem_[i].first == type && canuseitem_[i].second >= lev)
			return true;
	}

	return false;
}

bool Profession::canUseSkill(S32 group, S32 sklev)const
{
	for (size_t i=0; i<canuseskill_.size(); ++i)
	{
		if(canuseskill_[i].first == group && canuseskill_[i].second >= sklev)
			return true;
	}

	return false;
}

bool Profession::canLearnSkill(S32 group)const
{
	for (size_t i=0; i<canuseskill_.size(); ++i)
	{
		if(canuseskill_[i].first == group)
			return true;
	}

	return false;
}

S32  Profession::getSkillMaxLevel(S32 skid)const
{
	S32 max = 0;
	for (size_t i=0; i<canuseskill_.size(); ++i)
	{
		if(canuseskill_[i].first == skid && canuseskill_[i].second>max)
			max = canuseskill_[i].second;
	}

	return max;
}

S32  Profession::getItemMaxLevel(ItemSubType type)const
{

	S32 max = 0;
	for (size_t i=0; i<canuseitem_.size(); ++i)
	{
		if(canuseitem_[i].first == type && canuseitem_[i].second>max)
			max = canuseitem_[i].second;
	}

	return max;
}