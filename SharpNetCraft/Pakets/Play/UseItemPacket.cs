using System;

using SharpNetCraft.Utils;

namespace SharpNetCraft.Pakets.Play
{
    public class UseItemPacket : Packet<UseItemPacket>
    {
	    public int Hand = 0;
	    public UseItemPacket()
	    {
		    PacketId = 0x2F;
	    }

	    public override void Decode(MinecraftStream stream)
	    {
		    throw new NotImplementedException();
	    }

	    public override void Encode(MinecraftStream stream)
	    {
		    stream.WriteVarInt(Hand);
	    }
    }
}
