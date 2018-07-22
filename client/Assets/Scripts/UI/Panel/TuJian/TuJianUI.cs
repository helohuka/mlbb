using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TuJianUI : UIBase {

	public UILabel _TitleLable;
	public UILabel _HumanoidLable;
	public UILabel _BeastLable;
	public UILabel _PlantLable;
	public UILabel _ImmortalLable;
	public UILabel _InsectLable;
	public UILabel _MetalLable;
	public UILabel _DragonLable;
	public UILabel _FlightLable;
	public UILabel _SpecialLable;
	
	public GameObject item;
	public UIGrid grid;
	public static int babyId;
	public List<UIButton> btns = new List<UIButton>();
	private List<BabyData> datas = new List<BabyData>();
	public static TuJianUI TuJInstance;
	public UIButton closeBtn;
	public UIScrollBar scrBar;

	private List<GameObject> CellPool = new List<GameObject>();

	void Awake()
	{
		TuJInstance = this;
	}
	public static TuJianUI Instance
	{
		get
		{
			return TuJInstance;	
		}
	}
	
	void Start () {
		IinitUIText ();
		item.SetActive (false);
		foreach(KeyValuePair<int, BabyData> pair in BabyData.GetData())
		{
			if(pair.Value._Pet == 0)
				continue;
			datas.Add(pair.Value);
		}

		for(int i = 0;i<btns.Count;i++)
		{
			UIManager.SetButtonEventHandler (btns[i].gameObject, EnumButtonEvent.OnClick, OnClickTabsBtn, i, 0);
		}
//		if(selsetBtn != null)
//		UIManager.SetButtonEventHandler (selsetBtn.gameObject, EnumButtonEvent.OnClick, OnSelectBtn, 0, 0);  
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OncloseBtn, 0, 0); 

		TabsSelect (0);
		ShowTabsSelectInfo (0);
	}
	void IinitUIText()
	{
		_TitleLable.text = LanguageManager.instance.GetValue("Tujian_Title");
		_HumanoidLable.text = LanguageManager.instance.GetValue("Tujian_Humanoid");
		_BeastLable.text = LanguageManager.instance.GetValue("Tujian_Beast");
		_PlantLable.text = LanguageManager.instance.GetValue("Tujian_Plant");
		_ImmortalLable.text = LanguageManager.instance.GetValue("Tujian_Immortal");
		_InsectLable.text = LanguageManager.instance.GetValue("Tujian_Insect");
		_MetalLable.text = LanguageManager.instance.GetValue("Tujian_Metal");
		_DragonLable.text = LanguageManager.instance.GetValue("Tujian_Dragon");
		_FlightLable.text = LanguageManager.instance.GetValue("Tujian_Flight");
		_SpecialLable.text = LanguageManager.instance.GetValue("Tujian_Special");
	}
	void OncloseBtn(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	void OnClickTabsBtn(ButtonScript obj, object args, int param1, int param2)
	{
		TabsSelect (param1);
		ShowTabsSelectInfo (param1);
		//SelectType.gameObject.SetActive (false);
		//babyTypeLab.text = obj.transform.FindChild ("Label").GetComponent<UILabel> ().text;
	}

	private void OnSelectBtn(ButtonScript obj, object args, int param1, int param2)
	{
		//SelectType.gameObject.SetActive (true);
	}

	void TabsSelect (int index)
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
	void ShowTabsSelectInfo (int index)
	{
		List<BabyData> b = new List<BabyData> ();

		if(index ==0)
		{
			b = BabyListCell(RaceType.RT_Human);
		}else
			if(index ==1)
		{
			b = BabyListCell(RaceType.RT_Animal);
		}
		else
			if(index ==2)
		{
			b=BabyListCell(RaceType.RT_Plant);
		}
		else
			if(index ==3)
		{
			b = BabyListCell(RaceType.RT_Undead);
		}
		else
			if(index ==4)
		{
			b = BabyListCell(RaceType.RT_Insect);
		}
		else
			if(index ==5)
		{
			b = BabyListCell(RaceType.RT_Metal);
		}
		else
			if(index ==6)
		{
			b = BabyListCell(RaceType.RT_Dragon);
		}
		else
			if(index ==7)
		{
		    b = BabyListCell(RaceType.RT_Fly);
		}
		else
			if(index ==8)
		{
			b = BabyListCell(RaceType.RT_Extra);
		}
		RefreshItem (b);
	}
	// Update is called once per frame


	void RefreshItem(List<BabyData> bdts)
	{
		for(int i =0;i<CellPool.Count;i++)
		{
			CellPool[i].SetActive(false);
		}
		for(int i =0;i<bdts.Count;i++)
		{
			if(i<CellPool.Count)
			{
				CellPool[i].gameObject.SetActive(true);
				BabyListCell bCell = CellPool[i].GetComponent<BabyListCell>();
				bCell.Bdata = bdts[i];
				UIManager.SetButtonEventHandler (CellPool[i].gameObject, EnumButtonEvent.OnClick, OnClickExamine, bdts[i]._Id, 0);
			}else
			{
				GameObject clone = GameObject.Instantiate(item)as GameObject;
				clone.SetActive(true);
				clone.transform.parent = grid.transform;
				clone.transform.position = Vector3.zero;
				clone.transform.localScale = Vector3.one;
				UIManager.SetButtonEventHandler (clone.gameObject, EnumButtonEvent.OnClick, OnClickExamine, bdts[i]._Id, 0);
				BabyListCell bCell = clone.GetComponent<BabyListCell>();
				bCell.Bdata = bdts[i];
				bCell.backImg.spriteName = GetCellQualityBack((int)bdts[i]._PetQuality);
				CellPool.Add(clone);
			}
		}
		grid.Reposition();
		scrBar.value = 1;
	}
	void OnClickExamine(ButtonScript obj, object args, int param1, int param2)
	{
		TuJianUI.babyId = param1;
		TuJianUI.Instance.OpenBabyInfoObj ();
		//		BabyInfo binfo = TuJianUI.Instance.babyInfoObj.GetComponent<BabyInfo> ();
		//		binfo.Bdata = BabyData.GetData (param1);
		//		TuJianUI.Instance.babyListObj.SetActive (false);
	}
	void ClearGridItem()
	{
		foreach(Transform tra in grid.transform)
		{
			Destroy(tra.gameObject);
		}
	}

	List<BabyData> BabyListCell(RaceType rType)
	{
		List<BabyData> bds = new List<BabyData> ();
		for(int i =0;i<datas.Count;i++)
		{
			if(datas[i]._RaceType == rType)
			{
				bds.Add(datas[i]);
			}
		}
		return bds;
	}

	public static bool IsCaptureBaby(int babyId)
	{
		for(int i = 0;i<GamePlayer.Instance.babies_list_.Count;i++)
		{
			if(babyId == BabyData.GetData( GamePlayer.Instance.babies_list_[i].GetIprop(PropertyType.PT_TableId))._Id)
			{
			  return true;
			}
		}
		return false;
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_BabyTuJianPanel);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_BabyTuJianPanel);
	}
	public override void Destroyobj ()
	{
		if(MainbabyUI.UpdateTabelBtnStateOk != null)
		{
			MainbabyUI.UpdateTabelBtnStateOk();
		}

        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_BabyTuJianPanel, AssetLoader.EAssetType.ASSET_UI), true);
	}
	public void OpenBabyInfoObj()
	{
		BabyInfo.ShowMe ();
	}




	public string GetCellQualityBack(int quality)
	{
		if ((int)quality >= (int)PetQuality.PE_Orange)
		{
			return "hb_renwukuang_cheng";
		} 
		if((int)quality <= (int)PetQuality.PE_White)
		{
			return "hb_renwukuang_bai";
		}
		else if ((int)quality <= (int)PetQuality.PE_Green)
		{
			return "hb_renwukuang__lv";
		}
		else if((int)quality <= (int)PetQuality.PE_Blue)
		{
			return "hb_renwukuang_lan";
		}
		else if ((int)quality <= (int)PetQuality.PE_Purple)
		{
			return "hb_renwukuang_zi";
		}
		else if ((int)quality <= (int)PetQuality.PE_Golden)
		{
			return "hb_renwukuang_huang";
		}
		else if ((int)quality <= (int)PetQuality.PE_Orange)
		{
			return "hb_renwukuang_cheng";
		}
		else if ((int)quality <= (int)PetQuality.PE_Pink)
		{
			return "hb_renwukuang_fen";
		}
		return "";
	}
}
