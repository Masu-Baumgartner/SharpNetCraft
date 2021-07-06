using SharpNetCraft.Utils;

namespace SharpNetCraft.Pakets.Play
{
    public class DestroyEntitiesPacket : Packet<DestroyEntitiesPacket>
    {
		public DestroyEntitiesPacket()
		{

		}

		public int[] EntityIds;

		public override void Decode(MinecraftStream stream)
		{
			EntityIds = new int[1];
			int count = stream.ReadVarInt();
			EntityIds[0] = count;
		}

		public override void Encode(MinecraftStream stream)
		{
			
		}
	}
}
