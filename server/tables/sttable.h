
#include "config.h"

class StateTable
{
public:
	struct Core
	{
		U32 id_;
		StateType type_;
		U32 level_;
		U32 turn_;			//回合数
		U32 tick_;			//次数
		bool battleDelete_;
		std::string initClass_;
		std::string  updateClass_;
	};

	static bool load(char const *fn);
	static bool check();

	static Core const * getStateById(S32 id);

public:
	static std::map<S32 , Core* >  data_;

};