local gameLib = require "GameLib.cs"

gameLib.LoadScript("lua/global.lua");
gameLib.LoadScript("lua/guide.lua");
gameLib.LoadScript("lua/talk.lua");
gameLib.LoadScript("lua/hugeUI.lua");

gameLib.RegistEvent(SGE_TogetherState, "CheckHejiState");
gameLib.RegistEvent(SGE_CheckState, "CheckState");
gameLib.RegistEvent(SGE_UseItemOk, "UseItemOk");
gameLib.RegistEvent(SGE_CheckBuff, "CheckBuff");
gameLib.RegistEvent(SGE_NpcTalked, "NpcTalked");
gameLib.RegistEvent(SGE_EnterCopy, "EnterCopy");



gameLib.PushBattleScene("MazeBattleScene");
gameLib.PushBattleScene("HaiDiBattle");
gameLib.PushBattleScene("AttackScene");
gameLib.PushBattleScene("CunZhuang2");
gameLib.PushBattleScene("Cloister_zhandou");
gameLib.PushBattleScene("Ha_zhandou");
gameLib.PushBattleScene("Swamp_zhandou");
gameLib.PushBattleScene("Desert_zhandou");
gameLib.PushBattleScene("Mine_zhandou");
gameLib.PushBattleScene("Snow_zhandou");
gameLib.PushBattleScene("MagmaScene_zhandou");
gameLib.PushBattleScene("Desert_zhandou");
gameLib.PushBattleScene("Snow_zhandou");
gameLib.PushBattleScene("50jifuben_zhandou");
gameLib.PushBattleScene("chongdong_zhandou");
gameLib.PushBattleScene("buluofengyinzhidi_zhandou");
gameLib.PushBattleScene("xuebaomidao_zhandou");
gameLib.PushBattleScene("buluochangjing_zhandou");
gameLib.PushBattleScene("buluoxianzhuzhidi_zhandou");
gameLib.PushBattleScene("70jifuben_zhandou");

gameLib.PushFBScene("MainScene");
gameLib.PushFBScene("Ha_Maze");
gameLib.PushFBScene("Ha_Maze1");
gameLib.PushFBScene("Mine");
gameLib.PushFBScene("laboratoryRoom");
gameLib.PushFBScene("laboratoryRoom1");
gameLib.PushFBScene("Swamp");
gameLib.PushFBScene("Swamp1");
gameLib.PushFBScene("Submarine");
gameLib.PushFBScene("Submarine1");
gameLib.PushFBScene("Arena1");
gameLib.PushFBScene("Pub");
gameLib.PushFBScene("Snow");
gameLib.PushFBScene("MagmaScenario");
gameLib.PushFBScene("PalaceScene");
gameLib.PushFBScene("kanniuchang");
gameLib.PushFBScene("jixueshanlu");
gameLib.PushFBScene("SonwPalaceScene");
gameLib.PushFBScene("jiazhupk");
gameLib.PushFBScene("MainScene02");
gameLib.PushFBScene("Desert");
gameLib.PushFBScene("Desert1");
gameLib.PushFBScene("Desert_Maze");
gameLib.PushFBScene("Desert_Maze1");
gameLib.PushFBScene("Desert_Maze2");
gameLib.PushFBScene("Desert_Temple");
gameLib.PushFBScene("Snow_fuben");
gameLib.PushFBScene("Snow_kaitou");
gameLib.PushFBScene("SonwPalaceScene");
gameLib.PushFBScene("50jifuben");
gameLib.PushFBScene("60jifuben");
gameLib.PushFBScene("chongdong");
gameLib.PushFBScene("buluofengyinzhidi");
gameLib.PushFBScene("buluoxianzhuzhidi");
gameLib.PushFBScene("xuebaomidao");
gameLib.PushFBScene("buluochangjing");
gameLib.PushFBScene("70jifuben");


function GameMain(GAMEEVENT, ARG0, ARG1, ARG2, ARG3, ARG4, ARG5, ARG6, ARG7, ARG8, ARG9)
	return "haha"
end


--强力
function CalcSkillAffect_qiangli(GAMEEVENT, ARG0, ARG1)
	local pos_table = {ARG1+1, ARG1-1, ARG1+5, ARG1-5, ARG1}	
	--如果是边缘单位，删除不必要的
	if ARG1 == BP_Up0 or ARG1 == BP_Down0 then
		table.remove(pos_table,2)
		table.remove(pos_table,3)
	elseif ARG1 == BP_Up5 or ARG1 == BP_Down5 then
		table.remove(pos_table,2)
		table.remove(pos_table,2)
	elseif ARG1 == BP_Up4 or ARG1 == BP_Down4 then	
		table.remove(pos_table,1)
		table.remove(pos_table,3)
	elseif ARG1 == BP_Up9 or ARG1 == BP_Down9 then	
		table.remove(pos_table,1)
		table.remove(pos_table,2)
	end
	
	--删除无效的单位
	local a = 0;
	local b = 0;
	if ARG1 <= 10 and ARG1 >= 1 then
		a = 10;
		b = 1;
	elseif ARG1 <= 20 and ARG1 >= 11 then
		a = 20;
		b = 11;
	else
		return false;
	end
	
	for i=1,table.getn(pos_table),1
		do

			if pos_table[i] >= b and pos_table[i] <= a then
				gameLib.PushSkillAffectIndex(pos_table[i])
			end
	end
end


--全体位置
function CalcSkillAffect_quanti(GAMEEVENT, ARG0, ARG1)
	if ARG1 <= 10 and ARG1 >= 1 then
		for i=1,10,1
			do
				gameLib.PushSkillAffectIndex(i)
			end
	elseif ARG1 <= 20 and ARG1 >= 11 then
		for i=1,10,1
			do
				gameLib.PushSkillAffectIndex(i+10)
			end
	else
		return false;
	end
end


--石化昏睡状态不能操作技能
function CheckState(GAMEEVENT, ARG0)
	--ARG0 is table	
	--eg. 禁锢 禁止玩家本回合操作
	if(ARG0 == nil) then
		return
	end
	for i=1,table.getn(ARG0),1 
		do
			if ARG0[i] == ST_Basilisk or ARG0[i] == ST_Sleep then
				gameLib.DoSkill(12);
			end
		end	
end

--合击的时候播放掉血判断
function CheckHejiState(GAMEEVENT, ARG0)
	if(ARG0 == nil) then
		return TST_None;
	end
	for i=1,table.getn(ARG0),1 
		do
			if ARG0[i] == ST_ActionBounce then
				return TST_Enemy;
			elseif ARG0[i] == ST_ActionAbsorb then
				return TST_Self;
			end
		end	
	return TST_None;
end

function PlayerSelectSkill(GAMEEVENT,ARG0,ARG1)
	for i = 1, table.getn(ARG0), 1
		do
			if ARG0[i] == 1001 or ARG0[i] == 1021 or ARG0[i] == 1031 or ARG0[i] == 1081 then
				if ARG1 == WT_Bow or ARG1 == WT_Knife then
					gameLib.BanSkill(i-1)
				end
			elseif ARG0[i] == 1041 then
				if ARG1 ~= WT_Bow then
					gameLib.BanSkill(i-1)
				end
			elseif ARG0[i] == 1051 then
				if ARG1 ~= WT_None then
					gameLib.BanSkill(i-1)
				end
			elseif ARG0[i] == 1061 or ARG0[i] == 1011 then
				if ARG1 == WT_Knife then
					gameLib.BanSkill(i-1)
				end
			end
	end
	
end



--使用道具
function UseItemOk(GAMEEVENT,ARG0,ARG1)
	if ARG0 == 5037 then
		gameLib.PopText("useHpBuff")
	end
	if ARG0 == 5038 then
		gameLib.PopText("useMpBuff")
	end
	
	if ARG0 >= 3901 and ARG0 <= 3905 then
		if ARG0 == 3901 then
			gameLib.PopText("liliangzhongzi",ARG1)
		elseif ARG0 == 3902 then
			gameLib.PopText("tilizhongzi",ARG1)
		elseif ARG0 == 3903 then
			gameLib.PopText("suduzhongzi",ARG1)
		elseif ARG0 == 3904 then
			gameLib.PopText("qiangduzhongzi",ARG1)
		elseif ARG0 == 3905 then
			gameLib.PopText("mofazhongzi",ARG1)
		end
	end
	
	if ARG0 >= 1759 and ARG0 <= 1766 then
		if ARG0 == 1759 then
			ARG1 = ARG1 * 5000;
			gameLib.PopText("baby_exp500",ARG1)
		elseif ARG0 == 1760 then
			ARG1 = ARG1 * 10000;
			gameLib.PopText("baby_exp500",ARG1)
		elseif ARG0 == 1761 then
			ARG1 = ARG1 * 20000;
			gameLib.PopText("baby_exp500",ARG1)
		elseif ARG0 == 1762 then
			ARG1 = ARG1 * 100000;
			gameLib.PopText("baby_exp500",ARG1)
		elseif ARG0 == 1763 then
			ARG1 = ARG1 * 200000;
			gameLib.PopText("baby_exp500",ARG1)
		elseif ARG0 == 1764 then
			ARG1 = ARG1 * 500000;
			gameLib.PopText("baby_exp500",ARG1)
		elseif ARG0 == 1765 then
			ARG1 = ARG1 * 1000000;
			gameLib.PopText("baby_exp500",ARG1)
		elseif ARG0 == 1766 then
			ARG1 = ARG1 * 2000000;
			gameLib.PopText("baby_exp500",ARG1)
		end
	end
	if ARG0 >= 1716 and ARG0 <= 1719 then
		if ARG0 == 1716 then
			ARG1 = ARG1 * 1;
			gameLib.PopText("shuangbeishijian",ARG1)
		elseif ARG0 == 1717 then
			ARG1 = ARG1 * 2;
			gameLib.PopText("shuangbeishijian",ARG1)
		elseif ARG0 == 1718 then
			ARG1 = ARG1 * 3;
			gameLib.PopText("shuangbeishijian",ARG1)
		elseif ARG0 == 1719 then
			ARG1 = ARG1 * 6;
			gameLib.PopText("shuangbeishijian",ARG1)
		end
	end
	if ARG0 == 5051 then
		gameLib.ShowGainBaby(9)
	end
	if ARG0 == 5052 then
		gameLib.ShowGainBaby(17)
	end
	if ARG0 == 5053 then
		gameLib.ShowGainBaby(20)
	end
	if ARG0 == 5054 then
		gameLib.ShowGainBaby(26)
	end
	if ARG0 == 5055 then
		gameLib.ShowGainBaby(27)
	end
	if ARG0 == 5056 then
		gameLib.ShowGainBaby(28)
	end
	if ARG0 == 5057 then
		gameLib.ShowGainBaby(30)
	end
	if ARG0 == 5058 then
		gameLib.ShowGainBaby(33)
	end
	if ARG0 == 5059 then
		gameLib.ShowGainBaby(34)
	end
	if ARG0 == 5060 then
		gameLib.ShowGainBaby(45)
	end
	if ARG0 == 5061 then
		gameLib.ShowGainBaby(49)
	end
	if ARG0 == 5062 then
		gameLib.ShowGainBaby(51)
	end
	if ARG0 == 5063 then
		gameLib.ShowGainBaby(61)
	end
	if ARG0 == 5064 then
		gameLib.ShowGainBaby(62)
	end
	if ARG0 == 5065 then
		gameLib.ShowGainBaby(65)
	end
	if ARG0 == 5066 then
		gameLib.ShowGainBaby(67)
	end
	if ARG0 == 5067 then
		gameLib.ShowGainBaby(76)
	end
	if ARG0 == 5068 then
		gameLib.ShowGainBaby(77)
	end
	if ARG0 == 5069 then
		gameLib.ShowGainBaby(30003)
	end
	if ARG0 == 5070 then
		gameLib.ShowGainBaby(30007)
	end
	if ARG0 == 5071 then
		gameLib.ShowGainBaby(82)
	end
	if ARG0 == 5072 then
		gameLib.ShowGainBaby(83)
	end
	if ARG0 == 5073 then
		gameLib.ShowGainBaby(85)
	end
	if ARG0 == 5074 then
		gameLib.ShowGainBaby(92)
	end
end



function CheckBuff(GAMEEVENT,ARG0,ARG1)
	if ARG0 == 1000 and ARG1 <= 500 then
		gameLib.MessageBoxQuickUse("生命值存储量不足500,是否购买？",3003)
	end
	if ARG0 == 1001 and ARG1 <= 500 then
		gameLib.MessageBoxQuickUse("魔法值存储量不足500,是否购买？",3004)
	end
end

function NpcTalked(GAMEEVENT,ARG0,ARG1)

	if ARG1 == 3018 then
		gameLib.CreateGuideInScene(1,5,200,GPRT_None,"这个小姑娘怎么啦？让我过去看看吧。",0.7,true);
	end

	--野外战场
	if ARG0 == 1112 then
		local team_table = gameLib.TeamMemberLevel()
		if team_table ~= nil then
			local lv = 0
			local num = table.getn(team_table)
			for i=1,table.getn(team_table),1
			do
				if team_table[i] < 30 then
					gameLib.PopText("teamlevelpvp")
					return true
				else
					lv = lv + team_table[i]
				end
			end
			if lv/num >= 20 and lv/num < 35 then
				gameLib.ShowMsgBox("teampkbox", 800)
			elseif lv/num >= 35 and lv/num < 50 then
				gameLib.ShowMsgBox("teampkbox", 802)
			elseif lv/num >= 50 then
				gameLib.ShowMsgBox("teampkbox", 804)
			else
				gameLib.PopText(err)
			end
			return true
		end
		local lv = gameLib.PlayerLevel()
		if lv < 30 then
			gameLib.PopText("equipLevel")
			return true
		elseif lv >= 20 and lv < 35 then
			gameLib.ShowMsgBox("singlepkbox", 800)
			
		elseif lv >= 35 and lv < 50 then
			gameLib.ShowMsgBox("singlepkbox", 802)
		elseif lv >= 50 then
			gameLib.ShowMsgBox("singlepkbox", 804)
		end
	end
	if ARG0 == 1113 then
		local team_table = gameLib.TeamMemberLevel()
		if team_table == nil then
			local lv = gameLib.PlayerLevel()
			if lv < 30 then
				gameLib.PopText("equipLevel")
				return true
			elseif lv >= 20 and lv < 35 then
				gameLib.ShowMsgBox("singlepkbox", 801)
				
			elseif lv >= 35 and lv < 50 then
				gameLib.ShowMsgBox("singlepkbox", 803)
			elseif lv >= 50 then
				gameLib.ShowMsgBox("singlepkbox", 805)
			end
			return true
		end
		local lv = 0
		local num = table.getn(team_table)
		for i=1,table.getn(team_table),1
		do
			if team_table[i] < 30 then
				gameLib.PopText("teamlevelpvp")
				return true
			else
				lv = lv + team_table[i]
			end
		end
		if lv/num >= 20 and lv/num < 35 then
			gameLib.ShowMsgBox("teampkbox", 801)
		elseif lv/num >= 35 and lv/num < 50 then
			gameLib.ShowMsgBox("teampkbox", 803)
		elseif lv/num >= 50 then
			gameLib.ShowMsgBox("teampkbox", 805)
		else
			gameLib.PopText(err)
		end
	end
	return false	--返回false是不处理 客户端直接发送TalkedToNPC
end

function MessageBoxOkHandler(GameEvent,userdata)
	if userdata == 99940 then
		gameLib.TalkedToNpc(9571)
		return
	end
	if userdata == 99930 then
		gameLib.TalkedToNpc(9570)
		return
	end
	if userdata == 99950 then
		gameLib.TalkedToNpc(9580)
		return
	end
	if userdata == 99960 then
		gameLib.TalkedToNpc(9581)
		return
	end
	if userdata == 99961 then
		gameLib.TalkedToNpc(9582)
		return
	end
	if userdata == 99970 then
		gameLib.TalkedToNpc(9583)
		return
	end
	gameLib.TransforScene(userdata);
end

function EnterCopy(GameEvent,id)
	if id == 1001 then
		local team_table = gameLib.TeamMemberLevel()
		if team_table == nil then
			gameLib.PopText("bunengjinruben")
			return true
		end
		if gameLib.HasLeaveMember() == true then
			gameLib.PopText("pvpzuduizanli")
			return true
		end
		local num = table.getn(team_table)
		if num < 3 then
			gameLib.PopText("bunengjinruben")
			return true
		end
		for i=1,table.getn(team_table),1
		do
			if team_table[i] < 30 then
				gameLib.PopText("teamlevelfuben")
				return true
			end
		end
		
		gameLib.ShowMsgBox("confirmEnterInstance30", 99930)
		return true
	elseif id == 1003 then
		local team_table = gameLib.TeamMemberLevel()
		if team_table == nil then
			gameLib.PopText("bunengjinruben")
			return true
		end
		if gameLib.HasLeaveMember() == true then
			gameLib.PopText("pvpzuduizanli")
			return true
		end
		local num = table.getn(team_table)
		if num < 3 then
			gameLib.PopText("bunengjinruben")
			return true
		end
		for i=1,table.getn(team_table),1
		do
			if team_table[i] < 40 then
				gameLib.PopText("teamlevelfuben1")
				return true
			end
		end
		
		gameLib.ShowMsgBox("confirmEnterInstance40", 99940)
		return true
	elseif id == 1005 then
		local team_table = gameLib.TeamMemberLevel()
		if team_table == nil then
			gameLib.PopText("bunengjinruben")
			return true
		end
		if gameLib.HasLeaveMember() == true then
			gameLib.PopText("pvpzuduizanli")
			return true
		end
		local num = table.getn(team_table)
		if num < 3 then
			gameLib.PopText("bunengjinruben")
			return true
		end
		for i=1,table.getn(team_table),1
		do
			if team_table[i] < 50 then
				gameLib.PopText("teamlevelfuben2")
				return true
			end
		end
		
		gameLib.ShowMsgBox("confirmEnterInstance50", 99950)
		return true
	elseif id == 1006 then
		local team_table = gameLib.TeamMemberLevel()
		if team_table == nil then
			gameLib.PopText("bunengjinruben")
			return true
		end
		if gameLib.HasLeaveMember() == true then
			gameLib.PopText("pvpzuduizanli")
			return true
		end
		local num = table.getn(team_table)
		if num < 3 then
			gameLib.PopText("bunengjinruben")
			return true
		end
		for i=1,table.getn(team_table),1
		do
			if team_table[i] < 60 then
				gameLib.PopText("teamlevelfuben3")
				return true
			end
		end
		
		gameLib.ShowMsgBox("confirmEnterInstance60", 99960)
		return true
	elseif id == 1007 then
		local team_table = gameLib.TeamMemberLevel()
		if team_table == nil then
			gameLib.PopText("bunengjinruben")
			return true
		end
		if gameLib.HasLeaveMember() == true then
			gameLib.PopText("pvpzuduizanli")
			return true
		end
		local num = table.getn(team_table)
		if num < 3 then
			gameLib.PopText("bunengjinruben")
			return true
		end
		for i=1,table.getn(team_table),1
		do
			if team_table[i] < 60 then
				gameLib.PopText("teamlevelfuben")
				return true
			end
		end
		
		gameLib.ShowMsgBox("confirmEnterInstance60", 99961)
		return true
	elseif id == 1008 then
		local team_table = gameLib.TeamMemberLevel()
		if team_table == nil then
			gameLib.PopText("bunengjinruben")
			return true
		end
		if gameLib.HasLeaveMember() == true then
			gameLib.PopText("pvpzuduizanli")
			return true
		end
		local num = table.getn(team_table)
		if num < 3 then
			gameLib.PopText("bunengjinruben")
			return true
		end
		for i=1,table.getn(team_table),1
		do
			if team_table[i] < 70 then
				gameLib.PopText("teamlevelfuben")
				return true
			end
		end
		
		gameLib.ShowMsgBox("confirmEnterInstance70", 99970)
		return true
	end
end


