#include "GameEvent.h"
#include "tinyxml/tinyxml.h"
#include "ComScriptEvn.h"

std::vector<std::string> GameEvent::vEvent_;

bool 
GameEvent::load(const char* filename)
{
	if(NULL == filename)
		SRV_ASSERT(0);
	if(strlen(filename) == 0)
		SRV_ASSERT(0);

	TiXmlDocument doc( 	filename );
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), filename ) );
	if ( !doc.LoadFile() )
	{
		ACE_DEBUG( (LM_DEBUG, ACE_TEXT( "Could not load GameEvent file '%s'. ErrorRow:'%d'\n"), filename, doc.ErrorRow()));
		return false;
	}

	vEvent_.resize(SGE_Max);

	TiXmlElement* rootElem=doc.FirstChildElement("GameEvent");

	if (!rootElem)
	{
		return false;
	}

	for (TiXmlElement* subTemp =rootElem->FirstChildElement(); subTemp; subTemp = subTemp->NextSiblingElement() )
	{
		const char* v=subTemp->Value();
		if (v)
		{
			TiXmlNode* node=subTemp->FirstChild();
			if(node)
			{
				TiXmlText* text=node->ToText();
				if (text==NULL)
				{
					ACE_DEBUG( ( LM_ERROR, ACE_TEXT("Game event script null \n") ) );
				}
				std::string funcName("GameEvent_") ;
				funcName += v;

				std::string err;
				if(!ScriptEnv::loadScriptProc( text->Value(), err, funcName.c_str() ))
				{
					ACE_DEBUG( (LM_ERROR, ACE_TEXT("Error when compiling \"ENTER\" script err = \"%s\"\n"), err.c_str()));
					return false;
				}

				S32 intenum = ENUM(GameEventType).getItemId(v);
				vEvent_[intenum] = funcName;
			}
		}
	}

	return true;
}

bool GameEvent::check()
{
	return true;
}

void GameEvent::procGameEvent( GameEventType e, GEParam* paramList, int paramNum, U32 handleID)
{
	if(vEvent_[e].length() )
	{
		std::string err;
		if( !ScriptEnv::callGEProc( vEvent_[e].c_str(), handleID, paramList, paramNum, err ) )
		{
			ACE_DEBUG( (LM_ERROR, ACE_TEXT("GameEventTable::procGameEvent \"%s\"\n"), err.c_str() ) );
		}
	}
}
