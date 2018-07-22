

wget ftp://10.10.10.252/pub/runtime-files/ACE_wrappers.zip

wget ftp://10.10.10.252/pub/runtime-files/boost_1_55_0.zip

wget ftp://10.10.10.252/pub/runtime-files/sqlite3.zip

wget ftp://10.10.10.252/pub/runtime-files/mysql-connector-c++-noinstall-1.1.7-win32.zip

wget ftp://10.10.10.252/pub/runtime-files/libcurl.zip


7za x ACE_wrappers.zip

7za x boost_1_55_0.zip

7za x sqlite3.zip

7za x mysql-connector-c++-noinstall-1.1.7-win32.zip

7za x libcurl.zip

SETX  ACE_ROOT %cd%\ACE_wrappers

SETX  BOOST_INCLUDEDIR %cd%\boost_1_55_0

SETX  BOOST_LIBRARYDIR %cd%\boost_1_55_0\stage\lib

SETX  SQLITE_INCLUDEDIR %cd%\sqlite3

SETX  SQLITE_LIBRARYDIR %cd%\sqlite3

SETX  MYSQL_INCLUDEDIR %cd%\mysql-connector-c++-noinstall-1.1.7-win32\include

SETX  MYSQL_LIBRARYDIR %cd%\mysql-connector-c++-noinstall-1.1.7-win32\lib

SETX  LIBCURL_INCLUDEDIR %cd%\libcurl\include

SETX  LIBCURL_LIBRARYDIR %cd%\libcurl\lib