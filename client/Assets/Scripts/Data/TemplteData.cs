using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TemplteData  {


	public string _Text;

	private static List<TemplteData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new List<TemplteData>();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("TemplteData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		TemplteData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new TemplteData ();
			data._Text = parser.GetString (i, "Templet");

			metaData.Add(data);
		}
		parser.Dispose ();
		parser = null;
	}
	
	public static List<TemplteData> GetData()
	{

		return metaData;
	}

	


}
