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
using System.Text;
using Samp.Client;
using Samp.Util;

namespace Samp.API
{
    public class Vehicle
    {

        public static event EventHandler<OnVehicleCreatedEventArgs> OnVehicleDestroyed;
        public static event EventHandler<OnVehicleCreatedEventArgs> OnVehicleCreated;
        public class OnVehicleCreatedEventArgs : EventArgs
        {
            public Vehicle obj;
            public OnVehicleCreatedEventArgs(Vehicle o)
            {
                obj = o;
            }
        };


        public static Vehicle[] Vehicles = null;
        public static Vehicle GetVehicleByID(int id)
        {
            lock (Vehicles)
            {
                for (int i = 0; i < Vehicles.Count(); i++)
                {
                    if (Vehicles[i] == null) continue;
                    if (Vehicles[i].ID == id) return Vehicles[i];
                }
            }
            Samp.Util.Log.Debug("Vehicle not found, creating new.");
            return new Vehicle(id);
        }
        internal static bool RemoveVehicle(Vehicle v)
        {
            if (OnVehicleDestroyed != null) OnVehicleDestroyed(null, new OnVehicleCreatedEventArgs(v));
            lock (Vehicles)
            {
                for (int i = 0; i < Vehicles.Count(); i++)
                {
                    if (Vehicles[i] == null) continue;
                    if (Vehicles[i]== v) { Samp.Util.Log.Debug("Removing RemoveVehicle."); Vehicles[i] = null; return true; }
                }
            }
            return false;
        }

        internal Vehicle(int id)
        {
            lock (Vehicles)
            {
                ID = id;
                for (int i = 0; i < Vehicles.Count(); i++)
                {
                    if (Vehicles[i] == null) { Vehicles[i] = this; break; }
                }
            }
            if (OnVehicleCreated != null) OnVehicleCreated(this, new OnVehicleCreatedEventArgs(this));
        }


        public static void Init()
        {
            Vehicles = new Vehicle[World.MAX_VEHICLES];
        }

        public static void UnInit()
        {
            Vehicles = null;
        }







        public int ID;
        public int Colour1 = 0;
        public int Colour2 = 0;
        public int RespawnDelay = 0;


        public float Health
        {
            get
            {
                if (ID == -1) return 0;
                FloatRef za = new FloatRef(0.0F);
                NativeFunctionRequestor.RequestFunction("GetVehicleHealth", "iv", ID, za);
                return za.Value;
            }
            set
            {
                if (ID == -1) return;
                NativeFunctionRequestor.RequestFunction("SetVehicleHealth", "if", ID, value);
            }
        }

        private int m_Model=0;
        public int Model
        {
            get
            {
                if (ID == -1) return m_Model;
                return NativeFunctionRequestor.RequestFunction("GetVehicleModel", "i",ID);
            }
            set
            {
                m_Model = value;
            }
        }

        private Vector3 m_Pos = new Vector3();
        public Vector3 Pos
        {
            get
            {
                FloatRef x = new FloatRef(0.0F);
                FloatRef y = new FloatRef(0.0F);
                FloatRef z = new FloatRef(0.0F);
                NativeFunctionRequestor.RequestFunction("GetVehiclePos", "ivvv", ID, x, y, z);
                Vector3 vec = new Vector3(x.Value, y.Value, z.Value);
                return vec;
            }
            set
            {
                NativeFunctionRequestor.RequestFunction("SetVehiclePos", "ifff", ID, value.X, value.Y, value.Z);
            }
        }

        private float m_ZAngle;
        public float ZAngle
        {
            get
            {
                if (ID == -1) return m_ZAngle;
                FloatRef za = new FloatRef(0.0F);
                NativeFunctionRequestor.RequestFunction("GetVehicleZAngle", "iv", ID, za);
                return za.Value;
            }
            set
            {
                if (ID == -1) { m_ZAngle = value; return; }
                NativeFunctionRequestor.RequestFunction("SetVehicleZAngle", "if", ID, value);
            }
        }

        public Vector3 Velocity
        {
            get
            {
                FloatRef x = new FloatRef(0.0F);
                FloatRef y = new FloatRef(0.0F);
                FloatRef z = new FloatRef(0.0F);
                NativeFunctionRequestor.RequestFunction("GetVehicleVelocity", "ivvv", ID, x, y, z);
                Vector3 vec = new Vector3(x.Value, y.Value, z.Value);
                return vec;
            }
            set
            {
                NativeFunctionRequestor.RequestFunction("SetVehicleVelocity", "ifff", ID, value.X, value.Y, value.Z);
            }
        }

        public void RepairVehicle()
        {
            NativeFunctionRequestor.RequestFunction("RepairVehicle", "i", ID);
        }
    }
}
