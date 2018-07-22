Sys.log("load EMoShiTuLuoErDan_Boss.lua");

function EMoShiTuLuoErDan_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	--当老鼠数量少于4只，使用召唤，每次召唤6只，3只大地鼠，3只火焰鼠
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	local num = table.maxn(my_table) - 1;   -- num小怪数量
	if num < 4
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,5508);
	else
		--随机使用攻击吸收、攻击反弹、攻击无效、单体恢复、单体补血、超强风刃，全体混乱技能
		local skillTable = {2441,2481,2411,2361,2111,2291};
		local index = math.ceil(math.random(1,table.maxn(skillTable)));
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		if Battle.getSkill_TargetType(ARG0,skillTable[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,skillTable[index]) == STT_Team 
		then
			--选择自己阵营的一个随机目标
			random_pos = random_TargetPos(ARG0, ARG2, ARG1, ARG3, false)
		end
		Battle.ai_pushOrder(RECEIVER,random_pos,skillTable[index]);
	end
end

function EMoShiTuLuoErDan_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		EMoShiTuLuoErDan_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		EMoShiTuLuoErDan_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function EMoShiTuLuoErDan_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		EMoShiTuLuoErDan_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		EMoShiTuLuoErDan_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function EMoShiTuLuoErDan_Deadth(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)

end