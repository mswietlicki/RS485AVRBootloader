using System;
using System.Diagnostics;
using SerialAVRBootloader.Loader.Common;

namespace RS485AVRBootloader.Tests
{
    public class DebugLogger : BaseProgramLogger
    {
        public override void WriteLine(string rawInfo)
        {
            Debug.WriteLine(rawInfo);
        }

        public override void WriteError(Exception exception)
        {
            Debug.WriteLine(exception.Message);
        }

        public override void ProgramOutput(string text)
        {
            WriteLine(" --> " + text);
        }

        public override void ProgramInput(string text)
        {
            WriteLine(" <-- " + text);
        }
    }
}