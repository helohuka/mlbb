using UnityEngine;
using System.Collections;

public class ItemsTips : UIBase {

	public UILabel _Zhoanglei;
	public UILabel _LevelLable;
	public UILabel _GetWayLable;



	public UIButton closeBtn;
	public UILabel namelLabel;
	public UILabel zhonglieLabel;
	public UILabel levelLabel;
	public UILabel tujingLabel;
	public UILabel descLabel;
	public UIGrid grid;
	public GameObject propCell;
	public UITexture icon;
	public 
	static COM_Item itemInset;

	public static int itemid;

	void Start () {

		_Zhoanglei.text = LanguageManager.instance.GetValue("ItemTips_Zhoanglei");
		_LevelLable.text = LanguageManager.instance.GetValue("ItemTips_Level");
		_GetWayLable.text = LanguageManager.instance.GetValue("ItemTips_GetWay");


		propCell.SetActive (false);
		ItemData idata = ItemData.GetData (itemid);
		namelLabel.text = idata.name_;
		zhonglieLabel.text = LanguageManager.instance.GetValue(idata.mainType_.ToString());
		levelLabel.text = idata.level_.ToString();
		descLabel.text = idata.desc_;
		tujingLabel.text = idata.acquiringWay_;
		HeadIconLoader.Instance.LoadIcon( idata.icon_,icon);
		UIManager.SetButtonEventHandler(closeBtn.gameObject, EnumButtonEvent.OnClick, OnClicClose,0, 0);

		for(int i =0;i<idata.propArr.Count;i++)
		{
			GameObject clone = GameObject.Instantiate(propCell)as GameObject;
			clone.SetActive(true);
			clone.transform.parent = grid.transform;
			clone.transform.position = Vector3.zero;
			clone.transform.localScale = Vector3.one;
			UILabel la = clone.GetComponentInChildren<UILabel>();
			la.text = LanguageManager.instance.GetValue(idata.propArr[i].Key.ToString())+" "  
				+idata.propArr[i].Value[0].ToString()+"-"+idata.propArr[i].Value[1].ToString() + " ";
			grid.repositionNow = true;
		}

	}
	void OnClicClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}

	public static void ShowMe(int iid)
	{
		itemid = iid;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_ItemPanel);
	}
	public static void SwithShowMe(int iid)
	{
		itemid = iid;
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_ItemPanel);
	}
	public override void Destroyobj ()
	{
		itemid = 0;
	}
}
