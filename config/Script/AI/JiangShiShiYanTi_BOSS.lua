Sys.log("load JiangShiShiYanTi.lua");
--RECEIVER
--ARG_BATTLEID,		//0
--ARG_CASTERPOS,		//1
--ARG_TARGETPOS,		//2
--ARG_POSTABLE,		//3
--ARG_SKILLTABLE		//4

function JiangShiShiYanTi_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local num = table.maxn(ARG4)
	if num == 0 then
		return;
	end
	
	local myCurHp = Battle.get_prop(ARG0,ARG1,PT_HpCurr);
	local myMaxHp = Battle.get_prop(ARG0,ARG1,PT_HpMax);
	local skillId = 0;
	if myMaxHp == 0 then
		return;
	end
	
	local index = math.ceil(math.random(1,num));
	skillId = ARG4[index];
	if myCurHp/myMaxHp >= 0.5 then
		while (skillId == 2121 or skillId == 2081) do
			index = math.ceil(math.random(1,num));
			skillId = ARG4[index];
		end
		
		if skillId == 2 then
			local roll = math.ceil(math.random(0,100));
			if roll > 30 then
				skillId = 1;
			end
		end
	else
		while skillId == 2081 do
			index = math.ceil(math.random(1,num));
			skillId = ARG4[index];
		end
	end
	
	if myCurHp/myMaxHp < 0.2 then
		Battle.ai_pushOrder(RECEIVER,TargetPos,2081);
		return;
	end
	
	local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	if Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_Team then
		--选择自己阵营的一个随机目标
		TargetPos = random_TargetPos(ARG0, ARG2, ARG1, ARG3, false)
	end
	if skillId == 2 then
		local index1 = math.ceil(math.random(1,100));
		if index1 <= 10 then
			Battle.ai_pushOrder(RECEIVER,ARG1,2);
		else
			Battle.ai_pushOrder(RECEIVER,TargetPos,1);
		end
		return
	end
	Battle.ai_pushOrder(RECEIVER,TargetPos,skillId);
end

function JiangShiShiYanTi_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		JiangShiShiYanTi_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		JiangShiShiYanTi_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function JiangShiShiYanTi_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		JiangShiShiYanTi_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			if Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_Team then
			--选择自己阵营的一个随机目标
			TargetPos = random_TargetPos(ARG0, ARG2, ARG1, ARG3, false)
		end
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		if Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_Team then
			--选择自己阵营的一个随机目标
			TargetPos = random_TargetPos(ARG0, ARG2, ARG1, ARG3, false)
		end
		monsterpushOrder(RECEIVER,TargetPos);
		JiangShiShiYanTi_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function JiangShiShiYanTi_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)

end