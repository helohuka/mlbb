using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelRewardShopUI : UIBase
{
	public UIButton closeBtn;
	public UIButton buyBtn;
	public List<UISprite> rewardIconList = new List<UISprite>();
	public UILabel oldMoney;
	public UILabel nowMoney;
	public UILabel timeLab;
	public UISprite levelImg;
	public UITexture back;
	
	void Start ()
	{
		HeadIconLoader.Instance.LoadIcon("dengjilibao", back);
		UIManager.SetButtonEventHandler (buyBtn.gameObject, EnumButtonEvent.OnClick, OnBuyBtn, 0, 0);
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		GamePlayer.Instance.levelRewadEnvet += new RequestEventHandler<int> (OnLecelRewardShopEnvet);
	
		UpdateInfo ();
	}

	private void UpdateInfo()
	{
		List<COM_CourseGift> spData = GamePlayer.Instance.levelShopList; 
		COM_CourseGift data = null;
		for(int i=0;i<spData.Count;i++)
		{
			if(spData[i].timeout_ >0)
			{
				data = spData[i];
				break;
			}
		}

		if(data!= null)
		{
			CourseGiftData giftData = CourseGiftData.GetData ((int)data.id_);
			oldMoney.text = giftData.oldPrice_.ToString ();
			nowMoney.text = giftData.price_.ToString ();
			buyBtn.isEnabled = true;
			levelImg.spriteName = "zi"+giftData.level_;
			for(int j=0;j<rewardIconList.Count;j++)
			{
				rewardIconList[j].gameObject.SetActive(false);
			}
			for(int i =0;i<giftData.itemIds_.Length && i<4;i++)
			{
				string[] str = giftData.itemIds_[i].Split(':');
				rewardIconList[i].gameObject.SetActive(true);
				ItemCellUI cell = UIManager.Instance.AddItemCellUI(rewardIconList[i],uint.Parse(str[0]));
				cell.ItemCount = int.Parse(str[1]);
				cell.showTips  = true;
			}
			UIManager.SetButtonEventHandler (buyBtn.gameObject, EnumButtonEvent.OnClick, OnBuyBtn, giftData.shopId_, 0);
		}
		
	}

	
	void Update ()
	{
		
	}
	
	#region Fixed methods for UIBase derived cass
	 
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_levelRewardShop);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_levelRewardShop);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_levelRewardShop);
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
		if (param1 == 0)
			return;
		//gameHandler.PayProduct(param1);
	}
	
	void OnLecelRewardShopEnvet(int num)
	{
		Hide ();
		UpdateInfo ();
	}
	
	void OnDestroy()
	{
		HeadIconLoader.Instance.Delete("dengjilibao");
		GamePlayer.Instance.levelRewadEnvet -= OnLecelRewardShopEnvet;
	}
}


