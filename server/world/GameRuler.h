#ifndef __GAME_RULER_H__
#define __GAME_RULER_H__

class GameRuler{
public:
	static bool CanPetActivity(class Player*,S32 battleId);
	static enum ErrorNo CanHundredBattle(class Player*,S32 battleId);
};

#endif