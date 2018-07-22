using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EmailUI : UIBase
{

	public UIButton closeBtn;
	public UIButton getRewordBtn;
	public UIButton delMailBtn;
	public GameObject mailCell;
	public UIGrid mailGrid;
	public UIGrid itemGrid;
	public UILabel tetleLab;
	public UILabel infoLab;
	public UILabel noMailLab;
	public UISprite rightImg;
	public UILabel emialTitelLab;
	public UILabel emialNothinglLab;

	private COM_Mail  _selectMail;
	private List<GameObject> mailCellList =new List<GameObject>();
	private List<GameObject> mailCellPoolList =new List<GameObject>();
	private List<GameObject> itemCellList =new List<GameObject>();
	private List<GameObject> itemCellPoolList =new List<GameObject>();
	
	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		UIManager.SetButtonEventHandler (getRewordBtn.gameObject, EnumButtonEvent.OnClick, OnGetRewrad, 0, 0);
		UIManager.SetButtonEventHandler (delMailBtn.gameObject, EnumButtonEvent.OnClick, DelMail, 0, 0);

		EmailSystem.instance.mailEnven += new RequestEventHandler<bool> (OnMailEvent);
		EmailSystem.instance.UpdatemailEnven += new RequestEventHandler<COM_Mail> (OnUpdateMailEvent);
		emialTitelLab.text = LanguageManager.instance.GetValue("emialTitelLab");
		emialNothinglLab.text = LanguageManager.instance.GetValue("emialNothinglLab");


		if(EmailSystem.instance.Mails != null &&EmailSystem.instance.Mails.Length > 0)
		{
			noMailLab.gameObject.SetActive (false);
		}
		else
		{
			noMailLab.gameObject.SetActive (true);
		}

		OpenPanelAnimator.PlayOpenAnimation (this.panel, () => {
				UpdateList ();
				});
	}
	


	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_EmailPanel);
	}
	
	public static void ShowMe()

	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_EmailPanel);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_EmailPanel);
	}
	
	#endregion


	private void UpdateList()
	{
		COM_Mail[] mails = EmailSystem.instance.Mails;
		if (mails == null)
			return;

		for(int k =0; k<mailCellList.Count;k++)
		{
			mailCellList[k].transform.parent = null;
			mailCellList[k].gameObject.SetActive(false);
			mailCellPoolList.Add(mailCellList[k]); 
		}
		mailCellList.Clear ();

		int len = mails.Length-1;
		for(int i =len ;i>=0;i--)
		{
			if(mails[i] == null)
				continue;
			GameObject obj;
			if(mailCellPoolList.Count > 0)
			{
				obj= mailCellPoolList[0];
				mailCellPoolList.Remove(mailCellPoolList[0]);
			}
			else
			{
				obj= Object.Instantiate(mailCell.gameObject) as GameObject;
			}
			obj.GetComponent<mailCellUI>().Mail = mails[i];
			UIManager.SetButtonEventHandler(obj,EnumButtonEvent.OnClick,OnClicMail,0,0);
			obj.transform.parent =  mailGrid.transform;
			obj.SetActive(true);
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.zero;

			mailCellList.Add(obj);

		}
		mailGrid.Reposition ();
	}


	public override void Destroyobj ()
	{
		GameObject.Destroy (gameObject);
	}

	private void OnClose(ButtonScript obj, object args, int param1, int param2)
	{
		if(itemCellPoolList.Count  > 0)
		{
			for(int i = 0 ;i< itemCellPoolList.Count;i++)
			{
				Destroy(itemCellPoolList[i]);
				itemCellPoolList[i] = null;
			}
			itemCellPoolList.Clear();
		}

		OpenPanelAnimator.CloseOpenAnimation(this.panel, ()=>{
			Hide ();
		});
	}

	private void DelMail(ButtonScript obj, object args, int param1, int param2)
	{
		if(_selectMail.money_ > 0 || _selectMail.diamond_ > 0 || _selectMail.items_.Length > 0)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("delmailreword"));
			return;
		}
		NetConnection.Instance.delMail (_selectMail.mailId_);
		EmailSystem.instance.DelMial (_selectMail.mailId_);
		UpdateList ();
		ResetMailInfo ();
	}

	private void OnGetRewrad(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.getMailItem (_selectMail.mailId_);
		//if(NetWaitUI.instance!= null)
		//{
		//	NetWaitUI.instance.Show();
		//}
	}


	private void OnClicMail(ButtonScript obj, object args, int param1, int param2)
	{
		COM_Mail mail = obj.GetComponent<mailCellUI> ().Mail;
		if (mail == null)
			return;

		if(!delMailBtn.gameObject.activeSelf)
		{
			delMailBtn.gameObject.SetActive (true);
		}
		if(!mail.isRead_)
		{
			obj.GetComponent<mailCellUI>().icon.spriteName = "dakaixin";
		}

		_selectMail = mail;
		mail.isRead_ = true;
		NetConnection.Instance.readMail (_selectMail.mailId_);
		rightImg.gameObject.SetActive (true);

		for(int k =0; k<mailCellList.Count;k++)
		{
			mailCellList[k].GetComponent<mailCellUI> ().back.gameObject.SetActive (false);
		}
		obj.GetComponent<mailCellUI> ().back.gameObject.SetActive (true);


		updateMailInfo (mail);

	}


	void OnMailEvent(bool show)
	{
		UpdateList ();
	//	rightImg.gameObject.SetActive (false);
	}

	void OnUpdateMailEvent(COM_Mail mail)
	{
		NetWaitUI.HideMe();
		if(mail.mailId_ == _selectMail.mailId_)
		{
			_selectMail =mail;
			UpdateList();
			updateMailInfo(mail);
		}
	}


	void updateMailInfo(COM_Mail mail)
	{
		tetleLab.text = mail.title_;
		infoLab.text = mail.content_;
		
		for(int k =0; k<itemCellList.Count;k++)
		{
			itemCellList[k].transform.parent = null;
			itemCellList[k].gameObject.SetActive(false);
			itemCellPoolList.Add(itemCellList[k]); 
		}
		itemCellList.Clear ();
		
		getRewordBtn.gameObject.SetActive(false);
		
		if(mail.money_ > 0)
		{
			GameObject mobj;
			if(itemCellPoolList.Count > 0)
			{
				mobj= itemCellPoolList[0];
				itemCellPoolList.Remove(itemCellPoolList[0]);
			}
			else
			{
				mobj = UIManager.Instance.InstantiateBagCellUIObj().gameObject;
			}
			mobj.GetComponent<ItemCellUI>().itemId = 5035;
			mobj.GetComponent<ItemCellUI>().ItemCount = mail.money_;
			mobj.AddComponent<UIDragScrollView>();
			mobj.transform.parent = itemGrid.transform;
			mobj.transform.localPosition = Vector3.zero;
			mobj.transform.localScale = Vector3.one;
			mobj.SetActive(true);
			itemCellList.Add(mobj);
			getRewordBtn.gameObject.SetActive(true);
		}
		
		if(mail.diamond_ > 0)
		{
			GameObject mobj;
			if(itemCellPoolList.Count > 0)
			{
				mobj= itemCellPoolList[0];
				itemCellPoolList.Remove(itemCellPoolList[0]);
			}
			else
			{
				mobj = UIManager.Instance.InstantiateBagCellUIObj().gameObject;
			}
			mobj.GetComponent<ItemCellUI>().itemId = 5034;
			mobj.GetComponent<ItemCellUI>().ItemCount = mail.diamond_;
			mobj.AddComponent<UIDragScrollView>();
			mobj.transform.parent = itemGrid.transform;
			mobj.transform.localPosition = Vector3.zero;
			mobj.transform.localScale = Vector3.one;
			mobj.SetActive(true);
			itemCellList.Add(mobj);
			getRewordBtn.gameObject.SetActive(true);
		}
		
		for(int i = 0;i<mail.items_.Length;i++)
		{
			GameObject obj1;
			
			if(itemCellPoolList.Count > 0)
			{
				obj1= itemCellPoolList[0];
				itemCellPoolList.Remove(itemCellPoolList[0]);
			}
			else
			{
				obj1 = UIManager.Instance.InstantiateBagCellUIObj().gameObject;
			}
			obj1.GetComponent<ItemCellUI>().itemId = (uint)mail.items_[i].itemId_;
			obj1.GetComponent<ItemCellUI>().showTips = true;
			obj1.GetComponent<ItemCellUI>().ItemCount = mail.items_[i].itemStack_;
			obj1.AddComponent<UIDragScrollView>();
			obj1.transform.parent = itemGrid.transform;
			obj1.transform.localPosition = Vector3.zero;
			obj1.transform.localScale = Vector3.one;
			obj1.SetActive(true);
			itemCellList.Add(obj1);
		}

		if(mail.items_.Length > 0)
		{
			getRewordBtn.gameObject.SetActive(true);
		}

		itemGrid.Reposition ();
	}


	protected override void DoHide ()
	{
		EmailSystem.instance.mailEnven -= OnMailEvent;
		EmailSystem.instance.UpdatemailEnven -= OnUpdateMailEvent;



		base.DoHide ();
	}


	private void ResetMailInfo()
	{
		getRewordBtn.gameObject.SetActive(false);
		delMailBtn.gameObject.SetActive(false);
		tetleLab.gameObject.SetActive(false);
		infoLab.gameObject.SetActive(false);

		for(int k =0; k<itemCellList.Count;k++)
		{
			itemCellList[k].transform.parent = null;
			itemCellList[k].gameObject.SetActive(false);
			itemCellPoolList.Add(itemCellList[k]); 
		}
		itemCellList.Clear ();
	}


}

