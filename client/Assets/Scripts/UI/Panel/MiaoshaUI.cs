using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiaoshaUI : UIBase
{
	public UIButton closeBtn;
	public UIButton buyBtn;
	public List<UISprite> rewardIconList = new List<UISprite>();
	public UILabel oldMoney;
	public UILabel nowMoney;
	public UILabel timeLab;
	public UITexture back;
 	
	void Start ()
	{
		HeadIconLoader.Instance.LoadIcon("miaosha", back);
		UIManager.SetButtonEventHandler (buyBtn.gameObject, EnumButtonEvent.OnClick, OnBuyBtn, 0, 0);
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		GamePlayer.Instance.MiaoshaEnvet += new RequestEventHandler<COM_ADGiftBag> (OnMiaoshaEnvet);
		COM_ADGiftBag msData = GamePlayer.Instance.miaoshaData_; 
		if(msData.isflag_)
		{
			buyBtn.isEnabled = false;
		}
		else
		{
			buyBtn.isEnabled = true;
		}

		for(int i =0;i<msData.itemdata_.Length && i<4;i++)
		{
			ItemCellUI cell = UIManager.Instance.AddItemCellUI(rewardIconList[i],msData.itemdata_[i].itemId_);
			cell.ItemCount = (int)msData.itemdata_[i].itemNum_;
			cell.showTips  = true;
		}
		oldMoney.text = msData.oldprice_.ToString ();
		nowMoney.text = msData.price_.ToString ();

	}

	void Update ()
	{

	}

	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_MiaoSha);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_MiaoSha);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_MiaoSha);
	}

	public override void Destroyobj ()
	{

	}
	#endregion

	private void OnClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}

	private void OnBuyBtn(ButtonScript obj, object args, int param1, int param2)
	{
		COM_ADGiftBag msData = GamePlayer.Instance.miaoshaData_; 
		if (msData == null)
			return;
		if (msData.isflag_)
			return;
		//int smallChangeId = 0;
		//if(msData.price_ == 1)
		//{
		//	GlobalValue.Get(Constant.C_SmallChange1ShopID, out smallChangeId);
  //          SDK185.Pay(smallChangeId);
  //          //gameHandler.PayProduct(smallChangeId);

  //      }
		//else if(msData.price_ == 3)
		//{
		//	GlobalValue.Get(Constant.C_SmallChange3ShopID, out smallChangeId);
  //          SDK185.Pay(smallChangeId);
  //          //gameHandler.PayProduct(smallChangeId);
  //      }

        SDK185.Pay((int)Constant.C_SmallChange1ShopID, msData.price_, "ÃëÉ±");
	}

	void OnMiaoshaEnvet(COM_ADGiftBag adg)
	{
		if(adg.isflag_)
		{
			buyBtn.isEnabled = false;
		}
	}

	void OnDestroy()
	{
		HeadIconLoader.Instance.Delete("miaosha");
		GamePlayer.Instance.MiaoshaEnvet -= OnMiaoshaEnvet;
	}
}

