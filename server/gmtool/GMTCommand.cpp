/************************************************************************/
// File		: GMTCommand.cpp     
// Author	: LWH
// Date		: 2016-1-4 14:30
// Brief	: GM指令相关模块文件
/************************************************************************/

#include "handler.h"
#include "config.h"
#include "routine.h"
#include "SQLHelper.h"

void
ClientHandler::gmCommandModule(Json::Value& json)
{
	int32 cmdtype = ENUM(GMCommandType).getItemId(json["CmdType"].asString());

	if(cmdtype<GMCT_NoTalk || cmdtype>=GMCT_Max)
	{
		ACE_DEBUG((LM_ERROR, ACE_TEXT("post data format error\n")));
		return ;
	}

	uint32 playerId = json["PlayerId"].asUInt();
	
	switch(cmdtype)
	{
	case GMCT_NoTalk:
		{
			int32 param  = json["Param"].asInt();
			WorldHandler::instance()->noTalkPlayer(playerId,param);
			break;
		}
	case GMCT_Freeze:
		{
			break;
		}
	case GMCT_Seal:
		{
			WorldHandler::instance()->sealPlayer(playerId);
			break;
		}
	case GMCT_Kick:
		{
			WorldHandler::instance()->kickPlayer(playerId);
			break;
		}
	case GMCT_OpenTalk:
		{
			WorldHandler::instance()->openTalkPlayer(playerId);
			break;
		}
	case GMCT_AddMoney:
		{
			int32 param  = json["Param"].asInt();
			WorldHandler::instance()->addMoney(playerId,param);
			
			SQLHelper mysql;
			if(SQLHelper::SUCCESS!=mysql.Connect())
			{
				ACE_DEBUG((LM_ERROR, ACE_TEXT("mysql open error\n")));
				result(-1,"error");
				return ;
			}
			DBC *dbc = mysql.getDBC();
			SRV_ASSERT(dbc);
			std::stringstream ssselect;
			ssselect << "SELECT * FROM Player WHERE PlayerGuid = " << playerId ;
			DB_EXEC_GUARD
			std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
			
			std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(ssselect.str().c_str()));
			if(res->next())
			{
				SGE_DBPlayerData inst;
				sql::SQLString pCacheBlob= res->getString("BinData");
				ProtocolMemReader mr(pCacheBlob->c_str(),pCacheBlob->length());
				inst.deserialize(&mr);
				
				if(!inst.properties_.empty()){
					inst.properties_[PT_Money] += param;
				}

				enum {BUFFER_SIZE = 1024*1024};
				char *buffer = new char[BUFFER_SIZE];
				ProtocolMemWriter mw(buffer,BUFFER_SIZE);
				inst.serialize(&mw);
				std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement("UPDATE Player SET BinData = ? WHERE PlayerGuid = ?"));
				sql::SQLString binString(buffer,mw.length());
				prep_stmt->setString(1,binString);
				prep_stmt->setInt(2,playerId);
				prep_stmt->executeUpdate();
				delete []buffer;
				prep_stmt->close();
			}
			stmt->close();
			DB_EXEC_UNGUARD_RETURN
			break;
		}
	case GMCT_AddDiamond:
		{
			int32 param  = json["Param"].asInt();
			WorldHandler::instance()->addDiamond(playerId,param);
			
			SQLHelper mysql;
			if(SQLHelper::SUCCESS!=mysql.Connect())
			{
				ACE_DEBUG((LM_ERROR, ACE_TEXT("mysql open error\n")));
				result(-1,"error");
				return ;
			}
			DBC *dbc = mysql.getDBC();
			SRV_ASSERT(dbc);
			std::stringstream ssselect;
			ssselect << "SELECT * FROM Player WHERE PlayerGuid = " << playerId ;
			DB_EXEC_GUARD
			std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
			
			std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(ssselect.str().c_str()));
			if(res->next())
			{
				SGE_DBPlayerData inst;
				sql::SQLString pCacheBlob= res->getString("BinData");
				ProtocolMemReader mr(pCacheBlob->c_str(),pCacheBlob->length());
				inst.deserialize(&mr);
				
				if(!inst.properties_.empty()){
					inst.properties_[PT_Diamond] += param;
				}

				enum {BUFFER_SIZE = 1024*1024};
				char *buffer = new char[BUFFER_SIZE];
				ProtocolMemWriter mw(buffer,BUFFER_SIZE);
				inst.serialize(&mw);
				std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement("UPDATE Player SET BinData = ? WHERE PlayerGuid = ?"));
				sql::SQLString binString(buffer,mw.length());
				prep_stmt->setString(1,binString);
				prep_stmt->setInt(2,playerId);
				prep_stmt->executeUpdate();
				delete []buffer;
				prep_stmt->close();
			}
			stmt->close();
			DB_EXEC_UNGUARD_RETURN

			break;
		}
	case GMCT_AddExp:
		{
			int32 param  = json["Param"].asInt();
			if(param< 0){
				result(-1,"param is less 0 !!!!");
				return;
			}
			WorldHandler::instance()->addExp(playerId,param);
			break;
		}
	case GMCT_OpenGM:
		{
			WorldHandler::instance()->openGM(playerId);
			break;
		}
	case GMCT_CloseGM:
		{
			WorldHandler::instance()->closeGM(playerId);
			break;
		}
	case GMCT_Unseal:
		{
			WorldHandler::instance()->unsealPlayer(playerId);

			/*const char *pCode = "UPDATE Account SET Seal=? WHERE UserName=?;";

			SQLHelper mysql;
			if(SQLHelper::SUCCESS!=mysql.Connect())
			{
				ACE_DEBUG((LM_ERROR, ACE_TEXT("mysql open error\n")));
				return ;
			}
			DBC *dbc = mysql.getDBC();
			SRV_ASSERT(dbc);

			std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
			prep_stmt->setInt(1,0);
			prep_stmt->setUInt(2,accname);
			prep_stmt->executeUpdate();*/

			break;
		}
	case GMCT_DoScript:
		{
			std::string  param = json["ScriptParam"].asString();
			if(param.empty()){
				result(-1,"param is less 0 !!!!");
				return;
			}
			WorldHandler::instance()->playerDoScript(playerId,param);
			break;
		}
	}

	result(0,"success");
}


void ClientHandler::gmDoScript(Json::Value& json){
	std::string script = json["Script"].asString();
	if(script.empty()){
		result(-1,"GM do script empty !!!!!! ");
		return;
	}
	WorldHandler::instance()->doScript(script);
	result(0,"success");
}

void ClientHandler::gmMakeOrder(Json::Value& json){
	uint32 playerId = json["PlayerId"].asUInt();
	if(0 == playerId){
		result(-1,"Player id is 0!!!");
		return;
	}
	int32 shopId =  json["ShopId"].asInt();
	if(shopId <=0){
		result(-1,"Shop id is error!!!");
		return;
	}
	float payment = (float)json["Payment"].asDouble();
	if(payment <=0){
		result(-1,"Payment is error!!!");
		return;
	}
	std::string orderid = json["OrderId"].asString();
	if(orderid.empty()){
		result(-1,"Order id is error!!!");
		return;
	}
	SGE_GmtOrder gmOrder;
	gmOrder.orderId_ = orderid;
	gmOrder.shopId_ = shopId;
	gmOrder.payment_ = payment;

	WorldHandler::instance()->makeOrder(playerId,gmOrder);

	result(0,"success");
}