using UnityEngine;
using System.Collections;

public class ArenaRecordCellUI : MonoBehaviour
{
	public UIButton revengeBtn;
	public UILabel defierLab;
	public UILabel bydefierLab;
	public UILabel winLab;
	public UILabel rankLab;
	public UISprite winImg;

	private COM_JJCBattleMsg _battleMsg;


	void Start ()
	{
		UIManager.SetButtonEventHandler (revengeBtn.gameObject, EnumButtonEvent.OnClick, OnRevenge, 0, 0);
	}
	
	public COM_JJCBattleMsg BattleMsg 
	{
		set
		{
			if(value != null)
			{
				_battleMsg = value;
				defierLab.text =  _battleMsg.defier_;
				bydefierLab.text = _battleMsg.bydefier_;
				//rankLab.text = _battleMsg.rank_.ToString();
		
				if(_battleMsg.isWin_)
				{
					revengeBtn.gameObject.SetActive(false);
					winLab.text= LanguageManager.instance.GetValue("arenarecordwininfo").Replace("{n}",_battleMsg.rank_.ToString());
				}
				else
				{
					revengeBtn.gameObject.SetActive(false);
					if(_battleMsg.defier_ == GamePlayer.Instance.InstName)
					{
						winLab.text= LanguageManager.instance.GetValue("arenarecordfailno").Replace("{n}",_battleMsg.rank_.ToString());
					}
					else
					{
						winLab.text= LanguageManager.instance.GetValue("arenarecordfailfinfo").Replace("{n}",_battleMsg.rank_.ToString());
					}
				}
			}
		}
		get
		{
			return _battleMsg;
		}
	}


	private void OnRevenge(ButtonScript obj, object args, int param1, int param2)
	{
		string name = BattleMsg.defier_;
		if( name == GamePlayer.Instance.InstName)
		{
			name = BattleMsg.bydefier_;
		}
		NetConnection.Instance.requestChallenge (name);
	}



}

