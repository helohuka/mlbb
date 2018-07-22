
#include "ComEnv.h"
#include "config.h"

std::vector<S32>			Env::integers_;
std::vector<float>			Env::reals_;
std::vector<std::string>	Env::strings_;

class EnvInit
{
public:
	EnvInit()
	{
		Env::integers_.resize(V_Max);
		Env::reals_.resize(V_Max);
		Env::strings_.resize(V_Max);
	}

	~EnvInit()
	{
		Env::integers_.clear();
		Env::reals_.clear();
		Env::strings_.clear();
	}
	
};

static EnvInit __________________________________________;

