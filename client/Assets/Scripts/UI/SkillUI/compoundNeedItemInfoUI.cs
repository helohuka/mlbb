using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class compoundNeedItemInfoUI : MonoBehaviour
{
	public UILabel nameLab;
	public UITexture itemIcon;
	public UILabel levelLab;
	public UILabel typeLab;
	public UILabel descLab;
	public UILabel getWayLab;
	public UIButton getBtn;
	public UIButton closeBtn;
	public UIButton buyBtn;

	private ItemData _itemData;

	private List<string> _icons = new List<string>();

	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		UIManager.SetButtonEventHandler (getBtn.gameObject, EnumButtonEvent.OnClick, OnGetWay, 0, 0);


	}


	public ItemData item
	{
		set
		{
			if(value != null)
			{
				_itemData = value;
				nameLab.text =  _itemData.name_;
				HeadIconLoader.Instance.LoadIcon(_itemData.icon_,itemIcon);
				if(!_icons.Contains(_itemData.icon_))
				{
					_icons.Add(_itemData.icon_);
				}
				levelLab.text = _itemData.level_.ToString();
				descLab.text = _itemData.desc_;
				typeLab.text = LanguageManager.instance.GetValue(_itemData.mainType_.ToString());
				getWayLab.text = _itemData.acquiringWay_;

				int needLevel = GatherData.gatherLevel(item.id_);
				if( needLevel > GamePlayer.Instance.GetIprop(PropertyType.PT_Level) || needLevel == 0)
				{
					getBtn.gameObject.SetActive(false);
				}
				else
				{
					getBtn.gameObject.SetActive(true);
				}

                //if(GatherData.GetData(_itemData.id_) != null)
                //{
                //    getBtn.gameObject.SetActive(true);
                //}
                //else
                //{
                //    getBtn.gameObject.SetActive(false);
                //}
			}

		}
		get
		{
			return _itemData;
		}
	}
		
	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		this.gameObject.SetActive (false);
	}

	private void OnGetWay(ButtonScript obj, object args, int param1, int param2)
	{
		GatherData gData = GatherData.gatherItemId (item.id_);
		if (gData == null)
			return;
	//	SkillViewUI.SwithShowMe (1,(int)gData._Type,item.id_);
		SkillViewUI.ShowMe (1,(int)gData._Type,item.id_);
		this.gameObject.SetActive (false);
	}



	void OnDestroy()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}
}


