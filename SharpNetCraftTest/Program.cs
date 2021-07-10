using System;
using System.Net;
using System.Threading;

using SharpNetCraft;

namespace SharpNetCraftTest
{
    public class Program
    {
        public static MinecraftUser user;
        static void Main(string[] args)
        {
            Logger.SetLogger(new MyLogger());
            MCPacketFactory.Load();

            Logger.GetLogger().Info("Starting ...");

            user = new MinecraftUser();
            user.hook = new MyHook();

            user.Connect("127.0.0.1", 25565);

            Console.ReadLine();
        }
    }
}
