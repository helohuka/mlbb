Sys.log("load DesertDanditBaTuo_Boss.lua");

function DesertDanditBaTuo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local isDebuff = false;
	for j=ST_Poison,ST_Forget,1
	do
		if Battle.check_state(ARG0,ARG1,j)
		then
			isDebuff = true;
			break;
		end
	end
	
	if isDebuff
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		--有debuff，则必定使用气功弹或者连击
		local roll = math.random(1,10);
		if roll <= 5
		then
			Battle.ai_pushOrder(RECEIVER,random_pos,1051);
		else
			Battle.ai_pushOrder(RECEIVER,random_pos,1001);
		end
	else
		--否则40%概率使用连击或者气功弹，40%概率使用反击，5%概率使用崩击，15%概率使用攻击。优先攻击对面血少的目标
		local group_type = check_group_type(ARG2);
		local minHpPos = Battle.getMinHpPos(ARG0,group_type);
		local roll = math.random(1,100);
		if roll <= 40
		then
			local roll1 = math.random(1,10);
			if roll1 <= 5
			then
				Battle.ai_pushOrder(RECEIVER,minHpPos,1051);
			else
				Battle.ai_pushOrder(RECEIVER,minHpPos,1001);
			end
		elseif roll > 40 and roll <= 80
		then
			Battle.ai_pushOrder(RECEIVER,minHpPos,1021);
		elseif roll > 80 and roll <= 85
		then
			Battle.ai_pushOrder(RECEIVER,minHpPos,1031);
		else
			Battle.ai_pushOrder(RECEIVER,minHpPos,1);
		end
	end
end

function DesertDanditBaTuo_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		DesertDanditBaTuo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		DesertDanditBaTuo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function DesertDanditBaTuo_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		DesertDanditBaTuo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		DesertDanditBaTuo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function DesertDanditBaTuo_deadth(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)

end