﻿/*
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

namespace Samp.API
{
    public class Menu
    {
        /*
        public static Menu GetMenuByID(int id)
        {
            lock (World.Menus)
            {
                for (int i = 0; i < World.Menus.Count(); i++)
                {
                    if (World.Menus[i] == null) continue;
                    if (World.Menus[i].ID == id) return World.Menus[i];
                }
                return null;// new Menu(id, "");
            }
        }

        public static Menu GetMenuByName(string name)
        {
            lock (World.Menus)
            {
                for (int i = 0; i < World.Menus.Count(); i++)
                {
                    if (World.Menus[i] == null) continue;
                    if (String.Compare(World.Menus[i].Name, name) == 0) return World.Menus[i];
                }
                return null;
            }
        }

        public Menu(int id,string name)
        {
            ID = id;
            Name = name;
        }
        public int ID;
        public string Name;

        public void AddMenuItem(int column, string title)
        {
            NativeFunction func = new NativeFunction();
            func.name = "AddMenuItem";
            func.args = "iis";
            func.data.AddInt32(ID);
            func.data.AddInt32(column);
            func.data.AddString(title);
            NativeFunctionRequestor fr = new NativeFunctionRequestor(Client.Client.Instance);
            fr.RequestFunctionWithArgs(Server.Instance, func);
        }

        public void ShowMenuForPlayer(Player player)
        {
            NativeFunction func = new NativeFunction();
            func.name = "ShowMenuForPlayer";
            func.args = "ii";
            func.data.AddInt32(ID);
            func.data.AddInt32(player.ID);
            NativeFunctionRequestor fr = new NativeFunctionRequestor(Client.Client.Instance);
            fr.RequestFunctionWithArgs(Server.Instance, func);
        }
         * */
    }
}
