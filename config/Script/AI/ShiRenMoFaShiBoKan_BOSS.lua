Sys.log("load ShiRenMoFaShiBoKan.lua");
--RECEIVER
--ARG_BATTLEID,		//0
--ARG_CASTERPOS,		//1
--ARG_TARGETPOS,		//2
--ARG_POSTABLE,		//3
--ARG_SKILLTABLE		//4

function ShiRenMoFaShiBoKan_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local num = table.maxn(ARG4)
	if num == 0 then
		return;
	end
	local index = 0;
	local TargetPos = 0;
	local skillId = 0;
	index = math.ceil(math.random(1,num));
	while ARG4[index] ~= 1 and ARG4[index] ~= 1021 and ARG4[index] ~= 1051 and ARG4[index] ~= 1061 do
		index = math.ceil(math.random(1,num));
	end
	
	local myCurHp = Battle.get_prop(ARG0,ARG1,PT_HpCurr);
	local myMaxHp = Battle.get_prop(ARG0,ARG1,PT_HpMax);
	if myCurHp/myMaxHp < 0.5 then
		while ARG4[index] ~= 2081 and ARG4[index] ~= 2091 and ARG4[index] ~= 2101 and ARG4[index] ~= 2111 do
			index = math.ceil(math.random(1,num));
		end
	end
	
	skillId = ARG4[index];
	
	if Battle.CheckHp(ARG0, GT_Up, 0.3) then
		if Battle.getRound(ARG0)% 2 then
			skillId = 1021;
		else
			while ARG4[index] ~= 2081 and ARG4[index] ~= 2091 and ARG4[index] ~= 2101 and ARG4[index] ~= 2111 do
				index = math.ceil(math.random(1,num));
			end
			
			skillId = ARG4[index];
		end
	end
	
	TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	if skillId == 2 then
		local index1 = math.ceil(math.random(1,100));
		if index1 <= 10 then
			Battle.ai_pushOrder(RECEIVER,ARG1,2);
		else
			Battle.ai_pushOrder(RECEIVER,TargetPos,1);
		end
		return
	end
	Battle.ai_pushOrder(RECEIVER,TargetPos,skillId);
end

function ShiRenMoFaShiBoKan_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		ShiRenMoFaShiBoKan_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		ShiRenMoFaShiBoKan_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function ShiRenMoFaShiBoKan_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		ShiRenMoFaShiBoKan_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		ShiRenMoFaShiBoKan_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function ShiRenMoFaShiBoKan_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)

end