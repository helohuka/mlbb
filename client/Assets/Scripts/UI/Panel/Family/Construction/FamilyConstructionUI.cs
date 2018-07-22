using UnityEngine;
using System.Collections.Generic;

public class FamilyConstructionUI : MonoBehaviour {

    public UILabel static_haveGold_;

    public UIGrid grid_;

    public GameObject item_;

    public UILabel gold_;

    public ConstructionDetail detail_;

    List<FamilyData> contents_;

    List<GameObject> itemPool_;

    bool dirty_;


    void Start()
    {
		Show ();
		FamilySystem.instance.FamilyDataEvent += new RequestEventHandler<int> (OnFamilyDataEvent);
		FamilySystem.instance.UpdateGuildBuildingEvent += new RequestEventHandler<COM_GuildBuilding> (OnFamilyGuildBuilding);
        //static_haveGold_.text = LanguageManager.instance.GetValue("");
    }

    public void Show()
    {
        if (contents_ == null)
        {
            contents_ = FamilyData.AllL1Data();
            itemPool_ = new List<GameObject>();
            dirty_ = true;
        }

        gameObject.SetActive(true);
    }

    void Update()
    {
        if (dirty_)
        {
            ClearEvt();
            ConstructionItem cItem = null;
            for (int i = 0; i < contents_.Count; ++i)
            {
                if (i > itemPool_.Count - 1)
                {
                    GameObject go = (GameObject)GameObject.Instantiate(item_) as GameObject;
                    cItem = go.GetComponent<ConstructionItem>();
					go.SetActive(true);
					grid_.AddChild(go.transform);
					go.transform.localScale = Vector3.one;
					itemPool_.Add(go);
                }
                else
                {
                    cItem = itemPool_[i].GetComponent<ConstructionItem>();
                }

                //set data
                cItem.SetData(contents_[i]);
                UIEventListener listener = UIEventListener.Get(cItem.lvbtn_.gameObject);
				listener.parameter = i;
                listener.onClick += OnClickItem;
            }

            detail_.gameObject.SetActive(false);
			grid_.Reposition();
            dirty_ = false;
        }
    }

    void OnClickItem(GameObject go)
    {
        int idx = (int)UIEventListener.Get(go).parameter;
        detail_.SetData(contents_[idx].id_);
		detail_.SetBtnState (idx);
		UIManager.Instance.AdjustUIDepth (detail_.transform);
    }

    void ClearEvt()
    {
        ConstructionItem cItem = null;
        for (int i = 0; i < itemPool_.Count; ++i)
        {
            cItem = itemPool_[i].GetComponent<ConstructionItem>();
            UIEventListener.Get(cItem.lvbtn_.gameObject).onClick -= OnClickItem;
        }
    }

	void OnFamilyDataEvent(int num)
	{
		ConstructionItem cItem = null;
		for (int i = 0; i < itemPool_.Count; ++i)
		{
			cItem = itemPool_[i].GetComponent<ConstructionItem>();
			if(cItem != null)
				cItem.UpdataInfo();
		}

		if(detail_.gameObject.activeSelf)
		{
			detail_.SetData(detail_.data_.id_);
			EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_familyLevelUp, detail_.transform, () =>{});
		}
	}

	void OnFamilyGuildBuilding(COM_GuildBuilding g)
	{
		OnFamilyDataEvent (1);
	}


	public void Close()
	{
		ClearEvt();
        dirty_ = false;
        gameObject.SetActive(false);
    }

	void OnDestroy()
	{
		FamilySystem.instance.FamilyDataEvent -= OnFamilyDataEvent;
		FamilySystem.instance.UpdateGuildBuildingEvent -= OnFamilyGuildBuilding;

	}

}
