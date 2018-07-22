using System;
using UnityEngine;
using System.Collections.Generic;

public class XinshouStep1 : SenseBase {

    public Animator[] guards_;

    public Animator ambassador_;

	// Use this for initialization
	void Start () {
        
	}

    public override void SetUpActor()
    {
        if (SenseActorObjDic_ == null)
            SenseActorObjDic_ = new Dictionary<SenseActorType, List<GameObject>>();
        else
            SenseActorObjDic_.Clear();

        SenseActorObjDic_.Add(SenseActorType.SAT_Ambassdor, new List<GameObject>() { ambassador_.gameObject });

        for (int i = 0; i < guards_.Length; ++i)
        {
            if (SenseActorObjDic_.ContainsKey(SenseActorType.SAT_Guard))
                SenseActorObjDic_[SenseActorType.SAT_Guard].Add(guards_[i].gameObject);
            else
                SenseActorObjDic_.Add(SenseActorType.SAT_Guard, new List<GameObject>() { guards_[i].gameObject });
           // guards_[i].SetTrigger("Move");
        }
      //  ambassador_.SetTrigger("Move");
    }

    //当士兵到达位置
    void OnGuardMovedToAim()
    {
        for (int i = 0; i < guards_.Length; ++i)
        {
            if (i < guards_.Length / 2)
                guards_[i].SetTrigger("TurnLeft");
            else
                guards_[i].SetTrigger("TurnRight");
        }

        //1.77为士兵转向所需时间
        Invoke("Hoory", 0.8f);
    }

    void Hoory()
    {
        //抛事件给脚本 看是否有对话
        for (int i = 0; i < guards_.Length; ++i)
        {
            if (i < guards_.Length / 2)
                guards_[i].SetTrigger("HooryLeft");
            else
                guards_[i].SetTrigger("HooryRight");

            //卫兵可以开始说话了
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_WaitTalk, (int)SenseActorType.SAT_Guard, i);
        }
    }

    //大臣到位
    void OnAmMovedToAim()
    {
        ambassador_.SetTrigger("Idle");
        //抛事件给脚本 看是否有对话
        Invoke("AmTalk", 0.5f);
    }

    void AmTalk(/*string talk*/)
    {
        //卫兵可以开始说话了
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_WaitTalk, (int)SenseActorType.SAT_Ambassdor);
        ambassador_.SetTrigger("Talk");
    }

    void GuardTalk(int idx, string talk)
    {
        //guards_[idx].SetTrigger("Talk");
    }
}
