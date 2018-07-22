#include "config.h"
#include "TableSystem.h"
#include "scenetable.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"
#include "tinyxml/tinyxml.h"
#include "monstertable.h"
#include "npctable.h"
SceneTable::SceneMap SceneTable::scenes_;

ZoneInfo* SceneData::getZone(S32 zoneId){
	for (size_t i=0 ;i<zones_.size(); ++i)
	{
		if(!zones_[i]) continue;
		if(zones_[i]->zoneId_ == zoneId)
			return zones_[i];
	}
	return NULL;
}

bool
SceneTable::load(char const* fn)
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
		U32 id = csv.get_int(row,"sceneId");
		std::string str = csv.get_string(row, "SceneType");		
		SceneType type =  (SceneType)ENUM(SceneType).getItemId(str);
	
		std::string xmlfile(GetTableFilePath(""));
		xmlfile += csv.get_string(row,"scene_xml");
		
		std::string navfile = (GetTableFilePath(""));
		navfile += csv.get_string(row,"NavMesh");
		parse(xmlfile.c_str(),id,type,navfile);

		
	}

	return true;
}

bool
SceneTable::parse(char const *fn, U32 id, SceneType type,std::string navfile)
{
	TiXmlDocument mapFile( fn);
	if( !mapFile.LoadFile(TIXML_ENCODING_UTF8) )
	{
		ACE_DEBUG( (LM_ERROR, ACE_TEXT("Failed to load scene file \"%s\": %s line<%d>\n"), fn, mapFile.ErrorDesc() , mapFile.ErrorRow()));
		return false;
	}

	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Load from file \"%s\"...\n"), fn ) );

	TiXmlElement* elem = mapFile.FirstChildElement( "Scene" );
	ACE_ASSERT( elem );

	SceneData* pScene = new SceneData();
	pScene->sceneType_  = type;
	pScene->sceneId_	= id;
	pScene->navFileName_ = navfile;
	//atoi(elem->Attribute("SceneID"));
	//SRV_ASSERT(id == pScene->sceneId_);
	TiXmlNode* pZoneInfo = elem->FirstChild("ZoneInfo");
	if(pZoneInfo)
	{
		TiXmlElement* pZoneElem = pZoneInfo->ToElement();

		if( pZoneElem == NULL )
			return false;
		for ( TiXmlNode* pZoneNode = pZoneElem->FirstChild("Zone"); pZoneNode; pZoneNode = pZoneNode->NextSibling("Zone"))
		{
			TiXmlElement* pElem = pZoneNode->ToElement();

			if(pElem == NULL)
				continue;

			ZoneInfo* pData = new ZoneInfo;
			
			XML_GET_ATTRIBUTE_I(pElem,"ID",pData->zoneId_);
			XML_GET_ATTRIBUTE_I(pElem,"Rate",pData->prob_);
			XML_GET_ATTRIBUTE_I(pElem,"MonsterMin",pData->rollMin_);
			XML_GET_ATTRIBUTE_I(pElem,"MonsterMax",pData->rollMax_);
			
			XML_GET_ATTRIBUTE_F(pElem,"Radius",pData->zoneRadius_);
			XML_GET_ATTRIBUTE_F(pElem,"CenterX",pData->zoneCenterX_);
			XML_GET_ATTRIBUTE_F(pElem,"CenterZ",pData->zoneCenterZ_);

			std::string strMonsters;
			XML_GET_ATTRIBUTE_S(pElem,"Monsters",strMonsters);
			char const * pChar = strMonsters.c_str();
			std::string strToken;

			if(pChar)
			{
				while( TokenParser::getToken( pChar , strToken , ','))
				{
					if( !strToken.empty() && 0!=strToken.length() )
					{
						pData->monsterClasses_.push_back(strToken);
					}
				}
			}
			pScene->zones_.push_back(pData);
		}
	}

	TiXmlNode* pEntryInfo = elem->FirstChild("EntryInfo");
	if(pEntryInfo)
	{
		TiXmlElement* pEntryElem = pEntryInfo->ToElement();

		if( pEntryElem == NULL )
			return false;
		for ( TiXmlNode* pEntryNode = pEntryElem->FirstChild("Entry"); pEntryNode; pEntryNode = pEntryNode->NextSibling("Entry"))
		{
			TiXmlElement* pElem = pEntryNode->ToElement();

			if(pElem == NULL)
				continue;

			EntryInfo* pentry = new EntryInfo();

			XML_GET_ATTRIBUTE_I(pElem,"ID",pentry->entryId_);
			XML_GET_ATTRIBUTE_F(pElem,"Radius",pentry->entryRadius_);
			XML_GET_ATTRIBUTE_F(pElem,"CenterX",pentry->entryCenterX_);
			XML_GET_ATTRIBUTE_F(pElem,"CenterZ",pentry->entryCenterZ_);
			XML_GET_ATTRIBUTE_I(pElem,"toEntryID",pentry->toEntryId_);
			XML_GET_ATTRIBUTE_I(pElem,"toSceneID",pentry->toSceneId_);
			XML_GET_ATTRIBUTE_B(pElem,"IsBornPos",pentry->isBornPos_);
			
			pScene->entrys_.push_back(pentry);
		}

	}

	TiXmlNode* pFuncPinfo = elem->FirstChild( "FuncPInfo" );
	if(pFuncPinfo)
	{
		TiXmlElement* pFunctionElem = pFuncPinfo->ToElement();

		if( pFunctionElem == NULL )
			return false;
		std::vector<FuncPInfo*> funcs;
		for ( TiXmlNode* pFunctionNode = pFunctionElem->FirstChild("FunctionPoint"); pFunctionNode; pFunctionNode = pFunctionNode->NextSibling("FunctionPoint") )
		{
			TiXmlElement* pFunctionElem = pFunctionNode->ToElement();

			if (pFunctionElem == NULL)
				continue;

			FuncPInfo *fpi = new FuncPInfo();
			std::string tmp ;
			XML_GET_ATTRIBUTE_I(pFunctionElem,"ID",fpi->id_);
			XML_GET_ATTRIBUTE_E(pFunctionElem,"Type",FunctionalPointType,fpi->fpt_);
			XML_GET_ATTRIBUTE_F(pFunctionElem,"PositionX",fpi->x);
			XML_GET_ATTRIBUTE_F(pFunctionElem,"PositionY",fpi->y);
			XML_GET_ATTRIBUTE_F(pFunctionElem,"PositionZ",fpi->z);
			XML_GET_ATTRIBUTE_F(pFunctionElem,"RotationX",fpi->rx);
			XML_GET_ATTRIBUTE_F(pFunctionElem,"RotationY",fpi->ry);
			XML_GET_ATTRIBUTE_F(pFunctionElem,"RotationZ",fpi->rz);
			XML_GET_ATTRIBUTE_F(pFunctionElem,"RotationW",fpi->rw);
			funcs.push_back(fpi);
		}
		if(!funcs.empty()){
			pScene->funcpinfo_.clear();
			pScene->funcpinfo_.resize(funcs.size() + 1);
			for(size_t i=0;i<funcs.size(); ++i){
				pScene->funcpinfo_[funcs[i]->id_] = funcs[i];
			}
		}
	}
	
	scenes_[pScene->sceneId_] = pScene;
	/*ObjFile obj;
	
	if(!obj.load(navfile.c_str())){
		pScene->maxPoint_.x_ = 10;
		pScene->maxPoint_.z_ = 10;
		pScene->minPoint_.x_ = -10;
		pScene->minPoint_.z_ = -10;
		return true;
	}
	float maxX_ = 0.F,maxZ_= 0.F;
	float minX_ = 0.F,minZ_= 0.F;
	float const * vert = obj.getVerts();
	for(int i=0; i<obj.getVertCount(); ++i){
		if(maxX_ < vert[i])
			maxX_ = vert[i];
		else if(minX_ > vert[i])
			minX_ = vert[i];

		if(maxZ_ < vert[i+2])
			maxZ_ = vert[i+2];
		else if(minZ_ > vert[i+2])
			minZ_ = vert[i+2];
	}
	
	pScene->maxPoint_.x_ = maxX_ + 1;
	pScene->maxPoint_.z_ = maxZ_ + 1;
	pScene->minPoint_.x_ = minX_ - 1;
	pScene->minPoint_.z_ = minZ_ - 1;*/
	
	return true;
}

bool
SceneTable::check()
{
	for(SceneMap::iterator sitr = scenes_.begin(); sitr!=scenes_.end(); ++sitr)
	{
		SceneData* ptr = sitr->second;

		for (std::vector< ZoneInfo* >::iterator itor = ptr->zones_.begin(); itor != ptr->zones_.end(); ++itor)
		{
			ZoneInfo* pZone = *itor;

			if (!pZone)
				continue;
				//ACE_DEBUG((LM_ERROR,ACE_TEXT("Cannot find Zone in scene-table!!! Scene(%d)\n"),ptr->sceneId_));
				//SRV_ASSERT(0);
			

			if (pZone->rollMax_ < pZone->rollMin_)
			{
				ACE_DEBUG((LM_ERROR,ACE_TEXT("scene zone monsterMax < monsterMin sceneID = (%d),zoneID=(%d)\n"),ptr->sceneId_, pZone->zoneId_));
				SRV_ASSERT(0);
			}

			for ( U32 i = 0 ; i < pZone->monsterClasses_.size(); ++i )
			{
				if(NULL == MonsterClass::getClassByName(pZone->monsterClasses_[i]))
				{
					ACE_DEBUG((LM_ERROR,ACE_TEXT("Cannot find monsterclass(%s) in monster-table!!! zoneID(%d)\n"),pZone->monsterClasses_[i].c_str(),pZone->zoneId_));
					SRV_ASSERT(0);
				}
			}
		}

		/*for (std::set<U32>::iterator itr=ptr->npcs_.begin(); itr!=ptr->npcs_.end(); ++itr)
		{
			if(NULL == NpcTable::getNpcById(*itr))
			{
				ACE_DEBUG((LM_ERROR,ACE_TEXT("Cannot find npc(%d) in npc-table!!! Scene(%d)\n"),*itr,ptr->sceneId_));
				SRV_ASSERT(0);
			}
		}*/
	}

	return true;
}


SceneData* SceneTable::getHomeScene(){
	for(SceneMap::iterator itr= scenes_.begin(); itr!=scenes_.end(); ++itr){
		if(itr->second->sceneType_ == SCT_Home)
			return itr->second;
	}
	return NULL;
}

SceneData* SceneTable::getGuildHomeScene(){
	for(SceneMap::iterator itr= scenes_.begin(); itr!=scenes_.end(); ++itr){
		if(itr->second->sceneType_ == SCT_GuildHome)
			return itr->second;
	}
	return NULL;
}

SceneData* SceneTable::getGuildBattleScene(){
	for(SceneMap::iterator itr= scenes_.begin(); itr!=scenes_.end(); ++itr){
		if(itr->second->sceneType_ == SCT_GuildBattleScene)
			return itr->second;
	}
	return NULL;
}

SceneData* SceneTable::getNewScene(){
	for(SceneMap::iterator itr= scenes_.begin(); itr!=scenes_.end(); ++itr){
		if(itr->second->sceneType_ == SCT_New)
			return itr->second;
	}
	return NULL;
}