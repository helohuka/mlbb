#include "config.h"
#include "battle.h"
#include "player.h"
#include "itemtable.h"
#include "sttable.h"

void 
Battle::insertState(S8 pos, U32 stId, U32 isAction)
{
	Entity *p = findEntityByPos(pos);
	if(NULL == p)
	{
		//ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not find entity pos = %d at Battle::insertState(S8 pos, U32 stId)\n"),pos));
		return ;
	}

	StateTable::Core const * pCore = StateTable::getStateById(stId);
	if(NULL == pCore)
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("Can not instert state by id = %d, entity = %d\n"),stId,p->getGUID()));
		return;
	}

	COM_ReportState st;
	st.ownerId_ = p->getGUID();
	st.stateId_ = stId;
	st.tick_ = pCore->tick_;
	st.turn_ = pCore->turn_;
	st.add_ = 1;
	if(activeRound_)
	{
		if (isAction > 0)
		{
			if (!getCurrentAction()->counters_.empty())
			{
				getCurrentAction()->counters_.back().states_.push_back(st);
			}
			else
			{
				ACE_DEBUG((LM_ERROR,ACE_TEXT("insertState getCurrentReportAction()->counters_.empty() ")));
			}
		}
		else
		{
			COM_ReportAction* pa = getCurrentAction() ;
			if(pa!= NULL)
			{
				
				S32 sumQueue = 0;
				for(size_t i=0; i<pa->targets_.size(); ++i)
				{
					if(pa->targets_[i].position_ == pos)
					{
						++sumQueue;
					}
				}
				st.addQueue_ = sumQueue;
				if(0 == sumQueue){
					if(getCurrentOrder() && getCurrentOrder()->casterId_ == p->getGUID()){
						st.addQueue_ = 1;
					}
				}
				pa->stateIds_.push_back(st);
			}
		}
	}
	else
	{
		roundReport_.stateIds_.push_back(st);
	}
	p->insertState(stId);
}

bool 
Battle::checkState(S8 pos, U32 type)
{
	Entity *p = findEntityByPos(pos);
	if(NULL == p)
	{
		//ACE_DEBUG((LM_DEBUG,ACE_TEXT("Can not find entity pos = %d at Battle::checkState(S8 pos, U32 stId)\n"),pos));
		return false;
	}

	return p->checkState(type);
}

void 
Battle::removeState(S8 pos, U32 stType)
{
	Entity *p = findEntityByPos(pos);
	if(NULL == p)
	{
		ACE_DEBUG((LM_DEBUG,ACE_TEXT("Can not find entity pos = %d at Battle::removeState(S8 pos, U32 stId)\n"),pos));
		return ;
	}
	
	for (size_t i=0; i<p->states_.size(); ++i)
	{
		if(p->states_[i].type_ == stType)
		{
			COM_ReportState st;
			st.ownerId_ = p->getGUID();
			st.stateId_ = p->states_[i].stateId_;
			st.add_ = 0;
			if(activeRound_)
			{
				getCurrentAction()->stateIds_.push_back(st);
			}
			else
			{
				roundReport_.stateIds_.push_back(st);
			}

			p->states_.erase(p->states_.begin() + i);
		}
	}
}

void 
Battle::clearState(S8 pos)
{
	Entity *p = findEntityByPos(pos);
	if(NULL == p)
	{
		ACE_DEBUG((LM_DEBUG,ACE_TEXT("Can not find entity pos = %d at Battle::clearState(S8 pos, U32 stId)\n"),pos));
		return ;
	}
	p->clearState();
}

void 
Battle::deleteStateByTrun(){
	for(size_t i=0; i<entities_.size(); ++i)
	{
		typedef std::vector<COM_State> st_array;
		st_array & tmp = entities_[i]->states_;
		for(size_t k=0; k<tmp.size();++k)
		{
			const StateTable::Core* st = StateTable::getStateById(tmp[k].stateId_);
			if(!st->battleDelete_)
				continue;
			
			if(tmp[k].turn_ <=0)
			{
				COM_ReportState st;
				st.ownerId_ = entities_[i]->getGUID();
				st.stateId_ = tmp[k].stateId_;
				st.add_ = 0;
				roundReport_.stateIds_.push_back(st);

				tmp.erase(tmp.begin() + k--);
			}
		}
	}
}

void
Battle::lookupStateByTrun()
{
	for(size_t i=0; i<entities_.size(); ++i)
	{
		typedef std::vector<COM_State> st_array;
		st_array & tmp = entities_[i]->states_;
		for(size_t k=0; k<tmp.size();++k)
		{
			const StateTable::Core* st = StateTable::getStateById(tmp[k].stateId_);
			if(!st->battleDelete_)
				continue;
			tmp[k].turn_--;
		}
	}
}

COM_State* 
Battle::findState(StateType type, BattlePosition pos)
{
	Entity* pEntity = findEntityByPos(pos);

	if(!pEntity)
		return NULL;

	for (size_t i = 0; i < pEntity->states_.size(); ++i)
	{
		if (pEntity->states_[i].type_ == type)
		{
			return &pEntity->states_[i];
		}
	}

	return NULL;
}

U32
Battle::getStateLevel(StateType type, BattlePosition pos)
{
	COM_State* pState = findState(type, pos);

	if (!pState)
		return 0;

	StateTable::Core const * pCore = StateTable::getStateById(pState->stateId_);

	if(pCore == NULL)
		return 0;

	return pCore->level_;
}

void
Battle::cuttickState(StateType type, BattlePosition pos, U32 curNum)
{
	Entity* pEntity = findEntityByPos(pos);

	if(!pEntity)
		return;

	for (size_t i = 0; i < pEntity->states_.size(); ++i)
	{
		if (pEntity->states_[i].type_ == type)
		{
			pEntity->states_[i].tick_ -= curNum;

			if (pEntity->states_[i].tick_ <= 0)
			{
				pEntity->removeState(pEntity->states_[i].stateId_);
				return;
			}
		}
	}
}

void 
Battle::postState()
{///这个滞后检测还是有问题
 ///在2动时有不是dont care 的技能还有有几率出现直接结算buff
 ///e.g. 1动施放普通攻击 2动施放恢复魔法 就会出现直接结算情况
 ///目前没有好的处理方案 
	for (size_t i=0; i<entities_.size(); ++i)
	{
		if(NULL == findOrder(entities_[i]->getGUID(),false))
		{
			entities_[i]->updateState(posTable_);
		}
	}
}

