namespace SharpNetCraft.Pakets
{
	public interface IPacketHandler
	{
		void HandleHandshake(Packet packet);
		void HandleStatus(Packet packet);
		void HandleLogin(Packet packet);
		void HandlePlay(Packet packet);
	}
}