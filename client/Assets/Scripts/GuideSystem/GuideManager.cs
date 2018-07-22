using UnityEngine;
using System.Collections.Generic;

public class GuideManager {

    static GuideManager inst_ = null;
    static public GuideManager Instance
    {
        get
        {
            if (inst_ == null)
                inst_ = new GuideManager();
            return inst_;
        }
    }

    public ulong progressMask_;

    Dictionary<GuideAimType, GameObject> guideAimDic_;

    GuideCreator creator_;

	float guideTimer_ = 0f;
	float guideMaxTime_ = 10f;

    public bool IsGuiding_;

    public bool InBattleGuide_;

    public enum GuideType
    {
        GT_None,
        GT_UI,
        GT_Scene,
    }

    public GuideType crtGuideType_ = GuideType.GT_None;

    public void Init(ulong progressMask)
    {
        progressMask_ = progressMask;
        IsGuiding_ = false;
    }

    public GuideManager()
    {
        StageMgr.OnSceneBeginLoad += OnSceneLoadBegin;
        guideAimDic_ = new Dictionary<GuideAimType, GameObject>();
        creator_ = new GuideCreator();
        creator_.Init();
    }

    public void RegistGuideAim(GameObject go, GuideAimType gaType)
    {
        if (!guideAimDic_.ContainsKey(gaType))
            guideAimDic_.Add(gaType, go);
        else
            guideAimDic_[gaType] = go;
    }

    public void RemoveGuideAim(GuideAimType gaType)
    {
        if (!guideAimDic_.ContainsKey(gaType))
            return;

        guideAimDic_.Remove(gaType);
    }

    public GameObject GetGuideAim(GuideAimType gaType)
    {
        if (!guideAimDic_.ContainsKey(gaType))
            return null;

        return guideAimDic_[gaType];
    }

    public void ClearGuideAim()
    {
        guideAimDic_.Clear();
        creator_.ClearGuide();
        IsGuiding_ = false;
        crtGuideType_ = GuideType.GT_None;
		guideTimer_ = 0f;
    }

    public void ClearMask()
    {
        IsGuiding_ = false;
		if(creator_ != null)
        	creator_.ClearGuide();
        crtGuideType_ = GuideType.GT_None;
		guideTimer_ = 0f;
    }

    public void Update()
    {
        if (IsGuiding_ == false)
            return;

        if (creator != null)
            creator.Update();

        if (creator.crtObj == null || creator.crtObj.activeSelf == false)
            ClearMask();

//		guideTimer_ += Time.deltaTime;
//		if(guideTimer_ > guideMaxTime_)
//			ClearMask();
    }

	public void CreateMask(GuideAimType gaType, float offsetX, float offsetY, GuidePointerRotateType rotateType, string str, int step, bool mask = false,float alpha = 0.7f)
    {
        if (!guideAimDic_.ContainsKey(gaType))
            return;
        IsGuiding_ = true;
        crtGuideType_ = GuideType.GT_UI;
		creator_.Create(guideAimDic_[gaType], offsetX, offsetY, rotateType, str,step, alpha,mask);
    }

	public void CreateMask(GameObject go, float offsetX, float offsetY, GuidePointerRotateType rotateType, string str,  int step, bool mask = false,float alpha = 0.7f)
    {
        IsGuiding_ = true;
        crtGuideType_ = GuideType.GT_UI;
		creator_.Create(go, offsetX, offsetY, rotateType, str,step, alpha,mask);
    }

    public bool ProcEvent(ScriptGameEvent sgEvent, int param = 0, int param1 = 0, int param2 = 0)
    {
        string err = "";
		object[] results = new object[]{false};
	
		GameScript.Call(sgEvent, new object[] { param, param1, param2 }, results, ref err);
        if (!string.IsNullOrEmpty(err))
        {
            ClientLog.Instance.LogWarning(err);
			return false;
        }
		return (bool)results[0];
    }

    public void SetFinish(int progressIdx)
    {
        progressMask_ |= (ulong)1 << progressIdx;

        // send to server.
        NetConnection.Instance.guideFinish(progressMask_);
		CinemaUI.HideMe();
    }

    public bool IsFinish(int progressIdx)
    {
        return (progressMask_ >> progressIdx) % 2 == 1;
    }

    //TODO TEST
	public void CreateMaskInScene(int npcId, float offsetX, float offsetY, GuidePointerRotateType rotateType,string str, int step,bool mask = false, float alpha = 0.7f)
    {
        IsGuiding_ = true;
        UIManager.Instance.DoDeActive();
        GameObject[] npc = GameObject.FindGameObjectsWithTag("NPC");
        for (int i = 0; i < npc.Length; ++i )
        {
            if (npc[i].name.Equals(npcId.ToString()))
            {
				creator_.CreateInScene(npc[i], offsetX, offsetY, rotateType,str,step, alpha,mask);
            }
        }
        crtGuideType_ = GuideType.GT_Scene;
    }

	public void CreateMaskInScene(GuideAimType type, float offsetX, float offsetY, GuidePointerRotateType rotateType, string str,int step,bool mask = false, float alpha = 0.7f)
    {
        IsGuiding_ = true;
        UIManager.Instance.DoDeActive();
        switch(type)
        {
            case GuideAimType.GAT_MainCrystal:
                GameObject aim = GameObject.FindGameObjectWithTag("PVE");
			creator_.CreateInScene(aim, offsetX, offsetY, rotateType, str, step, alpha,mask);
                break;
            case GuideAimType.GAT_MainJiubaHouse:
                GameObject aim2 = GameObject.FindGameObjectWithTag("jiuba");
			creator_.CreateInScene(aim2, offsetX, offsetY, rotateType, str, step, alpha,mask);
                break;
            case GuideAimType.GAT_MainCastle:
                GameObject aim3 = GameObject.FindGameObjectWithTag("Team");
			creator_.CreateInScene(aim3, offsetX, offsetY, rotateType, str, step, alpha,mask);
                break;
            case GuideAimType.GAT_MainJJC:
                GameObject aim4 = GameObject.FindGameObjectWithTag("arena");
			creator_.CreateInScene(aim4, offsetX, offsetY, rotateType, str, step, alpha,mask);
                break;
            default:
                break;
        }
        crtGuideType_ = GuideType.GT_Scene;
    }

	public void CreateMaskInBattle(int battlePos, float offsetX, float offsetY, GuidePointerRotateType rotateType, string str,int step,bool mask = false, float alpha = 0.7f)
    {
        IsGuiding_ = true;
        BattleActor actor = Battle.Instance.GetActorByIdx(battlePos);
		creator_.CreateInScene(actor.ControlEntity.ActorObj, offsetX, offsetY, rotateType, str, step, alpha,mask);
        crtGuideType_ = GuideType.GT_Scene;
    }

    public GameObject crtGuideObj
    {
        get
        {
            return creator_.crtObj;
        }
    }

    public GuideCreator creator
    {
        get
        {
            return creator_;
        }
    }

    public bool guideOpen
    {
        get
        {
            return GlobalValue.GuideOpen;
        }
    }

    void OnSceneLoadBegin()
    {
        ClearMask();
    }
}
