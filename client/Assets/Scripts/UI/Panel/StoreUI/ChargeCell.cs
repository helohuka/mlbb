using UnityEngine;
using System.Collections;

public class ChargeCell : MonoBehaviour {

	public UISprite MarkSp;
	public UISprite TopMarkSp;
	public UITexture iconSp;
	public UISprite DiamondiconSp;
	public UILabel DiamondNameLabel;
	public UILabel DiamondPriceLabel;
	public UILabel DiamondSMLabel;
	public UILabel DiamondFrequencyLabel;
	public UILabel fanliLable;
	private ShopData spData_;
	
	public ShopData SpData
	{
		set
		{
			if(value != null)
			{
				spData_ = value;
				//iconSp.spriteName = spData_.icon_;
				HeadIconLoader.Instance.LoadIcon (spData_._Icon, iconSp);
				if(spData_._Hot==1)
				{
					TopMarkSp.gameObject.SetActive(true);
				}
				else
				{
					TopMarkSp.gameObject.SetActive(false);
				}
				if(spData_._Recommend == 1)
				{
					MarkSp.gameObject.SetActive(true);
				}else
				{
					MarkSp.gameObject.SetActive(false);
				}
			//	iconSp.spriteName = ItemData.GetData(spData_.Itemid_).icon_;
				DiamondNameLabel.text = spData_._Name;
				DiamondPriceLabel.text = spData_._Price.ToString()+LanguageManager.instance.GetValue("rmbbz");
				if(spData_._Purchase==0)
				{
					DiamondFrequencyLabel.text = "";
				}else
				{
					DiamondFrequencyLabel.text = spData_._Purchase.ToString();
				}

				DiamondSMLabel.text = "";
				DiamondiconSp.gameObject.SetActive(false);
			}
		}
		get
		{
			return spData_;
		}
	}
}
