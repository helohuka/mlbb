
#ifndef __SQL_H__
#define __SQL_H__

#include "config.h"
#include "server.h"
#include "routine.h"
#include "sqltask.h"

U32 GetInstanceMaxId(SQLTask *pTask);

bool CheckSamePlayerName(SQLTask *pTask,std::string const& name);

class InitSQLTables : public Routine
{
public:
	U32 go(SQLTask *pTask);
};

class QueryPlayerContact : public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_PLAYER;}
};

class QueryBabyCache : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back(){ 
		ACE_DEBUG((LM_INFO,"Query baby cache ok!!\n"));
		
		return 0;};
	inline U32 getTaskId(){return TASK_BABY;}
};

class QueryEmployeeCache : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back(){ 
		ACE_DEBUG((LM_INFO,"Query employee cache ok!!\n"));

		return 0;};
		inline U32 getTaskId(){return TASK_EMPLOYEE;}
};

///查询角色
class QueryPlayerSimple : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_PLAYER;}

	bool								hasPlayer_;
	int32								serverId_;
	std::string							username_;
	std::vector<COM_SimpleInformation>	players_;
};

///查询角色
class QueryPlayer : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_PLAYER;}
	
	int32						playerId_;
	std::string					username_;
	SGE_DBPlayerData			player_;
};

class QueryPlayerById : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_PLAYER;}
	int32						where_;
	std::string					initiator_;
	U32							playerGuid_;
	SGE_DBPlayerData			player_;
};

class QueryPlayerByLevel : public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_PLAYER;}
};

class QueryPlayerByFF : public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_PLAYER;}
};

///插入角色
class InsertPlayer : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_PLAYER;}
	std::string						username_;
	SGE_DBPlayerData				player_;
	int32 serverId_;
	bool hasSameName_;
};

///删除角色
class DeletePlayer : public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_PLAYER;}
	std::string					playername_;
};

///更新角色
class UpdatePlayer : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_PLAYER;}
	std::string					username_;
	SGE_DBPlayerData			player_;
};

//插入JJC数据
class InsertEndless : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_ENDLESSSTAIR;}

	std::string			playerName_;
	U32					rank_;
};

//更新JJC数据
class DeleteEndlsee : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_ENDLESSSTAIR;}

	std::string			playerName_;
};

//更新JJC数据
class UpdateEndlsee : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_ENDLESSSTAIR;}

	std::string			playerName_;
	U32					rank_;
};

//获取JJC数据
class QueryEndlessAllDate : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_ENDLESSSTAIR;}

	std::vector<std::string> names_;
};

class InsertBaby : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_BABY;}
	std::string					playername_;
	bool						isToStorage_;
	COM_BabyInst				baby_;
};

class DeleteBaby : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_BABY;}
	std::string playername_;
	U32							babyId_;
};

class UpdateBaby : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_BABY;}
	COM_BabyInst				baby_;
};

class UpdateBabyslot : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_BABY;}
	std::string playerName_;
	std::vector<COM_BabyInst> babys_;
};

class ResetBabyOwner : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_BABY;}
	
	U32 babyId_;
	std::string playerName_;
};


class QueryBabyByFF : public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_BABY;}
};

class QueryBabyById : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_BABY;}
	bool						hasBaby_;
	std::string					playerName_;
	U32							babyInstId_;
	COM_BabyInst				baby_;
};

class InsertEmployee : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_EMPLOYEE;}
	std::string playername_;
	COM_EmployeeInst				employee_;
};

class DeleteEmployee : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_EMPLOYEE;}
	std::string playername_;
	std::vector<U32>		ids_;
};

class UpdateEmployee : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_EMPLOYEE;}
	COM_EmployeeInst				employee_;
};

class QueryEmployeeByFF : public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_EMPLOYEE;}
};

class QueryEmployeeById : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_EMPLOYEE;}
	bool							hasEmployee_;
	std::string						playerName_;
	U32								employeeInstId_;
	COM_EmployeeInst				employee_;
};

class InsertMail : public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_MAIL;}

	COM_Mail mail_;
};

class InsertMailAll : public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_MAIL;}

	COM_Mail mail_;
};

class InsertMailByRecvs : public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_MAIL;}

	COM_Mail mail_;
	std::vector<std::string> resvs_;
};

class UpdateMail : public Routine{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_MAIL;}
	COM_Mail mail_;
};

class EraseMail : public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_MAIL;}

	std::string recvName_;
	S32 mailId_;
};

class FatchMail : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_FATCH_MAIL;}

	S32			fatchId_;
	std::string recvName_;
	std::vector<COM_Mail> mails_;
};

//帮派
class FetchGuild : public Routine
{
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_GUILD;}
	
	std::vector<COM_Guild>	guilds_;
};

class FetchGuildMember : public Routine
{
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_GUILD;}

	std::vector<COM_GuildMember> guildMembers_;
};

class InsertGuild : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_GUILD;}

	COM_Guild		guild_;
	COM_GuildMember guildMember_;
};


class UpdateGuildNotice:public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_GUILD;}
	U32		guildId_;
	std::string notice_;
};

class UpdateGuildStruction:public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_GUILD;}
	U32		guildId_;
	S32		level_;
	S32		contribution_;
};

class RefreshGuildRequest:public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_GUILD;}
	S32								guildId_;
	std::vector<COM_GuildRequestData> 	requestList_;
};

class UpdateGuildBuilding:public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_GUILD;}
	S32								guildId_;
	std::vector<COM_GuildBuilding> 		buildings_;
};
class DelGuild: public Routine
{
public:
	virtual U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_GUILD;}
	S32 guildId_;
};
class ClearAllGuildCredit:public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_GUILD;}
};
class CreateGuildMember: public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_GUILD;}
	COM_GuildMember guildMember_;
};

class UpdateMemberJob:public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_GUILD;}
	S32			roleId_;
	GuildJob		job_;
};

class UpdateMemberContribution:public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_GUILD;}
	S32			roleId_;
	S32			contribution_;
};

class UpdateMembersContribution:public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_GUILD;}
	std::vector<S32>			roles_;
	S32						contribution_;
};

class UpdateMemberOfflineTime:public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_GUILD;}
	S32			roleId_;
	S32			offlineTime_;
};
class UpdateGuild: public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_GUILD;}
	COM_Guild	guild_;
};

class DeleteGuildMember:public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_GUILD;}
	S32 roleId_;
};

//激活码
//class Queryidgen : public Routine
//{
//public:
//	U32 go(SQLTask *pTask);
//	U32 back();
//	inline U32 getTaskId(){return TASK_IDGEN;}
//	std::string idgen_;
//	bool		hasIdgen_;
//	std::string playerName_;
//	COM_KeyContent content_;
//};

class UpdateKeyGift : public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_SAVEKEYGIFT;}
	COM_KeyContent giftdata_;
};

class QueryKeyGift : public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_SAVEKEYGIFT;}
};

//运营活动
class InsertActivity : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_ACTIVITY;}

	ADType			adtype_;
	SGE_SysActivity	data_;
};

class FetchActivity : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_ACTIVITY;}

	ADType			adtype_;
	SGE_SysActivity	data_;
};

//添加充值信息
class InstertChargeCache  : public Routine{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_PLAYER;}
	
	int32 playerId_;
	SGE_OrderInfo order_;
};

//伙伴任务
class InsertEmployeeQuest : public Routine
{
public:
	U32 go(SQLTask *pTask);
	U32 back();
	inline U32 getTaskId(){return TASK_EMPLOYEEQUEST;}

	U32			playerID_;
	SGE_PlayerEmployeeQuest	data_;
};

class DelEmployeeQuest : public Routine
{
public:
	U32 go(SQLTask *pTask);
	inline U32 getTaskId(){return TASK_EMPLOYEEQUEST;}

	U32			playerId_;
};

class FetchEmployeeQuest : public Routine
{
public:
	U32 go(SQLTask *pTask);
	//U32 back();
	inline U32 getTaskId(){return TASK_EMPLOYEEQUEST;}
};

#endif