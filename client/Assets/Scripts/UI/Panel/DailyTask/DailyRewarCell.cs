using UnityEngine;
using System.Collections;

public class DailyRewarCell : MonoBehaviour {

	public UIButton PurchaseBtn;
	public UITexture IconSp;
	public UILabel itemName;
	public UILabel PriceLabel;
	public UISprite back;
	private ACT_RewardData  _adata;
	
	public ACT_RewardData Adata
	{
		set
		{
			if(value != null)
			{
				_adata = value;
				//IconSp.spriteName = ItemData.GetData(_adata.ItemID_).icon_;
				PriceLabel.text = _adata._Price.ToString();
				itemName.text = ItemData.GetData(_adata._ItemID).name_;
				HeadIconLoader.Instance.LoadIcon (ItemData.GetData(_adata._ItemID).icon_, IconSp);
				ItemData idata = ItemData.GetData(_adata._ItemID);
				back.spriteName = 	back.spriteName = BagSystem.instance.GetQualityBack((int)idata.quality_);

			}
		}
		get
		{
			return _adata;
		}
	}
	void Start () {
		UIManager.SetButtonEventHandler (PurchaseBtn.gameObject, EnumButtonEvent.OnClick, OnClickPurchase,0, 0);
	}
	void OnClickPurchase(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.ActivityTable.reward_<Adata._Price)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("huoyue"));
		}else
		{
			NetConnection.Instance.requestActivityReward (Adata._ItemID);
		}


	}

}
