
#include "skilltable.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"

std::map< S32 ,std::vector< SkillTable::Core* > >  SkillTable::data_;
 std::vector<std::pair<S32 ,S32> > SkillTable::babyEquipSkillGroup_;
std::string 
skillCompileScript(std::string &chunk, S32 skid, char const *fix)
{
	char id_str[128]={0};
	sprintf(id_str, "SK_%d_", skid);

	std::string func = id_str;				
	std::string err;

	func += fix;
	if(!ScriptEnv::loadScriptProc( chunk.c_str(), err, func.c_str()))
	{
		SRV_ASSERT(0);
	}
	return func;
}

bool SkillTable::load(char const *fn)
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
		Core * pCore = new Core;
		pCore->id_ = id;
		pCore->skillName_ = csv.get_string(row,"SkillName");
		pCore->level_ = csv.get_int(row,"Lv");
		pCore->exp_	= csv.get_int(row,"Pr");
		pCore->costHp_   = 0;
		pCore->costMana_ = csv.get_int(row,"cost");

		pCore->learnGroup_ = csv.get_int(row,"LearnGroup");
		pCore->learnpre_ = csv.get_int(row,"LearnQuestID");
		pCore->learnCoin_ = csv.get_int(row,"LearnCoin");
		pCore->learnLv_ = csv.get_int(row,"LearnLv");
		pCore->isPhysic_ = csv.get_bool(row, "isPhysic");
		pCore->isMelee_ = csv.get_bool(row,"isMelee");
		std::string sttmp = csv.get_string(row,"ImpactPropType");
		int resisttype = ENUM(PropertyType).getItemId(sttmp); 
		pCore->resistPropType_ = (PropertyType)resisttype;

		pCore->resistNum_ = csv.get_float(row, "ImpactPropNum");
		pCore->levelUpNeedItem_ = csv.get_int(row, "LevelUpNeedItem");
		std::string stt = csv.get_string(row,"SkillTargetType");
		int stte = ENUM(SkillTargetType).getItemId(stt);
		if(-1 == stte)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("Skill SkillTargetType error in row %d , id is %d\n"),row,id));
			SRV_ASSERT(0);	
		}

		pCore->targetType_ = (SkillTargetType)stte;

		stt = csv.get_string(row,"SkillType");
		stte = ENUM(SkillType).getItemId(stt);
		if(-1 == stte)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("Skill SkillType error in row %d , id is %d\n"),row,id));
			SRV_ASSERT(0);	
		}

		pCore->skType_ = (SkillType)stte;
		pCore->dropExp_ = csv.get_float(row,"exp");
		pCore->gloAction_ = csv.get_string(row,"GloAction");
		std::stringstream sstreamga;
		sstreamga << "GloAction_" << pCore->level_;
		pCore->gloAction_ = skillCompileScript(pCore->gloAction_,id,sstreamga.str().c_str());
		pCore->dontCare_ = csv.get_bool(row,"DontCare");
		pCore->gloCondition_ = csv.get_string(row,"GloCondition");
		std::stringstream sstreamgc;
		sstreamgc << "GloCondition_" << pCore->level_;
		if(!pCore->gloCondition_.empty())
		{
			std::string tmp(" return ");
			tmp += pCore->gloCondition_;
			pCore->gloCondition_ = skillCompileScript(tmp,id,sstreamgc.str().c_str());
		}

		std::string strTmp = csv.get_string(row,"Actions");
		char const * pChar = strTmp.c_str();
		std::string strToken;
		
		if(!strTmp.empty())
		{
			int idx = 0;
			while( TokenParser::getToken( pChar , strToken , ';'))
			{
				std::stringstream sstream;
				sstream << "Action_" << pCore->level_ << "_" << ++idx;
			
				strToken = skillCompileScript(strToken,id,sstream.str().c_str());
				pCore->actions_.push_back(strToken);
			}
			
		}

		strTmp = csv.get_string(row,"Conditions");
		pChar = strTmp.c_str();
		if(!strTmp.empty())
		{
			int idx = 0;
			while( TokenParser::getToken( pChar , strToken , ';'))
			{
				std::stringstream sstream;
				sstream <<"Confitions_"<< pCore->level_ << "_" << ++idx;

				if(!strToken.empty())
				{
					std::string tmp(" return ");
					tmp += strToken;
					strToken = skillCompileScript(tmp,id,sstream.str().c_str());
				}
				pCore->conditions_.push_back(strToken);
			}
		}

		if(pCore->actions_.size() != pCore->conditions_.size())
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("Skill actions & conditions can not match, id is %d\n"),id));
			SRV_ASSERT(0);	
		}
		if(data_[id].empty()){
			data_[id].resize(21);
		}

		data_[id][pCore->level_] = pCore;
	}

	return true;
}

bool SkillTable::loadBabyEquipSkillGroup(char const* fn){
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
		SRV_ASSERT(0);
	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);

	for(U32 row=0; row<csv.get_records_counter(); ++row){
		int32 skillId = csv.get_int(row,"SkillID");
		int32 skillLev = csv.get_int(row,"level");

		babyEquipSkillGroup_.push_back(std::pair<S32 ,S32>(skillId,skillLev));
	}
	return true;
}

bool
SkillTable::check()
{
	std::map< S32 , std::vector< Core* > >::iterator itr = data_.begin();
	while(itr != data_.end())
	{
		std::vector<Core*> tmp;
		tmp = itr->second;

		if(tmp.empty())
			return false;

		for (size_t i = 0; i < tmp.size(); ++i)
		{
			if(tmp[i] == NULL)
				continue;

			if(tmp[i]->level_ == 0)
			{
				ACE_DEBUG((LM_ERROR,ACE_TEXT("Skill level_ error skId_[%d]\n"),tmp[i]->id_));
				return false;
			}

			if(tmp[i]->skType_ == SKT_CannotUse && tmp[i]->resistPropType_ > PT_Max)
			{
				ACE_DEBUG((LM_ERROR,ACE_TEXT("Skill skType_ == SKT_CannotUse  tmp[i]->resistPropType_ > PT_Max  skId_[%d]\n"),tmp[i]->id_));
				return false;
			}
			if(tmp[i]->skType_ == SKT_CannotUse && tmp[i]->resistPropType_ < PT_None)
			{
				ACE_DEBUG((LM_ERROR,ACE_TEXT("Skill skType_ == SKT_CannotUse  tmp[i]->resistPropType_ <= PT_None  skId_[%d]\n"),tmp[i]->id_));
				return false;
			}

			for (size_t j = 0; j < tmp.size(); ++j)
			{
				if(tmp[j] == NULL)
					continue;

				if(tmp[i]->skType_ != tmp[j]->skType_)
				{
					ACE_DEBUG((LM_ERROR,ACE_TEXT("Skill skType_ different skId_[%d] -- skLv[%d]\n"),tmp[j]->id_,tmp[j]->level_));
					return false;
				}
				else if (tmp[i]->targetType_ != tmp[j]->targetType_)
				{
					ACE_DEBUG((LM_ERROR,ACE_TEXT("Skill targetType_ different skId_[%d] -- skLv[%d]\n"),tmp[j]->id_,tmp[j]->level_));
					return false;
				}
			}
		}

		++itr;
	}
	
	for(size_t i=0; i<babyEquipSkillGroup_.size();++i){
		SRV_ASSERT(getSkillById(babyEquipSkillGroup_[i].first,babyEquipSkillGroup_[i].second));
	}

	return true;
}

SkillTable::Core const*
SkillTable::getSkillById(S32 id,S32 skLevel)
{
	if(data_[id].empty())
		return NULL;
	else if(data_[id].size() <= skLevel)
		return NULL;

	return data_[id][skLevel];
}	

void SkillTable::randBabyEquipSkillInfo(int32 &skillId, int32 &skillLev){
	
	static int32 RAND_SKILL = babyEquipSkillGroup_.size() * 5;

	int32 i = UtlMath::randN(RAND_SKILL);
	if(i < babyEquipSkillGroup_.size()){
		skillId = babyEquipSkillGroup_[i].first;
		skillLev = babyEquipSkillGroup_[i].second;
	}
}