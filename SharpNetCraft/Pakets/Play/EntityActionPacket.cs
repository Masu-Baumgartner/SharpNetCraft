using SharpNetCraft.Utils;
using SharpNetCraft.Utils.Data;

namespace SharpNetCraft.Pakets.Play
{
    public class EntityActionPacket : Packet<EntityActionPacket>
    {
	    public EntityActionPacket()
	    {
		    PacketId = 0x1B;
	    }

	    public int EntityId;
	    public EntityAction Action;
	    public int JumpBoost = 0;

		public override void Decode(MinecraftStream stream)
		{
			EntityId = stream.ReadVarInt();
			Action = (EntityAction) stream.ReadVarInt();
			JumpBoost = stream.ReadVarInt();
		}

	    public override void Encode(MinecraftStream stream)
	    {
		    stream.WriteVarInt(EntityId);
		    stream.WriteVarInt((int) Action);
		    stream.WriteVarInt(JumpBoost);
	    }
    }
}
