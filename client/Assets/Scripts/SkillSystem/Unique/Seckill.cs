using UnityEngine;
using System.Collections;

public class Seckill {

    public delegate void FinishCallBack();
    FinishCallBack callBack_;
    BattleActor caster_;
    BattleActor aim_;

    GameObject killEff = null;
    public void Do(BattleActor caster, BattleActor aim, FinishCallBack callback)
    {
        callBack_ = callback;
        caster_ = caster;
        aim_ = aim;
        if (caster_ != null)
        {
            EffectInst[] effs = caster_.ControlEntity.ActorObj.GetComponentsInChildren<EffectInst>();
            for (int i = 0; i < effs.Length; ++i)
            {
                effs[i].DealNotExcuteHandler();
                effs[i].DestorySelf();
            }
        }

        caster.ControlEntity.ActorObj.SetActive(false);
        caster.ControlEntity.ActorObj.transform.position = aim.ControlEntity.ActorObj.transform.position + aim.ControlEntity.ActorObj.transform.forward * -0.5f;
        caster.ControlEntity.ActorObj.transform.LookAt(aim.ControlEntity.ActorObj.transform);

        GlobalInstanceFunction.Instance.Invoke(() =>
        {
            caster.ControlEntity.ActorObj.SetActive(true);
            EffectAPI.PlaySceneEffect(EFFECT_ID.EFFECT_Kill, Vector3.zero, aim_.ControlEntity.ActorObj.transform, KillEffCallBack, true);
            GlobalInstanceFunction.Instance.Invoke(() =>
            {
                caster.ControlEntity.MoveTo(aim.ControlEntity.ActorObj.transform.position + aim.ControlEntity.ActorObj.transform.forward, (int data) =>
                {
                    GlobalInstanceFunction.Instance.Invoke(() =>
                    {
                        if(killEff != null)
                            GameObject.Destroy(killEff);
                        aim.SetIprop(PropertyType.PT_HpCurr, 0);
						if(aim.InstId == GamePlayer.Instance.InstId)
							AttaclPanel.Instance.ChangeValue(PropertyType.PT_HpCurr,GamePlayer.Instance.GetIprop(PropertyType.PT_HpMax) * -1,GamePlayer.Instance.GetIprop(PropertyType.PT_HpMax),GamePlayer.Instance.GetIprop(PropertyType.PT_MpMax));
                        if (Battle.Instance.SelfActorBattleBaby != null)
                        {
                            if (aim.InstId == Battle.Instance.SelfActorBattleBaby.InstId)
                            {
                                AttaclPanel.Instance.ChangeValueBaby(PropertyType.PT_MpCurr, aim.battlePlayer.hpMax_ * -1, aim.battlePlayer.hpMax_, aim.battlePlayer.mpMax_);
                            }
                        }
                        if(aim.ControlEntity.PlayerInfoUI != null)
						{
							Roleui ro = aim.ControlEntity.PlayerInfoUI.GetComponent<Roleui>();
							if( null == ro ) return;
							ro.ValueChange( PropertyType.PT_HpCurr , GamePlayer.Instance.GetIprop(PropertyType.PT_HpMax) * -1 , GamePlayer.Instance.GetIprop(PropertyType.PT_HpMax), GamePlayer.Instance.GetIprop(PropertyType.PT_MpMax),false,false);
						}
							
						aim.ControlEntity.hitOver_ = false;
                        aim.ControlEntity.DealEntityDie();
                        Wait4AimDie();
                    }, 0.7f);
                }, false, false, 0.3f, 0, false);
            }, 0.3f);
        }, 1f);
    }

    void KillEffCallBack(GameObject go)
    {
        killEff = go;
    }

    void Wait4AimDie()
    {
        if (aim_ == null || aim_.ControlEntity == null || aim_.ControlEntity.m_bPlayDieAcitonFinish)
        {
            caster_.BackToOrigin(() =>
            {
                if (callBack_ != null)
                    callBack_();
            });
        }
        else
        {
            GlobalInstanceFunction.Instance.Invoke(Wait4AimDie, 2);
        }
    }
}
