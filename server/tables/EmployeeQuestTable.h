#ifndef __EMPLOYEE_QUEST_TABLES_H__
#define __EMPLOYEE_QUEST_TABLES_H__

class EmployeeSkill{
public:
	static bool load(char const* fn);
	static int32 getRestrain(int32 skillId, EmployeeSkillType type);
	static std::map<int32,std::vector<int32> > data_;
};

class EmployeeMonster{
public:
	int32 id_;
	std::vector<EmployeeSkillType> skills_;
public:
	static bool load(char const* fn);
	static EmployeeMonster const* find(int32 id);
	static std::map<int32, EmployeeMonster*> data_;
};



class EmployeeQuest{
public:
	int32 id_;
	EmployeeQuestColor color_;
	int32 timeRequier_;		//时间
	int32 employeeRequier_; //需要多少伙伴
	int32 successRate_;		//成功几率
	int32 cost_;			//花费
	std::vector<int32>	rewards_;	///奖励
	std::vector<int32> monsters_;

public:
	static bool load(char const* fn);
	static bool check();
	
	static EmployeeQuest const* getQuest(int32 id){
		return data_[id];
	}

	static std::vector<int32> const getColors(EmployeeQuestColor color){
		return colors_[color];
	}

	static std::vector< std::vector<int32> > colors_;
	static std::map<int32, EmployeeQuest*> data_;
};



#endif