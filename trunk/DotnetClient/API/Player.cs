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

        public static event EventHandler<OnPlayerKeyPressedEventArgs> OnPlayerKeyReleased;
        public static event EventHandler<OnPlayerKeyPressedEventArgs> OnPlayerKeyPressed;
        public class OnPlayerKeyPressedEventArgs : EventArgs
        {
            public Player player;
            public int Key;
            public OnPlayerKeyPressedEventArgs(Player pl, int key)
            {
                player = pl;
                Key = key;
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
        internal static bool RemovePlayer(Player p)
        {
            //if (OnPlayerDestroyed != null) OnPlayerDestroyed(null, new OnPlayersCreatedEventArgs(this));
            lock (Players)
            {
                for (int i = 0; i < Players.Count(); i++)
                {
                    if (Players[i] == null) continue;
                    if (Players[i] == p) { Samp.Util.Log.Debug("Removing Player."); Players[i] = null; return true; }
                }
            }
            return false;
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
        public int Keys = 0;
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
            Log.Debug("OnCallbackReceived: " + args.CallbackName);
            if (String.Compare(args.CallbackName, "OnPlayerConnect") == 0)
            {
                int playerid = args.Data.ReadInt32();
                Player player = World.GetPlayerById(playerid);
                Log.Debug("Player (" + playerid + ") " + player.Name + " Connected");
                if (OnPlayerConnect != null) OnPlayerConnect(null, new OnPlayerConnectEventArgs(player));
            }

            if (String.Compare(args.CallbackName, "OnPlayerDisconnect") == 0)
            {
                int playerid = args.Data.ReadInt32();
                Player player = World.GetPlayerById(playerid);
                Log.Debug("Player (" + playerid + ") " + player.Name + " Disconnected");
                if (OnPlayerDisconnect != null) OnPlayerDisconnect(null, new OnPlayerConnectEventArgs(player));
                RemovePlayer(player);
            }

            if (String.Compare(args.CallbackName, "OnPlayerCommandText") == 0)
            {
                int playerid = args.Data.ReadInt32();
                Samp.API.Player player = Player.GetPlayerByID(playerid);
                string command = args.Data.ReadString();
                if (OnPlayerCommandText != null) OnPlayerCommandText(null, new OnPlayerCommandTextEventArgs(player, command));
            }

            if (String.Compare(args.CallbackName, "OnPlayerKeyStateChange") == 0)
            {
                int playerid = args.Data.ReadInt32();
                //uint newkeys = args.Data.ReadUInt32(); // no...
                //uint oldkeys = args.Data.ReadUInt32();
                //int newkeys = args.Data.ReadInt32(); // also no...
                //int oldkeys = args.Data.ReadInt32();
                int newkeys = Math.Abs(args.Data.ReadInt32()); // erm... this seems to match http://wiki.sa-mp.com/wiki/GetPlayerKeys... WTF?
                int oldkeys = Math.Abs(args.Data.ReadInt32());

                Player player = Player.GetPlayerByID(playerid);
                player.Keys = newkeys;
                if (OnPlayerKeyStateChange != null) OnPlayerKeyStateChange(null, new OnPlayerKeyStateChangeEventArgs(player, newkeys, oldkeys));

                int key = Math.Abs(newkeys - oldkeys);
                
                if (Player.IsKeyJustPressed(key,newkeys,oldkeys))
                {
                    if (OnPlayerKeyPressed != null) OnPlayerKeyPressed(null, new OnPlayerKeyPressedEventArgs(player, key));
                }
                else
                {
                    if (OnPlayerKeyReleased != null) OnPlayerKeyReleased(null, new OnPlayerKeyPressedEventArgs(player, key));
                }
            }

            if (String.Compare(args.CallbackName, "OnPlayerDeath") == 0)
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


        private Cached<bool> _isConnected = false;
        public bool IsConnected
        {
            get
            {
                if (_isConnected.ElapsedMS >= APIMain.CacheMS)
                {
                    _isConnected = System.Convert.ToBoolean(NativeFunctionRequestor.RequestFunctionWithArgs("IsPlayerConnected", "i", ID));
                }
                return _isConnected;
            }
        }

        public static bool ServerSideMoney = false;
        private Cached<int> _money = 0; 
        //private int m_Money = 0;
        public int Money
        {
            get
            {
                if (!ServerSideMoney)
                {
                    if (_money.ElapsedMS >= APIMain.CacheMS)
                    {
                        _money = NativeFunctionRequestor.RequestFunctionWithArgs("GetPlayerMoney", "i", ID);
                    }
                    return _money;
                }
                else
                {
                    return _money;
                }
            }
            set
            {
                NativeFunctionRequestor.RequestFunctionWithArgs("ResetPlayerMoney", "i", ID);
                NativeFunctionRequestor.RequestFunctionWithArgs("GivePlayerMoney", "ii", ID, value);
                _money = value;
            }
        }
        public Dialog dialog;

        private Cached<float> _health = 0.0F; 
        public float Health
        {
            get
            {
                if (_health.ElapsedMS >= APIMain.CacheMS)
                {
                    FloatRef z = new FloatRef(0.0F);
                    NativeFunctionRequestor.RequestFunctionWithArgs("GetPlayerHealth", "iv", ID, z);
                    _health = z.Value;
                }
                return _health;
            }
            set
            {
                NativeFunctionRequestor.RequestFunctionWithArgs("SetPlayerHealth", "if", ID, value);
            }
        }

        private string _name = ""; // does name ever change?
        public string Name
        {
            get
            {
                if (_name.Length > 1) return _name; // does name ever change?
                StringRef s = new StringRef("");
                NativeFunctionRequestor.RequestFunctionWithArgs("GetPlayerName", "ipi", ID, s,32);
                _name = s.Value;
                return _name;
            }
            set
            {
                _name = value;
                NativeFunctionRequestor.RequestFunctionWithArgs("SetPlayerName", "is", ID, value);
            }
        }

        private Cached<float> _zAngle = 0.0F; 
        public float ZAngle
        {
            get
            {
                if (_zAngle.ElapsedMS >= APIMain.CacheMS)
                {
                    FloatRef fr = new FloatRef(0.0F);
                    NativeFunctionRequestor.RequestFunctionWithArgs("GetPlayerFacingAngle", "iv", ID, fr);
                    _zAngle = fr.Value;
                }
                return _zAngle;
            }
            set
            {
                _zAngle = value;
                NativeFunctionRequestor.RequestFunctionWithArgs("SetPlayerFacingAngle", "if", ID, value);
            }
        }

        private Cached<Vector3> _pos = new Cached<Vector3>(new Vector3()); 
        public Vector3 Pos
        {
            get
            {
                if (_pos.ElapsedMS >= APIMain.CacheMS)
                {

                    FloatRef x = new FloatRef(0.0F);
                    FloatRef y = new FloatRef(0.0F);
                    FloatRef z = new FloatRef(0.0F); ;
                    NativeFunctionRequestor.RequestFunctionWithArgs("GetPlayerPos", "ivvv", ID, x, y, z);
                    _pos = new Vector3(x.Value, y.Value, z.Value);
                }
                return _pos;
            }
            set
            {
                _pos = value;
                NativeFunctionRequestor.RequestFunctionWithArgs("SetPlayerPos", "ifff", ID, value.X, value.Y, value.Z);
            }
        }


        private Cached<Vector3> _velocity = new Cached<Vector3>(new Vector3()); 
        public Vector3 Velocity
        {
            get
            {
                if (_velocity.ElapsedMS >= APIMain.CacheMS)
                {

                    FloatRef x = new FloatRef(0.0F);
                    FloatRef y = new FloatRef(0.0F);
                    FloatRef z = new FloatRef(0.0F); ;
                    NativeFunctionRequestor.RequestFunctionWithArgs("GetPlayerVelocity", "ivvv", ID, x, y, z);
                    _velocity = new Vector3(x.Value, y.Value, z.Value);
                }
                return _velocity;
            }
            set
            {
                _velocity = value;
                NativeFunctionRequestor.RequestFunctionWithArgs("SetPlayerVelocity", "ifff", ID, value.X, value.Y, value.Z);
            }

        }

        public Vehicle Vehicle
        {
            get
            {
                int vid = NativeFunctionRequestor.RequestFunctionWithArgs("GetPlayerVehicleID", "i", ID);
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
            NativeFunctionRequestor.RequestFunctionWithArgs("PutPlayerInVehicle", "iii", ID, vehicle.ID, seat);
        }

        public void RemovePlayerFromVehicle()
        {
            NativeFunctionRequestor.RequestFunctionWithArgs("RemovePlayerFromVehicle", "i", ID);
        }

        /*public Menu GetPlayerMenu()
        {
            return Menu.GetMenuByID(NativeFunctionRequestor.RequestFunctionWithArgs("GetPlayerMenu", "i", ID));
        }*/


        public void ShowDialog(Dialog d)
        {
            NativeFunctionRequestor.RequestFunctionWithArgs("ShowPlayerDialog", "iiissss", ID, d.ID, d.Style, d.Name, d.Info, d.Button1, d.Button2);
            dialog = d;
        }

        public void ClearDialog()
        {
            NativeFunctionRequestor.RequestFunctionWithArgs("ShowPlayerDialog", "iiissss", ID, -1, 0,"","","","");
        }

        public Dialog GetPlayerDialog()
        {
            return dialog;
        }

        public void GameText(string text, int time, int style)
        {
            NativeFunctionRequestor.RequestFunctionWithArgs("GameTextForPlayer", "isii", ID, text,time,style);
        }

        public void ClientMessage(int colour, string message)
        {
            NativeFunctionRequestor.RequestFunctionWithArgs("SendClientMessage", "iis", ID, colour,message);
        }

        public void AttachObject(GameObject obj,Vector3 offset, Vector3 rot)
        {
            NativeFunctionRequestor.RequestFunctionWithArgs("AttachObjectToPlayer", "iiffffff", obj.ID, ID, offset.X, offset.Y, offset.Z, rot.X, rot.Y, rot.Z);
        }

        public bool IsPressingKey(int key)
        {
            if ((Keys & key) == key) return true;
            return false;
        }

        public static bool IsKeyJustPressed(int key, int newkeys, int oldkeys) 
        {
            if ((newkeys & key) == key && (oldkeys & key) != key) return true;
            return false;
        }

        public enum eKeys
        {
            KEY_ACTION = 1,
            PED_ANSWER_PHONE = 1,
            VEHICLE_FIREWEAPON = 1,
            KEY_CROUCH = 2,
            PED_DUCK = 2,
            VEHICLE_HORN = 2,
            KEY_FIRE = 4,
            PED_FIREWEAPON = 4,
            PED_FIREWEAPON_ALT = 4,
            KEY_LMB = 4,
            //VEHICLE_FIREWEAPON = 4,
            VEHICLE_FIREWEAPON_ALT = 4,
            KEY_SPRINT = 8,
            PED_SPRINT = 8,
            KEY_W = 8,
            VEHICLE_ACCELERATE = 8,
            KEY_SECONDARY_ATTACK = 16,
            VEHICLE_ENTER_EXIT = 16,
            KEY_ENTER = 16,
            //VEHICLE_FIREWEAPON_ALT = 16,
            KEY_JUMP = 32,
            PED_JUMPING = 32,
            VEHICLE_BRAKE = 32,
            KEY_VEHICEL_S = 32,
            KEY_LOOK_RIGHT = 64,
            VEHICLE_LOOKRIGHT = 64,
            KEY_HANDBRAKE = 128,
            PED_LOCK_TARGET = 128,
            VEHICLE_HANDBRAKE = 128,
            KEY_AIM = 128,
            KEY_H = 128,
            KEY_LOOK_LEFT = 256,
            VEHICLE_LOOKLEFT = 256,
            KEY_LOOK_BEHIND_VEHICLE = 320, // left + right,
            KEY_SUBMISSION = 512,
            TOGGLE_SUBMISSIONS = 512,
            KEY_LOOK_BEHIND = 512,
            PED_LOOKBEHIND = 512,
            VEHICLE_LOOKBEHIND = 512,
            KEY_WALK = 1024,
            SNEAK_ABOUT = 1024,
            //PED_LOCK_TARGET = 128,
            //PED_LOCK_TARGET = 128,
            KEY_ANALOG_UP = 2048,
            VEHICLE_TURRETUP = 2048,
            KEY_ANALOG_DOWN = 4096,
            VEHICLE_TURRETDOWN = 4096,
            KEY_ANALOG_LEFT = 8192,
            //VEHICLE_LOOKLEFT = 8192,
            VEHICLE_TURRETLEFT = 8192,
            KEY_ANALOG_RIGHT = 16384,
            //VEHICLE_LOOKRIGHT = 16384,
            VEHICLE_TURRETRIGHT = 16384,
            KEY_YES = 65536,
            CONVERSATION_YES = 65536,
            //CONVERSATION_YES = 65536,
            KEY_NO = 131072,
            CONVERSATION_NO = 131072,
            //CONVERSATION_NO = 131072,
            KEY_CTRL_BACK = 262144,
            GROUP_CONTROL_BWD = 262144,
            //GROUP_CONTROL_BWD = 262144,
            //KEY_VEHICLE_HANDBRAKE = 4294967168
            
        }

    }
}
