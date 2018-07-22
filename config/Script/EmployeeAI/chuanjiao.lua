Sys.log("load chuanjiao.lua");

function ChuanJiao_TongYongLiuCheng(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
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

function ChuanJiao_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getSneakAttack(ARG0) == SAT_SurpriseAttack then
		return;
	end
	
	local myForce = check_group_type(ARG1);
	--己方死亡列表
	local dieTable = isDeadthByForce(ARG0,ARG3,myForce);
	--1.如果己方有人死了使用气绝。
	if table.maxn(dieTable) > 0 then
		for i=1,table.getn(dieTable),1
		do
			if Battle.check_Order(ARG0,dieTable[i],2391) == false then
				Battle.ai_pushOrder(RECEIVER,dieTable[i],2391);
				return
			end
		end
		local index = math.ceil(math.random(1,table.maxn(dieTable)));
		Battle.ai_pushOrder(RECEIVER,dieTable[index],2361);
		return;
	end
	--2.如果1-5个单位血量小于50%单补，大于5个单位血量小于50%超补。
	--3.以上条件都不满足的话，攻击
	--己方血量小于50%列表
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	local l_table = {};
	for i=1,table.getn(my_table),1
		do
			local hpRatio = Battle.get_prop(ARG0,my_table[i],PT_HpCurr)/Battle.get_prop(ARG0,my_table[i],PT_HpMax);
			if  hpRatio < 0.8
			then
				table.insert(l_table,my_table[i]);
			end
		end
		
	if table.getn(l_table) >= 3
	then
		local index = math.ceil(math.random(1,table.maxn(l_table)));
		Battle.ai_pushOrder(RECEIVER,l_table[index],2381);
	elseif table.getn(l_table) < 3 and table.getn(l_table) >= 1
	then
		local index = math.ceil(math.random(1,table.maxn(l_table)));
		Battle.ai_pushOrder(RECEIVER,l_table[index],2361);
	else
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,8);
	end
end

function ChuanJiao_Updata(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	local myForce = check_group_type(ARG1);
	--己方死亡列表
	local dieTable = isDeadthByForce(ARG0,ARG3,myForce);
	--1.如果己方有人死了使用气绝。
	if table.maxn(dieTable) > 0 then
		for i=1,table.getn(dieTable),1
		do
			if Battle.check_Order(ARG0,dieTable[i],2391) == false then
				Battle.ai_pushOrder(RECEIVER,dieTable[i],2391);
				return
			end
		end
		local index = math.ceil(math.random(1,table.maxn(dieTable)));
		Battle.ai_pushOrder(RECEIVER,dieTable[index],2361);
		return;
	end
	--2.如果1-5个单位血量小于50%单补，大于5个单位血量小于50%超补。
	--3.以上条件都不满足的话，攻击
	--己方血量小于50%列表
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	local l_table = {};
	for i=1,table.getn(my_table),1
		do
			local hpRatio = Battle.get_prop(ARG0,my_table[i],PT_HpCurr)/Battle.get_prop(ARG0,my_table[i],PT_HpMax);
			if  hpRatio < 0.8
			then
				table.insert(l_table,my_table[i]);
			end
		end
		
	if table.getn(l_table) >= 3
	then
		local index = math.ceil(math.random(1,table.maxn(l_table)));
		Battle.ai_pushOrder(RECEIVER,l_table[index],2381);
	elseif table.getn(l_table) < 3 and table.getn(l_table) >= 1
	then
		local index = math.ceil(math.random(1,table.maxn(l_table)));
		Battle.ai_pushOrder(RECEIVER,l_table[index],2361);
	else
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,8);
	end
end

function chuanjiao_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)

end