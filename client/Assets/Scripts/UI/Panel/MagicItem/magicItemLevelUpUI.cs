using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class magicItemLevelUpUI : MonoBehaviour
{
	public UIButton levelupBtn;
	public UIButton zhuanhuanBtn;
	public UILabel needMoneyLab;
	public UIGrid grid;
	public UISprite icon;
	public UITexture iconImg;
	public UILabel nameLab;
	public UILabel levelLab;
	public UILabel barLevelLab;
	public UILabel barExpLab;
	public GameObject itemCell;
	public UILabel addExpLab;
	public UILabel addlevelLab;

	public UIGrid propGrid;
	public GameObject propCell;

	public GameObject itemsObj;
	public GameObject zhanhuanObj;
	public UILabel needZhuanMoneyLab;
	public UILabel selfJobLab;
	public UISprite selfJobImg;
	public UISlider expBar;
	public UISprite selectAllItemBtn;
	public UISprite cancelAllItemBtn;
	public UISprite suolianImg;
	public GameObject blackImg;
	public UIButton oneKeyBtn;

	public List<MagicChangeCellUI> changeCellList = new List<MagicChangeCellUI> ();
	public List<GameObject> effectObjlist = new List<GameObject> ();

	private List<GameObject> CellList = new List<GameObject> ();
	private List<GameObject> CellPool = new List<GameObject> ();

	private List<GameObject> propCellList = new List<GameObject>();
	private List<GameObject> propcellPool = new List<GameObject> ();

	private List<uint> expItems = new List<uint>();

	private ArtifactChangeData _seletData;

	private ItemCellUI iconCell;
	void Start ()
	{
		UIManager.SetButtonEventHandler (levelupBtn.gameObject, EnumButtonEvent.OnClick, OnClickLevelUp, 0, 0);
		UIManager.SetButtonEventHandler (oneKeyBtn.gameObject, EnumButtonEvent.OnClick, OnClickOneKey, 0, 0);
		UIManager.SetButtonEventHandler (zhuanhuanBtn.gameObject, EnumButtonEvent.OnClick, OnClickZhuan, 0, 0);

		UIManager.SetButtonEventHandler (cancelAllItemBtn.gameObject, EnumButtonEvent.OnClick, OnClickSelectAll, 0, 0);
		UIManager.SetButtonEventHandler (selectAllItemBtn.gameObject, EnumButtonEvent.OnClick, OnClickCancelAll, 1, 0);

		for(int i=0;i<changeCellList.Count;i++)
		{
			UIManager.SetButtonEventHandler (changeCellList[i].gameObject, EnumButtonEvent.OnClick, OnClickMagicCell, 0, 0);
		}

		GuideManager.Instance.RegistGuideAim(levelupBtn.gameObject, GuideAimType.GAT_MainMagicLevelBtn);

		//	magicItemInfo ();
	}

	public void magicItemInfo(int level,int job)
	{
		ArtifactLevelData data = ArtifactLevelData.GetData (level, job);
		if (data == null)
			return;

		ArtifactLevelData nowdata = ArtifactLevelData.GetData (GamePlayer.Instance.MagicItemLevel,(int)JobType.JT_Axe);
		if (nowdata == null)
			return;
		//if(iconCell == null)
		//{
		//iconCell = UIManager.Instance.AddItemCellUI (icon,(uint)data.itemId_);
		//	iconCell.showTips = true;

		if(GamePlayer.Instance.MagicItemLevel >= GamePlayer.Instance.MagicTupoLevel)
		{
			suolianImg.gameObject.SetActive(true);
			blackImg.gameObject.SetActive(true);
			levelupBtn.gameObject.SetActive(false);
			oneKeyBtn.gameObject.SetActive(false);
			UIManager.Instance.AdjustUIDepth(blackImg.transform);
		}
		else
		{
			suolianImg.gameObject.SetActive(false);
			blackImg.gameObject.SetActive(false);
			levelupBtn.gameObject.SetActive(true);
			oneKeyBtn.gameObject.SetActive(true);
		}
		//	}
		//else
		//{
		//iconCell.itemId = (uint)data.itemId_;
		//	}
		//icon.spriteName = BagSystem.instance.GetQualityBack((int)ItemData.GetData((int)data._ItemId).quality_);
		//	HeadIconLoader.Instance.LoadIcon(data._Icon,iconImg);
		ItemData item = ItemData.GetData (data._ItemId);
		nameLab.text =LanguageManager.instance.GetValue("MagicTitleLab") ;
		barExpLab.text = GamePlayer.Instance.MagicItemExp + "/" + data._Exp;
		expBar.value = (float)GamePlayer.Instance.MagicItemExp/(float)data._Exp ;
		levelLab.text = LanguageManager.instance.GetValue("mainbaby_Level")+": "+ GamePlayer.Instance.MagicItemLevel;
		barLevelLab.text = "Lv: "+ GamePlayer.Instance.MagicItemLevel;

		foreach(GameObject x in effectObjlist)
		{
			x.SetActive(false);
		}

		if(GamePlayer.Instance.MagicItemLevel/5 > 0)
		{
			if(GamePlayer.Instance.MagicItemLevel ==30)
			{
				effectObjlist [4].SetActive (true);
			}
			else
			{
				effectObjlist [GamePlayer.Instance.MagicItemLevel/5 -1].SetActive (true);
			}
		}

		foreach( GameObject o in propCellList)
		{
			grid.RemoveChild(o.transform);
			o.transform.parent = null;
			o.gameObject.SetActive(false);
			propcellPool.Add(o);
		}
		propCellList.Clear ();



		float propAddNum = (float)(((int)GamePlayer.Instance.MagicItemLevel/5)*0.1f);
		if(GamePlayer.Instance.MagicItemLevel == GamePlayer.Instance.MagicTupoLevel)
		{
			propAddNum = (float)(((int)(GamePlayer.Instance.MagicItemLevel-5)/5)*0.1f);
		}

		//float propAddNum = (float)(((int)GamePlayer.Instance.MagicTupoLevel/5)*0.1f);
		/*{
				if(level == 10)
				{
					propAddNum = 0.5f;
				}
				else if(level == 20)
				{
					propAddNum = 0.75f;
				}
				else if(level == 30)
				{
					propAddNum = 1f;
				} 
			}
			*/

		for(int i =0;i<data.propArr.Count;i++)
		{

			GameObject objCell ;
			if(propcellPool.Count>0)
			{
				objCell = propcellPool[0];
				propcellPool.Remove(objCell);  
				UIManager.RemoveButtonAllEventHandler(objCell);
			}
			else  
			{
				objCell = Object.Instantiate(propCell) as GameObject;
			}

			UILabel lable =	objCell.transform.FindChild("name").GetComponent<UILabel>(); 

			string nameStr = LanguageManager.instance.GetValue(nowdata.propArr[i].Key.ToString());

			int nowDataStr = (int)(float.Parse(nowdata.propArr[i].Value)) + (int)(float.Parse(nowdata.propArr[i].Value) * propAddNum);



			int DataStr =  (int)(float.Parse(data.propArr[i].Value)) + (int)(float.Parse(data.propArr[i].Value) * propAddNum);

			int num = DataStr - nowDataStr;
			if(num >0)
			{
				lable.text  = "[352f30]" + nameStr+" +"+nowDataStr +"[39b31d] +"+num;
			}
			else
			{
				lable.text  = "[352f30]" + nameStr+" +"+nowDataStr;
			}

			objCell.transform.parent = propGrid.transform;
			objCell.gameObject.SetActive(true);	
			objCell.transform.localScale = Vector3.one;
			propCellList.Add(objCell);
		}
		propGrid.Reposition ();
	}

	public void UpdataItems()
	{
		zhanhuanObj.gameObject.SetActive (false);
		itemsObj.gameObject.SetActive (true);	

		COM_Item[] bags = BagSystem.instance.BagItems;
		for(int i =0;i<CellList.Count;i++)
		{
			CellList[i].transform.parent = null;
			CellList[i].gameObject.SetActive(false);
			CellList[i].GetComponent<BagCellUI>().pane.gameObject.transform.Find ("Sprite").gameObject.SetActive (false);
			CellPool.Add(CellList[i]);
		}
		CellList.Clear ();
		selectAllItemBtn.gameObject.SetActive (false);
		cancelAllItemBtn.gameObject.SetActive (true);
		bool procOnce = false;
		for(int i =0;i<bags.Length;i++)
		{
			if(bags[i] == null)
			{
				continue;
			}
			ItemData iData = ItemData.GetData((int)bags[i].itemId_);
			if((iData.mainType_ != ItemMainType.IMT_Equip && iData.mainType_ != ItemMainType.IMT_EmployeeEquip) || iData.artifactExp_ == 0)
			{
				continue;
			}

			GameObject obj;
			if(CellPool.Count > 0)
			{
				obj = CellPool[0];
				CellPool.Remove(obj);
			}
			else
			{
				obj = Object.Instantiate(itemCell.gameObject) as GameObject;
			}

			BagCellUI bCell  = obj.GetComponent<BagCellUI>();
			bCell.Item = bags[i];
			ItemData idata = ItemData.GetData((int)bags[i].itemId_);
			if(idata == null)
				bCell.suoImg.gameObject.SetActive(false);
			if(idata.mainType_ == ItemMainType.IMT_Equip)
			{
				bCell.suoImg.gameObject.SetActive(true);
				bCell.suoImg.spriteName ="sq_zhuangbei";
			}
			else if(idata.mainType_ == ItemMainType.IMT_EmployeeEquip)
			{
				bCell.suoImg.gameObject.SetActive(true);
				bCell.suoImg.spriteName ="sq_huoban";
			}
			else
			{
				bCell.suoImg.gameObject.SetActive(false);
			}
			grid.AddChild(obj.transform);
			UIManager.SetButtonEventHandler (obj.GetComponent<BagCellUI>().pane.gameObject, EnumButtonEvent.OnClick, OnClickCell, 0, 0);

			obj.SetActive(true);
			obj.transform.localScale = Vector3.one;
			CellList.Add(obj);

			if (CellList.Count == 1 && procOnce == false)
			{
				GuideManager.Instance.RegistGuideAim(obj, GuideAimType.GAT_MainMagicLevelFirst);
				procOnce = true;
			}
		}
		grid.Reposition ();
		magicItemInfo (GamePlayer.Instance.MagicItemLevel,(int)JobType.JT_Axe);//GamePlayer.Instance.MagicItemJob);
		needMoneyLab.text = "0";
		expItems.Clear ();
		if((GamePlayer.Instance.MagicItemLevel+1) % 10 == 0 )
		{

		}
	}

	public void updateZhaunhuan()
	{
		//zhanhuanObj.gameObject.SetActive (true);
		itemsObj.gameObject.SetActive (false);

		List<ArtifactChangeData> aList = new List<ArtifactChangeData> ();
		for(int i=0;i<ArtifactChangeData.metaData.Count;i++)
		{
			if(ArtifactChangeData.metaData[i]._JobType !=JobType.JT_Axe)//GamePlayer.Instance.MagicItemJob)
			{
				aList.Add(ArtifactChangeData.metaData[i]);
			}
		}

		for(int j =0;j<aList.Count;j++ )
		{
			changeCellList[j].ArtifactData = aList[j];
		}

		for(int i=0;i<changeCellList.Count;i++)
		{
			changeCellList[i].selectImg.gameObject.SetActive(false);
		}
		_seletData = null;

		selfJobLab.text = LanguageManager.instance.GetValue("JT_Axe");//GamePlayer.Instance.MagicItemJob.ToString ());
		selfJobImg.spriteName = "JT_Axe"; 

		ArtifactLevelData data = ArtifactLevelData.GetData (GamePlayer.Instance.MagicItemLevel, (int)JobType.JT_Axe);//GamePlayer.Instance.MagicItemJob);

		//icon.spriteName = BagSystem.instance.GetQualityBack((int)ItemData.GetData((int)data._ItemId).quality_);
		//HeadIconLoader.Instance.LoadIcon(data._Icon,iconImg);
		ItemData item = ItemData.GetData (data._ItemId);
		nameLab.text = item.name_;
		//barExpLab.text = GamePlayer.Instance.MagicItemExp + "/" + data.exp_;
		//expBar.value = (float)GamePlayer.Instance.MagicItemExp/(float)data.exp_ ;
		//levelLab.text = "Lv: "+ GamePlayer.Instance.MagicItemLevel;
		//barLevelLab.text = "Lv: "+ GamePlayer.Instance.MagicItemLevel;

		foreach(GameObject x in effectObjlist)
		{
			x.SetActive(false);
		}
		if(GamePlayer.Instance.MagicItemLevel/10 > 0)
			effectObjlist [GamePlayer.Instance.MagicItemLevel/10-1].SetActive (true);
	}

	private void OnClickCell(ButtonScript obj, object args, int param1, int param2)
	{

		if(GamePlayer.Instance.MagicItemLevel>=30)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("magictupomax"));
			return;
		}
		if(GamePlayer.Instance.MagicItemLevel >= GamePlayer.Instance.MagicTupoLevel )
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("qingtupo"));
			return;	
		}

		if(GamePlayer.Instance.MagicItemLevel > GamePlayer.Instance.GetIprop(PropertyType.PT_Level) )
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("bukeshengjilevel"));
			return;
		}

		BagCellUI bCell = obj.GetComponentInParent<BagCellUI> ();
		if(bCell.Item == null)
		{
			return;
		}


		if(expItems.Contains(bCell.Item.instId_))
		{
			obj.transform.Find ("Sprite").gameObject.SetActive (false);
			expItems.Remove(bCell.Item.instId_);
		}
		else
		{
			obj.transform.Find ("Sprite").gameObject.SetActive (true);
			expItems.Add (bCell.Item.instId_);
		}

		int addExp = 0;
		for(int i =0;i<expItems.Count;i++)
		{
			addExp += ItemData.GetData((int)BagSystem.instance.GetItemByInstId((int)expItems[i]).itemId_).artifactExp_;
		}
		needMoneyLab.text = (addExp * 100).ToString ();
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Money)<addExp * 100)
		{
			needMoneyLab.color = Color.red;
		}
		else
		{
			needMoneyLab.color = Color.gray;
		}
		if(addExp <= 0)
		{
			barLevelLab.gameObject.SetActive(true);
			barExpLab.gameObject.SetActive(true);
			magicItemLevelUp (GamePlayer.Instance.MagicItemLevel,addExp + GamePlayer.Instance.MagicItemExp);
			addlevelLab.text = "";
			addExpLab.text = "";
		}
		else
		{
			barLevelLab.gameObject.SetActive(false);
			barExpLab.gameObject.SetActive(false);
			magicItemLevelUp (GamePlayer.Instance.MagicItemLevel,addExp + GamePlayer.Instance.MagicItemExp);
		}

		GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainMagicFirstClick);
	}

	private void selectAllItem( bool isSelect)
	{
		if(isSelect)
		{
			if(GamePlayer.Instance.MagicItemLevel>=30)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("magictupomax"));
				return;
			}
			if(GamePlayer.Instance.MagicItemLevel >= GamePlayer.Instance.MagicTupoLevel )
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("qingtupo"));
				return;	
			}


			if(GamePlayer.Instance.MagicItemLevel > GamePlayer.Instance.GetIprop(PropertyType.PT_Level) )
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("bukeshengjilevel"));
				return;
			}
		}

		expItems.Clear ();
		for(int i= 0;i<CellList.Count;i++)
		{
			BagCellUI bCell = CellList[i].GetComponentInParent<BagCellUI> ();
			if(bCell == null)
				continue;
			if(isSelect)
			{
				bCell.pane.gameObject.transform.Find ("Sprite").gameObject.SetActive (true);
				expItems.Add (bCell.Item.instId_);
			}
			else
			{
				bCell.pane.gameObject.transform.Find ("Sprite").gameObject.SetActive (false);
				expItems.Remove (bCell.Item.instId_);
			}
		}

		int addExp = 0;
		for(int i =0;i<expItems.Count;i++)
		{
			addExp += ItemData.GetData((int)BagSystem.instance.GetItemByInstId((int)expItems[i]).itemId_).artifactExp_;
		}
		needMoneyLab.text = (addExp * 100).ToString ();
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Money)<addExp * 100)
		{
			needMoneyLab.color = Color.red;
		}
		else
		{
			needMoneyLab.color = Color.gray;
		}
		if(addExp <= 0)
		{
			barLevelLab.gameObject.SetActive(true);
			barExpLab.gameObject.SetActive(true);
			magicItemLevelUp (GamePlayer.Instance.MagicItemLevel,addExp + GamePlayer.Instance.MagicItemExp);
			addlevelLab.text = "";
			addExpLab.text = "";
		}
		else
		{
			barLevelLab.gameObject.SetActive(false);
			barExpLab.gameObject.SetActive(false);
			magicItemLevelUp (GamePlayer.Instance.MagicItemLevel,addExp + GamePlayer.Instance.MagicItemExp);
		}
	}



	private void magicItemLevelUp(int level,int exp)
	{
		if(level > GamePlayer.Instance.GetIprop(PropertyType.PT_Level))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("bukeshengjilevel"));
			levelupBtn.gameObject.SetActive(false);
			return;
		}
		levelupBtn.gameObject.SetActive(true);
		ArtifactLevelData data = ArtifactLevelData.GetData (level, (int)JobType.JT_Axe);//GamePlayer.Instance.MagicItemJob);
		if(exp >= data._Exp )
		{

			if(level >= GamePlayer.Instance.MagicTupoLevel)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("qingtupo"));
				levelupBtn.gameObject.SetActive(false);
				addlevelLab.text = "LV:"  + level.ToString();
				addExpLab.text = exp+"/"+data._Exp;
				expBar.value = (float)exp/data._Exp ;
				//expBar.value = (float)GamePlayer.Instance.MagicItemExp/(float)data._Exp ;
				return;

			}
			level++;
			magicItemInfo (level,(int)JobType.JT_Axe);//GamePlayer.Instance.MagicItemJob);
			exp -=data._Exp ;
			addlevelLab.text = "LV:"  + level.ToString();
			addExpLab.text = exp+"/"+data._Exp;
			expBar.value = (float)exp/data._Exp ;
			magicItemLevelUp(level,exp);

		}
		else
		{
			magicItemInfo (level,(int)JobType.JT_Axe);//GamePlayer.Instance.MagicItemJob);
			exp = exp;
			addlevelLab.text = "LV:" + level ;
			addExpLab.text = exp+"/"+data._Exp;
			expBar.value = (float)exp/data._Exp ;
			return;
		}

	}

	private void OnClickOneKey(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.MagicItemLevel >= GamePlayer.Instance.MagicTupoLevel)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("qingtupo"));
			return;
		}
		ArtifactLevelData nowdata = ArtifactLevelData.GetData (GamePlayer.Instance.MagicItemLevel,(int)JobType.JT_Axe);
		if (nowdata == null)
			return;

		ArtifactLevelData data = ArtifactLevelData.GetData (GamePlayer.Instance.MagicItemLevel, (int)JobType.JT_Axe);
		if (data == null)
			return;

		int needExp = data._Exp - GamePlayer.Instance.MagicItemExp;

		if(GamePlayer.Instance.GetIprop(PropertyType.PT_MagicCurrency) >= needExp)
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("xiaohaoshuijingjixu").Replace("{n}",needExp.ToString()),
				()=>{NetConnection.Instance.magicItemOneKeyLevel();
					needMoneyLab.text = "0";
					addlevelLab.text = "";
					addExpLab.text = "";
					barLevelLab.gameObject.SetActive(true);
					barExpLab.gameObject.SetActive(true);});
		}
		else
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("notEnoughMagicCurrency"),()=>{StoreUI.SwithShowMe(2);});
		}
	}

	private void OnClickLevelUp(ButtonScript obj, object args, int param1, int param2)
	{
		if (expItems.Count <= 0)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("qingxuanze"));
			return;
		}
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Money) < int.Parse(needMoneyLab.text))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("nomoney"));
			return;
		}

		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue ("xiaohaoshengji"), () => {

			NetConnection.Instance.levelUpMagicItem (expItems.ToArray ());
			expItems.Clear ();
			needMoneyLab.text = "0";
			addlevelLab.text = "";
			addExpLab.text = "";
			barLevelLab.gameObject.SetActive(true);
			barExpLab.gameObject.SetActive(true);

		});
	}

	private void OnClickZhuan(ButtonScript obj, object args, int param1, int param2)
	{
		if(_seletData == null)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("selectZhaunhuan"));
			return;
		}

		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Diamond) < _seletData._Diamonds)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("nodiamond"));
			return;
		}
		else
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("shifouzhuanhuan"),()=>{
				NetConnection.Instance.changeMagicJob (_seletData._JobType);
			});
		}

	}

	private void OnClickSelectAll(ButtonScript obj, object args, int param1, int param2)
	{
		if (CellList.Count <= 0)
			return;

		if( GamePlayer.Instance.MagicItemLevel>=30)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("magictupomax"));
			return;
		}
		if(GamePlayer.Instance.MagicItemLevel >= GamePlayer.Instance.MagicTupoLevel )
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("qingtupo"));
			return;	
		}


		if(GamePlayer.Instance.MagicItemLevel > GamePlayer.Instance.GetIprop(PropertyType.PT_Level) )
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("bukeshengjilevel"));
			return;
		}

		selectAllItem (true);
		selectAllItemBtn.gameObject.SetActive (true);
		cancelAllItemBtn.gameObject.SetActive (false);
	}
	private void OnClickCancelAll(ButtonScript obj, object args, int param1, int param2)
	{
		selectAllItem (false);
		selectAllItemBtn.gameObject.SetActive (false);
		cancelAllItemBtn.gameObject.SetActive (true);
	}


	private void OnClickMagicCell(ButtonScript obj, object args, int param1, int param2)
	{
		for(int i=0;i<changeCellList.Count;i++)
		{
			changeCellList[i].selectImg.gameObject.SetActive(false);
		}
		obj.GetComponent<MagicChangeCellUI> ().selectImg.gameObject.SetActive (true);
		_seletData = obj.GetComponent<MagicChangeCellUI>().ArtifactData;
		needZhuanMoneyLab.text = obj.GetComponent<MagicChangeCellUI> ().ArtifactData._Diamonds.ToString ();
	}

	void OnDestroy()
	{
		for(int i= 0;i<CellPool.Count;i++)
		{
			GameObject.DestroyObject(CellPool[i]);
			CellPool[i] = null;
		}
		CellPool.Clear ();
	}
}