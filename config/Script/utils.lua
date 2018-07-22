Sys.log("load utils.lua");
--script Sys.load_script "utils.lua"
up_group_pos =  { 
				 BP_Up0, BP_Up1, BP_Up2, BP_Up3, BP_Up4,
				 BP_Up5, BP_Up6, BP_Up7, BP_Up8, BP_Up9, 
				}
down_group_pos ={
				 BP_Down0,BP_Down1,BP_Down2,BP_Down3,BP_Down4,
				 BP_Down5,BP_Down6,BP_Down7,BP_Down8,BP_Down9,
				}


function check_group_type(pos)
	if pos>=BP_Up0 and pos <= BP_Up9
	
	then 
		return GT_Up;
	elseif pos>=BP_Down0 and pos <= BP_Down9
	then 
		return GT_Down;
	else
		return GT_None;
	end
end

function check_enemy_group_type(pos)
	if pos>=BP_Up0 and pos <= BP_Up9
	then 
		return GT_Down;
	elseif pos>=BP_Down0 and pos <= BP_Down9
	then 
		return GT_Up;
	else
		return GT_None;
	end
end
				
function get_one_pos(is_random, pos_table, group_table,ARG1,isMagic)
	local isMagic1 = {};
	for i=1, table.getn(group_table), 1
		do
			if isMagic then
				table.insert(isMagic1,true);
			else
				table.insert(isMagic1,group_table[i] ~= ARG1);
			end
		end
	
	if is_random 
	then 
		local tmp_table = {};
		for i=1, table.getn(group_table), 1
		do
			if pos_table[group_table[i]] ~= 0 and isMagic1[i]
			then
				table.insert(tmp_table,group_table[i]);
			end
		end

		if table.getn(tmp_table) == 0
		then 
			return BP_None;
		else
			local r_index = math.ceil(math.random(1,table.getn(tmp_table)));
			return tmp_table[r_index];
		end
	else
		for i=1,table.getn(group_table),1
		do
			if pos_table[group_table[i]] ~= 0 and isMagic1[i]
			then
				return pos_table[group_table[i]];
			end
		end
		return BP_None;
	end
end	


--切换一个随机目标
function random_enemy_pos_ex(ARG0,ARG3,ARG1,ARG2,isMagic)
	local tmp_table = get_opposite_pos(ARG0,ARG3,ARG2);	

	if table.getn(tmp_table) == 0 then
		return BP_None;
	else
		return get_one_pos(true,ARG3,tmp_table,ARG1,isMagic);
	end
end

-- 计算站位
-- 0 1 2 3 4  
-- 5 6 7 8 9
--
-- 5 6 7 8 9
-- 0 1 2 3 4
-- 
-- 前端点命名
-- 10 11 12 13 14 
-- 15 16 17 18 19
--
-- 5 6 7 8 9
-- 0 1 2 3 4
-- 一一直接计算 中间点有没有站位 从而得到是否需要从新定位目标位置 magic 标识需不需要后排切换

function _is_valid(battle,pos)
    return Battle.get_prop(battle,pos,PT_HpCurr) >= 1;
end

function random_enemy_pos_ex_2(battle,poslist,self,target,magic)
     local pos = random_enemy_pos_ex(battle,poslist,self,target,magic);
    if magic then
        return pos;
    else
       if pos >= BP_Down0 and pos <= BP_Down4 then
            if _is_valid(battle,pos+5) and _is_valid(battle,self+5) then
                return pos+5;
            else
                return pos;
            end
       elseif pos >= BP_Up0 and pos <= BP_Up4 then
            if _is_valid(battle,pos+5) and _is_valid(battle,self+5) then
                return pos+5;
            else
                return pos;
            end
       else
           return pos;
       end
    end
end

--// 获得手上的武器
function get_hand_weapon(battle,selfpos)
    local lefthand = Battle.getItemType_BySlot(battle,selfpos,ES_SingleHand);
	local righthand = Battle.getItemType_BySlot(battle,selfpos,ES_DoubleHand);
    
    if lefthand ~= WT_None then return lefthand; end
    if righthand ~= WT_None then return righthand; end
    return WT_None;
end

--// 是否是近战武器
function is_melee_weapon(weapon)
    return ((weapon ~= WT_Bow) and (weapon ~= WT_Knife));
end

--检查血量，如果死亡就换个目标
--添加前后排逻辑
function check_hp_curr(battle,selfpos,targetpos,poslist,isMagic)
    
    local selfweapon = get_hand_weapon(battle, selfpos); --//自己的武器
    local meleeweapon = is_melee_weapon(selfweapon);     --//武器是否为近战武器
    local currskill = Battle.current_order_skill(battle);--//当前技能(也就是执行这个脚本的技能
    local meleeskill = Battle.get_skill_melee(currskill);--//技能是否是近战技能
    
    --//需要做后排处理
    if meleeweapon == true and meleeskill == true then
	    if poslist[targetpos] == 0 then
		    return random_enemy_pos_ex_2(battle,poslist, selfpos, targetpos,isMagic);
	    end
	    local hp_curr = Battle.get_prop(battle,targetpos,PT_HpCurr);

	    if hp_curr < 1 then
		    return random_enemy_pos_ex_2(battle,poslist, selfpos, targetpos,isMagic);
	    else
		    return targetpos;
	    end   
    else --// 不需要做后排处理
       	if poslist[targetpos] == 0 then
		    return random_enemy_pos_ex(battle,poslist, selfpos, targetpos,isMagic);
	    end
	    local hp_curr = Battle.get_prop(battle,targetpos,PT_HpCurr);

	    if hp_curr < 1 then
		    return random_enemy_pos_ex(battle,poslist, selfpos, targetpos,isMagic);
	    else
		    return targetpos;
	    end   
    end
end

--检测每个位置是否有人（所选阵营）
function get_opposite_pos(ARG0,ARG3,ARG2)
	local group_type = check_group_type(ARG2);
	local pos_table = {};
	if GT_None == group_type 
	then 
		return BP_None;
	elseif GT_Down == group_type
	then
		pos_table = down_group_pos;
	elseif GT_Up == group_type
	then
		pos_table = up_group_pos;
	end
	local l_table = {};
	for i=1,table.getn(pos_table),1
		do
			if ARG3[pos_table[i]] ~= 0 and Battle.get_prop(ARG0,pos_table[i],PT_HpCurr) >= 1 then
				table.insert(l_table,pos_table[i]);
			end
		end
	return l_table;
end

--敌对阵营,伙伴
function get_enemy_table(ARG0,ARG3,ARG1)
	local my_group = check_group_type(ARG1);

	local pos_table = {};
	if GT_None == my_group 
	then 
		return BP_None;
	elseif GT_Down == my_group
	then
		pos_table = up_group_pos;
	elseif GT_Up == my_group
	then
		pos_table = down_group_pos;
	end
	local l_table = {};
	for i=1,table.getn(pos_table),1
		do
			if ARG3[pos_table[i]] ~= 0 and Battle.get_prop(ARG0,pos_table[i],PT_HpCurr) >= 1 then
				table.insert(l_table,pos_table[i]);
			end
		end
	return l_table;
end

--检测每个位置是否有人（所有目标）
function get_all_pos(ARG0,ARG3,ARG1)
	local l_table = {};
	for i=1,table.getn(down_group_pos),1
		do
			if ARG3[down_group_pos[i]] ~= 0 and Battle.get_prop(ARG0,down_group_pos[i],PT_HpCurr) >= 1 and down_group_pos[i] ~= ARG1 then
				table.insert(l_table,down_group_pos[i]);
			end
		end
	for i=1,table.getn(up_group_pos),1
		do
			if ARG3[up_group_pos[i]] ~= 0 and Battle.get_prop(ARG0,up_group_pos[i],PT_HpCurr) >= 1 and up_group_pos[i] ~= ARG1 then
				table.insert(l_table,up_group_pos[i]);
			end
		end
	return l_table;
end

--AI 检查有效攻击位置
function check_validPos(ARG0, ARG3, ARG1)
	local Force = check_group_type(ARG1);	

	local opposite_pos_table = {};
	if GT_None == Force 
	then 
		return opposite_pos_table;
	elseif GT_Down == Force
	then
		opposite_pos_table = up_group_pos;
	elseif GT_Up == Force
	then
		opposite_pos_table = down_group_pos;
	end

	local l_table = {};
	for i=1,table.getn(opposite_pos_table),1
	do		
		if ARG3[opposite_pos_table[i]] ~= 0 and Battle.get_prop(ARG0,opposite_pos_table[i],PT_HpCurr) >= 1 then
			table.insert(l_table,opposite_pos_table[i]);

		end
	end

	return l_table;
end

--AI 随机有效目标
function random_TargetPos(ARG0, ARG1, ARG2, ARG3, isMagic)
	local group_table = check_validPos(ARG0, ARG3, ARG1);

	local isMagic1 = {};
	for i=1, table.getn(group_table), 1 
	do
		if isMagic then
			table.insert(isMagic1,true);
		else
			table.insert(isMagic1,group_table[i] ~= ARG1);
		end
	end	
	local tmp_table = {};
	for i=1, table.getn(group_table), 1
	do
		if ARG3[ group_table[i] ] ~= 0 and isMagic1[i]
		then
			table.insert(tmp_table,group_table[i]);
		end
	end

	if table.getn(tmp_table) == 0
	then 
		return BP_None;
	else
		local r_index = math.ceil(math.random(1,table.getn(tmp_table)));
		local pos = tmp_table[r_index];
		
		if pos >= BP_Down0 and pos <= BP_Down4 then
            if _is_valid(ARG0,pos+5) and _is_valid(ARG0,ARG1+5) then
                return pos+5;
            else
                return pos;
            end
		elseif pos >= BP_Up0 and pos <= BP_Up4 then
            if _is_valid(ARG0,pos+5) and _is_valid(ARG0,ARG1+5) then
                return pos+5;
            else
                return pos;
            end
		else
			return pos;
		end
	end
end

--强力位置
function qiangli_pos_table(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	local pos_table = {ARG2+1, ARG2-1, ARG2+5, ARG2-5, ARG2}
	
	--如果是边缘单位，删除不必要的
	if ARG2 == BP_Up0 or ARG2 == BP_Down0 then
		table.remove(pos_table,2)
		table.remove(pos_table,3)
	elseif ARG2 == BP_Up5 or ARG2 == BP_Down5 then
		table.remove(pos_table,2)
		table.remove(pos_table,2)
	elseif ARG2 == BP_Up4 or ARG2 == BP_Down4 then	
		table.remove(pos_table,1)
		table.remove(pos_table,3)
	elseif ARG2 == BP_Up9 or ARG2 == BP_Down9 then	
		table.remove(pos_table,1)
		table.remove(pos_table,2)
	end
	
	--删除无效的单位
	local l_table = {};
	local a = 0;
	local b = 0;
	if ARG2 <= 10 and ARG2 >= 1 then
		a = 10;
		b = 1;
	elseif ARG2 <= 20 and ARG2 >= 11 then
		a = 20;
		b = 11;
	else
		return false;
	end
	
	for i=1,table.getn(pos_table),1
		do

			if pos_table[i] >= b and pos_table[i] <= a and ARG3[pos_table[i]] ~=0 and Battle.get_prop(ARG0,pos_table[i],PT_HpCurr) > 0 then
				table.insert(l_table,pos_table[i]);
			end
	end
	
	return l_table;
end

--小刀位置
function xiaodao_pos_table(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	local pos_table = {ARG2+5, ARG2-5, ARG2}
	
	--删除无效的单位
	local l_table = {};
	local a = 0;
	local b = 0;
	if ARG2 <= 10 and ARG2 >= 1 then
		a = 10;
		b = 1;
	elseif ARG2 <= 20 and ARG2 >= 11 then
		a = 20;
		b = 11;
	else
		return false;
	end
	
	for i=1,table.getn(pos_table),1
		do

			if pos_table[i] >= b and pos_table[i] <= a and ARG3[pos_table[i]] ~=0 and Battle.get_prop(ARG0,pos_table[i],PT_HpCurr) > 0 then
				table.insert(l_table,pos_table[i]);
			end
	end
	
	return l_table;
end


-- 基础攻击力公式
function atk_reality(atk)
	local datk;
	if atk < 240 and atk > 0 then
		datk = atk;
	elseif atk >= 240 then
		datk = 240+5*(atk-240)/10;
	else
		datk = 1
	end
	return datk;
end
-- 基础防御力公式
function def_reality(def)
	local ddef;
	if def < 240 and def > 0 then
		ddef = def;
	elseif def >= 240 then
		ddef = 240+5*(def-240)/10;
	else
		ddef = 1
	end
	return ddef;
end


--//判定是否闪避
--//100%-1次攻击命中率=（被攻击方敏捷/攻击方敏捷）^2（注意！这个是2次方）*5-命中修正+回避修正
function check_dodge(RECEIVER, ARG0, caster_pos, target_pos,state_type)
	if Battle.getSneakAttack(ARG0) == SAT_SurpriseAttack or Battle.getSneakAttack(ARG0) == SAT_SneakAttack then
		if Battle.getRound(ARG0) == 1 then
			return false
		end
	end
	if Battle.check_state(ARG0,target_pos,ST_Basilisk) or Battle.check_state(ARG0,target_pos,ST_Sleep) or Battle.check_state(ARG0,target_pos,ST_NoDodge) then
		return false
	end
	local check_gongxi = Battle.check_state(ARG0,target_pos,ST_ActionAbsorb);
	if check_gongxi then
		return false;
	end
	local check_gongfan = Battle.check_state(ARG0,target_pos,ST_ActionBounce);
	if check_gongfan then
		return false;
	end
	local caster_agile = Battle.get_prop(ARG0,caster_pos,PT_Agile);
	local target_agile = Battle.get_prop(ARG0,target_pos,PT_Agile);
	local caster_hit = Battle.get_prop(ARG0,caster_pos,PT_Hit);
	local target_dodge = Battle.get_prop(ARG0,target_pos,PT_Dodge);
	local dodge = (target_agile / caster_agile) * (target_agile / caster_agile) * 5 - caster_hit + target_dodge
	local dodge_index = math.ceil(math.random(1,100));
	if dodge >= dodge_index then
		Battle.insert_state(ARG0,target_pos,2,state_type);
		return true;
	else
		return false;
	end
end

--//判定是否暗杀
function check_ansha(RECEIVER, ARG0, caster_pos, target_pos)
	--远程武器不能暗杀
	if Battle.getMonsterTypebyPos(ARG0,target_pos) == 2 then
		return false;
	end
	local zuoshou = Battle.getItemType_BySlot(ARG0,caster_pos,ES_SingleHand)
	local youshou = Battle.getItemType_BySlot(ARG0,caster_pos,ES_DoubleHand)
	if zuoshou == WT_Bow or zuoshou == WT_Knife or youshou == WT_Bow or youshou == WT_Knife then
		return false;
	end
	local ansha = Battle.get_prop(ARG0,caster_pos,PT_Assassinate);
	local Lv = math.ceil(Battle.get_prop(ARG0,target_pos,PT_Level) / 10) ;
	local index = 0
	if ansha/10 > Lv then
		index = (1-((Lv+1)/(2*(ansha/10+1))))*100
	elseif ansha/10 <= Lv then
		index = ((ansha/10)/(2*Lv))*100
	end
	local ansha_index = math.ceil(math.random(1,100));

	if index >= ansha_index then
		return true;
	else
		return false;
	end
end

--判断必杀
function check_bisha(RECEIVER, ARG0, caster_pos, target_pos)
	local bisha = Battle.get_prop(ARG0,caster_pos,PT_Crit);
	local bisha_index = math.ceil(math.random(1,100));
	if bisha >= bisha_index then
		return true;
	else
		return false;
	end
end


--无闪避基础伤害公式
function huwei_damage(RECEIVER, ARG0, caster_pos, target_pos,heji_num)
	--是否攻击无效
	local check_gongwu = Battle.check_state(ARG0,target_pos,ST_ActionInvalid);
	if check_gongwu then
		Battle.cutTime_state(ARG0,target_pos,ST_ActionInvalid,1)
		return 0;
	end
	local caster_attack = Battle.get_prop(ARG0,caster_pos,PT_Attack);
	local caster_attack_reality = atk_reality(caster_attack);
	local target_defense= Battle.get_prop(ARG0,target_pos,PT_Defense);
	local target_defense_reality = def_reality(target_defense);
	local shuijing = shuijing(RECEIVER, ARG0, caster_pos, target_pos);
	local damage = shuijing * ( caster_attack_reality * caster_attack_reality * 3.0 ) / (caster_attack_reality + target_defense_reality * 3.0);
	--是否石化
	local shihua = Battle.check_state(ARG0,target_pos,ST_Basilisk);
	if shihua then
		damage = damage / 2
	end
	--是否攻击吸收
	local a = 1;
	local check_gongxi = Battle.check_state(ARG0,target_pos,ST_ActionAbsorb);
	if check_gongxi then
		a = -1;
		Battle.cutTime_state(ARG0,target_pos,ST_ActionAbsorb,1)
	end
	if heji_num >= 2 then
		return -damage * a;
	end
	--是否攻击反弹
	local check_gongfan = Battle.check_state(ARG0,target_pos,ST_ActionBounce);
	if check_gongfan then
		return -damage * a;
	end
	if check_gongxi then
		return -damage * a;
	end
	return -damage * a;
end


--基础伤害公式
function nrm_damage(RECEIVER, ARG0, caster_pos, target_pos,heji_num)
	--是否攻击无效
	local check_gongwu = Battle.check_state(ARG0,target_pos,ST_ActionInvalid);
	if check_gongwu then
		Battle.cutTime_state(ARG0,target_pos,ST_ActionInvalid,1)
		return 0;
	end
	local caster_attack = Battle.get_prop(ARG0,caster_pos,PT_Attack);
	local caster_attack_reality = atk_reality(caster_attack);
	local target_defense= Battle.get_prop(ARG0,target_pos,PT_Defense);
	local target_defense_reality = def_reality(target_defense);
	local shuijing = shuijing(RECEIVER, ARG0, caster_pos, target_pos);
	local damage = shuijing * ( caster_attack_reality * caster_attack_reality * 3.0 ) / (caster_attack_reality + target_defense_reality * 3.0);
	--是否石化
	local shihua = Battle.check_state(ARG0,target_pos,ST_Basilisk);
	if shihua then
		damage = damage / 2
	end
	--是否攻击吸收
	local a = 1;
	local check_gongxi = Battle.check_state(ARG0,target_pos,ST_ActionAbsorb);
	if check_gongxi then
		a = -1;
		Battle.cutTime_state(ARG0,target_pos,ST_ActionAbsorb,1)
	end
	--是否圣盾
	local check_shengdun = Battle.check_state(ARG0,target_pos,ST_Shield);
	if check_shengdun then
		local damage_table = {1,2,3,4,5,6,7,8,9,10}
		local damage_index = math.ceil(math.random(1,10));
		return -damage * damage_table[damage_index] / 100 * a;
	end
	--是否防御
	local check_fangyu = Battle.check_state(ARG0,target_pos,ST_Defense);
	if check_fangyu then
		local damage_table = {1,damage*10/100,damage*20/100,damage*30/100,damage*40/100,damage*50/100};
		local damage_index = math.ceil(math.random(1,6));
		return -damage_table[damage_index] * a;	
	end
	if heji_num >= 2 then
		return -damage * a;
	end
	--是否攻击反弹
	local check_gongfan = Battle.check_state(ARG0,target_pos,ST_ActionBounce);
	if check_gongfan then
		return -damage * a;
	end
	if check_gongxi then
		return -damage * a;
	end
	--是否闪避
	local check_shanbi =  check_dodge(RECEIVER, ARG0, caster_pos, target_pos,0);
	if check_shanbi then
		damage = 0;
		Battle.cutTime_state(ARG0,target_pos,ST_Dodge,1)
	end
	
	return -damage * a;
end



--诸刃伤害公式
function zhuren_damage(RECEIVER, ARG0, caster_pos, target_pos,heji_num,level)
	--是否攻击无效
	local check_gongwu = Battle.check_state(ARG0,target_pos,ST_ActionInvalid);
	if check_gongwu then
		Battle.cutTime_state(ARG0,target_pos,ST_ActionInvalid,1)
		return 0;
	end
	local caster_attack = Battle.get_prop(ARG0,caster_pos,PT_Attack);
	local caster_attack_reality = atk_reality(caster_attack);
	local target_defense= Battle.get_prop(ARG0,target_pos,PT_Defense);
	local target_defense_reality = def_reality(target_defense);
	caster_attack_reality = caster_attack_reality*(1 + level*7/100)
	local shuijing = shuijing(RECEIVER, ARG0, caster_pos, target_pos);
	local damage = shuijing * ( caster_attack_reality * caster_attack_reality * 3.0 ) / (caster_attack_reality + target_defense_reality * 3.0);
	--是否石化
	local shihua = Battle.check_state(ARG0,target_pos,ST_Basilisk);
	if shihua then
		damage = damage / 2
	end
	--是否攻击吸收
	local a = 1;
	local check_gongxi = Battle.check_state(ARG0,target_pos,ST_ActionAbsorb);
	if check_gongxi then
		a = -1;
		Battle.cutTime_state(ARG0,target_pos,ST_ActionAbsorb,1)
	end
	--是否圣盾
	local check_shengdun = Battle.check_state(ARG0,target_pos,ST_Shield);
	if check_shengdun then
		local damage_table = {1,2,3,4,5,6,7,8,9,10}
		local damage_index = math.ceil(math.random(1,10));
		return -damage * damage_table[damage_index] / 100 * a;
	end
	--是否防御
	local check_fangyu = Battle.check_state(ARG0,target_pos,ST_Defense);
	if check_fangyu then
		local damage_table = {1,damage*10/100,damage*20/100,damage*30/100,damage*40/100,damage*50/100};
		local damage_index = math.ceil(math.random(1,6));
		return -damage_table[damage_index] * a;	
	end
	if heji_num >= 2 then
		return -damage * a;
	end
	--是否攻击反弹
	local check_gongfan = Battle.check_state(ARG0,target_pos,ST_ActionBounce);
	if check_gongfan then
		return -damage * a;
	end
	if check_gongxi then
		return -damage * a;
	end
	--是否闪避
	local check_shanbi =  check_dodge(RECEIVER, ARG0, caster_pos, target_pos,0);
	if check_shanbi then
		damage = 0;
		Battle.cutTime_state(ARG0,target_pos,ST_Dodge,1)
	end
	
	return -damage * a;
end

--判断反击
function check_fanji(RECEIVER, ARG0, caster_pos, target_pos)
	local selfweapon = get_hand_weapon(ARG0, target_pos);
	if selfweapon == WT_Bow or selfweapon == WT_Knife then
		return false
	end
    local currskill = Battle.current_order_skill(ARG0);--//当前技能(也就是执行这个脚本的技能
    local meleeskill = Battle.get_skill_melee(currskill);--//技能是否是近战技能
	if not meleeskill then
		return false
	end
	local check_fanji1 = Battle.check_state(ARG0,target_pos,ST_BeatBack);
	if check_fanji1 then
		return true;
	end
	local caster_counterpunch = Battle.get_prop(ARG0,target_pos,PT_counterpunch);
	local counterpunch_index = math.ceil(math.random(1,100));
	if caster_counterpunch >= counterpunch_index then
		return true;
	else
		return false;
	end
end

--反击
function fanji_damage(RECEIVER, ARG0, caster_pos, target_pos)
	if	Battle.getItemType_BySlot(ARG0,caster_pos,ES_DoubleHand) == WT_Bow or Battle.getItemType_BySlot(ARG0,target_pos,ES_DoubleHand) == WT_Bow then
		return;
	elseif Battle.getItemType_BySlot(ARG0,caster_pos,ES_SingleHand) == WT_Knife or Battle.getItemType_BySlot(ARG0,target_pos,ES_DoubleHand) == WT_Knife then
		return;
	end	
	local currskill = Battle.current_order_skill(ARG0);--//当前技能(也就是执行这个脚本的技能
    local meleeskill = Battle.get_skill_melee(currskill);--//技能是否是近战技能
	if not meleeskill then
		return
	end
	--是否圣盾
	local check_shengdun = Battle.check_state(ARG0,target_pos,ST_Shield);
	if check_shengdun then
		return;
	end
	--是否防御
	local check_fangyu = Battle.check_state(ARG0,target_pos,ST_Defense);
	if check_fangyu then
		return;
	end
	--是否石化
	local check_shihua = Battle.check_state(ARG0,target_pos,ST_Basilisk);
	if check_shihua then
		return;
	end
	--是否昏睡
	local check_hunshui = Battle.check_state(ARG0,target_pos,ST_Sleep);
	if check_hunshui then
		return;
	end
	--是否反击
	local checkfanji = check_fanji(RECEIVER, ARG0, caster_pos, target_pos);
	if checkfanji then
		local caster_pos1 = target_pos;
		local target_pos1 = caster_pos;
		skill_fanji_atk(RECEIVER,ARG0, caster_pos1, target_pos1);
	else
		return;
	end
end


--崩击伤害公式
function bengji_damage(RECEIVER, ARG0, ARG1, target_pos,level)
	--是否攻击无效
	local check_gongwu = Battle.check_state(ARG0,target_pos,ST_ActionInvalid);
	if check_gongwu then
		Battle.cutTime_state(ARG0,target_pos,ST_ActionInvalid,1)
		return 0;
	end
	local caster_attack = Battle.get_prop(ARG0,ARG1,PT_Attack);
	local caster_attack_reality = atk_reality(caster_attack);
	local target_defense= Battle.get_prop(ARG0,target_pos,PT_Defense);
	local target_defense_reality = def_reality(target_defense);
	local damage = 0;
	local a = 1;
	local check1 = Battle.check_state(ARG0,target_pos,ST_ActionAbsorb);
	local damage_Multiple = {1.20,1.43,1.68,1.95,2.24,2.55,2.88,3.23,3.60,3.99};
	if check1 then
		a = -1;
	end
	local check = Battle.check_state(ARG0,target_pos,ST_Defense);
	local check_shengdun = Battle.check_state(ARG0,target_pos,ST_Shield);
	if check or check_shengdun then
		local shuijing = shuijing(RECEIVER, ARG0, ARG1, target_pos);

		damage = shuijing * a * damage_Multiple[level] * ( caster_attack_reality * caster_attack_reality * 3.0 ) / (caster_attack_reality + target_defense_reality * 3.0);
		return -damage;	
	else
		return damage;
	end
end

--魔法攻击伤害
function magic_damage(RECEIVER, ARG0, caster_pos, target_pos,level,shuijing_magic)
	--是否魔法无效
	local check_mowu = Battle.check_state(ARG0,target_pos,ST_MagicInvalid);
	if check_mowu then
		Battle.cutTime_state(ARG0,target_pos,ST_MagicInvalid,1)
		return 0;
	end
	--是否魔法吸收
	local a = 1;
	local check_moxi = Battle.check_state(ARG0,target_pos,ST_MagicAbsorb);
	if check_moxi then
		a = -1;
		Battle.cutTime_state(ARG0,target_pos,ST_MagicAbsorb,1)
	end
	--是否魔法防御
	local check_magic_def = Battle.check_state(ARG0,caster_pos,ST_MagicDef);
	if check_magic_def then
		damage = 1;
		return -damage * a;
	end
	local basic_damage_table = {83,150,210,266,323,380,437,501,568,635};
	local caster_spirit = Battle.get_prop(ARG0,caster_pos,PT_Spirit);
	local spirit_index = level * 20 + 102
	local spirit_percent = spirit_scale(RECEIVER, ARG0, caster_pos, target_pos);
	local shuijing_index = shuijing(RECEIVER, ARG0, caster_pos, target_pos,shuijing_magic);
	local magic_attack = magic_atk(RECEIVER, ARG0, caster_pos, target_pos,level)
	local magic_def = Battle.get_prop(ARG0,target_pos,PT_Magicdefense);
	local magic_defper = (100 - magic_def / 10) / 100
	if magic_defper < 0 then
		magic_defper = 1
	end
	if caster_spirit <= 0 then
		caster_spirit = 1
	end
	local damage = magic_defper * magic_attack * basic_damage_table[level] * spirit_percent * shuijing_index * caster_spirit / spirit_index;
	--是否圣盾
	local check_shengdun = Battle.check_state(ARG0,target_pos,ST_Shield);
	if check_shengdun then
		local damage_table = {1,2,3,4,5,6,7,8,9,10}
		local damage_index = math.ceil(math.random(1,10));
		return -damage * damage_table[damage_index] / 100 * a;
	end
	--是否石化
	local shihua = Battle.check_state(ARG0,target_pos,ST_Basilisk);
	if shihua then
		damage = damage / 2
	end
	return -damage * a;
end

--精神比
function spirit_scale(RECEIVER, ARG0, ARG1, random_pos)
	local target_spirit = Battle.get_prop(ARG0,random_pos,PT_Spirit);
	local caster_spirit = Battle.get_prop(ARG0,ARG1,PT_Spirit);
	if target_spirit <= 0 then
		target_spirit = 1
	end
	if caster_spirit <= 0 then
		caster_spirit = 1
	end
	local spirit_percent = 1.00;
	local spirit_scale = caster_spirit/target_spirit;
	if spirit_scale >= 1.20 or spirit_scale <= 0 then
		spirit_percent = 1.00;
	elseif spirit_scale >= 1.14 and spirit_scale < 1.20 then
		spirit_percent = 0.90;
	elseif spirit_scale >= 1.05 and spirit_scale < 1.14 then
		spirit_percent = 0.82;
	elseif spirit_scale >= 0.98 and spirit_scale < 1.05 then
		spirit_percent = 0.63;
	elseif spirit_scale >= 0.90 and spirit_scale < 0.98 then
		spirit_percent = 0.55;
	elseif spirit_scale >= 0.80 and spirit_scale <0.90 then
		spirit_percent = 0.36;
	elseif spirit_scale >= 0.70 and spirit_scale <0.80 then
		spirit_percent = 0.27;
	else
		spirit_percent = 0.09;
	end
	return spirit_percent;
end


--魔攻
function magic_atk(RECEIVER, ARG0, ARG1, random_pos,level)
	local magic_attack = Battle.get_prop(ARG0,ARG1,PT_Magicattack);
	if level == 1 then
		if magic_attack <= 10 then
			return 1.0000
		elseif magic_attack == 11 then
			return 1.1250
		elseif magic_attack == 12 then
			return 1.2500
		elseif magic_attack == 13 then
			return 1.3750
		elseif magic_attack >= 14 then
			return 1.5000
		else
			return 1.0000
		end
	elseif level == 2 then
		if magic_attack <= 40 then
			return 1.0000
		elseif magic_attack == 41 then
			return 1.0625
		elseif magic_attack == 42 then
			return 1.1250
		elseif magic_attack == 43 then
			return 1.1875
		elseif magic_attack == 44 then
			return 1.2500
		elseif magic_attack == 45 then
			return 1.3125
		elseif magic_attack == 46 then
			return 1.3750
		elseif magic_attack == 47 then
			return 1.4375
		elseif magic_attack >= 48 then
			return 1.5000
		else
			return 1.0000
		end
	elseif level == 3 then
		if magic_attack <= 73 then
			return 1.0000
		elseif magic_attack <= 76 and magic_attack > 73 then
			return 1.1250
		elseif magic_attack <= 79 and magic_attack > 76 then
			return 1.2500
		elseif magic_attack <= 82 and magic_attack > 79 then
			return 1.3750
		elseif magic_attack > 82 then
			return 1.5000
		else
			return 1.0000
		end
	elseif level == 4 then
		if magic_attack <= 104 then
			return 1.0000
		elseif magic_attack <= 108 and magic_attack > 104 then
			return 1.1250
		elseif magic_attack <= 112 and magic_attack > 108 then
			return 1.2500
		elseif magic_attack <= 116 and magic_attack > 112 then
			return 1.3750
		elseif magic_attack > 116 then
			return 1.5000
		else
			return 1.0000
		end
	elseif level == 5 then
		if magic_attack <= 135 then
			return 1.0000
		elseif magic_attack <= 140 and magic_attack > 135 then
			return 1.1250
		elseif magic_attack <= 145 and magic_attack > 140 then
			return 1.2500
		elseif magic_attack <= 150 and magic_attack > 145 then
			return 1.3750
		elseif magic_attack > 150 then
			return 1.5000
		else
			return 1.0000
		end
	elseif level == 6 then
		if magic_attack <= 166 then
			return 1.0000
		elseif magic_attack <= 172 and magic_attack > 166 then
			return 1.1250
		elseif magic_attack <= 178 and magic_attack > 172 then
			return 1.2500
		elseif magic_attack <= 184 and magic_attack > 178 then
			return 1.3750
		elseif magic_attack > 184 then
			return 1.5000
		else
			return 1.0000
		end
	elseif level == 7 then
		if magic_attack <= 197 then
			return 1.0000
		elseif magic_attack <= 204 and magic_attack > 197 then
			return 1.1250
		elseif magic_attack <= 211 and magic_attack > 204 then
			return 1.2500
		elseif magic_attack <= 218 and magic_attack > 211 then
			return 1.3750
		elseif magic_attack > 218 then
			return 1.5000
		else
			return 1.0000
		end
	elseif level == 8 then
		if magic_attack <= 228 then
			return 1.0000
		elseif magic_attack <= 236 and magic_attack > 228 then
			return 1.1250
		elseif magic_attack <= 244 and magic_attack > 236 then
			return 1.2500
		elseif magic_attack <= 252 and magic_attack > 244 then
			return 1.3750
		elseif magic_attack > 252 then
			return 1.5000
		else
			return 1.0000
		end
	elseif level == 9 then
		if magic_attack <= 259 then
			return 1.0000
		elseif magic_attack <= 268 and magic_attack > 259 then
			return 1.1250
		elseif magic_attack <= 277 and magic_attack > 268 then
			return 1.2500
		elseif magic_attack <= 286 and magic_attack > 277 then
			return 1.3750
		elseif magic_attack > 286 then
			return 1.5000
		else
			return 1.0000
		end
	elseif level == 10 then
		if magic_attack <= 290 then
			return 1.0000
		elseif magic_attack <= 300 and magic_attack > 290 then
			return 1.1250
		elseif magic_attack <= 310 and magic_attack > 300 then
			return 1.2500
		elseif magic_attack <= 320 and magic_attack > 310 then
			return 1.3750
		elseif magic_attack > 320 then
			return 1.5000
		else
			return 1.0000
		end
	else
		return 1.0000
	end
end

--水晶优劣
function shuijing(RECEIVER, ARG0, ARG1, random_pos,shuijing_magic)
	local caster_wind = Battle.get_prop(ARG0,ARG1,PT_Wind);
	local caster_land = Battle.get_prop(ARG0,ARG1,PT_Land);
	local caster_water = Battle.get_prop(ARG0,ARG1,PT_Water);
	local caster_fire = Battle.get_prop(ARG0,ARG1,PT_Fire);
	local target_wind = Battle.get_prop(ARG0,random_pos,PT_Wind);
	local target_land = Battle.get_prop(ARG0,random_pos,PT_Land);
	local target_water = Battle.get_prop(ARG0,random_pos,PT_Water);
	local target_fire = Battle.get_prop(ARG0,random_pos,PT_Fire);
	
	local a = caster_land*target_water+caster_water*target_fire+caster_fire*target_wind+caster_wind*target_land
	local b = caster_land*target_wind+caster_wind*target_fire+caster_fire*target_water+caster_water*target_land
	local magic_index = 1
	if shuijing_magic == nil then
		magic_index = 1
	elseif shuijing_magic == PT_Wind then
		magic_index = (100  + 0.345 * (10 * target_land)) / 100
	elseif shuijing_magic == PT_Land then 
		magic_index = (100  + 0.345 * (10 * target_water)) / 100
	elseif shuijing_magic == PT_Water then 
		magic_index = (100  + 0.345 * (10 * target_fire)) / 100
	elseif shuijing_magic == PT_Fire then 
		magic_index = (100  + 0.345 * (10 * target_wind)) / 100
	end
	local index = (100+0.3*(a-b))/100 * magic_index
	return index;
end


--巫术不能重复的判断
function check_wushu(RECEIVER, ARG0, caster_pos, target_pos)
	local check_gongwu = Battle.check_state(ARG0,target_pos,ST_ActionInvalid);
	if check_gongwu then
		return true;
	end
	local check_gongxi = Battle.check_state(ARG0,target_pos,ST_ActionAbsorb);
	if check_gongxi then
		return true;
	end
	local check_gongfan = Battle.check_state(ARG0,target_pos,ST_ActionBounce);
	if check_gongfan then
		return true;
	end
	local check_mowu = Battle.check_state(ARG0,target_pos,ST_MagicInvalid);
	if check_mowu then
		return true;
	end
	local check_moxi = Battle.check_state(ARG0,target_pos,ST_MagicAbsorb);
	if check_moxi then
		return true;
	end
	local check_mofan = Battle.check_state(ARG0,target_pos,ST_MagicBounce);
	if check_mofan then
		return true;
	end
	return false;
end


--咒术不能重复的判断
function check_zhoushu(RECEIVER, ARG0, caster_pos, target_pos)
	local check_zhongdu = Battle.check_state(ARG0,target_pos,ST_Poison);
	if check_zhongdu then
		return true;
	end
	local check_shihua = Battle.check_state(ARG0,target_pos,ST_Basilisk);
	if check_shihua then
		return true;
	end
	local check_hunshui = Battle.check_state(ARG0,target_pos,ST_Sleep);
	if check_hunshui then
		return true;
	end
	local check_hunluan = Battle.check_state(ARG0,target_pos,ST_Chaos);
	if check_hunluan then
		return true;
	end
	local check_jiuzui = Battle.check_state(ARG0,target_pos,ST_Drunk);
	if check_jiuzui then
		return true;
	end
	local check_yiwang = Battle.check_state(ARG0,target_pos,ST_Forget);
	if check_yiwang then
		return true;
	end
	return false;
end

--昏睡可以打醒
function remove_hunshui(RECEIVER, ARG0, target_pos,damage)
	if damage >= 0 then
		return;
	end
	local check_hunshui = Battle.check_state(ARG0,target_pos,ST_Sleep);
	if check_hunshui then
		Battle.remove_state(ARG0,target_pos,ST_Sleep)
	end
end


--连击暂时使用的攻击
function lianji_damage(RECEIVER, ARG0, caster_pos, target_pos,heji_num)
	--是否攻击无效
	local check_gongwu = Battle.check_state(ARG0,target_pos,ST_ActionInvalid);
	if check_gongwu then
		Battle.cutTime_state(ARG0,target_pos,ST_ActionInvalid,1)
		return 0;
	end
	local caster_attack = Battle.get_prop(ARG0,caster_pos,PT_Attack);
	local caster_attack_reality = atk_reality(caster_attack);
	local target_defense= Battle.get_prop(ARG0,target_pos,PT_Defense);
	local target_defense_reality = def_reality(target_defense);
	local damage = ( caster_attack_reality * caster_attack_reality * 3.0 ) / (caster_attack_reality + target_defense_reality * 3.0)*6/10;
	--是否石化
	local shihua = Battle.check_state(ARG0,target_pos,ST_Basilisk);
	if shihua then
		damage = damage / 2
	end
	--是否攻击吸收
	local a = 1;
	local check_gongxi = Battle.check_state(ARG0,target_pos,ST_ActionAbsorb);
	if check_gongxi then
		a = -1;
		Battle.cutTime_state(ARG0,target_pos,ST_ActionAbsorb,1)
	end
	--是否圣盾
	local check_shengdun = Battle.check_state(ARG0,target_pos,ST_Shield);
	if check_shengdun then
		local damage_table = {1,2,3,4,5,6,7,8,9,10}
		local damage_index = math.ceil(math.random(1,10));
		return -damage * damage_table[damage_index] / 100 * a;
	end
	--是否防御
	local check_fangyu = Battle.check_state(ARG0,target_pos,ST_Defense);
	if check_fangyu then
		local damage_table = {1,damage*10/100,damage*20/100,damage*30/100,damage*40/100,damage*50/100};
		local damage_index = math.ceil(math.random(1,6));
		return -damage_table[damage_index] * a;	
	end
	--是否攻击反弹
	local check_gongfan = Battle.check_state(ARG0,target_pos,ST_ActionBounce);
	if check_gongfan then
		return -damage * a;
	end
	--是否攻吸
	if check_gongxi or heji_num >= 2 then
		return -damage * a;
	end
	--是否闪避
	local check_shanbi =  check_dodge(RECEIVER, ARG0, caster_pos, target_pos,0);
	if check_shanbi then
		damage = 0;
		Battle.cutTime_state(ARG0,target_pos,ST_Dodge,1)
	end
	return -damage * a;
end

--检查阵营中是否有死人，返回死人位置
function isDeadthByForce(battleId,posTable,group_type)
	local pos = {};
	if GT_None == group_type 
	then 
		return BP_None;
	elseif GT_Down == group_type
	then
		pos = down_group_pos;
	elseif GT_Up == group_type
	then
		pos = up_group_pos;
	end
	local l_table = {};
	for i=1,table.getn(pos),1
		do
			if posTable[pos[i]] ~= 0
			then
				local tmpFoce = Battle.get_Force(battleId,pos[i]);
				local curHp = Battle.get_prop(battleId,pos[i],PT_HpCurr);
				--Sys.log("=======pos[i]=========>"..pos[i].."=tmpFoce==>"..tmpFoce.."========curHp==>"..curHp);
				if tmpFoce == group_type and curHp < 1 then
					table.insert(l_table,pos[i]);
				end
			end
		end
	return l_table;	
end

--查看是否有需要的技能
function checkSkill(skillTable,skillID)
	for i=1,table.getn(skillTable),1
		do
			if skillTable[i] == skillID then
				return true;
			end
	end
	return false;
end

function monsterpushOrder(RECEIVER,targetPos)
	Battle.ai_pushOrder(RECEIVER,targetPos,1);
end
