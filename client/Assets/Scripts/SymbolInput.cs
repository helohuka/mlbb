//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_BLACKBERRY)
#define MOBILE
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Input field makes it possible to enter custom information within the UI.
/// </summary>

//[AddComponentMenu("NGUI/UI/Input Field")]
public class SymbolInput : UIInput
{
	/// <summary>
	/// Text label used to display the input's value.
	/// </summary>

	public SymbolLabel newLabel;

	/// <summary>
	/// Input field's value.
	/// </summary>

    //[SerializeField][HideInInspector] protected string mValue;

    //protected string mDefaultText = "";
    //protected Color mDefaultColor = Color.white;
    //protected float mPosition = 0f;
    //protected bool mDoInit = true;
    //protected UIWidget.Pivot mPivot = UIWidget.Pivot.TopLeft;

    //static protected int mDrawStart = 0;

//#if MOBILE
//    static protected TouchScreenKeyboard mKeyboard;
//#else
//    protected int mSelectionStart = 0;
//    protected int mSelectionEnd = 0;
//    protected UITexture mHighlight = null;
//    protected UITexture mCaret = null;
//    protected Texture2D mBlankTex = null;
//    protected float mNextBlink = 0f;
//    protected float mLastAlpha = 0f;

//    static protected string mLastIME = "";
//#endif

	/// <summary>
	/// Default text used by the input's label.
	/// </summary>

	public new string defaultText
	{
		get
		{
			return mDefaultText;
		}
		set
		{
			if (mDoInit) Init();
			mDefaultText = value;
			UpdateLabel();
		}
	}

	[System.Obsolete("Use UIInput.value instead")]
	public new string text { get { return this.value; } set { this.value = value; } }

	/// <summary>
	/// Input field's current text value.
	/// </summary>

	public new string value
	{
		get
		{
#if UNITY_EDITOR
			if (!Application.isPlaying) return "";
#endif
			if (mDoInit) Init();
			return mValue;
		}
		set
		{
#if UNITY_EDITOR
			if (!Application.isPlaying) return;
#endif
			if (mDoInit) Init();
			mDrawStart = 0;

#if MOBILE && !UNITY_3_5
			// BB10's implementation has a bug in Unity
			if (Application.platform == RuntimePlatform.BB10Player)
				value = value.Replace("\\b", "\b");
#endif
			// Validate all input
			value = Validate(value);
#if MOBILE
			if (isSelected && mKeyboard != null && mCached != value)
			{
				mKeyboard.text = value;
				mCached = value;
			}

			if (mValue != value)
			{
				mValue = value;
				if (!isSelected) SaveToPlayerPrefs(value);
				UpdateLabel();
				ExecuteOnChange();
			}
#else
			if (mValue != value)
			{
				mValue = value;

				if (isSelected)
				{
					if (string.IsNullOrEmpty(value))
					{
						mSelectionStart = 0;
						mSelectionEnd = 0;
					}
					else
					{
						mSelectionStart = value.Length;
						mSelectionEnd = mSelectionStart;
					}
				}
				else SaveToPlayerPrefs(value);

				UpdateLabel();
				ExecuteOnChange();
			}
#endif
		}
	}

	[System.Obsolete("Use UIInput.isSelected instead")]
	public new bool selected { get { return isSelected; } set { isSelected = value; } }

	/// <summary>
	/// Whether the input is currently selected.
	/// </summary>

	public new bool isSelected
	{
		get
		{
			return selection == this;
		}
		set
		{
			if (!value) { if (isSelected) UICamera.selectedObject = null; }
			else UICamera.selectedObject = gameObject;
		}
	}

#if MOBILE
	/// <summary>
	/// Current position of the cursor.
	/// </summary>

	public int cursorPosition { get { return value.Length; } set {} }

	/// <summary>
	/// Index of the character where selection begins.
	/// </summary>

	public int selectionStart { get { return value.Length; } set {} }

	/// <summary>
	/// Index of the character where selection ends.
	/// </summary>

	public int selectionEnd { get { return value.Length; } set {} }
#else
	/// <summary>
	/// Current position of the cursor.
	/// </summary>

	public new int cursorPosition
	{
		get
		{
			return isSelected ? mSelectionEnd : value.Length;
		}
		set
		{
			if (isSelected)
			{
				mSelectionEnd = value;
				UpdateLabel();
			}
		}
	}

	/// <summary>
	/// Index of the character where selection begins.
	/// </summary>

	public new int selectionStart
	{
		get
		{
			return isSelected ? mSelectionStart : value.Length;
		}
		set
		{
			if (isSelected)
			{
				mSelectionStart = value;
				UpdateLabel();
			}
		}
	}

	/// <summary>
	/// Index of the character where selection ends.
	/// </summary>

	public new int selectionEnd
	{
		get
		{
			return isSelected ? mSelectionEnd : value.Length;
		}
		set
		{
			if (isSelected)
			{
				mSelectionEnd = value;
				UpdateLabel();
			}
		}
	}
#endif

	/// <summary>
	/// Validate the specified text, returning the validated version.
	/// </summary>

	public new string Validate (string val)
	{
		if (string.IsNullOrEmpty(val)) return "";

		StringBuilder sb = new StringBuilder(val.Length);

		for (int i = 0; i < val.Length; ++i)
		{
			char c = val[i];
			if (onValidate != null) c = onValidate(sb.ToString(), sb.Length, c);
			else if (validation != Validation.None) c = Validate(sb.ToString(), sb.Length, c);
			if (c != 0) sb.Append(c);
		}

		if (characterLimit > 0 && sb.Length > characterLimit)
			return sb.ToString(0, characterLimit);
		return sb.ToString();
	}

	/// <summary>
	/// Automatically set the value by loading it from player prefs if possible.
	/// </summary>

	void Start ()
	{
		if (string.IsNullOrEmpty(mValue))
		{
			if (!string.IsNullOrEmpty(savedAs) && PlayerPrefs.HasKey(savedAs))
				value = PlayerPrefs.GetString(savedAs);
		}
		else value = mValue.Replace("\\n", "\n");
	}

	/// <summary>
	/// Labels used for input shouldn't support rich text.
	/// </summary>

	protected new void Init ()
	{
		if (mDoInit && newLabel != null)
		{
			mDoInit = false;
			mDefaultText = newLabel.text;
            mDefaultColor = newLabel.labelText.color;
            //newLabel.labelText.supportEncoding = false;

            if (newLabel.labelText.alignment == NGUIText.Alignment.Justified)
			{
                newLabel.labelText.alignment = NGUIText.Alignment.Left;
				Debug.LogWarning("Input fields using labels with justified alignment are not supported at this time", this);
			}

            mPivot = newLabel.labelText.pivot;
            mPosition = newLabel.labelText.cachedTransform.localPosition.x;
			UpdateLabel();
		}
	}

	/// <summary>
	/// Save the specified value to player prefs.
	/// </summary>

	protected new void SaveToPlayerPrefs (string val)
	{
		if (!string.IsNullOrEmpty(savedAs))
		{
			if (string.IsNullOrEmpty(val)) PlayerPrefs.DeleteKey(savedAs);
			else PlayerPrefs.SetString(savedAs, val);
		}
	}

	/// <summary>
	/// Selection event, sent by the EventSystem.
	/// </summary>

	override protected void OnSelect (bool isSelected)
	{
		if (isSelected) OnSelectEvent();
		else OnDeselectEvent();
	}

	/// <summary>
	/// Notification of the input field gaining selection.
	/// </summary>

	protected new void OnSelectEvent ()
	{
		selection = this;

		if (mDoInit) Init();
        
		if (newLabel != null && NGUITools.GetActive(this))
		{
            newLabel.labelText.color = activeTextColor;
#if MOBILE
			if (Application.platform == RuntimePlatform.IPhonePlayer ||
				Application.platform == RuntimePlatform.Android
#if UNITY_WP8
				|| Application.platform == RuntimePlatform.WP8Player
#endif
#if UNITY_BLACKBERRY
				|| Application.platform == RuntimePlatform.BB10Player
#endif
			)
			{
				mKeyboard = (inputType == InputType.Password) ?
					TouchScreenKeyboard.Open(mValue, TouchScreenKeyboardType.Default, false, false, true) :
					TouchScreenKeyboard.Open(mValue, (TouchScreenKeyboardType)((int)keyboardType), inputType == InputType.AutoCorrect, newLabel.labelText.multiLine, false, false, defaultText);
			}
			else
#endif
            {
				Vector2 pos = (UICamera.current != null && UICamera.current.cachedCamera != null) ?
                    UICamera.current.cachedCamera.WorldToScreenPoint(newLabel.labelText.worldCorners[0]) :
                    newLabel.labelText.worldCorners[0];
				pos.y = Screen.height - pos.y;
				Input.imeCompositionMode = IMECompositionMode.On;
				Input.compositionCursorPos = pos;
#if !MOBILE
				mSelectionStart = 0;
				mSelectionEnd = string.IsNullOrEmpty(mValue) ? 0 : mValue.Length;
#endif
				mDrawStart = 0;
			}
			UpdateLabel();
		}
	}

	/// <summary>
	/// Notification of the input field losing selection.
	/// </summary>

	protected new void OnDeselectEvent ()
	{
		if (mDoInit) Init();

		if (newLabel != null && NGUITools.GetActive(this))
		{
			mValue = value;
#if MOBILE
			if (mKeyboard != null)
			{
				mKeyboard.active = false;
				mKeyboard = null;
			}
#endif
			if (string.IsNullOrEmpty(mValue))
			{
				newLabel.text = mDefaultText;
                newLabel.labelText.color = mDefaultColor;
			}
			else newLabel.text = mValue;

			Input.imeCompositionMode = IMECompositionMode.Auto;
			RestoreLabelPivot();
		}
		
		selection = null;
		UpdateLabel();
	}

	/// <summary>
	/// Update the text based on input.
	/// </summary>

#if MOBILE
	string mCached = "";

	void Update()
	{
		if (mKeyboard != null && isSelected)
		{
			string text = mKeyboard.text;

			if (mCached != text)
			{
				mCached = text;
				value = text;
			}

			if (mKeyboard.done)
			{
#if !UNITY_3_5
				if (!mKeyboard.wasCanceled)
#endif
					Submit();
				mKeyboard = null;
				isSelected = false;
				mCached = "";
			}
		}
	}
#else
	void Update ()
	{
#if UNITY_EDITOR
		if (!Application.isPlaying) return;
#endif
		if (isSelected)
		{
			if (mDoInit) Init();

			if (selectOnTab != null && Input.GetKeyDown(KeyCode.Tab))
			{
				UICamera.selectedObject = selectOnTab;
				return;
			}

			string ime = Input.compositionString;

			// There seems to be an inconsistency between IME on Windows, and IME on OSX.
			// On Windows, Input.inputString is always empty while IME is active. On the OSX it is not.
			if (string.IsNullOrEmpty(ime) && !string.IsNullOrEmpty(Input.inputString))
			{
				// Process input ignoring non-printable characters as they are not consistent.
				// Windows has them, OSX may not. They get handled inside OnGUI() instead.
				string s = Input.inputString;

				for (int i = 0; i < s.Length; ++i)
				{
					char ch = s[i];
					if (ch < ' ') continue;

					// OSX inserts these characters for arrow keys
					if (ch == '\uF700') continue;
					if (ch == '\uF701') continue;
					if (ch == '\uF702') continue;
					if (ch == '\uF703') continue;

					Insert(ch.ToString());
				}
			}

			// Append IME composition
			if (mLastIME != ime)
			{
				mSelectionEnd = string.IsNullOrEmpty(ime) ? mSelectionStart : mValue.Length + ime.Length;
				mLastIME = ime;
				UpdateLabel();
				ExecuteOnChange();
			}

			// Blink the caret
			if (mCaret != null && mNextBlink < RealTime.time)
			{
				mNextBlink = RealTime.time + 0.5f;
				mCaret.enabled = !mCaret.enabled;
			}

			// If the label's final alpha changes, we need to update the drawn geometry,
			// or the highlight widgets (which have their geometry set manually) won't update.
            if (isSelected && mLastAlpha != newLabel.labelText.finalAlpha)
				UpdateLabel();
		}
	}

	/// <summary>
	/// Unfortunately Unity 4.3 and earlier doesn't offer a way to properly process events outside of OnGUI.
	/// </summary>

	void OnGUI ()
	{
		if (isSelected && Event.current.rawType == EventType.KeyDown)
			ProcessEvent(Event.current);
	}

	/// <summary>
	/// Handle the specified event.
	/// </summary>

	bool ProcessEvent (Event ev)
	{
		if (newLabel == null) return false;

		RuntimePlatform rp = Application.platform;

		bool isMac = (
			rp == RuntimePlatform.OSXEditor ||
			rp == RuntimePlatform.OSXPlayer ||
			rp == RuntimePlatform.OSXWebPlayer);

		bool ctrl = isMac ?
			((ev.modifiers & EventModifiers.Command) != 0) :
			((ev.modifiers & EventModifiers.Control) != 0);

		bool shift = ((ev.modifiers & EventModifiers.Shift) != 0);

		switch (ev.keyCode)
		{
			case KeyCode.Backspace:
			{
				ev.Use();

				if (!string.IsNullOrEmpty(mValue))
				{
					if (mSelectionStart == mSelectionEnd)
					{
						if (mSelectionStart < 1) return true;
						--mSelectionEnd;
					}
					Insert("");
				}
				return true;
			}

			case KeyCode.Delete:
			{
				ev.Use();

				if (!string.IsNullOrEmpty(mValue))
				{
					if (mSelectionStart == mSelectionEnd)
					{
						if (mSelectionStart >= mValue.Length) return true;
						++mSelectionEnd;
					}
					Insert("");
				}
				return true;
			}

			case KeyCode.LeftArrow:
			{
				ev.Use();

				if (!string.IsNullOrEmpty(mValue))
				{
					mSelectionEnd = Mathf.Max(mSelectionEnd - 1, 0);
					if (!shift) mSelectionStart = mSelectionEnd;
					UpdateLabel();
				}
				return true;
			}

			case KeyCode.RightArrow:
			{
				ev.Use();

				if (!string.IsNullOrEmpty(mValue))
				{
					mSelectionEnd = Mathf.Min(mSelectionEnd + 1, mValue.Length);
					if (!shift) mSelectionStart = mSelectionEnd;
					UpdateLabel();
				}
				return true;
			}

			case KeyCode.PageUp:
			{
				ev.Use();

				if (!string.IsNullOrEmpty(mValue))
				{
					mSelectionEnd = 0;
					if (!shift) mSelectionStart = mSelectionEnd;
					UpdateLabel();
				}
				return true;
			}

			case KeyCode.PageDown:
			{
				ev.Use();

				if (!string.IsNullOrEmpty(mValue))
				{
					mSelectionEnd = mValue.Length;
					if (!shift) mSelectionStart = mSelectionEnd;
					UpdateLabel();
				}
				return true;
			}

			case KeyCode.Home:
			{
				ev.Use();

				if (!string.IsNullOrEmpty(mValue))
				{
                    if (newLabel.labelText.multiLine)
					{
                        mSelectionEnd = newLabel.labelText.GetCharacterIndex(mSelectionEnd, KeyCode.Home);
					}
					else mSelectionEnd = 0;

					if (!shift) mSelectionStart = mSelectionEnd;
					UpdateLabel();
				}
				return true;
			}

			case KeyCode.End:
			{
				ev.Use();

				if (!string.IsNullOrEmpty(mValue))
				{
                    if (newLabel.labelText.multiLine)
					{
                        mSelectionEnd = newLabel.labelText.GetCharacterIndex(mSelectionEnd, KeyCode.End);
					}
					else mSelectionEnd = mValue.Length;

					if (!shift) mSelectionStart = mSelectionEnd;
					UpdateLabel();
				}
				return true;
			}

			case KeyCode.UpArrow:
			{
				ev.Use();

				if (!string.IsNullOrEmpty(mValue))
				{
                    mSelectionEnd = newLabel.labelText.GetCharacterIndex(mSelectionEnd, KeyCode.UpArrow);
					if (mSelectionEnd != 0) mSelectionEnd += mDrawStart;
					if (!shift) mSelectionStart = mSelectionEnd;
					UpdateLabel();
				}
				return true;
			}

			case KeyCode.DownArrow:
			{
				ev.Use();

				if (!string.IsNullOrEmpty(mValue))
				{
                    mSelectionEnd = newLabel.labelText.GetCharacterIndex(mSelectionEnd, KeyCode.DownArrow);
                    if (mSelectionEnd != newLabel.labelText.processedText.Length) mSelectionEnd += mDrawStart;
					else mSelectionEnd = mValue.Length;
					if (!shift) mSelectionStart = mSelectionEnd;
					UpdateLabel();
				}
				return true;
			}

			// Copy
			case KeyCode.C:
			{
				if (ctrl)
				{
					ev.Use();
					NGUITools.clipboard = GetSelection();
				}
				return true;
			}

			// Paste
			case KeyCode.V:
			{
				if (ctrl)
				{
					ev.Use();
					Insert(NGUITools.clipboard);
				}
				return true;
			}

			// Cut
			case KeyCode.X:
			{
				if (ctrl)
				{
					ev.Use();
					NGUITools.clipboard = GetSelection();
					Insert("");
				}
				return true;
			}

			// Submit
			case KeyCode.Return:
			case KeyCode.KeypadEnter:
			{
				ev.Use();

                if (newLabel.labelText.multiLine && !ctrl && newLabel.labelText.overflowMethod != UILabel.Overflow.ClampContent)
				{
					Insert("\n");
				}
				else
				{
					UICamera.currentScheme = UICamera.ControlScheme.Controller;
					UICamera.currentKey = ev.keyCode;
					Submit();
					UICamera.currentKey = KeyCode.None;
				}
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Insert the specified text string into the current input value, respecting selection and validation.
	/// </summary>

	override protected void Insert (string text)
	{
		string left = GetLeftText();
		string right = GetRightText();
		int rl = right.Length;

		StringBuilder sb = new StringBuilder(left.Length + right.Length + text.Length);
		sb.Append(left);

		// Append the new text
		for (int i = 0, imax = text.Length; i < imax; ++i)
		{
			// Can't go past the character limit
			if (characterLimit > 0 && sb.Length + rl >= characterLimit) break;

			// If we have an input validator, validate the input first
			char c = text[i];
			if (onValidate != null) c = onValidate(sb.ToString(), sb.Length, c);
			else if (validation != Validation.None) c = Validate(sb.ToString(), sb.Length, c);

			// Append the character if it hasn't been invalidated
			if (c != 0) sb.Append(c);
		}

		// Advance the selection
		mSelectionStart = sb.Length;
		mSelectionEnd = mSelectionStart;

		// Append the text that follows it, ensuring that it's also validated after the inserted value
		for (int i = 0, imax = right.Length; i < imax; ++i)
		{
			char c = right[i];
			if (onValidate != null) c = onValidate(sb.ToString(), sb.Length, c);
			else if (validation != Validation.None) c = Validate(sb.ToString(), sb.Length, c);
			if (c != 0) sb.Append(c);
		}

		mValue = sb.ToString();
		UpdateLabel();
		ExecuteOnChange();
	}

	/// <summary>
	/// Get the text to the left of the selection.
	/// </summary>

	protected new string GetLeftText ()
	{
		int min = Mathf.Min(mSelectionStart, mSelectionEnd);
		return (string.IsNullOrEmpty(mValue) || min < 0) ? "" : mValue.Substring(0, min);
	}

	/// <summary>
	/// Get the text to the right of the selection.
	/// </summary>

	protected new string GetRightText ()
	{
		int max = Mathf.Max(mSelectionStart, mSelectionEnd);
		return (string.IsNullOrEmpty(mValue) || max >= mValue.Length) ? "" : mValue.Substring(max);
	}

	/// <summary>
	/// Get currently selected text.
	/// </summary>

	protected new string GetSelection ()
	{
		if (string.IsNullOrEmpty(mValue) || mSelectionStart == mSelectionEnd)
		{
			return "";
		}
		else
		{
			int min = Mathf.Min(mSelectionStart, mSelectionEnd);
			int max = Mathf.Max(mSelectionStart, mSelectionEnd);
			return mValue.Substring(min, max - min);
		}
	}

	/// <summary>
	/// Helper function that retrieves the index of the character under the mouse.
	/// </summary>

	protected new int GetCharUnderMouse ()
	{
        Vector3[] corners = newLabel.labelText.worldCorners;
		Ray ray = UICamera.currentRay;
		Plane p = new Plane(corners[0], corners[1], corners[2]);
		float dist;
        return p.Raycast(ray, out dist) ? mDrawStart + newLabel.labelText.GetCharacterIndexAtPosition(ray.GetPoint(dist)) : 0;
	}

	/// <summary>
	/// Move the caret on press.
	/// </summary>

	protected override void OnPress (bool isPressed)
	{
		if (isPressed && isSelected && newLabel != null && UICamera.currentScheme == UICamera.ControlScheme.Mouse)
		{
			mSelectionEnd = GetCharUnderMouse();
			if (!Input.GetKey(KeyCode.LeftShift) &&
				!Input.GetKey(KeyCode.RightShift)) mSelectionStart = mSelectionEnd;
			UpdateLabel();
		}
	}

	/// <summary>
	/// Drag selection.
	/// </summary>

	protected override void OnDrag (Vector2 delta)
	{
		if (newLabel != null && UICamera.currentScheme == UICamera.ControlScheme.Mouse)
		{
			mSelectionEnd = GetCharUnderMouse();
			UpdateLabel();
		}
	}

	/// <summary>
	/// Ensure we've released the dynamically created resources.
	/// </summary>

	void OnDisable () { Cleanup(); }

	/// <summary>
	/// Cleanup.
	/// </summary>
    
	protected override void Cleanup ()
	{
		if (mHighlight) mHighlight.enabled = false;
		if (mCaret) mCaret.enabled = false;

		if (mBlankTex)
		{
			NGUITools.Destroy(mBlankTex);
			mBlankTex = null;
		}
	}
#endif // !MOBILE

	/// <summary>
	/// Submit the input field's text.
	/// </summary>

	public new void Submit ()
	{
		if (NGUITools.GetActive(this))
		{
			current = this;
			mValue = value;
			EventDelegate.Execute(onSubmit);
			SaveToPlayerPrefs(mValue);
			current = null;
		}
	}

	/// <summary>
	/// Update the visual text label.
	/// </summary>

	public new void UpdateLabel ()
	{
		if (newLabel != null)
		{
			if (mDoInit) Init();
			bool selected = isSelected;
			string fullText = value;
			bool isEmpty = string.IsNullOrEmpty(fullText) && string.IsNullOrEmpty(Input.compositionString);
            newLabel.labelText.color = (isEmpty && !selected) ? mDefaultColor : activeTextColor;
			string processed;

			if (isEmpty)
			{
				processed = selected ? "" : mDefaultText;
				RestoreLabelPivot();
			}
			else
			{
				if (inputType == InputType.Password)
				{
					processed = "";
					for (int i = 0, imax = fullText.Length; i < imax; ++i) processed += "*";
				}
				else processed = fullText;

				// Start with text leading up to the selection
				int selPos = selected ? Mathf.Min(processed.Length, cursorPosition) : 0;
				string left = processed.Substring(0, selPos);

				// Append the composition string and the cursor character
				if (selected) left += Input.compositionString;

				// Append the text from the selection onwards
				processed = left + processed.Substring(selPos, processed.Length - selPos);

				// Clamped content needs to be adjusted further
                if (selected && newLabel.labelText.overflowMethod == UILabel.Overflow.ClampContent)
				{
					// Determine what will actually fit into the given line
                    int offset = newLabel.labelText.CalculateOffsetToFit(processed);

					if (offset == 0)
					{
						mDrawStart = 0;
						RestoreLabelPivot();
					}
					else if (selPos < mDrawStart)
					{
						mDrawStart = selPos;
						SetPivotToLeft();
					}
					else if (offset < mDrawStart)
					{
						mDrawStart = offset;
						SetPivotToLeft();
					}
					else
					{
                        offset = newLabel.labelText.CalculateOffsetToFit(processed.Substring(0, selPos));

						if (offset > mDrawStart)
						{
							mDrawStart = offset;
							SetPivotToRight();
						}
					}

					// If necessary, trim the front
                    // 处理末尾 开始 出现{ } 转义符
                    if (mDrawStart != 0)
                    {
                        processed = processed.Substring(mDrawStart, processed.Length - mDrawStart);
                        int back = 0;
                        int front = 0;
                        const int t = 3;
                        front = processed.IndexOf('}', 0, t);

                        if (front > -1)
                        {
                            processed = processed.Substring(front + 1);
                        }

                        back = processed.LastIndexOf('{', processed.Length - 1, 3);

                        if (back > -1)
                        {
                            int l = processed.Length;
                            processed = processed.Substring(0, back);
                        }

                    }
				}
				else
				{
					mDrawStart = 0;
					RestoreLabelPivot();
				}
			}

            newLabel.text = processed;

#if !MOBILE
			if (selected)
			{
				int start = mSelectionStart - mDrawStart;
				int end = mSelectionEnd - mDrawStart;

				// Blank texture used by selection and caret
				if (mBlankTex == null)
				{
					mBlankTex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
					for (int y = 0; y < 2; ++y)
						for (int x = 0; x < 2; ++x)
							mBlankTex.SetPixel(x, y, Color.white);
					mBlankTex.Apply();
				}

				// Create the selection highlight
				if (start != end)
				{
					if (mHighlight == null)
					{
						mHighlight = NGUITools.AddWidget<UITexture>(newLabel.labelText.cachedGameObject);
						mHighlight.name = "Input Highlight";
						mHighlight.mainTexture = mBlankTex;
						mHighlight.fillGeometry = false;
                        mHighlight.pivot = newLabel.labelText.pivot;
                        mHighlight.SetAnchor(newLabel.labelText.cachedTransform);
					}
					else
					{
                        mHighlight.pivot = newLabel.labelText.pivot;
						mHighlight.mainTexture = mBlankTex;
						mHighlight.MarkAsChanged();
						mHighlight.enabled = true;
					}
				}

				// Create the caret
				if (mCaret == null)
				{
                    mCaret = NGUITools.AddWidget<UITexture>(newLabel.labelText.cachedGameObject);
					mCaret.name = "Input Caret";
					mCaret.mainTexture = mBlankTex;
					mCaret.fillGeometry = false;
                    mCaret.pivot = newLabel.labelText.pivot;
                    mCaret.SetAnchor(newLabel.labelText.cachedTransform);
				}
				else
				{
                    mCaret.pivot = newLabel.labelText.pivot;
					mCaret.mainTexture = mBlankTex;
					mCaret.MarkAsChanged();
					mCaret.enabled = true;
				}

				if (start != end)
				{
                    newLabel.labelText.PrintOverlay(start, end, mCaret.geometry, mHighlight.geometry, caretColor, selectionColor);
					mHighlight.enabled = mHighlight.geometry.hasVertices;
				}
				else
				{
                    newLabel.labelText.PrintOverlay(start, end, mCaret.geometry, null, caretColor, selectionColor);
					if (mHighlight != null) mHighlight.enabled = false;
				}

				// Reset the blinking time
				mNextBlink = RealTime.time + 0.5f;
                mLastAlpha = newLabel.labelText.finalAlpha;
			}
			else Cleanup();
#endif
		}
	}

	/// <summary>
	/// Set the label's pivot to the left.
	/// </summary>

	protected new void SetPivotToLeft ()
	{
		Vector2 po = NGUIMath.GetPivotOffset(mPivot);
		po.x = 0f;
        newLabel.labelText.pivot = NGUIMath.GetPivot(po);
	}

	/// <summary>
	/// Set the label's pivot to the right.
	/// </summary>

	protected new void SetPivotToRight ()
	{
		Vector2 po = NGUIMath.GetPivotOffset(mPivot);
		po.x = 1f;
        newLabel.labelText.pivot = NGUIMath.GetPivot(po);
	}

	/// <summary>
	/// Restore the input label's pivot point.
	/// </summary>

	protected new void RestoreLabelPivot ()
	{
        if (newLabel != null && newLabel.labelText.pivot != mPivot)
            newLabel.labelText.pivot = mPivot;
	}

	/// <summary>
	/// Validate the specified input.
	/// </summary>

	protected new char Validate (string text, int pos, char ch)
	{
		// Validation is disabled
		if (validation == Validation.None || !enabled) return ch;

		if (validation == Validation.Integer)
		{
			// Integer number validation
			if (ch >= '0' && ch <= '9') return ch;
			if (ch == '-' && pos == 0 && !text.Contains("-")) return ch;
		}
		else if (validation == Validation.Float)
		{
			// Floating-point number
			if (ch >= '0' && ch <= '9') return ch;
			if (ch == '-' && pos == 0 && !text.Contains("-")) return ch;
			if (ch == '.' && !text.Contains(".")) return ch;
		}
		else if (validation == Validation.Alphanumeric)
		{
			// All alphanumeric characters
			if (ch >= 'A' && ch <= 'Z') return ch;
			if (ch >= 'a' && ch <= 'z') return ch;
			if (ch >= '0' && ch <= '9') return ch;
		}
		else if (validation == Validation.Username)
		{
			// Lowercase and numbers
			if (ch >= 'A' && ch <= 'Z') return (char)(ch - 'A' + 'a');
			if (ch >= 'a' && ch <= 'z') return ch;
			if (ch >= '0' && ch <= '9') return ch;
		}
		else if (validation == Validation.Name)
		{
			char lastChar = (text.Length > 0) ? text[Mathf.Clamp(pos, 0, text.Length - 1)] : ' ';
			char nextChar = (text.Length > 0) ? text[Mathf.Clamp(pos + 1, 0, text.Length - 1)] : '\n';

			if (ch >= 'a' && ch <= 'z')
			{
				// Space followed by a letter -- make sure it's capitalized
				if (lastChar == ' ') return (char)(ch - 'a' + 'A');
				return ch;
			}
			else if (ch >= 'A' && ch <= 'Z')
			{
				// Uppercase letters are only allowed after spaces (and apostrophes)
				if (lastChar != ' ' && lastChar != '\'') return (char)(ch - 'A' + 'a');
				return ch;
			}
			else if (ch == '\'')
			{
				// Don't allow more than one apostrophe
				if (lastChar != ' ' && lastChar != '\'' && nextChar != '\'' && !text.Contains("'")) return ch;
			}
			else if (ch == ' ')
			{
				// Don't allow more than one space in a row
				if (lastChar != ' ' && lastChar != '\'' && nextChar != ' ' && nextChar != '\'') return ch;
			}
		}
		return (char)0;
	}

	/// <summary>
	/// Execute the OnChange callback.
	/// </summary>

	protected new void ExecuteOnChange ()
	{
		if (EventDelegate.IsValid(onChange))
		{
			current = this;
			EventDelegate.Execute(onChange);
			current = null;
		}
	}
}
