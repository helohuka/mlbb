#include "config.h"

#include "ComGlobal.h"
#include "scene.h"
#include "TokenParser.h"
#include "CSVParser.h"
#include "tinyxml/tinyxml.h"
#include "ObjFile.h"
#include "DetourCommon.h"
#include "Recast.h"
#include "DetourNavMesh.h"
#include "DetourNavMeshQuery.h"
#include "DetourCrowd.h"
#include "InputGeom.h"
#include "Recast.h"
#include "RecastDump.h"
#include "DetourNavMesh.h"
#include "DetourNavMeshBuilder.h"
#include "sceneplayer.h"
#include "npctable.h"
#include "worldhandler.h"
#include "NavMesh.h"
bool SceneArea::isIn(ScenePlayer* sp){
	COM_FPosition pos = sp->pos_;
	pos.x_ = -pos.x_;
	return IsInRange(center_,pos,radius_);
}

Scene::Scene()
:BINChannelBroadcaster<SGE_Player_Scene2WorldStub,ScenePlayer> (WorldHandler::instance())
,traceSceneLock_(false){

}
Scene::~Scene(){
	
}

bool Scene::Init(SceneData* data){
	
	ACE_DEBUG((LM_INFO,"Init scene %d zones %d entrys %d\n",data->sceneId_,data->zones_.size(),data->entrys_.size()));
	sceneId_ = data->sceneId_;
	sceneType_ = data->sceneType_;
	sceneName_ = data->sceneName_;
	

	for(size_t i=0; i<data->zones_.size(); ++i){
		if (data->zones_[i])
		{
			SceneZone * sz = new SceneZone();
			sz->center_.x_ = data->zones_[i]->zoneCenterX_;
			sz->center_.z_ = data->zones_[i]->zoneCenterZ_;
			sz->radius_ = data->zones_[i]->zoneRadius_;
			sz->zoneId_ = data->zones_[i]->zoneId_;
			sz->zoneProb_ = data->zones_[i]->prob_;
			zones_.push_back(sz);
		}
	}

	for (size_t i=0; i<data->entrys_.size(); ++i)
	{
		if(data->entrys_[i]){
			SceneEntry* se = new SceneEntry();
			se->center_.x_ = data->entrys_[i]->entryCenterX_;
			se->center_.z_ = data->entrys_[i]->entryCenterZ_;
			se->radius_ = data->entrys_[i]->entryRadius_;

			se->isBornPos_ = data->entrys_[i]->isBornPos_;
			se->nextSceneId_ = data->entrys_[i]->toSceneId_;
			se->nextEntryId_ = data->entrys_[i]->toEntryId_;
			se->entryId_ = data->entrys_[i]->entryId_;

			entrys_.push_back(se);
		}
	}

	for (size_t i=0; i<data->funcpinfo_.size(); ++i){
		if(data->funcpinfo_[i]){
			ScenePoint* sp = new ScenePoint();
			sp->center_.x_ = data->funcpinfo_[i]->x;
			sp->center_.z_ = data->funcpinfo_[i]->z;
			sp->py_ = data->funcpinfo_[i]->y;
			sp->rx_ = data->funcpinfo_[i]->rx;
			sp->ry_ = data->funcpinfo_[i]->ry;
			sp->rz_ = data->funcpinfo_[i]->rz;
			sp->rw_ = data->funcpinfo_[i]->rw;

			sp->pointId_ = data->funcpinfo_[i]->id_;
			sp->type_ = data->funcpinfo_[i]->fpt_;
			points_.push_back(sp);
		}
	}

	if(data->navFileName_.empty()){
		return false; //没有
	}

	navMesh_ = new SceneMesh(data->navFileName_);
	
	BuildNormalNpcs();

	return true;
}

void Scene::BuildNormalNpcs(){
	std::vector<const NpcTable::NpcData*> npcs;
	NpcTable::getNpcs(NT_Normal,sceneId_,npcs);
	for(size_t i=0; i<npcs.size(); ++i){
			staticNpcs_.push_back(npcs[i]->npcId_);
	}
}

bool Scene::CalcZone(ScenePlayer* sp) //计算是否在zone
{
	if(sp->isFollow_)
		return true;
	if(!sp->IsNormalState())
		return true;
	for(size_t i=0; i<zones_.size(); ++i){
		if(zones_[i]->isIn(sp)){
			//在zone里
			if(sp->step_ < Global::get<int>(C_RandomMin) ){
				//undo anything
			}
			else if( (sp->step_ >= Global::get<int>(C_RandomMax)) || (UtlMath::randN(100) <= zones_[i]->zoneProb_) ){
				sp->JoinBattle(zones_[i]->zoneId_);
				return true;
			}
		}
	}
	return false;
}
bool Scene::CalcEntry(ScenePlayer* sp) //计算是否在entry
{
	if(!sp){
		ACE_DEBUG((LM_ERROR,"ScenePlayer is invalid  \n"));
		return true;
	}
	Scene* s = sp->MyScene();
	if(!s){
		ACE_DEBUG((LM_ERROR,"Scene can not find %d \n",sp->sceneId_));
		return true;
	}
	if(s->sceneType_ == SCT_Bairen && !sp->entryFlag_)
		return true;
	if(sp->isFollow_)
		return true;
	if(sp->entryId_ != 0){
		for (size_t i=0; i<entrys_.size(); ++i)
		{
			if(entrys_[i]->entryId_ == sp->entryId_ && !entrys_[i]->isIn(sp)){
				sp->entryId_ = 0;
				break;
			}
		}
	}
	
	for(size_t i=0; i<entrys_.size(); ++i){
		if(entrys_[i]->isIn(sp)){
			if(sp->entryId_ == entrys_[i]->entryId_){
				return false;
			}
			else{
				if(!(entrys_[i]->nextSceneId_) || !(entrys_[i]->nextEntryId_))
					return true;
				BehaviorNode bn(sp->sceneId_);
				bn.SetTransforScene(entrys_[i]->nextEntryId_,entrys_[i]->nextSceneId_);
				sp->PushBehavior(bn);
				return true;
			}
		}
	}
	return false;
}

void Scene::Update(float dt){
	for(ScenePlayerSet::iterator itr = players_.begin(); itr!=players_.end();){
		if(CalcZone(*itr)){
			//进入战斗
			//ACE_DEBUG((LM_INFO,"Join battle\n"));
			++itr;
		}
		else if(CalcEntry(*itr++)){
			//进入entry
			if(itr==players_.end())
				break;
			//ACE_DEBUG((LM_INFO,"Transfor scene\n"));
		}
		else{
			itr;
			// 什么也碰不到
		}
	}
}


bool Scene::Fini(){
	
	for (size_t i=0; i<zones_.size(); ++i)
	{
		if(zones_[i])
			delete zones_[i];
	}

	for (size_t i=0; i<entrys_.size(); ++i)
	{
		if(entrys_[i])
			delete entrys_[i];
	}

	for (size_t i=0; i<points_.size(); ++i)
	{
		if(points_[i])
			delete points_[i];
	}

	if(navMesh_)
		delete navMesh_;

	return true;
}

bool Scene::JoinScene(ScenePlayer* sp, S32 entryId){
	
	SceneEntry* se = NULL;
	if(0 == entryId){
		se = FindBorn();
	}
	else {
		se = FindEntry(entryId);
	}
	if(!se)
	{
		ACE_DEBUG((LM_INFO,"Can not find entry %d %d %d\n",sp->playerId_,sp->sceneId_,entryId));
		return true;
	}
	
	
	COM_FPosition fp = se->center_;
	if(se->isBornPos_){
		const float _2PI = 2*3.1415926F;
		float theta = UtlMath::frandNM(0,_2PI);
		float radius = UtlMath::frandNM(0,se->radius_) - 0.01F;
		fp.x_ += UtlMath::cos(theta) * radius;
		fp.z_ += UtlMath::sin(theta) * radius;
	}
	players_.insert(sp);
	sp->JoinScene(sceneId_,se->entryId_,fp,staticNpcs_,dynamicNpcs_,true);
	addChannel(sp);
	return true;
}

bool Scene::JoinScene(ScenePlayer* sp, COM_FPosition pos){
	players_.insert(sp);
	sp->JoinScene(sceneId_,0,pos,staticNpcs_,dynamicNpcs_,true);
	return true;
}

bool Scene::ExitScene(ScenePlayer* sp){
	if(sp->sceneId_ != sceneId_ )
		return true;
	if(players_.find(sp) == players_.end())
		return true;
	players_.erase(sp);
	sp->sceneId_ = 0;
	removeChannel(sp);
	return true;
}


SceneZone* Scene::StandZone(ScenePlayer*sp){
	for(size_t i=0; i<zones_.size(); ++i)
		if(zones_[i]->isIn(sp))
			return zones_[i];
	return NULL;
}



bool Scene::RollPathInZone(COM_FPosition start, S32 zoneId,std::vector<COM_FPosition>& outpath){
	SceneZone* sz = FindZone(zoneId);
	if(sz){
		for(int i=0; i<3; ++i){
			std::vector<COM_FPosition> path;
			if(navMesh_->RollPathInRound(start,sz->center_,sz->radius_,path))
			{
				outpath = path;
				return true;
			}
		}
	}
	return false;
}

bool Scene::FindPath2Point(COM_FPosition start, COM_FPosition endpos,std::vector<COM_FPosition>& outpath){
	return FindPath(start,endpos,outpath,PLAYER_LENGTH);
}

bool Scene::FindPath2Entity(COM_FPosition start, S32 entryId,std::vector<COM_FPosition>& outpath){
	SceneEntry *end = FindEntry(entryId);
	if(!end)
		return false;
	COM_FPosition pos = end->center_;
	pos.x_ = -pos.x_;
	return FindPath(start,pos,outpath);
}

bool Scene::FindPath2Zone(COM_FPosition start, S32 zoneId,std::vector<COM_FPosition>& path){
	SceneArea* as = FindZone(zoneId);
	if(!as)
		return false;
	COM_FPosition end = as->center_;
	end.x_ = -end.x_;

	if(FindPath(start,end,path)){
		if(!path.empty())
			path.erase(path.begin());
		return true;
	}
	return false;
}


int32 Scene::FindStaticNpc(S32 npcid){
	for (size_t i=0; i<staticNpcs_.size(); ++i)
	{
		if(staticNpcs_[i] == npcid)
			return staticNpcs_[i];
	}
	return 0;
}

int32 Scene::FindDynamicNpc(S32 npcid){
	for (size_t i=0; i<dynamicNpcs_.size(); ++i)
	{
		if(dynamicNpcs_[i] == npcid)
			return dynamicNpcs_[i];
	}
	return 0;
}

int32 Scene::GetOneDynamicNpc(NpcType nt){
	for(size_t i=0; i<dynamicNpcs_.size(); ++i){
		const NpcTable::NpcData* npcd = NpcTable::getNpcById(dynamicNpcs_[i]);
		SRV_ASSERT(npcd);
		if(npcd->npcType_ == nt)
			return dynamicNpcs_[i];
	}
	return NULL;
}

int32 Scene::FindNpc(S32 npcid){
	int32 npc = FindStaticNpc(npcid);
	if(npc)
		return npc;
	return  FindDynamicNpc(npcid);
}

void Scene::GetNpcs(ScenePlayer* player,std::vector<int32>& npcs){
	SRV_ASSERT(sceneId_ == player->sceneId_);
	for(size_t i=0; i<staticNpcs_.size(); ++i){
		const NpcTable::NpcData* npc = NpcTable::getNpcById(staticNpcs_[i]);
		SRV_ASSERT(npc->sceneId_ == GET_SCENE_ORIGINAL_ID(sceneId_));
		if(npc->filterLevel_ >  player->playerLevel_) 
			continue;  //等级不够不能显示
		if( !npc->filterQuest_.empty() && !player->IsInCurrentQuest(npc->filterQuest_)) 
			continue;
		npcs.push_back(staticNpcs_[i]);
	}
	
	for(size_t i=0; i<dynamicNpcs_.size(); ++i){
		const NpcTable::NpcData* npc = NpcTable::getNpcById(dynamicNpcs_[i]);
		SRV_ASSERT(npc->sceneId_ == GET_SCENE_ORIGINAL_ID(sceneId_));
		if(npc->filterLevel_ >  player->playerLevel_) 
			continue;  //等级不够不能显示
		if(!npc->filterQuest_.empty() &&!player->IsInCurrentQuest(npc->filterQuest_)) 
			continue;
		npcs.push_back(dynamicNpcs_[i]);
	}


}

void Scene::AddDynamicNpcs(std::vector<S32> npcs){
	for(size_t i=0; i<npcs.size(); ++i){
		const NpcTable::NpcData *npcd = NpcTable::getNpcById(npcs[i]);
		SRV_ASSERT(npcd);
		SRV_ASSERT(npcd->sceneId_ == GET_SCENE_ORIGINAL_ID(sceneId_));

		dynamicNpcs_.push_back(npcd->npcId_);
	}

	for(ScenePlayerSet::iterator i = players_.begin(), e = players_.end(); i!=e; ++i ){
		(*i)->UpdateNpcs();
	}
}
void Scene::DelDynamicNpc(S32 npcid){
	for(size_t i=0; i<dynamicNpcs_.size(); ++i){
		if(dynamicNpcs_[i] == npcid){
			dynamicNpcs_.erase(dynamicNpcs_.begin() + i);
			for(ScenePlayerSet::iterator i = players_.begin(), e = players_.end(); i!=e; ++i ){
				(*i)->UpdateNpcs();
			}
			return;
		}
	}
}

void Scene::ClearDynamicNpcs(NpcType type){
	bool needSync = false;
	for(size_t i=0; i<dynamicNpcs_.size(); ++i){
		const NpcTable::NpcData *npcd = NpcTable::getNpcById(dynamicNpcs_[i]);
		SRV_ASSERT(npcd);
		SRV_ASSERT(npcd->sceneId_ == GET_SCENE_ORIGINAL_ID(sceneId_));
		if(type == npcd->npcType_){
			needSync = true;
			dynamicNpcs_.erase(dynamicNpcs_.begin() + i--);
		}
	}

	if(needSync){
		for(ScenePlayerSet::iterator i = players_.begin(), e = players_.end(); i!=e; ++i ){
			(*i)->UpdateNpcs();
		}
	}
}
void Scene::ClearDynamicNpcs(){
	bool needSync = !dynamicNpcs_.empty();
	dynamicNpcs_.clear();
	if(needSync){
		for(ScenePlayerSet::iterator i = players_.begin(), e = players_.end(); i!=e; ++i ){
			(*i)->UpdateNpcs();
		}
	}
}

SceneEntry* Scene::FindEntryByToSceneId(S32 toSceneId){
	for (size_t i=0; i<entrys_.size(); ++i)
	{
		if(entrys_[i]->nextSceneId_ == toSceneId){
			return entrys_[i];
		}
	}
	return NULL;
}

SceneEntry* Scene::FindEntry(S32 id){
	for(size_t i=0; i<entrys_.size(); ++i){
		if(entrys_[i]->entryId_ == id)
			return entrys_[i];
	}
	return NULL;
}
SceneEntry* Scene::FindBorn(){
	for(size_t i=0; i<entrys_.size(); ++i){
		if(entrys_[i]->isBornPos_)
			return entrys_[i];
	}
	return NULL;
}

ScenePoint* Scene::FindPoint(S32 id){
	for(size_t i=0; i<points_.size(); ++i){
		if(points_[i]->pointId_ == id)
			return points_[i];
	}
	return NULL;
}

SceneZone* Scene::FindZone(S32 id){
	for (size_t i=0; i<zones_.size(); ++i)
	{
		if(zones_[i]->zoneId_ == id)
			return zones_[i];
	}
	return NULL;
}

void Scene::AddInDegree(Scene* p){
	for (size_t i=0; i<inDegree_.size(); ++i)
	{
		if(inDegree_[i] == p)
			return;
	}
	inDegree_.push_back(p);
}
void  Scene::AddOutDegree(Scene* p){
	for (size_t i=0; i<outDegree_.size(); ++i)
	{
		if(outDegree_[i] == p)
			return;
	}
	outDegree_.push_back(p);
}

Scene* Scene::FindOutDegree(S32 id){
	for (size_t i=0; i<outDegree_.size(); ++i)
	{
		if(outDegree_[i]->sceneId_ == id)
			return outDegree_[i];
	}
	return NULL;
}

struct TraceSceneLock{
	TraceSceneLock(Scene* s):s_(s){
		s->traceSceneLock_ = true;
	}
	~TraceSceneLock(){
		s_->traceSceneLock_ = false;
	}

	Scene* s_;
};

S32 Scene::TraceScene(S32 toSceneId,std::vector<Scene*>& sceneRoad){
	if(sceneType_ == SCT_Bairen)
		return 0;
	
	if(traceSceneLock_)
		return 0;
	
	TraceSceneLock lock(this);	
	sceneRoad.push_back(this);

	if(sceneId_ == toSceneId){
		return 1; 
	}
	std::vector<Scene*> tmpR1;
	for(size_t i=0; i<outDegree_.size(); ++i){
		SceneEntry* se = FindEntryByToSceneId(outDegree_[i]->sceneId_);
		SRV_ASSERT(se);//能去这个场景必须有能去的 entry 点
		if(tmpR1.empty())
			outDegree_[i]->TraceScene(toSceneId,tmpR1);
		else {
			std::vector<Scene*> tmpR2;
			outDegree_[i]->TraceScene(toSceneId,tmpR2);
			if(!tmpR2.empty() && tmpR2.size() < tmpR1.size()){
				tmpR1 = tmpR2;
			}
		}
	}
	if(!tmpR1.empty()){
		sceneRoad.insert(sceneRoad.end(),tmpR1.begin(),tmpR1.end());
		return tmpR1.size() + 1;
	}

	sceneRoad.pop_back();
	return 0;
}

bool Scene::TraceScene(ScenePlayer* sp, S32 toScene, std::vector<S32>& list)
{
	if(traceSceneLock_)
		return false;
	if(sceneType_ == SCT_Bairen)
		return false;
	if(FindBorn() && !sp->CanTransforToScene(sceneId_))
		return false;
	TraceSceneLock lock(this);

	list.push_back(sceneId_);

	if(sceneId_ == toScene){
		return true;
	}
	
	std::vector<S32> list0;
	for(size_t i=0; i<outDegree_.size(); ++i){
		std::vector<S32> list1;
		if(outDegree_[i]->TraceScene(sp,toScene,list1))
		{
			if(list0.empty() && !list1.empty())
				list0 = list1;
			else if(!list1.empty() && list1.size() < list0.size())
				list0 = list1;
			else
			{
				//???
			}
		}
	}
	if(list0.empty())
		return false;
	list.insert(list.end(),list0.begin(),list0.end());
	return true;
}

Scene* Scene::Copy(S32 copyId){
	/*if(sceneType_ != SCT_Instance)
		return NULL;*/
	Scene* p = new Scene();	
	p->sceneId_ = COMBINE_SCENE_COPY_ID(copyId,sceneId_);

	p->sceneType_= sceneType_;
	p->sceneName_ = sceneName_;
	p->navMesh_ = navMesh_;
		//}
	p->traceSceneLock_ = traceSceneLock_;
	p->zones_ = zones_;	//区域怪物
	p->entrys_= entrys_; //入口点
	p->points_= points_; //场景内位置点


	p->staticNpcs_= staticNpcs_; ///场景内NPC
	p->dynamicNpcs_= dynamicNpcs_; //一些活动的NPC
	p->outDegree_= outDegree_;
	p->inDegree_= inDegree_;

	return p;
}

SceneManager::~SceneManager(){
	for (size_t i=0; i<sceneCache_.size(); ++i)
	{
		if(sceneCache_[i]){
			sceneCache_[i]->Fini();
			delete sceneCache_[i];
		}
	}

	sceneCache_.clear();
	scenes_.clear();
}

void SceneManager::Init(){
	FUNCTION_PROBE;
	//初始化单独场景
	for (SceneTable::SceneMap::iterator i=SceneTable::scenes_.begin(),e=SceneTable::scenes_.end(); i!=e; ++i)
	{
		Scene* p = new Scene();
		p->Init(i->second);
		sceneCache_.push_back(p);
		scenes_[p->sceneId_] = p;
	}
	
	//建立场景网络
	Scene* mainScene = FindMainScene();
	S32 mainSceneEntryDummyId = 1;
	SceneEntry* dummy = new SceneEntry();
	dummy->entryId_ = 999;
	dummy->isBornPos_ = true;
	mainScene->entrys_.push_back(dummy);
	
	for(size_t i=0; i<sceneCache_.size(); ++i){
		
		//bool canFromHome = false; //能不能从家直接进入
		for(size_t j=0; j<sceneCache_[i]->entrys_.size(); ++j){	
			//不要胡吉巴关联
			Scene* toScene = FindScene(sceneCache_[i]->entrys_[j]->nextSceneId_);
			if(toScene){ //不是单向节点就关联
				toScene->AddInDegree(sceneCache_[i]);
				sceneCache_[i]->AddOutDegree(toScene);
			}
		
		}
	}

	struct SSort{
		bool operator()(Scene const* p0, Scene const* p1){
			if(p0->sceneType_ < p1->sceneType_)
				return true;
			else if(p0->sceneType_ > p1->sceneType_)
				return false;
			else return p0->sceneId_ < p1->sceneId_;
		}
	};

	for(size_t i=0; i<sceneCache_.size(); ++i){
		std::sort(sceneCache_[i]->outDegree_.begin(),sceneCache_[i]->outDegree_.end(),SSort());
	}	
}

void SceneManager::Update(float dt){
	for(size_t i=0; i<sceneCache_.size(); ++i){
		sceneCache_[i]->Update(dt);
	}
}	

Scene* SceneManager::FindMainScene(){
	for(size_t i=0; i<sceneCache_.size(); ++i){
		if(sceneCache_[i]->sceneType_ == SCT_Home)
			return sceneCache_[i];
	}
	return NULL;
}

Scene* SceneManager::FindScene(S32 sceneId){
	if(scenes_.find(sceneId) == scenes_.end())
		return NULL;
	return scenes_[sceneId];
}

bool SceneManager::FindRoad(ScenePlayer* sp, S32 endSceneId, std::vector<BehaviorNode>& road){

	{
		Scene* myScene = FindScene(sp->sceneId_);
		if(NULL == myScene)
		{
			ACE_DEBUG((LM_INFO,"Can not find road to scene %d %d --> %d \n",sp->playerId_,sp->sceneId_,endSceneId));
			return true;
		}
		SceneEntry* se = myScene->FindEntryByToSceneId(endSceneId); 
		if(se) //如果直接能走过去 就直接走过去
		{
			BehaviorNode bn(sp->sceneId_);
			bn.SetEntry(se->entryId_);
			road.push_back(bn);
			return true;	
		}
		Scene* endScene = FindScene(endSceneId);
		if((endScene->sceneType_ == SCT_Home || endScene->sceneType_ == SCT_City || endScene->sceneType_ == SCT_GuildHome) && sp->CanTransforToScene(endSceneId))
		{
			BehaviorNode bn(sp->sceneId_);
			bn.SetTransforScene(endScene->FindBorn()->entryId_,endScene->sceneId_);
			road.push_back(bn);
			return true;
		}
	}

	
	Scene* s = FindScene(sp->sceneId_);
	std::vector<Scene*> sceneRoad;
	s->TraceScene(endSceneId,sceneRoad);
	for(size_t i=0; i<sp->openScenes_.size(); ++i)
	{
		s = FindScene(sp->openScenes_[i]);
		if(s->sceneType_ != SCT_Home && s->sceneType_ != SCT_City && s->sceneType_ != SCT_GuildHome)
			continue;
		if(sceneRoad.empty())
		{
			s->TraceScene(endSceneId,sceneRoad);
		}
		else
		{
			std::vector<Scene*> sceneRoad1;
			if(s->TraceScene(endSceneId,sceneRoad1))
			{
				if(sceneRoad1.size() < sceneRoad.size())
					sceneRoad = sceneRoad1;
			}
		}
	}

	s = FindScene(sp->sceneId_);

	if(sceneRoad.empty())
		return false;
	else if(sceneRoad[0]->sceneId_ == s->sceneId_)
		sceneRoad.erase(sceneRoad.begin());

	std::vector<BehaviorNode> road1;
	std::stringstream sstream;
	for(size_t i=0; i<sceneRoad.size(); ++i)
	{
		sstream << "-->" << sceneRoad[i]->sceneId_ ; 


		SRV_ASSERT(s);

		BehaviorNode bn(s->sceneId_);
	
		if(sp->CanTransforToScene(sceneRoad[i]->sceneId_) && (sceneRoad[i]->sceneType_ == SCT_Home || sceneRoad[i]->sceneType_ == SCT_GuildHome || sceneRoad[i]->sceneType_ == SCT_City) )
		{
			bn.SetTransforScene(sceneRoad[i]->FindBorn()->entryId_,sceneRoad[i]->sceneId_);
		}
		else {
			SceneEntry* se = s->FindEntryByToSceneId(sceneRoad[i]->sceneId_);
			SRV_ASSERT(se);
			bn.SetEntry(se->entryId_);
		}
		road1.push_back(bn);
		s = sceneRoad[i];
	}
	road.insert(road.end(),road1.rbegin(),road1.rend());
	ACE_DEBUG((LM_INFO," Trace Scene [[[ %s ]]]\n",sstream.str().c_str()));
	

	return true;
}

void SceneManager::InitDynamicNpcs(NpcType type,S32 totalCount){
	std::vector<const NpcTable::NpcData* > npcs;
	NpcTable::getNpcs(type,npcs);
	if(totalCount>npcs.size() || totalCount <0 )
		totalCount = npcs.size();
	std::random_shuffle(npcs.begin(),npcs.end());
	std::vector< std::pair<S32, std::vector<S32> > > updateSceneNpcs ;
	for(size_t i=0; i<totalCount;++i){
		bool isAdded = false;
		bool isJump = false;
		for(S32 j=i-1; j>=0; --j){
			if( (npcs[j]->npcId_ == npcs[i]->npcId_) /*&& (npcs[j]->pointId_ == npcs[i]->pointId_)*/ ){
				isJump = true;
				break;
			}
		}
		if(isJump)
			continue;
		for(size_t j=0; j<updateSceneNpcs.size(); ++j){
			if(updateSceneNpcs[j].first == npcs[i]->sceneId_){
				updateSceneNpcs[j].second.push_back(npcs[i]->npcId_);
				isAdded = true; 
				break;
			}
		}
		if(!isAdded){
			std::pair<S32, std::vector<S32> > tmp;
			tmp.first = npcs[i]->sceneId_;
			tmp.second.push_back(npcs[i]->npcId_);
			updateSceneNpcs.push_back(tmp);
		}
	}
	
	for (size_t i=0; i<updateSceneNpcs.size() ;++i)
	{
		Scene* s = FindScene(updateSceneNpcs[i].first);
		SRV_ASSERT(s);
		s->AddDynamicNpcs(updateSceneNpcs[i].second);
		dynamicNpcs_.insert(dynamicNpcs_.end(),updateSceneNpcs[i].second.begin(),updateSceneNpcs[i].second.end());
	}
	ACE_DEBUG((LM_INFO,"SceneManager::InitDynamicNpcs  type = %d  ------ totalCount = %d\n",type,totalCount));
}
void SceneManager::RefreshDynamicNpcs(NpcType type,S32 totalCount){
	FiniDynamicNpcs(type);
	InitDynamicNpcs(type,totalCount);
}
void SceneManager::FiniDynamicNpcs(NpcType type){
	for (size_t i=0; i<sceneCache_.size(); ++i)
	{
	/*	if(sceneCache_[i]->sceneType_ == SCT_Scene)*/
		sceneCache_[i]->ClearDynamicNpcs(type);
	}

	for(S32 i=0; i<dynamicNpcs_.size(); ++i){
		const NpcTable::NpcData* npcd = NpcTable::getNpcById(dynamicNpcs_[i]);
		if(npcd && npcd->npcType_ == type){
			dynamicNpcs_.erase(dynamicNpcs_.begin() + i);
			--i;
		}
	}
}

S32 SceneManager::GetDynamicNpc(NpcType type){
	for(S32 i=0; i<dynamicNpcs_.size(); ++i){
		const NpcTable::NpcData* npcd = NpcTable::getNpcById(dynamicNpcs_[i]);
		if(npcd && npcd->npcType_ == type){
			return dynamicNpcs_[i];
		}
	}
	return 0;
}

void SceneManager::DelDynamicNpc(S32 npcId){
	const NpcTable::NpcData* npcd = NpcTable::getNpcById(npcId);
	if(!npcd)
		return;
	Scene* s = SceneManager::instance()->FindScene(npcd->sceneId_);
	s->DelDynamicNpc(npcId);

	for(S32 i=0; i<dynamicNpcs_.size(); ++i){
		if(dynamicNpcs_[i] == npcId){
			dynamicNpcs_.erase(dynamicNpcs_.begin() + i);
			break;
		}
	}
}

bool SceneManager::OpenSceneCopy(S32 instId){
	if(scenes_[instId])
		return false;
	Scene* s =  FindScene(GET_SCENE_ORIGINAL_ID(instId));
	
	s = s->Copy(GET_SCENE_COPY_ID(instId));
	sceneCache_.push_back(s);
	scenes_[s->sceneId_] = s;
	return true;
}

bool SceneManager::CloseSceneCopy(S32 instId){
	if(scenes_[instId]){
		if(GET_SCENE_COPY_ID(scenes_[instId]->sceneId_)){
			
			for (size_t i = 0; sceneCache_.size(); ++i)
			{
				if(sceneCache_[i]->sceneId_ == instId)
				{
					sceneCache_.erase(sceneCache_.begin() + i);
					break;
				}
			}

			std::map<S32,Scene*>::iterator itr = scenes_.find(instId);
			if(itr != scenes_.end())
			{
				Scene* p = itr->second;
				if(p != NULL)
				{
					scenes_.erase(itr);
					delete p;
					p = NULL;
				}
			}
			return true;
		}
	}
	return false;
}
