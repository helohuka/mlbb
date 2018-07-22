#include "handler.h"
#include "config.h"
#include "routine.h"
#include "SQLHelper.h"

struct QueryResult{
	QueryResult():playerId_(0),value_(0){}
	int32 playerId_;
	int32 value_;
	std::string playerName_;
};

struct QueryResultSort{
	bool operator()(QueryResult const& l, QueryResult const& r){
		return l.value_ > r.value_;
	}
};

void ClientHandler::gmQueryRMB(){

	SQLHelper mysql;
	if(SQLHelper::SUCCESS!=mysql.Connect())
	{
		ACE_DEBUG((LM_ERROR, ACE_TEXT("mysql open error\n")));
		result(-1,"error");
		return ;
	}
	DBC *dbc = mysql.getDBC();
	SRV_ASSERT(dbc);
	std::vector<QueryResult> results;
DB_EXEC_GUARD
	SGE_DBPlayerData inst;
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery("SELECT * FROM Player"));
	while(res->next())
	{
		sql::SQLString pCacheBlob= res->getString("BinData");
		ProtocolMemReader mr(pCacheBlob->c_str(),pCacheBlob->length());
		inst.deserialize(&mr);
		QueryResult qr;
		
		qr.playerId_ = res->getInt("PlayerGuid");
		qr.playerName_ = res->getString("PlayerName");
		if(!inst.properties_.empty())
			qr.value_ = inst.properties_[PT_MagicCurrency];
		
		if (qr.value_ != 0)
			results.push_back(qr);	
	}
DB_EXEC_UNGUARD_RETURN
	std::sort(results.begin(),results.end(),QueryResultSort());
	Json::Value arr(Json::arrayValue);
	
	for(size_t i=0; i<results.size(); ++i){
		Json::Value obj(Json::objectValue);
		obj["id"] = results[i].playerId_;
		obj["name"] = results[i].playerName_.c_str();
		obj["value"]  = results[i].value_;
		arr.append(obj);
	}
	Json::FastWriter writer;
	std::string str = writer.write(arr);
	resultgzip(0,str.c_str());
}


void ClientHandler::gmQueryDia(){
	
	SQLHelper mysql;
	if(SQLHelper::SUCCESS!=mysql.Connect())
	{
		ACE_DEBUG((LM_ERROR, ACE_TEXT("mysql open error\n")));
		result(-1,"error");
		return ;
	}
	DBC *dbc = mysql.getDBC();
	SRV_ASSERT(dbc);
	std::vector<QueryResult> results;
	DB_EXEC_GUARD
		SGE_DBPlayerData inst;
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery("SELECT * FROM Player"));
	while(res->next())
	{
		sql::SQLString pCacheBlob= res->getString("BinData");
		ProtocolMemReader mr(pCacheBlob->c_str(),pCacheBlob->length());
		inst.deserialize(&mr);
		QueryResult qr;

		qr.playerId_ = res->getInt("PlayerGuid");
		qr.playerName_ = res->getString("PlayerName");
		if(!inst.properties_.empty())
			qr.value_ = inst.properties_[PT_Diamond];
		if (qr.value_ > 10000)
			results.push_back(qr);	
	}
	DB_EXEC_UNGUARD_RETURN
	std::sort(results.begin(),results.end(),QueryResultSort());
	Json::Value arr(Json::arrayValue);

	for(size_t i=0; i<results.size(); ++i){
		Json::Value obj(Json::objectValue);
		obj["id"] = results[i].playerId_;
		obj["name"] = results[i].playerName_.c_str();
		obj["value"]  = results[i].value_;
		arr.append(obj);
	}
	Json::FastWriter writer;
	std::string str = writer.write(arr);
	resultgzip(0,str.c_str());
	
}

void ClientHandler::gmQueryMoney(){
	
	SQLHelper mysql;
	if(SQLHelper::SUCCESS!=mysql.Connect())
	{
		ACE_DEBUG((LM_ERROR, ACE_TEXT("mysql open error\n")));
		result(-1,"error");
		return ;
	}
	DBC *dbc = mysql.getDBC();
	SRV_ASSERT(dbc);
	std::vector<QueryResult> results;
	DB_EXEC_GUARD
		SGE_DBPlayerData inst;
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery("SELECT * FROM Player"));
	while(res->next())
	{
		sql::SQLString pCacheBlob= res->getString("BinData");
		ProtocolMemReader mr(pCacheBlob->c_str(),pCacheBlob->length());
		inst.deserialize(&mr);
		QueryResult qr;

		qr.playerId_ = res->getInt("PlayerGuid");
		qr.playerName_ = res->getString("PlayerName");
		if(!inst.properties_.empty())
			qr.value_ = inst.properties_[PT_Money];

		if (qr.value_ >  100000)
			results.push_back(qr);	
	}
	DB_EXEC_UNGUARD_RETURN
		std::sort(results.begin(),results.end(),QueryResultSort());
	Json::Value arr(Json::arrayValue);

	for(size_t i=0; i<results.size(); ++i){
		Json::Value obj(Json::objectValue);
		obj["id"] = results[i].playerId_;
		obj["name"] = results[i].playerName_.c_str();
		obj["value"]  = results[i].value_;
		arr.append(obj);
	}
	Json::FastWriter writer;
	std::string str = writer.write(arr);
	resultgzip(0,str.c_str());
}