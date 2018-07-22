using UnityEngine;
using System.Collections;

public class NpcHeadChatUI : MonoBehaviour
{
	public UILabel textLab;

    string combieParam_;

    public delegate void DestroyHandler(string sValue);
    public DestroyHandler CallBack_;

	void Start ()
	{

	}
	

	public string Content
	{
		set
		{
			textLab.text = value;
		}
		get
		{
			return  textLab.text;
		}
	}

    public void SetParam(DestroyHandler callback, string combieParam)
    {
        CallBack_ = callback;
        combieParam_ = combieParam;
    }

    void OnDestroy()
    {
        if (CallBack_ != null)
            CallBack_(combieParam_);
    }

}

