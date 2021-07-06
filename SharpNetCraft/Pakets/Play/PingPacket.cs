using SharpNetCraft.Utils;

namespace SharpNetCraft.Pakets.Play
{
	public class PlayPingPacket : Packet<PlayPingPacket>
	{
		public int PingId { get; set; }
		/// <inheritdoc />
		public override void Decode(MinecraftStream stream)
		{
			PingId = stream.ReadInt();
		}

		/// <inheritdoc />
		public override void Encode(MinecraftStream stream)
		{
			stream.WriteInt(PingId);
		}
	}
}