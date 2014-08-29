using System;

namespace SerialAVRBootloader.Loader.Common
{
    public interface ILogger
    {
        void WriteLine(string rawInfo);
        void WriteError(Exception exception);
        void ProgramOutput(string text);
        void ProgramOutput(byte[] data);
        void ProgramInput(string text);
        void ProgramInput(byte data);
        void ProgramInput(byte[] data);
    }
}