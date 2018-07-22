using UnityEngine;
using System.Collections.Generic;

public class EffectAPI 
{

	public delegate void EffectInstanceCallBack (EffectInst ei, ParamData data);

    public delegate void InstCallBack(GameObject obj);

	public delegate void EffectLoadFinishCallBack ();

	public static void Play(EFFECT_ID id, Vector3 caster, Vector3[] aims = null, TakeDmgCallBackPack takedmagepack = null, EffectInst.FinishCallBack finishcallback = null, EffectInstanceCallBack effectinstancecallback = null, ParamData data = null, Transform[] parents = null)
	{
		EffectAssetsData ead = EffectAssetsData.GetData ((int)id);
		if(ead == null)
		{
            ClientLog.Instance.LogError("could not find the effect asset by id: " + id + "  caster position is " + caster);
			return;
		}

		int SoundId = EffectAssetsData.GetData( (int)id ).SoundID;
		
		SoundTools.PlaySound((SOUND_ID)SoundId);

        //////////////////////////////////////////////////////////////////////////
        // aims 有可能是null
        // 但在进场景那一刻 有可能会出现 需要有目标的特效没有目标 可能会导致卡死
        // 考虑是否加一个完成回调

		EffectBehaviourData ebd = EffectBehaviourData.GetData (ead.behaviour_id_);
		if(ebd != null)
		{
			EffectBehaviourData.CASTTYPE castType = ebd.cast_type_;
			int maxCount = (aims == null? 0: aims.Length);
//			int maxCount = aims.Length;
			if(castType.Equals(EffectBehaviourData.CASTTYPE.SameTime) ||
                castType.Equals(EffectBehaviourData.CASTTYPE.OneByOne))
			{
				if(aims != null)
				{
					for(int i=0; i < maxCount; ++i)
					{
						EffectMgr.Instance.LoadEffect (
							id, EffectInstCallBack, 
							new MetaData(takedmagepack == null? null: takedmagepack.hit_1,
						             takedmagepack == null? null: takedmagepack.hit_2,
						             takedmagepack == null? null: takedmagepack.effect,
						             takedmagepack == null? null: takedmagepack.changeVal,
						             i == aims.Length - 1? finishcallback: null,
						             caster,
						             new Vector3[]{aims[i]}, data == null? i: data.iParam, effectinstancecallback, data == null? new ParamData(i): data, parents));
					}
				}
				else
				{
					EffectMgr.Instance.LoadEffect (id, EffectInstCallBack, 
					                               new MetaData(takedmagepack == null? null: takedmagepack.hit_1,
					             takedmagepack == null? null: takedmagepack.hit_2,
					             takedmagepack == null? null: takedmagepack.effect,
                                 takedmagepack == null ? null : takedmagepack.changeVal, finishcallback, caster, aims, 0, effectinstancecallback, data, parents));
				}
			}
			else if(castType.Equals(EffectBehaviourData.CASTTYPE.OnlyOne))
			{
				EffectMgr.Instance.LoadEffect (id, EffectInstCallBack, 
				                               new MetaData(takedmagepack == null? null: takedmagepack.hit_1,
				             takedmagepack == null? null: takedmagepack.hit_2,
				             takedmagepack == null? null: takedmagepack.effect,
                             takedmagepack == null ? null : takedmagepack.changeVal, finishcallback, caster, aims, 0, effectinstancecallback, data, parents));
			}
		}
		else
			EffectMgr.Instance.LoadEffect (id, EffectInstCallBack, 
			                               new MetaData(takedmagepack == null? null: takedmagepack.hit_1,
			             takedmagepack == null? null: takedmagepack.hit_2,
			             takedmagepack == null? null: takedmagepack.effect,
                         takedmagepack == null ? null : takedmagepack.changeVal, finishcallback, caster, aims, 0, effectinstancecallback, data, parents));
	}

	public static void Play( EFFECT_ID id, 
	                        GameObject caster, 
	                        GameObject[] aims = null, 
	                        TakeDmgCallBackPack takedmagepack = null, 
	                        EffectInst.FinishCallBack finishcallback = null, 
	                        EffectInstanceCallBack effectinstancecallback = null,
	                        ParamData data = null)
	{
		EffectAssetsData ead = EffectAssetsData.GetData ((int)id);
		if(ead == null)
			return;
		EffectBehaviourData ebd = EffectBehaviourData.GetData (ead.behaviour_id_);

		Vector3		vcaster = new Vector3( 0f , 0f , 0f );
		Vector3[]	vaims = null;
		if( null != aims )
		{
			vaims = new Vector3[aims.Length];
		}

		if( ebd.effect_positionType == EffectBehaviourData.EffectPositionType.Up )
		{
			if(caster != null)
				vcaster = new Vector3( caster.collider.bounds.center.x , caster.collider.bounds.center.y + caster.collider.bounds.size.y/2 , caster.collider.bounds.center.z );
			if( aims != null )
			{
				for( int iCount = 0; iCount < aims.Length; ++ iCount )
				{
					vaims[iCount] = new Vector3( aims[iCount].collider.bounds.center.x , aims[iCount].collider.bounds.center.y + aims[iCount].collider.bounds.size.y/2 , aims[iCount].collider.bounds.center.z );
				}
			}
		}
		else if( ebd.effect_positionType == EffectBehaviourData.EffectPositionType.Center )
		{
			if(caster != null)
				vcaster = caster.collider.bounds.center;
			if( aims != null )
			{
				for( int iCount = 0; iCount < aims.Length; ++ iCount )
				{
					vaims[iCount] = aims[iCount].collider.bounds.center;
				}
			}
		}
		else if( ebd.effect_positionType == EffectBehaviourData.EffectPositionType.Down )
		{
			if(caster != null)
				vcaster = new Vector3( caster.collider.bounds.center.x , caster.collider.bounds.center.y - caster.collider.bounds.size.y/2 , caster.collider.bounds.center.z );
			if( aims != null )
			{
				for( int iCount = 0; iCount < aims.Length; ++ iCount )
				{
					vaims[iCount] = new Vector3( aims[iCount].collider.bounds.center.x , aims[iCount].collider.bounds.center.y - aims[iCount].collider.bounds.size.y/2 , aims[iCount].collider.bounds.center.z );
				}
			}
		}
		else
		{

		}

		Play( id , vcaster , vaims , takedmagepack , finishcallback , effectinstancecallback, data);

	}

    

    public static void PlayUIEffect(EFFECT_ID id, Transform parent = null, Destroy.FinishCallBack finishEvent = null, InstCallBack instCallBack = null)
    {
        EffectMgr.UIEffectInfo uiei = new EffectMgr.UIEffectInfo();
        uiei.parent_ = parent;
        uiei.instCallBack_ = instCallBack;
        uiei.finCallBack_ = finishEvent;
        uiei.finished_ = false;

        EffectAssetMgr.LoadAsset(id, (AssetBundle bundle, ParamData data) =>
        {
            if (bundle.mainAsset == null)
            {
                ClientLog.Instance.LogError("Effect bundle is broken.   ID : " + id);
                return;
            }
            data.uiEffectInfo_.bundle_ = bundle;
            Transform tParent = data.uiEffectInfo_.parent_;
            if (tParent.Equals(ApplicationEntry.Instance.uiRoot.transform))
            {
                EffectMgr.Instance.uiEffectQue_.Enqueue(data.uiEffectInfo_);
            }
            else
            {
                GameObject effObj = (GameObject)GameObject.Instantiate(bundle.mainAsset);
                EffectAssetMgr.DeleteAsset(bundle, false);
                effObj.transform.parent = data.uiEffectInfo_.parent_;
                effObj.transform.localScale = Vector3.one;
                if (data.uiEffectInfo_.instCallBack_ != null)
                    data.uiEffectInfo_.instCallBack_(effObj);
                Destroy de = effObj.GetComponent<Destroy>();
                if (de == null)
                {
                    de = effObj.AddComponent<Destroy>();
                    de.lifetime = 3f;
                }
                de.OnPlayFinish += data.uiEffectInfo_.finCallBack_;
            }
        }, new ParamData(uiei));
    }

	public static void PlaySceneEffect(EFFECT_ID id, Vector3 localPos, Transform parent = null, InstCallBack callback = null, bool billboard = false, float offsetY = 0f)
    {
        EffectAssetMgr.LoadAsset(id, (AssetBundle bundle, ParamData data) =>
        {
            if (bundle.mainAsset == null)
            {
                ClientLog.Instance.LogError("Effect bundle is broken.   ID : " + id);
                return;
            }
            GameObject effObj = (GameObject)GameObject.Instantiate(bundle.mainAsset);
            EffectAssetMgr.DeleteAsset(bundle, false);
            effObj.transform.parent = data.tTransform_;
            effObj.transform.localPosition = new Vector3(localPos.x, localPos.y + offsetY, localPos.z);
            if (billboard)
                effObj.AddComponent<Billboard>();
            if (callback != null)
                callback(effObj);
        }, new ParamData(parent));
    }

	static int SyncLoadMax = 0;
	static int SyncLoadCounter = 0;
	static EffectLoadFinishCallBack SyncLoadCallBack = null;

	public static void Load(EffectLoadFinishCallBack callback, params int[] effectIds)
	{
		SyncLoadCallBack = callback;
		SyncLoadMax = effectIds.Length;
		for(int i=0; i < effectIds.Length; ++i)
		{
			EffectMgr.Instance.PreLoadEffect ((EFFECT_ID)effectIds[i], EffectInstSyncLoadCallBack);
		}
	}

	static void EffectInstSyncLoadCallBack(EffectInst inst, MetaData metadata)
	{
		SyncLoadCounter ++;
		if(SyncLoadCounter >= SyncLoadMax && SyncLoadCallBack != null)
		{
			SyncLoadCallBack();
			SyncLoadMax = 0;
			SyncLoadCounter = 0;
			SyncLoadCallBack = null;
		}
	}

	static void EffectInstCallBack(EffectInst inst, MetaData metadata)
	{
		//
		if( null == inst ) return;

//		int SoundId = EffectAssetsData.GetData( (int)inst.ID ).SoundID;
//
//		SoundTools.PlaySound((SOUND_ID)SoundId);

        if (metadata.parents_ != null && metadata.parents_.Length > 0)
            inst.transform.parent = metadata.parents_[metadata.index_];
        inst.transform.localScale = Vector3.one;
        inst.transform.localPosition = Vector3.zero;
        inst.transform.localRotation = Quaternion.identity;
		inst.reach_call_back_1_ = metadata.hcb_1_;
		inst.reach_call_back_2_ = metadata.hcb_2_;
		inst.reach_call_back_eff_ = metadata.hcb_eff_;
		inst.reach_call_back_pop_ = metadata.hcb_pop_;
		inst.finish_call_back_ = metadata.fcb_;
		inst.Go (metadata.caster_, metadata.aims_, metadata.index_);
		if(metadata.eicb_ != null)
			metadata.eicb_(inst, metadata.data_);
	}

	public class MetaData
	{
		public EffectInst.ReachCallBack_1 hcb_1_;
		public EffectInst.ReachCallBack_2 hcb_2_;
		public EffectInst.ReachCallBack_Eff hcb_eff_;
		public EffectInst.ReachCallBack_Pop hcb_pop_;
		public EffectInst.FinishCallBack fcb_;
		public EffectAPI.EffectInstanceCallBack eicb_;
		public Vector3 caster_;
		public Vector3[] aims_;
        public Transform[] parents_;
		public int index_;
		public ParamData data_;
		public MetaData(EffectInst.ReachCallBack_1 hcb1,
		                EffectInst.ReachCallBack_2 hcb2,
		                EffectInst.ReachCallBack_Eff effcb,
		                EffectInst.ReachCallBack_Pop popcb,
		                EffectInst.FinishCallBack fcb, Vector3 caster, Vector3[] aims, int index = 0, EffectAPI.EffectInstanceCallBack eicb = null, ParamData data = null, Transform[] parents = null)
		{
			hcb_1_ = hcb1;
			hcb_2_ = hcb2;
			hcb_eff_ = effcb;
			hcb_pop_ = popcb;
			fcb_ = fcb;
			eicb_ = eicb;
			caster_ = caster;
			aims_ = aims;
			index_ = index;
			data_ = data;
            parents_ = parents;
		}
	}

	public class TakeDmgCallBackPack
	{
		public EffectInst.ReachCallBack_1 hit_1;
		public EffectInst.ReachCallBack_2 hit_2;
		public EffectInst.ReachCallBack_Eff effect;
		public EffectInst.ReachCallBack_Pop changeVal;
	}
}
