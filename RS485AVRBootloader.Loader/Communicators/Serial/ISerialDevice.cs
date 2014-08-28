﻿namespace SerialAVRBootloader.Loader.Communicators.Serial
{
    public interface ISerialDevice
    {
        void Write(string text);
        void Write(byte[] data, int offset, int count);
        int ReadByte();
        int ReadChar();
        string ReadExisting();
        string ReadTo(string endchar);
    }
}