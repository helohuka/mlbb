#include "config.h"
#include "../routine.h"
#include "../handler.h"
#include "../server.h"
#include "../sqltask.h"

U32 InsertActivity::go(SQLTask *pTask)
{
	DB_EXEC_GUARD
		const char* pCode = "REPLACE INTO Activity(ADType,BinData)"  
		"values(?,?);";
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);
#ifdef USE_SQLITE
	CppSQLite3Statement stmt = dbc->compileStatement(pCode);
	stmt.bind( 1 , adtype_);
	switch (adtype_)
	{
	case ADT_ChargeTotal:
		{
			char buffer[20480] = {'\0'};
			ProtocolMemWriter mw(buffer,sizeof(buffer));
			data_.chData_.serialize(&mw);
			stmt.bind( 2 , (unsigned char*)buffer,mw.length());
		}
		break;
	case ADT_ChargeEvery:
		{
			char buffer[20480] = {'\0'};
			ProtocolMemWriter mw(buffer,sizeof(buffer));
			data_.ceData_.serialize(&mw);
			stmt.bind( 2 , (unsigned char*)buffer,mw.length());
		}
		break;
	case ADT_DiscountStore:
		{
			char buffer[20480] = {'\0'};
			ProtocolMemWriter mw(buffer,sizeof(buffer));
			data_.stData_.serialize(&mw);
			stmt.bind( 2 , (unsigned char*)buffer,mw.length());
		}
		break;
	case ADT_LoginTotal:
		{
			char buffer[20480] = {'\0'};
			ProtocolMemWriter mw(buffer,sizeof(buffer));
			data_.loginData_.serialize(&mw);
			stmt.bind( 2 , (unsigned char*)buffer,mw.length());
		}
		break;
	case ADT_HotRole:
		{
			char buffer[20480] = {'\0'};
			ProtocolMemWriter mw(buffer,sizeof(buffer));
			data_.hrData_.serialize(&mw);
			stmt.bind( 2 , (unsigned char*)buffer,mw.length());
		}
		break;
	case ADT_Cards:
		{
			char buffer[20480] = {'\0'};
			ProtocolMemWriter mw(buffer,sizeof(buffer));
			data_.acData_.serialize(&mw);
			stmt.bind( 2 , (unsigned char*)buffer,mw.length());
		}
		break;
	case ADT_BuyEmployee:
		{
			char buffer[20480] = {'\0'};
			ProtocolMemWriter mw(buffer,sizeof(buffer));
			data_.etdata_.serialize(&mw);
			stmt.bind( 2 , (unsigned char*)buffer,mw.length());
		}
		break;
	case ADT_GiftBag:
		{
			char buffer[20480] = {'\0'};
			ProtocolMemWriter mw(buffer,sizeof(buffer));
			data_.gbdata_.serialize(&mw);
			stmt.bind( 2 , (unsigned char*)buffer,mw.length());
		}
		break;
	case ADT_Zhuanpan:
		{
			char buffer[20480] = {'\0'};
			ProtocolMemWriter mw(buffer,sizeof(buffer));
			data_.zpdata_.serialize(&mw);
			stmt.bind( 2 , (unsigned char*)buffer,mw.length());
		}
		break;
	case ADT_IntegralShop:
		{
			char buffer[20480] = {'\0'};
			ProtocolMemWriter mw(buffer,sizeof(buffer));
			data_.icdata_.serialize(&mw);
			stmt.bind( 2 , (unsigned char*)buffer,mw.length());
		}	
		break;
	default:
		break;
	}

	stmt.execDML();
#else
	std::auto_ptr< sql::PreparedStatement > prep_stmt(dbc->prepareStatement(pCode));
	prep_stmt->setInt( 1 , int(adtype_) );
	char buffer[20480] = {'\0'};
	ProtocolMemWriter mw(buffer,sizeof(buffer));
	switch (adtype_)
	{
	case ADT_ChargeTotal:
		{
			data_.chData_.serialize(&mw);
		}
		break;
	case ADT_ChargeEvery:
		{
			data_.ceData_.serialize(&mw);
		}
		break;
	case ADT_DiscountStore:
		{
			data_.stData_.serialize(&mw);
		}
		break;
	case ADT_LoginTotal:
		{
			data_.loginData_.serialize(&mw);
		}
		break;
	case ADT_HotRole:
		{
			data_.hrData_.serialize(&mw);
		}
		break;
	case ADT_Cards:
		{
			data_.acData_.serialize(&mw);
		}
		break;
	case ADT_BuyEmployee:
		{
			data_.etdata_.serialize(&mw);
		}
		break;
	case ADT_GiftBag:
		{
			data_.gbdata_.serialize(&mw);
		}
		break;
	case ADT_Zhuanpan:
		{
			data_.zpdata_.serialize(&mw);
		}
		break;
	case ADT_IntegralShop:
		{
			data_.icdata_.serialize(&mw);
		}
		break;
	default:
		break;
	}
	sql::SQLString binString(buffer,mw.length());
	prep_stmt->setString(2,binString);
	prep_stmt->executeUpdate();
#endif
	return 0;
	DB_EXEC_UNGUARD_RETURN
}

U32 InsertActivity::back(){return 0;}

U32 FetchActivity::go(SQLTask *pTask)
{
	DBC *dbc = pTask->getDBC();
	SRV_ASSERT(dbc);

	DB_EXEC_GUARD
		std::string str="SELECT * FROM Activity";
#ifdef USE_SQLITE
	CppSQLite3Query q = dbc->execQuery(str.c_str());
	while (!q.eof())
	{
		adtype_ = (ADType)q.getIntField("ADType");
		S32 len=0;
		const unsigned char* pCacheBlob= q.getBlobField("BinData",len);
		SRV_ASSERT(len);
		ProtocolMemReader mr(pCacheBlob,len);
	
		switch (adtype_)
		{
		case ADT_ChargeTotal:
			{
				COM_ADChargeTotal inst;
				inst.deserialize(&mr);
				data_.chData_ = inst;
			}
			break;
		case ADT_ChargeEvery:
			{
				COM_ADChargeEvery inst;
				inst.deserialize(&mr);
				data_.ceData_ = inst;
			}
			break;
		case ADT_DiscountStore:
			{
				COM_ADDiscountStore inst;
				inst.deserialize(&mr);
				data_.stData_ = inst;
			}
			break;
		case ADT_LoginTotal:
			{
				COM_ADLoginTotal inst;
				inst.deserialize(&mr);
				data_.loginData_ = inst;
			}
			break;
		case ADT_HotRole:
			{
				COM_ADHotRole inst;
				inst.deserialize(&mr);
				data_.hrData_ = inst;
			}
			break;
		case ADT_Cards:
			{
				COM_ADCards inst;
				inst.deserialize(&mr);
				data_.acData_ = inst;
			}
			break;
		case ADT_BuyEmployee:
			{
				COM_ADEmployeeTotal inst;
				inst.deserialize(&mr);
				data_.etdata_ = inst;
			}
			break;
		case ADT_GiftBag:
			{
				COM_ADGiftBag inst;
				inst.deserialize(&mr);
				data_.gbdata_ = inst;
			}
			break;
		case ADT_Zhuanpan:
			{
				COM_ZhuanpanData inst;
				inst.deserialize(&mr);
				data_.zpdata_ = inst;
			}	
			break;
		case ADT_IntegralShop:
			{
				COM_IntegralData inst;
				inst.deserialize(&mr);
				data_.icdata_ = inst;
			}
			break;
		default:
			break;
		}

		q.nextRow();
	}
#else
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(str.c_str()));
	while (res->next()) 
	{
		adtype_   = (ADType)res->getInt("ADType");
		sql::SQLString pCacheBlob= res->getString("BinData");
		ProtocolMemReader mr(pCacheBlob->c_str(),pCacheBlob->length());
		
		switch (adtype_)
		{
		case ADT_ChargeTotal:
			{
				COM_ADChargeTotal inst;
				inst.deserialize(&mr);
				data_.chData_ = inst;
			}
			break;
		case ADT_ChargeEvery:
			{
				COM_ADChargeEvery inst;
				inst.deserialize(&mr);
				data_.ceData_ = inst;
			}
			break;
		case ADT_DiscountStore:
			{
				COM_ADDiscountStore inst;
				inst.deserialize(&mr);
				data_.stData_ = inst;
			}
			break;
		case ADT_LoginTotal:
			{
				COM_ADLoginTotal inst;
				inst.deserialize(&mr);
				data_.loginData_ = inst;
			}
			break;
		case ADT_HotRole:
			{
				COM_ADHotRole inst;
				inst.deserialize(&mr);
				data_.hrData_ = inst;
			}
			break;
		case ADT_Cards:
			{
				COM_ADCards inst;
				inst.deserialize(&mr);
				data_.acData_ = inst;
			}
			break;
		case ADT_BuyEmployee:
			{
				COM_ADEmployeeTotal inst;
				inst.deserialize(&mr);
				data_.etdata_ = inst;
			}
			break;
		case ADT_GiftBag:
			{
				COM_ADGiftBag inst;
				inst.deserialize(&mr);
				data_.gbdata_ = inst;
			}
			break;
		case ADT_Zhuanpan:
			{
				COM_ZhuanpanData inst;
				inst.deserialize(&mr);
				data_.zpdata_ = inst;
			}
			break;
		case ADT_IntegralShop:
			{
				COM_IntegralData inst;
				inst.deserialize(&mr);
				data_.icdata_ = inst;
			}
			break;
		default:
			break;
		}
	}
#endif
	return 0;

	DB_EXEC_UNGUARD_RETURN
}

U32 FetchActivity::back()
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("FetchActivity back()\n")));
	WorldHandler::instance()->fatchActivity(data_);
	return 0;
}



//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
