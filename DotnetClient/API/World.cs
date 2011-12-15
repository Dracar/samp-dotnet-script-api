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
using System.Collections.Generic;
using System.Linq;
using Samp.Client;

namespace Samp.API
{
    public class World
    {
        public const int SaveTime = 60 * 1000; // world save every 60 seconds

        public static event EventHandler<OnWorldSaveEventArgs> OnWorldUnload;
        public static event EventHandler<OnWorldSaveEventArgs> OnWorldLoad;
        public static event EventHandler<OnWorldSaveEventArgs> OnWorldSave;
        public class OnWorldSaveEventArgs : EventArgs
        {
            public OnWorldSaveEventArgs()
            {
            }
        };

        // public static World Instance;
        public static int MAX_PLAYERS = 500;
        public static int MAX_MENUS = 127;
        public static int MAX_VEHICLES = 1023;
        public static int MAX_OBJECTS = 2047;

        public static System.Threading.Thread tSaveThread;
        public static bool IsActive = false;
        public static void Init()
        {
            //Menus = new Menu[MAX_MENUS];
            IsActive = true;
            tSaveThread = new System.Threading.Thread(SaveThread);
            tSaveThread.Start();

            if (OnWorldLoad != null) OnWorldLoad(null, new OnWorldSaveEventArgs());
        }

        public static void UnInit()
        {
            IsActive = false;
            if (OnWorldUnload != null) OnWorldUnload(null, new OnWorldSaveEventArgs());
        }

        static DateTime lastsave;
        public static void SaveThread()
        {
            while (IsActive)
            {
                if (Samp.Client.Server.Instance == null) break;
                if (!Samp.Client.Server.Instance.IsConnected) break;

                System.Threading.Thread.Sleep(SaveTime);
                lastsave = DateTime.Now;
                Util.Log.Message("Saving world data.");
                if (OnWorldSave != null) OnWorldSave(null, new OnWorldSaveEventArgs());
                TimeSpan ts = DateTime.Now - lastsave;
                float differenceInMs = (float)ts.TotalMilliseconds;
                Util.Log.Message("World save completed in " + Math.Round((float)(differenceInMs / 1000), 1).ToString() + " seconds.");
            }

        }
        public static Player[] Players
        {
            get
            {
                return Player.Players;
            }
        }
        public static Player GetPlayerById(int id)
        {
            return Player.GetPlayerByID(id);
        }

        public static Vehicle CreateVehicle(int modelid, Vector3 pos, float angle, int color1, int color2, int respawn_delay)
        {
            int id = NativeFunctionRequestor.RequestFunction("CreateVehicle", "iffffiii", modelid, pos.X, pos.Y, pos.Z, angle, color1, color2, respawn_delay);
            return Vehicle.GetVehicleByID(id);
        }

        public static void DestroyVehicle(Vehicle vehicle)
        {
            if (Vehicle.RemoveVehicle(vehicle)) NativeFunctionRequestor.RequestFunction("DestroyVehicle", "i", vehicle.ID);
        }

        public static GameObject CreateObject(int modelid, Vector3 pos, Vector3 rot, float drawdistance)
        {
            int id = NativeFunctionRequestor.RequestFunction("CreateObject", "ifffffff", modelid, pos.X, pos.Y, pos.Z, rot.X, rot.Y, rot.Z, drawdistance);
            GameObject obj = GameObject.GetObjectByID(id);
            obj.Model = modelid;
            return obj;
        }

        public static void DestroyObject(GameObject obj)
        {
            if (GameObject.RemoveObject(obj)) NativeFunctionRequestor.RequestFunction("DestroyObject", "i", obj.ID);
        }

        public static GameObject[] GetObjectsInArea(Vector3 wpos, float distance)
        { // todo: interior? virtulworld?
            List<GameObject> objs = new List<GameObject>();
            lock (GameObject.Objects)
            {
                for (int i = 0; i < GameObject.Objects.Length; i++)
                {
                    if (GameObject.Objects[i] == null) continue;
                    Vector3 opos = GameObject.Objects[i].Pos;
                    Util.Log.Debug("objdist: " + wpos.Distance(opos));
                    if (wpos.Distance(opos) > 20.0F) continue;
                    objs.Add(GameObject.Objects[i]);
                }
            }
            GameObject[] retobjs = new GameObject[objs.Count()];
            for (int i=0;i<objs.Count();i++)
            {
                retobjs[i] = objs[i];
            }
            return retobjs;
        }

        /*
        public static Menu[] Menus;// = new Menu[128];
        public static Menu CreateMenu(string title, int columns, Vector3 pos, Vector3 size)
        {
            NativeFunction func = new NativeFunction();
            func.name = "CreateMenu";
            func.args = "siffff";
            func.data.AddString(title);
            func.data.AddInt32(columns);
            func.data.AddFloat32(pos.X);
            func.data.AddFloat32(pos.Y);
            func.data.AddFloat32(size.X);
            func.data.AddFloat32(size.Y);
            NativeFunctionRequestor fr = new NativeFunctionRequestor(Client.Client.Instance);
            func = fr.RequestFunction(Server.Instance, func);
            Samp.Util.Log.Debug("Menu created: " + func.response);
            Menu m = Menu.GetMenuByID(func.response);
            if (m == null) m = new Menu(func.response, title);
            lock (Menus)
            {
                for (int i = 0; i < Menus.Count(); i++)
                {
                    if (Menus[i] == null) { Menus[i] = m; break; }
                }
            }
            return m;
        }

        public static void DestroyMenu(Menu menu)
        {
            NativeFunction func = new NativeFunction();
            func.name = "DestroyMenu";
            func.args = "i";
            func.data.AddInt32(menu.ID);
            NativeFunctionRequestor fr = new NativeFunctionRequestor(Client.Client.Instance);
            fr.RequestFunction(Server.Instance, func);
            lock (Menus)
            {
                for (int i = 0; i < Menus.Count(); i++)
                {
                    if (Menus[i] == null) continue;
                    if (Menus[i].ID == menu.ID) Menus[i] = null;
                }
            }
        }
        */

    }
}
