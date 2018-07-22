//==============================================================================
/**
	@date:		2012:7:12
	@file: 		ComScriptApi.h
	@author:	Lucifer
*/
//==============================================================================
#include "ComScriptHeader.h"

GAME_SCRIPT_API(Sys, log);
GAME_SCRIPT_API(Sys, load_script );

//Env
GAME_SCRIPT_API(Env, setInt);
GAME_SCRIPT_API(Env, setFloat);
GAME_SCRIPT_API(Env, setString);

GAME_SCRIPT_API(Env, getInt);
GAME_SCRIPT_API(Env, getFloat);
GAME_SCRIPT_API(Env, getString);

//Global
GAME_SCRIPT_API(Global, setInt);
GAME_SCRIPT_API(Global, setFloat);
GAME_SCRIPT_API(Global, setString);

GAME_SCRIPT_API(Global, getInt);
GAME_SCRIPT_API(Global, getFloat);
GAME_SCRIPT_API(Global, getString);
