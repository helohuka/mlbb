using UnityEngine;
using System.Collections;

public class MA_Cards : MonoBehaviour {

    public MA_CardsItem[] cards;
    public UILabel leftCoin;
    public UILabel nextCost;
    public UIButton resetBtn;
    public UISprite[] coinIcon;
	public UIButton displayBtn;
	public GameObject disPlayObj;
    int crtIdx = -1;
    GameObject drawCardObj;
    int needItem = 0;
    string needItemName;
    int resetCount = 0;

	// Use this for initialization
	void Start () {

        SetData(MoreActivityData.GetCardsData());
        UIManager.SetButtonEventHandler(resetBtn.gameObject, EnumButtonEvent.OnClick, OnReset, 0, 0);
		UIManager.SetButtonEventHandler(displayBtn.gameObject, EnumButtonEvent.OnClick, Ondisplay, 0, 0);
        GlobalValue.Get(Constant.C_OpenCardNeedItem, out needItem);
        GlobalValue.Get(Constant.C_ResetCardNeedItemNum, out resetCount);

        ItemData iData = ItemData.GetData(needItem);
        if (iData != null)
            needItemName = iData.name_;
        UpdateCoin();

        for (int i = 0; i < coinIcon.Length; ++i)
        {
            ItemCellUI coin = UIManager.Instance.AddItemCellUI(coinIcon[i], (uint)needItem);
            coin.showTips = true;
            coin.cellPane.gameObject.GetComponent<UISprite>().atlas = null;
        }
	}

    public void SetData(COM_ADCards adcards)
    {
        UIEventListener listener = null;
        for (int i = 0; i < cards.Length; ++i)
        {
            cards[i].SetStatus(true);
            listener = UIEventListener.Get(cards[i].gameObject);
            listener.parameter = i;
            listener.onClick += OnClickMask;
        }
        for (int j = 0; j < adcards.contents_.Length; ++j)
        {
            cards[adcards.contents_[j].count_].SetStatus(false);
            cards[adcards.contents_[j].count_].SetData((int)adcards.contents_[j].rewardId_);
            listener = UIEventListener.Get(cards[adcards.contents_[j].count_].gameObject);
            listener.onClick -= OnClickMask;
        }
    }

    void OnClickMask(GameObject go)
    {
        if (GamePlayer.Instance.isInBattle)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("battlecannot"), PopText.WarningType.WT_Warning, true);
            return;
        }

        if (BagSystem.instance.GetEmptySlotNum() == 0)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("EN_OpenBaoXiangBagFull"), PopText.WarningType.WT_Warning);
            return;
        }

        MoreCardsDrawData mcdd = MoreCardsDrawData.GetData(MoreActivityData.GetCardsData().contents_.Length + 1);
        if (mcdd == null)
            return;

        COM_Item item = BagSystem.instance.GetItemByItemId((uint)needItem);
        if (item == null || item.stack_ < mcdd.cost_)
        {
            PopText.Instance.Show(string.Format(LanguageManager.instance.GetValue("notEnoughItemCount"), needItemName));
            return;
        }

        if (crtIdx != -1)
            return;

        UIEventListener listener = UIEventListener.Get(go);
        listener.onClick -= OnClickMask;
        int idx = (int)listener.parameter;
        if (idx >= 0 && idx < cards.Length)
        {
            if (cards[idx].isNew())
            {
                drawCardObj = go;
                crtIdx = idx;
                NetConnection.Instance.openCard((ushort)idx);
                NetWaitUI.ShowMe();
            }
        }
    }

	private void Ondisplay(ButtonScript obj, object args, int param1, int param2)
	{
		disPlayObj.SetActive (true);
	}
    private void OnReset(ButtonScript obj, object args, int param1, int param2)
    {
        MessageBoxUI.ShowMe(string.Format(LanguageManager.instance.GetValue("adcardsNeedTips"), resetCount, needItemName), () =>
        {
            COM_Item item = BagSystem.instance.GetItemByItemId((uint)needItem);
            if (item != null && item.stack_ >= resetCount)
                NetConnection.Instance.resetCard();
            else
                PopText.Instance.Show(string.Format(LanguageManager.instance.GetValue("notEnoughItemCount"), needItemName));
        });
    }
	
	// Update is called once per frame
	void Update () {
        if (MoreActivityData.drawCardOk)
        {
            UpdateCoin();
            if (MoreActivityData.GetCardsData().contents_.Length == 0)
            {
                UIEventListener listener = null;
                for (int i = 0; i < cards.Length; ++i)
                {
                    cards[i].SetStatus(true);
                    listener = UIEventListener.Get(cards[i].gameObject);
                    listener.parameter = i;
                    listener.onClick += OnClickMask;
                }
            }
            if (drawCardObj != null)
            {
                MA_CardsItem card = drawCardObj.GetComponent<MA_CardsItem>();
                card.SetStatus(false);
                card.SetData(MoreActivityData.GetCardReward(crtIdx));
                EffectAPI.PlayUIEffect((EFFECT_ID)1066, card.got.transform, null, (GameObject eff) =>
                {
                    eff.transform.localPosition = Vector3.zero;
                });
                drawCardObj = null;
            }
            crtIdx = -1;
            MoreActivityData.drawCardOk = false;
        }
	}

    void UpdateCoin()
    {
        MoreCardsDrawData mcdd = MoreCardsDrawData.GetData(MoreActivityData.GetCardsData().contents_.Length + 1);
        if (mcdd != null)
            nextCost.text = mcdd.cost_.ToString();
        else
            nextCost.text = "-";

        int num = BagSystem.instance.GetItemMaxNum((uint)needItem);
        leftCoin.text = num.ToString();
    }
}
