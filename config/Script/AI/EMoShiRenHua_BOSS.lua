Sys.log("load EMoShiRenHua.lua");
--RECEIVER
--ARG_BATTLEID,		//0
--ARG_CASTERPOS,		//1
--ARG_TARGETPOS,		//2
--ARG_POSTABLE,		//3
--ARG_SKILLTABLE		//4

function EMoShiRenHua_PushOrder(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	local num = table.maxn(ARG4)
	if num == 0 then
		return;
	end
	
	local TargetPos = 0;
	local index = math.ceil(math.random(1,num));
	local h_table = {};  --血量少于20%成员位置表
	local l_table = get_opposite_pos(ARG0,ARG3,ARG2);
	for j=1,table.maxn(l_table) ,1 do
		local CurHp = Battle.get_prop(ARG0,l_table[j],PT_HpCurr);
		local MaxHp = Battle.get_prop(ARG0,l_table[j],PT_HpMax);
		if MaxHp ==  0 then
			return;
		end
		
		if CurHp/MaxHp < 0.2 then
			table.insert(h_table,l_table[i]);
		end
	end
	
	if table.maxn(h_table) >= 3 then
		while ARG4[index] ~= 2381 and ARG4[index] ~= 2081 and ARG4[index] ~= 2501 do
			index = math.ceil(math.random(1,num));
		end
	
		if ARG4[index] == 2381 then
			Battle.ai_pushOrder(RECEIVER,ARG1,2381);
		else
			TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.ai_pushOrder(RECEIVER,ARG0,ARG4[index]);
		end
		
		return;
	end
	
	for j=1,table.maxn(l_table) ,1 do
		local CurHp = Battle.get_prop(ARG0,l_table[j],PT_HpCurr);
		local MaxHp = Battle.get_prop(ARG0,l_table[j],PT_HpMax);
		if MaxHp ==  0 then
			return;
		end
		
		if CurHp/MaxHp < 0.2 then
			Battle.ai_pushOrder(RECEIVER,l_table[j],2361);
			return;
		end
	end
	
	local tmp = get_all_pos(ARG0,ARG3,ARG1);
	
	if table.maxn(tmp) <= 2 then
		--放召唤，现没有此技能放普通攻击
		TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.ai_pushOrder(RECEIVER,TargetPos,1);
		return;
	end
	
	while ARG4[index] ~= 1 and ARG4[index] ~= 2011 and ARG4[index] ~= 2501 and ARG4[index] ~= 2081 do
		index = math.ceil(math.random(1,num));
	end
	
	TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	if ARG4[index] == 2 then
		local index1 = math.ceil(math.random(1,100));
		if index1 <= 10 then
			Battle.ai_pushOrder(RECEIVER,ARG1,2);
		else
			Battle.ai_pushOrder(RECEIVER,TargetPos,1);
		end
		return
	end
	Battle.ai_pushOrder(RECEIVER,TargetPos,ARG4[index]);
end

function EMoShiRenHua_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		EMoShiRenHua_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		EMoShiRenHua_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function EMoShiRenHua_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	local roll = math.ceil(math.random(0,100));
	if roll <= 50 then
		EMoShiRenHua_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
	else
		local TargetPos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		monsterpushOrder(RECEIVER,TargetPos);
		EMoShiRenHua_PushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4);
	end
end

function EMoShiRenHua_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)

end