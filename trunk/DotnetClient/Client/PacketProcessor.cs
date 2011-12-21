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
    public class PacketProcessor
    {
        public Client _Client;
        public PacketProcessor(Client client)
        {
            _Client = client;
        }

        public struct ProcessPacketParams
        {
            public Server server;
            public Packet packet;
        }
        public void ProcessPacket(object args)
        {
            Server server = ((ProcessPacketParams)args).server;
            Packet pak = ((ProcessPacketParams)args).packet;
	        if (!server.IsConnected) return;
            //Log.Debug("Packet received: " + pak.Opcode.ToString());

	        if (pak.Opcode == (byte)Packet.Opcodes.Auth)
	        { // authentication response
                Log.Debug("Auth pak receive");
                bool success = pak.ReadBool();
                if (success) { server.IsAuthenticated = true; Log.Debug("Auth success."); }
                else {server.IsAuthenticated = false; Log.Debug("Auth failed.");}
	        }
	        if (!server.IsAuthenticated) return;
	        if (pak.Opcode == (byte)Packet.Opcodes.Ping)
	        {
                _Client.PakSender.SendPingReply(server);
	        }

            if (pak.Opcode == (byte)Packet.Opcodes.Callback)
            {
                string callbackname = pak.ReadString();
                Log.Debug("Callback Received: " + callbackname);

                byte[] callbackdata = pak.ReadData(pak.Length - pak.Pos);

                DataStream sdata = new DataStream();
                sdata.Data = callbackdata;
                sdata.Length = (ushort)callbackdata.Length;
                InternalEvents.FireOnCallbackReceived(null, new OnCallbackReceivedEventArgs(server,_Client,callbackname,sdata));
                 
                //return;
            }

            if (pak.Opcode == (byte)Packet.Opcodes.FunctionRequest)
            {
                Log.Debug("function request received");
                string funcname = pak.ReadString();
                string callbackname = pak.ReadString();
                string paramtypes = pak.ReadString();
                byte[] funcdata = pak.ReadData(pak.Length - pak.Pos);

                DataStream sdata = new DataStream();
                sdata.Data = funcdata;
                sdata.Length = (ushort)funcdata.Length;
                Log.Debug("funcreq: " + funcname);
                InternalEvents.FireOnFunctionRequestReceived(null, new OnFunctionRequestReceivedEventArgs(server, _Client, funcname,callbackname,paramtypes,sdata));

                //return;
            }

            if (pak.Opcode == (byte)Packet.Opcodes.Test)
            {
                Log.Debug("Packet Test received");
                Log.Debug(pak.ReadString());
                Log.Debug(pak.ReadInt32().ToString());
                Log.Debug(pak.ReadByte().ToString());
                Log.Debug(pak.ReadFloat32().ToString());
                Log.Debug(pak.ReadString());

            }
          // Log.Debug("paklen: "+pak.Data.Length);
            pak.Pos = 0;
            InternalEvents.FireOnPacketReceived(this,new OnPacketReceivedEventArgs(server,_Client,pak));


        }
    }
}
