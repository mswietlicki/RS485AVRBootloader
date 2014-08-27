using System;
using System.IO.Ports;
using SerialAVRBootloader.Loader.Common;

namespace SerialAVRBootloader.Loader.Communicators
{
    public class SerialCommunicator : ICommunicator
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly SerialPort _serialPort;
        public SerialCommunicator(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
            _serialPort = new SerialPort(_settingsProvider.GetSetting("PortName"), int.Parse(_settingsProvider.GetSetting("PortBaudRate")));
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