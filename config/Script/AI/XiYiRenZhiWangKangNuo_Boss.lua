Sys.log("load XiYiRenZhiWangKangNuo_Boss.lua");

function XiYiRenZhiWangKangNuo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	--每5回合有30%对敌对任意一个目标使用补血魔法
	local round = Battle.getRound(ARG0) + 1;
	if round%5 == 0
	then
		local group_type = check_group_type(ARG2);
		local minHpPos = Battle.getMinHpPos(ARG0,group_type);
		Battle.ai_pushOrder(RECEIVER,minHpPos,2361);
		return;
	end
	--如果身上有debuff效果，则一直使用连击
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
		Battle.ai_pushOrder(RECEIVER,random_pos,1001);
	else
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local index = math.ceil(math.random(1,table.maxn(ARG4)));
		while ARG4[index] == 2361
		do
			index = math.ceil(math.random(1,table.maxn(ARG4)));
		end
		Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
	end
end

function XiYiRenZhiWangKangNuo_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		XiYiRenZhiWangKangNuo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		XiYiRenZhiWangKangNuo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function XiYiRenZhiWangKangNuo_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		XiYiRenZhiWangKangNuo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		XiYiRenZhiWangKangNuo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function XiYiRenZhiWangKangNuo_Deadth(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)

end