#include "ArtifactLevelTable.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"
#include "itemtable.h"
std::map< S32 , std::vector<ArtifactLevelTable::ArtifactLevelData*> >  ArtifactLevelTable::data_;

bool ArtifactLevelTable::load(char const *fn)
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
		ArtifactLevelData * pCore = new ArtifactLevelData;
		pCore->id_ =csv.get_int(row,"Lv");
		pCore->exp_ = csv.get_int(row,"Exp");
		pCore->itemId_ = csv.get_int(row,"Item");
	
		std::string stt = csv.get_string(row,"JobType");
		int stte = ENUM(JobType).getItemId(stt);
		if(-1 == stte)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("ItemMainType error in row %d , id is %d\n"),row,pCore->id_ ));
			SRV_ASSERT(0);	
		}
		pCore->professionType_ = (JobType)stte;


		ArtifactPropData	cpv;

		cpv.type_ = PT_HpMax;
		cpv.value_ = csv.get_int(row,"HpMax");
		pCore->propValue_.push_back(cpv);
			
		cpv.type_ = PT_MpCurr;
		cpv.value_ = csv.get_int(row,"MpCurr");
		pCore->propValue_.push_back(cpv);

		cpv.type_ = PT_HpCurr;
		cpv.value_ = csv.get_int(row,"HpCurr");
		pCore->propValue_.push_back(cpv);


		cpv.type_ = PT_MpMax;
		cpv.value_ = csv.get_int(row,"MpMax");
		pCore->propValue_.push_back(cpv);
		
		cpv.type_ = PT_Attack;
		cpv.value_ = csv.get_int(row,"Attack");
		pCore->propValue_.push_back(cpv);
		
		cpv.type_ = PT_Defense;
		cpv.value_ = csv.get_int(row,"Defense");
		pCore->propValue_.push_back(cpv);

		cpv.type_ = PT_Agile;
		cpv.value_ = csv.get_int(row,"Agile");
		pCore->propValue_.push_back(cpv);

		cpv.type_ = PT_Spirit;
		cpv.value_ = csv.get_int(row,"Spirit");
		pCore->propValue_.push_back(cpv);

		cpv.type_ = PT_Reply;
		cpv.value_ = csv.get_int(row,"Reply");
		pCore->propValue_.push_back(cpv);

		cpv.type_ = PT_Hit;
		cpv.value_ = csv.get_int(row,"Hit");
		pCore->propValue_.push_back(cpv);

		cpv.type_ = PT_Dodge;
		cpv.value_ = csv.get_int(row,"Dodge");
		pCore->propValue_.push_back(cpv);

		cpv.type_ = PT_Crit;
		cpv.value_ = csv.get_int(row,"Crit");
		pCore->propValue_.push_back(cpv);

		cpv.type_ = PT_counterpunch;
		cpv.value_ = csv.get_int(row,"counterpunch");
		pCore->propValue_.push_back(cpv);

		cpv.type_ = PT_Magicattack;
		cpv.value_ = csv.get_int(row,"Magicattack");
		pCore->propValue_.push_back(cpv);

		cpv.type_ = PT_Magicdefense;
		cpv.value_ = csv.get_int(row,"Magicdefense");
		pCore->propValue_.push_back(cpv);
	
		cpv.type_ = PT_NoSleep;
		cpv.value_ = csv.get_int(row,"NoSleep");
		pCore->propValue_.push_back(cpv);

		cpv.type_ = PT_NoPetrifaction;
		cpv.value_ = csv.get_int(row,"NoPetrifaction");
		pCore->propValue_.push_back(cpv);

		cpv.type_ = PT_NoDrunk;
		cpv.value_ = csv.get_int(row,"NoDrunk");
		pCore->propValue_.push_back(cpv);

		cpv.type_ = PT_NoChaos;
		cpv.value_ = csv.get_int(row,"NoChaos");
		pCore->propValue_.push_back(cpv);

		cpv.type_ = PT_NoForget;
		cpv.value_ = csv.get_int(row,"NoForget");
		pCore->propValue_.push_back(cpv);

		cpv.type_ = PT_Wind;
		cpv.value_ = csv.get_int(row,"Wind");
		pCore->propValue_.push_back(cpv);

		cpv.type_ = PT_Land;
		cpv.value_ = csv.get_int(row,"Land");
		pCore->propValue_.push_back(cpv);

		cpv.type_ = PT_Water;
		cpv.value_ = csv.get_int(row,"Water");
		pCore->propValue_.push_back(cpv);

		cpv.type_ = PT_Fire;
		cpv.value_ = csv.get_int(row,"Fire");
		pCore->propValue_.push_back(cpv);

		cpv.type_ = PT_SneakAttack;
		cpv.value_ = csv.get_int(row,"PT_SneakAttack");
		pCore->propValue_.push_back(cpv);

		cpv.type_ = PT_NoPoison;
		cpv.value_ = csv.get_int(row,"Poison");
		pCore->propValue_.push_back(cpv);


		if(!data_[pCore->id_].empty())
		{
			data_[pCore->id_].push_back(pCore);
		}
		else
		{
			std::vector<ArtifactLevelData*> dataList ;
			dataList.push_back(pCore);
			data_[pCore->id_] = dataList;
		}

	}
		return true;
}

bool ArtifactLevelTable::check(){
	for(std::map< S32 , std::vector<ArtifactLevelData*> >::iterator i=data_.begin(),e=data_.end(); i!=e; ++i){
		std::vector<ArtifactLevelData*> &dv = i->second;
		for(size_t i=0; i<dv.size(); ++i){
			SRV_ASSERT(ItemTable::getItemById(dv[i]->itemId_));
		}
	}
	return true;
}
	
ArtifactLevelTable::ArtifactLevelData const*
ArtifactLevelTable::getArtifactById(S32 level,JobType job)
{
	for(size_t i =0;i<data_[level].size();i++)
	{
		if(data_[level][i]->professionType_ == job)
		{
			return data_[level][i];
		}
	}
	return NULL;
}
//////////////////////////////////////////////////////////////////////////

std::vector< CrystalTable::CrystalTableData* >  CrystalTable::data_;

bool
CrystalTable::load(char const *fn){
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false){
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);
	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		CrystalTableData* cyd = new CrystalTableData;
		int id = csv.get_int(row,"ID");
		cyd->id_ = id;
		cyd->weight_ = csv.get_int(row,"property-num");
		
		std::string stt = csv.get_string(row,"type");
		int stte = ENUM(PropertyType).getItemId(stt);
		if(-1 == stte)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("CrystalTable PropertyType error in row %d , id is %d\n"),row,id));
			SRV_ASSERT(0);	
		}
		cyd->type_ = (PropertyType)stte;
		
		SingletonData sld;
		sld.propLevel_ = csv.get_int(row,"Quality");
		sld.wt_ = csv.get_int(row,"item-num");

		std::string strproperty = csv.get_string(row,"property");
		if(!strproperty.empty())
		{
			const char* pchar = strproperty.c_str();
			std::string token;
			TokenParser::getToken( pchar , token , ';');
			sld.val_.first =  atoi(token.c_str());
			TokenParser::getToken( pchar , token , ';');
			sld.val_.second = atof(token.c_str());
		}
		cyd->td_ = sld;
		data_.push_back(cyd);
	}
	return true;
}

bool
CrystalTable::check(){
	return true;
}

PropertyType
CrystalTable::randType(){
	std::vector<std::pair<int ,int> > tmp;
	for (size_t i = 0; i < data_.size(); ++i)
	{	
		std::pair<int ,int> pool;
		pool = std::make_pair(data_[i]->id_,data_[i]->weight_);
		tmp.push_back(pool);	
	}

	U32 index = UtlMath::randWeight(tmp);

	for (size_t i = 0; i < data_.size(); ++i)
	{
		if(data_[i]->id_ == index)
			return data_[i]->type_;
	}
	return PT_None;
}

void
CrystalTable::randresetprop(COM_CrystalProp &cp){
	PropertyType tt = randType();
	if(tt == PT_None)
		return;
	std::vector<SingletonData> sdata;
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if(data_[i]->type_ == tt)
			sdata.push_back(data_[i]->td_);
	}
	std::vector<std::pair<int ,int> > tmp;
	for (size_t i = 0; i < sdata.size(); ++i)
	{	
		std::pair<int ,int> pool;
		pool = std::make_pair(sdata[i].propLevel_,sdata[i].wt_);
		tmp.push_back(pool);	
	}

	U32 level = UtlMath::randWeight(tmp);
	U32 val = 0;
	
	for (size_t i = 0; i < sdata.size(); ++i)
	{
		if(sdata[i].propLevel_ == level)
		{
			val = UtlMath::randNM(sdata[i].val_.first,sdata[i].val_.second);
		}
	}

	cp.level_ = level;
	cp.type_ = tt;
	cp.val_ = val;
}

void
CrystalTable::randupprop(COM_CrystalProp &cp){
	PropertyType tt = randType();
	if(tt == PT_None)
		return;
	std::vector<SingletonData> sdata;
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if(data_[i]->type_ == tt)
			sdata.push_back(data_[i]->td_);
	}
	std::vector<std::pair<int ,int> > tmp;
	for (size_t i = 0; i < sdata.size(); ++i)
	{	
		if(sdata[i].propLevel_ > 3)
			continue;
		std::pair<int ,int> pool;
		pool = std::make_pair(sdata[i].propLevel_,sdata[i].wt_);
		tmp.push_back(pool);	
	}

	U32 level = UtlMath::randWeight(tmp);
	U32 val = 0;
	
	for (size_t i = 0; i < sdata.size(); ++i)
	{
		if(sdata[i].propLevel_ == level)
		{
			val = UtlMath::randNM(sdata[i].val_.first,sdata[i].val_.second);
		}
	}

	cp.level_ = level;
	cp.type_ = tt;
	cp.val_ = val;
}

std::vector<CrystalUpTable::CrystalUpData*> CrystalUpTable::data_;
bool
CrystalUpTable::load(char const *fn)
{
	if(NULL == fn)
		SRV_ASSERT(0);
	if(strlen(fn) == 0)
		SRV_ASSERT(0);
	ACE_DEBUG( ( LM_INFO, ACE_TEXT("Loading from file \"%s\"...\n"), fn ) );
	CSVParser csv;
	if(csv.load_table_file(fn) == false){
		SRV_ASSERT(0);
	}

	if(csv.get_records_counter() == 0)
		SRV_ASSERT(0);
	for(size_t row=0; row<csv.get_records_counter(); ++row)
	{
		CrystalUpData* p = new CrystalUpData;
		p->level_ = csv.get_int(row,"levelup");
		p->neednum_ = csv.get_int(row,"DebrisNum");
		p->needgold_ = csv.get_int(row,"GodNum");
		p->prob_ = csv.get_int(row,"Mission");
		data_.push_back(p);
	}
	return true;
}

bool
CrystalUpTable::check(){return true;}

CrystalUpTable::CrystalUpData const*
CrystalUpTable::getdata(U32 level){
	for (size_t i = 0; i < data_.size(); ++i)
	{
		if(data_[i]->level_ == level)
			return data_[i];
	}
	return NULL;
}