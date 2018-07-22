Sys.log("load JuXiong.lua");
--RECEIVER
--ARG_BATTLEID,		//0
--ARG_CASTERPOS,		//1
--ARG_TARGETPOS,		//2
--ARG_POSTABLE,		//3
--ARG_SKILLTABLE		//4

function JuXiong_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local num = table.maxn(ARG4)
	if num == 0 then
		return;
	end
	
	local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	
	local myCurHp = Battle.get_prop(ARG0,ARG1,PT_HpCurr);
	local myMaxHp = Battle.get_prop(ARG0,ARG1,PT_HpMax);
	
	local ratio = myCurHp/myMaxHp;
	local index = math.ceil(math.random(1,num));
	
	if ratio > 0.3 then
		while ARG4[index] == 2501 do
			index = math.ceil(math.random(1,num));
		end
	end
	Battle.ai_pushOrder(RECEIVER,TargetPos,ARG4[index]);
end

function JuXiong_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		JuXiong_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		JuXiong_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function JuXiong_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		JuXiong_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		JuXiong_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function JuXiong_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)

end