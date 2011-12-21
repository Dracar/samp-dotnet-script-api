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
    public class CallbackProcessor
    {
        /*
        public struct Callback
        {
            public int Opcode;
            public string Name;
            public string Paramtypes;
            public Callback(int oc, string n, string p)
            {
                Opcode = oc;
                Name = n;
                Paramtypes = p;
            }
        };

        public static Callback[] Callbacks =
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

        public static Callback GetCallbackById(int id)
        {
            for (int i = 0; i < Callbacks.Length; i++)
            {
                if (Callbacks[i].Opcode == id) return Callbacks[i];
            }
            return Callbacks[0];
        }*/
        /*
        public static void ProcessCallback(byte callbackid, string paramtypes, byte[] data)
        {
            if (callbackid == 8) Samp.Util.Log.Debug("CalbackProcessor.cs OnPlayerCommandText");
            DataStream sdata = new DataStream();
            sdata.Data = data;
            System.Text.UTF8Encoding  encoding=new System.Text.UTF8Encoding();
            byte[] bparamtypes = encoding.GetBytes(paramtypes);
            object[] p = new object[10];
            for (int i = 0; i < bparamtypes.Length; i++)
            {
                if (bparamtypes[i] == 0) break;

                if (bparamtypes[i] == 'i') p[i] = sdata.ReadInt32();
                if (bparamtypes[i] == 'f') p[i] = sdata.ReadFloat32();
                if (bparamtypes[i] == 's') p[i] = sdata.ReadString();

            }

            /*
            if (callbackid == 9)
            { // OnPlayerConnect
                Int32 param0_i = (Int32)p[0];
                //Int32 param1_i = (Int32)p[1];
                //Int32 param2_i = (Int32)p[2];
                InternalEvents.FireOnPlayerConnect(null,new OnPlayerConnectEventArgs(param0_i));
            }
            *

        }*/
    }
}
