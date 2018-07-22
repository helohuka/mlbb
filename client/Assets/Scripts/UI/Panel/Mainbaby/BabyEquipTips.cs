using UnityEngine;
using System.Collections;

public class BabyEquipTips : MonoBehaviour {

    public BabyEquipTipItem EquipInBag;
    public BabyEquipTipItem EquipOnBody;

	public void ShowTips(uint target, COM_Item body, COM_Item bag)
    {
        EquipOnBody.SetData(body);
        EquipInBag.SetData(target, bag, true);
        gameObject.SetActive(true);
        EquipOnBody.gameObject.SetActive(true);
        UIManager.Instance.AdjustUIDepth(transform, false, 0f, 26);
    }

    public void ShowTips(uint target, COM_Item equip, bool onBody)
    {
        EquipInBag.SetData(target, equip, !onBody);
        EquipOnBody.gameObject.SetActive(false);
        gameObject.SetActive(true);
        UIManager.Instance.AdjustUIDepth(transform, false, 0f, 26);
    }

    public void CloseTip()
    {
        gameObject.SetActive(false);
    }

    void OnClick()
    {
        CloseTip();
    }
}
