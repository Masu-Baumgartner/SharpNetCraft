using System;
using SharpNetCraft;

namespace SharpNetCraftTest
{
    class MyLogger : Logger
    {
        public void Debug(string message)
        {
            Console.WriteLine("[DEBUG] > " + message);
        }

        public void Error(string message)
        {
            Console.WriteLine("[ERROR] > " + message);
        }

        public void Info(string message)
        {
            Console.WriteLine("[INFO] > " + message);
        }

        public void Warn(string message)
        {
            Console.WriteLine("[WARN] > " + message);
        }
    }
}
