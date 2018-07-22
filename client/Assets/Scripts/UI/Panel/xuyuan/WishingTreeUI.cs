using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WishingTreeUI : UIBase {

	public UIButton _ExpBtn;
	public UIButton _MoneyBtn;
	public UIButton _BabyBtn;
	public UIButton _HuobanBtn;
	public UIButton _XuyuanBtn;
	public UIButton _TaoluBtn;
	public UIButton _CloseBtn;
	public UIInput _Input;
	public GameObject _DeTextObj;
	public GameObject _Item;
	public UIGrid _Grid;
	public UIButton CloseObjBtn;
	List<UIButton>btns = new List<UIButton> ();
	WishType _Wtype;
	void Start () {
		_Item.SetActive (false);
		btns.Add (_ExpBtn);
		btns.Add (_MoneyBtn);
		btns.Add (_BabyBtn);
		btns.Add (_HuobanBtn);
		UIManager.SetButtonEventHandler (_ExpBtn.gameObject, EnumButtonEvent.OnClick, OnClickExp,0, 0);
		UIManager.SetButtonEventHandler (_MoneyBtn.gameObject, EnumButtonEvent.OnClick, OnClickMoney,1, 0);
		UIManager.SetButtonEventHandler (_BabyBtn.gameObject, EnumButtonEvent.OnClick, OnClickBaby,2, 0);
		UIManager.SetButtonEventHandler (_HuobanBtn.gameObject, EnumButtonEvent.OnClick, OnClickHuoban,3, 0);
		UIManager.SetButtonEventHandler (_XuyuanBtn.gameObject, EnumButtonEvent.OnClick, OnClickXuyuan,0, 0);
		UIManager.SetButtonEventHandler (_TaoluBtn.gameObject, EnumButtonEvent.OnClick, OnClickTaolu,0, 0);
		UIManager.SetButtonEventHandler (_CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose,0, 0);
		_Wtype = WishType.WIT_Exp;
		SelectBtn(0);
	}
	void OnClickExp(ButtonScript obj, object args, int param1, int param2)
	{
		_Wtype = WishType.WIT_Exp;
		SelectBtn (param1);
	}
	void OnClickMoney(ButtonScript obj, object args, int param1, int param2)
	{
		_Wtype = WishType.WIT_Money;
		SelectBtn (param1);
	}
	void OnClickBaby(ButtonScript obj, object args, int param1, int param2)
	{
		_Wtype = WishType.WIT_Baby;
		SelectBtn (param1);
	}
	void OnClickHuoban(ButtonScript obj, object args, int param1, int param2)
	{
		_Wtype = WishType.WIT_Employee;
		SelectBtn (param1);
	}
	void OnClickXuyuan(ButtonScript obj, object args, int param1, int param2)
	{
		COM_Wishing cwish = new COM_Wishing ();
		cwish.wt_ = _Wtype;
		cwish.wish_ = _Input.value;
		NetConnection.Instance.sendwishing (cwish);
		Hide ();
	}
	void OnClickTaolu(ButtonScript obj, object args, int param1, int param2)
	{
		_DeTextObj.SetActive (true);
		UIManager.SetButtonEventHandler (CloseObjBtn.gameObject, EnumButtonEvent.OnClick, OnClickCloseObj,1, 0);
		AddItem ();
	}
	void OnClickCloseObj(ButtonScript obj, object args, int param1, int param2)
	{
		_DeTextObj.SetActive (false);
	}
	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	void OnClickBtn(ButtonScript obj, object args, int param1, int param2)
	{
		List<TemplteData> tdata =  TemplteData.GetData ();
		_Input.value = tdata[param1]._Text;
		_DeTextObj.SetActive (false);
	}
	void AddItem()
	{
		ClearObj ();
		List<TemplteData> tdata =  TemplteData.GetData ();
		for(int i =0;i<tdata.Count;i++)
		{
			GameObject go = GameObject.Instantiate(_Item) as GameObject;
			go.SetActive(true);
			go.transform.parent = _Grid.transform;
			go.transform.localScale = Vector3.one;
			DefText dtext = go.GetComponent<DefText>();
			dtext.Tdata = tdata[i];
			UIManager.SetButtonEventHandler (go, EnumButtonEvent.OnClick, OnClickBtn,i, 0);
			_Grid.repositionNow = true;
		}
	}
	void ClearObj()
	{
		foreach(Transform tr in _Grid.transform )
		{
			Destroy(tr.gameObject);
		}
	}

	void SelectBtn(int index)
	{
		for(int i =0;i<btns.Count;i++)
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
	// Update is called once per frame
	void Update () {
	
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_WishingTreePanel);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_WishingTreePanel);
	}
	public override void Destroyobj ()
	{

	}
}
