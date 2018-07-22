using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FixUI : UIBase
{
	public UIButton closeBtn;
	public UIButton fixBtn;
	public UIButton perfectFixBtn;
	public UILabel mNowNum;
	public UILabel mNowMax;
	public UILabel mMaxNow;
	public UILabel mMaxNum;
	public UILabel needMoney;
	public UILabel dNowNum;
	public UILabel dNowMax;
	public UILabel dMaxNow;
	public UILabel dMaxNum;
	public UILabel needDiamond;
	public UISprite icon;
	public UILabel itemName;
	public static int itemInstId;
	private int _needMoney;
	private int _needDiamond;

	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		UIManager.SetButtonEventHandler (fixBtn.gameObject, EnumButtonEvent.OnClick, OnFix, 0, 0);
		UIManager.SetButtonEventHandler (perfectFixBtn.gameObject, EnumButtonEvent.OnClick, OnParfectFix, 0, 0);
		GamePlayer.Instance.fixItmeEnvet += new RequestEventHandler<COM_Item> (OnFixOk);



		COM_Item[] equipList = GamePlayer.Instance.Equips;
		for(int i=0;i<equipList.Length;i++)
		{
			if(equipList[i] == null)
				continue;
			if(equipList[i].instId_ == itemInstId)
			{
				SetItemData(equipList[i]);
				break;
			}
		}
	}
	
	private void SetItemData(COM_Item item)
	{
		ItemCellUI cell = UIManager.Instance.AddItemCellUI (icon, item.itemId_);
		itemName.text = ItemData.GetData ((int)item.itemId_).name_;
		mNowNum.text = item.durability_.ToString ();
		int nowMax = item.durabilityMax_ - 10;
		nowMax = nowMax > 60 ? nowMax : 60;
		mNowMax.text = (nowMax).ToString ();
		mMaxNow.text = item.durabilityMax_.ToString ();
		mMaxNum.text = (nowMax).ToString ();

		dNowNum.text = item.durability_.ToString ();
		int bnowMax = item.durabilityMax_;// + 10;
		dNowMax.text = bnowMax.ToString ();
		//bnowMax = bnowMax > 300 ? 300 : bnowMax;
		dMaxNow.text = item.durabilityMax_.ToString ();
		dMaxNum.text = (bnowMax).ToString ();

		_needMoney = (nowMax - item.durability_) * 50;
		_needDiamond = (bnowMax - item.durability_) * 1;
		needMoney.text = _needMoney.ToString ();
		needDiamond.text = _needDiamond.ToString ();

	}

	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_FixItemPanel);
	}
	
	public static void ShowMe(int id)
	{
		itemInstId = id;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_FixItemPanel);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_FixItemPanel);
	}
	
	#endregion

	public override void Destroyobj ()
	{
		GameObject.Destroy (gameObject);
	}

	private void OnClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();	
	}

	private void OnFix(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Money) < _needMoney)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("nomoney"));
			return;
		}
		NetConnection.Instance.fixItem (itemInstId, FixType.FT_Money);
	}

	private void OnParfectFix(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Diamond) < _needDiamond)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("nodiamond"));
			return;
		}

		NetConnection.Instance.fixItem (itemInstId, FixType.FT_Diamond);
	}


	void OnFixOk(COM_Item item)
	{
		PopText.Instance.Show ( LanguageManager.instance.GetValue("fixOK") );
		SetItemData (item);
	}

	protected override void DoHide ()
	{
		GamePlayer.Instance.fixItmeEnvet -= OnFixOk;
		base.DoHide ();
	}

}

