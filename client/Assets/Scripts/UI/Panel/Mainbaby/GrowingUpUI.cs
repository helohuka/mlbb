using UnityEngine;
using System.Collections;

public class GrowingUpUI : MonoBehaviour {

	public GameObject itemObj;
	public UITexture iconSp;
	public UILabel miaoshuLabel;
	public UIButton huanyuanBtn;
	public UIButton CloseBtn;
	public UIButton HelpBtn;
	public GameObject HelpObj;
	public UILabel tiLiLabel;
	public UILabel QiangduLabel;
	public UILabel mofaLabel;
	public UILabel LiliangLabel;
	public UILabel SuduLabel;
	private bool isDisPlayHelp;
	private Baby Inst;
	int itemid = 0;
	private int Uid;
	void Start () {
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose,0, 0);
		UIManager.SetButtonEventHandler (HelpBtn.gameObject, EnumButtonEvent.OnClick, OnClickHelp,0, 0);

		UIManager.SetButtonEventHandler (huanyuanBtn.gameObject, EnumButtonEvent.OnClick, OnClickhuanyuanBtn,0, 0);
//		Uid = MainbabyProperty.idss[0];
//		Inst = GamePlayer.Instance.GetBabyInst (Uid);
//		BabyData bdata = BabyData.GetData (Inst.GetIprop(PropertyType.PT_TableId));
//		mofaLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Magic].ToString()+"[-][33FF66]"+"  -"+(bdata.BIG_Magic_ - Inst.gear_[(int)BabyInitGear.BIG_Magic]);
//		tiLiLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Stama].ToString()+"[-][33FF66]"+"  -"+(bdata.BIG_Stama_ - Inst.gear_[(int)BabyInitGear.BIG_Stama]);
//		SuduLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Speed].ToString()+"[-][33FF66]"+"  -"+(bdata.BIG_Speed_ - Inst.gear_[(int)BabyInitGear.BIG_Speed]);
//		QiangduLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Power].ToString()+"[-][33FF66]"+"  -"+(bdata.BIG_Power_ - Inst.gear_[(int)BabyInitGear.BIG_Power]);
//		LiliangLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Strength].ToString()+"[-][33FF66]"+"  -"+(bdata.BIG_Strength_ - Inst.gear_[(int)BabyInitGear.BIG_Strength]);

		GlobalValue.Get(Constant.C_ResetBabyPay, out itemid);

		ItemCellUI cell = UIManager.Instance.AddItemCellUI (itemObj.GetComponent<UISprite> (), (uint)itemid);
		cell.showTips = true;
		HeadIconLoader.Instance.LoadIcon (ItemData.GetData(itemid).icon_, iconSp);
		miaoshuLabel.text = LanguageManager.instance.GetValue ("babyxiaohao").Replace ("{n}",ItemData.GetData(itemid).name_);
	}

	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		//ApplicationEntry.Instance.ui3DCamera.depth = 1.2f;
		//MainbabyListUI.babyObj.SetActive (true);
		gameObject.SetActive (false);

	}
	void OnClickhuanyuanBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(BagSystem.instance.GetItemByItemId ((uint)itemid)!= null)
		{
			NetConnection.Instance.resetBaby (Uid);
		}else
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("meiyoudaoju"));
		}



	}
	void OnClickHelp(ButtonScript obj, object args, int param1, int param2)
	{
		isDisPlayHelp = !isDisPlayHelp;
		if(isDisPlayHelp)
		{
			HelpObj.SetActive(true);
		}
		else
		{
			HelpObj.SetActive(false);
		}

	}
	void OnEnable()
	{
		Uid = MainbabyProperty.idss[0];
		Inst = GamePlayer.Instance.GetBabyInst (Uid);
		BabyData bdata = BabyData.GetData (Inst.GetIprop(PropertyType.PT_TableId));
		mofaLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Magic].ToString()+"[-][33FF66]"+"  -"+(bdata._BIG_Magic - Inst.gear_[(int)BabyInitGear.BIG_Magic]);
		tiLiLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Stama].ToString()+"[-][33FF66]"+"  -"+(bdata._BIG_Stama - Inst.gear_[(int)BabyInitGear.BIG_Stama]);
		SuduLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Speed].ToString()+"[-][33FF66]"+"  -"+(bdata._BIG_Speed - Inst.gear_[(int)BabyInitGear.BIG_Speed]);
		QiangduLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Power].ToString()+"[-][33FF66]"+"  -"+(bdata._BIG_Power - Inst.gear_[(int)BabyInitGear.BIG_Power]);
		LiliangLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Strength].ToString()+"[-][33FF66]"+"  -"+(bdata._BIG_Strength - Inst.gear_[(int)BabyInitGear.BIG_Strength]);

	}

}
