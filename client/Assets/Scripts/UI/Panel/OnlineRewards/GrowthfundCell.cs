using UnityEngine;
using System.Collections;

public class GrowthfundCell : MonoBehaviour {

	public UITexture icon;
	public UILabel decLable;
	public UIButton enterBtn;
	public UISprite sp;
	public UILabel btnLab;
	private GrowthFundData _growthdata;
	public GrowthFundData  GrowthReawData
	{
		set
		{
			if(value != null)
			{
				_growthdata = value;
				ItemData idata = ItemData.GetData(_growthdata._reward);
				HeadIconLoader.Instance.LoadIcon (idata.icon_, icon);
				UIManager.SetButtonEventHandler(enterBtn.gameObject, EnumButtonEvent.OnClick, OnenterBtn, 0, 0);
				decLable.text = _growthdata._des;
				sp.gameObject.SetActive(false);
			}
		}
		get
		{
			return _growthdata;
		}
		
	}

	void Start () {
		btnLab.text = LanguageManager.instance.GetValue ("Success_SuccessLQ");
	}

	private void OnenterBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(BagSystem.instance.BagIsFull())
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("bagfull"));
			return;
		}
		NetConnection.Instance.requestFundReward ((uint)GrowthReawData._Iv);
//		NetConnection.Instance.buyFund ((uint)GrowthReawData._Iv);
	}
}
