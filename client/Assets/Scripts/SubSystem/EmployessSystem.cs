using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmployessSystem 
{
	public RequestEventHandler<int> employeeRedEnvent;
	public bool openUIEmployee;
	public bool openUIArenaPvp;
	public bool openUIEmpTask;
	public int openEmployeeType;
	public BoxType _BuyEmployeeTable_;

	private static EmployessSystem _instance;
	private Employee _curEmployee;
	public  Employee _OldEmployee;
	public bool isShowAddPos;
	public static EmployessSystem instance
	{
		get
		{
			if(_instance == null)
				_instance = new EmployessSystem();
			return _instance;
		}
	}
	
	public Employee CurEmployee
	{
		set
		{
			if(value != null)
				_curEmployee = value;
		}
		get
		{
			return _curEmployee;
		}
	}


	public void UpdateEmployeeRed()
	{
		List<Employee> employees = GamePlayer.Instance.EmployeeList;
        for (int i = 0; i < employees.Count; ++i )
        {
			uint employeeNum = employees[i].soul_;
           /* for (int j=0; j < employees.Count; ++j)
            {
                if (employees[j].Properties[(int)PropertyType.PT_TableId] == employees[i].Properties[(int)PropertyType.PT_TableId] && employees[j].quality_ == employees[i].quality_
                    && employees[j].InstId != employees[i].InstId)
                {
                    employeeNum++;
                }
            }
            */
			if((int)employees[i].quality_ > (int)QualityColor.QC_Orange )
			{
				continue;
			}


			if(((int)(employees[i].quality_)-1) >= EmployeeData.GetData(employees[i].GetIprop(PropertyType.PT_TableId)).evolutionNum.Length)
			{
				continue;
			}
            int needNum = int.Parse(EmployeeData.GetData(employees[i].GetIprop(PropertyType.PT_TableId)).evolutionNum[(int)employees[i].quality_-1]);

            if (employeeNum >= needNum)
            {
                if (employeeRedEnvent != null)
                {
                    employeeRedEnvent(employees[i].InstId);
                }
                return;
            }

        }

		if(employeeRedEnvent != null)
		{
			employeeRedEnvent(-1);
		}

	}


	public bool GetBattleEmpty()
	{
		int num = GetCanBattleNum ();
		int battNum = 0;
		if(GamePlayer.Instance.CurEmployeesBattleGroup == EmployeesBattleGroup.EBG_GroupOne )
		{
			for(int i=0;i<GamePlayer.Instance.EmployeesBattleGroup1.Length;i++)
			{
				if(GamePlayer.Instance.EmployeesBattleGroup1[i] != 0)
				{
					battNum ++;
				}
			}
		}
		else if(GamePlayer.Instance.CurEmployeesBattleGroup == EmployeesBattleGroup.EBG_GroupTwo )
		{
			for(int i=0;i<GamePlayer.Instance.EmployeesBattleGroup2.Length;i++)
			{
				if(GamePlayer.Instance.EmployeesBattleGroup2[i] != 0)
				{
					battNum ++;
				}
			}
		}

		if(GamePlayer.Instance.EmployeeList.Count >= num && num - battNum >= 1)
		{

			return true;
		}

		return false;
	}


	private int GetCanBattleNum()
	{
		int canNum = 1;
		int level = 10;
		for(int i= 1;i<4;i++)
		{
			if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level) >= level)
			{
				canNum ++;
			}
			else
			{
			}
			level += 5;
		}
	    return canNum;
	}

	public string GetQualityBack(int quality)
	{
		if ((int)quality >= (int)QualityColor.QC_Orange2)
		{
			return "cw_chongwutouxiang7";
		} 
		if((int)quality <= (int)QualityColor.QC_White)
		{
			return "cw_chongwutouxiang1";
		}
		else if ((int)quality <= (int)QualityColor.QC_Green)
		{
			return "cw_chongwutouxiang2";
		}
		else if((int)quality <= (int)QualityColor.QC_Blue1)
		{
			return "cw_chongwutouxiang3";
		}
		else if ((int)quality <= (int)QualityColor.QC_Purple2)
		{
			return "cw_chongwutouxiang4";
		}
		else if ((int)quality <= (int)QualityColor.QC_Golden2)
		{
			return "cw_chongwutouxiang5";
		}
		else if ((int)quality <= (int)QualityColor.QC_Orange2)
		{
			return "cw_chongwutouxiang6";
		}
		else if ((int)quality <= (int)QualityColor.QC_Pink)
		{
			return "cw_chongwutouxiang7";
		}
		return "";
	}
	
	public string GetTavernQualityBack(int quality)
	{
		if ((int)quality >= (int)QualityColor.QC_Orange2)
		{
			return "dikuang_zi";
		} 
		if((int)quality <= (int)QualityColor.QC_White)
		{
			return "biankuang_bai";
		}
		else if ((int)quality <= (int)QualityColor.QC_Green)
		{
			return "dikuang_lv";
		}
		else if((int)quality <= (int)QualityColor.QC_Blue1)
		{
			return "dikuang_lan";
		}
		else if ((int)quality <= (int)QualityColor.QC_Purple2)
		{
			return "dikuang_fen";
		}
		else if ((int)quality <= (int)QualityColor.QC_Golden2)
		{
			return "dikuang_cheng";
		}
		else if ((int)quality <= (int)QualityColor.QC_Orange2)
		{
			return "dikuang_huang";
		}
		else if ((int)quality <= (int)QualityColor.QC_Pink)
		{
			return "dikuang_fen";
		}
		return "";
	}


	public string GetAddQualityNUmBack(int quality)
	{
		if ((int)quality >= (int)QualityColor.QC_Pink)
		{
			return "dengji_fen";
		} 
		if((int)quality <= (int)QualityColor.QC_White)
		{
			return "";
		}
		else if ((int)quality <= (int)QualityColor.QC_Green)
		{
			return "";
		}
		else if((int)quality > (int)QualityColor.QC_Blue &&(int)quality <= (int)QualityColor.QC_Blue1)
		{
			return "dengji_lan";
		}
		else if ((int)quality > (int)QualityColor.QC_Purple && (int)quality <= (int)QualityColor.QC_Purple2)
		{
			return "dengji_zi";
		}
		else if ((int)quality > (int)QualityColor.QC_Golden &&(int)quality <= (int)QualityColor.QC_Golden2)
		{
			return "dengji_huang";
		}
		else if ((int)quality > (int)QualityColor.QC_Orange && (int)quality <= (int)QualityColor.QC_Orange2)
		{
			return "dengji_cheng";
		}
		else if ((int)quality <= (int)QualityColor.QC_Pink)
		{
			return "";
		}
		return "";
	}



	public string GetQualityYuanBack(int quality)
	{
		if ((int)quality >= (int)QualityColor.QC_Orange2)
		{
			return "shuxing_zi";
		} 
		if((int)quality <= (int)QualityColor.QC_White)
		{
			return "shuxing_bai";
		}
		else if ((int)quality <= (int)QualityColor.QC_Green)
		{
			return "shuxing_lv";
		}
		else if((int)quality <= (int)QualityColor.QC_Blue1)
		{
			return "shuxing_lan";
		}
		else if ((int)quality <= (int)QualityColor.QC_Purple2)
		{
			return "shuxing_fen";
		}
		else if ((int)quality <= (int)QualityColor.QC_Golden2)
		{
			return "shuxing_cheng";
		}
		else if ((int)quality <= (int)QualityColor.QC_Orange2)
		{
			return "shuxing_huang";
		}
		else if ((int)quality <= (int)QualityColor.QC_Pink)
		{
			return "shuxing_fen";
		}
		return "";
	}

	public string GetCellQualityBack(int quality)
	{
		if ((int)quality >= (int)QualityColor.QC_Orange2)
		{
			return "hb_renwukuang_cheng";
		} 
		if((int)quality <= (int)QualityColor.QC_White)
		{
			return "hb_renwukuang_bai";
		}
		else if ((int)quality <= (int)QualityColor.QC_Green)
		{
			return "hb_renwukuang__lv";
		}
		else if((int)quality <= (int)QualityColor.QC_Blue1)
		{
			return "hb_renwukuang_lan";
		}
		else if ((int)quality <= (int)QualityColor.QC_Purple2)
		{
			return "hb_renwukuang_zi";
		}
		else if ((int)quality <= (int)QualityColor.QC_Golden2)
		{
			return "hb_renwukuang_huang";
		}
		else if ((int)quality <= (int)QualityColor.QC_Orange2)
		{
			return "hb_renwukuang_cheng";
		}
		else if ((int)quality <= (int)QualityColor.QC_Pink)
		{
			return "hb_renwukuang_fen";
		}
		return "";
	}
}