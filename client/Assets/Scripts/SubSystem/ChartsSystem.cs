using UnityEngine;
using System.Collections;

public static class ChartsSystem {

	public delegate void QueryPlayerEvent(COM_SimplePlayerInst Inst);
	public static event QueryPlayerEvent QueryPlayerEventOk;

	public delegate void  QueryEmployeeEvent(COM_EmployeeInst Inst);
	public static  event  QueryEmployeeEvent  queryEmployeeEventOk;

	public delegate void QueryBabyEvent(COM_BabyInst Inst);
	public static  event  QueryBabyEvent  QueryBabyEventOk;

	public static void queryPlayerOK(COM_SimplePlayerInst Inst)
	{
		if(QueryPlayerEventOk != null)
		{
			QueryPlayerEventOk(Inst);
		}
	}
	public static void queryEmployeeOK(COM_EmployeeInst Inst)
	{
		if(queryEmployeeEventOk != null)
		{
			queryEmployeeEventOk(Inst);
		}
	}
	public static void queryBabyOK(COM_BabyInst Inst)
	{
		if(QueryBabyEventOk != null)
		{
			QueryBabyEventOk(Inst);
		}
	}
}
