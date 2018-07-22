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
#include "DetourLocalBoundary.h"
#include "DetourNavMeshQuery.h"
#include "DetourCommon.h"


dtLocalBoundary::dtLocalBoundary() :
	nsegs_(0),
	npolys_(0)
{
	dtVset(center_, FLT_MAX,FLT_MAX,FLT_MAX);
}

dtLocalBoundary::~dtLocalBoundary()
{
}

void dtLocalBoundary::reset()
{
	dtVset(center_, FLT_MAX,FLT_MAX,FLT_MAX);
	npolys_ = 0;
	nsegs_ = 0;
}

void dtLocalBoundary::addSegment(const float dist, const float* s)
{
	// Insert neighbour based on the distance.
	Segment* seg = 0;
	if (!nsegs_)
	{
		// First, trivial accept.
		seg = &segs_[0];
	}
	else if (dist >= segs_[nsegs_-1].d)
	{
		// Further than the last segment, skip.
		if (nsegs_ >= MAX_LOCAL_SEGS)
			return;
		// Last, trivial accept.
		seg = &segs_[nsegs_];
	}
	else
	{
		// Insert inbetween.
		int i;
		for (i = 0; i < nsegs_; ++i)
			if (dist <= segs_[i].d)
				break;
		const int tgt = i+1;
		const int n = dtMin(nsegs_-i, MAX_LOCAL_SEGS-tgt);
		SRV_ASSERT(tgt+n <= MAX_LOCAL_SEGS);
		if (n > 0)
			memmove(&segs_[tgt], &segs_[i], sizeof(Segment)*n);
		seg = &segs_[i];
	}
	
	seg->d = dist;
	memcpy(seg->s, s, sizeof(float)*6);
	
	if (nsegs_ < MAX_LOCAL_SEGS)
		nsegs_++;
}

void dtLocalBoundary::update(dtPolyRef ref, const float* pos, const float collisionQueryRange,
							 NavMeshQuery* navquery, const dtQueryFilter* filter)
{
	static const int MAX_SEGS_PER_POLY = DT_VERTS_PER_POLYGON*3;
	
	if (!ref)
	{
		dtVset(center_, FLT_MAX,FLT_MAX,FLT_MAX);
		nsegs_ = 0;
		npolys_ = 0;
		return;
	}
	
	dtVcopy(center_, pos);
	
	// First query non-overlapping polygons.
	navquery->findLocalNeighbourhood(ref, pos, collisionQueryRange,
									 filter, polys_, 0, &npolys_, MAX_LOCAL_POLYS);
	
	// Secondly, store all polygon edges.
	nsegs_ = 0;
	float segs[MAX_SEGS_PER_POLY*6];
	int nsegs = 0;
	for (int j = 0; j < npolys_; ++j)
	{
		navquery->getPolyWallSegments(polys_[j], filter, segs, 0, &nsegs, MAX_SEGS_PER_POLY);
		for (int k = 0; k < nsegs; ++k)
		{
			const float* s = &segs[k*6];
			// Skip too distant segments.
			float tseg;
			const float distSqr = dtDistancePtSegSqr2D(pos, s, s+3, tseg);
			if (distSqr > dtSqr(collisionQueryRange))
				continue;
			addSegment(distSqr, s);
		}
	}
}

bool dtLocalBoundary::isValid(NavMeshQuery* navquery, const dtQueryFilter* filter)
{
	if (!npolys_)
		return false;
	
	// Check that all polygons still pass query filter.
	for (int i = 0; i < npolys_; ++i)
	{
		if (!navquery->isValidPolyRef(polys_[i], filter))
			return false;
	}
	
	return true;
}

