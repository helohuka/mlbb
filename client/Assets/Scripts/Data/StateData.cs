using UnityEngine;
using System;
using System.Collections.Generic;

public class StateData {

	public int _Id;

	public int _StateType;

	public string _Name;

    public string _Desc;

	public StatePkg _WorkPkg;

	public StatePkg _BeattackPkg;

	public StatePkg _ActionPkg;

    public string _Icon;
	
	private static Dictionary<int, StateData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, StateData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("StateData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		StateData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new StateData ();
			data._Id = parser.GetInt (i, "ID");
			data._StateType = (int)Enum.Parse(typeof(StateType), parser.GetString(i, "Type"));
			data._Name = parser.GetString(i, "Desc");

			data._BeattackPkg = new StatePkg();
			data._BeattackPkg.effectId_ = parser.GetInt (i, "BeattackEffectId");
			data._BeattackPkg.action_ = parser.GetString (i, "BeattackAnimation");

			data._WorkPkg = new StatePkg();
			data._WorkPkg.effectId_ = parser.GetInt (i, "WorkEffectId");
            string workEff = parser.GetString(i, "WorkEffect");
            if (!string.IsNullOrEmpty(workEff))
            {
                string[] workEffArr = workEff.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (workEffArr.Length == 3)
                {
                    string mc = workEffArr[0];
                    string rc = workEffArr[1];
                    float rw = float.Parse(workEffArr[2]);

                    string[] mcarr = mc.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] rcarr = rc.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if(mcarr.Length == 3)
                        data._WorkPkg.mainColor_ = new Color(float.Parse(mcarr[0]) / 255f, float.Parse(mcarr[1]) / 255f, float.Parse(mcarr[2]) / 255f);
                    if (rcarr.Length == 3)
                        data._WorkPkg.rimColor_ = new Color(float.Parse(rcarr[0]) / 255f, float.Parse(rcarr[1]) / 255f, float.Parse(rcarr[2]) / 255f);
                    data._WorkPkg.rimWidth_ = rw;
                }
                
            }
			data._ActionPkg = new StatePkg();
			data._ActionPkg.effectId_ = parser.GetInt (i, "ActionEffectId");
			data._ActionPkg.action_ = parser.GetString (i, "ActionAnimation");

            data._Icon = parser.GetString(i, "Icon");

			if(metaData.ContainsKey(data._Id))
			{
				ClientLog.Instance.LogError("StateData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data._Id] = data;
		}
		parser.Dispose ();
		parser = null;
	}

	public class StatePkg
	{
		public int effectId_;
		public string action_;
        public Color mainColor_;
        public Color rimColor_;
        public float rimWidth_;
	}

	public static StateData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}
}
