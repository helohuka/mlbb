//==============================================================================
/**
@date:		2012:7:26
@file: 		PeriodEvent.h
@author:	Lucifer
*/
//==============================================================================
#ifndef	__PeriodEvent_H__
#define	__PeriodEvent_H__
#include "config.h"
#define DAY_SECONDS		ONE_DAY_SEC
#define WEEK_SECONDS	604800

class PeriodEvent;
class BasePeriodEventTrigger
{
public:
	void init( PeriodEvent* event ){ event_ = event; }
	virtual void run(){};
private:
	PeriodEvent* event_;
};
class PeriodEvent
{
public:
	PeriodEvent(PeriodType type, const std::string script, int customPeriod = 0);
	~PeriodEvent();

	void setEventTrigger(BasePeriodEventTrigger* eventTrigger );
	// 计算第一次时间[idx, hour, minute] 的next时间。
	void initTimer(int day, int hour, int minute, int sec = 0);
	// 计算下一次到时时间。
	void calcNextTimer();

	PeriodType				eventType_;
	std::string				eventScript_;
	S64					triggerDatetime_;	// 秒.ACE_Time_Value.sec().
	int						customPeriod_;
	BasePeriodEventTrigger*	eventTrigger_;

public:
	static void		updateDatetime();
	
	/** 当前时间的整月份*/   /*有一定的误差 需要精确秒数时不能使用 */
	static S64	getMonthStart() { return monthlyStart_; }
	/** 当前时间的整星期*/
	static S64	getWeekStart() { return weekStart_; }
	/** 当前时间的整天*/
	static S64	getDailyStart() { return dailyStart_; }
	/** 当前时间的整小时 向后*/
	static S64	getHourlyStart() { return hourlyStart_; }
	
	/** 返回一个当前日期短时间格式 xxxx-xx-xx */
	static std::string getCurShortDate();
	/** 返回当前长日期长时间格式* xxxx-xx-xx hh:mm:ss */
	static std::string getCurLongDateTime();

	// 返回当前年
	static int getCurYear();
	// 返回当前月
	static int getCurMonth();
	// 返回当前日
	static int getCurDay();
	// 返回当前时
	static int getCurHour();
	// 返回当前分 
	static int getCurMinute();
	// 返回当前星期 
	static int getCurWeek();
	// 返回当前整点时秒数 
	static S64 getCurHourlyStart();

	static S64 monthlyStart_;
	static S64 weekStart_;
	static S64 dailyStart_;
	static S64 hourlyStart_;
};



#endif