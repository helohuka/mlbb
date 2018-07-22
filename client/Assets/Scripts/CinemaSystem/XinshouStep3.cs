using UnityEngine;
using System.Collections.Generic;

public class XinshouStep3 : SenseBase {

    public Animator pAxe_;
    public Animator pArchor_;
    public Animator pMage_;
    public Animator pSage_;
    public Animator pGirl_;

	// Use this for initialization
	void Start () {
        
	}

    public override void SetUpActor()
    {
        if (SenseActorObjDic_ == null)
            SenseActorObjDic_ = new Dictionary<SenseActorType, List<GameObject>>();
        else
            SenseActorObjDic_.Clear();

        SenseActorObjDic_.Add(SenseActorType.SAT_Axe, new List<GameObject>() { pAxe_.gameObject });
        SenseActorObjDic_.Add(SenseActorType.SAT_Archor, new List<GameObject>() { pArchor_.gameObject });
        SenseActorObjDic_.Add(SenseActorType.SAT_Mage, new List<GameObject>() { pMage_.gameObject });
        SenseActorObjDic_.Add(SenseActorType.SAT_Sage, new List<GameObject>() { pSage_.gameObject });
        SenseActorObjDic_.Add(SenseActorType.SAT_Girl, new List<GameObject>() { pGirl_.gameObject });
    }

    void OnPartnerReady()
    {
        pAxe_.SetTrigger("Idle");
        pArchor_.SetTrigger("Idle");
        pMage_.SetTrigger("Idle");
        pSage_.SetTrigger("Idle");
        Invoke("OnAxeReady", 1f);
    }

    void OnAxeReady()
    {
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_WaitTalk, (int)SenseActorType.SAT_Axe);
    }
}
