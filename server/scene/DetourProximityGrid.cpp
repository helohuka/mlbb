//
// Copyright (c) 2009-2010 Mikko Mononen memon@inside.org
//
// This software is provided 'as-is', without any express or implied
// warranty.  In no event will the authors be held liable for any damages
// arising from the use of this software.
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it
// freely, subject to the following restrictions:
// 1. The origin of this software must not be misrepresented; you must not
//    claim that you wrote the original software. If you use this software
//    in a product, an acknowledgment in the product documentation would be
//    appreciated but is not required.
// 2. Altered source versions must be plainly marked as such, and must not be
//    misrepresented as being the original software.
// 3. This notice may not be removed or altered from any source distribution.
//

#include "config.h"
#include "DetourProximityGrid.h"
#include "DetourCommon.h"
#include "DetourMath.h"
#include "DetourAlloc.h"


dtProximityGrid* dtAllocProximityGrid()
{
	void* mem = dtAlloc(sizeof(dtProximityGrid), DT_ALLOC_PERM);
	if (!mem) return 0;
	return new(mem) dtProximityGrid;
}

void dtFreeProximityGrid(dtProximityGrid* ptr)
{
	if (!ptr) return;
	ptr->~dtProximityGrid();
	dtFree(ptr);
}


inline int hashPos2(int x, int y, int n)
{
	return ((x*73856093) ^ (y*19349663)) & (n-1);
}


dtProximityGrid::dtProximityGrid() :
	cellSize_(0),
	pool_(0),
	poolHead_(0),
	poolSize_(0),
	buckets_(0),
	bucketsSize_(0)
{
}

dtProximityGrid::~dtProximityGrid()
{
	dtFree(buckets_);
	dtFree(pool_);
}

bool dtProximityGrid::init(const int poolSize, const float cellSize)
{
	SRV_ASSERT(poolSize > 0);
	SRV_ASSERT(cellSize > 0.0f);
	
	cellSize_ = cellSize;
	invCellSize_ = 1.0f / cellSize_;
	
	// Allocate hashs buckets
	bucketsSize_ = dtNextPow2(poolSize);
	buckets_ = (unsigned short*)dtAlloc(sizeof(unsigned short)*bucketsSize_, DT_ALLOC_PERM);
	if (!buckets_)
		return false;
	
	// Allocate pool of items.
	poolSize_ = poolSize;
	poolHead_ = 0;
	pool_ = (Item*)dtAlloc(sizeof(Item)*poolSize_, DT_ALLOC_PERM);
	if (!pool_)
		return false;
	
	clear();
	
	return true;
}

void dtProximityGrid::clear()
{
	memset(buckets_, 0xff, sizeof(unsigned short)*bucketsSize_);
	poolHead_ = 0;
	bounds_[0] = 0xffff;
	bounds_[1] = 0xffff;
	bounds_[2] = -0xffff;
	bounds_[3] = -0xffff;
}

void dtProximityGrid::addItem(const unsigned short id,
							  const float minx, const float miny,
							  const float maxx, const float maxy)
{
	const int iminx = (int)dtMathFloorf(minx * invCellSize_);
	const int iminy = (int)dtMathFloorf(miny * invCellSize_);
	const int imaxx = (int)dtMathFloorf(maxx * invCellSize_);
	const int imaxy = (int)dtMathFloorf(maxy * invCellSize_);
	
	bounds_[0] = dtMin(bounds_[0], iminx);
	bounds_[1] = dtMin(bounds_[1], iminy);
	bounds_[2] = dtMax(bounds_[2], imaxx);
	bounds_[3] = dtMax(bounds_[3], imaxy);
	
	for (int y = iminy; y <= imaxy; ++y)
	{
		for (int x = iminx; x <= imaxx; ++x)
		{
			if (poolHead_ < poolSize_)
			{
				const int h = hashPos2(x, y, bucketsSize_);
				const unsigned short idx = (unsigned short)poolHead_;
				poolHead_++;
				Item& item = pool_[idx];
				item.x = (short)x;
				item.y = (short)y;
				item.id = id;
				item.next = buckets_[h];
				buckets_[h] = idx;
			}
		}
	}
}

int dtProximityGrid::queryItems(const float minx, const float miny,
								const float maxx, const float maxy,
								unsigned short* ids, const int maxIds) const
{
	const int iminx = (int)dtMathFloorf(minx * invCellSize_);
	const int iminy = (int)dtMathFloorf(miny * invCellSize_);
	const int imaxx = (int)dtMathFloorf(maxx * invCellSize_);
	const int imaxy = (int)dtMathFloorf(maxy * invCellSize_);
	
	int n = 0;
	
	for (int y = iminy; y <= imaxy; ++y)
	{
		for (int x = iminx; x <= imaxx; ++x)
		{
			const int h = hashPos2(x, y, bucketsSize_);
			unsigned short idx = buckets_[h];
			while (idx != 0xffff)
			{
				Item& item = pool_[idx];
				if ((int)item.x == x && (int)item.y == y)
				{
					// Check if the id exists already.
					const unsigned short* end = ids + n;
					unsigned short* i = ids;
					while (i != end && *i != item.id)
						++i;
					// Item not found, add it.
					if (i == end)
					{
						if (n >= maxIds)
							return n;
						ids[n++] = item.id;
					}
				}
				idx = item.next;
			}
		}
	}
	
	return n;
}

int dtProximityGrid::getItemCountAt(const int x, const int y) const
{
	int n = 0;
	
	const int h = hashPos2(x, y, bucketsSize_);
	unsigned short idx = buckets_[h];
	while (idx != 0xffff)
	{
		Item& item = pool_[idx];
		if ((int)item.x == x && (int)item.y == y)
			n++;
		idx = item.next;
	}
	
	return n;
}
