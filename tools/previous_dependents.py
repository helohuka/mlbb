
import os
import sys
import commands
import string
import datetime
import time
import sched  
import threading
import platform
import operator
import urllib

os.system('""%VS90COMNTOOLS%vsvars32.bat""')

def schedule(loadsize,blocksize,totalsize):
	per = 100.0 * loadsize * blocksize / totalsize
	if per > 100 :
		per = 100
	print '%.2f%%' % per

#http://downloads.activestate.com/ActivePerl/releases/5.24.0.2400/ActivePerl-5.24.0.2400-MSWin32-x64-300558.exe

cwd = os.getcwd()

def prepare():
	if os.path.exists('7za.exe'):
		message = 'OK, the 7za file exists.'
	else:
		local = os.path.join(cwd,'7za.exe')
		urllib.urlretrieve("ftp://10.10.10.252/pub/develop-kits/7za.exe",local,schedule)
		
	if os.path.exists('7z.dll'):
		message = 'OK, the 7za file exists.'
	else:
		local = os.path.join(cwd,'7z.dll')
		urllib.urlretrieve("ftp://10.10.10.252/pub/develop-kits/7z.dll",local,schedule)
		
	
	if os.path.exists('ActivePerl-5.24.0.2400-MSWin32-x64-300558.exe'):
		message = 'OK, the 7za file exists.'
	else:
		local = os.path.join(cwd,'ActivePerl-5.24.0.2400-MSWin32-x64-300558.exe')
		urllib.urlretrieve("ftp://10.10.10.252/pub/develop-kits/ActivePerl-5.24.0.2400-MSWin32-x64-300558.exe",local,schedule)
		os.system('ActivePerl-5.24.0.2400-MSWin32-x64-300558.exe')
		
	if os.path.exists('cmake-3.6.0-win64-x64.msi'):
		message = 'OK, the 7za file exists.'
	else:
		local = os.path.join(cwd,'cmake-3.6.0-win64-x64.msi')
		urllib.urlretrieve("ftp://10.10.10.252/pub/develop-kits/cmake-3.6.0-win64-x64.msi",local,schedule)
		os.system('cmake-3.6.0-win64-x64.msi')
		
	
prepare()

def replace(str, old, new):
	newstr = ""
	for ch in str:
		if ch == old:
			newstr = newstr + new
		else:
			newstr = newstr + ch
			
	return newstr

	
def prepare_ace():
	
	if os.path.exists('ACE-src-6.3.4.zip'):
		message = 'OK, the ACE file exists.'
	else:
		local = os.path.join(cwd,'ACE-src-6.3.4.zip')
		urllib.urlretrieve("ftp://10.10.10.252/pub/develop-kits/ACE-src-6.3.4.zip",local,schedule)
	
	os.system("7za x ACE-src-6.3.4.zip")
	
	os.chdir(cwd + '/ACE_wrappers')
	
	os.system('"SETX  ACE_ROOT ' + os.getcwd() + '"' )
	
	os.system('"bin\\mwc.pl -type nmake ace\\ace.mwc"')
	
	os.chdir(cwd + '/ACE_wrappers/ace')
	
	os.system('echo #define ACE_HAS_IPV6 > config.h')
	os.system('echo #include "config-win32.h" >> config.h')
	
	
	os.system('nmake')
	
	os.chdir(cwd)
	
prepare_ace()

def prepare_boost():
	if os.path.exists('boost_1_55_0.zip'):
		message = 'OK, the boost file exists.'
	else:
		local = os.path.join(cwd,'boost_1_55_0.zip')
		urllib.urlretrieve("ftp://10.10.10.252/pub/develop-kits/boost_1_55_0.zip",local,schedule)

	os.system("7za x boost_1_55_0.zip")
	
	os.chdir(cwd + '/boost_1_55_0')
	
	os.system('"bootstrap.bat"')
	
	os.system('"b2 --toolset=msvc-9.0 --with-system --with-filesystem"')
	
	os.system('"SETX  BOOST_INCLUDEDIR ' + os.getcwd() + '"' )
	os.system('"SETX  BOOST_LIBRARYDIR ' + os.getcwd() + '\\stage\\lib"' )
	
	os.chdir(cwd)
prepare_boost()

def prepare_sqlite():
	
	if os.path.exists('sqlite-amalgamation-3130000.zip'):
		message = 'OK, the sqlite file exists.'
	else:
		local = os.path.join(cwd,'sqlite-amalgamation-3130000.zip')
		urllib.urlretrieve("ftp://10.10.10.252/pub/develop-kits/sqlite-amalgamation-3130000.zip",local,schedule)
		
	os.system("7za x sqlite-amalgamation-3130000.zip")
	
	
	
	os.chdir(cwd + '/sqlite-amalgamation-3130000')
	
	os.system('"echo CMAKE_MINIMUM_REQUIRED(VERSION 2.6) > CMakelists.txt"')
	os.system('"echo PROJECT(sqlite3) >> CMakelists.txt"')
	os.system('"echo INCLUDE_DIRECTORIES(${CMAKE_CURRENT_SOURCE_DIR}) >> CMakelists.txt"')
	os.system('"echo SET(SRC sqlite3.c) >> CMakelists.txt"')
	os.system('"echo ADD_LIBRARY(sqlite3 STATIC  ${SRC}) >> CMakelists.txt"')
	
	os.system('"cmake -G "NMake Makefiles""') #-DCMAKE_BUILD_TYPE=Release
	
	os.system('""nmake""')
	
	os.system('"SETX  SQLITE_INCLUDEDIR ' + os.getcwd() + '"' )
	os.system('"SETX  SQLITE_LIBRARYDIR ' + os.getcwd() + '"' )
	
	os.chdir(cwd)
	
prepare_sqlite()
	
	
def prepare_mysql():
	if os.path.exists('mysql-connector-c++-noinstall-1.1.7-win32.zip'):
		message = 'OK, the mysql file exists.'
	else:
		local = os.path.join(cwd,'mysql-connector-c++-noinstall-1.1.7-win32.zip')
		urllib.urlretrieve("ftp://10.10.10.252/pub/develop-kits/mysql-connector-c++-noinstall-1.1.7-win32.zip",local,schedule)
	
	os.system("7za x mysql-connector-c++-noinstall-1.1.7-win32.zip")
	
	os.chdir(cwd + '/mysql-connector-c++-noinstall-1.1.7-win32')
	
	os.system('"SETX  MYSQL_INCLUDEDIR ' + os.getcwd() + '\\include"' )
	os.system('"SETX  MYSQL_LIBRARYDIR ' + os.getcwd() + '\\lib"' )
	
	os.chdir(cwd)
	
prepare_mysql()	

def prepare_curl():
	if os.path.exists('libcurl.zip'):
		message = 'OK, the libcurl file exists.'
	else:
		local = os.path.join(cwd,'libcurl.zip')
		urllib.urlretrieve("ftp://10.10.10.252/pub/develop-kits/libcurl.zip",local,schedule)

	
	os.system("7za x libcurl.zip")
	
	
	os.chdir(cwd + '/libcurl')
	
	os.system('"SETX  LIBCURL_INCLUDEDIR ' + os.getcwd() + '\\include"' )
	os.system('"SETX  LIBCURL_LIBRARYDIR ' + os.getcwd() + '\\lib"' )
	
	os.chdir(cwd)
	

prepare_curl()
	