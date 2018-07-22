#include "config.h"
#include "player.h"
#include "worldserv.h"
#include "GMCmdMgr.h"
#include "team.h"
#include "FilterWord.h"
#include "broadcaster.h"
#include "TokenParser.h"
#include "baby.h"
#include "Guild.h"
#include "loghandler.h"
#include "robotTable.h"

void Player::requestAudio(int32 audioId){
	std::vector<U8> dummy;
	AudioInfo const * pInfo = WorldServ::instance()->findAudioInfo(audioId);
	if(NULL == pInfo)
		CALL_CLIENT(this,requestAudioOk(-1,dummy));
	else 
		CALL_CLIENT(this,requestAudioOk(audioId,pInfo->bytes_));
}

void 
Player::sendChat(COM_Chat& content, const char* targetName)
{

	COM_ChatInfo info;
	COM_Chat* p = (COM_Chat*)&info;
	*p = content;
	info.playerName_ = playerName_;
	info.assetId_ = getProp(PT_AssetId);
	info.instId_ = getGUID();
	if(info.ck_ == CK_GM)
	{
		bool isRobot = RobotActionTable::isRobot(account_->username_);

		if(!isGm_ && !isRobot){
			return ;
		}
		std::string cmdName;
		const char* pcontent = info.content_.c_str();
		if( !TokenParser::getToken( pcontent, cmdName ) )
			return ;
		std::map< std::string, GMCmdMgr::GmCmd >::iterator iter = GMCmdMgr::GmCmds_.find( cmdName );
		if( iter == GMCmdMgr::GmCmds_.end() )
			return ;

		std::string err;
		if( !ScriptEnv::callGMCmd( getHandleId(), iter->second.scriptFunName_.c_str(), pcontent, err ) )
		{
			ACE_DEBUG( (LM_ERROR, ACE_TEXT("%s\n"), err.c_str() ) );
		}
		LogHandler::instance()->playersay(playerId_,playerName_,content);
		ACE_DEBUG((LM_INFO,"Push GMCMD %s %s\n",playerName_.c_str(),pcontent));
	}
	else
	{
		if(noTalkTime_ > 0)
		{
			CALL_CLIENT(this,errorno(EN_DontTalk));
			return;
		}

		if(FilterWord::isValid(info.content_))
		{
			CALL_CLIENT(this,errorno(EN_FilterWord));
			return;
		}
		
		if(!info.audio_.empty()){
			info.audioId_ = WorldServ::instance()->pushAudioInfo(info.audio_);
			info.audio_.clear();
		}
		else{
			LogHandler::instance()->playersay(playerId_,playerName_,content);
		}

		switch(info.ck_)
		{
		case CK_World:
			
			if(getProp(PT_Level) < 30 )
			{
				errorMessageToC(EN_OpenBaoXiangLevel); //等级不够
				return;
			}

			if(worldTalkTime_ > 0){
				//不能说话
				CALL_CLIENT(this,errorno(EN_DontTalk));
				return;
			}
			worldTalkTime_ = Global::get<float>(C_WorldChatTime);
			
			WorldBroadcaster::instance()->receiveChat(info,COM_ContactInfo() );
			
			return ;
		case CK_Team:
			{
				Team* p = myTeam();
				if(p)
					p->receiveChat(info,COM_ContactInfo() );
				return ;
			}
		case CK_Friend:
			{
				Player* pPlayer = getPlayerByName(targetName);
				
				if(pPlayer == NULL || pPlayer == this)
				{
					CALL_CLIENT(this,errorno(EN_CannotfindPlayer));
					return;
				}
				if(pPlayer->getOpenSubSystemFlag(OSSF_Friend) == false)
				{
					CALL_CLIENT(this,errorno(EN_FirendNotOpen));
					return;
				}
				if(pPlayer->findBlacklistById(getGUID()))
					return; //在目标黑名单中

				if (!findFriendById(pPlayer->getGUID()))
					return;
				COM_ContactInfo * myinfo = WorldServ::instance()->findContactInfo(playerName_);
				CALL_CLIENT(pPlayer, receiveChat(info,*myinfo));
			}
			break;
		case CK_Guild:
			{
				Guild* p = Guild::findGuildByPlayer(getGUID());
				if(p != NULL)
					p->broadcaster_.receiveChat(info,COM_ContactInfo());
			}	
			break;
		}
	}
}

void
Player::setNoTalkTime(float t)
{
	CALL_CLIENT(this,setNoTalkTime(noTalkTime_ = t));
}

bool
Player::publishItemInst(ItemContainerType type, U32 itemInstId, ChatKind chatType, std::string& playerName)
{
	COM_Item* inst =getItemInst(type,itemInstId);
	if (NULL == inst)
		return true;

	COM_ShowItemInst*showItem = WorldServ::instance()->addShowItem(this,*inst);

	COM_ShowItemInstInfo info;
	info.showId_=showItem->showId_;
	info.itemId_=inst->itemId_;
	info.playerName_=showItem->playerName_;

	if(CK_World==chatType)
	{
		NamePlayerMap::iterator itr = nameStore_.begin();

		while(itr != nameStore_.end())
		{
			Player* pPlayer = itr->second;

			if (pPlayer != NULL)
			{
				CALL_CLIENT(this,publishItemInstRes(info,chatType));
			}
			itr++;
		}
	}
	else if (CK_Team==chatType)
	{
		if(isTeamMember())
		{
			CALL_CLIENT(this,errorno(EN_InTeam));
			return false;
		}

		Team* p = TeamLobby::instance()->getTeam(this,teamId_);
		if(p == NULL)
		{
			CALL_CLIENT(this,errorno(EN_InTeam));
			return false;
		}
		for(int i=0; i<p->teamMembers_.size(); ++i)
		{
			if(p->teamMembers_[i] == NULL)
				continue;
			CALL_CLIENT(p->teamMembers_[i],publishItemInstRes(info,chatType));
		}
	}

	return true;
}

bool Player::queryItemInst(int32  showId)
{
	COM_ShowItemInst* inst=WorldServ::instance()->getShowItemById(showId);
	if (inst ==NULL)
		return true;
	CALL_CLIENT(this,queryItemInstRes(*inst));
	return true;
}

bool
Player::publishbabyInst(ChatKind chatType,U32 babyInstId, std::string& playerName)
{
	Baby* pBaby = findBaby(babyInstId);
	if(pBaby == NULL)
	{
		CALL_CLIENT(this,errorno(EN_NoBaby));
		return false;
	}
	COM_BabyInst inst;
	pBaby->getBabyInst(inst);
	COM_ShowbabyInst*showBaby =WorldServ::instance()->addShowBaby(this,inst);

	COM_ShowbabyInstInfo info;
	info.showId_=showBaby->showId_;
	info.babyId_=inst.properties_[PT_TableId];
	info.playerName_=showBaby->playerName_;

	if(CK_World==chatType)
	{
		NamePlayerMap::iterator itr = nameStore_.begin();

		while(itr != nameStore_.end())
		{
			Player* pPlayer = itr->second;

			if (pPlayer != NULL)
			{
				CALL_CLIENT(this,publishBabyInstRes(info,chatType));
			}
			itr++;
		}
	}
	else if (CK_Team==chatType)
	{
		if(isTeamMember())
		{
			CALL_CLIENT(this,errorno(EN_InTeam));
			return false;
		}

		Team* p = TeamLobby::instance()->getTeam(this,teamId_);
		if(p == NULL)
		{
			CALL_CLIENT(this,errorno(EN_InTeam));
			return false;
		}
		for(int i=0; i<p->teamMembers_.size(); ++i)
		{
			if(p->teamMembers_[i] == NULL)
				continue;
			CALL_CLIENT(p->teamMembers_[i],publishBabyInstRes(info,chatType));
		}
	}
	return true;
}

bool
Player::querybabyInst(S32 showId)
{
	COM_ShowbabyInst* inst=WorldServ::instance()->getShowBabyById(showId);
	if (inst ==NULL)
		return true;
	CALL_CLIENT(this,queryBabyInstRes(*inst));
	return true;
}