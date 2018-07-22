using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmployeeTujianCellUI : MonoBehaviour
{
	public UILabel nameLab;
	public UITexture icon;
	public UISprite pinzhi;
	public UISprite backImg;
	public UISprite professionImg;
	public UIProgressBar bar;
	public UILabel barNumLab;
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
				professionImg.spriteName = _employeeData.professionType_.ToString();
				backImg.spriteName = EmployessSystem.instance.GetQualityBack((int) _employeeData.quality_);
				pinzhi.spriteName = EmployessSystem.instance.GetCellQualityBack((int) _employeeData.quality_);
				if(!_icons.Contains(EntityAssetsData.GetData(_employeeData.asset_id).assetsIocn_))
				{
					_icons.Add(EntityAssetsData.GetData(_employeeData.asset_id).assetsIocn_);
				}
				DebrisData ded = DebrisData.GetEmpData(_employeeData.id_);
				if(ded== null)
				{
					return;
				}
				int num = BagSystem.instance.GetItemMaxNum((uint)_employeeData.id_);

				bar.value = (float)num/(float)ded.needNum_;
				barNumLab.text = num+"/"+ded.needNum_;
				
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

