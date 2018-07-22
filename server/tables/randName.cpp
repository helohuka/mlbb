#include "randName.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"

std::vector<std::string>	RandNameTable::lastName_;
std::vector<std::string>	RandNameTable::firstName_;

bool
RandNameTable::load(char const *fn)
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
		std::string lastName = csv.get_string(row,"lastname");

		if(lastName.length() == 0)
		{
			SRV_ASSERT(0);	
		}

		lastName_.push_back(lastName);

		std::string firstName = csv.get_string(row,"name");
		
		if(firstName.length() == 0)
		{
			SRV_ASSERT(0);	
		}

		firstName_.push_back(firstName);
	}
	return true;
}

bool
RandNameTable::check()
{
	return true;
}

std::string
RandNameTable::randName()
{
	std::string tmpName = "";

	U32 tmp1 = UtlMath::randN(lastName_.size());

	std::string tmpLast = lastName_[tmp1];

	tmpName.append(tmpLast.c_str());

	U32 tmp2 = UtlMath::randN(firstName_.size());

	std::string tmpFirst = firstName_[tmp2];

	tmpName.append(tmpFirst.c_str());

	return tmpName;
}