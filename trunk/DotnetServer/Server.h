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

#pragma once


#include "main.h"

//#include <stdarg.h>
#include <stdio.h>
#include <sys/types.h>




#if defined(_WIN32)
	#ifndef _WINSOCKAPI_ 
		#define _WINSOCKAPI_ 
	#endif
	#include <winsock2.h>
	#pragma comment(lib, "ws2_32.lib")
#else
	#include <netinet/in.h>
	#include <sys/socket.h>
	#include <arpa/inet.h>
#endif


#include "Log.h"
#include "Client.h"
#include "Packet.h"
#include "PacketProcessor.h"
#include "PacketBuilder.h"
#include "NativeFunctionProcessor.h"

class Server
{
public:
		static Server* Instance; // singleton
		Server()
		{
			Instance = this;
			Online = false;
			for (int i=0;i<MAX_CLIENTS;i++) {Clients[i] = NULL;}
			PakProcessor = new PacketProcessor();
			PakSender = new PacketBuilder();
			FuncProcessor = new	NativeFunctionProcessor();
			AuthKey = "ChangeMe";
			Port = 7780;
			LoadConfig();
		}
		static const u_char MAX_CLIENTS = 10;
		Client* Clients[MAX_CLIENTS]; 
		Client* NewClient(SOCKET clientsock, sockaddr_in clientaddress);
		bool RemoveClient(Client* client);
		
		bool Start(); // starts the server
		void Close(); // stop the server

		bool Bind(); // binds to socket

		void WaitConnect(); // blocks, spawns new threads for waitreceive & waitsend when client connects
		void WaitReceive(Client* client); // blocks, 
		void WaitSend(Client* client); // blocks, sends when sendbuf has data
		void SendPacket(Client* client,Packet* packet); // adds packet to sendbuf for client, if client is null then it will send to all clients
		void Pinger(Client* client);

		void SampProcessTick(); // called by samp thread, all native calls will be made here

		void LoadConfig();

		PacketProcessor* PakProcessor;
		PacketBuilder* PakSender;
		NativeFunctionProcessor* FuncProcessor;
		char* AuthKey;
		int Port;

        
private:
		sockaddr_in Address;
		SOCKET Socket;
		bool Online;

};

struct ServerThreadStartStruct
{
	Server* server;
	Client* client;
        ServerThreadStartStruct(Server* s, Client* c)
        {
            server = s;
            client = c;
        }
};

inline void* ServerWaitConnectThreadStart(void* vstss){ ServerThreadStartStruct* stss = (ServerThreadStartStruct*)vstss; stss->server->WaitConnect(); free(stss); return NULL;}
inline void* ServerWaitReceiveThreadStart(void* vstss){ ServerThreadStartStruct* stss = (ServerThreadStartStruct*)vstss; stss->server->WaitReceive(stss->client); free(stss); return NULL;}
inline void* ServerWaitSendThreadStart(void* vstss){ ServerThreadStartStruct* stss = (ServerThreadStartStruct*)vstss; stss->server->WaitSend(stss->client); free(stss); return NULL;}
inline void* ServerPingerThreadStart(void* vstss){ ServerThreadStartStruct* stss = (ServerThreadStartStruct*)vstss; stss->server->Pinger(stss->client); free(stss); return NULL;}

;