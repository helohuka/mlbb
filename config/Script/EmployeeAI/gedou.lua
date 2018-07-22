Sys.log("load gedou.lua");

function GeDou_TongYongLiuCheng(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
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

function GeDou_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getSneakAttack(ARG0) == SAT_SurpriseAttack then
		return;
	end
	--如果自身血量小于30%如果有明镜技能
	local myHpRatio = Battle.get_prop(ARG0,ARG1,PT_HpCurr)/Battle.get_prop(ARG0,ARG1,PT_HpMax);
	if myHpRatio < 0.1 then
		Battle.ai_pushOrder(RECEIVER,ARG1,1091);
		return;
	end
	--2.如果对面怪物中有BOSS，则80%概率对BOSS使用战栗（直到BOSS的魔法值少于最大值的10%），20%概率随机使用其他技能
	local bossPos = Battle.getBossPos(ARG0);
	if bossPos ~= BP_None and ( Battle.get_prop(ARG0,bossPos,PT_MpCurr)/Battle.get_prop(ARG0,bossPos,PT_MpMax)>0.1 ) then
		local roll = math.random(1,10);
		if roll <= 8 then
			Battle.ai_pushOrder(RECEIVER,bossPos,1051);
		else
			local index = math.ceil(math.random(1,table.maxn(ARG4)));
			while(ARG4[index] == 1081 or ARG4[index] == 1091) 
			do
				index = math.ceil(math.random(1,table.maxn(ARG4)));
			end
			Battle.ai_pushOrder(RECEIVER,bossPos,ARG4[index]);
		end
		return;
	end
	
	--1.如果对面人数大于3人，使用气功弹技能。少于3人放攻击
	--敌方有效成员位置列表 enemy_table
	local enemy_table = get_enemy_table(ARG0,ARG3,ARG1);	
	local enemyNum = table.maxn(enemy_table);
	local enemyIndex = math.ceil(math.random(1,table.maxn(enemy_table)));
	Battle.ai_pushOrder(RECEIVER,enemy_table[enemyIndex],1051);
end

function GeDou_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	--如果自身血量小于30%如果有明镜技能
	local myHpRatio = Battle.get_prop(ARG0,ARG1,PT_HpCurr)/Battle.get_prop(ARG0,ARG1,PT_HpMax);
	if myHpRatio < 0.1 then
		Battle.ai_pushOrder(RECEIVER,ARG1,1091);
		return;
	end
	--2.如果对面怪物中有BOSS，则80%概率对BOSS使用战栗（直到BOSS的魔法值少于最大值的10%），20%概率随机使用其他技能
	local bossPos = Battle.getBossPos(ARG0);
	if bossPos ~= BP_None and ( Battle.get_prop(ARG0,bossPos,PT_MpCurr)/Battle.get_prop(ARG0,bossPos,PT_MpMax)>0.1 ) then
		local roll = math.random(1,10);
		if roll <= 8 then
			Battle.ai_pushOrder(RECEIVER,bossPos,1051);
		else
			local index = math.ceil(math.random(1,table.maxn(ARG4)));
			while(ARG4[index] == 1081 or ARG4[index] == 1091) 
			do
				index = math.ceil(math.random(1,table.maxn(ARG4)));
			end
			Battle.ai_pushOrder(RECEIVER,bossPos,ARG4[index]);
		end
		return;
	end
	
	--1.如果对面人数大于3人，使用气功弹技能。少于3人放攻击
	--敌方有效成员位置列表 enemy_table
	local enemy_table = get_enemy_table(ARG0,ARG3,ARG1);	
	local enemyNum = table.maxn(enemy_table);
	local enemyIndex = math.ceil(math.random(1,table.maxn(enemy_table)));
	Battle.ai_pushOrder(RECEIVER,enemy_table[enemyIndex],1051);
end

function GeDou_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	
end