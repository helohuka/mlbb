Sys.log("load fashi1.lua");

function FaShi1_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getSneakAttack(ARG0) == SAT_SurpriseAttack then
		return;
	end
	
		--1.如果对面都是怪物（练级情况下，包括BOSS），小于5人单体各50%随便，小于5则超强
		local enemy_table = get_enemy_table(ARG0,ARG3,ARG1);	
		local enemyNum = table.maxn(enemy_table);
		if enemyNum >= 4 
		then
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.ai_pushOrder(RECEIVER,random_pos,2101);
		else
			local roll = math.random(1,10);
			if roll <= 5 
			then 
				local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
				Battle.ai_pushOrder(RECEIVER,random_pos,2021);
			else
				local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
				Battle.ai_pushOrder(RECEIVER,random_pos,2001);
			end
		end
	
end

function FaShi1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)

		--1.如果对面都是怪物（练级情况下，包括BOSS），小于5人单体各50%随便，小于5则超强
		local enemy_table = get_enemy_table(ARG0,ARG3,ARG1);	
		local enemyNum = table.maxn(enemy_table);
		if enemyNum >= 4 
		then
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.ai_pushOrder(RECEIVER,random_pos,2101);
		else
			local roll = math.random(1,10);
			if roll <= 5 
			then 
				local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
				Battle.ai_pushOrder(RECEIVER,random_pos,2021);
			else
				local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
				Battle.ai_pushOrder(RECEIVER,random_pos,2001);
			end
		end
end

function FaShi1_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	
end
