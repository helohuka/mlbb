Sys.log("load CiKeAYi.lua");
--RECEIVER
--ARG_BATTLEID,		//0
--ARG_CASTERPOS,		//1
--ARG_TARGETPOS,		//2
--ARG_POSTABLE,		//3
--ARG_SKILLTABLE		//4

function CiKeAYi_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local num = table.maxn(ARG4)
	if num == 0 then
		return;
	end
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	if table.maxn(my_table) < 3 then
		--放分身，现没有此技能放普通攻击
		Battle.ai_pushOrder(RECEIVER,ARG1,5511);
		return;
	end
	local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	local index = math.ceil(math.random(1,num));
	local CurHp = Battle.get_prop(ARG0,ARG1,PT_HpCurr);
	local MaxHp = Battle.get_prop(ARG0,ARG1,PT_HpMax);
	local skillId = ARG4[index];
	if MaxHp ==  0 then
		return;
	end
	
	if CurHp/MaxHp < 0.2 then   
		local roll = math.ceil(math.random(0,100));
		if roll <= 10 then
			skillId = 1091;
		end
		
		if roll > 10 and roll <=30 then
			skillId = 1011;
		end
		
		if roll > 30 then
			skillId = 4;
		end
		
	else
		while ARG4[index] == 5511 do		--这里没加分身判断，有分身技能后要加上,1为分身
			index = math.ceil(math.random(1,num));
		end
		TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		
		skillId = ARG4[index];
		if skillId == 2511 then
			skillId = 1;
		end
		Battle.ai_pushOrder(RECEIVER,TargetPos,skillId);
		return;
	end
	
	if skillId == 4 then
		local tmpPos = math.ceil(math.random(1,table.maxn(my_table)));
		TargetPos = my_table[tmpPos];
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
	if skillId == 2511 then		--这里没加分身判断，有分身技能后要加上,1为分身
		skillId = 1;
	end
	Battle.ai_pushOrder(RECEIVER,TargetPos,skillId);
end

function CiKeAYi_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		CiKeAYi_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		CiKeAYi_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function CiKeAYi_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		CiKeAYi_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		CiKeAYi_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function CiKeAYi_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)

end