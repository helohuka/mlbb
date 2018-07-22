using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class familyCheckMonsterUI : UIBase
{
	public UIButton closeBtn;
	public UILabel nameLab;
	public UILabel levelLab;
	public UILabel expLab;
	public Transform mpos;
	public List<UISprite> skillIcon = new List<UISprite> ();  
	public UILabel attackLab;
	public UILabel defenseLab;
	public UILabel agileLab;
	public UILabel spiritLab;
	public UILabel replyLab;  
	public UIProgressBar fireBar;
	public UIProgressBar waterBar;
	public UIProgressBar windBar;
	public UIProgressBar groundBar;

	private GameObject babyObj;
	public static COM_GuildProgen _progen;

	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		Monster = _progen;
        UIPanel panel = GetComponent<UIPanel>();
        if (panel != null)
        {
            panel.renderQueue = UIPanel.RenderQueue.StartAt;
            panel.startingRenderQueue = 3000;
        }
	}

	void Update ()
	{

	}

	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe ()  
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_FamilyMonsterCheck);
	}
	
	public static void ShowMe(COM_GuildProgen progen)
	{
		_progen = progen;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_FamilyMonsterCheck );
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_FamilyMonsterCheck );
	}
	
	public override void Destroyobj ()
	{
		GameObject.Destroy (gameObject);
	}
	
	#endregion

	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}

	public COM_GuildProgen Monster
	{
		set
		{
			_progen = value;
			if(_progen != null)
			{
				familyMonsterData bData = familyMonsterData.GetData(_progen.mId_,_progen.lev_);
				if(bData == null)
					return;
				nameLab.text = bData._Name;
				levelLab.text = _progen.lev_.ToString();
				expLab.text =_progen.exp_+"/"+ bData._LevelExp;
				GameManager.Instance.GetActorClone((ENTITY_ID)bData._AssetsID, (ENTITY_ID)0, EntityType.ET_Baby, AssetLoadCallBack, null, "UI");
				attackLab.text = bData._PT_Attack.ToString();
				defenseLab.text = bData._PT_Defense.ToString();
				agileLab.text = bData._PT_Agile.ToString();
				spiritLab.text = bData._PT_Spirit.ToString();
				replyLab.text = bData._PT_Reply.ToString();
				fireBar.value = bData._Fire/10;
				waterBar.value = bData._Water/10;
				windBar.value = bData._Wind/10;
				groundBar.value = bData._Ground/10;
				for(int i =0;i<bData._Skills.Count;i++)
				{

					SkillCellUI cell = UIManager.Instance.AddSkillCellUI(skillIcon[i],SkillData.GetData(int.Parse(bData._Skills[i][0]),int.Parse(bData._Skills[i][1])));
					cell.showTips = true;
					//HeadIconLoader.Instance.LoadIcon( SkillData.GetData(int.Parse(bData._Skills[i][0]),int.Parse(bData._Skills[i][1]))._ResIconName,skillIcon[i].transform.FindChild("skillicon").GetComponent<UITexture>());
				}
			}
		}
		get
		{
			return _progen;
		}
	}

	void AssetLoadCallBack(GameObject ro, ParamData data)
	{
		if (gameObject == null || !this.gameObject.activeSelf)
		{
			Destroy(ro);
			PlayerAsseMgr.DeleteAsset((ENTITY_ID)data.iParam, false);
			return;
		}
		if(babyObj != null)
		{
			Destroy(ro);
			PlayerAsseMgr.DeleteAsset((ENTITY_ID)data.iParam, false);
			return;
		}
		ro.transform.parent = mpos;
		ro.transform.localScale = new Vector3(250f,250f,250f);
		ro.transform.localPosition = Vector3.forward * -40;
		ro.transform.localRotation = Quaternion.Euler (10f, 180f, 0f);
		babyObj = ro;
	}

}

