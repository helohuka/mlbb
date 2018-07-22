//==============================================================================
/**
@date:		2015:10:15
@file: 		Guild.h
@author:	liwenhao
*/
//==============================================================================
#ifndef __GUILD_H__
#define __GUILD_H__

#include "config.h"
#include "broadcaster.h"

class Player;

class Guild
{
	Guild():battleSceneCopyId_(0),bInfo_(NULL),battleWinCount_(0){
	}
public:
	enum BattleState{
		BS_Close,
		BS_Prepare,
		BS_Battle,
		BS_Stop,
	};

	enum BattleWinner{
		BW_None,
		BW_Left,
		BW_Right,
		BW_Lose,
	};

	struct BattleInfo{

		BattleInfo(Guild* pL, Guild* pR):isCalced_(false),pLeft(pL),pRight(pR)
		{
			pLeft->isLeft_ = true; 
			pRight->isLeft_ = false; 
			pLeft->bInfo_ = this;
			pRight->bInfo_ = this;
			winner_ = BW_None;
		}
		
		inline Guild* getOtherGuild(Guild* mine){
			if(pLeft!=mine && pRight !=mine)
				return NULL;
			return pLeft == mine ? pRight : pLeft;
		}

		inline bool hasWinner(){return winner_ != BW_None;}
		inline void setWinner(BattleWinner bw){winner_ = bw;}
		inline void open(int32 copyId);
		void close();
		bool isCalced_;
		Guild* pLeft;
		Guild* pRight;
		uint32 battleSceneId_;
		BattleWinner winner_;
		std::vector<S32> usedNpcs_;
	};
	
	static int					battleIntervalTime_;
	static BattleState				battleState_;
	static std::vector<BattleInfo*>  battleInfos_;
	
	static bool isBattlePrepare(){return battleState_ == BS_Prepare;}
	static bool isBattleStart(){return battleState_ == BS_Battle;}
	static bool isBattleOpen(){return battleState_ != BS_Close;}

	static void openBattle();
	static void startBattle();
	static void stopBattle();
	static void closeBattle();
	static void calcBattle();
	static void updateBattle(float interval);
	static void talked2Progenitus(Player* player,int32 npcId);
	static void prepareBattleTimeout();

	static void openGuildDemonInvaded(); //开启魔族入侵
	static void closeGuildDemonInvaded(); //关闭魔族入侵
	static void openGuildLeaderInvaded(); //开启首领入侵
	static void closeGuildLeaderInvaded(); //关闭首领入侵

public:

	static void clear();
	static std::vector<Guild*>  guildList_;
	static IdGuildMap			guilds_;			//<主索引
	static IdGuildMap			playerGuild_;		//<玩家id 索引
	static NameGuildMap			nameGuild_;			//<帮派名索引
	static std::set<std::string>		tempGuildName_;		//临时占位使用
	static std::set<S32>				tempGuildPlayerId_;
	static S32							maxGuildId_;		//<最大帮派id
	
	static bool		addCritical(const std::string& name , S32 id);
	static void		delCritical(const std::string& name , S32 id);
	
	static void		calcGuildRank();
	static Guild*	findGuildById(U32 guildId);
	static Guild*	findGuildByPlayer(S32 player);
	static Guild*	findGuildByName(const std::string& guildName);

	static Guild*	addGuild(COM_Guild&guild);
	static bool		delGuild(U32 id);	
	static bool		addGuildMember(COM_GuildMember& member);
	static bool		delGuildMember(S32 roleId, bool isKick);
	static void		checkGuildMember(std::vector< COM_GuildMember >& guildMember);
	
	static void		passZeroHour();
	static void		createGuild(Player* player,const std::string& guildName);
	static void		memberLevelUp(Player *player);
	static void		memberchangeProfession(Player *player);
	static void		memberOnline( Player* player);
	static void		memberOffline( Player* player);
	static void		leave(Player* player);
	static void		leave(int32 playerId); //封号特殊处理
	static void		kickOut(Player* player, S32 currPlayerID);
	static void		loseLeader(Player* player);
	static void		requestJoinGuild(Player*player , U32 guild);	
	static void		acceptRequestGuild(Player*player , S32 playerId);
	static void		refuseRequestGuild(Player*player , S32 playerId);
	static void		changeNotice(Player*   player , const std::string&);//<修改公告
	static void		changePosition(Player* player , S32 playerId , GuildJob job);	//<修改职位	
	static void		deposeMemberPosition(Player* player , S32 playerId);
	static void		transferPremier(Player*player , S32 playerId);//<移交帮主	
	static void		addMemberContribution(Player* player,S32 contri);
	static void		queryGuildList(Player* player , S16 page );
	static void		addGuildMoney(Player* player , int32 money); ///<捐献资金
	static void		addGuildMoneyOnly(Player* player , int32 gmoney); ///<只加不扣钱 捐献资金
	static void		levelupBuilding(Player* player,GuildBuildingType gbt);
	static void		modifyMemberContribution(Player* player , S32 cont);
	static void		inviteJoinGuild(Player* sendPlayer,const std::string& name);
	static void		respondInviteJoinGuild(const std::string& sendPlayer,U32 rePlayerId);
	static void		requestGuildShopItems(Player* player);
	static void		requestGuildShopItems(COM_Guild const& guild, COM_GuildMember & member);
	static COM_Skill* findMemberSkill(COM_GuildMember & member,int32 skId);
	static void presentGuildItem(Player*player,int32 num);
	//帮派技能
	static void levelupSkill(Player* player, int skId);
	static void progenitusAddExp(Player* player,int32 mId,bool isSuper);
	static void setProgenitusPosition(Player* player,int32 mId, int32 position);
	static int32 getGuildRank(int32 guildId);
	/** @remarks 帮派建筑捐献
	 *	@param	 type 建筑类型
	 *  @param	 cont 捐献数量
	 *	@return	 返回捐献剩余的钱 $ 如果达到顶级只捐差的一部分 $ 
	 * */
	/////////////////////////////////////////////////////////////////////
	int32					getGuildLevel();
	int32					getMemberLimit(); //<获得帮派最大人数限制
	int32					getGuildMoneyLimit(); ///获得帮派最大资金上限
 	void					updateMemberList(Player* player);					//<更新帮派成员列表
	void					updateMember(COM_GuildMember& member,ModifyListFlag change,bool self=false); //<更新成员
	void					updateGuild();											//<更新帮派数据
	
	bool					addMember(COM_GuildMember& member );					//<添加成员
	bool					delMember(S32 playerId );							//<删除成员
	COM_GuildMember*		findMember(S32 playerId);								//<查询成员
	S16						getPositionNum(GuildJob job);							//<查询职位人数
	int						getOnlinePlayerNum();
	bool					addRequestList(COM_GuildRequestData& newRequest);		//<添加申请列表
	bool					delRequestList(S32 playerId);							//删除申请列表
	COM_GuildRequestData const *findRequest(S32 roleId);							//<查询申请列表
	void					guildsign(Player* player);								//家族签到
	void					resetGuildSign();
	COM_Skill*				findGuildSkill(int skId);
	///建筑升级
	int32					getBuildingLevel(GuildBuildingType gbt);
	bool					updateStruction(S8 level ,S32 struction);			///<更新建设
	COM_GuildProgen*		getProgenitusById(int32 mid);
	//修改个人贡献
	void					exportGuildViewerData(COM_GuildViewerData& guildViewerData);
	void checkPresent(); ///检测家族捐献
	void checkFundz();
	void requestGuildShopItems(COM_GuildMember & member);
	void requestGuildShopItems();
	int32 getTotalFF()const;
	int32 sumInBattleSceneMembers();
	void openGuildBattle(int32 otherId,bool needEvt = true);
	void closeGuildBattle(bool isWin);
	void addGuildBattleWinCount(int32 i);
	void sendMemberMail(std::string & sender,std::string & title, std::string & content, int32 dia, int32 money, std::vector<COM_MailItem> & items);
	void	transferPremier(S32 officer, S32 playerId);
	COM_GuildMember* getLeader();
public:
	int32 demonCount_;
	int32 leaderCount_;
	bool isLeft_;
	BattleInfo* bInfo_;
	int32 battleSceneCopyId_;
	int32 battleWinCount_;
	IdGuildMemberMap guildMember_;
	COM_Guild		 guildData_;
	Broadcaster		 broadcaster_;
	std::string otherGuildName_;
};

#endif
