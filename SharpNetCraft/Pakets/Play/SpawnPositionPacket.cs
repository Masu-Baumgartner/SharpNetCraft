using SharpNetCraft.Utils;
using SharpNetCraft.Utils.Math;

namespace SharpNetCraft.Pakets.Play
{
	public class SpawnPositionPacket : Packet<SpawnPositionPacket>
	{
		public Vector3 SpawnPosition { get; set; }
		
		/// <inheritdoc />
		public override void Decode(MinecraftStream stream)
		{
			SpawnPosition = stream.ReadPosition();
		}

		/// <inheritdoc />
		public override void Encode(MinecraftStream stream)
		{
			throw new System.NotImplementedException();
		}
	}
}