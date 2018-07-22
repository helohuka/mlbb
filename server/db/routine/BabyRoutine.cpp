#include "config.h"
#include "../routine.h"
#include "../handler.h"
#include "../sqltask.h"

U32
InsertBaby::go(SQLTask *pTask)
{
	//ACE_DEBUG((LM_DEBUG,"DB INSERT BABY\n"));
	static const char * pCode =
		"INSERT INTO Baby(BabyGuid,BabyName,BinData,OwnerName,BabyGrade,BabyLevel,TableID,AddProp)"
		"VALUES(?,?,?,?,?,?,?,?);";
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);

	char buffer[20480] = {'\0'};
	ProtocolMemWriter mw(buffer,sizeof(buffer));
	baby_.serialize(&mw);
	
	F32 addprop = 0.F;
	for (size_t i=0; i< baby_.addprop_.size(); ++i){
		addprop += baby_.addprop_[i];
	}

DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Statement stmt =dbc->compileStatement(pCode);
	stmt.bind(1,baby_.instId_);
	stmt.bind(2,baby_.instName_.c_str());	
	stmt.bind(3,(unsigned char*)buffer,mw.length());
	stmt.bind(4,baby_.ownerName_.c_str());
	stmt.bind(5,baby_.properties_[PT_FightingForce]);
	stmt.bind(6,baby_.properties_[PT_Level]);
	stmt.bind(7,baby_.properties_[PT_TableId]);
	stmt.bind(8,int(addprop));
	stmt.execDML();
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	prep_stmt->setInt(1,baby_.instId_);
	prep_stmt->setString(2,baby_.instName_.c_str());
	sql::SQLString binString(buffer,mw.length());
	prep_stmt->setString(3,binString);
	prep_stmt->setString(4,baby_.ownerName_.c_str());
	prep_stmt->setInt(5,baby_.properties_[PT_FightingForce]);
	prep_stmt->setInt(6,baby_.properties_[PT_Level]);
	prep_stmt->setInt(7,baby_.properties_[PT_TableId]);
	prep_stmt->setInt(8,int(addprop));
	prep_stmt->executeUpdate();
#endif

DB_EXEC_UNGUARD_RETURN
	return 0;
}

U32
InsertBaby::back()
{
	//ACE_DEBUG((LM_DEBUG,"DB INSERT BABY BK\n"));
	Server::instance()->addBabyInst(baby_);
	WorldHandler::instance()->createBabyOK(playername_,baby_,isToStorage_);
	return 0;
}

//--------------------------------------------------------------------

U32
DeleteBaby::go(SQLTask *pTask)
{
	ACE_DEBUG((LM_DEBUG,ACE_TEXT("Delete baby %d \n"),babyId_));
	std::stringstream ss;
	ss << "UPDATE Baby SET OwnerName='++" << playername_ << "' WHERE BabyGuid=" << babyId_ <<";"; 
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);

DB_EXEC_GUARD
	
#ifdef USE_SQLITE
	dbc->execDML(ss.str().c_str());	
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(ss.str().c_str()));
	prep_stmt->execute();
#endif

DB_EXEC_UNGUARD_RETURN
	return 0;
}

U32 
DeleteBaby::back()
{
	Server::instance()->delBabyInst(babyId_);
	WorldHandler::instance()->deleteBabyOK(playername_,babyId_);
	return 0;
}

//----------------------------------------------------------------------------

U32
UpdateBaby::go(SQLTask *pTask)
{
	static const char * pCode =
		"UPDATE Baby SET BabyName=?,BabyGrade=?,BinData=?,BabyLevel=?,AddProp=?,TableID=? WHERE BabyGuid=?;";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	char buffer[20480] = {'\0'};
	ProtocolMemWriter mw(buffer,sizeof(buffer));
	baby_.serialize(&mw);
	F32 addprop = 0;
	for (size_t i=0;i<baby_.addprop_.size();++i){
		if(baby_.addprop_[i]<0)
			ACE_DEBUG((LM_DEBUG,ACE_TEXT("zhenimazhenchedan baby_.addprop_[%d] \n"),baby_.addprop_[i]));
		addprop += baby_.addprop_[i];
	}
DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Statement stmt =dbc->compileStatement(pCode);
	stmt.bind(1,baby_.instName_.c_str());
	stmt.bind(2,baby_.properties_[PT_FightingForce]);
	stmt.bind(3,(unsigned char*)buffer,mw.length());
	stmt.bind(4,baby_.properties_[PT_Level]);
	stmt.bind(5,int(addprop));
	stmt.bind(6,baby_.properties_[PT_TableId]);
	stmt.bind(7,baby_.instId_);
	stmt.execDML();
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	prep_stmt->setString(1,baby_.instName_);
	prep_stmt->setInt(2,baby_.properties_[PT_FightingForce]);
	sql::SQLString binString(buffer,mw.length());
	prep_stmt->setString(3,binString);
	prep_stmt->setInt(4,baby_.properties_[PT_Level]);
	prep_stmt->setInt(5,int(addprop));
	prep_stmt->setInt(6,baby_.properties_[PT_TableId]);
	prep_stmt->setInt(7,baby_.instId_);
	prep_stmt->executeUpdate();
#endif
	//ACE_DEBUG((LM_DEBUG,ACE_TEXT("UPDATE BABY[%d] SLOT[%d]\n"),baby_.instId_,baby_.slot_));
DB_EXEC_UNGUARD_RETURN	

	return 0;
}

U32
UpdateBaby::back()
{
	//ACE_DEBUG((LM_INFO,ACE_TEXT("Update baby ok!!! [%d]\n"),baby_.instId_));
	return 0;
}

//----------------------------------------------------------------------------

U32
ResetBabyOwner::go(SQLTask *pTask)
{
	static const char * pCode ="UPDATE Baby SET OwnerName=? WHERE BabyGuid=?;";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);

	DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Statement stmt =dbc->compileStatement(pCode);
	stmt.bind(1,playerName_.c_str());
	stmt.bind(2,babyId_);
	stmt.execDML();
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	prep_stmt->setString(1,playerName_.c_str());
	prep_stmt->setInt(2,int(babyId_));
	prep_stmt->executeUpdate();
#endif

	DB_EXEC_UNGUARD_RETURN	

		return 0;
}

U32
ResetBabyOwner::back()
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("Reset baby owner ok!!! %s %d\n"),playerName_.c_str(),babyId_));
	return 0;
}


U32 
QueryBabyByFF::go(SQLTask *pTask)
{
	//ACE_DEBUG((LM_DEBUG ,"QueryBabyByFF go\n"));
	std::stringstream sstream;
	sstream << "SELECT * FROM Baby WHERE OwnerName NOT LIKE '++%' ORDER BY BabyGrade DESC LIMIT 1000;";

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
			COM_BabyInst inst;
			inst.deserialize(&mr);

			COM_BabyRankData info;
			info.instId_ = inst.instId_;
			info.rank_ = ++rank;
			info.name_ = inst.instName_;
			info.ownerName_ = inst.ownerName_;
			info.ff_ = inst.properties_[PT_FightingForce];
			Server::instance()->babyFFrankCache_.push_back(info);
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
		COM_BabyInst inst;
		inst.deserialize(&mr);

		COM_BabyRankData info;
		info.instId_ = res->getInt("BabyGuid");
		info.rank_ = ++rank;
		info.name_ = inst.instName_;
		info.ownerName_ = res->getString("OwnerName");
		info.ff_ = inst.properties_[PT_FightingForce];
		Server::instance()->babyFFrankCache_.push_back(info);
	}

#endif

	DB_EXEC_UNGUARD_RETURN
		return 0;
}

//-----------------------------------------------------------------------
U32
QueryBabyById::go(SQLTask *pTask)
{
	//ACE_DEBUG((LM_DEBUG ,"QueryPlayerById go\n"));
	std::stringstream sstream;
	sstream << "SELECT * FROM Baby WHERE BabyGuid = \"" << babyInstId_ << "\"";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
		CppSQLite3Query q = dbc->execQuery(sstream.str().c_str());
	hasBaby_ = false;
	if(!q.eof())
	{
		while(!q.eof())
		{
			S32 len=0;
			const unsigned char* pCacheBlob= q.getBlobField("BinData",len);
			SRV_ASSERT(len);
			ProtocolMemReader mr(pCacheBlob,len);
			COM_BabyInst inst;
			inst.deserialize(&mr);
			baby_ = inst;
			hasBaby_ = true;
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
		COM_BabyInst inst;
		inst.deserialize(&mr);
		inst.instId_ = res->getInt("BabyGuid");
		inst.ownerName_ = res->getString("OwnerName");
		baby_ = inst;
		hasBaby_ = true;
	}

#endif

	DB_EXEC_UNGUARD_RETURN
		return 0;
}

U32
QueryBabyById::back()
{
	if(hasBaby_)
		WorldHandler::instance()->queryBabyByIdOK(playerName_,baby_);
	return 0;
}

U32
UpdateBabyslot::go(SQLTask *pTask){
	
	static const char * pCode =
		"UPDATE Baby SET BabyName=?,BabyGrade=?,BinData=? WHERE BabyGuid=?;";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
DB_EXEC_GUARD
	char buffer[20480] = {'\0'};
	for (size_t i=0; i<babys_.size();++i)
	{
		ProtocolMemWriter mw(buffer,sizeof(buffer));
		babys_[i].serialize(&mw);
#if defined(USE_SQLITE)
		CppSQLite3Statement stmt =dbc->compileStatement(pCode);
		stmt.bind(1,babys_[i].instName_.c_str());
		stmt.bind(2,babys_[i].properties_[PT_FightingForce]);
		stmt.bind(3,(unsigned char*)buffer,mw.length());
		stmt.bind(4,babys_[i].instId_);
		stmt.execDML();
#elif  defined(USE_MYSQL)
		std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
		prep_stmt->setString(1,babys_[i].instName_);
		prep_stmt->setInt(2,babys_[i].properties_[PT_FightingForce]);
		sql::SQLString binString(buffer,mw.length());
		prep_stmt->setString(3,binString);
		prep_stmt->setInt(4,babys_[i].instId_);
		prep_stmt->executeUpdate();
#endif
		ACE_DEBUG((LM_DEBUG,ACE_TEXT("UPDATE BABY[%d] SLOT[%d]\n"),babys_[i].instId_,babys_[i].slot_));
	}
DB_EXEC_UNGUARD_RETURN	

	return 0;
}

U32
UpdateBabyslot::back(){
	WorldHandler::instance()->UpdateBabysOK(playerName_);
	return 0;
}