using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class familyZhufuCellUI : MonoBehaviour
{
	public UILabel nameLab;
	public UISprite selectImg;
	public UITexture icon;
	public int id;
	private List<string> _icons = new List<string>();
	private int blessingId_;
	public int skillId_;
	public UISprite num1;
	public UISprite num2;

	void Start ()
	{
		blessingId = id; 
		FamilySystem.instance.FamilyMyDataEvent += new RequestEventHandler<int> (OnSkillExpEnevt);
		//BlessingData bData = BlessingData.GatData (id);
		//if (bData == null)
		//	return;
	//	nameLab.text = bData._Name;
		//HeadIconLoader.Instance.LoadIcon (bData._Icon, icon);

	}

	void Update ()
	{

	}

	public int blessingId
	{
		set
		{
			blessingId_ = value;

			BlessingData bData = BlessingData.GatData (id);
			if (bData == null)
				return;
			SkillData sData = SkillData.GetData(bData._SkillId,1);
			if(sData == null)
				return;
			skillId_= bData._SkillId;
			nameLab.text = sData._Name;
			HeadIconLoader.Instance.LoadIcon (bData._Icon, icon);
			if(!_icons.Contains(bData._Icon))
			{
				_icons.Add(bData._Icon);
			}

			COM_Skill skillInst = FamilySystem.instance.GetZhuFuSkill(bData._SkillId);
			if(skillInst == null)
			{
				num1.spriteName = "1";
				num2.gameObject.SetActive(false);
				return;
			}

			int lv1 = (int)skillInst.skillLevel_/10;
			int lv2 = (int)skillInst.skillLevel_%10;
			if(lv1 >0)
			{
				num1.spriteName = lv1.ToString();
				num2.gameObject.SetActive(true);
				num2.spriteName = lv2.ToString();
			}
			else
			{
				num1.spriteName = lv2.ToString();
				num2.gameObject.SetActive(false);
			}

		}
		get
		{
			return blessingId_;
		}
	}

	private void OnSkillExpEnevt(int id)
	{
		blessingId = id; 
	}

	void OnDestroy()
	{
		FamilySystem.instance.FamilyMyDataEvent -= OnSkillExpEnevt;
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}


}

 