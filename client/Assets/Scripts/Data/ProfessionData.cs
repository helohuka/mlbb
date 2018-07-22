using System;
using System.Collections.Generic;



public class Profession
{
    public int id_;
    public JobType jobtype_;
    public int joblevel_;
	public string jobName_;
    public List<Pair<ItemSubType, int>> canuseitem_ = new List<Pair<ItemSubType,int>>();
    public List<Pair<int, int>> canuseskill_ = new List<Pair<int,int>>();
	public List<int> proudSkillGroups_ = new List<int> ();
	public List<string> GroupsId_ = new List<string> ();
    private static List<Profession> professions_ = new List<Profession>();
	public string Recommand_;
	public string Describe_;
	public string RecommendSkills_;
	public string RecommendEquippes_;
	public string RecommendEquippesIcon_;
	public string RecommendSkills1_;
	public int  openLV_;
	public string chuansong_;
	public string proffImg;

	private static Dictionary<int,Profession> metaData;

    public static void parse(string content, string fileName)
    {
        CSVParser parser = new CSVParser();
        if (!parser.Parse(content))
            return;
		metaData = new Dictionary<int, Profession>();
        for (int i = 0, s = parser.GetRecordCounter(); i < s; ++i)
        {
            Profession p = new Profession();

            p.id_ = parser.GetInt(i, "ID");
            p.jobtype_ = (JobType)Enum.Parse(typeof(JobType), parser.GetString(i, "JobType"));
            p.joblevel_ = parser.GetInt(i, "JobLv");

			p.Recommand_ = parser.GetString(i, "Recommand");
			p.Describe_ = parser.GetString(i, "Describe");
			p.RecommendSkills_= parser.GetString(i, "RecommendSkills");
			p.RecommendEquippes_ = parser.GetString(i, "RecommendEquip");
			p.RecommendEquippesIcon_ = parser.GetString(i, "RecommendEquipicon");
			p.RecommendSkills1_ = parser.GetString(i, "RecommendSkills1");
			p.jobName_ = parser.GetString(i, "Name");
			p.openLV_ = parser.GetInt(i, "OpenLV");
			p.proffImg = parser.GetString(i,"pic");
			p.chuansong_ = parser.GetString(i, "chuansong");
            {
                string str1 = parser.GetString(i, "EquipType");
                string[] strarr1 = str1.Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < strarr1.Length; ++j)
                {
                    string[] strarr2 = strarr1[j].Split(':');

                    Pair<ItemSubType, int> pair = new Pair<ItemSubType, int>((ItemSubType)Enum.Parse(typeof(ItemSubType), strarr2[0]), int.Parse(strarr2[1]));
                    p.canuseitem_.Add(pair);

                }
            }
            {
                string str1 = parser.GetString(i, "SkillGroup");
                string[] strarr1 = str1.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < strarr1.Length; ++j)
                {
                    string[] strarr2 = strarr1[j].Split(':');
//
                    Pair<int, int> pair = new Pair<int, int>(int.Parse(strarr2[0]), int.Parse(strarr2[1]));
                    p.canuseskill_.Add(pair);
//		
                }
            }
			{
				string str2 =  parser.GetString(i, "ProudSkillGroup");
				if(str2!="")
				{
                    string[] strarr2 = str2.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

					for (int j = 0; j < strarr2.Length; ++j)
					{
						p.proudSkillGroups_.Add(int.Parse(strarr2[j]));
					}

				}
			}
            professions_.Add(p);

			if(metaData.ContainsKey(p.id_))
			{
				return;
			}
			metaData[p.id_] = p;

        }
 
    }

	public static Profession GetData(int id)
	{
		if(!metaData.ContainsKey(id)) 
			return null;
		return metaData[id];
	}


    public static Profession get(JobType type, int level)
    {
        for (int i = 0, s = professions_.Count; i < s; ++i)
        {
            if (professions_[i].jobtype_ == type && professions_[i].joblevel_ == level)
                return professions_[i];
        }
        return null;
    }

    public int getItemMaxLevel(ItemSubType type)
    {
        int max = 0;
        for (int i = 0, s = canuseitem_.Count; i < s; ++i)
        {
            if (canuseitem_[i].first == type && canuseitem_[i].second > max)
                max = canuseitem_[i].second;
        }
        return max;
    }

    public int getSkilMaxLevel(int skillgroup)
    {
        int max = 0;

		/*for (int i = 0;i<professions_.Count; ++i)
		{
			for(int j =0;j<professions_[i].canuseskill_.Count;j++)
			{
				if (professions_[i].canuseskill_[j].first == skillgroup && professions_[i].canuseskill_[j].second > 0)
					//max = professions_[i].canuseskill_[j].second;
				return professions_[i].canuseskill_[j].second;
			}
		}
		*/


        for (int i = 0, s = canuseskill_.Count; i < s; ++i)
        {
            if (canuseskill_[i].first == skillgroup && canuseskill_[i].second > max)
                max = canuseskill_[i].second;
        }
		return max;
    }

    public bool canuseItem(ItemSubType type, int level)
    {
        for (int i = 0, s = canuseitem_.Count; i < s; ++i)
        {
            if (canuseitem_[i].first == type && canuseitem_[i].second >= level)
                return true;
        }
		 
        return false;
    }

    public ItemSubType[] CanUsedItems(int lev) {
        List<ItemSubType> types = new List<ItemSubType>();
        for (int i = 0, s = canuseitem_.Count; i < s; ++i)
        {
            if (canuseitem_[i].second == lev)
                types.Add(canuseitem_[i].first);
        }
        return types.ToArray();
    }

    public bool canuseSkill(int group, int level)
    {
        for (int i = 0, s = canuseskill_.Count; i < s; ++i)
        {
            if (canuseskill_[i].first == group && canuseskill_[i].second >= level)
                return true;
        }

        return false;
    }

	public bool isProudSkill(int jobId, int groupId,int level)
	{
		for (int i = 0; i<professions_.Count; ++i)
		{
			if((int)professions_[i].jobtype_ == jobId)
			{
				for(int j = 0;j<professions_[i].proudSkillGroups_.Count;j++)
				{
					if(groupId == professions_[i].proudSkillGroups_[j])
						return true;
				}

			}
		}

		return false;
	}

}