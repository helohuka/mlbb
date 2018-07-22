Sys.log("load ZhnFu1.lua");

function ZhnFu1_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getSneakAttack(ARG0) == SAT_SurpriseAttack then
		return;
	end
	--1.如果自身血量少于最大血量的30%，使用明镜技能
	--2.优先攻击对面血量对多的怪物，80%概率使用乾坤，20%概率使用其他技能随机
	local myHpRatio = Battle.get_prop(ARG0,ARG1,PT_HpCurr)/Battle.get_prop(ARG0,ARG1,PT_HpMax);
	if myHpRatio < 0.1 then
		Battle.ai_pushOrder(RECEIVER,ARG1,1091);
	else
		local Force = check_enemy_group_type(ARG1);
		local maxHpPos = Battle.getMaxHpPos(ARG0,Force);
		local roll = math.random(1,10);
		
		if roll <= 8 then
			Battle.ai_pushOrder(RECEIVER,maxHpPos,1061);
		else
			local index = math.ceil(math.random(1,table.maxn(ARG4)));
			while ARG4[index] == 1061 or ARG4[index] == 1091
			do
				index = math.ceil(math.random(1,table.maxn(ARG4)));
			end
			Battle.ai_pushOrder(RECEIVER,maxHpPos,ARG4[index]);
		end
	end
end

function ZhnFu1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	--1.如果自身血量少于最大血量的30%，使用明镜技能
	--2.优先攻击对面血量对多的怪物，80%概率使用乾坤，20%概率使用其他技能随机
	local myHpRatio = Battle.get_prop(ARG0,ARG1,PT_HpCurr)/Battle.get_prop(ARG0,ARG1,PT_HpMax);
	if myHpRatio < 0.1 then
		Battle.ai_pushOrder(RECEIVER,ARG1,1091);
	else
		local Force = check_enemy_group_type(ARG1);
		local maxHpPos = Battle.getMaxHpPos(ARG0,Force);
		local roll = math.random(1,10);
		
		if roll <= 8 then
			Battle.ai_pushOrder(RECEIVER,maxHpPos,1061);
		else
			local index = math.ceil(math.random(1,table.maxn(ARG4)));
			while ARG4[index] == 1061 or ARG4[index] == 1091
			do
				index = math.ceil(math.random(1,table.maxn(ARG4)));
			end
			Battle.ai_pushOrder(RECEIVER,maxHpPos,ARG4[index]);
		end
	end
end

function ZhnFu1_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	
end