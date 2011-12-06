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
        Attribution Copyright Notice: Copyright 2011, Iain Gilbert
        Attribution Phrase (not exceeding 10 words): Samp Dotnet Script API
        Attribution URL: http://code.google.com/p/samp-dotnet-script-api/
 
    Display of Attribution Information to the end user is not required on server login, or at any time the end user is connected to your server.
    You must treat any External Deployment by You of the Original Code or Modifications as a distribution under section 3.1 and make Source Code available under Section 3.2.
    Display of Attribution Information is not required in Larger Works which are defined in the CPAL as a work which combines Covered Code or portions thereof with code not governed by the terms of the CPAL.
 */

#include "PacketBuilder.h"
#include "Server.h"

PacketBuilder::PacketBuilder()
{
	//_Server = server;
}

void PacketBuilder::SendPingRequest(Client* client)
{
	//Log::Debug("SendPingRequest");
	Packet* sp = new Packet();
	sp->Opcode = Packet::Ping;
	Server::Instance->SendPacket(client,sp);
	delete(sp);
}

void PacketBuilder::SendAuthReply(Client* client,bool success)
{
	//Log::Debug("SendAuthReply");
	Packet* sp = new Packet();
	sp->Opcode = Packet::Auth;
	sp->AddByte((char)success);
	Server::Instance->SendPacket(client,sp);
	delete(sp);
}
void PacketBuilder::SendCallbackToAll(Packet* sp)
//void PacketBuilder::SendCallbackToAll(char callbackid,char* paramtypes, char* data,char datalength)
{
	sp->pos = 0;
	char callbackid = sp->ReadByte();
	for (int i=0;i<Server::MAX_CLIENTS;i++)
	{
		if (Server::Instance->Clients[i] != NULL) 
		{
			Client* client = Server::Instance->Clients[i];

			if (client->CallbackFequency == 0) continue;
			if ((callbackid == 36) || (callbackid == 39))
			{ // player update, unoccupied vehicle update
				if (client->CallbackFequency == 1) continue;
			}
			SendCallback(client,sp);
		}
	}
}
void PacketBuilder::SendCallback(Client* client, Packet* sp)
//void PacketBuilder::SendCallback(Client* client, char callbackid,char* paramtypes, char* data,char datalength)
{
	Server::Instance->SendPacket(client,sp);
}

void PacketBuilder::SendFunctionResponse(Client* client,FunctionRequest* function)
{
	if (client == NULL) return;
	Packet* sendpak = new Packet();
	sendpak->Opcode = Packet::FunctionReply;
	sendpak->AddString(function->guid);
	sendpak->AddString(function->name);
	sendpak->AddInt32(function->response);
	sendpak->AddString(function->params);
	
	sendpak->AddData(&function->data->Data[0],function->data->Length);

	      
        /*char* qq = (char*)malloc(127);
        sprintf(qq,"requestpacketsize: %d",sendpak->Length);
        Log::Debug(qq);
        free(qq);*/
	//Log::Debug("SendFunctionResponse");
	Server::Instance->SendPacket(client,sendpak);
	delete(sendpak);
}

