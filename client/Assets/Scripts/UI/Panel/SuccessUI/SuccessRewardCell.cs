using UnityEngine;
using System.Collections;

public class SuccessRewardCell : MonoBehaviour {

	public UITexture iconSp;
	public UILabel namelLabel;
	public UISprite back;
	private ItemData _idata;
	
	public ItemData Idata
	{
		set
		{
			if(value != null)
			{
				_idata = value;
				//iconSp.spriteName = _idata.icon_;
				HeadIconLoader.Instance.LoadIcon (_idata.icon_, iconSp);
				namelLabel.text = _idata.name_;
				string sss = "";



				back.spriteName = BagSystem.instance.GetQualityBack((int)_idata.quality_);


			}
		}
		get
		{
			return _idata;
		}
	}
	void Start () {
	
	}
	

}
