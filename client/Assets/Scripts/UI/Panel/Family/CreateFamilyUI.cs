using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class CreateFamilyUI : MonoBehaviour {

	public UIButton CloseBtn;
	public UIButton EnterBtn;
	public UIButton cancelBtn;
	public UIInput input;
	public UILabel jinbiLbel;
	public UILabel levelLabel;
	private int jinbi;
	private int level;
	void Start () {
		UIManager.SetButtonEventHandler(CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		UIManager.SetButtonEventHandler(EnterBtn.gameObject, EnumButtonEvent.OnClick, OnClickEnter, 0, 0);
		UIManager.SetButtonEventHandler(cancelBtn.gameObject, EnumButtonEvent.OnClick, OnClickcancel, 0, 0);
		GlobalValue.Get(Constant.C_CreateGuildGold, out jinbi);
		jinbiLbel.text = jinbi.ToString ();
		GlobalValue.Get(Constant.C_CreateGuildLevel, out level);
		levelLabel.text = level.ToString ();
		GuildSystem.UpdateGuildInfo += UpdateUI;
	}
	void UpdateUI()
	{
		MyFamilyInfo.ShowMe ();
		FamilyPanelUI.HideMe ();
	}
	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}
	private void OnClickEnter(ButtonScript obj, object args, int param1, int param2)
	{
		int createItemid = 0;
		GlobalValue.Get (Constant.C_CreateGuildItem, out createItemid);
		if(input.value.Equals(""))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("jiazumingbunengkong"));
			return;
		}

		Regex reg = new Regex("^[\u4e00-\u9fa5\\w]+$");
		if (!reg.IsMatch(input.value))
		{
            PopText.Instance.Show(LanguageManager.instance.GetValue("NoSymbol"));
		}
		else
		{
			if(GamePlayer.Instance.GetIprop(PropertyType.PT_Money)<jinbi)
			{
                PopText.Instance.Show(LanguageManager.instance.GetValue("EN_MoneyLess"));
			}else			
			if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level)<level)
			{
                PopText.Instance.Show(LanguageManager.instance.GetValue("EN_OpenBaoXiangLevel"));
			}else
			{
				string inputValue = input.value;
				if(Filt(ref inputValue,0,1))
				{
					PopText.Instance.Show(LanguageManager.instance.GetValue("EN_PlayerNameSame"));
				}else
				{
					if(BagSystem.instance.GetItemByItemId((uint)createItemid)!=null)
					{
						NetConnection.Instance.createGuild (input.value);
					}else
					{
						ItemData idd = ItemData.GetData(createItemid);
						PopText.Instance.Show(LanguageManager.instance.GetValue("createGuildItem").Replace("{n}",idd.name_));
					}

				}
			}

		}

	}
	bool Filt(ref string matchStr, int start, int len)
	{
		if(len <= 0 || len > matchStr.Length)
			return false;
		
		//????????? ???
		string tStr = matchStr.Substring(start, len);
		if(tStr.Contains("*"))
		{
			if(start + len + 1 <= matchStr.Length)
				Filt(ref matchStr, start + 1, len);
			else
				Filt(ref matchStr, 0, len + 1);
			return true;
		}
		List<string> fdata = FilterwordData.GetData (len);
		if(fdata == null)
		{
			if(start + len + 1 <= matchStr.Length)
				Filt(ref matchStr, start + 1, len);
			else
				Filt(ref matchStr, 0, len + 1);
			return true;
		}
		for(int i=0; i < fdata.Count; ++i)
		{
			//????
			if(tStr.Equals(fdata[i]))
			{
				// dsdfsdf
				
//				Regex reg=new Regex(tStr);
//				string partten = "";
//				for(int j=0; j < len; ++j)
//				{
//					partten += "*";
//				}
//				matchStr=reg.Replace(matchStr,partten,len,matchStr.IndexOf(tStr));
				//?????
				if(start + len + 1 <= matchStr.Length)
					Filt(ref matchStr, start + 1, len);
				else
					Filt(ref matchStr, 0, len + 1);
				return true;
			}
		}
		if(start + len + 1 <= matchStr.Length)
			Filt(ref matchStr, start + 1, len);
		else
			Filt(ref matchStr, 0, len + 1);
		return false;
	}
	private void OnClickcancel(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}///^[\u4e00-\u9fa5\w]+$/
	void OnDestroy()
	{
		GuildSystem.UpdateGuildInfo -= UpdateUI;
	}
}
