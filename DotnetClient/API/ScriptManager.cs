
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
using System.Reflection;
//#if _WIN32
using Microsoft.VisualBasic;
using Microsoft.CSharp;
//#else
//using Mono.CSharp;
//using Mono.VisualBasic;
//#endif
using System.CodeDom.Compiler;
using Samp.Scripts;
using Samp.Util;

namespace Samp.API
{
    public class ScriptManager
    {
        public static event EventHandler<ScriptEventArgs> OnScriptLoad;
        public static event EventHandler<ScriptEventArgs> OnScriptUnload;

        public class ScriptEventArgs : EventArgs
        {
            public ScriptBase Script;
            public ScriptEventArgs(ScriptBase s)
            {
                Script = s;
            }
        }


        public static ScriptManager Instance;
        public static string CoreAsmLocation; // location of WiimoteScript main assembly
        public string GameDirectory; // directory that coreasm is loaded from
        public string ScriptFileDirectory; // directory to search in for scripts (/scripts)
        public string[] ScriptFilePattern; // filename patter to search for scripts (*.Net.dll)
        public System.Collections.Generic.List<ScriptBase> RunningScripts; // list of our running 



        public void LoadAllScripts()
        {
            try
            {
                CoreAsmLocation = Assembly.GetExecutingAssembly().CodeBase;//coreasmloc;
                //AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(currentDomain_AssemblyResolve);
                //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(currentDomain_UnhandledException);
                //AppDomain.CurrentDomain.DomainUnload += new System.EventHandler(currentDomain_DomainUnload);
                GameDirectory = System.Environment.CurrentDirectory;
                ScriptFileDirectory = Path.Combine(GameDirectory , "Scripts");
                ScriptFilePattern = new string[] { "*.dll", "*.cs", "*.vb" };
                RunningScripts = new System.Collections.Generic.List<ScriptBase>();
                //System.AppDomain currentDomain = AppDomain.CurrentDomain;
                Instance = this;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }

            FindScriptAssemblies();
        }
        public int FindScriptAssemblies()
        { // searches scripts directory for files to load
            //try
            int count = 0;
            try
            {
                Log.Message("Searching for script assemblies!");
                if (!Directory.Exists(ScriptFileDirectory))
                {
                    Log.Debug("Create scripts folder!", this);
                    Directory.CreateDirectory(ScriptFileDirectory);
                    return 0;
                }

                string[] files = null;// = new string[0];
                for (int i = 0; i < ScriptFilePattern.Length; i++)
                {
                    string[] tmp = Directory.GetFiles(ScriptFileDirectory, ScriptFilePattern[i], SearchOption.AllDirectories);
                    if (files == null) { files = tmp; continue; }

                    string[] tmpfiles = new string[files.Length + tmp.Length]; // new array to fit all
                    for (int q = 0; q < files.Length; q++) { tmpfiles[q] = files[q]; } // add old files
                    for (int q = files.Length; q < files.Length + tmp.Length; q++) { tmpfiles[q] = tmp[q - files.Length]; } // add new files
                    files = tmpfiles;
                }

                if (files == null) return 0;
                Log.Debug("Found " + files.Length + " files in scripts directory.", this);
                for (int i = 0; i < files.Length; i++)
                {
                    Log.Debug("Searching scripts in assembly: " + Util.Util.GetFilenameFromPath(files[i]), this);
                    count += FindScriptsInAssembly(files[i]); // search for the script classes in assembly, then load them
                    Log.Debug("Loaded " + count + " scripts from assembly: " + Util.Util.GetFilenameFromPath(files[i]), this);

                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
            Log.Message("Load scripts complete! Loaded " + count + " scripts in total.");
            return count;
        }

        public Assembly CompileScript(string csfile)
        {
            Log.Message("Compiling file: " + Util.Util.GetFilenameFromPath(csfile));
            CodeDomProvider csCompiler = null;// = new CSharpCodeProvider();
            if (csfile.Contains(".vb")) csCompiler = new VBCodeProvider(); // todo: fix
            else csCompiler = new CSharpCodeProvider();
            //CSharpCodeProvider csCompiler = new CSharpCodeProvider();
            //ICodeCompiler iCodeCompiler = csCompiler.CreateCompiler();

            // input params for the compiler
            CompilerParameters compilerParams = new CompilerParameters();
            //compilerParams.OutputAssembly = csfile+".dll";
            compilerParams.ReferencedAssemblies.Add("system.dll");

            string asmloc = Assembly.GetExecutingAssembly().Location;
            asmloc = asmloc.Substring(0, asmloc.LastIndexOf("\\") + 1); // todo: fix for linux
            Log.Debug(asmloc, this);
            compilerParams.ReferencedAssemblies.Add(asmloc + "DotnetClient.exe");
            //compilerParams.ReferencedAssemblies.Add(asmloc + "WiimoteScript.exe");
            //compilerParams.ReferencedAssemblies.Add(asmloc + "WiimoteLib.dll");
            //compilerParams.ReferencedAssemblies.Add(asmloc + "PPJoyWrapper.dll");

            // generate the DLL
            compilerParams.GenerateExecutable = false;

            // Run the compiler and build the assembly
            //CompilerResults result = iCodeCompiler.CompileAssemblyFromFile(compilerParams,csfile);
            string[] q = new string[1];
            q[0] = csfile;
            CompilerResults result = csCompiler.CompileAssemblyFromFile(compilerParams, q);

            //if (result == null) Log
            int ecount = result.Errors.Count;
            if (ecount > 0) Log.Warning("Compile script " + Util.Util.GetFilenameFromPath(csfile) + " failed!");
            for (int i = 0; i < ecount; i++) // why the fuck is VS flagging this as unreachable code?!?!!ELEVENTYONE!1!
            {
                if (result.Errors[i] == null) continue;
                //string msg = result.Errors[0].ErrorText;
                Log.Warning(result.Errors[i].ToString());
                return null;
            }

            Log.Debug(Util.Util.GetFilenameFromPath(csfile) + " compiled successfuly", this);
            //return true;
            return result.CompiledAssembly;
        }

        public int FindScriptsInAssembly(string assemblypath)
        { // Find all of Script classes in DLL file


            int count = 0;
            try
            {
                Assembly asm = null;
                if (assemblypath.Contains(".cs") || assemblypath.Contains(".vb")) // todo: fix
                {
                    asm = CompileScript(assemblypath);
                    if (asm == null) return 0;
                    //assemblypath = assemblypath.Replace(".cs", ".cs.dll");
                }
                else
                {
                    try
                    {
                        Log.Debug("Loading Script Assembly: " + Util.Util.GetFilenameFromPath(assemblypath), this);
                        asm = Assembly.Load(File.ReadAllBytes(assemblypath));
                    }
                    catch (Exception e)
                    {
                        Log.Exception(e);
                        return 0;
                    }
                }
                Log.Debug("Searching types in assembly: " + Util.Util.GetFilenameFromPath(assemblypath), this);
                Type[] types = null;
                try
                {
                    types = asm.GetTypes(); // get all class types in dll
                }
                catch (ReflectionTypeLoadException ex)
                { // prolly not a script of ours then
                    Log.Exception(ex);
                    for (int i = 0; i < ex.LoaderExceptions.Length; i++)
                    {
                        System.Exception[] e = ex.LoaderExceptions;
                        Log.Debug("--LoaderExceptions: " + e[i].Message, this);
                    }
                    //types = ex->Types; // debug
                    return 0;
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                }
                Log.Debug("Found " + types.Length + " types in assembly: " + Util.Util.GetFilenameFromPath(assemblypath), this);
                for (int i = 0; i < types.Length; i++)
                {
                    Log.Debug("Type found: " + types[i].Name, this);
                }
                for (int i = 0; i < types.Length; i++)
                {
                    try
                    {
                        //if (types[i]->IsAssignableFrom(ScriptBase::typeid)) 
                        if (types[i].IsSubclassOf(typeof(ScriptBase))) //typeid
                        { // if is typeof Script
                            Log.Debug("Found Script: " + types[i].Name + " in assembly " + Util.Util.GetFilenameFromPath(assemblypath), this);
                            RegisterScript(types[i]); // register the script thread
                            count++;
                        }
                        else
                        {
                            Log.Debug("Not a script! script: " + types[i].FullName + ", namespace: " + types[i].Namespace, this);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Exception(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
            return count;
        }
        public ScriptBase RegisterScript(Type scriptclass)
        { // register the script, create instance
            try
            {
                Log.Message("Loading Script: " + scriptclass.FullName);
                object obj = Activator.CreateInstance(scriptclass);
                ScriptBase scr = (ScriptBase)(obj);
                RunningScripts.Add(scr);
                Log.Debug("Script " + scr.GetType().FullName + " started.", this);
                scr.OnLoad();
                if (OnScriptLoad != null) OnScriptLoad(this, new ScriptEventArgs(scr));
                return scr;
            }
            catch (Exception e)
            {
                Log.Exception(e);
                return null;
            }
        }
        public void ReloadAllScripts()
        { 
            UnloadAllScripts();
            LoadAllScripts();

        }
        public void UnloadAllScripts()
        {
            Log.Debug("ScriptManager.UnloadAllScripts", this);
            for (int i = 0; i < RunningScripts.Count; i++)
            {
                if (RunningScripts[i] == null) continue;
                RunningScripts[i].OnUnload();
                if (OnScriptUnload != null) OnScriptUnload(this, new ScriptEventArgs(RunningScripts[i]));
                RunningScripts[i] = null;
            }
            //if (CleanUp != null) CleanUp(this, new EventArgs());
        }
        public void UnloadScript(ScriptBase script)
        {
            script.OnUnload();
            if (OnScriptUnload != null) OnScriptUnload(this, new ScriptEventArgs(script));
            for (int i = 0; i < RunningScripts.Count;i++ )
            {
                if (RunningScripts[i] == null) continue;
                if (RunningScripts[i] == script)
                {
                    RunningScripts[i] = null;
                    break;
                }
            }
            script = null;
        }

    }
}
