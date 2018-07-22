#ifndef __ProtocolMemWriter_h__
#define __ProtocolMemWriter_h__
#include "Common.h"
#include "ProtocolWriter.h"
/** 从一个指定内存区读取数据的读取器实现.
*/
class ProtocolMemWriter : public ProtocolWriter
{
public:
	ProtocolMemWriter(void* b, size_t l):
	buffer_((char*)b),
	space_(l),
	wtptr_(0)
	{}

	virtual void write(const void* data, size_t len)
	{
		if(space_ < wtptr_ + len)
			return;
		::memcpy(buffer_ + wtptr_, data, len);
		wtptr_ += len;
	}

	inline size_t space()
	{
		return space_;
	}

	inline size_t length()
	{
		return wtptr_;
	}

	template <typename T>
	bool writeVector(std::vector<T>& vIn)
	{
		if (0==space_)
			return true;
		if (0== vIn.size())
			return true;
		writeDynSize(vIn.size());
		for (size_t i=0;i<vIn.size();i++)
		{
			if (wtptr_>space_)
				return false;
			vIn[i].serialize(this);
		}
		return true;
	}

	bool writeVector(std::vector<S32>& vIn)
	{
		if (0==space_)
			return true;
		if (0== vIn.size())
			return true;
		writeDynSize(vIn.size());
		vIn.resize(vIn.size());
		for (size_t i=0;i<vIn.size();i++)
		{
			write(&vIn[i],sizeof(S32));
		}
		return true;
	}

private:
	char*			buffer_;
	size_t			space_;
	size_t			wtptr_;
};


#endif//__ProtocolMemWriter_h__