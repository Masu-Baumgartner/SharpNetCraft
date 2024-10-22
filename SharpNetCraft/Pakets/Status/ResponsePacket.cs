﻿using SharpNetCraft.Utils;

namespace SharpNetCraft.Pakets.Status
{
    public class ResponsePacket : Packet<ResponsePacket>
	{
		public string ResponseMsg { get; set; }

		public ResponsePacket()
		{
			PacketId = 0x00;
		}

		public override void Decode(MinecraftStream stream)
		{
			ResponseMsg = stream.ReadString();
		}

		public override void Encode(MinecraftStream stream)
		{
			stream.WriteString(ResponseMsg);
		}
	}
}
