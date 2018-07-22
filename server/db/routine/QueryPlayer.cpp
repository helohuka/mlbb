#include "config.h"
#include "../sqltask.h"
#include "../routine.h"
#include "../handler.h"
#include "../server.h"



U32
QueryPlayerSimple::go(SQLTask *pTask)
{
	hasPlayer_ = false;
	players_.clear();
	std::stringstream sstream;
	sstream << "SELECT * FROM Player WHERE UserName = \"" << username_ << "\" AND InDoorId = " << serverId_  ;

	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
	DB_EXEC_GUARD
#if defined(USE_SQLITE)
		CppSQLite3Query q = dbc->execQuery(sstream.str().c_str());

	if(!q.eof())
	{
		hasPlayer_ = true;

		while(!q.eof())
		{
			U32 instId = q.getIntField("PlayerGuid");
			S32 len=0;
			const unsigned char* pCacheBlob= q.getBlobField("BinData",len);
			SRV_ASSERT(len);
			ProtocolMemReader mr(pCacheBlob,len);
			SGE_DBPlayerData inst;
			
			inst.deserialize(&mr);
			
			inst.instId_ = instId;
			inst.freeze_ =  q.getIntField("Freeze");
			inst.seal_ =  q.getIntField("Seal");

			COM_SimpleInformation info;
			info.instId_ = inst.instId_;
			info.level_ = inst.properties_[PT_Level];
			info.asset_id_ = inst.properties_[PT_AssetId];
			info.instName_	= inst.instName_;
			info.jt_ = (JobType)(S32)inst.properties_[PT_Profession];
			info.jl_ = inst.properties_[PT_ProfessionLevel];
			for (size_t k=0; k<inst.equips_.size(); ++k)
			{
				if(inst.equips_[k].slot_ == ES_SingleHand)
					info.weaponItemId_ = inst.equips_[k].itemId_;
				else if(inst.equips_[k].slot_ == ES_DoubleHand)
					info.weaponItemId_ = inst.equips_[k].itemId_;
				if(inst.equips_[k].slot_ == ES_Fashion)
					info.fashionId_ = inst.equips_[k].itemId_;
			}
			
			players_.push_back(info);
			q.nextRow();
		}
	}
#elif  defined(USE_MYSQL)
		std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(sstream.str().c_str()));

	while(res->next())
	{
		hasPlayer_ = true;
		sql::SQLString pCacheBlob= res->getString("BinData");
		ProtocolMemReader mr(pCacheBlob->c_str(),pCacheBlob->length());
		SGE_DBPlayerData inst;
		inst.deserialize(&mr);

		inst.instId_ =  res->getInt("PlayerGuid");
		inst.instName_	= res->getString("PlayerName").c_str();
		inst.freeze_ = res->getInt("Freeze");
		inst.seal_ =  res->getInt("Seal");
		
		COM_SimpleInformation info;
		info.instId_ = inst.instId_;
		info.level_ = inst.properties_[PT_Level];
		info.asset_id_ = inst.properties_[PT_AssetId];
		info.instName_	= inst.instName_;
		info.jt_ = (JobType)(S32)inst.properties_[PT_Profession];
		info.jl_ = inst.properties_[PT_ProfessionLevel];
		for (size_t k=0; k<inst.equips_.size(); ++k)
		{
			if(inst.equips_[k].slot_ == ES_SingleHand)
				info.weaponItemId_ = inst.equips_[k].itemId_;
			else if(inst.equips_[k].slot_ == ES_DoubleHand)
				info.weaponItemId_ = inst.equips_[k].itemId_;
			if(inst.equips_[k].slot_ == ES_Fashion)
				info.fashionId_ = inst.equips_[k].itemId_;
		}

		players_.push_back(info);
	}

#endif
	DB_EXEC_UNGUARD_RETURN
		return 0;
}

U32
QueryPlayerSimple::back()
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("Query player simple %s back\n"),username_.c_str()));
	WorldHandler::instance()->queryPlayerSimpleInformationOk(username_,players_,serverId_);
	return 0;
}


U32
QueryPlayer::go(SQLTask *pTask)
{
	std::stringstream sstream;
	sstream << "SELECT * FROM Player WHERE PlayerGuid = " << playerId_ ;
	
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
DB_EXEC_GUARD
#if defined(USE_SQLITE)
	CppSQLite3Query q = dbc->execQuery(sstream.str().c_str());

	if(!q.eof())
	{	
			U32 instId = q.getIntField("PlayerGuid");
			S32 len=0;
			const unsigned char* pCacheBlob= q.getBlobField("BinData",len);
			SRV_ASSERT(len);
			ProtocolMemReader mr(pCacheBlob,len);
			SGE_DBPlayerData inst;
			inst.instId_ = instId;
			inst.freeze_ =  q.getIntField("Freeze");
			inst.seal_ =  q.getIntField("Seal");
			inst.deserialize(&mr);
		
			player_ = inst;
			
	}
#elif  defined(USE_MYSQL)
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(sstream.str().c_str()));
	
	if(res->next())
	{
		sql::SQLString pCacheBlob= res->getString("BinData");
		ProtocolMemReader mr(pCacheBlob->c_str(),pCacheBlob->length());
		SGE_DBPlayerData inst;
		inst.deserialize(&mr);

		inst.instId_ =  res->getInt("PlayerGuid");
		inst.instName_	= res->getString("PlayerName").c_str();
		inst.freeze_ = res->getInt("Freeze");
		inst.seal_ =  res->getInt("Seal");

		player_ = inst;
	}

#endif
DB_EXEC_UNGUARD_RETURN
	return 0;
}

U32
QueryPlayer::back()
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("Query player %s back\n"),username_.c_str()));
	player_.babies_.clear();
	player_.employees_.clear();

	Server::instance()->getPlayerBabyList(player_.instName_,player_.babies_);
	Server::instance()->getPlayerEmployeeList(player_.instName_,player_.employees_);
	WorldHandler::instance()->queryPlayerOk(username_,player_);
	return 0;
}