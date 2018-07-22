Sys.log("load HeiAnMoJunBaLiKe_Boss.lua");

function HeiAnMoJunBaLiKe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	--如果小弟数量少于4个，则召唤，每次召唤4个
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	local num = table.maxn(my_table) - 1;   -- num小怪数量
	if num < 4
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,5510);
	else
		--每回合有10%概率使用大地之怒，其他概率随机使用乾坤、连击、诸刃、圣盾、超强混乱，吸血魔法
		local roll = math.random(1,10);
		if roll == 10
		then
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.ai_pushOrder(RECEIVER,random_pos,5500);
		else
			local skillTable = {1061,1001,1011,2501,2291,2121};
			local index = math.ceil(math.random(1,table.maxn(skillTable)));
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.ai_pushOrder(RECEIVER,random_pos,skillTable[index]);
		end
	end
end

function HeiAnMoJunBaLiKe_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		HeiAnMoJunBaLiKe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		HeiAnMoJunBaLiKe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function HeiAnMoJunBaLiKe_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		HeiAnMoJunBaLiKe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		HeiAnMoJunBaLiKe_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function HeiAnMoJunBaLiKe_Deadth(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)

end