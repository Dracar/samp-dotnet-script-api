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

namespace Samp.Client
{
    public class InternalEvents
    {
        public static void FireOnPacketReceived(object sender, OnPacketReceivedEventArgs args) { if (OnPacketReceived != null) OnPacketReceived(sender, args); }
        public static event EventHandler<OnPacketReceivedEventArgs> OnPacketReceived;

        public static void FireOnPacketSent(object sender, OnPacketSentEventArgs args) { if (OnPacketSent != null) OnPacketSent(sender, args); }
        public static event EventHandler<OnPacketSentEventArgs> OnPacketSent;

        public static void FireOnCallbackReceived(object sender, OnCallbackReceivedEventArgs args) { if (OnCallbackReceived != null) OnCallbackReceived(sender, args); }
        public static event EventHandler<OnCallbackReceivedEventArgs> OnCallbackReceived;

        public static void FireOnFunctionRequestReceived(object sender, OnFunctionRequestReceivedEventArgs args) { if (OnFunctionRequestReceived != null) OnFunctionRequestReceived(sender, args); }
        public static event EventHandler<OnFunctionRequestReceivedEventArgs> OnFunctionRequestReceived;

    }

    public class OnPacketReceivedEventArgs : EventArgs
    {
        public Server _Server;
        public Client _Client;
        public Packet Pak;
        public OnPacketReceivedEventArgs(Server server, Client client, Packet pak)
        {
            _Server = server;
            _Client = client;
            Pak = pak;
        }
    };
    public class OnPacketSentEventArgs : EventArgs
    {
        public Server _Server;
        public Client _Client;
        public Packet Pak;
        public OnPacketSentEventArgs(Server server, Client client, Packet pak)
        {
            _Server = server;
            _Client = client;
            Pak = pak;
        }
    };


    public class OnCallbackReceivedEventArgs : EventArgs
    {
        public Server _Server;
        public Client _Client;
        public string CallbackName;
        //public CallbackProcessor.Callback CB;
        public DataStream Data;
        public OnCallbackReceivedEventArgs(Server server, Client client, string callbackname, DataStream data)
        {
            _Server = server;
            _Client = client;
            CallbackName = callbackname;
            Data = data;
        }
    };


    public class OnFunctionRequestReceivedEventArgs : EventArgs
    {
        public Server Server;
        public Client Client;
        public string FunctionName;
        public string CallbackName;
        public string Paramtypes;
        public DataStream Data;
        public OnFunctionRequestReceivedEventArgs(Server server, Client client, string funcname, string callbackname, string paramtypes, DataStream data)
        {
            Server = server;
            Client = client;
            FunctionName = funcname;
            CallbackName = callbackname;
            Paramtypes = paramtypes;
            Data = data;
        }
    };

}
