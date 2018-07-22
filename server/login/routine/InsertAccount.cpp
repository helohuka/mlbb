
#include "config.h"
#include "../routine.h"
#include "../handler.h"
#include "../server.h"
extern bool gCheckInsertHolder(std::string const& username);
extern void gDelInsertHolder(std::string const& username);
#define CREATE_ROUTINE(P,TYPE) TYPE* P = NULL; Routine::create(P); SRV_ASSERT(P);
U32
InsertAccount::go(SQLTask *pTask)
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("Insert account %s \n"),account_.username_.c_str()));
	static const char * pCode =
"INSERT INTO Account(UserName,Password,AccountInfo,Seal,PhoneNumber) "
"VALUES(?,?,?,?,?);";
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);

	char buffer[20480] = {'\0'};
	ProtocolMemWriter mw(buffer,sizeof(buffer));
	account_.createtime_ = ACE_OS::gettimeofday().sec();
	account_.serialize(&mw);

DB_EXEC_GUARD

#if defined(USE_SQLITE)
	CppSQLite3Statement stmt =dbc->compileStatement(pCode);
	stmt.bind(1,account_.username_.c_str());
	stmt.bind(2,account_.password_.c_str());
	stmt.bind(4,0);
	stmt.bind(3,(unsigned char*)buffer,mw.length());
	stmt.bind(5,"");
	stmt.execDML();
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	prep_stmt->setString(1,account_.username_.c_str());
	prep_stmt->setString(2,account_.password_.c_str());
	prep_stmt->setInt(4,0);
	sql::SQLString binString(buffer,mw.length());
	prep_stmt->setString(3,binString);
	prep_stmt->setString(5,"");
	prep_stmt->executeUpdate();
#endif
DB_EXEC_UNGUARD_RETURN		
	return 0;
}


U32
InsertAccount::back()
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("Insert account %s %d back\n"),account_.username_.c_str(),account_.guid_));
	WorldHandler::instance()->queryAccountOk(account_,true,false);
	return 0;
}
