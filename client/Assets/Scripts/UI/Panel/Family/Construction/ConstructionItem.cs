using UnityEngine;
using System.Collections;

public class ConstructionItem : MonoBehaviour {

    public UILabel static_level_;
    public UILabel static_levelup_;

    public UILabel namelbl_;
    public UILabel lvlbl_;
    public UISprite iconsp_;
    public UIButton lvbtn_;
    public UILabel btndesc_;
	public UISprite levelImg;
	public UISprite funImg;

    FamilyData data_;
	private COM_GuildBuilding BuildingData;

	// Use this for initialization
	void Start () {
        //static_level_.text = LanguageManager.instance.GetValue("");
        //static_levelup_.text = LanguageManager.instance.GetValue("");

       // namelbl_.text = data_.name_;
       // lvlbl_.text = data_.level_.ToString();
	}
	
    public void SetData(FamilyData data)
    {
		if (data == null)
			return;
        data_ = data;
		UpdataInfo ();
    }

	public void UpdataInfo()
	{
		if (data_ == null)
			return;
		namelbl_.text = data_.name_;
		funImg.spriteName = data_.icon_;
		if (FamilySystem.instance.Buildings.Length < (int)data_.type_)
			return;
		COM_GuildBuilding  build = FamilySystem.instance.Buildings[(int)data_.id_-1];
		if (build == null)
			return;
		BuildingData = build;
		levelImg.spriteName = "jz_"+build.level_;
	}

	void Update () 
	{
	
	}

	public COM_GuildBuilding Building
	{
		get
		{
			return BuildingData;
		}
	}

}
