
using UnityEngine;
using System.Collections;


/// <summary>
/// Sub system. 子系统基类。 玩家的背包/英雄/聊天/任务等都归类到不同的子系统中.
/// </summary>
public class SubSystem  {
	
	public virtual int serverDataVersion{
		get{
			return -1;
		}
	}
	
	public virtual void RegisterNetMessageHandlers()
	{
	}
	
	/// <summary>
	/// Loads From/Save to  local cache.
	/// </summary>
	public virtual void LoadFromLocalCache(){}
	public virtual void SaveToLocalCache(){}
}
