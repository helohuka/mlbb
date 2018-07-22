using System;
using UnityEngine;
using System.Collections.Generic;

public class EffectInst : MonoBehaviour {

	// 特效播放完的回调
	public delegate void FinishCallBack(int data = 0);
	public FinishCallBack finish_call_back_;

	// 到达目标时的回调 param: index为目标的索引
	public delegate void ReachCallBack_1 (int index);
	public ReachCallBack_1 reach_call_back_1_;

	// 到达目标时的回调 param: index为目标的索引
	public delegate void ReachCallBack_2 (int index);
	public ReachCallBack_2 reach_call_back_2_;

	// 到达目标时的回调 param: index为目标的索引
	public delegate void ReachCallBack_Eff (int index);
	public ReachCallBack_Eff reach_call_back_eff_;

	// 到达目标时的回调 param: index为目标的索引
	public delegate void ReachCallBack_Pop (int index);
	public ReachCallBack_Pop reach_call_back_pop_;

	private EFFECT_ID id_;

	private int reach_back_index = 0;

	private EffectBehaviourData eb_data_;

	private bool active_;

	private float live_time_;

	private bool beattack_timer_;

	private bool effect_stop_move_;

    private bool cameraShake_;

	private float take_damage_time_;

	private float lerp_time_;

	private Vector3 from_;

	private AimStruct crt_aim_;

	private Queue<AimStruct> aimsQue_;

	private Bezier bezier_;

	private bool force_straight_;

	void Update()
	{
        if (gameObject == null)
        {
            DestorySelf();
            return;
        }

		// 不激活直接返回
		if(!active_)
			return;

        //行为不是陨石的
        if (eb_data_.work_type_ == EffectBehaviourData.WORKTYPE.T && eb_data_.id_ != 13)
            transform.localRotation = Quaternion.identity;

		// 生存时间累加
		live_time_ += Time.deltaTime;

		if(eb_data_.dmage_hit1_ == -1)
			beattack_timer_ = true;

		if(beattack_timer_)
			take_damage_time_ += Time.deltaTime;

		// 目的地不为空才会有移动
		if(crt_aim_ != null)
		{
			if(!effect_stop_move_)
			{
				// 直线行进
				if(eb_data_.move_type_.Equals(EffectBehaviourData.MOVETYPE.Straight) || force_straight_)
				{
					transform.position = Vector3.Lerp(from_, crt_aim_.pos_, (lerp_time_ += Time.deltaTime) / eb_data_.move_time_);
                    transform.LookAt(crt_aim_.pos_);
				}
				// 曲线行进
				else if(eb_data_.move_type_.Equals(EffectBehaviourData.MOVETYPE.Bezier))
				{
					transform.position = bezier_.GetPointAtTime((lerp_time_ += Time.deltaTime) / eb_data_.move_time_);
				}
				// 暂不处理
				else
				{
					
				}
			}

			if(eb_data_.dmage_hit1_ > 0f && live_time_ > eb_data_.dmage_hit1_ && reach_call_back_1_ != null)
			{
				reach_call_back_1_(GetValidateIndex(crt_aim_.index_, reach_back_index));
				reach_call_back_1_ = null;
				//受击计时器启动
				beattack_timer_ = true;
				//特效停止移动
				effect_stop_move_ = true;
			}

			// 如果初始点和目的地距离小于0.1f 则视为到达目的地
			if(active_ && Vector3.Distance(transform.position, crt_aim_.pos_) < 0.1f)
			{
				// 调用到达回调
				if(eb_data_.dmage_hit1_ == -2 && reach_call_back_1_ != null)
				{
					reach_call_back_1_(GetValidateIndex(crt_aim_.index_, reach_back_index));
					reach_call_back_1_ = null;
					lerp_time_ = 0f;
					//受击计时器启动
					beattack_timer_ = true;
					//特效停止移动
					effect_stop_move_ = true;
				}
			}

			#region 处理自受击1开始的所有回调
			
			if(take_damage_time_ > eb_data_.beattack_effect_ && eb_data_.beattack_effect_ != -1 && reach_call_back_eff_ != null)
			{
				reach_call_back_eff_(GetValidateIndex(crt_aim_.index_, reach_back_index));
				reach_call_back_eff_ = null;
			}
			
			if(take_damage_time_ > eb_data_.pop_value_ && eb_data_.pop_value_ != -1 && reach_call_back_pop_ != null)
			{
				reach_call_back_pop_(GetValidateIndex(crt_aim_.index_, reach_back_index));
				reach_call_back_pop_ = null;
			}

			if(take_damage_time_ > eb_data_.dmage_hit2_ && eb_data_.dmage_hit2_ != -1 && reach_call_back_2_ != null)
			{
				reach_call_back_2_(GetValidateIndex(crt_aim_.index_, reach_back_index));
				reach_call_back_2_ = null;
				//特效继续移动
				effect_stop_move_ = false;
				
				// 更新下一个目的地
				if(aimsQue_ != null && aimsQue_.Count > 0 && active_)
				{
					from_ = transform.position;
					crt_aim_ = aimsQue_.Dequeue();
					//transform.LookAt(crt_aim_.pos_);
                    if (aimsQue_.Count == 0 && true/*feng skill*/)
                    {
                        transform.parent = null;
                        force_straight_ = true;
                    }
				}
				else
					crt_aim_ = null;
			}
			#endregion
		}

        if (eb_data_.shake_time_ != -1 && !cameraShake_ && live_time_ > eb_data_.shake_time_)
        {
            iTween.ShakePosition(Camera.main.gameObject, Vector3.one * 0.1f, 0.5f);
            cameraShake_ = true;
        }

		// 如果生存时间大于endTime则调用完成回调以便衔接下一个动作
		if(live_time_ > eb_data_.end_time_ && finish_call_back_ != null)
		{
			DealNotExcuteHandler();
			finish_call_back_();
			finish_call_back_ = null;
		}

		// 生存时间大于销毁时间则销毁实例
		if(eb_data_.destory_time_ > 0f && live_time_ > eb_data_.destory_time_)
		{
			DealNotExcuteHandler();
			DestorySelf();
		}
	}

	public void DealNotExcuteHandler()
	{
		#region 处理可能由于卡顿造成的回调没有调用或者销毁时间小于回调时间的填表错误造成的卡住bug
		if(reach_call_back_1_ != null && eb_data_.dmage_hit1_ != -1)
			reach_call_back_1_(reach_back_index);
		if(reach_call_back_2_ != null && eb_data_.dmage_hit2_ != -1)
			reach_call_back_2_(reach_back_index);
		if(reach_call_back_eff_ != null && eb_data_.beattack_effect_ != -1)
			reach_call_back_eff_(reach_back_index);
		if(reach_call_back_pop_ != null && eb_data_.pop_value_ != -1)
			reach_call_back_pop_(reach_back_index);

		reach_call_back_1_ = null;
		reach_call_back_2_ = null;
		reach_call_back_eff_ = null;
		reach_call_back_pop_ = null;
		#endregion
	}

	// EffectMgr在实例化时会赋值，其他地方最好不要调用赋值
	public EFFECT_ID ID
	{
		set
		{
			id_ = value;
			EffectAssetsData ead = EffectAssetsData.GetData((int)id_);
			if(ead == null)
				ClientLog.Instance.LogError("EffectAssetData is null ID: " + value);
			eb_data_ = EffectBehaviourData.GetData(ead.behaviour_id_);
			if(eb_data_ == null)
				ClientLog.Instance.LogError("EffectAssetID: " + value + " has not BehaviourData!");
		}
		get
		{
			return id_;
		}
	}

	// 运行这个效果
	public void Go(Vector3 from, Vector3[] to = null, int index = 0)
	{
		Battle.BattleOver += DestorySelf;
        live_time_ = 0f;
		reach_back_index = index;
		if(to != null && to.Length == 0)
		{
			ClientLog.Instance.LogError("there is no TO point!");
		}

		from_ = from;
		if(to == null)
		{
			crt_aim_ = new AimStruct(0, from_);
		}
		else
		{
			aimsQue_ = new Queue<AimStruct> ();
			for(int i=0; i < to.Length; ++i)
			{
				aimsQue_.Enqueue(new AimStruct(i, to[i]));
			}
			crt_aim_ = aimsQue_.Dequeue ();
			//transform.LookAt(crt_aim_.pos_);
		}

		// 设置初始位置
		SetStartPoint ();

		force_straight_ = false;

		active_ = true;
	}

	void SetStartPoint()
	{
		Vector3 startPos = Vector3.zero;
		switch(eb_data_.work_type_)
		{
		case EffectBehaviourData.WORKTYPE.C:
		case EffectBehaviourData.WORKTYPE.CT:
			startPos = from_;
			break;
		case EffectBehaviourData.WORKTYPE.T:
			startPos = crt_aim_.pos_;
			break;
		case EffectBehaviourData.WORKTYPE.UT:
			//TODO
			startPos = new Vector3(crt_aim_.pos_.x, GameObject.Find("HighLevelPoint").transform.position.y, crt_aim_.pos_.z + 1f);
			break;
		case EffectBehaviourData.WORKTYPE.CENTERT:
			startPos = GameObject.Find("CenterPoint").transform.position;
			break;
		case EffectBehaviourData.WORKTYPE.OLT:
			//TODO
			break;
		case EffectBehaviourData.WORKTYPE.ORT:
			//TODO
			break;
		default:
			break;
		}
		from_ = startPos;
		transform.position = startPos;
		if(eb_data_.move_type_ == EffectBehaviourData.MOVETYPE.Bezier)
		{
			float x = UnityEngine.Random.Range (-2.8f, 2.8f);
			bezier_ = new Bezier(from_, new Vector3(x, 0f, 0f), Vector3.zero, crt_aim_.pos_);
			if(true/*and is fengmofa special skill dealer*/ && crt_aim_ != null)
			{

				Vector3 finalPos = crt_aim_.pos_;
				finalPos = NearestPoint(crt_aim_.pos_); //GameObject.Find( x > 0f? "RightPoint": "LeftPoint").transform.position;
				aimsQue_.Enqueue(new AimStruct(-1, finalPos));
			}
		}
	}

	Vector3 NearestPoint(Vector3 point)
	{
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("Points");
		Vector3 tPos = gos[0].transform.position;
		for(int i=0; i < gos.Length; ++i)
		{
			if(Vector3.Distance(tPos, point) > Vector3.Distance(gos[i].transform.position, point))
				tPos = gos[i].transform.position;
		}
		return tPos;
	}

	int GetValidateIndex(params int[] idxs)
	{
		int valid = 0;
		for(int i=0; i < idxs.Length; ++i)
		{
			if(idxs[i] < 0)
				continue;
			valid |= idxs[i];
		}
		return valid;
	}

	// 结束时必须调用 否则会造成泄漏
	public void DestorySelf()
	{
		if(active_ == false)
			return;
		active_ = false;
        Battle.BattleOver -= DestorySelf;
        if(gameObject != null)
            Destroy(gameObject);
	}

	public void SetPosition(Vector3 position)
	{
		transform.position = position;
	}

    //陨石特效需要扭一下
    public bool needRotate()
    {
        return eb_data_.id_ == 13;
    }

    void OnDestroy()
    {
        active_ = false;
        Battle.BattleOver -= DestorySelf;
        EffectMgr.Instance.DeleteRef(id_);
    }
}

// 目标的结构
public class AimStruct
{
	public int index_;
	public Vector3 pos_;
	public AimStruct(int idx, Vector3 pos){ index_ = idx; pos_ = pos; }
}
