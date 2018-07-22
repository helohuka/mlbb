using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiningUI : MonoBehaviour
{
	public UIButton getBtn;
	public UIGrid  itemGrid;
	public GameObject itemCell;
	public UIButton startBtn;
	public Transform Mpos;

	private int _mineId;
	private int _skillId;
	private bool _isShow;
	private List<GameObject> cellList = new List<GameObject>();
	private List<GameObject> cellPoolList = new List<GameObject>();
	private List<COM_Item> mineList = new List<COM_Item>();
	private GameObject role;	
	public GameObject effectObj;
	public UILabel mineTimeLab;


	void Start ()
	{
		UIManager.SetButtonEventHandler (getBtn.gameObject, EnumButtonEvent.OnClick, OnGetBtn, 0, 0);
		UIManager.SetButtonEventHandler (startBtn.gameObject, EnumButtonEvent.OnClick, OnStartBtn, 0, 0);
		//GatherSystem.instance.MineItemEvent += new MineItemEventHandler(OnAddItemEvent);
        //GatherSystem.instance.MiningOkEvent += new MiningOkEventHandler (MiningOk);

	}




	void AssetLoadCallBack(GameObject ro, ParamData data)
	{
		role = ro;
		NGUITools.SetChildLayer(ro.transform, LayerMask.NameToLayer("3D"));
		ro.transform.parent = Mpos;
		ro.transform.localPosition = Vector3.zero;
		ro.transform.localScale = new Vector3 (200f, 200f, 200f);
		ro.transform.localRotation = Quaternion.Euler (10f, 180f, 0f);
	}


	private void OnGetBtn(ButtonScript obj, object args, int param1, int param2)
	{
        //if (BagSystem.instance.BagIsFull() && GatherSystem.instance.MineItemList.Count > 0)
        //{
        //    PopText.Instance.Show(LanguageManager.instance.GetValue("bagfull"));
        //    return;
        //}
        //NetConnection.Instance.stopMining ();
		//ChatSystem.instance.AddchatInfo (LanguageManager.instance.GetValue("getitem").Replace("{n}",ItemData.GetData((int)item.itemId_).name_),ChatKind.CK_System,false);
	}


	private void OnStartBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(BagSystem.instance.BagIsFull())
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("bagfull"));
			return;
		}

        //NetConnection.Instance.startMining (mineId);

		getBtn.gameObject.SetActive (true);
		//getBtn.isEnabled = false;
		startBtn.gameObject.SetActive (false);

		effectObj.gameObject.SetActive (true);

	

	}



	public void OnAddItemEvent(COM_Item item)
	{
		if(!_isShow)
		{
			return;
		} 
		AddItem(item);
	}


	private void AddItem(COM_Item item)
	{
		ItemData itemData = ItemData.GetData ((int)item.itemId_);
		if(itemData == null)
		{
			return;
		}

	//	if(!getBtn.isEnabled)
		//{
		//	getBtn.isEnabled = true;
		//}

		for(int i=0;i<cellList.Count;i++)
		{
			if(item.instId_ == int.Parse(cellList[i].name))
			{
				cellList[i].transform.Find ("num").GetComponent<UILabel> ().text = item.stack_.ToString();
				return;
			}
		}

		GameObject obj = null;

		if(cellPoolList.Count>0)
		{
			obj = cellPoolList[0];
			cellPoolList.Remove(cellPoolList[0]);
		}
		else
		{
			obj = Object.Instantiate(itemCell.gameObject) as GameObject;
		}


		obj.name = item.instId_.ToString ();
		HeadIconLoader.Instance.LoadIcon(itemData.icon_, obj.transform.Find ("icon").GetComponent<UITexture> ());
		obj.transform.Find ("name").GetComponent<UILabel> ().text = itemData.name_;
		obj.transform.Find ("num").GetComponent<UILabel> ().text = item.stack_.ToString();
		obj.transform.parent = itemGrid.transform;
		obj.SetActive(true);
		obj.transform.localScale = Vector3.one;
		cellList.Add (obj);

		itemGrid.Reposition ();
	}

	private void MiningOk()
	{
		/*foreach(var x in cellList)
		{
			GameObject.Destroy(x);
		}
		*/
		effectObj.gameObject.SetActive (false);
		//if(effectObj != null)
			//GameObject.Destroy(effectObj);

		//cellList.Clear();
		//mineList.Clear ();
		//getBtn.gameObject.SetActive (false);

		//if(mineId > 0 )
		//{
		//	startBtn.gameObject.SetActive (true);
		//}

		Show ();
	}


	//当前采集挖矿Id
	public int mineId
	{
		set
		{
			_mineId = value;
		}
		get
		{
			return _mineId;
		}
	}
	
	public int SkillId
	{
		set
		{
			_skillId = value;
		}
		get
		{
			return _skillId;
		}
	}


	public void Show()
	{
        //this.gameObject.SetActive (true);
        //_isShow = true;
        //if(GatherSystem.instance.IsMineing || GatherSystem.instance.MineItemList.Count>0)
        //{
        //    mineId = GatherSystem.instance.mineId;
        //}
        //    getBtn.gameObject.SetActive(true);
        //    startBtn.gameObject.SetActive(false);
		

        //if(GatherSystem.instance.IsMineing)
        //{
        //    effectObj.gameObject.SetActive(true);
        //}

        //foreach(var c in cellList)
        //{
        //    c.transform.parent = null;
        //    c.gameObject.SetActive(false);
        //    cellPoolList.Add(c);
        //}

        //cellList.Clear();


        //List<COM_Item> mines = GatherSystem.instance.MineItemList;
        //if(mines.Count > 0)
        //{
        //    foreach(COM_Item i in mines)
        //    {
        //        AddItem(i);
        //    }
        //    getBtn.isEnabled = true;
        //}
        //else
        //{
        //    //getBtn.isEnabled = false;
        //}

        //itemGrid.repositionNow = true;

        //if(!GatherSystem.instance.IsMineing && GatherSystem.instance.MineItemList.Count == 0)
        //{
        //    getBtn.gameObject.SetActive(false);
        //    startBtn.gameObject.SetActive(true);
        //}

	
        //if(role == null)
        //{
        //    GameManager.Instance.GetActorClone((ENTITY_ID)GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId), (ENTITY_ID)0, AssetLoadCallBack, new ParamData(GamePlayer.Instance.InstId));
        //}
	}
	
	public void Hide()
	{
		this.gameObject.SetActive (false);
		_isShow = false;
		for(int i=0;i<cellList.Count;i++)
		{
			if(cellList[i] != null)
			{
				GameObject.Destroy(cellList[i]);
				cellList[i] = null;
			}
		}
		cellList.Clear ();


		for(int i=0;i<cellPoolList.Count;i++)
		{
			if(cellPoolList[i] != null)
			{
				GameObject.Destroy(cellPoolList[i]);
				cellPoolList[i] = null;
			}
		}
		cellPoolList.Clear ();

		//GatherSystem.instance.MineItemEvent -= OnAddItemEvent;
		//G
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

	void OnDestroy()
	{
        //GatherSystem.instance.MiningOkEvent -= MiningOk;
	}
}

