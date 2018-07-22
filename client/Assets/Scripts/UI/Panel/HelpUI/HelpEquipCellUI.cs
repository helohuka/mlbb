using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelpEquipCellUI : MonoBehaviour
{
	public UILabel nameLab;
	public UITexture icon;
	public UISprite panel;
	public UISprite iconBack;
	public UISprite back;

	private ItemData _itemData;
	private SkillData _skillData;
	private List<string> _icons = new List<string>();
	void Start ()
	{

	}

	public ItemData Item
	{
		set
		{
			if(value != null)
			{
				_itemData = value;
				nameLab.text = _itemData.name_;
				HeadIconLoader.Instance.LoadIcon(_itemData.icon_,icon);
				if(!_icons.Contains(_itemData.icon_))
				{
					_icons.Add(_itemData.icon_);
				}

			}
		}
		get
		{
			return _itemData; 
		}
	}

	public SkillData Skill
	{
		set
		{
			if(value != null)
			{
				_skillData = value;
				nameLab.text = _skillData._Name;
				HeadIconLoader.Instance.LoadIcon(_skillData._ResIconName,icon);

				if(!_icons.Contains(_skillData._ResIconName))
				{
					_icons.Add(_skillData._ResIconName);
				}
			}
		}
		get
		{
			return _skillData;
		}
	}

	void OnDestroy()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}
}

