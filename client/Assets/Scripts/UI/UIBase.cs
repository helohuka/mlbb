using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public enum UIShowCmd{
	Showing = 0,   // 开始打开界面.
	Shown,         // 界面已经打开.
	Hide         // 界面已经关闭.
}

abstract public class UIBase :MonoBehaviour
{

	public abstract void Destroyobj();

	private static Dictionary<string, UIShowCmd>  _PanelShowStates = new Dictionary<string, UIShowCmd>();
	private static Dictionary<string, UIBase>  _PanelShown = new Dictionary<string, UIBase>();

	private string     _UIResPath;
	private  UIPanel  _Panel;
	private menuTypes _MenuType;

	public static void AsyncLoad(UIASSETS_ID id, bool isCollider = true)
	{
        bool mustReturn = StageMgr.Loading;

		string uiResPath = GlobalInstanceFunction.Instance.GetAssetsName( (int)id , AssetLoader.EAssetType.ASSET_UI );
        if (string.IsNullOrEmpty(uiResPath))
        {
            if (id == UIASSETS_ID.UIASSETS_MessageBoxPanel)
            {
                uiResPath = "MessageBoxPanel";
                mustReturn = false;
            }
            if(id == UIASSETS_ID.UIASSETS__ErrorTipsUI)
            {
                uiResPath = "PromptMessgaePanel";
                mustReturn = false;
            }
            if (id == UIASSETS_ID.UIASSETS_NoticePanel)
            {
                uiResPath = "UpdateNotice";
                mustReturn = false;
            }
            if (id == UIASSETS_ID.UIASSETS_NetWaitPanel)
                uiResPath = "NetWaitPanel";
        }

        if (mustReturn)
            return;

		if (UIManager.Instance.ContainsUI4zhizuo(uiResPath))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue( "zanweikaifang"));
			return;
		}
        foreach (UIShowCmd cmd in _PanelShowStates.Values)
        {
            if (cmd == UIShowCmd.Showing)
			{
				ClientLog.Instance.Log (" return  AsyncLoad    " + id);
				if(id != UIASSETS_ID.UIASSETS_MessageBoxPanel &&
                    id != UIASSETS_ID.UIASSETS_NpcTaskPanel)
                	return;
			}
        }
		

		if(_PanelShowStates.ContainsKey(uiResPath) && _PanelShowStates[uiResPath] != UIShowCmd.Hide)
		{
			if(_PanelShown.ContainsKey(uiResPath))
			{
				_PanelShown[uiResPath].Show();
			}

			return;
		}

		_PanelShowStates[uiResPath] = UIShowCmd.Showing;

        if (UIManager.Instance.ContainsUI4Loading(uiResPath))
            ApplicationEntry.Instance.ShowUILoading();

        UIAssetMgr.LoadUI(uiResPath, (Assets, paramData) =>
        {
			if( null == Assets || null == Assets.mainAsset )
			{
                ApplicationEntry.Instance.HideUILoading();
				return ;
			}

			GameObject go = (GameObject)GameObject.Instantiate(Assets.mainAsset)as GameObject;
			if(isCollider)
			{
				if(id != UIASSETS_ID.UIASSETS_NpcTaskPanel)
               	 GlobalInstanceFunction.Instance.MakeUIMask(go);
			}

            UIBase ui = go.GetComponent<UIBase>();
			ui._UIResPath = uiResPath;

			ui.AttachToGameObject(go);
			
			if(_PanelShowStates[uiResPath] == UIShowCmd.Hide)
			{    
				ui.Hide();
			}
			else
			{
				_PanelShowStates[uiResPath] = UIShowCmd.Shown;
				_PanelShown[uiResPath] = ui;
				ui.Show();
			}
            ApplicationEntry.Instance.HideUILoading();
		}
  		 , null);
	}

	public static void HidePanelByName(UIASSETS_ID id)
	{
        string uiResPath = GlobalInstanceFunction.Instance.GetAssetsName((int)id, AssetLoader.EAssetType.ASSET_UI);
        if (string.IsNullOrEmpty(uiResPath))
        {
            if (id == UIASSETS_ID.UIASSETS_MessageBoxPanel)
                uiResPath = "MessageBoxPanel";
            if (id == UIASSETS_ID.UIASSETS__ErrorTipsUI)
                uiResPath = "PromptMessgaePanel";
            if (id == UIASSETS_ID.UIASSETS_NoticePanel)
                uiResPath = "UpdateNotice";
            if (id == UIASSETS_ID.UIASSETS_NetWaitPanel)
                uiResPath = "NetWaitPanel";
        }

		if(!_PanelShowStates.ContainsKey(uiResPath))
		{
			return;
		}

        if (_PanelShowStates[uiResPath] == UIShowCmd.Showing)
        {
            _PanelShowStates[uiResPath] = UIShowCmd.Hide;
            return;
        }
		
		if(_PanelShown.ContainsKey(uiResPath))
		{
			_PanelShown[uiResPath].Hide();
		}
		else
		{
			//_PanelShowStates[uiResPath] = UIShowCmd.Hide;
		}
	}

	public static void SwitchShowPanelByName(UIASSETS_ID id)
	{
		string uiResPath = GlobalInstanceFunction.Instance.GetAssetsName( (int)id , AssetLoader.EAssetType.ASSET_UI );
		if(_PanelShowStates.ContainsKey(uiResPath) && _PanelShowStates[uiResPath] != UIShowCmd.Hide)
		{
			if(_PanelShown.ContainsKey(uiResPath))
			{
				_PanelShown[uiResPath].Show();
			}
			return;
		}

		List<string> strlist = new List<string>();
		foreach (var x in _PanelShowStates)
		{
			if(x.Value == UIShowCmd.Shown)
				strlist.Add(x.Key);
		}
        for (int i = 0; i < strlist.Count; ++i )
        {
            _PanelShown[strlist[i]].Hide();
        }

		AsyncLoad (id);
	}



	private void AttachToGameObject(GameObject goSelf)
	{
		_Panel = goSelf.GetComponent<UIPanel>();
	}
	
	public  UIPanel panel
	{
		get
		{
			return _Panel;
		}

		set
		{
			_Panel = value;
		
		}
	}


	public string UIName
	{
		get 
		{
			return _UIResPath;
		}
		set
		{
			_UIResPath = value;
		}
	}


	public virtual void Show()
	{
		// 加入到从属的AppState的UIManager中.
		UIManager.Instance.ShowUI (this);
		//2015-7-15 xiaoxiao add
		SoundTools.PlaySound( SOUND_ID.SOUND_CLOSEUI );
	}

	public void Hide()
	{
		// Remove from global container.
	
		if (_PanelShowStates.ContainsKey (_UIResPath)) 
		{
            //if(_PanelShowStates[_UIResPath] == UIShowCmd.Hide)
            //{
            //    return;
            //}
			_PanelShowStates[_UIResPath] = UIShowCmd.Hide;
			_PanelShown.Remove (_UIResPath); 
		}
		if(_Panel == null)
		{
			return;
		}
		// 从场景移除.
		DoHide();

        AtlasLoader.Instance.DeleteAtlas(UIDepedenceData.GetRefAtlas(_UIResPath));
        UIAssetMgr.DeleteAsset(_UIResPath);
	}

	protected virtual void DoHide()
	{
		if (_Panel == null) 
		{
			return;
		}

		UIManager.Instance.HideUI(this);

        _Panel.GetComponent<UIBase>().Destroyobj();
		UnityEngine.GameObject.Destroy(_Panel.gameObject);
		_Panel = null;

		//AssetInfoMgr.Instance.DecRefCount( _UIResPath, true );
	}


}
