using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FamilySystem 
{
	public RequestEventHandler<int> FamilyDataEvent;
	public RequestEventHandler<int> FamilyMyDataEvent;
	public RequestEventHandler<int[]> ProgenPosEvent;
	public RequestEventHandler<int> UpdateGuildFundzEvent;
	public RequestEventHandler<COM_GuildProgen> UpdateProgenitusEvent;
	public RequestEventHandler<COM_GuildBuilding> UpdateGuildBuildingEvent;
	private COM_GuildBuilding[] _buildings;
	private COM_Guild _guild;
	private COM_GuildMember _member;
	private int[] _progenPos;
	private List<COM_Skill> skills = new List<COM_Skill>();

	private static FamilySystem _instance;
	public static FamilySystem instance
	{
		get
		{
			if(_instance == null)
				_instance = new FamilySystem();
			return _instance;
		}
	}
	

	public COM_GuildBuilding[] Buildings
	{
		set
		{
			_buildings = value;
		}
		get
		{
			return _buildings;
		}
	}

	public COM_Guild GuildData
	{
		set
		{
			_guild = value;
			if(_guild != null)
			{
				Buildings = _guild.buildings_;
			}
			if(FamilyDataEvent != null)
				FamilyDataEvent(1);
		}              
		get
		{
			return _guild;
		}
	}

	public List<COM_Skill> ZhuFuSkills
	{
		set
		{
			skills = value;
		}
		get
		{
			return skills;
		}
	}


	public COM_GuildMember GuildMember
	{
		get
		{
			return _member;
		}
		set
		{
			if(value.roleId_ == GamePlayer.Instance.InstId)
			{
				_member = value;
				if(FamilyMyDataEvent != null)
					FamilyMyDataEvent(1);
			}
		}
	}

	public COM_Skill GetZhuFuSkill(int id)
	{
		if (GuildMember == null)
			return null;

		if (ZhuFuSkills == null || ZhuFuSkills.Count == 0)
			return null;
		for(int i =0;i<skills.Count;i++)
		{
			if(skills[i].skillID_ == id)
			{
				return skills[i];
			}
		}
		return null;
	}

	public void UpdateZhufuSkill(COM_Skill skill)
	{
		if (GuildMember == null)
			return ;
		if (ZhuFuSkills == null || ZhuFuSkills.Count == 0)
		{
			ZhuFuSkills.Add(skill);
			if(FamilyMyDataEvent != null)
				FamilyMyDataEvent(1);
			return;
		}
		for(int i =0;i<ZhuFuSkills.Count;i++)
		{
			if(ZhuFuSkills[i].skillID_ == skill.skillID_)
			{
				ZhuFuSkills[i] = skill;
				if(FamilyMyDataEvent != null)
					FamilyMyDataEvent(1);
				return;
			}
		}

		ZhuFuSkills.Add(skill);
		if(FamilyMyDataEvent != null)
			FamilyMyDataEvent(1);
	}
	//
	public void progenitusAddExpOk(COM_GuildProgen guild)
	{
		for(int i=0;i<GuildData.progenitus_.Length;i++)
		{
			if(GuildData.progenitus_[i].mId_ == guild.mId_)
			{
				GuildData.progenitus_[i] = guild;
				break;
			}
		}
		if (UpdateProgenitusEvent != null)
			UpdateProgenitusEvent (guild);
	}

	public void updateGuildFundz(int num)
	{
		GuildData.fundz_ = (uint)num;
		GuildSystem.Mguild.fundz_ = (uint)num;
		if (UpdateGuildFundzEvent != null)
			UpdateGuildFundzEvent (num);
	}

	public int[] ProgenPos
	{
		set
		{
			_progenPos = value;
			GuildData.progenitusPositions_ = value;
			if(ProgenPosEvent != null)
				ProgenPosEvent(_progenPos);
		}
		get
		{
			return _progenPos;
		}
	}

	public void  updateGuildBuilding(GuildBuildingType type,COM_GuildBuilding building)
	{
		Buildings [(int)type-1] = building;
		if(UpdateGuildBuildingEvent != null)
			UpdateGuildBuildingEvent(building);
	}

	public void presentGuildItemOk(int num)
	{
		GuildData.presentNum_ = num;
		GuildData = GuildData;
	}
}

