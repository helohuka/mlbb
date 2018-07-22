#include "EndlessStair.h"
#include "robotTable.h"
#include "battle.h"
#include "worldserv.h"
#include "GameEvent.h"
#include "PVPrunkTable.h"
#include "DropTable.h"

class RankSort
{
public:
	bool operator()(COM_EndlessStair l, COM_EndlessStair r)
	{
		if(l.rank_ < r.rank_)
			return true;
		else if(l.rank_ > r.rank_)
			return false;
		else 
			return false;
	}
};

std::map<U32,Robot*>		EndlessStair::robots_;
std::vector<std::string>	EndlessStair::nameRate_;

EndlessStair::EndlessStair()
{

}

void
EndlessStair::init()
{

}

void
EndlessStair::getEndlessStairData(std::vector<std::string> name)
{
	for (size_t i = 0; i < name.size(); ++i)
	{
		if(findRankByName(name[i]) != -1)
			continue;
		nameRate_.push_back(name[i]);
	}
	ACE_DEBUG((LM_INFO,"getEndlessStairDate OK\n"));
	robotInst();
}

void
EndlessStair::fini()
{
	for (size_t i = 0; i < robots_.size(); ++i){
		if(robots_[i])
			DEL_MEM(robots_[i]);
	}

	robots_.clear();
}

void
EndlessStair::robotInst()
{
	std::map< S32 , RobotTab::RobotData*>::iterator itor = RobotTab::data_.begin();

	while (itor != RobotTab::data_.end())
	{
		RobotTab::RobotData* pRobotData = itor->second;

		if(pRobotData == NULL)
			continue;
		if(robots_.find(pRobotData->robotId_) != robots_.end())
		{
			++itor;
			continue;
		}
		Robot* pRobot = NEW_MEM(Robot,pRobotData->robotId_);
		robots_[pRobotData->robotId_] = pRobot;
		if(findRankByName(pRobotData->robotName_) != -1)
		{
			++itor;
			continue;
		}
		nameRate_.push_back(pRobotData->robotName_);
		DBHandler::instance()->insertEndlessStair(nameRate_.size(),pRobotData->robotName_);
		++itor;
	}

	//ACE_DEBUG((LM_INFO,"robotInst ok\n"));
}

void
EndlessStair::queryPlayerDataBack(const std::string &rname ,SGE_DBPlayerData &data)
{
	
	std::vector<std::string>::iterator itr = std::find(rivalStub_.begin(),rivalStub_.end(), rname);
	if(itr == rivalStub_.end()){
		return; //没有在注册列表里 直接返回
	}
	rivalStub_.erase(itr);
	
	Player* rp = Player::getPlayerByName(rname);
	if(rp == NULL)
		return;
	
	if(rp->isBattle())
	{
		CALL_CLIENT(rp, errorno(EN_Battle));
		return;
	}

	Robot*	robot = NEW_MEM(Robot,data);
	Battle* pBattle = Battle::getOneFree();

	pBattle->create(rp,robot);	
	rp->resetRivalTime();
	rp->calcRivalNum();
}

Robot*
EndlessStair::findRobotByName(std::string name)
{
	std::map<U32,Robot*>::iterator itor = robots_.begin();

	while (itor != robots_.end())
	{
		Robot* pTmp = itor->second;

		if(pTmp == NULL)
		{
			SRV_ASSERT(pTmp);
			continue;
		}

		if(strcmp(name.c_str(),pTmp->robotName_.c_str()) == 0)
			return pTmp;

		++itor;
	}

	return NULL;
}

int
EndlessStair::findRankByName(std::string name)
{
	for (size_t i = 0; i < nameRate_.size(); ++i)
	{
		if(strcmp(nameRate_[i].c_str(), name.c_str()) == 0)
			return i+1;
	}

	return -1;
}

bool
EndlessStair::findRivalName(std::string name)
{
	for (size_t i = 0; i < rivalName_.size(); ++i)
	{
		if(strcmp(rivalName_[i].c_str(),name.c_str()) == 0)
			return true;
	}

	return false;
}

void
EndlessStair::calcRival(int rank)
{
	//计算规则见JJC文档
	if(rank == -1)
		return;

	if(rank <= 5)
	{
		for (size_t i = 1; i < 6; ++i)
		{
			if(i == rank)
				continue;

			rivalName_.push_back(nameRate_[i-1]);
		}
	}
	else if(rank <= 20)
	{
		while(rivalName_.size() < 4)
		{
			U32 index = UtlMath::randNM(1,rank);
			
			if(findRivalName(nameRate_[index-1]))
				continue;
			if(index == rank)
				continue;
			rivalName_.push_back(nameRate_[index-1]);
		}
	}
	else
	{
		U32 index = 0;

		while(rivalName_.size() < 4)
		{
			index = UtlMath::randNM((int)rank*0.3,(int)rank*0.4);
	
			while(index == rank || findRivalName(nameRate_[index-1]))
			{
				index = UtlMath::randNM((int)rank*0.3,(int)rank*0.4);
			}
			rivalName_.push_back(nameRate_[index-1]);

			//--
			index = UtlMath::randNM((int)rank*0.5,(int)rank*0.7);
			while(index == rank || findRivalName(nameRate_[index-1]))
			{
				index = UtlMath::randNM((int)rank*0.5,(int)rank*0.7);
			}
			rivalName_.push_back(nameRate_[index-1]);

			//--
			index = UtlMath::randNM((int)rank*0.8,(int)rank*0.9);
			while(index == rank || findRivalName(nameRate_[index-1]))
			{
				index = UtlMath::randNM((int)rank*0.8,(int)rank*0.9);
			}
			rivalName_.push_back(nameRate_[index-1]);

			//--
			index = UtlMath::randNM((int)rank*0.95,(int)rank*0.99);
			while(index == rank || findRivalName(nameRate_[index-1]))
			{
				index = UtlMath::randNM((int)rank*0.95,(int)rank*0.99);
			}
			rivalName_.push_back(nameRate_[index-1]);
		}
	}
}

void
EndlessStair::checkRival(Player* pPlayer)
{
	if(pPlayer == NULL)
		return;

	if(pPlayer->getProp(PT_Level) < Global::get<int>(C_JJCOpenlevel))
		return;

	rivalName_.clear();

	if(findRankByName(pPlayer->getNameC()) == -1)
	{
		nameRate_.push_back(pPlayer->getNameC());
		DBHandler::instance()->insertEndlessStair(nameRate_.size(),pPlayer->getNameC());
		pPlayer->save();
	}

	calcRival(findRankByName(pPlayer->getNameC()));

	std::vector<COM_EndlessStair> infos;

	for (size_t i = 0; i < rivalName_.size(); ++i)
	{
		COM_EndlessStair info;

		COM_ContactInfo* pContactInfo = WorldServ::instance()->findContactInfo(rivalName_[i]);

		if(pContactInfo)
		{
			info.rank_ = findRankByName(rivalName_[i]);
			info.name_ = rivalName_[i];
			info.level_= pContactInfo->level_;
			info.assetId_ = pContactInfo->assetId_;
			info.job_  = pContactInfo->job_;
			info.joblevel_ = pContactInfo->jobLevel_;

			infos.push_back(info);
		}
		else
		{
			Robot* pRobot = findRobotByName(rivalName_[i]);

			if(pRobot)
			{
				info.rank_  = findRankByName(rivalName_[i]);
				info.name_  = rivalName_[i];
				info.job_	= pRobot->Job_;
				info.joblevel_ = pRobot->JobLevel_;
				info.level_ = pRobot->getProp(PT_Level);
				info.assetId_ = pRobot->getProp(PT_AssetId);

				infos.push_back(info);
			}
		}

		//ACE_DEBUG((LM_INFO,"jjc checkRival rank(%d)\n",info.rank_ ));
	}

	RankSort rs;
	std::sort(infos.begin(),infos.end(),rs);

	CALL_CLIENT(pPlayer,requestRivalOK(infos));
}

void
EndlessStair::creatArena(Player* player,std::string name)
{
	if(player == NULL)
		return;

	if(player->getProp(PT_Level) < Global::get<int>(C_JJCOpenlevel))
		return;

	if(findRankByName(name) == -1 || strcmp(player->getNameC(), name.c_str()) == 0)
		return;
	if(player->isTeamMember())
		return;
	if(player->rivalTimes_ > 0)
		return;

	
	Battle* pBattle = Battle::getOneFree();

	Robot* pRobot = findRobotByName(name);
	
	if(pRobot)
	{
		if(player->isBattle() || pRobot->isBattle())
		{
			CALL_CLIENT(player, errorno(EN_Battle));
			return;
		}

		pBattle->create(player, pRobot);

		player->resetRivalTime();
		player->calcRivalNum();
	}
	else
	{

		std::vector<std::string>::iterator itr = std::find(rivalStub_.begin(), rivalStub_.end(), player->getNameC());
		if(itr != rivalStub_.end()){
			return ;//耐心等待创建战斗
		}


		COM_ContactInfo* pCont = WorldServ::instance()->findContactInfo(name);

		if(pCont == NULL)
		{
			ACE_DEBUG((LM_ERROR,"can not find player %d jjc \n",name.c_str()));
			return;
		}
	
		rivalStub_.push_back(player->getNameC());

		DBHandler::instance()->queryPlayerById(player->getNameC(),pCont->instId_,0);
		

	}

	GEParam params[1];
	params[0].type_ = GEP_INT;
	params[0].value_.i = findRankByName(name);

	GameEvent::procGameEvent(GET_EnterJJC,params,1,player->getHandleId());

	player->save();
}

void
EndlessStair::calcRank(Entity* pStart, std::string passName, bool isWin)
{
	if(pStart->asPlayer() == NULL)
		return;
	if(passName.empty())
		return;
	if (isWin)
	{
		int index1 = findRankByName(pStart->getNameC());
		int index2 = findRankByName(passName);
		
		bool isSave = true;

		if(index1 == -1 || index2 == -1 || index1 <= index2)
			isSave = false;

		if(isSave){
			std::swap(nameRate_[index1-1],nameRate_[index2-1]);

			DBHandler::instance()->updateEndlessStair(index1,nameRate_[index1-1]);
			DBHandler::instance()->updateEndlessStair(index2,nameRate_[index2-1]);
		}
	}

	COM_JJCBattleMsg sStartMsg;
	sStartMsg.defier_	= pStart->getNameC();
	sStartMsg.bydefier_	= passName;
	sStartMsg.rank_		= findRankByName(pStart->getNameC());
	sStartMsg.curTime_	= WorldServ::instance()->curTime_;
	sStartMsg.isWin_	= isWin;
	pStart->asPlayer()->battleMsg(sStartMsg);
	CALL_CLIENT(pStart, myBattleMsgOK(sStartMsg));

	COM_EndlessStair info;
	info.rank_ = findRankByName(pStart->getNameC());
	info.name_ = pStart->getNameC();
	info.level_ = pStart->getProp(PT_Level);
	info.assetId_ = pStart->getProp(PT_AssetId);
	info.rivalTime_ = pStart->asPlayer()->rivalTimes_;
	info.rivalNum_ = pStart->asPlayer()->rivalNum_;

	CALL_CLIENT(pStart, requestMySelfJJCDataOK(info));

	GEParam params[2];
	params[0].type_ = GEP_INT;
	params[0].value_.i = info.rank_;
	params[1].type_ = GEP_INT;
	params[1].value_.i = isWin;
	GameEvent::procGameEvent(GET_PvR,params,2,pStart->getHandleId());

	Player* player = Player::getPlayerByName(passName);

	if (player != NULL)
	{
		COM_JJCBattleMsg sEndMsg;
		sEndMsg.defier_		= pStart->getNameC();
		sEndMsg.bydefier_	= passName;
		sEndMsg.rank_		= findRankByName(pStart->getNameC());
		sEndMsg.curTime_	= WorldServ::instance()->curTime_;
		sEndMsg.isWin_		= !isWin;

		player->battleMsg(sEndMsg);
		CALL_CLIENT(player,myBattleMsgOK(sEndMsg));
	}
}

void
EndlessStair::requestJJCData(Player* pPlayer)
{
	COM_EndlessStair info;
	info.rank_ = findRankByName(pPlayer->getNameC());
	info.name_ = pPlayer->getNameC();
	info.level_ = pPlayer->getProp(PT_Level);
	info.assetId_ = pPlayer->getProp(PT_AssetId);
	info.rivalTime_ = pPlayer->rivalTimes_;
	info.rivalNum_ = pPlayer->rivalNum_;

	CALL_CLIENT(pPlayer, requestMySelfJJCDataOK(info));
}

//-----------------
void
EndlessStair::requestRank(Player* pPlayer)
{
	U32 myRank = 0;
	std::vector<COM_EndlessStair> infos;
	for (size_t i = 0; i < nameRate_.size(); ++i)
	{
		if(i < 100)
		{
			COM_EndlessStair info;

			Robot* pRobot = findRobotByName(nameRate_[i]);
			if(pRobot)
			{
				info.rank_		= findRankByName(nameRate_[i]);
				info.name_		= nameRate_[i];
				info.job_		= pRobot->Job_;
				info.joblevel_	= pRobot->JobLevel_;
				info.level_		= pRobot->getProp(PT_Level);
				info.assetId_	= pRobot->getProp(PT_AssetId);

				infos.push_back(info);
			}
			else
			{
				COM_ContactInfo* pContactInfo = WorldServ::instance()->findContactInfo(nameRate_[i]);

				if(pContactInfo == NULL)
					continue;

				info.rank_ = findRankByName(nameRate_[i]);
				info.name_ = nameRate_[i];
				info.level_= pContactInfo->level_;
				info.assetId_ = pContactInfo->assetId_;
				info.job_  = pContactInfo->job_;
				info.joblevel_ = pContactInfo->jobLevel_;

				infos.push_back(info);
			}
		}

		if(strcmp(pPlayer->getNameC(),nameRate_[i].c_str()) == 0 && myRank == 0)
		{
			myRank = findRankByName(nameRate_[i]);
		}
	}

	CALL_CLIENT(pPlayer,requestJJCRankOK(myRank,infos));
}

void
EndlessStair::checkMsg(Player* pPlayer, std::string name)
{
	Robot* pRobot = findRobotByName(name);

	if (pRobot)
	{
		COM_SimplePlayerInst inst;
		pRobot->getRobotInst(inst);
		CALL_CLIENT(pPlayer,checkMsgOK(inst));
	}
	else
	{

	
		enum {
			W_JJC_PLAYER_INFO = 2
		};
		COM_ContactInfo* pCont = WorldServ::instance()->findContactInfo(name);

		if(pCont == NULL)
		{
			ACE_DEBUG((LM_ERROR,"Can not find in contact info %s\n",name.c_str()));
			return;
		}

	
		DBHandler::instance()->queryPlayerById(pPlayer->getNameC(),pCont->instId_,W_JJC_PLAYER_INFO);
	}
}

bool
EndlessStair::isFirstWin(U32 rank){
	for (size_t i = 0; i< firstwin_.size(); ++i)
	{
		if(firstwin_[i] == nameRate_[rank-1])
			return false;
	}
	return true;
}

void
EndlessStair::firstWinreward(std::string& sendName,std::string& title, std::string& content,U32 rank,U32 dropid){
	if(rank < 1)
		rank = 1;
	WorldServ::instance()->sendMailByDrop(sendName,nameRate_[rank-1],title,content,dropid);
	firstwin_.push_back(nameRate_[rank-1]);
}

void
EndlessStair::rankrewardbytimes(std::string& sendName,std::string& title, std::string& content,U32 rank)
{
	PvRrewardTable::PvrrewardData const* pCore = PvRrewardTable::getPvrRunkById(rank);
	if(pCore == NULL)
	{
		ACE_DEBUG((LM_ERROR, "PVR EndlessStair::rankrewardbytimes() PvRrewardTable can not find reward by rank[%d]\n",rank));
		return;
	}
	if(rank < 1)
		rank = 1;
	WorldServ::instance()->sendMailByDrop(sendName,nameRate_[rank-1],title,content,pCore->dropitem_);
}

void
EndlessStair::rankrewardbyday(std::string& sendName,std::string& title, std::string& content)
{
	for (size_t i = 0; i < nameRate_.size(); ++i)
	{
		Robot* pRobot = findRobotByName(nameRate_[i]);
		if(pRobot != NULL)
			continue;

		PvRrewardTable::PvrrewardData const* pCore = PvRrewardTable::getPvrRunkById(i+1);
		if(pCore == NULL)
		{
			ACE_DEBUG((LM_ERROR, "PVR EndlessStair::rankrewardbyday() PvRrewardTable can not find reward by rank[%d]\n",i+1));
			continue;
		}
		WorldServ::instance()->sendMailByDrop(sendName,nameRate_[i],title,content,pCore->dropDay_);
	}
}

void
EndlessStair::rankrewardbysenson(std::string& sendName,std::string& title, std::string& content)
{
	for (size_t i = 0; i < nameRate_.size(); ++i)
	{
		Robot* pRobot = findRobotByName(nameRate_[i]);
		if(pRobot != NULL)
			continue;

		PvRrewardTable::PvrrewardData const* pCore = PvRrewardTable::getPvrRunkById(i+1);
		if(pCore == NULL)
		{
			ACE_DEBUG((LM_ERROR, "PVR EndlessStair::rankrewardbyday() PvRrewardTable can not find reward by rank[%d]\n",i+1));
			continue;
		}
		WorldServ::instance()->sendMailByDrop(sendName,nameRate_[i],title,content,pCore->dropQuarter_);
	}
}

void
EndlessStair::deleteplayerReCalcRank(std::string playname)
{
	for (size_t i = 0; i < nameRate_.size(); ++i)
	{
		if(nameRate_[i] == playname)
		{
			nameRate_.erase(nameRate_.begin() + i);
			DBHandler::instance()->deleteEndlessStair(playname);
			break;
		}
	}
	for (size_t i = 0; i < nameRate_.size(); ++i)
	{
		DBHandler::instance()->updateEndlessStair(i + 1 ,nameRate_[i]);
	}
}