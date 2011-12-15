/*
 * NativeFunctionExample
 * 
 * Use '/car2' command to spawn a vehicle.
 * Shows how to capture samps Callbacks, and invoke samps native functions.
 * Idealy you would use the Samp.API classes instead of this, but this is handy for when something is not yet implimented in Samp.API.
 * Note that you cannot return any values from samps callbacks (todo?)
 * Note that all callbacks & native functions must be implimented in DotnetFS.pwn filterscript.
 */

using System;
using Samp.Scripts;
using Samp.API;
using Samp.Util;
using Samp.Client;

namespace Samp.Scripts.ExampleScripts
{
    public class NativeFunctionExample : ScriptBase
    {
        public override void OnLoad()
        {
            Samp.Client.InternalEvents.OnCallbackReceived += OnCallbackReceived;
        }

        public override void OnUnload() 
        {
            Samp.Client.InternalEvents.OnCallbackReceived -= OnCallbackReceived;
        }

        public void OnCallbackReceived(object sender, Samp.Client.OnCallbackReceivedEventArgs args)
        {
            args.Data.Pos = 0; // Note: THIS IS REQUIRED; a method that previously subscribed to OnCallbackReceived could have left it at any arbitrary position in the data buffer
            string callbackname = args.CB.Name;

            if (String.Compare(callbackname, "OnPlayerCommandText") == 0) 
            {
                int playerid = args.Data.ReadInt32();
                string cmdtext = args.Data.ReadString();

                OnPlayerCommandText(playerid, cmdtext);
            }
        }

        public void OnPlayerCommandText(int playerid, string cmdtext)
        {
            string[] cmd = cmdtext.Split(' ');
            if (String.Compare(cmd[0], "/car2") == 0)
            {
                SpawnPlayerCar(playerid);
            }
        }

        public void SpawnPlayerCar(int playerid)
        {
            int model = 429; //banshee
            Samp.Util.FloatRef x = 0.0F, y = 0.0F, z = 0.0F, angle = 0.0F; // must use FloatRef class to return floats from native function, same goes for StringRef & IntRef
            Samp.Client.NativeFunctionRequestor.RequestFunction("GetPlayerPos", playerid, x, y, z);
            NativeFunctionRequestor.RequestFunction("GetPlayerFacingAngle", playerid, angle);
            int vehicleid = NativeFunctionRequestor.RequestFunction("CreateVehicle", model, x.Value, y.Value, z.Value, angle.Value, 0, 0, 300);// note that we use x.Value now
            NativeFunctionRequestor.RequestFunction("PutPlayerInVehicle", playerid,vehicleid,0);
            NativeFunctionRequestor.RequestFunction("SendClientMessage", playerid, 0,"{00FF00}Vehicle Spawned.");
        }
    }
}
