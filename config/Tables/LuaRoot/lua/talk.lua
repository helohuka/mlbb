local gameLib = require "GameLib.cs"

gameLib.RegistEvent(SGE_Talk_FirstEnterMainScene, "Talk_FirstEnterMainScene");
gameLib.RegistEvent(SGE_Talk_BattleReady, "Talk_BattleReady");
gameLib.RegistEvent(SGE_Talk_ActorReady, "Talk_ActorReady");
gameLib.RegistEvent(SGE_Talk_BattleOver, "Talk_BattleOver");


--剧情对话
gameLib.RegistEvent(SGE_SenseEnter2, "SenseEnter2");
gameLib.RegistEvent(SGE_WaitTalk, "SenseWaitTalk");
gameLib.RegistEvent(SGE_SenseTalked, "SenseTalked");

function Talk_FirstEnterMainScene(GAMEEVENT)
	gameLib.BeginTalk(3000);
end

function Talk_BattleReady(GAMEEVENT, battleId)
	if battleId == 1 then
		gameLib.BeginTalk(10000);
	elseif battleId == 2 then
		gameLib.BeginTalk(7011);
	elseif battleId == 3 then
		gameLib.BeginTalk(7015);
	elseif battleId == 4 then
		gameLib.BeginTalk(7017);
	elseif battleId == 5 then
		gameLib.BeginTalk(7019);
	elseif battleId == 6 then
		gameLib.BeginTalk(7021);
	elseif battleId == 7 then
		gameLib.BeginTalk(7013);
	elseif battleId == 8 then
		gameLib.BeginTalk(7023);
	elseif battleId == 9 then
		gameLib.BeginTalk(7025);
	elseif battleId == 10 then
		gameLib.BeginTalk(7027);
	elseif battleId == 11 then
		gameLib.BeginTalk(7029);
	elseif battleId == 12 then
		gameLib.BeginTalk(7031);
	else
		gameLib.BeginTalk(0);
	end
end

function Talk_ActorReady(GAMEEVENT, battleId, turn, pos)
	gameLib.BeginTalk(0);
	--[[if battleId == 1 and turn == 1 and pos == BP_Up0 then
		gameLib.BeginTalk(7001);
	elseif battleId == 1 and  turn == 1 and pos == BP_Up4 then
		gameLib.BeginTalk(7002);
	elseif battleId == 1 and  turn == 1 and pos == BP_Up3 then
		gameLib.BeginTalk(7003);
	elseif battleId == 1 and  turn == 1 and pos == BP_Up1 then
		gameLib.BeginTalk(7004);
	elseif battleId == 1 and  turn == 1 and pos == BP_Up2 then
		gameLib.BeginTalk(7005);
	elseif battleId == 1 and  turn == 1 and pos == BP_Down1 then
		gameLib.BeginTalk(7006);
	elseif battleId == 1 and  turn == 1 and pos == BP_Down3 then
		gameLib.BeginTalk(7007);
	elseif battleId == 1 and  turn == 1 and pos == BP_Down4 then
		gameLib.BeginTalk(7008);
	elseif battleId == 1 and  turn == 1 and pos == BP_Down0 then
		gameLib.BeginTalk(7009);
	else
		gameLib.BeginTalk(0);
	end]]--
end

function Talk_BattleOver(GAMEEVENT, battleId,BattleJudgeType)
	if BattleJudgeType ~= BJT_Win then
		gameLib.BeginTalk(0);
		return
	end
	if battleId == 1 then
		gameLib.BeginTalk(10001);
	elseif battleId == 2 then
		gameLib.BeginTalk(7012);
	elseif battleId == 3 then
		gameLib.BeginTalk(7016);
	elseif battleId == 4 then
		gameLib.BeginTalk(7018);
	elseif battleId == 5 then
		gameLib.BeginTalk(7020);
	elseif battleId == 6 then
		gameLib.BeginTalk(7022);
	elseif battleId == 7 then
		gameLib.BeginTalk(7014);
	elseif battleId == 8 then
		gameLib.BeginTalk(7024);
	elseif battleId == 9 then
		gameLib.BeginTalk(7026);
	elseif battleId == 10 then
		gameLib.BeginTalk(7028);
	elseif battleId == 11 then
		gameLib.BeginTalk(7030);
	elseif battleId == 12 then
		gameLib.BeginTalk(7032);
	else
		gameLib.BeginTalk(0);
	end
end


function SenseEnter2(GAMEEVENT)
	gameLib.PlaySense(1);
end

function SenseWaitTalk(GAMEEVENT, who, index)
	--卫兵第一句话
	if who == SAT_Guard then
		if index == 1 then
			gameLib.SenseTalk(SAT_Guard, "70001", 1,9);
			gameLib.SenseTalk(SAT_Guard, "70001", 6,9);
		end
	--大臣说第一句话
	elseif who == SAT_Ambassdor then
		gameLib.SenseTalk(SAT_Ambassdor, "70002",0,4);
	--国王说第一句话
	elseif who == SAT_King then
		gameLib.SenseTalk(SAT_King, "70003",0,5);
	elseif who == SAT_Yingzi then
		gameLib.SenseTalk(SAT_King, "70005",0,4);
	--第三段剧情
	elseif who == SAT_Axe then
		gameLib.SenseTalk(SAT_Axe, "70006",0,4);
	elseif who == SAT_VillageKing then
		gameLib.SenseTalk(SAT_VillageKing, "70011",0,4);
	elseif who == SAT_AllMonster then
		gameLib.SenseTalk(SAT_VillageKing, "70012",0,5);
	end
end
--[[
function SenseTalked(GAMEEVENT, who, talkKey, index)
	if who == SAT_Ambassdor then
		gameLib.QuitSense();
	elseif who == SAT_King and talkKey == 70003 then
		gameLib.SenseNext();
	elseif who == SAT_King and talkKey == 70005 then
		gameLib.SenseTalk(SAT_Yingzi, 70004,0,3);
	elseif who == SAT_Yingzi and talkKey == 70004 then
		gameLib.QuitSense();
	--第三段剧情
	elseif who == SAT_Axe then
		gameLib.SenseTalk(SAT_Archor, "70007",0,4);
	elseif who == SAT_Archor then
		gameLib.SenseTalk(SAT_Mage, "70008",0,4);
	elseif who == SAT_Mage then
		gameLib.SenseTalk(SAT_Sage, "70009",0,4);
	elseif who == SAT_Sage then
		gameLib.SenseTalk(SAT_Girl, "70010",0,4);
	elseif who == SAT_Girl then
		gameLib.QuitSense();
	elseif who == SAT_VillageKing and talkKey == 70011 then
		gameLib.SenseNext();
	elseif who == SAT_VillageKing and talkKey == 70012 then
		gameLib.QuitSense();
	end
end
]]--