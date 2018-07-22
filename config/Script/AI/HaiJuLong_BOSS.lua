Sys.log("load HaiJuLong.lua");
--RECEIVER
--ARG_BATTLEID,		//0
--ARG_CASTERPOS,		//1
--ARG_TARGETPOS,		//2
--ARG_POSTABLE,		//3
--ARG_SKILLTABLE		//4

function HaiJuLong_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local num = table.maxn(ARG4)
	if num == 0 then
		return;
	end
	local index = 0;
	local TargetPos = 0;
	local skillId = 0;
	
	local myCurHp = Battle.get_prop(ARG0,ARG1,PT_HpCurr);
	local myMaxHp = Battle.get_prop(ARG0,ARG1,PT_HpMax);
	
	if myMaxHp == 0 then
		return;
	end
	
	local index = math.ceil(math.random(1,num));
	
	if myCurHp/myMaxHp >= 0.3 then
		while ARG4[index] == 2511 or ARG4[index] == 2501 or ARG4[index] == 2291 do
			index = math.ceil(math.random(1,num));
		end
	else
		while ARG4[index] ~= 2511 and ARG4[index] ~= 2501 and ARG4[index] ~= 2291 do
			index = math.ceil(math.random(1,num));
		end
	end
	
	skillId = ARG4[index];
	
	local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	if skillId == 2 then
		local index1 = math.ceil(math.random(1,100));
		if index1 <= 10 then
			Battle.ai_pushOrder(RECEIVER,ARG1,2);
		else
			Battle.ai_pushOrder(RECEIVER,TargetPos,1);
		end
		return
	end
	if skillId == 2511 then
		skillId = 1;
	end
	Battle.ai_pushOrder(RECEIVER,TargetPos,skillId);
end

function HaiJuLong_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		HaiJuLong_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		HaiJuLong_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function HaiJuLong_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		HaiJuLong_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		HaiJuLong_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function HaiJuLong_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)

end