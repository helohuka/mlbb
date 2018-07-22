using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FixAllUI : UIBase
{
	public UIButton closeBtn;
	public UIButton fixBtn;
	public UIButton parfectFixBtn;
	public UIGrid grid;
	public GameObject itemCell;
	public UILabel needMoneyLab;
	public UILabel needDiamondLab;
	public UILabel noFixLab;
	private int _needMoney;
	private int _needDiamond;
	private List<COM_Item> durEquips = new List<COM_Item>();

	private List<GameObject> listCell = new List<GameObject>();

	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		UIManager.SetButtonEventHandler (fixBtn.gameObject, EnumButtonEvent.OnClick, OnFix, 0, 0);
		UIManager.SetButtonEventHandler (parfectFixBtn.gameObject, EnumButtonEvent.OnClick, OnParfectFix, 0, 0);

		SetDurEquips ();
	}


	private void SetDurEquips()
	{
		COM_Item[] equips = GamePlayer.Instance.Equips;
		for(int i=0;i<equips.Length;i++)
		{
			if(equips[i] != null)
			{
				ItemData idate = ItemData.GetData((int)equips[i].itemId_);
				if(idate == null)
					continue;
				if(idate.slot_ == EquipmentSlot.ES_Ornament_0 ||idate.slot_ == EquipmentSlot.ES_Ornament_1||idate.slot_ == EquipmentSlot.ES_Crystal)
					continue;
				if(equips[i].durability_ < equips[i].durabilityMax_*0.8)
				{
					durEquips.Add(equips[i]);
					GameObject obj = Object.Instantiate(itemCell) as GameObject;
					obj.SetActive(true);
					obj.GetComponent<BagCellUI>().Item = equips[i];
					obj.GetComponent<BagCellUI>().countLab.text = ItemData.GetData((int)equips[i].itemId_).name_;
					grid.AddChild(obj.transform);
					obj.transform.localScale = Vector3.one;
					listCell.Add(obj);
				}
			}
		}
		grid.Reposition ();
	
		if(durEquips.Count <= 0)
		{
			fixBtn.isEnabled = false;
			parfectFixBtn.isEnabled = false;
			return;
		}
		else
		{
			noFixLab.gameObject.SetActive(false); 
		}

		for(int j =0;j<durEquips.Count;j++)
		{
			int nowMax = durEquips[j].durabilityMax_ - 10;
			nowMax = nowMax > 60 ? nowMax : 60;
			_needMoney += (nowMax- durEquips[j].durability_ )* 50;

			int bnowMax = durEquips[j].durabilityMax_ ;//+ 10;
		//	bnowMax = bnowMax > 300 ? 300 : bnowMax;
			_needDiamond += (bnowMax- durEquips[j].durability_ )* 1;
		}

		needMoneyLab.text = _needMoney.ToString ();
		needDiamondLab.text = _needDiamond.ToString ();
	}

	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_FixAllPanel);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_FixAllPanel);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_FixAllPanel);
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
		if(durEquips.Count<=0)
		{
			return;
		}
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Money) < _needMoney)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("nomoney"));
			return;
		}
		uint[] item = new uint[durEquips.Count];
		for(int i=0;i<durEquips.Count;i++)
		{
			item[i] = durEquips[i].instId_;
		}
		NetConnection.Instance.fixAllItem (item, FixType.FT_Money);

		DelCell ();
	}

	private void OnParfectFix(ButtonScript obj, object args, int param1, int param2)
	{
		if(durEquips.Count<=0)
		{
			return;
		}
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Diamond) < _needDiamond)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("nodiamond"));
			return;
		}
		uint[] item = new uint[durEquips.Count];
		for(int i=0;i<durEquips.Count;i++)
		{
			item[i] = durEquips[i].instId_;
		}
		NetConnection.Instance.fixAllItem (item, FixType.FT_Diamond);
			
		DelCell ();
	}


	private void DelCell()
	{
		for(int i=0;i< listCell.Count;i++)
		{
			listCell[i].transform.parent  =null;
			Destroy(listCell[i]);
			listCell[i] = null;
		}
		listCell.Clear ();

		fixBtn.isEnabled = false;
		parfectFixBtn.isEnabled = false;
		needMoneyLab.text = "0";
		needDiamondLab.text = "0";
	}



}

