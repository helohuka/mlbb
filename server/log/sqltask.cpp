#include "config.h"
#include "sqltask.h"

std::queue<Routine *>	SQLTask::queue_;
std::vector<SQLTask *>	SQLTask::tasks_;
ACE_Recursive_Thread_Mutex SQLTask::mutex_;

extern std::string dbAddr_;
extern std::string dbName_;
extern std::string dbUser_;
extern std::string dbPwd_;

void SQLTask::sinit()
{
	tasks_.resize(TASK_SIZE);
	for(size_t i=0; i<tasks_.size(); ++i)
	{
		tasks_[i] = new SQLTask();
		tasks_[i]->taskId_ = (SQLTaskId)i;
		tasks_[i]->init();
		tasks_[i]->activate();
	}
}

void SQLTask::sglob(Routine *pRoutine)
{
	SQLTask task;
	task.init();
	task.push(pRoutine);
	task.linesvc();
}

void SQLTask::sfini()
{
	for (size_t i=0; i<tasks_.size(); ++i)
	{
		tasks_[i]->fini();
		delete tasks_[i]; ///ERR
	}

}

void SQLTask::doback()
{
	ACE_Guard<ACE_Recursive_Thread_Mutex> guard( mutex_ );
	while(!queue_.empty())
	{
		Routine *pRoutine = queue_.front();
		SRV_ASSERT(pRoutine);
		pRoutine->back();
		queue_.pop();
		delete pRoutine;
	}
}

void SQLTask::spost(Routine * pRoutine)
{
	SQLTask * current = tasks_[pRoutine->getTaskId()];
	SRV_ASSERT(current);
	current->push(pRoutine);
}

int SQLTask::init()
{
	enum
	{
		// 最多可以处理4万消息。
		HighMark=256*1024,
		LowMark	=256*1024
	};
	openSQL();
	msg_queue()->high_water_mark(HighMark);
	msg_queue()->low_water_mark(LowMark);


	return 0;
		//activate(THR_NEW_LWP | THR_JOINABLE | THR_INHERIT_SCHED , 1 );
}

int SQLTask::fini()
{
	ACE_Message_Block* msg  = new ACE_Message_Block();
	putq( msg );
	return 0;
}

int SQLTask::linesvc()
{
	ACE_Message_Block *mb = NULL;
	if(-1!=getq(mb)/*undo*/)
	{
		if( mb->length() == 0 )
		{
			mb->release();
			return 0;
		}
		Routine *pRoutine = NULL;
		if(!ACE_MSG_GET(pRoutine,mb))
			return 0;
		mb->release();
		pRoutine->go(this);
		back(pRoutine);
	}

	return 0;
}

int SQLTask::svc()
{
	
	Logger::instance()->init();
	ACE_DEBUG((LM_INFO,"int SQLTask::svc()\n"));
	//
	for(ACE_Message_Block *mb = NULL;-1!=getq(mb);/*undo*/)
	{
		if( mb->length() == 0 )
		{
			mb->release();
			break;
		}
		Routine *pRoutine = NULL;
		if(!ACE_MSG_GET(pRoutine,mb))
			continue;
		mb->release();
		SRV_ASSERT(pRoutine);
		pRoutine->go(this);
		back(pRoutine);
		//ACE_OS::sleep(ACE_Time_Value(1));
	}

	return 0;
}

int SQLTask::push(Routine * ptr)
{
	ACE_Message_Block* mb  = new ACE_Message_Block( sizeof(Routine*) );
	SRV_ASSERT(ACE_MSG_SET(ptr,mb));
	if( msg_queue()->message_count() > 1000 )
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("DB queue is over 1000  %d!!\n"),(int)taskId_));
	}
	while( putq( mb, (ACE_Time_Value *)&ACE_Time_Value::zero ) == -1 )
	{
		//DB消化能力不足
		ACE_DEBUG((LM_ERROR,ACE_TEXT(" Queue Full!! Num %d action is handling!\n"), msg_queue()->message_count() ));
		ACE_OS::sleep( ACE_Time_Value(1) );
	}

	return 0;
}

int SQLTask::quit()
{
	ACE_Message_Block* mb  = new ACE_Message_Block();
	if( msg_queue()->message_count() > 1000 )
	{
		ACE_DEBUG((LM_ERROR,ACE_TEXT("DB queue is over 1000!!\n")));
	}
	while( putq( mb, (ACE_Time_Value *)&ACE_Time_Value::zero ) == -1 )
	{
		//DB消化能力不足
		ACE_DEBUG((LM_ERROR,ACE_TEXT(" Queue Full!! Num %d action is handling!\n"), msg_queue()->message_count() ));
		ACE_OS::sleep( ACE_Time_Value(1) );
	}
	return 0;
}

int SQLTask::back(Routine *p)
{
	ACE_Guard<ACE_Recursive_Thread_Mutex> guard( mutex_ );
	queue_.push(p);
	return 0;
}

class PingRoutine : public Routine{
public:
	U32 go(SQLTask *pTask){
		DB_EXEC_GUARD
#ifndef USE_SQLITE
		//ACE_DEBUG((LM_INFO, "mysql ping ...\n"));
		std::auto_ptr< sql::PreparedStatement > prep_stmt(pTask->getDBC()->prepareStatement("select 1"));
		prep_stmt->execute();
#endif
		return 0;
		DB_EXEC_UNGUARD_RETURN
	}
};

void SQLTask::ping()
{
#ifndef _WIN32
#define DB_PING_TIME 30;
	static int time = DB_PING_TIME;
	static int currTime = ACE_OS::gettimeofday().sec();
	static int lastTime = currTime;
	lastTime = ACE_OS::gettimeofday().sec();
	time -= (lastTime - currTime);
	currTime = lastTime;
	if(time <= 0)
	{
		for (size_t i=0; i<tasks_.size(); ++i)
		{
			tasks_[i]->push(new PingRoutine());
		}
		time = DB_PING_TIME;
	}
#endif
}

int SQLTask::openSQL()
{
DB_EXEC_GUARD
#if defined(USE_SQLITE)
	dbContro_=new CppSQLite3DB();
	dbContro_->open(dbName_.c_str());
#elif  defined(USE_MYSQL)
	sql::Driver* driver = get_driver_instance();
	SRV_ASSERT(driver);
	dbContro_ = driver->connect(dbAddr_.c_str(), dbUser_.c_str(), dbPwd_.c_str());
	dbContro_->setSchema(dbName_.c_str());
	SRV_ASSERT(dbContro_);
	
#endif
	
DB_EXEC_UNGUARD_RETURN
	return 0;
}

int SQLTask::closeSQL()
{
DB_EXEC_GUARD
	if(dbContro_)
		dbContro_->close();
#if defined(USE_SQLITE)
	delete dbContro_;
#endif
	dbContro_ = NULL;
DB_EXEC_UNGUARD_RETURN
	return -1;
}