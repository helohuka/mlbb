Sys.log("load gedou1.lua");

function GeDou1_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getSneakAttack(ARG0) == SAT_SurpriseAttack then
		return;
	end
	--如果自身血量小于30%如果有明镜技能
	local myHpRatio = Battle.get_prop(ARG0,ARG1,PT_HpCurr)/Battle.get_prop(ARG0,ARG1,PT_HpMax);
	if myHpRatio < 0.1 then
		Battle.ai_pushOrder(RECEIVER,ARG1,1091);
		return;
	end
	--1.如果对面人数大于3人，使用气功弹技能。少于3人75%攻击。25%蹦极
	--敌方有效成员位置列表 enemy_table
	local enemy_table = get_enemy_table(ARG0,ARG3,ARG1);	
	local enemyNum = table.maxn(enemy_table);
	local enemyIndex = math.ceil(math.random(1,table.maxn(enemy_table)));
	if enemyNum > 1 then
		Battle.ai_pushOrder(RECEIVER,enemy_table[enemyIndex],1051);
	else
		local roll = math.random(1,100);
		if roll <= 75 
		then
			Battle.ai_pushOrder(RECEIVER,enemy_table[enemyIndex],1051);
		else
			Battle.ai_pushOrder(RECEIVER,enemy_table[enemyIndex],1051);
		end
	end
end

function GeDou1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	--如果自身血量小于30%如果有明镜技能
	local myHpRatio = Battle.get_prop(ARG0,ARG1,PT_HpCurr)/Battle.get_prop(ARG0,ARG1,PT_HpMax);
	if myHpRatio < 0.1 then
		Battle.ai_pushOrder(RECEIVER,ARG1,1091);
		return;
	end
	--1.如果对面人数大于3人，使用气功弹技能。少于3人75%攻击。25%蹦极
	--敌方有效成员位置列表 enemy_table
	local enemy_table = get_enemy_table(ARG0,ARG3,ARG1);	
	local enemyNum = table.maxn(enemy_table);
	local enemyIndex = math.ceil(math.random(1,table.maxn(enemy_table)));
	if enemyNum > 1 then
		Battle.ai_pushOrder(RECEIVER,enemy_table[enemyIndex],1051);
	else
		local roll = math.random(1,100);
		if roll <= 75 
		then
			Battle.ai_pushOrder(RECEIVER,enemy_table[enemyIndex],1051);
		else
			Battle.ai_pushOrder(RECEIVER,enemy_table[enemyIndex],1051);
		end
	end
end

function GeDou1_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	
end