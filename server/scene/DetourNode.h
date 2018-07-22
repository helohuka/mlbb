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

#ifndef DETOURNODE_H
#define DETOURNODE_H

#include "DetourNavMesh.h"

enum dtNodeFlags
{
	DT_NODE_OPEN = 0x01,
	DT_NODE_CLOSED = 0x02,
	DT_NODE_PARENT_DETACHED = 0x04, // parent of the node is not adjacent. Found using raycast.
};

typedef unsigned short dtNodeIndex;
static const dtNodeIndex DT_NULL_IDX = (dtNodeIndex)~0;

struct dtNode
{
	float pos[3];				///< Position of the node.
	float cost;					///< Cost from previous node to current node.
	float total;				///< Cost up to the node.
	unsigned int pidx : 24;		///< Index to parent node.
	unsigned int state : 2;		///< extra state information. A polyRef can have multiple nodes with different extra info. see DT_MAX_STATES_PER_NODE
	unsigned int flags : 3;		///< Node flags. A combination of dtNodeFlags.
	dtPolyRef id;				///< Polygon ref the node corresponds to.
};


static const int DT_MAX_STATES_PER_NODE = 4;	// number of extra states per node. See dtNode::state



class dtNodePool
{
public:
	dtNodePool(int maxNodes, int hashSize);
	~dtNodePool();
	inline void operator=(const dtNodePool&) {}
	void clear();

	// Get a dtNode by ref and extra state information. If there is none then - allocate
	// There can be more than one node for the same polyRef but with different extra state information
	dtNode* getNode(dtPolyRef id, unsigned char state=0);	
	dtNode* findNode(dtPolyRef id, unsigned char state);
	unsigned int findNodes(dtPolyRef id, dtNode** nodes, const int maxNodes);

	inline unsigned int getNodeIdx(const dtNode* node) const
	{
		if (!node) return 0;
		return (unsigned int)(node - nodes_)+1;
	}

	inline dtNode* getNodeAtIdx(unsigned int idx)
	{
		if (!idx) return 0;
		return &nodes_[idx-1];
	}

	inline const dtNode* getNodeAtIdx(unsigned int idx) const
	{
		if (!idx) return 0;
		return &nodes_[idx-1];
	}
	
	inline int getMemUsed() const
	{
		return sizeof(*this) +
			sizeof(dtNode)*maxNodes_ +
			sizeof(dtNodeIndex)*maxNodes_ +
			sizeof(dtNodeIndex)*hashSize_;
	}
	
	inline int getMaxNodes() const { return maxNodes_; }
	
	inline int getHashSize() const { return hashSize_; }
	inline dtNodeIndex getFirst(int bucket) const { return first_[bucket]; }
	inline dtNodeIndex getNext(int i) const { return next_[i]; }
	inline int getNodeCount() const { return nodeCount_; }
	
private:
	
	dtNode* nodes_;
	dtNodeIndex* first_;
	dtNodeIndex* next_;
	const int maxNodes_;
	const int hashSize_;
	int nodeCount_;
};

class dtNodeQueue
{
public:
	dtNodeQueue(int n);
	~dtNodeQueue();
	inline void operator=(dtNodeQueue&) {}
	
	inline void clear()
	{
		size_ = 0;
	}
	
	inline dtNode* top()
	{
		return heap_[0];
	}
	
	inline dtNode* pop()
	{
		dtNode* result = heap_[0];
		size_--;
		trickleDown(0, heap_[size_]);
		return result;
	}
	
	inline void push(dtNode* node)
	{
		size_++;
		bubbleUp(size_-1, node);
	}
	
	inline void modify(dtNode* node)
	{
		for (int i = 0; i < size_; ++i)
		{
			if (heap_[i] == node)
			{
				bubbleUp(i, node);
				return;
			}
		}
	}
	
	inline bool empty() const { return size_ == 0; }
	
	inline int getMemUsed() const
	{
		return sizeof(*this) +
		sizeof(dtNode*)*(capacity_+1);
	}
	
	inline int getCapacity() const { return capacity_; }
	
private:
	void bubbleUp(int i, dtNode* node);
	void trickleDown(int i, dtNode* node);
	
	dtNode** heap_;
	const int capacity_;
	int size_;
};		


#endif // DETOURNODE_H
