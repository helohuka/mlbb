Sys.log("load JuXingSiWangRuoChong_Boss.lua");

function JuXingSiWangRuoChong_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.random(1,10);
	local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	if	roll <= 5
	then
		Battle.ai_pushOrder(RECEIVER,random_pos,1041);
	elseif roll > 5 and roll <= 8
	then
		Battle.ai_pushOrder(RECEIVER,random_pos,1061);
	elseif roll == 9
	then
		Battle.ai_pushOrder(RECEIVER,random_pos,2501);
	else
		Battle.ai_pushOrder(RECEIVER,random_pos,1);
	end
end

function JuXingSiWangRuoChong_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		JuXingSiWangRuoChong_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		JuXingSiWangRuoChong_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function JuXingSiWangRuoChong_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		JuXingSiWangRuoChong_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		JuXingSiWangRuoChong_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function JuXingSiWangRuoChong_deadth(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	
end