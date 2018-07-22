using UnityEngine;
using System.Collections;

public class FamilyMonsterCellUI : MonoBehaviour
{
	public UILabel nameLab;
	public UILabel levelLab;
	public UILabel descLab;
	public UIButton checkBtn;
	public UIButton levelUpBtn;
	public Transform mpos;
	private GameObject babyObj;
	private COM_GuildProgen _monster; 
	void Start ()
	{
		UIManager.SetButtonEventHandler (checkBtn.gameObject, EnumButtonEvent.OnClick, OnClickCheckBtn, 0, 0);
		UIManager.SetButtonEventHandler (levelUpBtn.gameObject, EnumButtonEvent.OnClick, OnClickLevelUp, 0, 0);
		FamilySystem.instance.UpdateProgenitusEvent += new RequestEventHandler<COM_GuildProgen> (OnProgenitusEvent);
	}


	private void OnClickCheckBtn(ButtonScript obj, object args, int param1, int param2)
	{
		familyCheckMonsterUI.ShowMe (_monster);
	}

	private void OnClickLevelUp(ButtonScript obj, object args, int param1, int param2)
	{
		FamilyMonsterLevelUpUI.ShowMe (_monster);
	}


	public COM_GuildProgen Monster
	{
		set
		{
			_monster = value;
			if(_monster != null)
			{
				familyMonsterData bData = familyMonsterData.GetData(_monster.mId_,_monster.lev_);
				if(bData == null)
					return;
				nameLab.text = bData._Name;
				levelLab.text = _monster.lev_.ToString();
				descLab.text= bData._Desc;
				GameManager.Instance.GetActorClone((ENTITY_ID)bData._AssetsID, (ENTITY_ID)0, EntityType.ET_Baby, AssetLoadCallBack, null, "UI");
			}
		}
		get
		{
			return _monster;
		}
	}


	void AssetLoadCallBack(GameObject ro, ParamData data)
	{
		if (gameObject == null || !this.gameObject.activeSelf)
		{
			Destroy(ro);
			PlayerAsseMgr.DeleteAsset((ENTITY_ID)data.iParam, false);
			return;
		}
		if(babyObj != null)
		{
			Destroy(ro);
			PlayerAsseMgr.DeleteAsset((ENTITY_ID)data.iParam, false);
			return;
		}
		ro.transform.parent = mpos;
		ro.transform.localScale = new Vector3(250f,250f,250f);
		ro.transform.localPosition = Vector3.forward * -40;
		ro.transform.localRotation = Quaternion.Euler (10f, 180f, 0f);
		babyObj = ro;
	}
	private void OnProgenitusEvent(COM_GuildProgen progen )
	{
		if (progen.mId_ == Monster.mId_)
		{
			_monster = progen; 
			familyMonsterData bData = familyMonsterData.GetData(_monster.mId_,_monster.lev_);
			if(bData == null)
				return;
			nameLab.text = bData._Name;
			levelLab.text = _monster.lev_.ToString();
			descLab.text= bData._Desc;
		//	Monster = progen;
		}
	}
	void OnDestroy()
	{
		FamilySystem.instance.UpdateProgenitusEvent -= OnProgenitusEvent;
	}

}

