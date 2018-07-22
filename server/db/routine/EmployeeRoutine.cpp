#include "config.h"
#include "../routine.h"
#include "../handler.h"
#include "../sqltask.h"

U32
InsertEmployee::go(SQLTask *pTask)
{
	static const char * pCode =
		"INSERT INTO Employee(EmployeeGuid,EmployeeName,BinData,OwnerName,EmployeeGrade)"
		"VALUES(?,?,?,?,?);";
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	char buffer[20480] = {'\0'};
	ProtocolMemWriter mw(buffer,sizeof(buffer));
	employee_.serialize(&mw);
DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Statement stmt =dbc->compileStatement(pCode);
	stmt.bind(1,employee_.instId_);
	stmt.bind(2,employee_.instName_.c_str());	
	stmt.bind(3,(unsigned char*)buffer,mw.length());
	stmt.bind(4,playername_.c_str());
	stmt.bind(5,employee_.properties_[PT_FightingForce]);
	stmt.execDML();
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	prep_stmt->setInt(1,employee_.instId_);
	prep_stmt->setString(2,employee_.instName_);
	sql::SQLString binString(buffer,mw.length());
	prep_stmt->setString(3,binString);
	prep_stmt->setString(4,employee_.ownerName_);
	prep_stmt->setInt(5,employee_.properties_[PT_FightingForce]);
	prep_stmt->executeUpdate();
#endif

DB_EXEC_UNGUARD_RETURN
	return 0;
}

U32
InsertEmployee::back()
{
	Server::instance()->addEmployeeInst(employee_);
	WorldHandler::instance()->createEmployeeOK(playername_,employee_);
	return 0;
}

//--------------------------------------------------------------------

U32
DeleteEmployee::go(SQLTask *pTask)
{
	std::stringstream sstream;
	sstream << "UPDATE Employee SET OwnerName='++" << playername_ << "' WHERE EmployeeGuid IN (";
	for (size_t i = 0; i < ids_.size(); ++i)
	{
		sstream << ids_[i] << ( (i+1 == ids_.size()) ? "" : "," ) ;
	}
	sstream << ") AND OwnerName='" << playername_ << "';";
	//static const char* pCode = code.c_str();
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);

DB_EXEC_GUARD

#ifdef USE_SQLITE
	CppSQLite3Statement stmt = dbc->compileStatement(sstream.str().c_str());
	stmt.execDML();
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(sstream.str().c_str()));
	prep_stmt->execute();
#endif

DB_EXEC_UNGUARD_RETURN
		return 0;
}

U32 
DeleteEmployee::back()
{
	for(size_t i=0; i<ids_.size(); ++i){
		Server::instance()->delEmployeeInst(ids_[i]);
	}
	WorldHandler::instance()->deleteEmployeeOK(playername_,ids_);
	return 0;
}

//----------------------------------------------------------------------------

U32
UpdateEmployee::go(SQLTask *pTask)
{
	static const char * pCode =
		"UPDATE Employee SET EmployeeGrade=?,BinData=? WHERE EmployeeGuid=?;";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	char buffer[20480] = {'\0'};
	ProtocolMemWriter mw(buffer,sizeof(buffer));
	employee_.serialize(&mw);

DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Statement stmt =dbc->compileStatement(pCode);
	stmt.bind(1,employee_.properties_[PT_FightingForce]);
	stmt.bind(2,(unsigned char*)buffer,mw.length());
	stmt.bind(3,employee_.instId_);
	stmt.execDML();
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	prep_stmt->setInt(1,employee_.properties_[PT_FightingForce]);
	sql::SQLString binString(buffer,mw.length());
	prep_stmt->setString(2,binString);
	prep_stmt->setInt(3,employee_.instId_);
	prep_stmt->executeUpdate();
#endif

DB_EXEC_UNGUARD_RETURN	

	return 0;
}

U32
UpdateEmployee::back()
{
	//ACE_DEBUG((LM_INFO,ACE_TEXT("Update Employee ok!!! [%d]\n"),employee_.instId_));
	return 0;
}

U32 
QueryEmployeeByFF::go(SQLTask *pTask)
{
	//ACE_DEBUG((LM_DEBUG ,"QueryBabyByFF go\n"));
	std::stringstream sstream;
	sstream << "SELECT * FROM Employee WHERE OwnerName NOT LIKE '++%' ORDER BY EmployeeGrade DESC LIMIT 1000;";

	U32 rank = 0;

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
		CppSQLite3Query q = dbc->execQuery(sstream.str().c_str());

	if(!q.eof())
	{
		while(!q.eof())
		{
			S32 len=0;
			const unsigned char* pCacheBlob= q.getBlobField("BinData",len);
			SRV_ASSERT(len);
			ProtocolMemReader mr(pCacheBlob,len);
			COM_EmployeeInst inst;
			inst.deserialize(&mr);

			COM_EmployeeRankData info;
			info.instId_ = inst.instId_;
			info.rank_ = ++rank;
			info.name_ = inst.instName_;
			info.ownerName_ = inst.ownerName_;
			info.ff_ = inst.properties_[PT_FightingForce];
			Server::instance()->employeeFFrankCache_.push_back(info);
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
		COM_EmployeeInst inst;
		inst.deserialize(&mr);

		COM_EmployeeRankData info;
		info.instId_ = res->getInt("EmployeeGuid");
		info.rank_ = ++rank;
		info.name_ = inst.instName_;
		info.ownerName_ = res->getString("OwnerName");
		info.ff_ = inst.properties_[PT_FightingForce];
		Server::instance()->employeeFFrankCache_.push_back(info);
	}

#endif

	DB_EXEC_UNGUARD_RETURN
		return 0;
}

//-----------------------------------------------------------------------
U32
QueryEmployeeById::go(SQLTask *pTask)
{
	std::stringstream sstream;
	sstream << "SELECT * FROM Employee WHERE EmployeeGuid = \"" << employeeInstId_ << "\"";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Query q = dbc->execQuery(sstream.str().c_str());
	hasEmployee_ = false;
	if(!q.eof())
	{
		while(!q.eof())
		{
			S32 len=0;
			const unsigned char* pCacheBlob= q.getBlobField("BinData",len);
			SRV_ASSERT(len);
			ProtocolMemReader mr(pCacheBlob,len);
			COM_EmployeeInst inst;
			inst.deserialize(&mr);
			employee_ = inst;
			hasEmployee_ = true;
			q.nextRow();
		}
	}
#elif  defined(USE_MYSQL)
		std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(sstream.str().c_str()));

	while(res->next())
	{
		S32 len=0;
		sql::SQLString pCacheBlob= res->getString("BinData");
		ProtocolMemReader mr(pCacheBlob->c_str(),pCacheBlob->length());
		COM_EmployeeInst inst;
		inst.deserialize(&mr);
		inst.instId_ = res->getInt("EmployeeGuid");
		inst.ownerName_ = res->getString("OwnerName");
		employee_ = inst;
		hasEmployee_ = true;
	}

#endif

	DB_EXEC_UNGUARD_RETURN
		return 0;
}

U32
QueryEmployeeById::back()
{
	if(hasEmployee_)
		WorldHandler::instance()->queryEmployeeByIdOK(playerName_,employee_);
	return 0;
}

//////////////////////////////////伙伴任务////////////////////////////////////////

U32 InsertEmployeeQuest::go(SQLTask *pTask)
{
	enum {BUFFER_SIZE = 32*1024};
	char *buffer = new char[BUFFER_SIZE];
	DB_EXEC_GUARD
		const char* pCode = "REPLACE INTO EmployeeQuestTable(PlayerId,BinData)"  
		"values(?,?);";
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
#ifdef USE_SQLITE
	CppSQLite3Statement stmt = dbc->compileStatement(pCode);
	stmt.bind( 1 , playerID_);
	ProtocolMemWriter mw(buffer,BUFFER_SIZE);
	data_.serialize(&mw);
	stmt.bind( 2 , (unsigned char*)buffer,mw.length());
	stmt.execDML();
#else
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	prep_stmt->setInt( 1 , int(playerID_) );
	//char buffer[BUFFER_SIZE] = {'\0'};
	ProtocolMemWriter mw(buffer,BUFFER_SIZE);
	data_.serialize(&mw);
	ACE_DEBUG((LM_DEBUG,ACE_TEXT("PLAYER[%d] REPLACE EMPLOYEE QUEST QUESTSIZE[%d]\n"),data_.playerId_,data_.quests_.size()));
	sql::SQLString binString(buffer,mw.length());
	prep_stmt->setString(2,binString);
	prep_stmt->executeUpdate();
#endif
	delete []buffer;
	return 0;
	DB_EXEC_UNGUARD_RETURN
}

U32 InsertEmployeeQuest::back(){return 0;}

U32
FetchEmployeeQuest::go(SQLTask *pTask)
{
	//ACE_DEBUG((LM_DEBUG ,"FetchEmployeeQuest go\n"));
	std::stringstream sstream;
	sstream << "SELECT * FROM EmployeeQuestTable";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
		CppSQLite3Query q = dbc->execQuery(sstream.str().c_str());
	if(!q.eof())
	{
		while(!q.eof())
		{
			S32 len=0;
			const unsigned char* pCacheBlob= q.getBlobField("BinData",len);
			SRV_ASSERT(len);
			ProtocolMemReader mr(pCacheBlob,len);
			SGE_PlayerEmployeeQuest inst;
			inst.deserialize(&mr);
			Server::instance()->employeeQuestCache_.push_back(inst);
			q.nextRow();
		}
	}
#elif  defined(USE_MYSQL)
		std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(sstream.str().c_str()));
	while(res->next())
	{
		S32 len=0;
		sql::SQLString pCacheBlob= res->getString("BinData");
		ProtocolMemReader mr(pCacheBlob->c_str(),pCacheBlob->length());
		SGE_PlayerEmployeeQuest inst;
		inst.deserialize(&mr);
		Server::instance()->employeeQuestCache_.push_back(inst);
	}

#endif

	DB_EXEC_UNGUARD_RETURN
		return 0;
}

U32
DelEmployeeQuest::go(SQLTask *pTask)
{
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);

	DB_EXEC_GUARD
		std::stringstream delStr;
	delStr<<"DELETE FROM EmployeeQuestTable WHERE PlayerId="<<playerId_<<" ";
#ifdef USE_SQLITE
	CppSQLite3Statement delStmt=dbc->compileStatement(delStr.str().c_str());
	delStmt.execDML();
#else

	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(delStr.str().c_str()));
	prep_stmt->execute();
#endif
	DB_EXEC_UNGUARD_RETURN
		return 0; 
}