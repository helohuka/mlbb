using UnityEngine;
using System.Collections;

public class GemSystem 
{
	public RequestEventHandler<COM_CrystalData> crystalEnvent;
	public RequestEventHandler<bool> crystalUpLevelEnvent;
	public RequestEventHandler<bool> resetCrystalPropOKEnvent;

	private COM_CrystalData _crystalData;
	private static GemSystem _instance;
	public static GemSystem instance
	{
		get
		{
			if(_instance == null)
				_instance = new GemSystem();
			return _instance;
		}
	}

	public void sycnCrystal(COM_CrystalData data)
	{
		CrystalData = data;
		if(crystalEnvent != null)
		{
			crystalEnvent(data);
		}
	}
	
	public void crystalUpLeveResult(bool b)
	{
		if (crystalUpLevelEnvent != null)
			crystalUpLevelEnvent (b);
	}

	public void resetCrystalPropOK()
	{
		if (resetCrystalPropOKEnvent != null)
			 resetCrystalPropOKEnvent (true);
	}

	public COM_CrystalData CrystalData
	{
		set
		{
			if(value == null)
				return;

			_crystalData = value;

		}
		get
		{
			return _crystalData;
		}
	}
}

