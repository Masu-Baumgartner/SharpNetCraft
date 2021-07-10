using SharpNetCraft;
using SharpNetCraft.Pakets;
using SharpNetCraft.Pakets.Login;
using SharpNetCraft.Pakets.Play;

namespace SharpNetCraftTest
{
    public class MyHook : IPacketHandler
    {
        public static NetworkProvider provider;
        private static float lastHealth = 0f;
        public void HandleHandshake(Packet packet)
        {
            
        }

        public void HandleLogin(Packet packet)
        {
            if(packet is LoginSuccessPacket)
            {
                provider = new NetworkProvider(Program.user.GetConnection());
            }
        }

        public void HandlePlay(Packet packet)
        {
            if(packet is UpdateHealthPacket)
            {
                UpdateHealthPacket hup = (UpdateHealthPacket)packet;
                if(hup.Health != lastHealth)
                {
                    provider.SendChatMessage("My health is: " + hup.Health);
                    lastHealth = hup.Health;
                }
            }
        }

        public void HandleStatus(Packet packet)
        {
            
        }
    }
}
