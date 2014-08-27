using System;
using SerialAVRBootloader.Loader.Common;
using SerialAVRBootloader.Loader.Communicators.Serial;

namespace SerialAVRBootloader.Loader
{
    class Program
    {
        static void Main(string[] args)
        {
            ISettingsProvider settingsProvider = new PropertiesSettingsProvider();
            ISerialDevice serialDevice = new SerialPortDevice(settingsProvider);
            ILogger logger = new MultiLogger(new ConsoleLogger(), new FileLogger());
            try
            {
                var bootloader = new BootloaderCommunicator(new SerialCommunicator(serialDevice), logger);
                var bootloaderInfo = bootloader.GetBootloaderInfo();

                logger.WriteLine(bootloaderInfo.ToString());
            }
            catch (Exception exception)
            {
                logger.WriteError(exception);
            }

        }
    }
}
