using UnityEngine;
using System.Collections;

public class HuoDongDetail : MonoBehaviour {

    public UILabel questName_;
    public UILabel times_;
    public UILabel period_;
    public UILabel type_;
    public UILabel lvLmt_;
    public UILabel contentDetail_;
    public UILabel timeLabel;
    public UIGrid rewardGrid_;
    public GameObject canjiaBtn;
	public UIButton CloseBtn;
	void Start()
	{
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
	}
    public void SetData(int id)
    {
        DaliyActivityData data = DaliyActivityData.GetData(id);
        questName_.text = data.activityName_;
		times_.text = data.maxCount_.ToString ();
        period_.text = data.activityTime_;
        type_.text = data.activityFrom_;
        lvLmt_.text = data.joinLv_.ToString();
        contentDetail_.text = data.desc_;
        string[] rewards = null;
        ItemCellUI icon = null;
        int childCount = rewardGrid_.transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            Destroy(rewardGrid_.transform.GetChild(i).gameObject);
        }
        rewardGrid_.transform.DetachChildren();
        if (!string.IsNullOrEmpty(data.award_))
        {
            GameObject go = null;
            rewards = data.award_.Split(';');
            for (int i = 0; i < rewards.Length; ++i)
            {
                go = new GameObject("item_" + rewards[i]);
                go.AddComponent<UISprite>().depth = 0;
                icon = UIManager.Instance.AddItemCellUI(go.GetComponent<UISprite>(), uint.Parse(rewards[i]));
                icon.showTips = true;
                icon.gameObject.AddComponent<UIDragScrollView>();
                go.transform.parent = rewardGrid_.transform;
                go.transform.localScale = Vector3.one;
            }
        }
        rewardGrid_.repositionNow = true;
        //gameObject.SetActive(true);
    }

    public void Clear()
    {
        questName_.text = "任务类型";
        times_.text = "-";
        period_.text = "-";
        type_.text = "-";
        lvLmt_.text = "-";
        contentDetail_.text = "";
        timeLabel.gameObject.SetActive(false);
        canjiaBtn.gameObject.SetActive(false);
        for (int i = 0; i < rewardGrid_.transform.childCount; ++i)
        {
            Destroy(rewardGrid_.transform.GetChild(i).gameObject);
        }
        rewardGrid_.transform.DetachChildren();
    }
	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}
    public void SetJiaruBtnHandler(UIEventListener.VoidDelegate handler, object parameter, string timeOpen, bool isOpen)
    {
        UIEventListener listener = UIEventListener.Get(canjiaBtn);
        listener.onClick = handler;
        listener.parameter = parameter;

        canjiaBtn.SetActive(isOpen);
        timeLabel.text = timeOpen;
        timeLabel.gameObject.SetActive(!isOpen);
    }
}
