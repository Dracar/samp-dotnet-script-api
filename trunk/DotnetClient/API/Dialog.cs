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

namespace Samp.API
{
    public class Dialog
    { // I was tempted to call this class Dialogue, but whilst i would never use dialog in a conversation sense, i have come to associate dialog as a computer dialog box, so Dialog it is.
        
        public static int idinc = 0; // hmm, this should prolly be a uint...
        /*
        public static Menu GetDialogByID(int id)
        {
            for (int i = 0; i < World.Menus.Count(); i++)
            {
                if (World.Dialogs[i] == null) continue;
                if (World.Dialogs[i].ID == id) return World.Dialogs[i];
            }
            return null;// new Menu(id, "");
        }

        public static Menu GetDialogByName(string name)
        {
            for (int i = 0; i < World.Dialogs.Count(); i++)
            {
                if (World.Dialogs[i] == null) continue;
                if (String.Compare(World.Dialogs[i].Name, name) == 0) return World.Dialogs[i];
            }
            return null;
        }
        */

        public static void Init()
        {
            InternalEvents.OnCallbackReceived += OnCallbackReceived;
        }

        public static void UnInit()
        {
            InternalEvents.OnCallbackReceived -= OnCallbackReceived;
        }

        public static event EventHandler<OnDialogResponseEventArgs> OnDialogResponse;
        public class OnDialogResponseEventArgs : EventArgs
        {
            public Player player;
            public Dialog dialog;
            public int response;
            public int listitem;
            public string inputtext;
            public OnDialogResponseEventArgs(Player pl, Dialog d,int respons, int item, string input)
            {
                player = pl;
                dialog = d;
                response = respons;
                listitem = item;
                inputtext = input;
            }
        };


        public static void OnCallbackReceived(object sender, OnCallbackReceivedEventArgs args)
        {
            args.Data.Pos = 0;
            if (String.Compare(args.CallbackName, "OnDialogResponse") == 0)
            {
                Samp.Util.Log.Debug("OnDialogResponse");
                int playerid = args.Data.ReadInt32();
                int dialogid = args.Data.ReadInt32();
                int response = args.Data.ReadInt32();
                int listitem = args.Data.ReadInt32();
                string inputtext = args.Data.ReadString();
                Samp.API.Player player = Player.GetPlayerByID(playerid);
                Samp.API.Dialog dialog = player.dialog;
                player.dialog = null;
                if (OnDialogResponse != null) OnDialogResponse(null, new OnDialogResponseEventArgs(player, dialog, response, listitem, inputtext));

            }
        }


        public Dialog()
        {
            ID = idinc++;
            Name = "Dialog";
            Style = 0;
            Info = "";
            Button1 = "Yes";
            Button2 = "No";
        }
        public int ID;
        public string Name;
        public int Style;
        public string Info;
        public string Button1;
        public string Button2;



        public void ShowDialogForPlayer(Player pl)
        {
            pl.ShowDialog(this);
        }
    }
}
