Sys.log("load WuYaoKeJiaSuErDe_Boss.lua");

function WuYaoKeJiaSuErDe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	--当小怪数量少于4个以后，使用召唤僵尸技能，每次召唤2个。
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	local num = table.maxn(my_table) - 1;   -- num小怪数量
	if num < 4
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,5506);
		return;
	end
	
	--当血量少于30%以后，30%概率使用明镜，30%概率使用吸血魔法，40%概率使用圣盾、超强中毒、超强石化、超强酒醉、超强混乱、超强昏睡
	local hpRatio = Battle.get_prop(ARG0,ARG1,PT_HpCurr)/Battle.get_prop(ARG0,ARG1,PT_HpMax);
	if hpRatio < 0.3
	then
		local roll = math.random(1,10);
		if roll <= 3
		then
			Battle.ai_pushOrder(RECEIVER,ARG1,1091);
		elseif roll > 3 and roll <=6
		then
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.ai_pushOrder(RECEIVER,random_pos,2121);
		else
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			local skillTable = {2501,2251,2271,2281,2291,2201};
			local index = math.ceil(math.random(1,table.maxn(skillTable)));
			if Battle.getSkill_TargetType(ARG0,skillTable[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,skillTable[index]) == STT_Team then
				--选择自己阵营的一个随机目标
				random_pos = random_TargetPos(ARG0, ARG2, ARG1, ARG3, false)
			end
			Battle.ai_pushOrder(RECEIVER,random_pos,skillTable[index]);
		end
		return;
	end
	--随机使用技能，其中30%概率使用超强中毒、超强石化、超强酒醉、超强混乱、超强昏睡，30%概率使用吸血、圣盾、强力恢复、40%概率使用攻击。
	local roll = math.random(1,10);
	if roll <= 3
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local skillTable = {2251,2271,2281,2291,2201};
		local index = math.ceil(math.random(1,table.maxn(skillTable)));
		Battle.ai_pushOrder(RECEIVER,random_pos,skillTable[index]);
	elseif roll > 3 and roll <=6
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local skillTable = {2121,2501,2421};
		local index = math.ceil(math.random(1,table.maxn(skillTable)));
		if skillTable[index] == 2121
		then
			Battle.ai_pushOrder(RECEIVER,random_pos,skillTable[index]);
		else
			local group = check_group_type(ARG1);
			local targetPos = Battle.getMaxMpPos(ARG0,group);
			Battle.ai_pushOrder(RECEIVER,targetPos,skillTable[index]);
		end
	else
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,1);
	end
end

function WuYaoKeJiaSuErDe_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		WuYaoKeJiaSuErDe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		WuYaoKeJiaSuErDe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function WuYaoKeJiaSuErDe_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		WuYaoKeJiaSuErDe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		WuYaoKeJiaSuErDe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function WuYaoKeJiaSuErDe_Deadth(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)

end

