using UnityEngine;
using System.Collections;

public class ButtonHandler
{
	private event OnTouchButtonHandler mHandler = null;
	private object mParam1 = 0;
	private object mParam2 = 0;
	public void DestroyHandler()
	{
		if(mHandler != null)
		{
			mHandler -= mHandler;
			mHandler = null;
		}
	}

	public void SetHander(OnTouchButtonHandler handler, object param1, object param2)
	{
		DestroyHandler();
		
		mHandler += handler;
		mParam1 = param1;
		mParam2 = param2;
	}

	public void SetParam(object param1, object param2)
	{
		mParam1 = param1;
		mParam2 = param2;
	}
	
	public void HandlerCallback(ButtonScript buttonScript, object obj)
	{
		if(mHandler != null)
		{
			mHandler(buttonScript, obj, (int)mParam1, (int)mParam2);
		}
	}
}

public class ButtonScript : MonoBehaviour 
{	
	//	private event OnTouchButtonHandler mOnTouchButtonHandler = null;
	//	private int mOnTouchButtonHandlerParam1;
	//	private int mOnTouchButtonHandlerParam2;
	
	public int m_HandlerParam1;
	public int m_HandlerParam2;
	
	public event OnTouchButtonHandler mOnTouchButtonHandler = null;
	public event OnTouchButtonHandler mOnPressButtonHandler = null;
	public event OnTouchButtonHandler mOnDropButtonHandler = null;
	
	public event OnTouchButtonHandler mCheckOnButtonHandler = null;
	public event OnTouchButtonHandler mCheckOffButtonHandler = null;
	
	////////////////////////////////////////////////////////////
	// new 
	private ButtonHandler mOnClick = null;	
	private ButtonHandler mOnPress = null;
	private ButtonHandler mOnDrop = null;
	
	private ButtonHandler mCheckOn = null;
	private ButtonHandler mCheckOff = null;
	
	private ButtonHandler mTouchDown = null;
	private ButtonHandler mTouchUp = null;
	
	private long lastClickTime  = 0;
	private int interval       = 100;
	
	////////////////////////////////////////////////////////////
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	void OnDestroy()
	{
		RemoveAllHandler();
	}

	void RemoveAllHandler()
	{
		if (mOnTouchButtonHandler != null)
		{
			mOnTouchButtonHandler -= mOnTouchButtonHandler;
		}
		
		if(mOnPressButtonHandler != null)
		{
			mOnPressButtonHandler -= mOnPressButtonHandler;
		}
		
		if(mOnDropButtonHandler != null)
		{
			mOnDropButtonHandler -= mOnDropButtonHandler;
		}
		
		if(mCheckOnButtonHandler != null)
		{
			mCheckOnButtonHandler -= mCheckOnButtonHandler;
		}
		
		if(mCheckOffButtonHandler != null)
		{
			mCheckOffButtonHandler -= mCheckOffButtonHandler;
		}		
		
		////////////////////////////////////////////////////////////
		//new
		if(mOnClick != null)
		{
			mOnClick.DestroyHandler();
			mOnClick = null;
		}
		
		if(mOnPress != null)
		{
			mOnPress.DestroyHandler();
			mOnPress = null;
		}
		
		if(mCheckOn != null)
		{
			mCheckOn.DestroyHandler();
			mCheckOn = null;
		}
		
		if(mCheckOff != null)
		{
			mCheckOff.DestroyHandler();
			mCheckOff = null;
		}
		
		if(mOnDrop != null)
		{
			mOnDrop.DestroyHandler();
			mOnDrop = null;
		}
		
		if(mTouchDown != null)
		{
			mTouchDown.DestroyHandler();
			mTouchDown = null;
		}
		
		if(mTouchUp != null)
		{
			mTouchUp.DestroyHandler();
			mTouchUp = null;
		}
		////////////////////////////////////////////////////////////
	}
	

	public void OnClick()
	{
        if (Input.touchCount > 1)
            return;

		if(mOnTouchButtonHandler != null)
		{
			mOnTouchButtonHandler(this, null, m_HandlerParam1, m_HandlerParam2);
		}

		if(mOnClick != null)
		{
            if (GamePlayer.Instance.isInBattle && gameObject.CompareTag("DisableInBattle"))
            {
                //PopText.Instance.Show(LanguageManager.instance.GetValue("battlecannot"), PopText.WarningType.WT_Warning, true);
                return;
            }
			mOnClick.HandlerCallback(this, null);
			//2015-7-15 xiaoxiao add
			SoundTools.PlaySound( SOUND_ID.SOUND_BUTTONCLICK );
		}

		////////////////////////////////////////////////////////////
	}


	
	public void OnPress(bool isPressed)
	{
		if(mOnPressButtonHandler != null)
		{
			mOnPressButtonHandler(this, isPressed, m_HandlerParam1, m_HandlerParam2);
		}
		
		////////////////////////////////////////////////////////////
		//new
		
		if(mOnPress != null)
		{
			mOnPress.HandlerCallback(this, isPressed);
		}
		
		if(isPressed)
		{
			if(mTouchDown != null)
			{
				mTouchDown.HandlerCallback(this, null);
			}
		}
		else
		{
			if(mTouchUp != null)
			{
				mTouchUp.HandlerCallback(this, null);
			}
		}
		////////////////////////////////////////////////////////////
	}
	
	public void OnDrop(GameObject go)
	{
		if(mOnDropButtonHandler != null)
		{
			mOnDropButtonHandler(this, go, m_HandlerParam1, m_HandlerParam2);
		}
		
		////////////////////////////////////////		
		//new
		if(mOnDrop != null)
		{
			mOnDrop.HandlerCallback(this, go);
		}
		
		//////////////////////////////////////////
	}

	public void SetButtonScriptParam(object param1, object param2)
	{
		if(mOnClick == null)
		{
			mOnClick = new ButtonHandler();
		}
		mOnClick.SetParam(param1, param2);
	}
	
	public void SetButtonScriptHandler(EnumButtonEvent buttonEvent, OnTouchButtonHandler handler, object param1, object param2)
	{	
		switch(buttonEvent)
		{
		case EnumButtonEvent.OnClick:
			if(mOnClick == null)
			{
				mOnClick = new ButtonHandler();
			}
			mOnClick.SetHander(handler, param1, param2);
			break;
		case EnumButtonEvent.OnPress:
			if(mOnPress == null)
			{
				mOnPress = new ButtonHandler();
			}
			mOnPress.SetHander(handler, param1, param2);
			break;
		case EnumButtonEvent.OnDrop:
			if(mOnDrop == null)
			{
				mOnDrop = new ButtonHandler();
			}
			mOnDrop.SetHander(handler, param1, param2);
			break;
		case EnumButtonEvent.CheckOn:
			if(mCheckOn == null)
			{
				mCheckOn = new ButtonHandler();
			}
			mCheckOn.SetHander(handler, param1, param2);
			break;
		case EnumButtonEvent.CheckOff:
			if(mCheckOff == null)
			{
				mCheckOff = new ButtonHandler();
			}
			mCheckOff.SetHander(handler, param1, param2);
			break;
		case EnumButtonEvent.TouchDown:
			if(mTouchDown == null)
			{
				mTouchDown = new ButtonHandler();
			}
			mTouchDown.SetHander(handler, param1, param2);
			break;
		case EnumButtonEvent.TouchUp:
			if(mTouchUp == null)
			{
				mTouchUp = new ButtonHandler();
			}
			mTouchUp.SetHander(handler, param1, param2);
			break;
		}
	}
	
	public void RemoveButtonScriptHandler(EnumButtonEvent buttonEvent)
	{
		switch(buttonEvent)
		{
		case EnumButtonEvent.OnClick:
			if(mOnClick != null)
			{
				mOnClick.DestroyHandler();
				mOnClick = null;
			}
			break;
		case EnumButtonEvent.OnPress:
			if(mOnPress != null)
			{
				mOnPress.DestroyHandler();
				mOnPress = null;
			}
			break;
		case EnumButtonEvent.OnDrop:
			if(mOnDrop != null)
			{
				mOnDrop.DestroyHandler();
				mOnDrop = null;
			}
			break;
		case EnumButtonEvent.CheckOn:
			if(mCheckOn != null)
			{
				mCheckOn.DestroyHandler();
				mCheckOn = null;
			}
			break;
		case EnumButtonEvent.CheckOff:
			if(mCheckOff != null)
			{
				mCheckOff.DestroyHandler();
				mCheckOff = null;
			}
			break;
		case EnumButtonEvent.TouchDown:	
			if(mTouchDown != null)
			{
				mTouchDown.DestroyHandler();
				mTouchDown = null;
			}
			break;
		case EnumButtonEvent.TouchUp:
			if(mTouchUp != null)
			{
				mTouchUp.DestroyHandler();
				mTouchUp = null;
			}
			break;
		}
	}
	
	public void RemoveButtonScriptAllHandler()
	{
		RemoveAllHandler();
	}
}

