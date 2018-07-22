using UnityEngine;
using System.Collections;

public class SuccessTishi : MonoBehaviour {

	public GameObject item;
	public UIGrid grid;

	void Start () {
        item.SetActive(false);
	}

    void Update()
    {
        if (SuccessSystem.isDirty)
        {
            AchievementContent ac = null;
            if(SuccessSystem.newAchieve.Count > 0)
            {
                ac = SuccessSystem.newAchieve.Dequeue();
                GameObject clone = (GameObject)GameObject.Instantiate(item);
                clone.SetActive(true);
                clone.transform.parent = grid.transform;
                clone.transform.localScale = Vector3.one;
                Destroy des = clone.AddComponent<Destroy>();
                des.SetLifeTime(10.0f);
                SuccessTipsCell tipsCell = clone.GetComponent<SuccessTipsCell>();
                tipsCell.content = ac;
                UIManager.SetButtonEventHandler(clone.gameObject, EnumButtonEvent.OnClick, OnClickAchievement, 0, 0);
            }
            grid.repositionNow = true;
        }
    }

	void OnClickAchievement(ButtonScript obj, object args, int param1, int param2)
	{
		Destroy (obj.gameObject);
		SuccessPanelUI.ShowMe ();
	}
}
