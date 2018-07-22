using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Props : MonoBehaviour {

	public UIScrollBar sb;
	public UIButton zahuoBtn;
	public UIButton chongwuBtn;
	public UIButton cailiaoBtn;
	public UIButton shizhuangBtn;
	public UIButton zuanshiBtn;
	public GameObject Item;
	public UIGrid grid;
	List<ShopData>shopDatas = new List<ShopData>();
	List<UIButton>btns = new List<UIButton>();
	private List<GameObject> CellPool = new List<GameObject>();
	List<int>keys = new List<int>();
	void Start () {
		Item.SetActive (false);
		btns.Add (zahuoBtn);
		btns.Add (chongwuBtn);
		btns.Add (cailiaoBtn);
		btns.Add (shizhuangBtn);
		btns.Add (zuanshiBtn);
		for(int i=0;i< btns.Count;i++)
		{
			UIManager.SetButtonEventHandler (btns[i].gameObject, EnumButtonEvent.OnClick, OnClickbtn, i, 0);
		}
		ButtonSelect (0);
		foreach(ShopData sdata in ShopData.metaData.Values)
		{
			if(sdata._ShopType == ShopType.SIT_Shop)
			{
				shopDatas.Add(sdata);
			}
		}
		AddItems (ShowAchieve(0));
	}
	void AddItems(List<ShopData> Datas)
	{
		for(int i =0;i<CellPool.Count;i++)
		{

			PropsCell bCell = CellPool[i].GetComponent<PropsCell>();
			bCell.SpData = null;
			grid.transform.DetachChildren();
			CellPool[i].SetActive(false);
		}

		for(int i = 0;i<Datas.Count;i++)
		{
			if(i<CellPool.Count)
			{
				CellPool[i].transform.parent = grid.transform;
				CellPool[i].gameObject.SetActive(true);
				PropsCell bCell = CellPool[i].GetComponent<PropsCell>();
				bCell.SpData = Datas[i];
				UIManager.SetButtonEventHandler (CellPool[i].gameObject, EnumButtonEvent.OnClick, OnClickShop, 0, 0);
			}
			else
			{
				GameObject o = GameObject.Instantiate(Item)as GameObject;
				o.SetActive(true);
				PropsCell  pCell = o.GetComponent<PropsCell>();
				pCell.SpData = Datas[i];
				o.transform.parent = grid.transform;
				o.transform.position = Vector3.zero;
				o.transform.localScale = Vector3.one;
				UIManager.SetButtonEventHandler (o, EnumButtonEvent.OnClick, OnClickShop, 0, 0);
				CellPool.Add(o);

			}
			grid.Reposition();
			sb.value = 1;


//			UIManager.SetButtonEventHandler (pCell.iconSp.gameObject, EnumButtonEvent.TouchDown, OnClickDown, Datas[i].Itemid_, 0);
//			UIManager.SetButtonEventHandler (pCell.iconSp.gameObject, EnumButtonEvent.TouchUp, OnClickUp, Datas[i].Itemid_, 0);
		}
	
	}

	void ClearItem()
	{
		if(grid == null)return;
		foreach(Transform tr in grid.transform)
		{
			Destroy(tr.gameObject);
		}
	}

	void ButtonSelect(int index)
	{
		for(int i=0;i< btns.Count;i++)
		{
			if(index == i)
			{
				btns[i].isEnabled = false;
			}else
			{
				btns[i].isEnabled = true;
			}
		}
	}
	List<ShopData> ShowAchieve(int index)
	{
		List<ShopData>growingUp = new List<ShopData> ();
		

		for(int i =0;i<shopDatas.Count ;i++)
			{
				if(index ==0)
				{
				if(shopDatas[i]._classifytype == ClassifyType.SD_Debris)
					{
						
					growingUp.Add(shopDatas[i]);
					}
					
				}else
					if(index ==1)
				{
				if(shopDatas[i]._classifytype == ClassifyType.SD_Pet)
					{
						
					growingUp.Add(shopDatas[i]);
					}
					
				}else
					if(index ==2)
				{
				    if(shopDatas[i]._classifytype == ClassifyType.SD_Data)
					{
						
					   growingUp.Add(shopDatas[i]);
					}
					
				}
			else
				if(index ==3)
			{
				if(shopDatas[i]._classifytype == ClassifyType.SD_Fashion)
				{
					
					growingUp.Add(shopDatas[i]);
				}
				
			}else
				if(index ==4)
			{
				if(shopDatas[i]._classifytype == ClassifyType.SD_Diamond)
				{
					
					growingUp.Add(shopDatas[i]);
				}
				
			}

		}


		return Sortgrowing (growingUp);
	}
	List<ShopData> Sortgrowing(List<ShopData> growingUps)
	{
		List<ShopData>growingUp = new List<ShopData> ();
		for(int i=0;i<growingUps.Count;i++)
		{
			if(growingUps[i]._ShopPayType == ShopPayType.SPT_Gold)
			{
				growingUp.Insert(0,growingUps[i]);
			}else
			{
				growingUp.Add(growingUps[i]);
			}
		}
		return growingUp;
	}
	void OnClickbtn(ButtonScript obj, object args, int param1, int param2)
	{
		//ClearItem ();;
		ButtonSelect (param1);
		AddItems (ShowAchieve(param1));
	}


	void OnClickShop(ButtonScript obj, object args, int param1, int param2)
	{
		StoreUI.Instance.Tips.SetActive (true);
		if(GamePlayer.Instance.isInBattle)
		{
			StoreUI.Instance.Tips.GetComponent<StoreTips>().DetermineBtn.gameObject.SetActive(false);
		}
		else
		{
			StoreUI.Instance.Tips.GetComponent<StoreTips>().DetermineBtn.gameObject.SetActive(true);
		}
		StoreTips stips = StoreUI.Instance.Tips.GetComponent<StoreTips>();
		PropsCell cCell = obj.GetComponent<PropsCell>();
		stips.SpData = cCell.SpData;
		
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
}
