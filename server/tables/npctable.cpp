#include "npctable.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"
#include "Quest.h"
std::vector<NpcTable::NpcData* >  NpcTable::cache_;
std::map< S32 , const NpcTable::NpcData* > NpcTable::map_;
std::map< S32 , std::vector< std::vector< const NpcTable::NpcData* > > > NpcTable::mapping_; 
bool
NpcTable::load(char const *fn)
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
	

	for(U32 row=0; row<csv.get_records_counter(); ++row)
	{
		S32 id = csv.get_int(row,"NpcId");

		if(map_[id] != NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("npc has same id in row %d , id is %d\n"),row,id));
			SRV_ASSERT(0);	
		}

		NpcData* pNpc = new NpcData();
		
		pNpc->npcId_ = id;
		{
			std::string strtype = csv.get_string(row,"NpcType");
			if(strtype.empty() || strtype == ""){
				pNpc->npcType_ = NT_None;
			}
			else{
				int itype = ENUM(NpcType).getItemId(strtype);
				pNpc->npcType_ = (NpcType)itype;
			}
		}
		pNpc->sceneId_ = csv.get_int(row,"SceneID");
		pNpc->posi_.x_	= csv.get_float(row,"PosX");
		pNpc->posi_.z_	= csv.get_float(row,"PosZ");
		pNpc->filterLevel_ = csv.get_int(row,"OpenLv");
		
		{
			std::string questids = csv.get_string(row,"QuestID");
			char const * pChar = questids.c_str();
			std::string strToken;

			if(pChar)
			{
				while( TokenParser::getToken( pChar , strToken , ';'))
				{
					if( !strToken.empty() && 0!=strToken.length() )
					{
						pNpc->filterQuest_.push_back(::atoi(strToken.c_str()));
					}
				}
			}
		}

		cache_.push_back(pNpc);
		map_[pNpc->npcId_] = pNpc;
	}

	for(size_t i=0; i<cache_.size(); ++i){
		if(mapping_[cache_[i]->sceneId_].empty())
			mapping_[cache_[i]->sceneId_].resize(NT_Max);
		mapping_[cache_[i]->sceneId_][cache_[i]->npcType_].push_back(cache_[i]);
	}
	
	return true;
}

bool
NpcTable::check()
{
	for(size_t i=0; i<cache_.size(); ++i){
		for(size_t j=0; j<cache_[i]->filterQuest_.size(); ++j){
			//ACE_DEBUG((LM_ERROR,ACE_TEXT("npc has same id in row %d , id is %d\n"),cache_[i]->npcId_,cache_[i]->filterQuest_[j]));
			if(NULL == Quest::getQuestById(cache_[i]->filterQuest_[j])){
				ACE_DEBUG((LM_ERROR,"NPC %d quest %d can not find!!!\n",cache_[i]->npcId_,cache_[i]->filterQuest_[j]));	
				SRV_ASSERT(0);
			}
		}
	}
	return true;
}

NpcTable::NpcData const*
NpcTable::getNpcById(S32 id)
{
	return map_[id];
}

void NpcTable::getNpcs(NpcType type,S32 sceneId, std::vector<const NpcData*>& npcs)
{
	if(mapping_[sceneId].empty())
		return;
	npcs = mapping_[sceneId][type];
}

void NpcTable::getNpcs(NpcType type,std::vector<const NpcData*>& npcs){
	for(size_t i=0;i<cache_.size(); ++i){
		if(cache_[i]->npcType_ == type)
			npcs.push_back(cache_[i]);
	}
}
