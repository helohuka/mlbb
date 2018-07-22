
#define GAME_SCRIPT_API( LIB, NAME ) do{int api_##LIB##_##NAME( void* S );ScriptEnv::regGlobalAPI( api_##LIB##_##NAME, #NAME, #LIB );}while(0)