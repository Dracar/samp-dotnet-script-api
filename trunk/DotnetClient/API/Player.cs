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
        Attribution Copyright Notice: Copyright 2011, Iain Gilbert
        Attribution Phrase (not exceeding 10 words): Samp Dotnet Script API
        Attribution URL: http://code.google.com/p/samp-dotnet-script-api/
 
    Display of Attribution Information to the end user is not required on server login, or at any time the end user is connected to your server.
    You must treat any External Deployment by You of the Original Code or Modifications as a distribution under section 3.1 and make Source Code available under Section 3.2.
    Display of Attribution Information is not required in Larger Works which are defined in the CPAL as a work which combines Covered Code or portions thereof with code not governed by the terms of the CPAL.
 */

using System;
using System.Linq;
using Samp.Client;
using Samp.Util;

namespace Samp.API
{
    public class Player
    {
        //public static Player[] Players;

        public static event EventHandler<OnPlayerConnectEventArgs> OnPlayerConnect;
        public static event EventHandler<OnPlayerConnectEventArgs> OnPlayerDisconnect;
        public class OnPlayerConnectEventArgs : EventArgs
        {
            public Player player;
            public OnPlayerConnectEventArgs(Player pl)
            {
                player = pl;
            }
        };

        public static event EventHandler<OnPlayerCommandTextEventArgs> OnPlayerCommandText;
        public class OnPlayerCommandTextEventArgs : EventArgs
        {
            public Player player;
            public string text;
            public OnPlayerCommandTextEventArgs(Player pl, string txt)
            {
                player = pl;
                text = txt;
            }
        };


        public static event EventHandler<OnPlayerKeyStateChangeEventArgs> OnPlayerKeyStateChange;
        public class OnPlayerKeyStateChangeEventArgs : EventArgs
        {
            public Player player;
            public int newkeys;
            public int oldkeys;
            public OnPlayerKeyStateChangeEventArgs(Player pl, int newkeysbitmask, int oldkeysbitmask)
            {
                player = pl;
                newkeys = newkeysbitmask;
                oldkeys = oldkeysbitmask;
            }
        };

        public static event EventHandler<OnPlayerDeathEventArgs> OnPlayerDeath;
        public class OnPlayerDeathEventArgs : EventArgs
        {
            public Player player;
            public Player killer;
            public OnPlayerDeathEventArgs(Player pl, Player kilr)
            {
                player = pl;
                killer = kilr;
            }
        };



        public static Player[] Players;
        public static Player GetPlayerByID(int id)
        {
            lock (Players)
            {
                for (int i = 0; i < Players.Count(); i++)
                {
                    if (Players[i] == null) continue;
                    if (Players[i].ID == id) return Players[i];
                }
            }
            Samp.Util.Log.Debug("Player not found, creating new.");
            return new Player(id);
        }
        internal static void RemovePlayer(Player p)
        {
            //if (OnPlayerDestroyed != null) OnPlayerDestroyed(null, new OnPlayersCreatedEventArgs(this));
            lock (Players)
            {
                for (int i = 0; i < Players.Count(); i++)
                {
                    if (Players[i] == null) continue;
                    if (Players[i] == p) { Samp.Util.Log.Debug("Removing Player."); Players[i] = null; return; }
                }
            }
        }

        internal Player(int id)
        {
            lock (Players)
            {
                ID = id;
                for (int i = 0; i < Players.Count(); i++)
                {
                    if (Players[i] == null) { Players[i] = this; break; }
                }
            }
            //if (OnVehicleCreated != null) OnVehicleCreated(this, new OnVehicleCreatedEventArgs(this));
        }















        public static void Init()
        {
            Players = new Player[World.MAX_PLAYERS];
            InternalEvents.OnCallbackReceived += new EventHandler<Samp.Client.OnCallbackReceivedEventArgs>(OnCallbackReceived);
            
        }

        public static void UnInit()
        {
            Players = null;
            InternalEvents.OnCallbackReceived -= OnCallbackReceived;
        }

        public int ID;
        public int PrivLevel = 0;
        public enum PRIV_LEVELS
        {
            Guest,
            User,
            Donator,
            Helper,
            GM,
            Admin,
            Root
        }



        public static void OnCallbackReceived(object sender, OnCallbackReceivedEventArgs args)
        {
            args.Data.Pos = 0;
            Log.Debug("OnCallbackReceived: " + args.CB.Name);
            if (String.Compare(CallbackProcessor.GetCallbackById(args.CB.Opcode).Name, "OnPlayerConnect") == 0)
            {
                int playerid = args.Data.ReadInt32();
                Player player = World.GetPlayerById(playerid);
                Log.Debug("Player (" + playerid + ") " + player.Name + " Connected");
                if (OnPlayerConnect != null) OnPlayerConnect(null, new OnPlayerConnectEventArgs(player));
            }

            if (String.Compare(CallbackProcessor.GetCallbackById(args.CB.Opcode).Name, "OnPlayerDisconnect") == 0)
            {
                int playerid = args.Data.ReadInt32();
                Player player = World.GetPlayerById(playerid);
                Log.Debug("Player (" + playerid + ") " + player.Name + " Disconnected");
                if (OnPlayerDisconnect != null) OnPlayerDisconnect(null, new OnPlayerConnectEventArgs(player));
                RemovePlayer(player);
            }

            if (String.Compare(CallbackProcessor.GetCallbackById(args.CB.Opcode).Name, "OnPlayerCommandText") == 0)
            {
                int playerid = args.Data.ReadInt32();
                Samp.API.Player player = Player.GetPlayerByID(playerid);
                string command = args.Data.ReadString();
                if (OnPlayerCommandText != null) OnPlayerCommandText(null, new OnPlayerCommandTextEventArgs(player, command));
            }

            if (String.Compare(CallbackProcessor.GetCallbackById(args.CB.Opcode).Name, "OnPlayerKeyStateChange") == 0)
            {
                int playerid = args.Data.ReadInt32();
                int newkeys = args.Data.ReadInt32();
                int oldkeys = args.Data.ReadInt32();
                Player player = Player.GetPlayerByID(playerid);
                if (OnPlayerKeyStateChange != null) OnPlayerKeyStateChange(null, new OnPlayerKeyStateChangeEventArgs(player, newkeys,oldkeys));
            }

            if (String.Compare(CallbackProcessor.GetCallbackById(args.CB.Opcode).Name, "OnPlayerDeath") == 0)
            {
                int playerid = args.Data.ReadInt32();
                int killerid = args.Data.ReadInt32();
                Player player = Player.GetPlayerByID(playerid);
                Player killer = Player.GetPlayerByID(killerid);
                if (OnPlayerDeath != null) OnPlayerDeath(null, new OnPlayerDeathEventArgs(player, killer));
            }
        }
        /*
        public static void Player_OnPlayerDisconnect(object sender, Player.OnPlayerConnectEventArgs args)
        {
            Util.Log.Debug("Player OnPlayerDisconnect");
            RemovePlayer(args.player);
        }*/


        public bool IsConnected
        {
            get
            {
                return System.Convert.ToBoolean(NativeFunctionRequestor.RequestFunction("IsPlayerConnected", "i", ID));
            }
        }

        private int m_Money = 0;
        public int Money
        {
            get
            {
                int cash = NativeFunctionRequestor.RequestFunction("GetPlayerMoney", "i", ID);
                if (m_Money != cash)
                {
                    Log.Warning("Player " + Name + " Money mismatch. (" + cash + "/" + m_Money + ").");
                    Money = m_Money;
                }
                return m_Money;
            }
            set
            {
                NativeFunctionRequestor.RequestFunction("ResetPlayerMoney", "i", ID);
                NativeFunctionRequestor.RequestFunction("GivePlayerMoney", "ii", ID, value);
                m_Money = value;
            }
        }
        public Dialog dialog;


        public float Health
        {
            get
            {
                FloatRef z = new FloatRef(0.0F);
                NativeFunctionRequestor.RequestFunction("GetPlayerHealth", "iv", ID, z);
                return z.Value;
            }
            set
            {
                NativeFunctionRequestor.RequestFunction("SetPlayerHealth", "if", ID, value);
            }
        }

        private string m_Name = "";
        public string Name
        {
            get
            {
                if (m_Name.Length > 1) return m_Name;
                StringRef s = new StringRef("");
                NativeFunctionRequestor.RequestFunction("GetPlayerName", "ipi", ID, s,32);
                m_Name = s.Value;
                return m_Name;
            }
            set
            {
                m_Name = value;
                NativeFunctionRequestor.RequestFunction("SetPlayerName", "is", ID, value);
            }
        }

        public float ZAngle
        {
            get
            {
                FloatRef fr = new FloatRef(0.0F);
                NativeFunctionRequestor.RequestFunction("GetPlayerFacingAngle","iv",ID, fr);
                return fr.Value;
            }
            set
            {
                NativeFunctionRequestor.RequestFunction("SetPlayerFacingAngle", "if", ID, value);
            }
        }

        public Vector3 Pos
        {
            get
            {
                FloatRef x = new FloatRef(0.0F);
                FloatRef y = new FloatRef(0.0F);
                FloatRef z = new FloatRef(0.0F);
                NativeFunctionRequestor.RequestFunction("GetPlayerPos", "ivvv", ID, x, y, z);
                Vector3 vec = new Vector3(x.Value, y.Value, z.Value);
                return vec;
            }
            set
            {
                NativeFunctionRequestor.RequestFunction("SetPlayerPos", "ifff", ID, value.X, value.Y, value.Z);
            }
        }



        public Vector3 Velocity
        {
            get
            {
                FloatRef x = new FloatRef(0.0F);
                FloatRef y = new FloatRef(0.0F);
                FloatRef z = new FloatRef(0.0F);
                NativeFunctionRequestor.RequestFunction("GetPlayerVelocity", "ivvv", ID, x, y, z);
                Vector3 vec = new Vector3(x.Value, y.Value, z.Value);
                return vec;
            }
            set
            {
                NativeFunctionRequestor.RequestFunction("SetPlayerVelocity", "ifff", ID, value.X, value.Y, value.Z);
            }
        }

        public Vehicle Vehicle
        {
            get
            {
                int vid = NativeFunctionRequestor.RequestFunction("GetPlayerVehicleID", "i", ID);
                if (vid <= 0) return null;
                return Vehicle.GetVehicleByID(vid);
            }
            set
            {
                if (value == null)
                {
                    RemovePlayerFromVehicle();
                    return;
                }
                PutPlayerInVehicle(value, 0);
            }
        }

        public void PutPlayerInVehicle(Vehicle vehicle, int seat)
        {
            NativeFunctionRequestor.RequestFunction("PutPlayerInVehicle", "iii", ID, vehicle.ID, seat);
        }

        public void RemovePlayerFromVehicle()
        {
            NativeFunctionRequestor.RequestFunction("RemovePlayerFromVehicle", "i", ID);
        }

        /*public Menu GetPlayerMenu()
        {
            return Menu.GetMenuByID(NativeFunctionRequestor.RequestFunction("GetPlayerMenu", "i", ID));
        }*/


        public void ShowDialog(Dialog d)
        {
            NativeFunctionRequestor.RequestFunction("ShowPlayerDialog", "iiissss", ID, d.ID, d.Style, d.Name, d.Info, d.Button1, d.Button2);
            dialog = d;
        }

        public void ClearDialog()
        {
            NativeFunctionRequestor.RequestFunction("ShowPlayerDialog", "iiissss", ID, -1, 0,"","","","");
        }

        public Dialog GetPlayerDialog()
        {
            return dialog;
        }

        public void GameText(string text, int time, int style)
        {
            NativeFunctionRequestor.RequestFunction("GameTextForPlayer", "isii", ID, text,time,style);
        }

        public void ClientMessage(int colour, string message)
        {
            NativeFunctionRequestor.RequestFunction("SendClientMessage", "iis", ID, colour,message);
        }



        public enum Keys
        {
            KEY_ACTION = 1,
            KEY_CROUCH = 2,
            KEY_FIRE = 4,
            KEY_SPRINT = 8,
            KEY_SECONDARY_ATTACK = 16,
            KEY_JUMP = 32,
            KEY_LOOK_RIGHT = 64,
            KEY_HANDBRAKE = 128,
            KEY_LOOK_LEFT = 256,
            KEY_LOOK_BEHIND_VEHICLE = 320, //look left + look right
            KEY_SUBMISSION = 512,
            KEY_LOOK_BEHIND = 512,
            KEY_WALK = 1024,
            KEY_AIM = 128,
            KEY_ANALOG_UP = 2048,
            KEY_ANALOG_DOWN = 4096,
            KEY_ANALOG_LEFT = 8192,
            KEY_ANALOG_RIGHT = 16384,
            KEY_YES = 65536,
            KEY_NO = 131072,
            KEY_CTRL_BACK = 262144,
            KEY_UP = -128,
            KEY_DOWN = 128,
            KEY_LEFT = -128,
            KEY_RIGHT = 128
        };
    }
}
