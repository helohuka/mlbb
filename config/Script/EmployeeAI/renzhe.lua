Sys.log("load renzhe.lua");

function RenZhe_TongYongLiuCheng(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	local num = table.maxn(ARG4)
	if num == 0 then
		return;
	end
	local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	local index = math.ceil(math.random(1,num));
	while(ARG4[index] == 2391 or ARG4[index] == 1091 or ARG4[index] == 2401 )
		do
			index = math.ceil(math.random(1,num));
		end
	if Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_Team then
		--选择自己阵营的一个随机目标
		random_pos = random_TargetPos(ARG0, ARG2, ARG1, ARG3, false)
	end
	Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
end

function RenZhe_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getSneakAttack(ARG0) == SAT_SurpriseAttack 
	then
		return;
	end
	local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	Battle.ai_pushOrder(RECEIVER,random_pos,8);
end

function RenZhe_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	Battle.ai_pushOrder(RECEIVER,random_pos,8);
end

function RenZhe_deadth(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)

end
















