
#include "config.h"
#include "../routine.h"
#include "../handler.h"
#include "../sqltask.h"
extern WorldHandler   gWorldHandler;
U32
InsertPlayer::go(SQLTask *pTask)
{
	
	ACE_DEBUG((LM_DEBUG,ACE_TEXT("Insert player %s \n"),username_.c_str()));

	hasSameName_ = CheckSamePlayerName(pTask,player_.instName_);

	if(hasSameName_)
	{
		ACE_DEBUG((LM_DEBUG, "Create player same name error \n"));
		return 0;
	}
	static const char * pCode =
"INSERT INTO Player(UserName,PlayerGuid,BinData,PlayerName,PlayerLevel,PlayerProfession,PlayerGrade,Seal,Freeze,InDoorId,Money,Diamond,Magic,LogoutTime,VersionNumber)"
"VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?);";
	
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	
	if(!player_.babies_.empty()){
		InsertBaby ib;
		for (size_t i=0; i<player_.babies_.size(); ++i){
			ib.playername_ = player_.babies_[i].ownerName_ = player_.instName_;
			ib.baby_ = player_.babies_[i];
			ib.go(pTask);
			player_.babies_[i] = ib.baby_;
			Server::instance()->addBabyInst(ib.baby_);
		}
	}

	if(!player_.employees_.empty()){
		InsertEmployee ie;
		for (size_t i=0; i<player_.employees_.size(); ++i){
			ie.playername_ = player_.employees_[i].ownerName_ = player_.instName_;
			ie.employee_ = player_.employees_[i];
			ie.go(pTask);
			player_.employees_[i] = ie.employee_;
			Server::instance()->addEmployeeInst(ie.employee_);
		}
	}
	//player_.babies_.clear();
	//player_.employees_.clear();
	
	player_.createTime_ = ACE_OS::gettimeofday().sec();
	char buffer[20480] = {'\0'};
	ProtocolMemWriter mw(buffer,sizeof(buffer));
	player_.serialize(&mw);
DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Statement stmt =dbc->compileStatement(pCode);
	stmt.bind(1,username_.c_str());
	stmt.bind(2,player_.instId_);	
	stmt.bind(3,(unsigned char*)buffer,mw.length());
	stmt.bind(4,player_.instName_.c_str());
	stmt.bind(5,player_.properties_[PT_Level]);
	stmt.bind(6,player_.properties_[PT_Profession]);
	stmt.bind(7,player_.properties_[PT_FightingForce]);
	stmt.bind(8,0);
	stmt.bind(9,0);
	stmt.bind(10,serverId_);
	stmt.bind(11,player_.properties_[PT_Money]);
	stmt.bind(12,player_.properties_[PT_Diamond]);
	stmt.bind(13,player_.properties_[PT_MagicCurrency]);
	stmt.bind(14,int(player_.logoutTime_));
	stmt.bind(15,player_.versionNumber_);
	stmt.execDML();
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	prep_stmt->setString(1,username_.c_str());
	prep_stmt->setInt(2,player_.instId_);
	sql::SQLString binString(buffer,mw.length());
	prep_stmt->setString(3,binString);
	prep_stmt->setString(4,player_.instName_);
	prep_stmt->setInt(5,player_.properties_[PT_Level]);
	prep_stmt->setInt(6,player_.properties_[PT_Profession]);
	prep_stmt->setInt(7,player_.properties_[PT_FightingForce]);
	prep_stmt->setInt(8,0);
	prep_stmt->setInt(9,0);
	prep_stmt->setInt(10,serverId_);
	prep_stmt->setInt(11,player_.properties_[PT_Money]);
	prep_stmt->setInt(12,player_.properties_[PT_Diamond]);
	prep_stmt->setInt(13,player_.properties_[PT_MagicCurrency]);
	prep_stmt->setInt(14,int(player_.logoutTime_));
	prep_stmt->setInt(15,player_.versionNumber_);
	prep_stmt->executeUpdate();
#endif
	
DB_EXEC_UNGUARD_RETURN		
	return 0;
}

U32
InsertPlayer::back()
{
	ACE_DEBUG((LM_DEBUG,ACE_TEXT("Insert player %s back\n"),username_.c_str()));
	if(hasSameName_)
		WorldHandler::instance()->createPlayerSameName(username_);
	else
		WorldHandler::instance()->createPlayerOk(username_,player_,serverId_);
	return 0;
}



