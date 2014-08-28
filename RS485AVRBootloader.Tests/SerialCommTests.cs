using System.IO;
using NUnit.Framework;
using SerialAVRBootloader.Loader;
using SerialAVRBootloader.Loader.Common;
using SerialAVRBootloader.Loader.Communicators.Serial;

namespace RS485AVRBootloader.Tests
{
    public class SerialCommTests
    {
        [Test]
        public void GetInfoTest()
        {
            ILogger logger = new MultiLogger(new MemmoryLogger(), new DebugLogger());
            ISerialDevice serialDevice = new MockedSerialBootloader(logger);
            var bootloader = new BootloaderCommunicator(new SerialCommunicator(serialDevice), logger);

            var bootloaderInfo = bootloader.GetBootloaderInfo();

            logger.WriteLine(bootloaderInfo.ToString());
        }


        [Test]
        public void TryWriteProgram()
        {
            ILogger logger = new MultiLogger(new MemmoryLogger(), new DebugLogger());
            ISerialDevice serialDevice = new MockedSerialBootloader(logger);
            var bootloader = new BootloaderCommunicator(new SerialCommunicator(serialDevice), logger);

            using (var file = File.OpenRead("C:\\Code\\RS485AVRBootloader\\test_do_bootloadera.bin"))
            {
                bootloader.WriteProgram(file);
            }
        }
    }
}