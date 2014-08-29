using SerialAVRBootloader.Loader.Common;

namespace SerialAVRBootloader.Loader.Communicators.Serial
{
    public class LoggerSerialDevice : ISerialDevice
    {
        private readonly ISerialDevice _device;
        private readonly ILogger _logger;

        public LoggerSerialDevice(ISerialDevice device, ILogger logger)
        {
            _device = device;
            _logger = logger;
        }

        public void Dispose()
        {
            _device.Dispose();
        }

        public void Write(string text)
        {
            _logger.ProgramOutput(text);
            _device.Write(text);
        }

        public void Write(byte[] data, int offset, int count)
        {
            _logger.ProgramOutput(data);
            _device.Write(data, offset, count);
        }

        public byte ReadByte()
        {
            var data = _device.ReadByte();
            _logger.ProgramInput(data);
            return data;
        }

        public char ReadChar()
        {
            var data = _device.ReadChar();
            _logger.ProgramInput((byte)data);
            return data;
        }

        public string ReadExisting()
        {
            var data = _device.ReadExisting();
            _logger.ProgramInput(data);
            return data;
        }

        public string ReadTo(string endchar)
        {
            var data = _device.ReadTo(endchar);
            _logger.ProgramInput(data);
            return data;
        }
    }
}