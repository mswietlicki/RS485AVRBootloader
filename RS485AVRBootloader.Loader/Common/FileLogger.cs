using System;
using System.IO;

namespace SerialAVRBootloader.Loader.Common
{
    public class FileLogger : ILogger
    {
        public void WriteLine(string rawInfo)
        {
            File.AppendAllText("Output.txt", rawInfo + Environment.NewLine);
        }

        public void WriteError(Exception exception)
        {
            WriteLine("ERROR: " + exception.Message);
            WriteLine("STACKTRACE: " + exception.StackTrace);
        }

        public void ProgramOutput(string text)
        {
            WriteLine(" --> " + text);
        }

        public void ProgramInput(string text)
        {
            WriteLine(" <-- " + text);
        }
    }
}