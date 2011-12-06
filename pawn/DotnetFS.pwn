//
// DotnetFS
// List invokes & catch Callbacks

// todo: 0.3d callbacks & natives

#include <a_samp>

new HandleCommands = true;

const MAX_STRING = 32;
const MAX_PACKET = (MAX_STRING*3) + 20; // largest callback we will need to send
native DotnetServer_ReceiveCallback(packet[MAX_PACKET]); // our callback commands are sent as cell(4bytes) array

enum Callback
{
	opcode,
	name[32],
	paramtypes[8]
};

new Callbacks[][Callback] =
{
	{0,"NULL",""},
	{1,"OnEnterExitModShop","iii"},
	{2,"OnFilterScriptExit",""},
	{3,"OnFilterScriptInit",""},
	{4,"OnGameModeExit",""},
	{5,"OnGameModeInit",""},
	{6,"OnObjectMoved","i"},
	{7,"OnPlayerClickPlayer","iii"},
	{8,"OnPlayerCommandText","is"},
	{9,"OnPlayerConnect","i"},
	{10,"OnPlayerDeath","iii"},
	{11,"OnPlayerDisconnect","ii"},
	{12,"OnPlayerEnterCheckpoint","i"},
	{13,"OnPlayerEnterRaceCheckpoint","i"},
	{14,"OnPlayerEnterVehicle","iii"},
	{15,"OnPlayerExitVehicle","ii"},
	{16,"OnPlayerExitedMenu","i"},
	{17,"OnPlayerGiveDamage","iifi"},
	{18,"OnPlayerInteriorChange","iii"},
	{19,"OnPlayerKeyStateChange","iii"},
	{20,"OnPlayerLeaveCheckpoint","i"},
	{21,"OnPlayerLeaveRaceCheckpoint","i"},
	{22,"OnPlayerObjectMoved","ii"},
	{23,"OnPlayerPickUpPickup","ii"},
	{24,"OnPlayerPrivmsg","iis"},
	{25,"OnPlayerRequestClass","ii"},
	{26,"OnPlayerRequestSpawn","i"},
	{27,"OnPlayerSelectedMenuRow","ii"},
	{28,"OnPlayerSpawn","i"},
	{29,"OnPlayerStateChange","iii"},
	{30,"OnPlayerStreamIn","ii"},
	{31,"OnPlayerStreamOut","ii"},
	{32,"OnPlayerTakeDamage","iifi"},
	{33,"OnPlayerTakeDamage RU","iifi"},
	{34,"OnPlayerTeamPrivmsg","is"},
	{35,"OnPlayerText","is"},
	{36,"OnPlayerUpdate","i"},
	{37,"OnRconCommand","s"},
	{38,"OnRconLoginAttempt","ssi"},
	{39,"OnUnoccupiedVehicleUpdate","iii"}, // Important Note: This callback is called very frequently per second per unoccupied vehicle.
	{40,"OnVehicleDamageStatusUpdate","ii"},
	{41,"OnVehicleDeath","i"},
	{42,"OnVehicleMod","iii"},
	{43,"OnVehiclePaintjob","iii"},
	{44,"OnVehicleRespray","iiii"},
	{45,"OnVehicleSpawn","i"},
	{46,"OnVehicleStreamIn","ii"},
	{47,"OnVehicleStreamOut","ii"},
	{48,"OnDialogResponse","iiiis"}
};

GetCallbackByName(sname[32])
{
	new size = 49;//sizeof(Callbacks[][]);
	for(new i=0;i<size;i++)
	{
		if (strcmp(Callbacks[i][name],sname) == 0) return i;
	}
	return 0;
}

GetCallbackById(id)
{
	new size = 49;//sizeof(Callbacks[][]);
	for(new i=0;i<size;i++)
	{
		if (Callbacks[i][opcode] == id) return i;
	}
	return 0;
}

new CallbackPacket[MAX_PACKET];
new CallbackPacketPos = 1; // first 4 bytes is size

AddInt32ToCallbackPacket(int32)
{
    CallbackPacket[CallbackPacketPos] = int32;
    CallbackPacketPos += 1;
}
AddStringToCallbackPacket(str[])
{
	for (new i=0;i<strlen(str);i++)
	{
    	CallbackPacket[CallbackPacketPos+i] = str[i];
    }
   	for (new i=strlen(str);i<MAX_STRING;i++)
	{
    	CallbackPacket[CallbackPacketPos+i] = 0; // null remaining chars
    }
    //printf("pawn addstr %d %s", strlen(str), str);
    CallbackPacketPos += MAX_STRING;
}

ClearCallbackPacket()
{
	for (new i=0;i<MAX_PACKET;i++)
	{
    	CallbackPacket[i] = 0; // null the whole packet
    }
    CallbackPacketPos = 1;
}

SendCallbackPacket()
{ // sends the packet to our c++ plugin
	for (new i=CallbackPacketPos;i<MAX_PACKET;i++)
	{
	    CallbackPacket[i] = 0; // null all the remaining bytes, just incase
	}
	new paksize = CallbackPacketPos+1;
	CallbackPacketPos = 0;
	AddInt32ToCallbackPacket(paksize);
	DotnetServer_ReceiveCallback(CallbackPacket);
}







public OnEnterExitModShop(playerid, enterexit, interiorid)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnEnterExitModShop")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddInt32ToCallbackPacket(enterexit);
    AddInt32ToCallbackPacket(interiorid);
    SendCallbackPacket();
    return 1;
}
public OnFilterScriptExit()
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnFilterScriptExit")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    SendCallbackPacket();
    return 1;
}
public OnFilterScriptInit()
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnFilterScriptInit")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    SendCallbackPacket();
    return 1;
}
public OnGameModeExit()
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnGameModeExit")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    SendCallbackPacket();
    return 1;
}
public OnGameModeInit()
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnGameModeInit")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    SendCallbackPacket();
    return 1;
}
public OnObjectMoved(objectid)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnObjectMoved")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(objectid);
    SendCallbackPacket();
    return 1;
}
public OnPlayerClickPlayer(playerid, clickedplayerid, source)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerClickPlayer")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddInt32ToCallbackPacket(clickedplayerid);
    AddInt32ToCallbackPacket(source);
    SendCallbackPacket();
    return 1;
}

public OnPlayerCommandText(playerid, cmdtext[])
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerCommandText")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddStringToCallbackPacket(cmdtext);
    SendCallbackPacket();
    if (HandleCommands) return 1;
    return 0;
}

public OnPlayerConnect(playerid)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerConnect")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    SendCallbackPacket();
    return 1;
}
public OnPlayerDeath(playerid, killerid, reason)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerDeath")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddInt32ToCallbackPacket(killerid);
    AddInt32ToCallbackPacket(reason);
    SendCallbackPacket();
    return 1;
}
public OnPlayerDisconnect(playerid, reason)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerDisconnect")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddInt32ToCallbackPacket(reason);
    SendCallbackPacket();
    return 1;
}
public OnPlayerEnterCheckpoint(playerid)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerEnterCheckpoint")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    SendCallbackPacket();
    return 1;
}
public OnPlayerEnterRaceCheckpoint(playerid)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerEnterRaceCheckpoint")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    SendCallbackPacket();
    return 1;
}
public OnPlayerEnterVehicle(playerid, vehicleid, ispassenger)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerEnterVehicle")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddInt32ToCallbackPacket(vehicleid);
    AddInt32ToCallbackPacket(ispassenger);
    SendCallbackPacket();
    return 1;
}
public OnPlayerExitVehicle(playerid, vehicleid)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerExitVehicle")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddInt32ToCallbackPacket(vehicleid);
    SendCallbackPacket();
    return 1;
}
public OnPlayerExitedMenu(playerid)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerExitedMenu")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    SendCallbackPacket();
    return 1;
}

/*
public OnPlayerGiveDamage(param0_i, param1_i, param2_f, param3_i)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerGiveDamage")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(param0_i);
    AddInt32ToCallbackPacket(param1_i);
    AddFloat32ToCallbackPacket(param2_f);
    AddInt32ToCallbackPacket(param3_i);
    SendCallbackPacket();
    return 1;
}*/

public OnPlayerInteriorChange(playerid, newinteriorid, oldinteriorid)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerInteriorChange")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddInt32ToCallbackPacket(newinteriorid);
    AddInt32ToCallbackPacket(oldinteriorid);
    SendCallbackPacket();
    return 1;
}
public OnPlayerKeyStateChange(playerid, newkeys, oldkeys)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerKeyStateChange")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddInt32ToCallbackPacket(newkeys);
    AddInt32ToCallbackPacket(oldkeys);
    SendCallbackPacket();
    return 1;
}
public OnPlayerLeaveCheckpoint(playerid)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerLeaveCheckpoint")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    SendCallbackPacket();
    return 1;
}
public OnPlayerLeaveRaceCheckpoint(playerid)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerLeaveRaceCheckpoint")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    SendCallbackPacket();
    return 1;
}


public OnPlayerObjectMoved(playerid, objectid)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerObjectMoved")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddInt32ToCallbackPacket(objectid);
    SendCallbackPacket();
    return 1;
}
public OnPlayerPickUpPickup(playerid, pickupid)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerPickUpPickup")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddInt32ToCallbackPacket(pickupid);
    SendCallbackPacket();
    return 1;
}

/*
public OnPlayerPrivmsg(playerid, param1_i, param2_s)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerPrivmsg")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddInt32ToCallbackPacket(param1_i);
    AddStringToCallbackPacket(param2_s);
    SendCallbackPacket();
    return 1;
}
*/
public OnPlayerRequestClass(playerid, classid)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerRequestClass")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddInt32ToCallbackPacket(classid);
    SendCallbackPacket();
    return 1;
}
public OnPlayerRequestSpawn(playerid)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerRequestSpawn")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    SendCallbackPacket();
    return 1;
}
public OnPlayerSelectedMenuRow(playerid, row)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerSelectedMenuRow")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddInt32ToCallbackPacket(row);
    SendCallbackPacket();
    return 1;
}
public OnPlayerSpawn(playerid)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerSpawn")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    SendCallbackPacket();
    return 1;
}
public OnPlayerStateChange(playerid, newstate, oldstate)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerStateChange")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddInt32ToCallbackPacket(newstate);
    AddInt32ToCallbackPacket(oldstate);
    SendCallbackPacket();
    return 1;
}
public OnPlayerStreamIn(playerid, forplayerid)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerStreamIn")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddInt32ToCallbackPacket(forplayerid);
    SendCallbackPacket();
    return 1;
}
public OnPlayerStreamOut(playerid, forplayerid)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerStreamOut")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddInt32ToCallbackPacket(forplayerid);
    SendCallbackPacket();
    return 1;
}
/*
public OnPlayerTakeDamage(playerid, param1_i, param2_f, param3_i)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerTakeDamage")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddInt32ToCallbackPacket(param1_i);
    AddFloat32ToCallbackPacket(param2_f);
    AddInt32ToCallbackPacket(param3_i);
    SendCallbackPacket();
    return 1;
}
public OnPlayerTakeDamageRU(playerid, param1_i, param2_f, param3_i)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerTakeDamageRU")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddInt32ToCallbackPacket(param1_i);
    AddFloat32ToCallbackPacket(param2_f);
    AddInt32ToCallbackPacket(param3_i);
    SendCallbackPacket();
    return 1;
}
*/
/*
public OnPlayerTeamPrivmsg(playerid, param1_s)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerTeamPrivmsg")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddStringToCallbackPacket(param1_s);
    SendCallbackPacket();
    return 1;
}
*/
public OnPlayerText(playerid, text[])
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerText")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddStringToCallbackPacket(text);
    SendCallbackPacket();
    return 1;
}

public OnPlayerUpdate(playerid)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnPlayerUpdate")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    SendCallbackPacket();
    return 1;
}
public OnRconCommand(cmd[])
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnRconCommand")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddStringToCallbackPacket(cmd);
    SendCallbackPacket();
    return 1;
}
public OnRconLoginAttempt(ip[], password[], success)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnRconLoginAttempt")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddStringToCallbackPacket(ip);
    AddStringToCallbackPacket(password);
    AddInt32ToCallbackPacket(success);
    SendCallbackPacket();
    return 1;
}/*
public OnUnoccupiedVehicleUpdate(param0_i, param1_i, param2_i)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnUnoccupiedVehicleUpdate")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(param0_i);
    AddInt32ToCallbackPacket(param1_i);
    AddInt32ToCallbackPacket(param2_i);
    SendCallbackPacket();
    return 1;
}*/
public OnVehicleDamageStatusUpdate(vehicleid, playerid)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnVehicleDamageStatusUpdate")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(vehicleid);
    AddInt32ToCallbackPacket(playerid);
    SendCallbackPacket();
    return 1;
}
public OnVehicleDeath(vehicleid)
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnVehicleDeath")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(vehicleid);
    SendCallbackPacket();
    return 1;
}

public OnDialogResponse(playerid, dialogid, response, listitem, inputtext[])
{
    ClearCallbackPacket();
    new callbackid = Callbacks[GetCallbackByName("OnDialogResponse")][opcode];
    AddInt32ToCallbackPacket(callbackid);
    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);
    AddInt32ToCallbackPacket(playerid);
    AddInt32ToCallbackPacket(dialogid);
    AddInt32ToCallbackPacket(response);
    AddInt32ToCallbackPacket(listitem);
    AddStringToCallbackPacket(inputtext);
    SendCallbackPacket();
    return 1;
}




// --------------------------------------------
// One huge function calling most of SA-MP's functions;
// --------------------------------------------
    forward InvokeFunction();
    public InvokeFunction()
    {
            new Float:fVar;
            new Var[ 256 ];
            new iVar;
     
            // a_samp.inc
            SendClientMessage(0, 0, "");
            SendClientMessageToAll(0, "");
            SendDeathMessage(0, 0, 0);
            GameTextForAll("", 0, 0);
            GameTextForPlayer(0, "", 0, 0);
            GetTickCount();
            GetMaxPlayers();
            SetGameModeText("");
            SetTeamCount(0);
            AddPlayerClass(0, 0.0, 0.0, 0.0, 0.0, 0, 0, 0, 0, 0, 0);
            AddPlayerClassEx(0, 0, 0.0, 0.0, 0.0, 0.0, 0, 0, 0, 0, 0, 0);
            AddStaticVehicle(0, 0.0, 0.0, 0.0, 0.0, 0, 0);
            AddStaticVehicleEx(0, 0.0, 0.0, 0.0, 0.0, 0, 0, 0);
            AddStaticPickup(0, 0, 0.0, 0.0, 0.0);
            ShowNameTags(0);
            ShowPlayerMarkers(0);
            GameModeExit();
            SetWorldTime(0);
            GetWeaponName(0, Var, sizeof( Var ) );
            EnableTirePopping(0);
            AllowInteriorWeapons(0);
            SetWeather(0);
            SetGravity(0.0);
            AllowAdminTeleport(0);
            SetDeathDropAmount(0);
            CreateExplosion(0.0, 0.0, 0.0, 0, 0.0);
            //SetDisabledWeapons();
            EnableZoneNames(0);
            IsPlayerAdmin(0);
            Kick(0);
            Ban(0);
            SendRconCommand("");
            ShowPlayerDialog(0,0,0,"lol","lol","lol","lol");
     
            // a_players.inc
            SetSpawnInfo(0, 0, 0, 0.0, 0.0, 0.0, 0.0, 0, 0, 0, 0, 0,0);
            SpawnPlayer(0);
            SetPlayerPos(0, 0.0, 0.0, 0.0);
    //      SetPlayerPosFindZ(0, 0.0, 0.0, 0.0);
            GetPlayerPos(0, fVar, fVar, fVar);
            SetPlayerFacingAngle(0,0.0);
            GetPlayerFacingAngle(0,fVar);
            SetPlayerInterior(0,0);
            GetPlayerInterior(0);
            SetPlayerHealth(0, 0.0);
            GetPlayerHealth(0, fVar);
            SetPlayerArmour(0, 0.0);
            GetPlayerArmour(0, fVar);
            SetPlayerAmmo(0, 0,0);
            GetPlayerAmmo(0);
            SetPlayerTeam(0,0);
            GetPlayerTeam(0);
            SetPlayerScore(0,0);
            GetPlayerScore(0);
            SetPlayerColor(0,0);
            GetPlayerColor(0);
            SetPlayerSkin(0,0);
            GivePlayerWeapon(0, 0,0);
            ResetPlayerWeapons(0);
            GetPlayerWeaponData(0, 0, iVar, iVar );
            GivePlayerMoney(0,0);
            ResetPlayerMoney(0);
            SetPlayerName(0, "");
            GetPlayerMoney(0);
            GetPlayerState(0);
            GetPlayerIp(0, Var, sizeof( Var ));
            GetPlayerPing(0);
            GetPlayerWeapon(0);
            GetPlayerKeys(0,iVar,iVar,iVar);
            GetPlayerName(0, Var, sizeof( Var ));
            PutPlayerInVehicle(0, 0,0);
            GetPlayerVehicleID(0);
            RemovePlayerFromVehicle(0);
            TogglePlayerControllable(0,0);
            PlayerPlaySound(0, 0, 0.0, 0.0,0.0);
            SetPlayerCheckpoint(0, 0.0, 0.0, 0.0,0.0);
            DisablePlayerCheckpoint(0);
            SetPlayerRaceCheckpoint(0, 0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,0.0);
            DisablePlayerRaceCheckpoint(0);
            SetPlayerWorldBounds(0,0.0,0.0,0.0,0.0);
            SetPlayerMarkerForPlayer(0, 0,0);
            ShowPlayerNameTagForPlayer(0, 0,0);
            SetPlayerMapIcon(0, 0, 0.0, 0.0, 0.0, 0,0);
            RemovePlayerMapIcon(0,0);
            SetPlayerCameraPos(0,0.0, 0.0, 0.0);
            SetPlayerCameraLookAt(0, 0.0, 0.0, 0.0);
            SetCameraBehindPlayer(0);
            AllowPlayerTeleport(0,0);
            IsPlayerConnected(0);
            IsPlayerInVehicle(0,0);
            IsPlayerInAnyVehicle(0);
            IsPlayerInCheckpoint(0);
            IsPlayerInRaceCheckpoint(0);
            SetPlayerTime(0, 0,0);
            TogglePlayerClock(0,0);
            SetPlayerWeather(0,0);
            GetPlayerTime(0,iVar,iVar);
            SetPlayerVirtualWorld(0,0);
            GetPlayerVirtualWorld(0);
     
            // a_vehicle.inc
            CreateVehicle(0,0.0,0.0,0.0,0.0,0,0,0);
            DestroyVehicle(0);
            GetVehiclePos(0,fVar,fVar,fVar);
            SetVehiclePos(0,0.0,0.0,0.0);
            GetVehicleZAngle(0,fVar);
            SetVehicleZAngle(0,0.0);
            SetVehicleParamsForPlayer(0,0,0,0);
            SetVehicleToRespawn(0);
            LinkVehicleToInterior(0,0);
            AddVehicleComponent(0,0);
            ChangeVehicleColor(0,0,0);
            ChangeVehiclePaintjob(0,0);
            SetVehicleHealth(0,0.0);
            GetVehicleHealth(0,fVar);
            AttachTrailerToVehicle(0, 0);
            DetachTrailerFromVehicle(0);
            IsTrailerAttachedToVehicle(0);
            GetVehicleModel(0);
            SetVehicleNumberPlate(0,"");
            SetVehicleVirtualWorld(0,0);
            GetVehicleVirtualWorld(0);
            GetVehicleVelocity(0,fVar,fVar,fVar);
     		SetVehicleVelocity(0,0.0,0.0,0.0);
     
            ApplyAnimation(0,"","",1.0,0,0,0,0,0);
     
            // a_objects.inc
            CreateObject(0,0.0,0.0,0.0,0.0,0.0,0.0);
            SetObjectPos(0,0.0,0.0,0.0);
            GetObjectPos(0,fVar,fVar,fVar);
            SetObjectRot(0,0.0,0.0,0.0);
            GetObjectRot(0,fVar,fVar,fVar);
            IsValidObject(0);
            DestroyObject(0);
            MoveObject(0,0.0,0.0,0.0,0.0);
            StopObject(0);
            CreatePlayerObject(0,0,0.0,0.0,0.0,0.0,0.0,0.0);
            SetPlayerObjectPos(0,0,0.0,0.0,0.0);
            GetPlayerObjectPos(0,0,fVar,fVar,fVar);
            GetPlayerObjectRot(0,0,fVar,fVar,fVar);
            SetPlayerObjectRot(0,0,0.0,0.0,0.0);
            IsValidPlayerObject(0,0);
            DestroyPlayerObject(0,0);
            MovePlayerObject(0,0,0.0,0.0,0.0,0.0);
            StopPlayerObject(0,0);
            AttachObjectToPlayer(0,0,0.0,0.0,0.0,0.0,0.0,0.0);
     
            // Menu's
            CreateMenu("", 0, 0.0, 0.0, 0.0, 0.0);
            DestroyMenu(Menu:0);
            AddMenuItem(Menu:0, 0, "");
            SetMenuColumnHeader(Menu:0, 0, "");
            ShowMenuForPlayer(Menu:0, 0);
            HideMenuForPlayer(Menu:0, 0);
            IsValidMenu(Menu:0);
            DisableMenu(Menu:0);
            DisableMenuRow(Menu:0,0);
     
            // Textdraw
            TextDrawCreate(0.0,0.0,"");
            TextDrawDestroy(Text:0);
            TextDrawLetterSize(Text:0, 0.0,0.0);
            TextDrawTextSize(Text:0, 0.0,0.0);
            TextDrawAlignment(Text:0, 0);
            TextDrawColor(Text:0,0);
            TextDrawUseBox(Text:0, 0);
            TextDrawBoxColor(Text:0, 0);
            TextDrawSetShadow(Text:0, 0);
            TextDrawSetOutline(Text:0, 0);
            TextDrawBackgroundColor(Text:0,0);
            TextDrawFont(Text:0, 0);
            TextDrawSetProportional(Text:0, 0);
            TextDrawShowForPlayer(0, Text:0);
            TextDrawHideForPlayer(0, Text:0);
            TextDrawShowForAll(Text:0);
            TextDrawHideForAll(Text:0);
     
            // Others
            funcidx("");
            gettime(iVar,iVar,iVar);
            getdate(iVar,iVar,iVar);
            tickcount(iVar);
            
            GetPlayerCameraPos(0, fVar, fVar, fVar);
            GetPlayerCameraFrontVector(0, fVar, fVar, fVar);
            return 1;
    }
	

 
