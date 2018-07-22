using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConstructionDetail : MonoBehaviour {

    public UILabel static_needGold_;
    public UILabel static_haveGold_;
    public UILabel static_btnlvlbl_;

    public UILabel name_;
    public UILabel needGold_;
    public UILabel haveGold_;
    public UISprite icon_;
	public UIButton closeBtn;
	public UIButton levelUpBtn;
	public UISprite levelImg;
	public UILabel descLab;
	public List<UIButton> tabBtn = new List<UIButton> ();

    public FamilyData data_;
	List<FamilyData> contents_;
	void Start () 
	{
       // static_needGold_.text = LanguageManager.instance.GetValue("");
        //static_haveGold_.text = LanguageManager.instance.GetValue("");
        //static_btnlvlbl_.text = LanguageManager.instance.GetValue("");
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0,0);
		UIManager.SetButtonEventHandler (levelUpBtn.gameObject, EnumButtonEvent.OnClick, OnClickLevelUp, 0,0);
		FamilySystem.instance.UpdateGuildBuildingEvent += new RequestEventHandler<COM_GuildBuilding> (OnFamilyDataEvent);
		FamilySystem.instance.UpdateGuildFundzEvent += new RequestEventHandler<int> (OnGuildFundzEvent);
		for(int i=0;i<tabBtn.Count;i++)
		{
			UIManager.SetButtonEventHandler (tabBtn[i].gameObject, EnumButtonEvent.OnClick, OnClickTab,i,0);
		}

	}
	
    public void SetData(int id)
    {
		contents_ = FamilyData.AllL1Data();
		if (FamilySystem.instance.Buildings.Length < id-1)
			return;
		COM_GuildBuilding  build = FamilySystem.instance.Buildings[id-1];
		if (build == null)
			return;
		data_ = FamilyData.GetData (id, build.level_);
		if (data_ == null)
			return;
		name_.text = data_.name_;
		icon_.spriteName = data_.icon_;
		descLab.text = data_.desc_;
        gameObject.SetActive(true);
		levelImg.spriteName = "jz_"+build.level_;
		needGold_.text = data_.needMoney_.ToString();
		haveGold_.text = FamilySystem.instance.GuildData.fundz_.ToString();

	

		if(FamilySystem.instance.GuildData.fundz_ < data_.needMoney_)
		{
			levelUpBtn.isEnabled = false;
		}
		else
		{
			levelUpBtn.isEnabled = true;
		}

    }

	void Update () 
	{
	
	}

	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive(false);
	}

	private void OnClickTab(ButtonScript obj, object args, int param1, int param2)
	{
		SetBtnState (param1);
		SetData(contents_[param1].id_);
	}

	private void OnClickLevelUp(ButtonScript obj, object args, int param1, int param2)
	{
		if((GuildBuildingType)data_.type_ != GuildBuildingType.GBT_Main)
		{
			if(data_.level_ >=  FamilySystem.instance.Buildings[(int)GuildBuildingType.GBT_Main-1].level_)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("EN_LevelupGuildBuildingHallBuildLevelLess"));
				return;
			}
		}
		NetConnection.Instance.updateGuildBuiling ((GuildBuildingType)data_.type_);

	}

	void OnFamilyDataEvent(COM_GuildBuilding guild)
	{
		if (data_ == null)
			return;
		name_.text = data_.name_;
		icon_.spriteName = data_.icon_;
		gameObject.SetActive(true);
		levelImg.spriteName = "jz_"+guild.level_;
		needGold_.text = data_.needMoney_.ToString();
		haveGold_.text = FamilySystem.instance.GuildData.fundz_.ToString();
		if(FamilySystem.instance.GuildData.fundz_ < data_.needMoney_)
		{
			levelUpBtn.isEnabled = false;
		}
		else
		{
			levelUpBtn.isEnabled = true;
		}
		if((GuildBuildingType)data_.type_ == GuildBuildingType.GBT_Main)
		{
			if(GuildSystem.GetGuildMemberSelf ((int)GamePlayer.Instance.InstId).job_ <  (int)GuildJob.GJ_VicePremier)
			{
				levelUpBtn.isEnabled = false;
			}
		}
		else
		{
			if(GuildSystem.GetGuildMemberSelf ((int)GamePlayer.Instance.InstId).job_ <  (int)GuildJob.GJ_SecretaryHead)
			{
				levelUpBtn.isEnabled = false;
			}
		}
	}
	void OnGuildFundzEvent(int num)
	{
		needGold_.text = data_.needMoney_.ToString();
		haveGold_.text = FamilySystem.instance.GuildData.fundz_.ToString();
	}

	public void SetBtnState(int indx)
	{
		for(int i=0;i<tabBtn.Count;i++)
		{
			tabBtn[i].isEnabled = true;
		}
		tabBtn [indx].isEnabled = false;
	}

	void OnDestroy()
	{
		FamilySystem.instance.UpdateGuildBuildingEvent -= OnFamilyDataEvent;
		FamilySystem.instance.UpdateGuildFundzEvent -= OnGuildFundzEvent;
	}


}
