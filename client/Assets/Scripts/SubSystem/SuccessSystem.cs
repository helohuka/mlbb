using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SuccessSystem
{
    public static Queue<AchievementContent> newAchieve;
    public static bool isDirty = false;
	public static bool isShowRedAward;
    static public void Clear()
    {
        foreach (Dictionary<AchievementType, List<AchievementContent>> aTypes in achievementByTab.Values)
        {
            foreach (List<AchievementContent> contents in aTypes.Values)
            {
                for (int i = 0; i < contents.Count; ++i)
                {
                    contents[i].Clear();
                }
            }
        }
        finishCount = 0;
    }

    static public void InitMyAchieve(COM_Achievement[] hasProgress)
    {
        newAchieve = new Queue<AchievementContent>();
        for (int i = 0; i < hasProgress.Length; ++i)
        {
            UpdateMyAchieve(hasProgress[i]);
        }
    }

    static public void UpdateMyAchieve(COM_Achievement hasProgress, bool isNew = false)
    {
        AchieveData data = AchieveData.GetData((int)hasProgress.achId_);
        if (data == null)
        {
            Debug.LogError(" Achievement ID: " + hasProgress.achId_ + " is not excist.");
            return;
        }
        if (achievementByTab.ContainsKey(data._Category))
        {
            if (achievementByTab[data._Category].ContainsKey(data._AchieveType))
            {
                for (int j = 0; j < achievementByTab[data._Category][data._AchieveType].Count; ++j)
                {
                    if (achievementByTab[data._Category][data._AchieveType][j].data_._Id == hasProgress.achId_)
                    {
                        if (!achievementByTab[data._Category][data._AchieveType][j].isAch_ && hasProgress.isAch_)
                        {
                            if (isNew)
                            {
                                newAchieve.Enqueue(achievementByTab[data._Category][data._AchieveType][j]);
                                ChatSystem.PushSystemMessage(LanguageManager.instance.GetValue("chengjiu").Replace("{n}", AchieveData.GetData((int)hasProgress.achId_)._AtName));
                            }
                            finishCount++;
                        }
                        achievementByTab[data._Category][data._AchieveType][j].achValue_ = hasProgress.achValue_;
                        achievementByTab[data._Category][data._AchieveType][j].isAward_ = hasProgress.isAward_;
                        achievementByTab[data._Category][data._AchieveType][j].isAch_ = hasProgress.isAch_;
                    }
                }
            }
        }
        isDirty = true;
    }

	

    static int finishCount = 0;
    public static int FinishCount
    {
        get { return finishCount; }
    }

    public static bool isReceived(int id)
    {
        AchieveData data = AchieveData.GetData(id);
        if (achievementByTab.ContainsKey(data._Category))
        {
            if (achievementByTab[data._Category].ContainsKey(data._AchieveType))
            {
                for (int j = 0; j < achievementByTab[data._Category][data._AchieveType].Count; ++j)
                {
                    if (achievementByTab[data._Category][data._AchieveType][j].data_._Id == id)
                    {
                        return achievementByTab[data._Category][data._AchieveType][j].isAward_;
                    }
                }
            }
        }
        return false;
    }

	public static bool isReceived()
	{
		foreach(Dictionary<AchievementType, List<AchievementContent>> contentsDic in  achievementByTab.Values)
		{
			foreach(List<AchievementContent> contents in  contentsDic.Values)
			{
				for(int i=0; i < contents.Count; ++i)
				{
					if( contents[i].isAch_&& !contents[i].isAward_)
					{
						return true;
					}
				}
			}
		}
		return false;
	}




    public static Dictionary<CategoryType, Dictionary<AchievementType, List<AchievementContent>>> achievementByTab = new Dictionary<CategoryType, Dictionary<AchievementType, List<AchievementContent>>>();
    public static void SetData(AchieveData data)
    {
        AchievementContent ac = new AchievementContent(data);
        if (!achievementByTab.ContainsKey(data._Category))
            achievementByTab.Add(data._Category, new Dictionary<AchievementType, List<AchievementContent>>());

        if (!achievementByTab[data._Category].ContainsKey(data._AchieveType))
            achievementByTab[data._Category].Add(data._AchieveType, new List<AchievementContent>());

        achievementByTab[data._Category][data._AchieveType].Add(ac);
    }

    public static AchievementContent Reward50()
    {
        return achievementByTab[CategoryType.ACH_Growup][AchievementType.AT_Reward50][0];
    }
}

public class AchievementContent
{
    //inst prop
    public uint achValue_;
    public bool isAch_;
    public bool isAward_;

    //static prop
    public AchieveData data_;

    public AchievementContent(AchieveData data, uint progress = 0, bool isAch = false, bool isAward = false)
    {
        data_ = data;
        achValue_ = progress;
        isAch_ = isAch;
        isAward_ = isAward;
    }

    public void Clear()
    {
        achValue_ = 0;
        isAch_ = false;
        isAward_ = false;
    }
}