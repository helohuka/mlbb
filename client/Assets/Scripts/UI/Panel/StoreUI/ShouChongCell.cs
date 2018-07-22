using UnityEngine;
using System.Collections;

public class ShouChongCell : MonoBehaviour {

	public UILabel numLabel;
	public UILabel decLabel;
	public UITexture iconSp;

	private ShopData spData_;


	void Start()
	{

	}


	public ShopData SpData
	{
		set
		{
			if(value != null)
			{
				spData_ = value;
				numLabel.text = spData_._Num.ToString();
				decLabel.text = spData_._Name;
				//iconSp.spriteName = ItemData.GetData(spData_.Itemid_).icon_;
				HeadIconLoader.Instance.LoadIcon (ItemData.GetData(spData_._Itemid).icon_, iconSp);
			}
		}
		get
		{
			return spData_;
		}
	}
}
