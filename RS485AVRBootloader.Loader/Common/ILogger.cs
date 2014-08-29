using System;

namespace SerialAVRBootloader.Loader.Common
{
    public interface ILogger
    {
        void WriteLine(string rawInfo);
        void WriteError(Exception exception);
        void ProgramOutput(string text);
        void ProgramInput(string text);
    }
}