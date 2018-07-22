#ifndef __SCENE_H__
#define __SCENE_H__
#include "config.h"
#include "GlobalConstants.h"
#include "Recast.h"
#include "InputGeom.h"
#include "DetourNavMesh.h"
#include "DetourNavMeshQuery.h"
#include "scenetable.h"
#include "npctable.h"
#include "NavMesh.h"
struct BehaviorNode;
class ScenePlayer;

//一个场景

struct SceneArea{
	SceneArea():radius_(0){}
	bool isIn(ScenePlayer* sp);

	COM_FPosition center_;
	float radius_;
};

struct SceneZone : public SceneArea{
	SceneZone():zoneId_(0),zoneProb_(0){}
	S32		zoneId_;
	S32		zoneProb_; 
};

struct ScenePoint : public SceneArea{
	ScenePoint():pointId_(0),type_(FPT_None),py_(0),rx_(0),ry_(0),rz_(0),rw_(0){}
	S32	pointId_;
	FunctionalPointType type_;
	float py_, rx_,ry_,rz_,rw_; //旋转 
};


struct SceneEntry: public SceneArea{
	SceneEntry():sceneId_(0),entryId_(0),nextSceneId_(0),nextEntryId_(0),isBornPos_(0){}
	S32 sceneId_;
	S32 entryId_;
	S32 nextSceneId_;
	S32 nextEntryId_;
	bool isBornPos_;
};


class Scene : BINChannelBroadcaster<SGE_Player_Scene2WorldStub,ScenePlayer> {

	bool CalcZone(ScenePlayer* sp); //计算是否在zone
	bool CalcEntry(ScenePlayer* sp); //计算是否在entry
	enum{ MAX_POLYS = 256, MAX_SMOOTH = 2048};

#define STEP_SIZE (0.6F)
#define SLOP (0.01F)
public:
	Scene();
	virtual ~Scene();
	bool Init(SceneData* table);
	void BuildNormalNpcs();
	void Update(float dt);
	
	bool Fini();
	bool IsNearNpc(ScenePlayer* sp,S32 npcId); //计算是否在npc旁边

	bool JoinScene(ScenePlayer* sp, S32 entryId);
	bool JoinScene(ScenePlayer* sp, COM_FPosition pos);
	bool ExitScene(ScenePlayer* sp);
	
	SceneZone* StandZone(ScenePlayer*sp);
	inline bool FindPath(COM_FPosition start, COM_FPosition end, std::vector<COM_FPosition>& outpath,float length=0.F){
		return navMesh_->FindPath(start, end, outpath, length);
	};
	bool RollPathInZone(COM_FPosition start, S32 zoneId,std::vector<COM_FPosition>& outpath);
	bool FindPath2Point(COM_FPosition start, COM_FPosition endpos,std::vector<COM_FPosition>& outpath); 
	bool FindPath2Entity(COM_FPosition start, S32 entryId,std::vector<COM_FPosition>& outpath);
	bool FindPath2Zone(COM_FPosition start, S32 zoneId,std::vector<COM_FPosition>& outpath);
	bool FindPathToOutDegree(COM_FPosition start, S32 sceneId,std::vector<COM_FPosition>& outpath);
	int32 FindStaticNpc(int32 npcid);
	int32 FindDynamicNpc(int32 npcid);
	int32 GetOneDynamicNpc(NpcType type);
	int32 FindNpc(int32 npcid);
	void GetNpcs(ScenePlayer* player,std::vector<int32>& npcs);
	void AddDynamicNpcs(std::vector<S32> npcs);
	void DelDynamicNpc(S32 npcid);
	void ClearDynamicNpcs(NpcType type);
	void ClearDynamicNpcs();
	
	SceneEntry* FindEntryByToSceneId(S32 toSceneId);
	SceneEntry* FindEntry(S32 id);
	SceneEntry* FindBorn();
	ScenePoint* FindPoint(S32 id);
	SceneZone* FindZone(S32 id);
	Scene* FindOutDegree(S32 id);
	void AddInDegree(Scene* p);
	void AddOutDegree(Scene* p);
	//寻找到指定场景所通过的场景ID 
	S32 TraceScene(S32 toSceneId, std::vector<Scene*>& sceneroad);

	bool TraceScene(ScenePlayer* sp, S32 toScene,std::vector<S32>& list);

	Scene* Copy(S32 copyId);
	
	S32	sceneId_;
	SceneType sceneType_;
	std::string sceneName_;

	class SceneMesh* navMesh_;
	//}
	bool traceSceneLock_;
	std::vector<SceneZone*>		zones_;	//区域怪物
	std::vector<SceneEntry*>	entrys_; //入口点
	std::vector<ScenePoint*>	points_; //场景内位置点

	
	NpcList staticNpcs_; ///场景内NPC
	NpcList dynamicNpcs_; //一些活动的NPC

	typedef std::set<ScenePlayer*> ScenePlayerSet;
	ScenePlayerSet players_; //当前场景内人

	std::vector<Scene*> outDegree_;
	std::vector<Scene*> inDegree_;
};

class SceneManager{
public:
	SINGLETON_FUNCTION(SceneManager);

	~SceneManager();

	void Init();
	void Update(float dt);
	Scene* FindMainScene(); //获得主城
	Scene* FindScene(S32);
	bool FindRoad(ScenePlayer* sp, S32 endSceneId, std::vector<BehaviorNode>& road);
	void InitDynamicNpcs(NpcType type,S32 totalCount = 15);
	void RefreshDynamicNpcs(NpcType type,S32 totalCount = 15);
	void FiniDynamicNpcs(NpcType type);
	S32 GetDynamicNpc(NpcType type); 
	void DelDynamicNpc(S32 npcId);
	
	bool OpenSceneCopy(S32 instId);
	bool CloseSceneCopy(S32 instId);
	std::vector<Scene*> sceneCache_;
	std::map<S32,Scene*> scenes_;
	std::vector<S32> dynamicNpcs_;
};

#endif