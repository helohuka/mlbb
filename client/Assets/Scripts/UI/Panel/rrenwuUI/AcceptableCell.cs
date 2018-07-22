using UnityEngine;
using System.Collections;

public class AcceptableCell : MonoBehaviour {

	public UILabel nameLabel;
	public UILabel stateLabel;
	public UISprite stateSp;
	public UISprite statetwoSp;
	private QuestData _qdata;
	public QuestData Qdata
	{
		set
		{
			if(value != null)
			{
				_qdata = value;
				nameLabel.text = _qdata.questName_;
				if(QuestSystem.IsQuestFinish(_qdata.id_))
				{
					stateLabel.text = "完成";
				}else
				{
					stateLabel.text = "未开启";
				}
			}
		}
		get
		{
			return _qdata;
		}
	}
	void Start () {
		stateSp.gameObject.SetActive (false);
		statetwoSp.gameObject.SetActive (false);
	}
	

}
