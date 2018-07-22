using UnityEngine;
using System.Collections;

public class CollectGood : MonoBehaviour {

    public UILabel name_;

    public UISprite icon_;

    public UIButton cancelCollectBtn_;

    public int id_;

    public bool isItem_;

	// Use this for initialization
	void Start () {
	    //UIManager.SetButtonEventHandler(cancelCollectBtn_.gameObject, EnumButtonEvent.OnClick, OnUnCollect, 0, 0);
	}

    void OnUnCollect(ButtonScript obj, object args, int param1, int param2)
    {
        UIGrid grid = transform.parent.GetComponent<UIGrid>();
        AuctionHouseSystem.RemoveCollection(grid.GetIndex(transform) - 1);
        grid.RemoveChild(transform);
        gameObject.SetActive(false);
    }

    public void SetData(string info)
    {
        string[] infos = info.Split('|');
        if (bool.Parse(infos[1]))
        {
            ItemData data = ItemData.GetData(int.Parse(infos[0]));
            name_.text = data.name_;
            UIManager.Instance.AddItemCellUI(icon_, (uint)data.id_);
        }
        else
        {
            BabyData baby = BabyData.GetData(int.Parse(infos[0]));
            name_.text = baby._Name;
            UIManager.Instance.AddBabyCellUI(icon_, baby);
        }
        gameObject.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
