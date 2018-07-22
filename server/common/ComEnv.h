
#ifndef __ENV_H__
#define __ENV_H__

#include "config.h"

class Env
{
public:

	static void set(Var index,S32 v)
	{
		integers_[index] = v;
	}
	static void set(Var index,float v)
	{
		reals_[index] = v;
	}
	static void set(Var index,const char* v)
	{
		strings_[index] = v == NULL ? "" : v;
	}
	static void set(Var index,const std::string& v)
	{
		strings_[index] = v;
	}
	
	template<class T>
	static inline T get(Var index){}
	
	static std::vector<S32>			integers_;
	static std::vector<float>		reals_;
	static std::vector<std::string> strings_;
};

template<>
inline S32 Env::get<S32>(Var index)
{
	return integers_[index];
}

template<>
inline float Env::get<float>(Var index)
{
	return reals_[index];
}

template<>
inline std::string Env::get<std::string>(Var index)
{
	return strings_[index];
}

template<>
inline const char* Env::get<const char*>(Var index)
{
	return strings_[index].c_str();
}

#endif