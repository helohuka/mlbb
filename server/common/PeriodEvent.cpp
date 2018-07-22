//==============================================================================
/**
	@date:		2012:7:26
	@file: 		PeriodEvent.cpp
	@author:	Lucifer
*/
//==============================================================================
#include "config.h"
#include "PeriodEvent.h"

#include "ace/OS_NS_time.h"

S64 PeriodEvent::monthlyStart_ = 0;
S64 PeriodEvent::weekStart_ = 0;
S64 PeriodEvent::dailyStart_ = 0;
S64 PeriodEvent::hourlyStart_ = 0;

PeriodEvent::PeriodEvent( PeriodType type, const std::string script, int customPeriod /*= 0*/ ) :	eventType_(type),
customPeriod_(customPeriod),
eventTrigger_(NULL),
eventScript_(script)
{
	if( eventType_ == PT_Customly && customPeriod_ < 10 )
		customPeriod_ = 10;
}
void PeriodEvent::updateDatetime()
{
	time_t cur_time_t;
	ACE_OS::time(&cur_time_t);
	tm* period_time = ACE_OS::localtime(&cur_time_t);
	//period_time->tm_hour	= 0;
	period_time->tm_min		= 0;
	period_time->tm_sec		= 0;
	hourlyStart_ = ACE_OS::mktime(period_time);
	dailyStart_ = hourlyStart_-period_time->tm_hour* 3600;//ACE_OS::mktime(period_time);
	//int span_days = (period_time->tm_wday - GlobalVar::iVars_[GSIVariable::GSI_WeekStartDay]+ 7) % 7;
	//weekStart_ = dailyStart_ - span_days * 24* 3600;
	period_time->tm_mday = 1;
	period_time->tm_hour = 0;
	period_time->tm_min = 0;
	period_time->tm_sec = 0;
	monthlyStart_ = ACE_OS::mktime(period_time);
}

PeriodEvent::~PeriodEvent()
{
	if(eventTrigger_)
		delete eventTrigger_;
}

void PeriodEvent::initTimer( int day, int hour, int minute, int sec /*= 0*/ )
{
	// 获得当前时间。
	time_t cur_time_t;
	ACE_OS::time(&cur_time_t);
	tm* period_time = ACE_OS::localtime(&cur_time_t);
	period_time->tm_hour	= hour;
	period_time->tm_min		= minute;
	period_time->tm_sec		= sec;

	//
	switch(eventType_)
	{
	case PT_Weekly:
		{
			period_time->tm_mday	= 1;
			time_t tmp = ACE_OS::mktime(period_time);
			tm* first_day = ACE_OS::localtime(&tmp);
			int perday = day - first_day->tm_wday;
			if (perday < 0)
				perday += 7;
			period_time->tm_mday += perday;
		}
		break;
	default:
		break;
	}

	triggerDatetime_ = ACE_OS::mktime(period_time);
	while(triggerDatetime_ <= cur_time_t)//已经超时
		calcNextTimer();
}

void PeriodEvent::calcNextTimer()
{
	if(PT_Daily== eventType_)
		triggerDatetime_ += DAY_SECONDS;
	else if(PT_Weekly == eventType_)
		triggerDatetime_ += WEEK_SECONDS;
	else if(PT_Customly == eventType_)
	{
		triggerDatetime_ += customPeriod_;
	}
}

void PeriodEvent::setEventTrigger( BasePeriodEventTrigger* eventTrigger )
{
	eventTrigger_ = eventTrigger; eventTrigger_->init(this);
}

std::string PeriodEvent::getCurShortDate()
{
	time_t cur_time_t;
	ACE_OS::time(&cur_time_t);
	tm* cur_tm = ACE_OS::localtime(&cur_time_t);
	
	//output str
	char buf[128] = {0};
	ACE_OS::strftime(buf, 128, "%Y-%m-%d", cur_tm);

	return std::string(buf);
}
int PeriodEvent::getCurYear()
{
	time_t cur_time_t;
	ACE_OS::time(&cur_time_t);
	tm* cur_tm = ACE_OS::localtime(&cur_time_t);

	return cur_tm->tm_year;
}
int PeriodEvent::getCurMonth()
{
	time_t cur_time_t;
	ACE_OS::time(&cur_time_t);
	tm* cur_tm = ACE_OS::localtime(&cur_time_t);

	return cur_tm->tm_mon;
}
int PeriodEvent::getCurDay()
{
	time_t cur_time_t;
	ACE_OS::time(&cur_time_t);
	tm* cur_tm = ACE_OS::localtime(&cur_time_t);

	return cur_tm->tm_mday;
}
int PeriodEvent::getCurHour()
{
	time_t cur_time_t;
	ACE_OS::time(&cur_time_t);
	tm* cur_tm = ACE_OS::localtime(&cur_time_t);

	return cur_tm->tm_hour;
}
S64 PeriodEvent::getCurHourlyStart()
{
	time_t cur_time_t;
	ACE_OS::time(&cur_time_t);
	tm* cur_tm = ACE_OS::localtime(&cur_time_t);
	cur_tm->tm_min = 0;
	cur_tm->tm_sec = 0;
	S64 curHourStart = ACE_OS::mktime(cur_tm);

	return curHourStart;
}
int PeriodEvent::getCurMinute()
{
	time_t cur_time_t;
	ACE_OS::time(&cur_time_t);
	tm* cur_tm = ACE_OS::localtime(&cur_time_t);

	return cur_tm->tm_min;
}
int PeriodEvent::getCurWeek()
{
	time_t cur_time_t;
	ACE_OS::time(&cur_time_t);
	tm* cur_tm = ACE_OS::localtime(&cur_time_t);

	return cur_tm->tm_wday;
}