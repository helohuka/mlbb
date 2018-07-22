using UnityEngine;

/*
 * special skill realize --- runAway from battle
 */

public class RunAway {

	public delegate void RunAwayCallBack (ParamData data);
	public RunAwayCallBack call_back_;

	bool suc_;

    BattleActor self_;
    BattleActor baby_;

    public void Run(BattleActor man, BattleActor baby, bool suc, RunAwayCallBack callback = null)
	{
		suc_ = suc;
		self_ = man;
		baby_ = baby;
		call_back_ = callback;
		Transform	t1 = Battle.Instance.GetStagePointByIndex( man.BattlePos);
		if( null == t1 ) return ;
		//
		man.ControlEntity.ActorObj.transform.Rotate (man.ControlEntity.ActorObj.transform.up, 180f);
//		man.ControlEntity.PlayEntityAction (GlobalValue.ActionName, GlobalValue.Action_Run);
        man.ControlEntity.SetAnimationParam(man.GetWeaponType() + GlobalValue.FMove, AnimatorParamType.APT_Float, GlobalValue.MoveMinGap * 2f);
		man.ControlEntity.SetEntityActionTime (2);

		GlobalInstanceFunction.Instance.Invoke (ExcuteRunAway, 2f);


	}

	void ExcuteRunAway()
	{
		self_.ControlEntity.SetEntityActionTime (1);
		if(suc_)
		{
			self_.ControlEntity.MoveTo(self_.ControlEntity.ActorObj.transform.localPosition + self_.ControlEntity.ActorObj.transform.forward * 6f, Exit, false);
			if(baby_ != null)
				baby_.ControlEntity.MoveTo(baby_.ControlEntity.ActorObj.transform.localPosition + self_.ControlEntity.ActorObj.transform.forward * 6f, null, false);
		}
		else
		{
			Transform	t1 = Battle.Instance.GetStagePointByIndex( self_.BattlePos);
			if( null == t1 ) return ;
			self_.ControlEntity.ActorObj.transform.localRotation = t1.localRotation;
            self_.ControlEntity.SetAnimationParam(self_.GetWeaponType() + GlobalValue.FMove, AnimatorParamType.APT_Float, GlobalValue.MoveMinGap);
			call_back_(null);
		}
	}

	void Exit(int iVal)
	{
//		SimServer.crt_state_ = SimServer.GameState.GS_Game;
		if(GamePlayer.Instance.InstId == self_.InstId)
		{
//			NetConnection.Instance.battleReportOver ();
			Battle.Instance.SetBattleState(Battle.BattleStateType.BST_Battlejustice);
		}
		else
		{
			Battle.Instance.DeleteBattleEntityItem(self_.InstId);
            if (baby_ != null)
                Battle.Instance.DeleteBattleEntityItem(baby_.InstId);
			//if(((GamePlayer)self_).BattleBaby != null)
			//	BattleDemoMain.Instance.DeleteBattleEntityItem(((GamePlayer)self_).BattleBaby.InstId);
			call_back_(null);
		}
//		BattleDemoMain.Instance.ExitBattle(BattleStateType.BST_Lose);
	}
}
