using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemCellUI : MonoBehaviour
{
	public UISprite cellPane;
	public UITexture ItemIcon;
	public UILabel ItemConutLab;
    public UISprite mask;
	
	private ItemData _itemData;
	private COM_Item _itemInst;
	private uint _itemId;
	private int _itemCount;
	private bool _showCount = false;
	private bool _showTips = false;
    private bool _enable = true;
	private List<string> _icons = new List<string>();
	void Start ()
	{

	}
	

	public uint itemId
	{
		set
		{
			if(_itemId != value)
			{
				_itemId = value;
				data = ItemData.GetData((int)_itemId);
			}
		}
		get
		{
			return _itemId;
		}
	}

	public ItemData data
	{
		get
		{
			return _itemData;
		}
		set
		{
			_itemData = value;
			if(_itemData != null)
			{
                cellPane.spriteName = BagSystem.instance.GetQualityBack((int)_itemData.quality_);
                ItemIcon.width = (int)(cellPane.width * 0.8f);
                ItemIcon.height = (int)(cellPane.height * 0.8f);
				HeadIconLoader.Instance.LoadIcon(_itemData.icon_, ItemIcon);
				if(!_icons.Contains(_itemData.icon_))
				{
					_icons.Add(_itemData.icon_);
				}

				ItemIcon.depth = cellPane.depth +1;
                mask.depth = ItemIcon.depth + 1;
                mask.enabled = false;
			}
		

			if(_showTips)
			{
				RegesitTips();
			}
		}
	}


	public COM_Item ItemInst
	{
		set
		{
			_itemInst = value;
			if(_itemInst != null)
				data = ItemData.GetData((int)_itemInst.itemId_);
		}
		get
		{
			return _itemInst;
		}
	}



	public int ItemCount
	{
		set
		{
			if(value > 1)
			{
				ItemConutLab.gameObject.SetActive(true);
				_itemCount = value;
				ItemConutLab.text = _itemCount.ToString();
				ItemConutLab.depth = ItemIcon.depth+1;
			}
			else
			{
				ItemConutLab.gameObject.SetActive(false);
			}
		}
		get
		{
			return _itemCount;
		}
	}

	private void RegesitTips()
	{
		//UIManager.SetButtonEventHandler (cellPane.gameObject, EnumButtonEvent.TouchDown, OnMouseDown, 0, 0);
        UIManager.SetButtonEventHandler(cellPane.gameObject, EnumButtonEvent.OnClick, OnMouseDown, 0, 0);
	}

	private void UnRegesitTips()
	{
        UIManager.RemoveButtonEventHandler(cellPane.gameObject, EnumButtonEvent.OnClick);
	}


	public bool showTips
	{
		set
		{
			if(_showTips != value)
			{
				if(_itemData != null)
				{
					if(_showTips)
					{
						UnRegesitTips();
					}
					else
					{
						RegesitTips();
					}
				}
				_showTips = value;
			}
			
		}
		get
		{
			return _showTips;
		}
	}

    public bool enable
    {
        set
        {
            _enable = value;
            mask.enabled = !value;
            gameObject.GetComponent<BoxCollider>().enabled = value;
            _showTips = value;
        }
        get
        {
            return _enable;
        }
    }

    public bool collideEnable
    {
        set
        {
            gameObject.GetComponent<BoxCollider>().enabled = value;
        }
    }

	private void OnMouseDown( ButtonScript obj, object args, int param1, int param2 )
	{
		if(TipsItemUI.instance != null)
		{
			if(ItemInst != null)
			{
				TipsItemUI.instance.setData(_itemInst);
			}
			else
			{
				TipsItemUI.instance.setData(_itemData);
			}
		}

	}


	private void OnMouseUp( ButtonScript obj, object args, int param1, int param2)
	{

		if(TipsItemUI.instance != null)
		{
			TipsItemUI.instance.HideTips();
		}

	}

	void OnDestroy()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
        if (TipsItemUI._instance != null)
            TipsItemUI.instance.HideTips();
	}


}

