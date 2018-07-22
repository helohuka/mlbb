Sys.log("load JunFaPiNiSi_Boss.lua");

function JunFaPiNiSi_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local index = math.ceil(math.random(1,table.maxn(ARG4)));
		while ARG4[index] == 2
		do
			index = math.ceil(math.random(1,table.maxn(ARG4)));
		end
		Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
		--2
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		--2
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local index = math.ceil(math.random(1,table.maxn(ARG4)));
		while ARG4[index] == 2
		do
			index = math.ceil(math.random(1,table.maxn(ARG4)));
		end
		Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
	end
end

function JunFaPiNiSi_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local index = math.ceil(math.random(1,table.maxn(ARG4)));
		while ARG4[index] == 2
		do
			index = math.ceil(math.random(1,table.maxn(ARG4)));
		end
		Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
		--2
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		--2
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local index = math.ceil(math.random(1,table.maxn(ARG4)));
		while ARG4[index] == 2
		do
			index = math.ceil(math.random(1,table.maxn(ARG4)));
		end
		Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
	end
end

function JunFaPiNiSi_Deadth(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	
end