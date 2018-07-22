
#include "config.h"
#include "routine.h"
#include "sqltask.h"
#include "server.h"


static std::set<std::string> gHolder;

void gAddInsertHolder(std::string const& username)
{
	gHolder.insert(username);
}

bool gCheckInsertHolder(std::string const& username)
{
	return (gHolder.find(username) != gHolder.end());
}

void gDelInsertHolder(std::string const& username)
{
	std::set<std::string>::iterator itr = gHolder.find(username);
	if(itr!=gHolder.end())
		gHolder.erase(itr);
}



U32 GetInstanceMaxId(SQLTask *pTask){
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	const char* pCode = "SELECT MAX(Guid) AS GuidMax FROM Account ";
	U32 r = 0;
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Query q = dbc->execQuery(pCode);
	if (!q.eof())
	{
		 r  = q.getIntField("GuidMax");
	} 
#elif defined(USE_MYSQL)
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(pCode));
	if (!res->next())
	{
		 r = res->getInt("GuidMax");
	}
#endif

	DB_EXEC_UNGUARD_RETURN

	return r+1;
}

U32
InitSQLTables::go(SQLTask* pTask)
{
	static const char* AccountTableDDL = 
"CREATE TABLE IF NOT EXISTS Account("
"Guid INTEGER NOT NULL,"
"UserName	VARCHAR(60) NOT NULL,"
"Password	BLOB	 NOT NULL,"
"PhoneNumber VARCHAR(60) NOT NULL,"
"AccountInfo BLOB NOT NULL,"
"Seal INT NOT NULL,"
"PRIMARY KEY(Guid)"
");";
	
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
DB_EXEC_GUARD
#if defined(USE_SQLITE)
	dbc->execDML(AccountTableDDL);
#elif defined(USE_MYSQL)
	//dbc->prepareStatement(AccountTableDDL);
#endif
DB_EXEC_UNGUARD_RETURN
	return 0;
}
