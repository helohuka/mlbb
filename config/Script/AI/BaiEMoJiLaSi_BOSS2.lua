Sys.log("load BaiEMoJiLaSi_BOSS2.lua");

function BaiEMoJiLaSi_BOSS2_pushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
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
	if hpRatio < 0.5
	then
		--生命值少于50%以后随机使用超强冰冻、超强风刃、吸血魔法
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local skillTabel = {2091,2111,2231};
		local index = math.ceil(math.random(1,table.maxn(skillTabel)));
		Battle.ai_pushOrder(RECEIVER,random_pos,skillTabel[index]);
		return;
	end
	--影子少于2个以后，随机使用攻击反弹、魔法反弹，强力中毒魔法
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);  --my_table包含BOSS自己,要减一
	local num = table.maxn(my_table) - 1; --影子数量
	if num < 2
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local skillTabel = {2461,2471,2191};
		local index = math.ceil(math.random(1,table.maxn(skillTabel)));
		Battle.ai_pushOrder(RECEIVER,random_pos,skillTabel[index]);
	else
		--随机使用攻击、吸血魔法、攻击反弹、魔法反弹
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local skillTabel = {1,2461,2471,2231};
		local index = math.ceil(math.random(1,table.maxn(skillTabel)));
		Battle.ai_pushOrder(RECEIVER,random_pos,skillTabel[index]);
	end
end

function BaiEMoJiLaSi_BOSS2_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		BaiEMoJiLaSi_BOSS2_pushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		BaiEMoJiLaSi_BOSS2_pushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function BaiEMoJiLaSi_BOSS2_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		BaiEMoJiLaSi_BOSS2_pushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		BaiEMoJiLaSi_BOSS2_pushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function BaiEMoJiLaSi_BOSS2_Deadth(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)

end

