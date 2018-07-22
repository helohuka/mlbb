Sys.log("load ShiMoXiangKeLuTe_Boss.lua");

function ShiMoXiangKeLuTe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local hpRatio = Battle.get_prop(ARG0,ARG1,PT_HpCurr)/Battle.get_prop(ARG0,ARG1,PT_HpMax);
	if hpRatio < 0.3
	then
		--血量少于30%以后，30%概率使用明镜，其他概率随机使用连击、诸刃、乾坤
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local roll = math.random(1,10);
		if roll <= 3 then
			Battle.ai_pushOrder(RECEIVER,random_pos,5504);
		else
			local index = math.ceil(math.random(1,table.maxn(ARG4)));
			while ARG4[index] ~= 1001 and ARG4[index] ~= 1011 and ARG4[index] ~= 1061
			do
				index = math.ceil(math.random(1,table.maxn(ARG4)));
			end
			Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
		end
	else
		--随机使用睡眠攻击、恢复、连击、乾坤、诸刃、攻击
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local index = math.ceil(math.random(1,table.maxn(ARG4)));
		while ARG4[index] == 2
		do
			index = math.ceil(math.random(1,table.maxn(ARG4)));
		end
		if Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_Team 
		then
			--选择自己阵营的一个随机目标
			random_pos = random_TargetPos(ARG0, ARG2, ARG1, ARG3, false)
		end
		Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
	end
end

function ShiMoXiangKeLuTe_Bron(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		ShiMoXiangKeLuTe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		ShiMoXiangKeLuTe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function ShiMoXiangKeLuTe_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		ShiMoXiangKeLuTe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		ShiMoXiangKeLuTe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function ShiMoXiangKeLuTe_Deadth(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)

end