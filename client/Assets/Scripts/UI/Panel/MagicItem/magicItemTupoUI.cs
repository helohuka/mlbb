using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class magicItemTupoUI : MonoBehaviour
{
	public UILabel nameLab;
	public UILabel levelLab;
	public UIButton tupoBtn;
	public UISprite blackImg;
	public UILabel needMaonyLab;
	public UILabel canTupoLab;
	public UIGrid nowPropGrid;
	public GameObject nowPropCell;
	public UIGrid beferPropGrid;
	public GameObject beferPropCell;
	public UISprite needMaonyImg;
	private List<GameObject> propCellList = new List<GameObject>();
	private List<GameObject> propcellPool = new List<GameObject> ();
	public UISprite suolianImg;
	public UIButton buyBtn;

	void Start ()
	{
		UIManager.SetButtonEventHandler (tupoBtn.gameObject, EnumButtonEvent.OnClick, OnClickTupo, 0, 0);
		UIManager.SetButtonEventHandler (buyBtn.gameObject, EnumButtonEvent.OnClick, OnClickBuy, 0, 0);

		BagSystem.instance.UpdateItemEvent += new ItemUpdateEventHandler (UpdateNeedItem);
		BagSystem.instance.ItemChanged += new ItemChangedEventHandler (UpdateNeedItem);


	}
	

	void UpdateNeedItem(COM_Item item1)
	{
		//UpdateMagicItem ();

		ArtifactConfigData configData = ArtifactConfigData.GetData (GamePlayer.Instance.MagicTupoLevel/5, (int)JobType.JT_Axe);
		COM_Item item = BagSystem.instance.GetItemByItemId ((uint)configData._ItemId_1);
		if(item == null)
		{
			needMaonyLab.text =  "0/" + configData._ItemNum_1;
			needMaonyLab.color = Color.red;
			tupoBtn.isEnabled = false;
		}
		else
		{
			int num = BagSystem.instance.GetItemMaxNum((uint)configData._ItemId_1);
			needMaonyLab.text = num + "/" + configData._ItemNum_1;
			
			if(num >= configData._ItemNum_1)
			{
				needMaonyLab.color = Color.grey;
				tupoBtn.isEnabled = true;
			}
			else
			{
				needMaonyLab.color = Color.red;
				tupoBtn.isEnabled = false;
			}
		}

	}
	public void UpdateMagicItem()
	{

		if(GamePlayer.Instance.MagicItemLevel >= GamePlayer.Instance.MagicTupoLevel)
		{
			suolianImg.gameObject.SetActive(true);
		}
		else
		{
			suolianImg.gameObject.SetActive(false);
		}

		nameLab.text = LanguageManager.instance.GetValue ("MagicTitleLab");
		levelLab.text = LanguageManager.instance.GetValue("mainbaby_Level")+": "+ GamePlayer.Instance.MagicItemLevel;
		if (GamePlayer.Instance.MagicTupoLevel > 30)
		{
			//canTupoLab.gameObject.SetActive(true);
			canTupoLab.text = LanguageManager.instance.GetValue("magictupomax");
			//tupoBtn.gameObject.gameObject.SetActive(false);
			tupoBtn.isEnabled = false;
			//return;
		}
		else if(GamePlayer.Instance.MagicItemLevel < GamePlayer.Instance.MagicTupoLevel)
		{
			//canTupoLab.gameObject.SetActive(true);
			canTupoLab.text = LanguageManager.instance.GetValue("magictupopropnum" +GamePlayer.Instance.MagicTupoLevel);
			//tupoBtn.gameObject.gameObject.SetActive(false);
			tupoBtn.isEnabled = false;
			//return;
		}
		else
		{
			//canTupoLab.gameObject.SetActive(true);
			canTupoLab.text = LanguageManager.instance.GetValue("magictupopropnum" +GamePlayer.Instance.MagicTupoLevel);
			tupoBtn.gameObject.gameObject.SetActive(true);
		}

		tupoBtn.isEnabled = false;

		ArtifactLevelData levelData = ArtifactLevelData.GetData (GamePlayer.Instance.MagicItemLevel,(int)JobType.JT_Axe);//GamePlayer.Instance.MagicTupoLevel/10, (int)JobType.JT_Axe);//GamePlayer.Instance.MagicItemJob);

		if(levelData == null)
			return;
		float addNum1 = (float)(((int)GamePlayer.Instance.MagicItemLevel/5)*0.1f);
		if(GamePlayer.Instance.MagicItemLevel == GamePlayer.Instance.MagicTupoLevel)
		{
			addNum1 = (float)(((int)(GamePlayer.Instance.MagicItemLevel-5)/5)*0.1f);
		}

		for (int j=0; j<propCellList.Count; j++)
		{
			propCellList[j].transform.parent = null;
			propCellList[j].gameObject.SetActive(false);
			propcellPool.Add(propCellList[j]);
		}
		propCellList.Clear ();

		for(int i =0;i<levelData.propArr.Count;i++)
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
				objCell = Object.Instantiate(nowPropCell) as GameObject;
			}
			
			UILabel lable =	objCell.transform.FindChild("name").GetComponent<UILabel>(); 
			float vNum = (float.Parse(levelData.propArr[i].Value)) + (float.Parse(levelData.propArr[i].Value)  * addNum1);
			lable.text  =  LanguageManager.instance.GetValue(levelData.propArr[i].Key.ToString()) + " +" + (int)vNum;
			
			objCell.transform.parent = nowPropGrid.transform;
			objCell.gameObject.SetActive(true);	
			objCell.transform.localScale = Vector3.one;
			propCellList.Add(objCell);
		}
		nowPropGrid.Reposition ();
		ArtifactConfigData configData = ArtifactConfigData.GetData (GamePlayer.Instance.MagicTupoLevel/5, (int)JobType.JT_Axe);//  GamePlayer.Instance.MagicItemJob);
		if(configData == null)
		{
			buyBtn.gameObject.SetActive(false);
			//tupoBtn.gameObject.SetActive(false);
			tupoBtn.isEnabled = false;
			needMaonyImg.gameObject.SetActive(false);
			return;
		}
		COM_Item item = BagSystem.instance.GetItemByItemId ((uint)configData._ItemId_1);
		
		if(item == null)
		{
			needMaonyLab.text =  "0/" + configData._ItemNum_1;
			needMaonyLab.color = Color.red;
			tupoBtn.isEnabled = false;
		}
		else
		{
			int num = BagSystem.instance.GetItemMaxNum((uint)configData._ItemId_1);
			needMaonyLab.text = num + "/" + configData._ItemNum_1;
			
			if(num >= configData._ItemNum_1)
			{
				needMaonyLab.color = Color.grey;
				tupoBtn.isEnabled = true;
			}
			else
			{
				needMaonyLab.color = Color.red;
				tupoBtn.isEnabled = false;
			}
		}


		ArtifactLevelData beferLevelData = ArtifactLevelData.GetData (GamePlayer.Instance.MagicTupoLevel, (int)JobType.JT_Axe);
		if (beferLevelData == null)
			return;
		float addNum = (float)(((int)GamePlayer.Instance.MagicTupoLevel/5)*0.1f);
		for(int i =0;i<beferLevelData.propArr.Count;i++)
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
				objCell = Object.Instantiate(beferPropCell) as GameObject;
			}

			UILabel lable =	objCell.transform.FindChild("name").GetComponent<UILabel>(); 
			float vNum = (float.Parse(beferLevelData.propArr[i].Value)) + (float.Parse(beferLevelData.propArr[i].Value)  * addNum);
			lable.text  =  LanguageManager.instance.GetValue(beferLevelData.propArr[i].Key.ToString())+" +"+(int)vNum;//(int)(float.Parse(levelData.propArr[i].Value) + ((float.Parse(levelData.propArr[i].Value)  *0.1f)));
			objCell.transform.parent = beferPropGrid.transform;
			objCell.gameObject.SetActive(true);	
			objCell.transform.localScale = Vector3.one;
			propCellList.Add(objCell);
		}
		beferPropGrid.Reposition ();




		//needMaonyLab.text = data._Diamonds.ToString ();

	}

	private void OnClickTupo(ButtonScript obj, object args, int param1, int param2)
	{
		//if(GamePlayer.Instance.MagicItemLevel%10 > 0)
		//{
		//	PopText.Instance.Show(LanguageManager.instance.GetValue("tiaojianbuzhu"));
			//return;
	//	}
		NetConnection.Instance.tupoMagicItem (GamePlayer.Instance.MagicTupoLevel);
	}

	private void OnClickBuy(ButtonScript obj, object args, int param1, int param2)
	{
		ArtifactConfigData configData = ArtifactConfigData.GetData (GamePlayer.Instance.MagicTupoLevel/5, (int)JobType.JT_Axe);
		if (configData == null)
			return;
		int shopid = ShopData.GetShopId (configData._ItemId_1);
		QuickBuyUI.ShowMe (shopid);
	}
	void OnDestroy()
	{
		BagSystem.instance.UpdateItemEvent -= UpdateNeedItem;
		BagSystem.instance.ItemChanged -= UpdateNeedItem;
	}



}

