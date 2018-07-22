#include "config.h"
#include "NavMesh.h"
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

#define STEP_SIZE (1.5F)
#define SLOP (0.01F)

SceneMesh::SceneMesh(const std::string& meshpath)
:geom_(0)
,navMesh_(0)
,navQuery_(0)
,crowd_(0)
,keepInterResults_(true)
,totalBuildTimeMs_(0)
,triareas_(0)
,solid_(0)
,chf_(0)
,cset_(0)
,pmesh_(0)
,dmesh_(0)
{
	FUNCTION_PROBE;
	startOut_[0] = startOut_[1] = startOut_[2] = 0.F;
	endOut_[0] = endOut_[1] = endOut_[2] = 0.F;
	startPos_[0] = startPos_[1] = startPos_[2] = 0.F;
	endPos_[0] = endPos_[1] = endPos_[2] = 0.F;
	extention[0] = 2.F;
	extention[1] = 4.F;
	extention[2] = 2.F;

	

	geom_ = new InputGeom();
	if(!geom_->loadMesh(meshpath.c_str()))
	{
		delete geom_;
		ACE_DEBUG((LM_ERROR,"Can not load geom %s \n",meshpath.c_str()));
		return;
	}
	navQuery_ = dtAllocNavMeshQuery();
	crowd_ = AllocCrowd();

	Build();
}
SceneMesh::~SceneMesh(){
	dtFreeNavMeshQuery(navQuery_);
	dtFreeNavMesh(navMesh_);
	FreeCrowd(crowd_);
	Cleanup();
}


bool SceneMesh::Build()
{
	
	//settings 
	cellSize_ = 0.1f;
	cellHeight_ = 0.1f;
	agentHeight_ = 2.0f;
	agentRadius_ = 0.05f;
	agentMaxClimb_ = 0.9f;
	agentMaxSlope_ = 45.0f;
	regionMinSize_ = 8;
	regionMergeSize_ = 20;
	edgeMaxLen_ = 12.0f;
	edgeMaxError_ = 1.3f;
	vertsPerPoly_ = 6.0f;
	detailSampleDist_ = 6.0f;
	detailSampleMaxError_ = 1.0f;
	partitionType_ = PARTITION_WATERSHED;

	if (!geom_ || !geom_->getMesh())
	{
		ACE_DEBUG((LM_ERROR,  "buildNavigation: Input mesh is not specified.\n"));
		return false;
	}

	Cleanup();

	const float* bmin = geom_->getMeshBoundsMin();
	const float* bmax = geom_->getMeshBoundsMax();
	const float* verts = geom_->getMesh()->getVerts();
	const int nverts = geom_->getMesh()->getVertCount();
	const int* tris = geom_->getMesh()->getTris();
	const int ntris = geom_->getMesh()->getTriCount();

	//
	// Step 1. Initialize build config.
	//

	// Init build configuration from GUI
	memset(&cfg_, 0, sizeof(cfg_));
	cfg_.cs = cellSize_;
	cfg_.ch = cellHeight_;
	cfg_.walkableSlopeAngle = agentMaxSlope_;
	cfg_.walkableHeight = (int)ceilf(agentHeight_ / cfg_.ch);
	cfg_.walkableClimb = (int)floorf(agentMaxClimb_ / cfg_.ch);
	cfg_.walkableRadius = (int)ceilf(agentRadius_ / cfg_.cs);
	cfg_.maxEdgeLen = (int)(edgeMaxLen_ / cellSize_);
	cfg_.maxSimplificationError = edgeMaxError_;
	cfg_.minRegionArea = (int)rcSqr(regionMinSize_);		// Note: area = size*size
	cfg_.mergeRegionArea = (int)rcSqr(regionMergeSize_);	// Note: area = size*size
	cfg_.maxVertsPerPoly = (int)vertsPerPoly_;
	cfg_.detailSampleDist = detailSampleDist_ < 0.9f ? 0 : cellSize_ * detailSampleDist_;
	cfg_.detailSampleMaxError = cellHeight_ * detailSampleMaxError_;

	// Set the area where the navigation will be build.
	// Here the bounds of the input mesh are used, but the
	// area could be specified by an user defined box, etc.
	rcVcopy(cfg_.bmin, bmin);
	rcVcopy(cfg_.bmax, bmax);
	rcCalcGridSize(cfg_.bmin, cfg_.bmax, cfg_.cs, &cfg_.width, &cfg_.height);

	ACE_DEBUG((LM_INFO,  "Building navigation:\n"));
	ACE_DEBUG((LM_INFO,  " - %d x %d cells\n", cfg_.width, cfg_.height));
	ACE_DEBUG((LM_INFO,  " - %.1fK verts, %.1fK tris\n", nverts/1000.0f, ntris/1000.0f));

	//
	// Step 2. Rasterize input polygon soup.
	//

	// Allocate voxel heightfield where we rasterize our input data to.
	solid_ = rcAllocHeightfield();
	if (!solid_)
	{
		ACE_DEBUG((LM_ERROR,  "buildNavigation: Out of memory 'solid'."));
		return false;
	}
	if (!rcCreateHeightfield(*solid_, cfg_.width, cfg_.height, cfg_.bmin, cfg_.bmax, cfg_.cs, cfg_.ch))
	{
		ACE_DEBUG((LM_ERROR,  "buildNavigation: Could not create solid heightfield."));
		return false;
	}

	// Allocate array that can hold triangle area types.
	// If you have multiple meshes you need to process, allocate
	// and array which can hold the max number of triangles you need to process.
	triareas_ = new unsigned char[ntris];
	if (!triareas_)
	{
		ACE_DEBUG((LM_ERROR,  "buildNavigation: Out of memory 'm_triareas' (%d).\n", ntris));
		return false;
	}

	// Find triangles which are walkable based on their slope and rasterize them.
	// If your input data is multiple meshes, you can transform them here, calculate
	// the are type for each of the meshes and rasterize them.
	memset(triareas_, 0, ntris*sizeof(unsigned char));
	rcMarkWalkableTriangles(cfg_.walkableSlopeAngle, verts, nverts, tris, ntris, triareas_);
	rcRasterizeTriangles(verts, nverts, tris, triareas_, ntris, *solid_, cfg_.walkableClimb);

	if (!keepInterResults_)
	{
		delete [] triareas_;
		triareas_ = 0;
	}

	//
	// Step 3. Filter walkables surfaces.
	//

	// Once all geoemtry is rasterized, we do initial pass of filtering to
	// remove unwanted overhangs caused by the conservative rasterization
	// as well as filter spans where the character cannot possibly stand.
	rcFilterLowHangingWalkableObstacles(cfg_.walkableClimb, *solid_);
	rcFilterLedgeSpans(cfg_.walkableHeight, cfg_.walkableClimb, *solid_);
	rcFilterWalkableLowHeightSpans(cfg_.walkableHeight, *solid_);


	//
	// Step 4. Partition walkable surface to simple regions.
	//

	// Compact the heightfield so that it is faster to handle from now on.
	// This will result more cache coherent data as well as the neighbours
	// between walkable cells will be calculated.
	chf_ = rcAllocCompactHeightfield();
	if (!chf_)
	{
		ACE_DEBUG((LM_ERROR, "buildNavigation: Out of memory 'chf'.\n"));
		return false;
	}
	if (!rcBuildCompactHeightfield(cfg_.walkableHeight, cfg_.walkableClimb, *solid_, *chf_))
	{
		ACE_DEBUG((LM_ERROR,  "buildNavigation: Could not build compact data.\n"));
		return false;
	}

	if (!keepInterResults_)
	{
		rcFreeHeightField(solid_);
		solid_ = 0;
	}

	// Erode the walkable area by agent radius.
	if (!rcErodeWalkableArea(cfg_.walkableRadius, *chf_))
	{
		ACE_DEBUG((LM_ERROR, "buildNavigation: Could not erode.\n"));
		return false;
	}

	// (Optional) Mark areas.
	const ConvexVolume* vols = geom_->getConvexVolumes();
	for (int i  = 0; i < geom_->getConvexVolumeCount(); ++i)
		rcMarkConvexPolyArea(vols[i].verts, vols[i].nverts, vols[i].hmin, vols[i].hmax, (unsigned char)vols[i].area, *chf_);


	// Partition the heightfield so that we can use simple algorithm later to triangulate the walkable areas.
	// There are 3 martitioning methods, each with some pros and cons:
	// 1) Watershed partitioning
	//   - the classic Recast partitioning
	//   - creates the nicest tessellation
	//   - usually slowest
	//   - partitions the heightfield into nice regions without holes or overlaps
	//   - the are some corner cases where this method creates produces holes and overlaps
	//      - holes may appear when a small obstacles is close to large open area (triangulation can handle this)
	//      - overlaps may occur if you have narrow spiral corridors (i.e stairs), this make triangulation to fail
	//   * generally the best choice if you precompute the nacmesh, use this if you have large open areas
	// 2) Monotone partioning
	//   - fastest
	//   - partitions the heightfield into regions without holes and overlaps (guaranteed)
	//   - creates long thin polygons, which sometimes causes paths with detours
	//   * use this if you want fast navmesh generation
	// 3) Layer partitoining
	//   - quite fast
	//   - partitions the heighfield into non-overlapping regions
	//   - relies on the triangulation code to cope with holes (thus slower than monotone partitioning)
	//   - produces better triangles than monotone partitioning
	//   - does not have the corner cases of watershed partitioning
	//   - can be slow and create a bit ugly tessellation (still better than monotone)
	//     if you have large open areas with small obstacles (not a problem if you use tiles)
	//   * good choice to use for tiled navmesh with medium and small sized tiles

	if (partitionType_ == PARTITION_WATERSHED)
	{
		// Prepare for region partitioning, by calculating distance field along the walkable surface.
		if (!rcBuildDistanceField(*chf_))
		{
			ACE_DEBUG((LM_ERROR,  "buildNavigation: Could not build distance field.\n"));
			return false;
		}

		// Partition the walkable surface into simple regions without holes.
		if (!rcBuildRegions(*chf_, 0, cfg_.minRegionArea, cfg_.mergeRegionArea))
		{
			ACE_DEBUG((LM_ERROR,  "buildNavigation: Could not build watershed regions\n"));
			return false;
		}
	}
	else if (partitionType_ == PARTITION_MONOTONE)
	{
		// Partition the walkable surface into simple regions without holes.
		// Monotone partitioning does not need distancefield.
		if (!rcBuildRegionsMonotone( *chf_, 0, cfg_.minRegionArea, cfg_.mergeRegionArea))
		{
			ACE_DEBUG((LM_ERROR,  "buildNavigation: Could not build monotone regions.\n"));
			return false;
		}
	}
	else // SAMPLE_PARTITION_LAYERS
	{
		// Partition the walkable surface into simple regions without holes.
		if (!rcBuildLayerRegions(*chf_, 0, cfg_.minRegionArea))
		{
			ACE_DEBUG((LM_ERROR, "buildNavigation: Could not build layer regions.\n"));
			return false;
		}
	}

	//
	// Step 5. Trace and simplify region contours.
	//

	// Create contours.
	cset_ = rcAllocContourSet();
	if (!cset_)
	{
		ACE_DEBUG((LM_ERROR, "buildNavigation: Out of memory 'cset'.\n"));
		return false;
	}
	if (!rcBuildContours(*chf_, cfg_.maxSimplificationError, cfg_.maxEdgeLen, *cset_))
	{
		ACE_DEBUG((LM_ERROR, "buildNavigation: Could not create contours.\n"));
		return false;
	}

	//
	// Step 6. Build polygons mesh from contours.
	//

	// Build polygon navmesh from the contours.
	pmesh_ = rcAllocPolyMesh();
	if (!pmesh_)
	{
		ACE_DEBUG((LM_ERROR, "buildNavigation: Out of memory 'pmesh'.\n"));
		return false;
	}
	if (!rcBuildPolyMesh(*cset_, cfg_.maxVertsPerPoly, *pmesh_))
	{
		ACE_DEBUG((LM_ERROR, "buildNavigation: Could not triangulate contours.\n"));
		return false;
	}

	//
	// Step 7. Create detail mesh which allows to access approximate height on each polygon.
	//

	dmesh_ = rcAllocPolyMeshDetail();
	if (!dmesh_)
	{
		ACE_DEBUG((LM_ERROR, "buildNavigation: Out of memory 'pmdtl'.\n"));
		return false;
	}

	if (!rcBuildPolyMeshDetail(*pmesh_, *chf_, cfg_.detailSampleDist, cfg_.detailSampleMaxError, *dmesh_))
	{
		ACE_DEBUG((LM_ERROR, "buildNavigation: Could not build detail mesh.\n"));
		return false;
	}

	if (!keepInterResults_)
	{
		rcFreeCompactHeightfield(chf_);
		chf_ = 0;
		rcFreeContourSet(cset_);
		cset_ = 0;
	}

	// At this point the navigation mesh data is ready, you can access it from m_pmesh.
	// See duDebugDrawPolyMesh or dtCreateNavMeshData as examples how to access the data.

	//
	// (Optional) Step 8. Create Detour data from Recast poly mesh.
	//

	// The GUI may allow more max points per polygon than Detour can handle.
	// Only build the detour navmesh if we do not exceed the limit.
	if (cfg_.maxVertsPerPoly <= DT_VERTS_PER_POLYGON)
	{
		unsigned char* navData = 0;
		int navDataSize = 0;

		// Update poly flags from areas.
		for (int i = 0; i < pmesh_->npolys; ++i)
		{
			if (pmesh_->areas[i] == RC_WALKABLE_AREA)
				pmesh_->areas[i] = POLYAREA_GROUND;

			if (pmesh_->areas[i] == POLYAREA_GROUND ||
				pmesh_->areas[i] == POLYAREA_GRASS ||
				pmesh_->areas[i] == POLYAREA_ROAD)
			{
				pmesh_->flags[i] = POLYFLAGS_WALK;
			}
			else if (pmesh_->areas[i] == POLYAREA_WATER)
			{
				pmesh_->flags[i] = POLYFLAGS_SWIM;
			}
			else if (pmesh_->areas[i] == POLYAREA_DOOR)
			{
				pmesh_->flags[i] = POLYFLAGS_WALK | POLYFLAGS_DOOR;
			}
		}


		NavMeshCreateParams params;
		memset(&params, 0, sizeof(params));
		params.verts = pmesh_->verts;
		params.vertCount = pmesh_->nverts;
		params.polys = pmesh_->polys;
		params.polyAreas = pmesh_->areas;
		params.polyFlags = pmesh_->flags;
		params.polyCount = pmesh_->npolys;
		params.nvp = pmesh_->nvp;
		params.detailMeshes = dmesh_->meshes;
		params.detailVerts = dmesh_->verts;
		params.detailVertsCount = dmesh_->nverts;
		params.detailTris = dmesh_->tris;
		params.detailTriCount = dmesh_->ntris;
		params.offMeshConVerts = geom_->getOffMeshConnectionVerts();
		params.offMeshConRad = geom_->getOffMeshConnectionRads();
		params.offMeshConDir = geom_->getOffMeshConnectionDirs();
		params.offMeshConAreas = geom_->getOffMeshConnectionAreas();
		params.offMeshConFlags = geom_->getOffMeshConnectionFlags();
		params.offMeshConUserID = geom_->getOffMeshConnectionId();
		params.offMeshConCount = geom_->getOffMeshConnectionCount();
		params.walkableHeight = agentHeight_;
		params.walkableRadius = agentRadius_;
		params.walkableClimb = agentMaxClimb_;
		rcVcopy(params.bmin, pmesh_->bmin);
		rcVcopy(params.bmax, pmesh_->bmax);
		params.cs = cfg_.cs;
		params.ch = cfg_.ch;
		params.buildBvTree = true;

		if (!CreateNavMeshData(&params, &navData, &navDataSize))
		{
			ACE_DEBUG((LM_ERROR, "Could not build Detour navmesh.\n"));
			return false;
		}

		navMesh_ = dtAllocNavMesh();
		if (!navMesh_)
		{
			dtFree(navData);
			ACE_DEBUG((LM_ERROR, "Could not create Detour navmesh\n"));
			return false;
		}

		dtStatus status;

		status = navMesh_->init(navData, navDataSize, DT_TILE_FREE_DATA);
		if (dtStatusFailed(status))
		{
			dtFree(navData);
			ACE_DEBUG((LM_ERROR, "Could not init Detour navmesh\n"));
			return false;
		}

		status = navQuery_->init(navMesh_, 2048);
		if (dtStatusFailed(status))
		{
			ACE_DEBUG((LM_INFO,  "Could not init Detour navmesh query\n"));
			return false;
		}
	}

	// Show performance stats.
	ACE_DEBUG((LM_INFO, ">> Polymesh: %d vertices  %d polygons.\n", pmesh_->nverts, pmesh_->npolys));

	return true;
}


bool SceneMesh::GetSteerTarget(const float* startPos, const float* endPos,
						   float* steerPos, unsigned char& steerPosFlag, dtPolyRef& steerPosRef,
						   float* outPoints , int* outPointCount )							 
{
	// Find steer target.
	static const int MAX_STEER_POINTS = 3;
	float steerPath[MAX_STEER_POINTS*3];
	unsigned char steerPathFlags[MAX_STEER_POINTS];
	dtPolyRef steerPathPolys[MAX_STEER_POINTS];
	int nsteerPath = 0;
	navQuery_->findStraightPath(startPos, endPos, polys_, npoly_,
		steerPath, steerPathFlags, steerPathPolys, &nsteerPath, MAX_STEER_POINTS);
	if (!nsteerPath)
		return false;

	if (outPoints && outPointCount)
	{
		*outPointCount = nsteerPath;
		for (int i = 0; i < nsteerPath; ++i)
			dtVcopy(&outPoints[i*3], &steerPath[i*3]);
	}


	// Find vertex far enough to steer to.
	int ns = 0;
	while (ns < nsteerPath)
	{
		// Stop at Off-Mesh link or when point is further than slop away.
		if ((steerPathFlags[ns] & DT_STRAIGHTPATH_OFFMESH_CONNECTION) ||
			!inRange(&steerPath[ns*3], startPos, SLOP, 1000.0f))
			break;
		ns++;
	}
	// Failed to find good point to steer to.
	if (ns >= nsteerPath)
		return false;

	dtVcopy(steerPos, &steerPath[ns*3]);
	steerPos[1] = startPos[1];
	steerPosFlag = steerPathFlags[ns];
	steerPosRef = steerPathPolys[ns];

	return true;
}



int SceneMesh::FixupCorridor(const dtPolyRef* visited, const int nvisited)
{
	int furthestPath = -1;
	int furthestVisited = -1;

	// Find furthest common polygon.
	for (int i = npoly_-1; i >= 0; --i)
	{
		bool found = false;
		for (int j = nvisited-1; j >= 0; --j)
		{
			if (polys_[i] == visited[j])
			{
				furthestPath = i;
				furthestVisited = j;
				found = true;
			}
		}
		if (found)
			break;
	}

	// If no intersection found just return current path. 
	if (furthestPath == -1 || furthestVisited == -1)
		return npoly_;

	// Concatenate paths.	

	// Adjust beginning of the buffer to include the visited.
	const int req = nvisited - furthestVisited;
	const int orig = rcMin(furthestPath+1, npoly_);
	int size = rcMax(0, npoly_-orig);
	if (req+size > MAX_POLYS)
		size = MAX_POLYS-req;
	if (size)
		memmove(polys_+req, polys_+orig, size*sizeof(dtPolyRef));

	// Store visited
	for (int i = 0; i < req; ++i)
		polys_[i] = visited[(nvisited-1)-i];				

	return req+size;
}

// This function checks if the path has a small U-turn, that is,
// a polygon further in the path is adjacent to the first polygon
// in the path. If that happens, a shortcut is taken.
// This can happen if the target (T) location is at tile boundary,
// and we're (S) approaching it parallel to the tile edge.
// The choice at the vertex can be arbitrary, 
//  +---+---+
//  |:::|:::|
//  +-S-+-T-+
//  |:::|   | <-- the step can end up in here, resulting U-turn path.
//  +---+---+
int SceneMesh::FixupShortcuts()
{
	if (npoly_ < 3)
		return npoly_;

	// Get connected polygons
	static const int maxNeis = 16;
	dtPolyRef neis[maxNeis];
	int nneis = 0;

	const dtMeshTile* tile = 0;
	const dtPoly* poly = 0;
	if (dtStatusFailed(navQuery_->getAttachedNavMesh()->getTileAndPolyByRef(polys_[0], &tile, &poly)))
		return npoly_;

	for (unsigned int k = poly->firstLink; k != DT_NULL_LINK; k = tile->links[k].next)
	{
		const dtLink* link = &tile->links[k];
		if (link->ref != 0)
		{
			if (nneis < maxNeis)
				neis[nneis++] = link->ref;
		}
	}

	// If any of the neighbour polygons is within the next few polygons
	// in the path, short cut to that polygon directly.
	static const int maxLookAhead = 6;
	int cut = 0;
	for (int i = dtMin(maxLookAhead, npoly_) - 1; i > 1 && cut == 0; i--) {
		for (int j = 0; j < nneis; j++)
		{
			if (polys_[i] == neis[j]) {
				cut = i;
				break;
			}
		}
	}
	if (cut > 1)
	{
		int offset = cut-1;
		npoly_ -= offset;
		for (int i = 1; i < npoly_; i++)
			polys_[i] = polys_[i+offset];
	}

	return npoly_;
}

void SceneMesh::Cleanup(){
	delete [] triareas_;
	triareas_ = 0;
	rcFreeHeightField(solid_);
	solid_ = 0;
	rcFreeCompactHeightfield(chf_);
	chf_ = 0;
	rcFreeContourSet(cset_);
	cset_ = 0;
	rcFreePolyMesh(pmesh_);
	pmesh_ = 0;
	rcFreePolyMeshDetail(dmesh_);
	dmesh_ = 0;
	dtFreeNavMesh(navMesh_);
	navMesh_ = 0;
}


void SceneMesh::ResetFindPathContext(const COM_FPosition& start,const COM_FPosition& end){
	startRef_ = 0;
	endRef_ = 0;

	startPos_[0] = start.x_;
	startPos_[2] = start.z_;
	endPos_[0] = end.x_;
	endPos_[2] = end.z_;

	filter_.setIncludeFlags(POLYFLAGS_ALL ^ POLYFLAGS_DISABLED);
	filter_.setExcludeFlags(0);
	filter_.setAreaCost(POLYAREA_GROUND, 1.0f);
	filter_.setAreaCost(POLYAREA_WATER, 10.0f);
	filter_.setAreaCost(POLYAREA_ROAD, 1.0f);
	filter_.setAreaCost(POLYAREA_DOOR, 1.0f);
	filter_.setAreaCost(POLYAREA_GRASS, 2.0f);
	filter_.setAreaCost(POLYAREA_JUMP, 1.5f);

	npoly_ = 0;
	nsmooth_ = 0;
}

bool SceneMesh::PrepareFindPath(){
	navQuery_->findNearestPoly(startPos_,extention,&filter_,&startRef_,startOut_);
	navQuery_->findNearestPoly(endPos_,extention,&filter_,&endRef_,endOut_);
	
	return !!(startRef_ && endRef_);
}

bool SceneMesh::FindPathPloys(){
	dtStatus s = navQuery_->findPath(startRef_, endRef_, startPos_, endPos_, &filter_, polys_, &npoly_, MAX_POLYS);

	return s == DT_SUCCESS; //成功找到
}

bool SceneMesh::CalcSmoothPath(){
	if(!npoly_)
		return false;
	float iterPos[3], targetPos[3];
	navQuery_->closestPointOnPoly(startRef_, startPos_, iterPos, 0);
	navQuery_->closestPointOnPoly(polys_[npoly_-1], endPos_, targetPos, 0);

	dtVcopy(&smooth_[nsmooth_*3], iterPos);
	++nsmooth_;

	// Move towards target a small advancement at a time until target reached or
	// when ran out of memory to store the path.
	while (npoly_ && nsmooth_ < MAX_SMOOTH)
	{
		// Find location to steer towards.
		float steerPos[3];
		U8 steerPosFlag;
		dtPolyRef steerPosRef;

		if (!GetSteerTarget(iterPos, targetPos, steerPos, steerPosFlag, steerPosRef))
					break;

		bool endOfPath = (steerPosFlag & DT_STRAIGHTPATH_END) ? true : false;
		bool offMeshConnection = (steerPosFlag & DT_STRAIGHTPATH_OFFMESH_CONNECTION) ? true : false;

				// Find movement delta.
		float delta[3], len;
		dtVsub(delta, steerPos, iterPos);
		len = dtMathSqrtf(dtVdot(delta, delta));
		// If the steer target is end of path or off-mesh link, do not move past the location.
		if ((endOfPath || offMeshConnection) && len < STEP_SIZE)
			len = 1;
		else
			len = STEP_SIZE / len;
		
		float moveTgt[3];
		dtVmad(moveTgt, iterPos, delta, len);

		// Move
		float result[3];
		dtPolyRef visited[16];
		int nvisited = 0;
		navQuery_->moveAlongSurface(polys_[0], iterPos, moveTgt, &filter_,result, visited, &nvisited, 16);

		npoly_ = FixupCorridor(visited, nvisited);
		npoly_ = FixupShortcuts();

		float h = 0;
		navQuery_->getPolyHeight(polys_[0], result, &h);
		result[1] = h;
		dtVcopy(iterPos, result);

		// Handle end of path and off-mesh links when close enough.
		if (endOfPath && inRange(iterPos, steerPos, SLOP, 1.0f))
		{
			// Reached end of path.
			dtVcopy(iterPos, targetPos);
			if (nsmooth_ < MAX_SMOOTH)
			{
				dtVcopy(&smooth_[nsmooth_*3], iterPos);
				nsmooth_++;
			}
			break;
		}
		else if (offMeshConnection && inRange(iterPos, steerPos, SLOP, 1.0f))
		{
			// Reached off-mesh connection.
			float startPos[3], endPos[3];

			// Advance the path up to and over the off-mesh connection.
			dtPolyRef prevRef = 0, polyRef = polys_[0];
			int npos = 0;
			while (npos < npoly_ && polyRef != steerPosRef)
			{
				prevRef = polyRef;
				polyRef = polys_[npos];
				npos++;
			}
			for (int i = npos; i < npoly_; ++i)
				polys_[i-npos] = polys_[i];
			npoly_ -= npos;

			// Handle the connection.
			dtStatus status = navMesh_->getOffMeshConnectionPolyEndPoints(prevRef, polyRef, startPos, endPos);
			if (dtStatusSucceed(status))
			{
				if (nsmooth_ < MAX_SMOOTH)
				{
					dtVcopy(&smooth_[nsmooth_*3], startPos);
					nsmooth_++;
					// Hack to make the dotted path not visible during off-mesh connection.
					if (nsmooth_ & 1)
					{
						dtVcopy(&smooth_[nsmooth_*3], startPos);
						nsmooth_++;
					}
				}
				// Move position at the other side of the off-mesh link.
				dtVcopy(iterPos, endPos);
				float eh = 0.0f;
				navQuery_->getPolyHeight(polys_[0], iterPos, &eh);
				iterPos[1] = eh;
			}
		}

		// Store results.
		if (nsmooth_ < MAX_SMOOTH)
		{
			dtVcopy(&smooth_[nsmooth_*3], iterPos);
			nsmooth_++;
		}
	}
	return true;
}


bool SceneMesh::RollPathInRound(COM_FPosition start, COM_FPosition center, float radius,std::vector<COM_FPosition>& outpath){
	const float _2PI = 2*3.1415926F;
	float theta = UtlMath::frandNM(0,_2PI);
	center.x_ += UtlMath::cos(theta) * radius;
	center.z_ += UtlMath::sin(theta) * radius;
	center.x_ = -center.x_;
	return FindPath(start,center,outpath);
}

bool SceneMesh::TestPath(COM_FPosition start, COM_FPosition end){
	if(NULL == navMesh_)
		return false;
	ResetFindPathContext(start,end);
	return PrepareFindPath() && FindPathPloys();
}

bool SceneMesh::FindPath(COM_FPosition start, COM_FPosition end, std::vector<COM_FPosition>& outpath,float length){

	if(!TestPath(start,end))
		return false;
	//找索引 找面 做平滑
	if (CalcSmoothPath())
	{
		if(UtlMath::feq(length,0.F)){
			COM_FPosition pos;
			for(int i =nsmooth_-1; i>=0; --i){
				pos.x_ = smooth_[i*3];
				pos.z_ = smooth_[i*3+2];
				outpath.push_back(pos);
			}
			
		}
		else 
		{
			COM_FPosition pos,tmp,lastpos = end;
			int i = nsmooth_-1;
			
			while(i>=0){
				pos.x_ = smooth_[i*3];
				pos.z_ = smooth_[i*3+2];
				tmp = Sub(lastpos,pos);
				length -= Length(tmp);
				lastpos = pos;
				--i;
				if(length <= 0.F) break;
			}
			
			if(length < 0.F){
				Normal(tmp);
				tmp = Scale(tmp,-length);
				pos = Add(pos,tmp);
				outpath.push_back(pos);
			}

			while(i>=0){
				pos.x_ = smooth_[i*3];
				pos.z_ = smooth_[i*3+2];
				outpath.push_back(pos);
				--i;
			}

			if(outpath.size() == 1)
				return true;
		}
		if(!outpath.empty())
			outpath.pop_back();
		return true;
	}
	return false;
}
