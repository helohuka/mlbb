/************************************************************************/
/**
 * @file	account.h
 * @date	2015-2-2015/02/27 	
 * @author 	Lucifer<hotlala8088@gmail.com>            					
 * */
/************************************************************************/

#ifndef __ACCOUNT_H__
#define __ACCOUNT_H__

#include "config.h"
#include "ScriptHandle.h"
class Player;
class ClientHandler;
class Scene;


class VerifiedAccount ///无效账户 待验证
{
public:
	static std::map<std::string, VerifiedAccount> cache_;
	
	static VerifiedAccount* get(std::string& name);
	static void add(VerifiedAccount va);
	static void del(std::string& name);
	static void del(ClientHandler *client);

	inline ClientHandler* getClient(){
		return client_;
	}
	
public: 
	std::string		username_;
	std::string		password_;
	SDKInfo			sdkInfo_;
	ClientHandler*	client_;
};


enum GMLevel
{
	GML_None,
	GML_Desin,
	GML_All,
	GML_Dev = GML_All,
};

class Account
{
public:
	
	enum State
	{
		None,
		S_SessionLogin,
		S_NeedLoginBack,
		S_NeedDBBack,
		S_Normal,
		Max,
	};

public:
	Account(ClientHandler *handler);
	~Account();
	
	inline Player *getPlayer(){return player_;}
	inline ClientHandler* getClient(){return client_;}
	void reConnect(ClientHandler* cl);
	void deConnect(bool needDisconnect = true);
	bool isOffline(){return client_ == NULL;}
	
	void createPlayerSameName();
	void createPlayerOk(SGE_DBPlayerData &inst);
public:
	COM_SimpleInformation* findDBPlayerById(U32 playerId);
	COM_SimpleInformation* findDBPlayerByName(std::string& name);
	void deleteDBPlayer(COM_SimpleInformation* p);
	Account *asAccount(){return this;}
	void setDBMiniPlayers(std::vector<COM_SimpleInformation>& players);
	//void setDBPlayers(std::vector<SGE_DBPlayerData>& players);
	void getPlayerMini(std::vector<COM_SimpleInformation>& mini);
	//void updateDBPlayer(SGE_DBPlayerData& data);
	void enterGame(SGE_DBPlayerData& dbplayer);
public:
#include "Client2ServerMethods.h"
public:

	U32				guid_;
	GMLevel			gmlev_;
	State			state_;
	ClientHandler*	client_;
	Player*			player_;	///当前Player
	
	time_t			createtime_;
	std::string		ipaddr_;
	int32			serverId_;
	float			sessiontime_;  ///sessionkey有效时间
	time_t			lastping_;
	float			logintime_;
	float			logouttime_;
	std::string		sessionkey_;
	std::string		username_;
	std::string		password_;
	std::string		phoneNumber_;
	SDKInfo			sdkInfo_;
	std::vector<COM_SimpleInformation> miniPlayerList_;
	static std::map<std::string, Account*>	sessionStore_;
	static std::map<std::string, Account*>	accountStore_;		//保存已有角色名称


public:
	static Account *createAccount(ClientHandler *ch, const COM_AccountInfo& accinfo);
	static Account *getAccountBySessionkey(std::string const &sessionkey);
	static Account *getAccountByName(std::string const &accName);	
	static void removeAccount(Account* acc);
	static void removeAccountBySessionkey(std::string const &sessionkey);
	static void removeAccountByName(std::string const& accName);
	static void update(float dt);
	static void clean();

};

#endif