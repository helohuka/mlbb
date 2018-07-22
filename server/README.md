
## Production environment
### CentOS6.X
### Change /etc/security/limits.conf
#### * soft core 	unlimited
#### * hard core 	unlimited
#### * soft nofile 	65535
#### * hard nofile 	65535
### Change /etc/sysctl.conf
#### kernel.core_pattern=core.%e.%s.%t
### Mysql5.6 use innodb
### Change my.cnf
#### skip-name-resolve
#### long_query_time=1
#### log_queries_not_using_indexes
#### slow_query_log=ON

## Development environment

## Depends
### ACE6.3+             https://www.cse.wustl.edu/~schmidt/ACE.html
### Boost1.56+          https://www.boost.org
### mysqlcppconn1.1.x   https://dev.mysql.com/downloads/connector/cpp
### libcurl
### libz

## On Ubuntu 

### sudo apt-get install libboost-dev libboost-system-dev libboost-filesystem-dev libz-dev libcurl4-openssl-dev cmake gcc gcc-c++

## On CentOS

### yum -y groupinstall Development tools
### yum -y install epel-release
### yum -y install zlib-devel 
### yum -y install libcurl-devel
### Get boost libraries version 1.55.0+
### Get mysql-dev and download mysql-connector-c++ (5.7)

## On Macosx

### install macports https://www.macports.org
###
