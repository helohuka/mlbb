#include "exam.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"
#include "UtlMath.h"
#include "player.h"
#include "Activity.h"
#include "GameEvent.h"
#include "loghandler.h"

std::vector<ExamTable::ExamData*> ExamTable::edata_;
std::map<U32, ExamTable::ExamCore*> ExamTable::examDate_;

bool ExamTable::load(char const *fn)
{
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);

	for(S32 row=0; row<csv.get_records_counter(); ++row)
	{
		ExamData* pdata = NEW_MEM(ExamData);
		pdata->questionId_ = csv.get_int(row,"ID");
		pdata->answer_	   = csv.get_int(row,"answer");
		edata_.push_back(pdata);
	}

	return true;
}

bool ExamTable::check()
{return true;}

void ExamTable::clear(){
	for(size_t i=0;i<edata_.size(); ++i){
		if(edata_[i])
		{
			ExamData *p = edata_[i];
			DEL_MEM(p);
		}
	}
	edata_.clear();
}

S32
ExamTable::getExamAnswerById(U32 id)
{
	for (size_t i = 0; i < edata_.size(); ++i)
	{
		if(edata_[i]->questionId_ == id)
			return edata_[i]->answer_;
	}
	return -1;
}

void
ExamTable::randQuestion(std::vector<U32>& questions)
{
	for (size_t i = 0; i < Global::get<int>(C_ExamNumMax); ++i)
	{
		U32 index = UtlMath::randN(edata_.size());
		if(edata_[index] == NULL)
		{
			ACE_DEBUG((LM_ERROR,"ExamTable::randQuestion ERROR\n"));
			continue;
		}
		questions.push_back(edata_[index]->questionId_);
	}
}

void
ExamTable::openExam()
{
	for(size_t i=0; i<Player::store_.size(); ++i)
	{
		if(Player::store_[i]->getProp(PT_Level) < Global::get<int>(C_ExamOpenLevel))
			continue;
		U32 playerId = Player::store_[i]->getGUID();
		if(examDate_[playerId] != NULL)
			continue;

		ExamCore* p = NEW_MEM(ExamCore);
		p->questionIndex_ = 0;
		randQuestion(p->questions_);
		examDate_[playerId] = p;

		CALL_CLIENT(Player::store_[i],syncExam(*p));
	}
}

void
ExamTable::closeExam()
{
	std::map<U32,ExamCore*>::iterator itr = examDate_.begin();
	while(itr != examDate_.end())
	{
		COM_Exam* p = itr->second;
		if(p)
			DEL_MEM(p);

		itr++;
	}
	examDate_.clear();
}

void
ExamTable::addExam(Player* player)
{
	if(player->getProp(PT_Level) < Global::get<int>(C_ExamOpenLevel))
		return;

	ExamCore* p = getExam(player->getGUID());
	if(p == NULL)
	{
		ExamCore * pExam = NEW_MEM(ExamCore);
		pExam->questionIndex_ = 1;
		pExam->isget10_		  = false;
		pExam->isget20_		  = false;
		randQuestion(pExam->questions_);
		examDate_[player->getGUID()] = pExam;
		CALL_CLIENT(player,syncExam(*pExam));
		return;
	}
	CALL_CLIENT(player,syncExam(*p));
}

ExamTable::ExamCore*
ExamTable::getExam(U32 playerId)
{
	std::map<U32,ExamCore*>::iterator itr = examDate_.find(playerId);
	if(itr != examDate_.end())
		return itr->second;
	return NULL;
}

void
ExamTable::checkAnswer(Player* player,U32 questionIndex, U8 answer)
{
	if(!DayliActivity::status_[ACT_Exam])
		return;
	U32 playerLevel = player->getProp(PT_Level);
	if(playerLevel < Global::get<int>(C_ExamOpenLevel))
		return;

	ExamCore* p = getExam(player->getGUID());
	if(p == NULL)
		return;
	if(p->questionIndex_ >= Global::get<int>(C_ExamNumMax))
		return;
	if(p->questionIndex_ != questionIndex)
		return;
	bool isRight = false;

	if(questionIndex > p->questions_.size())
		return;

	U8 rightanswer = getExamAnswerById(p->questions_[questionIndex]);
	if(rightanswer == answer)
		isRight = true;
		
	if(isRight)
		p->rightNum_++;
	else
		p->errorNum_++;

	if(p->rightNum_ == 10 && !p->isget10_){
		player->addBagItemByItemId(Global::get<int>(C_ExamRight10),1,false,7);
		p->isget10_ = true;
	}
	else if(p->rightNum_ == 20 && !p->isget20_){
		player->addBagItemByItemId(Global::get<int>(C_ExamRight20),1,false,7);
		p->isget20_ = true;
	}

	if(isRight)
	{
		U32	calcmoney = CALC_EXAM_RIGHT_MONEY(playerLevel);
		U32 calcexp	  = CALC_EXAM_RIGHT_EXP(playerLevel);
		p->money_ += calcmoney;
		p->exp_	  += calcexp;
		player->addExp(calcexp,false);
		player->addMoney(calcmoney);

		SGE_LogProduceTrack track;
		track.playerId_ = player->getGUID();
		track.playerName_ = player->getNameC();
		track.from_ = 7;
		track.money_ = calcmoney;
		track.exp_ = calcexp;
		LogHandler::instance()->playerTrack(track);
	}
	else
	{
		U32	calcmoney = CALC_EXAM_ERROR_MONEY(playerLevel);
		U32 calcexp	  = CALC_EXAM_ERROR_EXP(playerLevel);
		p->money_ += calcmoney;
		p->exp_	  += calcexp;
		player->addExp(calcexp,false);
		player->addMoney(calcmoney);

		SGE_LogProduceTrack track;
		track.playerId_ = player->getGUID();
		track.playerName_ = player->getNameC();
		track.from_ = 7;
		track.money_ = calcmoney;
		track.exp_ = calcexp;
		LogHandler::instance()->playerTrack(track);
	}
	
	p->questionIndex_++;
	COM_Answer cc;
	cc.questionIndex_ = questionIndex;
	cc.money_ = p->money_;
	cc.exp_ = p->exp_;
	cc.isRigth_ = isRight;

	enum {
		ARG0,
		ARG_MAX_,
	};
	GEParam param[ARG_MAX_];
	param[ARG0].type_  = GEP_INT;
	param[ARG0].value_.i = 1;
	GameEvent::procGameEvent(GET_Exam,param,ARG_MAX_,player->getHandleId());
	
	CALL_CLIENT(player,syncExamAnswer(cc));
}