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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Samp.API
{
    public class Vector3
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3()
        {
            X = 0.0F;
            Y = 0.0F;
            Z = 0.0F;
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float Distance(Vector3 other)
        {
            //     __________________________________
            //d = &#8730; (x2-x1)^2 + (y2-y1)^2 + (z2-z1)^2
            //
 
            //Our end result
            float result = 0.0F;
            //Take x2-x1, then square it
            double part1 = Math.Pow((other.X - this.X), 2);
            //Take y2-y1, then sqaure it
            double part2 = Math.Pow((other.Y - this.Y), 2);
            //Take z2-z1, then square it
            double part3 = Math.Pow((other.Z - this.Z), 2);
            //Add both of the parts together
            double underRadical = part1 + part2 + part3;
            //Get the square root of the parts
            result = (float)Math.Sqrt(underRadical);
            //Return our result
            return result;
        }

        public Vector3 GetOffset2D(float rotation,float distance)
        {
            float x = this.X;
            float y = this.Y;
            //GetPlayerPos(playerid, x, y, Angle);
            //GetPlayerFacingAngle(playerid, Angle);
            x += (float)(distance * Math.Sin(rotation * (Math.PI/180))* -1);
            y += (float)(distance * Math.Cos(rotation * (Math.PI / 180)));
            return new Vector3(x, y, this.Z);
        }
    }
}
