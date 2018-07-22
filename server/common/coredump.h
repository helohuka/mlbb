#ifndef __COREDUMP_H__
#define __COREDUMP_H__

#ifdef _WINDOWS_
#	include <DbgHelp.h>
#	pragma comment( lib, "dbghelp.lib" )

inline LONG core(LPEXCEPTION_POINTERS pExceptionInfo)
{
	LPSYSTEMTIME systemtime = new _SYSTEMTIME();
	::GetLocalTime(systemtime);

	char filename[1024];
	sprintf( filename, "%02d-%02d-%02d-%02d-%02d-%02d.dmp",systemtime->wYear, systemtime->wMonth, systemtime->wDay, systemtime->wHour, systemtime->wMinute, systemtime->wMilliseconds);
	HANDLE hfile = CreateFile(filename,GENERIC_WRITE,0,NULL,CREATE_ALWAYS,FILE_FLAG_BACKUP_SEMANTICS,NULL);
	if(hfile == INVALID_HANDLE_VALUE)
		return EXCEPTION_EXECUTE_HANDLER;
	MINIDUMP_EXCEPTION_INFORMATION minidump;
	minidump.ThreadId = GetCurrentThreadId();
	minidump.ExceptionPointers = pExceptionInfo;
	minidump.ClientPointers		= false;
	MiniDumpWriteDump(GetCurrentProcess(),GetCurrentProcessId(),hfile,MiniDumpNormal,&minidump,NULL,NULL);	
	CloseHandle(hfile);
	abort();
	
	return EXCEPTION_EXECUTE_HANDLER;//EXCEPTION_CONTINUE_SEARCH; 
}

static LPTOP_LEVEL_EXCEPTION_FILTER __CORE__ = (SetErrorMode(SEM_NOGPFAULTERRORBOX),SetUnhandledExceptionFilter(LPTOP_LEVEL_EXCEPTION_FILTER(&core))); 

inline void core()
{
	__try
	{
		RaiseException( EXCEPTION_BREAKPOINT, 0, 0, NULL);       
	}
	__except(core(GetExceptionInformation()))
	{

	}

}
#endif // _WINDOWS

#endif