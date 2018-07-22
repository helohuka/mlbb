using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class CopyOpenCellUI : MonoBehaviour
{

	public UISprite backImg;
	public UILabel openLevelLab;
	public UISprite num1Sp;
	public UISprite num2Sp;
	public UISprite noOpenImg;
	public UISprite noOpenLvImg;
	public List<UISprite> rewards = new List<UISprite> ();

	private CopyData _copyData;

	void Start ()
	{
		UIManager.SetButtonEventHandler(gameObject, EnumButtonEvent.OnClick, OnClickCell, 0, 0);
		GamePlayer.Instance.CopyNumEnvet += new RequestEventHandler<int> (OnCopyNumEnvet);
	}

	public CopyData cellData
	{
		set
		{
			if(value == null)
				return;
			_copyData = value;
			backImg.spriteName = _copyData._PicName;
			openLevelLab.text =_copyData._CopyID + LanguageManager.instance.GetValue("Level");
			if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level) < _copyData._CopyID)
			{
				noOpenImg.gameObject.SetActive(true);
				noOpenLvImg.spriteName = _copyData._CopyID.ToString(); 
			}
			 int num = GamePlayer.Instance.GetCopyNum(_copyData._SceneID);
			if(num == 0)
			{
				num1Sp.gameObject.SetActive(true);
				num2Sp.gameObject.SetActive(true);
			}
			else if(num == 1)
			{
				num1Sp.gameObject.SetActive(true);
				num2Sp.gameObject.SetActive(false);
			}
			else
			{
				num1Sp.gameObject.SetActive(false);
				num2Sp.gameObject.SetActive(false);
			}
			for(int i =0;i<_copyData._Reward.Length;i++)
			{
				ItemCellUI cell = UIManager.Instance.AddItemCellUI(rewards[i], uint.Parse(_copyData._Reward[i]));
				cell.showTips = true;
			}
		}
		get
		{
			return _copyData;
		}
	}

	private void OnClickCell(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level) < _copyData._CopyID)
			return;
		if (GamePlayer.Instance.GetCopyNum (_copyData._SceneID) >= 2)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("meiyoucishu"));
			return;
		}
		GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_EnterCopy, cellData._SceneID);
	}

	void OnCopyNumEnvet(int id)
	{
		int num = GamePlayer.Instance.GetCopyNum(_copyData._SceneID);
		if(num == 0)
		{
			num1Sp.gameObject.SetActive(true);
			num2Sp.gameObject.SetActive(true);
		}
		else if(num == 1)
		{
			num1Sp.gameObject.SetActive(true);
			num2Sp.gameObject.SetActive(false);
		}
		else
		{
			num1Sp.gameObject.SetActive(false);
			num2Sp.gameObject.SetActive(false);
		}
	}

	void OnDestroy()
	{
		GamePlayer.Instance.CopyNumEnvet -= OnCopyNumEnvet;
	}

}

