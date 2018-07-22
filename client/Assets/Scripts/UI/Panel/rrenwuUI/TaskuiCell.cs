using UnityEngine;
using System.Collections;

public class TaskuiCell : MonoBehaviour {

	public UILabel nameLabel;
	public UILabel stateLabel;

	private QuestData qdata_;
	public QuestData Qdata
	{
		set
		{
			if(value != null)
			{
				qdata_ = value;
				nameLabel.text = qdata_.questName_;
				if(QuestSystem.IsQuestFinish(qdata_.id_))
				{
					stateLabel.text = "完成";

				}else
				{
					stateLabel.text = "进行中";
				}
			}
		}
		get
		{
			return qdata_;
		}
	}
	void Start () {
	
	}
	

}
