
#ifndef __TASK_H__
#define __TASK_H__

#include "config.h"
#include "SqlCommon.h"
class SQLTask;
class Routine
{
public:
	template < class T > static void create(T *& rhs)
	{
		rhs = new T();
	}
public:
	virtual U32 go(SQLTask *pTask){ return 0;};
	virtual U32 back(){ return 0;};
	virtual U32 getTaskId(){return 0;}
};

enum SQLTaskId
{
	TASK_AUTO,
	TASK_PLAYER,
	TASK_BABY,
	TASK_EMPLOYEE,
	TASK_GUILD,
	TASK_ENDLESSSTAIR,
	TASK_MAIL,
	TASK_FATCH_MAIL,
	TASK_HUNDERD,
	//TASK_IDGEN,
	TASK_ACTIVITY,
	TASK_EMPLOYEEQUEST,
	TASK_SAVEKEYGIFT,
	TASK_SIZE
};

class Routine;
class SQLTask : public ACE_Task<ACE_MT_SYNCH>
{
public:
	static void sinit();
	static void sglob(Routine *pRoutine);
	static void sfini();
	static void spost(Routine *pRoutine);
	static void doback();
	static void ping();
public:
	int init();
	int fini();
	int linesvc();
	int svc(void);
	int push(Routine * ptr);
	int quit();
	DBC *getDBC(){return dbContro_;}
private:
	int back(Routine *p);
	int openSQL();
	int closeSQL();
	DBC	*dbContro_;
	SQLTaskId taskId_;
	static std::vector<SQLTask *>	tasks_;
	static std::queue<Routine *>	queue_;
	static ACE_Recursive_Thread_Mutex mutex_;
	
};

#endif