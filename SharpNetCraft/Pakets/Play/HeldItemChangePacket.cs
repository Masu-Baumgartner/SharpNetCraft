﻿using SharpNetCraft.Utils;

namespace SharpNetCraft.Pakets.Play
{
    public class HeldItemChangePacket : Packet<HeldItemChangePacket>
    {
	    public HeldItemChangePacket()
	    {
		    PacketId = 0x25;
	    }

	    public short Slot;

	    public override void Decode(MinecraftStream stream)
	    {
		    Slot = (byte)stream.ReadByte();
	    }

	    public override void Encode(MinecraftStream stream)
	    {
		    stream.WriteShort(Slot);
	    }
    }
}
