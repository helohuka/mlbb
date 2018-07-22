Sys.log("load XieZiKingAKS_Boss.lua");

function XieZiKingAKS_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.random(1,10);
	local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	if	roll <= 3
	then
		Battle.ai_pushOrder(RECEIVER,random_pos,1001);
	elseif roll > 3 and roll <= 6
	then
		Battle.ai_pushOrder(RECEIVER,random_pos,1061);
	elseif roll > 6 and roll <= 9
	then
		Battle.ai_pushOrder(RECEIVER,random_pos,4020);
	else
		Battle.ai_pushOrder(RECEIVER,random_pos,1031);
	end
end

function XieZiKingAKS_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		XieZiKingAKS_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		XieZiKingAKS_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function XieZiKingAKS_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		XieZiKingAKS_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		XieZiKingAKS_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function XieZiKingAKS_deadth(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	
end
