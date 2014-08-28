using System;
using System.IO.Ports;
using SerialAVRBootloader.Loader.Common;

namespace SerialAVRBootloader.Loader.Communicators.Serial
{
    public class SerialPortDevice : ISerialDevice
    {
        private readonly SerialPort _serialPort;

        public SerialPortDevice(ISettingsProvider settingsProvider)
        {
            _serialPort = new SerialPort(settingsProvider.GetSetting("PortName"),
                int.Parse(settingsProvider.GetSetting("PortBaudRate")));
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

        public int ReadChar()
        {
            return _serialPort.ReadByte();
        }

        public string ReadExisting()
        {
            return _serialPort.ReadExisting();
        }

        public string ReadTo(string endchar)
        {
            return _serialPort.ReadTo(endchar);
        }

        public void Dispose()
        {
            if (_serialPort == null) return;

            if (_serialPort.IsOpen)
                _serialPort.Close();

            _serialPort.Dispose();
        }
    }
}