Sys.log("load ShiRenMoShouLingLuoKe.lua");
--RECEIVER
--ARG_BATTLEID,		//0
--ARG_CASTERPOS,		//1
--ARG_TARGETPOS,		//2
--ARG_POSTABLE,		//3
--ARG_SKILLTABLE		//4

function ShiRenMoShouLingLuoKe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local num = table.maxn(ARG4)
	if num == 0 then
		return;
	end
	
	local myCurHp = Battle.get_prop(ARG0,ARG1,PT_HpCurr);
	local myMaxHp = Battle.get_prop(ARG0,ARG1,PT_HpMax);
	
	if myMaxHp == 0 then
		return;
	end
	
	local index = math.ceil(math.random(1,num));
	
	if myCurHp/myMaxHp >= 0.5 then
		while (ARG4[index] == 1091 or ARG4[index] == 2501) do
			index = math.ceil(math.random(1,num));
		end
	end
	
	local TargetPos = 0;
	
	if ARG4[index] == 1011 then
		local group = check_group_type(ARG2)
		if group ~= GT_None then
			TargetPos = Battle.getMinHpPos(ARG0,group);
		end
	else
		TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	end
	if ARG4[index] == 2 then
		local index1 = math.ceil(math.random(1,100));
		if index1 <= 10 then
			Battle.ai_pushOrder(RECEIVER,ARG1,2);
		else
			Battle.ai_pushOrder(RECEIVER,TargetPos,1);
		end
		return
	end
	Battle.ai_pushOrder(RECEIVER,TargetPos,ARG4[index]);
end

function ShiRenMoShouLingLuoKe_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		ShiRenMoShouLingLuoKe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		ShiRenMoShouLingLuoKe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function ShiRenMoShouLingLuoKe_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		ShiRenMoShouLingLuoKe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		ShiRenMoShouLingLuoKe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function ShiRenMoShouLingLuoKe_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)

end