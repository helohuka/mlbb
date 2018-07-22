#include "config.h"
#include "timer.h"

GameTimer::GameTimer(int64 startt , int64 stopt, float interval /* = 0 */)
:isUsed_(true)
,isstart_(false)
,stoptimertamp_(stopt){
	ACE_Time_Value t;
	t.set(interval);
	
	int64 delay = startt - ACE_OS::gettimeofday().sec();
	ACE_Time_Value d;
	d.set(delay);

	timerId_ = ACE_Reactor::instance()->schedule_timer(this,NULL,d,t);
}

GameTimer::~GameTimer(){
	ACE_Reactor::instance()->cancel_timer(this);
}

int GameTimer::handle_timeout(const ACE_Time_Value &current_time, const void *act /* = 0 */){
	if(!isstart_){
		isstart_ = true;
		start();
	}
	int64 currTime = ACE_OS::gettimeofday().sec();
	if(stoptimertamp_ < currTime){
		isUsed_ = false;
		stop();
		return -1;
	}
	return 0;
}

void GameTimer::pushtimer(GameTimer* gt){
	if(!gt)
		return;
	timers_.push_back(gt);
}

void GameTimer::checkTimers(){
	for (size_t i=0; i<timers_.size(); ++i)
	{
		if(timers_[i]->isUsed_ ==false){
			delete timers_[i];
			timers_.erase(timers_.begin() + i--);
		}
	}
}

GameTimer::TimerList GameTimer::timers_;