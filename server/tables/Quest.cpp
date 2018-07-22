
#include "config.h"
#include "ComEnv.h"
#include "ComGlobal.h"
#include "ComScriptEvn.h"

#include "Quest.h"
#include "npctable.h"
#include "scenetable.h"
#include "CSVParser.h"
#include "TokenParser.h"
std::string static inline CompileScript(std::string &chunk, S32 questid, char const *fix)
{
	char id_str[128]={0};
	sprintf(id_str, "Quest_%d_", questid);

	std::string func = id_str;				
	std::string err;

	func += fix;
	if(!ScriptEnv::loadScriptProc( chunk.c_str(), err, func.c_str()))
	{
		SRV_ASSERT(0);
	}
	return func;
}

std::vector<Quest*> Quest::cache_;
std::map<S32, Quest*> Quest::map_;
std::vector<Quest*> Quest::tongji_;
std::vector<Quest*> Quest::rand_;
bool
Quest::load(char const *fn)
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
	clear();
	for (U32 row=0; row<csv.get_records_counter(); ++row)
	{
		Quest* q = new Quest();
		q->questId_ = csv.get_int(row,"QuestId");
		SRV_ASSERT(getQuestById(q->questId_) == NULL);

		CSV_GET_ENUMERATION(q->questKind_,QuestKind,csv,row,"QuestKind");
		CSV_GET_ENUMERATION(q->questType_,QuestType,csv,row,"QuestType");
		
		q->needLevel_ = csv.get_int(row,"NeedLevel");
		q->acceptNpcId_ = csv.get_int(row,"StartNPC");
		q->submitNpcId_ = csv.get_int(row,"FinishNPC");

		CSV_GET_ENUMERATION(q->requireType_,RequireType,csv,row,"RequireType");
		q->requirement_ = csv.get_int(row,"Requirement");
		
		{
			std::string strPrev = csv.get_string(row,"PreQuest");
			char const * ptr = strPrev.c_str();
			std::string token;

			if(!strPrev.empty())
			{
				while( TokenParser::getToken( ptr , token , ';'))
				{	
					S32 id = ::atoi(token.c_str());
					q->prevQuest_.push_back(id);
				}
			}
		}

		{
			std::string strPrev = csv.get_string(row,"NeedItemID");
			char const * ptr = strPrev.c_str();
			std::string token;

			if(!strPrev.empty())
			{
				while( TokenParser::getToken( ptr , token , ';'))
				{	
					S32 id = ::atoi(token.c_str());
					q->needItemId_.push_back(id);
				}
			}
		}

		{
			std::string strPrev = csv.get_string(row,"Target");
			char const * ptr = strPrev.c_str();
			std::string token;

			if(!strPrev.empty())
			{
				while( TokenParser::getToken( ptr , token , ';'))
				{	
					S32 id = ::atoi(token.c_str());
					q->target_.push_back(id);
				}
			}
		}

		{
			std::string strPrev = csv.get_string(row,"TargetNum");
			char const * ptr = strPrev.c_str();
			std::string token;

			if(!strPrev.empty())
			{
				while( TokenParser::getToken( ptr , token , ';'))
				{	
					S32 id = ::atoi(token.c_str());
					q->targetNum_.push_back(id);
				}
			}
		}

		if(q->target_.size() != q->targetNum_.size())
		{
			ACE_DEBUG( ( LM_ERROR, ACE_TEXT("QuestId[%d] q->target_.size() != q->targetNum_.size()\n"), q->questId_ ) );
			SRV_ASSERT(0);
		}

		q->dropId_ = csv.get_int(row,"DropID");
		q->attenuationdrop_ = csv.get_int(row,"DropID2");
		q->title_ =  csv.get_int(row,"Title");
		q->postQuestId_ = csv.get_int(row,"PostQuest");
		
		{
			std::string bossnpc = csv.get_string(row,"Bossnpc");
			if(!bossnpc.empty()){
				char const * ptr = bossnpc.c_str();
				std::string token;
				TokenParser::getToken( ptr , token , ';');
				q->targetSceneId_ = atoi(token.c_str());
				TokenParser::getToken( ptr , token , ';');
				q->targetNpcId_ = atoi(token.c_str());
			}
		}

		std::string str = csv.get_string(row,"JobType");
		if(str.empty())
			q->jt_ = JT_None;
		else
		{
			int enumint = ENUM(JobType).getItemId(str);
			if(-1 == enumint)
				q->jt_ = JT_None;
			else
				q->jt_ = (JobType)enumint;
		}
		q->jl_ = csv.get_int(row,"JobLevel");

		{
			std::string script = csv.get_string(row,"AcceptScript");
			q->acceptScript_= CompileScript(script,q->questId_,"Accept");
		}
		
		{
			std::string script = csv.get_string(row,"SubmitScript");
			q->submitScript_= CompileScript(script,q->questId_,"Submit");
		}
		if(q->questKind_ == QK_Tongji)
			tongji_.push_back(q);
		else if (q->questKind_ == QK_Rand)
			rand_.push_back(q);
		cache_.push_back(q);
		map_.insert(std::pair<S32,Quest*>(q->questId_,q));
	}

	for(size_t i=0; i<cache_.size(); ++i)
	{
		for (size_t j=0; j<cache_[i]->prevQuest_.size(); ++j)
		{
			if( 0 == cache_[i]->prevQuest_[j])
				continue;
			Quest* q = map_[cache_[i]->prevQuest_[j]];
			SRV_ASSERT(q);
			q->nextQuest_.push_back(cache_[i]->questId_);
		}
	}

	return true;
}

void Quest::clear(){
	for(size_t i=0; i<cache_.size(); ++i)
	{
		if(cache_[i]){
			delete cache_[i];
		}
	}
	cache_.clear();
	map_.clear();
	tongji_.clear();
}

bool Quest::check(){
	for(size_t i=0; i<cache_.size(); ++i)
	{
		if(cache_[i]->acceptNpcId_){
			SRV_ASSERT(NpcTable::getNpcById(cache_[i]->acceptNpcId_));
		}
		if(cache_[i]->submitNpcId_){
			SRV_ASSERT(NpcTable::getNpcById(cache_[i]->submitNpcId_));
		}
		if(cache_[i]->targetNpcId_){
			SRV_ASSERT(NpcTable::getNpcById(cache_[i]->targetNpcId_));
		}
		if(cache_[i]->targetSceneId_){
			SRV_ASSERT(SceneTable::getSceneById(cache_[i]->targetSceneId_));
		}
	}

	return true;
}

class QuestLevelSort
{
public:
	bool operator()(Quest* l, Quest* r)
	{
		if(l->needLevel_ > r->needLevel_)
			return true;
		else if(l->needLevel_ < r->needLevel_)
			return false;
		else 
			return false;
	}
};

const Quest* Quest::randomTongjiQuest(S32 minlevel){
	std::vector<Quest*> tmp;
	for (size_t i=0;i<tongji_.size();++i)
	{
		if(tongji_[i]->needLevel_ <= minlevel)
			tmp.push_back(tongji_[i]);
	}
	if(tmp.empty())
		return NULL;

	QuestLevelSort st;
	std::sort(tmp.begin(),tmp.end(),st);
	S32 lv = tmp[0]->needLevel_;
	tmp.clear();
	for (size_t i=0;i<tongji_.size();++i)
	{
		if(tongji_[i]->needLevel_ == lv)
			tmp.push_back(tongji_[i]);
	}
	int rdm = UtlMath::randN(tmp.size());
	return tmp[rdm];
}

const Quest* Quest::randomRandQuest(){
	int rdm = UtlMath::randN(rand_.size());
	return rand_[rdm];
}
