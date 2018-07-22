#include "config.h"
#include "com.h"
#include "account.h"
#include "player.h"
#include "GMCmdMgr.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"

std::map<std::string,GMCmdMgr::GmCmd>	GMCmdMgr::GmCmds_;
void GMCmdMgr::addGmCmd( std::string& cmdName,GMLevel level,std::string& funName  )
{
	GmCmd	cmd;
	cmd.gmLevel_=level;
	cmd.scriptFunName_=funName;
	GmCmds_[cmdName]=cmd;
}

void GMCmdMgr::callGmCmd( Account* acc,const char* cmd )
{
	/*std::string cmdName;
	if( !TokenParser::getToken( cmd, cmdName ) )
		return ;
	std::map< std::string, GmCmd >::iterator iter = GmCmds_.find( cmdName );
	if( iter == GmCmds_.end() )
		return ;
	if( iter->second.gmLevel_ > acc->gmlev_ )
		return ;

	std::string err;
	if( !ScriptEnv::callGMCmd( acc->handleId_, iter->second.scriptFunName_.c_str(), cmd, err ) )
	{
		ACE_DEBUG( (LM_ERROR, ACE_TEXT("%s\n"), err.c_str() ) );
	}*/

}

void GMCmdMgr::callGmCmd( Player* player,const char* cmd )
{
	
}