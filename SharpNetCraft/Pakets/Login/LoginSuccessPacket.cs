using System;
using SharpNetCraft.Utils;
using SharpNetCraft.Utils.Data;

namespace SharpNetCraft.Pakets.Login
{
	public class LoginSuccessPacket : Packet<LoginSuccessPacket>
	{
		public LoginSuccessPacket()
		{
			PacketId = 0x03;
		}

		public UUID UUID;
		public string Username;

		public override void Decode(MinecraftStream stream)
		{
			UUID = stream.ReadUuid();
			Username = stream.ReadString();
		}

		public override void Encode(MinecraftStream stream)
		{
			stream.WriteUuid(UUID);
			stream.WriteString(Username);
		}
	}
}
