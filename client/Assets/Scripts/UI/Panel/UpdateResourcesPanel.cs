using UnityEngine;
using System.Collections;

public class UpdateResourcesPanel : MonoBehaviour
{

	public UISlider  bar;
	public UILabel numLab;
	public UILabel copyTextLab;
	public UILabel downTextLab;
	private float maxNum;
	private string copyText;
	private string downText;

	void Start ()
	{
		VersionManager.Instance.startDownFileEvent += new RequestEventHandler<int>(OnStartVersionDownFileEvent);
		VersionManager.Instance.downFileEvent += new RequestEventHandler<int>(OnDownFileEvent);
		VersionManager.Instance.CopyEvent += new RequestEventHandler<int>(copyFileEvent);
        VersionManager.Instance.finishDownFileEvent += new RequestEventHandler<int>(finishDownFileEvent);
        VersionManager.Instance.startCopyEvent += new RequestEventHandler<int>(startCopyEvent);
		VersionManager.Instance.versionDataNumEvent += new RequestEventHandler<string>(versionDataNumEvent);
		ConfigLoader.Instance.downFileEvent += new RequestEventHandler<int>(OnDownFileEvent);
		ConfigLoader.Instance.startDownFileEvent += new RequestEventHandler<int>(OnStartDownFileEvent);
		ConfigLoader.Instance.finishDownFileEvent += new RequestEventHandler<int>(OnFinishDownFileEvent);
		copyText = copyTextLab.text;
		downText = downTextLab.text;

		if(!ApplicationEntry.Instance.isLoadFileFinish)
		{
			ApplicationEntry.Instance.LoginUIOk ();
		}
		else
		{
			OnFinishDownFileEvent(1);
		}
	}

	void OnStartDownFileEvent(int num)
	{
		maxNum = num;
		downTextLab.gameObject.SetActive (false);
		copyTextLab.gameObject.SetActive (true);
		copyTextLab.text = copyText;
		gameObject.SetActive (true);
	}

	void versionDataNumEvent(string str)
	{
		copyTextLab.text = copyText + "       "+str;
		downTextLab.text = downText + "       "+str;
	}

	void OnStartVersionDownFileEvent(int num)
	{
		maxNum = num;
		downTextLab.gameObject.SetActive (true);
		copyTextLab.gameObject.SetActive (false);
		gameObject.SetActive (true);
	}

    void finishDownFileEvent(int num)
    {
        gameObject.SetActive(false);
    }

	void OnDownFileEvent(int num)
	{
		if (maxNum <= 0)
			return;
		bar.value = (float)num / maxNum;
		numLab.text = ((int)(num / maxNum* 100)).ToString() + "%";
	}

	
	void OnFinishDownFileEvent(int num)
	{
		gameObject.SetActive (false);
	}

	void copyFileEvent(int num)
	{
		if (maxNum <= 0)
			return;
		bar.value = (float)num / maxNum;
		numLab.text = ((int)(num / maxNum* 100)).ToString() + "%";
	}

	void startCopyEvent(int num)
	{
		maxNum = num;
		downTextLab.gameObject.SetActive (false);
		copyTextLab.gameObject.SetActive (true);
		gameObject.SetActive (true);
	}

	void OnDestroy()
	{
		VersionManager.Instance.startDownFileEvent -= OnStartVersionDownFileEvent;
		VersionManager.Instance.downFileEvent -= OnDownFileEvent;
		VersionManager.Instance.CopyEvent -= copyFileEvent;
		VersionManager.Instance.finishDownFileEvent -= finishDownFileEvent;
		VersionManager.Instance.startCopyEvent -= startCopyEvent;
		VersionManager.Instance.versionDataNumEvent -= versionDataNumEvent;
		ConfigLoader.Instance.downFileEvent -= OnDownFileEvent;
		ConfigLoader.Instance.startDownFileEvent -= OnStartDownFileEvent;
		ConfigLoader.Instance.finishDownFileEvent -= OnFinishDownFileEvent;

	}
}

