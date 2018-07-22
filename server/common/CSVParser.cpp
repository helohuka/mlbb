#include "CSVParser.h"
bool CSVParser::load_table_file( const char* filename, const char sperator)
{
	FILE	*pFile;
	pFile = fopen(filename, "r");
	if( pFile == NULL )
		return false;
	enum { BUFFER_SIZE = 16384};
	char line_data_[BUFFER_SIZE];
	if(fgets(line_data_, sizeof(line_data_), pFile))
		parse_table_header(line_data_,strlen(line_data_),sperator);

	while(fgets(line_data_, sizeof(line_data_), pFile))
		parse_table_record(line_data_,strlen(line_data_),sperator);
	fclose(pFile);

	return true;
}

bool CSVParser::parse_table_header( char const *buffer, size_t length, char const sperator)
{
	char temp_name_buffer[1024] = {'\0'};
	size_t temp_name_ptr = 0;
	size_t read_ptr = 0;
	size_t temp_case_quot = 0;
	size_t temp_index = 0;
	while(read_ptr < length)
	{
		switch(buffer[read_ptr])
		{
		case '\n':
			temp_name_buffer[temp_name_ptr++] = '\0';
			column_names_[temp_name_buffer] = temp_index++;
			temp_name_ptr = 0;
			break;
		case '\r':
			break;
		case '\"':
			++temp_case_quot;
			break;
		case '\'':
			++temp_case_quot;
			break;
		default:
			if(buffer[read_ptr] == sperator)
			{
				if((temp_case_quot & 0x1) != 0x0)
				{
					SRV_ASSERT(0);
				}
				temp_name_buffer[temp_name_ptr++] = '\0';
				column_names_[temp_name_buffer] = temp_index++;
				temp_name_ptr = 0;
			}
			else temp_name_buffer[temp_name_ptr++] = buffer[read_ptr];
			break;
		}

		++read_ptr;
	}

	return true;
}

bool CSVParser::parse_table_record( char const *buffer, size_t length, char const sperator)
{
	char temp_name_buffer[1024] = {'\0'};
	size_t temp_name_ptr = 0;
	size_t read_ptr = 0;
	size_t temp_case_quot = 0;
	size_t temp_index = 0;
	Record record;
	std::string tmpString;
	while(read_ptr < length)
	{
		switch(buffer[read_ptr])
		{
		case '\n':
			{
				temp_name_buffer[temp_name_ptr++] = '\0';
				tmpString = temp_name_buffer;
				//ACE_DEBUG((LM_ERROR,"---%s\n",temp_name_buffer));
				record.push_back(tmpString);
				temp_name_ptr = 0;
			}break;
		case '\r':
			break;
		case '\"':
			++temp_case_quot;
			break;
		case '\'':
			++temp_case_quot;
			break;
		default:
			if(buffer[read_ptr] == sperator)
			{
				if((temp_case_quot & 0x1) != 0x0)
				{
					temp_name_buffer[temp_name_ptr++] = buffer[read_ptr];
				}
				else
				{	
					temp_name_buffer[temp_name_ptr++] = '\0';
					tmpString = temp_name_buffer;
					//ACE_DEBUG((LM_ERROR,"---%s\n",temp_name_buffer));
					record.push_back(tmpString);
					temp_name_ptr = 0;
				}
			}
			else temp_name_buffer[temp_name_ptr++] = buffer[read_ptr];
			break;
		}
		++read_ptr;
	}

	SRV_ASSERT(record.size() == column_names_.size());

	records_.push_back(record);
	return true;
}

std::string	CSVParser::get_item_data(unsigned row, unsigned col)
{
	SRV_ASSERT(col < column_names_.size());
	SRV_ASSERT(row < records_.size());
	SRV_ASSERT( col < records_[row].size() );

	return (records_[row])[col];
}
int	CSVParser::get_int(unsigned row, const char* columnname)
{
 	checkColunmKey(  columnname );
	std::string strData = get_item_data(row, column_names_[columnname]);
	int i = 0;
	if(strData.length())
		i = atoi( strData.c_str() );
	return i;
}
float CSVParser::get_float(unsigned row, const char* columnname)
{
	checkColunmKey( columnname );
	std::string strData = get_item_data(row, column_names_[columnname]);
	float f = (float)atof(strData.c_str());

	return f;
}
bool CSVParser::get_bool(unsigned row, const char* columnname)
{	
	checkColunmKey( columnname );
	return get_int(row,columnname) != 0;
}
std::string	CSVParser::get_string(unsigned row, const char* columnname)
{
	checkColunmKey( columnname );
	return get_item_data(row, column_names_[columnname]);
}
