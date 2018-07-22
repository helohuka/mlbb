using UnityEngine;
using System.Collections;

public class MA_CardsItem : MonoBehaviour {

    public UILabel name;
    public GameObject mask;
    public GameObject got;
    public UISprite icon;

    COM_Item reward;
    bool brandNew;
    Animation anim;

	// Use this for initialization
	void Start () {
        UpdateContent();
        anim = gameObject.GetComponent<Animation>();
	}

    void UpdateContent()
    {
        if (reward == null)
            return;

        ItemData item = ItemData.GetData((int)reward.itemId_);
        if (item == null)
        {
            ClientLog.Instance.LogError("Item " + reward.itemId_ + " has not found!");
            return;
        }
        name.text = item.name_;
        mask.SetActive(brandNew);
        ItemCellUI cell = UIManager.Instance.AddItemCellUI(icon, reward.itemId_);
        cell.showTips = true;
        cell.ItemCount = reward.stack_;
    }

    public void SetData(int rewardid)
    {
        if (icon != null)
            HeadIconLoader.Instance.Delete(icon.name);
        MoreCardsRewardData mcrd = MoreCardsRewardData.GetData(rewardid);
        if (mcrd != null)
        {
            reward = new COM_Item();
            reward.itemId_ = (uint)mcrd.itemid_;
            reward.stack_ = (short)mcrd.itemnum_;
        }
        UpdateContent();
    }

    public void SetStatus(bool isnew)
    {
        brandNew = isnew;
        mask.SetActive(brandNew);
        if (isnew == false && anim != null)
            anim.Play("fanpai");

        if (isnew)
        {
            for (int i = 0; i < icon.transform.childCount; ++i)
            {
                Destroy(icon.transform.GetChild(i).gameObject);
            }
        }
    }

    public bool isNew()
    {
        return brandNew;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {
        if(icon != null)
            HeadIconLoader.Instance.Delete(icon.name);
    }
}
