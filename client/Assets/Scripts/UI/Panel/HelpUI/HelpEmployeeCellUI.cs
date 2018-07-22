using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HelpEmployeeCellUI : MonoBehaviour
{
	public UITexture icon;
	public UILabel nameLab;
	public UISprite jobIcon;
	public UISprite iconBack;
	public UISprite backImg;
	private EmployeeData _employeeData;
	private List<string> _icons = new List<string>();
	void Start ()
	{

	}


	public EmployeeData Employee
	{
		set
		{
			if(value != null)
			{
				_employeeData = value;
				HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData(_employeeData.asset_id).assetsIocn_,icon);
				nameLab.text  = _employeeData.name_;
				jobIcon.spriteName = _employeeData.professionType_.ToString();
				iconBack.spriteName = EmployessSystem.instance.GetQualityBack((int) _employeeData.quality_);
				backImg.spriteName = EmployessSystem.instance.GetCellQualityBack((int) _employeeData.quality_);
				if(!_icons.Contains(EntityAssetsData.GetData(_employeeData.asset_id).assetsIocn_))
				{
					_icons.Add(EntityAssetsData.GetData(_employeeData.asset_id).assetsIocn_);
				}

				//gameObject.transform.FindChild("Sprite").GetComponent<UISprite>().spriteName  =  EmployessSystem.instance.GetQualityBack((int)_employeeData.quality_);
			}
		}
		get
		{
			return _employeeData;
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

