using UnityEngine;
using System.Collections;

public class TopMoneyUI : MonoBehaviour
{
	public UILabel moneyLab;
	public UILabel diamondLab;
	public UIButton addMoneyBtn;
	public UIButton addDiamondBtn;
	public UILabel moliLab;
	public UIButton addMoliBtn;
	
	void Start ()
	{
		UIManager.SetButtonEventHandler (addMoneyBtn.gameObject, EnumButtonEvent.OnClick, OnClickMoney, 0, 0);
		UIManager.SetButtonEventHandler (addDiamondBtn.gameObject, EnumButtonEvent.OnClick, OnClickDiamond, 0, 0);
		UIManager.SetButtonEventHandler (addMoliBtn.gameObject, EnumButtonEvent.OnClick, OnClickMoli, 0, 0);
		GamePlayer.Instance.OnIPropUpdate += UpdateMoney;
		UpdateMoney ();
	}


	void OnDestroy()
	{
		GamePlayer.Instance.OnIPropUpdate -= UpdateMoney;
	}

	public void UpdateMoney()
	{
		moneyLab.text = GamePlayer.Instance.GetIprop(PropertyType.PT_Money).ToString();
		diamondLab.text = GamePlayer.Instance.GetIprop(PropertyType.PT_Diamond).ToString();
		moliLab.text = GamePlayer.Instance.GetIprop(PropertyType.PT_MagicCurrency).ToString();
	}
		
	private void OnClickMoney(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.isInBattle)
		{
            PopText.Instance.Show(LanguageManager.instance.GetValue("battlecannot"), PopText.WarningType.WT_Warning, true);
			return;
		}
        if (StoreUI.Instance == null)
            StoreUI.SwithShowMe(2);
        else
            StoreUI.Instance.SwitchTab(2);
	}

	private void OnClickDiamond(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.isInBattle)
		{
            PopText.Instance.Show(LanguageManager.instance.GetValue("battlecannot"), PopText.WarningType.WT_Warning, true);
			return;
		}
        if (StoreUI.Instance == null)
            StoreUI.SwithShowMe(2);
        else
            StoreUI.Instance.SwitchTab(2);
	}
    private void OnClickMoli(ButtonScript obj, object args, int param1, int param2)
    {
		if(GamePlayer.Instance.isInBattle)
		{
            PopText.Instance.Show(LanguageManager.instance.GetValue("battlecannot"), PopText.WarningType.WT_Warning, true);
			return;
		}
    	if (StoreUI.Instance == null)
            StoreUI.SwithShowMe(1);
        else
            StoreUI.Instance.SwitchTab(1);
    }

}

