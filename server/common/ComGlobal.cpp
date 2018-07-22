
#include "ComGlobal.h"

std::vector<S32>			Global::integers_;
std::vector<float>			Global::reals_;
std::vector<std::string>	Global::strings_;

class GlobalInit
{
public:
	GlobalInit()
	{
		Global::integers_.resize(C_Max);
		Global::reals_.resize(C_Max);
		Global::strings_.resize(C_Max);
	}

	~GlobalInit()
	{
		Global::integers_.clear();
		Global::reals_.clear();
		Global::strings_.clear();
	}
	
};

static GlobalInit __________________________________________;

