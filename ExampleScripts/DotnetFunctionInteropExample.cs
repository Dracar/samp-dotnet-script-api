
/*
 * This is the companion of DotnetFunctionInteropExample.pwn
 * This is an example of calling a dotnet function from pawn, then calling a pawn public function from dotnet
 * 
 */


using System;
using System.Threading;
using Samp.Client;
using Samp.API;
using Samp.Util;

namespace ExampleScripts
{
    public class DotnetFunctionInteropExample : Samp.Scripts.ScriptBase
    {
        public override void  OnLoad()
        {
            Samp.Client.InternalEvents.OnFunctionRequestReceived += OnFunctionRequestReceived;
        }

        public override void OnUnload()
        {
            Samp.Client.InternalEvents.OnFunctionRequestReceived -= OnFunctionRequestReceived;
        }

        public void OnFunctionRequestReceived(object sender,OnFunctionRequestReceivedEventArgs args)
        {
            if (String.Compare(args.FunctionName, "DotnetFunctionExample") == 0) // we received our function request
            {
                DotnetFunctionRequestExample(args.CallbackName, (int)args.Args[0], (int)args.Args[1], (float)args.Args[2], (string)args.Args[3]); // call our function
            }
        }

        public void DotnetFunctionRequestExample(string callbackname,int playerid, int i,float f,string s)
        {
            i *= 2; // lets just double all the args for our example
            f *= 2;
            s += s;
            Samp.Client.NativeFunctionRequestor.RequestFunction(callbackname, s,f,i,playerid); // call our samp function
            // Note: you must REVERSE THE ARGS order here for calling PUBLIC amx functions (later i'll fix this, but for now you must reverse them)
            // Note: NativeFunctionRequestor will first search for samp native function, on failing that it will search all AMX scripts for public function, calling the FIRST it finds by that name
            // functions it finds are cached, so subsequent calls dont need to search again
        }
    }
}
