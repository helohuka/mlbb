#include "robotTable.h"
#include "tmptable.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"
#include "monstertable.h"
#include "npctable.h"

std::map< S32 , RobotTab::RobotData*>  RobotTab::data_;
std::vector<PlayerAI*>	PlayerAI::data_;

bool
RobotTab::load(char const *fn)
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

		if(data_[id] != NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("robotTable has same id in row %d , id is %d\n"),row,id));
			SRV_ASSERT(0);	
		}

		RobotData* pData = new RobotData;

		pData->robotId_			= id;
		pData->robotTmpId_		= csv.get_int(row,"PlayerID");
		pData->robotName_		= csv.get_string(row,"RobotName");
		pData->robotLevel_		= csv.get_int(row,"PlayerLv");
		pData->robotAIclass_	= csv.get_string(row,"PlayerAI");
		pData->babyId_			= csv.get_int(row,"BabyID");
		pData->babyLevel_		= csv.get_int(row,"BabyLv");
		pData->babyAIclass_		= csv.get_string(row,"BabyAI");

		std::string strDefaultEmp = csv.get_string(row,"EmployeeID");

		std::string strJob = csv.get_string(row,"JobType");
		int job = ENUM(JobType).getItemId(strJob);
		pData->job_ = (JobType)job;
		pData->JobLevel_ = 1;

		if(!strDefaultEmp.empty())
		{
			char const * pChar = strDefaultEmp.c_str();
			std::string strToken;
			while( TokenParser::getToken( pChar , strToken , ';'))
			{
				U32 uEmpId= atoi(strToken.c_str());
				pData->employees_.push_back(uEmpId);
			}
		}

		std::string strEquip = csv.get_string(row,"PlayerEquip");

		if (!strEquip.empty())
		{
			char const * pChar = strEquip.c_str();
			std::string strToken;
			while( TokenParser::getToken( pChar , strToken , ';'))
			{
				U32 uItemId = atoi(strToken.c_str());
				pData->equips_.push_back(uItemId);
			}
		}

		data_[id] = pData;
	}

	return true;
}

bool
RobotTab::check()
{
	std::map<S32,RobotData*>::iterator itor = data_.begin();

	while(itor != data_.end())
	{
		RobotData* pData = itor->second;
		SRV_ASSERT(pData);
		PlayerTmpTable::Core const * core = PlayerTmpTable::getTemplateById(pData->robotTmpId_);
		SRV_ASSERT(core);

		if(pData->robotAIclass_.empty())
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("robotTable [%d] robotClass is nil\n"),pData->robotId_));
			SRV_ASSERT(0);
		}

		if(pData->job_ == -1)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("robotTable [%d] robotJobType is nil\n"),pData->robotId_));
			SRV_ASSERT(0);
		}
		
		if(pData->babyId_)
		{
			SRV_ASSERT(MonsterTable::getMonsterById(pData->babyId_));
			SRV_ASSERT(pData->babyLevel_);
		}

		++itor;
	}

	return true;
}

RobotTab::RobotData const*
RobotTab::getRobotDataById(U32 id)
{
	return data_[id];
}

//----
bool
PlayerAI::load(char const *fn)
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

		PlayerAI* pData = new PlayerAI();
		pData->id_ = id;
		
		std::string strProf = csv.get_string(row,"Profession");
		int prof = ENUM(JobType).getItemId(strProf);
		pData->prof_ = (JobType)prof;

		pData->playerClass_ = csv.get_string(row,"ProfessionAI");
		pData->babyClass_	= csv.get_string(row,"BabyAI");

		data_.push_back(pData);
	}

	return true;
}

bool
PlayerAI::check()
{
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if (data_[i]->prof_ == -1)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("DBplayerAI ArenaAiData Table [%d] Profession is nil\n"),data_[i]->id_));
			SRV_ASSERT(0);
		}

		if (data_[i]->playerClass_.empty())
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("PlayerAI ArenaAiData Table [%d] DBbabyAIClass_ is nil\n"),data_[i]->id_));
			SRV_ASSERT(0);
		}

		if (data_[i]->babyClass_.empty())
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("PlayerAI ArenaAiData Table [%d] DBplayerAIClass_ is nil\n"),data_[i]->id_));
			SRV_ASSERT(0);

		}
	}

	return true;
}

PlayerAI const*
PlayerAI::getAI(JobType jt)
{
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if(data_[i]->prof_ == jt)
			return data_[i];
	}

	return NULL;
}

//////////////////////////////////////////////////////////////////////////

std::vector<RobotActionTable::RobotActionData*>	RobotActionTable::actiondata_;

bool
RobotActionTable::load(char const *fn)
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
		RobotActionData* pdata = new RobotActionData;
		pdata->robotName_	= csv.get_string(row,"RobotName");
		pdata->userName_	= csv.get_string(row,"UserName");

		{
			std::string stt = csv.get_string(row,"Type");
			int stte = ENUM(RobotActionType).getItemId(stt);
			if(-1 == stte)
				SRV_ASSERT(0);	
			pdata->actionType_ = (RobotActionType)stte;
		}

		{
			std::string stt = csv.get_string(row,"JobType");
			int stte = ENUM(JobType).getItemId(stt);
			if(-1 == stte)
				SRV_ASSERT(0);	
			pdata->jobtype_ = (JobType)stte;
		}

		//pdata->title_ = csv.get_int(row,"TitleId");
		pdata->npcid_		= csv.get_int(row,"Zuobiao");
		std::string npcids = csv.get_string(row,"NpcID");
		{
			char const * pChar = npcids.c_str();
			std::string strToken;
			if(pChar)
			{
				while( TokenParser::getToken( pChar , strToken , ';'))
				{
					if( !strToken.empty() && 0!=strToken.length() )
					{
						pdata->npclist_.push_back(::atoi(strToken.c_str()));
					}
				}
			}
		}
	
		std::string strEquip = csv.get_string(row,"PlayerEquip");

		if (!strEquip.empty())
		{
			char const * pChar = strEquip.c_str();
			std::string strToken;
			while( TokenParser::getToken( pChar , strToken , ';'))
			{
				U32 uItemId = atoi(strToken.c_str());
				pdata->equips_.push_back(uItemId);
			}
		}

		actiondata_.push_back(pdata);
	}
	return true;
}

bool
RobotActionTable::check(){
	for (size_t i=0; i<actiondata_.size();++i)
	{
		SRV_ASSERT(NpcTable::getNpcById(actiondata_[i]->npcid_));
		for (size_t j=0; j<actiondata_[i]->npclist_.size();++j)
		{
			SRV_ASSERT(NpcTable::getNpcById(actiondata_[i]->npclist_[j]));
		}
	}
	return true;
}

RobotActionTable::RobotActionData const*
RobotActionTable::getActionData(std::string userName){
	for (size_t i=0;i<actiondata_.size();++i){
		if(actiondata_[i]->userName_ == userName)
			return actiondata_[i];
	}
	return NULL;
}

bool RobotActionTable::isRobot(std::string& name){
	for (size_t i=0;i<actiondata_.size();++i){
		if(actiondata_[i]->userName_ == name)
			return true;
	}
	return false;
}