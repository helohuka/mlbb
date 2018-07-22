using UnityEngine;
using System.Collections;

public class Sellrecord : MonoBehaviour {

    public GameObject recordItem_;

    public UIGrid grid_;

    public UIButton closeBtn_;

	// Use this for initialization
	void Start () {
        UIManager.SetButtonEventHandler(closeBtn_.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
	}

    GameObject[] recordCache = new GameObject[AuctionHouseSystem.SellRecordMax];
    public void SetUp()
    {
        GameObject record = null;
        for (int i = 0; i < recordCache.Length; ++i)
        {
            if (i < AuctionHouseSystem.mySellRecordList_.Count)
            {
                if (recordCache[i] == null)
                {
                    record = GameObject.Instantiate(recordItem_) as GameObject;
                    record.GetComponent<RecordItem>().SetData(AuctionHouseSystem.mySellRecordList_[i]);
                    grid_.AddChild(record.transform);
                    record.transform.localScale = Vector3.one;
                    recordCache[i] = record;
                    record.SetActive(true);
                }
                else
                {
                    recordCache[i].GetComponent<RecordItem>().SetData(AuctionHouseSystem.mySellRecordList_[i]);
                    recordCache[i].SetActive(true);
                }
            }
            else
            {
                if(recordCache[i] != null)
                    recordCache[i].SetActive(false);
            }
        }
        gameObject.SetActive(true);
        grid_.Reposition();
    }

    void OnClose(ButtonScript obj, object args, int param1, int param2)
    {
        gameObject.SetActive(false);
    }

}
