using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
public class MainBabyChangeName : MonoBehaviour {

	public UIInput nameInput;
	public UIButton enterBtn;
	public UIButton CanelBtn;
	public int insID;

	public delegate void changeBabyName(int bid,string name);
	public static changeBabyName babyChangeName;
	void Start () {
		UIManager.SetButtonEventHandler (enterBtn.gameObject, EnumButtonEvent.OnClick, OnClickEnter,0, 0);
		UIManager.SetButtonEventHandler (CanelBtn.gameObject, EnumButtonEvent.OnClick, OnClickCanel,0, 0);
		babyChangeName = ChangeName;
	}
	void ChangeName(int babyId,string name)
	{
		if (MainbabyState.ShowBabyNewName != null)
		{
			MainbabyState.ShowBabyNewName (babyId,name);
		}
		if (MainbabyListUI.changeBabyNameOk != null)
		{
			MainbabyListUI.changeBabyNameOk(babyId,name);
		}
		Avatar player = Prebattle.Instance.GetSelf();
		if (player != null && player.myBaby_ != null)
		{
			player.myBaby_.SetInstName (name,true);
		}

		gameObject.SetActive (false);
		//ApplicationEntry.Instance.ui3DCamera.depth = 1.2f;
	}
	void OnClickEnter(ButtonScript obj, object args, int param1, int param2)
	{
		insID = MainbabyProperty.idss [0];
//		if(nameInput.value == "")
//		{
//			return;
//		}
		Regex reg = new Regex("^[a-zA-Z0-9_\u4e00-\u9fa5]{1,6}$");
		if (!reg.IsMatch(nameInput.value))
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("gaimingtishi"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("gaimingtishi"));
		}
		else
		{
			NetConnection.Instance.changeBabyName ((uint)insID,nameInput.value);
		}

		
	}
	void OnClickCanel(ButtonScript obj, object args, int param1, int param2)
	{
		//ApplicationEntry.Instance.ui3DCamera.depth = 1.2f;
		MainbabyListUI.babyObj.SetActive (true);
		gameObject.SetActive (false);
		
	}


}
