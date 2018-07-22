using UnityEngine;
using System.Collections;

public class SignUpPanel : MonoBehaviour{

	public UILabel _SumDayLable;
	public UILabel _DayLable;
	public UILabel _SignInLable;
	public UILabel _ConsumptionLable;
    public GameObject mendCostObj;

    public UILabel ComboSignUpMonth_;
    public UILabel ComboSignUpDay_;
    public UILabel MendSignUpCost_;

    public UIButton CloseBtn_;
    public UIButton SignBtn_;

    public UIScrollView ListView_;
    public UIGrid Grid_;
    public UISprite ListBackGround_;
    public GameObject IconBg_;

    public SignUpWarning warning_;

    float listBgWidth_, listBgHeight_;
    float iconWidth_, iconHeight_;

    int mendCost_;

    UIPanel listPanel_;
    BoxCollider listDragArea_;

	// Use this for initialization
	void Start ()
    {
		InitUIText ();
        listPanel_ = ListView_.gameObject.GetComponent<UIPanel>();
        listDragArea_ = ListView_.gameObject.GetComponent<BoxCollider>();

        UISprite sp = IconBg_.GetComponent<UISprite>();
        iconWidth_ = sp.width;
        iconHeight_ = sp.height;

        listBgWidth_ = ListBackGround_.width;
        listBgHeight_ = ListBackGround_.height;

        UIManager.SetButtonEventHandler(SignBtn_.gameObject, EnumButtonEvent.OnClick, OnSignUp, 0, 0);
		GamePlayer.Instance.signEnvet += new RequestEventHandler<bool> (OnSignEnvet);
        SignUpManager.Instance.OnUpdateUI += UpdateUI;
		InitUI ();
		UpdateUI ();
    }
	void InitUIText()
	{
//		_SumDayLable.text = LanguageManager.instance.GetValue("Sign_SumDay");
//		_DayLable.text = LanguageManager.instance.GetValue("Sign_Day");
//		_ConsumptionLable.text = LanguageManager.instance.GetValue("Sign_Consumption");
	}
    void Update()
    {
        listDragArea_.center = listPanel_.clipOffset;
    }

    void InitUI()
    {
        if (!SignUpManager.Instance.isEmpty())
        {
            GameObject iconBg = null;
            HookKeeper hk = null;
            ItemData data = null;
            ItemCellUI icon = null;
            for (int i = 1; i < SignUpManager.Instance.MaxCount; ++i)
            {
                iconBg = (GameObject)GameObject.Instantiate(IconBg_) as GameObject;
                iconBg.transform.parent = ListView_.transform;
                iconBg.transform.localScale = Vector3.one;
                iconBg.SetActive(true);
                data = ItemData.GetData(SignUpManager.Instance.GetRewardIDByIndex(i));
                hk = iconBg.GetComponent<HookKeeper>();
                icon = UIManager.Instance.AddItemCellUI(hk.icon_, (uint)data.id_, 0f, 20f);
                icon.showTips = true;
                iconBg.transform.parent = Grid_.transform;
            }
            Grid_.Reposition();
        }

        ComboSignUpMonth_.text = System.DateTime.Today.Month.ToString();
        GlobalValue.Get(Constant.C_SignPay, out mendCost_);
        MendSignUpCost_.text = mendCost_.ToString();
    }

    void UpdateUI(int index = 0, bool isMend = false)
    {
        if (!SignUpManager.Instance.isEmpty())
        {
            HookKeeper hk = null;

			for (int i = 0; i < SignUpManager.Instance.MaxCount - 1; ++i)
            {
                GameObject iconBg = Grid_.transform.GetChild(i).gameObject;
                ItemCellUI icon = iconBg.GetComponentInChildren<ItemCellUI>();
                icon.gameObject.AddComponent<UIDragScrollView>();

                hk = iconBg.GetComponent<HookKeeper>();
                hk.SetDepth(13);

				if (SignUpManager.Instance.IsSignUped(i + 1))
                {
                    hk.ToSigned();
                    RemoveClickEvent(iconBg);
                }
				else
				{
					hk.ToUnsignNormal();
				}
            }
        }
        ComboSignUpDay_.text = SignUpManager.Instance.ComboSignDay.ToString();
		SignBtn_.isEnabled = !GamePlayer.Instance.todaySigned_ ;
    }

    void SetBtn(UIButton btn, bool enable)
    {
        btn.GetComponent<BoxCollider>().enabled = enable;
        btn.SetState(enable? UIButtonColor.State.Normal: UIButtonColor.State.Disabled, true);
    }

    void AddClickEvent(GameObject go, int param)
    {
        go.AddComponent<UIButton>();
        BoxCollider bc = go.AddComponent<BoxCollider>();
        bc.size = new Vector2(iconWidth_, iconHeight_);
        UIManager.SetButtonEventHandler(go, EnumButtonEvent.OnClick, OnSignUp, param, 0);
    }

    void RemoveClickEvent(GameObject go)
    {
        GameObject.Destroy(go.GetComponent<UIButton>());
        GameObject.Destroy(go.GetComponent<BoxCollider>());
        UIManager.RemoveButtonEventHandler(go, EnumButtonEvent.OnClick);
    }

    void OnSignUp(ButtonScript obj, object args, int param1, int param2)
    {
        if (BagSystem.instance.GetEmptySlotNum() < 1)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("beibaokongjianbuzu"), PopText.WarningType.WT_Warning);
            return;
        }

        if(!GamePlayer.Instance.todaySigned_)
            SignUpManager.Instance.SignUp(SignUpManager.Instance.FirstUnSignIndex);
        else
        {
            warning_.Show(mendCost_, () =>
            {
                if (GamePlayer.Instance.GetIprop(PropertyType.PT_MagicCurrency) < mendCost_)
                {
                    MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("notEnoughMagicCurrency"), delegate
                    {
                        StoreUI.SwithShowMe(1);
                    });
                }
                else
                    SignUpManager.Instance.MendSign();
            });
        }
    }

    void OnDestroy()
    {
        SignUpManager.Instance.OnUpdateUI -= UpdateUI;
        GamePlayer.Instance.signEnvet -= OnSignEnvet;
        UIManager.RemoveButtonEventHandler(CloseBtn_.gameObject, EnumButtonEvent.OnClick);
    }

	void OnSignEnvet(bool sign)
	{
		UpdateUI ();
	}
}
