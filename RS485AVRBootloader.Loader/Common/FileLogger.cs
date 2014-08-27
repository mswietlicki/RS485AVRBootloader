using System;
using System.IO;

namespace SerialAVRBootloader.Loader.Common
{
    public class FileLogger : ILogger
    {
        public void WriteLine(string rawInfo)
        {
            File.WriteAllText("Output.txt", rawInfo);
        }

        public void WriteError(Exception exception)
        {
            File.WriteAllText("Output.txt", "ERROR: " + exception.Message);
            File.WriteAllText("Output.txt", "STACKTRACE: " + exception.StackTrace);
        }
    }
}