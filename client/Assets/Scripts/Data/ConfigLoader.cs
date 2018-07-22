using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ConfigLoader : MonoBehaviour{

	static ConfigLoader inst = null;
	public static ConfigLoader Instance
	{
		get
		{
			if(inst == null)
			{
				inst = GameObject.Find(GlobalValue.GlobalGameObjectName).GetComponent<ConfigLoader>();
			}
			return inst;
		} 
	}
	
	public void Init()
	{
		//inst = this;
		contents = new Dictionary<string, string> ();

		parseFunc = new Dictionary<string, ParseHandler> ();
		
		//Regist table
		RegistTable ("EntityAssets.csv", EntityAssetsData.ParseData);
		RegistTable ("EffectAssets.csv", EffectAssetsData.ParseData);
		RegistTable ("EffectBehaviour.csv", EffectBehaviourData.ParseData);
		RegistTable ("UIAssets.csv", UIAssetsData.ParseData);
		RegistTable ("SkillData.csv", SkillData.ParseData);
		RegistTable ("PlayerData.csv", PlayerData.ParseData);
		RegistTable ("Exp.csv", ExpData.ParseData);
		RegistTable ("Npc.csv", NpcData.ParseData);
		RegistTable ("Quest.csv", QuestData.ParseData);
		RegistTable ("Monster.csv", BabyData.ParseData);
        RegistTable("BattleData.csv", BattleData.ParseData);
		RegistTable ("PetActivityData.csv", PetActivityData.ParseData);
		RegistTable ("State.csv", StateData.ParseData);
		RegistTable ("ItemData.csv", ItemData.ParseData);
		RegistTable ("Scene.csv", SceneData.ParseData);
		RegistTable ("Course.csv", CourseData.ParseData);
		RegistTable ("Cheats.csv", CheatsData.ParseData);
		RegistTable ("EmployeeConfig.csv", EmployeeConfigData.ParseData);
		RegistTable ("DebrisConfig.csv", DebrisData.ParseData);
		RegistTable ("PVRreward.csv", PvRrewardData.ParseData);
		RegistTable ("PVPrunk.csv", PvpRewardData.ParseData);
		RegistTable ("HomeShopData.csv", HomeShopData.ParseData);
		RegistTable ("ZhuanpanConfig.csv", ZhuanpanConfigData.ParseData);
		RegistTable ("HomeExp.csv", HomeExpData.ParseData);
		RegistTable ("PetIntensive.csv", PetIntensiveData.ParseData);
        RegistTable ("LoadingTips.csv", LoadingTipsData.ParseData);
		RegistTable ("question.csv", QuestionData.ParseData);
		RegistTable ("Templet.csv", TemplteData.ParseData);
		RegistTable ("Copy.csv", CopyData.ParseData);
		RegistTable ("family.csv", FamilyData.ParseData);
        RegistTable("helpLevel.csv", HelpLevelData.ParseData);
		RegistTable("Monster2.csv", familyMonsterData.ParseData);
		RegistTable ("EmployeeData.csv", EmployeeData.ParseData);
        RegistTable("Title.csv", TitleData.ParseData);
		RegistTable ("Talk.csv", TalkData.ParseData);
		RegistTable ("Randname.csv", RandomNameData.ParseData);
		RegistTable ("Lottery.csv", LotteryData.ParseData);
		RegistTable ("Gather.csv", GatherData.ParseData);
		RegistTable ("Make.csv", MakeData.ParseData);
        RegistTable("ProfessionData.csv", Profession.parse);
		RegistTable("ShopData.csv", ShopData.ParseData);
		RegistTable("SoundAssets.csv", SoundAssetsData.ParseData);
		RegistTable("MusicAssets.csv", MusicAssetsData.ParseData);
		RegistTable("AchieveData.csv", AchieveData.ParseData);
		RegistTable("ACT_Reward.csv", ACT_RewardData.ParseData);
		RegistTable("Activity.csv", ActivityData.ParseData);
		RegistTable("ChallengeData.csv", ChallengeData.ParseData);
        RegistTable("DailyActivities.csv", DaliyActivityData.ParseData);
		RegistTable("Drop.csv", DropData.ParseData);
		RegistTable ("ArtifactConfig.csv", ArtifactConfigData.ParseData);
		RegistTable ("ArtifactLevel.csv", ArtifactLevelData.ParseData);
		RegistTable ("filterword.csv", FilterwordData.ParseData);
		RegistTable ("ArtifactChange.csv", ArtifactChangeData.ParseData);
		RegistTable ("EquipColour.csv", EquipColorData.ParseData);
        RegistTable("UIDependence.json", UIDepedenceData.ParseData);
        RegistTable("PlayerDependence.json", PlayerDepedenceData.ParseData);
        RegistTable("EffectDependence.json", EffectDepedenceData.ParseData);
		RegistTable ("Blessing.csv", BlessingData.ParseData);
        RegistTable("QA.csv", QaData.ParseData);
		RegistTable("timereward.csv", TimerReawData.ParseData);
		RegistTable("foundation.csv", GrowthFundData.ParseData);
        RegistTable("Sevendays.csv", SevenDaysData.ParseData);
        RegistTable("cardsreward.csv", MoreCardsRewardData.ParseData);
        RegistTable("cardsper.csv", MoreCardsDrawData.ParseData);
		RegistTable("recharge_all.csv", rechargeAllData.ParseData);
		RegistTable("LevelGift.csv", MoreLevelData.ParseData);
		RegistTable("Runes.csv", RunesData.ParseData);
		RegistTable("EmployeeQuest.csv", EmployeeQuestData.ParseData); 
		RegistTable("EmployeeMonster.csv", EmployeeMonsterData.ParseData);
		RegistTable("EmployeeMonSkill.csv", EmployeeMonsterSkillData.ParseData);
		RegistTable("EmployeeSkill.csv", EmployeeQuestSkillData.ParseData);
		RegistTable("CrystalUp.csv", CrystalUpData.ParseData);
		RegistTable("Crystal.csv", CrystalData.ParseData);
		RegistTable("CourseGift.csv", CourseGiftData.ParseData);
	}

	public delegate void ParseHandler (string content, string fileName);
	
	private Dictionary<string, ParseHandler> parseFunc;
	
	private void RegistTable(string name, ParseHandler handler)
	{
		if(parseFunc.ContainsKey(name))
		{
			ClientLog.Instance.LogError("more tables with same name!" + name);
			return;
		}
		
		parseFunc [name] = handler;
	}

	private void RegistFolder(string folderName, ParseHandler handler)
	{
		try
		{
			string filePath = Configure.cfgPath;
			if(!File.Exists(Configure.cfgPath + folderName))
				filePath = Configure.cfgPathStn;
			string[] files = Directory.GetFiles (filePath.Replace("file:///", "") + folderName);


			string fileName = "";
            for(int i=0; i < files.Length; ++i)
			{
                fileName = files[i].Substring(files[i].LastIndexOf("/"));
                if(fileName.Contains(".xml"))
				    RegistTable(folderName + files[i].Substring(files[i].LastIndexOf("/")), handler);
			}
		}
		catch(IOException e)
		{
			ClientLog.Instance.LogError(e.ToString());
		}

	}

	public void LoadAndParseData()
	{

		StartCoroutine (Load(ParseTable));
	}

	Dictionary<string, string> contents;
	
	public string csvext
	{
		get { return "csv"; }
	}

	public string xmlext
	{
		get { return "xml"; }
    }

	int total, current = 0;

	public delegate void CallBack();

	public delegate void ParseDataFinCallBack ();
	public ParseDataFinCallBack parseDataFin_;
	public RequestEventHandler<int> downFileEvent;
	public RequestEventHandler<int> startDownFileEvent;
	public RequestEventHandler<int> finishDownFileEvent;


	IEnumerator Load(CallBack callback)
	{

		Dictionary<string, ParseHandler> allTables = parseFunc;
		total = allTables.Count;

		if (startDownFileEvent != null)
			startDownFileEvent (total);
		int num = 1;
		foreach(string file in allTables.Keys)
		{
			++num;
			string filePath = Configure.cfgPath + file;
			filePath = filePath.Replace("file:///","");
			filePath = filePath.Replace("file://","");
			WWW www = null;
			if(File.Exists(filePath))
			{
				www = new WWW (Configure.cfgPath + file);
			}
			else
			{
				 www = new WWW (Configure.cfgPathStn + file);
			}

			yield return www;
			if (downFileEvent != null)
				downFileEvent (num/2);

			if(string.IsNullOrEmpty(www.error))
			{
				if(www.isDone)
				{
					string content = www.text;
					contents.Add(file, content);
					current ++;
					www.Dispose();
                }
			}
			else
			{
				ClientLog.Instance.LogError(www.error);
			}
		}
		if(total == current)
			callback();
		
    }

    public byte[] LoadTextFile(string url)
    {
        WWW www = new WWW(url);
        byte[] data = null;
        while (data == null)
        {
            float a = www.progress;
            if (string.IsNullOrEmpty(www.error))
            {
                if (www.isDone)
                {
                    data = www.bytes;
                    www.Dispose();
                }
            }
            else
            {
                ClientLog.Instance.LogError(www.error);
                return null;
            }
        }
        return data;
    }

	void ParseTable()
	{
        StartCoroutine(parseCroutine());
	}

    IEnumerator parseCroutine()
    {
        if (startDownFileEvent != null)
            startDownFileEvent(contents.Count);
        int num = 1;
        foreach (string table in contents.Keys)
        {
			++num;
            parseFunc[table](contents[table], table);

            if (downFileEvent != null)
				downFileEvent(contents.Count / 2 + num/2);

            yield return new WaitForEndOfFrame();
        }
        parseFunc.Clear();
        contents.Clear();

        if (parseDataFin_ != null)
            parseDataFin_();
        if (finishDownFileEvent != null)
            finishDownFileEvent(1);
    }

	public void ConfigLodaedEvent()
	{
		if (finishDownFileEvent != null)
			finishDownFileEvent (1);
	}
}
