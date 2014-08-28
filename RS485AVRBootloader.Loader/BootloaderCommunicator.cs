using System.IO;
using SerialAVRBootloader.Loader.Common;
using SerialAVRBootloader.Loader.Communicators;
using SerialAVRBootloader.Loader.Exceptions;
using SerialAVRBootloader.Loader.Model;

namespace SerialAVRBootloader.Loader
{
    public class BootloaderCommunicator
    {
        private readonly ICommunicator _communicator;
        private readonly ILogger _logger;

        public BootloaderCommunicator(ICommunicator communicator, ILogger logger)
        {
            _communicator = communicator;
            _logger = logger;
        }


        public BootloaderInfo GetBootloaderInfo()
        {
            if (_communicator.ReadChar() != '?')
                throw new CommunicationLostExpection();

            _communicator.Write("ui");

            var rawInfo = _communicator.ReadTo("?");
            _logger.WriteLine(rawInfo);

            return BootloaderInfo.Parse(rawInfo);
        }

        public void WriteProgram(Stream dataStream)
        {
            if (_communicator.ReadChar() != '?')
                throw new CommunicationLostExpection();

            _communicator.Write("uw");

            while (dataStream.Position < dataStream.Length)
            {
                _communicator.Write(new byte[] {1}, 0, 1);
                var data = new byte[64];
                dataStream.Read(data, 0, 64);
                _communicator.Write(data, 0, 64);

                if (_communicator.ReadChar() != '@')
                    throw new CommunicationLostExpection();
            }
        }
    }
}