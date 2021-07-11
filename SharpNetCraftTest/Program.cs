using System;

using SharpNetCraft;

namespace SharpNetCraftTest
{
    public class Program
    {
        public static MinecraftClient user;
        static void Main(string[] args)
        {
            Logger.SetLogger(new MyLogger());
            MCPacketFactory.Load();

            Logger.GetLogger().Info("Starting ...");

            user = new MinecraftClient();
            user.hook = new MyHook();

            user.SetOfflineUsername("Test1");

            user.Connect("127.0.0.1", 25565);

            Console.ReadLine();
        }
    }
}
