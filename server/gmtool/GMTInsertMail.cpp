#include "handler.h"
#include "config.h"
#include "routine.h"
#include "SQLHelper.h"
#include "itemtable.h"
void
ClientHandler::gmInsertMailModule(Json::Value& json)
{
	std::string srcname = json["Sender"].asString();
	std::string content	= json["Content"].asString();
	std::string title = json["Title"].asString();
	int32 lowLevel = json["LowLevel"].asInt();
	int32 highLevel = json["HighLevel"].asInt();
	int64 lowTime = json[""].asUInt();
	int64 highTime = json[""].asUInt();

	std::vector< COM_MailItem > items;
	{
		Json::Value jitems = json["Items"];
		if(!jitems.isNull() && jitems.isArray()){
			for(size_t i=0; i<jitems.size(); ++i){
				Json::Value jitem;
				jitem = jitems.get(i,jitem);
				if(jitem.isNull() && !jitem.isObject())
				{
					result(-1,"item is not match error");
					ACE_DEBUG((LM_ERROR, ACE_TEXT("item is not match error\n")));
					return ;
				}
				COM_MailItem item; 
				item.itemId_ = jitem["ItemId"].asInt();
				item.itemStack_ = jitem["ItemStack"].asInt();
				
				ItemTable::ItemData const* itemdata = ItemTable::getItemById(item.itemId_);
				if(!itemdata){
					result(-1,"item is int find error");
					ACE_DEBUG((LM_ERROR, ACE_TEXT("item is int find error\n")));
					return ;
				}

				if(itemdata->maxCount_ < item.itemStack_){
					result(-1,"item stack overflow error");
					ACE_DEBUG((LM_ERROR, ACE_TEXT("item is int find error\n")));
					return ;
				}
				items.push_back(item);
			}
		}
	}

	SQLHelper mysql;
	if(SQLHelper::SUCCESS!=mysql.Connect())
	{
		ACE_DEBUG((LM_ERROR, ACE_TEXT("mysql open error\n")));
		return ;
	}

	DBC *dbc = mysql.getDBC();
	SRV_ASSERT(dbc);
	std::string strtype = json["InsertMailType"].asString();
	if(strtype.empty())
	{
		result(-1,"insert mail type is nil error");
		return;
	}
	int32 sendType = ENUM(InsertMailType).getItemId(strtype);
	if(sendType < IGMT_PlayerId || sendType > IGMT_AllRegist){
		result(-1,"send type out side error");
		return;
	}
	
	switch(sendType)
	{
	case IGMT_PlayerId:
		{
			if(!json["Recvers"].isArray() || json["Recvers"].empty())
			{
				result(-1,"Recvers is error");
				return;
			}
			std::stringstream strCodeStream;
			strCodeStream << "SELECT PlayerName FROM Player WHERE PlayerGuid in ( ";
			for(size_t i=0; i<json["Recvers"].size(); ++i){
				int64 playerId = json["Recvers"][i].asUInt();
				strCodeStream << playerId;
				if(i+1 < json["Recvers"].size()){
					strCodeStream << ",";
				}
			}
			strCodeStream << " ) ";
			
			if( lowLevel != 0){
				strCodeStream << " AND PlayerLevel >= " << lowLevel;
			}
			if( highLevel != 0){
					strCodeStream << " AND PlayerLevel <= " << lowLevel;
			}
			if( lowTime != 0){
				strCodeStream << " AND CreateTime >= " << lowTime;
			}
			if( highTime != 0){
				strCodeStream << " AND CreateTime <= " << highTime;
			}
			strCodeStream << ";";
			
			std::vector<std::string> selectplayers;

			std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
			std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(strCodeStream.str().c_str()));
			while(res->next())
			{
				
				selectplayers.push_back(res->getString("PlayerName"));
			}	

			if(selectplayers.empty())
			{
				result(0,"send mail success");
				return;
			}
			char buffer[20480] = {'\0'};

			for(size_t i=0; i<selectplayers.size(); ++i)
			{
				COM_Mail mail;

				mail.mailType_ = MT_System;
				mail.sendPlayerName_   = srcname;
				mail.recvPlayerName_ = selectplayers[i];
				mail.content_  = content;
				mail.title_	   = title;
				mail.timestamp_ = ACE_OS::gettimeofday().sec();
				mail.items_ = items;

				static const char* pCode = "INSERT INTO Mail( RecvName , BinData , SendTime, ItemNum) VALUES(?,?,?,?);";
				
				ProtocolMemWriter mw(buffer,sizeof(buffer));
				mail.serialize(&mw);
				DB_EXEC_GUARD

				std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
				prep_stmt->setString(1,mail.recvPlayerName_.c_str());
				sql::SQLString binString(buffer,mw.length());
				prep_stmt->setString(2,binString);
				prep_stmt->setUInt64(3,mail.timestamp_);
				prep_stmt->setInt(4,items.size());
				prep_stmt->executeUpdate();

				DB_EXEC_UNGUARD_RETURN
			}	
			break;
		}
		
	case IGMT_AllOnline:
		{
			
			COM_Mail mail;
			mail.mailType_ = MT_System;
			mail.sendPlayerName_   = srcname;
			mail.content_  = content;
			mail.title_	   = title;
	
			mail.timestamp_ = ACE_OS::gettimeofday().sec();
			mail.items_ = items;

			WorldHandler::instance()->sendMailAllOnline(mail,lowLevel,highLevel,lowTime,highTime);
			break;
		}

	case IGMT_AllRegist:
		{
			COM_Mail mail;
			mail.mailType_ = MT_System;
			mail.sendPlayerName_   = srcname;
			mail.content_  = content;
			mail.title_	   = title;
			mail.timestamp_ = ACE_OS::gettimeofday().sec();
			mail.items_ = items;
			static const char* pCode = "INSERT INTO Mail( RecvName , BinData  , SendTime, ItemNum) VALUES(?,?,?,?);";
			SQLHelper mysql;
			if(SQLHelper::SUCCESS!=mysql.Connect())
			{
				ACE_DEBUG((LM_ERROR, ACE_TEXT("mysql open error\n")));
				return ;
			}
			DBC *dbc = mysql.getDBC();
			SRV_ASSERT(dbc);
DB_EXEC_GUARD
			std::stringstream sstring ;
			sstring << "SELECT PlayerName FROM Player ";
			if(lowLevel !=0 && highTime !=0 && lowTime !=0 && highTime !=0){
				sstring << "WHERE ";
				bool needAnd = false;
				if( lowLevel != 0){
					sstring << " PlayerLevel >= " << lowLevel;
					needAnd = true;
				}
				if( highLevel != 0){
					if(needAnd)
						sstring << " AND ";
					sstring << " PlayerLevel <= " << lowLevel;
					needAnd = true;
				}
				if( lowTime != 0){
					if(needAnd)
						sstring << " AND ";
					sstring << " CreateTime >= " << lowTime;
					needAnd = true;
				}
				if( highTime != 0){
					if(needAnd)
						sstring << " AND ";
					sstring << " CreateTime <= " << highTime;
				}
			}
			std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
			std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(sstring.str().c_str()));
			
			while(res->next())
			{
				mail.recvPlayerName_ = res->getString("PlayerName");
				char buffer[20480] = {'\0'};
				ProtocolMemWriter mw(buffer,sizeof(buffer));
				mail.serialize(&mw);
				std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
				prep_stmt->setString(1,mail.recvPlayerName_.c_str());
				sql::SQLString binString(buffer,mw.length());
				prep_stmt->setString(2,binString);
				prep_stmt->setUInt64(3,mail.timestamp_);
				prep_stmt->setInt(4,items.size());
				prep_stmt->executeUpdate();
			}
			DB_EXEC_UNGUARD_RETURN

			break;
		}
	default:
		break;
	}

	result(0,"send mail success");
}
