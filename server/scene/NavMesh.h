#ifndef __NAV_MESH_H__
#define __NAV_MESH_H__
#include "config.h"
#include "Recast.h"
#include "InputGeom.h"
#include "DetourNavMesh.h"
#include "DetourNavMeshQuery.h"
#include "scenetable.h"
#include "npctable.h"

enum PolyAreas
{
	POLYAREA_GROUND,
	POLYAREA_WATER,
	POLYAREA_ROAD,
	POLYAREA_DOOR,
	POLYAREA_GRASS,
	POLYAREA_JUMP,
};
enum PolyFlags
{
	POLYFLAGS_WALK		= 0x01,		// Ability to walk (ground, grass, road)
	POLYFLAGS_SWIM		= 0x02,		// Ability to swim (water).
	POLYFLAGS_DOOR		= 0x04,		// Ability to move through doors.
	POLYFLAGS_JUMP		= 0x08,		// Ability to jump.
	POLYFLAGS_DISABLED	= 0x10,		// Disabled polygon
	POLYFLAGS_ALL		= 0xffff	// All abilities.
};

enum PartitionType
{
	PARTITION_WATERSHED,
	PARTITION_MONOTONE,
	PARTITION_LAYERS,
};


class SceneMesh{
	enum{ MAX_POLYS = 256, MAX_SMOOTH = 2048};

public:
	SceneMesh(const std::string& meshpath );
	~SceneMesh();
	
	bool Build();
	void Cleanup();
	inline float GetAgentRadius() { return agentRadius_; }
	inline float GetAgentHeight() { return agentHeight_; }
	inline float GetAgentClimb() { return agentMaxClimb_; }
	inline const float* GetBoundsMin(){
		if(geom_)
			return geom_->getMeshBoundsMin();
		return NULL;
	}
	inline const float* GetBoundsMax(){
		if(geom_)
			return geom_->getMeshBoundsMax();
		return NULL;
	}
	bool GetSteerTarget(const float* startPos, const float* endPos,
		float* steerPos, unsigned char& steerPosFlag, dtPolyRef& steerPosRef,
		float* outPoints = 0, int* outPointCount = 0);
	int FixupCorridor(const dtPolyRef* visited, const int nvisited);
	int FixupShortcuts();
	void ResetFindPathContext(const COM_FPosition& start,const COM_FPosition& end);
	bool PrepareFindPath();
	bool FindPathPloys();
	bool CalcSmoothPath();
	bool TestPath(COM_FPosition start, COM_FPosition end); //测试能否通过
	bool FindPath(COM_FPosition start, COM_FPosition end, std::vector<COM_FPosition>& outpath, float length = 0);
	bool RollPathInRound(COM_FPosition start, COM_FPosition center, float radius ,std::vector<COM_FPosition>& outpath); //在一个园里 查找一个点
	//{寻路相关
	float startOut_[3];
	float endOut_[3];
	float startPos_[3];
	float endPos_[3]; 
	float extention[3];
	dtPolyRef startRef_;
	dtPolyRef endRef_; 
	dtPolyRef polys_[MAX_POLYS];
	S32		  npoly_;
	float smooth_[MAX_SMOOTH*3];
	S32 nsmooth_;
	dtQueryFilter filter_;
	
	class InputGeom* geom_;
	class dtNavMesh* navMesh_;
	class NavMeshQuery* navQuery_;
	class Crowd* crowd_;

	float cellSize_;
	float cellHeight_;
	float agentHeight_;
	float agentRadius_;
	float agentMaxClimb_;
	float agentMaxSlope_;
	float regionMinSize_;
	float regionMergeSize_;
	float edgeMaxLen_;
	float edgeMaxError_;
	float vertsPerPoly_;
	float detailSampleDist_;
	float detailSampleMaxError_;
	int partitionType_;

	bool keepInterResults_;
	float totalBuildTimeMs_;

	unsigned char* triareas_;
	rcHeightfield* solid_;
	rcCompactHeightfield* chf_;
	rcContourSet* cset_;
	rcPolyMesh* pmesh_;
	rcConfig cfg_;	
	rcPolyMeshDetail* dmesh_;
};

#endif