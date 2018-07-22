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

#define _USE_MATH_DEFINES
#include "config.h"
#include "Recast.h"
#include "InputGeom.h"
#include "ChunkyTriMesh.h"
#include "ObjFile.h"
#include "DetourNavMesh.h"

static bool intersectSegmentTriangle(const float* sp, const float* sq,
									 const float* a, const float* b, const float* c,
									 float &t)
{
	float v, w;
	float ab[3], ac[3], qp[3], ap[3], norm[3], e[3];
	rcVsub(ab, b, a);
	rcVsub(ac, c, a);
	rcVsub(qp, sp, sq);
	
	// Compute triangle normal. Can be precalculated or cached if
	// intersecting multiple segments against the same triangle
	rcVcross(norm, ab, ac);
	
	// Compute denominator d. If d <= 0, segment is parallel to or points
	// away from triangle, so exit early
	float d = rcVdot(qp, norm);
	if (d <= 0.0f) return false;
	
	// Compute intersection t value of pq with plane of triangle. A ray
	// intersects iff 0 <= t. Segment intersects iff 0 <= t <= 1. Delay
	// dividing by d until intersection has been found to pierce triangle
	rcVsub(ap, sp, a);
	t = rcVdot(ap, norm);
	if (t < 0.0f) return false;
	if (t > d) return false; // For segment; exclude this code line for a ray test
	
	// Compute barycentric coordinate components and test if within bounds
	rcVcross(e, qp, ap);
	v = rcVdot(ac, e);
	if (v < 0.0f || v > d) return false;
	w = -rcVdot(ab, e);
	if (w < 0.0f || v + w > d) return false;
	
	// Segment/ray intersects triangle. Perform delayed division
	t /= d;
	
	return true;
}

static char* parseRow(char* buf, char* bufEnd, char* row, int len)
{
	bool start = true;
	bool done = false;
	int n = 0;
	while (!done && buf < bufEnd)
	{
		char c = *buf;
		buf++;
		// multirow
		switch (c)
		{
			case '\n':
				if (start) break;
				done = true;
				break;
			case '\r':
				break;
			case '\t':
			case ' ':
				if (start) break;
			default:
				start = false;
				row[n++] = c;
				if (n >= len-1)
					done = true;
				break;
		}
	}
	row[n] = '\0';
	return buf;
}



InputGeom::InputGeom() :
	chunkyMesh_(0),
	mesh_(0),
	offMeshConCount_(0),
	volumeCount_(0)
{
}

InputGeom::~InputGeom()
{
	delete chunkyMesh_;
	delete mesh_;
}
		
bool InputGeom::loadMesh(const char* filepath)
{
	if (mesh_)
	{
		delete chunkyMesh_;
		chunkyMesh_ = 0;
		delete mesh_;
		mesh_ = 0;
	}
	offMeshConCount_ = 0;
	volumeCount_ = 0;
	
	mesh_ = new ObjFile;
	if (!mesh_)
	{
		ACE_DEBUG((LM_ERROR, "loadMesh: Out of memory 'm_mesh'.\n"));
		return false;
	}
	if (!mesh_->load(filepath))
	{
		ACE_DEBUG((LM_ERROR,  "buildTiledNavigation: Could not load '%s'\n", filepath));
		return false;
	}

	rcCalcBounds(mesh_->getVerts(), mesh_->getVertCount(), meshBMin_, meshBMax_);

	chunkyMesh_ = new ChunkyTriMesh;
	if (!chunkyMesh_)
	{
		ACE_DEBUG((LM_ERROR, "buildTiledNavigation: Out of memory 'm_chunkyMesh'.\n"));
		return false;
	}
	if (!CreateChunkyTriMesh(mesh_->getVerts(), mesh_->getTris(), mesh_->getTriCount(), 256, chunkyMesh_))
	{
		ACE_DEBUG((LM_ERROR, "buildTiledNavigation: Failed to build chunky mesh.\n"));
		return false;
	}		

	return true;
}

bool InputGeom::load(const char* filePath)
{
	char* buf = 0;
	FILE* fp = fopen(filePath, "rb");
	if (!fp)
		return false;
	fseek(fp, 0, SEEK_END);
	int bufSize = ftell(fp);
	fseek(fp, 0, SEEK_SET);
	buf = new char[bufSize];
	if (!buf)
	{
		fclose(fp);
		return false;
	}
	size_t readLen = fread(buf, bufSize, 1, fp);
	fclose(fp);
	if (readLen != 1)
	{
		delete[] buf;
		return false;
	}
	
	offMeshConCount_ = 0;
	volumeCount_ = 0;
	delete mesh_;
	mesh_ = 0;

	char* src = buf;
	char* srcEnd = buf + bufSize;
	char row[512];
	while (src < srcEnd)
	{
		// Parse one row
		row[0] = '\0';
		src = parseRow(src, srcEnd, row, sizeof(row)/sizeof(char));
		if (row[0] == 'f')
		{
			// File name.
			const char* name = row+1;
			// Skip white spaces
			while (*name && isspace(*name))
				name++;
			if (*name)
			{
				if (!loadMesh(name))
				{
					delete [] buf;
					return false;
				}
			}
		}
		else if (row[0] == 'c')
		{
			// Off-mesh connection
			if (offMeshConCount_ < MAX_OFFMESH_CONNECTIONS)
			{
				float* v = &offMeshConVerts_[offMeshConCount_*3*2];
				int bidir, area = 0, flags = 0;
				float rad;
				sscanf(row+1, "%f %f %f  %f %f %f %f %d %d %d",
					   &v[0], &v[1], &v[2], &v[3], &v[4], &v[5], &rad, &bidir, &area, &flags);
				offMeshConRads_[offMeshConCount_] = rad;
				offMeshConDirs_[offMeshConCount_] = (unsigned char)bidir;
				offMeshConAreas_[offMeshConCount_] = (unsigned char)area;
				offMeshConFlags_[offMeshConCount_] = (unsigned short)flags;
				offMeshConCount_++;
			}
		}
		else if (row[0] == 'v')
		{
			// Convex volumes
			if (volumeCount_ < MAX_VOLUMES)
			{
				ConvexVolume* vol = &volumes_[volumeCount_++];
				sscanf(row+1, "%d %d %f %f", &vol->nverts, &vol->area, &vol->hmin, &vol->hmax);
				for (int i = 0; i < vol->nverts; ++i)
				{
					row[0] = '\0';
					src = parseRow(src, srcEnd, row, sizeof(row)/sizeof(char));
					sscanf(row, "%f %f %f", &vol->verts[i*3+0], &vol->verts[i*3+1], &vol->verts[i*3+2]);
				}
			}
		}
	}
	
	delete [] buf;
	
	return true;
}

bool InputGeom::save(const char* filepath)
{
	if (!mesh_) return false;
	
	FILE* fp = fopen(filepath, "w");
	if (!fp) return false;
	
	// Store mesh filename.
	fprintf(fp, "f %s\n", mesh_->getFileName());
	
	// Store off-mesh links.
	for (int i = 0; i < offMeshConCount_; ++i)
	{
		const float* v = &offMeshConVerts_[i*3*2];
		const float rad = offMeshConRads_[i];
		const int bidir = offMeshConDirs_[i];
		const int area = offMeshConAreas_[i];
		const int flags = offMeshConFlags_[i];
		fprintf(fp, "c %f %f %f  %f %f %f  %f %d %d %d\n",
				v[0], v[1], v[2], v[3], v[4], v[5], rad, bidir, area, flags);
	}

	// Convex volumes
	for (int i = 0; i < volumeCount_; ++i)
	{
		ConvexVolume* vol = &volumes_[i];
		fprintf(fp, "v %d %d %f %f\n", vol->nverts, vol->area, vol->hmin, vol->hmax);
		for (int j = 0; j < vol->nverts; ++j)
			fprintf(fp, "%f %f %f\n", vol->verts[j*3+0], vol->verts[j*3+1], vol->verts[j*3+2]);
	}
	
	fclose(fp);
	
	return true;
}

static bool isectSegAABB(const float* sp, const float* sq,
						 const float* amin, const float* amax,
						 float& tmin, float& tmax)
{
	static const float EPS = 1e-6f;
	
	float d[3];
	d[0] = sq[0] - sp[0];
	d[1] = sq[1] - sp[1];
	d[2] = sq[2] - sp[2];
	tmin = 0.0;
	tmax = 1.0f;
	
	for (int i = 0; i < 3; i++)
	{
		if (fabsf(d[i]) < EPS)
		{
			if (sp[i] < amin[i] || sp[i] > amax[i])
				return false;
		}
		else
		{
			const float ood = 1.0f / d[i];
			float t1 = (amin[i] - sp[i]) * ood;
			float t2 = (amax[i] - sp[i]) * ood;
			if (t1 > t2) { float tmp = t1; t1 = t2; t2 = tmp; }
			if (t1 > tmin) tmin = t1;
			if (t2 < tmax) tmax = t2;
			if (tmin > tmax) return false;
		}
	}
	
	return true;
}


bool InputGeom::raycastMesh(float* src, float* dst, float& tmin)
{
	float dir[3];
	rcVsub(dir, dst, src);

	// Prune hit ray.
	float btmin, btmax;
	if (!isectSegAABB(src, dst, meshBMin_, meshBMax_, btmin, btmax))
		return false;
	float p[2], q[2];
	p[0] = src[0] + (dst[0]-src[0])*btmin;
	p[1] = src[2] + (dst[2]-src[2])*btmin;
	q[0] = src[0] + (dst[0]-src[0])*btmax;
	q[1] = src[2] + (dst[2]-src[2])*btmax;
	
	int cid[512];
	const int ncid = GetChunksOverlappingSegment(chunkyMesh_, p, q, cid, 512);
	if (!ncid)
		return false;
	
	tmin = 1.0f;
	bool hit = false;
	const float* verts = mesh_->getVerts();
	
	for (int i = 0; i < ncid; ++i)
	{
		const ChunkyTriMeshNode& node = chunkyMesh_->nodes[cid[i]];
		const int* tris = &chunkyMesh_->tris[node.i*3];
		const int ntris = node.n;

		for (int j = 0; j < ntris*3; j += 3)
		{
			float t = 1;
			if (intersectSegmentTriangle(src, dst,
										 &verts[tris[j]*3],
										 &verts[tris[j+1]*3],
										 &verts[tris[j+2]*3], t))
			{
				if (t < tmin)
					tmin = t;
				hit = true;
			}
		}
	}
	
	return hit;
}

void InputGeom::addOffMeshConnection(const float* spos, const float* epos, const float rad,
									 unsigned char bidir, unsigned char area, unsigned short flags)
{
	if (offMeshConCount_ >= MAX_OFFMESH_CONNECTIONS) return;
	float* v = &offMeshConVerts_[offMeshConCount_*3*2];
	offMeshConRads_[offMeshConCount_] = rad;
	offMeshConDirs_[offMeshConCount_] = bidir;
	offMeshConAreas_[offMeshConCount_] = area;
	offMeshConFlags_[offMeshConCount_] = flags;
	offMeshConId_[offMeshConCount_] = 1000 + offMeshConCount_;
	rcVcopy(&v[0], spos);
	rcVcopy(&v[3], epos);
	offMeshConCount_++;
}

void InputGeom::deleteOffMeshConnection(int i)
{
	offMeshConCount_--;
	float* src = &offMeshConVerts_[offMeshConCount_*3*2];
	float* dst = &offMeshConVerts_[i*3*2];
	rcVcopy(&dst[0], &src[0]);
	rcVcopy(&dst[3], &src[3]);
	offMeshConRads_[i] = offMeshConRads_[offMeshConCount_];
	offMeshConDirs_[i] = offMeshConDirs_[offMeshConCount_];
	offMeshConAreas_[i] = offMeshConAreas_[offMeshConCount_];
	offMeshConFlags_[i] = offMeshConFlags_[offMeshConCount_];
}


void InputGeom::addConvexVolume(const float* verts, const int nverts,
								const float minh, const float maxh, unsigned char area, unsigned long long userdata)
{
	if (volumeCount_ >= MAX_VOLUMES) return;
	ConvexVolume* vol = &volumes_[volumeCount_++];
	memset(vol, 0, sizeof(ConvexVolume));
	memcpy(vol->verts, verts, sizeof(float)*3*nverts);
	vol->hmin = minh;
	vol->hmax = maxh;
	vol->nverts = nverts;
	vol->area = area;
	vol->userdata = userdata;
}

void InputGeom::deleteConvexVolume(int i)
{
	volumeCount_--;
	volumes_[i] = volumes_[volumeCount_];
}
