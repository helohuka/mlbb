using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class QuestionData {

	public int _Id;
	public string _Question;
	public string _Answer1;
	public string _Answer2;
	public string _Answer3;
	public string _Answer4;
	public int _Answer;
	private static Dictionary<int, QuestionData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, QuestionData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("QuestionData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		QuestionData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new QuestionData ();
			data._Id = parser.GetInt (i, "ID");
			data._Question = parser.GetString (i, "question");
			data._Answer1 = parser.GetString (i, "answer1");
			data._Answer2 = parser.GetString (i, "answer2");
			data._Answer3 = parser.GetString (i, "answer3");
			data._Answer4 = parser.GetString (i, "answer4");
			data._Answer = parser.GetInt (i, "ID");

			if(metaData.ContainsKey(data._Id))
			{
				ClientLog.Instance.LogError("QuestionData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data._Id] = data;
		}
		parser.Dispose ();
		parser = null;
	}
	
	public static QuestionData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}
	

}
