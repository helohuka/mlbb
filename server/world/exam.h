#ifndef __EXAM_TABLE_H__
#define __EXAM_TABLE_H__

#include "config.h"

class ExamTable
{
public:
	struct ExamData
	{
		U32		questionId_;
		U32		answer_;
	};

	struct ExamCore : public COM_Exam{
		bool isget10_;
		bool isget20_;
	};

public:
	static bool load(char const *fn);
	static bool check();
	static void clear();
	static S32	getExamAnswerById(U32 id);
public:
	static void randQuestion(std::vector<U32>& questions);
	static void openExam();
	static void closeExam();
	static void addExam(Player* player);
	static ExamTable::ExamCore* getExam(U32 playerId);
	static void checkAnswer(Player* player,U32 questionIndex, U8 answer);
public:
	static std::vector<ExamData*>  edata_;
	static std::map<U32, ExamCore*> examDate_;
};


#endif