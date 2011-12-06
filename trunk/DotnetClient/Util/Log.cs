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
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Samp.Util
{
    [Serializable]
    public class Log : MarshalByRefObject
    {
        private static Log CurrentLog;

        public int m_DebugIndent = 0;
        public bool ToFile = true;
        public bool DebugEnabled = true;
        private String Name;
        private String filepath;
        private List<string> buffer;
        public int WriteInterval = 100; // delay in ms between disk writes, to stop potential hdd thrashing


        public static int DebugIndent
        {
            set
            {
                CurrentLog.m_DebugIndent = value;
            }
            get
            {
                return CurrentLog.m_DebugIndent;
            }
        }

        public Log(String name, bool overwrite)
        {
            Name = name;
            this.filepath = Name + ".log";
            CurrentLog = this;
            buffer = new List<string>();

            System.Threading.Thread IOThread = new System.Threading.Thread(BufferProcessor);
            IOThread.IsBackground = true;
            IOThread.Start();
            //System.Timers.Timer IOTimer = new System.Timers.Timer(WriteInterval);
            //IOTimer.Elapsed += new System.Timers.ElapsedEventHandler(BufferProcessor);
            //IOTimer.Start();

            if (ToFile && overwrite)
            {
                StreamWriter sr = null;
                sr = new StreamWriter(filepath, false);
                if (sr != null) sr.Close();
                Message(filepath + " created.");
            }
        }

        public static void Clean(String msg)
        {
            CurrentLog.logImplClean(msg, true);
        }

        public static void Line(String msg) { Log.Message(msg); }
        public static void Message(String msg)
        {
            CurrentLog.logImpl(msg, true);
        }

        public static void Message(String msg, bool tofile)
        {
            CurrentLog.logImpl(msg, tofile);
        }

        public static void Warning(String msg)
        {
            CurrentLog.logImpl(String.Concat("![Warning]! ", msg), true);
        }

        public static void Error(String msg)
        {
            CurrentLog.logImpl(String.Concat("![Error]! ", msg), true);
        }

        public static void Debug(String msg)
        {
            Debug(msg, null);
        }
        public static void Debug(String msg, object sender)
        {
            if (!CurrentLog.DebugEnabled) return;
            for (int i = 0; i < DebugIndent; i++) String.Concat("  ", msg);
            System.Diagnostics.Debug.WriteLine(msg);
            if (sender == null) msg = "[Debug] " + msg;
            else msg = "[Debug] [" + sender.GetType().ToString() + "] " + msg;
            CurrentLog.logImpl(msg, true);
            //CurrentLog.logImpl(String.Concat("[Debug] ", msg), true);
        }

        public static void Exception(System.Exception e)
        {
            StringBuilder sb = new StringBuilder(500);
            sb
            .Append("**[Exception] ")
            .Append(e.GetType().ToString())
            .Append(" ")
            .AppendLine(e.Message)
            .Append(e.StackTrace);

            CurrentLog.logImpl(sb.ToString(), true);

            if (e.InnerException != null)
            {
                CurrentLog.logImpl("*[Info] InnerExpception found!", true);
                Exception(e.InnerException);
            }
        }

        private void logImpl(String content, bool tofile)
        {
            string s = String.Concat(DateTime.Now.ToString("yyyy/MM/dd - HH:mm:ss"), " - ", content);
            CurrentLog.logImplClean(s, tofile);

        }

        private void logImplClean(String content, bool tofile)
        {
            if ((ToFile) && (tofile))
            {
                lock (buffer)
                {
                    buffer.Add(content);
                }
            }
            Console.WriteLine(content);

        }

        private void BufferProcessor()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(WriteInterval);
                if (buffer.Count <= 0) continue;
                StreamWriter sr = null;

                try { sr = new StreamWriter(filepath, true); }
                catch (Exception e) { continue; }
                lock (buffer)
                {
                    foreach (string msg in buffer)
                    {
                        //string s = String.Concat(DateTime.Now.ToString("yyyy/MM/dd - HH:mm:ss"), " - ", msg);
                        try { sr.WriteLine(msg); }
                        catch (Exception e) { continue; }
                    }
                }
                if (sr != null)
                    sr.Close();
                buffer.Clear();
            }
        }
    }
};