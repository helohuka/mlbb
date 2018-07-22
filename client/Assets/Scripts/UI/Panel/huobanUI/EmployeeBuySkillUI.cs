using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmployeeBuySkillUI : UIBase
{
	public UIGrid grid;
	public GameObject cell;
	public UILabel moneyLab;
	public UIButton closeBtn;

	private List<string> _icons = new List<string>();
	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		GamePlayer.Instance.OnIPropUpdate += UpdateMoney;
		OpenPanelAnimator.PlayOpenAnimation(this.panel, ()=>{
			UpdateBuyList();
		});

	}




	#region Fixed methods for UIBase derived cass
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_EmployeeBuySkill);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_EmployeeBuySkill);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_EmployeeBuySkill);
	}
	
	public override void Destroyobj ()
	{
		
	}
	#endregion

	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{

		OpenPanelAnimator.CloseOpenAnimation(this.panel, ()=>{
			Hide ();
		});
	}

	void UpdateBuyList()
	{
		moneyLab.text = GamePlayer.Instance.GetIprop (PropertyType.PT_EmployeeCurrency).ToString();
		foreach(ShopData s in ShopData.metaData.Values)
		{
			if(s._ShopType == ShopType.SIT_EmployeeShop)
			{
				GameObject o  = Instantiate(cell)as GameObject;
				o.transform.parent = grid.transform; 
				HeadIconLoader.Instance.LoadIcon( s._Icon,o.transform.FindChild("icon").GetComponent<UITexture>());

				
				if(!_icons.Contains(s._Icon))
				{
					_icons.Add(s._Icon);
				}

				o.transform.FindChild("name").GetComponent<UILabel>().text = s._Name;
				o.transform.FindChild("decs").GetComponent<UILabel>().text = ItemData.GetData(s._Itemid).desc_;
				o.transform.FindChild("xiaohao").GetComponent<UILabel>().text = s._Price.ToString(); 
				o.name = s._Id.ToString();
				o.SetActive(true);
				o.transform.localPosition = new Vector3(0,0,0);
				o.transform.localScale= new Vector3(1,1,1); 

				UIManager.SetButtonEventHandler (o.gameObject, EnumButtonEvent.OnClick, OnBuySkill, 0, 0);

			}
		}

		grid.Reposition ();
	}


	private void OnBuySkill(ButtonScript obj, object args, int param1, int param2)
	{
		ShopData sd = ShopData.GetData (int.Parse (obj.name));
		if(sd == null)
		{
			return;
		}
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_EmployeeCurrency) < sd._Price)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("noybzx"));
			return;
		}
		string type = "";
		if(sd._ShopPayType == ShopPayType.SPT_Diamond)
		{
			type = LanguageManager.instance.GetValue("zuanshi");
		}else if(sd._ShopPayType == ShopPayType.SPT_Gold)
		{
			type = LanguageManager.instance.GetValue("jinbi");
		}
		//else if(sd._ShopPayType == ShopPayType.SPT_EmployeeCurrency)
	//	{
		//	type = LanguageManager.instance.GetValue("yongbingzhixin");
		//}



		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("shopbuyitem").Replace("{n}",sd._Name+sd._Num).Replace("{n1}",(sd._Price).ToString()+type), () => {
			NetConnection.Instance.shopBuyItem(int.Parse(obj.name),1);
		});
	}

	public void UpdateMoney()
	{
		moneyLab.text = GamePlayer.Instance.GetIprop (PropertyType.PT_EmployeeCurrency).ToString();
	}
	

	void OnDestroy() 
	{
		GamePlayer.Instance.OnIPropUpdate -= UpdateMoney;

		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}

}

