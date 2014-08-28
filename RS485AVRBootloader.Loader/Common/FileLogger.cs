using System;
using System.IO;

namespace SerialAVRBootloader.Loader.Common
{
    public class FileLogger : ILogger
    {
        public void WriteLine(string rawInfo)
        {
            File.AppendAllText("Output.txt", rawInfo + "\r\n");
        }

        public void WriteError(Exception exception)
        {
            File.AppendAllText("Output.txt", "ERROR: " + exception.Message + "\r\n");
            File.AppendAllText("Output.txt", "STACKTRACE: " + exception.StackTrace + "\r\n");
        }
    }
}