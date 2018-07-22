using UnityEngine;
using System.Collections;

public class FamilyCollectionUI : UIBase {

    public UILabel static_title_;
    public UILabel static_level_;
    public UILabel static_progress_;
    public UILabel static_rewarditem_;
    public UILabel static_sendeveryone_;
    public UILabel static_buybtnlbl_;
    public UILabel static_submitbtnlbl_;

    public UILabel static_detail_havenumlbl_;
    public UILabel static_detail_submitnumlbl_;
    public UILabel static_detail_maxlbl_;
    public UILabel static_detail_submitlbl_;

    public UILabel lv_;
    public UILabel cent_;
    public UISlider progress_;
    public UISprite rewardicon_;
    public UISprite buildingicon_;
    public UIButton buyBtn_;
    public UIButton submitBtn_;

    public GameObject submitDetail_;
    public UIButton submitDetailBtn_;
    public UILabel havenum_;
    public UILabel submitnum_;
    public UISprite icon_;

	public UIButton closeBtn;
	public UILabel name;
	public UISprite levelImg;
	public UIButton submitCloseBtn;
	public UIButton addBtn; 
	public UIButton subBtn;

	private int _maxNum;

	bool isShow;
	// Use this for initialization
	void Start () {
       /* static_title_.text = LanguageManager.instance.GetValue("");
        static_level_.text = LanguageManager.instance.GetValue("");
        static_progress_.text = LanguageManager.instance.GetValue("");
        static_rewarditem_.text = LanguageManager.instance.GetValue("");
        static_sendeveryone_.text = LanguageManager.instance.GetValue("");
        static_buybtnlbl_.text = LanguageManager.instance.GetValue("");
        static_submitbtnlbl_.text = LanguageManager.instance.GetValue("");

        static_detail_havenumlbl_.text = LanguageManager.instance.GetValue("");
        static_detail_submitnumlbl_.text = LanguageManager.instance.GetValue(""); ;
        static_detail_maxlbl_.text = LanguageManager.instance.GetValue(""); ;
        static_detail_submitlbl_.text = LanguageManager.instance.GetValue(""); ;
		*/

		FamilySystem.instance.FamilyDataEvent += new RequestEventHandler<int> (OnFamilyDataEvent);
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnCloseBtn, 0, 0);
		UIManager.SetButtonEventHandler (submitBtn_.gameObject, EnumButtonEvent.OnClick, OnSubmitBtn, 0, 0);
		UIManager.SetButtonEventHandler (buyBtn_.gameObject, EnumButtonEvent.OnClick, OnBuyBtn, 0, 0);
		UIManager.SetButtonEventHandler (submitCloseBtn.gameObject, EnumButtonEvent.OnClick, OnCloseSubmit, 0, 0);
		UIManager.SetButtonEventHandler (addBtn.gameObject, EnumButtonEvent.OnClick, OnAdd, 0, 0);
		UIManager.SetButtonEventHandler (subBtn.gameObject, EnumButtonEvent.OnClick, OnSub, 0, 0);
		UIManager.SetButtonEventHandler (submitDetailBtn_.gameObject, EnumButtonEvent.OnClick, OnSubmitDetail, 0, 0);
		UpdateInfo ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_FamilyCollection);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_FamilyCollection);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_FamilyCollection);
	}
	
	public override void Destroyobj ()
	{
		GameObject.Destroy (gameObject);
	}
	
	#endregion


	private void OnCloseBtn(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}

	public void UpdateInfo()
	{
		COM_GuildBuilding guildBuild = FamilySystem.instance.Buildings [(int)GuildBuildingType.GBT_Collection-1];
		if (guildBuild == null)
			return;
		FamilyData fData = FamilyData.GetData((int)GuildBuildingType.GBT_Collection,guildBuild.level_);
		if (fData == null)
			return;
		name.text = fData.name_;
		levelImg.spriteName = "jz_"+guildBuild.level_; 
		lv_.text = guildBuild.level_.ToString ();
		COM_Guild guild = FamilySystem.instance.GuildData;
		if (guild == null)
			return;
		cent_.text = guild.presentNum_ + "/" + fData.number_;
			
		progress_.value = (float)guild.presentNum_ / (float)fData.number_;

		ItemCellUI cell =  UIManager.Instance.AddItemCellUI (rewardicon_,(uint)fData.rewrod_);
		cell.showTips = true;
	}



	private void OnBuyBtn(ButtonScript obj, object args, int param1, int param2)
	{
		QuickBuyUI.ShowMe (3074);
	}

	private void OnSubmitBtn(ButtonScript obj, object args, int param1, int param2)
	{
		submitDetail_.gameObject.SetActive (true);

		ItemCellUI cell =  UIManager.Instance.AddItemCellUI (icon_,21351);
		cell.showTips = true;
		COM_Item item = BagSystem.instance.GetItemByItemId (21351);
		if(item == null)
		{
			havenum_.text ="0";
			_maxNum = 0;
			submitDetailBtn_.isEnabled = false;
		}
		else
		{
			havenum_.text =item.stack_.ToString();
			_maxNum = (int)item.stack_;
			submitDetailBtn_.isEnabled = true;
		}
		submitnum_.text ="0";
	}

	private void OnCloseSubmit(ButtonScript obj, object args, int param1, int param2)
	{
		submitDetail_.gameObject.SetActive (false);
	}

	private void OnAdd(ButtonScript obj, object args, int param1, int param2)
	{
		if(int.Parse(submitnum_.text) >= _maxNum)
		{
			submitnum_.text = _maxNum.ToString();
		}
		else
		{
			submitnum_.text = (int.Parse(submitnum_.text) +1).ToString();
		}
	}

	private void OnSub(ButtonScript obj, object args, int param1, int param2)
	{
		
		if(int.Parse(submitnum_.text) <= 0)
		{
			submitnum_.text = "0";
		}
		else
		{
			submitnum_.text = (int.Parse(submitnum_.text) -1).ToString();
		}
	}
	private void OnSubmitDetail(ButtonScript obj, object args, int param1, int param2)
	{
		if (int.Parse (submitnum_.text) <= 0)
			return;
		NetConnection.Instance.presentGuildItem (int.Parse(submitnum_.text));
		EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_familyCaiji, gameObject.transform, null,
		(GameObject obj1)=>{
			if(!submitDetail_.activeSelf)
			{
				Destroy(obj1);
			}
		});
	}

	void OnFamilyDataEvent(int num)
	{
		UpdateInfo ();

		COM_Item item = BagSystem.instance.GetItemByItemId (21351);
		if(item == null)
		{
			havenum_.text ="0";
			_maxNum = 0;
			submitDetailBtn_.isEnabled = false;
		}
		else
		{
			havenum_.text =item.stack_.ToString();
			_maxNum = (int)item.stack_;
			submitDetailBtn_.isEnabled = true;
		}
		submitnum_.text ="0";

	}

	void OnDestroy()
	{
		FamilySystem.instance.FamilyDataEvent -= OnFamilyDataEvent;
		
	}


		
}
