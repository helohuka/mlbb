//==============================================================================
/**
	@date:		2012:5:12  
	@file: 		ItemData.cpp
	@author: 	zhangshuai
*/
//==============================================================================
#include "FilterWord.h"
#include "CSVParser.h"

std::map<std::string, worldList*>	FilterWord::keyWrodList;

//过滤字表标点符号
static std::string utfFilterSymbols = " ,./;'[]-=~!@#$%^&*()+{}|<>?:qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
static std::string nameSymbols = "`-=~!@#$%^&*()+[]\\{}|;':,./<>? \"\r\n";
static std::string ascFilterSymbols = " ,./;'[]-=~!@#$%^&*()+{}|<>?:";

void FilterWord::clear()
{
	keyWrodList.clear();
}

bool FilterWord::load( const char* filename )
{
	CSVParser filterTable;

	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), filename ) );

	if(!filterTable.load_table_file(filename))
	{
		ACE_DEBUG( (LM_ERROR, ACE_TEXT("Failed to load filterword from \"%s\"\n"), filename ) );
		return false;
	}

	clear();


	for( u_int i = 0; i < filterTable.get_records_counter(); i++ )
	{
		std::string filterword = filterTable.get_string(i, "word");

		//get first world;
		std::map< std::string, worldList* >::iterator it;
		std::string key;
		if( isascii(filterword[0]) )
		{
			key = filterword.substr(0, 1);
		}
		else
		{
			key = filterword.substr(0, FILTER_CHAR_LENGTH);
		}

		it = keyWrodList.find( key );
		if( it == keyWrodList.end() )
		{
			worldList* newKeyList = new worldList;
			newKeyList->push_back(filterword);
			keyWrodList[key] = newKeyList;
		}
		else
		{
			keyWrodList[key]->push_back(filterword);
		}
	}
	return true;
}

bool FilterWord::replace(std::string& str, bool direct)
{
	bool isFilter = false;

	if( str.length() == 0 )
		return false;

	std::string transStr, subStr;

	subStr = str;
	//屏蔽字中间的过滤字符 filterSymbols 中需要替换的字符
	int replaceNum = 0;

	std::map< std::string, worldList* >::iterator it;
	std::string key ="";
	int curSubStrIndex = 0;
	while(curSubStrIndex < subStr.length() )
	{
		if( isascii(subStr[curSubStrIndex]) )
		{
			key = tolower(subStr[curSubStrIndex]);
			it = keyWrodList.find( key );
		}
		else
		{
			key = subStr.substr(curSubStrIndex, FILTER_CHAR_LENGTH);
			it = keyWrodList.find( key );
		}
		if( it == keyWrodList.end() )
		{
			curSubStrIndex++;
			continue;
		}
		worldList* wlist = it->second;

		for( size_t i = 0; i < wlist->size();)
		{
			std::string word = (*wlist)[i];
			int id = findStr(subStr, word, replaceNum);
			if( id != -1 && direct)
			{
				return true;
			}
			i++;
			while(id!=-1)
			{
				for( size_t j = 0; j < word.length() + replaceNum; j++)
				{
					subStr[j+id] = '*';
					curSubStrIndex = j+id;
				}
				isFilter = true;
				id = findStr(subStr, word, replaceNum);
			}
		}
		curSubStrIndex++;
	}
	transStr += subStr;
	str = transStr;

	return isFilter;
}

int FilterWord::findStr( std::string originalStr, std::string filterStr, int& symbolNum )
{
	symbolNum = 0;
	if( filterStr.length() == 0 )
		return -1;

	int firstCmpIndex = -1;
	bool isAsc = false;
	for( size_t i = 0; i < originalStr.length();)
	{
		if( isascii(originalStr[i]) )
		{
			//find first same word index
			if( tolower(originalStr[i]) == tolower(filterStr[0]) )
			{
				if( originalStr.length() - i < filterStr.length() )
				{
					return -1;
				}
				else
				{
					firstCmpIndex = i;
					isAsc = true;
					break;
				}
			}
			i++;
		}
		else
		{
			std::string osub = originalStr.substr(i, FILTER_CHAR_LENGTH);
			std::string fsub = filterStr.substr(0, FILTER_CHAR_LENGTH);
			if( filterStr.length() >= FILTER_CHAR_LENGTH && originalStr.substr(i, FILTER_CHAR_LENGTH) == filterStr.substr(0, FILTER_CHAR_LENGTH) )
			{
				if( originalStr.length() < filterStr.length() )
				{
					return -1;
				}
				else
				{
					firstCmpIndex = i;
					break;
				}
			}
			i+=FILTER_CHAR_LENGTH;
		}
	}
	if( firstCmpIndex == -1 )
		return -1;

	if( originalStr.substr(firstCmpIndex, filterStr.length()) == filterStr.substr(0, filterStr.length()) )
	{
		return firstCmpIndex;
	}

	int filterLength = filterStr.length();
	int filterCount = 0;
	//非ASC和ASC 都是一个字符，用来保存字符个数
	for( size_t i = firstCmpIndex; i < originalStr.length();)
	{
		if( isascii(originalStr[i]) )
		{
			if( ascFilterSymbols.find(originalStr[i]) != std::string::npos )
			{
				symbolNum++;
				i++;
				continue;
			}
			if( isascii(filterStr[filterCount]) )
			{
				if( tolower(originalStr[i]) == tolower(filterStr[filterCount]) )
				{
					filterCount++;
					i++;
				}
				else
					return -1;
			}
			else
			{
				return -1;
			}
		}
		else if( originalStr.substr(i, 1) == filterStr.substr(filterCount, 1) )
		{
			filterCount++;
			i++;
		}
		else
		{
			int utf_high = utfFilterSymbols.find(originalStr.substr(i, 1));
			if( utf_high != std::string::npos )
			{
				i++;
				symbolNum++;
				continue;
			}
			return - 1;

		}
		if( filterCount == filterLength )
			return firstCmpIndex;
	}
	return -1;
}

bool FilterWord::strHasSymbols(std::string originalStr)
{
	for( size_t i = 0; i < originalStr.length(); )
	{
		if( isascii(originalStr[i]) )
		{
			if( nameSymbols.find(originalStr[i]) != std::string::npos )
				return true;
			i++;
		}
		else
		{
			i+=2;
		}
	}
	return false;
}

bool FilterWord::isValid( const std::string& str )
{
	std::string tmp(str);

	return replace(tmp);
}
