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

#ifndef DETOURLOCALBOUNDARY_H
#define DETOURLOCALBOUNDARY_H

#include "DetourNavMeshQuery.h"


class dtLocalBoundary
{
	static const int MAX_LOCAL_SEGS = 8;
	static const int MAX_LOCAL_POLYS = 16;
	
	struct Segment
	{
		float s[6];	///< Segment start/end
		float d;	///< Distance for pruning.
	};
	
	float center_[3];
	Segment segs_[MAX_LOCAL_SEGS];
	int nsegs_;
	
	dtPolyRef polys_[MAX_LOCAL_POLYS];
	int npolys_;

	void addSegment(const float dist, const float* s);
	
public:
	dtLocalBoundary();
	~dtLocalBoundary();
	
	void reset();
	
	void update(dtPolyRef ref, const float* pos, const float collisionQueryRange,
				NavMeshQuery* navquery, const dtQueryFilter* filter);
	
	bool isValid(NavMeshQuery* navquery, const dtQueryFilter* filter);
	
	inline const float* getCenter() const { return center_; }
	inline int getSegmentCount() const { return nsegs_; }
	inline const float* getSegment(int i) const { return segs_[i].s; }
};

#endif // DETOURLOCALBOUNDARY_H
