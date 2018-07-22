using System;
using UniLua;

public class LuaMaster {

	static public LuaMaster Instance = new LuaMaster();

	public LuaMaster()
	{}

	string luaEntryFile_ = "lua/main.lua";

	ILuaState lua_;
	public ILuaState Lua
	{ get { return lua_; } }

	public void Init()
	{
		lua_ = LuaAPI.NewState ();
		lua_.L_OpenLibs ();
		lua_.L_RequireF(GameScript.LIB_NAME, GameScript.OpenLib, false);
        lua_.L_RequireF(Sys.LIB_NAME, Sys.OpenLib, true);
        lua_.L_RequireF(Global_.LIB_NAME, Global_.OpenLib, true);
#region registGlobal

		GameScript.RegistGlobalEnum (typeof(ScriptGameEvent));
        GameScript.RegistGlobalEnum(typeof(GuidePointerRotateType));
		GameScript.RegistGlobalEnum (typeof(BattlePosition));
		GameScript.RegistGlobalEnum (typeof(PropertyType));
		GameScript.RegistGlobalEnum (typeof(OrderParamType));
		GameScript.RegistGlobalEnum (typeof(GroupType));
		GameScript.RegistGlobalEnum (typeof(StateType));
		GameScript.RegistGlobalEnum (typeof(ItemMainType));
        GameScript.RegistGlobalEnum(typeof(ItemSubType));
		GameScript.RegistGlobalEnum (typeof(TogetherStateType));
		GameScript.RegistGlobalEnum (typeof(StateType));
        GameScript.RegistGlobalEnum (typeof(GuideAimType));
        GameScript.RegistGlobalEnum(typeof(WeaponType));
        GameScript.RegistGlobalEnum(typeof(JobType));
        GameScript.RegistGlobalEnum(typeof(Constant));
        GameScript.RegistGlobalEnum(typeof(BattleJudgeType));
        GameScript.RegistGlobalEnum(typeof(SenseActorType));
#endregion

		LoadScript (luaEntryFile_);
	}

	public void LoadScript(string file)
	{
		ThreadStatus status = lua_.L_DoFile (file);
		if(status != ThreadStatus.LUA_OK)
		{
			throw new Exception( lua_.ToString(-1) );
		}
		
		lua_.Pop (1);
	}
}