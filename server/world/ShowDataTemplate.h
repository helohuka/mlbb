#ifndef	__ShowDataTemplate__
#define	__ShowDataTemplate__
#include "config.h"

#define MAX_SHOW_DATA_SIZE 50

template<typename T>
class ShowDataTemplate
{
public:
	bool addData(T* t)
	{
		if(NULL != getData(t->showId_))
			return false;

		if(showData_.size() >= MAX_SHOW_DATA_SIZE)
		{
			T* data = showData_.front();
			if(NULL != data)
				DEL_MEM(data);
			showData_.pop_front();
		}
		showData_.push_back(t);

		return true;
	}

	T* getData(S32 showId)
	{
		typename std::list<T*>::reverse_iterator iter = showData_.rbegin();
		while(iter != showData_.rend())
		{
			if(showId == (*iter)->showId_)
			{
				return (*iter);
			}
			++iter;
		}

		return NULL;
	}
private:
	std::list<T*>	showData_;
};

typedef ShowDataTemplate<COM_ShowbabyInst>		ShowbabyInstData;
typedef ShowDataTemplate<COM_ShowItemInst>		ShowItemInstData;

#endif