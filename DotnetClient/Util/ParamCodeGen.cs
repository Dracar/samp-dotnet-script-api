/*
 * Iain Gilbert
 * 2011
 *
*/

using System;

namespace Samp.Util
{
    public class ParamCodeGen
    {

        public struct Callback
        {
            public int opcode;
            public string name;
            public string paramtypes;
            public Callback(int oc, string n, string p)
            {
                opcode = oc;
                name = n;
                paramtypes = p;
            }
        };

        Callback[] Callbacks =
        {
	        new Callback(0,"NULL",""),
	        new Callback(1,"OnEnterExitModShop","iii"),
	        new Callback(2,"OnFilterScriptExit",""),
	        new Callback(3,"OnFilterScriptInit",""),
	        new Callback(4,"OnGameModeExit",""),
	        new Callback(5,"OnGameModeInit",""),
	        new Callback(6,"OnObjectMoved","i"),
	        new Callback(7,"OnPlayerClickPlayer","iii"),
	        new Callback(8,"OnPlayerCommandText","is"),
	        new Callback(9,"OnPlayerConnect","i"),
	        new Callback(10,"OnPlayerDeath","iii"),
	        new Callback(11,"OnPlayerDisconnect","ii"),
	        new Callback(12,"OnPlayerEnterCheckpoint","i"),
	        new Callback(13,"OnPlayerEnterRaceCheckpoint","i"),
	        new Callback(14,"OnPlayerEnterVehicle","iii"),
	        new Callback(15,"OnPlayerExitVehicle","ii"),
	        new Callback(16,"OnPlayerExitedMenu","i"),
	        new Callback(17,"OnPlayerGiveDamage","iifi"),
	        new Callback(18,"OnPlayerInteriorChange","iii"),
	        new Callback(19,"OnPlayerKeyStateChange","iii"),
	        new Callback(20,"OnPlayerLeaveCheckpoint","i"),
	        new Callback(21,"OnPlayerLeaveRaceCheckpoint","i"),
	        new Callback(22,"OnPlayerObjectMoved","ii"),
	        new Callback(23,"OnPlayerPickUpPickup","ii"),
	        new Callback(24,"OnPlayerPrivmsg","iis"),
	        new Callback(25,"OnPlayerRequestClass","ii"),
	        new Callback(26,"OnPlayerRequestSpawn","i"),
	        new Callback(27,"OnPlayerSelectedMenuRow","ii"),
	        new Callback(28,"OnPlayerSpawn","i"),
	        new Callback(29,"OnPlayerStateChange","iii"),
	        new Callback(30,"OnPlayerStreamIn","ii"),
	        new Callback(31,"OnPlayerStreamOut","ii"),
	        new Callback(32,"OnPlayerTakeDamage","iifi"),
	        new Callback(33,"OnPlayerTakeDamageRU","iifi"),
	        new Callback(34,"OnPlayerTeamPrivmsg","is"),
	        new Callback(35,"OnPlayerText","is"),
	        new Callback(36,"OnPlayerUpdate","i"),
	        new Callback(37,"OnRconCommand","s"),
	        new Callback(38,"OnRconLoginAttempt","ssi"),
	        new Callback(39,"OnUnoccupiedVehicleUpdate","iii"), // Important Note: This callback is called very frequently per second per unoccupied vehicle.
	        new Callback(40,"OnVehicleDamageStatusUpdate","ii"),
	        new Callback(41,"OnVehicleDeath","i"),
	        new Callback(42,"OnVehicleMod","iii"),
	        new Callback(43,"OnVehiclePaintjob","iii"),
	        new Callback(44,"OnVehicleRespray","iiii"),
	        new Callback(45,"OnVehicleSpawn","i"),
	        new Callback(46,"OnVehicleStreamIn","ii"),
	        new Callback(47,"OnVehicleStreamOut","ii"),
	        new Callback(48,"OnDialogResponse","iiiis")
        };
        /*
         * else if (opcode == GetCallbackByName("OnPlayerCommandText")->opcode)
	{
		int playerid = ReadInt32FromPacket(packet,&packetpos);
		char* cmd = ReadStringFromPacket(packet,&packetpos);
		logprintf("Player command! %d: %s",playerid,cmd);
		SAMP::InternalEvents::FirePlayerCommandText(playerid,cmd);
	}
         * */
        public void PrintParamDotnetHookBaseCode()
        {
            for (int i = 0; i < Callbacks.Length; i++)
            {
                char[] paramtypes = Callbacks[i].paramtypes.ToCharArray();
                string line = "else if (opcode == GetCallbackByName(\"" + Callbacks[i].name + "\")->opcode)";
                Log.Clean(line);
                line = "{";
                Log.Clean(line);
                for (int typeid = 0; typeid < paramtypes.Length; typeid++)
                {
                    if (paramtypes[typeid] == 'i')
                    {
                        line = "    int " + "param" + typeid + "_" + paramtypes[typeid].ToString() + " = ReadInt32FromPacket(packet,&packetpos);";
                    }
                    else if (paramtypes[typeid] == 'f')
                    {
                        line = "    float " + "param" + typeid + "_" + paramtypes[typeid].ToString() + " = ReadFloat32FromPacket(packet,&packetpos);";
                    }
                    else if (paramtypes[typeid] == 's')
                    {
                        line = "    string " + "param" + typeid + "_" + paramtypes[typeid].ToString() + " = ReadStringFromPacket(packet,&packetpos);";
                    }
                    Log.Clean(line);


                }
                line = "    SAMP::InternalEvents::Fire" + Callbacks[i].name + "(";
                for (int typeid = 0; typeid < paramtypes.Length; typeid++)
                {
                    line += "param" + typeid + "_" + paramtypes[typeid].ToString() + ", ";
                }
                line += ");";
                line = line.Replace(", )", ")");
                Log.Clean(line);
                line = "}";
                Log.Clean(line);

            }
        }

        public void PrintParamDotnetHookEventCode()
        {
            for (int i = 0; i < Callbacks.Length; i++)
            {
                char[] paramtypes = Callbacks[i].paramtypes.ToCharArray();
                //static event EventHandler<PlayerCommandTextEventArgs^>^ PlayerCommandText; // fired on player update
                //static void FirePlayerCommandText(int playerid,std::string cmd){PlayerCommandText(nullptr,gcnew PlayerCommandTextEventArgs(playerid, gcnew String(cmd.c_str())));}
                string line = "static event EventHandler<" + Callbacks[i].name + "EventArgs^>^ " + Callbacks[i].name + ";";
                Log.Clean(line);
                line = "static void Fire" + Callbacks[i].name + "(";
                for (int typeid = 0; typeid < paramtypes.Length; typeid++)
                {
                    if (paramtypes[typeid] == 'i')
                    {
                        line += " int ";
                    }
                    else if (paramtypes[typeid] == 'f')
                    {
                        line += " float ";
                    }
                    else if (paramtypes[typeid] == 's')
                    {
                        line += " std::string ";
                    }
                    line += "param" + typeid + "_" + paramtypes[typeid].ToString() + ", ";
                }
                line += ") ";
                line = line.Replace(", )", ")");
                line += "{" + Callbacks[i].name + "(nullptr, gcnew " + Callbacks[i].name + "EventArgs(";
                for (int typeid = 0; typeid < paramtypes.Length; typeid++)
                {
                    if (paramtypes[typeid] == 's') line += "gcnew String(" + "param" + typeid + "_" + paramtypes[typeid].ToString() + ".c_str()), ";
                    else line += "param" + typeid + "_" + paramtypes[typeid].ToString() + ", ";
                }
                line += ") ";
                line = line.Replace(", )", ")");
                line += ");}";
                Log.Clean(line);
                Log.Clean("");
            }
        }

        public void PrintParamDotnetHookEventArgsCode()
        {
            for (int i = 0; i < Callbacks.Length; i++)
            {
                char[] paramtypes = Callbacks[i].paramtypes.ToCharArray();

                /*
                public ref class PlayerCommandTextEventArgs: EventArgs
	            {
	            public:
		            int PlayerID;
		            System::String^ Command;
		            PlayerCommandTextEventArgs(int playerid, System::String^ cmd)
		            {
			            PlayerID = playerid;
			            Command = cmd;
		            }
	            };
                 * */

                string line = "public ref class " + Callbacks[i].name + "EventArgs: EventArgs";
                Log.Clean(line);
                line = "{";
                Log.Clean(line);
                line = " public:";
                Log.Clean(line);
                for (int typeid = 0; typeid < paramtypes.Length; typeid++)
                {
                    if (paramtypes[typeid] == 'i')
                    {
                        line = "    int " + "Param" + typeid + "_" + paramtypes[typeid].ToString() + ";";
                    }
                    else if (paramtypes[typeid] == 'f')
                    {
                        line = "    float " + "Param" + typeid + "_" + paramtypes[typeid].ToString() + ";";
                    }
                    else if (paramtypes[typeid] == 's')
                    {
                        line = "    System::String^ " + "Param" + typeid + "_" + paramtypes[typeid].ToString() + ";";
                    }
                    Log.Clean(line);
                }
                line = "    " + Callbacks[i].name + "EventArgs(";
                for (int typeid = 0; typeid < paramtypes.Length; typeid++)
                {
                    if (paramtypes[typeid] == 'i')
                    {
                        line += " int " + "param" + typeid + "_" + paramtypes[typeid].ToString() + ", ";
                    }
                    else if (paramtypes[typeid] == 'f')
                    {
                        line += " float " + "param" + typeid + "_" + paramtypes[typeid].ToString() + ", ";
                    }
                    else if (paramtypes[typeid] == 's')
                    {
                        line += " System::String^ " + "param" + typeid + "_" + paramtypes[typeid].ToString() + ", ";
                    }
                }

                line += ")";
                line = line.Replace(", )", ")");
                Log.Clean(line);
                line = "    {";
                Log.Clean(line);


                for (int typeid = 0; typeid < paramtypes.Length; typeid++)
                {
                    line = "        Param" + typeid + "_" + paramtypes[typeid].ToString() + " = " + "param" + typeid + "_" + paramtypes[typeid].ToString() + ";";
                    Log.Clean(line);
                }
                line = "   }";
                Log.Clean(line);
                line = "};";
                Log.Clean(line);

            }
        }

        public void PrintPawnParamCode()
        {
            for (int i = 0; i < Callbacks.Length; i++)
            {
                char[] paramtypes = Callbacks[i].paramtypes.ToCharArray();

                string line = "public " + Callbacks[i].name + "(";
                for (int typeid = 0; typeid < paramtypes.Length; typeid++)
                {
                    line += "param" + typeid + "_" + paramtypes[typeid].ToString() + ", ";
                }
                //line.Remove(line.Length - 2);
                line += ")";
                line = line.Replace(", )", ")");
                Log.Clean(line);
                line = "{";
                Log.Clean(line);
                line = "    ClearCallbackPacket();";
                Log.Clean(line);
                line = "    new callbackid = Callbacks[GetCallbackByName(\"";
                line += Callbacks[i].name;
                line += "\")][opcode];";
                Log.Clean(line);
                line = "    AddInt32ToCallbackPacket(callbackid);";
                Log.Clean(line);
                line = "    AddStringToCallbackPacket(Callbacks[GetCallbackById(callbackid)][paramtypes]);";
                Log.Clean(line);
                
                for (int typeid = 0; typeid < Callbacks[i].paramtypes.Length; typeid++)
                {
                    if (paramtypes[typeid] == 'i')
                    {
                        line = "    AddInt32ToCallbackPacket(";
                    }
                    if (paramtypes[typeid] == 'f')
                    {
                        line = "    AddFloat32ToCallbackPacket(";
                    }
                    else if (paramtypes[typeid] == 's')
                    {
                        line = "    AddStringToCallbackPacket(";
                    }
                    line += "param" + typeid + "_" + paramtypes[typeid].ToString() + ");";
                    Log.Clean(line);
                }
                line = "    SendCallbackPacket();";
                Log.Clean(line);
                line = "    return 1;";
                Log.Clean(line);
                line = "}";
                Log.Clean(line);

            }

        }

    }
}
