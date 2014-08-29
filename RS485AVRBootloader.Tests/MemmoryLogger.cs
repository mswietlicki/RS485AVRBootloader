using System;
using System.Text;
using SerialAVRBootloader.Loader.Common;

namespace RS485AVRBootloader.Tests
{
    public class MemmoryLogger : BaseProgramLogger
    {
        private readonly StringBuilder _sb;

        public MemmoryLogger()
        {
            _sb = new StringBuilder();
        }

        public override void WriteLine(string rawInfo)
        {
            _sb.AppendLine(rawInfo);
        }

        public override void WriteError(Exception exception)
        {
            _sb.AppendLine(exception.Message);
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