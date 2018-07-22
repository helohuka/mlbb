#ifndef __TABLE_SYSTEM_H__
#define __TABLE_SYSTEM_H__
#include "config.h"
#include "ComEnv.h"


static inline const char* GetTableFilePath(const char * filename) {
	static std::string pathname;
	pathname = Env::get<std::string>(V_TableFolder) + filename;
	return pathname.c_str();
}



class TableSystem{
public:
	SINGLETON_FUNCTION(TableSystem);
public:
	bool Load();
	bool Check();
};

#endif