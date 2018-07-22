#ifndef	__GMCMDMGR_H__
#define	__GMCMDMGR_H__
/** GM命令*/
#include "config.h"
#include "account.h"
class GMCmdMgr
{
public:
	struct GmCmd
	{
		GmCmd():gmLevel_(GML_All){}
		GMLevel		gmLevel_;		//<gm级别
		std::string scriptFunName_;	//<脚本函数名
	};

	static void addGmCmd( std::string& cmdName,GMLevel level,std::string& funName);
	static void callGmCmd(Account* account,char const* cmd);
	static void callGmCmd(Player* player,char const* cmd);
	static std::map<std::string,GmCmd>		GmCmds_;
};

#endif