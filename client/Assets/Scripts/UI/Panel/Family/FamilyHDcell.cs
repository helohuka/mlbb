using UnityEngine;
using System.Collections;

public class FamilyHDcell : MonoBehaviour {

	public GameObject DetailObj;
	public UILabel namelLable;
	public UILabel decLable;
	public UILabel huodongdengji;
	public UIButton xiangqingBtn;
	public UITexture icon;
	public UIButton enterBtn;
	private DaliyActivityData _data;
	public DaliyActivityData Data
	{
		set
		{
			if(value != null)
			{
				_data = value;
				namelLable.text = _data.activityName_;
				decLable.text =_data.activityTime_ +"\n"+_data.desc_;
				huodongdengji.text = _data.joinLv_.ToString();
				HeadIconLoader.Instance.LoadIcon (_data.Icon_, icon);
			}
		}
		get
		{
			return _data;
		}
	}
	void Start () {
		UIManager.SetButtonEventHandler(enterBtn.gameObject, EnumButtonEvent.OnClick, OnClickEnter, 0, 0);
		UIManager.SetButtonEventHandler(xiangqingBtn.gameObject, EnumButtonEvent.OnClick, OnClickxiangqingBtn, 0, 0);
	}
	private void OnClickxiangqingBtn(ButtonScript obj, object args, int param1, int param2)
	{
		DetailObj.SetActive (true);
		HuoDongDetail hcell = DetailObj.GetComponent<HuoDongDetail> ();
		hcell.SetData (Data.id_);
	}
	private void OnClickEnter(ButtonScript obj, object args, int param1, int param2)
	{
		string [] arr = Data.joinInfo_.Split (';');
		int npcid = int.Parse (arr[1]);
		NetConnection.Instance.moveToNpc (npcid);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
