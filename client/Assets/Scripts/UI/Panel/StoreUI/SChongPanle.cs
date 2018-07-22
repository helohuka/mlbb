using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SChongPanle : MonoBehaviour {
	public GameObject labelObj;
	public GameObject Item;
	public UIGrid grid;
	public UIButton ChargeBtn;	
	public UIButton ReceiveBtn;

	List<ShopData>shopDatas = new List<ShopData>();
	List<int>keys = new List<int>();
	int Diamond = 0;

	void Start () 
	{
		GamePlayer.Instance.shouChongEnvet += new RequestEventHandler<bool> (OnShouChongEnvet);
		GamePlayer.Instance.getShouChongEnvet += new RequestEventHandler<bool> (OnGetShouChongEnvet);
		Item.SetActive (false);
		labelObj.gameObject.SetActive (false);
		foreach(int key in ShopData.metaData.Keys)
		{
			keys.Add(key);
		}
		for(int i=0;i<keys.Count;i++)
		{
			ShopData sdata = ShopData.GetData(keys[i]);
			if(sdata._ShopType == ShopType.SIT_FirstRecharge)
			{
				shopDatas.Add(sdata);
			}
			
		}
		GlobalInstanceFunction.Instance.Invoke (InitItem,0.1f);
	
		if(GamePlayer.Instance.isShouchong && !GamePlayer.Instance.isGetShouchong)
		{
			ChargeBtn.gameObject.SetActive(false);
			ReceiveBtn.gameObject.SetActive(true);
		}

		if(GamePlayer.Instance.isShouchong && GamePlayer.Instance.isGetShouchong)
		{
			labelObj.gameObject.SetActive(true);
			ChargeBtn.gameObject.SetActive(false);
			ReceiveBtn.gameObject.SetActive(false);
		}
	
		UIManager.SetButtonEventHandler (ChargeBtn.gameObject, EnumButtonEvent.OnClick, OnClickCharge, 0, 0);
		UIManager.SetButtonEventHandler (ReceiveBtn.gameObject, EnumButtonEvent.OnClick, OnClickReceive, 0, 0);
	}
	void InitItem()
	{
		AddItems (shopDatas);
	}
	void OnClickCharge(ButtonScript obj, object args, int param1, int param2)
	{
        if (StoreUI.Instance != null)
            StoreUI.Instance.SwitchTab(1);
		//PopText.Instance.Show (LanguageManager.instance.GetValue("zanweikaiqi"));
	}
	void OnClickReceive(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.getFirstRechargeItem ();
		obj.gameObject.SetActive (false);
		labelObj.gameObject.SetActive (true);
	}




	void AddItems(List<ShopData> Datas)
	{
		for(int i = 0;i<Datas.Count;i++)
		{
			GameObject o = GameObject.Instantiate(Item)as GameObject;
			o.SetActive(true);
			ShouChongCell sCell = o.GetComponent<ShouChongCell>();
			sCell.SpData = Datas[i];
			o.transform.parent = grid.transform;
			o.transform.localScale = Vector3.one;
			grid.Reposition();
		}
	}
	void OnClickDown(ButtonScript obj, object args, int param1, int param2)
	{
		StoreUI.Instance.itemInfoTips.gameObject.SetActive (true);
		StoreUI.Instance.itemInfoTips.Item = ItemData.GetData(param1);
	}
	
	void OnClickUp (ButtonScript obj, object args, int param1, int param2)
	{
		StoreUI.Instance.itemInfoTips.gameObject.SetActive (false);
	}

	void OnShouChongEnvet(bool b)
	{
		if(GamePlayer.Instance.isShouchong && !GamePlayer.Instance.isGetShouchong)
		{
			ChargeBtn.gameObject.SetActive(false);
			ReceiveBtn.gameObject.SetActive(true);
		}
		
		if(GamePlayer.Instance.isShouchong && GamePlayer.Instance.isGetShouchong)
		{
			labelObj.gameObject.SetActive(true);
			ChargeBtn.gameObject.SetActive(false);
			ReceiveBtn.gameObject.SetActive(false);
		}
	}

	void OnGetShouChongEnvet(bool b)
	{
		if(GamePlayer.Instance.isShouchong && !GamePlayer.Instance.isGetShouchong)
		{
			ChargeBtn.gameObject.SetActive(false);
			ReceiveBtn.gameObject.SetActive(true);
		}
		
		if(GamePlayer.Instance.isShouchong && GamePlayer.Instance.isGetShouchong)
		{
			labelObj.gameObject.SetActive(true);
			ChargeBtn.gameObject.SetActive(false);
			ReceiveBtn.gameObject.SetActive(false);
		}
	}

	void OnDestroy()
	{
		GamePlayer.Instance.shouChongEnvet -= OnShouChongEnvet;
		GamePlayer.Instance.getShouChongEnvet -= OnGetShouChongEnvet;
	}
}
