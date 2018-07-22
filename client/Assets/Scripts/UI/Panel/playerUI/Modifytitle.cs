using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Modifytitle : MonoBehaviour {

	public UIButton closeBtn;
	public UIButton CancelBtn;
	public UILabel titleLabel;
	public UIGrid grid;
	public GameObject item;

	void Start () {
		item.SetActive (false);

		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickclose, 0, 0);
		UIManager.SetButtonEventHandler (CancelBtn.gameObject, EnumButtonEvent.OnClick, OnClickCancel, 0, 0);

//		for(int i =0;i<GamePlayer.Instance.titles.Count;i++)
//		{
//			TitleData pdata = TitleData.GetTitleByValue(GamePlayer.Instance.titles[i]);
//			titledata.Add(pdata);
//		}
		AddItem (GamePlayer.Instance.titles);

        if (GamePlayer.Instance.titleHide_)
        {
            titleLabel.text = "";
        }
        else
        {
            TitleData tData = TitleData.GetTitleData(GamePlayer.Instance.GetIprop(PropertyType.PT_Title));
            if (tData == null)
                titleLabel.text = "";
            else
                titleLabel.text = tData.desc_;
        }
            
	}
	void OnEnable()
	{
		GameManager.Instance.UpdatePlayermake += UpdatePlayerTitle;
	}
	void OnDisable()
	{
		GameManager.Instance.UpdatePlayermake -= UpdatePlayerTitle;
	}
	void UpdatePlayerTitle()
	{
        if (GamePlayer.Instance.titleHide_)
        {
            titleLabel.text = "";
            return;
        }

        TitleData tData = TitleData.GetTitleData(GamePlayer.Instance.GetIprop(PropertyType.PT_Title));
        if (tData == null)
            titleLabel.text = "";
        else
            titleLabel.text = tData.desc_;
	}
	void OnClickclose(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
		
	}
	void OnClickCancel(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.setCurrentTitle (0);
        GamePlayer.Instance.SetTitleDisable(true);
        titleLabel.text = "";
	}
	void AddItem(List<int> ptdata)
	{
		for(int i =0;i<ptdata.Count;i++)
		{
			GameObject clone = GameObject.Instantiate(item)as GameObject;
			clone.SetActive(true);
			clone.transform.parent = grid.transform;
			clone.transform.position = Vector3.zero;
			clone.transform.localScale = Vector3.one;
			TitleCell tcell = clone.GetComponent<TitleCell>();
			tcell.titleLabel.text = TitleData.GetTitleData(ptdata[i]).desc_;
			UIManager.SetButtonEventHandler (clone, EnumButtonEvent.OnClick, OnClickbtn, ptdata[i], 0);
			grid.repositionNow = true;
		}
	}
	void OnClickbtn(ButtonScript obj, object args, int param1, int param2)
	{
        GamePlayer.Instance.SetTitleDisable(false);
		NetConnection.Instance.setCurrentTitle (param1);
	}

}
