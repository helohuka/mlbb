//=============================================================================
/**
 *  @file	ScriptEnv.h
 *
 *  @date	2012/02/14
 *
 *  @author	zhouliang(zhouliang0605@gmail.com)
 *
 *  @brief	
 */
//=============================================================================

#ifndef __ScriptEnv_H_ // Inc Guard
#define __ScriptEnv_H_
#include "config.h"
#include "ComEnv.h"
#include "ComGlobal.h"
#include "EnumInfo.h"


class GameCharacter;
// A Game engine api function.
typedef int (*GameAPI) (void*);


static inline const char* GetScriptFilePath(const char * filename) {
	static std::string pathname;

	pathname = Env::get<std::string>(V_ScriptFolder) + filename;
	return pathname.c_str();
}


/**
 * @class QScriptEnv
 *
 * @brief Game engine script running environment.
 */
extern "C" 
{
#include "lua.h"
#include "lualib.h"
#include "lauxlib.h"
}

/**
* Game Event Param.
*/
enum EGameEventParamType
{
	GEP_FLOAT,
	GEP_INT,
	GEP_STR,
	GEP_PTR,
	GEP_HANDLE,
	GEP_HANDLE_ARRAY,
	GEP_POS_TABLE,
};

struct GEParam
{
	union
	{
		float			f;
		int				i;
		int				handle;
		const char*		s;
		void*			p;

		std::vector<int> *hArray;
		std::map<S32,S32> *hPosTable;
	}					value_;
	EGameEventParamType	type_;
};


class ScriptEnv
{
public:
	static void init();
	static void shutdown();

	// Register a global type to environment.
	static void regGlobalEnum( EnumInfo &e );

	// Register a game api.
	static void regGlobalAPI( GameAPI apiFunc, const char* name, const char* libName = NULL, lua_State* state = gState );

	/// @name Global var methods.
	/// @{

	//create a global table
	static void createTestTable(const char* tableName);
	static void beginTable();
	static void beginSubTable(const char* tableName);
	static void setTableIndexStr(int index, const char* strIndex, const char* strVal);
	static void setTableIndexNumber(int index, const char* strIndex, float intVal);
	static void setTableIndexBool(int index, const char* strIndex, bool b);
	static void setTableIndexPtr(int index, const char* strIndex, void* ptr);
	static void endSubTalbe();
	static void createTable(const char* tableName);

	// Set a global var
	static void setGlobalInt( const char* varName, int i, lua_State* state = gState );
	static void setGlobalFloat( const char* varName, float f );
	static void setGlobalBool( const char* varName, bool b );
	static void setGlobalString( const char* varName, const char* s );
	static void setGlobalPtr( const char* varName, void* ptr );

	// Get a global var.
	static int		getGlobalInt( const char* varName );
	static float	getGlobalFloat( const char* varName );
	static bool		getGlobalBool( const char* varName );
	static std::string		getGlobalString( const char* varName );
	static void*	getGlobalPtr( const char* varName );

	// Remove a global var.
	static void removeGlobal( const char* varName );

	/// @}


	/// @name Stack operations.
	/// @{

	static int			getStkInt( void*, int i );
	static float		getStkFloat( void*, int i );
	static bool			getStkBool( void*, int i );
	static const char*	getStkString( void*, int i );
	static void			LuaTable2Vector( void* L, int i , std::vector<std::string>& arr);
	static void*		getStkPtr( void*, int i );

	static void			pushStkInt( int i );
	static void			pushStkFloat( float f );
	static void			pushStkBool( bool b );
	static void			pushStkString( const char* );
	static void			pushStkPtr( void* p );
	static void			pushIntArray(const std::vector<S32>& arr);

	/// @}
	

	// Load a chunk of script code to global environment.
	static bool loadFile( const char* filename, std::string	& errMsg );
	static bool loadBuf( const char* chunk, std::string	& errMsg, size_t chunk_size, const char* funcName = NULL, const char* paramList = NULL );
	static bool loadChunk( const char* chunk, std::string	& errMsg, const char* funcName = NULL, const char* paramList = NULL);

	static bool loadScriptProc( const char* chunk, std::string& errMsg, const char* funcName);

	// Call a script function.
	static bool call( const char* funcName, std::string	& errMsg );
	static bool callInt( const char* funcName, int intParam, std::string	& errMsg );
	static bool callBool( const char* funcName, bool bParam, std::string	& errMsg );
	static bool callPtr( const char* funcName, void* pParam, std::string	& errMsg );
	static bool callPtrInt( const char* funcName, void* pParam, int i, std::string	& errMsg );
	static bool callPtrPtr( const char* funcName, void* pParam, void* p, std::string	& errMsg );

	static void callArgsBegin( const char* funcName );
	static bool callArgsEnd( std::string	& errMsg, int argNum );

	// Call a game event proc.
	static bool callGEProc( const char* procName, U32 receiverHandle, GEParam* params, int param_num, std::string& errMsg );
	// Call a game event proc and return a bool
	static bool callGEProc( const char* procName, U32 receiverHandle, GEParam* params, int param_num, int& ret, std::string& errMsg );
	// Call
	static bool callGEProc( const char* procName,int id, GEParam* params, int param_num, std::string& errMsg );

	static bool callGMCmd( U32 accountHander, const char* gmcmd, const char* params, std::string& errMsg );

	static int32 getUsedMemory();
public:
	static lua_State* gState;
};

// C Game Api Param list.
#define P_BEGIN		int _pCnt = 1;
#define P_INT(P)	int P				= ScriptEnv::getStkInt( S, _pCnt++ );
#define P_FLOAT(P)	float P				= ScriptEnv::getStkFloat( S, _pCnt++ );
#define P_BOOL(P)	bool P				= ScriptEnv::getStkBool( S, _pCnt++ );
#define P_STR(P)	std::string	 P		= ScriptEnv::getStkString( S, _pCnt++ );
#define P_PTR(P)	ScriptHandle* P		= ScriptHandle::getScriptHandleById( ScriptEnv::getStkInt( S, _pCnt++ ) );
#define P_ARR(ARR)						  ScriptEnv::LuaTable2Vector( S, _pCnt++, ARR);
#define P_END 

// C Game Api return list.
#define R_BEGIN		int _rCnt = 0;
#define R_INT(I)	ScriptEnv::pushStkInt(I); _rCnt++;
#define R_FLOAT(F)	ScriptEnv::pushStkFloat(F); _rCnt++;
#define R_BOOL(B)	ScriptEnv::pushStkBool(B); _rCnt++;
#define R_STR(S)	ScriptEnv::pushStkString(S); _rCnt++;
#define R_ARR(ARR)	ScriptEnv::pushIntArray(ARR); _rCnt++;
#define R_PTR(P)	ScriptEnv::pushStkInt(P?P->scriptId_:0); _rCnt++;
#define R_END		return _rCnt; 


// Lua GolbeTable Define
#define BEGIN_TABLE							ScriptEnv::beginTable();
#define BEGIN_SUBTABLE(STR_SUBTABLE)		ScriptEnv::beginSubTable(STR_SUBTABLE);
#define P_TABLE_INT(NINDEX, SINDEX, P)		ScriptEnv::setTableIndexNumber( NINDEX, SINDEX, P );
#define P_TABLE_FLOAT(NINDEX, SINDEX, P)	ScriptEnv::setTableIndexNumber( NINDEX, SINDEX, P );
#define P_TABLEP_BOOL(NINDEX, SINDEX, P)	ScriptEnv::setTableIndexBool( NINDEX, SINDEX, P );
#define P_TABLE_STR(NINDEX, SINDEX, P)		ScriptEnv::setTableIndexStr( NINDEX, SINDEX, P );
#define P_TABLE_PTR(NINDEX, SINDEX, P)		ScriptEnv::setTableIndexPtr( NINDEX, SINDEX, P );
#define END_SUBTABLE						ScriptEnv::endSubTalbe();
#define P_TABLE_END(STR_TABLE)				ScriptEnv::createTable(STR_TABLE);

//GameProc Event
#define  BEGIN_GAME_EVENT(E) {	GameEvent _e_ = E; GEParam _geParam[5];			int _rCnt=0;
#define  GAME_EVENT_HANDLE(H)	_geParam[_rCnt].type_ = GEP_HANDLE;				_geParam[_rCnt].value_.handle = H;_rCnt++;
#define  GAME_EVENT_INT(I)		_geParam[_rCnt].type_ = GEP_INT;				_geParam[_rCnt].value_.i = I;	_rCnt++;
#define  GAME_EVENT_FLOAT(F)	_geParam[_rCnt].type_ = GEP_FLOAT;				_geParam[_rCnt].value_.f = F;	_rCnt++;
#define  GAME_EVENT_STR(S)		_geParam[_rCnt].type_ = GEP_STR;				_geParam[_rCnt].value_.s = S;	_rCnt++;
#define  GAME_EVENT_PTR(P)		_geParam[_rCnt].type_ = GEP_PTR;				_geParam[_rCnt].value_.p = P;	_rCnt++;
#define  END_GAME_EVENT(GC)		GC->procGameEvent(_e_, _geParam, _rCnt);	}

// C Game API No return.
#define R_NONE		return 0;
#endif//__QScriptEnv_H_
