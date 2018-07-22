using UnityEngine;
using System.Collections;

public class ArrowTriggerHandler : MonoBehaviour {

    public int questLimit_;
    public GameObject aim_;

	// Use this for initialization
	void Start () {
        if (GetComponent<BoxCollider>() == null)
        {
            ClientLog.Instance.LogWarning(gameObject.name + " has no trigger.");
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!QuestSystem.IsQuestDoing(questLimit_))
            aim_.SetActive(false);
	}

    void OnTriggerEnter(Collider other)
    {
        if (QuestSystem.IsQuestDoing(questLimit_))
            aim_.SetActive(true);
    }

    void OnTriggerStay(Collider other)
    {
        if (QuestSystem.IsQuestDoing(questLimit_))
            aim_.SetActive(true);
        else
            aim_.SetActive(false);
    }

    void OnTriggerExit(Collider other)
    {
        if (QuestSystem.IsQuestDoing(questLimit_))
            aim_.SetActive(false);
    }
}
