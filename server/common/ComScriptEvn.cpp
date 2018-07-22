//=============================================================================
// QScriptEnv.cpp	2012/02/20	Created by Leon
//=============================================================================
#include "config.h"
#include "ComScriptEvn.h"
//#define  NO_NED_NAMESPACE			//不使用nedalloc的namespace，有了有链接错
//#define NO_MALLINFO 1				//为了能通过编译
//#include "nedmalloc/nedmalloc.h"	//使用nedmalloc做内存分配管理，这个比vc的快很多
#include "TokenParser.h"


static void *LuaAlloc (void *ud, void *ptr, size_t osize, size_t nsize) {
	(void)ud;
	(void)osize;
	if (nsize == 0) {
		if(ptr)
			nedalloc::nedfree(ptr);
		return NULL;
	}
	else
		return nedalloc::nedrealloc(ptr, nsize);
}

lua_State* ScriptEnv::gState = NULL;

int32 ScriptEnv::getUsedMemory(){
	lua_gc(gState, LUA_GCCOLLECT, 0);
	return lua_gc(gState, LUA_GCCOUNTB, 0) + (lua_gc(gState, LUA_GCCOUNT, 0) * 1024);
}


//提供给lua做错误处理
static int panic (lua_State *L)
{
	(void)L;  /* to avoid warnings */
	fprintf(stderr, "PANIC: unprotected error in call to Lua API (%s)\n",
		lua_tostring(L, -1));
	return 0;
}

// Tool to check lua stack.
struct LuaStackChecker
{
	LuaStackChecker( lua_State* s ): state( s ) 
	{
		SRV_ASSERT( state );
		stackTop = lua_gettop( state );
	}
	~LuaStackChecker()
	{
		SRV_ASSERT(stackTop == lua_gettop(state));
	}
	lua_State*	state;
	int			stackTop;
};

#define CHK_LUA_STACK	LuaStackChecker luaStackChecker( gState );

void ScriptEnv::init()
{
	ACE_DEBUG((LM_INFO,"ScriptEnv::init\n"));

	gState = lua_newstate(LuaAlloc, NULL );
	//gState = luaL_newstate();
	SRV_ASSERT( gState );
	if (gState) 
		lua_atpanic(gState ,&panic);
	SRV_ASSERT( gState );
	luaopen_base( gState );
	luaopen_math( gState );
	luaopen_os(gState);
	luaopen_table(gState);
	luaopen_debug(gState);
	regGlobalEnum(ENUM(Var));
	regGlobalEnum(ENUM(Constant));
	regGlobalEnum(ENUM(PropertyType));
	regGlobalEnum(ENUM(BattlePosition));
	regGlobalEnum(ENUM(OrderParamType));
	regGlobalEnum(ENUM(GroupType));
	regGlobalEnum(ENUM(StateType));
	regGlobalEnum(ENUM(ItemMainType));
	regGlobalEnum(ENUM(ItemSubType));
	regGlobalEnum(ENUM(EquipmentSlot));
	regGlobalEnum(ENUM(SkillTargetType));
	regGlobalEnum(ENUM(WeaponType));
	regGlobalEnum(ENUM(SneakAttackType));
	regGlobalEnum(ENUM(BattleType));
	regGlobalEnum(ENUM(OpenSubSystemFlag));
	regGlobalEnum(ENUM(JobType));
	regGlobalEnum(ENUM(ErrorNo));
	regGlobalEnum(ENUM(ActivityType));
	regGlobalEnum(ENUM(StorageType));
	regGlobalEnum(ENUM(EntityType));
	regGlobalEnum(ENUM(NpcType));
	regGlobalEnum(ENUM(QualityColor));
	regGlobalEnum(ENUM(BabyInitGear));
	regGlobalEnum(ENUM(GatherStateType));
	regGlobalEnum(ENUM(WishType));
	regGlobalEnum(ENUM(AchievementType));
	regGlobalEnum(ENUM(RaceType));
	regGlobalEnum(ENUM(EmployeeQuestColor));
}

void ScriptEnv::shutdown()
{
	SRV_ASSERT( gState );
	lua_close( gState );
	gState = NULL;
}

void ScriptEnv::regGlobalEnum( EnumInfo &e )
{
	SRV_ASSERT( gState );
	CHK_LUA_STACK;

	int count = 0;
	for(std::vector<std::string	>::iterator iter=e.items_.begin();iter!=e.items_.end();++iter)
	{
		lua_pushstring( gState, (*iter).c_str() );
		lua_pushnumber( gState, count );
		lua_settable( gState, LUA_GLOBALSINDEX );
		count++;
	}
}

void ScriptEnv::setGlobalInt( const char* varName, int i, lua_State* state )
{
	SRV_ASSERT( state );
	SRV_ASSERT( varName );
	CHK_LUA_STACK;

	lua_pushstring( state, varName );
	lua_pushnumber( state, i );
	lua_settable( state, LUA_GLOBALSINDEX );
}

void ScriptEnv::setGlobalFloat( const char* varName, float f )
{
	SRV_ASSERT( gState );
	SRV_ASSERT( varName );
	CHK_LUA_STACK;

	lua_pushstring( gState, varName );
	lua_pushnumber( gState, f );
	lua_settable( gState, LUA_GLOBALSINDEX );

}

void ScriptEnv::setGlobalBool( const char* varName, bool b )
{
	SRV_ASSERT( gState );
	SRV_ASSERT( varName );
	CHK_LUA_STACK;

	lua_pushstring( gState, varName );
	lua_pushboolean( gState, b?1:0 );
	lua_settable( gState, LUA_GLOBALSINDEX );
}

void ScriptEnv::setGlobalString( const char* varName, const char* s )
{
	SRV_ASSERT( gState );
	SRV_ASSERT( varName );
	SRV_ASSERT( s );
	CHK_LUA_STACK;

	lua_pushstring( gState, varName );
	lua_pushstring( gState, s );
	lua_settable( gState, LUA_GLOBALSINDEX );
}

void ScriptEnv::setGlobalPtr( const char* varName, void* ptr )
{
	SRV_ASSERT( gState );
	SRV_ASSERT( varName );
	CHK_LUA_STACK;

	lua_pushstring( gState, varName );
	lua_pushlightuserdata( gState, ptr );
	lua_settable( gState, LUA_GLOBALSINDEX );
}

int	ScriptEnv::getGlobalInt( const char* varName )
{
	SRV_ASSERT( gState );
	SRV_ASSERT( varName );
	CHK_LUA_STACK;

	lua_pushstring(gState, varName );
	lua_gettable( gState, LUA_GLOBALSINDEX );
	int r = (int)lua_tonumber( gState, -1 );
	lua_pop( gState, 1 );

	return r;
}

float ScriptEnv::getGlobalFloat( const char* varName )
{
	SRV_ASSERT( gState );
	SRV_ASSERT( varName );
	CHK_LUA_STACK;

	lua_pushstring(gState, varName );
	lua_gettable( gState, LUA_GLOBALSINDEX );
	float r = (float)lua_tonumber( gState, -1 );
	lua_pop( gState, 1 );

	return r;
}

bool ScriptEnv::getGlobalBool( const char* varName )
{
	SRV_ASSERT( gState );
	SRV_ASSERT( varName );
	CHK_LUA_STACK;

	lua_pushstring(gState, varName );
	lua_gettable( gState, LUA_GLOBALSINDEX );
	int r = lua_toboolean( gState, -1 );
	lua_pop( gState, 1 );

	return r?true:false;

}

std::string	ScriptEnv::getGlobalString( const char* varName )
{
	SRV_ASSERT( gState );
	SRV_ASSERT( varName );
	CHK_LUA_STACK;

	lua_pushstring(gState, varName );
	lua_gettable( gState, LUA_GLOBALSINDEX );
	std::string	 r = lua_tostring( gState, -1 );
	lua_pop( gState, 1 );

	return r;
}

void* ScriptEnv::getGlobalPtr( const char* varName )
{
	SRV_ASSERT( gState );
	SRV_ASSERT( varName );
	CHK_LUA_STACK;

	lua_pushstring(gState, varName );
	lua_gettable( gState, LUA_GLOBALSINDEX );
	void* r = lua_touserdata( gState, -1 );
	lua_pop( gState, 1 );

	return r;
}

void ScriptEnv::removeGlobal( const char* varName )
{
	SRV_ASSERT( gState );
	SRV_ASSERT( varName );
	CHK_LUA_STACK;

	lua_pushstring( gState, varName );
	lua_pushnil( gState );
	lua_settable( gState, LUA_GLOBALSINDEX );
}


void ScriptEnv::regGlobalAPI( GameAPI apiFunc, const char* name, const char* libName, lua_State* state )
{
	SRV_ASSERT( state );
	SRV_ASSERT( apiFunc );
	SRV_ASSERT( name );
	CHK_LUA_STACK;

	lua_CFunction funcPtr = (lua_CFunction)apiFunc;

	luaL_reg scriptLib[2];
	scriptLib[0].name = name;
	scriptLib[0].func = funcPtr;
	scriptLib[1].name = NULL;
	scriptLib[1].func = NULL;

	luaL_openlib( state, libName, scriptLib, 0 );
	lua_pop( gState, 1 );
}

int	ScriptEnv::getStkInt( void* L, int i )
{
	SRV_ASSERT( gState );
	SRV_ASSERT( gState == (lua_State*)L );
	CHK_LUA_STACK;

	lua_Number num = lua_tonumber( gState, i );

	return (int)num;
}

float ScriptEnv::getStkFloat( void* L, int i )
{
	SRV_ASSERT( gState );
	SRV_ASSERT( gState == (lua_State*)L );
	CHK_LUA_STACK;

	lua_Number num = lua_tonumber( gState, i );

	return (float)num;
}

bool ScriptEnv::getStkBool( void* L, int i )
{
	SRV_ASSERT( gState );
	SRV_ASSERT( gState == (lua_State*)L );
	CHK_LUA_STACK;

	int num = lua_toboolean( gState, i );

	return num?true:false;
}

const char*	ScriptEnv::getStkString( void* L, int i )
{
	SRV_ASSERT( gState );
	SRV_ASSERT( gState == (lua_State*)L );
	CHK_LUA_STACK;


	const char* ret = lua_tostring( gState, i );
	return ret?ret:"";
}

void* ScriptEnv::getStkPtr( void* L, int i )
{
	SRV_ASSERT( gState );
	SRV_ASSERT( gState == (lua_State*)L );
	CHK_LUA_STACK;

	return lua_touserdata( gState, i );
}

void ScriptEnv::LuaTable2Vector( void* L, int i ,std::vector<std::string>& arr)
{
	SRV_ASSERT( gState );
	SRV_ASSERT( gState == (lua_State*)L );
	CHK_LUA_STACK;
	SRV_ASSERT(lua_istable(gState,i));

	int n = luaL_getn(gState, -1);
	for (int p = 1; p <= n; ++p) {
		lua_rawgeti(gState, i, p);
		const char *strName = lua_tostring(gState, -1);
		arr.push_back(strName);
		lua_pop(gState, 1);
	}
}

void ScriptEnv::pushStkInt( int i )
{
	SRV_ASSERT( gState );
	lua_pushnumber( gState, i );
}

void ScriptEnv::pushStkFloat( float f )
{
	SRV_ASSERT( gState );
	lua_pushnumber( gState, f );
}

void ScriptEnv::pushStkBool( bool b )
{
	SRV_ASSERT( gState );
	lua_pushboolean( gState, b?1:0 );
}

void ScriptEnv::pushStkString( const char* s )
{
	SRV_ASSERT( gState );
	SRV_ASSERT( s );
	lua_pushstring( gState, s );
}

void ScriptEnv::pushIntArray(const std::vector<S32>& arr){
	SRV_ASSERT( gState );
	lua_newtable(gState);
	for(size_t k=0; k<arr.size(); ++k)
	{
		lua_pushnumber(gState, k);//压入key
		lua_pushnumber(gState, arr[k]);//压入key
		lua_settable(gState, -3); //把key,vlaue压入table中,并弹出key,value
	}
}

void ScriptEnv::pushStkPtr( void* p )
{

	SRV_ASSERT( gState );
	if( p )
		lua_pushlightuserdata( gState, p );
	else
		lua_pushnil( gState );
}

void ScriptEnv::createTestTable(const char* tableName)
{
	SRV_ASSERT( gState );
	SRV_ASSERT( tableName );
	CHK_LUA_STACK;

	lua_newtable( gState );
	lua_pushnumber(gState, 0);				//压入value
	lua_pushstring(gState, "index");		//压入key
	lua_settable(gState, -3);				//弹出key,value，并设置到table里面去
	lua_setglobal(gState, tableName);		//将table赋值给varName全局变量名
}

void ScriptEnv::beginTable()
{
	SRV_ASSERT( gState );
	lua_newtable( gState);
}

void ScriptEnv::beginSubTable(const char* tableName)
{
	lua_pushstring(gState, tableName);			//压入subtable的key
	lua_newtable( gState );
}

void ScriptEnv::setTableIndexStr(int index, const char* strIndex, const char* strVal)
{
	SRV_ASSERT( gState );
	if( index != -1 )
	{
		lua_pushnumber(gState, index);			//压入key
		lua_pushstring(gState, strVal);			//压入value
	}
	else if( ::strlen(strIndex) != 0 )
	{
		lua_pushstring(gState, strIndex);		//压入key
		lua_pushstring(gState, strVal);			//压入value
	}
	lua_settable(gState, -3);					//弹出key,value，并设置到table里面去
}

void ScriptEnv::setTableIndexNumber(int index, const char* strIndex, float intVal)
{
	SRV_ASSERT( gState );

	if( index != -1 )
	{
		lua_pushnumber(gState, index);			//压入key
		lua_pushnumber(gState, intVal);			//压入value
	}
	else if( ::strlen(strIndex) != 0 )
	{
		lua_pushstring(gState, strIndex);		//压入key
		lua_pushnumber(gState, intVal);			//压入value
	}
	lua_settable(gState, -3);		
}

void ScriptEnv::setTableIndexBool(int index, const char* strIndex, bool b)
{
	SRV_ASSERT( gState );

	if( index != -1 )
	{
		lua_pushnumber(gState, index);			//压入key
		lua_pushboolean(gState, b);				//压入value
	}
	else if( ::strlen(strIndex) != 0 )
	{
		lua_pushstring(gState, strIndex);		//压入key
		lua_pushboolean(gState, b);				//压入value
	}
	lua_settable(gState, -3);		
}

void ScriptEnv::setTableIndexPtr(int index, const char* strIndex, void* ptr)
{
	SRV_ASSERT( gState );

	if( index != -1 )
	{
		lua_pushnumber(gState, index);			//压入value
		lua_pushlightuserdata(gState, ptr);		//压入key
	}
	else if( ::strlen(strIndex) != 0 )
	{
		lua_pushstring(gState, strIndex);		//压入value
		lua_pushlightuserdata(gState, ptr);		//压入key
	}
	lua_settable(gState, -3);		
}

void ScriptEnv::endSubTalbe()
{
	SRV_ASSERT( gState );	
	lua_settable(gState,-3);				//将subtable赋值给nam变量名
}

void ScriptEnv::createTable(const char* tableName)
{
	SRV_ASSERT( gState );
	SRV_ASSERT( tableName );
	lua_setglobal(gState, tableName);			//将table赋值给varName全局变量名
}

#define SCRIPT_CHK_ERR		\
	if( r != 0 )			\
{	errMsg = lua_tostring( gState, -1 );	\
	ACE_DEBUG ((LM_ERROR, ACE_TEXT ("(%t) %s \n"), errMsg.c_str()));\
	lua_pop( gState, 1 );					\
	return false;	}

bool ScriptEnv::loadBuf( const char* chunk, std::string	& errMsg, size_t chunk_size, const char* funcName, const char* paramList )
{
	SRV_ASSERT( gState );
	SRV_ASSERT( chunk );
	CHK_LUA_STACK;

	int r;
	std::string	 chunkText;
	if( funcName )
	{
		chunkText = "function ";
		chunkText += funcName;
		chunkText += "(";
		if( paramList )
			chunkText += paramList;
		chunkText += ")\n";
		chunkText += chunk;
		chunkText += "\n end";

		r = luaL_loadbuffer( gState, chunkText.c_str(), chunkText.length(), NULL );
		SCRIPT_CHK_ERR;
	}
	else
	{
		if( chunk_size == 0 )
			r = luaL_loadbuffer( gState, chunk, ::strlen( chunk ), NULL );
		else
			r = luaL_loadbuffer( gState, chunk, chunk_size, NULL );
		SCRIPT_CHK_ERR;
	}

	// Load success!
	// Run this chunk.
	r = lua_pcall( gState, 0, 0, 0 );
	SCRIPT_CHK_ERR;

	return true;
}

bool ScriptEnv::loadFile( const char* filename, std::string	& errMsg )
{
	int r=0;
	r = luaL_loadfile( gState, filename );
	SCRIPT_CHK_ERR;
	r = lua_pcall( gState, 0, 0, 0 );
	SCRIPT_CHK_ERR;
	return true;
}
bool ScriptEnv::loadChunk( const char* chunk, std::string	& errMsg, const char* funcName, const char* paramList)
{

	SRV_ASSERT( gState );
	SRV_ASSERT( chunk );
	CHK_LUA_STACK;
	
	int r;
	std::string	 chunkText;
	if( funcName )
	{
		chunkText = "function ";
		chunkText += funcName;
		chunkText += "(";
		if( paramList )
			chunkText += paramList;
		chunkText += ")\n";
		chunkText += chunk;
		chunkText += "\n end";
		//ACE_DEBUG((LM_INFO,"%s\n",chunkText.c_str()));
		r = luaL_loadbuffer( gState, chunkText.c_str(), chunkText.length(), NULL );
		SCRIPT_CHK_ERR;
	}
	else
	{
		r = luaL_loadbuffer( gState, chunk, ::strlen( chunk ), NULL );
		SCRIPT_CHK_ERR;
	}

	// Load success!
	// Run this chunk.

	r = lua_pcall( gState, 0, 0, 0 );
	SCRIPT_CHK_ERR;

	return true;
}

void ScriptEnv::callArgsBegin( const char* funcName )
{
	SRV_ASSERT( gState );
	SRV_ASSERT( funcName );
	lua_pushstring( gState, funcName );
	lua_gettable( gState, LUA_GLOBALSINDEX );
}

bool ScriptEnv::callArgsEnd( std::string	& errMsg, int argNum )
{
	SRV_ASSERT( gState );
	int r = lua_pcall( gState, argNum, 0, 0 );
	SCRIPT_CHK_ERR;
	return true;
}

bool ScriptEnv::call( const char* funcName, std::string	& errMsg )
{

	SRV_ASSERT( gState );
	SRV_ASSERT( funcName );
	CHK_LUA_STACK;

	lua_pushstring( gState, funcName );
	lua_gettable( gState, LUA_GLOBALSINDEX );
	int r = lua_pcall( gState, 0, 0, 0 );
	SCRIPT_CHK_ERR;

	return true;
}

bool ScriptEnv::callInt( const char* funcName, int intParam, std::string	& errMsg )
{

	SRV_ASSERT( gState );
	SRV_ASSERT( funcName );
	CHK_LUA_STACK;

	lua_pushstring( gState, funcName );
	lua_gettable( gState, LUA_GLOBALSINDEX );
	lua_pushnumber( gState, intParam );
	int r = lua_pcall( gState, 1, 0, 0 );
	SCRIPT_CHK_ERR;

	return true;
}

bool ScriptEnv::callBool( const char* funcName, bool bParam, std::string	& errMsg )
{

	SRV_ASSERT( gState );
	SRV_ASSERT( funcName );
	CHK_LUA_STACK;

	lua_pushstring( gState, funcName );
	lua_gettable( gState, LUA_GLOBALSINDEX );
	lua_pushboolean( gState, bParam?1:0 );
	int r = lua_pcall( gState, 1, 0, 0 );
	SCRIPT_CHK_ERR;

	return true;
}

bool ScriptEnv::callPtr( const char* funcName, void* pParam, std::string	& errMsg )
{

	SRV_ASSERT( gState );
	SRV_ASSERT( funcName );
	CHK_LUA_STACK;

	lua_pushstring( gState, funcName );
	lua_gettable( gState, LUA_GLOBALSINDEX );
	lua_pushlightuserdata( gState, pParam );
	int r = lua_pcall( gState, 1, 0, 0 );
	SCRIPT_CHK_ERR;

	return true;
}

bool ScriptEnv::callPtrInt( const char* funcName, void* pParam, int i, std::string	& errMsg )
{
	SRV_ASSERT( gState );
	SRV_ASSERT( funcName );
	CHK_LUA_STACK;

	lua_pushstring( gState, funcName );
	lua_gettable( gState, LUA_GLOBALSINDEX );
	lua_pushlightuserdata( gState, pParam );
	lua_pushnumber( gState, i );
	int r = lua_pcall( gState, 2, 0, 0 );

	SCRIPT_CHK_ERR;

	return true;
}

bool ScriptEnv::callPtrPtr( const char* funcName, void* pParam, void* p, std::string	& errMsg )
{

	SRV_ASSERT( gState );
	SRV_ASSERT( funcName );
	CHK_LUA_STACK;

	lua_pushstring( gState, funcName );
	lua_gettable( gState, LUA_GLOBALSINDEX );
	lua_pushlightuserdata( gState, pParam );
	lua_pushlightuserdata( gState, p );
	int r = lua_pcall( gState, 2, 0, 0 );

	SCRIPT_CHK_ERR;

	return true;
}


bool ScriptEnv::callGEProc( const char* procName, U32 receiverHandle, GEParam* params, int param_num, std::string& errMsg )
{
	CHK_LUA_STACK;

	lua_pushstring( gState, procName );
	lua_gettable( gState, LUA_GLOBALSINDEX );

	// pass receiver.
	//lua_pushlightuserdata( gState, receiver );
	lua_pushnumber( gState, receiverHandle );

	// pass the parameters
	for(int i = 0; i < param_num; i++)
	{
		switch(params[i].type_)
		{
		case GEP_FLOAT:
			lua_pushnumber( gState, params[i].value_.f );
			break;
		case GEP_INT:
			lua_pushnumber( gState, params[i].value_.i );
			break;
		//case GEP_BOOL:
		//	lua_pushboolean( gState, params[i].value_.b?1:0 );
		//	break;
		case GEP_HANDLE:
			if( params[i].value_.handle == 0)
				lua_pushnil( gState );
			else
				//lua_pushlightuserdata( gState, params[i].value_.p );
				lua_pushnumber( gState, params[i].value_.handle );
			break;

		case GEP_HANDLE_ARRAY:
			if(params[i].value_.hArray == NULL)
				lua_pushnil( gState );
			else
			{
				lua_newtable(gState);
				for(size_t k=0; k<params[i].value_.hArray->size(); ++k)
				{
					lua_pushnumber(gState, k + 1);//压入key
					lua_pushnumber(gState, (*params[i].value_.hArray)[k]);//压入key
					lua_settable(gState, -3); //把key,vlaue压入table中,并弹出key,value
				}
				//lua_settable(gState,-1);
			}
			break;
		case GEP_POS_TABLE:
			if(params[i].value_.hPosTable == NULL)
				lua_pushnil( gState );
			else
			{
				lua_newtable(gState);
				for(std::map<S32,S32>::iterator itr = params[i].value_.hPosTable->begin(),end = params[i].value_.hPosTable->end();
					itr!=end;
					++itr)
				{
					lua_pushnumber(gState, itr->first);//压入key
					lua_pushnumber(gState, itr->second);//压入key
					lua_settable(gState, -3); //把key,vlaue压入table中,并弹出key,value
				}
				//lua_settable(gState,-1);
			}
			break;
		}		
	}
	int r = lua_pcall( gState, param_num+1, 0, 0 );
	SCRIPT_CHK_ERR;

	return true;
}

bool ScriptEnv::callGEProc( const char* procName, U32 receiverHandle, GEParam* params, int param_num, int& ret, std::string& errMsg )
{
	CHK_LUA_STACK;

	lua_pushstring( gState, procName );
	lua_gettable( gState, LUA_GLOBALSINDEX );

	// pass receiver.
	//lua_pushlightuserdata( gState, receiver );
	lua_pushnumber( gState, receiverHandle );

	// pass the parameters
	for(int i = 0; i < param_num; i++)
	{
		switch(params[i].type_)
		{
		case GEP_FLOAT:
			lua_pushnumber( gState, params[i].value_.f );
			break;
		case GEP_INT:
			lua_pushnumber( gState, params[i].value_.i );
			break;
		//case GEP_BOOL:
		//	lua_pushboolean( gState, params[i].value_.b?1:0 );
		//	break;
		case GEP_HANDLE:
			if( params[i].value_.handle == 0)
				lua_pushnil( gState );
			else
				//lua_pushlightuserdata( gState, params[i].value_.p );
				lua_pushnumber( gState, params[i].value_.handle );
			break;

		case GEP_HANDLE_ARRAY:
			if(params[i].value_.hArray == NULL)
				lua_pushnil( gState );
			else
			{
				lua_newtable(gState);
				for(size_t k=0; k<params[i].value_.hArray->size(); ++k)
				{
					lua_pushnumber(gState, k + 1);//压入key
					lua_pushnumber(gState, (*params[i].value_.hArray)[k]);//压入key
					lua_settable(gState, -3); //把key,vlaue压入table中,并弹出key,value
				}
				//lua_settable(gState,-1);
			}
			break;

		case GEP_POS_TABLE:
			if(params[i].value_.hPosTable == NULL)
				lua_pushnil( gState );
			else
			{
				lua_newtable(gState);
				for(std::map<S32,S32>::iterator itr = params[i].value_.hPosTable->begin(),end = params[i].value_.hPosTable->end();
					itr!=end;
					++itr)
				{
					lua_pushnumber(gState, itr->first);//压入key
					lua_pushnumber(gState, itr->second);//压入key
					lua_settable(gState, -3); //把key,vlaue压入table中,并弹出key,value
				}
				//lua_settable(gState,-1);
			}
			break;
		}		
	}
	int r = lua_pcall( gState, param_num+1, 1, 0 );
	SCRIPT_CHK_ERR;

	if(lua_isboolean(gState,-1))
	{
		ret = (int)lua_toboolean( gState, -1);
	}
	else {
		ret = lua_tointeger( gState, -1);
	}
	// Get return.
	
	lua_pop( gState, 1 );

	return true;
}

bool ScriptEnv::callGEProc( const char* procName,int id, GEParam* params, int param_num, std::string& errMsg )
{
	CHK_LUA_STACK;

	lua_pushstring( gState, procName );
	lua_gettable( gState, LUA_GLOBALSINDEX );

	// pass receiver.
	lua_pushnumber( gState, id );

	// pass the parameters
	for(int i = 0; i < param_num; i++)
	{
		switch(params[i].type_)
		{
		case GEP_FLOAT:
			lua_pushnumber( gState, params[i].value_.f );
			break;
		case GEP_INT:
			lua_pushnumber( gState, params[i].value_.i );
			break;
		case GEP_STR:
			lua_pushstring( gState, params[i].value_.s );
			break;
		//case GEP_BOOL:
		//	lua_pushboolean( gState, params[i].value_.b?1:0 );
		//	break;
		case GEP_HANDLE:
			if( params[i].value_.handle == 0)
				lua_pushnil( gState );
			else
				//lua_pushlightuserdata( gState, params[i].value_.p );
				lua_pushnumber( gState, params[i].value_.handle );// for safe.
			break;

		case GEP_HANDLE_ARRAY:
			if(params[i].value_.hArray == NULL)
				lua_pushnil( gState );
			else
			{
				lua_newtable(gState);
				for(size_t k=0; k<params[i].value_.hArray->size(); ++k)
				{
					lua_pushnumber(gState, k + 1);//压入key
					lua_pushnumber(gState, (*params[i].value_.hArray)[k]);//压入key
					lua_settable(gState, -3); //把key,vlaue压入table中,并弹出key,value
				}
				//lua_settable(gState,-1);
			}
			break;
		}		
	}
	int r = lua_pcall( gState, param_num+1, 0, 0 );
	SCRIPT_CHK_ERR;

	return true;
}

bool ScriptEnv::loadScriptProc( const char* chunk, std::string& errMsg, const char* funcName)
{
	#define SCRIPT_EVENT_ARGS "RECEIVER, ARG0, ARG1, ARG2, ARG3, ARG4, ARG5, ARG6, ARG7, ARG8, ARG9"
	return loadChunk( chunk, errMsg, funcName, SCRIPT_EVENT_ARGS);
}

bool ScriptEnv::callGMCmd( U32 accountHander, const char* gmcmd, const char* params, std::string& errMsg )
{
	CHK_LUA_STACK;

	lua_pushstring( gState, gmcmd );
	lua_gettable( gState, LUA_GLOBALSINDEX );

	// Parse params.
	//lua_pushlightuserdata( gState, player );
	lua_pushnumber( gState, accountHander );
	int param_num = 1;
	std::string		param;
	while( TokenParser::getToken( params, param ) )
	{
		if( param.length() && param[0] >= 48 && param[0] <= 57 )
			lua_pushnumber( gState, atof( param.c_str() ) );
		else
			lua_pushstring( gState, param.c_str() );
		param_num++;
	}

	int r = lua_pcall( gState, param_num, 0, 0 );

	SCRIPT_CHK_ERR;

	return true;
}