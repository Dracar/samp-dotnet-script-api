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

namespace Samp.Util
{
    public class Util
    {
        public static int GetEpochTime()
        {
            return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static string GetFilenameFromPath(string path)
        {
            int last = path.LastIndexOf('\\') + 1; // todo: fix
            if (last <= 1) last = path.LastIndexOf('/') + 1;
            string ret = path.Substring(last, path.Length - last);
            return ret;
        }
		
		public static int strcmp(string str1, string str2)
		{ 
			// mono doesnt like the \0 on the end of one string but not other & returns 1 
			// which while correct, is not what .net gives back. So ill just trim them.
			str1 = str1.Trim('\0');	
			str2 = str2.Trim('\0');	
			return String.Compare(str1,str2);
		}
    }

    public class Cached<T>
    {
        //public static int CacheMS = 2; 
        private T _Value { get; set; }
        public T Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                LastSet = DateTime.Now;
            }
        }

        public static implicit operator Cached<T>(T value)
        {
            return new Cached<T>(value);
        }

        public static implicit operator T(Cached<T> r)
        {
            return r.Value;
        }

        public DateTime LastSet;
        public int ElapsedMS
        {
            get
            {
                TimeSpan span = DateTime.Now.Subtract(LastSet);
                return (int)span.TotalMilliseconds;
            }
        }
        public Cached(T value) { this.Value = value;}
    }

    public class RefType<T> where T : struct
    {
        public T Value { get; set; }
        public RefType(T value) { this.Value = value; }
    }

    public class IntRef
    { 
        public int Value;
        public IntRef(int value) { this.Value = value; }

        public static implicit operator IntRef(int value)
        {
            return new IntRef(value);
        }

        public static implicit operator int(IntRef r)
        {
            return r.Value;
        }

        public static int operator +(IntRef one, IntRef two)
        {
            return one.Value + two.Value;
        }

        public static IntRef operator +(int one, IntRef two)
        {
            return new IntRef(one + two);
        }

        public static int operator -(IntRef one, IntRef two)
        {
            return one.Value - two.Value;
        }

        public static IntRef operator -(int one, IntRef two)
        {
            return new IntRef(one - two);
        } 
    }
    public class FloatRef
    {
        public float Value;
        public FloatRef() { this.Value = 0.0F; }
        public FloatRef(float value) { this.Value = value; }


        public static implicit operator FloatRef(float value)
        {
            return new FloatRef(value);
        }

        public static implicit operator float(FloatRef r)
        {
            return r.Value;
        }

        public static float operator +(FloatRef one, FloatRef two)
        {
            return one.Value + two.Value;
        }

        public static FloatRef operator +(float one, FloatRef two)
        {
            return new FloatRef(one + two);
        }

        public static float operator -(FloatRef one, FloatRef two)
        {
            return one.Value - two.Value;
        }

        public static FloatRef operator -(float one, FloatRef two)
        {
            return new FloatRef(one - two);
        } 
    }

    public class StringRef
    {
        public string Value;
        public StringRef(string value) { this.Value = value; }


        public static implicit operator StringRef(string value)
        {
            return new StringRef(value);
        }

        public static implicit operator string(StringRef r)
        {
            return r.Value;
        }

    }
}
