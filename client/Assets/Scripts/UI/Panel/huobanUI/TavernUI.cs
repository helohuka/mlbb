using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TavernUI : MonoBehaviour {

	public UIButton mercenaryBtn;
	public UIButton higherBtn;
	public UIButton topBtn;
	public UIButton PerfectBtn;
	public UIButton LotteryBtn;
	public UIButton CancelBtn;
	public GameObject EmployeItem;
	public GameObject prizeWin;
	public UILabel greenTimeCd;
	public UILabel blueTimeCd;
	public UILabel greenFreeLab;
	public UILabel blueFreeLab;
	public UILabel greenMoneyLab;
	public UILabel blueMoneyLab;
	public UILabel goldMoneyLab;
	public UILabel blueZhuanLab;
	public UILabel goldZhuanLab;
	public GameObject fiveEmployeePanel;
	public GameObject CloseFiveBtn;

	public GameObject panelObj;
	public UITexture effectImg;
	public UISprite effectBlack;

	public GameObject efftZi;
	public GameObject efftJin;
	public GameObject efftCheng;
	public GameObject efftFen;
	public UILabel buyBtnLab0;
	public UILabel buyBtnLab1;
	public UILabel buyBtnLab2;

	public UITexture backImg;
	public UITexture backImgRole;
	public UILabel haveItemNum;

	public List<UISprite> fiveEmployeeList = new List<UISprite> (); 
	private List<Transform> EmployeePos = new List<Transform>();
	private List<GameObject> EmployeeObj = new List<GameObject>();
	private List<GameObject> FiveEmployeeObj = new List<GameObject>();
	private List<GameObject> EmployeeKuangObj = new List<GameObject>();

	private List<EmployeeData> _employeeDataList = new List<EmployeeData>();
	private int[] fiveEmployee  = {2005,2001,2002,2003,2004};

	private int count;
	private int maxCount;
	private BoxType btpye;
	private List<string> _icons = new List<string>();
	private int _BoxGreenSpend;
	private int _BoxBlueSpend;
	private int _BoxGoldSpend;

	private int drawNeedItem_;
	private int drawNeedBlueNum_;
	private int drawNeedGoldNum_;
	private int drawNeedBlueZhuanNum_;
	private int drawNeedGoldZhuanNum_;

	private bool isLoadBack;

	void Start () 
	{
		UIManager.SetButtonEventHandler (mercenaryBtn.gameObject, EnumButtonEvent.OnClick, OnClickmercenaryBtn, 0, 0);
		UIManager.SetButtonEventHandler (higherBtn.gameObject, EnumButtonEvent.OnClick, OnClicktopBtn, 0, 0);
		UIManager.SetButtonEventHandler (topBtn.gameObject, EnumButtonEvent.OnClick, OnClickhigherBtn, 0, 0);
		UIManager.SetButtonEventHandler (PerfectBtn.gameObject, EnumButtonEvent.OnClick, OnClicPerfectBtn, 0, 0);
		UIManager.SetButtonEventHandler (CloseFiveBtn.gameObject, EnumButtonEvent.OnClick, OnCloseFuveBtn, 0, 0);

		UIManager.SetButtonEventHandler (LotteryBtn.gameObject, EnumButtonEvent.OnClick, OnClicLottery, 0, 0);
		UIManager.SetButtonEventHandler (CancelBtn.gameObject, EnumButtonEvent.OnClick, OnClicCancel, 0, 0);

		buyBtnLab0.text = LanguageManager.instance.GetValue("buyBtnLab");
		buyBtnLab1.text = LanguageManager.instance.GetValue("duihuanbuy");
		buyBtnLab2.text = LanguageManager.instance.GetValue("duihuanbuy");

		GamePlayer.Instance.DrawEmployeeEnvent += new RequestEventHandler<COM_Item[]> (OnDrawEmployessOk);
		BagSystem.instance.UpdateItemEvent += new ItemUpdateEventHandler (OnUpdateItem); 
		BagSystem.instance.DelItemInstEvent += new ItemDelInstEventHandler (OnUpdateItem);
		GlobalValue.Get(Constant.C_BoxGreenSpend, out _BoxGreenSpend);
		GlobalValue.Get(Constant.C_BoxBlueSpend, out _BoxBlueSpend);
		GlobalValue.Get(Constant.C_BoxGoldSpend, out _BoxGoldSpend);

		GlobalValue.Get(Constant.C_BoxBlueSpendItem, out drawNeedItem_);
		GlobalValue.Get(Constant.C_BoxBlueSpend, out drawNeedBlueNum_);
		GlobalValue.Get(Constant.C_BoxGoldSpend, out drawNeedGoldNum_);
		GlobalValue.Get(Constant.C_BoxBlueSpendDiamond, out drawNeedBlueZhuanNum_);
		GlobalValue.Get(Constant.C_BoxGoldSpendDiamond, out drawNeedGoldZhuanNum_);

		haveItemNum.text  = BagSystem.instance.GetItemMaxNum((uint)drawNeedItem_).ToString();
		greenMoneyLab.text = _BoxGreenSpend.ToString ();
		blueMoneyLab.text = drawNeedBlueNum_.ToString ()+"X";
		goldMoneyLab.text = drawNeedGoldNum_.ToString ()+"X";
		blueZhuanLab.text = drawNeedBlueZhuanNum_.ToString ()+"X";
		goldZhuanLab.text = drawNeedGoldZhuanNum_.ToString ()+"X";

		EmployeItem.SetActive (false);

		GuideManager.Instance.RegistGuideAim(topBtn.gameObject, GuideAimType.GAT_FreeLootPartner);
        GuideManager.Instance.RegistGuideAim(CancelBtn.gameObject, GuideAimType.GAT_PartnerShowCancel);

        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_ClickMainPartner);

    }


	void Update () 
	{
		if(BoxSystem.Instance.GreenCDTime > 0 )
		{
			greenTimeCd.gameObject.SetActive(true);
			greenTimeCd.text = FormatTime((int)BoxSystem.Instance.GreenCDTime);
			greenMoneyLab.gameObject.SetActive(true);
			greenFreeLab.gameObject.SetActive(false);
		}
		else
		{
			greenTimeCd.gameObject.SetActive(false);
			greenMoneyLab.gameObject.SetActive(false);
			greenFreeLab.gameObject.SetActive(true); 
		}

		if(BoxSystem.Instance.FreeNum <=0)
		{
			greenTimeCd.gameObject.SetActive(false);
			greenFreeLab.gameObject.SetActive(false);
			greenMoneyLab.gameObject.SetActive(true);
		}

		if(BoxSystem.Instance.BlueCDTime > 0)
		{
			blueTimeCd.gameObject.SetActive(true);
			blueTimeCd.text = FormatTimeHasHour((int)BoxSystem.Instance.BlueCDTime);
			blueMoneyLab.gameObject.SetActive(true);
			blueFreeLab.gameObject.SetActive(false);
		}
		else
		{
			blueTimeCd.gameObject.SetActive(false);
			blueMoneyLab.gameObject.SetActive(false);
			blueFreeLab.gameObject.SetActive(true);
		}

		
	}
	
	void OnClickmercenaryBtn(ButtonScript obj, object args, int param1, int param2)
	{
		ShowFiveEmployee ();
		fiveEmployeePanel.SetActive (true);
	}
	
	void OnClickhigherBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.EmployeeList.Count + 1 > 100)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("EN_EmployeeIsFull"));
			return;
		}
		showBuyBtn(false);


		if (BoxSystem.Instance.BlueCDTime > 0) 
		{

			if(BagSystem.instance.GetItemMaxNum((uint)drawNeedItem_) >= drawNeedBlueNum_)
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("xiaohaojizhnag").Replace("{n1}", drawNeedBlueNum_.ToString ()).Replace("{n}",ItemData.GetData(drawNeedItem_).name_),()=>{
					NetConnection.Instance.drawLotteryBox (BoxType.BX_Blue, false);
					btpye = BoxType.BX_Blue;
					
				},false,()=>{showBuyBtn (true);});
			}
			else if(GamePlayer.Instance.GetIprop(PropertyType.PT_Diamond) >= drawNeedBlueZhuanNum_)
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("chouxiaohanzs").Replace("{n}",drawNeedBlueZhuanNum_.ToString()),()=>{
					NetConnection.Instance.drawLotteryBox (BoxType.BX_Blue, false);
					btpye = BoxType.BX_Blue;
					
				},false,()=>{showBuyBtn (true);});
			}
			else if(GamePlayer.Instance.GetIprop(PropertyType.PT_MagicCurrency) >= drawNeedBlueZhuanNum_)
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("xiaohaotimoli").Replace("{n}",drawNeedBlueZhuanNum_.ToString()),()=>{
					NetConnection.Instance.drawLotteryBox (BoxType.BX_Blue, false);
					btpye = BoxType.BX_Blue;
					
				},false,()=>{showBuyBtn (true);});
			}
			else
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("daojubuzu"),()=>{StoreUI.SwithShowMe(2);});
			}
			showBuyBtn (true);
			return;
		}
		else
		{
			NetConnection.Instance.drawLotteryBox (BoxType.BX_Blue, false);
			btpye = BoxType.BX_Blue;
		}
		bool cost = BoxSystem.Instance.BlueCDTime > 0;
		EmployessSystem.instance._BuyEmployeeTable_ = cost? BoxType.BX_Blue: BoxType.BX_None;
	}
	void OnClicktopBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.EmployeeList.Count +1 > 100)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("EN_EmployeeIsFull"));
			return;
		}
		showBuyBtn(false);
		if (BoxSystem.Instance.GreenCDTime > 0) 
		{
			if(GamePlayer.Instance.GetIprop(PropertyType.PT_Money)< _BoxGreenSpend)
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("nomoney"),()=>{showBuyBtn(true);});
				showBuyBtn (true);
				return;
			}

			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("chouxiaohanjq").Replace("{n}", _BoxGreenSpend.ToString ()),()=>{
				//NetConnection.Instance.drawLotteryBox(btpye,false);
				NetConnection.Instance.drawLotteryBox (BoxType.BX_Normal, false);
				btpye = BoxType.BX_Normal;
				GuideManager.Instance.ClearMask();
			},false,()=>{showBuyBtn(true);});

		}
		else
		{
			NetConnection.Instance.drawLotteryBox (BoxType.BX_Normal, false);
			btpye = BoxType.BX_Normal;
			GuideManager.Instance.ClearMask();
		}
		EmployessSystem.instance._BuyEmployeeTable_ = btpye;
	}
	void OnClicPerfectBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.EmployeeList.Count+10 > 100)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("EN_EmployeeIsFull"));
			return;
		}
		showBuyBtn (false);


		if(BagSystem.instance.GetItemMaxNum((uint)drawNeedItem_) >= drawNeedGoldNum_)
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("xiaohaojizhnag").Replace("{n1}", drawNeedGoldNum_.ToString ()).Replace("{n}",ItemData.GetData(drawNeedItem_).name_),()=>{
				NetConnection.Instance.drawLotteryBox (BoxType.BX_Glod, false);
				btpye = BoxType.BX_Glod;
			},false,()=>{showBuyBtn (true);});
			EmployessSystem.instance._BuyEmployeeTable_ = BoxType.BX_Glod;
		}
		else if(GamePlayer.Instance.GetIprop(PropertyType.PT_Diamond) >= drawNeedGoldZhuanNum_)
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("chouxiaohanzs").Replace("{n}",drawNeedGoldZhuanNum_.ToString()),()=>{
				NetConnection.Instance.drawLotteryBox (BoxType.BX_Glod, false);
				btpye = BoxType.BX_Glod;
			},false,()=>{showBuyBtn (true);});
			EmployessSystem.instance._BuyEmployeeTable_ = BoxType.BX_Glod;
		}
		else if(GamePlayer.Instance.GetIprop(PropertyType.PT_MagicCurrency) >= drawNeedGoldZhuanNum_)
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("xiaohaotimoli").Replace("{n}",drawNeedGoldZhuanNum_.ToString()),()=>{
				NetConnection.Instance.drawLotteryBox (BoxType.BX_Glod, false);
				btpye = BoxType.BX_Glod;
			},false,()=>{showBuyBtn (true);});
			EmployessSystem.instance._BuyEmployeeTable_ = BoxType.BX_Glod;
		}
		else
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("daojubuzu"),()=>{StoreUI.SwithShowMe(2);});
		}
		showBuyBtn (true);

	}

	void OnClicLottery(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.EmployeeList.Count + 1 > 100)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("EN_EmployeeIsFull"));
			showBuyBtn (true);
			return;
		}

		string numStr ="";
		if(btpye == BoxType.BX_Glod)
		{
			if(BagSystem.instance.GetItemMaxNum((uint)drawNeedItem_) >= drawNeedGoldNum_)
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("xiaohaojizhnag").Replace("{n1}", drawNeedGoldNum_.ToString ()).Replace("{n}",ItemData.GetData(drawNeedItem_).name_),()=>{
					NetConnection.Instance.drawLotteryBox(btpye,false);
					LotteryBtn.isEnabled = false;
					SetPrizeWinDisplay (false);
				});
			}

			else if(GamePlayer.Instance.GetIprop(PropertyType.PT_Diamond) >= drawNeedGoldZhuanNum_)
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("chouxiaohanzs").Replace("{n}",drawNeedGoldZhuanNum_.ToString()),()=>{
					NetConnection.Instance.drawLotteryBox(btpye,false);
					LotteryBtn.isEnabled = false;
					SetPrizeWinDisplay (false);});
			}
			else if(GamePlayer.Instance.GetIprop(PropertyType.PT_MagicCurrency) >= drawNeedGoldZhuanNum_)
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("xiaohaotimoli").Replace("{n}",drawNeedGoldZhuanNum_.ToString()),()=>{
					NetConnection.Instance.drawLotteryBox(btpye,false);
					LotteryBtn.isEnabled = false;
					SetPrizeWinDisplay (false);});
			}
			else
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("daojubuzu"),()=>{StoreUI.SwithShowMe(2);});
			}
			showBuyBtn (true);

			return;
		}
		else if(btpye == BoxType.BX_Blue)
		{
			if(BagSystem.instance.GetItemMaxNum((uint)drawNeedItem_) >= drawNeedBlueNum_)
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("xiaohaojizhnag").Replace("{n1}", drawNeedBlueNum_.ToString ()).Replace("{n}",ItemData.GetData(drawNeedItem_).name_),()=>{
					NetConnection.Instance.drawLotteryBox(btpye,false);
					LotteryBtn.isEnabled = false;
					SetPrizeWinDisplay (false);
				});
			}
			else if(GamePlayer.Instance.GetIprop(PropertyType.PT_Diamond) >= drawNeedBlueZhuanNum_)
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("chouxiaohanzs").Replace("{n}",drawNeedBlueZhuanNum_.ToString()),()=>{
					NetConnection.Instance.drawLotteryBox (BoxType.BX_Blue, false);
					LotteryBtn.isEnabled = false;
					SetPrizeWinDisplay (false);
					
				});
			}
			else if(GamePlayer.Instance.GetIprop(PropertyType.PT_MagicCurrency) >= drawNeedBlueZhuanNum_)
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("xiaohaotimoli").Replace("{n}",drawNeedBlueZhuanNum_.ToString()),()=>{
					NetConnection.Instance.drawLotteryBox (BoxType.BX_Blue, false);
					LotteryBtn.isEnabled = false;
					SetPrizeWinDisplay (false);
					
				});
			}
			else
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("daojubuzu"),()=>{StoreUI.SwithShowMe(2);});
			}
			showBuyBtn (true);

		}
		else if(btpye == BoxType.BX_Normal)
		{
			if(GamePlayer.Instance.GetIprop(PropertyType.PT_Money)< _BoxGreenSpend)
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("nomoney"),()=>{});
				showBuyBtn (true);
				return;
			}

			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("chouxiaohanjq").Replace("{n}", _BoxGreenSpend.ToString ()),()=>{
				NetConnection.Instance.drawLotteryBox(btpye,false);
				LotteryBtn.isEnabled = false;
				SetPrizeWinDisplay (false);
			});

			showBuyBtn (true);
		}

	
	}

	void OnClicCancel(ButtonScript obj, object args, int param1, int param2)
	{
		ResetData ();
		SetPrizeWinDisplay (false);
		panelObj.gameObject.SetActive (true);
		backImg.gameObject.SetActive (false);
		backImgRole.gameObject.SetActive (false);
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_PartnerHideUI);
	}


	private void OnCloseFuveBtn (ButtonScript obj, object args, int param1, int param2)
	{
		fiveEmployeePanel.SetActive (false);
	}
	public  void OnDrawEmployessOk(COM_Item[] items)
	{
		if(items.Length<=0)
		{
			return;
		}
		panelObj.gameObject.SetActive (false);
		backImg.gameObject.SetActive (true);
		backImgRole.gameObject.SetActive (true);
		if(!isLoadBack)
		{
			HeadIconLoader.Instance.LoadIcon("jiubaxintu", backImg);
			HeadIconLoader.Instance.LoadIcon("renwu_quanshen", backImgRole);
			isLoadBack = true;
		}
		effectImg.gameObject.SetActive(false);
		effectBlack.gameObject.SetActive(false);

		if(items.Length > 1)
		{
			for (int e = 0; e<items.Length; e++) 
			{
				int p = Random.Range(0,e);
				COM_Item temp = items[p];
				items[p] = items[e];
				items[e] = temp;
			}
		}

		List<EmployeeData> Employees = new List<EmployeeData>();
		for (int i = 0; i<items.Length; i++) {
			ItemData item = ItemData.GetData ((int)items [i].itemId_);
			if(item == null)
			{
				return;
			}
			if (item.mainType_ == ItemMainType.IMT_Employee) 
			{
				EmployeeData employee = EmployeeData.GetData(item.employeeId_);
				if(employee == null)
				{
					return;
				}
				Employees.Add(employee);

			}	
		}

		ShowEmployees (Employees);
		SetPrizeWinDisplay (true);
	}

	void ShowEmployees(List<EmployeeData> Employee)
	{
        GuideManager.Instance.ClearMask();
		LotteryBtn.gameObject.SetActive (false);
		CancelBtn.gameObject.SetActive (false);
		DestroyMode ();

		for(int i = 0;i<EmployeeKuangObj.Count;i++)
		{
			EmployeeKuangObj[i].gameObject.SetActive(false);
		}

		_employeeDataList.Clear ();
		_employeeDataList = Employee;
		maxCount = Employee.Count;
		if (Employee.Count == 1) 
		{

			EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_Zhaomu, gameObject.GetComponentInParent<EmployessControlUI> ().transform,()=>{
				if(EmployeeKuangObj.Count <= 0)
				{
					GameObject clone = GameObject.Instantiate(EmployeItem) as GameObject;
					clone.SetActive(true);

					GameObject tx = GetEffetObj((int)Employee[0].quality_);
					if(tx != null)
					{
						tx.transform.parent = clone.transform;
						tx.SetActive(true);
						tx.transform.localPosition = Vector3.zero;
						tx.transform.localScale = Vector3.one;
					}
					clone.transform.parent = prizeWin.transform;
					clone.transform.position = Vector3.zero;
					clone.transform.localScale = Vector3.one;
					EmployeeKuangObj.Add(clone);
				}
				EmployeeKuangObj[0].GetComponent<UISprite>().spriteName = EmployessSystem.instance.GetQualityBack((int)Employee[0].quality_);

				HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData(Employee[0].asset_id).assetsIocn_,EmployeeKuangObj[0].transform.Find("icon").GetComponent<UITexture>());

				if(!_icons.Contains(EntityAssetsData.GetData(Employee[0].asset_id).assetsIocn_))
				{
					_icons.Add(EntityAssetsData.GetData(Employee[0].asset_id).assetsIocn_);
				}

				UILabel [] labs = EmployeeKuangObj[0].GetComponentsInChildren<UILabel>(true);
				foreach(UILabel la in labs)
				{
					if(la.gameObject.name.Equals("nameLabel"))
					{
						la.text = Employee[0].name_;
					}
					if(la.gameObject.name.Equals("zhiyeLabel"))
					{
						la.text =  Profession.get(Employee[0].professionType_, 
						                          Employee[0].jobLevel_).jobName_;
					}

				}
				EmployeeKuangObj[0].transform.Find("job").GetComponent<UISprite>().spriteName = Employee[0].professionType_.ToString(); 
				EmployeeKuangObj[0].transform.Find("back").GetComponent<UISprite>().spriteName = EmployessSystem.instance.GetCellQualityBack((int)Employee[0].quality_);

	           // GameManager.Instance.GetActorClone((ENTITY_ID)Employee[0].asset_id, (ENTITY_ID)0, AssetLoadCallBack,null,"UI");

				SetEmployeeObjPosition();

				effectImg.gameObject.SetActive(true);
				effectBlack.gameObject.SetActive(true);

                GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_PartnerShowUI);

			}, (GameObject obj) => {
				obj.SetActive(false);
                Partical4Pad p4p = obj.AddComponent<Partical4Pad>();
                p4p.SetScale();
				GlobalInstanceFunction.Instance.Invoke( () => {
					obj.SetActive(true);
				}, 1);
			});
			
		}
		else 
		{

			EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_Zhaomu, gameObject.GetComponentInParent<EmployessControlUI> ().transform,()=>{
			
			int pw = prizeWin.gameObject.GetComponent<UISprite>().width;
			int cellw = EmployeItem.gameObject.GetComponent<UISprite>().width;
			int startX = (pw-cellw*5)/2;


			if(EmployeeKuangObj.Count < Employee.Count)
			{
				for(int i = EmployeeKuangObj.Count;i<Employee.Count;i++)
				{
					int x = i/2;
					int y = i%2;
					GameObject clone = GameObject.Instantiate(EmployeItem) as GameObject;
					clone.SetActive(true);

						GameObject tx = GetEffetObj((int)Employee[i].quality_);
						if(tx != null)
						{
							tx.transform.parent = clone.transform;
							tx.SetActive(true);
							tx.transform.localPosition = Vector3.zero; 
							tx.transform.localScale = Vector3.one;
						}

					clone.transform.parent = prizeWin.transform;
					clone.transform.position = Vector3.zero;
					clone.transform.localScale = Vector3.one;
					clone.transform.localPosition = new Vector3(-670+x*350,230-y*380,0);
					
					clone.transform.Find("nameLabel").GetComponent<UILabel>().text = Employee[i].name_; 
					clone.transform.Find("zhiyeLabel").GetComponent<UILabel>().text =  Profession.get(Employee[i].professionType_, 
						                          Employee[i].jobLevel_).jobName_;

						HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData(Employee[i].asset_id).assetsIocn_,clone.transform.Find("icon").GetComponent<UITexture>());

						if(!_icons.Contains(EntityAssetsData.GetData(Employee[i].asset_id).assetsIocn_))
						{
							_icons.Add(EntityAssetsData.GetData(Employee[i].asset_id).assetsIocn_);
						}



						clone.transform.Find("job").GetComponent<UISprite>().spriteName = Employee[i].professionType_.ToString();
						clone.GetComponent<UISprite>().spriteName = EmployessSystem.instance.GetQualityBack((int)Employee[i].quality_);
						clone.transform.Find("back").GetComponent<UISprite>().spriteName = EmployessSystem.instance.GetCellQualityBack((int)Employee[i].quality_);
					EmployeeKuangObj.Add(clone);
				}
			}
			
			for(int i= 0;i<EmployeeKuangObj.Count;i++)
			{
					EmployeeKuangObj[i].transform.Find("nameLabel").GetComponent<UILabel>().text = Employee[i].name_; 
					EmployeeKuangObj[i].transform.Find("zhiyeLabel").GetComponent<UILabel>().text =  Profession.get(Employee[i].professionType_, 
					                                                                                  Employee[i].jobLevel_).jobName_;
					HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData(Employee[i].asset_id).assetsIocn_,EmployeeKuangObj[i].transform.Find("icon").GetComponent<UITexture>());

                    if (!_icons.Contains(EntityAssetsData.GetData(Employee[i].asset_id).assetsIocn_))
                    {
                        _icons.Add(EntityAssetsData.GetData(Employee[i].asset_id).assetsIocn_);
                    }

					EmployeeKuangObj[i].transform.Find("job").GetComponent<UISprite>().spriteName = Employee[i].professionType_.ToString();
					EmployeeKuangObj[i].GetComponent<UISprite>().spriteName = EmployessSystem.instance.GetQualityBack((int)Employee[i].quality_);
					EmployeeKuangObj[i].transform.Find("back").GetComponent<UISprite>().spriteName = EmployessSystem.instance.GetCellQualityBack((int)Employee[i].quality_);
					Transform txObj = EmployeeKuangObj[i].gameObject.transform.FindChild ("effct");
					if(txObj != null)
					{
						txObj.gameObject.SetActive(false);
						Destroy(txObj.gameObject);
					}	


					GameObject tx = GetEffetObj((int)Employee[i].quality_);
					if(tx != null)
					{
						tx.transform.parent = EmployeeKuangObj[i].transform;
						tx.SetActive(true);
						tx.transform.localPosition = Vector3.zero;
						tx.transform.localScale = Vector3.one;
					}


			}

			effectImg.gameObject.SetActive(true);
			effectBlack.gameObject.SetActive(true);
			//loadAssEntity();
				SetEmployeeObjPosition();
			/*LotteryBtn.gameObject.SetActive (true);
			LotteryBtn.isEnabled = true;
			CancelBtn.gameObject.SetActive (true);
			showBuyBtn(true);
			*/
                GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_PartnerShowUI);
			},(GameObject obj) => {
				obj.SetActive(false);
                Partical4Pad p4p = obj.AddComponent<Partical4Pad>();
                p4p.SetScale();
				GlobalInstanceFunction.Instance.Invoke( () => {
					obj.SetActive(true);
				}, 1);
			});

			//EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.TenComboZhaomuEffectId, gameObject.GetComponentInParent<PartnerPanel> ().transform);
		}


	}

	private void loadAssEntity()
	{
		if(_employeeDataList[0] != null)
		{
			GameManager.Instance.GetActorClone((ENTITY_ID)_employeeDataList[0].asset_id, (ENTITY_ID)0, EntityType.ET_Emplyee, AssetLoadCallBack,null,"UI");
			_employeeDataList.Remove (_employeeDataList [0]);
		}
	}

	void AssetLoadCallBack(GameObject ro, ParamData data)
	{
		//NGUITools.SetChildLayer(ro.transform, LayerMask.NameToLayer("3D"));
		ro.gameObject.SetActive (false);
		EmployeeObj.Add (ro);

		if(EmployeeObj.Count < EmployeeKuangObj.Count)
		{
			loadAssEntity();
		}
		else
		{
			SetEmployeeObjPosition();
		}

       
	}
	void SetEmployeeObjPosition()
	{
		for (int i = 0; i<EmployeeKuangObj.Count; i++) 
		{
			/*EmployeeObj[i].SetActive(true); 
			EmployeeKuangObj[i].gameObject.SetActive(true);
			EmployeeObj[i].transform.parent = EmployeeKuangObj[i].transform.Find("modPos");// EmployeePos[i];
			EmployeeObj[i].transform.localScale = new Vector3(300f,300f,300f);
			//EmployeeObj[i].transform.localPosition = Vector3.zero;
			EmployeeObj[i].transform.localPosition = Vector3.forward * 100f;
			EmployeeObj[i].transform.localRotation = Quaternion.Euler (0f, 180f, 0f);
			EffectLevel el =EmployeeObj[i].AddComponent<EffectLevel>();
			el.target =EmployeeObj[i].transform.parent.parent.GetComponent<UISprite>();
			*/


			//EmployeeObj[i].SetActive(false);
			EmployeeKuangObj[i].gameObject.SetActive(true);
			/*EmployeeObj[i].transform.parent =EmployeeKuangObj[i].transform.Find("modPos");;
			EmployeeObj[i].transform.localPosition = Vector3.forward * 100f;
			EmployeeObj[i].transform.localScale = new Vector3(400f,400f,1f);
			//modeList[i].transform.localPosition = Vector3.zero;
			EmployeeObj[i].transform.localRotation = Quaternion.Euler (0f, 180f, 0f);
			EmployeeObj[i].SetActive(true);
			EffectLevel el = EmployeeObj[i].AddComponent<EffectLevel>();
			el.target = EmployeeObj[i].transform.parent.parent.GetComponent<UISprite>();
			*/	



		}

		LotteryBtn.gameObject.SetActive (true);
		LotteryBtn.isEnabled = true;
		CancelBtn.gameObject.SetActive (true);
		showBuyBtn(true);

	}

	void SetPrizeWinDisplay(bool isDisplay)
	{
		prizeWin.SetActive (isDisplay);
	}
	void ResetData()
	{
		count = 0;
		EmployeePos.Clear ();
		DestroyMode ();
		for(int i = 0;i<EmployeeKuangObj.Count;i++)
		{
			GameObject.Destroy(EmployeeKuangObj[i].gameObject);
			EmployeeKuangObj[i] = null;
		}
		EmployeeKuangObj.Clear ();
	}



	void DestroyMode()
	{
		for(int i = 0;i<EmployeeObj.Count;i++)
		{
			GameObject.Destroy(EmployeeObj[i].gameObject);
			EmployeeObj[i] = null;
		}
		EmployeeObj.Clear ();
	}

	private void ShowFiveEmployee()
	{
		if(FiveEmployeeObj.Count > 0)
		{
			return;
		}
		_employeeDataList.Clear ();
		for (int i=0; i<fiveEmployee.Length; i++) 
		{
			EmployeeData employees = EmployeeData.GetData (fiveEmployee[i]);
			fiveEmployeeList[i].transform.Find ("nameLabel").GetComponent<UILabel>().text = employees.name_;
			fiveEmployeeList[i].transform.Find("zhiyeLabel").GetComponent<UILabel>().text =  Profession.get(employees.professionType_, 
			                                                                                                employees.jobLevel_).jobName_;
			fiveEmployeeList[i].GetComponent<UISprite>().spriteName = EmployessSystem.instance.GetTavernQualityBack((int)employees.quality_);

			_employeeDataList.Add(employees);
		}
		fiveLoad ();
	
	}


	private void fiveLoad()
	{
		GameManager.Instance.GetActorClone((ENTITY_ID)_employeeDataList[0].asset_id, (ENTITY_ID)0, EntityType.ET_Emplyee, AssetFiveLoadCallBack,null,"UI");
		_employeeDataList.Remove (_employeeDataList [0]);
	}


	void AssetFiveLoadCallBack(GameObject ro, ParamData data)
	{
		//NGUITools.SetChildLayer(ro.transform, LayerMask.NameToLayer("3D"));
		ro.gameObject.SetActive (false);
		FiveEmployeeObj.Add (ro);
		
		if(FiveEmployeeObj.Count >= fiveEmployeeList.Count)
		{
			SetFiveEmployeeObjPosition();
		}
		else
		{
			fiveLoad();
		}
	}

	void SetFiveEmployeeObjPosition()
	{
		for (int i = 0; i<FiveEmployeeObj.Count; i++) 
		{
			FiveEmployeeObj[i].SetActive(true);
			FiveEmployeeObj[i].transform.parent = fiveEmployeeList[i].transform.Find("modPos");
			FiveEmployeeObj[i].transform.localScale = new Vector3(320f,320f,320f);
			FiveEmployeeObj[i].transform.localPosition =  Vector3.forward * 100f;
			FiveEmployeeObj[i].transform.localRotation = Quaternion.Euler (0f, 180f, 0f);
			EffectLevel el = FiveEmployeeObj[i].AddComponent<EffectLevel>();
			el.target = FiveEmployeeObj[i].transform.parent.parent.GetComponent<UISprite>();
		}
	}


	public string FormatTime(int time)
	{
		int min = time/60;
		int second = time%60;
		return DoubleTime(min) + ":" + DoubleTime(second);
	}

	public  string FormatTimeHasHour(int time)
	{
		int hour = time/3600;
		int min = (time%3600)/60;
		int second = time%60;
		return DoubleTime(hour) + ":" + DoubleTime(min) + ":" + DoubleTime(second);
	}

	
	public string DoubleTime(int time)
	{
		return (time > 9)?time.ToString():("0" + time);
	}



	public void showBuyBtn(bool enabled)
	{
		higherBtn.isEnabled = enabled;
		topBtn.isEnabled = enabled;
		PerfectBtn.isEnabled = enabled;
	}

	public void hide()
	{

	}


	private GameObject GetEffetObj(int quality)
	{
		GameObject effObj = null;
		if ((int)quality >= (int)QualityColor.QC_Orange2)
		{
			GameObject tx = GameObject.Instantiate(efftFen.gameObject) as GameObject;
		} 
		if((int)quality <= (int)QualityColor.QC_White)
		{
		}
		else if ((int)quality <= (int)QualityColor.QC_Green)
		{
		}
		else if((int)quality <= (int)QualityColor.QC_Blue1)
		{
		}
		else if ((int)quality <= (int)QualityColor.QC_Purple2)
		{
			effObj = GameObject.Instantiate(efftZi.gameObject) as GameObject;
		}
		else if ((int)quality <= (int)QualityColor.QC_Golden2)
		{
			effObj = GameObject.Instantiate(efftJin.gameObject) as GameObject;
		}
		else if ((int)quality <= (int)QualityColor.QC_Orange2)
		{
			effObj = GameObject.Instantiate(efftCheng.gameObject) as GameObject;
		}
		else if ((int)quality <= (int)QualityColor.QC_Pink)
		{
			effObj = GameObject.Instantiate(efftFen.gameObject) as GameObject;
		}
		if(effObj != null)
		{
			effObj.name = "effct";
		}
		return effObj;
	}
	void OnUpdateItem(COM_Item inst)
	{
		haveItemNum.text  = BagSystem.instance.GetItemMaxNum((uint)drawNeedItem_).ToString();
	}

    void OnDestroy()
    {
        if (effectImg != null)
            GameObject.Destroy(effectImg);
		BagSystem.instance.UpdateItemEvent -= OnUpdateItem;
		BagSystem.instance.DelItemInstEvent -= OnUpdateItem;
		GamePlayer.Instance.DrawEmployeeEnvent -= OnDrawEmployessOk;
		if(isLoadBack)
		{
			HeadIconLoader.Instance.Delete("renwu_quanshen");
			HeadIconLoader.Instance.Delete("jiubaxintu");
			HeadIconLoader.Instance.LoadIcon("renwu_quanshen", backImgRole);
		}

    }
}
