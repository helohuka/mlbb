Sys.log("load XiYiRenZhaGe.lua");
--RECEIVER
--ARG_BATTLEID,		//0
--ARG_CASTERPOS,		//1
--ARG_TARGETPOS,		//2
--ARG_POSTABLE,		//3
--ARG_SKILLTABLE		//4

function XiYiRenZhaGe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local num = table.maxn(ARG4)
	if num == 0 then
		return;
	end
	local index = 0;
	local TargetPos = 0;
	local skillId = 0;
	index = math.ceil(math.random(1,num));
	while ARG4[index] == 2291 do
		index = math.ceil(math.random(1,num));
	end
	skillId = ARG4[index];
	
	local myGroup = get_opposite_pos(ARG0, ARG3, ARG1);
	
	if skillId == 1 or skillId == 2101 then
		TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	else
		local tmp = math.ceil(math.random(1,table.maxn(myGroup)));
		TargetPos = myGroup[tmp];
	end
	
	if table.maxn(myGroup) <= 4 then
		if Battle.getRound(ARG0)% 2 then
			skillId = 2291;
			TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		else
			skillId = 2101;
			TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		end
	end
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

function XiYiRenZhaGe_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		XiYiRenZhaGe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		XiYiRenZhaGe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function XiYiRenZhaGe_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		XiYiRenZhaGe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		XiYiRenZhaGe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function XiYiRenZhaGe_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)

end