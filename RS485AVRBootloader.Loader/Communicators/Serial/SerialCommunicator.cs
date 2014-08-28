using System;

namespace SerialAVRBootloader.Loader.Communicators.Serial
{
    public class SerialCommunicator : ICommunicator
    {
        private readonly ISerialDevice _serialPort;

        public SerialCommunicator(ISerialDevice serialPort)
        {
            _serialPort = serialPort;
        }

        public void Write(string text)
        {
            _serialPort.Write(text);
        }

        public void Write(byte[] data, int offset, int count)
        {
            _serialPort.Write(data, offset, count);
        }

        public int ReadByte()
        {
            return _serialPort.ReadByte();
        }

        public char ReadChar()
        {
            return Convert.ToChar(_serialPort.ReadChar());
        }

        public string ReadExisting()
        {
            return _serialPort.ReadExisting();
        }

        public string ReadTo(string endchar)
        {
            return _serialPort.ReadTo(endchar);
        }
    }
}