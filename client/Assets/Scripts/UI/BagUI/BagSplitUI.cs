using UnityEngine;
using System.Collections;

public class BagSplitUI : MonoBehaviour
{
	public UIPanel tipspane;
	public UISprite splitPane;
	public UITexture item1;
	public UITexture item2;
	public UILabel numLab;
	public UIButton splitBtn;
	public UIButton addBtn;
	public UIButton minusBtn;
	public UILabel itemNum1;
	public UILabel itemNum2;
	public UILabel name;
	public UILabel inputLab;
	public UILabel BagSplitTitleLab;
	public UILabel BagSplitBtnLab;

	private int _splitNum;
	private COM_Item _itemInst;

	void Start ()
	{
		UIManager.SetButtonEventHandler (addBtn.gameObject, EnumButtonEvent.OnClick, OnAdd, 0, 0);
		UIManager.SetButtonEventHandler (minusBtn.gameObject, EnumButtonEvent.OnClick, OnMinus, 0, 0);
		 
		UIManager.SetButtonEventHandler (splitBtn.gameObject, EnumButtonEvent.OnClick, OnSplit, 0, 0);
		BagSplitTitleLab.text = LanguageManager.instance.GetValue("BagSplitTitleLab");
		BagSplitBtnLab.text = LanguageManager.instance.GetValue("BagSplitBtnLab");
	}
	

	private void OnSplit(ButtonScript obj, object args, int param1, int param2)
	{
		_splitNum = int.Parse (numLab.text);
		if(_splitNum <= 0 || _splitNum >= (int)_itemInst.stack_ )
		{
			PopText.Instance.Show (LanguageManager.instance.GetValue ("cannotcaifen"));
			return;
		}

		NetConnection.Instance.bagItemSplit ((int)_itemInst.instId_, _splitNum);
		splitPane.gameObject.SetActive (false);
		tipspane.gameObject.SetActive (false);
		PopText.Instance.Show (LanguageManager.instance.GetValue ("caifenok"));
	}

	private void OnAdd(ButtonScript obj, object args, int param1, int param2)
	{
		SplitNum =int.Parse (numLab.text);
		SplitNum++;
	}

	private void OnMinus(ButtonScript obj, object args, int param1, int param2)
	{
		SplitNum =int.Parse (numLab.text);
		SplitNum--;
	}

	public int SplitNum
	{
		set
		{
			if(_splitNum != value)
			{
				if(value > (int)_itemInst.stack_-1 || value < 0)
				{
					return;
				}
				_splitNum = value;
			
				if(_splitNum <= 0)
				{
					splitBtn.isEnabled = false;
				}
				else
				{
					splitBtn.isEnabled = true;
				}

				itemNum1.text = (_itemInst.stack_ - _splitNum).ToString();
				itemNum2.text = _splitNum.ToString();
				numLab.text = _splitNum.ToString();
			}
		}
		get
		{
			return _splitNum;
		}
	}

	public COM_Item ItemInst
	{
		set
		{
			_itemInst = value;
			HeadIconLoader.Instance.LoadIcon(ItemData.GetData((int)_itemInst.itemId_).icon_, item1);
			HeadIconLoader.Instance.LoadIcon(ItemData.GetData((int)_itemInst.itemId_).icon_, item2);
			itemNum1.text = _itemInst.stack_.ToString();
			itemNum2.text = "1";
			numLab.text ="1";
			SplitNum = 1;
		}
		get
		{
			return _itemInst;
		}
	}

}

