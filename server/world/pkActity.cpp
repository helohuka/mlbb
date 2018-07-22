#include "pkActity.h"
#include "Scene.h"
#include "scenetable.h"
#include "player.h"
#include "battle.h"
#include "team.h"
#include "battle.h"

void
Battle::create(Player* sendplayer,U32 playerId)
{
	SceneData* sd = SceneTable::getSceneById(sendplayer->sceneId_);
	if(!sd)
		return;
	if(sd->sceneType_ != SCT_AlonePK && sd->sceneType_ != SCT_TeamPK)
		return;
	Scene *s = SceneManager::instance()->getScene(sendplayer->sceneId_);
	if(s == NULL)
		return;
	Player* otherPlayer = s->getPlayerById(playerId);
	if(otherPlayer == NULL)
		return;
	/*if((!!sendplayer->myTeam()) != (!!(p->myTeam()))){
		return ;
	}*/
	BattleType bt;
	if(sd->sceneType_ == SCT_TeamPK)
		bt = BT_PK2;
	else
		bt = BT_PK1;

	if(sendplayer->myTeam() && otherPlayer->myTeam())
	{
		create(sendplayer->myTeam(),otherPlayer->myTeam(),bt);
	}
	else if (otherPlayer->myTeam() && sendplayer->myTeam() == NULL)
	{
		create(sendplayer,otherPlayer->myTeam(),bt);
	}
	else if (otherPlayer->myTeam() == NULL && sendplayer->myTeam())
	{
		create(otherPlayer,sendplayer->myTeam(),bt);
	}
	else
		create(sendplayer,otherPlayer,bt);
}

void
Battle::calcBattleRewardPK()
{
	//Battle* pbattle = player->myBattle();
	//if(pbattle == NULL)
	//	return;
	std::vector<COM_Item*> drops;
	for (size_t i = 0; i < entities_.size(); ++i)
	{
		Player* player = entities_[i]->asPlayer();
		if(player == NULL)
			continue;
		if(battleWinner_ == player->battleForce_)
			continue;
		U32 roll = UtlMath::randN(100);
		if(roll < Global::get<int>(C_PkItemDorp))
			continue;
		std::vector<COM_Item*> items;
		player->getItemByItemSubType(IST_PVP,items);
		if(items.empty())
			continue;
		U32 index = UtlMath::randN(items.size());
		drops.push_back(items[index]);
		player->delBagItemByInstId(items[index]->instId_,1);
	}

	std::vector<Player*> winers;
	for (size_t i = 0; i < entities_.size(); ++i)
	{
		Player* player = entities_[i]->asPlayer();
		if(player == NULL)
			continue;
		if(battleWinner_ != player->battleForce_)
			continue;
		winers.push_back(player);
	}
	random_shuffle(winers.begin(),winers.end());

	for (size_t i = 0; i < drops.size(); ++i)
	{
		U32 index = i;
		if(index > winers.size() -1)
			index = 0;
		if(winers.empty())
			continue;
		winers[index]->addBagItemByItemId(drops[i]->itemId_,1,false,9);
	}

	COM_BattleOverClearing boc ;
	BattleJudgeType bst;

	for (size_t i = 0; i < entities_.size(); ++i)
	{
		Player* player = entities_[i]->asPlayer();
		if(player == NULL)
			continue;

		if(battleWinner_ == GT_Down){
			bst = player->battleForce_ == GT_Down ? BJT_Win : BJT_Lose;
		}
		else if(battleWinner_ == GT_Up){
			bst = player->battleForce_ == GT_Up ? BJT_Win : BJT_Lose;
		}else 
			bst = BJT_Lose;
		boc.playLevel_ = player->getProp(PT_Level);
		boc.playFree_ = player->getProp(PT_Free);

		Baby* pbaby = player->getBattleBaby();
		if(pbaby)
			boc.babyLevel_ = pbaby->getProp(PT_Level);

		CALL_CLIENT(player,exitBattleOk(bst,boc));
	}
}

void
Battle::calcBattlePKFlee(Player* fleeplayer)
{
	if(this != fleeplayer->myBattle())
		return;
	
	std::vector<COM_Item*> drops;
	U32 roll = UtlMath::randN(100);
	if(roll < Global::get<int>(C_PkItemDorp))
		return;
	std::vector<COM_Item*> items;
	fleeplayer->getItemByItemSubType(IST_PVP,items);
	if(items.empty())
		return;
	U32 index = UtlMath::randN(items.size());
	drops.push_back(items[index]);
	fleeplayer->delBagItemByInstId(items[index]->instId_,1);

	std::vector<Player*> winers;
	for (size_t i = 0; i < entities_.size(); ++i)
	{
		Player* player = entities_[i]->asPlayer();
		if(player->battleForce_ == fleeplayer->battleForce_)
			continue;
		winers.push_back(player);
	}
	random_shuffle(winers.begin(),winers.end());

	for (size_t i = 0; i < drops.size(); ++i)
	{
		U32 index = i;
		if(index > winers.size() -1)
			index = 0;
		winers[index]->addBagItemByItemId(drops[i]->itemId_,1,false,9);
	}
}