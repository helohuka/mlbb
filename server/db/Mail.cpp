
#include "config.h"
#include "routine.h"
#include "handler.h"
#include "sqltask.h"

U32 InsertMail::go(SQLTask *pTask){

	static const char* pCode = "INSERT INTO Mail( RecvName , SendTime , ItemNum , BinData ) VALUES(?,?,?,?);";
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	char buffer[20480] = {'\0'};
	ProtocolMemWriter mw(buffer,sizeof(buffer));
	mail_.serialize(&mw);
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Statement stmt =dbc->compileStatement(pCode);
	stmt.bind(1,mail_.recvPlayerName_.c_str());
	stmt.bind(2,mail_.timestamp_);
	stmt.bind(3,mail_.items_.size());
	stmt.bind(4,(unsigned char*)buffer,mw.length());
	stmt.execDML();
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	prep_stmt->setString(1,mail_.recvPlayerName_.c_str());
	prep_stmt->setInt64(2,mail_.timestamp_);
	prep_stmt->setInt(3,mail_.items_.size());
	sql::SQLString binString(buffer,mw.length());
	prep_stmt->setString(4,binString);
	prep_stmt->executeUpdate();
#endif
	DB_EXEC_UNGUARD_RETURN
	return 0;
}

U32 InsertMailAll::go(SQLTask *pTask){

	static const char* selCode = "SELECT PlayerName FROM Player;";

	static const char* pCode = "INSERT INTO Mail( RecvName , SendTime , ItemNum , BinData ) VALUES(?,?,?,?);";
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	char buffer[204800] = {'\0'};

	DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Query q = dbc->execQuery(selCode);
	CppSQLite3Statement stmt =dbc->compileStatement(pCode);
	while(!q.eof())
	{	
		mail_.recvPlayerName_ = q.getStringField("PlayerName");
		ProtocolMemWriter mw(buffer,sizeof(buffer));
		mail_.serialize(&mw);
		
		stmt.bind(1,mail_.recvPlayerName_.c_str());
		stmt.bind(2,mail_.timestamp_);
		stmt.bind(3,mail_.items_.size());
		stmt.bind(4,(unsigned char*)buffer,mw.length());
		stmt.execDML();

		q.nextRow();
	}
	
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(selCode));
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));

	while(res->next()){
		mail_.recvPlayerName_  = res->getString("PlayerName").c_str();
		ProtocolMemWriter mw(buffer,sizeof(buffer));
		mail_.serialize(&mw);
	
		prep_stmt->setString(1,mail_.recvPlayerName_.c_str());
		prep_stmt->setInt64(2,mail_.timestamp_);
		prep_stmt->setInt(3,mail_.items_.size());
		sql::SQLString binString(buffer,mw.length());
		prep_stmt->setString(4,binString);
		prep_stmt->executeUpdate();
	}
#endif
	DB_EXEC_UNGUARD_RETURN
		return 0;
}

U32 InsertMailByRecvs::go(SQLTask *pTask){

	static const char* pCode = "INSERT INTO Mail( RecvName , SendTime , ItemNum , BinData ) VALUES(?,?,?,?);";
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	char buffer[204800] = {'\0'};

	DB_EXEC_GUARD
#if defined(USE_SQLITE)

	CppSQLite3Statement stmt =dbc->compileStatement(pCode);
	for (size_t i=0; i<resvs_.size(); ++i)
	{	
		mail_.recvPlayerName_ = resvs_[i];
		ProtocolMemWriter mw(buffer,sizeof(buffer));
		mail_.serialize(&mw);

		stmt.bind(1,mail_.recvPlayerName_.c_str());
		stmt.bind(2,mail_.timestamp_);
		stmt.bind(3,mail_.items_.size());
		stmt.bind(4,(unsigned char*)buffer,mw.length());
		stmt.execDML();

	}

#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	
	for (size_t i=0; i<resvs_.size(); ++i)
	{
		mail_.recvPlayerName_  = resvs_[i];
		ProtocolMemWriter mw(buffer,sizeof(buffer));
		mail_.serialize(&mw);

		prep_stmt->setString(1,mail_.recvPlayerName_.c_str());
		prep_stmt->setInt64(2,mail_.timestamp_);
		prep_stmt->setInt(3,mail_.items_.size());
		sql::SQLString binString(buffer,mw.length());
		prep_stmt->setString(4,binString);
		prep_stmt->executeUpdate();
	}
#endif
	DB_EXEC_UNGUARD_RETURN
		return 0;
}

U32 EraseMail::go(SQLTask *pTask){
	
	static const char* pCode = "DELETE FROM Mail WHERE MailGuid=?;";
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD

#ifdef USE_SQLITE
	CppSQLite3Statement stmt = dbc->compileStatement(pCode);
	stmt.bind(1, mailId_);
	stmt.execDML();
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	prep_stmt->setInt(1, mailId_);
	prep_stmt->execute();
#endif
	DB_EXEC_UNGUARD_RETURN
	return 0;
}

U32 FatchMail::go(SQLTask *pTask){
	std::stringstream selStr;
	selStr<<"SELECT * FROM Mail WHERE RecvName=\""<<recvName_<<"\" and MailGuid >"<<fatchId_<<" order by MailGuid asc limit 100;";
	
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
DB_EXEC_GUARD
#ifdef USE_SQLITE
	CppSQLite3Query q = dbc->execQuery(selStr.str().c_str());
	if(!q.eof())
	{
		while(!q.eof())
		{
			S32 len=0;
			const unsigned char* pCacheBlob= q.getBlobField("BinData",len);
			SRV_ASSERT(len);
			ProtocolMemReader mr(pCacheBlob,len);
			COM_Mail mail;
			mail.deserialize(&mr);
			mail.recvPlayerName_ = q.getStringField("RecvName");
			mail.mailId_ = q.getIntField("MailGuid");
			mails_.push_back(mail);
			q.nextRow();
		}
	}

#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(selStr.str().c_str()));

	while(res->next())
	{
		sql::SQLString pCacheBlob= res->getString("BinData");
		ProtocolMemReader mr(pCacheBlob->c_str(),pCacheBlob->length());
		COM_Mail mail;
		mail.deserialize(&mr);
		mail.recvPlayerName_ = res->getString("RecvName");
		mail.mailId_ =  res->getInt("MailGuid");
		mails_.push_back(mail);
	}
#endif
DB_EXEC_UNGUARD_RETURN
	return 0;
}

U32 FatchMail::back(){
	//ACE_DEBUG((LM_DEBUG,ACE_TEXT("Name = %s,FatchMail::back() mails_.Size == %d\n"),recvName_.c_str(),mails_.size()));
	WorldHandler::instance()->appendMail(recvName_, mails_);
	return 0;
}
	
U32 UpdateMail::go(SQLTask *pTask){
	static const char * pCode =
		"UPDATE Mail SET BinData=?,ItemNum=? WHERE MailGuid=?;";
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);

	char buffer[20480] = {'\0'};
	ProtocolMemWriter mw(buffer,sizeof(buffer));
	mail_.serialize(&mw);

	DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Statement stmt =dbc->compileStatement(pCode);
	stmt.bind(1,(unsigned char*)buffer,mw.length());
	stmt.bind(2,mail_.items_.size());
	stmt.bind(3,mail_.mailId_);
	stmt.execDML();
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	sql::SQLString binString(buffer,mw.length());
	prep_stmt->setString(1,binString);
	prep_stmt->setInt(2,mail_.items_.size());
	prep_stmt->setInt(3,mail_.mailId_);
	prep_stmt->executeUpdate();
#endif

	DB_EXEC_UNGUARD_RETURN	
	return 0;
}