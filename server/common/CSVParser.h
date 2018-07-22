#ifndef	__CSVPARSER_H__
#define	__CSVPARSER_H__
#include "config.h"

class CSVParser
{
	typedef std::vector<std::string> Record;
public:
	CSVParser(){}
	CSVParser(const char* filename, const char sperator = ',')
	{
		load_table_file(filename, sperator);
	}

	bool load_table_file( const char* filename, const char sperator = ',');
	bool parse_table_header( char const *buffer, size_t length,char const sperator = ',');
	bool parse_table_record( char const *buffer, size_t length,char const sperator = ',');
	unsigned get_records_counter(){return records_.size();}
	unsigned get_columns_counter(){return column_names_.size();}


	int			get_int(unsigned  row, const char* columnname);
	float		get_float(unsigned  row, const char* columnname);
	bool		get_bool(unsigned  row, const char* columnname);
	std::string	get_string(unsigned  row, const char* columnname);
	void checkColunmKey( char const * columnname)
	{
		std::map<std::string, unsigned >::iterator itr = column_names_.find(columnname);
		if(column_names_.end() == itr )
		{
			ACE_DEBUG((LM_ERROR,"columnname %s error\n",columnname));
			SRV_ASSERT(0);
		}
	}
	
//protected:
	std::string	get_item_data(unsigned  row, unsigned  col);
private:
	std::map<std::string, unsigned >		column_names_;
	std::vector< Record >				records_;
};

#endif
