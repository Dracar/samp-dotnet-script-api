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

#include "PacketProcessor.h"
#include "Server.h"
#include "NativeFunctionProcessor.h"

PacketProcessor::PacketProcessor()
{
	//AuthString = "ChangeMe";
}
void PacketProcessor::ProcessPacket(Client* client,Packet* pak)
{
	if (!client->IsConnected) return;
	//Log::Debug("Packet received");
	if (pak->Opcode == Packet::Auth)
	{ // authentication request
		//Log::Debug("Auth received");
		char* auth = (char*)calloc(32,1);
		auth = pak->ReadString(auth,32);
		//int a = strcmp(auth,Server::Instance->AuthKey);
		//logprintf("Auth compare: %s / %s / %d",auth,Server::Instance->AuthKey, a);
		if (strcmp(auth,Server::Instance->AuthKey) == 0)
		{
			client->IsAuthenticated = true;
			Server::Instance->PakSender->SendAuthReply(client,true);
			Log::Line("Client Authentication Successful");
		}
		else
		{
			client->IsAuthenticated = false;
			Server::Instance->PakSender->SendAuthReply(client,false);
			Log::Warning("Client Authentication Failed");
		}
		free(auth);
		return;
	}
	if (!client->IsAuthenticated) return;
	//Log::Debug("Packet received2");
	
	if (pak->Opcode == Packet::Ping)
	{
		client->Timeout = 0;
		return;
	}

	if (pak->Opcode == Packet::FunctionRequest)
	{
		//Log::Debug("FunctionRequest received");
		FunctionRequest* function = new FunctionRequest();
		function->_Client = client;
		function->guid = pak->ReadString(function->guid,FunctionRequest::STRING_SIZE);
		function->name = pak->ReadString(function->name,FunctionRequest::STRING_SIZE);
		function->response = pak->ReadInt32();
		function->params = pak->ReadString(function->params,FunctionRequest::STRING_SIZE);
		int datastart = pak->pos;
		int datalength = pak->Length - datastart;
		function->data->AddData(&(pak->Data[datastart]),datalength);

		Server::Instance->FuncProcessor->AddFunctionRequestToQue(function);

		//function = _Server->FuncProcessor->ProcessFunctionRequest(function);
		//_Server->PakSender->SendFunctionResponse(client,function);
		//delete(function);
		return;
	}

	if (pak->Opcode == Packet::Test)
	{
		Log::Debug("Test packet received");
		char* str1 = (char*)calloc(32,1);
		str1 = pak->ReadString(str1,32);
		int i = pak->ReadInt32();
		char b = pak->ReadByte();
		float f = pak->ReadFloat32();

		char* str2 = (char*)calloc(32,1);
		str2 = pak->ReadString(str2,32);


		Packet* sp = new Packet();
		sp->Opcode = Packet::Test;
		sp->AddString(str1);
		sp->AddInt32(i);
		sp->AddByte(b);
		sp->AddFloat32(f);
		sp->AddString(str2);
		Server::Instance->SendPacket(client,sp);
		free(str1);
		free(str2);
		return;
	}


}
