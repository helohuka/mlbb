//==============================================================================
/**
	@date:		2012:5:4  
	@file: 		TokenParser.cpp
	@author: 	Lucifer
*/
//==============================================================================
#include "config.h"
#include "TokenParser.h"

#define NEXT_LINE	"\r\n"
#define ID_NULL		-1
#define GETFMTSTRING( STR, FMT )	va_list argptr;\
	va_start(argptr,FMT);\
	::vsprintf(STR,FMT,argptr);\
	va_end(argptr);

class TempCStr
{
public:
	static char* str()	{ return strBuf[(count++)&0XFF]; }
	static char* printf( const char* fmt, ... )
	{
		char* temp = str();
		GETFMTSTRING(temp, fmt);
		return temp;
	}
private:
	TempCStr(){}
	static char			strBuf[256][1024];
	static int			count;
};

//定义
char TempCStr::strBuf[256][1024];
int TempCStr::count = 0;

//const char* TokenParser::load_text_file( const char* filename )
//{
//	static std::string text_file;
//	text_file.clear();
//	FILE* in = ACE_OS::fopen (filename, "r");
//	if( in == NULL )
//		return NULL;
//	
//	int filesize = ACE_OS::filesize( in );
//	COM_ASSERT( filesize != -1 );
//	while( filesize )
//	{
//		char c = ACE_OS::fgetc( in );
//		text_file.push_back( c );
//		filesize--;
//	}
//
//	ACE_OS::fclose (in);
//
//	return text_file.c_str();
//}

void TokenParser::parseLines( std::string& src, std::vector<std::string>& lines, bool skipEmpty )
{
	lines.clear();
	std::string s = src;
	std::string line;
	for( int i = s.find( NEXT_LINE ); i != ID_NULL ; )
	{
		line = s.substr(i);//s.substr(i);
		if( line.length() || !skipEmpty )
			lines.push_back( line );
		s = s.substr( i + 2, s.length() );
		i = s.find( NEXT_LINE );
	}
	line = s;
	if( line.length() )
		lines.push_back(line);
}

bool TokenParser::getLine( const char*& str, std::string& line, bool skipEmpty )
{
	while(1)
	{
		if( *str == 0 )
			return false;

		const char* f = strstr( str, NEXT_LINE );
		if( f == NULL )
		{
			line = str;
			str += line.length();
			return true;
		}
		if( f == str )
		{
			str += strlen(NEXT_LINE);
			if( skipEmpty )
				continue;
			else
			{
				line = "";
				return true;
			}
		}
		line = std::string( str, f-str );
		str = f;
		str += strlen( NEXT_LINE );
		return true;
	}
}

bool TokenParser::parseKeyValue( std::string& line, std::string& key, std::string& value )
{
	int eq = line.find("=");
	if( eq == ID_NULL || eq == 0 )
		return false;
	key = line.substr(0, eq );
	//除掉两端的空白字符[空格，跳格]
	{
		size_t l = 0;
		size_t r = key.length();

		while( l<key.length() && ( key[l] == 0X09 || key[l] == 0X20 ) ) l++;
		if( l >= key.length() )
			key = "";

		while( r>=0 && ( key[r] == 0X09 || key[r] == 0X20 ) ) r--;
		if( r < l )
			key = "";

		key = key.substr(l, r);
	}
	//key = key.cutWhiteSpace();
	value = line.substr( eq+1, line.length() );
	//除掉两端的空白字符[空格，跳格]
	{
		size_t l = 0;
		size_t r = value.length();

		while( l<value.length() && ( value[l] == 0X09 || value[l] == 0X20 ) ) l++;
		if( l >= value.length() )
			value = "";

		while( r>=0 && ( value[r] == 0X09 || value[r] == 0X20 ) ) r--;
		if( r < l )
			value = "";

		value = value.substr(l, r);
	}
	//value = value.cutWhiteSpace();

	return true;
}

static bool checkSeparator( char t, const char* seps, int num )
{
	for( int i = 0; i < num; i++ )
	{
		if( t == seps[i] )
			return true;
	}
	return false;
}

bool TokenParser::getToken( const char*& str, std::string& token, char addSeps )
{
	if( !addSeps )
	{
		// Eat space.
		while( 1 )
		{
			if( *str != 0 && (*str == 0X09 || *str == 0X20 ) )
			{
				str++;
				continue;
			}
			if(strncmp( str, NEXT_LINE, strlen(NEXT_LINE) ) == 0)
			{
				str += strlen(NEXT_LINE);
				continue;
			}
			break;
		}

		if( *str == 0 )
			return false;
		char* temp = TempCStr::str();
		char* tempptr = temp;
		while( *str != 0 
			&& *str != 0X09 
			&& *str != 0X20 
			&& strncmp( str, NEXT_LINE, strlen(NEXT_LINE) ) != 0
			)
		{
			*tempptr = *str;
			str++;tempptr++;
		}
		*tempptr = 0;
		token = temp;
		return true;
	}
	else
	{
		if( *str == 0 )
			return false;
		if( *str == addSeps )
		{
			str++;
			token = "";
			return true;
		}

		const char* temp = str;
		while(1)
		{
			str++;
			if( *str == 0 )
			{
				token = std::string( temp, str-temp );
				return true;
			}

			if( *str == addSeps )
			{
				token = std::string( temp, str-temp );
				str++;
				return true;
			}
		}
		return false;
	}
}

bool TokenParser::getInt( const char*& str, int& i )
{
	std::string temp;
	if( getToken( str,temp ) )
	{
		i = ::atoi( (const char*)temp.c_str() );
		return true;
	}
	return false;
}

bool TokenParser::getFloat( const char*& str, float& f )
{
	std::string temp;
	if( getToken( str,temp ) )
	{
		f = (float)::atof( (const char*)temp.c_str() );
		return true;
	}
	return false;
}

bool TokenParser::getBool( const char*& str, bool& b )
{
	std::string temp;
	if( getToken( str,temp ) )
	{
		if( strcmp( temp.c_str(), "true") == 0 )
		{
			b = true;
			return true;
		}
		else if(strcmp( temp.c_str(), "false") == 0)
		{
			b = false;
			return true;
		}
	}
	return false;
}


bool TokenParser::checkToken( const char*& str, const char* token )
{
	const char* saved = str;
	std::string t;
	if( getToken( str, t ) && t == token )
		return true;
	str = saved;
	return false;
}

