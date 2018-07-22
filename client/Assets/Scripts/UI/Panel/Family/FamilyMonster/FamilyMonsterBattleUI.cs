using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FamilyMonsterBattleUI : UIBase
{
	public UIButton closeBtn;
	public List<FamilyMonsterCellUI> cellList = new List<FamilyMonsterCellUI>();
	public List<UISprite> iconBack = new List<UISprite>();
	public List<UITexture> icon = new List<UITexture>();
	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		FamilySystem.instance.ProgenPosEvent += new RequestEventHandler<int[]> (OnProgenPosEvent);
		COM_Guild guild = FamilySystem.instance.GuildData;
		if (guild == null)
			return;
		for(int i=0;i<guild.progenitus_.Length;i++)
		{
			cellList[i].Monster = guild.progenitus_[i];
		}

		for(int i=0;i<cellList.Count;i++)
		{
			UIManager.SetButtonEventHandler (cellList[i].gameObject, EnumButtonEvent.OnClick, OnClickCell, cellList[i].Monster.mId_, 0);
		}

		for(int i=0;i<iconBack.Count;i++)
		{
			UIManager.SetButtonEventHandler (iconBack[i].gameObject, EnumButtonEvent.OnClick, OnClickPosIcon, i, 0);
		}

		for(int j= 0;j<guild.progenitusPositions_.Length;j++)
		{
			if(guild.progenitusPositions_[j]!= 0)
			{
				icon[j].gameObject.SetActive(true);
				HeadIconLoader.Instance.LoadIcon(familyMonsterData.GetData(guild.progenitusPositions_[j],1)._Icon,icon[j]);
			}
			else
			{
				icon[j].gameObject.SetActive(false);
			}
		}
	}

	void Update ()
	{

	}

	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_FamilyMonsterBattle);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_FamilyMonsterBattle );
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_FamilyMonsterBattle );
	}
	
	public override void Destroyobj ()
	{
		GameObject.Destroy (gameObject);
	}
	
	#endregion
	
	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}

	private void OnClickCell(ButtonScript obj, object args, int param1, int param2)
	{
		return;
		int pos = GetNullPos ();
		if (pos == -1)
			return;
		NetConnection.Instance.setProgenitusPosition (param1, pos);
	}
	
	private void OnClickPosIcon(ButtonScript obj, object args, int param1, int param2)
	{
		return;
		COM_Guild guild = FamilySystem.instance.GuildData;
		if (guild == null)
			return;
		if (guild.progenitusPositions_ [param1] == 0) 
		{
			return;
		}
		else
		{
			NetConnection.Instance.setProgenitusPosition (0, param1);
		}
	}

	private int GetNullPos()
	{
		COM_Guild guild = FamilySystem.instance.GuildData;
		if (guild == null)
			return -1;
		for(int i =0;i< guild.progenitusPositions_.Length;i++)
		{
			if(guild.progenitusPositions_[i] == 0)
			{
				return i;
			}
		}

		return -1;

	}

	private void OnProgenPosEvent(int[] pos)
	{
		for(int j= 0;j<pos.Length;j++)
		{
			if(pos[j]!= 0)
			{
				icon[j].gameObject.SetActive(true);
				HeadIconLoader.Instance.LoadIcon(familyMonsterData.GetData(pos[j],1)._Icon,icon[j]);
			}
			else
			{
				icon[j].gameObject.SetActive(false);
			}
		}
	}
	void OnDestroy()
	{
		FamilySystem.instance.ProgenPosEvent -= OnProgenPosEvent;
	}
}

