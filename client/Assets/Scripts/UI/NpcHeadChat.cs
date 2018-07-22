using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NpcHeadChat 
{
	Object panelObj_;
	static NpcHeadChat inst_;
	List<GameObject> npcObjList = new List<GameObject>();
	List<GameObject> chatUIObjList = new List<GameObject>();

	public static NpcHeadChat Instance
	{
		get
		{
			if (inst_ == null)
			{
				inst_ = new NpcHeadChat();
			}
			return inst_;
		}
	}


	public void Init()
	{
        if (panelObj_ == null)
        {
            UIAssetMgr.LoadUI("headChatUI", (AssetBundle bundle, ParamData data) =>
            {
                panelObj_ = bundle.mainAsset;
            }, null);
        }
	}


	public void Show(string content,GameObject obj, NpcHeadChatUI.DestroyHandler callback, string combieParam, float time)
	{
		if (obj == null)
			return;

        //if (!npcObjList.Contains(obj))
        //{
        //    npcObjList.Add(obj);

            GameObject clone = (GameObject)GameObject.Instantiate(panelObj_);

            UIPanel rootPane = ApplicationEntry.Instance.uiRoot.GetComponent<UIPanel>();

            clone.transform.parent = rootPane.transform;
            clone.transform.localScale = Vector3.one;

            Vector3 Objpos = obj.transform.position;
            Objpos = new Vector3(Objpos.x, Objpos.y + obj.transform.GetComponent<BoxCollider>().size.y, Objpos.z);

            Vector3 pos = GlobalInstanceFunction.WorldToUI(Objpos);
			pos = new Vector3 (pos.x, pos.y + 80, pos.z);
            clone.transform.localPosition = pos;
            //clone.GetComponent<Destroy> ().CancelInvoke ();
            NpcHeadChatUI nhcUI = clone.GetComponent<NpcHeadChatUI>();
            nhcUI.textLab.text = content;
            nhcUI.SetParam(callback, combieParam);

            Destroy des = clone.AddComponent<Destroy>();
            if (Mathf.Approximately(time, 0f))
                des.lifetime = content.Length * 0.5f;
            else
                des.lifetime = time;

            chatUIObjList.Add(clone);
            for (int i = 0; i < chatUIObjList.Count; ++i)
            {
                if (chatUIObjList[i] == null)
                {
                    chatUIObjList.RemoveAt(i);
                }
            }
        //}
	}

    public void Clear()
    {
        for (int i = 0; i < chatUIObjList.Count; ++i)
        {
            if (chatUIObjList[i] != null)
            {
                GameObject.Destroy(chatUIObjList[i]);
            }
        }
    }
}

