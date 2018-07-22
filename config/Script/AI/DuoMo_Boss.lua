Sys.log("load DuoMo_Boss.lua");

function DuoMo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	--当小怪数量少于3个以后，一直释放即死技能
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	local num = table.maxn(my_table) - 1;   -- num小怪数量
	if num < 3
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,random_pos,5502);
		return;
	end
	--血量少于50%以后，随机使用攻击反弹、魔法反弹、圣盾、恢复技能
	local hpRatio = Battle.get_prop(ARG0,ARG1,PT_HpCurr)/Battle.get_prop(ARG0,ARG1,PT_HpMax);
	if hpRatio < 0.5
	then
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		local skillTable = {2461,2471,2501,2411};
		local index = math.ceil(math.random(1,table.maxn(skillTable)));
		if Battle.getSkill_TargetType(ARG0,skillTable[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,skillTable[index]) == STT_Team then
			--选择自己阵营的一个随机目标
			random_pos = random_TargetPos(ARG0, ARG2, ARG1, ARG3, false)
		end
		Battle.ai_pushOrder(RECEIVER,random_pos,skillTable[index]);
		return;
	end
	--随机使用召唤技能、攻击、防御、圣盾、恢复、攻击反弹、魔法反弹
	local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	local index = math.ceil(math.random(1,table.maxn(ARG4)));
	if Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_Team then
		--选择自己阵营的一个随机目标
		random_pos = random_TargetPos(ARG0, ARG2, ARG1, ARG3, false)
	end
	Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
end

function DuoMo_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		DuoMo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		DuoMo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function DuoMo_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		DuoMo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		DuoMo_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function DuoMo_Deadth(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	
end
