#ifndef __ProtocolWriter_h__
#define __ProtocolWriter_h__

#include "Common.h"

/** ARPC通讯协议写入器接口.
	ProtocolWriter 为生成的service stub提供arpc协议整编数据写入接口。
	一个 ProtocolWriter 可以得到 一个service stub 在序列化rpc数据后的写入事件，
	派生类可以重载这个接口，将数据写入到内存或直接写入网络。
*/
class ProtocolWriter
{
public:
	/** 写入rpc序列化数据. 
		stub在rpc调用过程中通过此接口写入序列化数据.
		@param data 数据指针.
		@param len 数据长度.
	*/
	virtual void write(const void* data, size_t len) = 0;

	/** @name write basic types. */
	//@{
	void writeType(S64 v)
	{
		write(&v, sizeof(S64));
	}
	void writeType(U64 v)
	{
		write(&v, sizeof(U64));
	}
	void writeType(F64 v)
	{
		write(&v, sizeof(F64));
	}
	void writeType(F32 v)
	{
		write(&v, sizeof(F32));
	}
	void writeType(S32 v)
	{
		write(&v, sizeof(S32));
	}
	void writeType(U32 v)
	{
		write(&v, sizeof(U32));
	}
	void writeType(S16 v)
	{
		write(&v, sizeof(S16));
	}
	void writeType(U16 v)
	{
		write(&v, sizeof(U16));
	}
	void writeType(S8 v)
	{
		write(&v, sizeof(S8));
	}
	void writeType(U8 v)
	{
		write(&v, sizeof(U8));
	}
	void writeType(B8 v)
	{
		char vv = v?1:0;
		write(&vv, sizeof(B8));
	}
	void writeType(const STRING& v)
	{
		U32 len = (U32)v.length();
		writeDynSize(len);
		write(v.c_str(), v.length());
	}
	void writeDynSize(U32 s)
	{
		U8* p = (U8*)(&s);
		U8 n = 0;
		if(s <= 0X3F)
			n = 0;
		else if(s <= 0X3FFF)
			n = 1;
		else if(s <= 0X3FFFFF)
			n = 2;
		else if(s <= 0X3FFFFFFF)
			n = 3;
		p[n] |= (n<<6);
		for(S32 i = (S32)n; i >= 0; i--)
			writeType(p[i]);
	}
	//@}
};



#endif//__ProtocolWriter_h__