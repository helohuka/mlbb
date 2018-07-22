#ifndef __ProtocolMemReader_h__
#define __ProtocolMemReader_h__

#include "Common.h"
#include "ProtocolReader.h"

/** 从一个指定内存区读取数据的读取器实现.
*/
class ProtocolMemReader : public ProtocolReader
{
public:
	ProtocolMemReader(const void* b, size_t l):
	buffer_((char*)b),
	length_(l),
	rdptr_(0)
	{}

	virtual bool read(void* data, size_t len)
	{
		if(length_ < rdptr_ + len)
			return false;
		::memcpy(data, buffer_ + rdptr_, len);
		rdptr_ += len;
		return true;
	}

	template <typename T>
	bool readVector(std::vector<T>& vOut)
	{
		if (0==length_) return true;
		U32 size=0;
		readDynSize(size);
		if (0==size) return true;
		vOut.resize(size);
		for(U32 i=0;i<size;i++)
		{
			vOut[i].deserialize(this);
		}
		return true;
	}
	
	bool readVector(std::vector<S32>& vOut){
		if (0==length_) return true;
		U32 size=0;
		readDynSize(size);
		if (0==size) return true;
		vOut.resize(size);
		for(U32 i=0;i<size;i++)
		{
			if(!read(&vOut[i],sizeof(S32)))
				return false;
		}
		return true;
	}
private:
	const char*			buffer_;
	size_t			length_;
	size_t			rdptr_;
};


#endif//__ProtocolMemReader_h__