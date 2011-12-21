/*
 * Iain Gilbert
 * 2011
 *
*/

/*
    The contents of this file are subject to the Common Public Attribution License Version 1.0 (the “License”); you may not use this file except in compliance with the License. 
    You may obtain a copy of the License at http://www.opensource.org/licenses/cpal_1.0 
    The License is based on the Mozilla Public License Version 1.1 but Sections 14 and 15 have been added to cover use of software over a computer network and provide for limited attribution for the Original Developer. 
    In addition, Exhibit A has been modified to be consistent with Exhibit B.
    Software distributed under the License is distributed on an “AS IS” basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. 
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
 
    Display of Attribution Information to the end user is not required on server login, or at any time the end user is connected to your server.
    You must treat any External Deployment by You of the Original Code or Modifications as a distribution under section 3.1 and make Source Code available under Section 3.2.
    Display of Attribution Information is not required in Larger Works which are defined in the CPAL as a work which combines Covered Code or portions thereof with code not governed by the terms of the CPAL.
 */

#include "main.h"

extern void	*pAMXFunctions;

#include "Packet.h"
#include "Server.h"
Server* MainServer;



PLUGIN_EXPORT unsigned int PLUGIN_CALL Supports() 
{
	return SUPPORTS_VERSION | SUPPORTS_AMX_NATIVES | SUPPORTS_PROCESS_TICK;
}

PLUGIN_EXPORT bool PLUGIN_CALL Load( void **ppData ) 
{
	pAMXFunctions = ppData[PLUGIN_DATA_AMX_EXPORTS];
	logprintf = (logprintf_t)ppData[PLUGIN_DATA_LOGPRINTF];

	MainServer = new Server();
	MainServer->Start();

	logprintf( "%s %s plugin loaded.\n",PLUGIN_NAME,PLUGIN_VERSION);
	return 1;
}

PLUGIN_EXPORT void PLUGIN_CALL Unload()
{
	//g_Invoke->p_Amx.clear();
	logprintf("%s plugin unloaded.\n",PLUGIN_NAME);
}

PLUGIN_EXPORT void PLUGIN_CALL ProcessTick()
{
	MainServer->SampProcessTick();
}


#if defined __cplusplus
	extern "C"
#endif


cell AMX_NATIVE_CALL Dotnet_AddInt32ToPacket(AMX * amx, cell * params)
{
	//logprintf("Dotnet_AddInt32ToPacket");
	cell *pPack, *pInt;
	amx_GetAddr(amx,params[1],&pPack);
	amx_GetAddr(amx,params[2],&pInt);
	Packet* pack = (Packet*)*pPack;
	if (pack->IsValid != 31415) return 0; // bad
	pack->AddInt32(*pInt);
	return (cell)pack;
}

cell AMX_NATIVE_CALL Dotnet_AddFloat32ToPacket(AMX * amx, cell * params)
{
	//logprintf("Dotnet_AddFloat32ToPacket");
	cell *pPack, *pFloat;
	amx_GetAddr(amx,params[1],&pPack);
	amx_GetAddr(amx,params[2],&pFloat);
	Packet* pack = (Packet*)*pPack;
	if (pack->IsValid != 31415) return 0; // bad
	pack->AddFloat32(*(float*)pFloat);
	return (cell)pack;
}

cell AMX_NATIVE_CALL Dotnet_AddCellStringToPacket(AMX * amx, cell * params)
{
	//logprintf("Dotnet_AddCellStringToPacket");
	cell *pPack, *pStr;
	amx_GetAddr(amx,params[1],&pPack);
	amx_GetAddr(amx,params[2],&pStr);
	Packet* pack = (Packet*)*pPack;
	if (pack->IsValid != 31415) return 0; // bad
	pack->AddCellString((char*)pStr);
	return (cell)pack;
}


Packet* tempp = NULL;
cell AMX_NATIVE_CALL Dotnet_NewPacket(AMX * amx, cell * params)
{
	//logprintf("Dotnet_NewPacket");
	cell *pOpcode = NULL;
	amx_GetAddr(amx,params[1],&pOpcode);
	int* opcode = (int*)pOpcode;
	Packet* pack = new Packet();
	pack->Opcode = *opcode;
	pack->IsValid = 31415;
	tempp = pack;
	//logprintf("newpack ptr: %d, opcode: %d",pack, *opcode);

	return (uint32_t)pack; // send the memory address of our new packet
}


cell AMX_NATIVE_CALL Dotnet_SendPacket(AMX * amx, cell * params)
{
	//logprintf("Dotnet_SendPacket");
	cell *pPack;
	amx_GetAddr(amx,params[1],&pPack);
	Packet* pack = (Packet*)*pPack;
	if (pack == NULL) return 0;
	if (pack->IsValid != 31415) return 0; // bad hack to ensure pawn script writers dont try to send bad packet
	MainServer->PakSender->SendPacketToAll(pack);
	pack->IsValid = 0; // hacks, yay
	delete(pack);
	return 1;
}


const AMX_NATIVE_INFO DotnetServerNatives[] = 
{
	{"Dotnet_NewPacket", Dotnet_NewPacket},
	{"Dotnet_SendPacket", Dotnet_SendPacket},
	{"Dotnet_AddInt32ToPacket", Dotnet_AddInt32ToPacket},
	{"Dotnet_AddFloat32ToPacket", Dotnet_AddFloat32ToPacket},
	{"Dotnet_AddCellStringToPacket", Dotnet_AddCellStringToPacket},
	//{"Dotnet_AddPackedStringToPacket", Dotnet_AddStringToPacket},
	{NULL,NULL}
};


PLUGIN_EXPORT int PLUGIN_CALL AmxLoad( AMX *amx )
{
	
	if (MainServer){if (MainServer->FuncProcessor) MainServer->FuncProcessor->Init(amx);}

	return amx_Register(amx,DotnetServerNatives,-1);
}

PLUGIN_EXPORT int PLUGIN_CALL AmxUnload( AMX *amx ) 
{
	/*for (std::list<AMX *>::iterator i = g_Invoke->amx_list.begin(); i != g_Invoke->amx_list.end(); ++i)
	{
		if (* i == amx)
		{
			g_Invoke->amx_list.erase(i);
			break;
		}
	}*/
	return AMX_ERR_NONE;
}