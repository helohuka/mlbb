using UnityEngine;
using System.Collections;

public class CommonEvent {

	public enum DefineAccountOperate
	{
		LOGIN,/**< enum value is the login operate. */
		LOGOUT,/**< enum value is the logout operate. */
		REGISTER,/**< enum value is the register operate. */
		LevelUp,
	} ;

	public delegate void ApplicationResumeHandler();
	public delegate void ApplicationPauseHandler();
	public delegate void AccountChangeHandler(DefineAccountOperate oper);
	public delegate void QuestStartHandler(int questid);
	public delegate void QuestFailHandler(int questid);
	public delegate void QuestFinishHandler(int questid);
	public delegate void PurchaseHandler(int itemid, int stack, int price);
	public delegate void UseItemHandler(int itemid, int stack);
	public delegate void RewardVirtualCashHandler(int count, string reason);
	public delegate void ExceptionHandler(string message);
    public delegate void UserExternalHandler(int code);

	public static event ApplicationPauseHandler OnAppPause;
	public static event ApplicationResumeHandler OnAppResume;
	public static event AccountChangeHandler OnAccountChange;
	public static event QuestStartHandler OnQuestStart;
	public static event QuestFailHandler OnQuestFail;
	public static event QuestFinishHandler OnQuestFinish;
	public static event PurchaseHandler OnPurchase;
	public static event UseItemHandler OnUseItem;
	public static event RewardVirtualCashHandler OnRewardVirtualCash;
	public static event ExceptionHandler OnException;
    public static event UserExternalHandler OnUserExternal;

	public static void ExcuteAppPause()
	{
		if(OnAppPause != null)
			OnAppPause();
	}

	public static void ExcuteAppResume()
	{
		if(OnAppResume != null)
			OnAppResume();
	}

	public static void ExcuteAccountChange(DefineAccountOperate type)
	{
		if(OnAccountChange != null)
			OnAccountChange(type);
	}

	public static void ExcuteQuestStart(int questid)
	{
		if(OnQuestStart != null)
			OnQuestStart(questid);
	}

	public static void ExcuteQuestFail(int questid)
	{
		if(OnQuestFail != null)
			OnQuestFail(questid);
	}

	public static void ExcuteQuestFinish(int questid)
	{
		if(OnQuestFinish != null)
			OnQuestFinish(questid);
	}

	public static void ExcutePurchase(int itemid, int stack, int price)
	{
		if(OnPurchase != null)
			OnPurchase(itemid, stack, price);
	}

	public static void ExcuteUseItem(int itemid, int stack)
	{
		if(OnUseItem != null)
			OnUseItem(itemid, stack);
	}

	public static void ExcuteRewardDiamond(int count, string reason)
	{
		if(OnRewardVirtualCash != null)
			OnRewardVirtualCash(count, reason);
	}

	public static void ExcuteException(string message)
	{
		if(OnException != null)
			OnException(message);
	}

    public static void ExcuteUserExternal(int code)
    {
        if (OnUserExternal != null)
            OnUserExternal(code);
    }
}
