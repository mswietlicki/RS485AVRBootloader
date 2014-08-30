using System;
using System.IO.Ports;
using System.Linq;
using SerialAVRBootloader.Loader.Common;
using System.Threading;

namespace SerialAVRBootloader.Loader.Communicators.Serial
{
    public class SerialPortDevice : ISerialDevice
    {
        private readonly ILogger _logger;
        private readonly SerialPort _serialPort;

        public SerialPortDevice(ISettingsProvider settingsProvider, ILogger logger)
        {
            _logger = logger;

            _logger.WriteLine("PORTS:");
            SerialPort.GetPortNames().ToList().ForEach(_ => _logger.WriteLine("    " + _));
            _logger.WriteLine("");

            _serialPort = new SerialPort(settingsProvider.GetSetting("PortName"),
                int.Parse(settingsProvider.GetSetting("PortBaudRate")));

            _logger.WriteLine(string.Format("Opening serial port {0}...", _serialPort.PortName));
            _serialPort.Open();
            _logger.WriteLine(string.Format("Serial port {0} opened.", _serialPort.PortName));
            _logger.WriteLine("");

        }

        public void Write(string text)
        {
            _serialPort.Write(text);
        }

        public void Write(byte[] data, int offset, int count)
        {
            var buff = new byte[1];
            for (int i = 0; i < count; i++)
            {
                buff[0] = data[i];
                Thread.Sleep(1);
                _serialPort.Write(buff, 0, 1);
            }
        }

        public byte ReadByte()
        {
            return (byte)_serialPort.ReadByte();
        }

        public char ReadChar()
        {
            
            return (char)_serialPort.ReadChar();
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
            {
                _serialPort.Close();
                _logger.WriteLine(string.Format("Serial port {0} closed.", _serialPort.PortName));
            }
            _serialPort.Dispose();
        }
    }
}