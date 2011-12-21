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
 
    Display of Attribution Information to the end user is not required on server connection, or at any time the end user is connected to your server.
    You must treat any External Deployment by You of the Original Code or Modifications as a distribution under section 3.1 and make Source Code available under Section 3.2.
    Display of Attribution Information is not required in Larger Works which are defined in the CPAL as a work which combines Covered Code or portions thereof with code not governed by the terms of the CPAL.
 */

#include "NativeFunctionProcessor.h"
#include "Client.h"
#include "Server.h"
#include "string"



void NativeFunctionProcessor::Init(AMX* pAMX)
{
	if(!pAMX) return;
	AMXScript* script = new AMXScript();
	script->Address = pAMX;
	script->Name = "NONAME"; // todo
	AddAMXScript(script);
	if (FindPublicFunction(script, "DotnetFSInit")) {logprintf("DotnetAMXScript %d",script); DotnetAMXScript = script;}
	logprintf("scr: %d",DotnetAMXScript);
}

bool NativeFunctionProcessor::AddAMXScript(AMXScript* script)
{
	for (int i=0;i<MAX_AMXSCRIPTS;i++)
	{
		if (AMXScripts[i] != NULL) continue;
		AMXScripts[i] = script;
		return true;
	}
	return false;
}
		
AMXFunction* NativeFunctionProcessor::FindNativeFunction(char* funcname)
{
	return FindNativeFunction(DotnetAMXScript,funcname);
}

AMXFunction* NativeFunctionProcessor::FindNativeFunction(AMXScript* script, char* funcname)
{
	int index=0;
	amx_FindNative(script->Address, funcname,&index);
	if (index == 2147483647) return NULL;
	logprintf("FindNativeFunction %s found",funcname);
	AMXFunction* func = new AMXFunction();
	func->Index = index;
	func->IsNative = true;
	memcpy(func->Name,funcname,strlen(funcname)+1);
	//func->Name = funcname;
	func->Script = script;
	return func;
}

		
AMXFunction* NativeFunctionProcessor::FindPublicFunction(char *funcname)
{
	AMXFunction* ret = NULL;
	for (int i=0;i<MAX_AMXSCRIPTS;i++)
	{
		if (AMXScripts[i] == NULL) continue;
		AMXScript* script = AMXScripts[i];
		ret = FindPublicFunction(script,funcname);
		if (ret != NULL) break;
	}
	return ret;
}

AMXFunction* NativeFunctionProcessor::FindPublicFunction(AMXScript* script, char *funcname)
{

		int index=0;
		amx_FindPublic(script->Address, funcname,&index);
		if (index == 2147483647) return NULL;
		Log::Debug("FindPublicFunction found");
		AMXFunction* func = new AMXFunction();
		func->Index = index;
		func->IsNative = false;
		memcpy(func->Name,funcname,strlen(funcname)+1);
		func->Script = script;
		return func;
}

AMXFunction* NativeFunctionProcessor::FindFunction(char *funcname)
{
	logprintf("DotnetServer Debug: Searching for AMX function %s.",funcname);
	AMXFunction* func = NULL;
	func = FindNativeFunction(funcname);
	if (func == NULL) func = FindPublicFunction(funcname);
	if (func == NULL) {logprintf("!DotnetServer Warining!: Unable to find AMX function %s.",funcname); return NULL;}
	logprintf("DotnetServer Debug: AMX function %s found.",funcname);
	return func; 
}

AMXFunction* NativeFunctionProcessor::FindFunction(AMXScript* script, char *funcname)
{
	AMXFunction* func = NULL;
	func = FindNativeFunction(script, funcname);
	if (func == NULL) func = FindPublicFunction(script, funcname);
	if (func == NULL) return NULL;
	return func; 
}

AMXFunction* NativeFunctionProcessor::GetFunctionByName(char *funcname)
{
	//logprintf("GetFunctionByName: %s",funcname);
	for (int i=0;i<MAX_LOADEDFUNCTIONS;i++)
	{
		if (LoadedFunctions[i] == NULL) continue;
		if (strcmp(LoadedFunctions[i]->Name,funcname) == 0) {return LoadedFunctions[i]; }// found it in our loaded list
	}
	AMXFunction* ret = FindFunction(funcname);
	if (ret == NULL) return NULL;
	for (int i=0;i<MAX_LOADEDFUNCTIONS;i++)
	{
		if (LoadedFunctions[i] == NULL) {LoadedFunctions[i] = ret; break;} // add the function to our list, so we dont have to search for it again
	}
	return ret;
}




uint32_t NativeFunctionProcessor::GetNativeFunctionAddress(AMXFunction* func)
{
	if (!func->IsNative) return NULL;

	unsigned int call_addr = 0;
	AMX_HEADER *hdr = (AMX_HEADER *) func->Script->Address->base;
	call_addr = (unsigned int)((AMX_FUNCSTUB *)((char *)(hdr) + (hdr)->natives + hdr->defsize * func->Index))->address;
	return call_addr;

}

bool NativeFunctionProcessor::AddFunctionRequestToQue(FunctionRequest* func)
{
	if (FunctionRequestQue[MAX_FUNCTIONREQUESTQUE-1] != NULL) 
	{
		Log::Warning("Function request que is full! Waiting for que to empty.");
		int i=0;
		while (FunctionRequestQue[MAX_FUNCTIONREQUESTQUE-1] != NULL && i < 100)
		{
			SLEEP(1);
			i++;
		}
		logprintf("DotnetServer Debug: Waited %d ms for que to empty.",i);
	}

	while (functionrequestquelock) {SLEEP(1);}
	functionrequestquelock = true;
	for (int i=0;i<MAX_FUNCTIONREQUESTQUE;i++)
	{
		if (FunctionRequestQue[i] == NULL)
		{
			FunctionRequestQue[i] = func;
			functionrequestquelock = false;
			return true;
		}
	}
	functionrequestquelock = false;
	Log::Warning("Function request que is still full! Dropping function request.");
	return false;
}


int NativeFunctionProcessor::ProcessFunctionRequestQue()
{
	if (FunctionRequestQue[0] == NULL) return 0; // que is empty
	int count = 0;
	while (functionrequestquelock) {SLEEP(1);}
	functionrequestquelock = true;
	for (int i=0;i<MAX_FUNCTIONREQUESTQUE;i++)
	{
		if (FunctionRequestQue[i] == NULL) continue;
		count++;
		FunctionRequest* functionresponse = ProcessFunctionRequest(FunctionRequestQue[i]);
		if (functionresponse == NULL) {FunctionRequestQue[i] = NULL; continue;}
		Server::Instance->PakSender->SendFunctionResponse(functionresponse->_Client,functionresponse);
		delete(functionresponse);
		FunctionRequestQue[i] = NULL;
	}
	functionrequestquelock = false;
	return count;
}

FunctionRequest* NativeFunctionProcessor::ProcessFunctionRequest(FunctionRequest* request)
{
	if (!DotnetAMXScript) return NULL;
	if (!request) return NULL;
	request->data->pos = 0;
	
	AMXFunction* func = GetFunctionByName(request->name);

	if (func == NULL) {sprintf("Dotnet Server: Warning: Failed to find native/public function $s.",request->name); return NULL;}

	
	cell* args = (cell*)calloc(strlen(request->params)+1,4);
	cell *phys_addr[10]; // 6
	args[0] = 4 * strlen(request->params);
	int vars = 0;

	for (unsigned int i=0;i<strlen(request->params);i++)
	{
		if (request->params[i] == 'i') 
		{
			int q = request->data->ReadInt32();
			if (func->IsNative)
			{
				args[i+1] = q;
			}
			else
			{
				amx_Push(func->Script->Address, q);
			}
		} 
		else if (request->params[i] == 'f') 
		{
			float f = request->data->ReadFloat32();
			if (func->IsNative)
			{
				args[i+1] = amx_ftoc(f);
			}
			else
			{
				amx_Push(func->Script->Address, amx_ftoc(f));
			}
		}
		else if (request->params[i] == 's') 
		{
			unsigned short slen = request->data->ReadUShort();
			request->data->pos -= 2;
			char* str = (char*)calloc(slen,1);
			str = request->data->ReadString(str,slen);
			size_t len = strlen(str);
			if (func->IsNative)
			{
				amx_Allot(func->Script->Address, (int)(len + 1), args + i+1,&phys_addr[vars++]);
				amx_SetString(phys_addr[vars-1], str, 0, 0, len + 1);
			}
			else
			{
				amx_PushString(func->Script->Address,args + i+1,&phys_addr[vars++],str,0,0);
			}
			free(str);
		}
		else 
		{
			//if (func->IsNative)
			//{
				if (request->params[i] == 'v') {amx_Allot(func->Script->Address, 1, args + i+1, &phys_addr[vars++]); request->data->ReadInt32();}
				if (request->params[i] == 'p') {amx_Allot(func->Script->Address, 1, args + i+1, &phys_addr[vars++]); request->data->ReadString(NULL,0);}
			/*}
			else
			{
				//if (request->params[i] == 'v') amx_PushAddress(
				if (request->params[i] == 'p') amx_Allot(func->Script->Address, 1, args + i+1, &phys_addr[vars++]);
			}*/
		}
	}
	
	if (func->IsNative)
	{
		amx_function_t amxFunc = (amx_function_t)GetNativeFunctionAddress(func);
		request->response = amxFunc(func->Script->Address, args);
	}
	else
	{
		cell ret = 0;
		amx_Exec(func->Script->Address,&ret,func->Index);
	}
	request->data->pos = 0;
	request->data->Length = 0;
	vars = 0;
	for (unsigned int i=0;i<strlen(request->params);i++)
	{
		if (request->params[i] == 'v') request->data->AddInt32(*phys_addr[vars]);

		if (request->params[i] == 'p')
		{
			char* str = (char*)calloc(32,1);
			amx_GetString(str, phys_addr[vars], 0, 32); // todo: string length greater than 32
			request->data->AddString(str);
			free(str);
		}

		if ((request->params[i] == 's') || (request->params[i] == 'v') || (request->params[i] == 'p')) 
		{ 
			vars++; 
			amx_Release(func->Script->Address, args[i+1]);
		}
	}
	
	free(args);
	//free(phys_addr);
	request->data->pos = 0;
        
	return request;
}

/*

FunctionRequest* NativeFunctionProcessor::ProcessFunctionRequestOLD(FunctionRequest* request)
{
	if (!AMXInit) return NULL;
	if (!request) return NULL;
	request->data->pos = 0;

	amx_function_t amxFunc = NULL;
	sNativeFunction* sfunc = GetNativeByName(request->name);
	if (sfunc != NULL) 
	{
		amxFunc = sfunc->func;
		memcpy(request->params,sfunc->Args,strlen(sfunc->Args)+1);
	}
	if (amxFunc == NULL) 
	{
		logprintf("Searching for AMX function: %s.",request->name);
		amxFunc = FindFunction(request->name);
		//logprintf("Debug: params: %s, %d.",request->params,strlen(request->params));
	}
	
	if (amxFunc == NULL)
	{
		logprintf("AMX Function '%s' not found!",request->name);
		return NULL;
	}



	cell* args = (cell*)calloc(strlen(request->params)+1,4);
	cell *phys_addr[10]; // 6
	args[0] = 4 * strlen(request->params);
	int vars = 0;

	for (unsigned int i=0;i<strlen(request->params);i++)
	{
		if (request->params[i] == 'i') 
		{
			int q = request->data->ReadInt32();
			args[i+1] = q;
		} 
		else if (request->params[i] == 'f') 
		{
			float f = request->data->ReadFloat32();
			args[i+1] = amx_ftoc(f);
		}
		else if (request->params[i] == 's') 
		{
			unsigned short slen = request->data->ReadUShort();
			request->data->pos -= 2;
			char* str = (char*)calloc(slen,1);
			str = request->data->ReadString(str,slen);
			size_t len = strlen(str);
			amx_Allot(__gpAMX, (int)(len + 1), args + i+1,&phys_addr[vars++]);
			amx_SetString(phys_addr[vars-1], str, 0, 0, len + 1);
			free(str);
		}
		else 
		{
			if (request->params[i] == 'v') amx_Allot(__gpAMX, 1, args + i+1, &phys_addr[vars++]); 
			if (request->params[i] == 'p') amx_Allot(__gpAMX, 1, args + i+1, &phys_addr[vars++]); 
		}
	}
	

	request->response = amxFunc(__gpAMX, args);
	
	request->data->pos = 0;
	request->data->Length = 0;
	vars = 0;
	for (unsigned int i=0;i<strlen(request->params);i++)
	{
		if (request->params[i] == 'v') request->data->AddInt32(*phys_addr[vars]);

		if (request->params[i] == 'p')
		{
			char* str = (char*)calloc(32,1);
			amx_GetString(str, phys_addr[vars], 0, 32); // todo: string length greater than 32
			request->data->AddString(str);
			free(str);
		}

		if ((request->params[i] == 's') || (request->params[i] == 'v') || (request->params[i] == 'p')) 
		{ 
			vars++; 
			amx_Release(__gpAMX, args[i+1]);
		}
	}
	
	free(args);
	//free(phys_addr);
	request->data->pos = 0;
        
	return request;
}*/