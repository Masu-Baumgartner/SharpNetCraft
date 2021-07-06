using SharpNetCraft.Utils;

namespace SharpNetCraft.Pakets.Status
{
    public class PingPacket : Packet<PingPacket>
	{
		public PingPacket()
		{
			PacketId = 0x01;
		}

		public long Payload;

		public override void Decode(MinecraftStream stream)
		{
			Payload = stream.ReadLong();
		}

		public override void Encode(MinecraftStream stream)
		{
			stream.WriteLong(Payload);
		}
	}
}
