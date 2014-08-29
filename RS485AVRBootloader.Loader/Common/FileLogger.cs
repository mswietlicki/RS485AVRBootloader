using System;
using System.IO;

namespace SerialAVRBootloader.Loader.Common
{
    public class FileLogger : BaseProgramLogger
    {
        public override void WriteLine(string rawInfo)
        {
            File.AppendAllText("Output.txt", rawInfo + Environment.NewLine);
        }

        public override void WriteError(Exception exception)
        {
            WriteLine("ERROR: " + exception.Message);
            WriteLine("STACKTRACE: " + exception.StackTrace);
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