#ifndef __RANDNAME_H__
#define __RANDNAME_H__

#include "config.h"

class RandNameTable
{
public:
	static bool load(char const *fn);
	static bool check();
	static std::string randName();

public:
	static std::vector<std::string>	lastName_;
	static std::vector<std::string>	firstName_;
};

#endif