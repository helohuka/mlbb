using UnityEngine;
using System.Collections;

public class VipPanel : MonoBehaviour {

	public UIButton closeB;
    public GameObject vip1Group;
	public GameObject vip2Group;
	public UILabel vip1Name_;
    public UILabel vip1Day_;
	public UILabel vip2Name_;
	public UILabel vip2Day_;


    public UIButton vip1Btn_;
    public UIButton vip2Btn_;

    public UISprite vip1Icon_;
    public UISprite vip2Icon_;

	public UIButton getBtn1;
	public UIButton getBtn2;

	// Use this for initialization
	void Start () {
       // UIManager.SetButtonEventHandler(closeB.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
        UIManager.SetButtonEventHandler(vip1Btn_.gameObject, EnumButtonEvent.OnClick, OnOpenVip, 1, 0);
        UIManager.SetButtonEventHandler(vip2Btn_.gameObject, EnumButtonEvent.OnClick, OnOpenVip, 2, 0);

		UIManager.SetButtonEventHandler(getBtn1.gameObject, EnumButtonEvent.OnClick, OnGetReward, 1, 0);
		UIManager.SetButtonEventHandler(getBtn2.gameObject, EnumButtonEvent.OnClick, OnGetReward, 2, 0);

        GamePlayer.Instance.OnVipUpdate += OnUpdateVip;
		GamePlayer.Instance.vipRewardfigEnvet += new RequestEventHandler<bool> (OnrewardEnvet);

        int itemId = 0;
        int itemCount = 1;
        GlobalValue.Get(Constant.C_Vip1Reward, out itemId);
        GlobalValue.Get(Constant.C_Vip1RewardNum, out itemCount);
        ItemCellUI cell = UIManager.Instance.AddItemCellUI(vip1Icon_, (uint)itemId);
        cell.showTips = true;
        cell.ItemCount = itemCount;
        GlobalValue.Get(Constant.C_Vip2Reward, out itemId);
        GlobalValue.Get(Constant.C_Vip2RewardNum, out itemCount);
        cell = UIManager.Instance.AddItemCellUI(vip2Icon_, (uint)itemId);
        cell.showTips = true;
        cell.ItemCount = itemCount;

        OnUpdateVip();
    }

    void OnUpdateVip()
    {
		getBtn1.isEnabled = false;
		getBtn2.isEnabled = false;
        if (GamePlayer.Instance.vipLevel_ == 0)
        {
			vip1Group.SetActive(false);
			vip2Group.SetActive(false);
        }
        else if (GamePlayer.Instance.vipLevel_ == 1)
        {
			vip1Group.SetActive(true);
			vip2Group.SetActive(false);
            vip1Name_.text = LanguageManager.instance.GetValue("vip1name");
            vip1Day_.text = GamePlayer.Instance.vipLeftDays_.ToString();
            vip1Group.SetActive(true);
			if(GamePlayer.Instance.vipRewardfig)
			{
				getBtn1.isEnabled  = true;
			}
        }
        else
        {
			vip1Group.SetActive(false);
			vip2Group.SetActive(true);
            vip2Name_.text = LanguageManager.instance.GetValue("vip2name");
            vip2Day_.text = GamePlayer.Instance.vipLeftDays_.ToString();
            vip2Group.SetActive(true);
			if(GamePlayer.Instance.vipRewardfig)
			{
				getBtn2.isEnabled  = true;
			}

        }
    }
	

    void OnClose(ButtonScript obj, object args, int param1, int param2)
    {
       // Destroyobj();
    }

	void OnGetReward(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.vipRewardfig)
		{
			NetConnection.Instance.vipreward ();
		}
	}


    void OnOpenVip(ButtonScript obj, object args, int param1, int param2)
    {
        // param1 == 1 vip1,      param1 == 2 vip2.

        if (GamePlayer.Instance.vipLevel_ == 2)
        {
            if (param1 == 1)
                MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("vip2Tovip1"), null, true);
            else
            {
                PopText.Instance.Show(LanguageManager.instance.GetValue("alreadyVip2"));
            }
        }
        else if(GamePlayer.Instance.vipLevel_ == 1)
        {
            if (param1 == 2)
                MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("vip1Tovip2"), () =>
                {
                    // send message to recharge vip2.
                    //COM_Chat info = new COM_Chat();
                    //info.content_ = "openvip 2 2592000";
                    //info.ck_ = ChatKind.CK_GM;
                    //NetConnection.Instance.sendChat(info, "");
                    int vip2Id;
                    GlobalValue.Get(Constant.C_Vip2ShopID, out vip2Id);
                   
                });
            else
            {
                PopText.Instance.Show(LanguageManager.instance.GetValue("alreadyVip1"));
            }
        }
        else
        {
            // send message to recharge param1.
            //COM_Chat info = new COM_Chat();
            //info.content_ = "openvip " + param1.ToString() + " 2592000";
            //info.ck_ = ChatKind.CK_GM;
            //NetConnection.Instance.sendChat(info, "");
            if (param1 == 1)
            {
                int vip1Id;
                GlobalValue.Get(Constant.C_Vip1ShopID, out vip1Id);
                SDK185.Pay(vip1Id);
                //gameHandler.PayProduct(vip1Id);
            }
            else
            {
                int vip2Id;
                GlobalValue.Get(Constant.C_Vip2ShopID, out vip2Id);
                SDK185.Pay(vip2Id);
                //gameHandler.PayProduct(vip2Id);
            }
        }
    }
	/*
    public static void ShowMe()
    {
        UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_VIPPanel);
    }
    public static void SwithShowMe()
    {
        UIBase.SwitchShowPanelByName(UIASSETS_ID.UIASSETS_VIPPanel);
    }

*/
	void OnrewardEnvet(bool flag)
	{
		OnUpdateVip ();
	}

	void OnDestroy()
    {
        UIManager.RemoveButtonEventHandler(vip1Btn_.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(vip2Btn_.gameObject, EnumButtonEvent.OnClick);
        GamePlayer.Instance.OnVipUpdate -= OnUpdateVip;
		GamePlayer.Instance.vipRewardfigEnvet -= OnrewardEnvet;
    }
}
