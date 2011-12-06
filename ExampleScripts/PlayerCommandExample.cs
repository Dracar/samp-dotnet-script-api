/*
 * PlayerCommandExample
 * 
 * Player uses '/car' command; a vehicle is spawned & player is put in it
 */


using System;
using Samp.Scripts;
using Samp.API;
using Samp.Util;

namespace Samp.Scripts.ExampleScripts
{
    public class PlayerCommandExample : ScriptBase
    {
        public override void OnLoad() // called when script is loaded
        {
            Samp.API.Player.OnPlayerCommandText += OnPlayerCommandText; // subscribe to OnPlayerCommandText event
        }

        public override void OnUnload() // called when script is unloaded
        {
            Samp.API.Player.OnPlayerCommandText -= OnPlayerCommandText;
        }

        public void OnPlayerCommandText(object sender, Player.OnPlayerCommandTextEventArgs args)
        {
			string[] cmd = args.text.Split(' ');
            if (Samp.Util.Util.strcmp(cmd[0], "/car") == 0)
            {
                SpawnPlayerCar(args.player);
            }
        }

        public void SpawnPlayerCar(Player pl)
        {
            int model = 415; //cheetah
            Vehicle v = World.CreateVehicle(model, pl.Pos, pl.ZAngle, 0, 0, 600); // spawn the vehicle
            pl.Vehicle = v; // put player in the vehicle
            pl.ClientMessage(0, "{00FF00}Spawning vehicle."); // send the player a message
        }
    }
}
