using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BlessingData
{
	public int _Id;
	public string _Name;
	public string _Icon;
	public string _Attribute;
	public int _ConsumeGold;
	public int _AchieveExp;
	public int _SkillId;
	public int _LvUP;
	public int _Lv1Exp;
	public int _Lv2Exp;
	public int _Lv3Exp;
	public int _Lv4Exp;
	public int _Lv5Exp;
	public int _Lv6Exp;
	public int _Lv7Exp;
	public int _Lv8Exp;
	public int _Lv9Exp;
	public int _Lv10Exp;
	public int _Lv11Exp;
	public int _Lv12Exp;
	public int _Lv13Exp;
	public int _Lv14Exp;
	public int _Lv15Exp;
	public int _Lv16Exp;
	public int _Lv17Exp;
	public int _Lv18Exp;
	public int _Lv19Exp;
	public int _Lv20Exp;

	private static Dictionary<int, BlessingData> metaData;

	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, BlessingData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("BlessingData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		BlessingData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new BlessingData ();
			data._Id = parser.GetInt (i, "ID");
			data._SkillId = parser.GetInt (i, "SkillID");
			data._Name = parser.GetString (i, "Name");
			data._Icon = parser.GetString (i, "ICON");
			data._Attribute = parser.GetString(i, "Attribute");
			data._ConsumeGold = parser.GetInt(i, "ConsumeGold");
			data._AchieveExp = parser.GetInt(i, "AchieveExp");
			data._LvUP = parser.GetInt(i, "LvUp");
			data._Lv1Exp = parser.GetInt(i, "Lv1Exp");
			data._Lv2Exp = parser.GetInt(i, "Lv2Exp");
			data._Lv3Exp = parser.GetInt(i, "Lv3Exp");
			data._Lv4Exp = parser.GetInt(i, "Lv4Exp");
			data._Lv5Exp = parser.GetInt(i, "Lv5Exp");
			data._Lv6Exp = parser.GetInt(i, "Lv6Exp");
			data._Lv7Exp = parser.GetInt(i, "Lv7Exp");
			data._Lv8Exp = parser.GetInt(i, "Lv8Exp");
			data._Lv9Exp = parser.GetInt(i, "Lv9Exp");
			data._Lv10Exp = parser.GetInt(i, "Lv10Exp");
			data._Lv11Exp = parser.GetInt(i, "Lv11Exp");
			data._Lv12Exp = parser.GetInt(i, "Lv12Exp");
			data._Lv13Exp = parser.GetInt(i, "Lv13Exp");
			data._Lv14Exp = parser.GetInt(i, "Lv14Exp");
			data._Lv15Exp = parser.GetInt(i, "Lv15Exp");
			data._Lv16Exp = parser.GetInt(i, "Lv16Exp");
			data._Lv17Exp = parser.GetInt(i, "Lv17Exp");
			data._Lv18Exp = parser.GetInt(i, "Lv18Exp");
			data._Lv19Exp = parser.GetInt(i, "Lv19Exp");
			data._Lv20Exp = parser.GetInt(i, "Lv20Exp");

			if(metaData.ContainsKey(data._Id))
			{
				ClientLog.Instance.LogError("BlessingData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data._Id] = data;
		}
		parser.Dispose ();
		parser = null;
	}
		

	public static BlessingData GatData(int id)
	{
		if(!metaData.ContainsKey(id))
		{
			return null;
		}
		return metaData[id];
	}

}

