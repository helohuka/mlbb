using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MainbabyReformUI : MonoBehaviour {




	public UILabel _PhysicalLable;
	public UILabel _PowerLable;
	public UILabel _StrengthLable;
	public UILabel _SpeedLable;
	public UILabel _MagicLable;


	public UILabel _PhysicalLable1;
	public UILabel _PowerLable1;
	public UILabel _StrengthLable1;
	public UILabel _SpeedLable1;
	public UILabel _MagicLable1;
	public UILabel _GZLable;
	public UILabel _DesLable;

	public delegate void ShowbabyInfo (Baby baby,string ReformItem,int ReformMonster);
	public static ShowbabyInfo ShowbabyInfoOk;

	public delegate void RefreshbabyInfo ( int babyId);
	public static RefreshbabyInfo RefreshbabyInfoOk;

	public GameObject iconObj;
	public GameObject oldiconObj;

	public UILabel oldtiliLabel;
	public UILabel oldliliangLabel;
	public UILabel oldqiangduLabel;
	public UILabel oldsuduLabel;
	public UILabel oldmofaLabel;
	public UILabel oldnameLabel;
	public UITexture oldicont;
	public UITexture oldRaceicon;
	public UITexture item01t;
	public UITexture item02t;
	public UITexture item03t;
	public UILabel item01Label;
	public UILabel item02Label;
	public UILabel item03Label;
	public UILabel item01numLabel;
	public UILabel item02numLabel;
	public UILabel item03numLabel;

	public GameObject item;
	public UIGrid grid;

	public UILabel _NPhysicalLable1;
	public UILabel _NPowerLable1;
	public UILabel _NStrengthLable1;
	public UILabel _NSpeedLable1;
	public UILabel _NMagicLable1;

	public UILabel numberLabel;

	public UIButton selectBtn;
	public UIButton enterBtn;

	public UILabel newtiliLabel;
	public UILabel newliliangLabel;
	public UILabel newqiangduLabel;
	public UILabel newsuduLabel;
	public UILabel newmofaLabel;
	public UILabel newnameLabel;
	public UITexture newicont;
	public UITexture newraceicont;
	public GameObject listbabyObj;
	public static bool isMainbabyReformUI;
	public UISprite item01;
	public UISprite item02;
	public UISprite item03;
	private int item1Num;
	private int item2Num;
	private int item3Num;
	private int insetId;
	private int newBabyId;
	private int oldBabyId;
	void Start () {
		item.SetActive (false);
		InitUIText ();
		UIManager.SetButtonEventHandler (selectBtn.gameObject, EnumButtonEvent.OnClick, OnClickselect, 0, 0);
		UIManager.SetButtonEventHandler (enterBtn.gameObject, EnumButtonEvent.OnClick, OnClickenter, 0, 0);
		UIManager.SetButtonEventHandler (iconObj, EnumButtonEvent.OnClick, OnClickshowTip, 0, 0);
		UIManager.SetButtonEventHandler (oldiconObj, EnumButtonEvent.OnClick, OnClickshowoldTip, 0, 0);
		//ShowbabyInfoOk = RefreshBabyData;
		RefreshbabyInfoOk = showNewBaby;
		//MainbabyUI.UpdateBabyInfoOk = UpdeateItem;
//		Baby inset = GamePlayer.Instance.GetBabyInst (MainbabyProperty.idss [0]);
		//		enterBtn
		//BagSystem.instance.ItemChanged += UpdeateItem;
		//BagSystem.instance.UpdateItemEvent += updateUI;
	//	BagSystem.instance.DelItemEvent += UpdeateItem;
		//enterBtn.isEnabled = false;
		if (GlobalValue.isBattleScene(StageMgr.Scene_name))
		{
			enterBtn.gameObject.SetActive(false);
		}
	}
	private void OnClickshowTip(ButtonScript obj, object args, int param1, int param2)
	{
		TuJianUI.babyId = newBabyId;
		BabyInfo.ShowMe ();
	}
	private void OnClickshowoldTip(ButtonScript obj, object args, int param1, int param2)
	{
		TuJianUI.babyId = oldBabyId;
		BabyInfo.ShowMe ();
	}
	void InitUIText()
	{

		_PhysicalLable.text = LanguageManager.instance.GetValue("playerPro_Physical");
		_PowerLable.text = LanguageManager.instance.GetValue("playerPro_Power");
		_StrengthLable.text = LanguageManager.instance.GetValue("playerPro_Strength");
		_SpeedLable.text = LanguageManager.instance.GetValue("playerPro_Speed");
		_MagicLable.text = LanguageManager.instance.GetValue("playerPro_Magic");


		_PhysicalLable1.text = LanguageManager.instance.GetValue("playerPro_Physical");
		_PowerLable1.text = LanguageManager.instance.GetValue("playerPro_Power");
		_StrengthLable1.text = LanguageManager.instance.GetValue("playerPro_Strength");
		_SpeedLable1.text = LanguageManager.instance.GetValue("playerPro_Speed");
		_MagicLable1.text = LanguageManager.instance.GetValue("playerPro_Magic");

		_GZLable.text = LanguageManager.instance.GetValue ("baby_GZ");
		_DesLable.text = LanguageManager.instance.GetValue ("baby_desl");
	}

	int _babyid;
	void RefreshBabyData(BabyData babyd,string ReformItem,int ReformMonster)
	{
		 
		_babyid = babyd._Id;
		oldnameLabel.text = babyd._Name;
		HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData(babyd._AssetsID).assetsIocn_, oldicont);
		HeadIconLoader.Instance.LoadIcon ( babyd._RaceIcon, oldRaceicon);
		oldmofaLabel.text ="[-][666666]"+ babyd._BIG_Magic.ToString();
		oldBabyId = babyd._Id;

		oldtiliLabel.text = "[-][666666]" + babyd._BIG_Stama.ToString ();
		oldsuduLabel.text ="[-][666666]"+ babyd._BIG_Speed.ToString();
		oldqiangduLabel.text ="[-][666666]"+ babyd._BIG_Power.ToString();
		oldliliangLabel.text ="[-][666666]"+babyd._BIG_Strength.ToString();
		string [] items = ReformItem.Split (';');

		ItemCellUI itemC01 =  UIManager.Instance.AddItemCellUI (item01, uint.Parse (items [0]));
		itemC01.showTips = true;
		ItemCellUI itemC02 = UIManager.Instance.AddItemCellUI (item02, uint.Parse (items [1]));
		itemC02.showTips = true;
		ItemCellUI itemC03 =UIManager.Instance.AddItemCellUI (item03, uint.Parse (items [2]));
		itemC03.showTips = true;

//		HeadIconLoader.Instance.LoadIcon ( ItemData.GetData(int.Parse(items[0])).icon_, item01t);
//		HeadIconLoader.Instance.LoadIcon ( ItemData.GetData(int.Parse(items[1])).icon_, item02t);
//		HeadIconLoader.Instance.LoadIcon ( ItemData.GetData(int.Parse(items[2])).icon_, item03t);
		item01Label.text = ItemData.GetData (int.Parse (items [0])).name_;
		item02Label.text = ItemData.GetData (int.Parse (items [1])).name_;
		item03Label.text = ItemData.GetData (int.Parse (items [2])).name_;
		item1Num = BagSystem.instance.GetItemMaxNum (uint.Parse (items [0]));
		item2Num = BagSystem.instance.GetItemMaxNum (uint.Parse (items [1]));
		item3Num = BagSystem.instance.GetItemMaxNum (uint.Parse (items [2]));
		item01numLabel.text = item1Num + "/1";
		item02numLabel.text = item2Num + "/1";
		item03numLabel.text = item3Num + "/1";
		BabyData bdatas = BabyData.GetData (ReformMonster);
		//_babyid = ReformMonster;
		newmofaLabel.text ="[-][666666]"+ bdatas._BIG_Magic.ToString();
		newtiliLabel.text ="[-][666666]"+ bdatas._BIG_Stama.ToString();
		newsuduLabel.text ="[-][666666]"+ bdatas._BIG_Speed.ToString();
		newqiangduLabel.text ="[-][666666]"+bdatas._BIG_Power.ToString();
		newliliangLabel.text ="[-][666666]"+ bdatas._BIG_Strength.ToString();

		_NPhysicalLable1.text = "";
		_NPowerLable1.text = "";
		_NStrengthLable1.text = "";
		_NSpeedLable1.text = "";
		_NMagicLable1.text = "";

		numberLabel.text = GetMyBabyNumber (babyd._Id) + "/1";
		insetId = GetMyBabyInseid (babyd._Id);
		newBabyId = bdatas._Id;
		newnameLabel.text = bdatas._Name;
		HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData(BabyData.GetData(bdatas._Id)._AssetsID).assetsIocn_, newicont);
		HeadIconLoader.Instance.LoadIcon ( BabyData.GetData(bdatas._Id)._RaceIcon, newraceicont);
		if(insetId!=0&&item1Num!=0&&item2Num !=0&&item3Num !=0&&GetMyBabyNumber (babyd._Id)>0)
		{
			enterBtn.isEnabled = true;
		}else
		{
			enterBtn.isEnabled = false;
		}
	}

	int GetMyBabyNumber(int inId)
	{
		int number = 0;
		for(int i=0;i<GamePlayer.Instance.babies_list_.Count;i++)
		{
			BabyData ba = BabyData.GetData(GamePlayer.Instance.babies_list_[i].GetIprop(PropertyType.PT_TableId));
			if(inId == ba._Id)
			{
				number++;
			}
		}
		return number;
	}

	int GetMyBabyInseid(int inId)
	{

		for(int i=0;i<GamePlayer.Instance.babies_list_.Count;i++)
		{
			BabyData ba = BabyData.GetData(GamePlayer.Instance.babies_list_[i].GetIprop(PropertyType.PT_TableId));
			if(inId == ba._Id)
			{
				return GamePlayer.Instance.babies_list_[i].InstId;
			}
		}
		return 0;
	}



	void OnEnable()
	{
		MainbabyReform ();
		if (GlobalValue.isBattleScene(StageMgr.Scene_name))
		{
			enterBtn.gameObject.SetActive(false);
		}
	}
	void MainbabyReform()
	{


		List<BabyData> babys = new List<BabyData> ();
		foreach(KeyValuePair <int, BabyData> Pair in BabyData.GetData ())
		{
			if(!Pair.Value._ReformItem.Equals(""))
			{
				babys.Add(Pair.Value);
			}
		}

		addItem (babys);
	}


	void addItem(List<BabyData> babys)
	{
		Refresh ();
		for(int i=0;i<babys.Count;i++)
		{
			GameObject o = GameObject.Instantiate(item)as GameObject;
			o.SetActive(true);
			o.name = o.name+i;
			o.transform.parent = grid.transform;
			MainbabyReductionCell mbCell = o.GetComponent<MainbabyReductionCell>();
			mbCell._Babydata = babys[i];
			o.transform.localScale= new Vector3(1,1,1);	
			UIManager.SetButtonEventHandler (o, EnumButtonEvent.OnClick, buttonClick,babys[i]._Id,0);
			grid.repositionNow = true;
			
		}
		if(babys.Count>0)
		{
			string ReformItem = babys[0]._ReformItem;
			RefreshBabyData (babys[0],ReformItem,babys[0]._ReformMonster);
		}
	}
	private void buttonClick(ButtonScript obj, object args, int param1, int param2)
	{
		

		BabyData bab = BabyData.GetData (param1);
		string ReformItem = bab._ReformItem;
//		if(ReformItem == "")
//		{
//			
//			PopText.Instance.Show(LanguageManager.instance.GetValue("babygaizao1"));
//			
//		}else
//			if(inst.GetIprop(PropertyType.PT_Level)>1)
//		{
//			
//			PopText.Instance.Show(LanguageManager.instance.GetValue("babygaizao2"));
//		}else
//		{
			
//			if(MainbabyReformUI.ShowbabyInfoOk != null)
//			{
//				MainbabyReformUI.ShowbabyInfoOk(inst,ReformItem,bab._ReformMonster);
//			}
		RefreshBabyData (bab,ReformItem,bab._ReformMonster);
			//gameObject.SetActive(false);
//		}
		
		
	}
	void Refresh()
	{
		foreach(Transform tr in grid.transform)
		{
			Destroy(tr.gameObject);
		}
	}
	void ClearInfo()
	{
		oldnameLabel.text = "";
		oldicont.mainTexture = null;
		oldRaceicon.mainTexture = null;
		oldmofaLabel.text = "";
		oldtiliLabel.text = "";
		oldsuduLabel.text = "";
		oldqiangduLabel.text = "";
		oldliliangLabel.text = "";
		
		item01t.mainTexture = null;
		item02t.mainTexture = null;
		item03t.mainTexture = null;
		
		item01Label.text = "";
		item02Label.text = "";
		item03Label.text = "";
		item1Num =0;
		item2Num = 0;
		item3Num =0;
		item01numLabel.text = item1Num + "/1";
		item02numLabel.text = item2Num + "/1";
		item03numLabel.text = item3Num + "/1";
		
		
		newmofaLabel.text ="";
		newtiliLabel.text ="";
		newsuduLabel.text ="";
		newqiangduLabel.text ="";
		newliliangLabel.text ="";
		newnameLabel.text ="";
		newicont.mainTexture = null;
		newraceicont.mainTexture = null;
	}
	void showNewBaby(int babyid)
	{
		GetBabyPanelUI.ShowMe (babyid);
		//ClearInfo ();
		UpdeateItem ();
		//enterBtn.isEnabled = false;
	}
	void UpdeateItem()
	{
		BabyData bdatas = BabyData.GetData (_babyid);
		string [] items = bdatas._ReformItem.Split (';');
		item01Label.text = ItemData.GetData (int.Parse (items [0])).name_;
		item02Label.text = ItemData.GetData (int.Parse (items [1])).name_;
		item03Label.text = ItemData.GetData (int.Parse (items [2])).name_;
		item1Num = BagSystem.instance.GetItemMaxNum (uint.Parse (items [0]));
		item2Num = BagSystem.instance.GetItemMaxNum (uint.Parse (items [1]));
		item3Num = BagSystem.instance.GetItemMaxNum (uint.Parse (items [2]));
		item01numLabel.text = item1Num + "/1";
		item02numLabel.text = item2Num + "/1";
		item03numLabel.text = item3Num + "/1";
		numberLabel.text = GetMyBabyNumber (bdatas._Id) + "/1";
		insetId = GetMyBabyInseid (bdatas._Id);
		if(insetId!=0&&item1Num!=0&&item2Num !=0&&item3Num !=0&&GetMyBabyNumber (bdatas._Id)>0)
		{
			enterBtn.isEnabled = true;
		}else
		{
			enterBtn.isEnabled = false;
		}
	}

	private void OnClickselect(ButtonScript obj, object args, int param1, int param2)
	{
		listbabyObj.SetActive (true);
	}
	private void OnClickenter(ButtonScript obj, object args, int param1, int param2)
	{

			NetConnection.Instance.remouldBaby (insetId);

	}
	void OnDestroy()
	{
		ShowbabyInfoOk = null;
		RefreshbabyInfoOk = null;
		MainbabyUI.UpdateBabyInfoOk = null;
	}

}
