#include "config.h"
#include "../routine.h"
#include "../handler.h"
#include "../server.h"
#include "../sqltask.h"

U32
FetchGuild::go(SQLTask *pTask)
{
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);

	DB_EXEC_GUARD
		std::string str="SELECT * FROM Guild";
#ifdef USE_SQLITE
	CppSQLite3Query q = dbc->execQuery(str.c_str());
	while (!q.eof())
	{
		COM_Guild	guild;
		guild.guildId_   = q.getIntField("GuildId");
		guild.guildLevel_= q.getIntField("GuildLevel");
		guild.guildName_ = q.getStringField("GuildName");
		guild.guildContribution_ = q.getIntField("Contribution");
		guild.fundz_ = q.getIntField("Fundz");
		guild.master_    = q.getInt64Field("Master");
		guild.masterName_=q.getStringField("MasterName");
		guild.notice_	 = q.getStringField("Notice");
		guild.createTime_ =q.getIntField("Credit");

		int len=0;


		const unsigned char* pBlob = q.getBlobField("RequestList" , len);
		ProtocolMemReader protocol1((S8*)pBlob , len);
		protocol1.readVector(guild.requestList_);

		pBlob = q.getBlobField("Buildings" , len);
		ProtocolMemReader protocol2((S8*)pBlob , len);
		protocol2.readVector(guild.buildings_);

		pBlob = q.getBlobField("Progenitus" , len);
		ProtocolMemReader protocol3((S8*)pBlob , len);
		protocol3.readVector(guild.progenitus_);

		pBlob = q.getBlobField("ProgenitusPos" , len);
		ProtocolMemReader protocol4((S8*)pBlob , len);
		protocol4.readVector(guild.progenitusPositions_);
		
		guild.presentNum_ =q.getIntField("PresentNum");

		guilds_.push_back(guild);

		q.nextRow();
	}
#else
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(str.c_str()));
	while (res->next()) 
	{
		COM_Guild	guild;
		guild.guildId_   = res->getInt("GuildId");
		guild.guildLevel_= res->getInt("GuildLevel");
		guild.guildName_ = res->getString("GuildName");
		guild.guildContribution_ = res->getInt("Contribution");
		guild.fundz_ = res->getInt("Fundz");
		guild.master_    = res->getInt64("Master");
		guild.masterName_=res->getString("MasterName");
		guild.notice_	 = res->getString("Notice");
		guild.createTime_=res->getInt("Credit");

		/*sql::SQLString tmpBlob=res->getString("GuildSkills");
		const  char * pBlob= tmpBlob.c_str();
		ProtocolMemReader protocol( (S8*)pBlob , tmpBlob.length() );
		protocol.readVector(guild.guildSkills_);*/

		sql::SQLString tmpBlob = res->getString("RequestList");
		const  char * pBlob= tmpBlob.c_str();
		ProtocolMemReader protocol1((S8*)pBlob , tmpBlob.length());
		protocol1.readVector(guild.requestList_);

		tmpBlob = res->getString("Buildings" );
		pBlob= tmpBlob.c_str();
		ProtocolMemReader protocol2((S8*)pBlob , tmpBlob.length());
		protocol2.readVector(guild.buildings_);

		tmpBlob = res->getString("Progenitus");
		pBlob= tmpBlob.c_str();
		ProtocolMemReader protocol3((S8*)pBlob, tmpBlob.length());
		protocol3.readVector(guild.progenitus_);

		tmpBlob = res->getString("ProgenitusPos");
		pBlob= tmpBlob.c_str();
		ProtocolMemReader protocol4((S8*)pBlob , tmpBlob.length());
		protocol4.readVector(guild.progenitusPositions_);

		guild.presentNum_ =res->getInt("PresentNum");

		guilds_.push_back(guild);
	}
#endif
	return 0;

	DB_EXEC_UNGUARD_RETURN
}

U32
FetchGuild::back()
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("Fetch Guild  back\n")));

	WorldHandler::instance()->syncGuild(guilds_);

	return 0;
}

U32
FetchGuildMember::go(SQLTask *pTask)
{
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);

	DB_EXEC_GUARD
		std::string str="SELECT * FROM GuildMember ";
#ifdef USE_SQLITE
	CppSQLite3Query q = dbc->execQuery(str.c_str());
	while (!q.eof())
	{
		COM_GuildMember	guildMember;
		guildMember.guildId_ = q.getIntField("GuildId");
		guildMember.roleId_ = q.getInt64Field("RoleId");
		guildMember.job_ = GuildJob(q.getIntField("Job"));
		guildMember.contribution_ = q.getIntField("Contribution");
		guildMember.level_ = q.getIntField("Rolelevel");
		guildMember.offlineTime_ = q.getInt64Field("OfflineTime");
		guildMember.roleName_ = q.getStringField("RoleName");
		guildMember.joinTime_ = q.getIntField("JoinTime");
		guildMember.profType_ = q.getIntField("Proftype");
		guildMember.profLevel_ = q.getIntField("Proflevel");

		guildMembers_.push_back(guildMember);

		q.nextRow();
	}
	return 0;
#else
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(str.c_str()));
	while (res->next()) 
	{
		COM_GuildMember	guildMember;
		guildMember.guildId_  = res->getInt("GuildId");
		guildMember.roleId_   = res->getInt64("RoleId");
		guildMember.job_ = GuildJob(res->getInt("Job"));
		guildMember.contribution_ = res->getInt("Contribution");
		guildMember.level_	  = res->getInt("Rolelevel");
		guildMember.offlineTime_ = res->getInt64("OfflineTime");
		guildMember.roleName_ = res->getString("RoleName");
		guildMember.joinTime_ = SexType(res->getInt("JoinTime"));
		guildMember.profType_ = res->getInt("Proftype");
		guildMember.profLevel_ = res->getInt("Proflevel");

		guildMembers_.push_back(guildMember);
	}
	return 0;

#endif
	DB_EXEC_UNGUARD_RETURN
}

U32 FetchGuildMember::back()
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("Fetch GuildMember  back\n")));

	WorldHandler::instance()->syncGuildMember(guildMembers_);

	return 0;
}

//------------------------------------------------------上面是获取数据下面是保存数据-------------------------------------------------------------------

U32 InsertGuild::go(SQLTask *pTask)
{
	DB_EXEC_GUARD
	const char* pGuildCode = "INSERT INTO Guild(GuildId,GuildName,GuildLevel,Contribution,Fundz,Credit,Master,MasterName,Notice,RequestList,Buildings,Progenitus,ProgenitusPos,PresentNum)"
		"values(?,?,?,?,?,?,?,?,?,?,?,?,?,?) ;";
	const char* pMemberCode = "INSERT INTO GuildMember(GuildId,RoleId,Job,Contribution,Rolelevel,JoinTime,Proftype,Proflevel,OfflineTime,RoleName)"
		"values(?,?,?,?,?,?,?,?,?,?) ;";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);

#ifdef USE_SQLITE
	CppSQLite3Statement stmt = dbc->compileStatement(pGuildCode);
	stmt.bind( 1 , int(guild_.guildId_) );
	stmt.bind( 2 , guild_.guildName_.c_str());
	stmt.bind( 3 , guild_.guildLevel_);
	stmt.bind( 4 , 0);
	stmt.bind( 5 , guild_.fundz_);
	stmt.bind( 6 , 0);
	stmt.bind( 7 , guild_.master_);
	stmt.bind( 8 , guild_.masterName_.c_str());
	stmt.bind( 9 , guild_.notice_.c_str());

	unsigned char buffer1[sizeof(COM_GuildRequestData)*32] = {0};
	ProtocolMemWriter protocol1((S8*)buffer1 , sizeof(buffer1));
	bool checker = protocol1.writeVector(guild_.requestList_);
	SRV_ASSERT(checker);
	stmt.bind( 10 , (unsigned char*)buffer1 , protocol1.length());

	unsigned char buffer2[sizeof(COM_GuildBuilding)*32] = {0};
	ProtocolMemWriter protocol2((S8*)buffer2 , sizeof(buffer2));
	checker = protocol2.writeVector(guild_.buildings_);
	SRV_ASSERT(checker);
	stmt.bind( 11 , (unsigned char*)buffer2 , protocol2.length());

	unsigned char buffer3[sizeof(COM_GuildProgen)*32] = {0};
	ProtocolMemWriter protocol3((S8*)buffer3 , sizeof(buffer3));
	checker = protocol3.writeVector(guild_.progenitus_);
	SRV_ASSERT(checker);
	stmt.bind( 12 , (unsigned char*)buffer3 , protocol3.length());

	unsigned char buffer4[sizeof(S32)*32] = {0};
	ProtocolMemWriter protocol4((S8*)buffer4 , sizeof(buffer4));
	checker = protocol4.writeVector(guild_.progenitusPositions_);
	SRV_ASSERT(checker);
	stmt.bind( 13 , (unsigned char*)buffer4 , protocol4.length());
	stmt.bind( 14 , 0);
	stmt.execDML();

	stmt = dbc->compileStatement(pMemberCode);
	stmt.bind( 1 , int(guildMember_.guildId_) );
	stmt.bind( 2 , guildMember_.roleId_);
	stmt.bind( 3 , guildMember_.job_);
	stmt.bind( 4 , guildMember_.contribution_);
	stmt.bind( 5 , guildMember_.level_);
	stmt.bind( 6 , int(guildMember_.joinTime_));
	stmt.bind( 7 , int(guildMember_.profType_));
	stmt.bind( 8 , int(guildMember_.profLevel_));
	stmt.bind( 9 , int(guildMember_.offlineTime_));
	stmt.bind( 10 , guildMember_.roleName_.c_str());

	stmt.execDML();
#else
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pGuildCode));
	prep_stmt->setInt( 1 , int(guild_.guildId_) );
	prep_stmt->setString( 2 , guild_.guildName_.c_str());
	prep_stmt->setInt( 3 , guild_.guildLevel_);
	prep_stmt->setInt( 4 , 0);
	prep_stmt->setInt( 5 , guild_.fundz_);
	prep_stmt->setInt( 6 , 0);
	prep_stmt->setInt64( 7 , guild_.master_);
	prep_stmt->setString( 8 , guild_.masterName_.c_str());
	prep_stmt->setString( 9 , guild_.notice_.c_str());

	char buffer1[sizeof(COM_GuildRequestData)*32] = {0};
	ProtocolMemWriter protocol1((S8*)buffer1 , sizeof(buffer1));
	bool checker = protocol1.writeVector(guild_.requestList_);
	SRV_ASSERT(checker);
	sql::SQLString tmpStr1(buffer1,protocol1.length());
	prep_stmt->setString( 10 ,tmpStr1 );

	char buffer2[sizeof(COM_GuildBuilding)*32] = {0};
	ProtocolMemWriter protocol2((S8*)buffer2 , sizeof(buffer2));
	checker = protocol2.writeVector(guild_.buildings_);
	SRV_ASSERT(checker);
	sql::SQLString tmpStr2(buffer2,protocol2.length());
	prep_stmt->setString( 11 , tmpStr2);

	char buffer3[sizeof(COM_GuildProgen)*32] = {0};
	ProtocolMemWriter protocol3((S8*)buffer3 , sizeof(buffer3));
	checker = protocol3.writeVector(guild_.progenitus_);
	SRV_ASSERT(checker);
	sql::SQLString tmpStr3(buffer3,protocol3.length());
	prep_stmt->setString( 12 , tmpStr3);

	char buffer4[sizeof(S32)*32] = {0};
	ProtocolMemWriter protocol4((S8*)buffer4 , sizeof(buffer4));
	checker = protocol4.writeVector(guild_.progenitusPositions_);
	SRV_ASSERT(checker);
	sql::SQLString tmpStr4(buffer4,protocol4.length());
	prep_stmt->setString( 13 , tmpStr4);
	prep_stmt->setInt( 14 , 0);

	prep_stmt->execute();

	prep_stmt.reset(dbc->prepareStatement(pMemberCode));
	prep_stmt->setInt( 1 , int(guildMember_.guildId_) );
	prep_stmt->setInt64( 2 , guildMember_.roleId_);
	prep_stmt->setInt( 3 , guildMember_.job_);
	prep_stmt->setInt( 4 , guildMember_.contribution_);
	prep_stmt->setInt( 5 , guildMember_.level_);
	prep_stmt->setInt( 6 , int(guildMember_.joinTime_));
	prep_stmt->setInt( 7 , int(guildMember_.profType_));
	prep_stmt->setInt( 8 , int(guildMember_.profLevel_));
	prep_stmt->setInt( 9 , int(guildMember_.offlineTime_));
	prep_stmt->setString( 10 , guildMember_.roleName_.c_str());
	prep_stmt->execute();

#endif

	return 0;
	DB_EXEC_UNGUARD_RETURN

}

U32
InsertGuild::back()
{
	WorldHandler::instance()->insertGuildOK(guild_,guildMember_);
	return 0;
}

U32
RefreshGuildRequest::go(SQLTask *pTask)
{
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD
	std::stringstream strS;
	strS<<"UPDATE Guild SET RequestList=? WHERE GuildId="<<guildId_<<";";
#ifdef USE_SQLITE
	CppSQLite3Statement stmt = dbc->compileStatement(strS.str().c_str());

	unsigned char buffer1[sizeof(COM_GuildRequestData)*32] = {0};
	ProtocolMemWriter protocol((S8*)buffer1 , sizeof(buffer1));
	bool checker = protocol.writeVector(requestList_);
	SRV_ASSERT(checker);
	stmt.bind(1 , (unsigned char*)buffer1 , protocol.length());
	stmt.execDML();
#else
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(strS.str().c_str()));
	char buffer1[sizeof(COM_GuildRequestData)*32] = {0};
	ProtocolMemWriter protocol((S8*)buffer1 , sizeof(buffer1));
	bool checker = protocol.writeVector(requestList_);
	SRV_ASSERT(checker);
	sql::SQLString tmpStr(buffer1 , protocol.length());
	prep_stmt->setString(1 ,tmpStr );
	prep_stmt->execute();

#endif
	return 0;
	DB_EXEC_UNGUARD_RETURN

}

U32
CreateGuildMember::go(SQLTask *pTask)
{
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);

	DB_EXEC_GUARD
		const char* pCode = "INSERT INTO  GuildMember(GuildId,RoleId,Job,Contribution,Rolelevel,JoinTime,Proftype,Proflevel,OfflineTime,RoleName) values(?,?,?,?,?,?,?,?,?,?) ;";
#ifdef USE_SQLITE
	CppSQLite3Statement stmt = dbc->compileStatement(pCode);
	stmt.bind( 1 , int(guildMember_.guildId_) );
	stmt.bind( 2 , guildMember_.roleId_);
	stmt.bind( 3 , guildMember_.job_);
	stmt.bind( 4 , guildMember_.contribution_);
	stmt.bind( 5 , guildMember_.level_);
	stmt.bind( 6 , int(guildMember_.joinTime_));
	stmt.bind( 7 , int(guildMember_.profType_));
	stmt.bind( 8 , int(guildMember_.profLevel_));
	stmt.bind( 9 , int(guildMember_.offlineTime_));
	stmt.bind( 10 , guildMember_.roleName_.c_str());
	stmt.execDML();
#else
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	prep_stmt->setInt( 1 , int(guildMember_.guildId_) );
	prep_stmt->setInt64( 2 , guildMember_.roleId_);
	prep_stmt->setInt( 3 , guildMember_.job_);
	prep_stmt->setInt( 4 , guildMember_.contribution_);
	prep_stmt->setInt( 5 , guildMember_.level_);
	prep_stmt->setInt( 6 , int(guildMember_.joinTime_));
	prep_stmt->setInt( 7 , int(guildMember_.profType_));
	prep_stmt->setInt( 8 , int(guildMember_.profType_));
	prep_stmt->setInt( 9 , int(guildMember_.offlineTime_));
	prep_stmt->setString( 10 , guildMember_.roleName_.c_str());
	prep_stmt->execute();

#endif
	return 0;
	DB_EXEC_UNGUARD_RETURN
}

U32
DelGuild::go(SQLTask *pTask)
{
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);

	DB_EXEC_GUARD
	std::stringstream delStr;
	delStr<<"DELETE FROM Guild WHERE GuildId="<<guildId_<<" ";
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

U32
UpdateGuildNotice::go(SQLTask *pTask)
{
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);

	DB_EXEC_GUARD
		char const * pCode = "UPDATE Guild SET Notice=? WHERE GuildId=?";
#ifdef USE_SQLITE
	CppSQLite3Statement stmt = dbc->compileStatement(pCode);

	stmt.bind(1 , notice_.c_str());
	stmt.bind(2 , int(guildId_));

	stmt.execDML();
#else
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	prep_stmt->setString(1 , notice_.c_str());
	prep_stmt->setInt(2 , int(guildId_));
	prep_stmt->execute();

#endif
	return 0;
	DB_EXEC_UNGUARD_RETURN

}

U32
UpdateGuild::go(SQLTask *pTask)
{
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);

	DB_EXEC_GUARD
		const char* pCode = "UPDATE Guild SET GuildName=?,GuildLevel=?,Contribution=?,Fundz=?,Credit=?,Master=?,MasterName=?,Notice=?,RequestList=?,Buildings=? ,Progenitus=?,ProgenitusPos=?,PresentNum=? WHERE GuildId=?";

#ifdef USE_SQLITE
	CppSQLite3Statement stmt = dbc->compileStatement( pCode );
	stmt.bind( 1 , guild_.guildName_.c_str());
	stmt.bind( 2 , guild_.guildLevel_);
	stmt.bind( 3 , int(guild_.guildContribution_));
	stmt.bind( 4 , int(guild_.fundz_));
	stmt.bind( 5 , guild_.createTime_);
	stmt.bind( 6 , guild_.master_);
	stmt.bind( 7 , guild_.masterName_.c_str());
	stmt.bind( 8 , guild_.notice_.c_str());

	unsigned char buffer1[sizeof(COM_GuildRequestData)*32] = {0};
	ProtocolMemWriter protocol1((S8*)buffer1 , sizeof(buffer1));
	bool checker = protocol1.writeVector(guild_.requestList_);
	SRV_ASSERT(checker);
	stmt.bind( 9 , (unsigned char*)buffer1 , protocol1.length());

	unsigned char buffer2[sizeof(COM_GuildBuilding)*32] = {0};
	ProtocolMemWriter protocol2((S8*)buffer2 , sizeof(buffer2));
	checker = protocol2.writeVector(guild_.buildings_);
	SRV_ASSERT(checker);
	stmt.bind( 10 , (unsigned char*)buffer2 , protocol2.length());

	unsigned char buffer3[sizeof(COM_GuildProgen)*32] = {0};
	ProtocolMemWriter protocol3((S8*)buffer3 , sizeof(buffer3));
	checker = protocol3.writeVector(guild_.progenitus_);
	SRV_ASSERT(checker);
	stmt.bind( 11 , (unsigned char*)buffer3 , protocol3.length());

	unsigned char buffer4[sizeof(S32)*32] = {0};
	ProtocolMemWriter protocol4((S8*)buffer4 , sizeof(buffer4));
	checker = protocol4.writeVector(guild_.progenitusPositions_);
	SRV_ASSERT(checker);
	stmt.bind( 12 , (unsigned char*)buffer4 , protocol4.length());

	stmt.bind( 13 , int(guild_.presentNum_));
	stmt.bind( 14, int(guild_.guildId_));
	
	stmt.execDML();
#else
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));

	prep_stmt->setString( 1 , guild_.guildName_.c_str());
	prep_stmt->setInt( 2 , guild_.guildLevel_);
	prep_stmt->setInt( 3 , int(guild_.guildContribution_));
	prep_stmt->setInt( 4 , int(guild_.fundz_));
	prep_stmt->setInt( 5 , guild_.createTime_);
	prep_stmt->setInt64( 6 , guild_.master_);
	prep_stmt->setString( 7 , guild_.masterName_.c_str());
	prep_stmt->setString( 8 , guild_.notice_.c_str());

	char buffer1[sizeof(COM_GuildRequestData)*32] = {0};
	ProtocolMemWriter protocol1((S8*)buffer1 , sizeof(buffer1));
	bool checker = protocol1.writeVector(guild_.requestList_);
	SRV_ASSERT(checker);
	sql::SQLString tmpStr1(buffer1,protocol1.length());
	prep_stmt->setString( 9 , tmpStr1);

	char buffer2[sizeof(COM_GuildBuilding)*32] = {0};
	ProtocolMemWriter protocol2((S8*)buffer2 , sizeof(buffer2));
	checker = protocol2.writeVector(guild_.buildings_);
	SRV_ASSERT(checker);
	sql::SQLString tmpStr2(buffer2,protocol2.length());
	prep_stmt->setString( 10 , tmpStr2);

	char buffer3[sizeof(COM_GuildProgen)*32] = {0};
	ProtocolMemWriter protocol3((S8*)buffer3 , sizeof(buffer3));
	checker = protocol3.writeVector(guild_.progenitus_);
	SRV_ASSERT(checker);
	sql::SQLString tmpStr3(buffer3,protocol3.length());
	prep_stmt->setString( 11 , tmpStr3);

	char buffer4[sizeof(S32)*32] = {0};
	ProtocolMemWriter protocol4((S8*)buffer4 , sizeof(buffer4));
	checker = protocol4.writeVector(guild_.progenitusPositions_);
	SRV_ASSERT(checker);
	sql::SQLString tmpStr4(buffer4,protocol4.length());
	prep_stmt->setString( 12 , tmpStr4);

	prep_stmt->setInt( 13 , int(guild_.presentNum_));
	prep_stmt->setInt( 14, int(guild_.guildId_));

	prep_stmt->execute();

#endif

	return 0;
	DB_EXEC_UNGUARD_RETURN
}

U32
UpdateMemberJob::go(SQLTask *pTask)
{
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
DB_EXEC_GUARD
		std::stringstream strCode;
	strCode<<"UPDATE GuildMember SET job="<<int(job_)<<" WHERE roleId="<<roleId_<<" ";
#ifdef USE_SQLITE
	CppSQLite3Statement stmt = dbc->compileStatement(strCode.str().c_str());
	stmt.execDML();
#else
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(strCode.str().c_str()));
	prep_stmt->execute();

#endif
	return 0;
DB_EXEC_UNGUARD_RETURN

}

U32 UpdateMemberContribution::go(SQLTask *pTask)
{
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD
		std::stringstream strCode;
	strCode<<"UPDATE GuildMember SET Contribution="<<int(contribution_)<<" WHERE roleId="<<roleId_<<" ";
#ifdef USE_SQLITE
	CppSQLite3Statement stmt = dbc->compileStatement(strCode.str().c_str());
	stmt.execDML();
#else
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(strCode.str().c_str()));
	prep_stmt->execute();

#endif
	return 0;
	DB_EXEC_UNGUARD_RETURN
}

U32
UpdateMemberJob::back()
{
	WorldHandler::instance()->updateMemberJobOk(roleId_,job_);
	return 0;
}

U32
DeleteGuildMember::go(SQLTask *pTask)
{
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD
	const char* pCode = "DELETE FROM GuildMember WHERE RoleId=?;";
#ifdef USE_SQLITE
	CppSQLite3Statement stmt = dbc->compileStatement(pCode);
	stmt.bind( 1 , roleId_);
	stmt.execDML();
#else
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	prep_stmt->setInt64(1,roleId_);
	prep_stmt->execute();

#endif
	return 0;
	DB_EXEC_UNGUARD_RETURN

}

U32 UpdateGuildStruction::go(SQLTask *pTask){
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD
		char const * pCode = "update Guild set guildLevel=?, contribution=? where guildId=?";

#ifdef USE_SQLITE
	CppSQLite3Statement stmt = dbc->compileStatement(pCode);

	stmt.bind(1 , int(level_));
	stmt.bind(2 , int(contribution_));
	stmt.bind(3 , int(guildId_));

	stmt.execDML();
#else
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	prep_stmt->setInt(1 , int(level_));
	prep_stmt->setInt(2 , int(contribution_));
	prep_stmt->setInt(3 , int(guildId_));
	prep_stmt->execute();

#endif

	return 0;
	DB_EXEC_UNGUARD_RETURN
}

U32
UpdateGuildBuilding::go(SQLTask *pTask){
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD

		char const * pCode = "update Guild set buildings=? where guildId=?";
#ifdef USE_SQLITE
	CppSQLite3Statement stmt = dbc->compileStatement(pCode);

	unsigned char buffer2[sizeof(COM_GuildBuilding)*32] = {0};
	ProtocolMemWriter protocol2((int8*)buffer2 , sizeof(buffer2));
	protocol2.writeVector(buildings_);
	//COM_ASSERT(checker);
	stmt.bind( 1 , (unsigned char*)buffer2 , protocol2.length());
	stmt.bind( 2 , int(guildId_));

	stmt.execDML();
#else
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	char buffer2[sizeof(COM_GuildBuilding)*32] = {0};
	ProtocolMemWriter protocol2((int8*)buffer2 , sizeof(buffer2));
	bool checker = protocol2.writeVector(buildings_);
	SRV_ASSERT(checker);
	sql::SQLString	tmpStr(buffer2,protocol2.length());
	prep_stmt->setString( 1 , tmpStr);
	prep_stmt->setInt( 2 , int(guildId_));
	prep_stmt->execute();
#endif

	DB_EXEC_UNGUARD_RETURN
return 0;
}
