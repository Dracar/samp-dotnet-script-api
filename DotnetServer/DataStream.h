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


#pragma once
#include "Log.h"

class DataStream
{
public:
		DataStream()
		{
			Length = 0;
			pos = 0;
			for (int i=0;i<MAX_PACKET;i++) Data[i] = 0;
		}

		static const int MAX_PACKET = 1300;
		char Data[MAX_PACKET];
		unsigned short pos;
		unsigned short Length;

		void AddData(char* startpos,unsigned short length)
		{
			for (int i=0;i<length;i++)
			{
				Data[pos+i] = startpos[i];
			}
			//memcpy(&Data[pos],startpos,length);
			pos += length;
			Length += length;
		}

		void AddByte(char b)
		{
			Data[pos] = b;
			pos += 1;
			Length += 1;
		}

		void AddUShort(unsigned short us)
		{
			memcpy(&Data[pos],&us,2);
			pos += 2;
			Length += 2;
		}

		void AddInt32(int i)
		{
			Data[pos] = (char)i;
			Data[pos+1] = (char)(i >> 8);
			Data[pos+2] = (char)(i >> 16);
			Data[pos+3] = (char)(i >> 24);
			pos += 4;
			Length += 4;
		}

		void AddFloat32(float f)
		{

			memcpy( Data+pos, &f, 4 );
			pos += 4;
			Length += 4;
		}

		int strlent(char* str)
		{
			for (int i=0;i<255;i++) {if (str[i] == 0) return i+1;}
			return 0;
		}
		void AddString(char* s)
		{ 
			int len=strlent(s);

			AddUShort(len);
			for (int i=0;i<len;i++)
			{
				Data[pos] = s[i];
				pos++;
				Length++;
			}
		}

		void AddString(char* s, int length)
		{ 
			AddUShort(length);
			int end=false;
			for (int i=0;i<length;i++)
			{
				if (!end)
				{
					Data[pos+i] = s[i];
					if (s[i] == 0) end=true;
				}
				else
				{
					Data[pos+i] = 0;
				}
			}

			pos += length;
			Length += length;
		}

		char ReadByte()
		{
			pos += 1;
			return Data[pos-1];
		}

		unsigned short ReadUShort()
		{
			unsigned short us = 0;
			memcpy(&us,&Data[pos],2);
			pos += 2;
			return us;
		}

		int ReadInt32()
		{
			//int ret = (int)(Data[pos] + (Data[pos+1] << 8) + (Data[pos+2] << 16) + (Data[pos+3] << 24)); 
			int ret = 0;
			memcpy(&ret,&Data[pos],4);
			pos += 4;
			return ret;
		}

		float ReadFloat32()
		{
			float f = 0.0F;
			memcpy(&f,&Data[pos],4);
			pos += 4;
			return f;
		}


		char* ReadString(char* dest, int size)
		{
			if (dest == NULL) return NULL;
			int slen = ReadUShort();
			for (int i=0;(i<size&&i<slen);i++)
			{
				dest[i] = Data[pos+i];
			}
			pos += slen;
			return dest;
		}


        
private:



};
