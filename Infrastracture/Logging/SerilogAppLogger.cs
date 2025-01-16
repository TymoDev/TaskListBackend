using Core.Interfaces.Logging;
using Serilog;
using System;

namespace Infrastracture.Logging
{
    public class SerilogAppLogger : IAppLogger
    {
        public void Debug(string message)
        {
            Log.Debug(message);
        }

        public void Information(string message)
        {
            Log.Information(message);
        }

        public void Warning(string message)
        {
            Log.Warning(message);
        }

        public void Error(string message, Exception ex = null)
        {
            if (ex != null)
                Log.Error(ex, message);
            else
                Log.Error(message);
        }

        public void Critical(string message, Exception ex = null)
        {
            if (ex != null)
                Log.Fatal(ex, message);
            else
                Log.Fatal(message);
        }
    }

}
