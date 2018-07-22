Sys.log("load wushi1.lua");

function WuShi1_TongYongLiuCheng(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
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

function WuShi1_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getSneakAttack(ARG0) == SAT_SurpriseAttack then
		return;
	end
	--3.如果己方有人死了，则使用气绝魔法
	--2.如果己方身上有不良BUFF则使用洁净
	--1.如果己方单位身上没有恢复效果，则使用强力恢复魔法。
	--4.以上条件都不满足则攻击
	
	local myForce = check_group_type(ARG1);
	--己方死亡列表
	local dieTable = isDeadthByForce(ARG0,ARG3,myForce);
	--如果有队员死亡并且有气绝技能50%概率释放
	if table.maxn(dieTable) > 0 
	then
		for i=1,table.getn(dieTable),1
		do
			if Battle.check_Order(ARG0,dieTable[i],2391) == false then
				Battle.ai_pushOrder(RECEIVER,dieTable[i],2391);
				return
			end
		end
		local index = math.ceil(math.random(1,table.maxn(dieTable)));
		Battle.ai_pushOrder(RECEIVER,dieTable[index],2391);
		return;
	end
	
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	local debuffPosTable = {};
	local recoverPosTable = {};
	for i=1, table.getn(my_table), 1
	do
		for j=ST_Poison,ST_Forget,1
		do
			if Battle.check_state(ARG0,my_table[i],j)
			then
				table.insert(debuffPosTable,my_table[i]);
			end
		end
	end
	if table.maxn(debuffPosTable) ~= 0
	then
		local index = math.ceil(math.random(1,table.maxn(debuffPosTable)));
		Battle.ai_pushOrder(RECEIVER,debuffPosTable[index],2401);
		return;
	end
	
	for i=1, table.getn(my_table), 1
	do
		local isRecover = not Battle.check_state(ARG0,my_table[i],ST_Recover);
		local isSRecover = not Battle.check_state(ARG0,my_table[i],ST_StrongRecover);
		local isGRecover = not Battle.check_state(ARG0,my_table[i],ST_GroupRecover);
		if  isRecover and isSRecover and isGRecover
		then
			table.insert(recoverPosTable,my_table[i]);
		end
	end
	if table.maxn(recoverPosTable) ~= 0
	then
		local index = math.ceil(math.random(1,table.maxn(recoverPosTable)));
		Battle.ai_pushOrder(RECEIVER,recoverPosTable[index],2421);
	else
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,8);
	end
end

function WuShi1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	--3.如果己方有人死了，则使用气绝魔法
	--2.如果己方身上有不良BUFF则使用洁净
	--1.如果己方单位身上没有恢复效果，则使用强力恢复魔法。
	--4.以上条件都不满足则攻击
	
	local myForce = check_group_type(ARG1);
	--己方死亡列表
	local dieTable = isDeadthByForce(ARG0,ARG3,myForce);
	--如果有队员死亡并且有气绝技能50%概率释放
	if table.maxn(dieTable) > 0 
	then
		local index = math.ceil(math.random(1,table.maxn(dieTable)));
		Battle.ai_pushOrder(RECEIVER,dieTable[index],2391);
		return;
	end
	
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	local debuffPosTable = {};
	local recoverPosTable = {};
	for i=1, table.getn(my_table), 1
	do
		for j=ST_Poison,ST_Forget,1
		do
			if Battle.check_state(ARG0,my_table[i],j)
			then
				table.insert(debuffPosTable,my_table[i]);
			end
		end
	end
	if table.maxn(debuffPosTable) ~= 0
	then
		local index = math.ceil(math.random(1,table.maxn(debuffPosTable)));
		Battle.ai_pushOrder(RECEIVER,debuffPosTable[index],2401);
		return;
	end
	
	for i=1, table.getn(my_table), 1
	do
		local isRecover = not Battle.check_state(ARG0,my_table[i],ST_Recover);
		local isSRecover = not Battle.check_state(ARG0,my_table[i],ST_StrongRecover);
		local isGRecover = not Battle.check_state(ARG0,my_table[i],ST_GroupRecover);
		if  isRecover and isSRecover and isGRecover
		then
			table.insert(recoverPosTable,my_table[i]);
		end
	end
	if table.maxn(recoverPosTable) ~= 0
	then
		local index = math.ceil(math.random(1,table.maxn(recoverPosTable)));
		Battle.ai_pushOrder(RECEIVER,recoverPosTable[index],2421);
	else
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,8);
	end
end

function WuShi1_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	
end





















