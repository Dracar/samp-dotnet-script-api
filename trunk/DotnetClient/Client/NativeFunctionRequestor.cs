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
using System.Text;
using Samp.Util;

namespace Samp.Client
{
    public class NativeFunction
    {
        public string Guid;
        public int Response;
	    public string Name;
	    public string Args;
	    public DataStream Data;
	    public NativeFunction()
	    {
            Guid = "";
		    Response=0;
		    Name = "";
		    Args = "";
		    Data = new DataStream();
	    }

        public NativeFunction(string name, string args)
        {
            Guid = "";
            Response = 0;
            Name = name;
            Args = args;
            Data = new DataStream();
        }
    }

    public class NativeFunctionRequestor
    {
        public static int RequestFunction(string name,params object[] data)
        {
            byte[] argsb = new byte[10];
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] is int) argsb[i] = (byte)'i';
                if (data[i] is float) argsb[i] = (byte)'f';
                if (data[i] is string) argsb[i] = (byte)'s';
                if (data[i] is IntRef) argsb[i] = (byte)'v';
                if (data[i] is FloatRef) argsb[i] = (byte)'v';
                if (data[i] is StringRef) argsb[i] = (byte)'p';
            }
            string args = System.Text.Encoding.ASCII.GetString(argsb);
            return RequestFunctionWithArgs(name, args, data);
        }
        public static int RequestFunctionWithArgs(string name, string args, params object[] data)
        {
            NativeFunction func = new NativeFunction(name,args);
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] is int) func.Data.AddInt32((int)data[i]);
                else if (data[i] is float) func.Data.AddFloat32((float)data[i]);
                else if (data[i] is string) func.Data.AddString(((string)data[i]));
                else if (data[i] is IntRef) func.Data.AddInt32((((IntRef)data[i]).Value));
                else if (data[i] is FloatRef) func.Data.AddFloat32((((FloatRef)data[i]).Value));
                else if (data[i] is StringRef) func.Data.AddString((((StringRef)data[i]).Value));
            }
            NativeFunctionRequestor fr = new NativeFunctionRequestor(Client.Instance);
            func = fr.RequestFunctionWithArgs(Server.Instance, func);
            if (func == null) { Log.Warning("Function request " + name + " failed."); return 0; }
			func.Data.Pos = 0;
			/*string dat = "data: ";
			for (int i=0;i<func.Data.Length;i++)
			{
				dat += func.Data.Data[i].ToString() + " ";
			}
			Log.Debug(dat);
			func.Data.Pos = 0;*/
			
            byte[] argsb = Encoding.ASCII.GetBytes(args);
            for (int i = 0; i < args.Length; i++)
            {
                if (argsb[i] == 'v')
                {
                    if (data[i] is int) data[i] = func.Data.ReadInt32();
                    else if (data[i] is float) { data[i] = func.Data.ReadFloat32(); }
                    else if (data[i] is IntRef) { ((IntRef)data[i]).Value = func.Data.ReadInt32(); }
                    else if (data[i] is FloatRef) { ((FloatRef)data[i]).Value = func.Data.ReadFloat32(); }
                }
                else if (argsb[i] == 'p')
                {
                    if (data[i] is string) data[i] = func.Data.ReadString();
                    else if (data[i] is StringRef) { ((StringRef)data[i]).Value = func.Data.ReadString();}
                }
            }
            return func.Response;
        }
        
        Client _Client;
        NativeFunction request;
        NativeFunction response;
        const int MAX_RESPONSETIME = 1000;

        public NativeFunctionRequestor(Client client)
        {
            _Client = client;
        }

        public NativeFunction RequestFunctionWithArgs(Server server,NativeFunction function)
        {
            request = function;
            response = null;
            function.Guid = System.Guid.NewGuid().ToString();

            InternalEvents.OnPacketReceived += OnPacketReceived;

            _Client.PakSender.SendNativeFunction(server, function.Guid, function.Name, function.Args, function.Data);

            int i=0;
            while ((response == null) && (i < MAX_RESPONSETIME))
            {
                System.Threading.Thread.Sleep(1);
                i++;
            }
            if ((response == null) || (i >= MAX_RESPONSETIME)) 
            { 
                Log.Warning("Native function request failed.");
                InternalEvents.OnPacketReceived -= OnPacketReceived;
                return null; 
            }
            InternalEvents.OnPacketReceived -= OnPacketReceived;
            response.Data.Pos = 0;
            return response;
        }

        public void OnPacketReceived(object sender, OnPacketReceivedEventArgs args)
        {
            if (request == null) return; // not awaiting request
            if (response != null) return; // already received a reply
            if ((Packet.Opcodes)args.Pak.Opcode != Packet.Opcodes.FunctionReply) return; // wrong packet opcode
            args.Pak.Pos = 0;
            string guid = args.Pak.ReadString();
            if (String.Compare(guid,request.Guid) != 0) return; // wrong function reply
            NativeFunction ret = new NativeFunction();
            ret.Guid = guid;
            ret.Name = args.Pak.ReadString();
            ret.Response = args.Pak.ReadInt32();
            ret.Args = args.Pak.ReadString();
            int datalength = args.Pak.Length - args.Pak.Pos;
			//Log.Debug("Good function reply received: " + ret.Name + " / " + ret.Response + " / " + ret.Args + " / " + args.Pak.Length + " / " + args.Pak.Pos + " / " + datalength);
            
            ret.Data.AddData(args.Pak.ReadData(datalength),datalength);
            response = ret;
        }

    }
}
