#ifndef __TASK_H__
#define __TASK_H__

#include "config.h"
#include "case.h"
extern std::string ghost;
extern int battleId;
#ifdef STRING
#undef STRING
#endif
#include <ace/Configuration_Import_Export.h>  
typedef TestCase* (CaseMaker)();
class CaseTask : public ACE_Task<ACE_MT_SYNCH>
{
public:
	CaseTask(std::vector<TestCase*> cases);
	~CaseTask();
	
	enum Type{
		None,
		Deconnect,
		MoveTo,
		Max,
	};

	int start();
	int stop();
	int svc(void);
	
	void checkCaseTask(TestCase* pcase);

	uint64	lastTime_;
	std::vector<TestCase*> cases_;
};


class CaseTaskFactory{
public:
	
	SINGLETON_FUNCTION(CaseTaskFactory);
	
	void init();
	void fini();
	void run();
	void initRobot();
	std::vector< CaseTask* > tasks_;
};


#endif