using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeathMaster : MonoBehaviour {

    public enum DieActionType
    {
        DAT_None,
        DAT_Normal,
        DAT_Fly,
        DAT_Knock,
        DAT_Max,
    }

    BattleActor actor_;

    DieActionType dieType_;

    List<GameObject> kpAs_; //上右
    List<GameObject> kpBs_; //上左
    List<GameObject> kpCs_; //下右
    List<GameObject> kpDs_; //下左

    GameObject sideA_;  //上右远端
    GameObject sideB_;  //上左远端
    GameObject sideC_;  //下右远端
    GameObject sideD_;  //下左远端

    float flyTime_; //击飞 时间计时

    Vector3 flyFrom_, flyTo_;

    public void Do(DieActionType dieType, BattleActor actor)
    {
        if(dieType == DieActionType.DAT_Fly ||
            dieType == DieActionType.DAT_Knock)
        {
            if(GamePlayer.Instance.BattleBaby != null &&
                GamePlayer.Instance.BattleBaby.InstId == actor.InstId)
            {
                GamePlayer.Instance.BabyState(GamePlayer.Instance.BattleBaby.InstId, false);
            }
        }
        dieType_ = dieType;
        actor_ = actor;
        switch (dieType_)
        {
            case DieActionType.DAT_Normal:
                actor_.ControlEntity.SetAnimationParam(GlobalValue.BDead, AnimatorParamType.APT_Boolean, true);
                if (actor_.battlePlayer.type_ == EntityType.ET_Monster && Battle.Instance.isEnemy(actor_.BattlePos))
                {
                    Invoke("DealAIDie", 1.5f);
                }
                else
                {
                    EffectAPI.Play(EFFECT_ID.Effect_Die, actor_.ControlEntity.ActorObj, null, null, null, actor_.ControlEntity.StarEffectInstCallBack);
                }
                break;
            case DieActionType.DAT_Fly:
                //ClientLog.Instance.Log("Actor die for Fly.");
                flyTime_ = 0f;
                flyFrom_ = actor_.ControlEntity.ActorObj.transform.position;
                flyTo_ = actor_.ControlEntity.ActorObj.transform.position + actor_.ControlEntity.ActorObj.transform.forward * -10f;
                
                //用这个moveTo会有问题 不到达目的地 没有回调

                //actor_.ControlEntity.MoveTo(actor_.ControlEntity.ActorObj.transform.localPosition + actor_.ControlEntity.ActorObj.transform.forward * -3f, (int data) =>
                //{
                //    Battle.Instance.DeleteBattleEntityItem(actor_.InstId);
                //    actor_.ControlEntity.DieActionFinishCallBack();
                //}, false, false, 0.5f);
                break;
            case DieActionType.DAT_Knock:
                //ClientLog.Instance.Log("Actor die for Knock.");
                MakeTrack();
                knockQue_ = new Queue<GameObject>();
                for (int i = 0; i < trackObjs_.Length; ++i)
                {
                    knockQue_.Enqueue(trackObjs_[i]);
                }
                MoveToOneKP();
                break;
            default:
                break;
        }
    }


    GameObject[] knockPoints_;

    GameObject[] trackObjs_;

    Queue<GameObject> knockQue_;

    float knockSpeed_ = 15f;

	void Awake () {
        kpAs_ = new List<GameObject>();
        kpBs_ = new List<GameObject>();
        kpCs_ = new List<GameObject>();
        kpDs_ = new List<GameObject>();

        knockPoints_ = GameObject.FindGameObjectsWithTag("KnockPoint");
        for (int i = 0; i < knockPoints_.Length; ++i )
        {
            if(knockPoints_[i].name.Contains("A_side"))
            {
                sideA_ = knockPoints_[i];
            }
            else if (knockPoints_[i].name.Contains("B_side"))
            {
                sideB_ = knockPoints_[i];
            }
            else if (knockPoints_[i].name.Contains("C_side"))
            {
                sideC_ = knockPoints_[i];
            }
            else if (knockPoints_[i].name.Contains("D_side"))
            {
                sideD_ = knockPoints_[i];
            }
            else
            {
                if (knockPoints_[i].name.Contains("A"))
                {
                    kpAs_.Add(knockPoints_[i]);
                }
                else if (knockPoints_[i].name.Contains("B"))
                {
                    kpBs_.Add(knockPoints_[i]);
                }
                else if (knockPoints_[i].name.Contains("C"))
                {
                    kpCs_.Add(knockPoints_[i]);
                }
                else if (knockPoints_[i].name.Contains("D"))
                {
                    kpDs_.Add(knockPoints_[i]);
                }
            }
        }
	}

    void MakeTrack()
    {
        if (actor_.BattlePos >= (int)BattlePosition.BP_Down0 && actor_.BattlePos <= (int)BattlePosition.BP_Down9)
        {
            trackObjs_ = new GameObject[4];
            trackObjs_[0] = kpDs_[Random.Range(0, kpDs_.Count)];
            trackObjs_[1] = kpCs_[Random.Range(0, kpCs_.Count)];
            trackObjs_[2] = kpAs_[Random.Range(0, kpAs_.Count)];
            trackObjs_[3] = sideB_;
        }
        else
        {
            trackObjs_ = new GameObject[4];
            trackObjs_[0] = kpBs_[Random.Range(0, kpBs_.Count)];
            trackObjs_[1] = kpAs_[Random.Range(0, kpAs_.Count)];
            trackObjs_[2] = kpCs_[Random.Range(0, kpCs_.Count)];
            trackObjs_[3] = sideD_;
        }
    }

    void MoveToOneKP(int data = 0)
    {
        if (knockQue_ != null && knockQue_.Count > 0)
        {
            GameObject aim = knockQue_.Dequeue();
            flyFrom_ = actor_.ControlEntity.ActorObj.transform.position;
            flyTo_ = aim.transform.position;
            //float dis = Vector3.Distance(aim.transform.position, actor_.ControlEntity.ActorObj.transform.position);
            //actor_.ControlEntity.MoveTo(aim, MoveToOneKP, false, false, dis / knockSpeed_, 0, false);
        }
        else
        {
            Battle.Instance.DeleteBattleEntityItem(actor_.InstId);
            actor_.ControlEntity.DieActionFinishCallBack();
            //if (actor_.InstId == GamePlayer.Instance.InstId)
            //{
            //    Battle.Instance.ReturnMainScene = true;
            //}
        }
    }

	// Update is called once per frame
	void Update () {
        if (actor_ == null)
            return;

        if (dieType_ == DieActionType.DAT_Knock)
        {
            flyTime_ += Time.deltaTime * 2f;
            actor_.ControlEntity.ActorObj.transform.position = Vector3.Lerp(flyFrom_, flyTo_, flyTime_);
            if (flyTime_ > 1f)
            {
                MoveToOneKP();
                flyTime_ = 0f;
            }
            actor_.ControlEntity.ActorObj.transform.Rotate(Vector3.up, Mathf.Rad2Deg * Mathf.Sin(Time.time * 1000f) * Mathf.PI);
        }

        if (dieType_ == DieActionType.DAT_Fly)
        {
            flyTime_ += Time.deltaTime * 2f;
            actor_.ControlEntity.ActorObj.transform.position = Vector3.Lerp(flyFrom_, flyTo_, flyTime_);
            if (flyTime_ > 1.5f)
            {
                Battle.Instance.DeleteBattleEntityItem(actor_.InstId);
                actor_.ControlEntity.DieActionFinishCallBack();
                //if (actor_.InstId == GamePlayer.Instance.InstId)
                //{
                //    Battle.Instance.ReturnMainScene = true;
                //}
            }
        }
	}

    void DealAIDie()
    {
        Battle.Instance.DeleteBattleEntityItem(actor_.InstId);
        actor_.ControlEntity.DieActionFinishCallBack();
    }
}
