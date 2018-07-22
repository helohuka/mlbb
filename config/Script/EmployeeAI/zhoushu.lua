Sys.log("load zhoushu.lua");

function ZhouShu_TongYongLiuCheng(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
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

function ZhouShu_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getSneakAttack(ARG0) == SAT_SurpriseAttack then
		return;
	end
	--3.如果己方血量小于40%，则使用单体补血。
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	local l_table = {}; --己方血量小于40%的位置
	for i=1,table.getn(my_table),1
		do
			local hpRatio = Battle.get_prop(ARG0,my_table[i],PT_HpCurr)/Battle.get_prop(ARG0,my_table[i],PT_HpMax);
			if  hpRatio < 0.4
			then
				table.insert(l_table,my_table[i]);
			end
		end
	if table.maxn(l_table) > 0
	then
		local index = math.ceil(math.random(1,table.maxn(l_table)));
		Battle.ai_pushOrder(RECEIVER,l_table[index],2361);
		return;
	end
	--2.如果对面80%以上的人身上都有不良buff效果，那么攻击
	local enemy_table = get_enemy_table(ARG0,ARG3,ARG1);
	local debuffPosTable = {};
	for i=1, table.getn(enemy_table), 1
	do
		for j=ST_Poison,ST_Forget,1
		do
			if Battle.check_state(ARG0,enemy_table[i],j)
			then
				table.insert(debuffPosTable,enemy_table[i]);
			end
		end
	end
	
	if table.maxn(debuffPosTable)/table.maxn(enemy_table) > 0.8
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,8);
		return;
	end
	--1.如果对面类型为野怪（不是battle表中的战斗），75%攻击，25%中毒。除此之外,50%石化，50%中毒。
	local nodebuffPosTable = {};
	for i=1, table.getn(enemy_table), 1
	do
		for j=ST_Poison,ST_Forget,1
		do
			if not Battle.check_state(ARG0,enemy_table[i],j)
			then
				table.insert(nodebuffPosTable,enemy_table[i]);
			end
		end
	end
	if	Battle.get_BattleType(ARG0) == BT_PVE
	then
		local roll = math.random(1,100);
		if roll <= 75
		then
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.ai_pushOrder(RECEIVER,random_pos,8);
		else
			local index = math.ceil(math.random(1,table.maxn(nodebuffPosTable)));
			Battle.ai_pushOrder(RECEIVER,nodebuffPosTable[index],2251);
		end
	else
		local roll = math.random(1,100);
		if roll <= 50
		then
			local index = math.ceil(math.random(1,table.maxn(nodebuffPosTable)));
			Battle.ai_pushOrder(RECEIVER,nodebuffPosTable[index],2271);
		else
			local index = math.ceil(math.random(1,table.maxn(nodebuffPosTable)));
			Battle.ai_pushOrder(RECEIVER,nodebuffPosTable[index],2251);
		end
	end
end

function ZhouShu_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	--3.如果己方血量小于40%，则使用单体补血。
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	local l_table = {}; --己方血量小于40%的位置
	for i=1,table.getn(my_table),1
		do
			local hpRatio = Battle.get_prop(ARG0,my_table[i],PT_HpCurr)/Battle.get_prop(ARG0,my_table[i],PT_HpMax);
			if  hpRatio < 0.4
			then
				table.insert(l_table,my_table[i]);
			end
		end
	if table.maxn(l_table) > 0
	then
		local index = math.ceil(math.random(1,table.maxn(l_table)));
		Battle.ai_pushOrder(RECEIVER,l_table[index],2361);
		return;
	end
	--2.如果对面80%以上的人身上都有不良buff效果，那么攻击
	local enemy_table = get_enemy_table(ARG0,ARG3,ARG1);
	local debuffPosTable = {};
	for i=1, table.getn(enemy_table), 1
	do
		for j=ST_Poison,ST_Forget,1
		do
			if Battle.check_state(ARG0,enemy_table[i],j)
			then
				table.insert(debuffPosTable,enemy_table[i]);
			end
		end
	end
	
	if table.maxn(debuffPosTable)/table.maxn(enemy_table) > 0.8
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,8);
		return;
	end
	--1.如果对面类型为野怪（不是battle表中的战斗），75%攻击，25%中毒。除此之外,50%石化，50%中毒。
	local nodebuffPosTable = {};
	for i=1, table.getn(enemy_table), 1
	do
		for j=ST_Poison,ST_Forget,1
		do
			if not Battle.check_state(ARG0,enemy_table[i],j)
			then
				table.insert(nodebuffPosTable,enemy_table[i]);
			end
		end
	end
	if	Battle.get_BattleType(ARG0) == BT_PVE
	then
		local roll = math.random(1,100);
		if roll <= 75
		then
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.ai_pushOrder(RECEIVER,random_pos,8);
		else
			local index = math.ceil(math.random(1,table.maxn(nodebuffPosTable)));
			Battle.ai_pushOrder(RECEIVER,nodebuffPosTable[index],2251);
		end
	else
		local roll = math.random(1,100);
		if roll <= 50
		then
			local index = math.ceil(math.random(1,table.maxn(nodebuffPosTable)));
			Battle.ai_pushOrder(RECEIVER,nodebuffPosTable[index],2271);
		else
			local index = math.ceil(math.random(1,table.maxn(nodebuffPosTable)));
			Battle.ai_pushOrder(RECEIVER,nodebuffPosTable[index],2251);
		end
	end
end

function ZhouShu_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)

end