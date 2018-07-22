#include "config.h"
#include "../routine.h"
#include "../handler.h"
#include "../sqltask.h"

U32
InsertEndless::go(SQLTask *pTask)
{
	ACE_DEBUG((LM_DEBUG,ACE_TEXT("Insert EndlessStair %s \n"),playerName_.c_str()));
	static const char * pCode =
		"INSERT INTO EndlessStair(PlayerName,Rank)"
		"VALUES(?,?);";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Statement stmt =dbc->compileStatement(pCode);
	stmt.bind(1,playerName_.c_str());
	stmt.bind(2,rank_);	
	stmt.execDML();
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	prep_stmt->setString(1,playerName_.c_str());
	prep_stmt->setInt(2,rank_);
	prep_stmt->executeUpdate();
#endif

	DB_EXEC_UNGUARD_RETURN		
		return 0;
}

U32
InsertEndless::back()
{
	ACE_DEBUG((LM_DEBUG,ACE_TEXT("Insert EndlessStair %s, rank == %d back\n"),playerName_.c_str(),rank_));
	return 0;
}

//////////////////////////////////////////////////////////////////////////

U32
DeleteEndlsee::go(SQLTask *pTask)
{
	ACE_DEBUG((LM_DEBUG,ACE_TEXT("DeleteEndlsee EndlessStair %s \n"),playerName_.c_str()));
	static const char * pCode =
		"DELETE FROM EndlessStair WHERE PlayerName=?;";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
		CppSQLite3Statement stmt =dbc->compileStatement(pCode);
	stmt.bind( 1 , playerName_.c_str());
	stmt.execDML();
#elif  defined(USE_MYSQL)
		std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	prep_stmt->setString(1,playerName_.c_str());
	prep_stmt->executeUpdate();
#endif

	DB_EXEC_UNGUARD_RETURN		
		return 0;
}

U32
DeleteEndlsee::back()
{
	ACE_DEBUG((LM_DEBUG,ACE_TEXT("DeleteEndlsee EndlessStair %s back\n"),playerName_.c_str()));
	return 0;
}
