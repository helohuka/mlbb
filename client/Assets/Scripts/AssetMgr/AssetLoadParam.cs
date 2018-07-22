using UnityEngine;
using System.Collections;

public	class BaseLoadParam
{
	public virtual void Handle()
    {
    }
}

public class ParamData
{
    public enum AssetType
    {
        AT_Player,
        AT_OtherPlayer,
        AT_Baby,
        AT_Npc,
    }

	public	int								iParam;
	public	int								iParam2;
	public	int								iParam3;
	public	int								iParam4;
    public  AssetType                       typeParam;
	public	string							szParam;
	public	string							szAssetName;
	public	Vector3							vParam;
    public  Quaternion                      qParam;
	public	BaseLoadParam					LoadParam;
	public	bool							bParam;
	public	Actor				            eEntity;
    public BattleActor                      battleActor_;
	public	LoadRequest						lrRequest;
	public	SkillInst.AttackFinishCallBack	attackfinishCallBack;
	public	COM_ReportAction				reportAction;
	public 	COM_ReportActionCounter			reportActionCounter;
	public	COM_ReportState[]				reportStates;
	public	MazeElement						mazeElement;
    public  Transform                       tTransform_;
    public  GameObject                      gObj_;
    public Destroy.FinishCallBack Callback_;
    public EffectMgr.UIEffectInfo uiEffectInfo_;
    //public UIManager.MainUILoaded uicallback_;

	public	static	ParamData	Empty
	{
		get { return new ParamData(); }
	}

    public ParamData Clone()
    {
        ParamData data = new ParamData();
        data.iParam = this.iParam;
        data.iParam2 = this.iParam2;
        data.iParam3 = this.iParam3;
        data.iParam4 = this.iParam4;
        data.szParam = this.szParam;
        data.szAssetName = this.szAssetName;
        data.vParam = this.vParam;
        data.qParam = this.qParam;
        data.LoadParam = this.LoadParam;
        data.bParam = this.bParam;
        data.eEntity = this.eEntity;
        data.lrRequest = this.lrRequest;
        data.attackfinishCallBack = this.attackfinishCallBack;
        data.reportAction = this.reportAction;
        data.reportActionCounter = this.reportActionCounter;
        data.reportStates = this.reportStates;
        data.mazeElement = this.mazeElement;
        data.tTransform_ = this.tTransform_;
        data.gObj_ = this.gObj_;
        data.Callback_ = this.Callback_;
        return data;
    }
	//
	public ParamData(int iv, MazeElement element)
	{
		iParam = iv;
		mazeElement = element;
	}
	//
	public ParamData(int iv, bool bv)
	{
		iParam = iv;
		bParam = bv;
	}
	//
	public ParamData( SkillInst.AttackFinishCallBack	callback )
	{
		attackfinishCallBack = callback;
	}
	//
	public ParamData( COM_ReportState[] states)
	{
		reportStates = states;
	}
	//
	public ParamData( COM_ReportAction action)
	{
		reportAction = action;
	}
	//
	public ParamData( COM_ReportActionCounter counter)
	{
		reportActionCounter = counter;
	}
    //
    public ParamData(Transform t)
    {
        tTransform_ = t;
    }

    public ParamData(Transform t, Destroy.FinishCallBack callback)
    {
        tTransform_ = t;
        Callback_ = callback;
    }

    public ParamData(EffectMgr.UIEffectInfo uiEffectInfo)
    {
        uiEffectInfo_ = uiEffectInfo;
    }

    public ParamData(Transform t, GameObject go)
    {
        tTransform_ = t;
        gObj_ = go;
    }
	//
	public	ParamData()
	{
		iParam		=	0;
		iParam2		=	0;
		iParam3		=	0;
		iParam4		=	0;
		szParam		=	"";
		szAssetName	=	"";
		LoadParam	=	null;
		bParam		=	false;
	}

	public ParamData( LoadRequest request )
	{
		iParam		=	0;
		iParam2		=	0;
		iParam3		=	0;
		iParam4		=	0;
		szParam		=	"";
		szAssetName	=	"";
		LoadParam	=	null;
		eEntity = null;
		lrRequest = request;
	}

	//
	public ParamData( Actor entity, int ival = 0, bool rightPos = true )
	{
        iParam      = ival;
		iParam2		=	0;
		iParam3		=	0;
		iParam4		=	0;
        bParam      = rightPos;
		szParam		=	"";
		szAssetName	=	"";
		LoadParam	=	null;
		eEntity = entity;
	}

    //
    public ParamData(BattleActor battleActor, int ival = 0, bool rightPos = true)
    {
        iParam = ival;
        iParam2 = 0;
        iParam3 = 0;
        iParam4 = 0;
        bParam = rightPos;
        szParam = "";
        szAssetName = "";
        LoadParam = null;
        battleActor_ = battleActor;
    }

	public	ParamData( int InParam )
	{
		iParam		=	InParam;
		iParam2		=	0;
		iParam3		=	0;
		iParam4		=	0;
		szParam		=	"";
		szAssetName	=	"";
		LoadParam	=	null;
		bParam		=	false;
	}
	//
	public	ParamData( int InParam, int InParam2 )
	{
		iParam		=	InParam;
		iParam2		=	InParam2;
		iParam3		=	0;
		iParam4		=	0;
		szParam		=	"";
		szAssetName	=	"";
		LoadParam	=	null;
		bParam		=	false;
	}
	
	public	ParamData( int InParam, int InParam2, int InParam3 )
	{
		iParam		=	InParam;
		iParam2		=	InParam2;
		iParam3		=	InParam3;
		iParam4		=	0;
		szParam		=	"";
		szAssetName	=	"";
		LoadParam	=	null;
		bParam		=	false;
	}

    public ParamData(int InParam, int InParam2, bool BParam)
    {
        iParam = InParam;
        iParam2 = InParam2;
        iParam3 = 0;
        iParam4 = 0;
        szParam = "";
        szAssetName = "";
        LoadParam = null;
        bParam = BParam;
    }	
	//
	public	ParamData( int InParam, int InParam2, string szInParam )
	{
		iParam		=	InParam;
		iParam2		=	InParam2;
		iParam3		=	0;
		iParam4		=	0;
		szParam		=	szInParam;
		szAssetName	=	"";
		LoadParam	=	null;
		bParam		=	false;
	}
	//
	public	ParamData( string szInParam )
	{
		iParam		=	0;
		iParam2		=	0;
		iParam3		=	0;
		iParam4		=	0;
		szParam		=	szInParam;
		szAssetName	=	"";
		LoadParam	=	null;
		bParam		=	false;
	}
	//
	public	ParamData( int InParam, string szInParam )
	{
		iParam		=	InParam;
		iParam2		=	0;
		iParam3		=	0;
		iParam4		=	0;
		szParam		=	szInParam;
		szAssetName	=	"";
		LoadParam	=	null;
		bParam		=	false;
	}
	//
	public	ParamData( int InParam, Vector3 vecParam, Quaternion quaParam, bool ignoreId)
	{
		iParam		=	InParam;
		vParam		=	vecParam;
        qParam      =   quaParam;
        bParam      =   ignoreId;
	}
	//
	public	ParamData( BaseLoadParam BaseParam )
	{
		iParam		=	0;
		iParam2		=	0;
		iParam3		=	0;
		iParam4		=	0;
		szParam		=	"";
		szAssetName	=	"";
		LoadParam	=	BaseParam;
		bParam		=	false;
	}
	//
	public	ParamData( int InParam, BaseLoadParam BaseParam )
	{
		iParam		=	InParam;
		iParam2		=	0;
		iParam3		=	0;
		iParam4		=	0;
		szParam		=	"";
		szAssetName	=	"";
		LoadParam	=	BaseParam;
		bParam		=	false;
	}
	//
	public	ParamData( int InParam, int InParam2, BaseLoadParam BaseParam )
	{
		iParam		=	InParam;
		iParam2		=	InParam2;
		iParam3		=	0;
		iParam4		=	0;
		szParam		=	"";
		szAssetName	=	"";
		LoadParam	=	BaseParam;
		bParam		=	false;
	}
	//
	public	ParamData( int InParam, int InParam2, int InParam3, BaseLoadParam BaseParam )
	{
		iParam		=	InParam;
		iParam2		=	InParam2;
		iParam3		=	InParam3;
		iParam4		=	0;
		szParam		=	"";
		szAssetName	=	"";
		LoadParam	=	BaseParam;
		bParam		=	false;
	}	
	//
	public	ParamData( int InParam, int InParam2, int InParam3, int InParam4, BaseLoadParam BaseParam )
	{
		iParam		=	InParam;
		iParam2		=	InParam2;
		iParam3		=	InParam3;
		iParam4		=	InParam4;
		szParam		=	"";
		szAssetName	=	"";
		LoadParam	=	BaseParam;
		bParam		=	false;
	}
	//
	public	ParamData( int InParam, int InParam2, int InParam3, int InParam4, string szInParam, BaseLoadParam BaseParam )
	{
		iParam		=	InParam;
		iParam2		=	InParam2;
		iParam3		=	InParam3;
		iParam4		=	InParam4;
		szParam		=	szInParam;
		szAssetName	=	"";
		LoadParam	=	BaseParam;
		bParam		=	false;
	}
	//
	public	ParamData( string szInParam, BaseLoadParam BaseParam )
	{
		iParam		=	0;
		iParam2		=	0;
		iParam3		=	0;
		iParam4		=	0;
		szParam		=	szInParam;
		szAssetName	=	"";
		LoadParam	=	BaseParam;
		bParam		=	false;
	}
	//
	public	ParamData( int InParam, string szInParam, BaseLoadParam BaseParam )
	{
		iParam		=	InParam;
		iParam2		=	0;
		iParam3		=	0;
		iParam4		=	0;
		szParam		=	szInParam;
		szAssetName	=	"";
		LoadParam	=	BaseParam;
		bParam		=	false;
	}
	//
	public	ParamData( int InParam, int InParam2, string szInParam, BaseLoadParam BaseParam )
	{
		iParam		=	InParam;
		iParam2		=	InParam2;
		iParam3		=	0;
		iParam4		=	0;
		szParam		=	szInParam;
		szAssetName	=	"";
		LoadParam	=	BaseParam;
		bParam		=	false;
	}
	//
	public	ParamData( bool boolParam )
	{
		iParam		=	0;
		iParam2		=	0;
		iParam3		=	0;
		iParam4		=	0;
		szParam		=	"";
		szAssetName	=	"";
		LoadParam	=	null;
		bParam		=	boolParam;
	}
	//
	public	ParamData( int InParam, int InParam2, string szInParam, bool boolParam )
	{
		iParam		=	InParam;
		iParam2		=	InParam2;
		iParam3		=	0;
		iParam4		=	0;
		szParam		=	szInParam;
		szAssetName	=	"";
		LoadParam	=	null;
		bParam		=	boolParam;
	}
	//
	public	ParamData( int InParam, int InParam2, string szInParam, bool boolParam, BaseLoadParam BaseParam )
	{
		iParam		=	InParam;
		iParam2		=	InParam2;
		iParam3		=	0;
		iParam4		=	0;
		szParam		=	szInParam;
		szAssetName	=	"";
		LoadParam	=	BaseParam;
		bParam		=	boolParam;
	}

    public ParamData(AssetType param1, int param2, int param3, Vector3 param4, Quaternion param5)
    {
        typeParam = param1;
        iParam = param2;
        iParam2 = param3;
        vParam = param4;
        qParam = param5;
    }
}enum APPLE_919065d9276d4a618c5f2a4628b3e749
{

}