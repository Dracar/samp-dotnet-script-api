//
// DotnetFS
// List invokes & catch Callbacks

// todo: 0.3d callbacks & natives


#include <a_samp>
#include "Dotnet.inc"

new HandleCommands = true;

forward DotnetFSInit();
public DotnetFSInit()
{
	// stuff
}






// -------- Callbacks ----------

public OnEnterExitModShop(playerid, enterexit, interiorid)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnEnterExitModShop");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddInt32ToPacket(pak,enterexit);
    Dotnet_AddInt32ToPacket(pak,interiorid);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnFilterScriptExit()
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnFilterScriptExit");
    Dotnet_SendPacket(pak);
    return 1;
}

public OnFilterScriptInit()
{
	new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnFilterScriptInit");
    Dotnet_SendPacket(pak);
    return 1;
}

public OnGameModeExit()
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnGameModeExit");
    Dotnet_SendPacket(pak);
    return 1;
}
public OnGameModeInit()
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnGameModeInit");
    Dotnet_SendPacket(pak);
    return 1;
}
public OnObjectMoved(objectid)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnObjectMoved");
    Dotnet_AddInt32ToPacket(pak,objectid);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerClickPlayer(playerid, clickedplayerid, source)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerClickPlayer");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddInt32ToPacket(pak,clickedplayerid);
    Dotnet_AddInt32ToPacket(pak,source);
    Dotnet_SendPacket(pak);
    return 1;
}

public OnPlayerCommandText(playerid, cmdtext[])
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerCommandText");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddCellStringToPacket(pak,cmdtext);
    Dotnet_SendPacket(pak);
    if (HandleCommands) return 1;
    return 0;
}

public OnPlayerConnect(playerid)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerConnect");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerDeath(playerid, killerid, reason)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerDeath");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddInt32ToPacket(pak,killerid);
    Dotnet_AddInt32ToPacket(pak,reason);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerDisconnect(playerid, reason)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerDisconnect");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddInt32ToPacket(pak,reason);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerEnterCheckpoint(playerid)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerEnterCheckpoint");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerEnterRaceCheckpoint(playerid)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerEnterRaceCheckpoint");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerEnterVehicle(playerid, vehicleid, ispassenger)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerEnterVehicle");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddInt32ToPacket(pak,vehicleid);
    Dotnet_AddInt32ToPacket(pak,ispassenger);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerExitVehicle(playerid, vehicleid)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerExitVehicle");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddInt32ToPacket(pak,vehicleid);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerExitedMenu(playerid)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerExitedMenu");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_SendPacket(pak);
    return 1;
}

public OnPlayerInteriorChange(playerid, newinteriorid, oldinteriorid)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerInteriorChange");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddInt32ToPacket(pak,newinteriorid);
    Dotnet_AddInt32ToPacket(pak,oldinteriorid);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerKeyStateChange(playerid, newkeys, oldkeys)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerKeyStateChange");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddInt32ToPacket(pak,newkeys);
    Dotnet_AddInt32ToPacket(pak,oldkeys);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerLeaveCheckpoint(playerid)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerLeaveCheckpoint");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerLeaveRaceCheckpoint(playerid)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerLeaveRaceCheckpoint");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_SendPacket(pak);
    return 1;
}


public OnPlayerObjectMoved(playerid, objectid)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerObjectMoved");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddInt32ToPacket(pak,objectid);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerPickUpPickup(playerid, pickupid)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerPickUpPickup");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddInt32ToPacket(pak,pickupid);
    Dotnet_SendPacket(pak);
    return 1;
}

public OnPlayerRequestClass(playerid, classid)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerRequestClass");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddInt32ToPacket(pak,classid);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerRequestSpawn(playerid)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerRequestSpawn");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerSelectedMenuRow(playerid, row)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerSelectedMenuRow");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddInt32ToPacket(pak,row);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerSpawn(playerid)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerSpawn");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerStateChange(playerid, newstate, oldstate)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerStateChange");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddInt32ToPacket(pak,newstate);
    Dotnet_AddInt32ToPacket(pak,oldstate);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerStreamIn(playerid, forplayerid)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerStreamIn");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddInt32ToPacket(pak,forplayerid);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerStreamOut(playerid, forplayerid)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerStreamOut");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddInt32ToPacket(pak,forplayerid);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerText(playerid, text[])
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerText");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddCellStringToPacket(pak,text);
    Dotnet_SendPacket(pak);
    return 1;
}

public OnPlayerUpdate(playerid)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerUpdate");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnRconCommand(cmd[])
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnRconCommand");
    Dotnet_AddCellStringToPacket(pak,cmd);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnRconLoginAttempt(ip[], password[], success)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnRconLoginAttempt");
    Dotnet_AddCellStringToPacket(pak,ip);
    Dotnet_AddCellStringToPacket(pak,password);
    Dotnet_AddInt32ToPacket(pak,success);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnVehicleDamageStatusUpdate(vehicleid, playerid)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnVehicleDamageStatusUpdate");
    Dotnet_AddInt32ToPacket(pak,vehicleid);
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnVehicleDeath(vehicleid)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnVehicleDeath");
    Dotnet_AddInt32ToPacket(pak,vehicleid);
    Dotnet_SendPacket(pak);
    return 1;
}

public OnDialogResponse(playerid, dialogid, response, listitem, inputtext[])
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnDialogResponse");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddInt32ToPacket(pak,dialogid);
    Dotnet_AddInt32ToPacket(pak,response);
    Dotnet_AddInt32ToPacket(pak,listitem);
    Dotnet_AddCellStringToPacket(pak,inputtext);
    Dotnet_SendPacket(pak);
    return 1;
}









/*
public OnUnoccupiedVehicleUpdate(param0_i, param1_i, param2_i)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnUnoccupiedVehicleUpdate");
    Dotnet_AddInt32ToPacket(pak,param0_i);
    Dotnet_AddInt32ToPacket(pak,param1_i);
    Dotnet_AddInt32ToPacket(pak,param2_i);
    Dotnet_SendPacket(pak);
    return 1;
}*


public OnPlayerTakeDamage(playerid, param1_i, param2_f, param3_i)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerTakeDamage");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddInt32ToPacket(pak,param1_i);
    AddFloat32ToPacket(param2_f);
    Dotnet_AddInt32ToPacket(pak,param3_i);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerTakeDamageRU(playerid, param1_i, param2_f, param3_i)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerTakeDamageRU");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddInt32ToPacket(pak,param1_i);
    AddFloat32ToPacket(param2_f);
    Dotnet_AddInt32ToPacket(pak,param3_i);
    Dotnet_SendPacket(pak);
    return 1;
}
public OnPlayerTeamPrivmsg(playerid, param1_s)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerTeamPrivmsg");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddCellStringToPacket(pak,param1_s);
    Dotnet_SendPacket(pak);
    return 1;
}
*


public OnPlayerPrivmsg(playerid, param1_i, param2_s)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerPrivmsg");
    Dotnet_AddInt32ToPacket(pak,playerid);
    Dotnet_AddInt32ToPacket(pak,param1_i);
    Dotnet_AddCellStringToPacket(pak,param2_s);
    Dotnet_SendPacket(pak);
    return 1;
}
*


public OnPlayerGiveDamage(param0_i, param1_i, param2_f, param3_i)
{
    new pak = Dotnet_NewPacket(Packet_Callback);
    Dotnet_AddCellStringToPacket(pak,"OnPlayerGiveDamage");
    Dotnet_AddInt32ToPacket(pak,param0_i);
    Dotnet_AddInt32ToPacket(pak,param1_i);
    AddFloat32ToPacket(param2_f);
    Dotnet_AddInt32ToPacket(pak,param3_i);
    Dotnet_SendPacket(pak);
    return 1;
}*
*/



// -------- Functions ----------

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
	

 
