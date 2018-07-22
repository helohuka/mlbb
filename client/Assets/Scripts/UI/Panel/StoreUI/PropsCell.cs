using UnityEngine;
using System.Collections;

public class PropsCell : MonoBehaviour {
	public UILabel _ConsumeLable;
	public UISprite MarkSp;
	public UISprite TopMarkSp;
	public UITexture iconSp;
	public UISprite DiamondiconSp;
	public UILabel PropsNameLabel;
	public UILabel PropsNumLabel;
	public UILabel PropsTimeLabel;
	public UILabel PropsSMLabel;
	public UILabel PropsSLLabel;
	public UISprite backsp;
	private ShopData spData_;


	void Start()
	{
		_ConsumeLable.text = LanguageManager.instance.GetValue ("Store_Consume");
	}

	public ShopData SpData
	{
		set
		{
			if(value != null)
			{
				spData_ = value;
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
				//iconSp.spriteName = ItemData.GetData(spData_.Itemid_).icon_;
				HeadIconLoader.Instance.LoadIcon (ItemData.GetData(spData_._Itemid).icon_, iconSp);
				PropsNameLabel.text = spData_._Name;
				PropsNumLabel.text = spData_._Price.ToString();
				if(spData_._Purchase==0)
				{
					PropsSMLabel.text = "不限购";
				}else
				{
					PropsSMLabel.text ="可以购买"+ spData_._Purchase.ToString()+"次";
				}
				PropsSLLabel.text = spData_._Num.ToString();
				PropsTimeLabel.text = "";
				if(spData_._ShopPayType == ShopPayType.SPT_Diamond)
				{
					DiamondiconSp.spriteName = "xiaozuanshi";
				}else if(spData_._ShopPayType == ShopPayType.SPT_Gold)
				{
					DiamondiconSp.spriteName = "xiaojinbi";
				}
				else if(spData_._ShopPayType == ShopPayType.SPT_MagicCurrency)
				{
					DiamondiconSp.spriteName = "molibi";
				}
				backsp.spriteName =BagSystem.instance.GetQualityBack((int)ItemData.GetData(spData_._Itemid).quality_) ;
			}
		}
		get
		{
			return spData_;
		}
	}
}
