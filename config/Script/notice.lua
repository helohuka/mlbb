
function BabyNotice(RECEIVER,ARG0,ARG1,ARG2,ARG3)
	if ARG2 >= 2 and ARG3 == 0 then
		local name = Player.getplayerName(RECEIVER)
		local babyname = Player.getbabyName(ARG0)
		local str = "恭喜".."[FF0000]"..name.."[-]".."获得了超凡品质的宠物："..babyname
		Sys.notice(str)
	end
end


function EmpNotice(RECEIVER,ARG0,ARG1)
	if ARG1 >= QC_Purple then
		local name = Player.getplayerName(RECEIVER)
		local empname = Player.getemployeeName(ARG0)
		local str = "恭喜".."[FF0000]"..name.."[-]".."获得了超凡品质的伙伴："..empname
		Sys.notice(str)
	end
end

function PvRnotice(RECEIVER,ARG0,ARG1)
	if ARG0 <= 3 and ARG1 ~= 0 then
		local name = Player.getplayerName(RECEIVER)
		local str = "恭喜".."[FF0000]"..name.."[-]".."达到了武斗场第"..ARG0.."名"
		Sys.notice(str)
	end
end

function PvPnotice(RECEIVER,ARG0)
	if ARG0 >= 15 then
		local name = Player.getplayerName(RECEIVER)
		local str = "恭喜".."[FF0000]"..name.."[-]".."达到了竞技场传奇I段位"
		Sys.notice(str)
	end
end


function BabyReset(RECEIVER,ARG0,ARG1)
	local data = Player.getbabydelat(RECEIVER,ARG1)
	if data <= 1 then
		local name = Player.getplayerName(RECEIVER)
		local babyname = Player.getbabyName(ARG0)
		local str = "恭喜".."[FF0000]"..name.."[-]".."的宠物"..babyname.."成长达到了传奇级别"
		Sys.notice(str)
	end
end

function SkillLevelUpNotice(RECEIVER,ARG0,ARG1)
	if ARG1 == 10 then
		local name = Player.getplayerName(RECEIVER)
		local skillname = Player.getskillName(ARG0,ARG1)
		local str = "恭喜".."[FF0000]"..name.."[-]".."将技能"..skillname.."升到了10级！"
		Sys.notice(str)
	end
end


function ZhuanpanNotice(RECEIVER,ARG0,ARG1)
	if ARG1 == 1 then
		--local name = Player.getplayerName(RECEIVER)
		--local str = "恭喜".."[FF0000]"..name.."[-]".."在幸运大转盘中抽中了珍稀宠物沙巨龙"
		--Sys.notice(str)
	end
end


function notice_babystr(RECEIVER,ARG0)
	if ARG0 >= 7 then
		local name = Player.getplayerName(RECEIVER)
		local str = "恭喜".."[FF0000]"..name.."[-]".."将宠物强化到了"..ARG0.."级！"
		Sys.notice(str)
	end
end

--破标装备
function Notice_MakeEquip(RECEIVER,ARG0,ARG1)
	if ARG1 == 1 then
		local name = Player.getplayerName(RECEIVER)
		local str = "恭喜".."[FF0000]"..name.."[-]".."锻造出了破标装备，属性爆表！"
		Sys.notice(str)
	end
end