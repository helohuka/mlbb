using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoreActivityLevelUI : MonoBehaviour
{
	public GameObject cell;
	public GameObject cell1;
	public UIGrid grid;
	public UITexture back;
	private List<GameObject> CellList = new List<GameObject>();
	void Start ()
	{
		HeadIconLoader.Instance.LoadIcon("leijichongzhi1", back);
		List<MoreLevelData> levelList = MoreLevelData.moreLevelList;
		for(int i =0;i<levelList.Count;i++)
		{
			GameObject objCell;
			if(levelList[i].items_.Count <= 5)
			{
				 objCell = Object.Instantiate(cell.gameObject) as GameObject;
			}
			else
			{
				objCell = Object.Instantiate(cell1.gameObject) as GameObject;
			}
			MoreActivityLevelCellUI cellUI  =  objCell.gameObject.GetComponent<MoreActivityLevelCellUI>();
			cellUI.MoreLevel = levelList[i].level_;


			objCell.transform.parent = grid.transform;
			objCell.SetActive(true);
			objCell.transform.localScale = Vector3.one;
			CellList.Add(objCell);
		}
	}
	
}

