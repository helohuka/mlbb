
#include "handler.h"
#include "config.h"
#include "routine.h"
#include "server.h"
#define CREATE_ROUTINE(P,TYPE) TYPE* P = NULL; Routine::create(P); SRV_ASSERT(P);


bool 
WorldHandler::queryPlayerSimpleInformation(std::string &username,int32 serverId){
	ACE_DEBUG((LM_INFO,ACE_TEXT("queryPlayerSimpleInformation %s \n"),username.c_str()));
	CREATE_ROUTINE(p,QueryPlayerSimple);
	p->serverId_ = serverId;
	p->username_ = username;
	SQLTask::spost(p);
	return true;
}

bool 
WorldHandler::queryPlayer(STRING& username,int32 playerId)
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("Query player %s \n"),username.c_str()));
	CREATE_ROUTINE(p,QueryPlayer);
	p->playerId_ = playerId;
	p->username_ = username;
	SQLTask::spost(p);
	return true;
}

bool 
WorldHandler::createPlayer(STRING& username, SGE_DBPlayerData& inst, int32 serverId)
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("Create player %s \n"),username.c_str()));
	CREATE_ROUTINE(p,InsertPlayer);
	inst.isFirstLogin_			= true;
	inst.genItemMaxGuid_		= 1;
	p->username_				= username;
	p->serverId_				= serverId;
	p->player_					= inst;
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::deletePlayer(STRING& playername)
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("Delete player %s \n"),playername.c_str()));
	CREATE_ROUTINE(p,DeletePlayer);
	p->playername_ = playername;
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::updatePlayer(STRING& username, SGE_DBPlayerData& inst)
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("Update player %s \n"),username.c_str()));
	CREATE_ROUTINE(p,UpdatePlayer);
	p->username_	= username;
	p->player_		= inst;
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::queryPlayerById(std::string& name,S32 instId, int32 where)
{
	//ACE_DEBUG((LM_INFO,ACE_TEXT("queryPlayerById player %d \n"),instId));
	CREATE_ROUTINE(p,QueryPlayerById);
	p->initiator_	= name;
	p->where_	= where;
	p->playerGuid_	= instId;
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::insertEndlessStair(S32 rank, STRING& name)
{
	//ACE_DEBUG((LM_INFO,ACE_TEXT("insertEndlessStair\n")));
	CREATE_ROUTINE(p,InsertEndless);
	p->rank_		= rank;
	p->playerName_	= name;
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::updateEndlessStair(S32 rank, STRING& name)
{
	//ACE_DEBUG((LM_INFO,ACE_TEXT("updateEndlessStair name %s \n"),name.c_str()));
	CREATE_ROUTINE(p,UpdateEndlsee);
	p->rank_		= rank;
	p->playerName_	= name;
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::deleteEndlessStair(std::string& name)
{
	CREATE_ROUTINE(p,DeleteEndlsee);
	p->playerName_	= name;
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::createBaby(STRING& playername,COM_BabyInst& inst,bool isToStorage)
{

	ACE_DEBUG((LM_INFO,ACE_TEXT("Insert Baby %d \n"),inst.instId_));
	CREATE_ROUTINE(p,InsertBaby);
	p->playername_	= playername;
	p->baby_		= inst;
	p->isToStorage_ = isToStorage;
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::deleteBaby(STRING& playername,S32 babyInstId)
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("Delete Baby %d \n"),babyInstId));
	CREATE_ROUTINE(p,DeleteBaby);
	p->playername_	= playername;
	p->babyId_		= babyInstId;
	SQLTask::spost(p);
	return true;
}

//bool
//WorldHandler::resetBabyOwner(std::string &playername,int32 babyInstId){
//	//ACE_DEBUG((LM_INFO,ACE_TEXT("Reset Baby Owner Baby %s %d \n"),playername.c_str(),babyInstId));
//	//CREATE_ROUTINE(p,ResetBabyOwner);
//	//p->playerName_ = playername;
//	//p->babyId_ = babyInstId;
//	//SQLTask::spost(p);
//	return true;
//}

bool
WorldHandler::updateBaby(COM_BabyInst& inst)
{
	Server::instance()->updateBabyInst(inst);
	//ACE_DEBUG((LM_INFO,ACE_TEXT("updateBaby BabyID[%d] \n"),inst.instId_));
	CREATE_ROUTINE(p,UpdateBaby);
	p->baby_ = inst;
	
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::updateBabys(std::string& playername, std::vector< COM_BabyInst >& babys){
	CREATE_ROUTINE(p,UpdateBabyslot);
	p->playerName_ = playername;
	p->babys_ = babys;
	for(size_t i=0; i<babys.size(); ++i){
		Server::instance()->updateBabyInst(babys[i]);
	}
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::queryBabyById(std::string& name, U32 instid)
{
	COM_BabyInst babyInst ;
	if(Server::instance()->getBabyInst(instid,babyInst)){
		if(babyInst.ownerName_ == name)
			queryBabyByIdOK(name,babyInst);
	}
	return true;
}

bool
WorldHandler::createEmployee(STRING& playername,COM_EmployeeInst& inst)
{
	CREATE_ROUTINE(p,InsertEmployee);
	p->playername_	= playername;
	p->employee_	= inst;
	ACE_DEBUG((LM_INFO,ACE_TEXT("Insert Employee %d \n"),inst.instId_));
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::deleteEmployee(STRING& playername,std::vector< U32 >& instIds)
{
	//ACE_DEBUG((LM_INFO,ACE_TEXT("Delete Employee %s \n"),playername.c_str()));
	CREATE_ROUTINE(p,DeleteEmployee);
	p->playername_	= playername;
	p->ids_			= instIds;
	SQLTask::spost(p);

	return true;
}

bool
WorldHandler::updateEmployee(COM_EmployeeInst& inst)
{
	Server::instance()->updateEmployeeInst(inst);
	CREATE_ROUTINE(p,UpdateEmployee);
	p->employee_ = inst;
	//ACE_DEBUG((LM_INFO,ACE_TEXT("updateEmployee Employee[%d]\n"),inst.instId_));
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::queryEmployeeById(std::string& name, U32 instid)
{	
	COM_EmployeeInst employeeInst;
	if(Server::instance()->getEmployeeInst(instid,employeeInst)){
		if(employeeInst.ownerName_ == name){
			queryEmployeeByIdOK(name,employeeInst);
		}
	}
	
	return true;
}

bool WorldHandler::insertMail(COM_Mail& mail){
	//ACE_DEBUG((LM_INFO,ACE_TEXT("Insert mail \n")));

	CREATE_ROUTINE(p,InsertMail);
	p->mail_ = mail;
	SQLTask::spost(p);
	return true;
}

bool WorldHandler::insertMailAll(COM_Mail& mail){
	//ACE_DEBUG((LM_INFO,ACE_TEXT("Insert mail all players\n")));

	CREATE_ROUTINE(p,InsertMailAll);
	p->mail_ = mail;
	SQLTask::spost(p);
	return true;
}

bool WorldHandler::insertMailByRecvs(COM_Mail& mail, std::vector<std::string>& resvs){
	//ACE_DEBUG((LM_INFO,ACE_TEXT("Insert mail by player group \n")));
	
	CREATE_ROUTINE(p,InsertMailByRecvs);
	p->mail_	= mail;
	p->resvs_	= resvs;
	SQLTask::spost(p);
	return true;
}

bool WorldHandler::fatchMail(STRING& recvName, S32 mailId){
	//ACE_DEBUG((LM_INFO,ACE_TEXT("fatchMail(STRING& recvName, S32 mailId)\n")));
	CREATE_ROUTINE(p,FatchMail);
	p->recvName_	= recvName;
	p->fatchId_		= mailId;
	SQLTask::spost(p);
	return true;
}
bool WorldHandler::delMail(STRING& recvName, S32 mailId){
	//ACE_DEBUG((LM_INFO,ACE_TEXT("delMail(STRING& recvName, S32 mailId)\n")));
	CREATE_ROUTINE(p,EraseMail);
	p->recvName_	= recvName;
	p->mailId_		= mailId;
	SQLTask::spost(p);
	return true;
}
bool WorldHandler::updateMail(COM_Mail& mail){
	//ACE_DEBUG((LM_INFO,ACE_TEXT("updateMail(COM_Mail& mail) \n")));
	CREATE_ROUTINE(p,UpdateMail);
	p->mail_ = mail;
	SQLTask::spost(p);
	return true;
}

//帮派
bool
WorldHandler::insertGuild(COM_Guild& guild, COM_GuildMember& guildMember)
{
	//ACE_DEBUG((LM_INFO,ACE_TEXT("intsertGuild(COM_Guild& guild, COM_GuildMember& guildMember) \n")));
	CREATE_ROUTINE(p,InsertGuild);
	p->guild_		= guild;
	p->guildMember_ = guildMember;
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::updateGuildRequestList(U32 guildId, std::vector< COM_GuildRequestData >& data)
{
	//ACE_DEBUG((LM_INFO,ACE_TEXT("updateGuildRequestList(U32 guildId, std::vector< COM_GuildRequestData >& data) \n")));
	CREATE_ROUTINE(p,RefreshGuildRequest);
	p->guildId_		= guildId;
	p->requestList_ = data;
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::createGuildMember(COM_GuildMember& guildMember)
{
	//ACE_DEBUG((LM_INFO,ACE_TEXT("createGuildMember(COM_GuildMember& guildMember) \n")));
	CREATE_ROUTINE(p,CreateGuildMember);
	p->guildMember_ = guildMember;
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::delGuild(S32 guildId)
{
	//ACE_DEBUG((LM_INFO,ACE_TEXT("delGuild(S32 guildId) \n")));
	CREATE_ROUTINE(p,DelGuild);
	p->guildId_ = guildId;
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::updateGuildNotice(U32 guildId, std::string& notice)
{
	//ACE_DEBUG((LM_INFO,ACE_TEXT("updateGuildNotice(U32 guildId, std::string& notice) \n")));
	CREATE_ROUTINE(p,UpdateGuildNotice);
	p->guildId_ = guildId;
	p->notice_  = notice;
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::updateGuild(COM_Guild& guild)
{
	//ACE_DEBUG((LM_INFO,ACE_TEXT("updateGuild(COM_Guild& guild) \n")));
	CREATE_ROUTINE(p,UpdateGuild);
	p->guild_ = guild;
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::updateMemberPosition(S32 roleId, GuildJob job)
{
	//ACE_DEBUG((LM_INFO,ACE_TEXT("updateMemberPosition(S64 roleId, GuildJob job) \n")));
	CREATE_ROUTINE(p,UpdateMemberJob);
	p->roleId_	= roleId;
	p->job_		= job;
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::updateMemberContribution(S32 roleId, S32 contri)
{
	//ACE_DEBUG((LM_INFO,ACE_TEXT("updateMemberContribution(S64 roleId, S32 job) \n")));
	CREATE_ROUTINE(p,UpdateMemberContribution);
	p->roleId_			= roleId;
	p->contribution_	= contri;
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::deleteGuildMember(S32 playerId)
{
	//ACE_DEBUG((LM_INFO,ACE_TEXT("deleteGuildMember(S64 playerId) \n")));

	CREATE_ROUTINE(p,DeleteGuildMember);
	p->roleId_ = playerId;
	SQLTask::spost(p);

	return true;
}

bool WorldHandler::updateGuildStruction(U32 guildId, S8 level, S32 struction){
	//ACE_DEBUG((LM_INFO,ACE_TEXT("updateGuildStruction(S64 playerId) \n")));

	CREATE_ROUTINE(p,UpdateGuildStruction);
	p->guildId_			= guildId;
	p->level_			= level;
	p->contribution_	= struction;
	SQLTask::spost(p);

	return true;
}



bool WorldHandler::insertActivity(ADType adt, SGE_SysActivity& date)
{
	//ACE_DEBUG((LM_INFO,ACE_TEXT("insertActivity\n")));
	CREATE_ROUTINE(p,InsertActivity);
	p->adtype_ = adt;
	p->data_   = date;
	SQLTask::spost(p);
	return true;
}

bool
WorldHandler::insertLoseCharge(int32 playerId, SGE_OrderInfo& order){
	ACE_DEBUG((LM_INFO,ACE_TEXT("insertLoseRMBRecharge %d %s \n"),playerId,order.orderId_.c_str()));
	CREATE_ROUTINE(p,InstertChargeCache);
	p->playerId_	= playerId;
	p->order_		= order;
	SQLTask::spost(p);
	return true;
}

bool WorldHandler::insertEmployeeQuest(U32 playerId, SGE_PlayerEmployeeQuest& data)
{
	//ACE_DEBUG((LM_INFO,ACE_TEXT("insertEmployeeQuest\n")));
	CREATE_ROUTINE(p,InsertEmployeeQuest);
	p->playerID_	= playerId;
	p->data_		= data;
	SQLTask::spost(p);
	return true;
}

bool WorldHandler::delEmployeeQuest(U32 playerId)
{
	ACE_DEBUG((LM_INFO,ACE_TEXT("PLAYER[%d] delEmployeeQuest\n"),playerId));
	CREATE_ROUTINE(p,DelEmployeeQuest);
	p->playerId_ = playerId;
	SQLTask::spost(p);
	return true;
}