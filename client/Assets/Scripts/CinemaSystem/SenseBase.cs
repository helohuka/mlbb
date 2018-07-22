using System;
using UnityEngine;
using System.Collections.Generic;

public class SenseBase : MonoBehaviour {

    public static Dictionary<SenseActorType, List<GameObject>> SenseActorObjDic_ = new Dictionary<SenseActorType, List<GameObject>>();

    public void Talk(SenseActorType sat, string talkKey, int index = 0, float time = 0f)
    {
        if (SenseActorObjDic_.ContainsKey(sat))
        {
            string combie = string.Format("{0}:{1}:{2}", sat, talkKey, index);
            NpcHeadChat.Instance.Show(LanguageManager.instance.GetValue(talkKey), SenseActorObjDic_[sat][index], TalkCallBack, combie, time);
            SenseActorObjDic_[sat][index].GetComponent<Animator>().SetTrigger("Talk");
        }
    }

    public virtual void SetUpActor() { }

    void TalkCallBack(string combieParam)
    {
        if(gameObject != null)
            SendMessage("TalkFin", combieParam);
    }

    protected void TalkFin(string paramCombie)
    {
        string[] userDatas = paramCombie.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
        SenseActorType sat = (SenseActorType)Enum.Parse(typeof(SenseActorType), userDatas[0]);
        string talkKey = userDatas[1];
        int index = int.Parse(userDatas[2]);
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_SenseTalked, (int)sat, int.Parse(talkKey), index);
    }
}
