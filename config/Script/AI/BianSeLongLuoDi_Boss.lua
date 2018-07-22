Sys.log("load BianSeLongLuoDi_Boss.lua");

function BianSeLongLuoDi_pushOrder(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	local num = table.maxn(ARG4)
	if num == 0 then
		return;
	end
	local hpRatio = Battle.get_prop(ARG0,ARG1,PT_HpCurr)/Battle.get_prop(ARG0,ARG1,PT_HpMax);
	if hpRatio < 0.3
	then
		local roll = math.random(1,10);
		if roll < 5
		then
			Battle.ai_pushOrder(RECEIVER,ARG1,1091);
		elseif roll > 8
		then
			Battle.ai_pushOrder(RECEIVER,ARG1,2);
		else
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.ai_pushOrder(RECEIVER,random_pos,1061);
		end
	else
		local roll = math.random(1,10);
		if roll < 5
		then
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.ai_pushOrder(RECEIVER,random_pos,1061);
		else
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			local index = math.ceil(math.random(1,num));
			while(ARG4[index] == 1061)
			do
				index = math.ceil(math.random(1,num));
			end
			if Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_Team 
			then
				--选择自己阵营的一个随机目标
				random_pos = random_TargetPos(ARG0, ARG2, ARG1, ARG3, false)
			end
			Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
		end
	end
end

function BianSeLongLuoDi_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		BianSeLongLuoDi_pushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		BianSeLongLuoDi_pushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function BianSeLongLuoDi_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		BianSeLongLuoDi_pushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		BianSeLongLuoDi_pushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function BianSeLongLuoDi_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)

end