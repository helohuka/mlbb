using UnityEngine;
using System.Collections;

public class TeamListCell : MonoBehaviour {

	public UILabel numberLabel;
	public UILabel NameLabel;
	public UILabel TypeLabel;
	public UILabel leaderLabel;
	public UILabel peopleNumLabel;

	private COM_SimpleTeamInfo _SimpleTeamInfo ;
	public COM_SimpleTeamInfo SimpleTeamInfo
	{
		set
		{
			if(value !=null)
			{
				_SimpleTeamInfo = value;
				numberLabel.text = _SimpleTeamInfo.teamId_.ToString();
				NameLabel.text = _SimpleTeamInfo.name_;
                //if(_SimpleTeamInfo.teamType_ == TeamType.TT_Pve)
                //{
                //    TypeLabel.text = "冒险";
                //}
                //else
                //    if(SimpleTeamInfo.teamType_ == TeamType.TT_Pvp)
                //{
                //    TypeLabel.text = "竞技";
                //}
				leaderLabel.text = _SimpleTeamInfo.leaderName_;
//				if(_SimpleTeamInfo.isBattle_)
//				{
//					peopleNumLabel.text = "战斗中";
//				}else
//				{
//					peopleNumLabel.text =_SimpleTeamInfo.curMemberSize_+"/"+ _SimpleTeamInfo.maxMemberSize_.ToString();
//				}
			}
		}
		get
		{
			return _SimpleTeamInfo;
		}
	}

	void Start () {
	
	}
	

}
