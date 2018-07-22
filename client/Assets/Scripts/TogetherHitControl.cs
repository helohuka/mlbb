using UnityEngine;
using System.Collections;

public class TogetherHitControl : MonoBehaviour {

    public UISprite gewei_;
    public UISprite shiwei_;
    public Transform effectPoint_;

    private string[] nums = { "tf_huang0", "tf_huang1", "tf_huang2", "tf_huang3", "tf_huang4", "tf_huang5", "tf_huang6", "tf_huang7", "tf_huang8", "tf_huang9" };

	// Use this for initialization
	void Start () {
        gewei_.gameObject.SetActive(false);
        shiwei_.gameObject.SetActive(false);
	}

    public void Open()
    {
        EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_Heji, effectPoint_);
    }

    public void SetNum(int num)
    {
        int ge = num % 10;
        int shi = num / 10 % 10;

        if (ge == 0)
        {
            if (shi == 0)
            {
                gewei_.gameObject.SetActive(false);
                shiwei_.gameObject.SetActive(false);
            }
            else
            {
                gewei_.gameObject.SetActive(true);
                shiwei_.gameObject.SetActive(true);
                gewei_.spriteName = nums[ge];
                shiwei_.spriteName = nums[shi];
            }
        }
        else
        {
            gewei_.gameObject.SetActive(true);
            gewei_.spriteName = nums[ge];
            if (shi == 0)
                shiwei_.gameObject.SetActive(false);
            else
            {
                shiwei_.gameObject.SetActive(true);
                shiwei_.spriteName = nums[shi];
            }
        }
    }

    public void Close()
    {
        EffectMgr.Instance.DeleteRef((EFFECT_ID)GlobalValue.EFFECT_Heji);
        for (int i = 0; i < effectPoint_.transform.childCount; ++i )
        {
            Destroy(effectPoint_.transform.GetChild(i).gameObject);
        }
        effectPoint_.DetachChildren();
        gewei_.gameObject.SetActive(false);
        shiwei_.gameObject.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
