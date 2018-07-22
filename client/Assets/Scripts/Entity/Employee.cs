using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Employee :Actor
{
	public bool isForBattle_;
	public uint star_;
	public uint soul_;
	public QualityColor quality_;

	public void SetEntity(COM_EmployeeInst employee)
	{
		star_ = employee.star_;
		soul_ =employee.soul_;
		quality_ = employee.quality_;
		isForBattle_ = employee.isBattle_;
		base.SetEntity (employee);
	}

	public override void SetIprop(COM_PropValue[] props)
	{
        base.SetIprop(props);

        if (GamePlayer.Instance.UpdateEmployeeEnvent != null)
        {
            GamePlayer.Instance.UpdateEmployeeEnvent(this,0);
        }
    }
}

