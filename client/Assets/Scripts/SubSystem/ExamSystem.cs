using UnityEngine;
using System.Collections;
using System.Collections.Generic;
static public class ExamSystem {

	public delegate void InitExam(COM_Exam exam);
	public static event InitExam InitExamData;
	public delegate void OpenExam();
	public static event OpenExam OpenExamH;
	public delegate void OnUpdateExam(COM_Answer Answer);
	public static event OnUpdateExam UpdateExam;

	public delegate void UpdateActivity( bool open);
	public static event UpdateActivity UpdateActivityState;


	public static COM_Exam _Exam;
	public static bool _IsOpenExam = false;
	public static int _Qindex;
	public static int _RightNum;
	public static int _Money;
	public static int _Exp;
	public static void SyncExam(COM_Exam exam)
	{
		_Exam = exam;
		_Qindex = (int)exam.questionIndex_;
		_RightNum = (int)exam.rightNum_;
		_Money = (int)exam.money_;
		_Exp = (int)exam.exp_;
		if(InitExamData != null)
		{
			InitExamData(exam);
		}
		if(OpenExamH != null)
		{
			OpenExamH();
		}
	}
	public static void SyncExamAnswer(COM_Answer Answer)
	{
		_Qindex = (int)Answer.questionIndex_;
		_Money = (int)Answer.money_;
		_Exp = (int)Answer.exp_;
		if(UpdateExam != null)
		{
			UpdateExam(Answer);
		}
	}
	public static int _ExamOpenLevel;
	public static void updateActivityStatus(ActivityType type, bool open)
	{
		GlobalValue.Get(Constant.C_ExamOpenLevel, out _ExamOpenLevel);
		if(type == ActivityType.ACT_Exam)
		{
			if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level)>_ExamOpenLevel)
			{
				_IsOpenExam = open;
				if(UpdateActivityState != null)
				{
					UpdateActivityState(open);
				}
			}

		}

	}
	public static void ClearData()
	{
		 _Exam = null;
		 _IsOpenExam = false;
		 _Qindex = 0;
		_RightNum =0;
		 _Money = 0;
		 _Exp = 0;
	}

}
