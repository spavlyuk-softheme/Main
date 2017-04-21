namespace SimpleLogger
{
    public static class Logger
    {
        private static LogCreator _log;
        private static readonly object LockObj = new object();

        public static LogCreator Log
        {
            get
            {
                if (_log == null)
                {
                    lock (LockObj)
                    {
                        if (_log == null)
                        {
                            _log = new LogCreator();
                        }
                    }
                }
                return _log;
            }
        }

    }
}
