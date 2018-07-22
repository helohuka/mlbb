using UnityEngine;
using System.Collections;

public class HelpWayCellUI : MonoBehaviour
{
	public UILabel nameLab;
	public UILabel descLab;
	public UISprite nameBack;

	private CourseData _courseData; 


	void Start ()
	{

	}



	public CourseData Course
	{
		set
		{
			if(value != null)
			{
				_courseData = value;
				nameLab.text= _courseData.name_;
				descLab.text= _courseData.desc_;
			}
		}
		get
		{
			return _courseData;
		}
	}
}

