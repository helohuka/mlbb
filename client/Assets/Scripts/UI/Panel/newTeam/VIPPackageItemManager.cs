using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VIPPackageItemManager : UIWrapContent 
{
	public delegate void ScrollviewEvent();
	public static event ScrollviewEvent ScrollviewEventOk;
	public static int tempCount = 60;//测试用

	public bool isRight = true;

	int itemMaxCount = 6;
	

	void Start()
	{
		InitGridItemGameObj(tempCount);
	}

	public void UpdateGridItem()
	{
		InitGridItemGameObj(tempCount);
	}
 	void InitGridItemGameObj(int InCount)
	{
		itemSize = 150;
		ClientLog.Instance.LogError("sum:" + InCount + "  childCount:" + transform.childCount);
		int itemCount = transform.childCount;
		for (int i = 0, imax = transform.childCount; i < imax; ++i)
		{
			Transform t = transform.GetChild(i);
			if (i < InCount)
			{
				t.gameObject.SetActive(true);
			}
			else
			{
				t.gameObject.SetActive(false);
			}
		}
		Init(InCount - 1);
	}
	static int _level = 1;
	public static int TempCount
	{
		set
		{
			tempCount = value;
		}
		get
		{
			return tempCount;
		}
	}
	public static int level
	{
		set
		{
			_level = value;
		}
		get
		{
			return _level;
		}
	}
	// 为每个Item赋值
	protected override void UpdateItem(Transform item, int index)
	{

		UILabel la = item.GetComponent<UILabel> ();

		if(isRise)
		{
			la.text = (index+_level).ToString ();
		}else
		{
			la.text = (tempCount-index).ToString();
		}
			
	
			



	}
}
