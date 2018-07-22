#ifndef __EMPLOYEECONFIG_TABLE_H__
#define __EMPLOYEECONFIG_TABLE_H__

#include "config.h"
class Employee;
class EmployeeConfigTable
{
public:
	struct EmployeeConfigData
	{
		U32					Id_;
		U32					employeeId_;
		U32					star_;
		U32					money_;		//升星所需
		std::vector<U32>	equips_;	//升星消耗的装备
	};
public:
	static bool load(char const *fn);
	static bool check();
	static EmployeeConfigData const * getEmployeeConfig(U32 empId,U32 star);
	
public:
	static std::map< S32 , EmployeeConfigData* >  data_;
};

#endif