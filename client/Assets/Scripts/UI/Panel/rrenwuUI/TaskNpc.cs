 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TaskNpc : MonoBehaviour {

	// Use this for initialization

    
    const int Process_Finish = 3;
    const int Process_Accepting =2;
    const int Process_Doing = 1;
    

    int NpcId;
    int Process = 0;
    List<int> Quests = new List<int>();

	GameObject Title;

    bool hasDestroyed = false;

    public UILabel Name;

	void Start () {
        //GamePlayer.Instance.OnQuestUpdate += ChangeFuHao;
        //ChangeFuHao ();
		InitData ();
		QuestSystem.OnQuestUpdate += OnQuestUpdate;
        GamePlayer.Instance.PlayerLevelUpEvent += OnLevelUp;
        OnQuestUpdate();
        Name = (GameObject.Instantiate(ApplicationEntry.Instance.nameLabel) as GameObject).GetComponent<UILabel>();
		Name.transform.parent = ApplicationEntry.Instance.uiRoot.transform;
        Name.transform.localPosition = GlobalInstanceFunction.WorldToUI(gameObject.transform.position);
        Name.transform.localScale = Vector3.one;
        UISprite vip = Name.transform.GetComponentInChildren<UISprite>();
        vip.gameObject.SetActive(false);
        NpcData ndata = NpcData.GetData(NpcId);

        Name.text = string.Format("[b]{0}[-]", ndata.Name);
        TalkData talk = TalkData.GetData(ndata.NpcTalk);
        if (talk != null && talk.BattleId != 0)
            Name.color = new Color32(255, 144, 0, 255);
        else
            Name.color = new Color32(66, 255, 253, 255);

        if(Application.loadedLevelName.Equals(GlobalValue.StageName_JiazuPkScene))
        {
            bool isEnemyGuildMonster = familyMonsterData.isEnemyGuildMonster(GameManager.Instance.isLeft, NpcId);
            if(isEnemyGuildMonster)
                Name.color = GlobalValue.RED;
        }

        hasDestroyed = false;
    }

	void InitData()
	{
		Quests.Clear ();
		NpcId = int.Parse(gameObject.name);
		NpcData data = NpcData.GetData(NpcId);
		int[] queststr = data.Quests;
		for (int i = 0; i < queststr.Length; i++)
		{
			
			QuestData qd = QuestData.GetData (queststr[i]);
			//if(qd==null)return false;
			Profession pro = Profession.get ((JobType)GamePlayer.Instance.GetIprop (PropertyType.PT_Profession), GamePlayer.Instance.GetIprop (PropertyType.PT_ProfessionLevel));
			//if(pro==null)return false;
			if(qd.questKind_ == QuestKind.QK_Profession)
			{
				if(qd.JobLevel_ != 1 && qd.JobLevel_ != 0)
				{
					if(qd.jobtype_ != (int)GamePlayer.Instance.GetIprop (PropertyType.PT_Profession) || qd.JobLevel_ - GamePlayer.Instance.GetIprop (PropertyType.PT_ProfessionLevel) != 1)
					{
						continue;
					}
				}
			}
			if(QuestSystem.IsFDailyQuest())
			{
				if(qd.questKind_ == QuestKind.QK_Daily)
				continue;
			}
			Quests.Add(queststr[i]);
		}
		
		//如果是第一个职业任务npc 任务不在quest字段 而在附加参数字段 已避开正常任务的一部分逻辑
		if(data.AssetsId == UIASSETS_ID.UIASSETS_ProfessionPanel)
		{
			string[] questStr = data.BabySkillLearn.Split(';');
			for (int i = 0; i < questStr.Length; ++i)
			{
				Quests.Add(int.Parse(questStr[i]));
			}
		}

	}
	void LoadAssetBundleCallBack(AssetBundle bundle, ParamData data)
	{
		if (hasDestroyed)
			return;
		
        GameObject srcObj = bundle.mainAsset as GameObject;
        if (Title != null)
            Destroy(Title);
        Title = GameObject.Instantiate(srcObj) as GameObject;
        Title.transform.parent = gameObject.transform;
        BoxCollider collider = gameObject.GetComponent<BoxCollider>();
        float offsetY = collider.size.y;
        Title.transform.localPosition = new Vector3(0f, offsetY, 0f);
        EffectAssetMgr.DeleteAsset(bundle, false);
	}
	void OnDestroy()
	{
        hasDestroyed = true;
        QuestSystem.OnQuestUpdate -= OnQuestUpdate;
        GamePlayer.Instance.PlayerLevelUpEvent -= OnLevelUp;
        if (Title != null)
            Destroy(Title);

        if (Name != null && Name.gameObject != null)
            Destroy(Name.gameObject);
        
        Quests.Clear();
	}

    void Update()
    {
        if (Name == null)
        {
            ClientLog.Instance.Log("Name   is null....................");
            return;
        }
        if (!hasDestroyed && Name)
        {
            Name.transform.localPosition = GlobalInstanceFunction.WorldToUI(gameObject.transform.position);
        }
        if(!hasDestroyed && Title)
        {
            Title.transform.LookAt(Camera.main.transform);
        }
    }

    void OnLevelUp(int lv = 0)
    {
        OnQuestUpdate();
    }

	void OnQuestUpdate ()
	{
		InitData ();
		int currentprocess = 0;
        if (QuestSystem.IsQuestFinishNpc(NpcId))
        {
            currentprocess = Process_Finish;
        }
        else
        {

            for (int i = 0; i < Quests.Count; i++)
            {
                /*if (QuestSystem.IsQuestFinishNpc(NpcId))
                {
                    currentprocess = Process_Finish;
                    break ;
               
                }
                else */
                if (QuestSystem.IsQuestAcceptable(Quests[i]))
                {
                    currentprocess = Process_Accepting;
                    break;

                }
                else if (QuestSystem.IsQuestDoing(Quests[i]))
                {
                    QuestData questData = QuestData.GetData(Quests[i]);
                    if (questData.finishNpcId_ == NpcId)
                    {
                        currentprocess = Process_Doing;
                        break;
                    }
                }
            }
        }

        if (currentprocess == Process)
        {
            return;
        }
        Process = currentprocess;
        switch (Process)
        {
            case Process_Doing:
                EffectAssetMgr.LoadAsset(EFFECT_ID.EFFECT_huiwenhao, LoadAssetBundleCallBack, null);
                break;
            case Process_Accepting:
                EffectAssetMgr.LoadAsset(EFFECT_ID.EFFECT_tanhao, LoadAssetBundleCallBack, null);
                break;
            case Process_Finish:
                EffectAssetMgr.LoadAsset(EFFECT_ID.EFFECT_huangwenhao, LoadAssetBundleCallBack, null);
                break;
            default:
                if (Title != null)
                    Destroy(Title);
                break;
        }
	}

    public void ShowName()
    {
        if (Name != null && Name.gameObject != null)
            Name.gameObject.SetActive(true);
    }

    public void HideName()
    {
        if(Name != null && Name.gameObject != null)
            Name.gameObject.SetActive(false);
    }
}
