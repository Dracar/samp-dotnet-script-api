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
 
    Display of Attribution Information to the end user is not required on server login, or at any time the end user is connected to your server.
    You must treat any External Deployment by You of the Original Code or Modifications as a distribution under section 3.1 and make Source Code available under Section 3.2.
    Display of Attribution Information is not required in Larger Works which are defined in the CPAL as a work which combines Covered Code or portions thereof with code not governed by the terms of the CPAL.
 */

#include "Server.h"



void CreateThreadP(void* (*func)(void*),ServerThreadStartStruct* stss)//, void* param)
{ // just simple create thread function, to reduce all the ifdefs i'd otherwise need every time i want to create a new thread
	ServerThreadStartStruct* pstss = (ServerThreadStartStruct*)(calloc(1,sizeof(ServerThreadStartStruct))); // psts is freed in new thread, todo: change this?
	memcpy(pstss,stss,sizeof(ServerThreadStartStruct)); 
	Log::Debug("Creating Thread");
	#ifdef _WIN32
		HANDLE threadHandle;
		DWORD dwThreadId = 0;
		threadHandle = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)func, pstss, 0, &dwThreadId);
		if (threadHandle == 0 || threadHandle == INVALID_HANDLE_VALUE) { logprintf( "v%s plugin error! Failed to start thread.\n",PLUGIN_NAME); return;}
		CloseHandle(threadHandle);
	#else
		pthread_t threadHandle;
		pthread_attr_t attr;
		pthread_attr_init(&attr);
		pthread_attr_setdetachstate(&attr, PTHREAD_CREATE_DETACHED);
		int error = pthread_create(&threadHandle, &attr, func, pstss);
	#endif
	
}

Server* Server::Instance; // singleton

Server::Server()
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



void Server::SampProcessTick()
{
	/*FunctionRequest* fr = new FunctionRequest();
	fr->name = "TestNativeForward2";
	fr->params = "sss";
	fr->data->AddString("Moo1");
	fr->data->AddString("Moo2");
	fr->data->AddString("Moo3");
	FuncProcessor->ProcessFunctionRequest(fr);
	Log::Debug("DONE");*/
	int count = FuncProcessor->ProcessFunctionRequestQue();
	/*if (count > 0)
	{
		char* msg = (char*)calloc(255,1);
		sprintf(msg,"%d native functions processed.",count);
		Log::Debug(msg);
		free(msg);
	}*/
	
}

#include "SimpleIni.h"
void Server::LoadConfig()
{
	CSimpleIniA ini;
	ini.LoadFile("plugins/DotnetServer.ini");
	char* keystr = (char*)ini.GetValue("config", "AuthKey", "ChangeMe");
	AuthKey = (char*)malloc(strlen(keystr)+1);
	memcpy(AuthKey,keystr,strlen(keystr)+1);
	//AuthKey = (char*)ini.GetValue("config", "AuthKey", "ChangeMe");
	char* portstr = (char*)ini.GetValue("config", "Port", "7780");
	Port = atoi(portstr);
    //free(portstr);

    char* q = (char*)malloc(127);
    sprintf(q,"AuthKey: %s, Port: %d",AuthKey,Port);
    Log::Debug(q);
    free(q);
}

Client* Server::NewClient(SOCKET clientsock, sockaddr_in clientaddress)
{
	int clientid = -1;
	for (int i=0;i<MAX_CLIENTS;i++)
	{
		if (Clients[i] == NULL) {clientid = i; break;}
	}
	if (clientid == -1) return NULL;
	Clients[clientid] = new Client();
	Clients[clientid]->ID = clientid;
	Clients[clientid]->Socket = clientsock;
	Clients[clientid]->Address = clientaddress;
	Clients[clientid]->IsConnected = true;
	return Clients[clientid];
}



bool Server::RemoveClient(Client* client)
{
	if (client == NULL) return false;
	logprintf("DotnetServer: Client (%d - %s) disconnected.",client->ID,inet_ntoa(client->Address.sin_addr));
	for (int i=0;i<MAX_CLIENTS;i++) 
	{ 
		if (Clients[i] == client) 
		{
			client->IsConnected = false;
			SLEEP(10); // give other client threads time to exit :/
			shutdown(client->Socket, 2);
#if defined(_WIN32)
			closesocket(client->Socket);
#else
            close(client->Socket);
#endif
			SLEEP(10);
			delete(Clients[i]); 
			Clients[i] = NULL;
			return true;
		}
	}
	return false;
}

bool Server::Start()
{
	Log::Debug("Server Starting...");
	Online = Bind();
	if (!Online) {Log::Warning("Failed to start server."); return false; }
	ServerThreadStartStruct* stss = new ServerThreadStartStruct(this,NULL);
	CreateThreadP(&ServerWaitConnectThreadStart,stss);
    delete(stss);
	Log::Line("Server Started.");
	return true;
}


bool Server::Bind()
{
#if defined(_WIN32)
	WSADATA t_wsa; // WSADATA structure
        WORD wVers; // version number
        int iError; // error number

        wVers = MAKEWORD(2, 2); // Set the version number to 2.2
        iError = WSAStartup(wVers, &t_wsa); // Start the WSADATA
		if (iError != 0) 
		{
			Log::Warning("Server Bind Failed 1");
			WSACleanup();
			return false;
		}
#endif

        Socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
		if(Socket == INVALID_SOCKET)
		{
			Log::Warning("Server Bind Failed 2");
#if defined(_WIN32)
            WSACleanup();
#endif
            return false;
        }

#ifndef _WIN32
		/* Enable address reuse */
		int on = 1;
		int ret = setsockopt( Socket, SOL_SOCKET, SO_REUSEADDR, &on, sizeof(on) );

		signal(SIGPIPE,SIG_IGN); // ignore sigpipe errors
#endif

		memset(&Address, 0,sizeof(Address)); // zero
		Address.sin_family = AF_INET;
        Address.sin_addr.s_addr = INADDR_ANY; // Where to start server?
        Address.sin_port = htons(Port); // Port

		if(bind(Socket, (LPSOCKADDR)&Address, sizeof(Address)) == SOCKET_ERROR)
		{
			Log::Warning("Server Bind Failed 3");
			
#if defined(_WIN32)
			closesocket(Socket);
            WSACleanup();
#else
			close(Socket);
#endif
            return false;
        }

		char* msg = (char*)malloc(127);
		sprintf(msg,"Server bound to port %d.",Port);
		Log::Line(msg);
		free(msg);

		return true;

}


void Server::WaitConnect()
{
	Log::Debug("WaitConnect");
	while (Online)
	{
		int l = 0;
		while(l = listen(Socket, 20) == SOCKET_ERROR)
		{
			SLEEP(10);
		}
		if (l == -1)
		{
			Log::Warning("Server Listen Failed");
#if defined(_WIN32)
			closesocket(Socket);
            WSACleanup();
#else
			close(Socket);
#endif
            return;
        }
		SOCKET clientsock;
		sockaddr_in clientaddress;
#if defined(_WIN32)
                int clientnsize = sizeof(Address);
#else
		unsigned int clientnsize = sizeof(Address);
#endif
        clientsock = accept(Socket, (LPSOCKADDR)&clientaddress, &clientnsize);
		if (clientsock == INVALID_SOCKET) {Log::Warning("Invalid Socket for client."); continue;}

		Client* client = NewClient(clientsock,clientaddress);
		logprintf("DotnetServer: Client (%d - %s) connected.",client->ID,inet_ntoa(client->Address.sin_addr));

		ServerThreadStartStruct* stss = new ServerThreadStartStruct(this,client);
		CreateThreadP(&ServerWaitReceiveThreadStart,stss);
		CreateThreadP(&ServerWaitSendThreadStart,stss);
		CreateThreadP(&ServerPingerThreadStart,stss);
        delete(stss);
		
	}
	Log::Debug("WaitConnectEND");
}


void Server::WaitReceive(Client* client)
{
	int length;
	char* buf = (char*)calloc(Client::MAX_BUFF,1);
	while (client != NULL && Online && client->IsConnected)
	{
		length = recv(client->Socket, buf, Client::MAX_BUFF, 0);
		if (length <= 0) { Log::Debug("Receive thread error.");  RemoveClient(client); free(buf); return;}
		Packet* pak = new Packet();
		memcpy(&pak->Length,buf,2);
		pak->Opcode = buf[2];
		for (int i=0;i<pak->Length-pak->Headerlength;i++) {pak->Data[i] = buf[i+pak->Headerlength]; }
		
		pak->Length -= pak->Headerlength;
		PakProcessor->ProcessPacket(client,pak);
		delete(pak);

		memset(buf, 0, Client::MAX_BUFF);
		SLEEP(1);
	}
	free(buf);
	Log::Debug("Receive thread exiting."); 
}

void Server::WaitSend(Client* client)
{
	while (client != NULL && Online && client->IsConnected)
	{
		if (client->sendbuf[0] != 0)
		{ // if we have some data in out send buffer
			while (client->sbuflock) {SLEEP(1);} 
			client->sbuflock = true;
			int bufpos = 0;
			while (bufpos < Client::MAX_BUFF && client->sendbuf[bufpos] != 0) 
			{ // for each packet that has data

				unsigned short size = 0;
				memcpy(&size,&(client->sendbuf[bufpos]),2);
				char* buf = (char*)calloc(size,1);
				for (int i=0;i<size;i++){buf[i] = client->sendbuf[bufpos+i];} // copy the data

				int iRet = send(client->Socket, buf, size, MSG_NOSIGNAL); // send data
				if (iRet == -1) {Log::Debug("Send thread error.");  free(buf); RemoveClient(client); return;}

				bufpos += size; // next packet
				free(buf);
			}
			for (int i=0;i<Client::MAX_BUFF;i++) {client->sendbuf[i] = 0;}
			client->sbufpos = 0;
			client->sbuflock = false;
		}
		SLEEP(1); 
	}
	Log::Debug("Send thread exiting."); 
}

void Server::SendPacket(Client* client,Packet* packet)
{ // just adds the packet data to clients send buffer
	if (packet == NULL) return;
	if (packet->Opcode == 0x00) return;
	packet->Length += Packet::Headerlength; // todo: move this
	if (packet->Length <= 0) return;
	if (client->sbufpos + packet->Length >= client->MAX_BUFF) 
	{
		Log::Warning("CLient send buffer is full! Wating for buffer to empty!");
		int i=0;
		while (client->sbufpos != 0 && i < 100)
		{
			i++;
			SLEEP(1);
		}
	}
	if (client->sbufpos + packet->Length >= client->MAX_BUFF) {Log::Warning("CLient send buffer is STILL full! Dropping packet!"); return; }

	while (client->sbuflock) {SLEEP(1);} 
	client->sbuflock = true;

	memcpy(&(client->sendbuf[client->sbufpos]),&packet->Length,2);
	client->sendbuf[client->sbufpos+2] = packet->Opcode;
	for (int i=0;i<packet->Length-Packet::Headerlength;i++) {client->sendbuf[client->sbufpos+Packet::Headerlength+i] = packet->Data[i];}

	client->sbufpos += packet->Length;
	client->sbuflock = false;
	packet->Length -= Packet::Headerlength;;
}

void Server::Close()
{
	Log::Line("Server Closing");
	for (int i=0;i<MAX_CLIENTS;i++) {if (Clients[i] != NULL) RemoveClient(Clients[i]);}
	Online = false;
	SLEEP(10);
#if defined(_WIN32)
	WSACleanup();
#else
	close(Socket);
#endif
}


bool Server::ClientExists(Client* client)
{
	if (client == NULL) return false;
	for (int i=0;i<MAX_CLIENTS;i++) 
	{ 
		if (Clients[i] == client) return true;
	}
	return false;
}

void Server::Pinger(Client* client)
{
	while (Online && client->IsConnected)
	{ 
		if (client->Timeout >= Client::PING_INTERVAL)
		{
			PakSender->SendPingRequest(client);
		}
		if (client->Timeout >= Client::MAX_TIMEOUT) 
		{
            Log::Debug("Ping timeout");
			RemoveClient(client);
			return;
		}
		client->Timeout += 1;
		for (int i=0;i<1000;i+=5)
		{
			if (!Online || client->IsConnected == false) {Log::Debug("Ping thread exiting."); return;}
			SLEEP(5);
		}
	}
	Log::Debug("Ping thread exiting."); 
}

