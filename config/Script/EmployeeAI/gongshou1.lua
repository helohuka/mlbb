Sys.log("load gongshou1.lua");

function GongShou1_TongYongLiuCheng(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	local num = table.maxn(ARG4)
	if num == 0 then
		return;
	end
	local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	local index = math.ceil(math.random(1,num));
	while(ARG4[index] == 2391 or ARG4[index] == 1091 or ARG4[index] == 2401 )
		do
			index = math.ceil(math.random(1,num));
		end
	if Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_Team then
		--选择自己阵营的一个随机目标
		random_pos = random_TargetPos(ARG0, ARG2, ARG1, ARG3, false)
	end
	Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
end

function GongShou1_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getSneakAttack(ARG0) == SAT_SurpriseAttack then
		return;
	end
	--1.如果己方有人血量少于30%，则护卫那个人
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	local pos = BP_None;
	for i=1,table.getn(my_table),1
	do
		local hpRatio = Battle.get_prop(ARG0,my_table[i],PT_HpCurr)/Battle.get_prop(ARG0,my_table[i],PT_HpMax)
	
		if hpRatio < 0.3
		then
			pos = my_table[i];
			break;
		end
	end
	if pos ~= BP_None
	then
		if pos == ARG1
		then
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.ai_pushOrder(RECEIVER,random_pos,10);
		else
			Battle.ai_pushOrder(RECEIVER,pos,2521);
		end
		return;
	end
	--2.如果对面的类型为怪物则大于4个使用乱射(PVE)，除此之外大于4个50%使用乱射50%使用攻击或者乾坤
	--3.如果怪物数量少于4个，则80%概率使用乾坤，20%概率随机使用其他技能
	local enemy_table = get_enemy_table(ARG0,ARG3,ARG1);	
	local enemyNum = table.maxn(enemy_table);
	if enemyNum >= 3
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		if Battle.get_BattleType(ARG0) == BT_PVE
		then
			Battle.ai_pushOrder(RECEIVER,random_pos,1041);
		else
			local roll = math.random(1,10);
			if roll <= 5
			then
				Battle.ai_pushOrder(RECEIVER,random_pos,1041);
			else
				local skillt = {10,1062};
				local index = math.ceil(math.random(1,table.maxn(skillt)));
				Battle.ai_pushOrder(RECEIVER,random_pos,skillt[index]);
			end
		end
	else
		local Force = check_enemy_group_type(ARG1);
		local maxHpPos = Battle.getMaxHpPos(ARG0,Force);
		local roll = math.random(1,10);
		if roll <= 8 
		then
			Battle.ai_pushOrder(RECEIVER,maxHpPos,1062);
		else
			local index = math.ceil(math.random(1,table.maxn(ARG4)));
			while ARG4[index] == 1062 or ARG4[index] == 2521
			do
				index = math.ceil(math.random(1,table.maxn(ARG4)));
			end
			Battle.ai_pushOrder(RECEIVER,maxHpPos,ARG4[index]);
		end
	end
end

function GongShou1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	--1.如果己方有人血量少于30%，则护卫那个人
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	local pos = BP_None;
	for i=1,table.getn(my_table),1
	do
		local hpRatio = Battle.get_prop(ARG0,my_table[i],PT_HpCurr)/Battle.get_prop(ARG0,my_table[i],PT_HpMax)
	
		if hpRatio < 0.3
		then
			pos = my_table[i];
			break;
		end
	end
	if pos ~= BP_None
	then
		if pos == ARG1
		then
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.ai_pushOrder(RECEIVER,random_pos,10);
		else
			Battle.ai_pushOrder(RECEIVER,pos,2521);
		end
		return;
	end
	--2.如果对面的类型为怪物则大于4个使用乱射(PVE)，除此之外大于4个50%使用乱射50%使用攻击或者乾坤
	--3.如果怪物数量少于4个，则80%概率使用乾坤，20%概率随机使用其他技能
	local enemy_table = get_enemy_table(ARG0,ARG3,ARG1);	
	local enemyNum = table.maxn(enemy_table);
	if enemyNum >= 3
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		if Battle.get_BattleType(ARG0) == BT_PVE
		then
			Battle.ai_pushOrder(RECEIVER,random_pos,1041);
		else
			local roll = math.random(1,10);
			if roll <= 5
			then
				Battle.ai_pushOrder(RECEIVER,random_pos,1041);
			else
				local skillt = {10,1062};
				local index = math.ceil(math.random(1,table.maxn(skillt)));
				Battle.ai_pushOrder(RECEIVER,random_pos,skillt[index]);
			end
		end
	else
		local Force = check_enemy_group_type(ARG1)
		local maxHpPos = Battle.getMaxHpPos(ARG0,Force);
		local roll = math.random(1,10);
		if roll <= 8 
		then
			Battle.ai_pushOrder(RECEIVER,maxHpPos,1062);
		else
			local index = math.ceil(math.random(1,table.maxn(ARG4)));
			while ARG4[index] == 1062 or ARG4[index] == 2521
			do
				index = math.ceil(math.random(1,table.maxn(ARG4)));
			end
			Battle.ai_pushOrder(RECEIVER,maxHpPos,ARG4[index]);
		end
	end
end

function GongShou1_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	
end

















