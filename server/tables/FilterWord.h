//==============================================================================
/**
	@date:		2016:1:6  
	@file: 		FilterWord.h
	@author:	lwh
*/
//==============================================================================

#ifndef	__FILTERWORD_H__
#define	__FILTERWORD_H__
#include "config.h"
#define GB_CHAR_LENGTH	2
#define UTF8_CHAR_LENGTH 3

#ifndef USE_CHAR_GB
#define FILTER_CHAR_LENGTH UTF8_CHAR_LENGTH
#else
#define FILTER_CHAR_LENGTH GB_CHAR_LENGTH
#endif

class CSVParser;
typedef std::vector<std::string>				worldList;

class FilterWord
{
public:
	static void clear();
	static bool load( const char* filename );
	static bool replace(std::string& str, bool direct = true);
	static bool isValid(const std::string& str);
	static int findStr( std::string originalStr, std::string filterStr, int& replaceNum );
	static bool strHasSymbols(std::string originalStr);
	static std::map<std::string, worldList*>		keyWrodList;	//key == ascii or unicode code
};

#endif