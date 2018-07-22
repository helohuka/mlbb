using UnityEngine;
using System.Collections;

public class SellConfirmBaby : MonoBehaviour {

    public UILabel name_;

    public UISprite icon_;

    public UILabel level_;

    public UILabel tax_;

    public GameObject[] skills_;

    public UIInput price_;

    public UIButton sellBtn_;

    public UIButton cancelBtn_;


    // OtherSelling
    public UIGrid otherGrid_;

    public GameObject sellItemGo_;

    public UIButton pageUp_;
    public UIButton pageDown_;
    public UILabel pageNum_;

    Baby inst_;

    string realName_;

    COM_SearchContext searchContext_;
	// Use this for initialization
	void Start () {
        GlobalInstanceFunction.Instance.MakeUIMask(gameObject);
	}

    void OnEnable()
    {
        searchContext_ = new COM_SearchContext();
        AuctionHouseSystem.otherCurrentStartIndex = 0;
        AuctionHouseSystem.OnOtherSellingListUpdate += OnUpdateOtherSelling;
        UIManager.SetButtonEventHandler(pageUp_.gameObject, EnumButtonEvent.OnClick, OnPageUpdate, -1, 0);
        UIManager.SetButtonEventHandler(pageDown_.gameObject, EnumButtonEvent.OnClick, OnPageUpdate, 1, 0);

        // fetch other people selling request.
        LaunchContext(name_.text);
        NetConnection.Instance.fetchSelling2(searchContext_);
    }

    void LaunchContext(string searchStr = "")
    {
        searchContext_.title_ = searchStr;
        searchContext_.begin_ = AuctionHouseSystem.otherCurrentStartIndex;
        searchContext_.limit_ = AuctionHouseSystem.OtherSellingMax;
    }

    public void SetData(Baby inst)
    {
        inst_ = inst;
        name_.text = inst_.InstName;
        realName_ = BabyData.GetData((int)inst.GetEntity().properties_[(int)PropertyType.PT_TableId])._Name;
        level_.text = (inst_.GetIprop(PropertyType.PT_Level)).ToString();
        float tax = 0f;
        GlobalValue.Get(Constant.C_MallTax, out tax);
        tax_.text = string.Format("[ff0000]{0}[-]", ((int)(tax * 100f)).ToString());

        SkillData skill = null;
        UITexture tex = null;
        GameObject texGo = null;
        for (int i = 0; i < inst_.SkillInsts.Count; ++i)
        {
            skill = SkillData.GetData((int)inst_.SkillInsts[i].skillID_, (int)inst_.SkillInsts[i].skillLevel_);
            if (skill._Name.Equals(LanguageManager.instance.GetValue("playerPro_FightBack")) || skill._Name.Equals(LanguageManager.instance.GetValue("playerPro_Dodge")))
                continue;

            tex = skills_[i].GetComponentInChildren<UITexture>();
            if (tex == null)
            {
                texGo = new GameObject();
                texGo.layer = LayerMask.NameToLayer("UI");
                tex = texGo.AddComponent<UITexture>();
                tex.transform.parent = skills_[i].transform;
                texGo.transform.localPosition = Vector3.zero;
                texGo.transform.localScale = Vector3.one;
                tex.depth = skills_[i].GetComponent<UISprite>().depth + 1;
            }
            HeadIconLoader.Instance.LoadIcon(skill._ResIconName, tex);
        }

        BabyCellUI cell = UIManager.Instance.AddBabyCellUI(icon_, inst);
        UIManager.SetButtonEventHandler(cell.gameObject, EnumButtonEvent.OnClick, OnClickIcon, 0, 0);
        UIManager.SetButtonEventHandler(sellBtn_.gameObject, EnumButtonEvent.OnClick, OnSell, 0, 0);
        UIManager.SetButtonEventHandler(cancelBtn_.gameObject, EnumButtonEvent.OnClick, (ButtonScript obj, object args, int param1, int param2) =>
        {
            gameObject.SetActive(false);
        }, 0, 0);
        gameObject.SetActive(true);
    }

    void OnClickIcon(ButtonScript obj, object args, int param1, int param2)
    {
        ChatBabytips.ShowMe(inst_.GetInst());
    }

    void OnSell(ButtonScript obj, object args, int param1, int param2)
    {
		int price = 0;
		GlobalValue.Get(Constant.C_MallMinPrice, out price);
        if (string.IsNullOrEmpty(price_.value))
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("inputPrice"), PopText.WarningType.WT_Warning);
            return;
        }
		if (int.Parse(price_.value)<price)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("chushoujiage"), PopText.WarningType.WT_Warning);
			return;
		}
        if (AuctionHouseSystem.mySellingList_.Count == AuctionHouseSystem.SellingMax)
        {
            PopText.Instance.Show(string.Format(LanguageManager.instance.GetValue("maxCount4Sell"), AuctionHouseSystem.SellingMax), PopText.WarningType.WT_Warning);
            return;
        }

        if (inst_.isForBattle_)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("DonotSellBattleBaby"), PopText.WarningType.WT_Warning);
            return;
        }

        if (inst_.isShow_)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("DonotSellShowingBaby"), PopText.WarningType.WT_Warning);
            return;
        }

        NetConnection.Instance.selling(0, (int)inst_.InstId, int.Parse(price_.value));
        gameObject.SetActive(false);
    }

    void OnPageUpdate(ButtonScript obj, object args, int param1, int param2)
    {
        int toPage = AuctionHouseSystem.otherCurrentPage + param1;
        if (toPage < 1)
            toPage = 1;
        if (toPage > AuctionHouseSystem.otherTotalPage_ + 1)
            toPage = AuctionHouseSystem.otherTotalPage_ + 1;
        AuctionHouseSystem.otherCurrentStartIndex = (toPage - 1) * AuctionHouseSystem.OtherSellingMax;
        searchContext_.begin_ = AuctionHouseSystem.otherCurrentStartIndex;
        NetConnection.Instance.fetchSelling2(searchContext_);
    }
	
	// Update is called once per frame
	void Update () {
        if (!string.IsNullOrEmpty(price_.value))
        {
            int maxCoin = 0;
            GlobalValue.Get(Constant.C_MoneyMax, out maxCoin);
            if (long.Parse(price_.value) > maxCoin)
                price_.value = maxCoin.ToString();
        }
	}

    GameObject[] otherSellingGo = new GameObject[AuctionHouseSystem.OtherSellingMax];

    void OnUpdateOtherSelling()
    {
        if (AuctionHouseSystem.otherSellingList_.Count == 0)
        {
            if (searchContext_.begin_ != 0)
                return;
        }
        GameObject go = null;
        for (int i = 0; i < AuctionHouseSystem.OtherSellingMax; ++i)
        {
            if (i < AuctionHouseSystem.otherSellingList_.Count)
            {
                if (otherSellingGo[i] == null)
                {
                    go = GameObject.Instantiate(sellItemGo_) as GameObject;
                    go.GetComponent<SellingGood>().SetData(AuctionHouseSystem.otherSellingList_[i]);
                    otherGrid_.AddChild(go.transform);
					go.transform.localScale = Vector3.one;
                    go.SetActive(true);
                    otherSellingGo[i] = go;
                }
                else
                {
                    otherSellingGo[i].GetComponent<SellingGood>().SetData(AuctionHouseSystem.otherSellingList_[i]);
                    otherSellingGo[i].SetActive(true);
                }
            }
            else
            {
                if (otherSellingGo[i] != null)
                {
                    otherSellingGo[i].SetActive(false);
                }
            }
        }
        otherGrid_.Reposition();
        pageNum_.text = string.Format("{0}/{1}", AuctionHouseSystem.otherCurrentPage > AuctionHouseSystem.otherTotalPage_ ? AuctionHouseSystem.totalPage_ : AuctionHouseSystem.otherCurrentPage, AuctionHouseSystem.otherTotalPage_);
    }

    void OnDisable()
    {
        AuctionHouseSystem.OnOtherSellingListUpdate -= OnUpdateOtherSelling;
        UIManager.RemoveButtonEventHandler(pageUp_.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(pageDown_.gameObject, EnumButtonEvent.OnClick);
		for (int i = 0; i < skills_.Length; ++i)
		{
			for (int j = 0; j < skills_[i].transform.childCount; ++j)
			{
				Destroy(skills_[i].transform.GetChild(j).gameObject);
			}
		}
        for (int i = 0; i < skills_.Length; ++i)
        {
            for (int j = 0; j < skills_[i].transform.childCount; ++j)
            {
                Destroy(skills_[i].transform.GetChild(j).gameObject);
            }
        }
    }
}
