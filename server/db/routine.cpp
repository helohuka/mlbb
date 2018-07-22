
#include "config.h"
#include "routine.h"
#include "sqltask.h"
#include "server.h"
#include "Logger.h"

bool CheckSamePlayerName(SQLTask *pTask,std::string const& name){
	std::stringstream ss;
	ss << "SELECT PlayerGuid FROM Player WHERE PlayerName = \"" <<  name << "\" ;";
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Query q = dbc->execQuery(ss.str().c_str());
	if(!q.eof()){
		return true;
	}
#elif defined(USE_MYSQL)
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(ss.str().c_str()));
	if(res->next()){
		return true;
	}
#endif
		DB_EXEC_UNGUARD_RETURN
	return false;
}

U32
InitSQLTables::go(SQLTask* pTask)
{
	static const char* PlayerTableDDL = 
"CREATE TABLE IF NOT EXISTS Player("
"UserName	VARCHAR(60) NOT NULL,"
"PlayerName VARCHAR(60) NOT NULL,"
"PlayerGuid INT NOT NULL,"
"PlayerLevel INT NOT NULL,"
"PlayerProfession INT NOT NULL,"
"PlayerGrade INT NOT NULL,"
"Money INT NOT NULL,"
"Diamond INT NOT NULL,"
"Magic INT NOT NULL,"
"LogoutTime BIGINT NOT NULL,"
"BinData	BLOB	 NOT NULL,"
"Seal	INT	 NOT NULL,"
"Freeze	INT	 NOT NULL,"
"InDoorId	INT	 NOT NULL,"
"VersionNumber	INT	 NOT NULL,"
"PRIMARY KEY(PlayerGuid)"
");";

	static const char* EndlessStairTableDDL = 
"CREATE TABLE IF NOT EXISTS EndlessStair("
"PlayerName	VARCHAR(60) NOT NULL,"
"Rank INT NOT NULL,"
"PRIMARY KEY(Rank)"
");";

	static const char* BabyTableDDL = 
"CREATE TABLE IF NOT EXISTS Baby("
"BabyGuid INT NOT NULL,"
"BabyName	VARCHAR(60) NOT NULL,"
"OwnerName	VARCHAR(60) NOT NULL,"
"BabyGrade INT NOT NULL,"
"BabyLevel INT NOT NULL,"
"TableID INT NOT NULL,"
"AddProp INT NOT NULL,"
"BinData	BLOB	 NOT NULL,"
"PRIMARY KEY(BabyGuid)"
");";

	static const char* EmployeeTableDDL = 
"CREATE TABLE IF NOT EXISTS Employee("
"EmployeeGuid INT NOT NULL,"
"EmployeeName	VARCHAR(60) NOT NULL,"
"OwnerName	VARCHAR(60) NOT NULL,"
"EmployeeGrade INT NOT NULL,"
"BinData	BLOB	 NOT NULL,"
"PRIMARY KEY(EmployeeGuid)"
");";

	static const char* MailTableDDL = 
"CREATE TABLE IF NOT EXISTS Mail("
#if defined(USE_SQLITE)
"MailGuid INTEGER NOT NULL,"
#else
"MailGuid INT NOT NULL AUTO_INCREMENT,"
#endif
"RecvName VARCHAR(60) NOT NULL,"
"SendTime BIGINT NOT NULL,"
"ItemNum	INT NOT NULL,"
"BinData	BLOB	 NOT NULL,"
"PRIMARY KEY(MailGuid)"
");";

	static const char* GuildTableDDL = 
"CREATE TABLE IF NOT EXISTS Guild("		    
"GuildId	    INT NOT NULL,"		
"GuildName		TEXT NOT NULL,"	    
"GuildLevel		TINYINT NOT NULL,"	
"Contribution	INT NOT NULL,"
"Fundz			INT NOT NULL,"
"Credit			INT NOT NULL,"		
"Master			BIGINT NOT NULL,"	
"MasterName		VARCHAR(60) NOT NULL,"
"Notice			TEXT NOT NULL,"	    	    
"RequestList	BLOB NOT NULL,"      
"Buildings		BLOB NOT NULL,"
"Progenitus		BLOB NOT NULL,"
"ProgenitusPos	BLOB NOT NULL,"
"PresentNum		INT NOT NULL,"
"PRIMARY KEY(guildId)"
");";

	static const char* GuildMemberTableDDL = 
"CREATE TABLE IF NOT EXISTS GuildMember("			
"GuildId	INT NOT NULL,"			
"RoleId		BIGINT NOT NULL,"		
"Job		TINYINT NOT NULL,"	    
"Contribution INT NOT NULL,"			
"Rolelevel      TINYINT NOT NULL,"		
"JoinTime		INT NOT NULL,"			
"Proftype		INT NOT NULL,"			
"Proflevel		INT NOT NULL,"			
"OfflineTime BIGINT NOT NULL,"		
"RoleName   VARCHAR(60) NOT NULL,"	
"PRIMARY KEY(roleId)"
");";

	static const char* KeyGiftTableDDL = 
"CREATE TABLE IF NOT EXISTS KeyGiftTable("
#if defined(USE_SQLITE)
"sortId INTEGER NOT NULL,"
#else
"sortId INT NOT NULL AUTO_INCREMENT,"
#endif
"cdkey   VARCHAR(60) NOT NULL,"
"pfIp   VARCHAR(60) NOT NULL,"
"giftName   VARCHAR(60) NOT NULL,"
"playerName   VARCHAR(60) NOT NULL,"
"useTime BIGINT NOT NULL,"
"rewardItem	BLOB	 NOT NULL,"
"PRIMARY KEY(sortId)"
");";

	static const char* ActivityTableDDL = 
"CREATE TABLE IF NOT EXISTS Activity("
"ADType INT NOT NULL,"
"BinData	BLOB	 NOT NULL,"
"PRIMARY KEY(ADType)"
");";

	static const char* EmployeeQuestTableDDL = 
"CREATE TABLE IF NOT EXISTS EmployeeQuestTable("
"PlayerId INT NOT NULL,"
"BinData	BLOB	 NOT NULL,"
"PRIMARY KEY(PlayerId)"
");";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
DB_EXEC_GUARD
#if defined(USE_SQLITE)
	dbc->execDML(PlayerTableDDL);
	dbc->execDML(EndlessStairTableDDL);
	dbc->execDML(BabyTableDDL);
	dbc->execDML(EmployeeTableDDL);
	dbc->execDML(MailTableDDL);
	dbc->execDML(GuildTableDDL);
	dbc->execDML(GuildMemberTableDDL);
	dbc->execDML(ActivityTableDDL);
	dbc->execDML(EmployeeQuestTableDDL);
#elif defined(USE_MYSQL)
	dbc->prepareStatement(PlayerTableDDL);
	dbc->prepareStatement(EndlessStairTableDDL);
	dbc->prepareStatement(BabyTableDDL);
	dbc->prepareStatement(EmployeeTableDDL);
	dbc->prepareStatement(MailTableDDL);
	dbc->prepareStatement(GuildTableDDL);
	dbc->prepareStatement(GuildMemberTableDDL);
	dbc->prepareStatement(ActivityTableDDL);
	dbc->prepareStatement(EmployeeQuestTableDDL);
#endif
DB_EXEC_UNGUARD_RETURN
	return 0;
}

U32
QueryPlayerContact::go(SQLTask *pTask)
{
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	
	U32 maxGuid = 0;

	DB_EXEC_GUARD
#if defined(USE_SQLITE)
	{
		CppSQLite3Query q = dbc->execQuery("SELECT Max(PlayerGuid) AS MaxGuid FROM Player ");
		if(!q.eof()){
			maxGuid = q.getIntField("MaxGuid");
		}
	}
	{
		CppSQLite3Query q = dbc->execQuery("SELECT Max(BabyGuid) AS MaxGuid FROM Baby ");
		if(!q.eof()){
			U32 guid =  q.getIntField("MaxGuid");
			maxGuid = maxGuid > guid ? maxGuid : guid;
		}
	}
	{
		CppSQLite3Query q = dbc->execQuery("SELECT Max(EmployeeGuid) AS MaxGuid FROM Employee ");
		if(!q.eof()){
			U32 guid =  q.getIntField("MaxGuid");
			maxGuid = maxGuid > guid ? maxGuid : guid;
		}
	}
#elif defined(USE_MYSQL)
	{
		std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
		std::auto_ptr< sql::ResultSet > res(stmt->executeQuery("SELECT Max(PlayerGuid) AS MaxGuid FROM Player "));
		if(res->next()){
			maxGuid = res->getInt("MaxGuid");
		}
	}
	{
		std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
		std::auto_ptr< sql::ResultSet > res(stmt->executeQuery("SELECT Max(BabyGuid) AS MaxGuid FROM Baby "));
		if(res->next()){
			U32 guid = res->getInt("MaxGuid");
			maxGuid = maxGuid > guid ? maxGuid : guid;
		}
	}
	{
		std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
		std::auto_ptr< sql::ResultSet > res(stmt->executeQuery("SELECT Max(EmployeeGuid) AS MaxGuid FROM Employee "));
		if(res->next()){
			U32 guid = res->getInt("MaxGuid");
			maxGuid = maxGuid > guid ? maxGuid : guid;
		}
	}
#endif
	DB_EXEC_UNGUARD_RETURN
	
	Server::instance()->maxGuid_ = maxGuid;

	DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Query q = dbc->execQuery("SELECT * FROM Player ");
	if (!q.eof())
	{
		while(!q.eof())
		{
			U32 instId = q.getIntField("PlayerGuid");
			S32 len=0;
			const unsigned char* pCacheBlob= q.getBlobField("BinData",len);
			SRV_ASSERT(len);
			ProtocolMemReader mr(pCacheBlob,len);
			SGE_DBPlayerData inst;
			inst.instId_ = instId;
			inst.deserialize(&mr);

			SGE_ContactInfoExt info;
			info.accName_	=  q.getStringField("UserName");
			info.instId_	= instId;
			info.level_		= inst.properties_[PT_Level];
			info.name_		= inst.instName_;
			info.assetId_	= inst.properties_[PT_AssetId];
			info.job_		= (JobType)(int)inst.properties_[PT_Profession];
			info.jobLevel_	= inst.properties_[PT_ProfessionLevel];
			info.gold_		= inst.properties_[PT_Money];
			info.diamond_	= inst.properties_[PT_Diamond];
			info.magicgold_ = inst.properties_[PT_MagicCurrency];
			info.vip_		= (VipLevel)(int)inst.properties_[PT_VipLevel];
			info.ff_		= inst.properties_[PT_FightingForce];
			info.exp_		= inst.properties_[PT_Exp];
			info.rolefirst_ = inst.createTime_;
			info.rolelast_	= inst.loginTime_;
			info.section_	= inst.pvpInfo_.section_;
			info.value_		= inst.pvpInfo_.value_;
			info.guildContribute_ = inst.guildContribution_;
			info.pfid_ = inst.pfid_;
			info.logoutTime_ = inst.logoutTime_;
			info.serverid_ = q.getIntField("InDoorId");
			Server::instance()->contactCache_.push_back(info);
			q.nextRow();
		}
	}
#elif defined(USE_MYSQL)
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery("SELECT * FROM Player "));

	while(res->next())
	{
	
		S32 len=0;
		sql::SQLString pCacheBlob= res->getString("BinData");
		ProtocolMemReader mr(pCacheBlob->c_str(),pCacheBlob->length());
		SGE_DBPlayerData inst;
		inst.deserialize(&mr);

		SGE_ContactInfoExt info;
		info.accName_ = res->getString("UserName").c_str();
		info.instId_	= res->getInt("PlayerGuid");
		info.level_		= inst.properties_[PT_Level];
		info.name_		= res->getString("PlayerName").c_str();
		info.assetId_	= inst.properties_[PT_AssetId];
		info.job_		= (JobType)(int)inst.properties_[PT_Profession];
		info.jobLevel_	= inst.properties_[PT_ProfessionLevel];
		info.gold_		= inst.properties_[PT_Money];
		info.diamond_	= inst.properties_[PT_Diamond];
		info.magicgold_ = inst.properties_[PT_MagicCurrency];
		info.vip_		= (inst.properties_[PT_VipLevel] <= VL_None || inst.properties_[PT_VipLevel] >= VL_Max)? VL_None : (VipLevel)(int)inst.properties_[PT_VipLevel];
		info.ff_		= inst.properties_[PT_FightingForce];
		info.exp_		= inst.properties_[PT_Exp];
		info.rolefirst_ = inst.createTime_;
		info.rolelast_	= inst.loginTime_;
		info.section_	= inst.pvpInfo_.section_;
		info.value_		= inst.pvpInfo_.value_;
		info.logoutTime_ = inst.logoutTime_;
		info.guildContribute_ = inst.guildContribution_;
		info.serverid_ = res->getInt("InDoorId");
		Server::instance()->contactCache_.push_back(info);

	}
	
#endif
	DB_EXEC_UNGUARD_RETURN
return 0;

}

U32 QueryBabyCache::go(SQLTask *pTask){
	std::stringstream sstream;
	sstream << "SELECT * FROM Baby WHERE OwnerName NOT LIKE '++%';";

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
			
			Server::instance()->addBabyInst(inst);

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
		inst.instId_ = res->getInt("BabyGuid");
		inst.ownerName_ = res->getString("OwnerName");
		Server::instance()->addBabyInst(inst);
	}

#endif

	DB_EXEC_UNGUARD_RETURN
		return 0;
}

U32 QueryEmployeeCache::go(SQLTask *pTask){
	std::stringstream sstream;
	sstream << "SELECT * FROM Employee WHERE OwnerName NOT LIKE '++%';";

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

			Server::instance()->addEmployeeInst(inst);
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
		inst.instId_ = res->getInt("EmployeeGuid");
		inst.ownerName_ = res->getString("OwnerName");
		Server::instance()->addEmployeeInst(inst);
	}

#endif

	DB_EXEC_UNGUARD_RETURN
		return 0;
}

U32 InstertChargeCache::go(SQLTask *pTask){
	enum {BUFFER_SIZE = 1024*1024};
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	std::stringstream ssQ;
	ssQ << "SELECT * FROM Player WHERE PlayerGuid = " << playerId_ << ";";
DB_EXEC_GUARD
#if defined(USE_MYSQL)
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(ssQ.str().c_str()));
	if(res->next())
	{
		S32 len=0;
		sql::SQLString pCacheBlob= res->getString("BinData");
		
		ProtocolMemReader mr(pCacheBlob->c_str(),pCacheBlob->length());
		
		SGE_DBPlayerData inst;
		inst.deserialize(&mr);
		inst.orders_.push_back(order_);

		char buffer[BUFFER_SIZE] = {'\0'};
		ProtocolMemWriter mw(buffer,BUFFER_SIZE);
		inst.serialize(&mw);

		std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement("UPDATE Player SET BinData = ? WHERE PlayerGuid = ?"));
		
		sql::SQLString binString(buffer,mw.length());
		prep_stmt->setString(1,binString);
		prep_stmt->setInt(2,playerId_);
		prep_stmt->executeUpdate();
	}else{
		ACE_DEBUG((LM_ERROR,"Can not find player for insert charge %d\n",playerId_));
	}
#endif
	DB_EXEC_UNGUARD_RETURN
return 0;
}