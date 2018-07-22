Sys.log("load ShiRenMoAoZi.lua");
--RECEIVER
--ARG_BATTLEID,		//0
--ARG_CASTERPOS,		//1
--ARG_TARGETPOS,		//2
--ARG_POSTABLE,		//3
--ARG_SKILLTABLE		//4

function ShiRenMoAoZi_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local num = table.maxn(ARG4)
	if num == 0 then
		return;
	end
	
	local index = 0;
	local skillID = 0;
	
	local roll = math.ceil(math.random(1,100));
	
	if roll <= 10 then
		skillID = 1091;
	else
		index = math.ceil(math.random(1,num));
		
		skillID = ARG4[index];
		
		while (ARG4[index] == 1091) do
			index = math.ceil(math.random(1,num));
		end
		
		skillID = ARG4[index];
	end
	
	local TargetPos = 0;
	
	if skillID == 1091 then
		local group = check_group_type(ARG2);
		if group ~= GT_None then
			TargetPos = Battle.getMaxMpPos(ARG0,group);
		end
	else
		TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	end
	if skillID == 2 then
		local index1 = math.ceil(math.random(1,100));
		if index1 <= 10 then
			Battle.ai_pushOrder(RECEIVER,ARG1,2);
		else
			Battle.ai_pushOrder(RECEIVER,TargetPos,1);
		end
		return
	end
	Battle.ai_pushOrder(RECEIVER,TargetPos,skillID);
end

function ShiRenMoAoZi_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		ShiRenMoAoZi_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		ShiRenMoAoZi_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function ShiRenMoAoZi_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		ShiRenMoAoZi_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		ShiRenMoAoZi_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function ShiRenMoAoZi_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)

end