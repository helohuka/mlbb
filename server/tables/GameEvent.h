#ifndef __GAMEEVENT_H__
#define __GAMEEVENT_H__

#include "config.h"
#include "ComScriptEvn.h"

class GameEvent
{
public:
	static bool load(const char* filename);

	static bool check();

	static void procGameEvent(GameEventType e, GEParam* paramList, int paramNum, U32 handleID);

public:
	static std::vector<std::string> vEvent_; 
};

#endif
