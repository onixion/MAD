
![MAD - Network Monitoring Program](http://picload.org/image/cgoaowp/logo6.png)

MAD is an opensource network monitoring software, which should become an alternative version of Nagios, but shouldn't get as complex as Nagios. The main aim is to get a simple, easy-to-use, easy-understandable and nice-written C# program, which can analyse and monitoring a network.

##Things to implement:

- GUI
- CLI
- CLI-Server / CLI-Client
- NotificationSystem (e-mail)
- Database
- JobSystem

##What is currently working?

- CLI working. It can parse command, parameter and arguments. Some commands supports multiple arguments, no arguments and multiple types.
- JobSystem working. Currently available and working jobs are Ping-Request, PortScan, Http-Request, FTPCheck, DNSCheck and SNMP-Request.
- Jobs can have delaytime or multiple times (with dates) on which they should be executed (JobSystem have a scedule, which decides when a job should be executed)
- Jobs can define multiple JobRules, which decides when a notification is necessary; every job can have an own particular e-mail configuration (if it does not provide any, it uses the global settings from config)
- CLIServer/CLIClient working fully (no key-exchange, only AES)
- Database: the JobScedule stores all job outputs into the database, it is also possible to make a summarize of all stored data





