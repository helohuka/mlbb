Sys.log("load QianNianXueYao_Boss.lua");

function QianNianXueYao_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local myGroup = check_group_type(ARG1);
	local deadthTable = isDeadthByForce(ARG0,ARG3,myGroup);
	
	if table.maxn(deadthTable) > 3
	then
		--如果有小弟死3个以上，BOSS连续释放超强即死
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,5503);
	else
		--BOSS随机施放技能超强冰冻魔法、冰冻魔法、强力混乱、超强昏睡魔法、魔法反弹、魔法吸收，
		local skillTable = {2091,2011,2231,2261,2471,2451,5503};
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local index = math.ceil(math.random(1,table.maxn(skillTable)));
		while skillTable[index] == 2361
		do
			index = math.ceil(math.random(1,table.maxn(ARG4)));
		end
		Battle.ai_pushOrder(RECEIVER,random_pos,skillTable[index]);
	end
end

function QianNianXueYao_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		QianNianXueYao_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		QianNianXueYao_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function QianNianXueYao_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		QianNianXueYao_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		QianNianXueYao_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function QianNianXueYao_Deadth(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	
end