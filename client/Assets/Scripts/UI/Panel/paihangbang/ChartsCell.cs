using UnityEngine;
using System.Collections;

public class ChartsCell : MonoBehaviour {

	public UILabel numLabel;
	public UILabel nameLabel;
	public UILabel zhiyeLabel;
	public UILabel levelLabel;
	public UISprite paiSp;
	private COM_EndlessStair COM_endless;
	private COM_ContactInfo contactInfo;

	private COM_BabyRankData babyRankDate;
	private COM_EmployeeRankData employeeRankDate;
	private COM_ContactInfo CContactInfo;

	public COM_ContactInfo COContactInfo
	{
		set
		{
			if(value != null)
			{
				CContactInfo = value;
				paiSp.gameObject.SetActive(false);
				nameLabel.text = CContactInfo.name_;
				zhiyeLabel.text = Profession.get ((JobType)CContactInfo.job_, (int)CContactInfo.jobLevel_).jobName_;
				levelLabel.text = CContactInfo.ff_.ToString();
			}
		}
		get
		{
			return CContactInfo;
		}
	}




	public COM_EmployeeRankData EmployeeRankDate
	{
		set
		{
			if(value != null)
			{
				employeeRankDate = value;
				paiSp.gameObject.SetActive(false);
				nameLabel.text = employeeRankDate.name_;
				//zhiyeLabel.text = Profession.get ((JobType)contactInfo.job_, (int)contactInfo.jobLevel_).jobName_;
				levelLabel.text = employeeRankDate.ff_.ToString();
				zhiyeLabel.text = employeeRankDate.ownerName_;
			}
		}
		get
		{
			return employeeRankDate;
		}
	}






	public COM_BabyRankData BabyRankDate
	{
		set
		{
			if(value != null)
			{
				babyRankDate = value;
				paiSp.gameObject.SetActive(false);
				nameLabel.text = babyRankDate.name_;
				//zhiyeLabel.text = Profession.get ((JobType)contactInfo.job_, (int)contactInfo.jobLevel_).jobName_;
				levelLabel.text = babyRankDate.ff_.ToString();
				zhiyeLabel.text = babyRankDate.ownerName_;
			}
		}
		get
		{
			return babyRankDate;
		}
	}


	public COM_ContactInfo ContactInfo
	{
		set
		{
			if(value != null)
			{
				contactInfo = value;
				paiSp.gameObject.SetActive(false);
				nameLabel.text = contactInfo.name_;
				zhiyeLabel.text = Profession.get ((JobType)contactInfo.job_, (int)contactInfo.jobLevel_).jobName_;
				levelLabel.text = contactInfo.level_.ToString();;
			}
		}
		get
		{
			return contactInfo;
		}
	}
	public COM_EndlessStair COM_Endless
	{
		set
		{
			if(value != null)
			{
				COM_endless = value;
				paiSp.gameObject.SetActive(false);
				nameLabel.text = COM_endless.name_;
				zhiyeLabel.text = Profession.get ((JobType)COM_endless.job_, (int)COM_endless.joblevel_).jobName_;
				levelLabel.text = COM_endless.level_.ToString();;
			}
		}
		get
		{
			return COM_endless;
		}
	}


	void Start () {
	
	}
	

}
