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
#include "SDK/amx/amx.h"
#include "SDK/plugincommon.h"
#include "DataStream.h"
#include "Log.h"

class Client;
typedef int (*amx_function_t)(AMX *amx, cell* params);

class FunctionRequest
{
public:
	static const int STRING_SIZE = 39; // guid is 36 chars, +1 for terminator, extra 2 incase of parenthesis
	Client* _Client; // the client who requested this function
	char* guid;
	int response;
	char* name;
	char* params;
	DataStream* data;
	FunctionRequest()
	{
		_Client = NULL;
		guid = (char*)calloc(STRING_SIZE,1);
		response=0;
		name = (char*)calloc(STRING_SIZE,1);
		params = (char*)calloc(STRING_SIZE,1);
		data = new DataStream();
	}
	~FunctionRequest()
	{
		_Client = NULL;
		delete(data);
		free(guid);
		free(name);
		free(params);
	}
};

class AMXScript
{
public:
	AMX* Address;
	char* Name;

	AMXScript()
	{
		Name = (char*)calloc(32,1);
	}
	~AMXScript()
	{
		free(Name);
	}
};

class AMXFunction
{
public:
	bool IsNative;
	AMXScript* Script;
	char* Name;
	int Index;

	AMXFunction()
	{
		Name = (char*)calloc(32,1);
	}
	~AMXFunction()
	{
		free(Name);
	}
	//uint32_t Address;
};

class NativeFunctionProcessor
{
public:
		NativeFunctionProcessor()
		{
			DotnetAMXScript = NULL;
			functionrequestquelock = false;

			//FunctionRequestQue = (FunctionRequest**)calloc(MAX_FUNCTIONREQUESTQUE,4);
			//AMXScripts = (AMXScript**)calloc(MAX_AMXSCRIPTS,4);
			//LoadedFunctions = (AMXFunction**)calloc(MAX_LOADEDFUNCTIONS,4);
			for (int i=0;i<MAX_FUNCTIONREQUESTQUE;i++) {FunctionRequestQue[i] = NULL;}
			for (int i=0;i<MAX_AMXSCRIPTS;i++) {AMXScripts[i] = NULL;}
			for (int i=0;i<MAX_LOADEDFUNCTIONS;i++) {LoadedFunctions[i] = NULL;}
		}

		AMXScript* DotnetAMXScript;
		bool AddAMXScript(AMXScript* script);
		static const int MAX_AMXSCRIPTS = 100;
		AMXScript* AMXScripts[MAX_AMXSCRIPTS];


		void Init(AMX *pAMX);
		AMXFunction* FindFunction(AMXScript* script, char *name); // 
		AMXFunction* FindFunction(char *name); // 
		AMXFunction* FindNativeFunction(AMXScript* script, char *name); // These are mostly just wrappers for SDK functions
		AMXFunction* FindNativeFunction(char *name); 
		AMXFunction* FindPublicFunction(AMXScript* script, char *name); 
		AMXFunction* FindPublicFunction(char *name); 
		AMXFunction* GetFunctionByName(char *funcname);
		uint32_t GetNativeFunctionAddress(AMXFunction* func); //
		static const int MAX_LOADEDFUNCTIONS = 1000;
		AMXFunction* LoadedFunctions[MAX_LOADEDFUNCTIONS]; // a list of all the functions we have already searched for
		

		FunctionRequest* ProcessFunctionRequest(FunctionRequest* function);
		bool functionrequestquelock;
		bool AddFunctionRequestToQue(FunctionRequest* func);
		int ProcessFunctionRequestQue();
		static const int MAX_FUNCTIONREQUESTQUE = 100;
		FunctionRequest* FunctionRequestQue[MAX_FUNCTIONREQUESTQUE];


private:



};
