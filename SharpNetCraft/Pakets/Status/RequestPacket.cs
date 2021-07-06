using SharpNetCraft.Utils;

namespace SharpNetCraft.Pakets.Status
{
	public class RequestPacket : Packet<RequestPacket>
	{
		public RequestPacket()
		{
			PacketId = 0x00;
		}

		public override void Decode(MinecraftStream stream)
		{
			
		}

		public override void Encode(MinecraftStream stream)
		{
			
		}
	}
}
