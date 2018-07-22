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
#include "DetourNode.h"
#include "DetourAlloc.h"
#include "DetourCommon.h"

#ifdef DT_POLYREF64
// From Thomas Wang, https://gist.github.com/badboy/6267743
inline unsigned int dtHashRef(dtPolyRef a)
{
	a = (~a) + (a << 18); // a = (a << 18) - a - 1;
	a = a ^ (a >> 31);
	a = a * 21; // a = (a + (a << 2)) + (a << 4);
	a = a ^ (a >> 11);
	a = a + (a << 6);
	a = a ^ (a >> 22);
	return (unsigned int)a;
}
#else
inline unsigned int dtHashRef(dtPolyRef a)
{
	a += ~(a<<15);
	a ^=  (a>>10);
	a +=  (a<<3);
	a ^=  (a>>6);
	a += ~(a<<11);
	a ^=  (a>>16);
	return (unsigned int)a;
}
#endif

//////////////////////////////////////////////////////////////////////////////////////////
dtNodePool::dtNodePool(int maxNodes, int hashSize) :
	nodes_(0),
	first_(0),
	next_(0),
	maxNodes_(maxNodes),
	hashSize_(hashSize),
	nodeCount_(0)
{
	SRV_ASSERT(dtNextPow2(hashSize_) == (unsigned int)hashSize_);
	SRV_ASSERT(maxNodes_ > 0);

	nodes_ = (dtNode*)dtAlloc(sizeof(dtNode)*maxNodes_, DT_ALLOC_PERM);
	next_ = (dtNodeIndex*)dtAlloc(sizeof(dtNodeIndex)*maxNodes_, DT_ALLOC_PERM);
	first_ = (dtNodeIndex*)dtAlloc(sizeof(dtNodeIndex)*hashSize, DT_ALLOC_PERM);

	SRV_ASSERT(nodes_);
	SRV_ASSERT(next_);
	SRV_ASSERT(first_);

	memset(first_, 0xff, sizeof(dtNodeIndex)*hashSize_);
	memset(next_, 0xff, sizeof(dtNodeIndex)*maxNodes_);
}

dtNodePool::~dtNodePool()
{
	dtFree(nodes_);
	dtFree(next_);
	dtFree(first_);
}

void dtNodePool::clear()
{
	memset(first_, 0xff, sizeof(dtNodeIndex)*hashSize_);
	nodeCount_ = 0;
}

unsigned int dtNodePool::findNodes(dtPolyRef id, dtNode** nodes, const int maxNodes)
{
	int n = 0;
	unsigned int bucket = dtHashRef(id) & (hashSize_-1);
	dtNodeIndex i = first_[bucket];
	while (i != DT_NULL_IDX)
	{
		if (nodes_[i].id == id)
		{
			if (n >= maxNodes)
				return n;
			nodes[n++] = &nodes_[i];
		}
		i = next_[i];
	}

	return n;
}

dtNode* dtNodePool::findNode(dtPolyRef id, unsigned char state)
{
	unsigned int bucket = dtHashRef(id) & (hashSize_-1);
	dtNodeIndex i = first_[bucket];
	while (i != DT_NULL_IDX)
	{
		if (nodes_[i].id == id && nodes_[i].state == state)
			return &nodes_[i];
		i = next_[i];
	}
	return 0;
}

dtNode* dtNodePool::getNode(dtPolyRef id, unsigned char state)
{
	unsigned int bucket = dtHashRef(id) & (hashSize_-1);
	dtNodeIndex i = first_[bucket];
	dtNode* node = 0;
	while (i != DT_NULL_IDX)
	{
		if (nodes_[i].id == id && nodes_[i].state == state)
			return &nodes_[i];
		i = next_[i];
	}
	
	if (nodeCount_ >= maxNodes_)
		return 0;
	
	i = (dtNodeIndex)nodeCount_;
	nodeCount_++;
	
	// Init node
	node = &nodes_[i];
	node->pidx = 0;
	node->cost = 0;
	node->total = 0;
	node->id = id;
	node->state = state;
	node->flags = 0;
	
	next_[i] = first_[bucket];
	first_[bucket] = i;
	
	return node;
}


//////////////////////////////////////////////////////////////////////////////////////////
dtNodeQueue::dtNodeQueue(int n) :
	heap_(0),
	capacity_(n),
	size_(0)
{
	SRV_ASSERT(capacity_ > 0);
	
	heap_ = (dtNode**)dtAlloc(sizeof(dtNode*)*(capacity_+1), DT_ALLOC_PERM);
	SRV_ASSERT(heap_);
}

dtNodeQueue::~dtNodeQueue()
{
	dtFree(heap_);
}

void dtNodeQueue::bubbleUp(int i, dtNode* node)
{
	int parent = (i-1)/2;
	// note: (index > 0) means there is a parent
	while ((i > 0) && (heap_[parent]->total > node->total))
	{
		heap_[i] = heap_[parent];
		i = parent;
		parent = (i-1)/2;
	}
	heap_[i] = node;
}

void dtNodeQueue::trickleDown(int i, dtNode* node)
{
	int child = (i*2)+1;
	while (child < size_)
	{
		if (((child+1) < size_) && 
			(heap_[child]->total > heap_[child+1]->total))
		{
			child++;
		}
		heap_[i] = heap_[child];
		i = child;
		child = (i*2)+1;
	}
	bubbleUp(i, node);
}
