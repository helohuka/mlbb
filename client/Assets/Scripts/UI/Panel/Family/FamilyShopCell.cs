using UnityEngine;
using System.Collections;

public class FamilyShopCell : MonoBehaviour {

	public UILabel nameLable;
	public UILabel propsnumLabel;
	public UILabel numlabel;
	public UILabel propsSMLabel;
	public UITexture icon;
	public UILabel _XiaoHaoLable;
	public UISprite xiaohaoSp;
	public UISprite hotSp;
	public UISprite spname;
	private COM_GuildShopItem item_;
	public COM_GuildShopItem HShopItem
	{
		set
		{
			if(value != null)
			{
				item_ = value;
				HomeShopData hdata = HomeShopData.GetHomeShopData(item_.shopId_);	
				nameLable.text = hdata.name_;
				propsnumLabel.text = hdata.Price_.ToString();
				//propsSMLabel.text = hpmeData_.timeLimit_.ToString();
//				if(item_.buyLimit_==0)
//				{
//					propsSMLabel.text = "不限购";
//				}else
//				{
					propsSMLabel.text ="可以购买"+ item_.buyLimit_+"次";
//				}
				numlabel.text = hdata.Num_.ToString();
				HeadIconLoader.Instance.LoadIcon (ItemData.GetData(hdata.Itemid_).icon_, icon);
			}
		}
		get
		{
			return item_;
		}
	}
	private HomeShopData hpmeData_;
	public HomeShopData HpmeData
	{
		set
		{
			if(value != null)
			{
				hpmeData_ = value;
				nameLable.text = hpmeData_.name_;
				propsnumLabel.text = hpmeData_.Price_.ToString();
				//propsSMLabel.text = hpmeData_.timeLimit_.ToString();
				if(hpmeData_.timeLimit_==0)
				{
					propsSMLabel.text = "不限购";
				}else
				{
					propsSMLabel.text ="可以购买"+ hpmeData_.timeLimit_.ToString()+"次";
				}
				numlabel.text = hpmeData_.Num_.ToString();
				HeadIconLoader.Instance.LoadIcon (ItemData.GetData(hpmeData_.Itemid_).icon_, icon);
			}
		}
		get
		{
			return hpmeData_;
		}
	}

	public void HideUI(int index)
	{
		xiaohaoSp.gameObject.SetActive (false);
		spname.spriteName = "webzudihong";
		hotSp.gameObject.SetActive (false);
		numlabel.gameObject.SetActive (false);
		HeadIconLoader.Instance.LoadIcon ("suo", icon);
		nameLable.text = LanguageManager.instance.GetValue ("weikaiqi");
		if(GuildSystem.GuildShopLevel(index)==1)
		{
			propsSMLabel.text = LanguageManager.instance.GetValue("shandkaiqi1");
		}else
			if(GuildSystem.GuildShopLevel(index)==2)
		{
			propsSMLabel.text = LanguageManager.instance.GetValue("shandkaiqi2");;
		}else
			if(GuildSystem.GuildShopLevel(index)==3)
		{
			propsSMLabel.text = LanguageManager.instance.GetValue("shandkaiqi2");;
		}
	}
	void Start () {
		_XiaoHaoLable.text = LanguageManager.instance.GetValue ("Guild_XiaoHao");
	}
	

}
