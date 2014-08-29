using System;
using System.Diagnostics;
using SerialAVRBootloader.Loader.Common;

namespace RS485AVRBootloader.Tests
{
    public class DebugLogger : ILogger
    {
        public void WriteLine(string rawInfo)
        {
            Debug.WriteLine(rawInfo);
        }

        public void WriteError(Exception exception)
        {
            Debug.WriteLine(exception.Message);
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