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
using Samp.Util;

namespace Samp.Client
{
    public class DataStream
    {
        public const int MAX_PACKET = 1024; 
        public ushort Length = 0;
        public ushort Pos = 0;
        public byte[] Data = new byte[MAX_PACKET];

        public void AddData(byte[] data,int length)
        {
            for (int i = 0; i < length; i++)
            {
                if (i >= data.Length) Data[Pos] = 0;
                else Data[Pos] = data[i];
                Pos++;
                Length++;
            }
        }


        public void AddByte(byte b)
        {
            Data[Pos] = b;
            Pos += 1;
            Length += 1;
        }

        public void AddUShort(ushort s)
        {
            byte[] bar = BitConverter.GetBytes(s);
            for (int i = 0; i < bar.Length; i++)
            {
                Data[Pos] = bar[i];
                Pos++;
                Length++;
            }
        }


        public void AddInt32(Int32 int32)
        {
            byte[] fb = BitConverter.GetBytes(int32);
            for (int i = 0; i < 4; i++) Data[Pos + i] = fb[i];
            Pos += 4;
            Length += 4;
        }

        public void AddFloat32(float f)
        {
            byte[] fb = BitConverter.GetBytes(f);
            for (int i = 0; i < 4; i++) Data[Pos + i] = fb[i];
            Pos += 4;
            Length += 4;
        }

        public static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] s = encoding.GetBytes(str);
            byte[] r = new byte[s.Length + 1]; // null terminator
            for (int i = 0; i < s.Length; i++) { r[i] = s[i]; } 
            return r;
        }
        public void AddString(string str)
        { 
            byte[] s = StrToByteArray(str);
            int strlen = str.Length+1;

            if (Pos + strlen > MAX_PACKET) { Log.Warning("MAX_PACKET Exceeded."); return; }

            AddUShort((ushort)strlen);
            for (int i = 0; i < s.Length; i++)
            {
                Data[Pos + i] = s[i];
            }
            Pos += (ushort)strlen;
            Length += (ushort)strlen;
        }

        public void AddString(string str,int length)
        {
            byte[] s = StrToByteArray(str);
            AddUShort((ushort)length);
            if (Pos + length > MAX_PACKET) { Log.Warning("MAX_PACKET Exceeded."); return; }
            for (int i = 0; i < length; i++)
            {
                if (i >= s.Length) Data[Pos + i] = 0;
                else Data[Pos + i] = s[i];
            }
            Pos += (ushort)length;
            Length += (ushort)length;
        }


        public byte[] ReadData(int length)
        {
            byte[] ret = new byte[length];
            for (int i = 0; i < length; i++)
            {
                //if (pos > Length) break;
                ret[i] = Data[Pos];
                Pos++;
            }
            return ret;
        }


        public bool ReadBool()
        {
            byte t = ReadByte();
            return System.Convert.ToBoolean(t);
        }
        public byte ReadByte()
        {
            Pos += 1;
            return Data[Pos - 1];
        }

        public ushort ReadUShort()
        {
            ushort ret = BitConverter.ToUInt16(Data, Pos);
            Pos += 2;
            return ret;
        }

        public int ReadInt32()
        {
            int ret = (int)(Data[Pos] + (Data[Pos + 1] << 8) + (Data[Pos + 2] << 16) + (Data[Pos + 3] << 24));
            Pos += 4;
            return ret;
        }

        public uint ReadUInt32()
        {
            uint ret = (uint)(Data[Pos] + (Data[Pos + 1] << 8) + (Data[Pos + 2] << 16) + (Data[Pos + 3] << 24));
            Pos += 4;
            return ret;
        }


        public float ReadFloat32()
        {
            float ret = System.BitConverter.ToSingle(Data, Pos);
            Pos += 4;
            return ret;
        }

        public string ByteArraytoStr(byte[] b)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            return enc.GetString(b);
        }
        public string ReadString()
        {
            ushort strlen = ReadUShort();
            //Samp.Util.Log.Debug("readstr: " + strlen + " / " + (pos + strlen).ToString() + " / " + Data.Count());
            byte[] b = new byte[strlen];
            for (int i = 0; i < strlen; i++)
            {
                //Samp.Util.Log.Debug("q: " + Data[pos + i]);
                b[i] = Data[Pos + i];
            }
            Pos += strlen;
			string ret = ByteArraytoStr(b);
			ret = ret.Trim('\0'); // monos String.Compare() function was giving me issue with the null char (which .net would ignore)
            return ret;
        }



    }
}
