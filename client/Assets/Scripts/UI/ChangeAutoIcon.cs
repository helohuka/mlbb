using UnityEngine;
using System.Collections;

public class ChangeAutoIcon : MonoBehaviour {

    public UISprite iconBg_;

    public UILabel name_;

    public GameObject best_;

	// Use this for initialization
	void Start () {
	
	}

    public void SetData(int skillId, int level, bool isProud)
    {
        SkillData skdata = SkillData.GetData(skillId, level);
        GameObject go = new GameObject();
        UITexture tex = go.AddComponent<UITexture>();
        tex.depth = iconBg_.depth;
        go.transform.parent = iconBg_.transform;
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = Vector3.zero;
        HeadIconLoader.Instance.LoadIcon(skdata._ResIconName, tex);
        name_.text = string.Format("[b]{0}[-]", skdata._Name);
        best_.SetActive(isProud);
    }

}
