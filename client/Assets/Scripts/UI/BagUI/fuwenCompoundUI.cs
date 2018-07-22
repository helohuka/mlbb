using UnityEngine;
using System.Collections;

public class fuwenCompoundUI : MonoBehaviour
{
	public UIButton hcBtn;
	public UISprite icon;
	public UISprite icon1;
	public UISprite icon2;
	public UISprite icon3;
	public UIButton closeBtn;
	public UISprite needItem;
	public UISprite black;
	public UILabel needNumLab;
	COM_Item  item_;


	void Start () 
	{
		UIManager.SetButtonEventHandler (hcBtn.gameObject, EnumButtonEvent.OnClick, OnHcBtn, 0, 0);
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnCloseBtn, 0, 0);

        GuideManager.Instance.RegistGuideAim(hcBtn.gameObject, GuideAimType.GAT_MainFuwenUICombieBtn);
        GuideManager.Instance.RegistGuideAim(closeBtn.gameObject, GuideAimType.GAT_MainFuwenCloseBtn);
		GamePlayer.Instance.fuwenCompoundOkEnvet += new RequestEventHandler<int> (CompoundOkEnvet);

		GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BagFuwenCombieUI, hcBtn.isEnabled == false? 0: 1);
	}

	void Update () 
	{

	}

	private void OnCloseBtn(ButtonScript obj, object args, int param1, int param2)
	{
		
		gameObject.SetActive (false);
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_FuwenUIClose);
	}

	private void OnHcBtn(ButtonScript obj, object args, int param1, int param2)
	{   
		NetConnection.Instance.compFuwen((int)item_.instId_);
	}

	public COM_Item Item
	{
		set
		{
			if(value == null)
			{
				if(item_!= null)
				{
					ItemCellUI cell01 = UIManager.Instance.AddItemCellUI(icon1,(uint)item_.itemId_);
					cell01.ItemIcon.color = new Color(0f,1f,1f);
					ItemCellUI cell02 = UIManager.Instance.AddItemCellUI(icon2,(uint)item_.itemId_);
					cell02.ItemIcon.color = new Color(0f,1f,1f);
					ItemCellUI cell03 = UIManager.Instance.AddItemCellUI(icon3,(uint)item_.itemId_);
					cell03.ItemIcon.color = new Color(0f,1f,1f);
					hcBtn.isEnabled = false;
					needItem.gameObject.SetActive(false);
				}
				return;
			}
			item_ = value;
			RunesData rData = RunesData.getData((int)value.itemId_);
			if(rData== null)
				return;
			hcBtn.isEnabled = true;
			int haveNum = BagSystem.instance.GetItemMaxNum(value.itemId_);
			ItemCellUI cell = UIManager.Instance.AddItemCellUI(icon,(uint)rData.resultId);
			cell.cellPane.spriteName ="";
			cell.showTips = true;
			if(haveNum < 3)
			{
				cell.ItemIcon.color = new Color(0f,1f,1f);
			}
			else
			{
				cell.ItemIcon.color = Color.white;
			}

			ItemCellUI cell1 = UIManager.Instance.AddItemCellUI(icon1,(uint)value.itemId_);
			cell1.cellPane.spriteName ="";
			cell1.showTips = true;
			if(haveNum < 1)
			{
				cell1.ItemIcon.color = new Color(0f,1f,1f);
				hcBtn.isEnabled = false;
			}
			else
			{
				cell1.ItemIcon.color = Color.white;
			}

			ItemCellUI cell2 = UIManager.Instance.AddItemCellUI(icon2,(uint)value.itemId_);
			cell2.cellPane.spriteName ="";
			cell2.showTips = true;
			if(haveNum < 2)
			{
				cell2.ItemIcon.color = new Color(0f,1f,1f);
				hcBtn.isEnabled = false;
			}
			else
			{
				cell2.ItemIcon.color = Color.white;
			}

			ItemCellUI cell3 = UIManager.Instance.AddItemCellUI(icon3,(uint)value.itemId_);
			cell3.cellPane.spriteName ="";
			cell3.showTips = true;
			if(haveNum < 3)
			{
				cell3.ItemIcon.color = new Color(0f,1f,1f);
				hcBtn.isEnabled = false;
			}
			else
			{
				cell3.ItemIcon.color = Color.white;
			}
			if(rData.needItemId != 0)
			{
				needItem.gameObject.SetActive(true);
				ItemCellUI nCell = UIManager.Instance.AddItemCellUI(needItem,(uint)rData.needItemId);
				nCell.cellPane.spriteName ="";
				nCell.showTips = true;
				int num  = BagSystem.instance.GetItemMaxNum((uint)rData.needItemId);
				//nCell.ItemConutLab.gameObject.SetActive(true);
				//nCell.ItemConutLab.text = num +"/"+rData.needItemNum;
				needNumLab.text = num +"/"+rData.needItemNum;
				if(num < rData.needItemNum)
				{
					nCell.ItemIcon.color = new Color(0f,1f,1f);
					needNumLab.color = Color.red;
					hcBtn.isEnabled = false;
				}
				else
				{
					needNumLab.color = Color.green;
					nCell.ItemIcon.color = Color.white;
				}

			}
			else
			{
				needItem.gameObject.SetActive(false);
			}
		}
	}

	void CompoundOkEnvet(int id)
	{
		Item = BagSystem.instance.GetItemByInstId ((int)item_.instId_);
		black.gameObject.SetActive (true);
		EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_fuwenhecheng, gameObject.transform, () =>
        {
            black.gameObject.SetActive (false);
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BagFuwenCombieSuccess);
        });
	}


	void OnDestroy()
	{
		GamePlayer.Instance.fuwenCompoundOkEnvet -= CompoundOkEnvet;
	}
}

