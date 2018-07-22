using UnityEngine;
using System.Collections;

public class AuthLogo : MonoBehaviour {

	AsyncOperation async_;

	bool CanEnter_;
	bool LoginLoaded;

    void Start()
    {
		UIManager.Instance.ToString();;
		CanEnter_ = false;
		async_ = Application.LoadLevelAsync(GlobalValue.StageName_LoginScene);
		async_.allowSceneActivation = false;
        StartCoroutine(showLogo());
		AssetLoader.LoadAssetBundle("commonAssets", AssetLoader.EAssetType.ASSET_UI, (AssetBundle bundle, ParamData data) =>
		{
			UIFactory.Instance.LoadUIPanel("LoginPanel", () => {
				LoginLoaded = true;
			});
		}, null, Configure.assetsPathstreaming);
    }

    IEnumerator showLogo()
    {
        TweenAlpha.Begin(gameObject, 2f, 1f);

        yield return new WaitForSeconds(2f);
        yield return new WaitForSeconds(1.5f);

        TweenAlpha.Begin(gameObject, 2f, 0f);
        yield return new WaitForSeconds(2f);
        yield return null;

		CanEnter_ = true;
    }

	void Update()
	{
		if(async_ != null)
		{
			if(async_.progress >= 0.9f && CanEnter_ && LoginLoaded)
			{
				async_.allowSceneActivation = true;
			}
		}
		AssetLoader.Update();
		AtlasLoader.Instance.Update();
		UIAssetMgr.Update();
	}
}
