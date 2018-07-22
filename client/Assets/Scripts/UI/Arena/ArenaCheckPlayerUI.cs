using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public delegate void closeArenaCheckEvent();

public class ArenaCheckPlayerUI : MonoBehaviour
{
	public closeArenaCheckEvent callBack;
	public UIButton closeBtn;
	public UILabel nameLab;
	public UILabel levelLab;
	public UILabel proLab;
	public List<UITexture> equip = new List<UITexture>();
	public List<UISprite> employes = new List<UISprite> ();
	public List<UISprite> babies = new List<UISprite> ();
	public Transform Mpos; 
	public UIButton skillBtn;
	public UIPanel skilList;
	public UIButton skilListCloseBtn;
	public GameObject  skillCell;
	public UIGrid skillGrid;
	public UIButton friendBtn;

	public UILabel arenaCheckPlayerTitleLab;
	public UILabel arenaCheckPlayerNameLab;
	public UILabel arenaCheckPlayerJobLab;
	public UILabel arenaCheckPlayerLevelLab;
	public UILabel arenaCheckPlayerEmployeeLab;
	public UILabel arenaCheckPlayerBabyLab;


	private COM_SimplePlayerInst _playerInst;
	private GameObject babyObj;
	private List<GameObject> skillCellList = new List<GameObject>();
	private List<string> _icons = new List<string>();
	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		UIManager.SetButtonEventHandler (skillBtn.gameObject, EnumButtonEvent.OnClick, OnSkillBtn, 0, 0);
		UIManager.SetButtonEventHandler (friendBtn.gameObject, EnumButtonEvent.OnClick, OnFriendBtn, 0, 0);
		UIManager.SetButtonEventHandler (skilListCloseBtn.gameObject, EnumButtonEvent.OnClick, OnSkillCloseBtn, 0, 0);
	}

	public void ShowPanel(COM_SimplePlayerInst inst)
	{
		//ApplicationEntry.Instance.ui3DCamera.depth = -1;
		this.gameObject.SetActive (true);
		UIManager.Instance.AdjustUIDepth (gameObject.transform);
		PlayerInfo = inst;
	}

    public COM_SimplePlayerInst PlayerInfo
    {
        set
        {
            if (value != null)
            {
                _playerInst = value;
                nameLab.text = _playerInst.instName_;
                levelLab.text = _playerInst.properties_[(int)PropertyType.PT_Level] + "";
				proLab.text = Profession.get((JobType)_playerInst.properties_[(int)PropertyType.PT_Profession], 
				(int) _playerInst.properties_[(int)PropertyType.PT_ProfessionLevel]).jobName_;//Profession.get(JobType.JT_Axe, 1).jobName_;//

                ENTITY_ID weaponAssetId = 0;
                if (GlobalInstanceFunction.Instance.WeaponID(_playerInst) != 0)
                    weaponAssetId = (ENTITY_ID)ItemData.GetData(GlobalInstanceFunction.Instance.WeaponID(_playerInst)).weaponEntityId_;
                GameManager.Instance.GetActorClone((ENTITY_ID)_playerInst.properties_[(int)PropertyType.PT_AssetId], weaponAssetId, EntityType.ET_Player, AssetLoadCallBack, null, "UI", GlobalInstanceFunction.Instance.GetDressId(_playerInst.equips_));

                for (int i = 0; i < _playerInst.equips_.Length; i++)
                {
                    if (_playerInst.equips_[i] != null)
                    {
                        equip[i + 1].gameObject.SetActive(true);
                        HeadIconLoader.Instance.LoadIcon(ItemData.GetData((int)_playerInst.equips_[i].itemId_).icon_, equip[(int)ItemData.GetData((int)_playerInst.equips_[i].itemId_).slot_]);
                        if (!_icons.Contains(ItemData.GetData((int)_playerInst.equips_[i].itemId_).icon_))
                        {
                            _icons.Add(ItemData.GetData((int)_playerInst.equips_[i].itemId_).icon_);
                        }

                    }
                    else
                    {
                        equip[i + 1].gameObject.SetActive(false);
                    }
                }

                for (int i = 0; i < employes.Count; i++)
                {
                    employes[i].gameObject.SetActive(false);
                }

                for (int i = 0; i < babies.Count; i++)
                {
                    babies[i].gameObject.SetActive(false);
                }

                for (int i = 0; i < _playerInst.battleEmps_.Length; i++)
                {
                    employes[i].gameObject.SetActive(true);
                    employes[i].transform.Find("name").GetComponent<UILabel>().text = _playerInst.battleEmps_[i].instName_;
                    //	employes[i].transform.Find("pross").GetComponent<UILabel>().text =  LanguageManager.instance.GetValue(((JobType)_playerInst.battleEmps_[i].properties_[(int)PropertyType.PT_Profession]).ToString());
                    HeadIconLoader.Instance.LoadIcon(
                        EntityAssetsData.GetData(EmployeeData.GetData((int)_playerInst.battleEmps_[i].properties_[(int)PropertyType.PT_TableId]).asset_id).assetsIocn_,
                        employes[i].transform.Find("icon").Find("icon").GetComponent<UITexture>());
                    employes[i].transform.Find("job").GetComponent<UISprite>().spriteName =
                        EmployeeData.GetData((int)_playerInst.battleEmps_[i].properties_[(int)PropertyType.PT_TableId]).professionType_.ToString();

                    if (!_icons.Contains(EntityAssetsData.GetData(EmployeeData.GetData((int)_playerInst.battleEmps_[i].properties_[(int)PropertyType.PT_TableId]).asset_id).assetsIocn_))
                    {
                        _icons.Add(EntityAssetsData.GetData(EmployeeData.GetData((int)_playerInst.battleEmps_[i].properties_[(int)PropertyType.PT_TableId]).asset_id).assetsIocn_);
                    }

                    employes[i].transform.Find("icon").GetComponent<UISprite>().spriteName = EmployessSystem.instance.GetQualityBack((int)_playerInst.battleEmps_[i].quality_);

                }

                for (int i = 0; i < _playerInst.babies1_.Length; i++)
                {
                    babies[i].gameObject.SetActive(true);
                    HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData(BabyData.GetData((int)_playerInst.babies1_[i].properties_[(int)PropertyType.PT_TableId])._AssetsID).assetsIocn_, babies[i].transform.FindChild("icon").GetComponent<UITexture>());

                    HeadIconLoader.Instance.LoadIcon(BabyData.GetData((int)_playerInst.babies1_[i].properties_[(int)PropertyType.PT_TableId])._RaceIcon, babies[i].transform.FindChild("zhongzuIcon").GetComponent<UITexture>());
                    if (!_icons.Contains(EntityAssetsData.GetData(BabyData.GetData((int)_playerInst.babies1_[i].properties_[(int)PropertyType.PT_TableId])._AssetsID).assetsIocn_))
                    {
                        _icons.Add(EntityAssetsData.GetData(BabyData.GetData((int)_playerInst.babies1_[i].properties_[(int)PropertyType.PT_TableId])._AssetsID).assetsIocn_);
                    }

                    babies[i].transform.FindChild("Label").GetComponent<UILabel>().text = "LV: " + (int)_playerInst.babies1_[i].properties_[(int)PropertyType.PT_Level];

                    if (!_icons.Contains(EntityAssetsData.GetData(BabyData.GetData((int)_playerInst.babies1_[i].properties_[(int)PropertyType.PT_TableId])._AssetsID).assetsIocn_))
                    {
                        _icons.Add(EntityAssetsData.GetData(BabyData.GetData((int)_playerInst.babies1_[i].properties_[(int)PropertyType.PT_TableId])._AssetsID).assetsIocn_);
                    }

                }

            }
        }
        get
        {
            return _playerInst;
        }
    }

	void AssetLoadCallBack(GameObject ro, ParamData data)
	{
		//NGUITools.SetChildLayer(ro.transform, LayerMask.NameToLayer("3D"));
		//ro.transform.parent = Mpos;
		//ro.transform.localScale = new Vector3(300,300,300);
		//ro.transform.localPosition = Vector3.zero;
		//ro.transform.localRotation = Quaternion.Euler (10f, 180f, 0f);


		ro.transform.parent = Mpos;
		ro.transform.localPosition = Vector3.forward * -100f;
		ro.transform.localScale = new Vector3(400f,400f,1f);
		ro.transform.localRotation = Quaternion.Euler (0f, 180f, 0f);
		//EffectLevel el =ro.AddComponent<EffectLevel>();
		//el.target =ro.transform.parent.parent.GetComponent<UISprite>();


		if(babyObj != null)
		{
			Destroy (babyObj);
			babyObj = null;
		}
		babyObj = ro;
	}


	private void UpdateSkillList()
	{
		for(int i =0;i<skillCellList.Count;i++)
		{
			skillGrid.RemoveChild(skillCellList[i].transform);
			skillCellList[i].transform.parent = null;
			GameObject.Destroy(skillCellList[i]);
			skillCellList[i]= null;

		}
		skillCellList.Clear ();
		for(int i =0;i<_playerInst.skill_.Length;i++)
		{
			SkillData sData =SkillData.GetData((int) _playerInst.skill_[i].skillID_,(int)_playerInst.skill_[i].skillLevel_);
			if(sData._SkillType == SkillType.SKT_Active || sData._SkillType == SkillType.SKT_Passive ||sData._SkillType == SkillType.SKT_CannotUse)
			{
				GameObject obj = Object.Instantiate(skillCell.gameObject) as GameObject;
				obj.SetActive(true);
	            HeadIconLoader.Instance.LoadIcon(SkillData.GetData((int)_playerInst.skill_[i].skillID_, (int)_playerInst.skill_[i].skillLevel_)._ResIconName, obj.transform.Find("Sprite").transform.Find("icon").GetComponent<UITexture>());
				if(!_icons.Contains(SkillData.GetData((int)_playerInst.skill_[i].skillID_, (int)_playerInst.skill_[i].skillLevel_)._ResIconName))
				{
					_icons.Add(SkillData.GetData((int)_playerInst.skill_[i].skillID_, (int)_playerInst.skill_[i].skillLevel_)._ResIconName);
				}


				obj.transform.Find("name").GetComponent<UILabel>().text = SkillData.GetData((int)_playerInst.skill_[i].skillID_,(int) _playerInst.skill_[i].skillLevel_)._Name;
	            obj.transform.Find("desc").GetComponent<UILabel>().text = SkillData.GetData((int)_playerInst.skill_[i].skillID_, (int)_playerInst.skill_[i].skillLevel_)._Level.ToString();
				obj.transform.parent =skillGrid.transform;
				skillCellList.Add(obj);
				obj.transform.localScale = Vector3.one;
			}
		}
		skillGrid.Reposition();
	}

	private void OnClose(ButtonScript obj, object args, int param1, int param2)
	{
		this.gameObject.SetActive(false);
		if(callBack != null)
		{
			callBack();
		}
		//ApplicationEntry.Instance.ui3DCamera.depth = 1;
	}

	private void OnSkillBtn(ButtonScript obj, object args, int param1, int param2)
	{
		skilList.gameObject.SetActive (true);
		Mpos.gameObject.SetActive (false);
		UpdateSkillList ();
	}

	private void OnFriendBtn(ButtonScript obj, object args, int param1, int param2)
	{
		int fMax = 0;
		GlobalValue.Get(Constant.C_FriendMax, out fMax);
		if(FriendSystem.Instance().friends_.Count>= fMax)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue( "haoyoumax"));
			return;
		}

		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue( "addfriend").Replace("{n}",PlayerInfo.instName_), () => {

			NetConnection.Instance.addFriend(PlayerInfo.instId_);
		});
	}

	private void OnSkillCloseBtn(ButtonScript obj, object args, int param1, int param2)
	{
		skilList.gameObject.SetActive (false);
		Mpos.gameObject.SetActive (true);
	}
	void OnDestroy()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}

}

