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
	cell *pPack;
	amx_GetAddr(amx,params[1],&pPack);
	int i = params[2];
	Packet* pack = (Packet*)*pPack;
	if (pack == NULL) return NULL;
	pack->AddInt32(i);
	return (cell)pack;
}

cell AMX_NATIVE_CALL Dotnet_AddFloat32ToPacket(AMX * amx, cell * params)
{
	//logprintf("Dotnet_AddFloat32ToPacket");
	cell *pPack;
	amx_GetAddr(amx,params[1],&pPack);
	float f = amx_ctof(params[2]);
	Packet* pack = (Packet*)*pPack;
	if (pack == NULL) return NULL;
	pack->AddFloat32(f);
	return (cell)pack;
}

cell AMX_NATIVE_CALL Dotnet_AddCellStringToPacket(AMX * amx, cell * params)
{
	//logprintf("Dotnet_AddCellStringToPacket");
	cell *pPack, *pStr;
	amx_GetAddr(amx,params[1],&pPack);
	amx_GetAddr(amx,params[2],&pStr);
	Packet* pack = (Packet*)*pPack;
	if (pack == NULL) return NULL;
	pack->AddCellString((char*)pStr);
	return (cell)pack;
}


Packet* tempp = NULL;
cell AMX_NATIVE_CALL Dotnet_NewPacket(AMX * amx, cell * params)
{
	int opcode = params[1];
	Packet* pack = new Packet();
	pack->Opcode = opcode;
	tempp = pack;
	//logprintf("newpack ptr: %d, opcode: %d",pack, opcode);

	return (cell)pack; // send the memory address of our new packet
}


cell AMX_NATIVE_CALL Dotnet_SendPacket(AMX * amx, cell * params)
{
	//logprintf("Dotnet_SendPacket");
	cell *pPack;
	amx_GetAddr(amx,params[1],&pPack);
	Packet* pack = (Packet*)*pPack;
	if (pack == NULL) return 0;
	MainServer->PakSender->SendPacketToAll(pack);
	delete(pack);
	*pPack = 0;
	return 1;
}

cell AMX_NATIVE_CALL Dotnet_DeletePacket(AMX * amx, cell * params)
{
	cell *pPack;
	amx_GetAddr(amx,params[1],&pPack);
	Packet* pack = (Packet*)*pPack;
	if (pack == NULL) return 0;
	delete(pack);
	*pPack = 0;
	return 1;
}

cell AMX_NATIVE_CALL Dotnet_SetPacketPosition(AMX * amx, cell * params)
{
	cell *pPack, *pPos;
	amx_GetAddr(amx,params[1],&pPack);
	amx_GetAddr(amx,params[2],&pPos);
	Packet* pack = (Packet*)*pPack;
	if (pack == NULL) return 0;
	pack->pos = (unsigned short)*pPos;
	return 1;
}

cell AMX_NATIVE_CALL Dotnet_GetPacketPosition(AMX * amx, cell * params)
{
	cell *pPack;
	amx_GetAddr(amx,params[1],&pPack);
	Packet* pack = (Packet*)*pPack;
	if (pack == NULL) return 0; 
	return pack->pos;
}

cell AMX_NATIVE_CALL Dotnet_GetPacketLength(AMX * amx, cell * params)
{
	cell *pPack;
	amx_GetAddr(amx,params[1],&pPack);
	Packet* pack = (Packet*)*pPack;
	if (pack == NULL) return 0;
	return pack->Length;
}

cell AMX_NATIVE_CALL Dotnet_ReadInt32FromPacket(AMX * amx, cell * params)
{
	cell *pPack;//, *pInt;
	amx_GetAddr(amx,params[1],&pPack);
	//amx_GetAddr(amx,params[2],&pInt);
	Packet* pack = (Packet*)*pPack;
	if (pack == NULL) return 0;
	//*pInt = pack->ReadInt32();
	//return *pInt;
	return pack->ReadInt32();
}

cell AMX_NATIVE_CALL Dotnet_ReadFloat32FromPacket(AMX * amx, cell * params)
{
	cell *pPack;//, *pFloat;
	amx_GetAddr(amx,params[1],&pPack);
	//amx_GetAddr(amx,params[2],&pFloat);
	Packet* pack = (Packet*)*pPack;
	if (pack == NULL) return 0;
	float f = pack->ReadFloat32();
	//*pFloat = amx_ftoc(f);
	//return *pFloat;
	return amx_ftoc(f);
}

cell AMX_NATIVE_CALL Dotnet_ReadStringFromPacket(AMX * amx, cell * params)
{
	cell *pPack, *pStr;//, *pLen;
	amx_GetAddr(amx,params[1],&pPack);
	amx_GetAddr(amx,params[2],&pStr);
	//amx_GetAddr(amx,params[3],&pLen);
	Packet* pack = (Packet*)*pPack;
	if (pack == NULL) return 0;
	
	int strlen = pack->ReadUShort();
	pack->pos -= 2;
	amx_SetString(pStr, (char*)pack->pos, 0, 0, strlen);
	pack->ReadString(NULL,0);
	return strlen;
}








const AMX_NATIVE_INFO DotnetServerNatives[] = 
{
	{"Dotnet_NewPacket", Dotnet_NewPacket},
	{"Dotnet_SendPacket", Dotnet_SendPacket},
	{"Dotnet_DeletePacket", Dotnet_DeletePacket},
	{"Dotnet_SetPacketPosition", Dotnet_SetPacketPosition},
	{"Dotnet_GetPacketPosition", Dotnet_GetPacketPosition},
	{"Dotnet_GetPacketLength", Dotnet_GetPacketLength},

	{"Dotnet_AddInt32ToPacket", Dotnet_AddInt32ToPacket},
	{"Dotnet_AddFloat32ToPacket", Dotnet_AddFloat32ToPacket},
	{"Dotnet_AddCellStringToPacket", Dotnet_AddCellStringToPacket},
	//{"Dotnet_AddStringToPacket", Dotnet_AddStringToPacket},

	{"Dotnet_ReadInt32FromPacket", Dotnet_ReadInt32FromPacket},
	{"Dotnet_ReadFloat32FromPacket", Dotnet_ReadFloat32FromPacket},
	{"Dotnet_ReadStringFromPacket", Dotnet_ReadStringFromPacket},


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

