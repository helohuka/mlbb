using UnityEngine;
using System.Collections;

public class HelpQAContentItem : MonoBehaviour {

    public UILabel q_;
    public UILabel a_;

    public UISprite qBg_;
    public UISprite aBg_;


	// Use this for initialization
	void Start () {
        
	}
	
    /*
     * return the total height
     */
    public int SetData(string q, string a)
    {
        q_.text = q;
        a_.text = a;

        //一行最大25个字 背景最小宽90 最小高68 一行高34 最大不限
        int qline = q.Length / 25 + (q.Length % 25 > 0 ? 1 : 0);
        int aline = a.Length / 25 + (a.Length % 25 > 0 ? 1 : 0);

        int qh = (qline - 1) * 34 + 68;
        int ah = (aline - 1) * 34 + 68;
        qBg_.height = qh;
        aBg_.height = ah;
        qBg_.transform.localPosition = new Vector3(qBg_.transform.localPosition.x, -40f + (qline - 1) * -10, qBg_.transform.localPosition.z);
        aBg_.transform.localPosition = new Vector3(aBg_.transform.localPosition.x, -40f + (aline - 1) * -10, aBg_.transform.localPosition.z);

        return (qh + ah) + 200 + qline * 20 + aline * 20;//200 预留空间
    }

	// Update is called once per frame
	void Update () {
	
	}
}
