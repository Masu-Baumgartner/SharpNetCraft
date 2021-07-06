using SharpNetCraft.Utils;

namespace SharpNetCraft.Pakets.Play
{
	public class KeepAlivePacket : Packet<KeepAlivePacket>
	{
		public KeepAlivePacket()
		{
			PacketId = 0x0F; //Clientbound
		}

		public long KeepAliveid;

		public override void Decode(MinecraftStream stream)
		{
			KeepAliveid = stream.ReadLong();
		}

		public override void Encode(MinecraftStream stream)
		{
			stream.WriteLong(KeepAliveid);
		}

		/// <inheritdoc />
		protected override void ResetPacket()
		{
			base.ResetPacket();
			PacketId = 0x0F;
		}
	}
}
