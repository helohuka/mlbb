using UnityEngine;
using System.Collections;

public class MA_7DaysQuestItem : MonoBehaviour {

    public UILabel desc;
    public UISprite[] rewards;
    public UIButton getBtn;
    public UILabel getBtnLbl;
	public UISprite isReward;

    public int id_;

	// Use this for initialization
	void Start () {
        UIManager.SetButtonEventHandler(getBtn.gameObject, EnumButtonEvent.OnClick, OnReward, 0, 0);
	}

    public void SetData(int id)
    {
        id_ = id;
        SevenDaysData sdd = SevenDaysData.GetData(id_);
        desc.text = sdd.desc;
        ItemCellUI cell;
        for (int i = 0; i < sdd.rewardItem.Length; ++i)
        {
            cell = UIManager.Instance.AddItemCellUI(rewards[i], sdd.rewardItem[i].itemId_);
            cell.showTips = true;
            cell.ItemCount = sdd.rewardItem[i].stack_;
        }
        MoreActivityData.sevenDaysDirty = true;
    }

    public void UpdateBtnStatus()
    {
        COM_Sevenday sd = MoreActivityData.Get7DaysData(id_);
        getBtnLbl.text = LanguageManager.instance.GetValue("Store_Received");
		getBtn.GetComponent<BoxCollider>().enabled = true;
        if (sd != null)
        {
            if (sd.isreward_)
            {
                getBtn.GetComponent<BoxCollider>().enabled = false;
                getBtn.normalSprite = "huianniu";
                getBtnLbl.text = LanguageManager.instance.GetValue("Store_HaveReceived");
				isReward.gameObject.SetActive(true);
				getBtn.gameObject.SetActive(false);
            }
            else
            {
                if(sd.isfinish_)
                {
					getBtn.gameObject.SetActive(true);
					isReward.gameObject.SetActive(false);
                    getBtn.GetComponent<BoxCollider>().enabled = true;
                    getBtn.normalSprite = "huanganniu";
                }
                else
                {
					getBtn.gameObject.SetActive(true);
					isReward.gameObject.SetActive(false);
                    getBtn.GetComponent<BoxCollider>().enabled = false;
                    getBtn.normalSprite = "huianniu";
                }
            }
        }
        else
        {
			isReward.gameObject.SetActive(false);
			getBtn.gameObject.SetActive(true);
            getBtn.GetComponent<BoxCollider>().enabled = false;
            getBtn.normalSprite = "huianniu";

        }
    }

    private void OnReward(ButtonScript obj, object args, int param1, int param2)
    {
        SevenDaysData sdd = SevenDaysData.GetData(id_);
        if (sdd == null)
            return;

        if (sdd.day > GamePlayer.Instance.DaysOld)
        {
            MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("notReachDay"), null, true);
            return;
        }

        if (BagSystem.instance.GetEmptySlotNum() < 3)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("EN_OpenBaoXiangBagFull"), PopText.WarningType.WT_Warning);
            return;
        }
        //send message
        NetConnection.Instance.requestSevenReward((uint)id_);
    }

	// Update is called once per frame
	void Update () {
        
	}
}
