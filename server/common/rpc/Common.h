#ifndef __COMMON_H__
#define __COMMON_H__

/** @page arpc 异步RPC系统.

arpc系统( asynchronous remote procedure calling )是一个类似于CORBA的系统，将网络层封装到对象方法调用级别，方便了网络协议的编写。

@section aprc_ref 参考系统
- google protocol buffers.
- CORBA

这些实现都很成熟且被广泛使用，但是这些实现对于游戏来说有一些缺点:
- 过于复杂，不便于游戏应用
- 效率问题
- 很难对idl进行自定义扩展 


@section desc 系统描述
arpc系统的设计目标是一个轻便的rpc系统，主要完成rpc的整编解编和调用功能，其他功能不在考虑范围内。

@par arpc的功能
- 简化应用中通讯消息的解析工作，所有的消息解析代码依赖于RPC编译器自动生成，减少手动编写代码产生错误的概率
- 简化消息的修改工作
- RPC与具体的传输层实现分开，使RPC系统可以在各种传输协议以及IO模型中使用
- 可以通过RPC方便的产生出各种实现代码，比如服务自动测试工具，消息log工具等等
为了达到系统设计简单轻便的目标，arpc所有的调用全部基于异步，所以每个调用只能使用传入参数，而没有传出参数和返回值。
在使用arpc系统时，可以将交互双方看作是基于异步的请求与事件处理。

@par
aprc系统主要由一个idl编译器和一个运行时库组成
@image html arpc.png
@see Sepcification ProtocolWriter ProtocolReader 

@subsection aprc_stub Service Stub
arpcc 会根据一个service描述生成这个service调用方stub类。
stub主要负责将一个成员函数调用进行整编，并将整编数据写入到引用的 ProtocolWriter 中。
使用者可以通过派生 ProtocolWriter 来实现运载协议和数据的传输功能。 

@subsection proxy Service Proxy
arpcc 会根据一个service描述生成这个service被调用方的proxy类。
通过调用proxy的全局应该函数dispatch，并传入一个 ProtocolReader 实例和一个proxy派生类实例，
可以将stub整编数据解编成对这个proxy对象成员函数的调用。
使用者需要派生proxy，实现纯虚成员函数具体功能。
@note proxy 只提供解编功能，而不提供具体处理对象的定位。proxy派生类的定位应该由使用者自行实现。


@section safety arpc安全性
如果发送端有安全问题(如game client),应该对一些关键点进行设置。
这些设置只会在接收端生成检测代码。

@par 动态大小参数
尽量不要使用动态大小参数的method，以免受到攻击。
动态参数包括 array 和 string.
如果使用，因该设定array和string允许的最大长度.

*/

#include <string>

typedef signed long long	int64;//S64;
typedef unsigned long long	uint64;//U64;
//typedef double				F64;
//typedef float				F32;
typedef signed int			int32;//S32;
typedef unsigned int		uint32;//U32;
typedef signed short		int16;//S16;
typedef unsigned short		uint16;//U16;
typedef signed char			int8;//S8;
typedef unsigned char		uint8;//U8;
//typedef bool				B8;

//typedef std::string			STRING;

#define S64 int64
#define U64 uint64
#define S32 int32
#define U32 uint32
#define S16 int16
#define U16 uint16
#define S8 int8
#define U8 uint8
#define F64 double
#define F32 float
#define B8 bool
#define STRING std::string
/** 表示一个enum的类型. */
typedef uint8 EnumSize;


#include <vector>
#include <string>
#include <algorithm>
#include <sstream>


#endif//__COMMON_H__
