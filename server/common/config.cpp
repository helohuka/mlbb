#include "config.h"

namespace TimeFormat{
	std::string StrLocalTimeNow(const char* fmt){
		time_t	t;
		ACE_OS::time(&t);	
		tm* lt = ACE_OS::localtime(&t);
		char st[32] = {0} ;
		ACE_OS::snprintf(st , 31 ,fmt, lt->tm_year + 1900 , lt->tm_mon +1 , lt->tm_mday , lt->tm_hour , lt->tm_min , lt->tm_sec );
		return st;
	}

	std::string StrLocalTime(uint64 t64, const char* fmt)
	{
		time_t	t = (time_t)t64;
		tm* lt = ACE_OS::localtime(&t);
		char st[32] = {0} ;
		ACE_OS::snprintf(st , 31 ,fmt, lt->tm_year + 1900 , lt->tm_mon +1 , lt->tm_mday , lt->tm_hour , lt->tm_min , lt->tm_sec );
		return st;
	}
}

namespace String
{
	void Trim(std::string& str, bool left, bool right)
	{
		static const std::string delims = " \t\r\n";
		if (right)
			str.erase(str.find_last_not_of(delims) + 1); // trim right
		if (left)
			str.erase(0, str.find_first_not_of(delims)); // trim left
	}

	void ToLowerCase(std::string& str)
	{
		std::transform(
			str.begin(),
			str.end(),
			str.begin(),
			tolower);
	}

	void ToUpperCase(std::string& str)
	{
		std::transform(
			str.begin(),
			str.end(),
			str.begin(),
			toupper);
	}

	std::vector<std::string > Split(std::string const &str, std::string const&delims, U32 maxSplits )
	{
		std::vector<std::string> ret;
		ret.reserve(maxSplits ? maxSplits + 1 : 1000);

		unsigned int numSplits = 0;
		size_t start, pos;
		start = 0;
		do
		{
			pos = str.find_first_of(delims, start);
			if (pos == start)
			{
				start = pos + 1;
			}
			else if (pos == std::string::npos || (maxSplits && numSplits == maxSplits))
			{
				ret.push_back(str.substr(start));
				break;
			}
			else
			{
				ret.push_back(str.substr(start, pos - start));
				start = pos + 1;
			}
			start = str.find_first_not_of(delims, start);
			++numSplits;

		} while (pos != std::string::npos);
		return ret;
	}

	void Split(const char* line, char delims, std::vector<std::string>& out, bool skip)
	{
		size_t i = 0, p = 0, s = strlen(line);
		char word[1024] = { '\0' };
		if (0 == s)
			return;
		do
		{
			if (line[i] == delims)
			{
				word[p] = '\0';
				out.push_back(word);

				if (!skip)
				{
					word[0] = delims;
					word[1] = '\0';
					out.push_back(word);
				}

				p = 0;
			}
			else
			{
				word[p++] = line[i];
			}
			++i;
		} while (i < s);

		word[p] = '\0';
		out.push_back(word);
	}

	std::string Join(std::vector< std::string > arr, std::string const&delims)
	{
		std::string ret = "";
		if (arr.empty())
			return ret;

		for (size_t i = 0, len = arr.size(); i < len; ++i)
		{
			ret += arr[i];
			if (1 + i != len)
			{
				ret += delims;
			}
		}

		return ret;
	}

	S32 IndexOf(std::string const& str, char ch)
	{
		size_t i = str.find_first_of(ch);
		if (i == std::string::npos)
			return -1;
		return i;
	}

	S32 IndexOf(const char* cstr, char ch)
	{
		for (size_t i = 0, l = strlen(cstr); i < l; ++i)
		{
			if (cstr[i] == ch)
				return i;
		}

		return -1;
	}
}

