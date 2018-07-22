using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FilterwordData {

	public string _word;
	private static List<char>chararr = new List<char>();
	private static Dictionary<int,List<string>> words = new Dictionary<int, List<string>>();

	public static void ParseData(string content, string fileName)
	{	
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("FilterwordData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		FilterwordData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new FilterwordData ();
			data._word = parser.GetString(i, "word");
			if(!words.ContainsKey(data._word.Length))
				words.Add(data._word.Length,new List<string>());
			words[data._word.Length].Add(data._word);
		}
		parser.Dispose ();
		parser = null;
	}
	public static List<string> GetData(int len)
	{
		if(!words.ContainsKey(len))
			return null;
		return words [len];
	}
}
