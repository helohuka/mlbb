using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DropData {

	public int DropID_;
		
	public int exp_;
	
	public int money_;
	
	public int diamond_;
	
	public int item_1_;
	
	public int item_num_1_;
	public int pro_type_1_;
	public int probability_1;
	public int item_2;
	public int item_num_2;
	public int pro_type_2;
	public int probability_2;
	public int item_3;
	public int item_num_3;
	public int pro_type_3;
	public int probability_3;
	public int item_4;
	public int item_num_4;
	public int pro_type_4;
	public int probability_4;
	public int item_5;
	public int item_num_5;
	
	public int pro_type_5;
	
	public int probability_5;
	
	public int item_6;
	
	public int item_num_6;
	
	public int pro_type_6;
	
	public int probability_6;

	public List<int>itemNumList = new List<int>();
	public List<int>itemList = new List<int>();

	private static Dictionary<int, DropData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, DropData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("QuestData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		DropData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new DropData ();
			data.DropID_ = parser.GetInt (i, "DropID");
			data.exp_ = parser.GetInt (i, "exp");
			data.money_ = parser.GetInt (i, "money");
			data.diamond_ = parser.GetInt (i, "diamond");
			data.item_1_ = parser.GetInt (i, "item-1");
			data.item_num_1_ = parser.GetInt(i, "item-num-1");
			data.pro_type_1_ = parser.GetInt(i, "pro-type-1");
			data.probability_1 = parser.GetInt(i, "probability-1");
			data.item_2 = parser.GetInt(i, "item-2");
			data.item_num_2 = parser.GetInt(i, "item-num-2");
			data.pro_type_2 = parser.GetInt(i, "pro-type-2");
			data.probability_2 = parser.GetInt(i, "probability-2");
			data.item_3 = parser.GetInt(i, "item-3");
			data.item_num_3 = parser.GetInt(i, "item-num-3");
			data.pro_type_3 = parser.GetInt(i, "pro-type-3");
			data.probability_3 = parser.GetInt(i, "probability-3");

			data.item_4 = parser.GetInt(i, "item-4");
			data.item_num_4 = parser.GetInt(i, "item-num-4");
			data.pro_type_4 = parser.GetInt(i, "pro-type-4");
			data.probability_4 = parser.GetInt(i, "probability-4");

			data.probability_4 = parser.GetInt(i, "item-5");
			data.probability_4 = parser.GetInt(i, "item-num-5");
			data.probability_4 = parser.GetInt(i, "pro-type-5");
			data.probability_4 = parser.GetInt(i, "probability-5");

			data.probability_4 = parser.GetInt(i, "item-6");
			data.probability_4 = parser.GetInt(i, "item-num-6");
			data.probability_4 = parser.GetInt(i, "pro-type-6");
			data.probability_4 = parser.GetInt(i, "probability-6");
			data.itemList.Add(data.item_1_);
			data.itemList.Add(data.item_2);
			data.itemList.Add(data.item_3);
			data.itemList.Add(data.item_4);
			data.itemList.Add(data.item_5);
			data.itemList.Add(data.item_6);
            data.itemNumList.Add(data.item_num_1_);
            data.itemNumList.Add(data.item_num_2);
            data.itemNumList.Add(data.item_num_3);
            data.itemNumList.Add(data.item_num_4);
            data.itemNumList.Add(data.item_num_5);
            data.itemNumList.Add(data.item_num_6);

			if(metaData.ContainsKey(data.DropID_))
			{
				ClientLog.Instance.LogError("QuestData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.DropID_] = data;
		}
		parser.Dispose ();
		parser = null;
	}


	public static DropData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}







}
