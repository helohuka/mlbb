using UnityEngine;
using System.Collections;

public class DatiPanel : UIBase {

	public UILabel _TenNumLable;
	public UILabel _TwentyNumLable;
	public UILabel _CorrectRateLable;
	public UILabel _TimeTitleLable;
	public UILabel _SurplusTimeLable;
	public UILabel _ObtainExpLable;
	public UILabel _ObtainMoneyLable;
	public UILabel _TenNameLable;
	public UILabel _TwentyNameLable;
	public UILabel _NumberLable;
	public UILabel _ContentLable;
	public UILabel _ALable;
	public UILabel _BLable;
	public UILabel _CLable;
	public UILabel _DLable;
	public UITexture _TenIcon;
	public UITexture _TwentyIcon;
	public UISprite stateSp;
	public UIButton _Abtn;
	public UIButton _Bbtn;
	public UIButton _Cbtn;
	public UIButton _Dbtn;
	public UIButton _Closebtn;
	public GameObject _TenObj;
	public GameObject _TwentyObj;
	public UISprite pos;
	private int _RigthNum;
	private int _TenExamRight;
	private int _TwentyExamRight;
	private UIEventListener _TenListener;
	private UIEventListener _TwentyListener;
	void Start () {
		stateSp.gameObject.SetActive (false);
		InitData ();
		UIManager.SetButtonEventHandler (_Abtn.gameObject, EnumButtonEvent.OnClick, OnClickconfirm,1, 0);
		UIManager.SetButtonEventHandler (_Bbtn.gameObject, EnumButtonEvent.OnClick, OnClickconfirm,2, 0);
		UIManager.SetButtonEventHandler (_Cbtn.gameObject, EnumButtonEvent.OnClick, OnClickconfirm,3, 0);
		UIManager.SetButtonEventHandler (_Dbtn.gameObject, EnumButtonEvent.OnClick, OnClickconfirm,4, 0);
		UIManager.SetButtonEventHandler (_Closebtn.gameObject, EnumButtonEvent.OnClick, OnClickClose,0, 0);
		UIManager.SetButtonEventHandler (_TenObj.gameObject, EnumButtonEvent.OnClick, ShowTips,_TenExamRight, 0);
		UIManager.SetButtonEventHandler (_TwentyObj.gameObject, EnumButtonEvent.OnClick, ShowTips,_TwentyExamRight, 0);

		_CorrectRateLable.text = "";
		ExamSystem.InitExamData += InitExam;
		ExamSystem.UpdateExam += UpdateExamData;
		ExamSystem.UpdateActivityState += CloseActivity;
		InitExam (ExamSystem._Exam);
		if(ExamSystem._IsOpenExam)
		{
			EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_UI_huodongkaishi, gameObject.transform,null,(GameObject obj)=>{
				obj.transform.localPosition = Vector3.zero;
			});
		}

	}
	void CloseActivity( bool open)
	{
		if(open==false)
		{
			ClearTextData();
		}
	}
	void ShowTips(ButtonScript obj, object args, int param1, int param2)
	{
		ItemsTips.ShowMe (param1);
	}
	void InitData()
	{
		_TimeTitleLable.text = LanguageManager.instance.GetValue ("time");
		_SurplusTimeLable.text = DaliyActivityData.GetData (ActivityType.ACT_Exam).activityTime_.ToString();
		GlobalValue.Get(Constant.C_ExamRight10, out _TenExamRight);
		GlobalValue.Get(Constant.C_ExamRight20, out _TwentyExamRight);
		_TenNameLable.text = ItemData.GetData (_TenExamRight).name_;
		_TwentyNameLable.text = ItemData.GetData (_TwentyExamRight).name_;
		HeadIconLoader.Instance.LoadIcon (ItemData.GetData (_TenExamRight).icon_, _TenIcon);
		HeadIconLoader.Instance.LoadIcon (ItemData.GetData (_TwentyExamRight).icon_, _TwentyIcon);
		ResExamData(ExamSystem._Qindex);
		_RigthNum = (int)ExamSystem._RightNum;
		if(_RigthNum>=10)
		{
			_TenNumLable.text = "1/1";
		}else
		{
			_TenNumLable.text = "0/1";
		}
		if(_RigthNum>=20)
		{
			_TwentyNumLable.text = "1/1";
		}else
		{
			_TwentyNumLable.text = "0/1";
		}
		if(ExamSystem._IsOpenExam)
		{
			stateSp.gameObject.SetActive (false);
			_Abtn.gameObject.SetActive(true);
			_Bbtn.gameObject.SetActive(true);
			_Cbtn.gameObject.SetActive(true);
			_Dbtn.gameObject.SetActive(true);
		}else
		{
			stateSp.gameObject.SetActive (true);
			_Abtn.gameObject.SetActive(false);
			_Bbtn.gameObject.SetActive(false);
			_Cbtn.gameObject.SetActive(false);
			_Dbtn.gameObject.SetActive(false);
		}
	}
	void InitExam(COM_Exam exam)
	{
		ResExamData (ExamSystem._Qindex);
		_RigthNum = (int)ExamSystem._RightNum;
		ResExamData (ExamSystem._Qindex);
		_RigthNum = ExamSystem._RightNum;
		_ObtainExpLable.text = ExamSystem._Exp.ToString ();
		_ObtainMoneyLable.text = ExamSystem._Money.ToString ();
		_CorrectRateLable.text = _RigthNum +"/"+ ExamSystem._Qindex;
		if(_RigthNum>=10)
		{
			_TenNumLable.text = "1/1";
		}else
		{
			_TenNumLable.text = "0/1";
		}
		if(_RigthNum>=20)
		{
			_TwentyNumLable.text = "1/1";
		}else
		{
			_TwentyNumLable.text = "0/1";
		}

	}

	void UpdateExamData(COM_Answer Answer)
	{
		if(Answer.isRigth_)
		{
			_RigthNum++;
			ExamSystem._RightNum = _RigthNum;
			EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_UI_huidazhengque, pos.transform,null,(GameObject obj)=>{
				obj.transform.localPosition = Vector3.zero;
			});

		}else
		{
			EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_UI_huidacuowu,  pos.transform,null,(GameObject obj)=>{
				obj.transform.localPosition = Vector3.zero;
			});

		}
		ExamSystem._Qindex = Answer.questionIndex_;
		_ObtainExpLable.text = Answer.exp_.ToString ();
		_ObtainMoneyLable.text = Answer.money_.ToString ();
		ExamSystem._Qindex++;
		_CorrectRateLable.text = ExamSystem._RightNum +"/"+ExamSystem._Qindex;
		ResExamData(ExamSystem._Qindex);
		if(_RigthNum>=10)
		{
			_TenNumLable.text = "1/1";
		}else
		{
			_TenNumLable.text = "0/1";
		}
	    if(_RigthNum>=20)
		{
			_TwentyNumLable.text = "1/1";
		}else
		{
			_TwentyNumLable.text = "0/1";
		}
					

	}

	void ResExamData(int index)
	{

		if((index)< ExamSystem._Exam.questions_.Length)
		{
			uint questionId = ExamSystem._Exam.questions_[index];
			QuestionData qdata = QuestionData.GetData ((int)questionId);
			_ContentLable.text = qdata._Question;
			_ALable.text = qdata._Answer1;
			_BLable.text = qdata._Answer2;
			_CLable.text = qdata._Answer3;
			_DLable.text = qdata._Answer4;
			_NumberLable.text = LanguageManager.instance.GetValue ("ti").Replace ("{n}",(++index).ToString());
		}else
		{
			//
			ClearTextData();
		}


	}
	string ExamNum(int num)
	{
		string numStr = "";
		string tempstr = "";
		char[] numcar = num.ToString ().ToCharArray ();
		int le =numcar.Length;
		if(le == 1)
		{
			if(numcar[0].ToString()=="0")
			{
				numcar[0] = '1';
			}
			numStr = LanguageManager.instance.GetValue(numcar[0].ToString());
		}else
		{
			for(int i =0;i<numcar.Length;i++)
			{
				if(int.Parse(numcar[0].ToString())<2)
				{
					numcar[0] = '0';
				}
				numStr+=LanguageManager.instance.GetValue(numcar[i].ToString());
			}
		}
		return numStr;
	}
	void ClearTextData()
	{
		stateSp.gameObject.SetActive (true);
		_ContentLable.text = "";
		_ALable.text ="";
		_BLable.text = "";
		_CLable.text = "";
		_DLable.text = "";
		_NumberLable.text = "";
		_Abtn.gameObject.SetActive(false);
		_Bbtn.gameObject.SetActive(false);
		_Cbtn.gameObject.SetActive(false);
		_Dbtn.gameObject.SetActive(false);
	}
	void num(int index)
	{
		//_NumberLable.text = index.ToString ();
	}
	void OnClickconfirm(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.sendExamAnswer ((uint)ExamSystem._Qindex,(byte)param1);
	}
	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	void Update () {
	
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_Dati);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_Dati);
	}
	public override void Destroyobj ()
	{
		ExamSystem.InitExamData -= InitExam;
		ExamSystem.UpdateExam -= UpdateExamData;
		ExamSystem.UpdateActivityState -= CloseActivity;
	}
}
