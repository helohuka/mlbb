Sys.log("load wushi.lua");

function WuShi_TongYongLiuCheng(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
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

function WuShi_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getSneakAttack(ARG0) == SAT_SurpriseAttack then
		return;
	end
	--2.如果己方身上有不良BUFF则使用洁净
	--1.如果己方身上没有恢复效果。如果己方有1-4个单位则使用单恢复随便一个目标，4个以上则使用超恢复
	--3.以上条件都不满足则攻击
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
	if table.maxn(recoverPosTable) > 4
	then
		local index = math.ceil(math.random(1,table.maxn(recoverPosTable)));
		Battle.ai_pushOrder(RECEIVER,recoverPosTable[index],2431);
	elseif table.maxn(recoverPosTable)>=1 and table.maxn(recoverPosTable) <= 4
	then
		local index = math.ceil(math.random(1,table.maxn(recoverPosTable)));
		Battle.ai_pushOrder(RECEIVER,recoverPosTable[index],2411);
	else
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,8);
	end
end

function WuShi_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	--2.如果己方身上有不良BUFF则使用洁净
	--1.如果己方身上没有恢复效果。如果己方有1-4个单位则使用单恢复随便一个目标，4个以上则使用超恢复
	--3.以上条件都不满足则攻击
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
	if table.maxn(recoverPosTable) > 4
	then
		local index = math.ceil(math.random(1,table.maxn(recoverPosTable)));
		Battle.ai_pushOrder(RECEIVER,recoverPosTable[index],2431);
	elseif table.maxn(recoverPosTable)>=1 and table.maxn(recoverPosTable) <= 4
	then
		local index = math.ceil(math.random(1,table.maxn(recoverPosTable)));
		Battle.ai_pushOrder(RECEIVER,recoverPosTable[index],2411);
	else
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,8);
	end
end

function WuShi_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	
end





















