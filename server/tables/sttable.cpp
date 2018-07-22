
#include "sttable.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"
std::map< S32 , StateTable::Core* >  StateTable::data_;

std::string 
stateCompileScript(std::string &chunk, S32 skid, char const *fix)
{
	char id_str[128]={0};
	sprintf(id_str, "St_%d_", skid);

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
StateTable::load(char const *fn)
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
			ACE_DEBUG((LM_ERROR,ACE_TEXT("State has same id in row %d , id is %d\n"),row,id));
			SRV_ASSERT(0);	
		}

		Core * pCore = new Core();
		SRV_ASSERT(pCore);
		pCore->id_ = id;

		pCore->updateClass_ = csv.get_string(row,"Update");
		pCore->updateClass_ = stateCompileScript(pCore->updateClass_,id,"Update");
		pCore->initClass_ = csv.get_string(row,"Init");
		pCore->initClass_ = stateCompileScript(pCore->initClass_,id,"Init");
		//ACE_DEBUG((LM_ERROR,ACE_TEXT("%s  ==> %s\n"),pCore->updateClass_.c_str(),csv.get_string(row,"Update").c_str()));
		
		pCore->turn_ = csv.get_int(row, "Turn");
		pCore->tick_ = csv.get_int(row, "Tick");
		std::string sttmp = csv.get_string(row,"Type");
		int resisttype = ENUM(StateType).getItemId(sttmp); 
		pCore->type_ = (StateType)resisttype;
		pCore->level_ = csv.get_int(row, "Level");
		pCore->battleDelete_ = csv.get_int(row,"BattleDelete") == 0 ? false : true;
		data_[id] = pCore;
	}
	return true;
}

bool 
StateTable::check()
{
	std::map<S32 , Core* >::iterator itor = data_.begin();

	while(itor != data_.end())
	{
		Core* pCore = itor->second;

		if(pCore == NULL)
			return false;

		if(pCore->type_ == -1)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("StateTable [%d] StateType Error\n"),pCore->id_));
			return false;
		}

		++itor;
	}

	return true;
}

StateTable::Core const * 
StateTable::getStateById(S32 id)
{
	return data_[id];
}



