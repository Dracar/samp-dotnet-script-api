/*
 * ReloadScriptsCommand
 * 
 * A very basic script to elevate a players priv level to gm with '/gmlogin <pass>' command, 
 * and reload all dotnet scripts with '/reloadscripts' command.
 * Idealy you would use a proper player accounts system for gm logins instead of something stupid like this.
*/

using System;
using Samp.API;

namespace Samp.Scripts.ExampleScripts
{
    public class ReloadScriptsCommand : ScriptBase
    {
        private const string GMLoginPass = "gmpass";
        public override void OnLoad()
        {
            Player.OnPlayerCommandText += OnPlayerCommandText;
        }

        public override void OnUnload()
        {
            Player.OnPlayerCommandText -= OnPlayerCommandText;
        }

        public static void OnPlayerCommandText(object sender, Player.OnPlayerCommandTextEventArgs args)
        {
            string[] cmd = args.text.Split(' ');

            if (String.Compare(cmd[0], "/gmlogin") == 0)
            {
                if (cmd.Length < 2) return; // no password supplied
                if (String.Compare(cmd[1], GMLoginPass) != 0)
                {
                    args.player.ClientMessage(0, "{FF0000}Incorrect password.");
                    return;
                }
                args.player.ClientMessage(0, "{00FF00}GM login successful.");
                args.player.PrivLevel = (int)Player.PRIV_LEVELS.GM;
                return;
            }

            if (ScriptManager.Instance == null) return; // scriptmanager is a singleton
            if (String.Compare(cmd[0], "/reloadscripts") == 0)
            {
                if (args.player.PrivLevel < (int)Player.PRIV_LEVELS.GM)
                {
                    args.player.ClientMessage(0, "{FF0000}Insufficient privlevel to reload scripts.");
                    return;
                }
                args.player.ClientMessage(0,"{00FF00}Reloading All Scripts.");
                ScriptManager.Instance.ReloadAllScripts();
            }
        }
    }
}
