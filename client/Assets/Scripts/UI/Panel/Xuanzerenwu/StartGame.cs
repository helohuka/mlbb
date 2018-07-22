using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class StartGame : MonoBehaviour {

	public UIButton startBtn;
	public UIButton backBtn;
	public UIButton ranBtn;
	public UIInput input;

	public UILabel desLabel;
	public UILabel racLabel;


	// Use this for initialization
	void Start () {

		input.value = RandomNameData.RandomName ();

		UIManager.SetButtonEventHandler (startBtn.gameObject, EnumButtonEvent.OnClick, OnClickEnterB, 0, 0);
		UIManager.SetButtonEventHandler (ranBtn.gameObject, EnumButtonEvent.OnClick, OnClickRandomBtn, 0, 0);
		UIManager.SetButtonEventHandler (backBtn.gameObject, EnumButtonEvent.OnClick, OnClickbackBtn, 0, 0);
	}

	public void ShowPlayerDes(int rid)
	{
		PlayerData pdata = PlayerData.GetData (rid);
		desLabel.text = pdata.des_;
		racLabel.text = pdata.race_des_;
	}

	private void OnClickEnterB(ButtonScript obj, object args, int param1, int param2)
	{
//		if(CreatePlayerRole.isCreate)
//		{

		Regex reg = new Regex("^[a-zA-Z0-9_\u4e00-\u9fa5]{1,6}$");
		if (!reg.IsMatch(input.value))
		{

			PopText.Instance.Show(LanguageManager.instance.GetValue("gaimingtishi"));

		}
		else
		{
			chatStr = input.value;
//			if(Filt(ref chatStr,0,1))
//			{
//				PopText.Instance.Show(LanguageManager.instance.GetValue("EN_FilterWord"));
//			}else
//			{
				NetConnection.Instance.createPlayer(chatStr, (byte)CreatePlayerRole.roleId);
//			}

		}

			
//		}else
//		{
//			GamePlayer.Instance.isCreate = false;
//			NetConnection.Instance.enterGame((uint)CreatePlayerRole.roleId);
//			
//		}
	}
	string chatStr = "";
	private void OnClickRandomBtn(ButtonScript obj, object args, int param1, int param2)
	{
		input.value = RandomNameData.RandomName ();
	}
	private void OnClickbackBtn(ButtonScript obj, object args, int param1, int param2)
	{
		CreatePlayerRole cpr = Camera.main.GetComponent<CreatePlayerRole> ();
		if(cpr !=null)
		cpr.CamerMoveBack ();
		cpr.RecoveryRoleDisplay();
		XuanPanel.Instance.SetIconShowState (true);
		Camera.main.GetComponent<CreatePlayerRole> ().ShowPlayerObj();
		gameObject.SetActive (false);
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
			return false;
		}
		
		//??????????? ???
		List<string> fdata = FilterwordData.GetData (len);
		if(fdata == null)
		{
			if(start + len + 1 <= matchStr.Length)
				Filt(ref matchStr, start + 1, len);
			else
				Filt(ref matchStr, 0, len + 1);
			return false;
		}
		
		
		//??
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
//				//?????
//				if(start + len + 1 <= matchStr.Length)
//					Filt(ref matchStr, start + 1, len);
//				else
//					Filt(ref matchStr, 0, len + 1);
				return true;
			}
		}
		
		//???? ?????
		if(start + len + 1 <= matchStr.Length)
			Filt(ref matchStr, start + 1, len);
		else
			Filt(ref matchStr, 0, len + 1);

		return false;
	}

}
