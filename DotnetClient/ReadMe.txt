
Samp Dotnet Script API

Provide scripting API for SA:MP in any Dotnet language, via TCP interface. 



Install:
	Put DotnetServer.dll & DotnetServer.ini in your "\samp\plugins\" directory.
	Put DotnetFS.amx in your "\samp\filterscripts\" directory.
	Add "plugins DotnetServer" to your server.cfg.
	Add "filterscripts DotnetFS" to your server.cfg.
	Put DotnetClient.exe & DotnetClient.xml anywhere you want.
	Put scripts in "\yourdotnetclientdir\Scripts\". It will auto load any files ending in ".cs/.vb/.dll" from that directory.
	
Config:
	Open DotnetServer.ini & modify Port & AuthKey as desired.
	note: Make sure you do change AuthKey to something private, as it is the password required to login to the DotnetServer.
	note: If you are not running DotnetClient from a remote location then i recommend you firewall off all outside access to the DotnetServer port.
	Open DotnetClient.xml & modify Port & AuthKey to match what you put in DotnetServer.ini & modify Address as desired.
	
	
Known Bugs / Missing features:
	* No encryption.
	* No brute force protection.
	* No unicode support.
	* Can't return values from native callbacks.