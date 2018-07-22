Sys.log("load XiYiRenMoLuo_Boss.lua");

function XiYiRenMoLuo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local num = table.maxn(ARG4)
	if num == 0 then
		return;
	end
	local group_type = check_group_type(ARG2);
	local minHpPos = Battle.getMinHpPos(ARG0,group_type);
	local hpRatio = Battle.get_prop(ARG0,ARG1,PT_HpCurr)/Battle.get_prop(ARG0,ARG1,PT_HpMax);
	if	hpRatio < 0.3
	then
		local roll = math.random(1,10);
		if	roll < 3
		then
			Battle.ai_pushOrder(RECEIVER,ARG1,1091);
		else
			local index = math.ceil(math.random(1,num));
			while(ARG4[index] == 1091)
			do
				index = math.ceil(math.random(1,num));
			end
			Battle.ai_pushOrder(RECEIVER,minHpPos,ARG4[index]);
		end
	else
		local index = math.ceil(math.random(1,num));
		if ARG4[index] == 1091
		then
			Battle.ai_pushOrder(RECEIVER,ARG1,ARG4[index]);
		else
			local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.ai_pushOrder(RECEIVER,TargetPos,ARG4[index]);
		end
	end
end

function XiYiRenMoLuo_Bron(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		XiYiRenMoLuo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		XiYiRenMoLuo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function XiYiRenMoLuo_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		XiYiRenMoLuo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		XiYiRenMoLuo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function XiYiRenMoLuo_deadth(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)

end