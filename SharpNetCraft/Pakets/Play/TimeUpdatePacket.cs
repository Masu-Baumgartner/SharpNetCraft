﻿using SharpNetCraft.Utils;

namespace SharpNetCraft.Pakets.Play
{
    public class TimeUpdatePacket : Packet<TimeUpdatePacket>
    {
	    public long WorldAge;
	    public long TimeOfDay;
	    public TimeUpdatePacket()
	    {

	    }

	    public override void Decode(MinecraftStream stream)
	    {
		    WorldAge = stream.ReadLong();
		    TimeOfDay = stream.ReadLong();
	    }

	    public override void Encode(MinecraftStream stream)
	    {
		    
	    }
    }
}
