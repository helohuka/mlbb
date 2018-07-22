using UnityEngine;

/*
 * special skill realize --- change position between player and baby
 */

public class ChangePosition {

	public delegate void ChangePositionCallBack(ParamData data);
	public ChangePositionCallBack callback_;

    private BattleActor a1_;
    private BattleActor a2_;

	int callbacknum;
	public void ChangePos(int pos1, int pos2, ChangePositionCallBack callback = null, float inTime = 1f)
	{
		a1_ = Battle.Instance.GetActorByIdx (pos1);
		a2_ = Battle.Instance.GetActorByIdx (pos2);
		callback_ = callback;

		if(a2_ == null)
		{
			callbacknum = 1;
			Transform t1 = Battle.Instance.GetStagePointByIndex(pos2);
			a1_.ControlEntity.MoveTo (t1.gameObject, MoveFinish, false);
			if(a1_.ControlEntity.PlayerInfoUI != null)
			{
				a1_.ControlEntity.PlayerInfoUI.transform.position = t1.position;
			}
			a1_.BattlePos = pos2;
		}
		else
		{
			callbacknum = 2;
			int tpos = a1_.BattlePos;
			a1_.BattlePos = a2_.BattlePos;
			a2_.BattlePos = (byte)tpos;
			if(a1_.ControlEntity.PlayerInfoUI != null)
			{
				Vector3 tui = a1_.ControlEntity.PlayerInfoUI.transform.position;
				a1_.ControlEntity.PlayerInfoUI.transform.position = a2_.ControlEntity.PlayerInfoUI.transform.position;
				a2_.ControlEntity.PlayerInfoUI.transform.position = tui;
			}
			
			// play animation
			a1_.ControlEntity.MoveTo (a2_.ControlEntity.ActorObj, MoveFinish, false, !a1_.isDead);
			a2_.ControlEntity.MoveTo (a1_.ControlEntity.ActorObj, MoveFinish, false, !a2_.isDead);
		}


	}

	void MoveFinish(int iVal)
	{
		callbacknum --;
		if(callbacknum == 0 && callback_ != null)
		{
			callback_(null);
		}

		if(a1_ != null && !a1_.isDead)
            a1_.ControlEntity.SetAnimationParam(GlobalValue.FMove, AnimatorParamType.APT_Float, GlobalValue.MoveMinGap);
//			a1_.ControlEntity.PlayEntityAction (GlobalValue.ActionName, GlobalValue.Action_Idle);
		if(a2_ != null && !a2_.isDead)
            a2_.ControlEntity.SetAnimationParam(GlobalValue.FMove, AnimatorParamType.APT_Float, GlobalValue.MoveMinGap);

		Transform t1 = null;
		Transform t2 = null;
		if(a1_ != null)
			t1 = Battle.Instance.GetStagePointByIndex( a1_.BattlePos );
		if(a2_ != null)
			t2 = Battle.Instance.GetStagePointByIndex( a2_.BattlePos );
		else
			t2 = Battle.Instance.GetStagePointByIndex( Battle.Instance.GetPairPos(a1_.BattlePos));
		if( null == t1 || t2 == null ) return ;
		//
		if(a1_ != null)
			a1_.ControlEntity.ActorObj.transform.localRotation = t1.localRotation;
		if(a2_ != null)
			a2_.ControlEntity.ActorObj.transform.localRotation = t2.localRotation;
	}
}
