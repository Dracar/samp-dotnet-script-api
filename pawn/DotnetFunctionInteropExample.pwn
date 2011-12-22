
// Shows example of how to asynchronously call a dotnet script function from pawn
// player types "/dotnetfunction" and it prints "8, 12.0, BlaBla"

#include <a_samp>
#include "Dotnet.inc"

public OnPlayerCommandText(playerid, cmdtext[])
{
	if (strcmp(cmdtext, "/dotnetfunction", true) == 0)
	{
	    DotnetFunctionRequestExample(playerid);
		return 1;
	}
	return 0;
}

DotnetFunctionRequestExample(playerid)
{ // this calls our dotnet function

    new packet = Dotnet_NewFunctionRequestPacket("DotnetFunctionExample","DotnetFunctionCallback","iifs"); 
    // DotnetFunctionExample = the dotnet script function we want to call
    // DotnetFunctionCallback = the pawn function that dotnet will callback
    // iifs = the argument types we will be sending, i=int,f=float,s=string. make sure you get them right else client will prolly crash
    
    Dotnet_AddInt32ToPacket(packet,playerid);
    Dotnet_AddInt32ToPacket(packet,4);
    Dotnet_AddFloat32ToPacket(packet,6.0);
    Dotnet_AddCellStringToPacket(packet,"Bla");
    Dotnet_SendPacket(packet);
}

forward DotnetFunctionCallback(playerid, exampleint, Float:examplefloat,examplestring[]); // this is the function that will receive the reponse
public DotnetFunctionCallback(playerid, exampleint, Float:examplefloat,examplestring[])
{ // this is called by dotnet function
	new str[64];
	format(str, 64, "{0000FF}DotnetFunctionCallback received %d, %f, %s.",exampleint,examplefloat,examplestring);
	printf(str); 
    if (IsPlayerConnected(playerid)) SendClientMessage(playerid, 0, str);
    //"DotnetFunctionCallback received 8, 12.0, BlaBla"
}

