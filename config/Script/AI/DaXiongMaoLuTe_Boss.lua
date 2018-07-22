Sys.log("load DaXiongMaoLuTe_Boss.lua");

function DaXiongMaoLuTe_UpdatePushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	local index = math.ceil(math.random(1,table.maxn(ARG4)));
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	
	if table.maxn(my_table) < 2
	then
		Battle.ai_pushOrder(RECEIVER,ARG1,5507);
	else
		while ARG4[index] == 5507
		do
			index = math.ceil(math.random(1,table.maxn(ARG4)));
		end
		--随机使用除了召唤以外的所有技能
		if Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_Team then
			--选择自己阵营的一个随机目标
			random_pos = random_TargetPos(ARG0, ARG2, ARG1, ARG3, false)
		end
		Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
	end
end

function DaXiongMaoLuTe_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		Battle.ai_pushOrder(RECEIVER,ARG1,5507);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		Battle.ai_pushOrder(RECEIVER,ARG1,5507);
	end
end

function DaXiongMaoLuTe_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		DaXiongMaoLuTe_UpdatePushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		DaXiongMaoLuTe_UpdatePushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function DaXiongMaoLuTe_Deadth(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	
end