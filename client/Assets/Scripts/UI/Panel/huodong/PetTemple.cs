using UnityEngine;
using System.Collections.Generic;

public class PetTemple : UIBase {

    public UILabel timesLeft_;

    public UIButton catchBtn_;

    public UIButton closeBtn_;

    public PetDifficult diffPanel_;

    public UIGrid grid_;

    public GameObject bossPrefab_;

    int crtSelectIdx_;

    List<GameObject> itemPool_;

	public Transform mopos;
	public UILabel decLable;
	public UILabel nameLable;
	public GameObject SkillItem;
	public UIGrid skillGrid;
	public GameObject tipsObj;
	public UILabel skillName;
	public UILabel skillLevel;
	public UILabel skillXa;
	public UILabel skillDec;
	bool destroyed_;
	PetActivityData pdata;
	UIEventListener Listener;
	// Use this for initialization
	void Start () 
    {
		DaliyActivityData dad = DaliyActivityData.GetData (ActivityType.ACT_Pet);
		ActivitySystem.Instance.petTempTimes_ = dad.maxCount_;
		SkillItem.SetActive (false);
		if( ActivitySystem.Instance.petTempTimes_ - ActivitySystem.Instance.GetCount(ActivityType.ACT_Pet) <= 0)
		{
			catchBtn_.isEnabled = false;
		}else
		{
			catchBtn_.isEnabled = true;
		}
        crtSelectIdx_ = -1;
        diffPanel_.gameObject.SetActive(false);
        if (itemPool_ == null)
        {
            itemPool_ = new List<GameObject>();
        }
        
        UIManager.SetButtonEventHandler(catchBtn_.gameObject, EnumButtonEvent.OnClick, OnCatch, 0, 0);
        UIManager.SetButtonEventHandler(closeBtn_.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);

	    pdata = PetActivityData.GetData (1);
		BabyData baby = BabyData.GetData(pdata.monsterID_);
		if (baby != null)
		{
			nameLable.text = baby._Name;
			decLable.text = baby._Desc;
			GameManager.Instance.GetActorClone((ENTITY_ID)baby._AssetsID, (ENTITY_ID)0, EntityType.ET_Baby, (GameObject go, ParamData pData) =>			                                   {
				if (destroyed_)
				{
					Destroy(go);
					return;
				}
				go.transform.parent = mopos;
				go.transform.localScale = Vector3.one * EntityAssetsData.GetData(pData.iParam).zoom_ * 0.7f;
				go.transform.localPosition = new Vector3(0f, 0f, -90f);
				go.transform.localScale = new Vector3(550,550,550);
				go.transform.Rotate(Vector3.up, 180f);

			}, new ParamData(baby._AssetsID), "UI");

			for(int i =0;i<pdata.skillIDs_.Length;i++)
			{
				GameObject go = GameObject.Instantiate(SkillItem)as GameObject;
				go.SetActive(true);
				go.transform.parent = skillGrid.transform;
				go.transform.localScale = Vector3.one;
				UITexture tex = go.GetComponentInChildren<UITexture>();
				HeadIconLoader.Instance.LoadIcon(SkillData.GetMinxiLevelData(int.Parse(pdata.skillIDs_[i]))._ResIconName, tex);
				Listener = UIEventListener.Get(go);
				Listener.parameter = pdata.skillIDs_[i];
				Listener.onPress = buttonPress; 
			}
		}
        UpdateUI();
    }

    void UpdateUI()
    {
		timesLeft_.text = ActivitySystem.Instance.petTempTimes_.ToString();
        Dictionary<int, PetActivityData> metaData = PetActivityData.GetMetaData();
        GameObject go = null;
        PetTempleBossInfo bossInfo = null;
        int index = 0;
        bool isNew = false;
        foreach (PetActivityData data in metaData.Values)
        {
            if (itemPool_.Count > index)
            {
                go = itemPool_[index];
            }
            else
            {
                isNew = true;
                go = (GameObject)GameObject.Instantiate(bossPrefab_) as GameObject;
                go.transform.parent = grid_.transform;
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
                go.SetActive(true);
                itemPool_.Add(go);
            }
            bossInfo = go.GetComponent<PetTempleBossInfo>();
            bossInfo.SetData(data, index++);
            if (isNew)
            {
//                bossInfo.OnSelect += OnSelect;
            }
        }
        grid_.Reposition();
    }

    void OnDestroy()
    {
        destroyed_ = true;
    }

	void buttonPress(GameObject sender,bool isPressed)
	{
		if (isPressed)
		{
			tipsObj.SetActive(true);
			int str = int.Parse(UIEventListener.Get (sender).parameter.ToString());
			SkillData sdata = SkillData.GetMinxiLevelData(str);
			skillDec.text = sdata._Desc;
			skillName.text = sdata._Name;
			skillXa.text = sdata._Cost_mana.ToString()+" MP";
			skillLevel.text = sdata._Level.ToString();
		}
		else
		{
			//ApplicationEntry.Instance.ui3DCamera.depth = 1.2f;
			tipsObj.SetActive(false);
		}
	}

    public static void ShowMe()
    {
        UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_PetTemple);
    }

    public static void SwithShowMe()
    {
        UIBase.SwitchShowPanelByName(UIASSETS_ID.UIASSETS_PetTemple);
    }

    public static void HideMe()
    {
        UIBase.HidePanelByName(UIASSETS_ID.UIASSETS_PetTemple);
    }

    private void OnClose(ButtonScript obj, object args, int param1, int param2)
    {
        OpenPanelAnimator.CloseOpenAnimation(this.panel, () =>
        {
            Hide();
        });
    }

    private void OnCatch(ButtonScript obj, object args, int param1, int param2)
    {
		diffPanel_.UpdateUI(pdata);
        diffPanel_.gameObject.SetActive(true);
    }

    public override void Destroyobj()
    {
        UIManager.RemoveButtonEventHandler(catchBtn_.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(closeBtn_.gameObject, EnumButtonEvent.OnClick);
        BabyData baby = BabyData.GetData(pdata.monsterID_);
        if(baby != null)
            PlayerAsseMgr.DeleteAsset((ENTITY_ID)baby._AssetsID, false);
    }
}
