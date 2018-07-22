Sys.log("load YinLongAiWeiDa_Boss.lua");

function YinLongAiWeiDa_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local enemy_table = get_opposite_pos(ARG0,ARG3,ARG2);
	local debuffPosTable = {};
	local recoverPosTable = {};
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
	
	if table.maxn(debuffPosTable) <= 3
	then
		--如果对面敌人身上的debuff少于3个，则随机使用超强酒醉、超强中毒魔法、超强混乱魔法
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local skillTable = {2281,2251,2291};
		local index = math.ceil(math.random(1,table.maxn(skillTable)));
		Battle.ai_pushOrder(RECEIVER,random_pos,skillTable[index]);
		
		--Sys.log("YinLongAiWeiDa_PushOrder table.maxn(debuffPosTable) <= 3 table.maxn(debuffPosTable)=="..table.maxn(debuffPosTable));
	else
		--dubuff多于3个时，随机使用攻击、乾坤、连击
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local skillTable = {1,1061,1001};
		local index = math.ceil(math.random(1,table.maxn(skillTable)));
		Battle.ai_pushOrder(RECEIVER,random_pos,skillTable[index]);
		--Sys.log("YinLongAiWeiDa_PushOrder table.maxn(debuffPosTable) > 3 table.maxn(debuffPosTable)=="..table.maxn(debuffPosTable));
	end
end

function YinLongAiWeiDa_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		YinLongAiWeiDa_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		YinLongAiWeiDa_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function YinLongAiWeiDa_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		YinLongAiWeiDa_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		YinLongAiWeiDa_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function YinLongAiWeiDa_Deadth(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)

end