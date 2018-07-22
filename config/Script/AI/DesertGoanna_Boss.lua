Sys.log("load DesertGoanna_Boss.lua");--沙漠巨蜥

function DesertGoanna_BronPushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local enemy_table = get_opposite_pos(ARG0,ARG3,ARG2);
	local isBaby = false;
	for i = 1,table.getn(enemy_table),1
	do
		if Battle.isBabybyPos(enemy_table[i])
		then
			isBaby = true;
			break;
		end
	end
	--如果对面有人携带了宠物，则一直使用连击
	if isBaby
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,1001);
		return;
	end
	--如果血量少于30%，则30%概率使用明镜，其他概率随机使用乾坤、反击、攻击、连击
	local hpRatio = Battle.get_prop(ARG0,ARG1,PT_HpCurr)/Battle.get_prop(ARG0,ARG1,PT_HpMax);
	if hpRatio < 0.3
	then
		local roll = math.random(1,10);
		if roll <= 3
		then
			Battle.ai_pushOrder(RECEIVER,ARG1,1091);
		else
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			local index = math.ceil(math.random(1,table.maxn(ARG4)));
			while ARG4[index] == 1091 or ARG4[index] == 2361
			do
				index = math.ceil(math.random(1,table.maxn(ARG4)));
			end
			Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
		end
		return;
	end
	--如果都不满足则随机使用乾坤、反击、攻击、连击。
	local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	local index = math.ceil(math.random(1,table.maxn(ARG4)));
	while ARG4[index] == 1091 or ARG4[index] == 2361
	do
		index = math.ceil(math.random(1,table.maxn(ARG4)));
	end
	Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
end

function DesertGoanna_UpdatePushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	--每5回合有30%对敌对任意一个目标使用补血魔法
	local round = Battle.getRound(ARG0) + 1;
	if round%5 == 0
	then
		local group_type = check_group_type(ARG2);
		local minHpPos = Battle.getMinHpPos(ARG0,group_type);
		Battle.ai_pushOrder(RECEIVER,minHpPos,2361);
		return;
	end
	
	--如果对面有人携带了宠物，则一直使用连击
	local enemy_table = get_opposite_pos(ARG0,ARG3,ARG2);
	local isBaby = false;
	for i = 1,table.getn(enemy_table),1
	do
		if Battle.isBabybyPos(enemy_table[i])
		then
			isBaby = true;
			break;
		end
	end
	
	if isBaby
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,1001);
		return;
	end
	
	--如果血量少于30%，则30%概率使用明镜，其他概率随机使用乾坤、反击、攻击、连击
	local hpRatio = Battle.get_prop(ARG0,ARG1,PT_HpCurr)/Battle.get_prop(ARG0,ARG1,PT_HpMax);
	if hpRatio < 0.3
	then
		local roll = math.random(1,10);
		if roll <= 3
		then
			Battle.ai_pushOrder(RECEIVER,ARG1,1091);
		else
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			local index = math.ceil(math.random(1,table.maxn(ARG4)));
			while ARG4[index] == 1091 or ARG4[index] == 2361
			do
				index = math.ceil(math.random(1,table.maxn(ARG4)));
			end
			Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
		end
		return;
	end
	--如果都不满足则随机使用乾坤、反击、攻击、连击。
	local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	local index = math.ceil(math.random(1,table.maxn(ARG4)));
	while ARG4[index] == 1091 or ARG4[index] == 2361
	do
		index = math.ceil(math.random(1,table.maxn(ARG4)));
	end
	Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
end

function DesertGoanna_Bron(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		DesertGoanna_BronPushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		DesertGoanna_BronPushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function DesertGoanna_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		DesertGoanna_UpdatePushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		DesertGoanna_UpdatePushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function DesertGoanna_deadth(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	
end

