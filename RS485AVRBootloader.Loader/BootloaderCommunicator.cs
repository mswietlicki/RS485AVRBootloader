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
            
        }

    }
}