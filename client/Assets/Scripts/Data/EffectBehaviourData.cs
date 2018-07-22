using System;
using System.Collections.Generic;

public class EffectBehaviourData 
{
	public int id_;

	public string desc_;

	public WORKTYPE work_type_;

	public MOVETYPE move_type_;

	public float move_time_;

	public float dmage_hit1_;

	public float dmage_hit2_;

	public float beattack_effect_;

	public float pop_value_;

    public float shake_time_;

	public float end_time_;

	public float destory_time_;

	public CASTTYPE cast_type_;

	public int count_;

	public EffectPositionType	effect_positionType;

	public enum EffectPositionType
	{
		Up,
		Center,
		Down,
		Forword
	}

	public enum CASTTYPE
	{
		OnlyOne,	//一个打若干目标
		SameTime,	//多个同时释放
		OneByOne	//一个接一个释放
	}

	public enum WORKTYPE
	{
		C,			//释放者
		T,			//目标
		CT,			//释放者到目标
		UT,			//特定点(目标上方)到目标
		CENTERT,	//特定点(场景中间)到目标
		OLT,		//屏幕外一点到目标 左
		ORT,		//屏幕外一点到目标 右
	}

	public enum MOVETYPE
	{
		Straight,
		Bezier,
		Fixed
	}
	
	private static Dictionary<int, EffectBehaviourData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, EffectBehaviourData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("EffectBehaviourData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		EffectBehaviourData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new EffectBehaviourData ();
			data.id_ = parser.GetInt (i, "ID");
			data.desc_ = parser.GetString (i, "Desc");
			data.work_type_ = (WORKTYPE)Enum.Parse(typeof(WORKTYPE), parser.GetString (i, "WorkType"));
			data.move_type_ = (MOVETYPE)Enum.Parse(typeof(MOVETYPE), parser.GetString(i, "MoveType"));
			data.move_time_ = parser.GetFloat(i, "moveTime");
			data.dmage_hit1_ = parser.GetFloat(i, "Hit1Time");
			data.dmage_hit2_ = parser.GetFloat(i, "Hit2Time");
			data.beattack_effect_ = parser.GetFloat(i, "BeattackEffTime");
			data.pop_value_ = parser.GetFloat(i, "PopValueTime");
            data.shake_time_ = parser.GetFloat(i, "ShakeCameraTime");
			data.end_time_ = parser.GetFloat(i, "EndTime");
			data.destory_time_ = parser.GetFloat(i, "DestoryTime");
			data.cast_type_ = (CASTTYPE)Enum.Parse(typeof(CASTTYPE), parser.GetString(i, "CastType"));
			data.count_ = parser.GetInt(i, "Count");
			data.effect_positionType = (EffectPositionType)Enum.Parse(typeof(EffectPositionType), parser.GetString(i, "EffectPosition"));
			if(metaData.ContainsKey(data.id_))
			{
				ClientLog.Instance.LogError("EffectBehaviourData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.id_] = data;
		}
		parser.Dispose ();
		parser = null;
	}
	
	public static EffectBehaviourData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}
}
