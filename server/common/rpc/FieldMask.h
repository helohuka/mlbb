#ifndef __FieldMask_h__
#define __FieldMask_h__

#include "Common.h"

/** 用于设置和读取按照bit排列的field mask. */
template<int LEN>
class FieldMask
{
public:
	FieldMask():
	pos_(0)
	{
		for( int i = 0; i < LEN; i++ )
			masks_[i] = 0;
	}

	void writeBit( bool b )
	{
		if(b)
			masks_[pos_>>3] |= (128>>(pos_&0X00000007));
		pos_++;
	}

	bool readBit()
	{
		bool r = masks_[pos_>>3] & (128>>(pos_&0X00000007))?true:false;
		pos_++;
		return r;
	}

	unsigned char	masks_[LEN];
	unsigned int	pos_;
};

#endif//__FieldMask_h__
