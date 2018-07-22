Sys.log("load jianshi.lua");

function JianShi_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getSneakAttack(ARG0) == SAT_SurpriseAttack then
		return;
	end
	--1.如果对面怪物数量大于等于3个，则使用连击
	--2.优先攻击对面血量最多的怪物，80%概率使用乾坤，20%概率随机其他技能
	local enemy_table = get_enemy_table(ARG0,ARG3,ARG1);	
	local enemyNum = table.maxn(enemy_table);
	if enemyNum >= 3 
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,1001);
	else
		local enemyForce = check_enemy_group_type(ARG1);
		local maxHpPos = Battle.getMaxHpPos(ARG0,enemyForce);
		local roll = math.random(1,10);
		
		if roll <= 8 
		then
			Battle.ai_pushOrder(RECEIVER,maxHpPos,1061);
		else
			local index = math.ceil(math.random(1,table.maxn(ARG4)));
			while ARG4[index] == 1061 
			do
				index = math.ceil(math.random(1,table.maxn(ARG4)));
			end
			Battle.ai_pushOrder(RECEIVER,maxHpPos,ARG4[index]);
		end
	end
end

function JianShi_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	--1.如果对面怪物数量大于等于3个，则使用连击
	--2.优先攻击对面血量最多的怪物，80%概率使用乾坤，20%概率随机其他技能
	local enemy_table = get_enemy_table(ARG0,ARG3,ARG1);	
	local enemyNum = table.maxn(enemy_table);
	if enemyNum >= 3 
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,1001);
	else
		local enemyForce = check_enemy_group_type(ARG1);
		local maxHpPos = Battle.getMaxHpPos(ARG0,enemyForce);
		local roll = math.random(1,10);
		if roll <= 8 
		then
			Battle.ai_pushOrder(RECEIVER,maxHpPos,1061);
		else
			local index = math.ceil(math.random(1,table.maxn(ARG4)));
			while ARG4[index] == 1061 
			do
				index = math.ceil(math.random(1,table.maxn(ARG4)));
			end
			Battle.ai_pushOrder(RECEIVER,maxHpPos,ARG4[index]);
		end
	end
end

function JianShi_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	
end