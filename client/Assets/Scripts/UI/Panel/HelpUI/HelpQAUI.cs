using UnityEngine;
using System.Collections.Generic;

public class HelpQAUI : MonoBehaviour {

    public UIGrid leftGrid_;
    public GameObject leftItem_;

    public Transform rightGrid_;
    public GameObject rightItem_;

    public UIPanel rightPanel_;

	List<GameObject> leftList = new List<GameObject>();
    List<GameObject> leftPool_;
    List<GameObject> rightPool_;

    int crtType;

    void OnEnable()
    {
        if (leftPool_ == null)
            leftPool_ = new List<GameObject>();

        if (rightPool_ == null)
            rightPool_ = new List<GameObject>();

        //left
        QaData.TypePkg[] tDatas = QaData.GetAllType();
        GameObject go = null;
        UIEventListener lis = null;
        for (int i = 0; i < tDatas.Length; ++i)
        {
            if (i > leftPool_.Count - 1)
            {
                go = (GameObject)GameObject.Instantiate(leftItem_) as GameObject;
                go.transform.parent = leftGrid_.transform;
                go.transform.localScale = Vector3.one;
                lis = UIEventListener.Get(go);
                lis.parameter = i;
                lis.onClick += OnClickTab;
                leftPool_.Add(go);
				leftList.Add(go);
                go.SetActive(true);
            }
            else
            {
                go = leftPool_[i];
            }
            go.GetComponent<HelpQATypeItem>().SetData(tDatas[i].icon_, tDatas[i].name_);
        }

        //right
        crtType = 0;
        UpdateRight();
    }

    void UpdateRight()
    {
        ClearRight();
        QaData[] datas = QaData.GetDataByType(crtType);

        int crtHeight = 0;
        GameObject go = null;
        for (int i = 0; i < datas.Length; ++i)
        {
            if (i > rightPool_.Count - 1)
            {
                go = (GameObject)GameObject.Instantiate(rightItem_) as GameObject;
                go.transform.parent = rightGrid_;
                go.transform.localScale = Vector3.one;
                rightPool_.Add(go);
            }
            else
            {
                go = rightPool_[i];
            }

            go.transform.localPosition = new Vector3(0f, crtHeight, 0f);
            int height = go.GetComponent<HelpQAContentItem>().SetData(datas[i].question, datas[i].answer);
            crtHeight -= height;
            BoxCollider col = go.GetComponent<BoxCollider>();
            col.size = new Vector3(col.size.x, height);
            go.SetActive(true);
        }
    }

    void OnClickTab(GameObject go)
    {
		for(int i =0;i<leftList.Count;i++)
		{
			leftList[i].gameObject.GetComponent<UIButton> ().isEnabled = true;
		}
        int tab = (int)UIEventListener.Get(go).parameter;
		go.gameObject.GetComponent<UIButton> ().isEnabled = false;
        crtType = tab;
        UpdateRight();
    }

    void ClearRight()
    {
        rightPanel_.clipOffset = Vector2.zero;
        rightPanel_.transform.localPosition = Vector3.zero;
        rightPanel_.GetComponent<UIScrollView>().ResetPosition();
        for (int i = 0; i < rightPool_.Count; ++i)
        {
            rightPool_[i].SetActive(false);
        }
    }

    void OnDisable()
    {
        ClearRight();
    }
}
