using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSVParser {

	// data of a line
	List<string> line_data_;

	// whole data
	List<List<string>> records_;

	//
	Dictionary<string, int> column_names_;

	const int HEAD_LINE = 0;
	const int DATA_LINE = 1;


	public CSVParser()
	{
		line_data_ = new List<string> ();
		records_ = new List<List<string>> ();
		column_names_ = new Dictionary<string, int> ();
	}

	public bool Parse(string content, char seprator = ',')
	{
		string[] linesData = content.Split (new char[]{'\r', '\n'}, System.StringSplitOptions.RemoveEmptyEntries);

		ParseHeader (linesData [HEAD_LINE], seprator);

		for(int i=DATA_LINE; i < linesData.Length; ++i)
		{
			ParseRecord(linesData[i],seprator);
		}
		return true;
	}

	public void ParseHeader(string line,char seprator)
	{
		line += '\n';

		int tempIndex = 0;
		int tempCaseQuot =0;
		string tempName = string.Empty;
		for (int i=0; i<line.Length; ++i)
		{
			switch(line[i])
			{
			case '\n':
				column_names_.Add(tempName,tempIndex++);
				tempName = "";
				break;
			case '\r':
				break;
			case '\"':
				++tempCaseQuot;
				break;
			case '\'':
				++tempCaseQuot;
				break;
			default:
				if(line[i] == seprator)
				{
					if((tempCaseQuot & 0x1) != 0x0)
					{

					}
					column_names_.Add(tempName,tempIndex++);
					tempName = "";
				}
				else
					tempName += line[i];
				break;
			}
		}

	}

	public void ParseRecord(string line, char seprator)
	{
		line += '\n';

		int tempCaseQuot =0;
		List<string> record = new List<string>();
		string tempName = string.Empty;
		for (int i=0; i<line.Length; ++i)
		{
			switch(line[i])
			{
			case '\n':
				record.Add(tempName);
				tempName = "";
				break;
			case '\r':
				break;
			case '\"':
				++tempCaseQuot;
				break;
			case '\'':
				++tempCaseQuot;
				break;
			default:
				if(line[i] == seprator)
				{
					if((tempCaseQuot & 0x1) != 0x0)
					{
						tempName += line[i];
					}
					else
					{
						record.Add(tempName);
						tempName = "";
					}
				}
				else 
					tempName += line[i];
				break;
			}
		}

		records_.Add(record);
	}

	public float GetFloat(int row, string columName)
	{
		CheckColum(columName);
		float res;
		float.TryParse(GetItemData (row, column_names_[columName]), out res);
		return res;
	}

	public int GetInt(int row, string columName)
	{
		CheckColum(columName);
		int res;
		int.TryParse(GetItemData (row, column_names_[columName]), out res);
		return res;
	}

	public bool GetBool(int row, string columName)
	{
		CheckColum(columName);
		return GetItemData (row, column_names_[columName]).Equals("1");
	}

	public string GetString(int row, string columName)
	{
		CheckColum (columName);
		return GetItemData (row, column_names_[columName]);
	}

	string GetItemData(int row, int col)
	{
		return (records_[row])[col];
	}

	bool CheckColum(string columName)
	{
		if(!column_names_.ContainsKey(columName))
		{
			ClientLog.Instance.LogError("key not found!" + columName);
			return false;
		}
		return true;
	}

	public int GetRecordCounter()
	{
		return records_.Count;
	}
	
	public void Dispose()
	{
		for(int i=0; i < records_.Count; ++i)
		{
			records_[i].Clear();
		}
		records_.Clear ();
		records_ = null;
	}
}
