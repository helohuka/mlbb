using UnityEngine;
using System.Collections.Generic;

public class NoticeManager {

    static NoticeManager inst_ = null;
    public static NoticeManager Instance
    {
        get 
        {
            if (inst_ == null)
            {
                inst_ = new NoticeManager();
                inst_.Init();
            }
            return inst_;
        }
    }

    public delegate void NoticeReceiveHandler(string content);
    public event NoticeReceiveHandler OnNoticeReceived;

    Queue<string> noticeQue_;

    Queue<string> vipNoticeQue_;

    UIEventListener.VoidDelegate callback_;

    public void Init()
    {
        noticeQue_ = new Queue<string>();
        vipNoticeQue_ = new Queue<string>();
        UIAssetMgr.LoadUI("Notice", (AssetBundle bundle, ParamData data) => 
        {
            GameObject noticePanel = GameObject.Instantiate(bundle.mainAsset) as GameObject;
            noticePanel.transform.parent = ApplicationEntry.Instance.uiRoot.transform;
            noticePanel.transform.localScale = Vector3.one;
            //GlobalInstanceFunction.Instance.Invoke(() =>
            //{
            //    bundle.Unload(false);
            //    bundle = null;
            //}, 1);
        }, null);
    }

    public void ShowUpdateNotice(string title, string msg, UIEventListener.VoidDelegate callback = null)
    {
		string[] strArr = msg.Split(';');
		if(strArr == null || strArr.Length < 2)
			strArr = new string[2];
        updateNoticeUI.ShowMe(title, strArr[1], strArr[0], callback);
    }

    public void PushNotice(string msg, bool vip)
    {
        if (vip)
            vipNoticeQue_.Enqueue(msg);
        else
            noticeQue_.Enqueue(msg);
        if (OnNoticeReceived != null)
            OnNoticeReceived(msg);
    }

    public string LastedNotice
    {
        get
        {
            if (vipNoticeQue_.Count > 0)
                return vipNoticeQue_.Dequeue();
            if (noticeQue_.Count > 0)
                return noticeQue_.Dequeue();
            return "";
        }
    }

    public bool HasNotice
    {
        get
        {
            if (vipNoticeQue_.Count == 0)
                return noticeQue_.Count > 0;
            return vipNoticeQue_.Count > 0;
        }
    }
}
