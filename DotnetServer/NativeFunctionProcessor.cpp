/*
 * Iain Gilbert
 * 2011
 *
*/

/*
    The contents of this file are subject to the Common Public Attribution License Version 1.0 (the �License�); you may not use this file except in compliance with the License. 
    You may obtain a copy of the License at http://www.opensource.org/licenses/cpal_1.0 
    The License is based on the Mozilla Public License Version 1.1 but Sections 14 and 15 have been added to cover use of software over a computer network and provide for limited attribution for the Original Developer. 
    In addition, Exhibit A has been modified to be consistent with Exhibit B.
    Software distributed under the License is distributed on an �AS IS� basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. 
    See the License for the specific language governing rights and limitations under the License.

    The Original Code is SampDotnetScriptAPI.
    The Initial Developer of the Original Code is Iain Gilbert. All portions of the code written by Iain Gilbert are Copyright (c) 2011. All Rights Reserved.

    Contributors: ______________________.

    The text of this license may differ slightly from the text of the notices in Exhibits A and B of the license at http://code.google.com/p/samp-dotnet-script-api/. 
    You should use the latest text at http://code.google.com/p/samp-dotnet-script-api/ for your modifications.
    You may not remove this license text from the source files.
 
    Attribution Information
        Attribution Copyright Notice: 
        Attribution Phrase (not exceeding 10 words): Samp Dotnet Script API
        Attribution URL: http://code.google.com/p/samp-dotnet-script-api/
 
    Display of Attribution Information to the end user is not required on server connection, or at any time the end user is connected to your server.
    You must treat any External Deployment by You of the Original Code or Modifications as a distribution under section 3.1 and make Source Code available under Section 3.2.
    Display of Attribution Information is not required in Larger Works which are defined in the CPAL as a work which combines Covered Code or portions thereof with code not governed by the terms of the CPAL.
 */

#include "NativeFunctionProcessor.h"
#include "Client.h"
#include "Server.h"
#include "string"





struct sNativeFunction
{
	int id;
	char* Name;
	char* Args;
	amx_function_t func;
};

sNativeFunction NativeFunctions[] = 
{
	{0,"NULL",								"",		NULL },
	{1,"SendClientMessage",					"iis" , NULL },
	{2,"SendClientMessageToAll",				"is" , NULL },
	{3,"SendDeathMessage",					"iii" , NULL },
	{4,"GameTextForAll",						"sii" , NULL },
	{5,"GameTextForPlayer",					"isii" , NULL },
	{6,"GetTickCount",						"" , NULL },
	{7,"GetMaxPlayers",						"" , NULL },
	{8,"SetGameModeText",						"s" , NULL },
	{9,"SetTeamCount",						"i" , NULL },
	{10,"AddPlayerClass",						"iffffiiiiii" , NULL },
	{11,"AddPlayerClassEx",					"iiffffiiiiii" , NULL },
	{12,"AddStaticVehicle",					"iffffii" , NULL },
	{13,"AddStaticVehicleEx",					"iffffiii" , NULL },
	{14,"AddStaticPickup",						"iifff" , NULL },
	{15,"ShowNameTags",						"i" , NULL },
	{16,"ShowPlayerMarkers",					"i" , NULL },
	{17,"GameModeExit",						"" , NULL },
	{18,"SetWorldTime",						"i" , NULL },
	{19,"GetWeaponName",						"ivi" , NULL },
	{20,"EnableTirePopping",					"i" , NULL },
	{21,"AllowInteriorWeapons",				"i" , NULL },
	{22,"SetWeather",							"i" , NULL },
	{23,"SetGravity",							"f" , NULL },
	{24,"AllowAdminTeleport",					"i" , NULL },
	{25,"SetDeathDropAmount",					"i" , NULL },
	{26,"CreateExplosion",						"fffif" , NULL },
	//{27,"SetDisabledWeapons",					"" , NULL },
	{28,"EnableZoneNames",						"i" , NULL },
	{29,"IsPlayerAdmin",						"i" , NULL },
	{30,"Kick",								"i" , NULL },
	//{31,"BanEx",								"is" , NULL },
	{32,"Ban",									"i" , NULL },
	{33,"SendRconCommand",						"s" , NULL },


	{34,"SetSpawnInfo",						"iiiffffiiiiii" , NULL },
	{35,"SpawnPlayer",							"i" , NULL },
	{36,"SetPlayerPos",						"ifff" , NULL },
	{37,"GetPlayerPos",						"ivvv" , NULL },
	{38,"SetPlayerFacingAngle",				"if" , NULL },
	{39,"GetPlayerFacingAngle",				"iv" , NULL },
	{40,"SetPlayerInterior",					"ii" , NULL },
	{41,"GetPlayerInterior",					"i" , NULL },
	{42,"SetPlayerHealth",						"if" , NULL },
	{43,"GetPlayerHealth",						"iv" , NULL },
	{44,"SetPlayerArmour",						"if" , NULL },
	{45,"GetPlayerArmour",						"iv" , NULL },
	{46,"SetPlayerAmmo",						"iii" , NULL },
	{47,"GetPlayerAmmo",						"i" , NULL },
	{48,"SetPlayerTeam",						"ii" , NULL },
	{49,"GetPlayerTeam",						"i" , NULL },
	{50,"SetPlayerScore",						"ii" , NULL },
	{51,"GetPlayerScore",						"i" , NULL },
	{52,"SetPlayerColor",						"ii" , NULL },
	{53,"GetPlayerColor",						"i" , NULL },
	{54,"SetPlayerSkin",						"ii" , NULL },
	{55,"GivePlayerWeapon",					"iii" , NULL },
	{56,"ResetPlayerWeapons",					"i" , NULL },
	{57,"GetPlayerWeaponData",					"iiiviv{" , NULL },
	{58,"GivePlayerMoney",						"ii" , NULL },
	{59,"ResetPlayerMoney",					"i" , NULL },
	{60,"SetPlayerName",						"is" , NULL },
	{61,"GetPlayerMoney",						"i" , NULL },
	{62,"GetPlayerState",						"i" , NULL },
	{63,"GetPlayerIp",							"ipi" , NULL },
	{64,"GetPlayerPing",						"i" , NULL },
	{65,"GetPlayerWeapon",						"i" , NULL },
	{66,"GetPlayerKeys",						"ivvv" , NULL },
	{67,"GetPlayerName",						"ipi" , NULL },
	{68,"PutPlayerInVehicle",					"iii" , NULL },
	{69,"GetPlayerVehicleID",					"i" , NULL },
	{70,"RemovePlayerFromVehicle",				"i" , NULL },
	{71,"TogglePlayerControllable",			"ii" , NULL },
	{72,"PlayerPlaySound",						"iifff" , NULL },
	{73,"SetPlayerCheckpoint",					"iffff" , NULL },
	{74,"DisablePlayerCheckpoint",				"i" , NULL },
	{75,"SetPlayerRaceCheckpoint",				"iifffffff" , NULL },
	{76,"DisablePlayerRaceCheckpoint",			"i" , NULL },
	{77,"SetPlayerWorldBounds",				"iffff" , NULL },
	{78,"SetPlayerMarkerForPlayer",			"iii" , NULL },
	{79,"ShowPlayerNameTagForPlayer",			"iii" , NULL },
	{80,"SetPlayerMapIcon",					"iifffii" , NULL },
	{81,"RemovePlayerMapIcon",					"ii" , NULL },
	{82,"SetPlayerCameraPos",					"ifff" , NULL },
	{83,"SetPlayerCameraLookAt",				"ifff" , NULL },
	{84,"SetCameraBehindPlayer",				"i" , NULL },
	{85,"AllowPlayerTeleport",					"ii" , NULL },
	{86,"IsPlayerConnected",					"i" , NULL },
	{87,"IsPlayerInVehicle",					"ii" , NULL },
	{88,"IsPlayerInAnyVehicle",				"i" , NULL },
	{89,"IsPlayerInCheckpoint",				"i" , NULL },
	{90,"IsPlayerInRaceCheckpoint",			"i" , NULL },
	{91,"SetPlayerTime",						"iii" , NULL },
	{92,"TogglePlayerClock",					"ii" , NULL },
	{93,"SetPlayerWeather",					"ii" , NULL },
	{94,"GetPlayerTime",						"ivv" , NULL },
	{95,"SetPlayerVirtualWorld",				"ii" , NULL },
	{96,"GetPlayerVirtualWorld",				"i" , NULL },


	{97,"CreateVehicle",						"iffffiii" , NULL },
	{98,"DestroyVehicle",						"i" , NULL },
	{99,"GetVehiclePos",						"ivvv" , NULL },
	{100,"SetVehiclePos",						"ifff" , NULL },
	{101,"GetVehicleZAngle",					"iv" , NULL },
	{102,"SetVehicleZAngle",					"if" , NULL },
	{103,"SetVehicleParamsForPlayer",			"iiii" , NULL },
	{104,"SetVehicleToRespawn",					"i" , NULL },
	{105,"LinkVehicleToInterior",				"ii" , NULL },
	{106,"AddVehicleComponent",					"ii" , NULL },
	{107,"ChangeVehicleColor",					"iii" , NULL },
	{108,"ChangeVehiclePaintjob",				"ii" , NULL },
	{109,"SetVehicleHealth",					"if" , NULL },
	{110,"GetVehicleHealth",					"iv" , NULL },
	{111,"AttachTrailerToVehicle",				"ii" , NULL },
	{112,"DetachTrailerFromVehicle",			"i" , NULL },
	{113,"IsTrailerAttachedToVehicle",			"i" , NULL },
	{114,"SetVehicleNumberPlate",				"is" , NULL },
	{115,"SetVehicleVirtualWorld",				"ii" , NULL },
	{116,"GetVehicleVirtualWorld",				"i" , NULL },
	{117,"ApplyAnimation",						"issfiiiii" , NULL },


	{118,"CreateObject",						"ifffffff" , NULL },
	{119,"SetObjectPos",						"ifff" , NULL },
	{120,"GetObjectPos",						"ivvv" , NULL },
	{121,"SetObjectRot",						"ifff" , NULL },
	{122,"GetObjectRot",						"ivvv" , NULL },
	{123,"IsValidObject",						"i" , NULL },
	{124,"DestroyObject",						"i" , NULL },
	{125,"MoveObject",							"iffff" , NULL },
	{126,"StopObject",							"i" , NULL },
	{127,"CreatePlayerObject",					"iiffffff" , NULL },
	{128,"SetPlayerObjectPos",					"iifff" , NULL },
	{129,"GetPlayerObjectPos",					"iivvv" , NULL },
	{130,"GetPlayerObjectRot",					"iivvv" , NULL },
	{131,"SetPlayerObjectRot",					"iifff" , NULL },
	{132,"IsValidPlayerObject",					"ii" , NULL },
	{133,"DestroyPlayerObject",					"ii" , NULL },
	{134,"MovePlayerObject",					"iiffff" , NULL },
	{135,"StopPlayerObject",					"ii" , NULL },

	{136,"CreateMenu",							"siffff" , NULL },
	{137,"DestroyMenu",							"i" , NULL },
	{138,"AddMenuItem",							"iis" , NULL },
	{139,"SetMenuColumnHeader",					"iis" , NULL },
	{140,"ShowMenuForPlayer",					"ii" , NULL },
	{141,"HideMenuForPlayer",					"ii" , NULL },
	{142,"IsValidMenu",							"i" , NULL },
	{143,"DisableMenu",							"i" , NULL },
	{144,"DisableMenuRow",						"ii" , NULL },

	{145,"TextDrawCreate",						"ffs" , NULL },
	{146,"TextDrawDestroy",						"i" , NULL },
	{147,"TextDrawLetterSize",					"iff" , NULL },
	{148,"TextDrawTextSize",					"iff" , NULL },
	{149,"TextDrawAlignment",					"ii" , NULL },
	{150,"TextDrawColor",						"ii" , NULL },
	{151,"TextDrawUseBox",						"ii" , NULL },
	{152,"TextDrawBoxColor",					"ii" , NULL },
	{153,"TextDrawSetShadow",					"ii" , NULL },
	{154,"TextDrawSetOutline",					"ii" , NULL },
	{155,"TextDrawBackgroundColor",				"ii" , NULL },
	{156,"TextDrawFont",						"ii" , NULL },
	{157,"TextDrawSetProportional",				"ii" , NULL },
	{158,"TextDrawShowForPlayer",				"ii" , NULL },
	{159,"TextDrawHideForPlayer",				"ii" , NULL },
	{160,"TextDrawShowForAll",					"i" , NULL },
	{161,"TextDrawHideForAll",					"i" , NULL },
	{162,"ShowPlayerDialog",					"iiissss" , NULL },
	{163,"GetVehicleVelocity",					"ivvv" , NULL },
	{164,"SetVehicleVelocity",					"ifff" , NULL },
	{165,"AttachObjectToPlayer",				"iiffffff", NULL }
};
int NativeFunctions_Count()
{
	return (sizeof(NativeFunctions) / (sizeof(int) + sizeof(const char *) + sizeof(const char *) + sizeof(amx_function_t)));
}
/*
sNativeFunction* GetNativeByID(int id)
{
	for (int i=0;i<NativeFunctions_Count();i++)
	{
		if (NativeFunctions[i].id == id) return &NativeFunctions[i];
	}
	return &NativeFunctions[0];
}*/
sNativeFunction* GetNativeByName(char* name)
{
	for (int i=0;i<NativeFunctions_Count();i++)
	{
		if (strcmp(NativeFunctions[i].Name,name) == 0) return &NativeFunctions[i];
	}
	return NULL;
}





void NativeFunctionProcessor::Init(AMX *pAMX)
{
	if (AMXInit) return; // already init
	if(!pAMX)
	{
		Log::Warning("pAMX null");
		return;
	}
	for (int i=0;i<MAX_FUNCTIONREQUESTQUE;i++) {FunctionRequestQue[i] = NULL;}

	__gpAMX = pAMX;

	for (int i=1;i<NativeFunctions_Count();i++)
	{
		amx_function_t __sampAmxFunc = FindFunction((char*)NativeFunctions[i].Name);
		NativeFunctions[i].func = __sampAmxFunc;
		if (__sampAmxFunc == NULL)
		{
			//logprintf("AMX Function '%s' not found!",NativeFunctions[i].Name);
			return;
		}
	}
	
	AMXInit = true;
	
}

		
amx_function_t NativeFunctionProcessor::FindFunction(char *name)
{
	//if (!AMXInit) return NULL;
	if(!__gpAMX)
	{
		Log::Warning("AMX not initialized!");
		return NULL;
	}

	int index=0;

	amx_FindNative(__gpAMX, name,&index);//(const char*)name, &index);
	
	if(index == 2147483647)//The command cannot be found.
	{
		//Log::Warning("Native function not found!");
		return NULL;
	}

	// Proceed with locating the memory address for this function;
	AMX_HEADER *hdr = (AMX_HEADER *) __gpAMX->base;
	unsigned int call_addr = (unsigned int)((AMX_FUNCSTUB *)
		((char *)(hdr) + (hdr)->natives + hdr->defsize * index))->address;

	if(call_addr == 0)//Could not locate the function's address.
	{
		Log::Warning("Native function address not found!");
		return NULL;
	}

	return (amx_function_t)call_addr;
}

bool NativeFunctionProcessor::AddFunctionRequestToQue(FunctionRequest* func)
{
	if (FunctionRequestQue[MAX_FUNCTIONREQUESTQUE-1] != NULL) 
	{
		Log::Warning("Function request que is full! Waiting for que to empty.");
		int i=0;
		while (FunctionRequestQue[MAX_FUNCTIONREQUESTQUE-1] != NULL && i < 100)
		{
			SLEEP(1);
			i++;
		}
		logprintf("DotnetServer Debug: Waited %d ms for que to empty.",i);
	}

	while (functionrequestquelock) {SLEEP(1);}
	functionrequestquelock = true;
	for (int i=0;i<MAX_FUNCTIONREQUESTQUE;i++)
	{
		if (FunctionRequestQue[i] == NULL)
		{
			FunctionRequestQue[i] = func;
			functionrequestquelock = false;
			return true;
		}
	}
	functionrequestquelock = false;
	Log::Warning("Function request que is still full! Dropping function request.");
	return false;
}


int NativeFunctionProcessor::ProcessFunctionRequestQue()
{
	if (FunctionRequestQue[0] == NULL) return 0; // que is empty
	int count = 0;
	while (functionrequestquelock) {SLEEP(1);}
	functionrequestquelock = true;
	for (int i=0;i<MAX_FUNCTIONREQUESTQUE;i++)
	{
		if (FunctionRequestQue[i] == NULL) continue;
		count++;
		FunctionRequest* functionresponse = ProcessFunctionRequest(FunctionRequestQue[i]);
		if (functionresponse == NULL) {FunctionRequestQue[i] = NULL; continue;}
		Server::Instance->PakSender->SendFunctionResponse(functionresponse->_Client,functionresponse);
		delete(functionresponse);
		FunctionRequestQue[i] = NULL;
	}
	functionrequestquelock = false;
	return count;
}

FunctionRequest* NativeFunctionProcessor::ProcessFunctionRequest(FunctionRequest* request)
{
	if (!AMXInit) return NULL;
	if (!request) return NULL;
	request->data->pos = 0;

	amx_function_t amxFunc = NULL;
	sNativeFunction* sfunc = GetNativeByName(request->name);
	if (sfunc != NULL) 
	{
		amxFunc = sfunc->func;
		memcpy(request->params,sfunc->Args,strlen(sfunc->Args)+1);
	}
	if (amxFunc == NULL) 
	{
		logprintf("Searching for AMX function: %s.",request->name);
		amxFunc = FindFunction(request->name);
		//logprintf("Debug: params: %s, %d.",request->params,strlen(request->params));
	}
	
	if (amxFunc == NULL)
	{
		logprintf("AMX Function '%s' not found!",request->name);
		return NULL;
	}



	cell* args = (cell*)calloc(strlen(request->params)+1,4);
	cell *phys_addr[10]; // 6
	args[0] = 4 * strlen(request->params);
	int vars = 0;

	for (unsigned int i=0;i<strlen(request->params);i++)
	{
		if (request->params[i] == 'i') 
		{
			int q = request->data->ReadInt32();
			args[i+1] = q;
		} 
		else if (request->params[i] == 'f') 
		{
			float f = request->data->ReadFloat32();
			args[i+1] = amx_ftoc(f);
		}
		else if (request->params[i] == 's') 
		{
			unsigned short slen = request->data->ReadUShort();
			request->data->pos -= 2;
			char* str = (char*)calloc(slen,1);
			str = request->data->ReadString(str,slen);
			size_t len = strlen(str);
			amx_Allot(__gpAMX, (int)(len + 1), args + i+1,&phys_addr[vars++]);
			amx_SetString(phys_addr[vars-1], str, 0, 0, len + 1);
			free(str);
		}
		else 
		{
			if (request->params[i] == 'v') amx_Allot(__gpAMX, 1, args + i+1, &phys_addr[vars++]); 
			if (request->params[i] == 'p') amx_Allot(__gpAMX, 1, args + i+1, &phys_addr[vars++]); 
		}
	}
	

	request->response = amxFunc(__gpAMX, args);
	
	request->data->pos = 0;
	request->data->Length = 0;
	vars = 0;
	for (unsigned int i=0;i<strlen(request->params);i++)
	{
		if (request->params[i] == 'v') request->data->AddInt32(*phys_addr[vars]);

		if (request->params[i] == 'p')
		{
			char* str = (char*)calloc(32,1);
			amx_GetString(str, phys_addr[vars], 0, 32); // todo: string length greater than 32
			request->data->AddString(str);
			free(str);
		}

		if ((request->params[i] == 's') || (request->params[i] == 'v') || (request->params[i] == 'p')) 
		{ 
			vars++; 
			amx_Release(__gpAMX, args[i+1]);
		}
	}
	
	free(args);
	//free(phys_addr);
	request->data->pos = 0;
        
	return request;
}
