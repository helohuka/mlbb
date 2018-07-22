using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Baby : Actor
{
	public bool isForBattle_;
    public bool isShow_;
	public bool isLock;
	public int lastSellTime_;
	public uint intensifyLevel_;
	public int[] gear_ = new int[(int)BabyInitGear.BIG_Max];
	
	public void SetBaby(COM_BabyInst baby)
	{
		for(int i=0;i<baby.gear_.Length;i++)
		{
			gear_[i] = baby.gear_[i];
		}
		isForBattle_ = baby.isBattle_;
        isShow_ = baby.isShow_;
		isLock = baby.isLock_;
		lastSellTime_ = baby.lastSellTime_;
		intensifyLevel_ = baby.intensifyLevel_;
		SetEntity (baby);
	}

    public COM_BabyInst GetInst()
    {
        COM_BabyInst inst = new COM_BabyInst();
        inst.type_ = type_;
        inst.instId_ = (uint)InstId;
        inst.instName_ = InstName;
        inst.properties_ = properties_;
        inst.states_ = buffList_.ToArray();
        inst.skill_ = skillInsts_.ToArray();
        inst.gear_ = gear_;
		inst.isShow_ = isShow_;
		inst.isLock_ = isLock;
		inst.intensifyLevel_ = intensifyLevel_;
		inst.lastSellTime_ = (int)lastSellTime_;
        return inst;
    }

    public override void SetIprop(COM_PropValue[] props)
    {
        base.SetIprop(props);

        if (MainbabyUI.Instance != null)
        {
            MainbabyUI.Instance.markBabyOff();
        }
		if(MainPanle.Instance != null)
		{
			MainPanle.Instance.OnUpdateBaby(0);
		}
    }
}

