#ifndef __SCENE_PLAYER_H_____
#define __SCENE_PLAYER_H_____
#include "config.h"
class Player;
class ScenePlayer :public BINChannel<SGE_Player_World2SceneStub , SGE_Player_Scene2WorldProxy >
	, public SGE_Player_Scene2WorldProxy {
public:
#include "SGE_Player_Scene2WorldMethods.h"
public:
	ScenePlayer(){SRV_ASSERT(0);}
	ScenePlayer(Player* p);
private:
	Player* owner_;
};


#endif