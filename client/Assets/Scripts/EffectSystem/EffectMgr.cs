using UnityEngine;
using System.Collections.Generic;

public class EffectMgr {

	static private EffectMgr inst = null;
	static public EffectMgr Instance
	{
		get
		{
			if(inst == null)
				inst = new EffectMgr();
			return inst;
		}
	}

	private bool loading = false;

	public delegate void InstanceCallBack(EffectInst effInst, EffectAPI.MetaData metadata);

	private InstanceCallBack callback_;

	private Queue<LoadRequest> requestQue_;
	private List<EffectStruct> effectLst_;

    float effectLoadTimeout = 5f;
    float timeoutTimer = 0f;

    public class UIEffectInfo
    {
        public AssetBundle bundle_;
        public Transform parent_;
        public EffectAPI.InstCallBack instCallBack_;
        public Destroy.FinishCallBack finCallBack_;
        public bool finished_;
        public void Fin()
        {
            finished_ = true;
        }
        public bool showing_;
    }

    public Queue<UIEffectInfo> uiEffectQue_;

	public EffectMgr()
	{
		requestQue_ = new Queue<LoadRequest> ();
		effectLst_ = new List<EffectStruct> ();
        uiEffectQue_ = new Queue<UIEffectInfo>();
	}

	public void LoadEffect(EFFECT_ID effID, InstanceCallBack callback, EffectAPI.MetaData metadata)
	{
		if(EffectAssetsData.GetData((int)effID) == null)
			return;
		GameObject eff = GetEffect (effID);
		if(eff != null)
		{
			EffectInst ei = eff.AddComponent<EffectInst> ();
			ei.ID = effID;
			callback(ei, metadata);
			return;
		}

		requestQue_.Enqueue (new LoadRequest(effID, callback, metadata));
	}

	public void PreLoadEffect(EFFECT_ID effID, InstanceCallBack callback)
	{
		if(EffectAssetsData.GetData((int)effID) == null)
		{
			if(callback != null)
				callback(null, null);
			return;
		}
		if(HasEffect(effID))
		{
			if(callback != null)
				callback(null, null);
		}
		else
			requestQue_.Enqueue (new LoadRequest(effID, callback, null));
	}

	public void DeleteRef(EFFECT_ID id)
	{
		for(int i=0; i < effectLst_.Count; ++i)
		{
			if(effectLst_[i].id_.Equals(id))
			{
				effectLst_[i].refCount_ -= 1;
				if(effectLst_[i].refCount_ <= 0)
				{
					effectLst_[i].DestoryAsset(false);
					//effectLst_.RemoveAt(i);
				}
				break;
			}
		}
	}

	public void DeleteCache()
	{
		for(int i=0; i < effectLst_.Count; ++i)
		{
			float destoryTime = EffectBehaviourData.GetData(EffectAssetsData.GetData((int)effectLst_[i].id_).behaviour_id_).destory_time_;
			if(destoryTime == -1)
				continue;
			effectLst_[i].DestoryAsset(false);
			//effectLst_.RemoveAt(i--);
		}
	}

	public void DeleteAll()
	{
        if (effectLst_ == null)
            return;

		for(int i=0; i < effectLst_.Count; ++i)
		{
			effectLst_[i].DestoryAsset(true);
		}
        effectLst_.Clear();
	}

	public void Update()
	{
        if (uiEffectQue_.Count > 0)
        {
            UIEffectInfo info = uiEffectQue_.Peek();
            if (info.bundle_ == null)
            {
                uiEffectQue_.Dequeue();
                return;
            }
            if (!info.showing_)
            {
                GameObject effObj = (GameObject)GameObject.Instantiate(info.bundle_.mainAsset);
                effObj.transform.parent = info.parent_;
                effObj.transform.localScale = Vector3.one;
                if (info.instCallBack_ != null)
                    info.instCallBack_(effObj);
                Destroy de = effObj.GetComponent<Destroy>();
                if (de == null)
                {
                    de = effObj.AddComponent<Destroy>();
                    de.lifetime = 3f;
                }
                de.OnPlayFinish += info.finCallBack_;
                de.OnPlayFinish += info.Fin;
                info.showing_ = true;
                EffectAssetMgr.DeleteAsset(info.bundle_, false);
            }
            if (info.finished_)
            {
                uiEffectQue_.Dequeue();
				//EffectAssetMgr.DeleteAsset(info.bundle_, false);
            }
        }

        //if (loading)
        //{
        //    timeoutTimer += Time.deltaTime;
        //    if (timeoutTimer > effectLoadTimeout)
        //    {
        //        timeoutTimer = 0f;
        //        loading = false;
        //    }
        //    return;
        //}

		if(requestQue_.Count == 0)
			return;

		LoadRequest lr = requestQue_.Dequeue ();
		if(lr != null)
		{
			loading = true;
			EffectAssetMgr.LoadAsset (lr.id_, LoadAssetBundleCallBack, new ParamData(lr));
		}
	}

	void LoadAssetBundleCallBack(AssetBundle bundle, ParamData data)
	{
        if (bundle.mainAsset == null)
        {
            ClientLog.Instance.LogError("Effect bundle is broken.   ID : " + data.lrRequest.id_);
			return;
        }
		EffectInst ei = null;
		if(data.lrRequest.meta_data_ != null)
		{
			GameObject effObj = null;
			try
			{
				effObj = (GameObject)GameObject.Instantiate (bundle.mainAsset) as GameObject;
			}
			catch(System.InvalidCastException e)
			{
				effObj = new GameObject();
				ClientLog.Instance.Log("InvalidCastException    " + e.ToString());
			}
			ei = effObj.AddComponent<EffectInst> ();
			ei.ID = (EFFECT_ID)data.lrRequest.id_;
		}

		EffectStruct es = new EffectStruct ();
		es.id_ = (EFFECT_ID)data.lrRequest.id_;
		es.bundle_ = bundle;
		es.refCount_ += 1;
		effectLst_.Add (es);

		data.lrRequest.callback_ (ei, data.lrRequest.meta_data_);
		loading = false;
	}

	bool HasEffect(EFFECT_ID effID)
	{
		for(int i=0; i < effectLst_.Count; ++i)
		{
			if(effectLst_[i].id_.Equals(effID))
			{
				return true;
			}
		}
		return false;
	}

	GameObject GetEffect(EFFECT_ID effID)
	{
		for(int i=0; i < effectLst_.Count; ++i)
		{
			if(effectLst_[i].id_.Equals(effID))
			{
                if (effectLst_[i].bundle_ == null)
                {
                    effectLst_.RemoveAt(i);
                    break;
                }
                else
                {
                    effectLst_[i].refCount_ += 1;
                    return (GameObject)GameObject.Instantiate(effectLst_[i].bundle_.mainAsset) as GameObject;
                }
			}
		}
		return null;
	}
}

public class LoadRequest
{
	public EFFECT_ID id_;
	public EffectMgr.InstanceCallBack callback_;
	public EffectAPI.MetaData meta_data_;
	public LoadRequest(EFFECT_ID id, EffectMgr.InstanceCallBack callback, EffectAPI.MetaData metadata)
	{ 
		id_ = id;
		callback_ = callback;
		meta_data_ = metadata;
	}
}

public class EffectStruct
{
	public EFFECT_ID id_;
	public GameObject src_obj_;
	public int refCount_;
	public AssetBundle bundle_;

	public void DestoryAsset(bool clear)
	{
        EffectAssetMgr.DeleteAsset(bundle_, clear);
	}
}
