Sys.log("load ShaYingCiKeDaShiLuoSaier_Boss.lua");

function ShaYingCiKeDaShiLuoSaier_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local num = table.maxn(ARG4)
	if num == 0 then
		return;
	end
	local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	local roll = math.random(1,100);
	if roll <= 40 
	then
		--连击概率40%
		Battle.ai_pushOrder(RECEIVER,random_pos,1001);
	elseif roll >= 60
	then
		--乾坤概率40%
		Battle.ai_pushOrder(RECEIVER,random_pos,1061);
	else
		local index = math.ceil(math.random(1,num));
		while(ARG4[index] == 1001 or ARG4[index] == 1061)
		do
			index = math.ceil(math.random(1,num));
		end
		Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
	end
end

function ShaYingCiKeDaShiLuoSaier_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		ShaYingCiKeDaShiLuoSaier_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		ShaYingCiKeDaShiLuoSaier_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function ShaYingCiKeDaShiLuoSaier_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		ShaYingCiKeDaShiLuoSaier_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		ShaYingCiKeDaShiLuoSaier_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function ShaYingCiKeDaShiLuoSaier_deadth(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	
end
