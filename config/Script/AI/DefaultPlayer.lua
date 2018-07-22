Sys.log("DefaultPlayer.lua")

--随机到技能3,6,7,12,13的话,重新随机
--随机到4（换位）的话目标就是自己的宠物位置（自己身前或者自己身后）
--1,8,9,10,11都是攻击,如果随到这几个的话,根据他带的武器给他变技能ID,
--如果空手就是1,如果带的是剑,斧,杖就是8,如果带的枪就是9,如果带的弓就是10,如果带的小刀就是11

--RECEIVER
--ARG_BATTLEID,		//0
--ARG_CASTERPOS,    //1
--ARG_TARGETPOS,	//2
--ARG_POSTABLE,		//3
--ARG_SKILLTABLE	//4

local DefaultSkill = 1;
local MaxRandomSkillTimes = 3;      --//随机技能最大次数 次数到了直接普通攻击

local WeaponSkills = {1,8,9,10,11};
local SkipSkills = {3,6,7,12,13,4};

function DefaultPlayer_Debug_Log(msg)
    if Debug then
        Sys.log("DefaultPlayer.lua "..msg);
    end
end

--//不能使用的技能
function DefaultPlayer_IsSkipSkill(id)
    for i=0, table.maxn(SkipSkills), 1 do
        if id == SkipSkills[i] then
            return true;
        end
    end
    return false;
end

--//武器相关技能
function DefaultPlayer_IsWeaponSkill(id)
     for i=0, table.maxn(WeaponSkills), 1 do
        if id == WeaponSkills[i] then
            return true;
        end
    end
    return false;
end

--//调整技能
function DefaultPlayer_SkillFixup(BATTLEID,pos,skillid)

    if (false == DefaultPlayer_IsSkipSkill(skillid)) and (false == DefaultPlayer_IsWeaponSkill(skillid)) then
        return skillid;
    end

    local lefthand = Battle.getItemType_BySlot(BATTLEID,pos,ES_SingleHand);
    local righhand = Battle.getItemType_BySlot(BATTLEID,pos,ES_DoubleHand);

    if lefthand == WT_None and righhand == WT_None then
        DefaultPlayer_Debug_Log(" No weapon!");
        return 1;
    end
    
    if lefthand == WT_Axe or lefthand == WT_Sword or lefthand == WT_Stick then
        return 8;
    end

    if lefthand == WT_Spear then
        return 9;
    end

    if lefthand == WT_Knife or lefthand == WT_V then
        return 11;
    end

    if righhand == WT_Bow then
        return 10;
    end

    DefaultPlayer_Debug_Log(" Unkown weapon!!");
    return 1;
end

--//选择技能
function DefaultPlayer_SwitchSkill(BATTLEID,pos,skills)
    
    local length = table.maxn(skills)
    if 0 == length then
	    DefaultPlayer_Debug_Log(" if 0 == length then");
		return;
	end
    local skillid = 0;

    for i=0, MaxRandomSkillTimes, 1 do
        local idx = math.ceil(math.random(0,length));
        skillid = skills[idx];
        DefaultPlayer_Debug_Log(" Radnom skill index "..idx);
        if  false == DefaultPlayer_IsSkipSkill(skillid) then
            break;
        end
    end
    
    return DefaultPlayer_SkillFixup(BATTLEID,pos,skillid);
end

--//选择目标
function DefaultPlayer_SwitchTaget(ARG0, ARG1, ARG2, ARG3,skillid)
    local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
    if Battle.getSkill_TargetType(ARG0,skillid) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,skillid) == STT_Team then
		--选择自己阵营的一个随机目标
		random_pos = random_TargetPos(ARG0, ARG2, ARG1, ARG3, false)
	elseif STT_Self == Battle.getSkill_TargetType(ARG0,skillid) then
        random_pos = ARG1;
    end
    return random_pos;
end

function DefaultPlayer_Battle_Do(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
    DefaultPlayer_Debug_Log("DefaultPlayer_Battle_Do "..table.concat(ARG4,"-"));
    --//偷袭
    if Battle.getSneakAttack(ARG0) == SAT_SneakAttack then
		DefaultPlayer_Debug_Log(" Battle.getSneakAttack(ARG0) == SAT_SneakAttack");
		Battle.ai_pushOrder(RECEIVER,taget,12);--//什么也不做
        return;
	end
    
    local skillid = DefaultPlayer_SwitchSkill(ARG0, ARG1, ARG4);
    local taget = DefaultPlayer_SwitchTaget(ARG0, ARG1, ARG2, ARG3, skillid);
    
    DefaultPlayer_Debug_Log(" Caster ("..RECEIVER..","..ARG1..") Target "..taget.." Skill "..skillid);
    Battle.ai_pushOrder(RECEIVER,taget,skillid);
end

function DefaultPlayer_Battle_Bron(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
    DefaultPlayer_Debug_Log("DefaultPlayer_Battle_Bron");
    DefaultPlayer_Battle_Do(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4);
end

function DefaultPlayer_Battle_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
    DefaultPlayer_Debug_Log("DefaultPlayer_Battle_Update");
    DefaultPlayer_Battle_Do(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4);
end


function DefaultPlayer_Battle_Death(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
    DefaultPlayer_Debug_Log("DefaultPlayer_Battle_Death");
end
