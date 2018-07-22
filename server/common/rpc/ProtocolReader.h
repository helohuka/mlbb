#ifndef __ProtocolReader_h__
#define __ProtocolReader_h__

#include "Common.h"

/** ARPC通讯协议读取器接口.
	ProtocolReader 为 service proxy 提供arpc通讯协议解编数据读取接口。
	一个 ProtocolReader 可以得到 一个service proxy 在反序列化rpc数据后的读取事件，
	派生类可以重载这个接口，将数据从内存或网络读取出来。
*/
class ProtocolReader
{
public:
	/** 读入rpc序列化数据. 
		proxy在rpc调用过程中通过此接口读取序列化数据.
		@param data 数据指针.
		@param len 数据长度.
	*/
	virtual bool read(void* data, size_t len) = 0;

	/** @name read basic types. */
	//@{
	bool readType(S64& v)
	{
		return read(&v, sizeof(S64));
	}
	bool readType(U64& v)
	{
		return read(&v, sizeof(U64));
	}
	bool readType(F64& v)
	{
		return read(&v, sizeof(F64));
	}
	bool readType(F32& v)
	{
		return read(&v, sizeof(F32));
	}
	bool readType(S32& v)
	{
		return read(&v, sizeof(S32));
	}
	bool readType(U32& v)
	{
		return read(&v, sizeof(U32));
	}
	bool readType(S16& v)
	{
		return read(&v, sizeof(S16));
	}
	bool readType(U16& v)
	{
		return read(&v, sizeof(U16));
	}
	bool readType(S8& v)
	{
		return read(&v, sizeof(S8));
	}
	bool readType(U8& v)
	{
		return read(&v, sizeof(U8));
	}
	bool readType(B8& v)
	{
		char vv;
		if(!read(&vv, sizeof(B8)))
			return false;
		v = vv?true:false;
		return true;
	}
	bool readType(STRING& v, U32 maxlen)
	{
		U32 len;
		if(!readDynSize(len) || len > maxlen)
			return false;
		v.resize(len);
		return read((void*)v.c_str(), len);
	}
	bool readDynSize(U32& s)
	{
		s = 0;
		U8 b;
		if(!readType(b))
			return false;
		S32 n = (b & 0XC0)>>6;
		s = (b & 0X3F);
		for(S32 i = 0; i < n; i++)
		{
			if(!readType(b))
				return false;
			s = (s<<8)|b;
		}
		return true;
	}
	//@}
};


#endif//__ProtocolReader_h__