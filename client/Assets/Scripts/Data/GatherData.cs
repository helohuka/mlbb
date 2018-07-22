using System;
using System.Collections.Generic;

public class GatherData {
	
    const int CellMaxSize = 5;

	public int _Id;
	
	public int _Level;

    public int _Type;

    public int _Show0;
    public int _Show1;

    public string _Title;

    public string _Icon;

    public int _Position;

    public int _Money;
	public int _AddMoney;

    static List<GatherData>[] _Meta = new List<GatherData>[(int)MineType.MT_Max]{
        null
        ,new List<GatherData>()
        ,new List<GatherData>()
        ,new List<GatherData>()
    };

    //public static Dictionary<int, GatherData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
        //metaData = new Dictionary<int, GatherData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("GatherData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		for(int i=0,l = parser.GetRecordCounter(); i < l; ++i)
		{
			GatherData data = new GatherData ();
			data._Id = parser.GetInt (i, "Id");
            data._Level = parser.GetInt(i, "Level");
            data._Position = parser.GetInt(i,"Position");
            data._Type = (int)Enum.Parse(typeof(MineType), parser.GetString(i, "Type"));
            string[] strshow = parser.GetString(i, "Show").Split(new string[]{";"}, StringSplitOptions.RemoveEmptyEntries);

            data._Show0 = int.Parse(strshow[0]);
            data._Show1 = int.Parse(strshow[1]);
			data._Money = parser.GetInt(i, "money");
            data._Title = parser.GetString(i, "Title");
            data._Icon = parser.GetString(i, "Icon");
			data._AddMoney = parser.GetInt(i, "addvalue");
            _Meta[data._Type].Add(data);
		}
		parser.Dispose ();
		parser = null;
	}

    public static List<GatherData> GetGatherList(MineType type)
    {
        return _Meta[(int)type];
    }

    public static GatherData GetGather(int id)
    {
        for (int i = 0; i < _Meta.Length; ++i)
        {
            if (null == _Meta[i])
                continue;
            for (int j = 0; j < _Meta[i].Count; ++j)
            {
                if (_Meta[i][j]._Id == id)
                    return _Meta[i][j];
            }
        }
        return null;
    }

	public static int gatherLevel(int id)
	{
        for (int i = 0; i < _Meta.Length; ++i)
		{
            if (null == _Meta[i])
				continue;
            for (int j = 0; j < _Meta[i].Count; ++j)
			{
                if (_Meta[i][j]._Show0 == id || _Meta[i][j]._Show1 == id)
                    return _Meta[i][j]._Level;
			}
		}
		return 0;
	}

	public static GatherData gatherItemId(int id)
	{
        for (int i = 0; i < _Meta.Length; ++i)
		{
			if (null == _Meta[i])
				continue;
            for (int j = 0; j < _Meta[i].Count; ++j)
			{
                if (_Meta[i][j]._Show0 == id || _Meta[i][j]._Show1 == id)
                    return _Meta[i][j];
			}
		}
		return null;
	}


}

