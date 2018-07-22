using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class chatfaceUI : MonoBehaviour {

	public UIGrid grid;
	public UIGrid daojuGird;
	public GameObject petItem;
	public GameObject daojuItem;
	public GameObject faceItem;
	public UIGrid facegrid;
	public GameObject textItem;
	public UIGrid textgrid;
	public UIButton closeBtn;
	public List<UIButton> btns = new List<UIButton>();
	public UIInput input;

	public UIScrollView ListView_;
	UIPanel listPanel_;
	BoxCollider listDragArea_;


	//public SymbolInput _symbolInput;
	public static int index;
	private List<GameObject> itemsobj = new List<GameObject>();

	void Start () {
		petItem.SetActive (false);
		daojuItem.SetActive (false);
		textItem.SetActive (false);
		faceItem.SetActive (false);
		for(int i =0;i<btns.Count;i++)
		{
			UIManager.SetButtonEventHandler (btns[i].gameObject, EnumButtonEvent.OnClick, OnClickbtn,i, 0);
		}
		listPanel_ = ListView_.gameObject.GetComponent<UIPanel>(); 
		listDragArea_ = ListView_.gameObject.GetComponent<BoxCollider>();

		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickclose,0, 0);
		ChatSystem.PublishItemInstResOk += PublishItemInstRes;
		ChatSystem.PublishBabyInstResOk += PublishBabyInstRes;
		SelectBtnForindex (0);
		addFaceItem(ChatSystem.faceStrl);
	}
	void Update()
	{
		listDragArea_.center = listPanel_.clipOffset;
	}
	void PublishItemInstRes(COM_ShowItemInstInfo ShowItem, ChatKind Kind)
	{
		input.value = LanguageManager.instance.GetValue ("wupinzhanshi").Replace("{t1}","2" ).Replace("{t2}",ShowItem.showId_.ToString()).Replace("{t4}",GetPinzhi(ItemData.GetData ((int)ShowItem.itemId_))).Replace("{t3}",ItemData.GetData ((int)ShowItem.itemId_).name_);
	}
	void PublishBabyInstRes(COM_ShowbabyInstInfo InstInfo, ChatKind Kind)
	{
		input.value = LanguageManager.instance.GetValue ("wupinzhanshi").Replace("{t1}","3" ).Replace("{t2}",InstInfo.showId_.ToString()).Replace("{t4}",GetBPinzhi(BabyData.GetData((int)InstInfo.babyId_))).Replace("{t3}",BabyData.GetData((int)InstInfo.babyId_)._Name);

	}
	string GetBPinzhi(BabyData bdata)
	{
		
		if(bdata._PetQuality == PetQuality.PE_Blue)
		{
			return "62c6ff";
		}else
			if(bdata._PetQuality == PetQuality.PE_Golden)
		{
			return "ffea5e";
		}else
			if(bdata._PetQuality == PetQuality.PE_Green)
		{
			return "5af936";
		}
		else
			if(bdata._PetQuality == PetQuality.PE_Orange)
		{
			return "ff6600";
		}
		else
			if(bdata._PetQuality == PetQuality.PE_Pink)
		{
			return "ff618a";
		}
		else
			if(bdata._PetQuality == PetQuality.PE_Purple)
		{
			return "d05eff";
		}
		else
			if(bdata._PetQuality == PetQuality.PE_White)
		{
			return "ffffff";
		}
		return "00ff00";
	}
	string GetPinzhi(ItemData idata)
	{
		
		if ((int)idata.quality_ >= (int)QualityColor.QC_Pink)
		{
			return "ff618a";
		} 
		if((int)idata.quality_  <= (int)QualityColor.QC_White)
		{
			return "ffffff";
		}
		else if ((int)idata.quality_  <= (int)QualityColor.QC_Green)
		{
			return "5af936";
		}
		else if((int)idata.quality_  <= (int)QualityColor.QC_Blue1)
		{
			return "62c6ff";
		}
		else if ((int)idata.quality_  <= (int)QualityColor.QC_Purple2)
		{
			return "d05eff";
		}
		else if ((int)idata.quality_  <= (int)QualityColor.QC_Golden2)
		{
			return "ffea5e";
		}
		else if ((int)idata.quality_  <= (int)QualityColor.QC_Orange2)
		{
			return "ff6600";
		}
		else if ((int)idata.quality_  <= (int)QualityColor.QC_Pink)
		{
			return "ff618a";
		}
		return "00ff00";
	}
	private void OnClickbtn(ButtonScript obj, object args, int param1, int param2)
	{
		index = param1;
		SelectBtnForindex (param1);
		ClearItems ();
		if(param1 == 0)
		{
			addFaceItem(ChatSystem.faceStrl);
		}else
			if(param1 ==1)
		{
			addBabyItem(GamePlayer.Instance.babies_list_);
		}else
			if(param1 ==2)
		{
			addItem (BagSystem.instance.BagItems);
		}else
			if(param1 ==3)
		{
			addTextItem(ChatSystem.SendRecords);
		}
	}
	private void OnClickclose(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}
	void addItem(COM_Item [] items)
	{
		for(int i =0;i<items.Length;i++)
		{
			if(items[i] == null)return;
			GameObject go = GameObject.Instantiate(daojuItem)as GameObject;
			go.SetActive(true);
			go.transform.parent = daojuGird.transform;
			go.transform.localScale = Vector3.one;
			UITexture te = go.GetComponentInChildren<UITexture>();
			HeadIconLoader.Instance.LoadIcon (ItemData.GetData((int)items[i].itemId_).icon_, te);
			UIManager.SetButtonEventHandler (go.gameObject, EnumButtonEvent.OnClick, OnClickItemgo,(int)items[i].instId_, 0);
			daojuGird.repositionNow = true;
		}

	}
	void addFaceItem(string[] strs)
	{
		for(int i =0;i<strs.Length;i++)
		{
			GameObject go = GameObject.Instantiate(faceItem)as GameObject;
			go.SetActive(true);
			go.transform.parent = facegrid.transform;
			//go.transform.position = Vector3.zero;
			go.transform.localScale = Vector3.one;
			UISprite ssp = go.GetComponent<UISprite>();
			ssp.spriteName = "";
			Transform tsp = go.transform.FindChild("face");
			UISprite sp = tsp.GetComponent<UISprite>();
			sp.spriteName = strs[i];
			go.name = strs[i];
			UIManager.SetButtonEventHandler (go.gameObject, EnumButtonEvent.OnClick, OnClickFaceItem,i, 0);
		}
		facegrid.repositionNow = true;
	}
	private void OnClickFaceItem(ButtonScript obj, object args, int param1, int param2)
	{
		input.value += ChatSystem.faceStrl[param1];
	}
	private void OnClickItemgo(ButtonScript obj, object args, int param1, int param2)
	{
//		if(itemCom (param1)!=null)
//		{
//			//OpenTeam.comItem =  itemCom (param1);
//			OpenTeam.type = chatType.item;
//		}
		NetConnection.Instance.publishItemInst (ItemContainerType.ICT_BagContainer, (uint)param1, ChatKind.CK_World, GamePlayer.Instance.InstName);

	}
	private COM_Item itemCom (int itemid)
	{
		for(int i=0;i<BagSystem.instance.BagItems.Length;i++)
		{
			if(BagSystem.instance.BagItems[i] ==null)continue;
			if(BagSystem.instance.BagItems[i].itemId_ == itemid)
			{
				return BagSystem.instance.BagItems[i];
			}
		}
		return null;
	}
	void addBabyItem(List<Baby> babys)
	{
		for(int i =0;i<babys.Count;i++)
		{
			if(babys[i] == null)return;
			GameObject go = GameObject.Instantiate(petItem)as GameObject;
			go.SetActive(true);
			go.transform.parent = grid.transform;
			//go.transform.position = Vector3.zero;
			go.transform.localScale = Vector3.one;
			go.name = babys[i].InstName;
			UIManager.SetButtonEventHandler (go.gameObject, EnumButtonEvent.OnClick, OnClickbabygo,babys[i].InstId, 0);
			UITexture[] tes= go.GetComponentsInChildren<UITexture>();
			foreach(UITexture tex in tes)
			{
				if(tex.name.Equals("peticon"))
				{
					HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData(babys[i].GetIprop(PropertyType.PT_AssetId)).assetsIocn_, tex);
				}
				if(tex.name.Equals("zhongzuicon"))
				{
					HeadIconLoader.Instance.LoadIcon (BabyData.GetData(babys[i].GetIprop(PropertyType.PT_TableId))._RaceIcon, tex);
				}
			}
		

		}
		grid.repositionNow = true;
//		for(int i =0;i<itemsobj.Count;i++)
//		{
//			itemsobj[i].SetActive(true);
//		}
		//grid.Reposition ();
	}
	private void OnClickbabygo(ButtonScript obj, object args, int param1, int param2)
	{
		Baby binset = GamePlayer.Instance.GetBabyInst ((int)param1);
//		OpenTeam.babyinset =  binset.GetInst ();
//		OpenTeam.type = chatType.baby;
	  
		NetConnection.Instance.publishbabyInst (ChatKind.CK_World, (uint)param1, GamePlayer.Instance.InstName);
	}
	void addTextItem(LinkedList<string> node)
	{
		foreach( string str in node)
		{
			GameObject go = GameObject.Instantiate(textItem)as GameObject;
			go.SetActive(true);
			go.transform.parent = textgrid.transform;
			go.transform.position = Vector3.zero;
			go.transform.localScale = Vector3.one;
			go.name = str;
			UIManager.SetButtonEventHandler (go.gameObject, EnumButtonEvent.OnClick, OnClicktext,0, 0);
			UILabel la = go.GetComponent<UILabel>();
			la.text = str;
			textgrid.repositionNow = true;
		}

	}
	private void OnClicktext(ButtonScript obj, object args, int param1, int param2)
	{
		input.value = obj.name;
	}
	void SelectBtnForindex(int index)
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
	void ClearItems()
	{

		foreach(Transform tr in grid.transform)
		{
			Destroy(tr.gameObject);
		}
		foreach(Transform tr in daojuGird.transform)
		{
			Destroy(tr.gameObject);
		}
		foreach(Transform tr in textgrid.transform)
		{
			Destroy(tr.gameObject);
		}
		foreach(Transform tr in facegrid.transform)
		{
			Destroy(tr.gameObject);
		}
	}

	void OnEnable()
	{
		index = 0;
		SelectBtnForindex (index);
		ClearItems ();
		if(index == 0)
		{
			addFaceItem(ChatSystem.faceStrl);
		}else
			if(index ==1)
		{
			addBabyItem(GamePlayer.Instance.babies_list_);
		}else
			if(index ==2)
		{
			addItem (BagSystem.instance.BagItems);
		}else
			if(index ==3)
		{
			addTextItem(ChatSystem.SendRecords);
		}

	}
	void OnDestroy()
	{
		index = 0;
		ChatSystem.PublishItemInstResOk -= PublishItemInstRes;
		ChatSystem.PublishBabyInstResOk -= PublishBabyInstRes;
	}

}
