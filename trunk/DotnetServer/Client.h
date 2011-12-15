/*
 * Iain Gilbert
 * 2011
 *
*/

/*
    The contents of this file are subject to the Common Public Attribution License Version 1.0 (the �License�); you may not use this file except in compliance with the License. 
    You may obtain a copy of the License at http://www.opensource.org/licenses/cpal_1.0 
    The License is based on the Mozilla Public License Version 1.1 but Sections 14 and 15 have been added to cover use of software over a computer network and provide for limited attribution for the Original Developer. 
    In addition, Exhibit A has been modified to be consistent with Exhibit B.
    Software distributed under the License is distributed on an �AS IS� basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. 
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


#include "main.h"

//#include <stdarg.h>


#if defined(_WIN32)
	#ifndef _WINSOCKAPI_ 
		#define _WINSOCKAPI_ 
	#endif
	#include <winsock2.h>
	#pragma comment(lib, "ws2_32.lib")
	#define MSG_NOSIGNAL 0
#else
	#include <netinet/in.h>
	#include <sys/socket.h>
	#include <arpa/inet.h>
        #include <signal.h>
        typedef int SOCKET;
        #define SD_BOTH 2;
        typedef struct sockaddr* LPSOCKADDR;
        #define SOCKET_ERROR (-1)
        #define INVALID_SOCKET (-1)
#endif


#include "Packet.h"
		
class Client // represents a client that will connect to our server
{
public:
		Client()
		{
			IsConnected = false;
			IsAuthenticated = false;
			ID = -1;
			sbuflock = false;
			sbufpos = 0;
			Timeout = 0;
			for (int i=0;i<MAX_BUFF;i++) {sendbuf[i] = 0;}
			CallbackFequency = 1;
		}

		//void OnConnect();

		bool IsConnected;
		bool IsAuthenticated;
		int ID;
		sockaddr_in Address;
		SOCKET Socket;
		char CallbackFequency;

		static const int MAX_BUFF = 1300;
		char sendbuf[MAX_BUFF]; // buffer structure = {ushort paklength, char[] data, repeat for more packets in buffer}
		bool sbuflock; // locks send buffer from read/write, for thread safety
		unsigned short sbufpos;

		u_short Timeout;
		static const u_short MAX_TIMEOUT = 30; // seconds
		static const u_short PING_INTERVAL = 20; // seconds
        
private:


};

