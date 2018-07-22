#include "EmployeeConfig.h"
#include "itemtable.h"
#include "CSVParser.h"
#include "TokenParser.h"
#include "ComScriptEvn.h"
#include "employeeTable.h"
std::map< S32 , EmployeeConfigTable::EmployeeConfigData* >  EmployeeConfigTable::data_;

bool EmployeeConfigTable::load(char const *fn)
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

	for(S32 row=0; row<csv.get_records_counter(); ++row)
	{
		U32 id = csv.get_int(row,"ID");
		if(data_[id] != NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("EmployeeConfigTable  has same id in row %d , id is %d\n"),row,id));
			SRV_ASSERT(0);	
		}

		EmployeeConfigData * pCore = new EmployeeConfigData ;
		pCore->equips_.resize(ES_Max);
		pCore->Id_ = id;
		pCore->employeeId_ = csv.get_int(row,"EmployeeId");
		pCore->star_ = csv.get_int(row,"star");
		pCore->equips_[ES_Head] = csv.get_int(row,"item-1");
		pCore->equips_[ES_Body] = csv.get_int(row,"item-2");
		pCore->equips_[ES_SingleHand] = csv.get_int(row,"item-3");
		pCore->equips_[ES_Boot] = csv.get_int(row,"item-4");
		pCore->equips_[ES_Ornament_0] = csv.get_int(row,"item-5");
		pCore->money_ = csv.get_int(row,"money");
		data_[id] = pCore;
	}

	return true;
}

bool EmployeeConfigTable::check()
{	
	std::map<S32, EmployeeConfigData*>::iterator  itor = data_.begin();
	while(itor != data_.end())
	{
		EmployeeConfigData* pEmp = itor->second;

		if (!pEmp)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("EmployeeConfigTable is NULL error\n")));
			return false;
		}

		SRV_ASSERT(EmployeeTable::getEmployeeById(pEmp->employeeId_));

		for(size_t i=0; i<pEmp->equips_.size(); ++i){
			if(pEmp->equips_[i]){
				ItemTable::ItemData const * data = ItemTable::getItemById(pEmp->equips_[i]);
				SRV_ASSERT(data && data->mainType_ == IMT_EmployeeEquip);
			}
				
		}
		
		itor++;
	}
	return true;
}

EmployeeConfigTable::EmployeeConfigData const* EmployeeConfigTable::getEmployeeConfig(U32 empId,U32 star)
{
	std::map<S32, EmployeeConfigData*>::iterator  itor = data_.begin();
	while(itor != data_.end())
	{
		EmployeeConfigData* pEmp = itor->second;

		if (pEmp == NULL)
		{
			ACE_DEBUG((LM_ERROR,ACE_TEXT("EmployeeConfigTable is NULL error\n")));
			return NULL;
		}

		if(empId == pEmp->employeeId_ && star == pEmp->star_)
			return pEmp;

		itor++;
	}

	return NULL;
}


