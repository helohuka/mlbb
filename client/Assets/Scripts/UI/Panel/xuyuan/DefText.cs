using UnityEngine;
using System.Collections;

public class DefText : MonoBehaviour {

	public UILabel lable;
	TemplteData _Tdata;
	public TemplteData Tdata
	{
		set
		{
			if(value != null)
			{
				_Tdata = value;
				lable.text = _Tdata._Text;
			}
		}
		get
		{
			return _Tdata;
		}
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
