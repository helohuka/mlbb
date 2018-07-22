/************************************************************************/
/**
 * @file	employeeouput.h
 * @date	2016/07/19 	
 * @author 	lwh            					
 * */
/************************************************************************/
#include "config.h"

#ifndef __EMPLOYEEOUTPUT_TABLE_H__
#define __EMPLOYEEOUTPUT_TABLE_H__

class Player;
class Employeeoutput
{
public:
	struct QualityWeight
	{
		QualityColor	color_;
		U32				goldWeight_;
		U32				diamondWeight_;
	};

	struct EmployeeoutputData
	{
		U32				id_;
		QualityColor	color_;
		U32				goldWeight_;
		U32				diamondWeight_;
	};
public:
	static bool qualityload(char const *fn);			//QualityWeight table
	static bool load(char const *fn);		//EmployeeWeight table
	static bool check();

public:
	static QualityColor randQualityGold();
	static QualityColor randQualityDiamond();

	//--十连抽时紫色与其他区分
	static U32	randOther();
	static U32	randPurple();
public:
	static U32	randGoldOnce();
	static U32  randDiamondOnce(Player* player);
	static void randDiamondTen(Player* player,std::vector<U32> &out);
	
public: 
	static void rollGreen(Player *player);
	static void rollBlue(Player *player);
	static void rollGold(Player *player);
	static void calcofflinetime(Player* player, float offlinetime);
public:
	static std::vector<QualityWeight*>	qwList_;
	static std::map< QualityColor,std::vector<EmployeeoutputData*> > data_;
};

#endif