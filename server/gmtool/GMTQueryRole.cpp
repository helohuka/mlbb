#include "handler.h"
#include "config.h"
#include "routine.h"
#include "SQLHelper.h"
#include "itemtable.h"
#include "skilltable.h"
void
ClientHandler::gmQueryRoleModule(Json::Value& json)
{
	uint32 playerId = json["PlayerId"].asUInt();
	std::string playerName = json["PlayerName"].asString();

	std::stringstream sstream ;
	if(!playerId && playerName.empty()){
		result(-1,"Query case is nil");
		return;
	}
	if(playerId)
		sstream << "SELECT * FROM Player WHERE PlayerGuid=" << playerId << ";";
	else if(!playerName.empty())
		sstream << "SELECT * FROM Player WHERE PlayerName=\"" << playerName <<"\"";
	SQLHelper mysql;
	if(SQLHelper::SUCCESS!=mysql.Connect())
	{
		ACE_DEBUG((LM_ERROR, ACE_TEXT("mysql open error\n")));
		return ;
	}
	DBC *dbc = mysql.getDBC();
	SRV_ASSERT(dbc);
	std::string strresult;
DB_EXEC_GUARD
	SGE_DBPlayerData inst;
	std::string accId;
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(sstream.str().c_str()));
	
	if(res->next())
	{
		U32 instId = res->getInt("PlayerGuid");
		accId = res->getString("UserName");
		S32 len=0;
		sql::SQLString pCacheBlob= res->getString("BinData");
		ProtocolMemReader mr(pCacheBlob->c_str(),pCacheBlob->length());
		
		inst.freeze_ = res->getInt("Freeze");
		inst.seal_ =  res->getInt("Seal");
		inst.deserialize(&mr);
		

		{
			std::stringstream ssbaby;
			ssbaby << "SELECT * FROM Baby WHERE OwnerName = \"" << inst.instName_ << "\"";
			std::auto_ptr< sql::Statement > stmt1(dbc->createStatement());
			std::auto_ptr< sql::ResultSet > res1(stmt1->executeQuery(ssbaby.str().c_str()));
			inst.babies_.clear();
			while(res1->next())
			{
				sql::SQLString pCacheBlob1= res1->getString("BinData");
				ProtocolMemReader mrbaby(pCacheBlob1->c_str(),pCacheBlob1->length());
				COM_BabyInst instbaby;
				instbaby.deserialize(&mrbaby);
				inst.babies_.push_back(instbaby);
			}
		}

		{
			std::stringstream ssemployee;
			ssemployee << "SELECT * FROM Employee WHERE OwnerName = \"" << inst.instName_ << "\"";
			std::auto_ptr< sql::Statement > stmt1(dbc->createStatement());
			std::auto_ptr< sql::ResultSet > res1(stmt1->executeQuery(ssemployee.str().c_str()));
			inst.employees_.clear();
			while(res1->next())
			{
				sql::SQLString pCacheBlob1= res1->getString("BinData");
				ProtocolMemReader mremployee(pCacheBlob1->c_str(),pCacheBlob1->length());
				COM_EmployeeInst instemployee;
				instemployee.deserialize(&mremployee);
				inst.employees_.push_back(instemployee);
			}
		}
	}
	
	if(inst.instId_ == 0){
		result(-1,"Query error");
		return;
	}
		
	if(inst.instId_ != 0){
		Json::Value jroot(Json::objectValue);
		jroot["AccId"] = Json::Value(accId.c_str());
		jroot["Id"] = Json::Value((Json::UInt)inst.instId_);
		jroot["Name"] = Json::Value(inst.instName_.c_str());
		jroot["Level"] = Json::Value((Json::UInt)inst.properties_[PT_Level]);
		jroot["Exp"] = Json::Value((Json::UInt)inst.properties_[PT_Exp]);
		jroot["Guide"] = Json::Value((Json::UInt)inst.guideIdx_);
		jroot["Guild"] = Json::Value(inst.guildName_.c_str());
		jroot["Prof"] = Json::Value((Json::UInt)inst.properties_[PT_Profession]);
		jroot["ProfLevel"] = Json::Value((Json::UInt)inst.properties_[PT_ProfessionLevel]);
		jroot["Gold"] = Json::Value((Json::UInt)inst.properties_[PT_Money]);
		jroot["Money"] = Json::Value((Json::UInt)inst.properties_[PT_Diamond]);
		jroot["MagicCurrency"] = Json::Value((Json::UInt)inst.properties_[PT_MagicCurrency]);
		
		jroot["ComplateQuest"] = Json::Value(Json::arrayValue);
		for(size_t i=0; i<inst.completeQuests_.size(); ++i){
			jroot["ComplateQuest"].append(Json::Value((Json::UInt)inst.completeQuests_[i]));
		}

		jroot["CurrentQuest"] = Json::Value(Json::arrayValue);
		for(size_t i=0; i<inst.quests_.size(); ++i){
			jroot["CurrentQuest"].append(Json::Value((Json::UInt)inst.quests_[i].questId_));
		}

		jroot["Skills"] = Json::Value(Json::arrayValue);
		for(size_t i=0; i<inst.skill_.size();++i){
			Json::Value jskill(Json::objectValue);
			jskill["Id"] = Json::Value((Json::UInt)inst.skill_[i].skillID_);
			jskill["Level"] = Json::Value((Json::UInt)inst.skill_[i].skillLevel_);
			jskill["Exp"] = Json::Value((Json::UInt)inst.skill_[i].skillExp_);
			jroot["Skills"].append(jskill);
		}

		jroot["BagItems"] = Json::Value(Json::arrayValue);
		for(size_t i=0; i<inst.bagItems_.size();++i){
			Json::Value jitem(Json::objectValue);
			jitem["Id"] = Json::Value((Json::UInt)inst.bagItems_[i].itemId_);
			jitem["Stack"] = Json::Value((Json::UInt)inst.bagItems_[i].stack_);

			jroot["BagItems"].append(jitem);
		}

		jroot["Equipments"] = Json::Value(Json::arrayValue);
		for(size_t i=0; i<inst.equips_.size();++i){
			Json::Value jitem(Json::objectValue);
			jitem["Id"] = Json::Value((Json::UInt)inst.equips_[i].itemId_);
			jitem["Stack"] = Json::Value((Json::UInt)inst.equips_[i].stack_);

			jroot["Equipments"].append(jitem);
		}

		jroot["Babies"] = Json::Value(Json::arrayValue);
		for(size_t i=0; i<inst.babies_.size();++i){
			Json::Value jbaby(Json::objectValue);
			jbaby["Id"] = Json::Value((Json::UInt)inst.babies_[i].instId_);
			jbaby["Name"] = Json::Value(inst.babies_[i].instName_.c_str());
			jbaby["Level"] = Json::Value((Json::UInt)inst.babies_[i].properties_[PT_Level]);
			jbaby["Exp"] = Json::Value((Json::UInt)inst.babies_[i].properties_[PT_Exp]);
			jbaby["StrongLevel"] = Json::Value((Json::UInt)inst.babies_[i].intensifyLevel_);
			
			jbaby["Gears"] = Json::Value(Json::arrayValue);
			for(size_t j=0; j<inst.babies_[i].gear_.size(); ++j){
				jbaby["Gears"].append((Json::UInt)inst.babies_[i].gear_[j]);
			}
			

			jbaby["Skills"] = Json::Value(Json::arrayValue);
			for(size_t j=0; j<inst.babies_[i].skill_.size(); ++j){
				Json::Value jskill(Json::objectValue);
				jskill["Id"] = Json::Value((Json::UInt)inst.babies_[i].skill_[j].skillID_);
				jskill["Level"] = Json::Value((Json::UInt)inst.babies_[i].skill_[j].skillLevel_);
				jskill["Exp"] = Json::Value((Json::UInt)inst.babies_[i].skill_[j].skillExp_);
				jbaby["Skills"].append(jskill);
			}
			jroot["Babies"].append(jbaby);
		}
		
		/*jroot["Emplyees"] = Json::Value(Json::arrayValue);

		for(size_t i=0; i<inst.employees_.size(); ++i){
			Json::Value jemployee(Json::objectValue);
			jemployee["Id"] = Json::Value((Json::UInt)inst.employees_[i].instId_);
			jemployee["Name"] = Json::Value(inst.employees_[i].instName_.c_str());
			jemployee["Level"] = Json::Value((Json::UInt)inst.employees_[i].properties_[PT_Level]);
			jemployee["Exp"] = Json::Value((Json::UInt)inst.employees_[i].properties_[PT_Exp]);
			jemployee["Color"] = Json::Value((Json::UInt)inst.employees_[i].quality_);

			jemployee["Skills"] = Json::Value(Json::arrayValue);
			for(size_t j=0; j<inst.employees_[i].skill_.size(); ++j){
				Json::Value jskill(Json::objectValue);
				jskill["Id"] = Json::Value((Json::UInt)inst.employees_[i].skill_[j].skillID_);
				jskill["Level"] = Json::Value((Json::UInt)inst.employees_[i].skill_[j].skillLevel_);
				jskill["Exp"] = Json::Value((Json::UInt)inst.employees_[i].skill_[j].skillExp_);
				jemployee["Skills"].append(jskill);
			}

			jemployee["Equipments"] = Json::Value(Json::arrayValue);
			for(size_t j=0; j<inst.employees_[i].equips_.size(); ++j){
				Json::Value jskill(Json::objectValue);
				jskill["Id"] = Json::Value((Json::UInt)inst.employees_[i].equips_[j].instId_);
				jskill["Stack"] = Json::Value((Json::UInt)inst.employees_[i].equips_[j].stack_);
				jemployee["Equipments"].append(jskill);
			}
			jroot["Emplyees"].append(jemployee);
		}*/

		/*jroot["ItemStore"] = Json::Value(Json::arrayValue);
		for(size_t i=0; i<inst.itemStorage_.size();++i){
			Json::Value jitem(Json::objectValue);
			jitem["Id"] = Json::Value((Json::UInt)inst.itemStorage_[i].itemId_);
			jitem["Stack"] = Json::Value((Json::UInt)inst.itemStorage_[i].stack_);

			jroot["ItemStore"].append(jitem);
		}*/

		/*jroot["BabyStore"] = Json::Value(Json::arrayValue);
		for(size_t i=0; i<inst.babyStorage_.size();++i){
			Json::Value jbaby(Json::objectValue);
			jbaby["Id"] = Json::Value((Json::UInt)inst.babyStorage_[i].instId_);
			jbaby["Name"] = Json::Value(inst.babyStorage_[i].instName_.c_str());
			jbaby["Level"] = Json::Value((Json::UInt)inst.babyStorage_[i].properties_[PT_Level]);
			jbaby["Exp"] = Json::Value((Json::UInt)inst.babyStorage_[i].properties_[PT_Exp]);
			jbaby["StrongLevel"] = Json::Value((Json::UInt)inst.babyStorage_[i].properties_[PT_Strength]);

			jbaby["Gears"] = Json::Value(Json::arrayValue);
			for(size_t j=0; j<inst.babyStorage_[i].gear_.size(); ++j){
				jbaby["Gears"].append((Json::UInt)inst.babyStorage_[i].gear_[j]);
			}


			jbaby["Skills"] = Json::Value(Json::arrayValue);
			for(size_t j=0; j<inst.babyStorage_[i].skill_.size(); ++j){
				Json::Value jskill(Json::objectValue);
				jskill["Id"] = Json::Value((Json::UInt)inst.babyStorage_[i].skill_[j].skillID_);
				jskill["Level"] = Json::Value((Json::UInt)inst.babyStorage_[i].skill_[j].skillLevel_);
				jskill["Exp"] = Json::Value((Json::UInt)inst.babyStorage_[i].skill_[j].skillExp_);
				jbaby["Skills"].append(jskill);
			}
			jroot["BabyStore"].append(jbaby);
		}*/
		Json::FastWriter jwriter;
		strresult = jwriter.write(jroot);
	}
DB_EXEC_UNGUARD_RETURN

	resultgzip(0,strresult.c_str());
}

void ClientHandler::gmQueryRoleList(Json::Value& json)
{
	std::string playerName = json["UserName"].asString();

	std::stringstream sstream ;
	if(playerName.empty()){
		result(-1,"Query case is nil");
		return;
	}
	
	sstream << "SELECT * FROM Player WHERE PlayerName=\"" << playerName <<"\"";
	SQLHelper mysql;
	if(SQLHelper::SUCCESS!=mysql.Connect())
	{
		ACE_DEBUG((LM_ERROR, ACE_TEXT("mysql open error\n")));
		return ;
	}
	DBC *dbc = mysql.getDBC();
	SRV_ASSERT(dbc);
	std::string strresult;
DB_EXEC_GUARD
	std::string accId;
	std::auto_ptr< sql::Statement > stmt(dbc->createStatement());
	std::auto_ptr< sql::ResultSet > res(stmt->executeQuery(sstream.str().c_str()));
	std::vector<SGE_DBPlayerData> rolelist;
	if(res->next())
	{
		SGE_DBPlayerData inst;
		accId = res->getString("UserName");
		S32 len=0;
		sql::SQLString pCacheBlob= res->getString("BinData");
		ProtocolMemReader mr(pCacheBlob->c_str(),pCacheBlob->length());
		
		inst.freeze_ = res->getInt("Freeze");
		inst.seal_ =  res->getInt("Seal");
		inst.deserialize(&mr);
		rolelist.push_back(inst);
	}
	
	Json::Value jroot(Json::arrayValue);
	for (size_t i = 0; i < rolelist.size(); ++i)
	{
		if(rolelist[i].instId_ == 0){
			result(-1,"Query error");
			return;
		}
		Json::Value jrole(Json::objectValue);
		jrole["id"] = Json::Value(accId.c_str());
		jrole["roleId"] = Json::Value((Json::UInt)rolelist[i].instId_);
		jrole["userName"] = Json::Value(rolelist[i].instName_.c_str());
		jrole["level"] = Json::Value((Json::UInt)rolelist[i].properties_[PT_Level]);
		jrole["vip"] = Json::Value((Json::UInt)rolelist[i].properties_[PT_VipLevel]);
		jroot.append(jrole);
	}

	Json::FastWriter jwriter;
	strresult = jwriter.write(jroot);
	
DB_EXEC_UNGUARD_RETURN

	result(0,strresult.c_str());
}