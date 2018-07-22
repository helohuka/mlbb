#ifndef __TIMER_H__
#define __TIMER_H__

class GameTimer : public ACE_Service_Object
{
	typedef std::vector<GameTimer*> TimerList;
public:
	GameTimer(int64 startt,int64 stopt, float interval = 0.1);
	~GameTimer();
public:
	int handle_timeout(const ACE_Time_Value &current_time, const void *act /* = 0 */);
	virtual bool start(){return true;}
	virtual bool stop(){return true;}
protected:
	bool	isUsed_;
	int32	timerId_;
	bool	isstart_;
	int64	stoptimertamp_;
public:
	
	static void pushtimer(GameTimer* gt);
	static void checkTimers();
private:
	static TimerList timers_;

};

#endif