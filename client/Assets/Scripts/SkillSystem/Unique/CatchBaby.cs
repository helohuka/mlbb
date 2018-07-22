using UnityEngine;
using System.Collections;

public class CatchBaby {

    public delegate void CatchCallBack();

    CatchCallBack callBack_;

    BattleActor aim_;

    BattleActor master_;

	public void Catch(COM_ReportAction action, CatchCallBack callback)
    {
        callBack_ = callback;
        aim_ = Battle.Instance.GetActorByIdx(action.target_);
        master_ = Battle.Instance.GetActorByInstId(action.casterId_);

		if(aim_ == null)
			return;

		if(master_ == null)
			return;

        master_.ControlEntity.SetAnimationParam(GlobalValue.TCast, AnimatorParamType.APT_Trigger);
        EFFECT_ID id = (EFFECT_ID)SkillData.GetMinxiLevelData(action.skill_)._Cast_effectID;
		if(aim_.ControlEntity == null || aim_.ControlEntity.ActorObj == null)
			return;

        EffectAPI.Play(id, aim_.ControlEntity.ActorObj);
        GlobalInstanceFunction.Instance.ScaleLerp(aim_.ControlEntity.ActorObj.transform, 1f, 0.3f, 0.5f, () =>
        {
			if(aim_.ControlEntity == null || aim_.ControlEntity.ActorObj == null)
				return;
            GlobalInstanceFunction.Instance.ScaleLerp(aim_.ControlEntity.ActorObj.transform, 0.3f, 1f, 0.5f, () =>
            {
				if(aim_.ControlEntity == null || aim_.ControlEntity.ActorObj == null)
					return;
                GlobalInstanceFunction.Instance.ScaleLerp(aim_.ControlEntity.ActorObj.transform, 1f, 0.3f, 0.5f, () =>
                {
					if(aim_.ControlEntity == null || aim_.ControlEntity.ActorObj == null)
						return;
                    GlobalInstanceFunction.Instance.ScaleLerp(aim_.ControlEntity.ActorObj.transform, 0.3f, 1f, 0.5f, () =>
                    {
						if(aim_.ControlEntity == null || aim_.ControlEntity.ActorObj == null)
							return;
                        if (action.zhuachongOk_)
                        {
							if(aim_.ControlEntity == null || aim_.ControlEntity.ActorObj == null)
								return;
                            GlobalInstanceFunction.Instance.ScaleLerp(aim_.ControlEntity.ActorObj.transform, 1f, 0f, 0.5f, () =>
                            {
								Transform pos = Battle.Instance.GetStagePointByIndex(aim_.BattlePos);
								if(pos == null)
									return;
								EffectAPI.PlaySceneEffect(EFFECT_ID.EFFECT_Fengyinchenggong, Vector3.zero, pos, null, true);
                                Battle.Instance.DeleteBattleEntityItem(aim_.InstId);
                                Battle.Instance.DeleteDeadEntity(aim_.InstId);
                                GlobalInstanceFunction.Instance.Invoke(() =>
                                {
                                    if (callBack_ != null)
                                        callBack_();
                                }, 1f);
                            });
                        }
                        else
                        {
                            if (callBack_ != null)
                                callBack_();
                        }
                    });
                });
            });
        });
    }
}
