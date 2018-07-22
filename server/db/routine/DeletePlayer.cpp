
#include "../routine.h"

U32
DeletePlayer::go(SQLTask *pTask)
{
	ACE_DEBUG((LM_INFO,"Delete player %s \n",playername_.c_str()));
	

	std::stringstream ss0;
	ss0 << "UPDATE Player SET UserName = '' WHERE PlayerName = '" << playername_ << "';";
	std::stringstream ss1;
	ss1 << "UPDATE Baby SET OwnerName = '++"<< playername_ << "' WHERE OwnerName = '" << playername_ << "';";
	std::stringstream ss2;
	ss2 << "UPDATE Employee SET OwnerName = '++"<< playername_ << "' WHERE OwnerName = '" << playername_ << "';";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);

DB_EXEC_GUARD
#if defined(USE_SQLITE)
dbc->execDML(ss0.str().c_str());
dbc->execDML(ss1.str().c_str());
dbc->execDML(ss2.str().c_str());
#elif  defined(USE_MYSQL)
{
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(ss0.str().c_str()));
	prep_stmt->execute();
}		
{
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(ss1.str().c_str()));
	prep_stmt->execute();
}	
{
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(ss2.str().c_str()));
	prep_stmt->execute();
}	
#endif
DB_EXEC_UNGUARD_RETURN		
return 0;
};