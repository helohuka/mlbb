using UnityEngine;
using System.Collections.Generic;

public class SellConfirmItem : MonoBehaviour {

    public UILabel name_;

    public UISprite icon_;

    public UILabel level_;

    public UILabel pr_;

    public UILabel type_;

    public UILabel desc_;

    public UILabel tax_;

    public UIInput price_;

    public UIButton sellBtn_;

    public UIButton cancelBtn_;

    public GameObject propCell_;

    public UIGrid grid_;


    // OtherSelling
    public UIGrid otherGrid_;

    public GameObject sellItemGo_;

    public UIButton pageUp_;
    public UIButton pageDown_;
    public UILabel pageNum_;

    COM_Item inst_;

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

        GlobalInstanceFunction.Instance.Invoke( () => {
            UIManager.SetButtonEventHandler(pageUp_.gameObject, EnumButtonEvent.OnClick, OnPageUpdate, -1, 0);
            UIManager.SetButtonEventHandler(pageDown_.gameObject, EnumButtonEvent.OnClick, OnPageUpdate, 1, 0);

            // fetch other people selling request.
            LaunchContext(name_.text);
            NetConnection.Instance.fetchSelling2(searchContext_);
        }, 1);
    }

    void LaunchContext(string searchStr = "")
    {
        searchContext_.title_ = searchStr;
        searchContext_.begin_ = AuctionHouseSystem.otherCurrentStartIndex;
        searchContext_.limit_ = AuctionHouseSystem.OtherSellingMax;
    }

    List<GameObject> cellPool = new List<GameObject>();

    public void SetData(COM_Item inst)
    {
		if (inst == null)
			return;
		gameObject.SetActive(true);
        for (int i = 0; i < cellPool.Count; ++i)
        {
            cellPool[i].SetActive(false);
        }
        inst_ = inst;
        ItemData data = ItemData.GetData((int)inst_.itemId_);
        name_.text = data.name_;
        desc_.text = data.desc_;
        level_.text = inst_.strLevel_ == 0 ? LanguageManager.instance.GetValue("Nothing") : inst_.strLevel_.ToString();
        float tax = 0f;
        GlobalValue.Get(Constant.C_MallTax, out tax);
        tax_.text = string.Format("[ff0000]{0}[-]", ((int)(tax * 100f)).ToString());
        int durability = GetDurability(inst.propArr);
        if (durability == 0)
        {
            pr_.text = LanguageManager.instance.GetValue("Nothing");
        }
        else
        {
            pr_.text = string.Format("{0}/{1}", durability, durability);
        }
        type_.text = LanguageManager.instance.GetValue(data.subType_.ToString());

        GameObject cellGo = null;
        for (int i = 0; i < inst_.propArr.Length; ++i)
        {
            if (i < cellPool.Count)
            {
                cellPool[i].transform.FindChild("name").GetComponent<UILabel>().text = LanguageManager.instance.GetValue(inst_.propArr[i].type_.ToString()) + ": " + ((int)inst_.propArr[i].value_).ToString();
                cellPool[i].SetActive(true);
            }
            else
            {
                cellGo = GameObject.Instantiate(propCell_) as GameObject;
                cellGo.transform.FindChild("name").GetComponent<UILabel>().text = LanguageManager.instance.GetValue(inst_.propArr[i].type_.ToString()) + ": " + ((int)inst_.propArr[i].value_).ToString();
                grid_.AddChild(cellGo.transform);
                cellGo.transform.localScale = Vector3.one;
                cellGo.SetActive(true);
                cellPool.Add(cellGo);
            }
        }
        grid_.Reposition();

		ItemCellUI cell = UIManager.Instance.AddItemInstCellUI(icon_, inst_);
		if (cell == null)
			return;
        cell.showTips = true;
        cell.ItemCount = (int)inst_.stack_;
        UIManager.SetButtonEventHandler(sellBtn_.gameObject, EnumButtonEvent.OnClick, OnSell, 0, 0);
        UIManager.SetButtonEventHandler(cancelBtn_.gameObject, EnumButtonEvent.OnClick, (ButtonScript obj, object args, int param1, int param2) =>
        {
            gameObject.SetActive(false);
        }, 0, 0);
        
    }

    int GetDurability(COM_PropValue[] propArr)
    {
        for (int i = 0; i < propArr.Length; ++i)
        {
            if (propArr[i].type_ == PropertyType.PT_Durability)
                return (int)propArr[i].value_;
        }
        return 0;
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
        NetConnection.Instance.selling((int)inst_.instId_, 0, int.Parse(price_.value));
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
            GlobalValue.Get(Constant.C_DiamondMax, out maxCoin);
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
    }
}
