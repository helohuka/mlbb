
#ifndef __GLOBAL_H__
#define __GLOBAL_H__

#include "config.h"
#include "global.h"

class Global
{
public:

	static void set(Constant index,S32 v)
	{
		integers_[index] = v;
	}
	static void set(Constant index,float v)
	{
		reals_[index] = v;
	}
	static void set(Constant index,const char* v)
	{
		strings_[index] = v == NULL ? "" : v;
	}
	static void set(Constant index,const std::string& v)
	{
		strings_[index] = v;
	}
	
	template<class T>
	static inline T get(Constant index){}
	
	
	
	static std::vector<S32>			integers_;
	static std::vector<float>		reals_;
	static std::vector<std::string> strings_;
};

template<>
inline S32 Global::get<S32>(Constant index)
{
	return integers_[index];
}

template<>
inline float Global::get<float>(Constant index)
{
	return reals_[index];
}

template<>
inline std::string Global::get<std::string>(Constant index)
{
	return strings_[index];
}

template<>
inline const char* Global::get<const char*>(Constant index)
{
	return strings_[index].c_str();
}

#endif