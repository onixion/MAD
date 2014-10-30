
![MAD - Network Monitoring Program](http://picload.org/image/cgoaowp/logo6.png)

MAD is an opensource network monitoring software, which should become an alternative version of Nagios, but shouldn't get as complex as Nagios. The main aim is to get a simple, easy-to-use, easy-understandable and nice-written C# program, which can analyse and monitoring a network.

##Things to implement:

- GUI
- NotificationSystem (E-Mail ...)
- Database
- JobSystem (PingRequest, HttpRequest, PortRequest, Arp-Request, SNMP ...)
- CLI
- CLI-Server
- CLI-Client

##What is currently working?

- CLI working. It can parse command, parameter and arguments. Some commands supports multiple arguments, no arguments and multiple types.
- JobSystem working. Currently available and working jobs are Ping-Request, PortScan, Http-Request and HostDetection.
- Jobs can have delaytime or multiple times (with dates) on which they should be executed (JobSystem have a scedule, which decides when a job should be executed)
- Jobs can define multiple JobRules, which decides when a notification is necessary; every Job can have a own particular E-Mail configuration (if it does not provide any, it uses the global settings)
- CLIServer/CLIClient working fully (Key-Exchange over RSA2048; AES256)





