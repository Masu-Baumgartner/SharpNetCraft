namespace SharpNetCraft.Pakets
{
	public class DefaultPacketHandler : IPacketHandler
	{
		private void UnhandledPacket(Packet packet)
		{
			Logger.GetLogger().Warn($"Unhandled packet: 0x{packet.PacketId:X2} ({packet.GetType().Name})");
		}
		
		/// <inheritdoc />
		public void HandleHandshake(Packet packet)
		{
			UnhandledPacket(packet);
		}

		/// <inheritdoc />
		public void HandleStatus(Packet packet)
		{
			UnhandledPacket(packet);
		}

		/// <inheritdoc />
		public void HandleLogin(Packet packet)
		{
			UnhandledPacket(packet);
		}

		/// <inheritdoc />
		public void HandlePlay(Packet packet)
		{
			UnhandledPacket(packet);
		}
	}
}