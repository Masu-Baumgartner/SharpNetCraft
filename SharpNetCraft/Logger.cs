namespace SharpNetCraft
{
    public interface Logger
    {
        private static Logger INSTANCE = null;
        public void Debug(string message);
        public void Info(string message);
        public void Warn(string message);
        public void Error(string message);

        public static void SetLogger(Logger l)
        {
            INSTANCE = l;
        }

        public static Logger GetLogger()
        {
            return INSTANCE;
        }
    }
}
