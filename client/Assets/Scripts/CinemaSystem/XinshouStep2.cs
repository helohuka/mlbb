using System;
using UnityEngine;
using System.Collections.Generic;

public class XinshouStep2 : SenseBase {

    public Animator king_;
    public Animator yingzi_;

	// Use this for initialization
	void Start () {
        
	}

    public override void SetUpActor()
    {
        if (SenseActorObjDic_ == null)
            SenseActorObjDic_ = new Dictionary<SenseActorType, List<GameObject>>();
        else
            SenseActorObjDic_.Clear();

        SenseActorObjDic_.Add(SenseActorType.SAT_King, new List<GameObject>() { king_.gameObject });
        SenseActorObjDic_.Add(SenseActorType.SAT_Yingzi, new List<GameObject>() { yingzi_.gameObject });
    }

    //国王准备好
    void OnGuowangReady()
    {
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_WaitTalk, (int)SenseActorType.SAT_King);
    }

    void GuowangZhaohuan()
    {
        king_.SetTrigger("Zhaohuan");
    }

    void OnYinyingReady()
    {
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_WaitTalk, (int)SenseActorType.SAT_Yingzi);
    }

    void OnTriggerEnter()
    {
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_SenseEnter2);
    }
}
