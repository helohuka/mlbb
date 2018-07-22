Sys.log("load DaodanGui.lua");
--RECEIVER
--ARG0,		//0
--ARG1,		//1
--ARG2,		//2
--ARG3,		//3
--ARG4		//4

function DaodanGui_BornPushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local num = table.maxn(ARG4)
	if num == 0 then
		return;
	end
	local index = math.ceil(math.random(1,num));
	local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	Battle.ai_pushOrder(RECEIVER,TargetPos,ARG4[index]);
end

function DaodanGui_UpdatePushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local num = table.maxn(ARG4)
	if num == 0 then
		return;
	end
	local index = math.ceil(math.random(1,num));
	local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
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

function DaodanGui_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		DaodanGui_BornPushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		DaodanGui_BornPushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function DaodanGui_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		DaodanGui_UpdatePushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		DaodanGui_UpdatePushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function DaodanGui_Deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	
end