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

const char MAX_STRING = 32; // todo: increase this
char* cpystr = (char*)calloc(MAX_STRING,1);
char* ReadCellStringFromPacket(char* packet,int* pos)
{ // todo: unicode (2 byte per character) strings
	char count = 0;
	for (int i=0;i<=MAX_STRING*4;i+=4)
	{
		count++;
		//logprintf("ReadStringFromPacket pos=%d, int=%d",*pos+i,packet[(*pos)+i]);
		cpystr[(int)(i/4)] = packet[*pos+i];
		if (packet[*pos+i] == 0) break;
	}
	for (int i=count;i<MAX_STRING;i++)
	{
		cpystr[i] = 0;
	}
	*pos += MAX_STRING*4;


	return cpystr;
}


float ReadFloat32FromPacket(char* packet,int* pos)
{
	float ret = (float)(packet[*pos] + (packet[*pos+1] << 8) + (packet[*pos+2] << 16) + (packet[*pos+3] << 24)); // todo: check this
	*pos += 4;
	return ret;
}

int ReadInt32FromPacket(char* packet,int* pos)
{
	int ret = (packet[*pos] + (packet[*pos+1] << 8) + (packet[*pos+2] << 16) + (packet[*pos+3] << 24));
	*pos += 4;
	return ret;
}

char ReadByteFromPacket(char* packet,int* pos)
{
	char ret =  (char)ReadInt32FromPacket(packet,pos);
	return ret;
}

cell AMX_NATIVE_CALL DotnetServer_ReceiveCallback(AMX * amx, cell * params)
{
	cell *pString;
	amx_GetAddr(amx,params[1],&pString);
	char* packet = (char*) pString;
	int packetpos = 0;
	int size =  ReadInt32FromPacket(packet,&packetpos);
	char callbackid = ReadByteFromPacket(packet,&packetpos); // the callback opcode
	char* paramtypes = (char*)calloc(10,1); 
	memcpy(paramtypes,ReadCellStringFromPacket(packet,&packetpos),10); // the param types (eg. "iisf" for int,int,string,float)

	Packet* pak = new Packet();
	pak->Opcode = Packet::Callback;
	pak->AddByte(callbackid);
	pak->AddString(paramtypes);
	for (int i=0;i<10;i++)
	{
		if (paramtypes[i] == 0) break;
		if (paramtypes[i] == 'i') {pak->AddInt32(ReadInt32FromPacket(packet,&packetpos));}
		if (paramtypes[i] == 'f') {pak->AddFloat32(ReadFloat32FromPacket(packet,&packetpos));}
		if (paramtypes[i] == 's') {pak->AddString(ReadCellStringFromPacket(packet,&packetpos));}
	}

	if (callbackid == 0)
	{
		logprintf("DotnetHook Warning! Invalid Callback received.");
	}
	else
	{
		MainServer->PakSender->SendCallbackToAll(pak);

	}

	free(paramtypes);
	delete(pak);

	return 1;
}

const AMX_NATIVE_INFO DotnetServerNatives[] = 
{
	{"DotnetServer_ReceiveCallback", DotnetServer_ReceiveCallback},
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