using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SerialAVRBootloader.Loader;
using SerialAVRBootloader.Loader.Common;
using SerialAVRBootloader.Loader.Communicators;
using SerialAVRBootloader.Loader.Communicators.Serial;

namespace RS485AVRBootloader.Tests
{
    public class SerialCommTests
    {
        [Test]
        public void GetInfoTest()
        {
            ILogger logger = new MultiLogger(new MemmoryLogger(), new DebugLogger());
            ISerialDevice serialDevice = new MockedSerialBootloader();
            var bootloader = new BootloaderCommunicator(new SerialCommunicator(serialDevice), logger);

            var bootloaderInfo = bootloader.GetBootloaderInfo();

            logger.WriteLine(bootloaderInfo.ToString());
        }

    }
}

