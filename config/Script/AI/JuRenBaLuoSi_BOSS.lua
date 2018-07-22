Sys.log("load JuRenBaLuoSi.lua");
--RECEIVER
--ARG_BATTLEID,		//0
--ARG_CASTERPOS,		//1
--ARG_TARGETPOS,		//2
--ARG_POSTABLE,		//3
--ARG_SKILLTABLE		//4

function JuRenBaLuoSi_UpdatePushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local num = table.maxn(ARG4)
	if num == 0 then
		return;
	end
	
	local index = 0;
	local skillID = 0;
	local roll = 0;
	local l_table = get_opposite_pos(ARG0,ARG3,ARG2);
	for j=1,table.maxn(l_table) ,1 do
		local CurHp = Battle.get_prop(ARG0,l_table[j],PT_HpCurr);
		local MaxHp = Battle.get_prop(ARG0,l_table[j],PT_HpMax);
		if MaxHp == 0 then
			return;
		end
		
		if CurHp/MaxHp < 0.2 then
			Battle.ai_pushOrder(RECEIVER,l_table[j],2381);
			return;
		end
	end
	
	local myCurHp = Battle.get_prop(ARG0,ARG1,PT_HpCurr);
	local myMaxHp = Battle.get_prop(ARG0,ARG1,PT_HpMax);
	
	if myMaxHp == 0 then
		return;
	end
	
	local index = math.ceil(math.random(1,num));
	
	if myCurHp/myMaxHp > 0.2 then
		while ARG4[index] ~= 1 and ARG4[index] ~= 1061 and ARG4[index] ~= 1021 do
			index = math.ceil(math.random(1,num));
		end
	else
		local roll = math.ceil(math.random(0,100));
		
		if roll <= 30 then
			skillID = 1031;
		else
			while ARG4[index] ~= 1 and ARG4[index] ~= 1061 and ARG4[index] ~= 1021 do
				index = math.ceil(math.random(1,num));
			end
			skillID = ARG4[index];
		end
		Battle.ai_pushOrder(RECEIVER,TargetPos,skillID);
		return;
	end
	
	index = math.ceil(math.random(1,num));
	while ARG4[index] ~= 1 and ARG4[index] ~= 1061 and ARG4[index] ~= 1021 do
		index = math.ceil(math.random(1,num));
	end
	
	roll = math.ceil(math.random(0,100));
	if roll <= 10 then
		skillID = 1081;
	end
	
	skillID = ARG4[index];
	
	TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	if skillId == 2 then
		local index1 = math.ceil(math.random(1,100));
		if index1 <= 10 then
			Battle.ai_pushOrder(RECEIVER,ARG1,2);
		else
			Battle.ai_pushOrder(RECEIVER,TargetPos,1);
		end
		return
	end
	Battle.ai_pushOrder(RECEIVER,TargetPos,skillID);
end

function JuRenBaLuoSi_Born(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local num = table.maxn(ARG4)
	if num == 0 then
		return;
	end
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		Battle.ai_pushOrder(RECEIVER,ARG1,2421);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		Battle.ai_pushOrder(RECEIVER,ARG1,2421);
	end
end

function JuRenBaLuoSi_Update(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		JuRenBaLuoSi_UpdatePushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		JuRenBaLuoSi_UpdatePushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function JuRenBaLuoSi_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)

end