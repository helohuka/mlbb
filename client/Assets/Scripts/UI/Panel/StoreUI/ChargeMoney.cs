using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ChargeMoney : MonoBehaviour {


	public GameObject Item;
	public UIGrid grid;
	List<ShopData>shopDatas = new List<ShopData>();
	List<int>keys = new List<int>();

	void Start () {
		int index =0;
		Item.SetActive (false);
        Dictionary<int, ShopData> metaData = ShopData.metaData;
        if (metaData == null)
            return;

        foreach(ShopData sData in metaData.Values)
        {
            if (sData._ShopType == ShopType.SIT_Recharge)
            {
                GameObject o = GameObject.Instantiate(Item) as GameObject;
                o.SetActive(true);
                ChargeCell cCell = o.GetComponent<ChargeCell>();
                cCell.SpData = sData;
				cCell.fanliLable.text = ShopData.fanlis[index];
                o.transform.parent = grid.transform;
                o.transform.position = Vector3.zero;
                o.transform.localScale = Vector3.one;
				index++;
                UIManager.SetButtonEventHandler(o, EnumButtonEvent.TouchDown, OnClickPay, sData._Id, 0);
            }
        }
        grid.Reposition();
	}
	
	void OnClickPay(ButtonScript obj, object args, int param1, int param2)
	{
		int pId = param1;
		ClientLog.Instance.Log(" shot id is : " + pId);
        SDK185.Pay(pId);
        //gameHandler.PayProduct(pId);
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
