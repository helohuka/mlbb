using UnityEngine;
using System;
using System.Collections.Generic;

public class QuestData {

	public int id_;
	
	public string questName_;

	public QuestKind questKind_;

	public QuestType questType_;

	public int needLevel_;

	public RequireType requireType_;

	public int startTalk_;

	public int finishNpcId_;

    public int finishSceneId_;

    public int doingSceneId_;

    public Vector2 doingLocation_;

	public int proTalk_;

	public int finishTalk_;

	public int preQuest_;

	public int targetId_;

	public string[] targetNum_;

	public int coin_;

	public int exp_;

	public int JobLevel_;


	public int itemId_;
    public int jobtype_;
	public string xunlu;

	public int itemNum_;
	public string QuestDes_;
    public string miniDesc_;
    public string miniFinDesc_;
	public int DropID_;
    public int postQuest_; /// Next quest
	//	public string name_;

	public static int defaultTongjiQuestId_;
	public static int defaultXuanshangQuestId_;//默认悬赏任务ID
	private static Dictionary<int, QuestData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, QuestData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("QuestData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		QuestData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new QuestData ();
			data.id_ = parser.GetInt (i, "QuestId");
			data.questName_ = parser.GetString (i, "questName");
			data.questKind_ = (QuestKind)Enum.Parse(typeof(QuestKind), parser.GetString (i, "QuestKind"));
			data.questType_ = (QuestType)Enum.Parse(typeof(QuestType), parser.GetString (i, "QuestType"));
			data.needLevel_ = parser.GetInt (i, "NeedLevel");
			data.requireType_ = (RequireType)Enum.Parse(typeof(RequireType), parser.GetString (i, "RequireType"));
			data.startTalk_ = parser.GetInt (i, "StartTalk");
			data.finishNpcId_ = parser.GetInt (i, "FinishNPC");
            data.finishSceneId_ = parser.GetInt(i, "FinishNPCScene");
            data.doingSceneId_ = parser.GetInt(i, "DoingScene");
            string[] tmp = parser.GetString(i, "DoingCoordinate").Split(',');
            if(tmp.Length == 2)
                data.doingLocation_ = new Vector2(float.Parse(tmp[0]), float.Parse(tmp[1]));
            data.proTalk_ = parser.GetInt(i, "ProTalk");
            data.finishTalk_ = parser.GetInt(i, "FinishTalk");
			data.targetId_ = parser.GetInt (i, "Target");
			data.targetNum_ = parser.GetString (i, "TargetNum").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
			data.coin_ = parser.GetInt (i, "Coin");
			data.exp_ = parser.GetInt (i, "Exp");
			data.itemId_ = parser.GetInt (i, "Item");
			data.itemNum_ = parser.GetInt (i, "ItemNum");
			data.QuestDes_ = parser.GetString(i,"QuestDes");
            data.miniDesc_ = parser.GetString(i, "QuestMiniDesc");
            data.miniFinDesc_ = parser.GetString(i, "Endtarget");
			data.preQuest_ = parser.GetInt(i,"PreQuest");
            data.postQuest_ = parser.GetInt(i, "PostQuest");
            data.xunlu = parser.GetString(i, "chuansong");
			data.JobLevel_ = parser.GetInt(i, "JobLevel");
	
            if (string.IsNullOrEmpty(parser.GetString(i, "JobType")))
            {
                data.jobtype_ = 0;
            }
            else
                data.jobtype_ = (int)(JobType)Enum.Parse(typeof(JobType), parser.GetString(i, "JobType"));
			data.DropID_ = parser.GetInt(i, "DropID");
			if(metaData.ContainsKey(data.id_))
			{
				ClientLog.Instance.LogError("QuestData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}

			if(data.targetId_ == 0 ){
				if(data.questKind_ == QuestKind.QK_Rand){
					defaultXuanshangQuestId_ = data.id_;
				}else if (data.questKind_ == QuestKind.QK_Tongji){
					defaultTongjiQuestId_ = data.id_;
				}
			}

			metaData[data.id_] = data;
		}
		parser.Dispose ();
		parser = null;
	}
	 
	public static QuestData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}

    //public static QuestData GetQuestData(int finishId)
    //{
    //    foreach(KeyValuePair<int, QuestData> data in metaData)
    //    {
    //        if(data.Value.finishNpcId_ == finishId)
    //            return data.Value;
    //    }
    //    return null;
    //}
	public static List<QuestData> GetQuestDataForQuestKind(QuestKind questKind)
	{
		List<QuestData> qdatas = new List<QuestData> ();
		foreach(QuestData qd in metaData.Values)
		{
			if(qd.questKind_ == questKind)
			{
				qdatas.Add(qd);
			}
		}
		return qdatas;
	}
	public static Dictionary<int, QuestData> GetMetaData()
	{
		return metaData;
	}
}
