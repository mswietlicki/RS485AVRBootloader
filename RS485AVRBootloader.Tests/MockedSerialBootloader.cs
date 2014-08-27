using SerialAVRBootloader.Loader.Communicators.Serial;

namespace RS485AVRBootloader.Tests
{
    public class MockedSerialBootloader : ISerialDevice
    {
        
        public void Write(string text)
        {
            
        }

        public void Write(byte[] data, int offset, int count)
        {
            
        }

        public int ReadByte()
        {
            throw new System.NotImplementedException();
        }

        public int ReadChar()
        {
            return 0x3F;
        }

        public string ReadExisting()
        {
            throw new System.NotImplementedException();
        }

        public string ReadTo(string endchar)
        {
            return "\r\n&64,0x1E00,atmega88,8000000,1*\r\n";
        }
    }
}