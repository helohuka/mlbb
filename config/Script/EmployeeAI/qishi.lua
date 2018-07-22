Sys.log("load qishi.lua");

function QiShi_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getSneakAttack(ARG0) == SAT_SurpriseAttack then
		return;
	end
	--1.如果己方有目标血量少于30%，则对这个目标使用护卫技能
	--2.70%概率使用诸刃技能，20%概率使用反击技能,10概率随机其他技能
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	local pos = BP_None;
	for i=1,table.getn(my_table),1
	do
		local hpRatio = Battle.get_prop(ARG0,my_table[i],PT_HpCurr)/Battle.get_prop(ARG0,my_table[i],PT_HpMax)
	
		if hpRatio < 0.5 and hpRatio ~= 0
		then
			pos = my_table[i];
			break;
		end
	end
	
	if pos ~= BP_None
	then
		if pos == ARG1
		then
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.ai_pushOrder(RECEIVER,random_pos,9);
		else
			Battle.ai_pushOrder(RECEIVER,pos,2521);
		end
	else
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local roll = math.random(1,10);
		if roll <= 7 
		then
			Battle.ai_pushOrder(RECEIVER,random_pos,1011);
		elseif roll == 8 or roll == 9 
		then
			Battle.ai_pushOrder(RECEIVER,random_pos,1021);
		else
			local index = math.ceil(math.random(1,table.maxn(ARG4)));
			while ARG4[index] == 1011 or ARG4[index] == 1021 
			do
				index = math.ceil(math.random(1,table.maxn(ARG4)));
			end
			local skillid = ARG4[index];
			if skillid == 2521
			then
				skillid = 9;
			end
			Battle.ai_pushOrder(RECEIVER,random_pos,skillid);
		end
	end
end

function QiShi_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	--1.如果己方有目标血量少于30%，则对这个目标使用护卫技能
	--2.70%概率使用诸刃技能，20%概率使用反击技能,10概率随机其他技能
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	local pos = BP_None;
	for i=1,table.getn(my_table),1
	do
		local hpRatio = Battle.get_prop(ARG0,my_table[i],PT_HpCurr)/Battle.get_prop(ARG0,my_table[i],PT_HpMax)
	
		if hpRatio < 0.5 and hpRatio ~= 0
		then
			pos = my_table[i];
			break;
		end
	end
	
	if pos ~= BP_None
	then
		if pos == ARG1
		then
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.ai_pushOrder(RECEIVER,random_pos,9);
		else
			Battle.ai_pushOrder(RECEIVER,pos,2521);
		end
	else
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local roll = math.random(1,10);
		if roll <= 7 
		then
			Battle.ai_pushOrder(RECEIVER,random_pos,1011);
		elseif roll == 8 or roll == 9 
		then
			Battle.ai_pushOrder(RECEIVER,random_pos,1021);
		else
			local index = math.ceil(math.random(1,table.maxn(ARG4)));
			while ARG4[index] == 1011 or ARG4[index] == 1021
			do
				index = math.ceil(math.random(1,table.maxn(ARG4)));
			end
			local skillid = ARG4[index];
			if skillid == 2521
			then
				skillid = 9;
			end
			Battle.ai_pushOrder(RECEIVER,random_pos,skillid);
		end
	end
end

function QiShi_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)

end