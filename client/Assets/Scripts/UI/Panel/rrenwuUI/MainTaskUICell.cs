using UnityEngine;
using System.Collections;

public class MainTaskUICell : MonoBehaviour {

	public UILabel targetLabel;
	public UILabel descLabel;
	public UILabel QusetKindLabel;
	//public UILabel numlabel;
	public UISprite stateSp;
	private COM_QuestInst _questInst;
	public COM_QuestInst QuestInst
	{
		set
		{
			if(value != null)
			{
				_questInst = value;
				QuestData qdata = QuestData.GetData((int)_questInst.questId_);           
				if (QuestSystem.IsQuestFinish(qdata.id_))
				{
					
					if(qdata.questKind_ == QuestKind.QK_Main)
					{
						stateSp.spriteName = "zhuxian";
						//Tasks[i].text =string.Format("{0}    {1}",LanguageManager.instance.GetValue("mainQuset"),qdata.questName_+"\n"+qdata.miniFinDesc_);
						QusetKindLabel.text = LanguageManager.instance.GetValue("mainQuset");
						targetLabel.text = string.Format("{0}",qdata.questName_);
						descLabel.text = qdata.miniFinDesc_;
					}
					else//(qdata.questKind_ == QuestKind.QK_Sub)
					{
						if(qdata.questKind_ == QuestKind.QK_Profession)
						{
							stateSp.spriteName = "zhiye";
							//Tasks[i].text =string.Format("{0}    {1}",LanguageManager.instance.GetValue("QK_Profession"),qdata.questName_+"\n"+qdata.miniFinDesc_);
							QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Profession");
							targetLabel.text =string.Format("{0}",qdata.questName_);
							descLabel.text = qdata.miniFinDesc_;
						}else
							if(qdata.questKind_ == QuestKind.QK_Tongji)
						{
							stateSp.spriteName = "zhixian";
							//Tasks[i].text =string.Format("{0}    {1}",LanguageManager.instance.GetValue("QK_Tongji"),qdata.questName_+"\n"+qdata.miniFinDesc_);
							QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Tongji");
							targetLabel.text = string.Format("{0}",qdata.questName_);
							descLabel.text = qdata.miniFinDesc_;
						}else
							if(qdata.questKind_ == QuestKind.QK_Sub)
						{
							stateSp.spriteName = "zhixian";
							QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Sub");
							//Tasks[i].text =string.Format("{0}      {1}",LanguageManager.instance.GetValue("QK_Sub"),qdata.questName_+"\n"+qdata.miniFinDesc_);
							targetLabel.text = string.Format("{0}",qdata.questName_);
							descLabel.text = qdata.miniFinDesc_;
						}else
						if(qdata.questKind_ == QuestKind.QK_Wishing)
						{
							stateSp.spriteName = "zhixian";
							QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Wishing");
							//Tasks[i].text =string.Format("{0}      {1}",LanguageManager.instance.GetValue("QK_Sub"),qdata.questName_+"\n"+qdata.miniFinDesc_);
							targetLabel.text = string.Format("{0}",qdata.questName_);
							descLabel.text = qdata.miniFinDesc_;
						}
						else
							if(qdata.questKind_ == QuestKind.QK_Copy)
						{
							stateSp.spriteName = "zhixian";
							QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Copy");
							//Tasks[i].text =string.Format("{0}      {1}",LanguageManager.instance.GetValue("QK_Sub"),qdata.questName_+"\n"+qdata.miniFinDesc_);
							targetLabel.text = string.Format("{0}",qdata.questName_);
							descLabel.text = qdata.miniFinDesc_;
						}else
							if(qdata.questKind_ == QuestKind.QK_Rand)
						{
							stateSp.spriteName = "zhixian";
							QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Rand");
							targetLabel.text = string.Format("{0}",qdata.questName_);

							int randMaxcount = 0;
							GlobalValue.Get(Constant.C_AccecptRandQuestLimit, out randMaxcount);
							descLabel.text =qdata.miniFinDesc_+"("+QuestSystem.randCount+"/"+randMaxcount+")";
							 
						}
						else if(qdata.questKind_ == QuestKind.QK_Sub1)
						{
							stateSp.spriteName = "huodong";
							QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Sub1");
							targetLabel.text = string.Format("{0}",qdata.questName_);
							descLabel.text = qdata.miniFinDesc_;
						}else
						{
							stateSp.spriteName = "zhixian";
							QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Daily");
							targetLabel.text = string.Format("{0}",qdata.questName_);
							descLabel.text = qdata.miniFinDesc_;
						}
						
					}
					
				}
				
				else
				{
					if(qdata.questKind_ == QuestKind.QK_Main)
					{
						stateSp.spriteName = "zhuxian";
						QusetKindLabel.text = LanguageManager.instance.GetValue("mainQuset");
						//Tasks[i].text = string.Format("{0}     {1}({2}/{3})",LanguageManager.instance.GetValue("mainQuset") ,qdata.+"\n"+qdata.miniDesc_, qlist[i].questNum_, qdata.targetNum_);
						targetLabel.text = string.Format("{0}",qdata.questName_);
						string []strs = qdata.miniDesc_.Split(':');
						string text = "";
//						if(strs.Length == 1)
//						{
//
//							descLabel.text = string.Format("{0}[fbf21c]({1}/{2})[-]",qdata.miniDesc_, _questInst.targets_[0].targetNum_, qdata.targetNum_);
//						}else
//						{
							for(int i =0;i<_questInst.targets_.Length;i++)
							{
								//QuestData qd = QuestData.GetData((int)_questInst.targets_[i].targetId_);
								text += strs[i]+"("+_questInst.targets_[i].targetNum_+"/"+qdata.targetNum_[i]+")";
							}
							descLabel.text =text;
//						}

						//descLabel.text = string.Format("{0}[fbf21c]({1}/{2})[-]",qdata.miniDesc_, _questInst.targets_[i].targetNum_, qdata.targetNum_);
						//numlabel.text = "("+_questInst.questNum_+"/"+ qdata.targetNum_+")";
					}
					else
					{
						if(qdata.questKind_ == QuestKind.QK_Profession)
						{
							stateSp.spriteName = "zhiye";
							QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Profession");
							//Tasks[i].text = string.Format("{0}    {1}({2}/{3})",LanguageManager.instance.GetValue("QK_Profession") ,qdata.questName_+"\n"+qdata.miniDesc_, qlist[i].questNum_, qdata.targetNum_);
							targetLabel.text = string.Format("{0}" ,qdata.questName_);

							string []strs = qdata.miniDesc_.Split(':');
							string text = "";
//							if(strs.Length == 1)
//							{
//								descLabel.text = string.Format("{0}[fbf21c]({1}/{2})[-]",qdata.miniDesc_, _questInst.targets_[0].targetNum_, qdata.targetNum_);
//							}else
//							{
								for(int i =0;i<_questInst.targets_.Length;i++)
								{
									//QuestData qd = QuestData.GetData((int)_questInst.targets_[i].targetId_);
									text += strs[i]+"("+_questInst.targets_[i].targetNum_+"/"+qdata.targetNum_[i]+")";
								}
								descLabel.text =text;
//							}


							//descLabel.text = string.Format("{0}[fbf21c]({1}/{2})[-]",qdata.miniDesc_, _questInst.questNum_, qdata.targetNum_);
							//numlabel.text = "("+_questInst.questNum_+"/"+ qdata.targetNum_+")";
						}else
							if(qdata.questKind_ == QuestKind.QK_Tongji)
						{
							stateSp.spriteName = "zhixian";
							QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Tongji");
							//Tasks[i].text = string.Format("{0}    {1}({2}/{3})",LanguageManager.instance.GetValue("QK_Tongji") ,qdata.questName_+"\n"+qdata.miniDesc_, qlist[i].questNum_, qdata.targetNum_);
							targetLabel.text = string.Format("{0}" ,qdata.questName_);

							string []strs = qdata.miniDesc_.Split(':');
							string text = "";
//							if(strs.Length == 0)
//							{
//								descLabel.text = string.Format("{0}[fbf21c]({1}/{2})[-]",qdata.miniDesc_, _questInst.targets_[0].targetNum_, qdata.targetNum_);
//							}else
//							{
								for(int i =0;i<_questInst.targets_.Length;i++)
								{
									
									//QuestData qd = QuestData.GetData((int)_questInst.targets_[i].targetId_);
									text += strs[i]+"("+_questInst.targets_[i].targetNum_+"/"+qdata.targetNum_[i]+")";
								}
								descLabel.text =text;

//							}

							//descLabel.text = string.Format("{0}[fbf21c]({1}/{2})[-]",qdata.miniDesc_, _questInst.questNum_, qdata.targetNum_);
							//numlabel.text = "("+_questInst.questNum_+"/"+ qdata.targetNum_+")";
						}else
							if(qdata.questKind_ == QuestKind.QK_Sub)
						{
							stateSp.spriteName = "zhixian";
							QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Sub");
							//Tasks[i].text =string.Format("{0}    {1}",LanguageManager.instance.GetValue("QK_Sub"),qdata.questName_+"\n"+qdata.miniFinDesc_);
							targetLabel.text = string.Format("{0}",qdata.questName_);

							string []strs = qdata.miniDesc_.Split(':');
							string text = "";
//							if(strs.Length == 0)
//							{
//								descLabel.text = string.Format("{0}[fbf21c]({1}/{2})[-]",qdata.miniDesc_, _questInst.targets_[0].targetNum_, qdata.targetNum_);
//							}else
//							{
								for(int i =0;i<_questInst.targets_.Length;i++)
								{
									//QuestData qd = QuestData.GetData((int)_questInst.targets_[i].targetId_);
									text += strs[i]+"("+_questInst.targets_[i].targetNum_+"/"+qdata.targetNum_[i]+")";
								}
								descLabel.text =text;
//							}


						}
						else
							if(qdata.questKind_ == QuestKind.QK_Wishing)
						{
							stateSp.spriteName = "zhixian";
							QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Wishing");
							//Tasks[i].text =string.Format("{0}    {1}",LanguageManager.instance.GetValue("QK_Sub"),qdata.questName_+"\n"+qdata.miniFinDesc_);
							targetLabel.text = string.Format("{0}",qdata.questName_);
							
							string []strs = qdata.miniDesc_.Split(':');
							string text = "";
							//							if(strs.Length == 0)
							//							{
							//								descLabel.text = string.Format("{0}[fbf21c]({1}/{2})[-]",qdata.miniDesc_, _questInst.targets_[0].targetNum_, qdata.targetNum_);
							//							}else
							//							{
							for(int i =0;i<_questInst.targets_.Length;i++)
							{
								//QuestData qd = QuestData.GetData((int)_questInst.targets_[i].targetId_);
								text += strs[i]+"("+_questInst.targets_[i].targetNum_+"/"+qdata.targetNum_[i]+")";
							}
							descLabel.text =text;
							//							}
							
							
						}else
							if(qdata.questKind_ == QuestKind.QK_Copy)
						{
							stateSp.spriteName = "zhixian";
							QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Copy");
							//Tasks[i].text =string.Format("{0}    {1}",LanguageManager.instance.GetValue("QK_Sub"),qdata.questName_+"\n"+qdata.miniFinDesc_);
							targetLabel.text = string.Format("{0}",qdata.questName_);
							
							string []strs = qdata.miniDesc_.Split(':');
							string text = "";
							//							if(strs.Length == 0)
							//							{
							//								descLabel.text = string.Format("{0}[fbf21c]({1}/{2})[-]",qdata.miniDesc_, _questInst.targets_[0].targetNum_, qdata.targetNum_);
							//							}else
							//							{
							for(int i =0;i<_questInst.targets_.Length;i++)
							{
								//QuestData qd = QuestData.GetData((int)_questInst.targets_[i].targetId_);
								text += strs[i]+"("+_questInst.targets_[i].targetNum_+"/"+qdata.targetNum_[i]+")";
							}
							descLabel.text =text;
							//							}
							
							
						}
						else
							if(qdata.questKind_ == QuestKind.QK_Rand)
						{
							stateSp.spriteName = "zhixian";
							QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Rand");
							targetLabel.text = string.Format("{0}",qdata.questName_);							
							string []strs = qdata.miniDesc_.Split(':');
							string text = "";
							int randMaxcount = 0;
							GlobalValue.Get(Constant.C_AccecptRandQuestLimit, out randMaxcount);

							for(int i =0;i<_questInst.targets_.Length;i++)
							{
								//QuestData qd = QuestData.GetData((int)_questInst.targets_[i].targetId_);
								text += strs[i]+"("+_questInst.targets_[i].targetNum_+"/"+qdata.targetNum_[i]+")";
							}
							descLabel.text = text +"("+QuestSystem.randCount+"/"+randMaxcount+")";
						}
						else if(qdata.questKind_ == QuestKind.QK_Sub1)
						{
							stateSp.spriteName = "huodong";
							QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Sub1");
							targetLabel.text = string.Format("{0}",qdata.questName_);
							descLabel.text = qdata.miniDesc_;
						}
						else
						{
							stateSp.spriteName = "zhixian";
							QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Daily");
							targetLabel.text = string.Format("{0}",qdata.questName_);

							string []strs = qdata.miniDesc_.Split(':');
							string text = "";
//							if(strs.Length == 0)
//							{
//								descLabel.text = string.Format("{0}[fbf21c]({1}/{2})[-]",qdata.miniDesc_, _questInst.targets_[0].targetNum_, qdata.targetNum_);
//							}else
//							{
								for(int i =0;i<_questInst.targets_.Length;i++)
								{
									//QuestData qd = QuestData.GetData((int)_questInst.targets_[i].targetId_);
									text += strs[i]+"("+_questInst.targets_[i].targetNum_+"/"+qdata.targetNum_[i]+")";
								}
								descLabel.text =text;
//							}




							//descLabel.text = string.Format("{0}[fbf21c]({1}/{2})[-]",qdata.miniDesc_, _questInst.questNum_, qdata.targetNum_);
						}
						
					}
				}
			}
		}
		get
		{
			return _questInst;
		}
	}

    public QuestData QData
    {
        set
        {
            if (value.questKind_ == QuestKind.QK_Main)
			{
				stateSp.spriteName = "zhuxian";
				QusetKindLabel.text = LanguageManager.instance.GetValue("mainQuset");
				//Tasks[i].text = string.Format("{0}     {1}({2}/{3})",LanguageManager.instance.GetValue("mainQuset") ,qdata.questName_+"\n"+qdata.miniDesc_, qlist[i].questNum_, qdata.targetNum_);
                targetLabel.text = string.Format("{0}", value.questName_);

                descLabel.text = StringTool.MakeNGUIStringQuestNPC(value.xunlu);
			}
			else if (value.questKind_ == QuestKind.QK_Profession)
			{
				stateSp.spriteName = "zhiye";
				QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Profession");
				//Tasks[i].text = string.Format("{0}    {1}({2}/{3})",LanguageManager.instance.GetValue("QK_Profession") ,qdata.questName_+"\n"+qdata.miniDesc_, qlist[i].questNum_, qdata.targetNum_);
                targetLabel.text = string.Format("{0}", value.questName_);
                descLabel.text = StringTool.MakeNGUIStringQuestNPC(value.xunlu);
			}
            else if (value.questKind_ == QuestKind.QK_Tongji)
			{
				stateSp.spriteName = "zhixian";
				QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Tongji");
				//Tasks[i].text = string.Format("{0}    {1}({2}/{3})",LanguageManager.instance.GetValue("QK_Tongji") ,qdata.questName_+"\n"+qdata.miniDesc_, qlist[i].questNum_, qdata.targetNum_);
                targetLabel.text = string.Format("{0}", value.questName_);
                descLabel.text = StringTool.MakeNGUIStringQuestNPC(value.xunlu);
			}
            else if (value.questKind_ == QuestKind.QK_Sub)
			{
				stateSp.spriteName = "zhixian";
				QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Sub");
				//Tasks[i].text =string.Format("{0}    {1}",LanguageManager.instance.GetValue("QK_Sub"),qdata.questName_+"\n"+qdata.miniFinDesc_);
                targetLabel.text = string.Format("{0}",  value.questName_);
                descLabel.text = StringTool.MakeNGUIStringQuestNPC(value.xunlu);
			}
			else if (value.questKind_ == QuestKind.QK_Rand)
			{
				stateSp.spriteName = "zhixian";
				QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Rand");
				targetLabel.text = string.Format("{0}",  value.questName_);
				descLabel.text = StringTool.MakeNGUIStringQuestNPC(value.xunlu);
			}
			else if(value.questKind_ == QuestKind.QK_Sub1)
			{
				stateSp.spriteName = "huodong";
				QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Sub1");
				targetLabel.text = string.Format("{0}",  value.questName_);
				descLabel.text = StringTool.MakeNGUIStringQuestNPC(value.xunlu);
			}

            else
			{
				stateSp.spriteName = "zhixian";
				QusetKindLabel.text = LanguageManager.instance.GetValue("QK_Daily");
                targetLabel.text = string.Format("{0}", value.questName_);
                descLabel.text = StringTool.MakeNGUIStringQuestNPC(value.xunlu);
			}
            //QuestData a = value;
			
		}
		
    }

	void Start () {
	
	}

}
