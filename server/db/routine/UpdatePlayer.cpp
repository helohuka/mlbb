
#include "config.h"
#include "../routine.h"
#include "../sqltask.h"
U32 
UpdatePlayer::go(SQLTask *pTask)
{	
	//ACE_DEBUG((LM_INFO,ACE_TEXT("Update player go!!! [%s]\n"),username_.c_str()));
	enum {BUFFER_SIZE = 1024*1024};
	char *buffer = new char[BUFFER_SIZE];
	static const char * pCode =
		"UPDATE Player SET PlayerLevel=?,PlayerProfession=?,PlayerGrade=?,BinData=?,Money=?,Diamond=?,Magic=?,LogoutTime=?,VersionNumber=? WHERE PlayerGuid=?;";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);

	{
		for (size_t i=0; i<player_.babies_.size(); ++i){			
			
			WorldHandler::instance()->updateBaby(player_.babies_[i]);
		}
	
		for (size_t i=0; i<player_.employees_.size(); ++i){
			WorldHandler::instance()->updateEmployee(player_.employees_[i]);
		}
		
	}
	player_.babies_.clear();
	player_.employees_.clear();
	ProtocolMemWriter mw(buffer,BUFFER_SIZE);
	player_.serialize(&mw);
DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Statement stmt =dbc->compileStatement(pCode);
	stmt.bind(1,player_.properties_[PT_Level]);
	stmt.bind(2,player_.properties_[PT_Profession]);
	stmt.bind(3,player_.properties_[PT_FightingForce]);
	stmt.bind(4,(unsigned char*)buffer,mw.length());
	stmt.bind(5,player_.properties_[PT_Money]);
	stmt.bind(6,player_.properties_[PT_Diamond]);
	stmt.bind(7,player_.properties_[PT_MagicCurrency]);
	stmt.bind(8,int(player_.logoutTime_));
	stmt.bind(9,player_.versionNumber_);
	stmt.bind(10,player_.instId_);
	stmt.execDML();
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	prep_stmt->setInt(1,player_.properties_[PT_Level]);
	prep_stmt->setInt(2,player_.properties_[PT_Profession]);
	prep_stmt->setInt(3,player_.properties_[PT_FightingForce]);
	sql::SQLString binString(buffer,mw.length());
	prep_stmt->setString(4,binString);
	prep_stmt->setInt(5,player_.properties_[PT_Money]);
	prep_stmt->setInt(6,player_.properties_[PT_Diamond]);
	prep_stmt->setInt(7,player_.properties_[PT_MagicCurrency]);
	prep_stmt->setInt(8,int(player_.logoutTime_));
	prep_stmt->setInt(9,player_.versionNumber_);
	prep_stmt->setInt(10,player_.instId_);
	prep_stmt->executeUpdate();
#endif
	
DB_EXEC_UNGUARD_RETURN		
	delete []buffer;
	return 0;
}

U32 
UpdatePlayer::back()
{
	//ACE_DEBUG((LM_INFO,ACE_TEXT("Update player ok!!! [%s]\n"),username_.c_str()));
	return 0;
}