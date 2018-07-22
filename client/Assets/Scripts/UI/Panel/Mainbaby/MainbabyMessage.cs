using UnityEngine;
using System.Collections;

public class MainbabyMessage : MonoBehaviour {

	public UIButton  enterBtn;
	public UIButton cancelBtn;
	public UILabel messUilabel;
	private  COM_Item[] Equipss;
	void Start () {
		UIManager.SetButtonEventHandler (enterBtn.gameObject, EnumButtonEvent.OnClick, OnClickenterBtn,0, 0);
		UIManager.SetButtonEventHandler (cancelBtn.gameObject, EnumButtonEvent.OnClick, OnClickcancel,0, 0);
		messUilabel.text = "放生后会失去该宠物,宠物装备,是否确定将它放生？";
	}
	void OnEnable()
	{	
		Equipss = GamePlayer.Instance.GetBabyInst (MainbabyState.babyInId).Equips;
		string str = "";
	    if(IsBabyEquip())
		{
			str =LanguageManager.instance.GetValue("fangshengbabyEqu");
		}else
		{
			str = LanguageManager.instance.GetValue("fangshengbaby");
		}
		messUilabel.text = str;

		
	}
	void OnClickenterBtn(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.delBaby (MainbabyState.babyInId);
		gameObject.SetActive (false);
	}
	void OnClickcancel(ButtonScript obj, object args, int param1, int param2)
	{
		MainbabyListUI.babyObj.SetActive (true);
		gameObject.SetActive (false);
	}
	bool IsBabyEquip()
	{
		for(int i=0;i< Equipss.Length;i++)
		{
			if(Equipss[i]!=null)
			{
				return true;
			}
		}
		return false;
	}
}
