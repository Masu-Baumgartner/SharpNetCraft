using SharpNetCraft.Utils;
using SharpNetCraft.Utils.Data;
//using fNbt.Tags;

namespace SharpNetCraft.Pakets.Play
{
    public class WindowItems : Packet<WindowItems>
    {
	    public WindowItems()
	    {

	    }

	    public byte WindowId = 0;
	    public SlotData[] Slots;

	    public override void Decode(MinecraftStream stream)
	    {
		    WindowId = (byte)stream.ReadByte();

		    short slotCount = stream.ReadShort();
			Slots = new SlotData[slotCount];

		    for (int i = 0; i < Slots.Length; i++)
		    {
			    Slots[i] = stream.ReadSlot();
		    }
	    }

	    public override void Encode(MinecraftStream stream)
	    {
		    
	    }
    }
}
