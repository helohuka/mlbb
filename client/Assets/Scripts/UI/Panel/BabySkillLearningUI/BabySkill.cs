using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BabySkill : MonoBehaviour {

	public delegate void Refresh(int bid);
	public static Refresh RefreshbabyData;
	public UIButton CloseBtn;
	public GameObject item;
	public UIGrid grid;

	public GameObject skillDecObj;

	//public static int babyId;
	private int SkillCount = 10;
	public List<SkillData> skillDatas = new List<SkillData>();
	public List<int>skillIds = new List<int>();
	public List<GameObject>items = new List<GameObject>();
	public delegate void BabyLearnOk(int slot,int skId);
	public static BabyLearnOk BabyskillLearnOk; 
	private int itemId;
	Baby Inst;
	void Start () {
		item.SetActive (false);
		BabyskillLearnOk = BabyskillLearn;
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose,0, 0);

		RefreshbabyData = RefreshItem;
		RefreshItem (BabyList.babyId);
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BabyLearningSkill_BabySkillUI);
	}
	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}
	int LearnType;

	void RefreshItem(int babyId)
	{
		if (grid == null)
						return;
		foreach(Transform tra in grid.transform)
		{
			Destroy(tra.gameObject);
		}
        skillIds.Clear();
		skillDatas.Clear ();
		items.Clear ();
	    Inst = GamePlayer.Instance.GetBabyInst(babyId);
		BabyData bdata = BabyData.GetData (Inst.GetIprop(PropertyType.PT_TableId));
		for (int i = 0; i<Inst.SkillInsts.Count; i++)
		{
            SkillData sdata = SkillData.GetMinxiLevelData((int)Inst.SkillInsts[i].skillID_);
            if (sdata._Name.Equals(LanguageManager.instance.GetValue("playerPro_FightBack")))
			{
				continue;
			}
            if (sdata._Name.Equals(LanguageManager.instance.GetValue("playerPro_Dodge")))
			{
				continue;
			}
			if(!skillDatas.Contains(sdata))
			skillDatas.Add(sdata);
		}


		for(int i =0;i<bdata._SkillNum;i++)
		{
			GameObject o = GameObject.Instantiate(item)as GameObject;
			o.SetActive(true);
			o.name = o.name+i;
			o.transform.parent = grid.transform;
			o.transform.localScale= new Vector3(1,1,1);	
			BSkillCell bcell = o.GetComponent<BSkillCell>();
//			if(i<bdata.skillNum_)
//			{
				if(i<skillDatas.Count)
				{
					LearnType = 1;
					bcell.SData = skillDatas[i];
				bcell.SetUIDisableb(true);
//					bcell.TishiLabel.gameObject.SetActive(false);
//					bcell.IconSuo.gameObject.SetActive(false);
					UIManager.SetButtonParam(o.gameObject,0,i);
					skillIds.Add(skillDatas[i]._Id);
				}else
				{
					LearnType = 2;
				bcell.SetUIDisableb(false);
//					bcell.TishiLabel.text ="学习技能";
//					bcell.TishiLabel.gameObject.SetActive(false);
//					bcell.nameLabel.gameObject.SetActive(false);
//					bcell.LevelLabel.gameObject.SetActive(false);
//					bcell.Icon.gameObject.SetActive(true);
//					bcell.IconSuo.gameObject.SetActive(false);
//					bcell.IconKuang.gameObject.SetActive(true);
				}
//			}
//			else
//			{

//				LearnType = 3;
//				bcell.TishiLabel.text ="尚未解锁";
//				bcell.TishiLabel.gameObject.SetActive(false);
//				bcell.nameLabel.gameObject.SetActive(false);
//				bcell.LevelLabel.gameObject.SetActive(false);
//				bcell.Icon.gameObject.SetActive(false);
//				bcell.IconKuang.gameObject.SetActive(true);
//				bcell.IconSuo.gameObject.SetActive(true);
//			}
			UIManager.SetButtonEventHandler (o, EnumButtonEvent.OnClick, OnClickLearn,LearnType, i);
				items.Add(o);
			grid.repositionNow = true;

            if(i == 2)
                GuideManager.Instance.RegistGuideAim(o, GuideAimType.GAT_ThirdLearningBabySkill_SkillSlot);
		}

	}
	void OnClickLearn(ButtonScript obj, object args, int param1, int param2)
	{
		itemId = param2;
			if (param1 == 1)
			{
                if (SkillData.GetMinxiLevelData(skillIds[param2])._Name.Equals(LanguageManager.instance.GetValue("playerPro_Attack")) || SkillData.GetMinxiLevelData(skillIds[param2])._Name.Equals(LanguageManager.instance.GetValue("playerPro_Defense")))
			{
                PopText.Instance.Show(LanguageManager.instance.GetValue("skillDontReplace"), PopText.WarningType.WT_Tip);
			}else
			{
				GuideManager.Instance.ClearMask();
				skillDecObj.SetActive(true);
				BabySkillEnter ben = skillDecObj.GetComponent<BabySkillEnter>(); 
				SkillData newSkdata = SkillData.GetData(BabySkillLearning.SkillId,BabySkillLearning.newLevel);
				SkillData oldSkdata = SkillData.GetMinxiLevelData(skillIds[param2]);
				ben.newData = newSkdata;
				ben.curData = oldSkdata;
				ben.newSkillId = BabySkillLearning.SkillId;
				ben.newSkillLevel = BabySkillLearning.newLevel;
				ben.oldskillId = skillIds[param2];
//
//
//                MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("IsReplaceSkill"), () =>
//                {
//					if(GamePlayer.Instance.Properties[(int)PropertyType.PT_Money]<BabySkillLearning.Price) 
//					{
//						//ErrorTipsUI.ShowMe("金币不足");
//                        PopText.Instance.Show(LanguageManager.instance.GetValue("nomoney"));
//					}else if(!IsArrivalsLevel())
//					{
//						//ErrorTipsUI.ShowMe("等级不足");
//                        PopText.Instance.Show(LanguageManager.instance.GetValue("EN_OpenBaoXiangLevel"));
//					}else
//					{
//						if(param2>=skillDatas.Count)
//						{
//							NetConnection.Instance.babyLearnSkill((uint)BabyList.babyId,0,(uint)BabySkillLearning.SkillId,(uint)BabySkillLearning.newLevel);
//						}else
//						{
//							NetConnection.Instance.babyLearnSkill((uint)BabyList.babyId,(uint)skillIds[param2],(uint)BabySkillLearning.SkillId,(uint)BabySkillLearning.newLevel);
//						}
//						
//						
//					}
//					
//				});
			}

				
			}
            else if(param1 ==2)
			{
                MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("learnNewSkill"), () =>
                {
				if(GamePlayer.Instance.Properties[(int)PropertyType.PT_Money]<BabySkillLearning.Price) 
				{
					PopText.Instance.Show (LanguageManager.instance.GetValue("nomoney"));
				}
                else if(!IsArrivalsLevel())
				{
                    PopText.Instance.Show(LanguageManager.instance.GetValue("chongwudengjibuzu"));
				}else
				{
					if(param2>=skillDatas.Count)
					{
						NetConnection.Instance.babyLearnSkill((uint)BabyList.babyId,0,(uint)BabySkillLearning.SkillId,(uint)BabySkillLearning.newLevel);
					}else
					{
						NetConnection.Instance.babyLearnSkill((uint)BabyList.babyId,(uint)skillIds[param2],(uint)BabySkillLearning.SkillId,(uint)BabySkillLearning.newLevel);
					}

				}
					
				});
			}

	}
	bool IsArrivalsLevel()
	{
		SkillData sdata = SkillData.GetMinxiLevelData (BabySkillLearning.SkillId);
		if(sdata._IsPhysic == true)
		{
			int ilevel = GamePlayer.Instance.GetBabyInst(BabyList.babyId).GetIprop(PropertyType.PT_Level);
            int slevel = ilevel / 20 + 1;
			return (slevel >= sdata._Level);
		}
		else
		{
			int ilevel = GamePlayer.Instance.GetBabyInst(BabyList.babyId).GetIprop(PropertyType.PT_Level);
            int slevel = ilevel / 10 + 1;
            return (slevel >= sdata._Level);
		}
	}

	void BabyskillLearn(int nbabyId,int newskId)
	{
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BabyLearningSkillOk);
		BSkillCell bcell = items [itemId].GetComponent<BSkillCell> ();
		if(bcell.SData !=null)
		{
		  Inst.SkillInsts.RemoveAt(itemId);
		}
					
		RefreshItem (BabyList.babyId);
        PopText.Instance.Show(LanguageManager.instance.GetValue("learnSuccess"));
        ChatSystem.PushSystemMessage(LanguageManager.instance.GetValue("chongwuxude").Replace("{n}", SkillData.GetMinxiLevelData(newskId)._Name));
		BabySkillLearning.Instance.CloseBabyListObj ();
		BabySkillLearning.Instance.CloseBabyskillObj ();
	}

}
