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

using System;
using Samp.Util;

namespace Samp.Client
{
    public class PacketBuilder
    {

        public Client _Client;
        public PacketBuilder(Client client)
        {
            _Client = client;
        }

        public void SendPingReply(Server server)
        {
            Log.Debug("SendPingReply");
            Packet sp = new Packet(Packet.Opcodes.Ping);
            _Client.SendPacket(server,sp);
        }

        public void SendAuthRequest(Server server)
        {
            Log.Debug("SendAuthRequest");
	        Packet sp = new Packet(Packet.Opcodes.Auth);
	        sp.AddString(server.AuthKey);
	        _Client.SendPacket(server,sp);
        }

        public void SendNativeFunction(Server server, string guid,string name, string args, DataStream data)
        {
            Log.Debug("SendNativeFunction: "+name);
            Packet sp = new Packet(Packet.Opcodes.FunctionRequest);
            sp.AddString(guid);
            sp.AddString(name); // name
            sp.AddInt32(0); // response
            sp.AddString(args);
            sp.AddData(data.Data,data.Length);
            _Client.SendPacket(server, sp);
        }

        public void SendTest(Server server)
        {
            Log.Debug("Sending Test packet");
            Packet sp = new Packet(Packet.Opcodes.Test);
            sp.AddString("TEST");
            sp.AddInt32(300);
            sp.AddByte((byte)32);
            sp.AddFloat32(1.234F);
            sp.AddString("TEST2");
            _Client.SendPacket(server, sp);

        }

        /*
         * 		function->name = pak->ReadString();
		function->response = pak->ReadInt32();
		function->params = pak->ReadString();
		function->vars = pak->ReadString();
		int datastart = pak->pos;
		int datalength = pak->Length - datastart;
		function->data->AddData(pak->Data+datastart,datalength);
         * */

    }
}
