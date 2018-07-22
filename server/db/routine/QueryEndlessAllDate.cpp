#include "config.h"
#include "../sqltask.h"
#include "../routine.h"
#include "../handler.h"
#include "../server.h"

U32
QueryEndlessAllDate::go(SQLTask *pTask)
{
	names_.clear();

	ACE_DEBUG((LM_DEBUG ,"queryEndlessAllDate go\n"));
	std::stringstream sstream;
	sstream << "SELECT * FROM EndlessStair order by Rank asc limit 1000;";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
		CppSQLite3Query q = dbc->execQuery(sstream.str().c_str());

	if(!q.eof())
	{

		while(!q.eof())
		{
			std::string name = q.getStringField("PlayerName");
			names_.push_back(name);
			q.nextRow();
		}
	}
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(sstream.str().c_str()));

	while(res->next())
	{
		std::string name = res->getString("PlayerName");
		names_.push_back(name);
	}

#endif

	DB_EXEC_UNGUARD_RETURN
		return 0;
}

U32
QueryEndlessAllDate::back()
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("queryEndlessStairAllDateOK  back\n")));
	
	WorldHandler::instance()->queryEndlessStairAllDateOK(names_);
	return 0;
}