using SharpNetCraft.Utils;

namespace SharpNetCraft.Pakets.Play
{
	public class PlayerMovementPacket : Packet<PlayerMovementPacket>
	{
		public bool OnGround { get; set; }
		public PlayerMovementPacket()
		{
			PacketId = 0x14;
		}
		
		/// <inheritdoc />
		public override void Decode(MinecraftStream stream)
		{
			throw new System.NotImplementedException();
		}

		/// <inheritdoc />
		public override void Encode(MinecraftStream stream)
		{
			stream.WriteBool(OnGround);
		}
	}
}