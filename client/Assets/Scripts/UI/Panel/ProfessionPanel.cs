using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProfessionPanel : UIBase {

    public GameObject[] Profession_;

    public GameObject CloseBtn_;

	public UILabel _TitleLable;
	public UILabel _MercenaryLable;

	public List<ProfessionCellUI> profCellList = new List<ProfessionCellUI> ();
	public UISprite proIcon;
	public UILabel proLab;
	public UILabel openLevel;
	public UILabel descLab;
	public List<UILabel> equipNameList = new List<UILabel>();
	public List<UISprite> equipIconList = new List<UISprite>();
	public UIButton jobOkBtn;
	public UILabel recommendSkillLab;
	public UILabel recommendPropLab;
	public UITexture proImg;

    string[] pt = new string[] { "猎人", "法师", "牧师", "佣兵"};
    int crtSelect_;
    string[] questIds_;

	private ProfessionCellUI selectCell;


	// Use this for initialization2294 【11372 BUG】主线任务应该不可以放弃
	void Start () {
		//InitUIText ();
        crtSelect_ = 0;
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Profession) == (int)JobType.JT_Newbie)
		{
			NpcData nData = NpcData.GetData(NpcRenwuUI.NpcId);
			if (nData.BabySkillLearn != "")
			{
				questIds_ = nData.BabySkillLearn.Split(';');
			}
		}
		
		UIManager.SetButtonEventHandler(CloseBtn_.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		UIManager.SetButtonEventHandler(jobOkBtn.gameObject, EnumButtonEvent.OnClick, OnJobOk, 0, 0);
		for(int i =0;i<profCellList.Count;i++)
		{
			UIManager.SetButtonEventHandler(profCellList[i].gameObject, EnumButtonEvent.OnClick, OnProfessionCell, i, 0);
		}
		selectCell = profCellList [0];
		UpdateInfo (profCellList [0].jobId);

	}
	void InitUIText()
	{
		_TitleLable.text = LanguageManager.instance.GetValue("Profession_Title");
		_MercenaryLable.text = LanguageManager.instance.GetValue("Profession_Mercenary");
		/*_LocateLable.text = LanguageManager.instance.GetValue("Profession_Locate");
		_SkillsLable.text = LanguageManager.instance.GetValue("Profession_Skills");
		_AddBitLable.text = LanguageManager.instance.GetValue("Profession_AddBit");
		_HunterLable.text = LanguageManager.instance.GetValue("Profession_Hunter");
		_HSkillsLable.text = LanguageManager.instance.GetValue("Profession_Skills");
		_HAddBitLable.text = LanguageManager.instance.GetValue("Profession_AddBit");
		_HLocateLable.text = LanguageManager.instance.GetValue("Profession_Locate");
		_PriestLable.text = LanguageManager.instance.GetValue("Profession_Priest");
		_PAddBitLable.text = LanguageManager.instance.GetValue("Profession_AddBit");
		_PSkillsLable.text = LanguageManager.instance.GetValue("Profession_Skills");
		_PLocateLable.text = LanguageManager.instance.GetValue("Profession_Locate");
		_MasterLable.text = LanguageManager.instance.GetValue("Profession_Master");
		_MAddBitLable.text = LanguageManager.instance.GetValue("Profession_AddBit");
		_MSkillsLable.text = LanguageManager.instance.GetValue("Profession_Skills");
		_MLocateLable.text = LanguageManager.instance.GetValue("Profession_Locate");
		*/
	}

	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_ProfessionPanel);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_ProfessionPanel);
	}

	void OnProfessionCell(ButtonScript obj, object args, int param1, int param2)
	{
		if (selectCell != null) 
		{
			selectCell.back.spriteName = "jn_jinlan";
		}
		crtSelect_ = param1;
		profCellList[param1].back.spriteName = "jn_jinlanliang";
		selectCell = profCellList[param1];
		UpdateInfo(profCellList [param1].jobId);
	}

	private void UpdateInfo(int id)
	{
		Profession proData = Profession.GetData (id);
		if(proData == null)
			return;
		proLab.text = proData.jobName_;
		openLevel.text = proData.openLV_.ToString ();
		descLab.text = proData.Describe_;
		recommendSkillLab.text = proData.RecommendSkills1_;
		proIcon.spriteName = proData.jobtype_.ToString();
		string [] Attribute = proData.Recommand_.Split(';');
		recommendPropLab.text = "";
		for(int i = 0;i<Attribute.Length;i++)
		{
			string [] addStr = Attribute[i].Split(':');
			recommendPropLab.text+= LanguageManager.instance.GetValue( addStr[0]) +" +"+addStr[1] +"\n";
		}

		HeadIconLoader.Instance.LoadIcon (proData.proffImg, proImg);

	/*	for(int i=0;i<equipIconList.Count;i++)
		{
			equipIconList[i].gameObject.SetActive(false);
			equipNameList[i].gameObject.SetActive(false);
		}

		string [] equip = proData.RecommendEquippesIcon_.Split(';');
		for(int i=0;i<equip.Length;i++)
		{
			int itemId = int.Parse(equip[i]);
			equipIconList[i].gameObject.SetActive(true);
			ItemCellUI cell = UIManager.Instance.AddItemCellUI(equipIconList[i],(uint)itemId);
			cell.showTips = true;
			equipNameList[i].gameObject.SetActive(true);
			equipNameList[i].text = ItemData.GetData(itemId).name_;
		}
		*/
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level) < proData.openLV_)
		{
			jobOkBtn.gameObject.SetActive(false);
		}
		else
		{
			jobOkBtn.gameObject.SetActive(true);
		}

	}
	
	void OnJobOk(ButtonScript obj, object args, int param1, int param2)
    {
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Profession) == (int)JobType.JT_Newbie)
		{
	        if (QuestSystem.HasQuestByType(QuestKind.QK_Profession))
	        {
	            PopText.Instance.Show(LanguageManager.instance.GetValue("onlyOneJobQuest"), PopText.WarningType.WT_Warning);
	            //Hide();
	            return;
	        }
			if(IsJobQuestSame(int.Parse(questIds_[crtSelect_])))
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue( "bunengjie"));
				return;
			}
	        MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("IsToDoJob") + pt[crtSelect_], () =>
	        {
	            NetConnection.Instance.acceptQuest(int.Parse(questIds_[crtSelect_]));
	            Hide();
	        });
		}
		else
		{
			GameManager.Instance.ParseNavMeshInfo(Profession.GetData(selectCell.jobId).chuansong_);
		}
    }
	bool IsJobQuestSame(int quid)
	{
		QuestData qd = QuestData.GetData (quid);
		Profession pro = Profession.get ((JobType)GamePlayer.Instance.GetIprop (PropertyType.PT_Profession), GamePlayer.Instance.GetIprop (PropertyType.PT_ProfessionLevel));
		if(qd.questKind_ == QuestKind.QK_Profession)
		{
			if(qd.JobLevel_ == GamePlayer.Instance.GetIprop (PropertyType.PT_ProfessionLevel)&&qd.jobtype_ == (int)GamePlayer.Instance.GetIprop (PropertyType.PT_Profession))
			{
				return true;
			}
		}
		return false;
	}
    void OnClose(ButtonScript obj, object args, int param1, int param2)
    {
        Hide();
    }
	override public void Destroyobj()
	{
		
	}
}
