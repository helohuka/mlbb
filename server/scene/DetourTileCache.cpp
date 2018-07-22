#include "config.h"
#include "DetourTileCache.h"
#include "DetourTileCacheBuilder.h"
#include "DetourNavMeshBuilder.h"
#include "DetourNavMesh.h"
#include "DetourCommon.h"
#include "DetourMath.h"
#include "DetourAlloc.h"


dtTileCache* dtAllocTileCache()
{
	void* mem = dtAlloc(sizeof(dtTileCache), DT_ALLOC_PERM);
	if (!mem) return 0;
	return new(mem) dtTileCache;
}

void dtFreeTileCache(dtTileCache* tc)
{
	if (!tc) return;
	tc->~dtTileCache();
	dtFree(tc);
}

static bool contains(const dtCompressedTileRef* a, const int n, const dtCompressedTileRef v)
{
	for (int i = 0; i < n; ++i)
		if (a[i] == v)
			return true;
	return false;
}

inline int computeTileHash(int x, int y, const int mask)
{
	const unsigned int h1 = 0x8da6b343; // Large multiplicative constants;
	const unsigned int h2 = 0xd8163841; // here arbitrarily chosen primes
	unsigned int n = h1 * x + h2 * y;
	return (int)(n & mask);
}


struct BuildContext
{
	inline BuildContext(struct dtTileCacheAlloc* a) : layer(0), lcset(0), lmesh(0), alloc(a) {}
	inline ~BuildContext() { purge(); }
	void purge()
	{
		dtFreeTileCacheLayer(alloc, layer);
		layer = 0;
		dtFreeTileCacheContourSet(alloc, lcset);
		lcset = 0;
		dtFreeTileCachePolyMesh(alloc, lmesh);
		lmesh = 0;
	}
	struct dtTileCacheLayer* layer;
	struct dtTileCacheContourSet* lcset;
	struct dtTileCachePolyMesh* lmesh;
	struct dtTileCacheAlloc* alloc;
};


dtTileCache::dtTileCache() :
	tileLutSize_(0),
	tileLutMask_(0),
	posLookup_(0),
	nextFreeTile_(0),	
	tiles_(0),	
	saltBits_(0),
	tileBits_(0),
	talloc_(0),
	tcomp_(0),
	tmproc_(0),
	obstacles_(0),
	nextFreeObstacle_(0),
	nreqs_(0),
	nupdate_(0)
{
	memset(&params_, 0, sizeof(params_));
}
	
dtTileCache::~dtTileCache()
{
	for (int i = 0; i < params_.maxTiles; ++i)
	{
		if (tiles_[i].flags & DT_COMPRESSEDTILE_FREE_DATA)
		{
			dtFree(tiles_[i].data);
			tiles_[i].data = 0;
		}
	}
	dtFree(obstacles_);
	obstacles_ = 0;
	dtFree(posLookup_);
	posLookup_ = 0;
	dtFree(tiles_);
	tiles_ = 0;
	nreqs_ = 0;
	nupdate_ = 0;
}

const dtCompressedTile* dtTileCache::getTileByRef(dtCompressedTileRef ref) const
{
	if (!ref)
		return 0;
	unsigned int tileIndex = decodeTileIdTile(ref);
	unsigned int tileSalt = decodeTileIdSalt(ref);
	if ((int)tileIndex >= params_.maxTiles)
		return 0;
	const dtCompressedTile* tile = &tiles_[tileIndex];
	if (tile->salt != tileSalt)
		return 0;
	return tile;
}


dtStatus dtTileCache::init(const dtTileCacheParams* params,
						   dtTileCacheAlloc* talloc,
						   dtTileCacheCompressor* tcomp,
						   dtTileCacheMeshProcess* tmproc)
{
	talloc_ = talloc;
	tcomp_ = tcomp;
	tmproc_ = tmproc;
	nreqs_ = 0;
	memcpy(&params_, params, sizeof(params_));
	
	// Alloc space for obstacles.
	obstacles_ = (dtTileCacheObstacle*)dtAlloc(sizeof(dtTileCacheObstacle)*params_.maxObstacles, DT_ALLOC_PERM);
	if (!obstacles_)
		return DT_FAILURE | DT_OUT_OF_MEMORY;
	memset(obstacles_, 0, sizeof(dtTileCacheObstacle)*params_.maxObstacles);
	nextFreeObstacle_ = 0;
	for (int i = params_.maxObstacles-1; i >= 0; --i)
	{
		obstacles_[i].salt = 1;
		obstacles_[i].next = nextFreeObstacle_;
		nextFreeObstacle_ = &obstacles_[i];
	}
	
	// Init tiles
	tileLutSize_ = dtNextPow2(params_.maxTiles/4);
	if (!tileLutSize_) tileLutSize_ = 1;
	tileLutMask_ = tileLutSize_-1;
	
	tiles_ = (dtCompressedTile*)dtAlloc(sizeof(dtCompressedTile)*params_.maxTiles, DT_ALLOC_PERM);
	if (!tiles_)
		return DT_FAILURE | DT_OUT_OF_MEMORY;
	posLookup_ = (dtCompressedTile**)dtAlloc(sizeof(dtCompressedTile*)*tileLutSize_, DT_ALLOC_PERM);
	if (!posLookup_)
		return DT_FAILURE | DT_OUT_OF_MEMORY;
	memset(tiles_, 0, sizeof(dtCompressedTile)*params_.maxTiles);
	memset(posLookup_, 0, sizeof(dtCompressedTile*)*tileLutSize_);
	nextFreeTile_ = 0;
	for (int i = params_.maxTiles-1; i >= 0; --i)
	{
		tiles_[i].salt = 1;
		tiles_[i].next = nextFreeTile_;
		nextFreeTile_ = &tiles_[i];
	}
	
	// Init ID generator values.
	tileBits_ = dtIlog2(dtNextPow2((unsigned int)params_.maxTiles));
	// Only allow 31 salt bits, since the salt mask is calculated using 32bit uint and it will overflow.
	saltBits_ = dtMin((unsigned int)31, 32 - tileBits_);
	if (saltBits_ < 10)
		return DT_FAILURE | DT_INVALID_PARAM;
	
	return DT_SUCCESS;
}

int dtTileCache::getTilesAt(const int tx, const int ty, dtCompressedTileRef* tiles, const int maxTiles) const 
{
	int n = 0;
	
	// Find tile based on hash.
	int h = computeTileHash(tx,ty,tileLutMask_);
	dtCompressedTile* tile = posLookup_[h];
	while (tile)
	{
		if (tile->header &&
			tile->header->tx == tx &&
			tile->header->ty == ty)
		{
			if (n < maxTiles)
				tiles[n++] = getTileRef(tile);
		}
		tile = tile->next;
	}
	
	return n;
}

dtCompressedTile* dtTileCache::getTileAt(const int tx, const int ty, const int tlayer)
{
	// Find tile based on hash.
	int h = computeTileHash(tx,ty,tileLutMask_);
	dtCompressedTile* tile = posLookup_[h];
	while (tile)
	{
		if (tile->header &&
			tile->header->tx == tx &&
			tile->header->ty == ty &&
			tile->header->tlayer == tlayer)
		{
			return tile;
		}
		tile = tile->next;
	}
	return 0;
}

dtCompressedTileRef dtTileCache::getTileRef(const dtCompressedTile* tile) const
{
	if (!tile) return 0;
	const unsigned int it = (unsigned int)(tile - tiles_);
	return (dtCompressedTileRef)encodeTileId(tile->salt, it);
}

dtObstacleRef dtTileCache::getObstacleRef(const dtTileCacheObstacle* ob) const
{
	if (!ob) return 0;
	const unsigned int idx = (unsigned int)(ob - obstacles_);
	return encodeObstacleId(ob->salt, idx);
}

const dtTileCacheObstacle* dtTileCache::getObstacleByRef(dtObstacleRef ref)
{
	if (!ref)
		return 0;
	unsigned int idx = decodeObstacleIdObstacle(ref);
	if ((int)idx >= params_.maxObstacles)
		return 0;
	const dtTileCacheObstacle* ob = &obstacles_[idx];
	unsigned int salt = decodeObstacleIdSalt(ref);
	if (ob->salt != salt)
		return 0;
	return ob;
}

dtStatus dtTileCache::addTile(unsigned char* data, const int dataSize, unsigned char flags, dtCompressedTileRef* result)
{
	// Make sure the data is in right format.
	dtTileCacheLayerHeader* header = (dtTileCacheLayerHeader*)data;
	if (header->magic != DT_TILECACHE_MAGIC)
		return DT_FAILURE | DT_WRONG_MAGIC;
	if (header->version != DT_TILECACHE_VERSION)
		return DT_FAILURE | DT_WRONG_VERSION;
	
	// Make sure the location is free.
	if (getTileAt(header->tx, header->ty, header->tlayer))
		return DT_FAILURE;
	
	// Allocate a tile.
	dtCompressedTile* tile = 0;
	if (nextFreeTile_)
	{
		tile = nextFreeTile_;
		nextFreeTile_ = tile->next;
		tile->next = 0;
	}
	
	// Make sure we could allocate a tile.
	if (!tile)
		return DT_FAILURE | DT_OUT_OF_MEMORY;
	
	// Insert tile into the position lut.
	int h = computeTileHash(header->tx, header->ty, tileLutMask_);
	tile->next = posLookup_[h];
	posLookup_[h] = tile;
	
	// Init tile.
	const int headerSize = dtAlign4(sizeof(dtTileCacheLayerHeader));
	tile->header = (dtTileCacheLayerHeader*)data;
	tile->data = data;
	tile->dataSize = dataSize;
	tile->compressed = tile->data + headerSize;
	tile->compressedSize = tile->dataSize - headerSize;
	tile->flags = flags;
	
	if (result)
		*result = getTileRef(tile);
	
	return DT_SUCCESS;
}

dtStatus dtTileCache::removeTile(dtCompressedTileRef ref, unsigned char** data, int* dataSize)
{
	if (!ref)
		return DT_FAILURE | DT_INVALID_PARAM;
	unsigned int tileIndex = decodeTileIdTile(ref);
	unsigned int tileSalt = decodeTileIdSalt(ref);
	if ((int)tileIndex >= params_.maxTiles)
		return DT_FAILURE | DT_INVALID_PARAM;
	dtCompressedTile* tile = &tiles_[tileIndex];
	if (tile->salt != tileSalt)
		return DT_FAILURE | DT_INVALID_PARAM;
	
	// Remove tile from hash lookup.
	const int h = computeTileHash(tile->header->tx,tile->header->ty,tileLutMask_);
	dtCompressedTile* prev = 0;
	dtCompressedTile* cur = posLookup_[h];
	while (cur)
	{
		if (cur == tile)
		{
			if (prev)
				prev->next = cur->next;
			else
				posLookup_[h] = cur->next;
			break;
		}
		prev = cur;
		cur = cur->next;
	}
	
	// Reset tile.
	if (tile->flags & DT_COMPRESSEDTILE_FREE_DATA)
	{
		// Owns data
		dtFree(tile->data);
		tile->data = 0;
		tile->dataSize = 0;
		if (data) *data = 0;
		if (dataSize) *dataSize = 0;
	}
	else
	{
		if (data) *data = tile->data;
		if (dataSize) *dataSize = tile->dataSize;
	}
	
	tile->header = 0;
	tile->data = 0;
	tile->dataSize = 0;
	tile->compressed = 0;
	tile->compressedSize = 0;
	tile->flags = 0;
	
	// Update salt, salt should never be zero.
	tile->salt = (tile->salt+1) & ((1<<saltBits_)-1);
	if (tile->salt == 0)
		tile->salt++;
	
	// Add to free list.
	tile->next = nextFreeTile_;
	nextFreeTile_ = tile;
	
	return DT_SUCCESS;
}


dtObstacleRef dtTileCache::addObstacle(const float* pos, const float radius, const float height, dtObstacleRef* result)
{
	if (nreqs_ >= MAX_REQUESTS)
		return DT_FAILURE | DT_BUFFER_TOO_SMALL;
	
	dtTileCacheObstacle* ob = 0;
	if (nextFreeObstacle_)
	{
		ob = nextFreeObstacle_;
		nextFreeObstacle_ = ob->next;
		ob->next = 0;
	}
	if (!ob)
		return DT_FAILURE | DT_OUT_OF_MEMORY;
	
	unsigned short salt = ob->salt;
	memset(ob, 0, sizeof(dtTileCacheObstacle));
	ob->salt = salt;
	ob->state = DT_OBSTACLE_PROCESSING;
	dtVcopy(ob->pos, pos);
	ob->radius = radius;
	ob->height = height;
	
	ObstacleRequest* req = &reqs_[nreqs_++];
	memset(req, 0, sizeof(ObstacleRequest));
	req->action = REQUEST_ADD;
	req->ref = getObstacleRef(ob);
	
	if (result)
		*result = req->ref;
	
	return DT_SUCCESS;
}

dtObstacleRef dtTileCache::removeObstacle(const dtObstacleRef ref)
{
	if (!ref)
		return DT_SUCCESS;
	if (nreqs_ >= MAX_REQUESTS)
		return DT_FAILURE | DT_BUFFER_TOO_SMALL;
	
	ObstacleRequest* req = &reqs_[nreqs_++];
	memset(req, 0, sizeof(ObstacleRequest));
	req->action = REQUEST_REMOVE;
	req->ref = ref;
	
	return DT_SUCCESS;
}

dtStatus dtTileCache::queryTiles(const float* bmin, const float* bmax,
								 dtCompressedTileRef* results, int* resultCount, const int maxResults) const 
{
	const int MAX_TILES = 32;
	dtCompressedTileRef tiles[MAX_TILES];
	
	int n = 0;
	
	const float tw = params_.width * params_.cs;
	const float th = params_.height * params_.cs;
	const int tx0 = (int)dtMathFloorf((bmin[0]-params_.orig[0]) / tw);
	const int tx1 = (int)dtMathFloorf((bmax[0]-params_.orig[0]) / tw);
	const int ty0 = (int)dtMathFloorf((bmin[2]-params_.orig[2]) / th);
	const int ty1 = (int)dtMathFloorf((bmax[2]-params_.orig[2]) / th);
	
	for (int ty = ty0; ty <= ty1; ++ty)
	{
		for (int tx = tx0; tx <= tx1; ++tx)
		{
			const int ntiles = getTilesAt(tx,ty,tiles,MAX_TILES);
			
			for (int i = 0; i < ntiles; ++i)
			{
				const dtCompressedTile* tile = &tiles_[decodeTileIdTile(tiles[i])];
				float tbmin[3], tbmax[3];
				calcTightTileBounds(tile->header, tbmin, tbmax);
				
				if (dtOverlapBounds(bmin,bmax, tbmin,tbmax))
				{
					if (n < maxResults)
						results[n++] = tiles[i];
				}
			}
		}
	}
	
	*resultCount = n;
	
	return DT_SUCCESS;
}

dtStatus dtTileCache::update(const float /*dt*/, dtNavMesh* navmesh)
{
	if (nupdate_ == 0)
	{
		// Process requests.
		for (int i = 0; i < nreqs_; ++i)
		{
			ObstacleRequest* req = &reqs_[i];
			
			unsigned int idx = decodeObstacleIdObstacle(req->ref);
			if ((int)idx >= params_.maxObstacles)
				continue;
			dtTileCacheObstacle* ob = &obstacles_[idx];
			unsigned int salt = decodeObstacleIdSalt(req->ref);
			if (ob->salt != salt)
				continue;
			
			if (req->action == REQUEST_ADD)
			{
				// Find touched tiles.
				float bmin[3], bmax[3];
				getObstacleBounds(ob, bmin, bmax);

				int ntouched = 0;
				queryTiles(bmin, bmax, ob->touched, &ntouched, DT_MAX_TOUCHED_TILES);
				ob->ntouched = (unsigned char)ntouched;
				// Add tiles to update list.
				ob->npending = 0;
				for (int j = 0; j < ob->ntouched; ++j)
				{
					if (nupdate_ < MAX_UPDATE)
					{
						if (!contains(update_, nupdate_, ob->touched[j]))
							update_[nupdate_++] = ob->touched[j];
						ob->pending[ob->npending++] = ob->touched[j];
					}
				}
			}
			else if (req->action == REQUEST_REMOVE)
			{
				// Prepare to remove obstacle.
				ob->state = DT_OBSTACLE_REMOVING;
				// Add tiles to update list.
				ob->npending = 0;
				for (int j = 0; j < ob->ntouched; ++j)
				{
					if (nupdate_ < MAX_UPDATE)
					{
						if (!contains(update_, nupdate_, ob->touched[j]))
							update_[nupdate_++] = ob->touched[j];
						ob->pending[ob->npending++] = ob->touched[j];
					}
				}
			}
		}
		
		nreqs_ = 0;
	}
	
	// Process updates
	if (nupdate_)
	{
		// Build mesh
		const dtCompressedTileRef ref = update_[0];
		dtStatus status = buildNavMeshTile(ref, navmesh);
		nupdate_--;
		if (nupdate_ > 0)
			memmove(update_, update_+1, nupdate_*sizeof(dtCompressedTileRef));

		// Update obstacle states.
		for (int i = 0; i < params_.maxObstacles; ++i)
		{
			dtTileCacheObstacle* ob = &obstacles_[i];
			if (ob->state == DT_OBSTACLE_PROCESSING || ob->state == DT_OBSTACLE_REMOVING)
			{
				// Remove handled tile from pending list.
				for (int j = 0; j < (int)ob->npending; j++)
				{
					if (ob->pending[j] == ref)
					{
						ob->pending[j] = ob->pending[(int)ob->npending-1];
						ob->npending--;
						break;
					}
				}
				
				// If all pending tiles processed, change state.
				if (ob->npending == 0)
				{
					if (ob->state == DT_OBSTACLE_PROCESSING)
					{
						ob->state = DT_OBSTACLE_PROCESSED;
					}
					else if (ob->state == DT_OBSTACLE_REMOVING)
					{
						ob->state = DT_OBSTACLE_EMPTY;
						// Update salt, salt should never be zero.
						ob->salt = (ob->salt+1) & ((1<<16)-1);
						if (ob->salt == 0)
							ob->salt++;
						// Return obstacle to free list.
						ob->next = nextFreeObstacle_;
						nextFreeObstacle_ = ob;
					}
				}
			}
		}
			
		if (dtStatusFailed(status))
			return status;
	}
	
	return DT_SUCCESS;
}


dtStatus dtTileCache::buildNavMeshTilesAt(const int tx, const int ty, dtNavMesh* navmesh)
{
	const int MAX_TILES = 32;
	dtCompressedTileRef tiles[MAX_TILES];
	const int ntiles = getTilesAt(tx,ty,tiles,MAX_TILES);
	
	for (int i = 0; i < ntiles; ++i)
	{
		dtStatus status = buildNavMeshTile(tiles[i], navmesh);
		if (dtStatusFailed(status))
			return status;
	}
	
	return DT_SUCCESS;
}

dtStatus dtTileCache::buildNavMeshTile(const dtCompressedTileRef ref, dtNavMesh* navmesh)
{	
	SRV_ASSERT(talloc_);
	SRV_ASSERT(tcomp_);
	
	unsigned int idx = decodeTileIdTile(ref);
	if (idx > (unsigned int)params_.maxTiles)
		return DT_FAILURE | DT_INVALID_PARAM;
	const dtCompressedTile* tile = &tiles_[idx];
	unsigned int salt = decodeTileIdSalt(ref);
	if (tile->salt != salt)
		return DT_FAILURE | DT_INVALID_PARAM;
	
	talloc_->reset();
	
	BuildContext bc(talloc_);
	const int walkableClimbVx = (int)(params_.walkableClimb / params_.ch);
	dtStatus status;
	
	// Decompress tile layer data. 
	status = dtDecompressTileCacheLayer(talloc_, tcomp_, tile->data, tile->dataSize, &bc.layer);
	if (dtStatusFailed(status))
		return status;
	
	// Rasterize obstacles.
	for (int i = 0; i < params_.maxObstacles; ++i)
	{
		const dtTileCacheObstacle* ob = &obstacles_[i];
		if (ob->state == DT_OBSTACLE_EMPTY || ob->state == DT_OBSTACLE_REMOVING)
			continue;
		if (contains(ob->touched, ob->ntouched, ref))
		{
			dtMarkCylinderArea(*bc.layer, tile->header->bmin, params_.cs, params_.ch,
							   ob->pos, ob->radius, ob->height, 0);
		}
	}
	
	// Build navmesh
	status = dtBuildTileCacheRegions(talloc_, *bc.layer, walkableClimbVx);
	if (dtStatusFailed(status))
		return status;
	
	bc.lcset = dtAllocTileCacheContourSet(talloc_);
	if (!bc.lcset)
		return status;
	status = dtBuildTileCacheContours(talloc_, *bc.layer, walkableClimbVx,
									  params_.maxSimplificationError, *bc.lcset);
	if (dtStatusFailed(status))
		return status;
	
	bc.lmesh = dtAllocTileCachePolyMesh(talloc_);
	if (!bc.lmesh)
		return status;
	status = dtBuildTileCachePolyMesh(talloc_, *bc.lcset, *bc.lmesh);
	if (dtStatusFailed(status))
		return status;
	
	// Early out if the mesh tile is empty.
	if (!bc.lmesh->npolys)
		return DT_SUCCESS;
	
	NavMeshCreateParams params;
	memset(&params, 0, sizeof(params));
	params.verts = bc.lmesh->verts;
	params.vertCount = bc.lmesh->nverts;
	params.polys = bc.lmesh->polys;
	params.polyAreas = bc.lmesh->areas;
	params.polyFlags = bc.lmesh->flags;
	params.polyCount = bc.lmesh->npolys;
	params.nvp = DT_VERTS_PER_POLYGON;
	params.walkableHeight = params_.walkableHeight;
	params.walkableRadius = params_.walkableRadius;
	params.walkableClimb = params_.walkableClimb;
	params.tileX = tile->header->tx;
	params.tileY = tile->header->ty;
	params.tileLayer = tile->header->tlayer;
	params.cs = params_.cs;
	params.ch = params_.ch;
	params.buildBvTree = false;
	dtVcopy(params.bmin, tile->header->bmin);
	dtVcopy(params.bmax, tile->header->bmax);
	
	if (tmproc_)
	{
		tmproc_->process(&params, bc.lmesh->areas, bc.lmesh->flags);
	}
	
	unsigned char* navData = 0;
	int navDataSize = 0;
	if (!CreateNavMeshData(&params, &navData, &navDataSize))
		return DT_FAILURE;

	// Remove existing tile.
	navmesh->removeTile(navmesh->getTileRefAt(tile->header->tx,tile->header->ty,tile->header->tlayer),0,0);

	// Add new tile, or leave the location empty.
	if (navData)
	{
		// Let the navmesh own the data.
		status = navmesh->addTile(navData,navDataSize,DT_TILE_FREE_DATA,0,0);
		if (dtStatusFailed(status))
		{
			dtFree(navData);
			return status;
		}
	}
	
	return DT_SUCCESS;
}

void dtTileCache::calcTightTileBounds(const dtTileCacheLayerHeader* header, float* bmin, float* bmax) const
{
	const float cs = params_.cs;
	bmin[0] = header->bmin[0] + header->minx*cs;
	bmin[1] = header->bmin[1];
	bmin[2] = header->bmin[2] + header->miny*cs;
	bmax[0] = header->bmin[0] + (header->maxx+1)*cs;
	bmax[1] = header->bmax[1];
	bmax[2] = header->bmin[2] + (header->maxy+1)*cs;
}

void dtTileCache::getObstacleBounds(const struct dtTileCacheObstacle* ob, float* bmin, float* bmax) const
{
	bmin[0] = ob->pos[0] - ob->radius;
	bmin[1] = ob->pos[1];
	bmin[2] = ob->pos[2] - ob->radius;
	bmax[0] = ob->pos[0] + ob->radius;
	bmax[1] = ob->pos[1] + ob->height;
	bmax[2] = ob->pos[2] + ob->radius;	
}
