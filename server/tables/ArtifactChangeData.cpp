#include "ArtifactChangeData.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"

std::map< S32 ,ArtifactChangeTable::ArtifactChangeData* >  ArtifactChangeTable::data_;

bool ArtifactChangeTable::load(char const *fn)
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
	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		ArtifactChangeData * pCore = new ArtifactChangeData;
		
		std::string stt = csv.get_string(row,"Type");
		int stte = ENUM(JobType).getItemId(stt);
		if(-1 == stte)
		{
			SRV_ASSERT(0);	
		}
		pCore->professionType_ = (JobType)stte;
		pCore->diamonds_ = csv.get_int(row,"Diamonds");
		S32 id  = (S32)pCore->professionType_ ;

		data_[id ] = pCore;

	}
	return true;
}

ArtifactChangeTable::ArtifactChangeData const*
ArtifactChangeTable::getArtifactById(S32 id)
{
	return data_[id];
}