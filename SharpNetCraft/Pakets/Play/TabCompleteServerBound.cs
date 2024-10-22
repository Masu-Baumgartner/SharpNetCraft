﻿using System;
using SharpNetCraft.Utils;

namespace SharpNetCraft.Pakets.Play
{
    public class TabCompleteServerBound : Packet<TabCompleteServerBound>
    {
	    public int TransactionId;
	    public string Text;

	    public TabCompleteServerBound()
	    {
		    PacketId = 0x06;
	    }

		public override void Decode(MinecraftStream stream)
	    {
		    throw new NotImplementedException();
	    }

	    public override void Encode(MinecraftStream stream)
	    {
		    stream.WriteVarInt(TransactionId);
			stream.WriteString(Text);
	    }
    }
}
