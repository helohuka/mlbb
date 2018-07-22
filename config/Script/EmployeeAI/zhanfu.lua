Sys.log("load zhanfu.lua");

function ZhanFu_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getSneakAttack(ARG0) == SAT_SurpriseAttack then
		return;
	end
	--1.如果对面怪物中有BOSS，则80%概率对BOSS使用战栗（直到BOSS的魔法值少于最大值的10%），20%概率随机使用其他技能
	local bossPos = Battle.getBossPos(ARG0);
	local ratio = Battle.get_prop(ARG0,bossPos,PT_MpCurr)/Battle.get_prop(ARG0,bossPos,PT_MpMax);
	if bossPos ~= BP_None and ( ratio >0.1 ) then
		local roll = math.random(1,10);
		if roll <= 8 then
			Battle.ai_pushOrder(RECEIVER,bossPos,1061);
		else
			local index = math.ceil(math.random(1,table.maxn(ARG4)));
			while(ARG4[index] == 1081) 
			do
				index = math.ceil(math.random(1,table.maxn(ARG4)));
			end
			Battle.ai_pushOrder(RECEIVER,bossPos,ARG4[index]);
		end
		return;
	end
	--2.如果对面怪物数量大于等于3个，则使用连击
	--3.如果少于3个，优先攻击对面血量最多的怪物，70%概率使用乾坤技能，20%概率使用连击。10%其他技能随机
	local enemy_table = get_enemy_table(ARG0,ARG3,ARG1);	
	local enemyNum = table.maxn(enemy_table);
	if enemyNum >= 3 
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,1001);
	else
		local Force = check_enemy_group_type(ARG1);
		local maxHpPos = Battle.getMaxHpPos(ARG0,Force);
		local roll = math.random(1,10);
		
		if roll <= 7 
		then
			Battle.ai_pushOrder(RECEIVER,maxHpPos,1061);
		elseif roll == 8 or roll == 9 
		then
			Battle.ai_pushOrder(RECEIVER,maxHpPos,1001);
		else
			Battle.ai_pushOrder(RECEIVER,maxHpPos,8);
		end
	end
end

function ZhanFu_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	--1.如果对面怪物中有BOSS，则80%概率对BOSS使用战栗（直到BOSS的魔法值少于最大值的10%），20%概率随机使用其他技能
	local bossPos = Battle.getBossPos(ARG0);
	local ratio = Battle.get_prop(ARG0,bossPos,PT_MpCurr)/Battle.get_prop(ARG0,bossPos,PT_MpMax);
	if bossPos ~= BP_None and ( ratio >0.1 ) then
		local roll = math.random(1,10);
		if roll <= 8 then
			Battle.ai_pushOrder(RECEIVER,bossPos,1061);
			Sys.log("BOSS POS===>"..bossPos.."===PT_MpCurr ==>"..ratio);
		else
			local index = math.ceil(math.random(1,table.maxn(ARG4)));
			while(ARG4[index] == 1081) 
			do
				index = math.ceil(math.random(1,table.maxn(ARG4)));
			end
			Battle.ai_pushOrder(RECEIVER,bossPos,ARG4[index]);
		end
		return;
	end
	--2.如果对面怪物数量大于等于3个，则使用连击
	--3.如果少于3个，优先攻击对面血量最多的怪物，70%概率使用乾坤技能，20%概率使用连击。10%其他技能随机
	local enemy_table = get_enemy_table(ARG0,ARG3,ARG1);	
	local enemyNum = table.maxn(enemy_table);
	if enemyNum >= 3 
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,1001);
	else
		local Force = check_enemy_group_type(ARG1);
		local maxHpPos = Battle.getMaxHpPos(ARG0,Force);
		local roll = math.random(1,10);
		
		if roll <= 7 
		then
			Battle.ai_pushOrder(RECEIVER,maxHpPos,1061);
		elseif roll == 8 or roll == 9 
		then
			Battle.ai_pushOrder(RECEIVER,maxHpPos,1001);
		else
			Battle.ai_pushOrder(RECEIVER,maxHpPos,8);
		end
	end
end

function ZhanFu_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	
end