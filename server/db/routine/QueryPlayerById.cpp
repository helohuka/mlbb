
#include "config.h"
#include "../sqltask.h"
#include "../routine.h"
#include "../handler.h"
#include "../server.h"

U32
QueryPlayerById::go(SQLTask *pTask)
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("QueryPlayerById player %d \n"),playerGuid_));
	std::stringstream sstream;
	sstream << "SELECT * FROM Player WHERE playerGuid = \"" << playerGuid_ << "\"";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
		CppSQLite3Query q = dbc->execQuery(sstream.str().c_str());

	if(!q.eof())
	{
		while(!q.eof())
		{
			U32 instId = q.getIntField("PlayerGuid");
			S32 len=0;
			const unsigned char* pCacheBlob= q.getBlobField("BinData",len);
			SRV_ASSERT(len);
			ProtocolMemReader mr(pCacheBlob,len);
			SGE_DBPlayerData inst;
			inst.instId_ = instId;
			inst.deserialize(&mr);

			

			player_ = inst;
			q.nextRow();
		}
	}
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(sstream.str().c_str()));

	if(res->next())
	{
		U32 instId = res->getInt("PlayerGuid");
		S32 len=0;
		sql::SQLString pCacheBlob= res->getString("BinData");
		ProtocolMemReader mr(pCacheBlob->c_str(),pCacheBlob->length());
		SGE_DBPlayerData inst;
		inst.instId_ = instId;
		inst.deserialize(&mr);
		
		inst.instId_ =  res->getInt("PlayerGuid");
		inst.instName_	= res->getString("PlayerName").c_str();

		player_ = inst;
	}
#endif

	DB_EXEC_UNGUARD_RETURN
		return 0;
}

U32
QueryPlayerById::back()
{
	//ACE_DEBUG((LM_INFO,ACE_TEXT("QueryPlayerById player %d back\n"),playerGuid_));
	Server::instance()->getPlayerBabyList(player_.instName_,player_.babies_);
	Server::instance()->getPlayerEmployeeList(player_.instName_,player_.employees_);
	WorldHandler::instance()->queryPlayerByIdOK(initiator_,player_,where_);
	
	return 0;
}