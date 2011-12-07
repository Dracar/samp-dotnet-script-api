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
    public class GameObject
    {
        public static event EventHandler<OnObjectCreatedEventArgs> OnObjectDestroyed;
        public static event EventHandler<OnObjectCreatedEventArgs> OnObjectCreated;
        public class OnObjectCreatedEventArgs : EventArgs
        {
            public GameObject obj;
            public OnObjectCreatedEventArgs(GameObject o)
            {
                obj = o;
            }
        };

        public static event EventHandler<OnObjectMovedEventArgs> OnObjectMoved;
        public class OnObjectMovedEventArgs : EventArgs
        {
            public GameObject obj;
            public Vector3 NewPos;
            public Vector3 OldPos;
            public Vector3 NewRot;
            public Vector3 OldRot;
            public OnObjectMovedEventArgs(GameObject o, Vector3 newpos, Vector3 oldpos, Vector3 newrot, Vector3 oldrot)
            {
                obj = o;
                NewPos = newpos;
                OldPos = oldpos;
                NewRot = newrot;
                OldRot = oldrot;
            }
        };

        public static GameObject[] Objects = null;
        public static GameObject GetObjectByID(int id)
        {
            lock (Objects)
            {
                for (int i = 0; i < Objects.Count(); i++)
                {
                    if (Objects[i] == null) continue;
                    if (Objects[i].ID == id) return Objects[i];
                }
            }
            Samp.Util.Log.Debug("Objects not found, creating new.");
            return new GameObject(id);
        }
        internal static void RemoveObject(GameObject v)
        {
            if (OnObjectDestroyed != null) OnObjectDestroyed(null, new OnObjectCreatedEventArgs(v));
            lock (Objects)
            {
                for (int i = 0; i < Objects.Count(); i++)
                {
                    if (Objects[i] == null) continue;
                    if (Objects[i]== v) { Samp.Util.Log.Debug("Removing Object."); Objects[i] = null; return; }
                }
            }
        }

        public static void Init()
        {
            Objects = new GameObject[World.MAX_OBJECTS];
        }

        public static void UnInit()
        {
            Objects = null;
        }

        internal GameObject(int id)
        {
            lock (Objects)
            {
                ID = id;
                for (int i = 0; i < Objects.Count(); i++)
                {
                    if (Objects[i] == null) { Objects[i] = this; break; }
                }
            }
            if (OnObjectCreated != null) OnObjectCreated(this, new OnObjectCreatedEventArgs(this));
        }



        public int ID;
        //public bool IsStatic = false; // will be saved to DB
        public int Model;

        public Vector3 Pos
        {
            get
            {
                FloatRef x = new FloatRef(0.0F);
                FloatRef y = new FloatRef(0.0F);
                FloatRef z = new FloatRef(0.0F);
                NativeFunctionRequestor.RequestFunction("GetObjectPos", "ivvv", ID, x, y, z);
                Vector3 vec = new Vector3(x.Value, y.Value, z.Value);
                return vec;
            }
            set
            {
                Vector3 oldpos = Pos;
                Vector3 oldrot = Rot;
                NativeFunctionRequestor.RequestFunction("SetObjectPos", "ifff", ID, value.X, value.Y, value.Z);
                if (OnObjectMoved != null) OnObjectMoved(this,new OnObjectMovedEventArgs(this,value,oldpos,oldrot,oldrot));
            }
        }


        public Vector3 Rot
        {
            get
            {
                FloatRef x = new FloatRef(0.0F);
                FloatRef y = new FloatRef(0.0F);
                FloatRef z = new FloatRef(0.0F);
                NativeFunctionRequestor.RequestFunction("GetObjectRot", "ivvv", ID, x, y, z);
                Vector3 vec = new Vector3(x.Value, y.Value, z.Value);
                return vec;
            }
            set
            {
                Vector3 oldpos = Pos;
                Vector3 oldrot = Rot;
                NativeFunctionRequestor.RequestFunction("SetObjectRot", "ifff", ID, value.X, value.Y, value.Z);
                if (OnObjectMoved != null) OnObjectMoved(this,new OnObjectMovedEventArgs(this,oldpos,oldpos,value,oldrot));
            }
        }
    }
}
