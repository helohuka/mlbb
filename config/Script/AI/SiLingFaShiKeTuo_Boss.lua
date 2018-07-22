Sys.log("load SiLingFaShiKeTuo_Boss.lua");

function SiLingFaShiKeTuo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	--如果小弟数量少于4个，则召唤，每次召唤4个
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	local num = table.maxn(my_table) - 1;   -- num小怪数量
	--[[if num < 4
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,5509);
		return;
	end]]--
	--如果小怪全部死亡，使用大地之怒
	if num == 0
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,5500);
		return;
	end
	--如果身上没有恢复的BUFF，则50%概率对自己使用恢复，其他概率随机使用乾坤，圣盾、超强风刃、超强冰冻魔法、战栗（优先对面当前魔法值最高的目标）
	local isRecover = not Battle.check_state(ARG0,my_table[i],ST_Recover);
	local isSRecover = not Battle.check_state(ARG0,my_table[i],ST_StrongRecover);
	local isGRecover = not Battle.check_state(ARG0,my_table[i],ST_GroupRecover);
	
	if isRecover and isSRecover and isGRecover
	then
		local roll = math.random(1,10);
		if roll <= 5
		then
			Battle.ai_pushOrder(RECEIVER,ARG1,2411);
		else
			local skillTable = {1061,2501,2111,2091,1081};
			local index = math.ceil(math.random(1,table.maxn(skillTable)));
			if Battle.getSkill_TargetType(ARG0,skillTable[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,skillTable[index]) == STT_Team then
				--选择自己阵营的一个随机目标
				random_pos = random_TargetPos(ARG0, ARG2, ARG1, ARG3, false)
			end
			if skillTable[index] == 1081
			then
				local group = check_group_type(ARG2);
				local targetPos = Battle.getMaxMpPos(ARG0,group);
				Battle.ai_pushOrder(RECEIVER,targetPos,1081);
			else
				local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
				Battle.ai_pushOrder(RECEIVER,random_pos,skillTable[index]);
			end
		end
		return;
	end
	
	local skillTable = {1061,2501,2111,2091,1081};
	local index = math.ceil(math.random(1,table.maxn(skillTable)));
	if Battle.getSkill_TargetType(ARG0,skillTable[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,skillTable[index]) == STT_Team then
		--选择自己阵营的一个随机目标
		random_pos = random_TargetPos(ARG0, ARG2, ARG1, ARG3, false)
	end
	if skillTable[index] == 1081
	then
		local group = check_group_type(ARG2);
		local targetPos = Battle.getMaxMpPos(ARG0,group);
		Battle.ai_pushOrder(RECEIVER,targetPos,1081);
	else
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,skillTable[index]);
	end
end

function SiLingFaShiKeTuo_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		SiLingFaShiKeTuo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		SiLingFaShiKeTuo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function SiLingFaShiKeTuo_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		SiLingFaShiKeTuo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		SiLingFaShiKeTuo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function SiLingFaShiKeTuo_Deadth(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)

end