#include "handler.h"
#include "config.h"
#include "routine.h"

enum
{
	MAX_NOTICE_LENGTH=400 ///<100个汉字 UTF-8
};

void
ClientHandler::gmNoticeModule(Json::Value& json)
{

	int32 noticeSendType =  ENUM(NoticeSendType).getItemId(json["NoticeSendType"].asString());
	if(noticeSendType < NST_Immediately || noticeSendType > NST_Loop)
	{
		ACE_DEBUG((LM_ERROR, ACE_TEXT("notice-type out-side error\n")));
		result(-1,"Notice send type outside error");
		return;
	}

	std::string str_note = json["Content"].asString();

	if(str_note.length() > MAX_NOTICE_LENGTH)
	{
	
		result(-1,"Content length out-side error");
		ACE_DEBUG((LM_ERROR, ACE_TEXT("Content length out-side error\n")));
		return;
	}

	uint32 thetime = json["TheTime"].asUInt();
	uint32 itvtime = json["ItvTime"].asUInt();


	WorldHandler::instance()->gmtNotice(NoticeSendType(noticeSendType),str_note,thetime,itvtime);

	result(0,"Notice send success");
}