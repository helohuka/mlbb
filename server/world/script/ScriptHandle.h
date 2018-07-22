/************************************************************************/
/**
 * @file	handle.h
 * @date	2015-3-2015/03/04 	
 * @author 	Lucifer<hotlala8088@gmail.com>            					
 * */
/************************************************************************/
#ifndef __SCRIPT_HANDLE_H__
#define __SCRIPT_HANDLE_H__

#include "config.h"

class Account;
class Player;
class Entity;
class Baby;
class Monster;
class Employee;
class InnerPlayer;
class Robot;
class ScriptHandle
{
	static U32 idMaker_;
	static std::vector<U32> idStore_;
	
	static inline U32 makeId(){
		if(idStore_.empty())
			return ++idMaker_;
		U32 ret = idStore_.back();
		idStore_.pop_back();
		return ret;
	}

	static inline void storeId(U32 id){
		idStore_.push_back(id);
	}

	static std::map<U32, ScriptHandle*> handles_;

public:
	static ScriptHandle* getScriptHandleById(U32 scriptId);
public:
	ScriptHandle();
	virtual ~ScriptHandle();
public:
	virtual Account*	asAccount()		{return NULL;}
	virtual Player*		asPlayer()		{return NULL;}
	virtual Entity*		asEntity()		{return NULL;}
	virtual Baby*		asBaby()		{return NULL;}
	virtual Monster*	asMonster()		{return NULL;}
	virtual Employee*	asEmployee()	{return NULL;}
	virtual InnerPlayer*asInnerPlayer()	{return NULL;}
	virtual Robot*		asRobot()		{return NULL;}

public:
	U32 getHandleId(){return handleId_;}
public:
	U32 handleId_;
};

#endif