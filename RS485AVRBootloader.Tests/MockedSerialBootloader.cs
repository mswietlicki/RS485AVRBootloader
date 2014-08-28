using System.Text;
using SerialAVRBootloader.Loader.Common;
using SerialAVRBootloader.Loader.Communicators.Serial;

namespace RS485AVRBootloader.Tests
{
    public class MockedSerialBootloader : ISerialDevice
    {
        private readonly ILogger _logger;

        public MockedSerialBootloader(ILogger logger)
        {
            _logger = logger;
        }

        public void Write(string text)
        {

        }

        private int _dataRecived;
        public void Write(byte[] data, int offset, int count)
        {
            _logger.WriteLine(ToHexString(data));
            _dataRecived += count;
        }

        public int ReadByte()
        {
            throw new System.NotImplementedException();
        }

        public int ReadChar()
        {
            if (_dataRecived >= 64)
            {
                _dataRecived = 0;
                return '@';
            }
            return '?';
        }

        public string ReadExisting()
        {
            throw new System.NotImplementedException();
        }

        public string ReadTo(string endchar)
        {
            return "\r\n&64,0x1E00,atmega88,8000000,1*\r\n";
        }


        public static string ToHexString(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                sb.AppendFormat("{0} ", b.ToString("X2"));
            }
            return sb.ToString();
        }

        public void Dispose()
        {
        }
    }
}