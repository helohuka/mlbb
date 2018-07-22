using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaRecordListUI : MonoBehaviour
{
	public UIButton closeBtn;
	public UIGrid grid;
	public GameObject itemCell;
	private List<GameObject> CellList = new List<GameObject>();
	private List<GameObject> CellListPool = new List<GameObject>();

	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
	}
	

	public void UpdateList()
	{
		COM_JJCBattleMsg[] rank = ArenaSystem.Instance.BattleMsgs;
		 
		for(int i =0;i<CellList.Count;i++)
		{
			CellList[i].transform.parent = null;
			CellListPool.Add(CellList[i]);
			
		}
		CellList.Clear ();


		for(int i=0; i < rank.Length; ++i)
		{
			GameObject obj;
			if(CellListPool.Count > 0)
			{
				obj = CellListPool[0];
				CellListPool.Remove(obj);
			}
			else
			{
				obj = Object.Instantiate(itemCell.gameObject) as GameObject;
			}

            obj.GetComponent<ArenaRecordCellUI>().BattleMsg = rank[i];

			obj.transform.parent = grid.transform;
			obj.SetActive(true);
			obj.transform.localScale = Vector3.one;
			CellList.Add(obj);
		}
	}

	private void OnClose(ButtonScript obj, object args, int param1, int param2)
	{
		this.gameObject.SetActive(false);
	}

}

