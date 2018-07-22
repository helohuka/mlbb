using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class moreJiFenShopUI : MonoBehaviour
{
	public MoreDiscountStoreCellUI cell;
	public UIGrid grid;
	public GameObject Tips;
	public UIButton buyBtn;   
	public UISprite buyIcon;
	public UIButton getBtn;
	public UILabel haveNumLab; 
	public UILabel timeLab;
	private List<GameObject> CellList = new List<GameObject>();
	void Start () 
	{
		MoreActivityData.instance.IntegralEvent += new RequestEventHandler<COM_IntegralData> (OnUpdataEvent);
		UIManager.SetButtonEventHandler (buyBtn.gameObject, EnumButtonEvent.OnClick, OnBuyBtn, 0, 0);
		UIManager.SetButtonEventHandler (getBtn.gameObject, EnumButtonEvent.OnClick, OnFreeGetBtn, 0, 0);
		UpdateInfo ();
	}
	
	void  OnUpdataEvent(COM_IntegralData data)
	{
		UpdateInfo ();  
	}      
	
	
	void UpdateInfo ()
	{
		COM_IntegralData data = MoreActivityData.instance.GetIntegralData ();
		if (data == null)
			return; 
		string sfmt = "yyyy/MM/dd";
		string efmt = "yyyy/MM/dd";
		Define.FormatUnixTimestamp(ref sfmt, (int)data.sinceStamp_);
		Define.FormatUnixTimestamp(ref efmt, (int)data.endStamp_);
		
		timeLab.text = LanguageManager.instance.GetValue("leijichongzhitime").Replace("{n}", sfmt +" - "+  efmt);
		getBtn.isEnabled = !data.isflag_;
		haveNumLab.text = data.integral_.ToString ();
		for (int i = 0; i < CellList.Count; ++i )
		{
			grid.RemoveChild(CellList[i].transform);
			CellList[i].transform.parent = null;
			CellList[i].gameObject.SetActive(false);
		}
		CellList.Clear ();
		for(int i =0;i<data.contents_.Length;i++)
		{
			GameObject objCell = Object.Instantiate(cell.gameObject) as GameObject;
			MoreDiscountStoreCellUI cellUI = objCell.GetComponent<MoreDiscountStoreCellUI>();
			cellUI.integralData = data.contents_[i];
			ItemCellUI cellItem = UIManager.Instance.AddItemCellUI(cellUI.item,data.contents_[i].itemid_);
			cellItem.showTips = true;
			cellUI.nameLab.text = ItemData.GetData((int)data.contents_[i].itemid_).name_;
			cellUI.needMoney.text = (data.contents_[i].cost_).ToString();
			cellUI.buyNumLab.text = LanguageManager.instance.GetValue("leijikemainum").Replace("{n}",data.contents_[i].times_.ToString());
			//cellUI.data = data.contents_[i];
			UIManager.SetButtonEventHandler (cellUI.gameObject, EnumButtonEvent.OnClick, OnGetBtn, (int)data.contents_[i].itemid_, data.contents_[i].cost_);
			objCell.transform.parent = grid.transform;
			objCell.SetActive(true);
			objCell.transform.localScale = Vector3.one;
			CellList.Add(objCell);
		}
		grid.Reposition ();
	}
	
	private void OnGetBtn(ButtonScript obj, object args, int param1, int param2)
	{
		COM_IntegralData data = MoreActivityData.instance.GetIntegralData ();
		if (data == null)
			return;
		for(int i =0;i<data.contents_.Length;i++)
		{
			if(data.contents_[i].itemid_ == param1)
			{
				if(data.contents_[i].times_ == 0)
				{
					PopText.Instance.Show(LanguageManager.instance.GetValue("EN_DisShopLimitLess"));
					return;
				}
			}
		}


		Tips.SetActive (true);
		UIManager.Instance.AdjustUIDepth(Tips.transform, false);
		StoreTips stips = Tips.GetComponent<StoreTips>();
		stips.nameLabel.text = ItemData.GetData (param1).name_;
		stips.count = 1;
		stips.buyNumLab.text = "1";
		MoreDiscountStoreCellUI cellUI = obj.gameObject.GetComponent<MoreDiscountStoreCellUI>();
		Tips.name =cellUI.integralData.id_.ToString ();
		stips.maxCount = param2;
		ItemCellUI cellui = UIManager.Instance.AddItemCellUI (buyIcon, (uint)param1);
		cellui.showTips = true;

		stips.jiageLabel.text = param2.ToString ();// ((int)((float)cellUI.data.price_*cellUI.data.discount_)).ToString();
		stips.DesLabel.text = ":"+ItemData.GetData (param1).desc_;
	}
	
	private void OnBuyBtn(ButtonScript obj, object args, int param1, int param2)
	{
		StoreTips stips = Tips.GetComponent<StoreTips>();
		int count = stips.count;

		if(MoreActivityData.instance.GetIntegralData().integral_ <  (count * int.Parse(stips.jiageLabel.text)))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("jifenbuzu"));
			return;
		}

		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue ("shopbuyitem").Replace ("{n}", count .ToString ()).Replace ("{n1}", stips.nameLabel.text).Replace ("{n2}", (count * int.Parse(stips.jiageLabel.text)).ToString ()
		                                                                                                                                                           + LanguageManager.instance.GetValue ("jifen")), () => {
			
			NetConnection.Instance.buyIntegralItem (uint.Parse (Tips.name), (uint)count);});
	}

	private void OnFreeGetBtn(ButtonScript obj, object args, int param1, int param2) 
	{ 
		NetConnection.Instance.requestEverydayIntegral ();// (uint.Parse (Tips.name), (uint)count);
	}

	void OnDestroy()
	{
		MoreActivityData.instance.IntegralEvent -= OnUpdataEvent;
	}
}


