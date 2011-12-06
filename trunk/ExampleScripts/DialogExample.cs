/*
 * DialogExample
 * 
 * Player uses '/v' command; a dialog listing vehicles is sent, player selects vehicle & clicks spawn to spawn it
 */



using System;
using Samp.API;
using Samp.Scripts;

namespace Samp.Scripts.ExampleScripts
{
    public class DialogExample : ScriptBase
    {


        public override void OnLoad() 
        {
            Player.OnPlayerCommandText += OnPlayerCommandText;
            Dialog.OnDialogResponse += OnDialogResponse;
        }

        public override void OnUnload() 
        {
            Player.OnPlayerCommandText -= OnPlayerCommandText;
            Dialog.OnDialogResponse -= OnDialogResponse;
        }

        public void OnPlayerCommandText(object sender, Player.OnPlayerCommandTextEventArgs args)
        {
            string[] cmd = args.text.Split(' ');
            if (String.Compare(cmd[0], "/v") == 0)
            {
                SendVehicleDialog(args.player);
            }
        }

        public void OnDialogResponse(object sender, Dialog.OnDialogResponseEventArgs args)
        {
            if (String.Compare("Vehicles", args.dialog.Name) == 0) // comparing name :/
            {
                if (args.response == 0) { return; }
                SpawnVehicle(args.player, args.listitem);
            }
        }


        public void SendVehicleDialog(Player pl)
        {
            API.Dialog d = new Dialog();
            d.Name = "Vehicles";
            d.Style = 2;
            d.Button1 = "Spawn";
            d.Button2 = "Close";
            d.Info = "";
            for (int i = 0; i < Vehicles.Length; i++)
            {
                d.Info += Vehicles[i].Name + "\r\n";
            }
            d.ShowDialogForPlayer(pl);
        }

        public void SpawnVehicle(Player pl, int listitem)
        {
            int model = Vehicles[listitem].Model;
            Vehicle v = World.CreateVehicle(model, pl.Pos, pl.ZAngle, 0, 0, 600); // spawn the vehicle
            pl.Vehicle = v; // put player in the vehicle
            pl.ClientMessage(0, "{00FF00}Spawning vehicle."); // send the player a message
        }


        public struct sVehicle
        {
            public int Model;
            public string Name;
            public sVehicle(int model, string name)
            {
                Model = model;
                Name = name;
            }
        }
        public sVehicle[] Vehicles = 
        {
            new sVehicle(400, "Landstalker"),
            new sVehicle(401, "Bravura"),
            new sVehicle(402, "Buffalo"),
            new sVehicle(403, "Linerunner"),
            new sVehicle(404, "Perenail"),
            new sVehicle(405, "Sentinel"),
            new sVehicle(406, "Dumper"),
            new sVehicle(407, "Firetruck"),
            new sVehicle(408, "Trashmaster"),
            new sVehicle(409, "Stretch"),
            new sVehicle(410, "Manana"),
            new sVehicle(411, "Infernus"),
            new sVehicle(412, "Voodoo"),
            new sVehicle(413, "Pony"),
            new sVehicle(414, "Mule"),
            new sVehicle(415, "Cheetah"),
            new sVehicle(416, "Ambulance"),
            new sVehicle(417, "Levetian"),
            new sVehicle(418, "Moonbeam"),
            new sVehicle(419, "Esperanto"),
            new sVehicle(420, "Taxi"),
            new sVehicle(421, "Washington"),
            new sVehicle(422, "Bobcat"),
            new sVehicle(423, "Mr Whoopee"),
            new sVehicle(424, "BF Injection"),
            new sVehicle(425, "Hunter"),
            new sVehicle(426, "Premier"),
            new sVehicle(427, "Enforcer"),
            new sVehicle(428, "Securicar"),
            new sVehicle(429, "Banshee"),
        };
    }
}
