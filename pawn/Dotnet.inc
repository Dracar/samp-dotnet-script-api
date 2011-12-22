

native Dotnet_NewPacket(opcode); // returns pointer to new packet
native Dotnet_SendPacket(&packet); // sends the packet to all dotnet clients & deletes the packet
native Dotnet_DeletePacket(&packet); // deletes the packet
native Dotnet_SetPacketPosition(&packet,pos);
native Dotnet_GetPacketPosition(&packet);
native Dotnet_GetPacketLength(&packet);

native Dotnet_AddInt32ToPacket(&packet,i);
native Dotnet_AddFloat32ToPacket(&packet,Float:f);
native Dotnet_AddCellStringToPacket(&packet, cstr[]);

native Dotnet_ReadInt32FromPacket(&packet);
native Float:Dotnet_ReadFloat32FromPacket(&packet);
native Dotnet_ReadStringFromPacket(&packet, cstr[]);



stock Packet_Null = 0x00; // unused
stock Packet_CryptKey = 0x01; // todo: encryption
stock Packet_Auth = 0x02;
stock Packet_Ping = 0x03;
stock Packet_Callback = 0x04;
stock Packet_CallbackFrequency = 0x05; // client sends to server to set which callbacks are sent to client. 0 = no callbacks are sent, 1 = all but player/vehicle updates, 2 = all. default is 1
stock Packet_FunctionRequest = 0x06; // client can send to server to request native/public samp/pawn function. server can send to client when pawn calls dotnet function
stock Packet_FunctionReply = 0x07;	
stock Packet_Test = 0x0A;
		
		
		
stock Dotnet_NewFunctionRequestPacket(functionname[],callbackname[],argtypes[])
{
    new packet = Dotnet_NewPacket(Packet_FunctionRequest);
    Dotnet_AddCellStringToPacket(packet,functionname); 
    Dotnet_AddCellStringToPacket(packet,callbackname); 
    Dotnet_AddCellStringToPacket(packet,argtypes); 
	return packet;
}