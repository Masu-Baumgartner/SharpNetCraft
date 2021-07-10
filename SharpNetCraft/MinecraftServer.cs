namespace SharpNetCraft
{
    public class MinecraftServer
    {
        private string host { get; set; }
        private int port { get; set; }
        public MinecraftServer() { }
        public MinecraftServer(string host, int port) 
        {
            this.host = host;
            this.port = port;
        }
    }
}
