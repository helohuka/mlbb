Sys.log("load ShaJuLongAoMaJiTe_Boss.lua");

function ShaJuLongAoMaJiTe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
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
		Sys.log("11111111111111111");
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
		return;
	end
	--血量少于30%以后，30%概率使用明镜，其他概率随机使用反击、昏睡攻击、气功弹
	local hpRatio = Battle.get_prop(ARG0,ARG1,PT_HpCurr)/Battle.get_prop(ARG0,ARG1,PT_HpMax);
	if hpRatio < 0.3
	then
		local roll = math.random(1,10);
		if roll <= 1
		then
			Battle.ai_pushOrder(RECEIVER,ARG1,2361);
		else
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			local index = math.ceil(math.random(1,table.maxn(ARG4)));
			while ARG4[index] ~= 1021 and ARG4[index] ~= 4004 and ARG4[index] ~= 1051
			do
				index = math.ceil(math.random(1,table.maxn(ARG4)));
			end
			Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
		end
	else
		--随机使用所有技能
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local index = math.ceil(math.random(1,table.maxn(ARG4)));
		if ARG4[index] == 2361
		then
			local group_type = check_group_type(ARG1);
			local minHpPos = Battle.getMinHpPos(ARG0,group_type);
			Battle.ai_pushOrder(RECEIVER,minHpPos,ARG4[index]);
		else
			Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
		end
	end
end

function ShaJuLongAoMaJiTe_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		ShaJuLongAoMaJiTe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		ShaJuLongAoMaJiTe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function ShaJuLongAoMaJiTe_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		ShaJuLongAoMaJiTe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		ShaJuLongAoMaJiTe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function ShaJuLongAoMaJiTe_Deadth(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)

end