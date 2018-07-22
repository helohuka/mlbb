Sys.log("load DesertTunShiHua_Boss.lua");

function DesertTunShiHua_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local num = table.maxn(ARG4)
	if num == 0 then
		return;
	end
	local enemy_table = get_opposite_pos(ARG0,ARG3,ARG2);	
	local l_table = {};
	for i=1,table.getn(enemy_table),1
	do
		local hpRatio = Battle.get_prop(ARG0,enemy_table[i],PT_HpCurr)/Battle.get_prop(ARG0,enemy_table[i],PT_HpMax);
		if  hpRatio < 0.3
		then
			table.insert(l_table,enemy_table[i]);
		end
	end
	
	if	table.maxn(l_table) ~= 0
	then
		local index = math.ceil(math.random(1,table.maxn(l_table)));
		Battle.ai_pushOrder(RECEIVER,l_table[index],2031);
	else
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local index = math.ceil(math.random(1,table.maxn(ARG4)));
		while ARG4[index] == 2031 
		do
			index = math.ceil(math.random(1,table.maxn(ARG4)));
		end
		Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
	end
end

function DesertTunShiHua_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		DesertTunShiHua_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		DesertTunShiHua_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function DesertTunShiHua_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		DesertTunShiHua_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		DesertTunShiHua_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function DesertTunShiHua_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	
end