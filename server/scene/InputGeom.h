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

#ifndef INPUTGEOM_H
#define INPUTGEOM_H

#include "ChunkyTriMesh.h"
#include "ObjFile.h"

static const int MAX_CONVEXVOL_PTS = 12;
struct ConvexVolume
{
	float verts[MAX_CONVEXVOL_PTS*3];
	float hmin, hmax;
	int nverts;
	int area;
	unsigned long long userdata;
};

class InputGeom
{
	ChunkyTriMesh* chunkyMesh_;
	ObjFile* mesh_;
	float meshBMin_[3], meshBMax_[3];
	
	/// @name Off-Mesh connections.
	///@{
	static const int MAX_OFFMESH_CONNECTIONS = 256;
	float offMeshConVerts_[MAX_OFFMESH_CONNECTIONS*3*2];
	float offMeshConRads_[MAX_OFFMESH_CONNECTIONS];
	unsigned char offMeshConDirs_[MAX_OFFMESH_CONNECTIONS];
	unsigned char offMeshConAreas_[MAX_OFFMESH_CONNECTIONS];
	unsigned short offMeshConFlags_[MAX_OFFMESH_CONNECTIONS];
	unsigned int offMeshConId_[MAX_OFFMESH_CONNECTIONS];
	int offMeshConCount_;
	///@}

	/// @name Convex Volumes.
	///@{
	static const int MAX_VOLUMES = 256;
	ConvexVolume volumes_[MAX_VOLUMES];
	int volumeCount_;
	///@}
	
public:
	InputGeom();
	~InputGeom();
	
	bool loadMesh(const char* filepath);
	
	bool load(const char* filepath);
	bool save(const char* filepath);
	
	/// Method to return static mesh data.
	inline const ObjFile* getMesh() const { return mesh_; }
	inline const float* getMeshBoundsMin() const { return meshBMin_; }
	inline const float* getMeshBoundsMax() const { return meshBMax_; }
	inline const ChunkyTriMesh* getChunkyMesh() const { return chunkyMesh_; }
	bool raycastMesh(float* src, float* dst, float& tmin);

	/// @name Off-Mesh connections.
	///@{
	int getOffMeshConnectionCount() const { return offMeshConCount_; }
	const float* getOffMeshConnectionVerts() const { return offMeshConVerts_; }
	const float* getOffMeshConnectionRads() const { return offMeshConRads_; }
	const unsigned char* getOffMeshConnectionDirs() const { return offMeshConDirs_; }
	const unsigned char* getOffMeshConnectionAreas() const { return offMeshConAreas_; }
	const unsigned short* getOffMeshConnectionFlags() const { return offMeshConFlags_; }
	const unsigned int* getOffMeshConnectionId() const { return offMeshConId_; }
	void addOffMeshConnection(const float* spos, const float* epos, const float rad,
							  unsigned char bidir, unsigned char area, unsigned short flags);
	void deleteOffMeshConnection(int i);
	///@}

	/// @name Box Volumes.
	///@{
	int getConvexVolumeCount() const { return volumeCount_; }
	const ConvexVolume* getConvexVolumes() const { return volumes_; }
	void addConvexVolume(const float* verts, const int nverts,
						 const float minh, const float maxh, unsigned char area, unsigned long long userdata = 0);
	void deleteConvexVolume(int i);
	///@}
};

#endif // INPUTGEOM_H
