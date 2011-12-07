
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
using System.IO;
using System.Xml;

namespace Samp.Util
{
    public class DB_XML
    {

        public static void WriteString(string rootnode, string childnode, string element, string str)
        {
            string path = System.Environment.CurrentDirectory;
            string filepath = Path.Combine(path, rootnode + ".xml");

            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                if (System.IO.File.Exists(filepath))
                {
                    try
                    {
                        xmlDoc.Load(filepath);
                    }
                    catch (Exception e)
                    {
                        Log.Exception(e);
                        return;
                    }
                }
                else
                {
                    //if file is not found, create a new xml file
                    XmlTextWriter xmlWriter = new XmlTextWriter(filepath, System.Text.Encoding.UTF8);
                    xmlWriter.Formatting = Formatting.Indented;
                    xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                    xmlWriter.WriteStartElement(rootnode);
                    xmlWriter.Close();
                    xmlDoc.Load(filepath);
                }

                XmlNode root = xmlDoc.SelectSingleNode(rootnode);
                if (root == null) root = xmlDoc.DocumentElement;

                XmlNode childNode = root.SelectSingleNode(childnode);
                if (childNode == null)
                {
                    childNode = xmlDoc.CreateElement(childnode);
                    root.AppendChild(childNode);
                }

                XmlNode childNode2 = childNode.SelectSingleNode(element);
                if (childNode2 == null)
                {
                    childNode2 = xmlDoc.CreateElement(element);
                    childNode.AppendChild(childNode2);
                }

                XmlNode node = xmlDoc.SelectSingleNode(rootnode + "/" + childnode + "/" + element);
                node.InnerText = str;

                xmlDoc.Save(filepath);
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }


        public static string ReadString(string rootnode, string childnode, string element, string defaultval)
        {
            string ret = defaultval;
            string path = System.Environment.CurrentDirectory;
            string filepath = Path.Combine(path, rootnode+".xml");

            try
            {

                XmlDocument xmlDoc = new XmlDocument();

                if (System.IO.File.Exists(filepath))
                {
                    try
                    {
                        xmlDoc.Load(filepath);
                    }
                    catch (Exception e)
                    {
                        Log.Exception(e);
                    }
                }
                else
                {
                    Log.Debug("creating xml file.");
                    WriteString(rootnode, childnode, element, defaultval);
                    return defaultval;
                    /*
                    //if file is not found, create a new xml file
                    XmlTextWriter xmlWriter = new XmlTextWriter(filepath, System.Text.Encoding.UTF8);
                    xmlWriter.Formatting = Formatting.Indented;
                    xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                    xmlWriter.WriteStartElement(rootnode);
                    xmlWriter.Close();
                    xmlDoc.Load(filepath);
                     * */
                }

                //xmlDoc.
                XmlNode root = xmlDoc.SelectSingleNode(rootnode);
                if (root == null)
                {
                    Log.Debug("root == null");
                    root = xmlDoc.DocumentElement;
                }

                XmlNode childNode = root.SelectSingleNode(childnode);
                XmlNode childNode2 = childNode.SelectSingleNode(element);
                if (childNode2 == null)
                {
                    Log.Debug("creating xml data.");
                    WriteString(rootnode, childnode, element, defaultval);
                    return defaultval;
                    //childNode = xmlDoc.CreateElement(childnode);
                    //root.AppendChild(childNode);
                }
                
                //XmlNode childNode2 = childNode.SelectSingleNode(element);
                /*if (childNode2 == null)
                {
                    childNode2 = xmlDoc.CreateElement(element);
                    childNode.AppendChild(childNode2);
                }
                */
                XmlNode node = xmlDoc.SelectSingleNode(rootnode + "/" + childnode + "/" + element);
                ret = node.InnerText;

                //xmlDoc.Save(filepath);
                return ret;

            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootnode"></param>
        /// <param name="childnode"></param>
        /// <param name="element"></param>
        /// <param name="defaultval"></param>
        /// <returns></returns>
        public static bool ReadBool(string rootnode, string childnode, string element, bool defaultval)
        {
            bool retval = defaultval;
            string str = "";
            str = ReadString(rootnode, childnode, element, defaultval.ToString());
            if (!string.IsNullOrEmpty(str))
            {
                retval = Convert.ToBoolean(str);
            }
            return retval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootnode"></param>
        /// <param name="childnode"></param>
        /// <param name="element"></param>
        /// <param name="defaultval"></param>
        /// <returns></returns>
        public static int ReadInt(string rootnode, string childnode, string element, int defaultval)
        {
            int retval = defaultval;
            string str = "";
            str = ReadString(rootnode, childnode, element, defaultval.ToString());
            if (!string.IsNullOrEmpty(str))
            {
                retval = Convert.ToInt32(str);
            }
            return retval;
        }
    }
}
