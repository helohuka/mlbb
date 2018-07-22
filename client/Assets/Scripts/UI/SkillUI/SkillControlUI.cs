using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillControlUI : UIBase
{

	public UIButton closeBtn;  
	public UIButton gatherBtn;
	public UIButton compoundBtn;

	public GameObject _gathePane;
	public GameObject _compoundPane;
	private static SkillControlUI _instance = null;
	public static SkillControlUI Instance 
	{
		get{
			return _instance;
		}
	}
	
	void Start ()
	{
		_instance = this;
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		UIManager.SetButtonEventHandler (gatherBtn.gameObject, EnumButtonEvent.OnClick, OnClickGather, 0, 0);
		UIManager.SetButtonEventHandler (compoundBtn.gameObject, EnumButtonEvent.OnClick, OnClickCompound, 0, 0);
		OpenPanelAnimator.PlayOpenAnimation(this.panel,()=>{
	//	_gathePane.GetComponent<GatherUI> ().UpdateSkillList();
		_compoundPane.GetComponent<CompoundUI>().UpdateSkillList();
		compoundBtn.isEnabled = false;
		});

	}
	


	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_SkillTabPanel);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_SkillTabPanel);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_SkillTabPanel);
	}
	
	#endregion


	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		_compoundPane.GetComponent<CompoundUI>().Hide();
		OpenPanelAnimator.CloseOpenAnimation(this.panel, ()=>{
			Hide ();	
		});
	}

	protected override void DoHide ()
	{
		base.DoHide ();
	}

	private void OnClickGather(ButtonScript obj, object args, int param1, int param2)
	{
	}
	
	private void OnClickCompound(ButtonScript obj, object args, int param1, int param2)
	{
		TabBtnEnabled ();
		compoundBtn.isEnabled = false;
		if(_compoundPane!=null)
		{
			if(_compoundPane.gameObject.activeSelf)
				return;
			_compoundPane.gameObject.SetActive(true);
			_compoundPane.GetComponent<CompoundUI>().Show();
		}
	}


	private bool _gatherLoaded = false;
	private void LoadUI(UIASSETS_ID id)
	{
		string uiResPath = GlobalInstanceFunction.Instance.GetAssetsName( (int)id , AssetLoader.EAssetType.ASSET_UI );

		AssetLoader.LoadAssetBundle (uiResPath, AssetLoader.EAssetType.ASSET_UI,(Assets,paramData)=> {
			if( null == Assets || null == Assets.mainAsset )
			{
				return ;
			}
			
			GameObject go = (GameObject)GameObject.Instantiate(Assets.mainAsset)as GameObject;
			if(this == null && !gameObject.activeSelf)
			{
				Destroy(go);
			}

			UIPanel con = go.GetComponent<UIPanel>();

			con.transform.parent = this.panel.transform;    
			con.transform.localPosition = Vector3.zero;
			con.transform.localScale = Vector3.one;
			con.depth = 5;

			if(id == UIASSETS_ID.UIASSETS_Compound)
			{
				_compoundPane = go;
			}
			else if(id == UIASSETS_ID.UIASSETS_GatherPanel)
			{
				_gathePane = go;
			}
			else if(id == UIASSETS_ID.UIASSETS_SkillTabPanel)
			{

			}
		}
		, null);

	}


	private void TabBtnEnabled()
	{
		gatherBtn.isEnabled = true;
		compoundBtn.isEnabled = true;
	}

	public override void Destroyobj ()
	{
		GameObject.Destroy (gameObject);
	}

	
	public void makeGather()
	{
		_compoundPane.gameObject.SetActive (false);
		TabBtnEnabled ();
		gatherBtn.isEnabled = false;
		//_gathePane.gameObject.SetActive (true);
		//_gathePane.GetComponent<GatherUI>().Show();
	}
	


}

