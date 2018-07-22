//==============================================================================
/**
	@date:		2012:5:4  
	@file: 		TokenParser.h
	@author:	Lucifer
*/
//==============================================================================

#ifndef	__TokenParser_H__
#define	__TokenParser_H__
#include <string>
#include <vector>

#define DEASSEMBLE_PARAMS_I(str, int_value)											\
	if( !TokenParser::getInt( str, int_value ) )										\
	{																				\
	ACE_DEBUG( (LM_ERROR, ACE_TEXT("Error: expect an intergal value\n") ) );	\
	return false;																\
	}
#ifdef DEASSEMBLE_PARAMS_F
#undef DEASSEMBLE_PARAMS_F
#endif
#define DEASSEMBLE_PARAMS_F(str, float_value)										\
	if( !TokenParser::getFloat( str, float_value ) )									\
	{																				\
	ACE_DEBUG( (LM_ERROR, ACE_TEXT("Error: expect an decimal value\n") ) );	\
	return false;																\
	}
#ifdef DEASSEMBLE_PARAMS_B
#undef DEASSEMBLE_PARAMS_B
#endif
#define DEASSEMBLE_PARAMS_B(str, bool_value)										\
	if( !TokenParser::getBool( str, bool_value ) )										\
	{																				\
	ACE_DEBUG( (LM_ERROR, ACE_TEXT("Error: expect an boolean value\n") ) );	\
	return false;																\
	}

#ifdef DEASSEMBLE_PARAMS_S
#undef DEASSEMBLE_PARAMS_S
#endif
#define DEASSEMBLE_PARAMS_S(str, str_value)											\
	if( !TokenParser::getToken( str, str_value ) )										\
	{																				\
	ACE_DEBUG( (LM_ERROR, ACE_TEXT("Error: expect an string value\n") ) );		\
	return false;																\
	}

class TokenParser
{
public:
	//static const char* load_text_file( const char* filename );

	static void parseLines( std::string& src, std::vector<std::string>& lines, bool skipEmpty = true );

	static bool getLine( const char*& str, std::string& line, bool skipEmpty = true );

	static bool parseKeyValue( std::string& line, std::string& key, std::string& value );

	static bool getToken( const char*& str, std::string& token, char addSeps = 0 );

	static bool getInt( const char*& str, int& i );

	static bool getFloat( const char*& str, float& f );

	static bool getBool( const char*& str, bool& b );

	static bool checkToken( const char*& str, const char* token );
};
#endif
