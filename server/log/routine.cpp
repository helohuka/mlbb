#include "config.h"
#include "routine.h"
#include "sqltask.h"
#include "server.h"

U32 LogAccount::go(SQLTask *pTask){
	const char* sqlcode = 
"INSERT INTO "
"account"
"("
"game,"
"pfid,"
"pfname,"
"accountid,"
"createtime,"
"mac,"
"idfa,"
"ip,"
"devicetype"
")"
"VALUES"
"("
"?,"
"?,"
"?,"
"?,"
"?,"
"?,"
"?,"
"?,"
"?"
");";
	
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
DB_EXEC_GUARD
#if  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(sqlcode));
	prep_stmt->setString(1,"mhflc");
	prep_stmt->setString(2,data_.pfid_.c_str());
	prep_stmt->setString(3,data_.pfname_.c_str());
	prep_stmt->setString(4,data_.accountid_.c_str());
	prep_stmt->setString(5,TimeFormat::StrLocalTime(data_.createtime_).c_str());
	prep_stmt->setString(6,data_.mac_.c_str());
	prep_stmt->setString(7,data_.idfa_.c_str());
	prep_stmt->setString(8,data_.ip_.c_str());
	prep_stmt->setString(9,data_.devicetype_.c_str());
	prep_stmt->executeUpdate();
#endif
DB_EXEC_UNGUARD_RETURN
	return 0;
}

U32 LogLogin::go(SQLTask *pTask){
	const char* sqlcode = 
"INSERT INTO "
"login"
"("
"game,"
"pfid,"
"pfname,"
"accountid,"
"roleid,"
"firstserid,"
"serverid,"
"logintime,"
"logouttime,"
"firsttime,"
"rolefirsttime,"
"mac,"
"idfa,"
"ip,"
"devicetype"
")"
"VALUES"
"("
"?,"
"?,"
"?,"
"?,"
"?,"
"?,"
"?,"
"?,"
"?,"
"?,"
"?,"
"?,"
"?,"
"?,"
"?"
");";
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
DB_EXEC_GUARD
#if  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(sqlcode));
	prep_stmt->setString(1,"mhflc");
	prep_stmt->setString(2,data_.pfid_.c_str());
	prep_stmt->setString(3,data_.pfname_.c_str());
	prep_stmt->setString(4,data_.accountid_.c_str());
	prep_stmt->setInt(5,data_.roleId_);
	prep_stmt->setInt(6,data_.firstserid_);
	prep_stmt->setInt(7,data_.serverid_);
	prep_stmt->setString(8,TimeFormat::StrLocalTime(data_.logintime_).c_str());
	prep_stmt->setString(9,TimeFormat::StrLocalTime(data_.logouttime_).c_str());
	prep_stmt->setString(10,TimeFormat::StrLocalTime(data_.firsttime_).c_str());
	prep_stmt->setString(11,TimeFormat::StrLocalTime(data_.rolefirsttime_).c_str());
	prep_stmt->setString(12,data_.mac_.c_str());
	prep_stmt->setString(13,data_.idfa_.c_str());
	prep_stmt->setString(14,data_.ip_.c_str());
	prep_stmt->setString(15,data_.devicetype_.c_str());
	prep_stmt->executeUpdate();
#endif
DB_EXEC_UNGUARD_RETURN
	return 0;
}

U32 LogOrder::go(SQLTask *pTask){
	const char* sqlcode = 
"INSERT INTO "
"`order`"
"("
"game,"
"pfid,"
"pfname,"
"orderid,"
"accountid,"
"roleId,"
"rolelv,"
"payment,"
"paytime"
")"
"VALUES"
"("
"?,"
"?,"
"?,"
"?,"
"?,"
"?,"
"?,"
"?,"
"?"
");";
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
DB_EXEC_GUARD
#if  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(sqlcode));
	prep_stmt->setString(1,"mhflc");
	prep_stmt->setString(2,data_.pfid_.c_str());
	prep_stmt->setString(3,data_.pfname_.c_str());
	prep_stmt->setString(4,data_.orderid_.c_str());
	prep_stmt->setString(5,data_.accountid_.c_str());
	prep_stmt->setInt(6,data_.roleId_);
	prep_stmt->setInt(7,data_.rolelv_);
	prep_stmt->setDouble(8,data_.payment_);
	prep_stmt->setString(9,data_.paytime_.c_str());
	prep_stmt->executeUpdate();
#endif
DB_EXEC_UNGUARD_RETURN
	return 0;
}


U32 LogUIBehavior::go(SQLTask *pTask){
	const char* sqlcode = 
"INSERT INTO "
"PlayerLoginout"
"("
"PlayerGuid,"
"PlayerName,"
"UIBehaviorId,"
"dTime,"
"Version"
")"
"VALUES"
"("
"?,"
"?,"
"?,"
"?,"
"?"
");";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
DB_EXEC_GUARD
#if  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(sqlcode));
	prep_stmt->setInt(1,core_.playerGuid_);
	prep_stmt->setString(2,core_.playerName_.c_str());
	prep_stmt->setInt(3,core_.type_);
	prep_stmt->setDateTime(4,TimeFormat::StrLocalTimeNow().c_str());
	prep_stmt->setInt(5,VERSION_NUMBER);
	prep_stmt->executeUpdate();
#endif
DB_EXEC_UNGUARD_RETURN
	return 0;
}

U32 LogPlayerSay::go(SQLTask *pTask){
	const char* sqlcode = 
"INSERT INTO "
"PlayerSay"
"("
"PlayerGuid,"
"PlayerName,"
"ChannelId,"
"Content"
")"
"VALUES"
"("
"?,"
"?,"
"?,"
"?"
");";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	try
	{
#if  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(sqlcode));
	prep_stmt->setInt(1,playerId_);
	prep_stmt->setString(2,playerName_.c_str());
	prep_stmt->setInt(3,(int)core_.ck_);
	prep_stmt->setString(4,core_.content_.c_str());
	prep_stmt->executeUpdate();
#endif
	}catch(std::exception& e){
		ACE_DEBUG((LM_ERROR, ACE_TEXT("DB exception failed (%s).\n"), e.what()));		
	}
	return 0;
}

U32 LogPlayerTrack::go(SQLTask *pTask){
	const char* sqlcode = 
"INSERT INTO "
"PlayerTrack"
"("
"Tiemstamp,"
"PlayerName,"
"PlayerGuid,"
"ItemId,"
"ItemInstId,"
"ItemStack,"
"MLB,"
"Diamond,"
"Money,"
"ExpP,"
"FromP"
")"
"VALUES"
"("
"?,"
"?,"
"?,"
"?,"
"?,"
"?,"
"?,"
"?,"
"?,"
"?,"
"?"
");";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD
#if  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(sqlcode));
	prep_stmt->setDateTime(1,TimeFormat::StrLocalTimeNow().c_str());
	prep_stmt->setString(2,logdata_.playerName_.c_str());
	prep_stmt->setInt(3,logdata_.playerId_);
	prep_stmt->setInt(4,logdata_.itemId_);
	prep_stmt->setInt(5,logdata_.itemInstId_);
	prep_stmt->setInt(6,logdata_.itemStack_);
	prep_stmt->setInt(7,logdata_.magic_);
	prep_stmt->setInt(8,logdata_.diamond_);
	prep_stmt->setInt(9,logdata_.money_);
	prep_stmt->setInt(10,logdata_.exp_);
	prep_stmt->setInt(11,logdata_.from_);
	prep_stmt->executeUpdate();
#endif
	DB_EXEC_UNGUARD_RETURN
		return 0;
}

U32 LogRole::go(SQLTask *pTask){
	const char* sqlcode = 
		"REPLACE INTO "
		"role"
		"("
		"game,"
		"pfid,"
		"pfname,"
		"roleid,"
		"cachedate,"
		"accountid,"
		"serverid,"
		"servername,"
		"firstserid,"
		"rolefirstdate,"
		"rolelastdate,"
		"rolelv,"
		"gold,"
		"diamond,"
		"vip,"
		"ce"
		")"
		"VALUES"
		"("
		"?,"
		"?,"
		"?,"
		"?,"
		"?,"
		"?,"
		"?,"
		"?,"
		"?,"
		"?,"
		"?,"
		"?,"
		"?,"
		"?,"
		"?,"
		"?"
		");";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD
#if  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(sqlcode));
	prep_stmt->setString(1,"mhflc");
	prep_stmt->setString(2,logdata_.pfid_.c_str());
	prep_stmt->setString(3,logdata_.pfname_.c_str());
	prep_stmt->setInt(4,logdata_.roleid_);
	prep_stmt->setDateTime(5,TimeFormat::StrLocalTime(logdata_.cachetime_).c_str());
	prep_stmt->setString(6,logdata_.accountid_.c_str());
	prep_stmt->setInt(7,logdata_.serverid_);
	prep_stmt->setString(8,logdata_.servername_.c_str());
	prep_stmt->setInt(9,logdata_.firstserid_);
	prep_stmt->setDateTime(10,TimeFormat::StrLocalTime(logdata_.rolefirsttime_).c_str());
	prep_stmt->setDateTime(11,TimeFormat::StrLocalTime(logdata_.rolelasttime_).c_str());
	prep_stmt->setInt(12,logdata_.rolelv_);
	prep_stmt->setInt64(13,logdata_.gold_);
	prep_stmt->setInt64(14,logdata_.magicgold_);
	prep_stmt->setInt(15,logdata_.vip_);
	prep_stmt->setInt64(16,logdata_.ce_);
	prep_stmt->executeUpdate();
#endif
	DB_EXEC_UNGUARD_RETURN
		return 0;
}