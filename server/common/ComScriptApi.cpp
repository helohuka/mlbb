//==============================================================================
/**
	@date:		2012:7:12
	@file: 		ComSripctApi.cpp
	@author:	Lucifer
*/
//==============================================================================
#include "config.h"
#include "ComScriptEvn.h"
#include "ComScriptHeader.h"

GAME_SCRIPT_API(Sys, log)
{
	P_BEGIN;
	P_STR( log_str );
	//ACE_DEBUG( ( LM_DEBUG, ACE_TEXT("SCRIPT LOG>>:%s\n"), log_str.c_str() ) );
	P_END;
	R_NONE;
}
GAME_SCRIPT_API( Sys, load_script )
{
	P_BEGIN;
	P_STR( filename );
	P_END;
	
	std::string revName = GetScriptFilePath(filename.c_str());
	std::string err;
	if( !ScriptEnv::loadFile( revName.c_str(), err ) )
	{
		ACE_DEBUG( (LM_ERROR, ACE_TEXT("Failed to load script file \"%s\":%s\n"), revName.c_str(), err.c_str() ) );
		//SRV_ASSERT(0);
		R_BEGIN;
		R_BOOL(false);
		R_END;
	}
	R_BEGIN;
	R_BOOL(true);
	R_END;
}

//----------------------------------------------------------------------------------------------------
GAME_SCRIPT_API(Env, setInt)
{
	P_BEGIN;
	P_INT(i);
	P_INT(v);
	P_END;

	if(i<V_Max && i>V_None)
		Env::set((Var)i,v);

	R_NONE;
}
GAME_SCRIPT_API(Env, setFloat)
{
	P_BEGIN;
	P_INT(i);
	P_FLOAT(v);
	P_END;
	
	if(i<V_Max && i>V_None)
		Env::set((Var)i,v);
	R_NONE;
}
GAME_SCRIPT_API(Env, setString)
{
	P_BEGIN;
	P_INT(i);
	P_STR(v);
	P_END;
	
	if(i<V_Max && i>V_None)
		Env::set((Var)i,v);
	R_NONE;
}

GAME_SCRIPT_API(Env, getInt)
{
	P_BEGIN;
	P_INT(i);
	P_END;
	
	if(i<V_Max && i>V_None)
	{
		R_BEGIN;
		R_INT(Env::get<S32>((Var)i));
		R_END;
	}
	
	R_BEGIN;
	R_INT(0);
	R_END;


}
GAME_SCRIPT_API(Env, getFloat)
{
	P_BEGIN;
	P_INT(i);
	P_END;

	if(i<V_Max && i>V_None)
	{
		R_BEGIN;
		R_FLOAT(Env::get<float>((Var)i));
		R_END;
	}

	R_BEGIN;
	R_FLOAT(0.F);
	R_END;
}
GAME_SCRIPT_API(Env, getString)
{
	P_BEGIN;
	P_INT(i);
	P_END;

	if(i<V_Max && i>V_None)
	{
		R_BEGIN;
		R_STR(Env::get<const char*>((Var)i));
		R_END;
	}

	R_BEGIN;
	R_STR("");
	R_END;
}

//-----------------------------------------------------------------------------------------------------

GAME_SCRIPT_API(Global, setInt)
{
	P_BEGIN;
	P_INT(i);
	P_INT(v);
	P_END;

	if(i<C_Max && i>C_None)
		Global::set((Constant)i,v);

	R_NONE;
}
GAME_SCRIPT_API(Global, setFloat)
{
	P_BEGIN;
	P_INT(i);
	P_FLOAT(v);
	P_END;

	if(i<C_Max && i>C_None)
		Global::set((Constant)i,v);
	R_NONE;
}
GAME_SCRIPT_API(Global, setString)
{
	P_BEGIN;
	P_INT(i);
	P_STR(v);
	P_END;

	if(i<C_Max && i>C_None)
		Global::set((Constant)i,v);
	R_NONE;
}

GAME_SCRIPT_API(Global, getInt)
{
	P_BEGIN;
	P_INT(i);
	P_END;

	if(i<C_Max && i>C_None)
	{
		R_BEGIN;
		R_INT(Global::get<S32>((Constant)i));
		R_END;
	}

	R_BEGIN;
	R_INT(0);
	R_END;


}
GAME_SCRIPT_API(Global, getFloat)
{
	P_BEGIN;
	P_INT(i);
	P_END;

	if(i<C_Max && i>C_None)
	{
		R_BEGIN;
		R_FLOAT(Global::get<float>((Constant)i));
		R_END;
	}

	R_BEGIN;
	R_FLOAT(0.F);
	R_END;
}
GAME_SCRIPT_API(Global, getString)
{
	P_BEGIN;
	P_INT(i);
	P_END;

	if(i<C_Max && i>V_None)
	{
		R_BEGIN;
		R_STR(Global::get<const char*>((Constant)i));
		R_END;
	}

	R_BEGIN;
	R_STR("");
	R_END;
}
