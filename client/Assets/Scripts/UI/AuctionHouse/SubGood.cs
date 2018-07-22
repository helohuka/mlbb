using UnityEngine;
using System.Collections;

public class SubGood : MonoBehaviour {


    public GameObject babyGroup_;
    public GameObject itemGroup_;

    public UILabel babyName_;
    public UITexture babyRace_;

    public UILabel itemName_;
	public UISprite dikuangSp;
    public enum GoodType
    {
        GT_Baby,
        GT_Item,
    }
    GoodType goodType_;

	// Use this for initialization
	void Start () {
        babyGroup_.SetActive(goodType_ == GoodType.GT_Baby);
        itemGroup_.SetActive(goodType_ == GoodType.GT_Item);

	}

    public void SetType(GoodType type)
    {
        goodType_ = type;
    }

    public void SetData(string name, string extraParam = "")
    {



        if (goodType_ == GoodType.GT_Item)
        {
            itemName_.text = name;
        }
        else
        {
            babyName_.text = name;
            if(!string.IsNullOrEmpty(extraParam))
                HeadIconLoader.Instance.LoadIcon(extraParam, babyRace_);
        }
    }
}
