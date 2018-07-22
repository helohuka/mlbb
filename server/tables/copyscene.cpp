#include "copyscene.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "scenetable.h"
#include "Quest.h"

std::vector<CopyScene::copyTableData> CopyScene::data_;

bool
CopyScene::load(char const *fn)
{
	if(NULL == fn)
		return false;
	if(strlen(fn) == 0)
		return false;
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false)
	{
		return false;
	}

	if(csv.get_records_counter() == 0)
		return false;

	for(U32 row=0; row<csv.get_records_counter(); ++row)
	{
		copyTableData co;
		co.copyLevel_	= csv.get_int(row,"CopyID");
		co.questId_		= csv.get_int(row,"StartTaskID");
		co.endQuest_	= csv.get_int(row,"EndID");
		co.sceneId_		= csv.get_int(row,"SceneID");
		co.copyNum_		= csv.get_int(row,"RefreshFrequency");

		std::string strTmp = csv.get_string(row,"NextSceneID");
		if(!strTmp.empty())
		{
			char const * pChar = strTmp.c_str();
			std::string strToken;
			while( TokenParser::getToken( pChar , strToken , ';'))
			{
				co.nextSecnes_.push_back(atoi(strToken.c_str()));
			}
		}

		data_.push_back(co);
	}
	return true;
}

bool
CopyScene::check()
{
	for (size_t i =0; i < data_.size(); ++i)
	{
		SceneData* sd = SceneTable::getSceneById(data_[i].sceneId_);
		if(!sd)
			return false;
		if(sd->sceneType_ != SCT_Instance)
			return false;
		const Quest* q = Quest::getQuestById(data_[i].questId_);
		if(!q)
			return false;
		if(q->questKind_ != QK_Copy)
			return false;
		const Quest* qend = Quest::getQuestById(data_[i].endQuest_);
		if(!qend)
			return false;
		if(qend->questKind_ != QK_Copy)
			return false;
		if(data_[i].copyLevel_ == 0)
			return false;
		if(data_[i].copyNum_ == 0)
			return false;

		SRV_ASSERT(SceneTable::getSceneById(data_[i].sceneId_));
	}
	return true;
}

U32
CopyScene::getCopyStartQuestById(U32 id){
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if(data_[i].sceneId_ == id)
			return data_[i].questId_;
	}
	return 0;
}

U32
CopyScene::getCopyLevelById(U32 id){
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if(data_[i].sceneId_ == id)
			return data_[i].copyLevel_;
	}
	return 0;
}

U32
CopyScene::getCopyNumById(U32 id){
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if(data_[i].sceneId_ == id)
			return data_[i].copyNum_;
	}
	return 0;
}

bool
CopyScene::isNextCopySecne(U32 copylevel,U32 nextId)
{
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if(data_[i].copyLevel_ == copylevel)
		{
			for (size_t j = 0; j < data_[i].nextSecnes_.size(); ++j)
			{
				if(data_[i].nextSecnes_[j] == nextId)
					return true;
			}
		}
	}
	return false;
}

U32
CopyScene::getCopyAllNumByLevel(U32 playerLevel){
	U32 copycount = 0;
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if(data_[i].copyLevel_ <= playerLevel)
			copycount += data_[i].copyNum_;
	}
	return copycount;
}