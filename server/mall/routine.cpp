
#include "config.h"
#include "routine.h"
#include "sqltask.h"
#include "server.h"

U32 InitSQLTables::go(SQLTask* pTask){
	static const char* MallTableDDL = 
"CREATE TABLE IF NOT EXISTS MallTable("
"Guid INTEGER NOT NULL,"
"PlayerId	INT NOT NULL,"
"SellPrice INT NOT NULL,"
"Title VARCHAR(60) NOT NULL,"
"ItemSubType INT NOT NULL,"
"RaceType INT NOT NULL,"
"ItemId INT NOT NULL,"
"BabyId INT NOT NULL,"
"SellItem		BLOB	 NOT NULL,"
"PRIMARY KEY(Guid)"
");";
	static const char* MallSelledTableDDL = 
"CREATE TABLE IF NOT EXISTS MallSelledTable("
"Guid INT NOT NULL,"
"PlayerId	INT NOT NULL,"
"SellPrice INT NOT NULL,"
"Stack INT NOT NULL,"
"BabyId INT NOT NULL,"
"ItemId INT NOT NULL,"
"SelledTime INT NOT NULL,"
"Tax INT NOT NULL"
");";
	
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
DB_EXEC_GUARD
#if defined(USE_SQLITE)
	dbc->execDML(MallTableDDL);
	dbc->execDML(MallSelledTableDDL);
#elif defined(USE_MYSQL)
	dbc->prepareStatement(MallTableDDL);
	dbc->prepareStatement(MallSelledTableDDL);
#endif
DB_EXEC_UNGUARD_RETURN
	return 0;
}

//////////////////////////////////////////////////////////////////////////

U32 FetchSell::go(SQLTask *pTask){
	std::stringstream count;
	std::stringstream sstream;
	if (context_.begin_ < 0){
		context_.begin_ = 0;
	}
	if(!context_.title_.empty())
	{
		sstream << "SELECT * FROM MallTable WHERE Title LIKE \'%" << context_.title_  << "%\'  ORDER BY ItemId ASC,BabyId ASC,SellPrice ASC LIMIT "<< context_.begin_ << " , " << context_.limit_ << ";";
		count << "SELECT COUNT(*) AS GuidMax FROM MallTable WHERE Title LIKE \'%" << context_.title_  << "%\';";
	}
	else if( context_.ist_ != 0)
	{
		sstream << "SELECT * FROM MallTable WHERE ItemSubType = " << (int)context_.ist_ << "  ORDER BY ItemId ASC,BabyId ASC,SellPrice ASC LIMIT "<< context_.begin_ << " , " <<  context_.limit_ << ";";
		count << "SELECT COUNT(*) AS GuidMax FROM MallTable WHERE ItemSubType = " << (int)context_.ist_ << ";";
	}
	else if( context_.rt_ != 0 )
	{
		sstream << "SELECT * FROM MallTable WHERE RaceType = " << (int)context_.rt_ << "  ORDER BY ItemId ASC,BabyId ASC,SellPrice ASC LIMIT  "<< context_.begin_<< " , " <<  context_.limit_ << ";";
		count << "SELECT COUNT(*) AS GuidMax FROM MallTable WHERE RaceType = " << (int)context_.rt_ << ";";
	}
	else if( context_.itemId_ != 0) 
	{
		sstream << "SELECT * FROM MallTable WHERE ItemId = " << (int)context_.itemId_ << "  ORDER BY ItemId ASC,BabyId ASC,SellPrice ASC LIMIT "<< context_.begin_<< " , " <<  context_.limit_ << ";";
		count << "SELECT COUNT(*) AS GuidMax FROM MallTable WHERE ItemId = " << (int)context_.itemId_ << ";";
	}
	else if( context_.babyId_ != 0)
	{
		sstream << "SELECT * FROM MallTable WHERE BabyId = " << (int)context_.babyId_ << "  ORDER BY ItemId ASC,BabyId ASC,SellPrice ASC LIMIT "<< context_.begin_<< " , " <<  context_.limit_ << ";";
		count << "SELECT COUNT(*) AS GuidMax FROM MallTable WHERE BabyId = " << (int)context_.babyId_ << ";";
	}
	else
		return 0;
		//sstream << "SELECT * FROM MallTable WHERE Guid > " << context_.begin_  << " LIMIT " << context_.limit_ << ";";

	//sstream << " LIMIT " << context_.limit_ << ";";
	
	DBC *dbc = pTask->getDBC();
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Query q = dbc->execQuery(sstream.str().c_str());
	while(!q.eof()){
		S32 len=0;
		const unsigned char* pCacheBlob= q.getBlobField("SellItem",len);
		SRV_ASSERT(len);
		ProtocolMemReader mr(pCacheBlob,len);
		COM_SellItem item;
		item.deserialize(&mr);
		item.guid_ = q.getIntField("Guid");
		items_.push_back(item);
		q.nextRow();
	}
	q = dbc->execQuery(count.str().c_str());
	if (!q.eof()){
		total_ =q.getIntField("GuidMax");
	} 
#elif defined(USE_MYSQL)
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(sstream.str().c_str()));
	while (res->next()) 
	{
		sql::SQLString pCacheBlob = res->getString("SellItem");
		ProtocolMemReader mr(pCacheBlob.c_str(),pCacheBlob.length());
		COM_SellItem item;
		item.deserialize(&mr);
		item.guid_ = res->getInt("Guid");
		item.sellPlayerId_ = res->getInt("PlayerId");
		item.baby_.instId_ = res->getInt("BabyId");
		items_.push_back(item);
	}
	std::auto_ptr< sql::Statement > stmt1(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res1(stmt1->executeQuery(count.str().c_str()));
	if(res1->next()){
		total_ =res1->getInt("GuidMax");
	}
	
#endif
	DB_EXEC_UNGUARD_RETURN
	return 0;
}

U32 FetchSell::back(){
	Server::instance()->worldhandler()->fetchSellOk(playerId_,items_,total_);
	return 0;
}

//////////////////////////////////////////////////////////////////////////

U32 FetchMySell::go(SQLTask *pTask){
	
	std::stringstream sstream;
	sstream << "SELECT * FROM MallTable WHERE PlayerId = " << playerId_ << ";";
	DBC *dbc = pTask->getDBC();
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Query q = dbc->execQuery(sstream.str().c_str());
	while(!q.eof()){
		S32 len=0;
		const unsigned char* pCacheBlob= q.getBlobField("SellItem",len);
		SRV_ASSERT(len);
		ProtocolMemReader mr(pCacheBlob,len);
		COM_SellItem item;
		item.deserialize(&mr);
		item.guid_ = q.getIntField("Guid");
		items_.push_back(item);
		q.nextRow();
	}
	
#elif defined(USE_MYSQL)
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(sstream.str().c_str()));
	while (res->next()) 
	{
		sql::SQLString pCacheBlob = res->getString("SellItem");
		ProtocolMemReader mr(pCacheBlob.c_str(),pCacheBlob.length());
		COM_SellItem item;
		item.deserialize(&mr);
		item.guid_ = res->getInt("Guid");
		item.sellPlayerId_ = res->getInt("PlayerId");
		item.baby_.instId_ = res->getInt("BabyId");
		items_.push_back(item);
	}
#endif
	DB_EXEC_UNGUARD_RETURN
	return 0;
}

U32 FetchMySell::back(){
	Server::instance()->worldhandler()->fetchMySellOk(playerId_,items_);
	return 0;
}


//////////////////////////////////////////////////////////////////////////

U32 Sell::go(SQLTask *pTask){
	static const char* pCode = "SELECT MAX(Guid) AS GuidMax FROM MallTable ";
	static const char* insertcode = "INSERT INTO MallTable(Guid,PlayerId,Title,ItemSubType,RaceType,ItemId,BabyId,SellPrice,SellItem) VALUES(?,?,?,?,?,?,?,?,?)";
	DBC *dbc = pTask->getDBC();
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Query q = dbc->execQuery(pCode);
	if (!q.eof()){
		item_.guid_ =q.getIntField("GuidMax") + 1;
	} 
	CppSQLite3Statement stmt =dbc->compileStatement(insertcode);
	stmt.bind(1,item_.guid_);
	stmt.bind(2,item_.sellPlayerId_);
	stmt.bind(3,item_.title_.c_str());
	stmt.bind(4,item_.ist_);
	stmt.bind(5,item_.rt_);
	stmt.bind(6,item_.item_.itemId_);
	if(item_.baby_.properties_.empty())
		stmt.bind(7,0);
	else
		stmt.bind(7,item_.baby_.instId_);
	stmt.bind(8,item_.sellPrice);
	unsigned char buffer[204800] = {'\0'};
	ProtocolMemWriter mw(buffer,sizeof(buffer));
	item_.serialize(&mw);
	stmt.bind(9,buffer,mw.length());
	stmt.execDML();
	
#elif defined(USE_MYSQL)
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(pCode));
	if(res->next()){
		item_.guid_ =res->getInt("GuidMax") + 1;
	}
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(insertcode));
	prep_stmt->setInt( 1 , int(item_.guid_) );
	prep_stmt->setInt( 2 , int(item_.sellPlayerId_) );
	prep_stmt->setString( 3 , item_.title_.c_str());
	prep_stmt->setInt( 4 , item_.ist_);
	prep_stmt->setInt64( 5 , item_.rt_);
	prep_stmt->setInt(6,item_.item_.itemId_);
	if(item_.baby_.properties_.empty())
		prep_stmt->setInt(7,0);
	else
		prep_stmt->setInt(7,item_.baby_.instId_);
	prep_stmt->setInt( 8 , item_.sellPrice);
	char buffer[20480] = {'\0'};
	ProtocolMemWriter mw(buffer,sizeof(buffer));
	item_.serialize(&mw);
	sql::SQLString str(buffer,mw.length());
	prep_stmt->setString(9, str);
	prep_stmt->execute();
#endif
	DB_EXEC_UNGUARD_RETURN
	return 0;
}

U32 Sell::back(){
	Server::instance()->worldhandler()->sellOk(playerId_,item_);
	return 0;
}

//////////////////////////////////////////////////////////////////////////

U32 Unsell::go(SQLTask *pTask){
	std::stringstream sstream;
	sstream << "DELETE FROM MallTable WHERE Guid = " << guid_ << ";";
	DBC *dbc = pTask->getDBC();
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
	dbc->execDML(sstream.str().c_str());
#elif defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(sstream.str().c_str()));
	prep_stmt->execute();
#endif
	DB_EXEC_UNGUARD_RETURN
	return 0;
}

U32 Unsell::back(){
	Server::instance()->worldhandler()->unSellOk(playerId_,guid_);
	return 0;
}

//////////////////////////////////////////////////////////////////////////

U32 Buy::go(SQLTask *pTask){
	ACE_DEBUG((LM_INFO,"BUY %d \n",bc_.guid_));
	std::stringstream sstream;
	sstream << "SELECT * FROM MallTable WHERE Guid = " << bc_.guid_ << ";";
	DBC *dbc = pTask->getDBC();
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Query q = dbc->execQuery(sstream.str().c_str());
	if(!q.eof()){
		S32 len=0;
		const unsigned char* pCacheBlob= q.getBlobField("SellItem",len);
		SRV_ASSERT(len);
		ProtocolMemReader mr(pCacheBlob,len);
		item_.deserialize(&mr);
		item_.guid_ = q.getIntField("Guid");
	}
	else{
		error_ = EN_MallBuyFailSelled;
		return 0;
	}

	if(item_.sellPrice > bc_.diamond_ && item_.sellPrice > bc_.magic_)
	{
		error_ = EN_MallBuyFailDiamondLess;
		return 0;
	}
	
	if(item_.item_.instId_ !=0 && bc_.isBagFull_){
		error_ = EN_MallBuyFailBagFull;
		return 0;
	}

	if(item_.baby_.instId_ !=0 && bc_.isBabyFull_){
		error_ = EN_MallBuyFailBabyFull;
		return 0;
	}
	
	std::stringstream delsstream;
	delsstream << "DELETE FROM MallTable WHERE Guid = " << bc_.guid_ << ";";
	dbc->execDML(delsstream.str().c_str());
#elif defined(USE_MYSQL)
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(sstream.str().c_str()));
	if(res->next()) 
	{
		sql::SQLString pCacheBlob = res->getString("SellItem");
		ProtocolMemReader mr(pCacheBlob->c_str(),pCacheBlob->length());
		if (!item_.deserialize(&mr)){
			ACE_DEBUG((LM_INFO,"DES ERROR %d \n",bc_.guid_));
		}
		item_.guid_ = res->getInt("Guid");
		item_.sellPlayerId_ = res->getInt("PlayerId");
		//item_.baby_.instId_ = res->getInt("BabyId");
	}
	else{
		error_ = EN_MallBuyFailSelled;
		return 0;
	}

	if(item_.sellPrice > bc_.diamond_ && item_.sellPrice > bc_.magic_)
	{
		error_ = EN_MallBuyFailDiamondLess;
		return 0;
	}

	if(item_.item_.instId_ !=0 && bc_.isBagFull_){
		error_ = EN_MallBuyFailBagFull;
		return 0;
	}

	if(item_.baby_.instId_ !=0 && bc_.isBabyFull_){
		error_ = EN_MallBuyFailBabyFull;
		return 0;
	}

	ACE_DEBUG((LM_INFO,"BUY %d %d \n",item_.baby_.instId_,item_.item_.instId_));
	
	std::stringstream delsstream;
	delsstream << "DELETE FROM MallTable WHERE Guid = " << bc_.guid_ << ";";
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(delsstream.str().c_str()));
	prep_stmt->execute();
#endif
	DB_EXEC_UNGUARD_RETURN
	return 0;
}

U32 Buy::back(){
	if(error_ == EN_MallBuyOk)
		Server::instance()->worldhandler()->buyOk(bc_.playerId_,item_);
	else
		Server::instance()->worldhandler()->buyFail(bc_.playerId_,error_);
	return 0;
}

U32 InsertSelledItem::go(SQLTask *pTask){

	static const char* insertcode = "INSERT INTO MallSelledTable(PlayerId,SellPrice,Stack,BabyId,ItemId,SelledTime,Tax) VALUES(?,?,?,?,?,?,?)";
	DBC *dbc = pTask->getDBC();
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Statement stmt =dbc->compileStatement(insertcode);
	stmt.bind(1,item_.sellPlayerId_);
	stmt.bind(2,item_.price_);
	stmt.bind(3,item_.itemStack_);
	stmt.bind(4,item_.babyId_);
	stmt.bind(5,item_.itemId_);
	stmt.bind(6,item_.selledTime_);
	stmt.bind(7,item_.tax_);
	stmt.execDML();
#elif defined(USE_MYSQL)
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(insertcode));
	prep_stmt->setInt( 1,item_.sellPlayerId_);
	prep_stmt->setInt( 2,item_.price_);
	prep_stmt->setInt( 3,item_.itemStack_);
	prep_stmt->setInt( 4,item_.babyId_);
	prep_stmt->setInt( 5,item_.itemId_);
	prep_stmt->setInt( 6,item_.selledTime_);
	prep_stmt->setInt( 7,item_.tax_);
	prep_stmt->execute();
#endif
	DB_EXEC_UNGUARD_RETURN
	return 0;
}

U32 FetachSelledItem::go(SQLTask *pTask){
	std::stringstream sstream;
	sstream << "SELECT * FROM MallSelledTable WHERE PlayerId = " << playerId_ << ";";
	DBC *dbc = pTask->getDBC();
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Query q = dbc->execQuery(sstream.str().c_str());
	while(!q.eof()){
		COM_SelledItem item;
		item.guid_ = q.getIntField("Guid");
		item.sellPlayerId_ = q.getIntField("PlayerId");
		item.price_ = q.getIntField("SellPrice");
		item.itemStack_ = q.getIntField("Stack");
		item.itemId_ = q.getIntField("ItemId");
		item.babyId_ = q.getIntField("BabyId");
		item.selledTime_ = q.getIntField("SelledTime");
		item.tax_ = q.getIntField("Tax");
		items_.push_back(item);
		q.nextRow();
	}

#elif defined(USE_MYSQL)
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(sstream.str().c_str()));
	while (res->next()) 
	{
		COM_SelledItem item;
		item.guid_ = res->getInt("Guid");
		item.sellPlayerId_ = res->getInt("PlayerId");
		item.price_ = res->getInt("SellPrice");
		item.itemStack_ = res->getInt("Stack");
		item.itemId_ = res->getInt("ItemId");
		item.babyId_ = res->getInt("BabyId");
		item.selledTime_ = res->getInt("SelledTime");
		item.tax_ = res->getInt("Tax");
		items_.push_back(item);
	}
#endif
	DB_EXEC_UNGUARD_RETURN
	return 0;
}

U32 FetachSelledItem::back(){
	Server::instance()->worldhandler()->fetchSelledItemOk(playerId_,items_);
	return 0;
}