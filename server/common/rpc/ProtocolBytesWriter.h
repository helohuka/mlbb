#ifndef __ProtocolBytesWriter_h__
#define __ProtocolBytesWriter_h__

#include "Common.h"
#include "ProtocolWriter.h"


/** 写入一个bytes数组. */
class ProtocolBytesWriter : public ProtocolWriter
{
public:
	ProtocolBytesWriter(std::vector<UINT8>& b):
	bytes_(b)
	{}

	virtual void write(const void* data, size_t len)
	{
		size_t s = bytes_.size();
		bytes_.resize(s + len);
		::memcpy(&(bytes_[s]), data, len);
	}
	std::vector<UINT8>& getBytes()	{ return bytes_; }

private:
	std::vector<UINT8>& bytes_;
};


#endif//__ProtocolBytesWriter_h__