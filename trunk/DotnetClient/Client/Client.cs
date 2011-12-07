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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Samp.Util;

namespace Samp.Client
{
    public class Client
    {
        public static Client Instance; // singleton instance
        Socket _Socket;
        System.Net.IPEndPoint RemoteEP;
        public PacketProcessor PakProcessor;
        public PacketBuilder PakSender;

        public Server _Server;

        public Client()
        {
            Instance = this;
            PakProcessor = new PacketProcessor(this);
            PakSender = new PacketBuilder(this);
        }

        public bool Connect(Server server)
        {
            Log.Debug("Connecting to server: " + server.Address);
            _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse(server.Address);
            RemoteEP = new IPEndPoint(ipAdd, server.Port);
            try
            {
                _Socket.Connect(RemoteEP);
            }
            catch (Exception ex)
            {
                Log.Warning("Connection failed");
                return false;
            }
            _Server = server;
            _Server.IsConnected = true;

            Thread t = new Thread(new ParameterizedThreadStart(WaitReceive));
            t.Start(_Server);
            t = new Thread(new ParameterizedThreadStart(WaitSend));
            t.Start(_Server);
            t = new Thread(new ParameterizedThreadStart(CheckConnectionTimeout));
            t.Start(_Server);
            Log.Debug("Connection successful");

            Thread.Sleep(100);
            PakSender.SendAuthRequest(server);

            return true;
        }

        public void CheckConnectionTimeout(object oserver)
        {
            Server server = (Server)oserver;
            server.LastPacketReceived = DateTime.Now;
            while (server.IsConnected)
            {
                Thread.Sleep(1000);
                 TimeSpan span = server.LastPacketReceived.Subtract ( DateTime.Now );
                if (span.TotalSeconds <= 30) continue;
                Disconnect(server);
                return;
                // no packet received for 30 seconds
            }
        }

        public bool Disconnect(Server server)
        {
            server.IsAuthenticated = false;
            server.IsConnected = false;
            Thread.Sleep(100);
            return true;
        }

                
        public void WaitReceive(object oserver)
        {
            Server server = (Server)oserver;
            byte[] buf = new byte[Server.MAX_BUFF];
	        while (server.IsConnected)
	        {
	            int length=0;
	            try
                {
                    length = _Socket.Receive(buf);
                }
                catch (Exception ex)
                {
                    Log.Warning("Server closed connection.");
                    server.IsConnected = false;
                    return;
                }
                //Log.Debug("Data receive: "+length);
		        //length = recv(server->_Socket, buf, Server.MAX_BUFF, 0);
                //Log.Debug("DATA RECV");
		        if(length <= 0)
		        {
                    Log.Debug("data length 0?");
			        for (int i=0;i<Server.MAX_BUFF;i++) {buf[i] = 0;}
			        Thread.Sleep(1);
			        continue;
		        }
		        Packet pak = new Packet();
                pak.Length = BitConverter.ToUInt16(buf, 0);
		        pak.Opcode = buf[2];
                for (int i = 0; i < pak.Length - pak.headerlength; i++) { pak.Data[i] = buf[i + pak.headerlength]; }
                pak.Length -= pak.headerlength;
                pak.Pos = 0;
                Thread t = new Thread(new ParameterizedThreadStart(PakProcessor.ProcessPacket));
                PacketProcessor.ProcessPacketParams args = new PacketProcessor.ProcessPacketParams();
                args.server = server;
                args.packet = pak;
                t.Start(args);
                server.LastPacketReceived = DateTime.Now;
		        //PakProcessor.ProcessPacket(server,pak);

                for (int i = 0; i < Server.MAX_BUFF; i++) { buf[i] = 0; }
		        Thread.Sleep(1);
	        }
            Log.Debug("Server Disconnected");
            server.IsConnected = false;
            server.IsAuthenticated = false;
        }


        public void WaitSend(object oserver)
        {
            Server server = (Server)oserver;
	        while (server.IsConnected)
	        {
                //Log.Debug("Q");
		        if (server.sendbuf[0] != 0)
		        { // if we have some data in out send buffer
                    //Log.Debug("Data to send!");
			        while (server.sbuflock) {System.Threading.Thread.Sleep(1);} 
			        server.sbuflock = true;
			        int bufpos = 0;
			        while (bufpos < Server.MAX_BUFF && server.sendbuf[bufpos] != 0) 
			        { // for each packet that has data
                        ushort size = BitConverter.ToUInt16(server.sendbuf, bufpos);
                        byte[] buf = new byte[size];
				        for (int i=0;i<size;i++){buf[i] = server.sendbuf[bufpos+i];} // copy the data
                        try
                        {
                            _Socket.Send(buf);
                        }
                        catch (Exception ex)
                        {
                            Log.Message("Server closed connection.");
                            server.IsConnected = false;
                            return;
                        }
				        bufpos += size; // next packet
			        }
			        for (int i=0;i<Server.MAX_BUFF;i++) {server.sendbuf[i] = 0;}
			        server.sbufpos = 0;
			        server.sbuflock = false;
		        }
                System.Threading.Thread.Sleep(1); 
	        }
        }

        public void SendPacket(Server server, Packet packet)
        { // just adds the packet data to clients send buffer
	        if (packet == null) return;
	        if (packet.Opcode == 0x00) return;
	        //packet.SetLength();
            packet.Length += packet.headerlength;
	        if (packet.Length <= 0) return;
            if (server.sbufpos + packet.Length >= Server.MAX_BUFF) return;
            while (server.sbuflock) { Thread.Sleep(1); }
            server.sbuflock = true;


            byte[] bar = BitConverter.GetBytes(packet.Length);
            server.sendbuf[server.sbufpos + 0] = bar[0];
            server.sendbuf[server.sbufpos + 1] = bar[1];
            server.sendbuf[server.sbufpos + 2] = packet.Opcode;
            for (int i = 0; i < packet.Length-packet.headerlength; i++) { server.sendbuf[server.sbufpos + packet.headerlength + i] = packet.Data[i]; }

            server.sbufpos += packet.Length;
            server.sbuflock = false;
            packet.Length -= packet.headerlength;

            InternalEvents.FireOnPacketSent(this, new OnPacketSentEventArgs(server, this, packet));
        }

    }
}
