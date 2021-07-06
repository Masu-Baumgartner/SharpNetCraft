using System;

namespace SharpNetCraft.Utils.Data
{
    public class GameMode
    {
        public int mode { get; set; }

        public GameMode(byte b)
        {
            mode = Convert.ToInt32(b);
        }

        public GameMode(int i)
        {
            mode = i;
        }
    }
}