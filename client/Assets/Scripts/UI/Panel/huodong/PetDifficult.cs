using UnityEngine;
using System.Collections.Generic;

public class PetDifficult : MonoBehaviour {

    public UIButton closeBtn_;

    public UIGrid grid_;

    public GameObject item_;

    List<GameObject> itemPool_;

	// Use this for initialization
	void Start () {
	    
	}

    public void UpdateUI(PetActivityData data)
    {
        if (itemPool_ == null)
            itemPool_ = new List<GameObject>();

        string[] diffc = data.difficults_;
        GameObject go = null;
        PetDifficultItem pdiffc = null;
        bool isNew = false;
        for (int i = 0; i < diffc.Length; ++i)
        {
            if (i < itemPool_.Count)
            {
                go = itemPool_[i];
            }
            else
            {
                isNew = true;
                go = (GameObject)GameObject.Instantiate(item_) as GameObject;
                go.transform.parent = grid_.transform;
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
                go.SetActive(true);
                itemPool_.Add(go);
            }

            pdiffc = go.GetComponent<PetDifficultItem>();
            int lvLmt = 0;
            if (data.levels_.Length > i)
                lvLmt = int.Parse(data.levels_[i]);
            pdiffc.SetData(i, int.Parse(diffc[i]), lvLmt);

            if (isNew)
                pdiffc.OnSelectDifficult += OnSelectDifficult;
        }
        grid_.Reposition();
    }

    void OnSelectDifficult(int diff)
    {
        if (itemPool_ == null || diff < 0 || diff > itemPool_.Count)
            return;

        gameObject.SetActive(false);
   }

    void OnEnable()
    {
        UIManager.SetButtonEventHandler(closeBtn_.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
    }

    void OnDisable()
    {
        UIManager.RemoveButtonEventHandler(closeBtn_.gameObject, EnumButtonEvent.OnClick);
    }

    private void OnClose(ButtonScript obj, object args, int param1, int param2)
    {
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        PetDifficultItem pdiffc = null;
        for (int i = 0; i < itemPool_.Count; ++i)
        {
            pdiffc = itemPool_[i].GetComponent<PetDifficultItem>();
            pdiffc.OnSelectDifficult -= OnSelectDifficult;
        }
    }
	

}
