#include "monstertable.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"
#include "skilltable.h"
#include "itemtable.h"

std::map< S32 , MonsterTable::MonsterData* >  MonsterTable::data_;

bool MonsterTable::sortBabyFunction::operator()(struct COM_BabyInst* r, struct COM_BabyInst* l)
{
	if(NULL == l && NULL == r)
		return false;
	if(NULL == l)
		return true;
	if(NULL == r)
		return false;

	if(r->properties_[PT_Race] > l->properties_[PT_Race])
		return true;

	return false;
}


std::string 
inline compileScript(std::string &chunk, S32 skid, char const *fix)
{
	char id_str[128]={0};
	sprintf(id_str, "MS_%d_", skid);

	std::string func = id_str;				
	std::string err;

	func += fix;
	if(!ScriptEnv::loadScriptProc( chunk.c_str(), err, func.c_str()))
	{
		SRV_ASSERT(0);
	}
	return func;
}

bool
MonsterTable::load(char const *fn)
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
		S32 id = csv.get_int(row,"ID");

		if(data_[id] != NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("monster has same id in row %d , id is %d\n"),row,id));
			SRV_ASSERT(0);	
		}

		MonsterData* pmonster = new MonsterData;

		pmonster->monsterId_ = id;
		pmonster->monsterLevel_ = csv.get_int(row, "monsterLevel");

		pmonster->assetId_ = csv.get_int(row,"AssetsID");
		pmonster->name_ = csv.get_string(row, "Name");
		pmonster->monsterType_ = csv.get_int(row,"Type");
		pmonster->twiceAction_ = csv.get_int(row,"TwiceAction");
		
		pmonster->pet_ = csv.get_bool(row,"Pet");
		pmonster->petProbability_ = csv.get_int(row,"PetProbability");
		std::string stt = csv.get_string(row,"PetQuality");
		int resist = ENUM(PetQuality).getItemId(stt);
		pmonster->petQuality_ = (PetQuality)resist;

		std::string sttmp = csv.get_string(row,"Race");
		int resisttype = ENUM(RaceType).getItemId(sttmp); 
		pmonster->race_ = (RaceType)resisttype;

		pmonster->gearProps_.resize(BIG_Max);
		pmonster->gearProps_[BIG_Stama] = csv.get_int(row,"BIG_Stama");
		pmonster->gearProps_[BIG_Strength] = csv.get_int(row,"BIG_Strength");
		pmonster->gearProps_[BIG_Power] = csv.get_int(row,"BIG_Power");
		pmonster->gearProps_[BIG_Speed] = csv.get_int(row,"BIG_Speed");
		pmonster->gearProps_[BIG_Magic] = csv.get_int(row,"BIG_Magic");

		pmonster->properties_.resize(PT_Max);
		pmonster->properties_[PT_HpMax] = csv.get_float(row,"PT_Hp");
		pmonster->properties_[PT_MpMax] = csv.get_float(row,"PT_Mp");
		pmonster->properties_[PT_Attack] = csv.get_float(row,"PT_Attack");
		pmonster->properties_[PT_Defense] = csv.get_float(row,"PT_Defense");
		pmonster->properties_[PT_Agile] = csv.get_float(row,"PT_Agile");
		pmonster->properties_[PT_Fire] = csv.get_float(row,"Fire");
		pmonster->properties_[PT_Water] = csv.get_float(row,"Water");
		pmonster->properties_[PT_Land] = csv.get_float(row,"Ground");
		pmonster->properties_[PT_Wind] = csv.get_float(row,"Wind");
		pmonster->bindType_ = (BindType)ENUM(BindType).getItemId(csv.get_string(row,"BindType"));
		pmonster->skillNum_ = csv.get_int(row,"SkillNum");

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
				pmonster->defaultSkill_.push_back(one);
			}
		}

		pmonster->monsterClass_ = csv.get_string(row,"monster_class");
		pmonster->monsterClass_ = compileScript(pmonster->monsterClass_,id,"monster_class");

		pmonster->refromMonster_ = csv.get_int(row,"ReformMonster");
		
		std::string strItem = csv.get_string(row,"ReformItem");
		if(!strItem.empty())
		{
			char const * pChar = strItem.c_str();
			std::string strToken;
			while( TokenParser::getToken( pChar , strToken , ';'))
			{
				U32 itemId= atoi(strToken.c_str());
				pmonster->refrommItem_.push_back(itemId);
			}
		}

		data_[id] = pmonster;
	}
	return true;
}

bool
MonsterTable::check()
{
	std::map< S32 , MonsterData*>::iterator itr = data_.begin();
	
	while(itr != data_.end())
	{
		MonsterData* p = itr->second;
		if(p == NULL)
		{
			ACE_DEBUG((LM_ERROR,"MonsterTable table can not find\n"));
			SRV_ASSERT(0);
		}

		for (size_t j=0; j<p->defaultSkill_.size();++j)
		{
			if(p->defaultSkill_[j].first == 0)
			{
				ACE_DEBUG((LM_ERROR,"Monster[%d] SkId error\n",p->monsterId_));
				SRV_ASSERT(0);
			}
			else if (p->defaultSkill_[j].second == 0)
			{
				ACE_DEBUG((LM_ERROR,"Monster[%d] Skilllevel error skid[%d]\n",p->monsterId_,p->defaultSkill_[j].first));
				SRV_ASSERT(0);
			}

			if(NULL ==  SkillTable::getSkillById(p->defaultSkill_[j].first,p->defaultSkill_[j].second))
			{
				ACE_DEBUG((LM_ERROR,"Monster[%d] Skill table can not find skid[%d]---skLv[%d]\n",p->monsterId_,p->defaultSkill_[j].first,p->defaultSkill_[j].second));
				SRV_ASSERT(0);
			}
		}

		if(p->twiceAction_ == 0)
		{
			ACE_DEBUG((LM_ERROR,"Monster[%d] twiceAction_ is 0 \n",p->monsterId_));
			SRV_ASSERT(0);
		}

		if(p->refromMonster_ != 0 && p->refrommItem_.empty())
		{
			ACE_DEBUG((LM_ERROR,"Monster[%d] refromMonster_ not 0 but refrommItem_ is null\n",p->monsterId_));
			SRV_ASSERT(0);
		}

		if(p->refromMonster_ == 0 && !p->refrommItem_.empty())
		{
			ACE_DEBUG((LM_ERROR,"Monster[%d] refromMonster_ == 0 but refrommItem_ not null\n",p->monsterId_));
			SRV_ASSERT(0);
		}

		if(!p->refrommItem_.empty())
		{
			for (size_t i = 0; i < p->refrommItem_.size(); ++i)
			{
				ItemTable::ItemData const* item = ItemTable::getItemById(p->refrommItem_[i]);
				if(item == NULL)
				{
					ACE_DEBUG((LM_ERROR,"Monster[%d] refrommItem_[%d] not found in the itemTable\n",p->monsterId_,p->refrommItem_[i]));
					SRV_ASSERT(0);
				}
			}
		}

		if(p->race_ < RT_None || p->race_ > RT_Max)
			SRV_ASSERT(0);

		++itr;
	}
	return true;
}

MonsterTable::MonsterData const*
MonsterTable::getMonsterById(U32 id)
{
	return data_[id];
}

void
static randInitDelta(std::vector<S8> &out)
{
	std::vector<std::pair<S32, S32> > wi;
	for(S32 i=BIG_Stama; i<BIG_Max; ++i)
	{
		std::pair<int ,int> pair(i,1);
		wi.push_back(pair);
	}

	out.resize(BIG_Max);
	for(S8 i=0; i<10; ++i)
	{
		S32 gearId = UtlMath::randWeight(wi);
		++out[gearId];
	}
}

void
MonsterTable::genMonsterPropFromTable(U32 monsterID, COM_MonsterInst& out)
{
	MonsterTable::MonsterData const * tmp = MonsterTable::getMonsterById(monsterID);
	if(NULL == tmp)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find %d monster form monster-table\n"),monsterID));
		return ;
	}

	out.instName_ = tmp->name_;

	out.gear_.resize(BIG_Max);
	out.gear_[BIG_Stama] = CALC_BABY_GEAR(tmp->gearProps_[BIG_Stama]);
	out.gear_[BIG_Strength] = CALC_BABY_GEAR(tmp->gearProps_[BIG_Strength]);
	out.gear_[BIG_Power] = CALC_BABY_GEAR(tmp->gearProps_[BIG_Power]);
	out.gear_[BIG_Speed] = CALC_BABY_GEAR(tmp->gearProps_[BIG_Speed]);
	out.gear_[BIG_Magic] = CALC_BABY_GEAR(tmp->gearProps_[BIG_Magic]);

	out.properties_.resize(PT_Max);
	out.properties_[PT_Stama] = CALC_BABY_BASE(out.gear_[BIG_Stama],tmp->monsterLevel_,0.042);
	out.properties_[PT_Strength] = CALC_BABY_BASE(out.gear_[BIG_Strength],tmp->monsterLevel_,0.042);
	out.properties_[PT_Power] = CALC_BABY_BASE(out.gear_[BIG_Power],tmp->monsterLevel_,0.042);
	out.properties_[PT_Speed] = CALC_BABY_BASE(out.gear_[BIG_Speed],tmp->monsterLevel_,0.042);
	out.properties_[PT_Magic] = CALC_BABY_BASE(out.gear_[BIG_Magic],tmp->monsterLevel_,0.042);

	CALC_BABY_PRO_TRANS_STAMA(out,out.properties_[PT_Stama]);
	CALC_BABY_PRO_TRANS_STRENGTH(out,out.properties_[PT_Strength]);
	CALC_BABY_PRO_TRANS_POWER(out,out.properties_[PT_Power]);
	CALC_BABY_PRO_TRANS_SPEED(out,out.properties_[PT_Speed]);
	CALC_BABY_PRO_TRANS_MAGIC(out,out.properties_[PT_Magic]);

	out.properties_[PT_TableId] = tmp->monsterId_;
	out.properties_[PT_AssetId] = tmp->assetId_;

	out.properties_[PT_HpCurr] = out.properties_[PT_HpMax];
	out.properties_[PT_MpCurr] = out.properties_[PT_MpMax];

	for (size_t i = 0; i < tmp->defaultSkill_.size(); ++i)
	{
		COM_Skill skill;
		skill.skillID_ = tmp->defaultSkill_[i].first;
		skill.skillExp_ = 0;
		skill.skillLevel_ = tmp->defaultSkill_[i].second;

		out.skill_.push_back(skill);
	}	
}


///////////////////////////////////////////////////////////////////////////////

std::map< std::string, MonsterClass::Core* > MonsterClass::classes_;

bool 
MonsterClass::load(char const *fn)
{
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);
	
	clear();

	for(U32 row=0; row<csv.get_records_counter(); ++row)
	{
		Core * pCore = new Core;
		pCore->className_ = csv.get_string(row,"Name");
		pCore->monsterId_ = csv.get_int(row,"monsterID");
		pCore->level_	  = csv.get_int(row,"level");
		pCore->dropId_	  = csv.get_int(row,"DropID");
		pCore->isAddValue_  = csv.get_bool(row,"Addedvalue");

		pCore->events_.resize(ME_Max);
		for (size_t me = ME_Born; me < ME_Max; ++me)
		{	
			if(ENUM(AIEvent).getItemName(me) == NULL)
			{
				ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find %s form AIClass-table\n"),ENUM(AIEvent).getItemName(me)));
				continue;
			}
			
			std::string script;

			script = csv.get_string(row,ENUM(AIEvent).getItemName(me));
			
			std::string funcName("M_");
			funcName += pCore->className_;
			funcName += "_";
			funcName += ENUM(AIEvent).getItemName(me);

			std::string err;
			if(!ScriptEnv::loadScriptProc( script.c_str() , err, funcName.c_str() ))
			{
				ACE_DEBUG( (LM_ERROR, ACE_TEXT("Error when compiling \"ENTER\" script err = \"%s\"\n"), err.c_str()));
				SRV_ASSERT(0);
			}
			pCore->events_[(AIEvent)me] = funcName;
		}

		pCore->properties_.resize(PT_Max);
		pCore->properties_[PT_HpMax] = csv.get_float(row,"PT_HpMax");
		pCore->properties_[PT_MpMax] = csv.get_float(row,"PT_MpMax");
		pCore->properties_[PT_Attack] = csv.get_float(row,"PT_Attack");
		pCore->properties_[PT_Defense] = csv.get_float(row,"PT_Defense");
		pCore->properties_[PT_Agile] = csv.get_float(row,"PT_Agile");
		pCore->properties_[PT_Spirit] = csv.get_float(row,"PT_Spirit");
		pCore->properties_[PT_Reply] = csv.get_float(row,"PT_Reply");
		pCore->properties_[PT_Magicattack] = csv.get_float(row,"PT_Magicattack");
		pCore->properties_[PT_Magicdefense] = csv.get_float(row,"PT_Magicdefense");
		pCore->properties_[PT_Hit] = csv.get_float(row,"PT_Hit");
		pCore->properties_[PT_Dodge] = csv.get_float(row,"PT_Dodge");
		pCore->properties_[PT_Crit] = csv.get_float(row,"PT_Crit");
		pCore->properties_[PT_counterpunch] = csv.get_float(row,"PT_counterpunch");
		pCore->properties_[PT_NoSleep] = csv.get_float(row,"PT_NoSleep");
		pCore->properties_[PT_NoPetrifaction] = csv.get_float(row,"PT_NoPetrifaction");
		pCore->properties_[PT_NoDrunk] = csv.get_float(row,"PT_NoDrunk");
		pCore->properties_[PT_NoChaos] = csv.get_float(row,"PT_NoChaos");
		pCore->properties_[PT_NoForget] = csv.get_float(row,"PT_NoForget");
		pCore->properties_[PT_NoPoison] = csv.get_float(row,"PT_NoPoison");
		pCore->properties_[PT_Assassinate] = csv.get_float(row,"PT_Assassinate");

		classes_.insert(std::pair<std::string, Core*>(pCore->className_,pCore));
	}

	return true;
}

void MonsterClass::clear(){
	for(std::map< std::string, Core* >::iterator itr = classes_.begin(); itr!=classes_.end(); ++itr)
	{
		if(itr->second)
			delete itr->second;
	}
	classes_.clear();
}

bool 
MonsterClass::check()
{
	for(std::map< std::string, Core* >::iterator itr = classes_.begin(); itr!=classes_.end(); ++itr)
	{
		if(0 == itr->second->monsterId_)
			continue; ///可以填0 
		if(NULL == MonsterTable::getMonsterById(itr->second->monsterId_)){
			ACE_DEBUG((LM_ERROR,"Can not find monster %d in monster table \n",itr->second->monsterId_));
			return false;
		}
	}
	return true;
}

MonsterClass::Core const * 
MonsterClass::getClassByName(std::string const& name)
{
	return classes_[name];
}

// ----------------------------------------------------------- [5/6/2016 lwh]
std::map< U32, PetIntensive::Core* > PetIntensive::data_;

bool
PetIntensive::load(char const *fn)
{
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);

	for(U32 row=0; row<csv.get_records_counter(); ++row)
	{
		U32 id = csv.get_int(row,"Lv");

		if(data_[id] != NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("PetIntensive has same Lv in row %d , LV is %d\n"),row,id));
			SRV_ASSERT(0);	
		}
		Core * pCore = new Core;
		pCore->level_		= id;
		pCore->needItem_	= csv.get_int(row,"item");
		pCore->needItemNum_	= csv.get_int(row,"itemnum");
		pCore->probability_	= csv.get_int(row,"probability");
		pCore->maxtime_		= csv.get_int(row,"maxtime");
		pCore->grow_		= csv.get_float(row,"grow");
		data_[id] = pCore;
	}
	return true;
}
bool
PetIntensive::check()
{
	return true;
}

PetIntensive::Core const * 
PetIntensive::getCoreBylevel(U32 level)
{
	return data_[level];
}

// ----------------------------------------------------------- 
std::vector< Monster2Table::Core* > Monster2Table::data_;

bool 
Monster2Table::load(char const *fn)
{
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);

	for(U32 row=0; row<csv.get_records_counter(); ++row)
	{
		Core * pCore = new Core;
		pCore->monsterId_ = csv.get_int(row,"ID");
		pCore->level_	  = csv.get_int(row,"Level");
		pCore->levelExp_  = csv.get_int(row,"LevelExp");
		pCore->assetId_	  = csv.get_int(row,"AssetsID");
		pCore->monsterId2_ = csv.get_int(row,"MonsterID");
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

		pCore->monsterClass_ = csv.get_string(row,"AI_class");

		pCore->properties_.resize(PT_Max);
		pCore->properties_[PT_HpMax] = csv.get_float(row,"PT_HpMax");
		pCore->properties_[PT_MpMax] = csv.get_float(row,"PT_MpMax");
		pCore->properties_[PT_Attack] = csv.get_float(row,"PT_Attack");
		pCore->properties_[PT_Defense] = csv.get_float(row,"PT_Defense");
		pCore->properties_[PT_Agile] = csv.get_float(row,"PT_Agile");
		pCore->properties_[PT_Spirit] = csv.get_float(row,"PT_Spirit");
		pCore->properties_[PT_Reply] = csv.get_float(row,"PT_Reply");
		pCore->properties_[PT_Magicattack] = csv.get_float(row,"PT_Magicattack");
		pCore->properties_[PT_Magicdefense] = csv.get_float(row,"PT_Magicdefense");
		pCore->properties_[PT_Hit] = csv.get_float(row,"PT_Hit");
		pCore->properties_[PT_Dodge] = csv.get_float(row,"PT_Dodge");
		pCore->properties_[PT_Crit] = csv.get_float(row,"PT_Crit");
		pCore->properties_[PT_counterpunch] = csv.get_float(row,"PT_counterpunch");
		pCore->properties_[PT_NoSleep] = csv.get_float(row,"PT_NoSleep");
		pCore->properties_[PT_NoPetrifaction] = csv.get_float(row,"PT_NoPetrifaction");
		pCore->properties_[PT_NoDrunk] = csv.get_float(row,"PT_NoDrunk");
		pCore->properties_[PT_NoChaos] = csv.get_float(row,"PT_NoChaos");
		pCore->properties_[PT_NoForget] = csv.get_float(row,"PT_NoForget");
		pCore->properties_[PT_NoPoison] = csv.get_float(row,"PT_NoPoison");
		//pCore->properties_[PT_Assassinate] = csv.get_float(row,"PT_Assassinate");
		
		std::string strtmp = csv.get_string(row,"NpcID");
		if(!strtmp.empty())
		{
			std::vector<std::string> arrstring = String::Split(strtmp,";");
			if(arrstring.size() == 2){
				std::vector<std::string> arrstrnpc = String::Split(arrstring[0].c_str(),",");
				for(size_t i=0; i<arrstrnpc.size();++i){
					pCore->leftNpcs_.push_back(atoi(arrstrnpc[i].c_str()));
				}
 
				arrstrnpc = String::Split(arrstring[1].c_str(),",");
				for(size_t i=0; i<arrstrnpc.size();++i){
					pCore->rigthNpcs_.push_back(atoi(arrstrnpc[i].c_str()));
				}
			}
		}
		data_.push_back(pCore);
	}

	return true;
}

bool
Monster2Table::check()
{
	for (size_t i=0; i<data_.size();++i)
	{
		for (size_t j=0; j<data_[i]->defaultSkill_.size();++j)
		{
			if(data_[i]->defaultSkill_[j].first == 0)
			{
				ACE_DEBUG((LM_ERROR,"Monster2table monsterid==>[%d] level==>[%d] skill error\n",data_[i]->monsterId_,data_[i]->level_));
				SRV_ASSERT(0);
			}
			else if (data_[i]->defaultSkill_[j].second == 0)
			{
				ACE_DEBUG((LM_ERROR,"Monster2table monsterid==>[%d] level==>[%d] skill error\n",data_[i]->monsterId_,data_[i]->level_));
				SRV_ASSERT(0);
			}

			if(NULL ==  SkillTable::getSkillById(data_[i]->defaultSkill_[j].first,data_[i]->defaultSkill_[j].second))
			{
				ACE_DEBUG((LM_ERROR,"Monster2table[%d]==[%d] Skill table can not find skid[%d]---skLv[%d]\n",data_[i]->monsterId_,data_[i]->level_,data_[i]->defaultSkill_[j].first,data_[i]->defaultSkill_[j].second));
				SRV_ASSERT(0);
			}

			if(NULL == MonsterClass::getClassByName(data_[i]->monsterClass_))
			{
				ACE_DEBUG((LM_ERROR,ACE_TEXT("Cannot find monsterclass(%s) in monster2table!!! monsterid(%d),level(%d)\n"),data_[i]->monsterClass_.c_str(),data_[i]->monsterId_,data_[i]->level_));
				SRV_ASSERT(0);
			}
		}
	}
	return true;
}

Monster2Table::Core const * 
Monster2Table::getMonster2Core(U32 id,U32 level)
{
	for (size_t i=0; i<data_.size();++i)
	{
		if(data_[i]->monsterId_ == id && data_[i]->level_ == level)
			return data_[i];
	}

	return NULL;
}

S32	Monster2Table::getMonsterLeftNpc(U32 id,U32 level,S32 index){
	Core const* tmp = getMonster2Core(id,level);
	if(!tmp)
		return 0;
	if(index < 0 || index >= tmp->leftNpcs_.size())
		return 0;
	return tmp->leftNpcs_[index];
}
S32	Monster2Table::getMonsterRightNpc(U32 id,U32 level,S32 index){
	Core const* tmp = getMonster2Core(id,level);
	if(!tmp)
		return 0;
	if(index < 0 || index >= tmp->rigthNpcs_.size())
		return 0;
	return tmp->rigthNpcs_[index];
}

S32 Monster2Table::findMonsterIdByNpcId(S32 npcId, bool&isLeft){
	for(size_t i=0; i<data_.size(); ++i){
		for(size_t k=0; k<data_[i]->leftNpcs_.size(); ++k){
			if(data_[i]->leftNpcs_[k] == npcId)
			{
				isLeft = true;
				return data_[i]->monsterId_;
			}
		}

		for(size_t k=0; k<data_[i]->rigthNpcs_.size(); ++k){
			if(data_[i]->rigthNpcs_[k] == npcId)
			{
				isLeft = false;
				return data_[i]->monsterId_;
			}
		}
	}

	return 0;
}
