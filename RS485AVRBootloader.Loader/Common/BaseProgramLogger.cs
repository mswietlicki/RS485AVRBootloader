using System;

namespace SerialAVRBootloader.Loader.Common
{
    public abstract class BaseProgramLogger : ILogger
    {
        public abstract void WriteLine(string rawInfo);
        public abstract void WriteError(Exception exception);
        public abstract void ProgramOutput(string text);
        public virtual void ProgramOutput(byte[] data)
        {
            ProgramOutput(data.ToHexString());
        }
        public abstract void ProgramInput(string text);
        public virtual void ProgramInput(byte data)
        {
            ProgramInput(string.Format("{0} '{1}'", data.ToHexString(), (char)data));
        }
        public virtual void ProgramInput(byte[] data)
        {
            ProgramInput(data.ToHexString());
        }
    }
}