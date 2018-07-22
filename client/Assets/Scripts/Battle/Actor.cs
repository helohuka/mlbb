using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Actor : Entity
{
	int battlePosition_;
	public string attackAnim_;
	public string castAnim_;
	public delegate void MoveBackHandler();
	MoveBackHandler moveBackCallback_;

	public COM_BabyInst[] babiesList = null;
	public COM_EmployeeInst[] empList = null;

    public COM_ScenePlayerInformation playerSimp_ = new COM_ScenePlayerInformation();

    public void SetEntity(COM_Entity sPlayer)
	{
		base.SetEntity (sPlayer);
		//babiesList = sPlayer.babies1_;
		//empList = sPlayer.battleEmps_;
	}

    public void SetActor(COM_ScenePlayerInformation info)
    {
        playerSimp_ = info;
    }

	public int BattlePos
	{
		set { battlePosition_ = value; }
		get { return battlePosition_; }
	}

	int guardPosition_;

	public int ForGuardPos
	{
		set { guardPosition_ = value; }
		get
		{
			int tP = guardPosition_;
			guardPosition_ = (int)BattlePosition.BP_None;
			return tP;
		}
	}

	public void BackToOrigin(MoveBackHandler callback = null, float moveTime = 0.3f)
	{
		moveBackCallback_ = callback;
		Transform aimOrigin = Battle.Instance.GetStagePointByIndex (BattlePos);
        if (aimOrigin == null)
            return;

        if (ControlEntity == null)
        {
            ApplicationEntry.Instance.PostSocketErr(57557);
            return;
        }

		ControlEntity.MoveTo (aimOrigin.position, (int data) => {
            if (ControlEntity == null)
            {
                ApplicationEntry.Instance.PostSocketErr(57557);
                return;
            }
            ControlEntity.SetAnimationParam(GlobalValue.FMove, AnimatorParamType.APT_Float, GlobalValue.MoveMinGap);
			if(moveBackCallback_ != null)
			{
				moveBackCallback_();
				moveBackCallback_ = null;
			}
		}, false, false, moveTime);
	}

    public void ResetPos()
    {
        Battle.Instance.ResetActor(InstId);
    }

    public Vector3 SkillNamePos()
    {
        if (ControlEntity == null)
        {
            ApplicationEntry.Instance.PostSocketErr(57557);
            return Vector3.zero;
        }
        Vector3 pos = new Vector3(ControlEntity.ActorObj.transform.position.x, ControlEntity.ActorObj.transform.position.y + (ControlEntity.ActorObj.collider.bounds.size.y - ControlEntity.ActorObj.collider.bounds.center.y) / 2f, ControlEntity.ActorObj.transform.position.z);
        return pos;
        //Vector2 uiPos = GlobalInstanceFunction.WorldToUI(new Vector3(ControlEntity.ActorObj.transform.position.x, ControlEntity.ActorObj.transform.position.y + ControlEntity.ActorObj.collider.bounds.size.y, ControlEntity.ActorObj.transform.position.z));
        //return uiPos;
    }
}