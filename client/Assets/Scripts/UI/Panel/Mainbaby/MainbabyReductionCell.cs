using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MainbabyReductionCell : MonoBehaviour {

	public UILabel nameLabel;
	public UITexture icontou;
	public UITexture zhongzuicON;
	public UISprite leixingSp;
	private BabyData babydata;
	private List<string> _icons = new List<string>();

	public BabyData _Babydata
	{
		set
		{
			if(value != null)
			{
				babydata = value;
				nameLabel.text = babydata._Name;
				
				HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData(babydata._AssetsID).assetsIocn_, icontou);
				HeadIconLoader.Instance.LoadIcon ( babydata._RaceIcon, zhongzuicON);
				if(!_icons.Contains(EntityAssetsData.GetData(babydata._AssetsID).assetsIocn_))
				{
					_icons.Add(EntityAssetsData.GetData(babydata._AssetsID).assetsIocn_);
				}
				if(!_icons.Contains(babydata._RaceIcon))
				{
					_icons.Add(babydata._RaceIcon);
				}
			}
		}
		get
		{
			return babydata;
		}
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
