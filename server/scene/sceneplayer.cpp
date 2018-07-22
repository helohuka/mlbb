#include "config.h"
#include "sceneplayer.h"
#include "scene.h"
#include "npctable.h"


ScenePlayer::Set ScenePlayer::sceneplayers_;

void ScenePlayer::UpdateScenePlayers(float dt){
	
	for(Set::iterator s = sceneplayers_.begin(),e=sceneplayers_.end(); s!=e; ++s){
		(*s)->Update(dt);
	}
}

ScenePlayer* ScenePlayer::GetScenePlayer(S32 playerId){
	for(Set::iterator s = sceneplayers_.begin(),e=sceneplayers_.end(); s!=e; ++s){
		if(((*s)->playerId_ == playerId))
			return (*s);
	}
	return NULL;
}

ScenePlayer::ScenePlayer()
:sceneId_(0)
,status_(Normal)
,entryId_(0)
,step_(0)
,entryFlag_(false)
,isFollow_(false){
	setProxy(this);	
	sceneplayers_.insert(this);
	openScenes_.push_back(SceneTable::getHomeScene()->sceneId_);
}

ScenePlayer::~ScenePlayer(){
	Scene* s = MyScene();
	if(s)
		s->ExitScene(this);
	sceneplayers_.erase(this);
}

bool ScenePlayer::handleClose(){
	delete this;
	return true;
}
Scene* ScenePlayer::MyScene()
{
	return SceneManager::instance()->FindScene(sceneId_);
}

bool ScenePlayer::Update(float dt){
	
	//CalcMovePosition(dt);
	CalcMove(dt);

	return true;
}

void ScenePlayer::PushBehavior(BehaviorNode bn){
	if(status_ != Normal)
		return;
	if(!behaviorRoad_.empty() && behaviorRoad_.back().type_ == bn.type_)
		return;
	behaviorRoad_.push_back(bn);
}

void ScenePlayer::CalcBehavior(){
	if(!behaviorRoad_.empty()){
		BehaviorNode ns = behaviorRoad_.back();
		if(!(ns.noPop_))
			behaviorRoad_.pop_back();
		ns.TraceDebug();
		if(GET_SCENE_ORIGINAL_ID(ns.sceneId_) != GET_SCENE_ORIGINAL_ID(sceneId_))
		{
			//ACE_DEBUG((LM_ERROR, "Calc behavior node scene not eq. %d, %d!=%d ,type=%d\n",playerId_,ns.sceneId_,sceneId_,ns.type_));
			return;
		}
		Scene* s = MyScene();
		if(!s)
			return;
		if(ns.type_ == BehaviorNode::T_Entry){
			s->FindPath2Entity(pos_,ns.value_,path_);
		}	
		else if(ns.type_ == BehaviorNode::T_Point){
			s->FindPath2Point(pos_,ns.posi_,path_);
		}
		else if(ns.type_ == BehaviorNode::T_Zone){
			s->FindPath2Zone(pos_,ns.value_,path_);
		}
		else if(ns.type_ == BehaviorNode::T_PathStopNpc){
			talkedNpc(ns.value_);
		} 
		else if(ns.type_ == BehaviorNode::T_RollPathInZone){
			autoBattleResult(true);
			s->RollPathInZone(pos_,ns.value_,path_);
		}
		else if(ns.type_ == BehaviorNode::T_TransforScene){
			EnterScene(ns.exten_,ns.value_);
		}
		else if(ns.type_ == BehaviorNode::T_TraceScene){
			if(GET_SCENE_ORIGINAL_ID(ns.value_) != GET_SCENE_ORIGINAL_ID(sceneId_))
				SceneManager::instance()->FindRoad(this,ns.value_,behaviorRoad_);
		}
		
	}
}

//计算行走路线
bool ScenePlayer::CalcMove(float dt){
	if(!IsNormalState())
		return true;
	if(IsInRange(pos_,targetPos_,0.1F)){
		if(path_.empty()){
			CalcBehavior();
			return true;
		}
		SetTargetPos(path_.back(),path_.size() == 1);
		CalcFollowsTargetPos(targetPos_,path_.size() == 1);
		path_.pop_back();
		
	}
	
	if(IsInRange(pos_,targetPos_,0.1F))
		return true;

	//方向
	float dx = targetPos_.x_ - pos_.x_;
	float dz = targetPos_.z_ - pos_.z_;

	float mod =  (float)UtlMath::sqrt(dx*dx+dz*dz);

	dx /= mod;
	dz /= mod;

	//附加 速度
	dt = dt * PLAYER_SPEED; ///每秒两米

	pos_.x_ += dx*dt;
	pos_.z_ += dz*dt;

	++step_;

	return true;
}

bool ScenePlayer::Move2Pos(COM_FPosition pos){
	if(Normal != status_){
		ACE_DEBUG((LM_ERROR,"State(%d) player %d is not move to pos\n",status_,playerId_));
		return true;
	}
	if(isFollow_){
		ACE_DEBUG((LM_ERROR,"Follow player %d is not move to pos\n",playerId_));
		return true;
	}
	CleanMove();
	if(sceneId_ == 0){
		return false; ///这个是在主城 不要乱走
	}
	
	Scene* s = MyScene();

	if(s == NULL){
		return false;
	}
	
	path_.clear();

	pos.x_ = -pos.x_;
	if(s->FindPath(pos_,pos,path_)){
		
		for(size_t i=1; i<path_.size(); ++i){
			float dx = path_[i-1].x_ - path_[i].x_;
			float dz = path_[i-1].z_ - path_[i-1].z_;
			float res = (dx*dx + dz*dz);
			res = UtlMath::sqrt(res);
			//  ACE_DEBUG((LM_INFO,"%d ----> %d lengh2 = %f\n",i-1,i,res)); 
		}
		return true;
	}

	return false;
}

bool ScenePlayer::Move2Npc(S32 npcId){

	if(Normal != status_){
		ACE_DEBUG((LM_ERROR,"State(%d) player %d is not move to npc %d\n",status_,playerId_,npcId));
		return false;
	}
	if(isFollow_){
		ACE_DEBUG((LM_ERROR,"Follow player %d is not move to npc %d\n",playerId_,npcId));
		return false;
	}
	const NpcTable::NpcData* npcd = NpcTable::getNpcById(npcId);
	if(!npcd)
		return false;
	if(npcd->sceneId_ == GET_SCENE_ORIGINAL_ID(sceneId_))
	{
		CleanMove();
		BehaviorNode ns(npcd->sceneId_);
		ns.SetPathStopNpc(npcId);
		behaviorRoad_.push_back(ns);
		ns.SetPoint(npcd->posi_);
		behaviorRoad_.push_back(ns);
	}
	else if(behaviorRoad_.empty())
	{
		CleanMove();
		BehaviorNode ns(npcd->sceneId_);
		ns.SetPathStopNpc(npcId);
		behaviorRoad_.push_back(ns);
		ns.SetPoint(npcd->posi_);
		behaviorRoad_.push_back(ns);
		if(!SceneManager::instance()->FindRoad(this,npcd->sceneId_,behaviorRoad_)){
			ACE_DEBUG((LM_INFO,"Can not find road\n"));
			return false;
		}
		
	}
	else
	{
		bool hasSceneId = false;
		for(size_t i=0;i<behaviorRoad_.size(); ++i)
		{
			if(behaviorRoad_[i].type_ == BehaviorNode::T_TransforScene)
			{
				if(behaviorRoad_[i].exten_ == npcd->sceneId_)
				{
					hasSceneId = true;
				}
			}
			else if(behaviorRoad_[i].type_ == BehaviorNode::T_Entry)
			{
				Scene*s = SceneManager::instance()->FindScene(behaviorRoad_[i].sceneId_);
				if(s)
				{
					SceneEntry* se = s->FindEntry(behaviorRoad_[i].value_);
					if(se && se->nextSceneId_ == npcd->sceneId_)
					{
						hasSceneId = true;
					}
				}
			}
			if(hasSceneId)
			{
				behaviorRoad_.erase(behaviorRoad_.begin(),behaviorRoad_.begin()+i+1);
				break;
			}
		}
		if(hasSceneId)
		{
			BehaviorNode ns(npcd->sceneId_);
			ns.SetPathStopNpc(npcId);
			behaviorRoad_.insert(behaviorRoad_.begin(),ns);
			ns.SetPoint(npcd->posi_);
			behaviorRoad_.insert(behaviorRoad_.begin(),ns);
		}
		else
		{
			CleanMove();
			BehaviorNode ns(npcd->sceneId_);
			ns.SetPathStopNpc(npcId);
			behaviorRoad_.push_back(ns);
			ns.SetPoint(npcd->posi_);
			behaviorRoad_.push_back(ns);
			if(!SceneManager::instance()->FindRoad(this,npcd->sceneId_,behaviorRoad_)){
				ACE_DEBUG((LM_INFO,"Can not find road\n"));
				return false;
			}
		}
	}

	if(!behaviorRoad_.empty()){
		BehaviorNode &bn = behaviorRoad_.back();
		if(bn.type_ == BehaviorNode::T_Entry){
			if(bn.value_ == entryId_){
				behaviorRoad_.pop_back();
				entryId_ = 0;
			}
		}
	}
	
	return true;
}

bool ScenePlayer::Move2Zone(S32 sceneId,S32 zoneId){
	if(Normal != status_){
		ACE_DEBUG((LM_ERROR,"State(%d) player %d is not move zone %d\n",status_,playerId_,zoneId));
		return false;
	}
	if(isFollow_){
		ACE_DEBUG((LM_ERROR,"Follow player %d is not move to zone %d\n",playerId_,zoneId));
		return false;
	}
	BehaviorNode ns(sceneId);
	ns.SetRollPathInZone(zoneId);
	behaviorRoad_.push_back(ns);
	ns.SetZone(zoneId);
	behaviorRoad_.push_back(ns);
	
	if(sceneId == sceneId_)
	{
		CleanMove();
		BehaviorNode ns(sceneId);
		ns.SetRollPathInZone(zoneId);
		behaviorRoad_.push_back(ns);
		ns.SetZone(zoneId);
		behaviorRoad_.push_back(ns);
	}
	else if(behaviorRoad_.empty())
	{
		CleanMove();
		BehaviorNode ns(sceneId);
		ns.SetRollPathInZone(zoneId);
		behaviorRoad_.push_back(ns);
		ns.SetZone(zoneId);
		behaviorRoad_.push_back(ns);
		if(!SceneManager::instance()->FindRoad(this,sceneId,behaviorRoad_)){
			ACE_DEBUG((LM_INFO,"Can not find road\n"));
			return false;
		}
	}
	else
	{
		bool hasSceneId = false;
		for(size_t i=0;i<behaviorRoad_.size(); ++i)
		{
			if(behaviorRoad_[i].type_ == BehaviorNode::T_TransforScene)
			{
				if(behaviorRoad_[i].exten_ == sceneId)
				{
					hasSceneId = true;
				}
			}
			else if(behaviorRoad_[i].type_ == BehaviorNode::T_Entry)
			{
				Scene*s = SceneManager::instance()->FindScene(behaviorRoad_[i].sceneId_);
				if(s)
				{
					SceneEntry* se = s->FindEntry(behaviorRoad_[i].value_);
					if(se && se->nextSceneId_ == sceneId)
					{
						hasSceneId = true;
					}
				}
			}
			if(hasSceneId)
			{
				behaviorRoad_.erase(behaviorRoad_.begin(),behaviorRoad_.begin()+i+1);
				break;
			}
		}
		if(hasSceneId)
		{
			BehaviorNode ns(sceneId);
			ns.SetRollPathInZone(zoneId);
			behaviorRoad_.insert(behaviorRoad_.begin(),ns);
			ns.SetZone(zoneId);
			behaviorRoad_.insert(behaviorRoad_.begin(),ns);
			
		}
		else
		{
			CleanMove();
			BehaviorNode ns(sceneId);
			ns.SetRollPathInZone(zoneId);
			behaviorRoad_.push_back(ns);
			ns.SetZone(zoneId);
			behaviorRoad_.push_back(ns);
			if(!SceneManager::instance()->FindRoad(this,sceneId,behaviorRoad_)){
				ACE_DEBUG((LM_INFO,"Can not find road\n"));
				return false;
			}
		}

	}

	if(!behaviorRoad_.empty()){
		BehaviorNode &bn = behaviorRoad_.back();
		if(bn.type_ == BehaviorNode::T_Entry){
			if(bn.value_ == entryId_){
				behaviorRoad_.pop_back();
				entryId_ = 0;
			}
		}
	}
	return true;
}

void ScenePlayer::SetTargetPos(COM_FPosition pos, bool isLast){
	targetPos_ = pos;
	pos.x_ = -pos.x_;
	pos.isLast_ = isLast;
	move2(pos);
}

void ScenePlayer::CalcFollowsTargetPos(COM_FPosition pos,bool isLast){
	if(isFollow_){
		return;
	}
	for(size_t i=0; i<follows_.size(); ++i){
		if(IsInRange(pos,follows_[i]->pos_,0.1F))continue;
		float dx = pos.x_ - follows_[i]->pos_.x_;
		float dz = pos.z_ - follows_[i]->pos_.z_;
		float mod =  (float)UtlMath::sqrt(dx*dx+dz*dz);
		pos.x_ -= dx/mod*PLAYER_LENGTH;
		pos.z_ -= dz/mod*PLAYER_LENGTH;
		follows_[i]->SetTargetPos(pos,isLast);
	}
}

void ScenePlayer::StopFollowsMove(){
	for(size_t i=0; i<follows_.size(); ++i){
		follows_[i]->CleanMove();
	}
}

void ScenePlayer::ToSceneFollows(S32 sceneId,S32 entryId){
	for(size_t i=0; i<follows_.size(); ++i){
		follows_[i]->CleanMove();
		follows_[i]->EnterScene(sceneId,entryId);
	}
}


void ScenePlayer::ToSceneFollows(S32 sceneId,COM_FPosition pos){
	for(size_t i=0; i<follows_.size(); ++i){
		follows_[i]->CleanMove();
		follows_[i]->EnterScene(sceneId,pos);
	}
}

void ScenePlayer::CleanMove(){
	path_.clear();
	behaviorRoad_.clear();
	targetPos_ = pos_;
	for(size_t i=0; i<follows_.size(); ++i){
		follows_[i]->CleanMove();
	}
}

void ScenePlayer::JoinBattle(S32 zoneId){
	zoneJoinBattle(zoneId);
}

void ScenePlayer::EnterScene(S32 sceneId, S32 entryId){
	if(sceneId == sceneId_){
		return;
		//
	}
	Scene* s = MyScene();
	if(s!=NULL)
		s->ExitScene(this);
	s = SceneManager::instance()->FindScene(sceneId);
	if(NULL == s)
		return;
	s->JoinScene(this,entryId);

	ToSceneFollows(sceneId,entryId);
}

void ScenePlayer::EnterScene(S32 sceneId, COM_FPosition pos){
	if(sceneId == sceneId_){
		pos_ = pos;
		pos_.x_ = -pos_.x_;
		transfor2(pos);
		CleanMove();
		return;
		//
	}
	Scene* s = MyScene();
	if(s!=NULL)
		s->ExitScene(this);
	s = SceneManager::instance()->FindScene(sceneId);
	if(NULL == s)
		return;
	s->JoinScene(this,pos);

	ToSceneFollows(sceneId,pos);
}


void ScenePlayer::JoinScene(S32 sceneId,S32 entryId,COM_FPosition pos, NpcList& staticNpcs, NpcList& dynamicNpcs,bool needLoadSceneStatus){
	
	if(needLoadSceneStatus && sceneId != sceneId_)
		status_ = LoadScene;

	path_.clear();
	entryId_ = entryId;
	COM_SceneInfo info;
	info.sceneId_ = sceneId_ = sceneId;
	info.position_= pos_ = pos;
	pos_.x_ = -pos_.x_; //草尼玛
	targetPos_ = pos_;
	step_ = 0;
	
	Scene* s = MyScene();
	if(!s)
	{
		ACE_DEBUG((LM_INFO,"Join scene %d scene %d\n",playerId_,sceneId_));
		return;
	}
	s->GetNpcs(this,info.npcs_);

	for(size_t i=0; i<privateNpcs_.size(); ++i){
		const NpcTable::NpcData* npc = NpcTable::getNpcById(privateNpcs_[i]);
		if(npc->sceneId_ != sceneId_)
			continue;
		if(npc->filterLevel_ >  playerLevel_) 
			continue;  //等级不够不能显示
		if(!npc->filterQuest_.empty() &&!IsInCurrentQuest(npc->filterQuest_)) 
			continue;
		info.npcs_.push_back(privateNpcs_[i]);
	}
	
	cacheNpcs_.clear();
	for(size_t i=0; i<info.npcs_.size(); ++i){
		cacheNpcs_.push_back(info.npcs_[i]);
	}
	joinScene(info);
	if(s->sceneType_ == SCT_Bairen)
		entryFlag_ = false;
}

void ScenePlayer::UpdateNpcs(){
	
	Scene* s = MyScene();
	if(!s)
	{
		ACE_DEBUG((LM_INFO,"Update npc find %d scene %d\n",playerId_,sceneId_));
		return;
	}
	
	NpcList delNpcs;
	NpcList addNpcs;
	NpcList sceneNpcs;
	s->GetNpcs(this,sceneNpcs);
	
	for(size_t i=0; i<privateNpcs_.size(); ++i){
		const NpcTable::NpcData* npc = NpcTable::getNpcById(privateNpcs_[i]);
		if(npc->sceneId_ != sceneId_)
			continue;
		if(npc->filterLevel_ >  playerLevel_) 
			continue;  //等级不够不能显示
		if(!npc->filterQuest_.empty() &&!IsInCurrentQuest(npc->filterQuest_)) 
			continue;
		sceneNpcs.push_back(privateNpcs_[i]);
	}
	//查找cache 有但是目前不能看见的NPC
	for(size_t i=0; i<cacheNpcs_.size(); ++i){
		bool needDel = true;
		for(size_t j=0; j<sceneNpcs.size(); ++j){
			if(cacheNpcs_[i] == sceneNpcs[j])
			{
				needDel = false;
				break;
			}
		}
		if(needDel){
			delNpcs.push_back(cacheNpcs_[i]);
		}
	}
	
	//查找cache 没有 但是能看见的NPC
	for(size_t i=0; i<sceneNpcs.size(); ++i){
		if(std::find(cacheNpcs_.begin(),cacheNpcs_.end(),sceneNpcs[i]) == cacheNpcs_.end()){
			cacheNpcs_.push_back(sceneNpcs[i]);
			addNpcs.push_back(sceneNpcs[i]);
		}
	}
	
	for(size_t i=0; i<delNpcs.size(); ++i){
		cacheNpcs_.erase(std::find(cacheNpcs_.begin(),cacheNpcs_.end(),delNpcs[i]));
	}

	if(!addNpcs.empty())
		playerAddNpc(addNpcs);
	if(!delNpcs.empty())
		playerDelNpc(delNpcs);
}

bool ScenePlayer::IsInCurrentQuest(const std::vector<S32>& ids){
	for(size_t i=0; i<ids.size(); ++i){
		for(size_t j=0; j<currentQuestIds_.size(); ++j){
			if(currentQuestIds_[j] == ids[i]){
				return true;				
			}
		}
	}
	return false;
}

bool ScenePlayer::CanTransforToScene(S32 sceneId){
	if(SceneTable::getHomeScene()->sceneId_ == sceneId)
		return true;
	if(SceneTable::getGuildHomeScene()->sceneId_ == sceneId)
		return true;
	for(size_t i=0;i<openScenes_.size();++i)
		if(openScenes_[i] == sceneId)
			return true;
	return false;
}

bool ScenePlayer::HasNpc(int32 npcId){
	return FindNpc(npcId) != NULL;
}

int32 ScenePlayer::FindNpc(int32 npcId){
	for(size_t i=0; i<cacheNpcs_.size(); ++i)
	{
		if(cacheNpcs_[i] == npcId)
		{
			Scene* s = MyScene();
			if(!s)
			{
				ACE_DEBUG((LM_INFO,"Find npc %d scene %d %d\n",playerId_,sceneId_,npcId));
				return NULL;
			}
			return s->FindNpc(npcId);
		}
	}
	return NULL;
}

bool ScenePlayer::initScenePlayer(SGE_ScenePlayerInfo& info){
	playerId_ = info.playerId_;
	playerLevel_ = info.playerLevel_;
	currentQuestIds_ = info.currentQuestIds_;
	acceptableQuestIds_ = info.accecptQuestIds_;
	openScenes_ = info.openScenes_;
	Scene* s = MyScene();
	if(s!=NULL)
		s->ExitScene(this);
	s = SceneManager::instance()->FindScene(info.sceneId_);
	if(NULL == s)
		return true;
	s->JoinScene(this,info.entryId_);

	return true;
}
bool ScenePlayer::scenePlayerUpLevel(S32 level){
	playerLevel_ = level;
	UpdateNpcs();
	return true;
}
bool ScenePlayer::scenePlayerAddCurrentQuest(S32 questId){
	for (size_t i=0; i<currentQuestIds_.size(); ++i)
	{
		if(currentQuestIds_[i] == questId)
			return true;
	}
	currentQuestIds_.push_back(questId);
	UpdateNpcs();
	return true;
}
bool ScenePlayer::scenePlayerDelCurrentQuest(S32 questId){
	for (size_t i=0; i<currentQuestIds_.size(); ++i)
	{
		if(currentQuestIds_[i] == questId)
		{
			currentQuestIds_.erase(currentQuestIds_.begin() + i);
			UpdateNpcs();
			return true;
		}
	}
	return true;
}

bool ScenePlayer::scenePlayerAddAcceptableQuest(S32 questId){
return true;
}
bool ScenePlayer::scenePlayerDelAcceptableQuest(S32 questId){
return true;
}

bool ScenePlayer::openScene(S32 sceneId){
	if(CanTransforToScene(sceneId))
		return true;
	openScenes_.push_back(sceneId);
	return true;
}

bool ScenePlayer::joinBattle(){
	step_ = 0; //清理战斗步数
	status_ = Battle;
	for(size_t i=0; i<follows_.size(); ++i){
		follows_[i]->joinBattle();
	}
	return true;
}

bool ScenePlayer::finishBattle()
{
	//isBattle_ = false;
	status_ = LoadScene;
	for(size_t i=0; i<follows_.size(); ++i){
		follows_[i]->finishBattle();
	}
	return true;
}

bool ScenePlayer::transforScene(S32 sceneId){
	if(sceneId_ == sceneId)
		return true;
	CleanMove();
	Scene* s = MyScene();
	if(NULL == s)
		return true;
	s->ExitScene(this);
	for(size_t i=0;i<follows_.size();++i){
		follows_[i]->CleanMove();
		s->ExitScene(follows_[i]);

	}
	s = SceneManager::instance()->FindScene(sceneId);
	if(!s)
	{
		ACE_DEBUG((LM_INFO,"Transfor scene find %d scene %d\n",playerId_,sceneId));
		return true;
	}
	s->JoinScene(this,0);
	
	for(size_t i=0;i<follows_.size();++i){
		follows_[i]->CleanMove();
		s->JoinScene(follows_[i],0);
	}

	return true;
}

bool ScenePlayer::transforSceneByEntry(int32 sceneId,int32 entryId){
	if(sceneId_ == sceneId)
		return true;
	CleanMove();
	Scene* s = MyScene();
	if(NULL == s)
		return true;
	s->ExitScene(this);
	for(size_t i=0;i<follows_.size();++i){
		follows_[i]->CleanMove();
		s->ExitScene(follows_[i]);

	}
	s = SceneManager::instance()->FindScene(sceneId);
	if(!s)
	{
		ACE_DEBUG((LM_INFO,"Transfor scene find %d scene %d\n",playerId_,sceneId));
		return true;
	}
	
	SceneEntry* e = s->FindEntry(entryId);
	if(!e)
	{
		ACE_DEBUG((LM_INFO,"Transfor scene find %d scene %d entry %d\n",playerId_,sceneId,entryId));
		return true;
	}

	s->JoinScene(this,entryId);

	for(size_t i=0;i<follows_.size();++i){
		follows_[i]->CleanMove();
		s->JoinScene(follows_[i],entryId);
	}

	return true;
}

bool ScenePlayer::backHomeScene(){
	CleanMove();
	Scene* s = MyScene();
	if(s)s->ExitScene(this);
	status_ = Normal;
	s = SceneManager::instance()->FindMainScene();
	s->players_.insert (this);
	sceneId_ = s->sceneId_;
	return true;
}

bool ScenePlayer::sceneLoaded(){
	//SRV_ASSERT(status_ != Battle);
	if(status_ == Battle)
	{
		ACE_DEBUG((LM_ERROR,"This state is battle donot change to Normal!!!!!!!!!!\n"));
		return true;
	}
	status_ = Normal;
	for(size_t i=0; i<follows_.size(); ++i){
		follows_[i]->sceneLoaded();
	}
	return true;
}

bool ScenePlayer::move(COM_FPosition& pos){
	if(!Move2Pos(pos)){
		cantMove();
	}
	return true;
}

bool ScenePlayer::moveToNpc(S32 npcid){
	if(!Move2Npc(npcid)){
		cantMove();
	}
	return true;
}

bool ScenePlayer::moveToNpc2(NpcType type){
	/*if(type != NT_Mogu && type != NT_Xiji)
		return true;*/
	S32 npcId = SceneManager::instance()->GetDynamicNpc(type);
	if(!npcId)
	{
		for (size_t i=0; i<privateNpcs_.size(); ++i)
		{
			const NpcTable::NpcData* npc = NpcTable::getNpcById(privateNpcs_[i]);
			if(npc->npcType_ == type)
				npcId = npc->npcId_;
		}
	}
	if(!!npcId) {
		if(!Move2Npc(npcId))
		{
			cantMove();
		}
	}
	return true;
}

bool ScenePlayer::moveToZone(S32 sceneId, S32 zoneId){
	if(!Move2Zone(sceneId,zoneId)){
		cantMove();
	}
	return true;
}

bool ScenePlayer::autoBattle(){
	CleanMove();
	Scene* s = MyScene();
	if(!s)
	{
		ACE_DEBUG((LM_INFO,"Auto battle can not find %d scene %d\n",playerId_,sceneId_));
		return true;
	}
	SceneZone* sz = s->StandZone(this);
	if(NULL == sz){
		autoBattleResult(false);
	}
	else {
		autoBattleResult(true);
		BehaviorNode bn(sceneId_);
		bn.SetRollPathInZone(sz->zoneId_);
		behaviorRoad_.push_back(bn);
	}
	return true;
}

bool ScenePlayer::stopMove(){
	//ACE_DEBUG((LM_INFO,"ScenePlayer::stopMove \n"));
	CleanMove();
	return true;
}

bool ScenePlayer::addFollow(S32 scenePlayerId){
	ScenePlayer* p = ScenePlayer::GetScenePlayer(scenePlayerId);
	if(!p)
	{
		ACE_DEBUG((LM_INFO,"Add follow can not find %d sceneplayer %d\n",playerId_,scenePlayerId));
		return true;
	}
	for(size_t i=0; i<follows_.size(); ++i){
		if(follows_[i] == p)
		{
			ACE_DEBUG((LM_ERROR, "Add follow two times %d %d\n",playerId_,scenePlayerId));
			//SRV_ASSERT(0);
			return true;
		}
	}
	p->CleanMove();
	p->isFollow_ = true;
	COM_FPosition pos = pos_;
	pos.x_ = -pos.x_;
	S32 oldScene = p->sceneId_;
	p->EnterScene(sceneId_,pos); ///直接传送过去
	if(oldScene == sceneId_){
		p->status_ = Normal;
	}
	follows_.push_back(p);
	return true;
}

bool ScenePlayer::delFollow(S32 scenePlayerId){
	for(size_t i=0; i<follows_.size(); ++i){
		if(follows_[i]->playerId_ == scenePlayerId){
			follows_[i]->CleanMove();
			follows_[i]->isFollow_ = false;
			follows_.erase(follows_.begin() + i);
			return true;
		}
	}
	//SRV_ASSERT(0);
	return true;
}

bool ScenePlayer::addFollows(std::vector<S32>& scenePlayers){
	for (size_t i=0; i<scenePlayers.size(); ++i){
		addFollow(scenePlayers[i]);
	}
	return true;
}

bool ScenePlayer::delFollows(){
	for(size_t i=0; i<follows_.size(); ++i){
		follows_[i]->CleanMove();
		follows_[i]->isFollow_ = false;
	}
	follows_.clear();
	return true;
}

bool ScenePlayer::setEntryFlag(S32 scenePlayerId, bool isFlag){
	ScenePlayer* p = ScenePlayer::GetScenePlayer(scenePlayerId);
	if(!p)
	{
		ACE_DEBUG((LM_INFO,"Set entry flag can not find %d sceneplayer %d\n",playerId_,scenePlayerId));
		return true;
	}
	p->entryFlag_ = isFlag;
	return true;
}

bool ScenePlayer::addNpc(int32 npcid){
	const NpcTable::NpcData * npcd = NpcTable::getNpcById(npcid);
	if(!npcd)
	{	
		ACE_DEBUG((LM_ERROR, "Can not find npc %d in bool ScenePlayer::addNpc(int32 npcid){\n",npcid));
		return true;
	}
	for(size_t i=0; i<privateNpcs_.size(); ++i){
		if(privateNpcs_[i] == npcid){
			return true;
		}
	}
	Scene* scene = SceneManager::instance()->FindScene(npcd->sceneId_);
	if(!scene){
		ACE_DEBUG((LM_ERROR, "Can not find scene %d in bool ScenePlayer::addNpc(int32 npcid){\n",npcd->sceneId_));
		return true;
	}

	privateNpcs_.push_back(npcid);

	UpdateNpcs();
	return true;
}

bool ScenePlayer::delNpc(int32 npcid){
	for(size_t i=0; i<privateNpcs_.size(); ++i){
		if(privateNpcs_[i] == npcid){
			privateNpcs_.erase(privateNpcs_.begin() + i);
			break; //return true;
		}
	}
	UpdateNpcs();
	return true;
}

bool ScenePlayer::findDynamicNpc(S32 npcId)
{
	const NpcTable::NpcData * npcd = NpcTable::getNpcById(npcId);
	if(!npcd)
	{	
		ACE_DEBUG((LM_ERROR, "Can not find npc %d in bool ScenePlayer::findDynamicNpc(int32 npcid){\n",npcId));
		return true;
	}
	Scene* p = MyScene();
	if(p == NULL)
		return true;
	findDynamicNpcOK(npcId,!!p->FindDynamicNpc(npcId) );
	return true;
}