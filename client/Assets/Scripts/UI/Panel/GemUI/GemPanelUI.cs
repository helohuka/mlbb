using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class GemPanelUI :UIBase
{
	public UIButton closeBtn;  
	public UIButton levelUpBtn;
	public UIButton changeBtn;
	public UIButton levelTabBtn;
	public UIButton changeTabBtn;   
	public UISprite gemIcon;
	public UISprite gemLevelIcon;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
	public List<UISprite> gemProp = new List<UISprite>();
	public List<UISprite> levelUpProp = new List<UISprite>();  
	public UILabel needMoneyLab;
	public UILabel needGem;  
	public UILabel cgLab;
	public GameObject levelUpObj;
	public GameObject changeObj;
	public List<UIButton> lockBtns = new List<UIButton>();
	public List<UIButton> ourlockBtns = new List<UIButton>();
	public List<UISprite> changeProp = new List<UISprite>();
	public List<GameObject> gemEffectObj = new List<GameObject> ();
	public List<UISprite> lockImg = new List<UISprite> ();
	public GameObject levelUpNeedObj;
	public UILabel changeNeedLab;
	public UILabel maxLevelLab;
	public UIButton buyBtn;
	private List<int> lockNumList =new List<int>();

	void Start ()
	{  
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		UIManager.SetButtonEventHandler (levelTabBtn.gameObject, EnumButtonEvent.OnClick, OnLevelTabBtn, 0, 0);
		UIManager.SetButtonEventHandler (changeTabBtn.gameObject, EnumButtonEvent.OnClick, OnChangeTabBtn, 0, 0);
		UIManager.SetButtonEventHandler (levelUpBtn.gameObject, EnumButtonEvent.OnClick, OnLevelUpBtn, 0, 0);
		UIManager.SetButtonEventHandler (changeBtn.gameObject, EnumButtonEvent.OnClick, OnChangeBtn, 0, 0);
		UIManager.SetButtonEventHandler (buyBtn.gameObject, EnumButtonEvent.OnClick, OnBuyBtn, 0, 0);
		GemSystem.instance.crystalEnvent += new RequestEventHandler<COM_CrystalData> (OncCystalEnvent);
		GemSystem.instance.crystalUpLevelEnvent += new RequestEventHandler<bool> (LevelUpEnvent);
		GemSystem.instance.resetCrystalPropOKEnvent += new RequestEventHandler<bool> (CrystalPropOKEnvent);
		BagSystem.instance.UpdateItemEvent += new ItemUpdateEventHandler (UpdateNeedItem);
		BagSystem.instance.ItemChanged += new ItemChangedEventHandler (UpdateNeedItem);
		GamePlayer.Instance.OnIPropUpdate += UpdateMoney;
		UIManager.Instance.LoadMoneyUI(this.gameObject);
		for(int i=0;i<lockBtns.Count;i++)    
		{
			UIManager.SetButtonEventHandler (lockBtns[i].gameObject, EnumButtonEvent.OnClick, OnlockBtn, i, 0);
			UIManager.SetButtonEventHandler (ourlockBtns[i].gameObject, EnumButtonEvent.OnClick, OnOutlockBtn, i, 0);
		}

		levelTabBtn.isEnabled = false;
		changeTabBtn.isEnabled = true;
		UpdateInfo ();
	}   

	void Update ()
	{

	}

	#region Fixed methods for UIBase derived cass
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_Gem);
	}  
	
	public static void ShowMe() 
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_Gem);
	}
	
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_Gem);
	}   
	public override void Destroyobj ()
	{
	}
	 
	#endregion

	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}

	private void OnLevelTabBtn(ButtonScript obj, object args, int param1, int param2)
	{
		levelTabBtn.isEnabled = false;
		changeTabBtn.isEnabled = true;
		levelUpObj.gameObject.SetActive (true);
		changeObj.gameObject.SetActive (false);

		UpdateInfo ();
	}

	private void OnChangeTabBtn(ButtonScript obj, object args, int param1, int param2)
	{
		levelTabBtn.isEnabled = true;
		changeTabBtn.isEnabled = false;
		levelUpObj.gameObject.SetActive (false);
		changeObj.gameObject.SetActive (true);
		UpdateInfo ();
	}

	private void OnChangeBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.GetProperty(PropertyType.PT_Diamond) < int.Parse(changeNeedLab.text))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("NoMoreDiamond"));
			return;
		}

		bool b = false;
		for(int i =0;i<GemSystem.instance.CrystalData.props_.Length;i++)
		{
			if(!lockNumList.Contains(i))
			{
				if(GemSystem.instance.CrystalData.props_[i].level_ >=4)
				{
					b = true;
				}
			}
		}

		if(b)
		{
			MessageBoxUI.ShowMe (LanguageManager.instance.GetValue ("gaojisuxing"), () => {
				
				NetConnection.Instance.resetCrystalProp(lockNumList.ToArray());
			});
		}
		else
		{
			NetConnection.Instance.resetCrystalProp(lockNumList.ToArray());
		}
	}

	private void OnBuyBtn(ButtonScript obj, object args, int param1, int param2)
	{
		int shopid = ShopData.GetShopId (21365);
		QuickBuyUI.ShowMe (shopid);
	}

	private void OnLevelUpBtn(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.crystalUpLevel();
	}

	private void OnlockBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(GemSystem.instance.CrystalData.props_.Length <= 1)
		{
			return;
		}
		lockBtns [param1].gameObject.SetActive (false);
		ourlockBtns [param1].gameObject.SetActive (true);
		if(!lockNumList.Contains(param1))
		{
			lockNumList.Add(param1);
		}
		int num = GemSystem.instance.CrystalData.props_.Length - lockNumList.Count;
		changeNeedLab.text = (20*(Math.Pow(2,lockNumList.Count))).ToString();

		for(int i=0;i<lockImg.Count;i++)
		{
			lockImg[i].gameObject.SetActive(false);
			if(lockNumList.Contains(i))
			{
				lockImg[i].gameObject.SetActive(true);
			}
		}
		if(num == 1)
		{
			for(int i=0;i<lockBtns.Count;i++)
			{
				lockBtns[i].gameObject.SetActive(false);
			}
		}
		else
		{
			for(int i=0;i<lockBtns.Count;i++)
			{
				if(!lockNumList.Contains(i))
				{
					lockBtns[i].gameObject.SetActive(true);
				}
			}
		}
	}

	private void OnOutlockBtn(ButtonScript obj, object args, int param1, int param2)
	{
		lockBtns [param1].gameObject.SetActive (true);
		ourlockBtns [param1].gameObject.SetActive (false); 

		if(lockNumList.Contains(param1))
		{
			lockNumList.Remove(param1);
		}
		int num = GemSystem.instance.CrystalData.props_.Length - lockNumList.Count;
		changeNeedLab.text = (20*(Math.Pow(2,lockNumList.Count))).ToString();

		for(int i=0;i<lockImg.Count;i++)
		{
			lockImg[i].gameObject.SetActive(false);
			if(lockNumList.Contains(i))
			{
				lockImg[i].gameObject.SetActive(true);
			}
		}
		if(num == 1)
		{
			for(int i=0;i<lockBtns.Count;i++)
			{
				lockBtns[i].gameObject.SetActive(false);
			}
		}
		else
		{
			for(int i=0;i<lockBtns.Count;i++)
			{
				if(!lockNumList.Contains(i))
				{
					lockBtns[i].gameObject.SetActive(true);
				}
			}
		}
	}
		
	public void UpdateMoney()
	{
		UpdateInfo ();
	}

	void OncCystalEnvent(COM_CrystalData data)
	{
		UpdateInfo ();
	}

	void UpdateNeedItem(COM_Item item )
	{
		COM_CrystalData data = GemSystem.instance.CrystalData; 
		if (data == null)
			return;
		CrystalUpData cData = CrystalUpData.GetData ((int)data.level_ + 1);
		int haveNum = BagSystem.instance.GetItemMaxNum(21365);
		needGem.text = haveNum +"/"+cData.DebrisNum.ToString();      
		if(haveNum < cData.DebrisNum)
		{
			levelUpBtn.isEnabled = false;
			needGem.color = Color.red;
		}
		else
		{
			needGem.color = Color.grey;
		}
	}

	void LevelUpEnvent(bool b)
	{
		if(b)
		{
			EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_familyLevelUp, gameObject.transform,()=>{});
		}
		else
		{
			EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_zhihuanSb, gameObject.transform,()=>{});
			UpdateInfo();
		}
	}

	void CrystalPropOKEnvent(bool b)
	{
		for(int i = 0;i<changeProp.Count;i++)
		{
			if(!lockNumList.Contains(i))
			{
				EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_zhihuanKS, changeProp[i].transform,null,(GameObject eff)=>{eff.transform.localPosition = Vector3.zero; });
			}
		}
		//lockNumList.Clear ();
		EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_zhihuancg, gameObject.transform,()=>{});
	}

	private void UpdateInfo()
	{
		COM_CrystalData data = GemSystem.instance.CrystalData; 
		//data.level_ = 2;
		if (data == null)
			return;
		gemIcon.spriteName = "baoshi_lv"+data.level_;
		gemLevelIcon.spriteName = "dengji_"+data.level_;

		for(int e =1;e<gemEffectObj.Count;e++)
		{
			gemEffectObj[e].gameObject.SetActive(false);
		}
		if(data.level_ > 1)
			gemEffectObj[(int)data.level_-1].gameObject.SetActive(true);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    

		CrystalUpData cData = CrystalUpData.GetData ((int)data.level_ + 1);
		//if(cData == null)
		//{  
			//return;
		//}
		if(levelUpObj.activeSelf)
		{
			for(int j=0;j<levelUpProp.Count;j++)
			{
				levelUpProp[j].gameObject.SetActive(false);
				gemProp[j].gameObject.SetActive(false);
			}
			for(int i=0;i<data.props_.Length;i++)
			{
  				levelUpProp[i].gameObject.SetActive(true);
				gemProp[i].gameObject.SetActive(true);
				gemPropCellUI cell = gemProp[i].GetComponent<gemPropCellUI>();    
				cell.propName.text =  LanguageManager.instance.GetValue(data.props_[i].type_.ToString());
				cell.propNum.text = "+"+data.props_[i].val_;
				gemProp[i].GetComponent<UISprite>().spriteName = getPropString( (int)data.props_[i].level_);    
				gemPropCellUI cell0 = levelUpProp[i].GetComponent<gemPropCellUI>();
				cell0.propName.text =  LanguageManager.instance.GetValue(data.props_[i].type_.ToString());
				cell0.propNum.text = "+"+data.props_[i].val_;
				levelUpProp[i].GetComponent<UISprite>().spriteName = getPropString( (int)data.props_[i].level_); 
				CrystalData qjData = CrystalData.GetData((int)data.props_[i].type_,(int)data.props_[i].level_);
				if(qjData == null)
					continue;
				string[] strArr = qjData.property.Split(';');
				cell0.qjLab.text = "("+ strArr[0] + "-" +strArr[1]+")"; 
			}
			levelUpBtn.isEnabled = true;
			if(cData == null)
			{  
				levelUpBtn.isEnabled = false;
				levelUpNeedObj.gameObject.SetActive(false);
				maxLevelLab.gameObject.SetActive(true);
				return;
			}
			else
			{
				levelUpNeedObj.gameObject.SetActive(true);
				maxLevelLab.gameObject.SetActive(false);
			}
			needMoneyLab.text = cData.GodNum.ToString();
			if(GamePlayer.Instance.GetIprop(PropertyType.PT_Money)< cData.GodNum)
			{ 
				levelUpBtn.isEnabled = false;
				needMoneyLab.color = Color.red;
			}    
			else  
			{ 
				needMoneyLab.color = Color.grey;   
			} 
			int haveNum = BagSystem.instance.GetItemMaxNum(21365);
			needGem.text = haveNum +"/"+cData.DebrisNum.ToString();      
			if(haveNum < cData.DebrisNum)
			{
				levelUpBtn.isEnabled = false;
				needGem.color = Color.red;
			}
			else
			{
				needGem.color = Color.grey;
			}
			cgLab.text =  cData.Mission +"%"; 
		}
		else 
		{
			for(int j=0;j<changeProp.Count;j++)
			{
				changeProp[j].gameObject.SetActive(false);
			}
			for(int i=0;i<data.props_.Length;i++)
			{  

				gemProp[i].gameObject.SetActive(true);
				gemPropCellUI cell = gemProp[i].GetComponent<gemPropCellUI>();    
				cell.propName.text =  LanguageManager.instance.GetValue(data.props_[i].type_.ToString());
				cell.propNum.text = "+"+data.props_[i].val_;
				gemProp[i].GetComponent<UISprite>().spriteName = getPropString( (int)data.props_[i].level_);    

				changeProp[i].gameObject.SetActive(true);
				gemPropCellUI cell0 = changeProp[i].GetComponent<gemPropCellUI>();
				cell0.propName.text =  LanguageManager.instance.GetValue(data.props_[i].type_.ToString());
				cell0.propNum.text = "+"+data.props_[i].val_;
				changeProp[i].GetComponent<UISprite>().spriteName = getPropString( (int)data.props_[i].level_); 
			}
			if(data.props_.Length <=1)
			{
				lockBtns[0].gameObject.SetActive(false);  
			} 
			else
			{
				if(data.props_.Length - lockNumList.Count > 1)
				{
					for(int i=0;i<lockBtns.Count;i++)
					{
						if(!lockNumList.Contains(i))
						{
							lockBtns[i].gameObject.SetActive(true);
							
						}
					}
				}
			}

			int num = data.props_.Length - lockNumList.Count;
			changeNeedLab.text = (20*(Math.Pow(2,lockNumList.Count))).ToString();

			for(int i=0;i<lockImg.Count;i++)
			{
				lockImg[i].gameObject.SetActive(false);
				if(lockNumList.Contains(i))
				{
					lockImg[i].gameObject.SetActive(true);
				}
			}
		}
	}
	void OnDestroy()
	{
		GemSystem.instance.crystalEnvent -= OncCystalEnvent;
		GemSystem.instance.crystalUpLevelEnvent -= LevelUpEnvent;
		GemSystem.instance.resetCrystalPropOKEnvent -= CrystalPropOKEnvent;
		GamePlayer.Instance.OnIPropUpdate -= UpdateMoney;
		BagSystem.instance.UpdateItemEvent -= UpdateNeedItem;
		BagSystem.instance.ItemChanged -= UpdateNeedItem;
	}

	public string getPropString(int level)
	{
		switch(level)
		{
		case 1:
		{
			return "webzudi";
		}
			break;
		case 2:
		{
			return "webzudilv";
		}
			break;
		case 3:
		{
			return "webzudidilan";
		}
			break;
		case 4:
		{
			return "webzudizi";
		}
			break;
		case 5:
		{
			return "webzudihuang";
		}
			break;
			
		}

		return "webzudihei";
	}

}

