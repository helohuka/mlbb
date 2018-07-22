/** File generate by <hotlala8088@gmail.com> 2015/01/13  
 */

#ifndef __CONFIG_H__
#define __CONFIG_H__

#include "predef.h"
#include "Common.h"
#include "GlobalConstants.h"
#include "UtlMath.h"
#include "Logger.h"

#include "ProtocolMemWriter.h"
#include "ace/Channel.h"
#include "ace/BINChannel.h"
#include "ace/Connection.h"
#include "ace/BINConnection.h"
#include "ace/BINCompressConnection.h"
#include "ace/ChannelConnection.h"
#include "ace/BINChannelConnection.h"
#include "ace/BINChannelBroadcaster.h"
#include "env.h"
#include "global.h"
#include "com.h"
#include "struct.h"
#include "proto.h"



#include "tinyxml/tinyxml.h"
void assertPrepare();
#ifdef ACE_ASSERT
#	undef ACE_ASSERT
#	define ACE_ASSERT(X) SRV_ASSERT(X)
#endif

#define DB_EXEC_GUARD							try{
#define DB_EXEC_UNGUARD_RETURN					}catch(std::exception& e){\
	ACE_DEBUG((LM_ERROR, ACE_TEXT("DB exception failed (%s).\n"), e.what()));		\
	SRV_ASSERT(0);\
}

#if defined(WIN32)

#define ACE_REACTOR_MAKE do{static ACE_Select_Reactor __select_reactor; static ACE_Reactor __reactor(&__select_reactor); ACE_Reactor::instance(&__reactor);}while(0)
#else 
#define ACE_REACTOR_MAKE do{}while(0)
#endif

template< class T >
static inline bool ACE_MSG_SET(T value, ACE_Message_Block *mb)
{
	SRV_ASSERT( mb->space() >= sizeof( T ) );
	mb->copy( (const char*)&value, sizeof(T) );
	return true;
}

template< class T >
static inline bool ACE_MSG_GET(T &value, ACE_Message_Block *mb)
{
	if(mb->length() < sizeof(T))
		return false;
	value = *(T*)mb->rd_ptr();
	mb->rd_ptr(sizeof(T));
	return true;
}

#define GET_BIT_BOOL( FLAG, OFFSET ) (FLAG&(1<<OFFSET))
#define SET_BIT_BOOL( FLAG, OFFSET, B ) FLAG = B?(FLAG|(1<<OFFSET)):(FLAG&(~(1<<OFFSET)))
#define COMPARE_BIT_BOOL( FLAG0, FLAG1, OFFSET ) (GET_BIT_BOOL( FLAG0, OFFSET )==GET_BIT_BOOL( FLAG1, OFFSET ))

#define XML_GET_ATTRIBUTE_S(Element, AttributeName, Target_Str)	\
do{															\
	const char* tmp = Element->Attribute(AttributeName);	\
	if(tmp) Target_Str=tmp;							\
}while(0)

#define XML_GET_ATTRIBUTE_I(Element, AttributeName, Target_Int)	\
do{															\
	const char* tmp = Element->Attribute(AttributeName);	\
	if(tmp)Target_Int=::atoi(tmp);						\
}while(0)

#define XML_GET_ATTRIBUTE_F(Element, AttributeName, Target_Float)	\
do{															\
	const char* tmp = Element->Attribute(AttributeName);	\
	if(tmp) Target_Float=(float)::atof(tmp);				\
}while(0)

#define XML_GET_ATTRIBUTE_B( Element,AttributeName, Target_Bool )	\
do{															\
	const char* tmp = Element->Attribute( AttributeName );	\
	if(tmp)	Target_Bool=( ACE_OS::strcasecmp( tmp, "true" ) ? false : true  );\
}while(0)	

#define XML_GET_ATTRIBUTE_E( Element,AttributeName, EnumType,Target_Enum )		\
do{																		\
	const char* tmp=Element->Attribute(AttributeName);					\
	if(tmp)																\
	{																	\
		int e=ENUM(EnumType).getItemId(tmp);							\
		if (-1==e) SRV_ASSERT(0);										\
		Target_Enum=(EnumType)e;										\
	}																	\
}while(0)

#define XML_GET_VALUE_E( Element,EnumType,Target_Enum )		\
do{																		\
	const char* tmp=Element->Value();					\
	if(tmp)																\
	{																	\
		int e=ENUM(EnumType).getItemId(tmp);							\
		if (-1==e) SRV_ASSERT(0);										\
		Target_Enum=(EnumType)e;										\
	}																	\
}while(0)

#define XML_GET_S(Node,Target_Str) do { TiXmlText* text=Node->ToText(); if(text) Target_Str = text->Value();} while (0)

#define CSV_GET_ENUMERATION(VAL,E,CSV,ROW,columnname)\
do \
{\
	std::string name = CSV.get_string(row,columnname);\
	if(name.empty())\
		VAL = (E)0;\
	else\
	{\
		int i = ENUM(E).getItemId(name);\
		VAL = (E)i;\
	}\
} while (0);


inline int32 GetUtf8Len2GBLen(const char* p,int32 len)
{
	SRV_ASSERT(p);
	int result=0;
	while(p!=0&&len>0)
	{
		int step=TiXmlBase::utf8ByteTable[*((const unsigned char*)p)];
		if ( step == 0 )
			SRV_ASSERT(0);
		if (step==1)
		{
			result+=1;
		}
		else
		{
			result+=2;
		}
		p += step;
		len-=step;
	}
	return result;
}

//渠道信息
struct SDKInfo{
	std::string userId_;  ///真实用户ID
	std::string pfName_;  ///渠道名
	std::string pfId_;	  ///渠道ID
	std::string mac_;	  
	std::string idfa_;
	std::string pluginId_;
	std::string deviceType_;
};

namespace TimeFormat{
	 std::string StrLocalTimeNow(const char* fmt = "%04d-%02d-%02d %02d:%02d:%02d");
	
	 std::string StrLocalTime(uint64 t64, const char* fmt = "%04d-%02d-%02d %02d:%02d:%02d");
}

namespace String
{
	 void Trim(std::string& str, bool left, bool right);
	 void ToLowerCase(std::string& str);
	 void ToUpperCase(std::string& str);
	 std::vector<std::string > Split(std::string const &str, std::string const&delims, U32 maxSplits = 0);
	 void Split(const char* line, char delims, std::vector<std::string>& out, bool skip = true);
	 std::string Join(std::vector< std::string > arr, std::string const&delims);
	 S32 IndexOf(std::string const& str, char ch);
	 S32 IndexOf(const char* cstr, char ch);
	
	 template<class T> 
	 inline std::string ToString(T const& v){
		 std::stringstream ss;
		 ss << v;
		 return ss.str();
	 } 
	 template<class T>
	 inline T Convert(std::string const& v){
		 std::stringstream ss;
		 ss << v;
		 T r;
		 ss >> r;
		 return r;
	 }
}

inline COM_FPosition Add(const COM_FPosition& p0, const COM_FPosition& p1){
	COM_FPosition p;
	p.x_ = p1.x_ + p0.x_;
	p.z_ = p1.z_ + p0.z_;
	return p;
}

inline COM_FPosition Sub(const COM_FPosition& p0, const COM_FPosition& p1){
	COM_FPosition p;
	p.x_ = p0.x_ - p1.x_;
	p.z_ = p0.z_ - p1.z_;
	return p;
}

inline float Length(const COM_FPosition& p){
	return sqrtf(p.x_*p.x_ + p.z_*p.z_);
}

inline void Normal(COM_FPosition& p){
	float l = Length(p);
	p.x_/=l;
	p.z_/=l;
}

inline COM_FPosition Scale(const COM_FPosition& p0, float s){
	COM_FPosition p;
	p.x_ = p0.x_ * s;
	p.z_ = p0.z_ * s;
	return p;
}

inline bool IsInRange(const COM_FPosition& p0, const COM_FPosition& p1, const float r)
{
	return Length(Sub(p0,p1)) <= r ;
}

#define SINGLETON_FUNCTION(T) static T* instance(){ static T T##INST; return (&T##INST);}

//执行时间
struct RuntimeProbe{
	RuntimeProbe():start_(0.F),stop_(0.F){}
	inline float begin(){
		return start_ =  ACE_OS::gettimeofday().sec();
	}

	inline float end(){
		return stop_ = ACE_OS::gettimeofday().sec();
	}

	inline float interval(){
		float ret = (stop_ - start_);
		return ret;
	}
private:
	float start_;
	float stop_;
};

//函数探针
struct FunctionProbe : RuntimeProbe{
	FunctionProbe(const char* filename, const char* funcname, int line)
		:filename_(filename)
		,funcname_(funcname)
		,line_(line){
		begin();
	}
	~FunctionProbe(){
		end();
		ACE_DEBUG((LM_INFO,"Function probe:%s(%s:%d) runtime %f\n",funcname_,filename_,line_,interval()));
	}
	const char* filename_;
	const char* funcname_;
	int line_;
};

struct ChunkProbe : RuntimeProbe{
	ChunkProbe(const char* filename)
		:filename_(filename){
	}
	
	inline void _begin(int line){
		begin();
		ACE_DEBUG((LM_INFO,"Chunk probe: %s:%d begin \n",filename_,line));
	}

	inline void _end(int line){
		ACE_DEBUG((LM_INFO,"Chunk probe: %s:%d end \n",filename_,line));
	}
	
	const char* filename_;

};
/* MD5 context. */
struct MD5Context {
	uint64 state[4];                                   /* state (ABCD) */
	uint64 count[2];        /* number of bits, modulo 2^64 (lsb first) */
	uint8 buffer[64];                         /* input buffer */
} ;


void MD5Init(MD5Context *context);
void MD5Update(MD5Context *context, uint8 *input, uint32 inputLen);
void MD5Final(uint8 digest[16], MD5Context *context);
int Base64encode_len(int len);
int Base64encode(char * coded_dst, const char *plain_src,int len_plain_src);
int Base64decode_len(const char * coded_src);
int Base64decode(char * plain_dst, const char *coded_src);


class Player;
class Guild;
struct AnyOauth;
struct SGE_ContactInfoExt;
#ifdef WIN32
typedef std::unordered_map< std::string, Player*> NamePlayerMap;
typedef std::unordered_map< U32, Player*> IdPlayerMap;
typedef std::unordered_map<S32,Guild*> IdGuildMap;
typedef std::unordered_map<std::string,Guild*> NameGuildMap;
typedef std::unordered_map<S32,COM_GuildMember> IdGuildMemberMap;
typedef std::unordered_map<std::string,AnyOauth> AnyOauthMap;
typedef std::unordered_map<std::string, SGE_ContactInfoExt*> NameContentInfoMap;
typedef std::unordered_map<U32, SGE_ContactInfoExt*> IdContentInfoMap;
typedef std::unordered_map<int32, SGE_PlayerEmployeeQuest* > PlayerEmployeeQuestTable;
#else 
typedef std::unordered_map<std::string, Player*> NamePlayerMap;
typedef std::unordered_map<U32, Player*> IdPlayerMap;
typedef std::unordered_map<S32,Guild*> IdGuildMap;
typedef std::unordered_map<std::string,Guild*> NameGuildMap;
typedef std::unordered_map<S32,COM_GuildMember> IdGuildMemberMap;
typedef std::unordered_map<std::string,AnyOauth> AnyOauthMap;
typedef std::unordered_map<std::string, SGE_ContactInfoExt*> NameContentInfoMap;
typedef std::unordered_map<U32, SGE_ContactInfoExt*> IdContentInfoMap;
typedef std::unordered_map<int32, SGE_PlayerEmployeeQuest* > PlayerEmployeeQuestTable;
#endif
typedef std::vector<int32> NpcList;
typedef std::vector<Player*> PlayerList;
typedef std::vector<SGE_ContactInfoExt*> ContextInfoList;
typedef std::vector<SGE_PlayerEmployeeQuest*> PlayerEmployeeQuestList;
#define FUNCTION_PROBE FunctionProbe __FUNCTION_RUNTIME_PROBE__(__FILE__,__FUNCTION__,__LINE__);
#define CHUNK_PROBE_BEGIN(NAME) ChunkProbe __CHUNK_PROBE_##NAME##__(__FILE__); __CHUNK_PROBE_##NAME##__._begin(__LINE__);
#define CHUNK_PROBE_END(NAME) __CHUNK_PROBE_##NAME##__._end();


#define NEW_MEM(T,...)  new T(__VA_ARGS__); //new(MemoryLeaks::Ref().Malloc(sizeof(T),__FILE__,__LINE__))T(__VA_ARGS__);
#define DEL_MEM(V) delete V; //DELHELP(V);

#define SERVER_INIT "/servs/inilize"
#define CDKET_REQ "/servs/cdkey"
#define PUSH_LOG "/insert"

#endif ///__CONFIG_H__
