/** File generate by <hotlala8088@gmail.com> 2015/01/14  
 */

#ifndef __GLOBLE_CONSTANTS_H__
#define __GLOBLE_CONSTANTS_H__

#define VERSION_NUMBER  ( (MajorNumber<<24) | (MinorNumber<<16) | PatchNumber )

//-------------------------------------------------------------------------
/** 这里定义全局宏变量 
 *  例如 公式宏 等
 */

#define CALC_PARAMS(x) ( x * (x + x) ) 

enum PLATPROM_TYPE{
	NONE,
	ANY_SDK,
};

///走开放平台 LoginServer
#define SYNC_PROP_MAX 30
#define WAIT_BATTLE_ORDER_TIMEOUT 32
#define WAIT_BATTLE_CALC_ORDER_TIMEOUT 2
#define WAIT_BATTLE_CLOSE_TIMEOUT 600
#define MAX_PLAYERS 3
#define VALID_PING_TIME 5
#define TIMER_FREQ   20
#define SAVE_FREQ	 300  ///存储频率
#define SESSION_VALID_TIMEOUT 10 * 60
#define UPDATE_VISIBLE_PLAYERS_TIMEOUT 1.F
#define RANK_MAX	1000
#define ONE_DAY_SEC 86400
#define TRANSFOR_GUILD_MONEY(X) ((int)(X*0.1))

//#define DB_HOST "127.0.0.1"
//#define DB_USER "admin"
//#define DB_PASSWORD "password"
//#define DB_DATA_BASE_NAME "test"

//标志位
//0X80------ ///PLAYER
//0X40------ ///BABY
//0X20------ ///EMPLOYEE
//0X10------ ///ROBOT
//0X01------ ///AI

#define MASK_PLAYER		0X0
#define MASK_ROBOT		0X40000000
#define MASK_MONSTER	0X80000000
#define MAKE_GUID(MASK,ID) ((MASK)|(ID))
#define MAKE_PALYER_ID(ID)		ID
#define MAKE_BABY_ID(ID)		ID
#define MAKE_EMPLOYEE_ID(ID)	ID
#define MAKE_ROBOT_ID(ID)		MAKE_GUID(MASK_ROBOT,ID)
#define MAKE_MONSTER_ID(ID)		MAKE_GUID(MASK_MONSTER,ID)


#define SCENE_COPY_ID_OFFSET	(16)
#define SCENE_ORIGINAL_ID_MASK  (0X0000FFFF)
#define SCENE_COPY_ID_MASK (~SCENE_ORIGINAL_ID_MASK)
#define COMBINE_SCENE_COPY_ID(CopyId,SceneId) ((CopyId << SCENE_COPY_ID_OFFSET) | SceneId)
#define GET_SCENE_ORIGINAL_ID(InstId) (SCENE_ORIGINAL_ID_MASK & InstId)
#define GET_SCENE_COPY_ID(InstId) ((SCENE_COPY_ID_MASK & InstId) >> SCENE_COPY_ID_OFFSET)
#define EQ_SCENE_COPY_ID(CopyId,InstId) (CopyId == GET_SCENE_COPY_ID(InstId))
#define CALL_CLIENT(H,FUN)do{if(H && H->getClient()){H->getClient()->FUN;}else{/*ACE_DEBUG((LM_DEBUG,ACE_TEXT("Get client is NULL!!! %s %d\n"),__FILE__,__LINE__));*/} }while(0)


///一级属性最高加点
#define CALC_PLAYER_BASEPROP_MAX(LEVEL)(13+LEVEL*2)
///========================================================================
///@group 属性计算公式
///@{

#define CALC_CLEAN_PRO(INST) do \
{\
	INST.properties_[PT_HpMax]			= 0.f;\
	INST.properties_[PT_MpMax]			= 0.f;\
	INST.properties_[PT_Attack]			= 0.f;\
	INST.properties_[PT_Defense]		= 0.f;\
	INST.properties_[PT_Agile]			= 0.f;\
	INST.properties_[PT_Reply]			= 0.f;\
	INST.properties_[PT_Spirit]			= 0.f;\
	INST.properties_[PT_NoSleep]		= 0.f;\
	INST.properties_[PT_NoPetrifaction]	= 0.f;\
	INST.properties_[PT_NoDrunk]		= 0.f;\
	INST.properties_[PT_NoChaos]		= 0.f;\
	INST.properties_[PT_NoPoison]		= 0.f;\
	INST.properties_[PT_Magicattack]	= 0.f;\
	INST.properties_[PT_Hit]			= 0.f;\
	INST.properties_[PT_Dodge]			= 0.f;\
	INST.properties_[PT_Crit]			= 0.f;\
	INST.properties_[PT_counterpunch]	= 0.f;\
} while (0);

///< 一级属性转换二级属性 
///< 体力
#define CALC_PLAYER_PRO_TRANS_STAMA(INST,VAL) do{\
	INST.properties_[PT_HpMax]		+= VAL * 8.F;	\
	INST.properties_[PT_MpMax]		+= VAL * 1.F;	\
	INST.properties_[PT_Attack]	+= VAL * 0.1F;	\
	INST.properties_[PT_Defense] += VAL * 0.1F;	\
	INST.properties_[PT_Agile]	+= VAL * 0.1F;	\
	INST.properties_[PT_Reply]	+= VAL * 0.8F;	\
	INST.properties_[PT_Spirit]	+= VAL * -0.3F;	\
 } while (0);

///< 力量
#define CALC_PLAYER_PRO_TRANS_STRENGTH(INST,VAL) do{\
	INST.properties_[PT_HpMax]		+= VAL * 2.F;	\
	INST.properties_[PT_MpMax]		+= VAL * 2.F;	\
	INST.properties_[PT_Attack]	+= VAL * 2.F;	\
	INST.properties_[PT_Defense] += VAL * 0.2F;	\
	INST.properties_[PT_Agile]	+= VAL * 0.2F;	\
	INST.properties_[PT_Reply]	+= VAL * -0.1F;	\
	INST.properties_[PT_Spirit]	+= VAL * -0.1F;	\
 } while (0);

///<强度
#define CALC_PLAYER_PRO_TRANS_POWER(INST,VAL) do{\
	INST.properties_[PT_HpMax]		+= VAL * 3.F;	\
	INST.properties_[PT_MpMax]		+= VAL * 2.F;	\
	INST.properties_[PT_Attack]	+= VAL * 0.2F;	\
	INST.properties_[PT_Defense] += VAL * 2.F;	\
	INST.properties_[PT_Agile]	+= VAL * 0.2F;	\
	INST.properties_[PT_Reply]	+= VAL * -0.1F;	\
	INST.properties_[PT_Spirit]	+= VAL * 0.2F;	\
} while (0);

///<速度
#define CALC_PLAYER_PRO_TRANS_SPEED(INST,VAL) do{\
	INST.properties_[PT_HpMax]		+= VAL * 3.F;	\
	INST.properties_[PT_MpMax]		+= VAL * 2.F;	\
	INST.properties_[PT_Attack]	+= VAL * 0.2F;	\
	INST.properties_[PT_Defense] += VAL * 0.2F;	\
	INST.properties_[PT_Agile]	+= VAL * 2.F;	\
	INST.properties_[PT_Reply]	+= VAL * 0.2F;	\
	INST.properties_[PT_Spirit]	+= VAL * -0.1F;	\
} while (0);

///<魔法
#define CALC_PLAYER_PRO_TRANS_MAGIC(INST,VAL) do{\
	INST.properties_[PT_HpMax]		+= VAL * 1.F;	\
	INST.properties_[PT_MpMax]		+= VAL * 10.F;	\
	INST.properties_[PT_Attack]	+= VAL * 0.1F;	\
	INST.properties_[PT_Defense] += VAL * 0.1F;	\
	INST.properties_[PT_Agile]	+= VAL * 0.1F;	\
	INST.properties_[PT_Reply]	+= VAL * -0.3F;	\
	INST.properties_[PT_Spirit]	+= VAL * 0.8F;	\
} while (0);

///<宠物一级属性计算公式 1级属性计算公式为：0.042 * (GEAR+GEAR_DELTA(RAND))*(4+LEV )+ DELTA ( RAND < 10 )
///随机掉档值
#define CALC_BABY_GEAR(BASE) (BASE - UtlMath::randNM(0,4))
#define CALC_BABY_BASE(GEAR,LEVEL,GROW) ( GEAR * ( GROW *(LEVEL-1.F) + 0.2F ) )
#define CALC_BABY_LEVELUP(GEAR,GROW) ( GEAR * GROW )
//<计算宠物档位

///< 一级属性转换二级属性 
///< 体力

#define CALC_BABY_PRO_TRANS_STAMA(INST,VAL) do{\
	INST.properties_[PT_HpMax]		+= VAL * 8.F;	\
	INST.properties_[PT_MpMax]		+= VAL * 1.F;	\
	INST.properties_[PT_Attack]	+= VAL * 0.2F;	\
	INST.properties_[PT_Defense] += VAL * 0.2F;	\
	INST.properties_[PT_Agile]	+= VAL * 0.1F;	\
	INST.properties_[PT_Reply]	+= VAL * 0.8F;	\
	INST.properties_[PT_Spirit]	+= VAL * -0.3F;	\
 } while (0);

///< 力量
#define CALC_BABY_PRO_TRANS_STRENGTH(INST,VAL) do{\
	INST.properties_[PT_HpMax]		+= VAL * 2.F;	\
	INST.properties_[PT_MpMax]		+= VAL * 2.F;	\
	INST.properties_[PT_Attack]	+= VAL * 2.7F;	\
	INST.properties_[PT_Defense] += VAL * 0.3F;	\
	INST.properties_[PT_Agile]	+= VAL * 0.2F;	\
	INST.properties_[PT_Reply]	+= VAL * -0.1F;	\
	INST.properties_[PT_Spirit]	+= VAL * -0.1F;	\
 } while (0);

///<强度
#define CALC_BABY_PRO_TRANS_POWER(INST,VAL) do{\
	INST.properties_[PT_HpMax]		+= VAL * 3.F;	\
	INST.properties_[PT_MpMax]		+= VAL * 2.F;	\
	INST.properties_[PT_Attack]	+= VAL * 0.3F;	\
	INST.properties_[PT_Defense] += VAL * 3.F;	\
	INST.properties_[PT_Agile]	+= VAL * 0.2F;	\
	INST.properties_[PT_Reply]	+= VAL * -0.1F;	\
	INST.properties_[PT_Spirit]	+= VAL * 0.2F;	\
} while (0);

///<速度
#define CALC_BABY_PRO_TRANS_SPEED(INST,VAL) do{\
	INST.properties_[PT_HpMax]		+= VAL * 3.F;	\
	INST.properties_[PT_MpMax]		+= VAL * 2.F;	\
	INST.properties_[PT_Attack]	+= VAL * 0.3F;	\
	INST.properties_[PT_Defense] += VAL * 0.3F;	\
	INST.properties_[PT_Agile]	+= VAL * 2.F;	\
	INST.properties_[PT_Reply]	+= VAL * 0.2F;	\
	INST.properties_[PT_Spirit]	+= VAL * -0.1F;	\
} while (0);

///<魔法
#define CALC_BABY_PRO_TRANS_MAGIC(INST,VAL) do{\
	INST.properties_[PT_HpMax]		+= VAL * 1.F;	\
	INST.properties_[PT_MpMax]		+= VAL * 10.F;	\
	INST.properties_[PT_Attack]	+= VAL * 0.2F;	\
	INST.properties_[PT_Defense] += VAL * 0.2F;	\
	INST.properties_[PT_Agile]	+= VAL * 0.1F;	\
	INST.properties_[PT_Reply]	+= VAL * -0.3F;	\
	INST.properties_[PT_Spirit]	+= VAL * 0.8F;	\
} while (0);


///<计算出手公式 //(1+损血量/满血量+耗魔量/总魔量)*敏捷=下回合的敏捷参数
#define CALC_COMPUTE_ORDER(E,RESULT) do {\
	float l_Agile = E->getProp(PT_Agile);\
	float l_MpMax = E->getProp(PT_MpMax);\
	float l_HpMax = E->getProp(PT_HpMax);\
	float l_lossHp = E->getProp(PT_HpMax) - E->getProp(PT_HpCurr);\
	float l_lossMp = E->getProp(PT_MpMax) - E->getProp(PT_MpCurr);\
	RESULT = (1+l_lossHp/l_HpMax+l_lossMp/l_MpMax)*l_Agile;\
} while (0);

///@}

///========================================================================
///@group 攻防攻势
///@{

#define CALC_ATK_DAMAGE(CASTER_ATK,TARGET_DEF) ( ( (UtlMath::pow(CASTER_ATK,2) * 3.F) / (CASTER_ATK + TARGET_DEF * 3.F) ) * (float)(UtlMath::randNM(90,110)) / 100.F )

#define CALC_SNEAK_ATTACK(MONSTER_RATE,PLAYER_RATE,RATE) (RATE < MONSTER_RATE ? SAT_SurpriseAttack : (RATE < (MONSTER_RATE + PLAYER_RATE)? SAT_SneakAttack : SAT_None))	
///@}

#define EMPLOYEE_MAXNUM 100
//伙伴最大出战数量
#define BATTLEEMP_MAX 4
//反击最大次数
#define COUNTER_MAX  100
//JJC战斗信息最大存储数量
#define JJCBATTLEMSGNUM 10
#define  MAX_GUILD_NAME_LEN	12
//技能经验
#define CALC_SKILL_EXP 10

//战斗力公式
//基础属性战斗战斗力
#define CALC_BASE_FightingForce(E) (\
	((E)->properties_[PT_HpMax]	 * 3)	+ \
	((E)->properties_[PT_MpMax]	 * 2)		+ \
	((E)->properties_[PT_Attack] * 10)	+ \
	((E)->properties_[PT_Defense]* 8)	+ \
	((E)->properties_[PT_Agile]	 * 8)		+ \
	((E)->properties_[PT_Spirit] * 10)	+ \
	((E)->properties_[PT_Reply]	 * 8)	+ \
	((E)->properties_[PT_Hit] * 15)	+ \
	((E)->properties_[PT_Dodge] * 15)	+ \
	((E)->properties_[PT_Crit] * 20)	+ \
	((E)->properties_[PT_counterpunch] * 20)	+ \
	((E)->properties_[PT_Magicattack] * 8)	+ \
	((E)->properties_[PT_Magicdefense] * 8)	+ \
	((E)->properties_[PT_NoPetrifaction] * 15)	+ \
	((E)->properties_[PT_NoDrunk] * 15)	+ \
	((E)->properties_[PT_NoChaos] * 15)	+ \
	((E)->properties_[PT_NoForget] * 15)	+ \
	((E)->properties_[PT_NoPoison] * 15)\
)

#define CALC_BASE_FightingForceCOM(E) (\
	((E).properties_[PT_HpMax]	 * 3)	+ \
	((E).properties_[PT_MpMax]	 * 2)		+ \
	((E).properties_[PT_Attack] * 10)	+ \
	((E).properties_[PT_Defense]* 8)	+ \
	((E).properties_[PT_Agile]	 * 8)		+ \
	((E).properties_[PT_Spirit] * 10)	+ \
	((E).properties_[PT_Reply]	 * 8)	+ \
	((E).properties_[PT_Hit] * 15)	+ \
	((E).properties_[PT_Dodge] * 15)	+ \
	((E).properties_[PT_Crit] * 20)	+ \
	((E).properties_[PT_counterpunch] * 20)	+ \
	((E).properties_[PT_Magicattack] * 8)	+ \
	((E).properties_[PT_Magicdefense] * 8)	+ \
	((E).properties_[PT_NoPetrifaction] * 15)	+ \
	((E).properties_[PT_NoDrunk] * 15)	+ \
	((E).properties_[PT_NoChaos] * 15)	+ \
	((E).properties_[PT_NoForget] * 15)	+ \
	((E).properties_[PT_NoPoison] * 15)\
)

//技能影响战斗力
#define CALC_SKILL_FightingForce(S) (S->skLevel_ * 40)
#define CALC_SKILL_SPECIAL_FightingForce(S) (S->skLevel_ * 250)		//特殊职业
#define CALC_SKILL_FightingForceCOM(S) (S.skillLevel_ * 40)

//抓宠几率
//等级修正值=1+（(角色等级-目标等级）/（200+目标等级））*2
#define CALC_CATCH_BABY_INTERNAL(PLAYER,BABY)  ((((PLAYER)->properties_[PT_Level] - (BABY)->properties_[PT_Level] ) / (200 + (BABY)->properties_[PT_Level])) * 2  + 1 ) 
//概率计算结果最大值为100%，溢出部分不计，最小值0.01%
#define CALC_CATCH_BABY_FIX(PLAYER,BABY,RES) do{ RES *= CALC_CATCH_BABY_INTERNAL(PLAYER,BABY); RES = RES >= 1 ? 1 : RES;}while(0)
//列：基础概率=1000，等级修正系数=0.5，则最终抓捕成功概率=500，即5% 
//基础概率：万分比 ，表格中为1-10000
#define CALC_CATCH_BABY(PLAYER,BABY,BASE,RES) do { RES = (float)BASE / (float)10000 ; CALC_CATCH_BABY_FIX(PLAYER,BABY,RES); } while (0)

//经验值产出规则			
//怪物经验通过drop表中的EXP字段读取			
//经验衰减公式	衰减率=（角色等级-目标等级）*5%	公式计算没有负数，取值0%--100%	如怪物等级大于角色等级5级，或小于5级，均得出衰减率=25%
//经验值公式	所得经验值=exp*(1-衰减率)	角色与宠物通用此公式	获得经验最小值=1
//队伍经验	1个人的队伍里所有人没有加成，2个人的队伍加成4%，3个人6%，4个人8%，5个人10%，队长再额外加成5%	
//每个人获得经验根据以上经验规则单独计算	
#define CALC_NORMAL_EXP_INTERNAL(BASE,CASTER_LEVEL,TARGET_LEVEL,RES) do \
{\
	RES = UtlMath::abs(CASTER_LEVEL-TARGET_LEVEL) * 0.05F;\
	RES = RES<0.F?0.F:RES;\
	RES = RES>1.F?1.F:RES;\
	RES = 1.F-RES;\
	RES *= BASE;\
} while (0);

#define CALC_NORMAL_EXP(BASE,CASTER_LEVEL,TARGET_LEVEL,RES) do { CALC_NORMAL_EXP_INTERNAL(BASE,CASTER_LEVEL,TARGET_LEVEL,RES); RES = RES == 0 ? 1 : RES; } while (0)
#define CALC_LEADER_EXP(NORMAL_EXP)  ((S32)(NORMAL_EXP * 1.05F))
#define CALC_FREE_POINT(LEVEL) (LEVEL*4);

//伙伴回收 公式=2^品阶*10 
#define CALC_EMPLOYEERECYCLE_POINT(COLOR) (UtlMath::pow(2,COLOR)*10)
//答题收获
#define CALC_EXAM_RIGHT_MONEY(LEVEL) (10*(LEVEL + 10)*(LEVEL + 10)/LEVEL)
#define CALC_EXAM_RIGHT_EXP(LEVEL) (20*LEVEL*LEVEL + 2000)
#define CALC_EXAM_ERROR_MONEY(LEVEL) ((LEVEL + 10)*(LEVEL + 10)/LEVEL)
#define CALC_EXAM_ERROR_EXP(LEVEL) (2*LEVEL*LEVEL + 200)

//gg
enum { //佣兵抽奖
	GGT_Gold = 1000, //金币
	GGT_Diamond = GGT_Gold + 1000, //钻石
	GGT_Max = GGT_Diamond + 1000,
};

//经验加成BUFF id
enum{
	EXT_BUFF_ID_50 = 1002,
	EXT_BUFF_ID_75 = 1003,
	EXT_BUFF_ID_100 = 1004,
};

#define CALC_RAND_QUEST_EXP(COUNT,LEVEL) ((350 + 10 * COUNT) * LEVEL)

#endif