using System;

namespace SerialAVRBootloader.Loader.Communicators.Serial
{
    public interface ISerialDevice: IDisposable
    {
        void Write(string text);
        void Write(byte[] data, int offset, int count);
        byte ReadByte();
        char ReadChar();
        string ReadExisting();
        string ReadTo(string endchar);
    }
}