Sys.log("load CiKeFenShen.lua");
--RECEIVER
--ARG_BATTLEID,		//0
--ARG_CASTERPOS,		//1
--ARG_TARGETPOS,		//2
--ARG_POSTABLE,		//3
--ARG_SKILLTABLE		//4

function CiKeFenShen_PushOrder(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	local num = table.maxn(ARG4)
	if num == 0 then
		return;
	end
	local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	local index = math.ceil(math.random(1,num));
	local CurHp = Battle.get_prop(ARG0,ARG1,PT_HpCurr);
	local MaxHp = Battle.get_prop(ARG0,ARG1,PT_HpMax);
	local skillId = ARG4[index];
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	if MaxHp ==  0 then
		return;
	end
	
	if CurHp/MaxHp < 0.2 then   
		local roll = math.ceil(math.random(0,100));
		if roll <= 20 then
			skillId = 1091;
		end
		
		if roll > 20 and roll <=40 then
			skillId = 2501;
		end
		
		if roll > 40 then
			skillId = 4;
		end
		
	else
		while ARG4[index] == 1 do		--这里没加分身判断，有分身技能后要加上,1为分身
			index = math.ceil(math.random(1,num));
		end
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

function CiKeFenShen_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
		CiKeFenShen_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		--local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		--monsterpushOrder(RECEIVER,TargetPos);
end

function CiKeFenShen_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
		CiKeFenShen_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		--local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		--monsterpushOrder(RECEIVER,TargetPos);
end

function CiKeFenShen_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)

end