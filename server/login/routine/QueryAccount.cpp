
#include "config.h"
#include "../routine.h"
#include "../handler.h"
#include "../sqltask.h"
#include "../server.h"
extern void gAddInsertHolder(std::string const& username);
extern bool gCheckInsertHolder(std::string const& username);
U32
QueryAccount::go(SQLTask *pTask)
{
	//ACE_DEBUG((LM_INFO,ACE_TEXT("Query account %s \n"),username_.c_str()));
	hasAccount_ = false;

	//ACE_DEBUG((LM_DEBUG ,"QueryPlayer go\n"));
	std::stringstream sstream;
	sstream << "SELECT * FROM Account where UserName = \"" << username_ << "\"";

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
DB_EXEC_GUARD

#if defined(USE_SQLITE)
	CppSQLite3Query q = dbc->execQuery(sstream.str().c_str());

	if(!q.eof())
	{
		hasAccount_ = true;
		S32 len=0;
		const unsigned char* pCacheBlob= q.getBlobField("AccountInfo",len);
		SRV_ASSERT(len);
		ProtocolMemReader mr(pCacheBlob,len);
		COM_AccountInfo inst;
		inst.deserialize(&mr);
		account_ = inst;
		account_.guid_ = q.getIntField("Guid");
		account_.username_ = q.getStringField("UserName");
		account_.password_ = q.getStringField("Password");
		isSeal_ = !!q.getIntField("Seal");
		
	}
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(sstream.str().c_str()));
	if(res->next())
	{
		hasAccount_ = true;

	
		S32 len=0;
		sql::SQLString pCacheBlob= res->getString("AccountInfo");
		ProtocolMemReader mr(pCacheBlob->c_str(),pCacheBlob->length());
		COM_AccountInfo inst;
		inst.deserialize(&mr);
		account_ = inst;

		account_.guid_ = res->getInt("Guid");
		account_.username_ = res->getString("UserName").c_str();
		account_.password_ = res->getString("Password").c_str();
		account_.phoneNumber_ = res->getString("PhoneNumber").c_str();
		isSeal_ = !!res->getInt("Seal");
	}
#endif

DB_EXEC_UNGUARD_RETURN
	return 0;
}


U32
QueryAccount::back()
{
	
	if(!hasAccount_)
	{
		if(!gCheckInsertHolder(username_))
		{
			gAddInsertHolder(username_);
			InsertAccount *p = NULL;
			Routine::create(p);
			account_.username_ = username_;
			p->account_ = account_;
			SQLTask::spost(p);
		}
	}
	else
	{
		ACE_DEBUG((LM_INFO,ACE_TEXT("Query account %s %d back\n"),account_.username_.c_str(),account_.guid_));
		WorldHandler::instance()->queryAccountOk(account_,false,isSeal_);
	}
	return 0;
}



U32 SealAccount::go(SQLTask *pTask){
	std::stringstream sstream;
	sstream << "UPDATE Account SET Seal=" << (isSeal_ ? "1" : "0") << " WHERE UserName='" << accname_ << "';";


	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
DB_EXEC_GUARD
#if defined(USE_SQLITE)
	dbc->execQuery(sstream.str().c_str());	
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(sstream.str().c_str()));
	prep_stmt->execute();
#endif
	DB_EXEC_UNGUARD_RETURN
	return 0;
}

U32 SealAccount::back(){
	if(isSeal_)
		WorldHandler::instance()->setAccountSealOk(accname_);
	return 0;
}


U32 SetPhoneNumber::go(SQLTask *pTask){
	std::stringstream sstream;
	sstream << "UPDATE Account SET PhoneNumber='" << phoneNumber_ << "' WHERE UserName='" << accname_ << "';";


	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
		dbc->execQuery(sstream.str().c_str());	
#elif  defined(USE_MYSQL)
		std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(sstream.str().c_str()));
	prep_stmt->execute();
#endif
	DB_EXEC_UNGUARD_RETURN
		return 0;
}

U32 SetPhoneNumber::back(){
	return 0;
}