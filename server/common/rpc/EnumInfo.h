#ifndef __EnumInfo_h__
#define __EnumInfo_h__

#include "Common.h"

/** 运行期enum信息. 
 * 组要负责在运行期进行id到名称的转换.
 */
class EnumInfo
{
public:
	EnumInfo(const std::string& name, void (*initFunc)(EnumInfo* einfo)):
	name_(name)
	{
		initFunc(this);
	}

	/** 从一个字符串转换为对应的enum item id.
		@param item enum item name.
		@return -1 表示转换失败.
	*/
	int getItemId(const std::string& item)
	{
		std::vector<std::string>::iterator r;
		r = std::find(items_.begin(), items_.end(), item);
		if(r == items_.end())
			return -1;
		return (int)(r - items_.begin());
	}

	/** 从一个enum item id转换为字符串名称.
		@param item enum item id.
		@return NULL 表示转换失败.
	*/
	const char* getItemName(int item)
	{
		if(item < 0 || item >= (int)items_.size())
			return NULL;
		return items_[item].c_str();
	}

	std::string						name_;
	std::vector<std::string>		items_;
};

/** 用来获取一个enum运行期信息的宏. */
#define ENUM(E)	enum##E


#endif//__EnumInfo_h__
