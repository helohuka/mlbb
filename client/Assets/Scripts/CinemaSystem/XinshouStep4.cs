using UnityEngine;
using System.Collections.Generic;

public class XinshouStep4 : SenseBase {

    public GameObject[] monster_;

    public Animator cunzhang_;

	// Use this for initialization
	void Start () {
        
	}

    public override void SetUpActor()
    {
        if (SenseActorObjDic_ == null)
            SenseActorObjDic_ = new Dictionary<SenseActorType, List<GameObject>>();
        else
            SenseActorObjDic_.Clear();

        SenseActorObjDic_.Add(SenseActorType.SAT_VillageKing, new List<GameObject>() { cunzhang_.gameObject });
    }

    void OnCunzhangReady()
    {
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_WaitTalk, (int)SenseActorType.SAT_VillageKing);
    }

    void CunzhangZhaohuan()
    {
        cunzhang_.SetTrigger("Zhaohuan");
    }

    void OnZhaohuanFinish()
    {
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_WaitTalk, (int)SenseActorType.SAT_AllMonster);
    }
}
