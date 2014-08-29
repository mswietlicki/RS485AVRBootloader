using System;
using System.Text;
using SerialAVRBootloader.Loader.Common;

namespace RS485AVRBootloader.Tests
{
    public class MemmoryLogger : ILogger
    {
        private readonly StringBuilder _sb;

        public MemmoryLogger()
        {
            _sb = new StringBuilder();
        }

        public void WriteLine(string rawInfo)
        {
            _sb.AppendLine(rawInfo);
        }

        public void WriteError(Exception exception)
        {
            _sb.AppendLine(exception.Message);
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