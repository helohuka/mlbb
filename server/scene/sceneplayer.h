#ifndef __SCENE_PLAYER_H__
#define __SCENE_PLAYER_H__
#include "config.h"
#include "npctable.h"
#define  PLAYER_SPEED 2.6F
#define  PLAYER_LENGTH 0.6F

struct BehaviorNode{

	enum Type{
		T_None
		,T_Entry
		,T_Point
		,T_Zone
		,T_TransforScene
		,T_PathStopNpc
		,T_RollPathInZone
		,T_TraceScene
	};
	bool noPop_;
	S32 sceneId_;
	S32 value_;
	COM_FPosition posi_;
	S32 exten_;
	Type type_;
	BehaviorNode(S32 sceneId):noPop_(false),sceneId_(sceneId),value_(0),type_(T_None){
		Reset();
	}

	inline void Reset(){
		value_ = 0;
		exten_ = 0;
		noPop_ = false;
		type_ = T_None;
	}

	inline void SetZone(S32 zoneId){
		Reset();
		type_ = T_Zone;
		value_ = zoneId;
	}

	inline void SetPoint(COM_FPosition posi){
		Reset();
		type_ = T_Point;
		posi_ = posi;
		posi_.x_ = -posi_.x_;
	}

	inline void SetEntry(S32 entryId){
		Reset();
		type_ = T_Entry;
		value_ = entryId;
	}

	inline void SetPathStopNpc(S32 npcId){
		Reset();
		type_ = T_PathStopNpc;
		value_ = npcId;
	}

	inline void SetRollPathInZone(S32 zoneId){
		Reset();
		type_ = T_RollPathInZone;
		noPop_ = true;
		value_ = zoneId;
	}

	inline void SetTransforScene(S32 entryId,S32 sceneId){
		Reset();
		type_ = T_TransforScene;
		value_ = entryId;
		exten_ = sceneId;
	}

	inline void SetTraceScene(S32 sceneId){
		Reset();
		type_ = T_TraceScene;
		value_ = sceneId;
	}

	inline void TraceDebug(){
		//ACE_DEBUG((LM_INFO,"Scene %d, Type %d, Value %d, Exten %d \n",sceneId_,(int32)type_,value_,exten_));
	}
};

class ScenePlayer :public BINChannel< SGE_Player_Scene2WorldStub , SGE_Player_World2SceneProxy >
	, public SGE_Player_World2SceneProxy {
public:
	typedef std::vector<int32> NpcList;

	typedef std::set<ScenePlayer*> Set;
	static Set sceneplayers_;

	static void UpdateScenePlayers(float dt);
	
	static ScenePlayer* GetScenePlayer(S32 playerId);

	bool handleClose();
public:
#include "SGE_Player_World2SceneMethods.h"
public:
	enum{
		Normal,
		Battle,
		LoadScene,
	};

	ScenePlayer();
	~ScenePlayer();
	class Scene* MyScene();
	bool Update(float dt);
	
	void PushBehavior(BehaviorNode bn);
	void CalcBehavior();
	bool CalcMove(float dt);
	bool Move2Pos(COM_FPosition pos);
	bool Move2Npc(S32 npcId);
	bool Move2Zone(S32 sceneId,S32 zoneId);
	
	void CleanMove();
	void JoinBattle(S32 zoneId);
	void EnterScene(S32 sceneId, S32 entryId);
	void EnterScene(S32 sceneId, COM_FPosition pos);
	void JoinScene(S32 sceneId,S32 entryId,COM_FPosition pos, NpcList& staticNpcs, NpcList& dynamicNpcs,bool needLoadSceneStatus);
	void UpdateNpcs();
	bool HasNpc(int32 npcId);
	void DelNpc(int32 npcId);
	int32 FindNpc(int32 npcid);
	bool IsInCurrentQuest(const std::vector<S32>& ids);
	bool CanTransforToScene(S32 sceneId);
	
	/* **/ 
	void SetTargetPos(COM_FPosition pos, bool isLast=false);
	void CalcFollowsTargetPos(COM_FPosition pos,bool isLast = false);
	void StopFollowsMove();
	void ToSceneFollows(S32 sceneId,S32 entryId);
	void ToSceneFollows(S32 sceneId,COM_FPosition pos);
	bool IsNormalState(){return status_ == Normal;}
	S32 playerId_;
	S32 playerLevel_;
	S32 sceneId_;
	S32 entryId_; //阻止重复且场景
	S32 step_; //场景内走了几步
	S32 status_;
	bool entryFlag_;	
	COM_FPosition pos_; //当前坐标
	COM_FPosition targetPos_;
	
	std::vector<S32> currentQuestIds_;
	std::vector<S32> acceptableQuestIds_;
	std::vector<S32> openScenes_;
	std::vector<int32> cacheNpcs_; //自己的NPC; 跟这自己的任务阶段
	std::vector<int32> privateNpcs_;
	//{ 寻路相关

	std::vector<COM_FPosition> path_; //所寻得的路径
	std::vector<BehaviorNode> behaviorRoad_;	  //场景路径
	
	
	bool isFollow_; //是否是跟随者
	std::vector<ScenePlayer*> follows_; ///跟随玩家 (组队
};

#endif