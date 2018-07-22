/** File generate by <hotlala8088@gmail.com> 2015/01/13  
 */

#ifndef __WORLDSERV_H__
#define __WORLDSERV_H__

#include "config.h"
#include "client.h"
#include "player.h"
#include "gwhandler.h"
#include "dbhandler.h"
#include "loginhandler.h"
#include "GMThandler.h"
#include "MallHandle.h"
#include "broadcaster.h"
#include "PeriodEvent.h"
#include "ShowDataTemplate.h"
#include "centerServer.h"
//#include "gift.h"
//#include "sms.h"
//#include "logTask.h"
struct NoticeCmd{
	NoticeSendType type_;
	float thetime_;
	float itvtime_;
	float dlttime_;
	std::string content_;
};

struct AudioInfo{
	AudioInfo(int audioId, std::vector<U8>& bytes):audioId_(audioId),bytes_(bytes){}
	int audioId_;
	std::vector<U8> bytes_;
};

class WorldServ 
	: public ACE_Service_Object
{
public:
	SINGLETON_FUNCTION(WorldServ);
public:
	int init (int argc, ACE_TCHAR *argv[]);
	int fini (void);
	int handle_timeout (const ACE_Time_Value &current_time, const void *act);

	void addContactInfo(std::vector<SGE_ContactInfoExt>& info);
	void addContactInfo(SGE_ContactInfoExt info);
	void delContactInfo(U32 id);
	std::string getAccontNameByPlayerId(uint32 playerId);
	COM_ContactInfo* findContactInfo(U32 id);
	SGE_ContactInfoExt* findContactInfoExt(uint32 id);
	COM_ContactInfo* findContactInfo(std::string name);
	void updateContactInfo(Player* player);
	void getContactInfos(U32 index,std::vector<COM_ContactInfo>& infos);
	void vipitemmaill(std::string& sendName,std::string &title,std::string &content);
	void updateAverageLevel();
	U32 getAverageLevel(){return averageLevel_;}

	void passZeroHour();
	bool loadPeriodEvent(const char* fn);
	void registerPeriodEvent( PeriodEvent* newEvent );
	void updatePeriodEvents();
	void updateRoleLogTable();
public:
	void accept();
public:
	void storeCmd(std::string & cmd);
	void doCmd();
	
	void sendMailByDrop(std::string& sendName, std::string& recvPlayerName, std::string &title, std::string &content , S32 dropId);
	void sendMailByDrop(std::string& sendName, std::vector<std::string>&recvs, std::string &title, std::string &content , S32 dropId);
	void sendMailByDropAll(std::string& sendName, std::string &title, std::string &content , S32 dropId);
	void sendMail(Player* player, std::string& recvPlayerName, std::string &title, std::string &content, std::vector<COM_MailItem> &items);
	void sendMail(std::string& sendName, std::string& recvPlayerName, std::string &title, std::string &content , S32 money , S32 diamond ,  std::vector<COM_MailItem> &items);
	void sendMail(std::string& sendName, std::vector<std::string>& resvs, std::string &title, std::string &content , S32 money , S32 diamond ,  std::vector<COM_MailItem> &items);
	void sendMailAll(COM_Mail& mail);
	void sendMailAllOnline(COM_Mail& mail,int32 lowLevel,int32 highLevel,int64 lowTime, int64 highTime);
	void notice(std::string& content, bool isGm);
	void gmNotice(NoticeSendType bType, std::string& note, U64 thetime, S64 itvtime);
	void updateGmNotice(float delta);
	void savewish(COM_Wish& wish);
	COM_Wish* getWish();
public:
	COM_ShowItemInst*		 addShowItem(Player* player,COM_Item& itemInst);
	COM_ShowItemInst*		 getShowItemById( int32 showId );
	void					 sysShowItem(Player* player,int32 cType,int32 itemInstId,const std::string content);
	COM_ShowbabyInst*		 addShowBaby(Player* player,COM_BabyInst& babyInst);
	COM_ShowbabyInst*		 getShowBabyById( int32 showId );
	uint32 getMaxGuid(){
		return ++maxGuid_;
	}
public:
	void syncPlayerFFRank(std::vector< COM_ContactInfo >& infos);
	void getPlayerFFRank(std::vector< COM_ContactInfo >& infos);
	void syncPlayerLevelRank(std::vector< COM_ContactInfo >& infos);
	void getPlayerLevelRank(std::vector< COM_ContactInfo >& infos);
	void fatchBabyRankOK(std::vector< COM_BabyRankData >& infos);
	void getBabyFFRank(std::vector< COM_BabyRankData >& infos);
	void fatchEmpRankOK(std::vector<COM_EmployeeRankData>& infos);
	void getEmployeeFFRank(std::vector<COM_EmployeeRankData>& infos);
	void updateBabyRank(U32 instid);
	void updateBabyRank(U32 instid, std::string name);

	COM_ContactInfo*		findPlayerFFRank(U32 instid);
	COM_ContactInfo*		findPlayerLevelRank(U32 instid);
	COM_BabyRankData*		findBabyFFRank(U32 instid);
	COM_EmployeeRankData*	findEmployeeFFRank(U32 instid);

	void deleteRank(std::string const& playerName);
	void delBabyRank(U32 instid);
	void calcPlayerLevelRank(COM_ContactInfo info);
	void calcPlayerFFRank(COM_ContactInfo info);
	void calcBabyFFRank(COM_BabyRankData ff);
	void calcEmployeeFFRank(COM_EmployeeRankData ff);
	void checkplayerlevelrank();
public:
	AudioInfo const * findAudioInfo(int audioId);
	int pushAudioInfo(std::vector<U8>& bytes);
public:
	void reqCDKey(std::string cdkey, std::string playername, std::vector<std::string> &giftNames);
public:
	void prepareVerificationCode(std::string phoneNumber, uint32 playerId);
	void complateVerificationCode();
public:
	void pushAccountLog(Account *acc);
	void pushLoginLog(Player *player);
	void pushLoginLog(Account *acc);
	void pushOrderLog(Account *acc, int32 playerId, int32 playerLevel, std::string const &orderId, int32 payment, std::string const &payTime);
	void pushRoleLog(std::vector<SGE_ContactInfoExt*> &infos);
public:
	bool						quitFlag_;
	time_t						curTime_;			//<全局时间戳
	time_t			smsTimeout_;
	ACE_Recursive_Thread_Mutex  gmcmdmutex_;
	std::queue< std::string >	gmcmdqueue_;
	
	U32 averageLevel_;
	NameContentInfoMap		contactInfoNameIndex_;
	IdContentInfoMap		contactInfoIdIndex_;
	ContextInfoList			contactInfoCache_;			
	// periodEvent
	typedef std::list<PeriodEvent*>		PeriodEventList;
	typedef PeriodEventList::iterator	PeriodEventListIter;
	PeriodEventList						periodEventsList_;	//<周期事件.
	//show data
	int32								maxShowItemId_;
	uint32								maxGuid_; ///创建 角色 佣兵 宝宝使用
	ShowItemInstData					showItemInstData_;
	ShowbabyInstData					showBabyInstData_;

	SyncCentreServerTask				syncCentreTask_;
	//GiftTask							giftTask_;
	//SMSTask								smsTask_;
	//LogTask								logTask_;
	std::vector<U32>			inDoorIds_;
	///tmp	
private:
	//排行榜数据
	std::vector<COM_ContactInfo>		playerlevelrank_;	//角色等级排行
	std::vector<COM_ContactInfo>		playerFFrank_;		//角色战斗力排行
	std::vector<COM_BabyRankData>		babyffrank_;		//宠物战斗力排行
	std::vector<COM_EmployeeRankData>	employeeffrank_;	//伙伴战斗力排行
	//--
	std::vector<AudioInfo>				audioCache_;
	std::vector<NoticeCmd>				gmNotice_;
	std::vector<COM_Wish>				wishstore_;			//许愿池
	
	
	//std::vector<SMSContent>		prepareSMS_;
	//std::vector<SMSContent>		complateSMS_;

	
};



#endif /// endif __WORLDSERV_H__