using UnityEngine;
using System.Collections;

public class OnlineRewardCell : MonoBehaviour {

	public UITexture icon;
	public UILabel decLable;
	public UIButton enterBtn;
	public UISprite sp;
	private TimerReawData _timedata;
	public TimerReawData TimeReawData
	{
		set
		{
			if(value != null)
			{
				_timedata = value;
				ItemData idata = ItemData.GetData(_timedata._reward);
				HeadIconLoader.Instance.LoadIcon (idata.icon_, icon);
				UIManager.SetButtonEventHandler(enterBtn.gameObject, EnumButtonEvent.OnClick, OnenterBtn, 0, 0);
				int t = _timedata._time/60;
				decLable.text = LanguageManager.instance.GetValue("decqian")+t+LanguageManager.instance.GetValue("dechou");
				sp.gameObject.SetActive(false);
			}
		}
		get
		{
			return _timedata;
		}

	}
	private void OnenterBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(BagSystem.instance.BagIsFull())
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("bagfull"));
			return;
		}
		NetConnection.Instance.requestOnlineReward ((uint)TimeReawData._Id);
	}
	void Start () {

	}

	// Update is called once per frame
	void Update () {
	
	}
}
