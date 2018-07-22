#include "config.h"
#include "DailyReward.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"
#include "itemtable.h"
std::vector< std::vector<S32> > DailyReward::monthReward_;


bool DailyReward::load(char const *fn)
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
	monthReward_.clear();
	enum { MaxSize = 12 + 1};
	monthReward_.resize(MaxSize);
	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		S32 month = csv.get_int(row,"month");	
		std::vector<S32> rewards;
		rewards.push_back(0);
		std::string rewardsstr = csv.get_string(row,"reward");
		
		{
			const char* pchar = rewardsstr.c_str();
			std::string reward;
			while( TokenParser::getToken( pchar , reward , ';'))
			{
				rewards.push_back(atoi(reward.c_str()));
			}
		}

		monthReward_[month] = rewards;
	}

	return true;
}

bool DailyReward::check()
{
	for (size_t i=1; i< monthReward_.size(); ++i)
	{
		for (size_t j=1; j<monthReward_[i].size(); ++j)
		{
			SRV_ASSERT(ItemTable::getItemById(monthReward_[i][j]) != NULL);
		}
	}

	return true;
}
std::vector< std::vector<DiamondsConfig::Config> > DiamondsConfig::configs_;

bool DiamondsConfig::load(char const *fn){
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

	configs_.resize(DBT_Type_Max);

	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		Config cf;
		cf.configId_ = csv.get_int(row,"ID");
		cf.classType_ = (DiamondConfigClass)ENUM(DiamondConfigClass).getItemId(csv.get_string(row,"Class"));
		cf.type_ = (DiamondConfigType)ENUM(DiamondConfigType).getItemId(csv.get_string(row,"Type"));
		cf.val0_ = csv.get_int(row,"vol_1");
		cf.val1_ = csv.get_int(row,"vol_2");
		cf.diam_ = csv.get_int(row,"diamonds");

		configs_[cf.classType_].push_back(cf);
	}

	return true;
}

const DiamondsConfig::Config* DiamondsConfig::getCondig(DiamondConfigClass clazz, int times){
	DiamondsConfig::Config* cfg = NULL;
	for(size_t i=0; i<configs_[clazz].size(); ++i){
		if(configs_[clazz][i].val0_ == times)
			return &configs_[clazz][i];
		else if(cfg == NULL)
			cfg = &configs_[clazz][i];
		else if(cfg->val0_ < configs_[clazz][i].val0_)
			cfg = &configs_[clazz][i];
	}
	return cfg;
}

