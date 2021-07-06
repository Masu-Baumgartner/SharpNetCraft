using SharpNetCraft.Utils;

namespace SharpNetCraft.Pakets.Play
{
	public class ClientStatusPacket : Packet<ClientStatusPacket>
	{
		public ClientStatusPacket()
		{
			PacketId = 0x04;
		}

		public Action ActionID;

		public override void Decode(MinecraftStream stream)
		{
			ActionID = (Action)stream.ReadVarInt();	
		}

		public override void Encode(MinecraftStream stream)
		{
			stream.WriteVarInt((int)ActionID);
		}

		public enum Action
		{
			PerformRespawnOrConfirmLogin = 0,
			RequestStats = 1
		}
	}
}
