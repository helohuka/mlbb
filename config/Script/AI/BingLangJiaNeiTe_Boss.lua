Sys.log("load BingLangJiaNeiTe_Boss.lua");

function BingLangJiaNeiTe_pushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local enemy_table = get_opposite_pos(ARG0,ARG3,ARG2);
	local isBuff = false;
	for i=1, table.getn(enemy_table), 1
	do
		for j=ST_ActionAbsorb,ST_MagicInvalid,1
		do
			if j ~= ST_Shield and Battle.check_state(ARG0,enemy_table[i],j)
			then
				isBuff = true;
			end
		end
	end
	if isBuff
	then
		--如果对面的人身上有buff状态，则使用强力即死
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,5503);
		return;
	end
	
	local hpRatio = Battle.get_prop(ARG0,ARG1,PT_HpCurr)/Battle.get_prop(ARG0,ARG1,PT_HpMax);
	if hpRatio < 0.3
	then
		--如果血量少于30%，则随机使用攻击、超强冰冻、强力冰冻、吸血魔法
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local skillTabel = {1,2091,2051,2121};
		local index = math.ceil(math.random(1,table.maxn(skillTabel)));
		Battle.ai_pushOrder(RECEIVER,random_pos,skillTabel[index]);
	else
		--随机使用攻击、连击、反击
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local skillTabel = {1,1001,1021};
		local index = math.ceil(math.random(1,table.maxn(skillTabel)));
		Battle.ai_pushOrder(RECEIVER,random_pos,skillTabel[index]);
	end
end

function BingLangJiaNeiTe_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		BingLangJiaNeiTe_pushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		BingLangJiaNeiTe_pushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function BingLangJiaNeiTe_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		BingLangJiaNeiTe_pushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		BingLangJiaNeiTe_pushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function BingLangJiaNeiTe_Deadth(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	
end