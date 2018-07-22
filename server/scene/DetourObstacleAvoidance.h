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

#ifndef DETOUROBSTACLEAVOIDANCE_H
#define DETOUROBSTACLEAVOIDANCE_H

struct dtObstacleCircle
{
	float p[3];				///< Position of the obstacle
	float vel[3];			///< Velocity of the obstacle
	float dvel[3];			///< Velocity of the obstacle
	float rad;				///< Radius of the obstacle
	float dp[3], np[3];		///< Use for side selection during sampling.
};

struct dtObstacleSegment
{
	float p[3], q[3];		///< End points of the obstacle segment
	bool touch;
};


class dtObstacleAvoidanceDebugData
{
public:
	dtObstacleAvoidanceDebugData();
	~dtObstacleAvoidanceDebugData();
	
	bool init(const int maxSamples);
	void reset();
	void addSample(const float* vel, const float ssize, const float pen,
				   const float vpen, const float vcpen, const float spen, const float tpen);
	
	void normalizeSamples();
	
	inline int getSampleCount() const { return nsamples_; }
	inline const float* getSampleVelocity(const int i) const { return &vel_[i*3]; }
	inline float getSampleSize(const int i) const { return ssize_[i]; }
	inline float getSamplePenalty(const int i) const { return pen_[i]; }
	inline float getSampleDesiredVelocityPenalty(const int i) const { return vpen_[i]; }
	inline float getSampleCurrentVelocityPenalty(const int i) const { return vcpen_[i]; }
	inline float getSamplePreferredSidePenalty(const int i) const { return spen_[i]; }
	inline float getSampleCollisionTimePenalty(const int i) const { return tpen_[i]; }

private:
	int nsamples_;
	int maxSamples_;
	float* vel_;
	float* ssize_;
	float* pen_;
	float* vpen_;
	float* vcpen_;
	float* spen_;
	float* tpen_;
};

dtObstacleAvoidanceDebugData* dtAllocObstacleAvoidanceDebugData();
void dtFreeObstacleAvoidanceDebugData(dtObstacleAvoidanceDebugData* ptr);


static const int DT_MAX_PATTERN_DIVS = 32;	///< Max numver of adaptive divs.
static const int DT_MAX_PATTERN_RINGS = 4;	///< Max number of adaptive rings.

struct dtObstacleAvoidanceParams
{
	float velBias;
	float weightDesVel;
	float weightCurVel;
	float weightSide;
	float weightToi;
	float horizTime;
	unsigned char gridSize;	///< grid
	unsigned char adaptiveDivs;	///< adaptive
	unsigned char adaptiveRings;	///< adaptive
	unsigned char adaptiveDepth;	///< adaptive
};

class dtObstacleAvoidanceQuery
{
public:
	dtObstacleAvoidanceQuery();
	~dtObstacleAvoidanceQuery();
	
	bool init(const int maxCircles, const int maxSegments);
	
	void reset();

	void addCircle(const float* pos, const float rad,
				   const float* vel, const float* dvel);
				   
	void addSegment(const float* p, const float* q);

	int sampleVelocityGrid(const float* pos, const float rad, const float vmax,
						   const float* vel, const float* dvel, float* nvel,
						   const dtObstacleAvoidanceParams* params,
						   dtObstacleAvoidanceDebugData* debug = 0);

	int sampleVelocityAdaptive(const float* pos, const float rad, const float vmax,
							   const float* vel, const float* dvel, float* nvel,
							   const dtObstacleAvoidanceParams* params, 
							   dtObstacleAvoidanceDebugData* debug = 0);
	
	inline int getObstacleCircleCount() const { return ncircles_; }
	const dtObstacleCircle* getObstacleCircle(const int i) { return &circles_[i]; }

	inline int getObstacleSegmentCount() const { return nsegments_; }
	const dtObstacleSegment* getObstacleSegment(const int i) { return &segments_[i]; }

private:

	void prepare(const float* pos, const float* dvel);

	float processSample(const float* vcand, const float cs,
						const float* pos, const float rad,
						const float* vel, const float* dvel,
						const float minPenalty,
						dtObstacleAvoidanceDebugData* debug);

	dtObstacleAvoidanceParams params_;
	float invHorizTime_;
	float vmax_;
	float invVmax_;

	int maxCircles_;
	dtObstacleCircle* circles_;
	int ncircles_;

	int maxSegments_;
	dtObstacleSegment* segments_;
	int nsegments_;
};

dtObstacleAvoidanceQuery* dtAllocObstacleAvoidanceQuery();
void dtFreeObstacleAvoidanceQuery(dtObstacleAvoidanceQuery* ptr);


#endif // DETOUROBSTACLEAVOIDANCE_H
