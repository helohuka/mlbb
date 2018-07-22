#include "config.h"
#include "../sqltask.h"
#include "../routine.h"
#include "../handler.h"


#include "config.h"
#include "../sqltask.h"
#include "../routine.h"
#include "../handler.h"
#include "../server.h"

U32
QueryPlayerByLevel::go(SQLTask *pTask)
{
	ACE_DEBUG((LM_DEBUG ,"QueryPlayerByLevel go\n"));
	std::stringstream sstream;
	sstream << "SELECT * FROM Player WHERE UserName<>'' order by PlayerLevel DESC LIMIT 1000;";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Query q = dbc->execQuery(sstream.str().c_str());
	U32 rank = 0;
	if(!q.eof())
	{
		while(!q.eof())
		{
			S32 len=0;
			const unsigned char* pCacheBlob= q.getBlobField("BinData",len);
			SRV_ASSERT(len);
			ProtocolMemReader mr(pCacheBlob,len);
			SGE_DBPlayerData inst;
			inst.deserialize(&mr);

			COM_ContactInfo info;
			info.level_ = q.getIntField("PlayerLevel");
			info.name_ = q.getStringField("PlayerName");
			info.job_ = (JobType)q.getIntField("PlayerProfession");
			info.jobLevel_ = (S32)inst.properties_[PT_ProfessionLevel];
			info.instId_ = q.getIntField("PlayerGuid");
			info.exp_	= inst.properties_[PT_Exp];
			//info.rank_ = ++rank;
			Server::instance()->playerlevelrankCache_.push_back(info);
			q.nextRow();
		}
	}
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(sstream.str().c_str()));
	U32 rank = 0;
	while(res->next())
	{
		S32 len=0;
		sql::SQLString pCacheBlob= res->getString("BinData");
		ProtocolMemReader mr(pCacheBlob->c_str(),pCacheBlob->length());
		SGE_DBPlayerData inst;
		inst.deserialize(&mr);

		COM_ContactInfo info;
		info.level_ = res->getInt("PlayerLevel");
		info.name_ = res->getString("PlayerName");
		info.instId_ = res->getInt("PlayerGuid");
		info.job_ = (JobType)res->getInt("PlayerProfession");
		info.jobLevel_ = inst.properties_[PT_ProfessionLevel];
		info.exp_	= inst.properties_[PT_Exp];
		//info.rank_ = ++rank;
		Server::instance()->playerlevelrankCache_.push_back(info);
	}

#endif

	DB_EXEC_UNGUARD_RETURN
		return 0;
}

U32
QueryPlayerByFF::go(SQLTask *pTask)
{
	ACE_DEBUG((LM_DEBUG ,"QueryPlayerByFF go\n"));
	std::stringstream sstream;
	sstream << "SELECT * FROM Player WHERE UserName<>'' ORDER BY PlayerGrade DESC LIMIT 1000;";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Query q = dbc->execQuery(sstream.str().c_str());
	U32 rank = 0;
	if(!q.eof())
	{
		while(!q.eof())
		{
			S32 len=0;
			const unsigned char* pCacheBlob= q.getBlobField("BinData",len);
			SRV_ASSERT(len);
			ProtocolMemReader mr(pCacheBlob,len);
			SGE_DBPlayerData inst;
			inst.deserialize(&mr);

			COM_ContactInfo info;
			info.instId_ = q.getIntField("PlayerGuid");
			info.level_ = q.getIntField("PlayerLevel");
			info.name_ = q.getStringField("PlayerName");
			info.job_ = (JobType)q.getIntField("PlayerProfession");
			info.jobLevel_ = (S32)inst.properties_[PT_ProfessionLevel];
			info.ff_ = (S32)inst.properties_[PT_FightingForce];
			info.rank_ = ++rank;
			Server::instance()->playerFFrankCache_.push_back(info);
			q.nextRow();
		}
	}
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(sstream.str().c_str()));
	U32 rank = 0;
	while(res->next())
	{
		S32 len=0;
		sql::SQLString pCacheBlob= res->getString("BinData");
		ProtocolMemReader mr(pCacheBlob->c_str(),pCacheBlob->length());
		SGE_DBPlayerData inst;
		inst.deserialize(&mr);

		COM_ContactInfo info;
		info.instId_ = res->getInt("PlayerGuid");
		info.level_ = res->getInt("PlayerLevel");
		info.name_ = res->getString("PlayerName");
		info.job_ = (JobType)res->getInt("PlayerProfession");
		info.jobLevel_ = inst.properties_[PT_ProfessionLevel];
		info.ff_ = inst.properties_[PT_FightingForce];
		info.rank_ = ++rank;
		Server::instance()->playerFFrankCache_.push_back(info);
	}

#endif

	DB_EXEC_UNGUARD_RETURN
		return 0;
}

