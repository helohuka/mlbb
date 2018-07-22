using UnityEngine;
using System.Collections;


public enum SwitchScenEffect
{
	SMPlainTransition,
	SMFadeTransition,
	SMBlindsTransition,
	SMNewspaperTransition,
	SMCartoonTransition,
	SMNinjaTransition,
	SMCinemaTransition,
	SMPixelateTransition,
	SMTetrisTransition,
	SMTilesTransition,
	LoadingBar
}

public class ShowGame : MonoBehaviour {

	     
	private static ShowGame instance;   
	
	void Awake()
	{
		instance = this;
	}
	public static ShowGame Instance
	{
		get
		{
			return instance;	
		}
	}

	void Start () {         
	
	}                
	// Update is called once per frame        
	void Update () {                 
	}  

	public void ShowScenEffect(SwitchScenEffect ScenEffect)
	{
        SceneLoader.Instance.LoadScene(StageMgr.Scene_name, () =>
        {
            string transitionPrefab = "Transitions/" + ScenEffect.ToString();
            GameObject prefab = (GameObject)Resources.Load(transitionPrefab);
            GameObject instance = (GameObject)GameObject.Instantiate(prefab);
            SMTransition transition = instance.GetComponent<SMTransition>();
            transition.screenId = StageMgr.Scene_name;
            transition.loadAsync = true;
            transition.prefetchLevel = true;
        }) ;
	}
}
