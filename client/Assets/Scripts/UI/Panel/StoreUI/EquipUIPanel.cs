using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class EquipUIPanel : UIBase {

	public UIButton closeBtn;
	public UIButton oneBtn;
	public UIButton twoBtn;
	public GameObject item;
	public UIGrid grid;
	private List<ShopData>EquipData;
	private List<GameObject> CellPool = new List<GameObject>();
	private List<UIButton>btns = new List<UIButton>();
	int count;
	void Start () {
		item.SetActive (false);
		EquipData = new List<ShopData> ();
		InitData ();
		btns.Add (oneBtn);
		btns.Add (twoBtn);
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickclose,0, 0);
		UIManager.SetButtonEventHandler (oneBtn.gameObject, EnumButtonEvent.OnClick, OnClickone,(int)ClassifyType.SD_1Ji, 0);
		UIManager.SetButtonEventHandler (twoBtn.gameObject, EnumButtonEvent.OnClick, OnClickctwo,(int)ClassifyType.SD_2Ji, 1);
		Additem ((int)ClassifyType.SD_1Ji);
		slectBtn (0);
	}
	void OnClickclose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	void OnClickone(ButtonScript obj, object args, int param1, int param2)
	{
		slectBtn (param2);
		Additem (param1);
	}
	void OnClickctwo(ButtonScript obj, object args, int param1, int param2)
	{
		slectBtn (param2);
		Additem (param1);
	}
	void OnClickShop(ButtonScript obj, object args, int param1, int param2)
	{
		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("querengoumai"), () => {
			NetConnection.Instance.shopBuyItem(param1,1);
				});
	}
	void Additem(int ictype)
	{
		ClassifyType ctype = (ClassifyType)Enum.Parse (typeof(ClassifyType), ictype.ToString());
		count = 0;
		for(int i =0;i<CellPool.Count;i++)
		{
			
			EquipitemCell bCell = CellPool[i].GetComponent<EquipitemCell>();
			bCell.EquipshopData = null;
			grid.transform.DetachChildren();
			CellPool[i].SetActive(false);
		}
		for(int i =0;i< EquipData.Count;i++)
		{
			if(EquipData[i]._classifytype == ctype)
			{

				if(count<CellPool.Count)
				{
					CellPool[count].transform.parent = grid.transform;
					CellPool[count].gameObject.SetActive(true);
					EquipitemCell bCell = CellPool[count].GetComponent<EquipitemCell>();
					bCell.EquipshopData = EquipData[i];
					UIManager.SetButtonEventHandler (CellPool[count].gameObject, EnumButtonEvent.OnClick, OnClickShop, EquipData[i]._Id, 0);
				}
				else
				{
					GameObject o = GameObject.Instantiate(item)as GameObject;
					o.SetActive(true);
					EquipitemCell  pCell = o.GetComponent<EquipitemCell>();
					pCell.EquipshopData = EquipData[i];
					o.transform.parent = grid.transform;
					o.transform.position = Vector3.zero;
					o.transform.localScale = Vector3.one;
					UIManager.SetButtonEventHandler (o, EnumButtonEvent.OnClick, OnClickShop, EquipData[i]._Id, 0);
					CellPool.Add(o);
					
				}
				count++;
			}
		}			
	}
	void InitData()
	{
		foreach(ShopData sd in ShopData.metaData.Values)
		{
			if(sd._ShopType == ShopType.SIT_Equip)
			{
				EquipData.Add(sd);
			}

		}
	}
	void slectBtn(int index)
	{
		for(int i =0;i<btns.Count;i++)
		{
			if(i==index)
			{
				btns[i].isEnabled = false;
			}else
			{
				btns[i].isEnabled = true;
			}
		}
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_EquipUIPanel);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_EquipUIPanel);
	}
	// Update is called once per frame
	void Update () {
	
	}
	public override void Destroyobj ()
	{

	}
}
