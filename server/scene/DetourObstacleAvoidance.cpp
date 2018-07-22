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
#include "DetourObstacleAvoidance.h"
#include "DetourCommon.h"
#include "DetourMath.h"
#include "DetourAlloc.h"


static const float DT_PI = 3.14159265f;

static int sweepCircleCircle(const float* c0, const float r0, const float* v,
							 const float* c1, const float r1,
							 float& tmin, float& tmax)
{
	static const float EPS = 0.0001f;
	float s[3];
	dtVsub(s,c1,c0);
	float r = r0+r1;
	float c = dtVdot2D(s,s) - r*r;
	float a = dtVdot2D(v,v);
	if (a < EPS) return 0;	// not moving
	
	// Overlap, calc time to exit.
	float b = dtVdot2D(v,s);
	float d = b*b - a*c;
	if (d < 0.0f) return 0; // no intersection.
	a = 1.0f / a;
	const float rd = dtMathSqrtf(d);
	tmin = (b - rd) * a;
	tmax = (b + rd) * a;
	return 1;
}

static int isectRaySeg(const float* ap, const float* u,
					   const float* bp, const float* bq,
					   float& t)
{
	float v[3], w[3];
	dtVsub(v,bq,bp);
	dtVsub(w,ap,bp);
	float d = dtVperp2D(u,v);
	if (dtMathFabsf(d) < 1e-6f) return 0;
	d = 1.0f/d;
	t = dtVperp2D(v,w) * d;
	if (t < 0 || t > 1) return 0;
	float s = dtVperp2D(u,w) * d;
	if (s < 0 || s > 1) return 0;
	return 1;
}



dtObstacleAvoidanceDebugData* dtAllocObstacleAvoidanceDebugData()
{
	void* mem = dtAlloc(sizeof(dtObstacleAvoidanceDebugData), DT_ALLOC_PERM);
	if (!mem) return 0;
	return new(mem) dtObstacleAvoidanceDebugData;
}

void dtFreeObstacleAvoidanceDebugData(dtObstacleAvoidanceDebugData* ptr)
{
	if (!ptr) return;
	ptr->~dtObstacleAvoidanceDebugData();
	dtFree(ptr);
}


dtObstacleAvoidanceDebugData::dtObstacleAvoidanceDebugData() :
	nsamples_(0),
	maxSamples_(0),
	vel_(0),
	ssize_(0),
	pen_(0),
	vpen_(0),
	vcpen_(0),
	spen_(0),
	tpen_(0)
{
}

dtObstacleAvoidanceDebugData::~dtObstacleAvoidanceDebugData()
{
	dtFree(vel_);
	dtFree(ssize_);
	dtFree(pen_);
	dtFree(vpen_);
	dtFree(vcpen_);
	dtFree(spen_);
	dtFree(tpen_);
}
		
bool dtObstacleAvoidanceDebugData::init(const int maxSamples)
{
	SRV_ASSERT(maxSamples);
	maxSamples_ = maxSamples;

	vel_ = (float*)dtAlloc(sizeof(float)*3*maxSamples_, DT_ALLOC_PERM);
	if (!vel_)
		return false;
	pen_ = (float*)dtAlloc(sizeof(float)*maxSamples_, DT_ALLOC_PERM);
	if (!pen_)
		return false;
	ssize_ = (float*)dtAlloc(sizeof(float)*maxSamples_, DT_ALLOC_PERM);
	if (!ssize_)
		return false;
	vpen_ = (float*)dtAlloc(sizeof(float)*maxSamples_, DT_ALLOC_PERM);
	if (!vpen_)
		return false;
	vcpen_ = (float*)dtAlloc(sizeof(float)*maxSamples_, DT_ALLOC_PERM);
	if (!vcpen_)
		return false;
	spen_ = (float*)dtAlloc(sizeof(float)*maxSamples_, DT_ALLOC_PERM);
	if (!spen_)
		return false;
	tpen_ = (float*)dtAlloc(sizeof(float)*maxSamples_, DT_ALLOC_PERM);
	if (!tpen_)
		return false;
	
	return true;
}

void dtObstacleAvoidanceDebugData::reset()
{
	nsamples_ = 0;
}

void dtObstacleAvoidanceDebugData::addSample(const float* vel, const float ssize, const float pen,
											 const float vpen, const float vcpen, const float spen, const float tpen)
{
	if (nsamples_ >= maxSamples_)
		return;
	SRV_ASSERT(vel_);
	SRV_ASSERT(ssize_);
	SRV_ASSERT(pen_);
	SRV_ASSERT(vpen_);
	SRV_ASSERT(vcpen_);
	SRV_ASSERT(spen_);
	SRV_ASSERT(tpen_);
	dtVcopy(&vel_[nsamples_*3], vel);
	ssize_[nsamples_] = ssize;
	pen_[nsamples_] = pen;
	vpen_[nsamples_] = vpen;
	vcpen_[nsamples_] = vcpen;
	spen_[nsamples_] = spen;
	tpen_[nsamples_] = tpen;
	nsamples_++;
}

static void normalizeArray(float* arr, const int n)
{
	// Normalize penaly range.
	float minPen = FLT_MAX;
	float maxPen = -FLT_MAX;
	for (int i = 0; i < n; ++i)
	{
		minPen = dtMin(minPen, arr[i]);
		maxPen = dtMax(maxPen, arr[i]);
	}
	const float penRange = maxPen-minPen;
	const float s = penRange > 0.001f ? (1.0f / penRange) : 1;
	for (int i = 0; i < n; ++i)
		arr[i] = dtClamp((arr[i]-minPen)*s, 0.0f, 1.0f);
}

void dtObstacleAvoidanceDebugData::normalizeSamples()
{
	normalizeArray(pen_, nsamples_);
	normalizeArray(vpen_, nsamples_);
	normalizeArray(vcpen_, nsamples_);
	normalizeArray(spen_, nsamples_);
	normalizeArray(tpen_, nsamples_);
}


dtObstacleAvoidanceQuery* dtAllocObstacleAvoidanceQuery()
{
	void* mem = dtAlloc(sizeof(dtObstacleAvoidanceQuery), DT_ALLOC_PERM);
	if (!mem) return 0;
	return new(mem) dtObstacleAvoidanceQuery;
}

void dtFreeObstacleAvoidanceQuery(dtObstacleAvoidanceQuery* ptr)
{
	if (!ptr) return;
	ptr->~dtObstacleAvoidanceQuery();
	dtFree(ptr);
}


dtObstacleAvoidanceQuery::dtObstacleAvoidanceQuery() :
	maxCircles_(0),
	circles_(0),
	ncircles_(0),
	maxSegments_(0),
	segments_(0),
	nsegments_(0)
{
}

dtObstacleAvoidanceQuery::~dtObstacleAvoidanceQuery()
{
	dtFree(circles_);
	dtFree(segments_);
}

bool dtObstacleAvoidanceQuery::init(const int maxCircles, const int maxSegments)
{
	maxCircles_ = maxCircles;
	ncircles_ = 0;
	circles_ = (dtObstacleCircle*)dtAlloc(sizeof(dtObstacleCircle)*maxCircles_, DT_ALLOC_PERM);
	if (!circles_)
		return false;
	memset(circles_, 0, sizeof(dtObstacleCircle)*maxCircles_);

	maxSegments_ = maxSegments;
	nsegments_ = 0;
	segments_ = (dtObstacleSegment*)dtAlloc(sizeof(dtObstacleSegment)*maxSegments_, DT_ALLOC_PERM);
	if (!segments_)
		return false;
	memset(segments_, 0, sizeof(dtObstacleSegment)*maxSegments_);
	
	return true;
}

void dtObstacleAvoidanceQuery::reset()
{
	ncircles_ = 0;
	nsegments_ = 0;
}

void dtObstacleAvoidanceQuery::addCircle(const float* pos, const float rad,
										 const float* vel, const float* dvel)
{
	if (ncircles_ >= maxCircles_)
		return;
		
	dtObstacleCircle* cir = &circles_[ncircles_++];
	dtVcopy(cir->p, pos);
	cir->rad = rad;
	dtVcopy(cir->vel, vel);
	dtVcopy(cir->dvel, dvel);
}

void dtObstacleAvoidanceQuery::addSegment(const float* p, const float* q)
{
	if (nsegments_ >= maxSegments_)
		return;
	
	dtObstacleSegment* seg = &segments_[nsegments_++];
	dtVcopy(seg->p, p);
	dtVcopy(seg->q, q);
}

void dtObstacleAvoidanceQuery::prepare(const float* pos, const float* dvel)
{
	// Prepare obstacles
	for (int i = 0; i < ncircles_; ++i)
	{
		dtObstacleCircle* cir = &circles_[i];
		
		// Side
		const float* pa = pos;
		const float* pb = cir->p;
		
		const float orig[3] = {0,0,0};
		float dv[3];
		dtVsub(cir->dp,pb,pa);
		dtVnormalize(cir->dp);
		dtVsub(dv, cir->dvel, dvel);
		
		const float a = dtTriArea2D(orig, cir->dp,dv);
		if (a < 0.01f)
		{
			cir->np[0] = -cir->dp[2];
			cir->np[2] = cir->dp[0];
		}
		else
		{
			cir->np[0] = cir->dp[2];
			cir->np[2] = -cir->dp[0];
		}
	}	

	for (int i = 0; i < nsegments_; ++i)
	{
		dtObstacleSegment* seg = &segments_[i];
		
		// Precalc if the agent is really close to the segment.
		const float r = 0.01f;
		float t;
		seg->touch = dtDistancePtSegSqr2D(pos, seg->p, seg->q, t) < dtSqr(r);
	}	
}


/* Calculate the collision penalty for a given velocity vector
 * 
 * @param vcand sampled velocity
 * @param dvel desired velocity
 * @param minPenalty threshold penalty for early out
 */
float dtObstacleAvoidanceQuery::processSample(const float* vcand, const float cs,
											  const float* pos, const float rad,
											  const float* vel, const float* dvel,
											  const float minPenalty,
											  dtObstacleAvoidanceDebugData* debug)
{
	// penalty for straying away from the desired and current velocities
	const float vpen = params_.weightDesVel * (dtVdist2D(vcand, dvel) * invVmax_);
	const float vcpen = params_.weightCurVel * (dtVdist2D(vcand, vel) * invVmax_);

	// find the threshold hit time to bail out based on the early out penalty
	// (see how the penalty is calculated below to understnad)
	float minPen = minPenalty - vpen - vcpen;
	float tThresold = ((double)params_.weightToi/(double)minPen - 0.1) * (double)params_.horizTime;
	if (tThresold - params_.horizTime > -FLT_EPSILON)
		return minPenalty; // already too much

	// Find min time of impact and exit amongst all obstacles.
	float tmin = params_.horizTime;
	float side = 0;
	int nside = 0;
	
	for (int i = 0; i < ncircles_; ++i)
	{
		const dtObstacleCircle* cir = &circles_[i];
			
		// RVO
		float vab[3];
		dtVscale(vab, vcand, 2);
		dtVsub(vab, vab, vel);
		dtVsub(vab, vab, cir->vel);
		
		// Side
		side += dtClamp(dtMin(dtVdot2D(cir->dp,vab)*0.5f+0.5f, dtVdot2D(cir->np,vab)*2), 0.0f, 1.0f);
		nside++;
		
		float htmin = 0, htmax = 0;
		if (!sweepCircleCircle(pos,rad, vab, cir->p,cir->rad, htmin, htmax))
			continue;
		
		// Handle overlapping obstacles.
		if (htmin < 0.0f && htmax > 0.0f)
		{
			// Avoid more when overlapped.
			htmin = -htmin * 0.5f;
		}
		
		if (htmin >= 0.0f)
		{
			// The closest obstacle is somewhere ahead of us, keep track of nearest obstacle.
			if (htmin < tmin)
			{
				tmin = htmin;
				if (tmin < tThresold)
					return minPenalty;
			}
		}
	}

	for (int i = 0; i < nsegments_; ++i)
	{
		const dtObstacleSegment* seg = &segments_[i];
		float htmin = 0;
		
		if (seg->touch)
		{
			// Special case when the agent is very close to the segment.
			float sdir[3], snorm[3];
			dtVsub(sdir, seg->q, seg->p);
			snorm[0] = -sdir[2];
			snorm[2] = sdir[0];
			// If the velocity is pointing towards the segment, no collision.
			if (dtVdot2D(snorm, vcand) < 0.0f)
				continue;
			// Else immediate collision.
			htmin = 0.0f;
		}
		else
		{
			if (!isectRaySeg(pos, vcand, seg->p, seg->q, htmin))
				continue;
		}
		
		// Avoid less when facing walls.
		htmin *= 2.0f;
		
		// The closest obstacle is somewhere ahead of us, keep track of nearest obstacle.
		if (htmin < tmin)
		{
			tmin = htmin;
			if (tmin < tThresold)
				return minPenalty;
		}
	}
	
	// Normalize side bias, to prevent it dominating too much.
	if (nside)
		side /= nside;
	
	const float spen = params_.weightSide * side;
	const float tpen = params_.weightToi * (1.0f/(0.1f+tmin*invHorizTime_));
	
	const float penalty = vpen + vcpen + spen + tpen;
	
	// Store different penalties for debug viewing
	if (debug)
		debug->addSample(vcand, cs, penalty, vpen, vcpen, spen, tpen);
	
	return penalty;
}

int dtObstacleAvoidanceQuery::sampleVelocityGrid(const float* pos, const float rad, const float vmax,
												 const float* vel, const float* dvel, float* nvel,
												 const dtObstacleAvoidanceParams* params,
												 dtObstacleAvoidanceDebugData* debug)
{
	prepare(pos, dvel);
	
	memcpy(&params_, params, sizeof(dtObstacleAvoidanceParams));
	invHorizTime_ = 1.0f / params_.horizTime;
	vmax_ = vmax;
	invVmax_ = vmax > 0 ? 1.0f / vmax : FLT_MAX;
	
	dtVset(nvel, 0,0,0);
	
	if (debug)
		debug->reset();

	const float cvx = dvel[0] * params_.velBias;
	const float cvz = dvel[2] * params_.velBias;
	const float cs = vmax * 2 * (1 - params_.velBias) / (float)(params_.gridSize-1);
	const float half = (params_.gridSize-1)*cs*0.5f;
		
	float minPenalty = FLT_MAX;
	int ns = 0;
		
	for (int y = 0; y < params_.gridSize; ++y)
	{
		for (int x = 0; x < params_.gridSize; ++x)
		{
			float vcand[3];
			vcand[0] = cvx + x*cs - half;
			vcand[1] = 0;
			vcand[2] = cvz + y*cs - half;
			
			if (dtSqr(vcand[0])+dtSqr(vcand[2]) > dtSqr(vmax+cs/2)) continue;
			
			const float penalty = processSample(vcand, cs, pos,rad,vel,dvel, minPenalty, debug);
			ns++;
			if (penalty < minPenalty)
			{
				minPenalty = penalty;
				dtVcopy(nvel, vcand);
			}
		}
	}
	
	return ns;
}


// vector normalization that ignores the y-component.
inline void dtNormalize2D(float* v)
{
	float d = dtMathSqrtf(v[0] * v[0] + v[2] * v[2]);
	if (d==0)
		return;
	d = 1.0f / d;
	v[0] *= d;
	v[2] *= d;
}

// vector normalization that ignores the y-component.
inline void dtRorate2D(float* dest, const float* v, float ang)
{
	float c = cosf(ang);
	float s = sinf(ang);
	dest[0] = v[0]*c - v[2]*s;
	dest[2] = v[0]*s + v[2]*c;
	dest[1] = v[1];
}


int dtObstacleAvoidanceQuery::sampleVelocityAdaptive(const float* pos, const float rad, const float vmax,
													 const float* vel, const float* dvel, float* nvel,
													 const dtObstacleAvoidanceParams* params,
													 dtObstacleAvoidanceDebugData* debug)
{
	prepare(pos, dvel);
	
	memcpy(&params_, params, sizeof(dtObstacleAvoidanceParams));
	invHorizTime_ = 1.0f / params_.horizTime;
	vmax_ = vmax;
	invVmax_ = vmax > 0 ? 1.0f / vmax : FLT_MAX;
	
	dtVset(nvel, 0,0,0);
	
	if (debug)
		debug->reset();

	// Build sampling pattern aligned to desired velocity.
	float pat[(DT_MAX_PATTERN_DIVS*DT_MAX_PATTERN_RINGS+1)*2];
	int npat = 0;

	const int ndivs = (int)params_.adaptiveDivs;
	const int nrings= (int)params_.adaptiveRings;
	const int depth = (int)params_.adaptiveDepth;
	
	const int nd = dtClamp(ndivs, 1, DT_MAX_PATTERN_DIVS);
	const int nr = dtClamp(nrings, 1, DT_MAX_PATTERN_RINGS);
	const float da = (1.0f/nd) * DT_PI*2;
	const float ca = cosf(da);
	const float sa = sinf(da);

	// desired direction
	float ddir[6];
	dtVcopy(ddir, dvel);
	dtNormalize2D(ddir);
	dtRorate2D (ddir+3, ddir, da*0.5f); // rotated by da/2

	// Always add sample at zero
	pat[npat*2+0] = 0;
	pat[npat*2+1] = 0;
	npat++;
	
	for (int j = 0; j < nr; ++j)
	{
		const float r = (float)(nr-j)/(float)nr;
		pat[npat*2+0] = ddir[(j%1)*3] * r;
		pat[npat*2+1] = ddir[(j%1)*3+2] * r;
		float* last1 = pat + npat*2;
		float* last2 = last1;
		npat++;

		for (int i = 1; i < nd-1; i+=2)
		{
			// get next point on the "right" (rotate CW)
			pat[npat*2+0] = last1[0]*ca + last1[1]*sa;
			pat[npat*2+1] = -last1[0]*sa + last1[1]*ca;
			// get next point on the "left" (rotate CCW)
			pat[npat*2+2] = last2[0]*ca - last2[1]*sa;
			pat[npat*2+3] = last2[0]*sa + last2[1]*ca;

			last1 = pat + npat*2;
			last2 = last1 + 2;
			npat += 2;
		}

		if ((nd&1) == 0)
		{
			pat[npat*2+2] = last2[0]*ca - last2[1]*sa;
			pat[npat*2+3] = last2[0]*sa + last2[1]*ca;
			npat++;
		}
	}


	// Start sampling.
	float cr = vmax * (1.0f - params_.velBias);
	float res[3];
	dtVset(res, dvel[0] * params_.velBias, 0, dvel[2] * params_.velBias);
	int ns = 0;

	for (int k = 0; k < depth; ++k)
	{
		float minPenalty = FLT_MAX;
		float bvel[3];
		dtVset(bvel, 0,0,0);
		
		for (int i = 0; i < npat; ++i)
		{
			float vcand[3];
			vcand[0] = res[0] + pat[i*2+0]*cr;
			vcand[1] = 0;
			vcand[2] = res[2] + pat[i*2+1]*cr;
			
			if (dtSqr(vcand[0])+dtSqr(vcand[2]) > dtSqr(vmax+0.001f)) continue;
			
			const float penalty = processSample(vcand,cr/10, pos,rad,vel,dvel, minPenalty, debug);
			ns++;
			if (penalty < minPenalty)
			{
				minPenalty = penalty;
				dtVcopy(bvel, vcand);
			}
		}

		dtVcopy(res, bvel);

		cr *= 0.5f;
	}	
	
	dtVcopy(nvel, res);
	
	return ns;
}
