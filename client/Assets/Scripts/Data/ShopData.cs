using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class ShopData  {

	public int _Id;
	
	public string _Name;

	public ShopType _ShopType;

	public ShopPayType _ShopPayType;

	public int _Itemid;

	public int _Num;

	public string _Currency;

	public int _Price;

	public int _Recommend;

	public int _Hot;

	public int _Purchase;

	public int _Timelimit;

	public int _Sell;

	public string _Icon;

	public int _Undercarriage;

	public ClassifyType _classifytype;

	public string _IosShopid;
	public static string[] fanlis = {"3","15","49","99","229","648"};
	public static Dictionary<int, ShopData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, ShopData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("ShopData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		ShopData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new ShopData ();
			data._Id = parser.GetInt (i, "ID");
			data._Name = parser.GetString (i, "Name");
			data._ShopType = (ShopType)Enum.Parse(typeof(ShopType), parser.GetString(i, "ShopType"));
			data._classifytype = (ClassifyType)Enum.Parse(typeof(ClassifyType), parser.GetString(i, "Classify"));
			data._ShopPayType = (ShopPayType)Enum.Parse(typeof(ShopPayType), parser.GetString(i, "ShopPayType"));
			data._Itemid = parser.GetInt (i, "Itemid");
			data._Num = parser.GetInt (i, "Num");
			//data.Currency_ = parser.GetString (i, "Currency");
			data._Price = parser.GetInt (i, "Price");
			data._Recommend = parser.GetInt (i, "Recommend");
			data._Hot = parser.GetInt (i, "Hot");
			data._Purchase = parser.GetInt (i, "Purchase");
			data._Timelimit = parser.GetInt (i, "Timelimit");
			data._Sell = parser.GetInt (i, "Sell");
			data._Undercarriage = parser.GetInt (i, "Undercarriage");
			data._IosShopid = parser.GetString (i, "IOSID");
			data._Icon = parser.GetString (i, "Icon");
			if(metaData.ContainsKey(data._Id))
			{
				ClientLog.Instance.LogError("ShopData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data._Id] = data;
		}
		parser.Dispose ();
		parser = null;
	}
	
	public static ShopData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}
	public static int GetShopId(int itemId)
	{
		foreach(KeyValuePair<int,ShopData> Pair  in metaData)
		{
			if(Pair.Value._Itemid == itemId)
			{
				return Pair.Value._Id;
			}
		}
		return 0;
	}
}
