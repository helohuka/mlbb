using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AcceptableRenwuUI : MonoBehaviour {

    const int NONE_TASK_TYPE = 0;
    const int MAIN_TASK_TYPE = 1;
    const int SUB_TASK_TYPE = 2;
	public UIPanel jianliPanle;
	public UIButton mainBtn;
	public GameObject item;
	public UIGrid maingrid;
	public GameObject mainTaskObj;
	public UILabel taskgoal;
	public UILabel taskDescription;
	public UILabel ReceiveNpc;
	public UILabel completeNpc;
	public GameObject questKind_item;
	public GameObject taskInfoObj;
	private List<QuestKind> QuestKinds = new List<QuestKind>();
	private List<GameObject>QuestKindsObj = new List<GameObject> ();


    public UIGrid JGrid;
    public GameObject JItem;

    public int TaskType = NONE_TASK_TYPE;
    
    public UIEventListener Listener;

	public List<QuestKind> QuestKindList
	{
		get
		{
			return QuestKinds;
		}
	}


	void Start () {
	
//		item.SetActive (false);
//		questKind_item.SetActive (false);
//		for(int i =0;i<QuestSystem.AcceptableList.Count;i++)
//		{
//			QuestData qda = QuestData.GetData(QuestSystem.AcceptableList[i]);
//			if(!QuestKinds.Contains(qda.questKind_))
//			{
//				QuestKinds.Add(qda.questKind_);
//			}
//				
//		}
//
//		AddQuestKindItem ();
		//GlobalInstanceFunction.Instance.Invoke(() => {ChooseKindItem(); }, 1);
	}
	void AddQuestKindItem()
	{
		for(int i =0;i<QuestKindsObj.Count;i++)
		{
			TaskKindCell taCell = QuestKindsObj[i].GetComponent<TaskKindCell>();
			taCell.raSp.spriteName = "sanjiao";
			taCell.backSp.gameObject.SetActive(false);
			QuestKindsObj[i].SetActive(false);
		}


		for(int i =0;i<QuestKinds.Count;i++)
		{

			GameObject go = null;
			if(i<QuestKindsObj.Count)
			{
				go = QuestKindsObj[i];
			}else
			{
				go = GameObject.Instantiate(questKind_item)as GameObject;
			}
			go.SetActive(true);
			go.transform.parent = maingrid.transform;
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one;
			QuestKindsObj.Add(go);
			UILabel la = go.GetComponentInChildren<UILabel>();
			la.text = LanguageManager.instance.GetValue(QuestKinds[i].ToString()); 
			UIManager.SetButtonEventHandler (go, EnumButtonEvent.OnClick, OnClickReceive,(int)QuestKinds[i], 0);
			maingrid.repositionNow = true;
		}


	}
	bool IsShowProfession()
	{
		Profession pro = Profession.get ((JobType)GamePlayer.Instance.GetIprop (PropertyType.PT_Profession), GamePlayer.Instance.GetIprop (PropertyType.PT_ProfessionLevel));
		if (QuestSystem.aqid == 0)
						return false;
		QuestData qdai = QuestData.GetData (QuestSystem.aqid);
		if (qdai == null)return false;
		if(qdai.questKind_ == QuestKind.QK_Profession)
		{
			if(qdai.JobLevel_ != 1)
			{
				if(qdai.jobtype_ == (int)GamePlayer.Instance.GetIprop (PropertyType.PT_Profession))
				{
					if( qdai.JobLevel_ - GamePlayer.Instance.GetIprop (PropertyType.PT_ProfessionLevel) == 1)
					{
						return true;
					}
					
				}
			}
			
		}
		return false;
	}
	void OnEnable()
	{
		item.SetActive (false);
		questKind_item.SetActive (false);
		for(int i =0;i<QuestSystem.AcceptableList.Count;i++)
		{
			QuestData qda = QuestData.GetData(QuestSystem.AcceptableList[i]);
			if(!QuestKinds.Contains(qda.questKind_))
			{
				QuestKinds.Add(qda.questKind_);
			}
			
		}
		
		AddQuestKindItem ();
		ChooseKindItem ();
//		if(IsShowProfession())
//		{
//			TaskUI tui = taskInfoObj.GetComponent<TaskUI> ();
//			tui.ShowTaskIonf (QuestSystem.aqid);
//		}else
//		{
//			TaskUI tui = taskInfoObj.GetComponent<TaskUI> ();
//			tui.closeItem();
//		}
	}
	void ChooseKindItem()
	{
		if(QuestKindsObj.Count>0)
		{
			btnsp = QuestKindsObj[0];
			isOpen = true;
			TaskKindCell taCell = btnsp.GetComponent<TaskKindCell>();
			taCell.raSp.spriteName = "sanjiao2";
			taCell.backSp.gameObject.SetActive(true);
			InitQuestKindItem();
		}
	}
	void InitQuestKindItem()
	{


		foreach(GameObject c in chindCellList)
		{
			maingrid.RemoveChild(c.transform);
			c.transform.parent = null;
			c.gameObject.SetActive(false);
			chindCellPoolList.Add(c);
		}
		
		chindCellList.Clear ();
		maingrid.Reposition();

		int index = maingrid.GetIndex (QuestKindsObj[0].transform);
		List<QuestData> qdas = QuestSystem.GetQuestDataForQuestKind (QuestKinds[0]);
		List<QuestData> qds = new List<QuestData> ();
		Profession pro = Profession.get ((JobType)GamePlayer.Instance.GetIprop (PropertyType.PT_Profession), GamePlayer.Instance.GetIprop (PropertyType.PT_ProfessionLevel));
		for(int i =0;i<qdas.Count;i++)
		{
			if(qdas[i].questKind_ == QuestKind.QK_Profession)
			{
				if(qdas[i].JobLevel_ != 1)
				{
					if(qdas[i].jobtype_ == (int)GamePlayer.Instance.GetIprop (PropertyType.PT_Profession))
					{
						if( qdas[i].JobLevel_ - GamePlayer.Instance.GetIprop (PropertyType.PT_ProfessionLevel) == 1)
						{
							qds.Add(qdas[i]);
						}
						
					}
				}
				
			}
		}
	
		for(int i =0;i<qds.Count;i++)
		{
			if(QuestSystem.IsFDailyQuest())
			{
				if(qds[i].questKind_ == QuestKind.QK_Daily)
				{
					continue;
				}
			}
		
			GameObject objCell = null;
			if(chindCellPoolList.Count>0)
			{
				objCell = chindCellPoolList[0];
				chindCellPoolList.Remove(objCell);  
			}
			else  
			{
				objCell = Object.Instantiate(item) as GameObject;
			}			
			UIManager.SetButtonEventHandler (objCell, EnumButtonEvent.OnClick, OnClickChindItem,qds[i].id_, 0);
			maingrid.AddChild(objCell.transform,++index);
			objCell.SetActive(true);
			objCell.transform.localScale = Vector3.one;
			AcceptableCell acell =  objCell.GetComponent<AcceptableCell>();
			acell.Qdata = qds[i];
			chindCellList.Add(objCell);
			maingrid.repositionNow = true;
		}
		if(chindCellList.Count>0)
		{
			//
			AcceptableCell acell = chindCellList[0].GetComponent<AcceptableCell>();
			acell.stateSp.gameObject.SetActive(true);
			acell.statetwoSp.gameObject.SetActive (true);
			curCell = acell;
			TaskUI tui = taskInfoObj.GetComponent<TaskUI> ();
			tui.ShowTaskIonf (qds[0].id_);
			QuestSystem.aqid = qds[0].id_;
		}else
		{
			TaskUI tui = taskInfoObj.GetComponent<TaskUI> ();
			tui.closeItem();
			tui.chuansongBtn.gameObject.SetActive(false);
			tui.abnegateBtn.gameObject.SetActive(false);
		}


	}
	private List<GameObject> chindCellList = new List<GameObject>();
	private List<GameObject> chindCellPoolList = new List<GameObject>();
	bool isOpen = false;
	GameObject btnsp;
	void OnClickReceive(ButtonScript obj, object args, int param1, int param2)
	{
		if(btnsp != obj.gameObject && btnsp != null)
		{

			TaskKindCell taCell = btnsp.GetComponent<TaskKindCell>();
			taCell.raSp.spriteName = "sanjiao";
			taCell.backSp.gameObject.SetActive(false);
		}

		if(btnsp != obj.gameObject)
		{

			if(isOpen )
			{
				foreach(GameObject c in chindCellList)
				{
					maingrid.RemoveChild(c.transform);
					c.transform.parent = null;
					c.gameObject.SetActive(false);
					chindCellPoolList.Add(c);
				}

				chindCellList.Clear ();
				maingrid.Reposition();
				isOpen = false;
			}

			TaskKindCell taCell = obj.GetComponent<TaskKindCell>();
			taCell.raSp.spriteName = "sanjiao2";
			taCell.backSp.gameObject.SetActive(true);

		}else
		{

			TaskKindCell taCell = obj.GetComponent<TaskKindCell>();
			taCell.raSp.spriteName = "sanjiao";
			taCell.backSp.gameObject.SetActive(false);


			if(isOpen )
			{
				foreach(GameObject c in chindCellList)
				{
					maingrid.RemoveChild(c.transform);
					c.transform.parent = null;
					c.gameObject.SetActive(false);
					chindCellPoolList.Add(c);
				}
				chindCellList.Clear ();
				maingrid.Reposition();
				isOpen = false;
				return;
			}else
			{

				TaskKindCell taCells = obj.GetComponent<TaskKindCell>();
				taCells.raSp.spriteName = "sanjiao2";
				taCells.backSp.gameObject.SetActive(true);

			}
		}
		isOpen = true;
		btnsp = obj.gameObject;
		int index = maingrid.GetIndex (obj.transform);

		List<QuestData> qdas = QuestSystem.GetQuestDataForQuestKind ((QuestKind)param1);
		List<QuestData> qds = new List<QuestData> ();
		Profession pro = Profession.get ((JobType)GamePlayer.Instance.GetIprop (PropertyType.PT_Profession), GamePlayer.Instance.GetIprop (PropertyType.PT_ProfessionLevel));
		for(int i =0;i<qdas.Count;i++)
		{
			if(qdas[i].questKind_ == QuestKind.QK_Profession)
			{
				if(qdas[i].JobLevel_ != 1)
				{
					if(qdas[i].jobtype_ == (int)GamePlayer.Instance.GetIprop (PropertyType.PT_Profession))
					{
						if( qdas[i].JobLevel_ - GamePlayer.Instance.GetIprop (PropertyType.PT_ProfessionLevel) == 1)
						{
							qds.Add(qdas[i]);
						}

					}
				}

			}else 
				if(qdas[i].questKind_ == QuestKind.QK_Daily)
			{
				if(QuestSystem.IsFDailyQuest())
				{
						continue;
				}
				if(IsQuestDailyLevel(qdas[i]))
				{
					qds.Add(qdas[i]);
				}
			}else
			{
				qds.Add(qdas[i]);
			}
		}


		for(int i =0;i<qds.Count;i++)
		{
			if(qds[i].targetId_ != 0&& qds[i].questKind_ == QuestKind.QK_Tongji && qds[i].questType_ == QuestType.QT_Kill)
				continue;

            if (qds[i].targetId_ != 0 && qds[i].questKind_ == QuestKind.QK_Rand)
                continue;

			GameObject objCell = null;
			if(chindCellPoolList.Count>0)
			{
				objCell = chindCellPoolList[0];
				chindCellPoolList.Remove(objCell);  
			}
			else  
			{
				objCell = Object.Instantiate(item) as GameObject;
			}			
			UIManager.SetButtonEventHandler (objCell, EnumButtonEvent.OnClick, OnClickChindItem,qds[i].id_, 0);
			maingrid.AddChild(objCell.transform,++index);
			objCell.SetActive(true);
			objCell.transform.localScale = Vector3.one;
			AcceptableCell acell =  objCell.GetComponent<AcceptableCell>();
			acell.Qdata = qds[i];
			chindCellList.Add(objCell);

			maingrid.repositionNow = true;

		}

	}


	void ssss()
	{

	}


	bool IsQuestDaily( QuestData qdata)
	{
		for(int i =0;i<QuestSystem.CurrentList.Count;i++)
		{
			QuestData qdataa = QuestData.GetData(QuestSystem.CurrentList[i].questId_);
			if(qdata.questKind_ == qdataa.questKind_)
			{
				return true;
			}
		}
		return false;
	}
	bool IsQuestDailyLevel(QuestData qdata)
	{
		List<QuestData> temp = new List<QuestData> ();
		foreach(KeyValuePair<int,QuestData> par in QuestData.GetMetaData ())
		{
			if(par.Value.questKind_ == QuestKind.QK_Daily)
			{
				if(par.Value.needLevel_<=GamePlayer.Instance.GetIprop(PropertyType.PT_Level))
				{
					temp.Add(par.Value);
				}
			}
		}
		if(temp.Count==0)
			return true;
		QuestData qdat = temp[0];
		for(int j =0;j<temp.Count;j++)
		{
			qdat = qdat.needLevel_>temp[j].needLevel_?qdat:temp[j];
		}
		if(qdata.needLevel_>=qdat.needLevel_)
		{
			return true;
		}
		return false;
	}
//	bool ContinueCompletedJopQuest()
//	{
//
//		for(int i =0;i<QuestSystem.AcceptableList.Count;i++)
//		{
//			QuestData q = QuestData.GetData(QuestSystem.AcceptableList[i]);
//			if(IsCompletedJopQuest(q))
//			{
//				return true;
//			}
//
//	    }
//		return false;
//	}

	bool IsCompletedJopQuest()
	{
		for(int i =0;i<QuestSystem.CompletedList.Count;i++)
		{
			QuestData q = QuestData.GetData(QuestSystem.CompletedList[i]);
			if(q.questKind_ == QuestKind.QK_Profession)
			{
				return true;
			}

		}
		return false;
	}

	private AcceptableCell curCell;
	void OnClickChindItem(ButtonScript obj, object args, int param1, int param2)
	{
		TaskUI.CurrentId = param1;
		QuestSystem.aqid = param1;
		AcceptableCell acell =  obj.GetComponent<AcceptableCell>();
		if(curCell != null )
		{
			curCell.stateSp.gameObject.SetActive(false);
			curCell.statetwoSp.gameObject.SetActive (false);
		}
		curCell = acell;
		acell.stateSp.gameObject.SetActive (true);
		acell.statetwoSp.gameObject.SetActive (true);
		//ShowTaskIonf (param1);
		TaskUI tui = taskInfoObj.GetComponent<TaskUI> ();
		tui.ShowTaskIonf (param1);
	}
	void ShowTaskIonf(int qid)
	{
		string goal_str = "";
		QuestData quest = QuestData.GetData(qid);
        if (quest == null)
            return;
		
		taskDescription.text = quest.QuestDes_;
		//ReceiveNpc.text = NpcData.GetData(qid).npcname_;
        NpcData npc = NpcData.GetData(quest.finishNpcId_);
        if(npc != null)
            completeNpc.text = npc.NpcName;
		
		
		if(quest.questType_ == QuestType.QT_Battle)
		{
			SceneData sData = SceneData.GetData(quest.targetId_);
			goal_str = string.Format("在{0}中发生{1}战斗",sData.sceneName_,quest.targetNum_);
			
		}
		else if(quest.questType_ == QuestType.QT_Dialog)
		{
			goal_str = quest.QuestDes_ ;
		}
		else if(quest.questType_ == QuestType.QT_Item)
		{
			ItemData iDta = ItemData.GetData(quest.targetId_);
			NpcData nData = NpcData.GetData(quest.finishNpcId_);
			goal_str = string.Format("把{0}个{1}交给{2}只",quest.targetNum_,iDta.name_,nData.Name);
		}
		else if(quest.questType_ == QuestType.QT_Kill)
		{
			BabyData bData = BabyData.GetData(quest.targetId_);
			if(bData != null)
			goal_str = string.Format("杀死{0}{1}只",bData._Name,quest.targetNum_);
		}
		taskgoal.text = goal_str;

        DropData droData = DropData.GetData(quest.DropID_);

        foreach (Transform tr in JGrid.transform)
        {
            Destroy(tr.gameObject);
        }
		jianliPanle.clipOffset = Vector2.zero;
		jianliPanle.transform.localPosition = Vector3.zero;
		jianliPanle.GetComponent<UIScrollView>().ResetPosition();
        if (droData == null)
            return;

        if (droData.exp_ != 0)
        {
            GameObject cloneExp = GameObject.Instantiate(JItem) as GameObject;
            cloneExp.SetActive(true);
            cloneExp.transform.parent = JGrid.transform;
            cloneExp.transform.position = Vector3.zero;
            cloneExp.transform.localScale = Vector3.one;
            RewardCell rCellExp = cloneExp.GetComponent<RewardCell>();
            rCellExp.RewardIcon.gameObject.SetActive(true);
            //rCellExp.RewardIcon.spriteName = "jingyan";
           // rCellExp.icont.gameObject.SetActive(false);
            rCellExp.RewardIcon.MakePixelPerfect();
            rCellExp.RewardLabel.text = droData.exp_.ToString(); ;
            HeadIconLoader.Instance.LoadIcon("jingyan_icon", rCellExp.icont);
        }
        if (droData.money_ != 0)
        {


            GameObject cloneJin = GameObject.Instantiate(JItem) as GameObject;
            cloneJin.SetActive(true);
            cloneJin.transform.parent = JGrid.transform;
            cloneJin.transform.position = Vector3.zero;
            cloneJin.transform.localScale = Vector3.one;
            RewardCell rCellJin = cloneJin.GetComponent<RewardCell>();
            rCellJin.RewardIcon.gameObject.SetActive(true);
            //rCellJin.icont.gameObject.SetActive(false);
            //rCellJin.RewardIcon.spriteName = "jinbitubiao";
            rCellJin.RewardIcon.MakePixelPerfect();
            rCellJin.RewardLabel.text = droData.money_.ToString();
			HeadIconLoader.Instance.LoadIcon ("jingbitubiao", rCellJin.icont);
        }
		if(droData.item_1_ !=0)
		{
			GameObject clone = GameObject.Instantiate (JItem)as GameObject;
			clone.SetActive(true);
			clone.transform.parent = JGrid.transform;
			clone.transform.position = Vector3.zero;
			clone.transform.localScale = Vector3.one;
			RewardCell rCell = clone.GetComponent<RewardCell>();
			rCell.RewardIcon.gameObject.SetActive(false);
			rCell.icont.gameObject.SetActive(true);
			UISprite sp = clone.GetComponent<UISprite>();
			//rCell.RewardIcon.spriteName = ItemData.GetData (droData.item_1_).icon_;
			ItemCellUI ic =  UIManager.Instance.AddItemCellUI(sp,(uint)droData.item_1_);
			ic.showTips = true;
			//HeadIconLoader.Instance.LoadIcon (ItemData.GetData (droData.item_1_).icon_, rCell.icont);
			rCell.RewardLabel.text =droData.item_num_1_.ToString();	
			rCell.RewardIcon.MakePixelPerfect();
		}
		if(droData.item_2 !=0)
		{
			GameObject clone = GameObject.Instantiate (JItem)as GameObject;
			clone.SetActive(true);
			clone.transform.parent = JGrid.transform;
			clone.transform.position = Vector3.zero;
			clone.transform.localScale = Vector3.one;
			RewardCell rCell = clone.GetComponent<RewardCell>();
			rCell.RewardIcon.gameObject.SetActive(false);
			rCell.icont.gameObject.SetActive(true);
			//rCell.RewardIcon.spriteName = ItemData.GetData (droData.item_2).icon_;
			UISprite sp = clone.GetComponent<UISprite>();
			ItemCellUI ic=  UIManager.Instance.AddItemCellUI(sp,(uint)droData.item_2);
			ic.showTips = true;
			//HeadIconLoader.Instance.LoadIcon (ItemData.GetData (droData.item_2).icon_, rCell.icont);
			rCell.RewardLabel.text =droData.item_num_2.ToString();	
			rCell.RewardIcon.MakePixelPerfect();
		}
		
		if(droData.item_3 !=0)
		{
			GameObject clone = GameObject.Instantiate (JItem)as GameObject;
			clone.SetActive(true);
			clone.transform.parent = JGrid.transform;
			clone.transform.position = Vector3.zero;
			clone.transform.localScale = Vector3.one;
			RewardCell rCell = clone.GetComponent<RewardCell>();
			rCell.RewardIcon.gameObject.SetActive(false);
			rCell.icont.gameObject.SetActive(true);
			//rCell.RewardIcon.spriteName = ItemData.GetData (droData.item_3).icon_;;
			UISprite sp = clone.GetComponent<UISprite>();
			ItemCellUI ic = UIManager.Instance.AddItemCellUI(sp,(uint)droData.item_3);
			ic.showTips = true;
			//HeadIconLoader.Instance.LoadIcon (ItemData.GetData (droData.item_3).icon_, rCell.icont);
			rCell.RewardLabel.text =droData.item_num_3.ToString();	
			rCell.RewardIcon.MakePixelPerfect();
		}
		
		if(droData.item_4 !=0)
		{
			GameObject clone = GameObject.Instantiate (JItem)as GameObject;
			clone.SetActive(true);
			clone.transform.parent = JGrid.transform;
			clone.transform.position = Vector3.zero;
			clone.transform.localScale = Vector3.one;
			RewardCell rCell = clone.GetComponent<RewardCell>();
			rCell.RewardIcon.gameObject.SetActive(false);
			rCell.icont.gameObject.SetActive(true);
			UISprite sp = clone.GetComponent<UISprite>();
			ItemCellUI ic = UIManager.Instance.AddItemCellUI(sp,(uint)droData.item_4);
			ic.showTips = true;
			//rCell.RewardIcon.spriteName = ItemData.GetData (droData.item_4).icon_;;
			//HeadIconLoader.Instance.LoadIcon (ItemData.GetData (droData.item_4).icon_, rCell.icont);
			rCell.RewardLabel.text =droData.item_num_4.ToString();	
			rCell.RewardIcon.MakePixelPerfect();
		}
		if(droData.item_5 !=0)
		{
			GameObject clone = GameObject.Instantiate (JItem)as GameObject;
			clone.SetActive(true);
			clone.transform.parent = JGrid.transform;
			clone.transform.position = Vector3.zero;
			clone.transform.localScale = Vector3.one;
			RewardCell rCell = clone.GetComponent<RewardCell>();
			rCell.icont.gameObject.SetActive(true);
			//rCell.RewardIcon.spriteName = ItemData.GetData (droData.item_5).icon_;;
			rCell.RewardIcon.gameObject.SetActive(false);
			UISprite sp = clone.GetComponent<UISprite>();
			ItemCellUI ic = UIManager.Instance.AddItemCellUI(sp,(uint)droData.item_5);
			ic.showTips = true;
			//HeadIconLoader.Instance.LoadIcon (ItemData.GetData (droData.item_5).icon_, rCell.icont);
			rCell.RewardLabel.text =droData.item_num_5.ToString();	
			rCell.RewardIcon.MakePixelPerfect();
		}
		if(droData.item_6 !=0)
		{
			GameObject clone = GameObject.Instantiate (JItem)as GameObject;
			clone.SetActive(true);
			clone.transform.parent = JGrid.transform;
			clone.transform.position = Vector3.zero;
			clone.transform.localScale = Vector3.one;
			RewardCell rCell = clone.GetComponent<RewardCell>();
			rCell.icont.gameObject.SetActive(true);
			//rCell.RewardIcon.spriteName = ItemData.GetData (droData.item_6).icon_;;
			rCell.RewardIcon.gameObject.SetActive(false);
			UISprite sp = clone.GetComponent<UISprite>();
			ItemCellUI ic = UIManager.Instance.AddItemCellUI(sp,(uint)droData.item_6);
			ic.showTips = true;
			//HeadIconLoader.Instance.LoadIcon (ItemData.GetData (droData.item_6).icon_, rCell.icont);
			rCell.RewardLabel.text =droData.item_num_6.ToString();	
			rCell.RewardIcon.MakePixelPerfect();
		}
//        for (int i = 0; i < droData.item_num_1_; i++)
//        {
//            GameObject clone = GameObject.Instantiate(JItem) as GameObject;
//            clone.SetActive(true);
//            clone.transform.parent = JGrid.transform;
//            clone.transform.position = Vector3.zero;
//            clone.transform.localScale = Vector3.one;
//            RewardCell rCell = clone.GetComponent<RewardCell>();
//            rCell.RewardIcon.gameObject.SetActive(false);
//            rCell.icont.gameObject.SetActive(true);
//            //rCell.RewardIcon.spriteName = ItemData.GetData (droData.item_1_).icon_;
//            HeadIconLoader.Instance.LoadIcon(ItemData.GetData(droData.item_1_).icon_, rCell.icont);
//            rCell.RewardLabel.text = droData.item_num_1_.ToString();
//            rCell.RewardIcon.MakePixelPerfect();
//
//        }
//        for (int i = 0; i < droData.item_num_2; i++)
//        {
//            GameObject clone = GameObject.Instantiate(JItem) as GameObject;
//            clone.SetActive(true);
//            clone.transform.parent = JGrid.transform;
//            clone.transform.position = Vector3.zero;
//            clone.transform.localScale = Vector3.one;
//            RewardCell rCell = clone.GetComponent<RewardCell>();
//            rCell.RewardIcon.gameObject.SetActive(false);
//            rCell.icont.gameObject.SetActive(true);
//            //rCell.RewardIcon.spriteName = ItemData.GetData (droData.item_2).icon_;
//            HeadIconLoader.Instance.LoadIcon(ItemData.GetData(droData.item_2).icon_, rCell.icont);
//            rCell.RewardLabel.text = droData.item_num_2.ToString();
//            rCell.RewardIcon.MakePixelPerfect();
//
//        }
//        for (int i = 0; i < droData.item_num_3; i++)
//        {
//            GameObject clone = GameObject.Instantiate(JItem) as GameObject;
//            clone.SetActive(true);
//            clone.transform.parent = JGrid.transform;
//            clone.transform.position = Vector3.zero;
//            clone.transform.localScale = Vector3.one;
//            RewardCell rCell = clone.GetComponent<RewardCell>();
//            rCell.RewardIcon.gameObject.SetActive(false);
//            rCell.icont.gameObject.SetActive(true);
//            //rCell.RewardIcon.spriteName = ItemData.GetData (droData.item_3).icon_;;
//            HeadIconLoader.Instance.LoadIcon(ItemData.GetData(droData.item_3).icon_, rCell.icont);
//            rCell.RewardLabel.text = droData.item_num_3.ToString();
//            rCell.RewardIcon.MakePixelPerfect();
//
//        }
//        for (int i = 0; i < droData.item_num_4; i++)
//        {
//            GameObject clone = GameObject.Instantiate(JItem) as GameObject;
//            clone.SetActive(true);
//            clone.transform.parent = JGrid.transform;
//            clone.transform.position = Vector3.zero;
//            clone.transform.localScale = Vector3.one;
//            RewardCell rCell = clone.GetComponent<RewardCell>();
//            rCell.RewardIcon.gameObject.SetActive(false);
//            rCell.icont.gameObject.SetActive(true);
//            //rCell.RewardIcon.spriteName = ItemData.GetData (droData.item_4).icon_;;
//            HeadIconLoader.Instance.LoadIcon(ItemData.GetData(droData.item_4).icon_, rCell.icont);
//            rCell.RewardLabel.text = droData.item_num_4.ToString();
//            rCell.RewardIcon.MakePixelPerfect();
//
//        }
//        for (int i = 0; i < droData.item_num_5; i++)
//        {
//            GameObject clone = GameObject.Instantiate(JItem) as GameObject;
//            clone.SetActive(true);
//            clone.transform.parent = JGrid.transform;
//            clone.transform.position = Vector3.zero;
//            clone.transform.localScale = Vector3.one;
//            RewardCell rCell = clone.GetComponent<RewardCell>();
//            rCell.icont.gameObject.SetActive(true);
//            //rCell.RewardIcon.spriteName = ItemData.GetData (droData.item_5).icon_;;
//            rCell.RewardIcon.gameObject.SetActive(false);
//            HeadIconLoader.Instance.LoadIcon(ItemData.GetData(droData.item_5).icon_, rCell.icont);
//            rCell.RewardLabel.text = droData.item_num_5.ToString();
//            rCell.RewardIcon.MakePixelPerfect();
//
//        }
//        for (int i = 0; i < droData.item_num_6; i++)
//        {
//            GameObject clone = GameObject.Instantiate(JItem) as GameObject;
//            clone.SetActive(true);
//            clone.transform.parent = JGrid.transform;
//            clone.transform.position = Vector3.zero;
//            clone.transform.localScale = Vector3.one;
//            RewardCell rCell = clone.GetComponent<RewardCell>();
//            rCell.icont.gameObject.SetActive(true);
//            //rCell.RewardIcon.spriteName = ItemData.GetData (droData.item_6).icon_;;
//            rCell.RewardIcon.gameObject.SetActive(false);
//            HeadIconLoader.Instance.LoadIcon(ItemData.GetData(droData.item_6).icon_, rCell.icont);
//            rCell.RewardLabel.text = droData.item_num_6.ToString();
//            rCell.RewardIcon.MakePixelPerfect();
//
//        }
        JGrid.repositionNow = true;
	}
//	void OnClickmain(ButtonScript obj, object args, int param1, int param2)
//	{
//        if (TaskType == MAIN_TASK_TYPE)
//        {
//            TaskType = NONE_TASK_TYPE;
//            mainTaskObj.SetActive(false);
//            SubTaskObj.SetActive(false);
//        }
//        else
//        {
//            TaskType = MAIN_TASK_TYPE;
//			InitData();
//            mainTaskObj.SetActive(true);
//            SubTaskObj.SetActive(false);
////			TweenPosition tp = subBtn.GetComponent<TweenPosition>();
////			UISprite sp = item.GetComponent<UISprite>();
////			tp.to = new Vector3(subBtn.transform.position.x,mainQuests_.Count*sp.height,subBtn.transform.position.z);
////			tp.enabled = true;
//            RefreshGrid();           
//            AddItems(mainQuests_);
//
//        }
//	}
//	void OnClickSub(ButtonScript obj, object args, int param1, int param2)
//	{
//        if (TaskType == SUB_TASK_TYPE)
//        {
//            TaskType = NONE_TASK_TYPE;
//            mainTaskObj.SetActive(false);
//            SubTaskObj.SetActive(false);
//        }
//        else
//        {
//            TaskType = SUB_TASK_TYPE;
//
//            mainTaskObj.SetActive(false);
//            SubTaskObj.SetActive(true);
//
//            RefreshGrid();
//
//            InitData();
//            AddItems(subQuests_);
//        }
//	}

//	public void AddItems(List<int> qlist)
//	{
//		for (int i = 0; i<qlist.Count; i++) {
//
//            QuestData qdata = QuestData.GetData(qlist[i]);
//
//			GameObject o = GameObject.Instantiate(item)as GameObject;
//			o.SetActive(true);
//			o.name = o.name+i;
//            if (qdata.questKind_ == QuestKind.QK_Main)
//			{
//				o.transform.parent = maingrid.transform;
//			}
//			else
//			{
//				o.transform.parent = Subgrid.transform;
//			}
//
//			UILabel []labs = o.GetComponentsInChildren<UILabel>();
//			foreach(UILabel la in labs)
//			{
//				if(la.gameObject.name.Equals("TaskLabel"))
//				{
//
//                    la.text = qdata.questName_;
//
//				}
//				if(la.gameObject.name.Equals("stateLabel"))
//				{
//					if(QuestSystem.IsQuestFinish(qdata.id_))
//					{
//						la.text ="完成";
//					}else
//					{
//						la.text ="未开启";
//					}
//					 
//					
//				}
//			}
//			o.transform.localPosition = new Vector3(0,0,0);
//			o.transform.localScale= new Vector3(1,1,1);	
//			Listener = UIEventListener.Get(o);
//			Listener.onClick +=buttonClick;
//            Listener.parameter = qdata.id_;
//			maingrid.repositionNow = true;
//			Subgrid.repositionNow = true;
//		}
//		if(qlist.Count>0)
//		{
//            ShowTaskIonf(qlist[0]);
//		}
//
//
//	}

//	void buttonClick(GameObject sender)
//	{
//		int qid = (int)UIEventListener.Get (sender).parameter;
//		ShowTaskIonf (qid);
//	}


//	void InitData()
//	{
//		for (int i = 0; i < QuestSystem.AcceptableList.Count; i++)
//		{
//
//            QuestData questData_ = QuestData.GetData(QuestSystem.AcceptableList[i]);
//			
//			if(questData_.questKind_== QuestKind.QK_Main)
//			{
//                mainQuests_.Add(QuestSystem.AcceptableList[i]);
//			}
//			else
//			{
//				subQuests_.Add(QuestSystem.AcceptableList[i]);
//			}
//		}
//	}
//	void RefreshGrid()
//	{
//		mainQuests_.Clear ();
//		foreach(Transform tr in maingrid.transform)
//		{
//			if(tr.gameObject.name.Equals("zhuButton"))
//			{
//				continue;
//			}
//			Destroy(tr.gameObject);
//		}
//		subQuests_.Clear ();
//		foreach(Transform tr in Subgrid.transform)
//		{
//			if(tr.gameObject.name.Equals("ZhiButton"))
//			{
//				continue;
//			}
//			Destroy(tr.gameObject);
//		}
//	}
}
