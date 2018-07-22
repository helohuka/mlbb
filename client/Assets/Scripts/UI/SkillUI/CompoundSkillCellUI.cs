using UnityEngine;
using System.Collections;

public class CompoundSkillCellUI : MonoBehaviour
{
	public UILabel nameLab;
	public UILabel lelveLab;
	public UISprite arrow;
	public UITexture icon;
	public UISprite propImg;
	private int _skillId;
	private int _makeId;


	void Start ()
	{

	}

	public int SkillId
	{
		set
		{
			if(_skillId != value)
			{
				_skillId = value;
			}
		}
		get
		{
			return _skillId;
		}
	}

	public int MakeId
	{
		set
		{
			if( _makeId!= value)
			{
				_makeId = value;

				nameLab.text = ItemData.GetData(_makeId).name_;
				propImg.spriteName = GetQualityBack((int)ItemData.GetData(_makeId).quality_);
				updateRed();
			}
		}
		get
		{
			return _makeId;
		}
	}

	public void updateRed()
	{
		MakeData make = MakeData.GetData (_makeId);
		if (make == null)
			return;

		if( make.skillLevel >= 40 && !CompoundSystem.instance.GetIsOPenEquip((uint)_makeId) )
		{
            this.gameObject.transform.FindChild("bg").FindChild("can").GetComponent<UISprite>().gameObject.SetActive(false);
            this.gameObject.transform.FindChild("bg").FindChild("noPag").GetComponent<UISprite>().gameObject.SetActive(true);
			return;
		}
		else
		{
            this.gameObject.transform.FindChild("bg").FindChild("noPag").GetComponent<UISprite>().gameObject.SetActive(false);
		}


		bool isItemEnough = true;
		for (int i =0; i<make.needItems.Length; i++) 
		{
			ItemData needItem = ItemData.GetData (int.Parse (make.needItems [i]));
			int itemCount = BagSystem.instance.GetItemMaxNum (uint.Parse (make.needItems [i]));
			if (itemCount < int.Parse (make.needItemNum [i])) 
			{
				isItemEnough = false;
			}
		}
		if(isItemEnough)
		{
            this.gameObject.transform.FindChild("bg").FindChild("can").GetComponent<UISprite>().gameObject.SetActive(true);
		}
		else
		{
            this.gameObject.transform.FindChild("bg").FindChild("can").GetComponent<UISprite>().gameObject.SetActive(false);
		}

        this.gameObject.transform.FindChild("bg").FindChild("recommend").GetComponent<UISprite>().gameObject.SetActive(false);
		Profession pro =  Profession.get((JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession),GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel));
		ItemSubType[] items = pro.CanUsedItems(GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel)*2+2);

		for(int i =0 ;i<items.Length;i++)
		{
			if(ItemData.GetData(_makeId).subType_ == items[i])
			{
                this.gameObject.transform.FindChild("bg").FindChild("recommend").GetComponent<UISprite>().gameObject.SetActive(true);
				break;
			}
		}
	}

	public string GetQualityBack(int quality)
	{
		if ((int)quality >= (int)QualityColor.QC_Orange2)
		{
			return "wenzidi_7";
		} 
		if((int)quality <= (int)QualityColor.QC_White)
		{
			return "wenzidi_1";
		}
		else if ((int)quality <= (int)QualityColor.QC_Green)
		{
			return "wenzidi_2";
		}
		else if((int)quality <= (int)QualityColor.QC_Blue1)
		{
			return "wenzidi_3";
		}
		else if ((int)quality <= (int)QualityColor.QC_Purple2)
		{
			return "wenzidi_4";
		}
		else if ((int)quality <= (int)QualityColor.QC_Golden2)
		{
			return "wenzidi_5";
		}
		else if ((int)quality <= (int)QualityColor.QC_Orange2)
		{
			return "wenzidi_6";
		}
		else if ((int)quality <= (int)QualityColor.QC_Pink)
		{
			return "wenzidi_7";
		}
		return "wenzidi_1";
	}

}

