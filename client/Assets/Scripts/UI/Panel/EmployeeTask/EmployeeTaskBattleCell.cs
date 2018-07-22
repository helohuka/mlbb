using UnityEngine;
using System.Collections;

public class EmployeeTaskBattleCell : MonoBehaviour
{
	public UITexture icon;
	public UILabel nameLab;
	public UISprite IconBack;
	public UISprite proffession; 

	private int empInstId_;


	void Start ()
	{

	}

	public int EmployeeInstId
	{
		set
		{
			empInstId_ = value;
			Employee emp = GamePlayer.Instance.GetEmployeeById(empInstId_);
			if(emp == null)
				return;
			nameLab.text = emp.InstName;
			IconBack.spriteName  =  EmployessSystem.instance.GetQualityBack((int)emp.quality_);
			HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData(EmployeeData.GetData(emp.GetIprop(PropertyType.PT_TableId)).asset_id).assetsIocn_,icon);
			proffession.spriteName =  ((JobType) emp.GetIprop(PropertyType.PT_Profession)).ToString();

		}
		get
		{
			return empInstId_;
		}
	}
}

