namespace SerialAVRBootloader.Loader.Communicators
{
    public interface ICommunicator
    {
        void Write(string text);
        void Write(byte[] data, int offset, int count);
        int ReadByte();
        char ReadChar();
        string ReadExisting();
        string ReadTo(string endchar);
    }
}