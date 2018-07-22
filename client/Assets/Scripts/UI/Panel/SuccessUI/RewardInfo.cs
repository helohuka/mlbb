using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RewardInfo : MonoBehaviour {

	public GameObject item;
	public UIGrid grid;
	public UIButton ReceiveBtn;
	public UIPanel ProgressBarPanle;
	public UILabel contionlabel;
	public UILabel ProgressBarLabel;
    AchievementContent content_;
	void Start () {
		item.SetActive (false);
        DropData dropD = DropData.GetData(SuccessSystem.Reward50().data_._DropId);
        if (dropD != null)
        {
            GameObject clone = null;
            for (int i = 0; i < dropD.itemList.Count; i++)
            {
                if (dropD.itemList[i] == 0)
                    continue;

                clone = GameObject.Instantiate(item) as GameObject;
                clone.SetActive(true);
                clone.transform.parent = grid.transform;
                clone.transform.position = Vector3.zero;
                clone.transform.localScale = Vector3.one;
                ItemData idata = ItemData.GetData(dropD.itemList[i]);
                UIManager.SetButtonEventHandler(clone.gameObject, EnumButtonEvent.OnClick, OnClickclone, idata.id_, 0);
                SuccessRewardCell asCell = clone.GetComponent<SuccessRewardCell>();
                asCell.Idata = idata;
            }
            grid.repositionNow = true;
        }
        contionlabel.text = SuccessSystem.Reward50().data_._Desc;
        UIManager.SetButtonEventHandler(ReceiveBtn.gameObject, EnumButtonEvent.OnClick, OnClickReceive, 0, 0);
	}

	private void OnClickclone(ButtonScript obj, object args, int param1, int param2)
	{
		ItemData ida =	ItemData.GetData (param1);
		ItemsTips.ShowMe(ida.id_);
	}
    
	public void Percentage(int count, AchievementContent content, int maxcount)
	{
        content_ = content;
        float rate = (count*1f) / (maxcount*1f);
		float size = rate * 360f;
        if (size <= 0)
            size = 1;
        ProgressBarLabel.text = count.ToString();
		ProgressBarPanle.baseClipRegion = new Vector4 (ProgressBarPanle.baseClipRegion.x,ProgressBarPanle.baseClipRegion.y,ProgressBarPanle.baseClipRegion.z,size);
        if (content_ != null)
		{
            ReceiveBtn.isEnabled = (content_.isAch_ && !content_.isAward_);
		}
		else
		{
			ReceiveBtn.isEnabled = false;
		}
	}

	void OnClickReceive(ButtonScript obj, object args, int param1, int param2)
	{
		if(BagSystem.instance.BagIsFull())
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("EN_BagFull"));
			return ;
		}
        NetConnection.Instance.requestAchaward((int)content_.data_._Id);

	}
}
