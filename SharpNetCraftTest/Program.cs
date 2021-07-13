using System;

using SharpNetCraft;

namespace SharpNetCraftTest
{
    public class Program
    {
        public static MinecraftClient client;
        static void Main(string[] args)
        {
            Logger.SetLogger(new MyLogger());
            MCPacketFactory.Load();

            Logger.GetLogger().Info("Starting ...");

            client = new MinecraftClient();
            client.hook = new MyHook();

            client.SetOfflineUsername("Test2");

            client.Connect("127.0.0.1", 25565);

            Console.ReadLine();
        }
    }
}
