using SharpNetCraft.Utils;
using SharpNetCraft.Utils.Math;

namespace SharpNetCraft.Pakets.Play
{
    public class BlockChangePacket : Packet<BlockChangePacket>
    {

	    public BlockChangePacket()
	    {
		    PacketId = 0x0B;
	    }

	    public BlockCoordinates Location;
	    public uint PalleteId;

	    public override void Decode(MinecraftStream stream)
	    {
		    Location = stream.ReadPosition();
		    PalleteId = (uint) stream.ReadVarInt();
	    }

	    public override void Encode(MinecraftStream stream)
	    {
		    
	    }
    }
}
