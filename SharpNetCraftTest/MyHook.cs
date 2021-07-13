using SharpNetCraft;
using SharpNetCraft.Pakets;
using SharpNetCraft.Pakets.Play;

namespace SharpNetCraftTest
{
    public class MyHook : IPacketHandler
    {
        private static float lastHealth = -1f;
        public void HandleHandshake(Packet packet)
        {
            
        }

        public void HandleLogin(Packet packet)
        {

        }

        public void HandlePlay(Packet packet)
        {
            if(packet is UpdateHealthPacket)
            {
                UpdateHealthPacket hup = (UpdateHealthPacket)packet;
                if(hup.Health != lastHealth)
                {
                    Program.client.actionProvider.SendChatMessage("Debug: My health is: " + hup.Health);
                    lastHealth = hup.Health;
                }
            }
        }

        public void HandleStatus(Packet packet)
        {
            
        }
    }
}
