#include "config.h"
#include "ScriptHandle.h"

U32 ScriptHandle::idMaker_;
std::vector<U32> ScriptHandle::idStore_;
std::map< U32 , ScriptHandle *> ScriptHandle::handles_;

ScriptHandle::ScriptHandle(){
	handleId_ = makeId();
	handles_[handleId_] = this;
}

ScriptHandle::~ScriptHandle(){
	storeId(handleId_);
	handles_[handleId_] = NULL;
}

ScriptHandle* 
ScriptHandle::getScriptHandleById(U32 scriptId){
	return handles_[scriptId];
}