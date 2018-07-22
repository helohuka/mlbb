using UnityEngine;
using System.Collections;

public class AttaclEvent
{
	/// <summary>
	/// The 显示技能 event.
	/// </summary>
	public delegate void ASkillBtnClick();
	public AttackBtnClick SkillEvent;

	public delegate void ARefreshSkillLevel();
	public AttackBtnClick RefreshSkillEvent;

    /// <summary>
    /// The捕获event.
	/// </summary>
    public delegate void CatchBtnClick(BattleActor casrer);
    public CatchBtnClick CatchEvent;
	/// <summary>
	/// The攻击event.
	/// </summary>
    public delegate void AttackBtnClick(Entity casrer);
	public AttackBtnClick attackEvent;
	/// <summary>
	/// The 自动攻击 event .
	/// </summary>
	public delegate void AUTOBtnClick();
	public AttackBtnClick AUTOEvent;

	/// <summary>
	/// The 位置变换 event .
	/// </summary>
	public delegate void PositionBtnClick();
	public AttackBtnClick PositionEvent;

	/// <summary>
	/// The 宠物 event.
	/// </summary>
	public delegate void PetBtnClick();
	public AttackBtnClick PetEvent;

	/// <summary>
	/// The 防御 event.
	/// </summary>
	public delegate void DefenseClick();
	public AttackBtnClick DefenseEvent;

	/// <summary>SkillEvent
	/// The 逃走 event.
	/// </summary>
	public delegate void FleeClick();
	public AttackBtnClick FleeEvent;

	/// <summary>
	/// The 物品 event.
	/// </summary>
	public delegate void ArticleClick();
	public AttackBtnClick ArticleEvent;
	/// <summary>
	/// The 技能选级别 event.
	/// </summary>
	public delegate void ASkillShowClick(int SkillID);
	public ASkillShowClick SkillShowEvent;
	/// <summary>
	/// The c倒计时event.
	/// </summary>
	public delegate void CountDownEnd();
	public CountDownEnd CountDownEvent;
	/// <summary>
	/// The 宠物技能 event.
	/// </summary>
	public delegate void PetOnClick(int Skillid);
	public PetOnClick PetOnEvent;

	/// <summary>
	/// The 返回按钮 event.
	/// </summary>
	public delegate void BackOnClick();
	public BackOnClick BackEvent;


	public delegate void BabyOnClick(int uid);
	public BabyOnClick BabyEvent;

	public delegate void SetPanelActive(bool isActive);
	public SetPanelActive OnSetPanelActive;

	private static AttaclEvent instance;
	public static AttaclEvent getInstance {
		get {
			if (instance == null) {
				instance = new AttaclEvent ();
			}
			return instance;
		}
	}
}
