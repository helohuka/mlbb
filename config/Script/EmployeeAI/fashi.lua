Sys.log("load fashi.lua");

function FaShi_TongYongLiuCheng(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
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

function FaShi_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getSneakAttack(ARG0) == SAT_SurpriseAttack then
		return;
	end
		--1.如果对面都是怪物（练级情况下，包括BOSS），小于5人单体各50%随便，小于5则超强
		local enemy_table = get_enemy_table(ARG0,ARG3,ARG1);	
		local enemyNum = table.maxn(enemy_table);
		if enemyNum >= 4 
		then
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.ai_pushOrder(RECEIVER,random_pos,2081);
		else
			local roll = math.random(1,10);
			if roll <= 5 
			then 
				local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
				Battle.ai_pushOrder(RECEIVER,random_pos,2011);
			else
				local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
				Battle.ai_pushOrder(RECEIVER,random_pos,2031);
			end
		end
end

function FaShi_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
		--1.如果对面都是怪物（练级情况下，包括BOSS），小于5人单体各50%随便，小于5则超强
		local enemy_table = get_enemy_table(ARG0,ARG3,ARG1);	
		local enemyNum = table.maxn(enemy_table);
		if enemyNum >= 4 
		then
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.ai_pushOrder(RECEIVER,random_pos,2081);
		else
			local roll = math.random(1,10);
			if roll <= 5 
			then 
				local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
				Battle.ai_pushOrder(RECEIVER,random_pos,2011);
			else
				local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
				Battle.ai_pushOrder(RECEIVER,random_pos,2031);
			end
		end

end

function FaShi_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	
end
