using UnityEngine;
using System.Collections;

public class QrestACell : MonoBehaviour {

	public UISprite questBishi;
	public UISprite background;
	public UILabel decLabel;
	public string tishiLabel;
	public UILabel typeLabel;
	private  QuestData _qdata;
	public QuestData Qdata
	{
		set
		{
			if(value != null)
			{
				_qdata = value;


				if (_qdata.questKind_ == QuestKind.QK_Main)
	            {
					questBishi.spriteName = "zhuxian";
					typeLabel.text = LanguageManager.instance.GetValue("QK_Main");

	            }
	            else
	            {

					if(_qdata.questKind_ == QuestKind.QK_Profession)
					{
						typeLabel.text = LanguageManager.instance.GetValue("QK_Profession");
						questBishi.spriteName = "zhiye";

					}else if(_qdata.questKind_ == QuestKind.QK_Tongji)
					{
						questBishi.spriteName = "zhixian";
						typeLabel.text = LanguageManager.instance.GetValue("QK_Tongji");

					}
					else
						if(_qdata.questKind_ == QuestKind.QK_Sub)
					{
						typeLabel.text =LanguageManager.instance.GetValue("QK_Sub");
						questBishi.spriteName = "zhixian";
					}else
					{
						typeLabel.text =LanguageManager.instance.GetValue("QK_Daily");
						questBishi.spriteName = "zhixian";
					}

	            }
			}
		}
		get
		{
			return _qdata;
		}
	}
	void Start () {
	
	}
	

}
