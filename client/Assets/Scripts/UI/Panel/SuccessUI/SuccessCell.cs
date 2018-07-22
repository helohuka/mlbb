using UnityEngine;
using System.Collections;

public class SuccessCell : MonoBehaviour {

	public UIButton receiveBtn;
	public GameObject wanchengObj;
	public GameObject uibtn;
	public UILabel nameLabel;
	public UILabel decLabel;
	public UILabel Numlable;
	public UILabel FinishLabel;
	public UISprite StateSp;
	public UITexture iconSp;
	public int finish_count;
	private AchieveData _adata;
	private COM_Achievement _Achievement;
    //public AchieveData Adata
    //{
    //    set
    //    {
    //        if(value != null)
    //        {
    //            _adata = value;

    //            decLabel.text = _adata._Desc;
    //            //HeadIconLoader.Instance.LoadIcon (ItemData.GetData(DropData.GetData(_adata._DropId).item_1_).icon_, iconSp);
    //        //	Numlable.text = DropData.GetData(_adata._DropId).item_num_1_.ToString();
    //            nameLabel.text = _adata._AtName;
    //            StateSp.gameObject.SetActive(false);
    //            if(Adata._AchieveType == AchievementType.AT_KillBoss)
    //            {
    //                finish_count = 1;
    //                StateSp.gameObject.SetActive(false);
    //                FinishLabel.text =(0+"/"+1);
    //            }else
    //            {
    //                finish_count = Adata._Num;
    //                FinishLabel.text = (0+"/"+Adata._Num);
    //            }
    //            string sss = "";
    //            UISprite back = uibtn.GetComponent<UISprite>();
    //            DropData da = DropData.GetData(_adata._DropId);
    //            ItemData ida = ItemData.GetData(da.item_1_);
    //            if(ida != null)
    //            {
    //                sss = BagSystem.instance.GetQualityBack((int)ida.quality_);
    //            }
    //            if(back != null)
    //            {
    //                back.spriteName = sss;
    //            }
    //            ItemCellUI icell =  UIManager.Instance.AddItemCellUI(back,(uint)ida.id_);
    //            icell.ItemCount = DropData.GetData(_adata._DropId).item_num_1_;
    //            icell.showTips = true;
    //            RefreshFinishProgress();
    //        }
    //    }
    //    get
    //    {
    //        return _adata;
    //    }
    //}

    //public COM_Achievement Achievement
    //{
    //    set
    //    {
    //        if(value != null)
    //        {
    //            _Achievement = value;
    //            if(_Achievement.achType_ == AchievementType.AT_KillBoss)
    //            {
    //                if(_Achievement.isAch_)
    //                {
    //                    receiveBtn.gameObject.SetActive(true);
    //                    wanchengObj.SetActive(false);
    //                    StateSp.gameObject.SetActive(true);
    //                    StateSp.spriteName = "";
    //                    FinishLabel.text =(1+"/"+1);
    //                }else
    //                {
    //                    receiveBtn.gameObject.SetActive(false);
    //                    wanchengObj.SetActive(true);
    //                    StateSp.spriteName = "";
    //                    StateSp.gameObject.SetActive(false);
    //                    FinishLabel.text =(0+"/"+1);
    //                }
    //                if(_Achievement.isAward_)
    //                {
    //                    wanchengObj.SetActive(true);
    //                    receiveBtn.gameObject.SetActive(false);
    //                    StateSp.gameObject.SetActive(true);
    //                    StateSp.spriteName = "yiwancheng";
    //                }
    //            }else
    //            {
    //                if(_Achievement.isAch_)
    //                {
    //                    receiveBtn.gameObject.SetActive(true);
    //                    wanchengObj.SetActive(false);
    //                    StateSp.spriteName = "";
    //                    StateSp.gameObject.SetActive(true);
    //                }else
    //                {
    //                    receiveBtn.gameObject.SetActive(false);
    //                    wanchengObj.SetActive(true);
    //                    StateSp.spriteName = "";
    //                    StateSp.gameObject.SetActive(false);
    //                }
    //                if(_Achievement.isAward_)
    //                {
    //                    wanchengObj.SetActive(true);
    //                    receiveBtn.gameObject.SetActive(false);
    //                    StateSp.gameObject.SetActive(true);
    //                    StateSp.spriteName = "yiwancheng";
    //                }
    //                FinishLabel.text = (_Achievement.achValue_+"/"+Adata._Num);
    //            }
    //        }
    //        else
    //        {


    //                receiveBtn.gameObject.SetActive(false);
    //                wanchengObj.SetActive(true);
    //                StateSp.spriteName = "";
    //                StateSp.gameObject.SetActive(false);
    //                FinishLabel.text = (0+"/"+finish_count);


    //        }
    //    }
    //    get
    //    {
    //        return _Achievement;
    //    }
    //}

    AchievementContent content;
    public AchievementContent Info
    {
        set
        {
            content = value;
            decLabel.text = value.data_._Desc;
            nameLabel.text = value.data_._AtName;
            StateSp.gameObject.SetActive(value.isAch_);
            wanchengObj.SetActive(!value.isAch_);
            receiveBtn.gameObject.SetActive(!value.isAward_ && value.isAch_);
            StateSp.spriteName = value.isAward_ ? "yiwancheng" : "";
            finish_count = value.data_._Num;
            FinishLabel.text = string.Format("{0}/{1}", value.achValue_, finish_count);
            UISprite back = uibtn.GetComponent<UISprite>();
            DropData drop = DropData.GetData(value.data_._DropId);
            ItemData itemdata = ItemData.GetData(drop.item_1_);
            if (drop != null && itemdata != null && back != null)
            {
                back.spriteName = BagSystem.instance.GetQualityBack((int)itemdata.quality_);
                ItemCellUI icell = UIManager.Instance.AddItemCellUI(back, (uint)drop.item_1_);
                icell.ItemCount = drop.item_num_1_;
                icell.showTips = true;
            }
        }
        get
        {
            return content;
        }
    }

    //public void RefreshFinishProgress()
    //{

    //    if(Adata._AchieveType == AchievementType.AT_KillBoss)
    //    {
    //        if(_Achievement != null &&_Achievement.isAch_)
    //        {
    //            StateSp.spriteName = "";
    //            StateSp.gameObject.SetActive(true);
    //            receiveBtn.gameObject.SetActive(true);
    //            wanchengObj.SetActive(false);
    //            FinishLabel.text =(1+"/"+1);
    //        }else
    //        {
    //            StateSp.spriteName = "";
    //            StateSp.gameObject.SetActive(false);
    //            receiveBtn.gameObject.SetActive(false);
    //            wanchengObj.SetActive(true);
    //            FinishLabel.text =(0+"/"+1);
    //        }

    //    }else
    //    {
    //        if(_Achievement != null )
    //        {
    //            if(_Achievement.isAch_)
    //            {
    //                StateSp.spriteName = "";
    //                StateSp.gameObject.SetActive(true);
    //                receiveBtn.gameObject.SetActive(true);
    //                wanchengObj.SetActive(false);
    //            }else
    //            {
    //                StateSp.spriteName = "";
    //                StateSp.gameObject.SetActive(false);
    //                receiveBtn.gameObject.SetActive(false);
    //                wanchengObj.SetActive(true);
    //            }
    //            FinishLabel.text = (_Achievement.achValue_+"/"+Adata._Num);
    //        }else
    //        {
    //            FinishLabel.text = (0+"/"+Adata._Num);
    //        }

    //    }
    //}
    //void Start () {
		//RefreshFinishProgress ();
		//UIManager.SetButtonEventHandler (uibtn.gameObject, EnumButtonEvent.OnClick, OnClickuibtn, 0, 0);


    //}

	private void OnClickuibtn(ButtonScript obj, object args, int param1, int param2)
	{
	    ItemData ida =	ItemData.GetData (DropData.GetData (content.data_._DropId).item_1_);

		ItemsTips.ShowMe(ida.id_);
		
	}

}
