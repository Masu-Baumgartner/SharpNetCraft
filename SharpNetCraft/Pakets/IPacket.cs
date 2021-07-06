using System.IO;

namespace SharpNetCraft.Packets
{
	public interface IPacket<in TStream> where TStream : Stream
	{
		void Encode(TStream stream);
		void Decode(TStream stream);
	}
}
