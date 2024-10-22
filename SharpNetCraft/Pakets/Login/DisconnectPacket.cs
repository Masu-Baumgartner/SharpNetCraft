﻿using SharpNetCraft.Utils;

namespace SharpNetCraft.Pakets.Login
{
    public class DisconnectPacket : Packet<DisconnectPacket>
    {
	    public string Message;
		public DisconnectPacket()
	    {
		    PacketId = 0x00;
	    }

	    public override void Decode(MinecraftStream stream)
	    {
		    Message = stream.ReadString();
	    }

	    public override void Encode(MinecraftStream stream)
	    {
		    stream.WriteString(Message);
	    }
    }
}
