using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BabyTipsUI : MonoBehaviour {
	public UIButton fangshengBtn;
	public UILabel namelLabel;
	public UILabel levelLabel;
	public UITexture babyIcon;
	public UITexture RaceIcon;
	public GameObject skill_item;
	public GameObject close;
	public UISprite numSp;
	public UIButton btn;
	public UIGrid grid;
	public  bool isbabyList;
	public StorageBabyCell bcell;
	public List<SkillData> skillDatas = new List<SkillData>();
	private Baby _baby;
	public Baby baby
	{
		set
		{
			if(value != null)
			{
				_baby = value;
				namelLabel.text = _baby.InstName;
				levelLabel.text = _baby.GetIprop(PropertyType.PT_Level).ToString();
				HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData(_baby.GetIprop(PropertyType.PT_AssetId)).assetsIocn_, babyIcon);
				HeadIconLoader.Instance.LoadIcon (BabyData.GetData(_baby.GetIprop(PropertyType.PT_TableId))._RaceIcon, RaceIcon);
				this.gameObject.transform.FindChild("Bg").transform.FindChild("IconBg").GetComponent<UISprite>().spriteName =  
					BabyData.GetPetQuality(BabyData.GetData(_baby.GetIprop(PropertyType.PT_TableId))._PetQuality);
				BabyData bdata = BabyData.GetData(baby.GetIprop(PropertyType.PT_TableId));

				int Magic =   bdata._BIG_Magic - baby.gear_[(int)BabyInitGear.BIG_Magic];
				int Stama =   bdata._BIG_Stama - baby.gear_[(int)BabyInitGear.BIG_Stama];
				int Speed =   bdata._BIG_Speed - baby.gear_[(int)BabyInitGear.BIG_Speed];
				int Power =   bdata._BIG_Power - baby.gear_[(int)BabyInitGear.BIG_Power];
				int Strength =   bdata._BIG_Strength - baby.gear_[(int)BabyInitGear.BIG_Strength];
				int num = Magic+Stama+Speed+Power+Strength;
				numSp.spriteName = BabyData.GetBabyLeveSp(num);
				if(isbabyList)
				{
					btn.GetComponentInChildren<UILabel>().text = "存入";
					fangshengBtn.gameObject.SetActive(false);
				}else
				{
					btn.GetComponentInChildren<UILabel>().text = "取出";
					fangshengBtn.gameObject.SetActive(true);
				}
			}

			InitbabySkillData ();
		}
		get
		{
			return _baby;
		}
	}
	void OnEnable()
	{	

		//GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BagTipOpen);
	}
	void Start () {
		UIManager.SetButtonEventHandler (fangshengBtn.gameObject, EnumButtonEvent.OnClick, OnClickfangshengBtn,0, 0);
		UIManager.SetButtonEventHandler (btn.gameObject, EnumButtonEvent.OnClick, buttonClick,0,0);
		UIManager.SetButtonEventHandler (close.gameObject, EnumButtonEvent.OnClick,onClickclose,0,0);
		skill_item.SetActive (false);

	}
	void OnClickfangshengBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(baby.isLock)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("suodingchongwu"));
			return;
		}
		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("shifoufangsheng"),()=>{
			NetConnection.Instance.delStorageBaby ((uint)baby.InstId);
			gameObject.SetActive (false);
		});

	}
	private void buttonClick(ButtonScript obj, object args, int param1, int param2)
	{
		if(isbabyList)
		{

			NetConnection.Instance.depositBabyToStorage ((uint)baby.InstId);
			if(bcell != null)
			{
				bcell.raceIcon.gameObject.SetActive(true);
				bcell.icon.gameObject.SetActive(true);
				//BabybankUI.isqu = false;
			}
		}else
		{
			if(BankSystem.instance.IsBabyListFull())
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("EN_BabyFull"));
				return ;
			}
			NetConnection.Instance.storageBabyToPlayer((uint)baby.InstId);
			UIManager.RemoveButtonAllEventHandler(bcell.gameObject);
			bcell.raceIcon.gameObject.SetActive(false);
			bcell.icon.gameObject.SetActive(false);
			bcell.numsp.spriteName  = "";
			bcell.iconBack.spriteName = "cw_chongwutouxiang1";
			//BabybankUI.isqu = true;

		}

		gameObject.SetActive (false);
	}
	private void onClickclose(ButtonScript obj, object args, int param1, int param2)
	{
		obj.transform.parent.gameObject.SetActive (false);
	}
	void InitbabySkillData()
	{
		BabyData bdata = BabyData.GetData (baby.GetIprop(PropertyType.PT_TableId));
		for (int i = 0; i<baby.SkillInsts.Count; i++)
		{
			SkillData sdata = SkillData.GetMinxiLevelData((int)baby.SkillInsts[i].skillID_);
            if (sdata._Name.Equals(LanguageManager.instance.GetValue("playerPro_FightBack")))
			{
				continue;
			}
            if (sdata._Name.Equals(LanguageManager.instance.GetValue("playerPro_Dodge")))
			{
				continue;
			}
			skillDatas.Add(sdata);
		}
		AddSkillItem(bdata);
	}
	void AddSkillItem(BabyData bdata)
	{
		for (int i = 0; i < skillDatas.Count; ++i)
		{
			if (i > bdata._SkillNum)
				break; ///错误
			GameObject clone = GameObject.Instantiate(skill_item)as GameObject;
			clone.SetActive(true);
			clone.transform.parent = grid.transform;
			clone.transform.position = Vector3.zero;
			clone.transform.localScale = Vector3.one;
			UITexture te = clone.GetComponentInChildren<UITexture>();
			if(te != null)
			{
				HeadIconLoader.Instance.LoadIcon(skillDatas[i]._ResIconName, te);     
			}


		}
		GlobalInstanceFunction.Instance.Invoke (()=>{grid.Reposition ();},1);

	}
	void OnDisable()
	{
		ClearItem ();
		skillDatas.Clear ();
	}
	void ClearItem()
	{
		if(grid == null)
		{
			return;
		}
		foreach(Transform tr in grid.transform)
		{
			Destroy(tr.gameObject);
		}
	}

}
