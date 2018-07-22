using UnityEngine;
using System.Collections;

/*
 * special skill realize --- baby in or out
 */

public class BabyInnOut {
	
	public delegate void ScaleCallBack();

	OrderStatus isIn_;
    BattleActor player_;
    BattleActor babyInst_;
	ScaleCallBack call_back_;
	int pairPosition_;

    public void Excute(BattleActor player, OrderStatus isIn, int babyinst_id, COM_BattleEntityInformation babyInst, ScaleCallBack callback)
	{
		isIn_ = isIn;
		player_ = player;
		babyInst_ = GetActor(isIn, babyinst_id, babyInst);
		call_back_ = callback;
		SkillData sd = SkillData.GetMinxiLevelData (GlobalValue.SKILL_BABYINNOUT);

		player.castAnim_ = GlobalValue.TCast;
		player.ControlEntity.SetAnimationParam (player.castAnim_, AnimatorParamType.APT_Trigger);

		if(sd._Cast_effectID != -1)
			EffectAPI.Play((EFFECT_ID)sd._Cast_effectID, Battle.Instance.GetStagePointByIndex(babyInst_.BattlePos).gameObject, null, null, DoEffect);
		else
			DoEffect();

	}

    BattleActor GetActor(OrderStatus isIn, int instId, COM_BattleEntityInformation babyInst)
	{
        BattleActor baby = GamePlayer.Instance.FindBabyBattleActor(instId);
        if (isIn_.Equals(OrderStatus.OS_ShouBaobao))
        {
            //如果是自己的宝宝
            if (baby != null)
            {
                //改出站状态
                GamePlayer.Instance.BabyState(instId, false);
            }
            return Battle.Instance.GetActorByInstId(instId);
        }
        else
        {
            //如果是自己的宝宝
            if (baby != null)
            {
                pairPosition_ = Battle.Instance.GetPairPos(player_.BattlePos);
                baby.BattlePos = pairPosition_;
            }
            else //别人的宝宝
            {
                baby = new BattleActor();
                baby.SetBattlePlayer(babyInst);
                pairPosition_ = (int)babyInst.battlePosition_;
            }

            return baby;
        }
	}

	void DoEffect(int iVal = 0)
	{
		if(isIn_.Equals(OrderStatus.OS_ShouBaobao))
		{
			if(babyInst_ == null)
			{
				ClientLog.Instance.LogError("there is not baby to call in.");
				return;
			}

			ScaleObj(babyInst_.ControlEntity.ActorObj, 1f, 0.3f, () => {
				Battle.Instance.DeleteBattleEntityItem(babyInst_.InstId);
				if(call_back_ != null)
					call_back_();
			});
		}
		else
		{
            BattleActor originBaby = Battle.Instance.GetActorByIdx(pairPosition_);
			if(originBaby != null)
			{
                ScaleObj(originBaby.ControlEntity.ActorObj, 1f, 0.3f, () =>
                {
                    Battle.Instance.DeleteBattleEntityItem(originBaby.InstId);
					Battle.Instance.AddBattleEntityItem(babyInst_, (AssetBundle asset, ParamData data) => {
						ScaleObj(babyInst_.ControlEntity.ActorObj, 0.3f, 1f, () => {
							if(call_back_ != null)
								call_back_();
						});
					});
				});
			}
			else
			{
				Battle.Instance.AddBattleEntityItem(babyInst_, (AssetBundle asset, ParamData data) => {
                    ScaleObj(babyInst_.ControlEntity.ActorObj, 0.3f, 1f, () => {
						if(call_back_ != null)
							call_back_();
					});
				});
			}
        }
		if(Battle.Instance.SelfActor.InstId == player_.InstId)
			GamePlayer.Instance.BabyState(babyInst_.InstId, !isIn_.Equals(OrderStatus.OS_ShouBaobao));
		Battle.Instance.ExcuteBabyUIUpdate();
	}

	void ScaleObj(GameObject go, float from, float to, ScaleCallBack callback = null)
	{
		GlobalInstanceFunction.Instance.ScaleLerp (go.transform, from, to, 1f, () => {
			if(callback != null)
				callback();
		});
	}

	int GetValidPosition(int basePos)
	{
		if((basePos >= 0 && basePos < 5) || (basePos >= 15 && basePos < 20))
			return basePos + 5;
		else if((basePos >= 5 && basePos < 10) || (basePos >= 10 && basePos < 15))
			return basePos - 5;
		return basePos;
	}
}
