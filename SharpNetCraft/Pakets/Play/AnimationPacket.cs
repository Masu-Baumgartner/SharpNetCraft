using SharpNetCraft.Utils;

namespace SharpNetCraft.Pakets.Play
{
	public class AnimationPacket : Packet<AnimationPacket>
	{
		public AnimationPacket()
		{
			PacketId = 0x2C;
		}
		
		public int Hand { get; set; }
		
		/// <inheritdoc />
		public override void Decode(MinecraftStream stream)
		{
			throw new System.NotImplementedException();
		}

		/// <inheritdoc />
		public override void Encode(MinecraftStream stream)
		{
			stream.WriteVarInt(Hand);
		}
	}
}